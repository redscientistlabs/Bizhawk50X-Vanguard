using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;

namespace RTC
{


    public static class RTC_RPC
    {
        private static string ip = "127.0.0.1";
        private static int pluginPortOUT = 56664;
        private static int pluginPortIN = 56665;
        private static int bridgePortIN = 56666;
        private static int killswitchPort = 56667;
        private static System.Windows.Forms.Timer time;
        private static Thread bridgeThread;
        private static Thread pluginThread;
        private static volatile Queue<String> messages = new Queue<string>();
        private static UdpClient pluginSender = new UdpClient(ip, pluginPortOUT);
        private static UdpClient killswitchSender = new UdpClient(ip, killswitchPort);
        private static volatile bool Running = false;
		private static Thread t;
		public static volatile bool Heartbeat = false;
		public static volatile bool Freeze = false;

		public static void Start()
        {
            Running = true;


            time = new System.Windows.Forms.Timer();
            time.Interval = 200;
            time.Tick += new EventHandler(CheckMessages);
            time.Start();

            if (RTC_Hooks.isRemoteRTC)
            {
                RTC_RPC.SendToKillSwitch("UNFREEZE");
            }
            else
            {
                bridgeThread = new Thread(new ThreadStart(ListenToBridge));
                bridgeThread.IsBackground = true;
                bridgeThread.Start();

                pluginThread = new Thread(new ThreadStart(ListenToPlugin));
                pluginThread.IsBackground = true;
                pluginThread.Start();
            }
        }

		public static void StartKillswitch()
		{
			t = new Thread(new ThreadStart(ListenToKillSwitch));
			t.IsBackground = true;
			t.Start();
		}

		public static void Stop()
        {
            Running = false;
        }

        private static void ListenToBridge()
        {
            bool done = false;

			UdpClient Listener;
			IPEndPoint groupEP;

			try
			{
				Listener = new UdpClient(bridgePortIN);
				groupEP = new IPEndPoint(IPAddress.Parse(ip), bridgePortIN);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}

            try
            {
                while (!done)
                {
                    if (!Running)
                        break;

                    byte[] bytes = Listener.Receive(ref groupEP);

                    messages.Enqueue(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Listener.Close();
            }
        }
        private static void ListenToPlugin()
        {
            bool done = false;
			UdpClient Listener;
			IPEndPoint groupEP;

			try
			{
				Listener = new UdpClient(pluginPortIN);
				groupEP = new IPEndPoint(IPAddress.Parse(ip), pluginPortIN);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}

            try
            {
                while (!done)
                {
                    if (!Running)
                        break;

                    byte[] bytes = Listener.Receive(ref groupEP);

                    messages.Enqueue(Encoding.ASCII.GetString(bytes, 0, bytes.Length));
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Listener.Close();
            }
        }

		private static void ListenToKillSwitch()
		{
			bool done = false;

			UdpClient listener = new UdpClient(killswitchPort);
			IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(ip), killswitchPort);

			try
			{
				while (!done)
				{
					byte[] bytes = listener.Receive(ref groupEP);
					string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
					string[] splits = msg.Split('|');

					switch (splits[0])
					{
						default:
							break;

						case "RTC_Heartbeat":

							switch (splits[1])
							{
								default:
									break;

								case "TICK":
									Heartbeat = true;
									break;
								case "FREEZE":
									Heartbeat = true;
									Freeze = true;
									break;
								case "UNFREEZE":
									Heartbeat = true;
									Freeze = false;
									break;
								case "CLOSE":
									Heartbeat = true;
									Application.Exit();
									break;

							}
							break;
					}

				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				listener.Close();
			}
		}


        public static void CheckMessages(object sender, EventArgs e)
        {

            if (RTC_Hooks.isRemoteRTC)
                SendHeartbeat();
            else
            {

                string msg = "";
                while (messages.Count != 0)
                {
                    msg = messages.Dequeue();
                    string[] splits = msg.Split('|');

                    switch (splits[0])
                    {
                        default:
                            break;

                        case "RTC":
                            switch (splits[1])
                            {
                                default:
                                    break;

                                case "CORRUPT":
                                    RTC_Core.ghForm.btnCorrupt_Click(null, null);
                                    break;
                                case "ASK_PLUGIN_SET":
                                    if (RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true) != null)
                                        RefreshPlugin();
                                    break;


                            }
                            break;

                    }
                }

            }
        
        }

        public static void RefreshPlugin()
        {
            SendToPlugin("RTC_Plugin|SET|" + RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true).ToString() + "|" + RTC_Core.bizhawkDir + "\\CorruptedROM.rom" + "|" + RTC_Core.bizhawkDir + "\\ExternalCorrupt.exe");
        }
        public static void ClosePlugin()
        {
            SendToPlugin("RTC_Plugin|CLOSE");
        }
        public static void SendToPlugin(string msg)
        {
            if (!Running)
                return;

            Byte[] sdata = Encoding.ASCII.GetBytes(msg);
            pluginSender.Send(sdata, sdata.Length);
        }
        public static void SendHeartbeat()
        {
            SendToKillSwitch("TICK");
        }
        public static void SendToKillSwitch(string extra)
        {
            if (!Running)
                return;

            string message = "RTC_Heartbeat";

            if (extra != null)
                message += "|" + extra;

            Byte[] sdata = Encoding.ASCII.GetBytes(message);
            killswitchSender.Send(sdata, sdata.Length);
        }
    }
}

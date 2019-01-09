using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RTC.Legacy;

namespace RTC
{
	public static class RTC_RPC
	{
		private static string ip = "127.0.0.1";
		private static int killswitchPort = 56667;
		private static System.Windows.Forms.Timer time;
		private static volatile Queue<string> messages = new Queue<string>();
		private static UdpClient killswitchSender = new UdpClient(ip, killswitchPort);
		private static volatile bool Running = false;
		private static Thread t;
		public static volatile bool Heartbeat = false;
		public static volatile bool Freeze = false;

		public static void Start()
		{
			Running = true;

			time = new System.Windows.Forms.Timer
			{
				Interval = 200
			};
			time.Tick += CheckMessages;
			time.Start();

			if (RTC_NetcoreImplementation.isStandaloneEmu)
			{
				RTC_RPC.SendToKillSwitch("UNFREEZE");
			}
		}

		public static void StartKillswitch()
		{
			t = new Thread(new ThreadStart(ListenToKillSwitch))
			{
				IsBackground = true
			};
			t.Start();
		}

		public static void Stop()
		{
			Running = false;
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
						case "RTC_Heartbeat":

							switch (splits[1])
							{
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
				Console.WriteLine(e);
			}
			finally
			{
				listener.Close();
			}
		}

		public static void CheckMessages(object sender, EventArgs e)
		{
			//Send the heartbeat unless it's the StandaloneRTC process
			if (RTC_NetcoreImplementation.isStandaloneEmu)
				SendHeartbeat();

			if (!RTC_NetcoreImplementation.isStandaloneUI && !RTC_NetcoreImplementation.isStandaloneEmu)
				SendHeartbeat();
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

			byte[] sdata = Encoding.ASCII.GetBytes(message);
			killswitchSender.Send(sdata, sdata.Length);
		}
	}
}

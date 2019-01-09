using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC.Legacy
{
	public static class RTC_NetcoreImplementation
	{
		//NetCores
		public static RTC_NetCore Multiplayer = null;

		public static RTC_NetCore RemoteRTC = null;

		public static bool isStandaloneUI = false;
		public static bool isStandaloneEmu = false;
		public static bool RemoteRTC_SupposedToBeConnected = false;

		public static bool isAttached { get { return (!isStandaloneUI && !isStandaloneEmu); } }
		public static bool FirstConnection = true;

		public static void Start()
		{
			//Initiation of loopback TCP, only in DETACHED MODE
			if (isStandaloneEmu || isStandaloneUI)
			{
				RemoteRTC = new RTC_NetCore();
				RemoteRTC.port = 42042;
				RemoteRTC.address = "";
			}

			//Initialize RemoteRTC server
			if (isStandaloneEmu && !isStandaloneUI)
			{
				//Bizhawk has started in REMOTERTC mode, no RTC form will be loaded
				RemoteRTC.StartNetworking(NetworkSide.CLIENT, true);
				RemoteRTC.SendCommand(new RTC_Command(CommandType.REMOTE_EVENT_BIZHAWKSTARTED), false, true);

				RemoteRTC.ClientDisconnected += (ob, ev) => { RemoteRTC_SupposedToBeConnected = false; };
				RemoteRTC.ClientConnectionLost += (ob, ev) => { RemoteRTC_SupposedToBeConnected = false; };
				RemoteRTC.ClientConnected += (ob, ev) =>
				{
					RemoteRTC_SupposedToBeConnected = true;


					int maxFailed = 5;
					int failedCount = 0;
					//Push the spec to StandaloneRTC
					while (RTC_Unispec.PushEmuSpec() == false)
					{
						if (failedCount++ > maxFailed)
						{
							MessageBox.Show(
								"Failed to push the EMUspec! You should probably save your stockpile as errors may occur.\n If you're seeing this message, let the devs know.");

						}

						//Let the main thread sleep so netcore can unjam if it's jammed
						Thread.Sleep(200);
					}
				};
			}
			else
			{
				//Setup of Detached-exclusive features
				if (isStandaloneUI)
				{
					S.GET<RTC_Core_Form>()
						.Text = "RTC : Detached Mode";

					if (S.ISNULL<RTC_ConnectionStatus_Form>())
						S.SET(new RTC_ConnectionStatus_Form());

					S.GET<RTC_Core_Form>()
						.ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());

					RemoteRTC.ServerStarted += (ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerStarted");

						if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>()
							.IsDisposed)
						{
							if (S.ISNULL<RTC_ConnectionStatus_Form>())
								S.SET(new RTC_ConnectionStatus_Form());

							S.GET<RTC_Core_Form>()
								.ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
						}

						if (!S.ISNULL<RTC_GlitchHarvester_Form>() && !S.GET<RTC_GlitchHarvester_Form>()
							.IsDisposed)
						{
							S.GET<RTC_GlitchHarvester_Form>()
								.pnHideGlitchHarvester.BringToFront();
							S.GET<RTC_GlitchHarvester_Form>()
								.pnHideGlitchHarvester.Show();
						}
					};

					RemoteRTC.ServerConnected += (ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = true;
						Console.WriteLine("RemoteRTC.ServerConnected");
						S.GET<RTC_ConnectionStatus_Form>()
							.lbConnectionStatus.Text = "Connection status: Connected";

						int maxFailed = 5;
						int failedCount = 0;
						//It's absolutely vital that the spec goes through so we make sure it didn't time out
						//Push the spec to Bizhawk
						while (RTC_Unispec.PushRTCSpec() == false)
						{
							if (failedCount++ > maxFailed)
							{
								MessageBox.Show(
									"Failed to push the RTCspec! You should probably save your stockpile as errors may occur.\n If you're seeing this message, let the devs know and bring a copy of the console output with you.");

							}

							//Let the main thread sleep so netcore can unjam if it's jammed
							Thread.Sleep(200);
						}


						if (FirstConnection)
						{
							FirstConnection = false;
							S.GET<RTC_Core_Form>()
								.btnEngineConfig_Click(ob, ev);
						}
						else
							S.GET<RTC_Core_Form>()
								.ShowPanelForm(S.GET<RTC_Core_Form>()
									.previousForm, false);

						S.GET<RTC_GlitchHarvester_Form>()
							.pnHideGlitchHarvester.Size = S.GET<RTC_GlitchHarvester_Form>()
							.Size;
						S.GET<RTC_GlitchHarvester_Form>()
							.pnHideGlitchHarvester.Hide();
						S.GET<RTC_ConnectionStatus_Form>()
							.btnStartEmuhawkDetached.Text = "Restart BizHawk";

						RTC_RPC.Heartbeat = true;
						RTC_RPC.Freeze = false;
					};

					RemoteRTC.ServerConnectionLost += (ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerConnectionLost");

						if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>()
							.IsDisposed)
						{
							S.GET<RTC_ConnectionStatus_Form>()
								.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
							S.GET<RTC_Core_Form>()
								.ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
						}

						if (S.GET<RTC_GlitchHarvester_Form>() != null && !S.GET<RTC_GlitchHarvester_Form>()
							.IsDisposed)
						{
							S.GET<RTC_GlitchHarvester_Form>()
								.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
							S.GET<RTC_GlitchHarvester_Form>()
								.pnHideGlitchHarvester.BringToFront();
							S.GET<RTC_GlitchHarvester_Form>()
								.pnHideGlitchHarvester.Show();
						}

						RTC_GameProtection.Stop();
						//Kill the active table autodumps
						S.GET<RTC_VmdAct_Form>()
							.cbAutoAddDump.Checked = false;
					};

					RemoteRTC.ServerDisconnected += (ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerDisconnected");
						S.GET<RTC_ConnectionStatus_Form>()
							.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						S.GET<RTC_GlitchHarvester_Form>()
							.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						S.GET<RTC_Core_Form>()
							.ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());

						S.GET<RTC_GlitchHarvester_Form>()
							.pnHideGlitchHarvester.BringToFront();
						S.GET<RTC_GlitchHarvester_Form>()
							.pnHideGlitchHarvester.Show();

						RTC_GameProtection.Stop();
						//Kill the active table autodumps
						S.GET<RTC_VmdAct_Form>()
							.cbAutoAddDump.Checked = false;
					};

					RemoteRTC.StartNetworking(NetworkSide.SERVER, false, false);
				}
				else if (isStandaloneEmu)
				{
					//WILL THIS EVER HAPPEN? TO BE REMOVED IF NOT
					RemoteRTC.StartNetworking(NetworkSide.SERVER, false, true);
				}
			}
		}

		public static object SendCommandToBizhawk(RTC_Command cmd, bool sync = false, bool priority = false)
		{
			//This is a NetCore wrapper that guarantees a NetCore command is sent to BizHawk no matter which mode.
			//It can query a value in sync or async

			if (RTC_NetcoreImplementation.RemoteRTC == null)
			{
				RTC_NetCore tempNetCore = new RTC_NetCore();
				LinkedList<RTC_Command> tempQueue = new LinkedList<RTC_Command>();
				tempQueue.AddLast(cmd);
				Console.WriteLine($"TEMP_NetCore -> {cmd.Type.ToString()}");
				if (sync)
					cmd.requestGuid = Guid.NewGuid();
				return tempNetCore.ProcessQueue(tempQueue, true);
			}
			else
			{
				if ((!RTC_NetcoreImplementation.isStandaloneEmu && !RTC_NetcoreImplementation.isStandaloneUI) || RTC_NetcoreImplementation.RemoteRTC.side == NetworkSide.CLIENT)
				{
					if (sync)
						return RTC_NetcoreImplementation.RemoteRTC.SendSyncCommand(cmd, true, priority);
					else
						return RTC_NetcoreImplementation.RemoteRTC.SendCommand(cmd, true, priority);
				}
				else
				{
					if (sync)
						return RTC_NetcoreImplementation.RemoteRTC.SendSyncCommand(cmd, false, priority);
					else
						return RTC_NetcoreImplementation.RemoteRTC.SendCommand(cmd, false, priority);
				}
			}
		}

		public static void SendCommandToRTC(RTC_Command cmd)
		{
			//This is a NetCore wrapper that guarantees a NetCore command is sent to RTC no matter which mode.
			//It CANNOT query a value

			if (RTC_NetcoreImplementation.RemoteRTC == null)
			{
				RTC_NetCore tempNetCore = new RTC_NetCore();
				LinkedList<RTC_Command> tempQueue = new LinkedList<RTC_Command>();
				tempQueue.AddLast(cmd);
				Console.WriteLine($"TEMP_NetCore -> {cmd.Type.ToString()}");
				//Console.WriteLine($"{RTC_Core.RemoteRTC.expectedSide.ToString()}:SendCommand -> {cmd.Type.ToString()}");
				tempNetCore.ProcessQueue(tempQueue);
			}
			else
			{
				RTC_NetcoreImplementation.RemoteRTC.SendCommand(cmd, false);
			}
		}
	}
}

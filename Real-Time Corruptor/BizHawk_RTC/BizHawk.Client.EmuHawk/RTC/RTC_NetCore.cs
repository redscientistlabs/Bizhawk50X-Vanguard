using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_NetCore
	{
		/*
        NetCore is a side-agnostic TCP library for executing functions between processes whether locally or over the internet.
        It is the base of Detached Mode and RTC Multiplayer in Attached Mode.

        As of RTC 3.0, Every function that links RTC and Bizhawk must be routed Through NetCore Commands.
        While this renders debugging harder, it also allows normal functionality in both Detached and Attached mode.

        NetCore uses serialized objects to transport command data and returns accross points.
        Additionnal wrapping functions for NetCore exist in RTC_Core as a way of guaranteeing the direction commands go.
        Commands can also loop inside the same process (A command sent to Bizhawk from the Bizhawk process will take the required route
        arrive to its destination regardless of the mode NetCore is currently running)

        Because the functions it serves is critical, it is built to automatically disconnect and reconnect upon timeout, packet loss or error.
        While this prevents a lot of issues from happenning, it also creates some.

        Executing Sync commands accross processes will JAM the mainthread of one while waiting for the second process to respond.
        This makes detection of infinite loop, errors and innacceptable timeouts crucial for it to work well.

        A setting called "Aggressiveness" in the main menu can either TRIPLE the timeout values or even Disable any timeout (not recommended).
        */

		volatile TcpClient client;
		volatile NetworkStream clientStream;
		System.Windows.Forms.Timer CommandQueueProcessorTimer;
		public volatile int port = 42069;
		public volatile string address = "";

		public volatile NetworkSide side = NetworkSide.DISCONNECTED;
		public volatile NetworkSide expectedSide = NetworkSide.DISCONNECTED;

		public volatile LinkedList<RTC_Command> CommandQueue = new LinkedList<RTC_Command>();
		public volatile LinkedList<RTC_Command> PeerCommandQueue = new LinkedList<RTC_Command>();
		public Thread streamReadingThread = null;

		public bool supposedToBeConnected = false;
		public bool expectingSomeone = false;
		static volatile bool isStreamReadingThreadAlive = false;

		public static int KeepAliveCounter = 5;
		public static int DefaultKeepAliveCounter = 5;
		public static int DefaultNetworkStreamTimeout = 2000;
		public static volatile int DefaultMaxRetries = 666;
		private static bool showBoops = false;

		System.Windows.Forms.Timer KeepAliveTimer = null;

		private static object CommandQueueLock = new object();
		public static bool NetCoreCommandSynclock = false;

		public event EventHandler ClientConnecting;

		protected virtual void OnClientConnecting(EventArgs e) => ClientConnecting?.Invoke(this, e);

		public event EventHandler ClientConnected;

		protected virtual void OnClientConnected(EventArgs e) => ClientConnected?.Invoke(this, e);

		public event EventHandler ClientDisconnected;

		protected virtual void OnClientDisconnected(EventArgs e) => ClientDisconnected?.Invoke(this, e);

		public event EventHandler ClientConnectionLost;

		protected virtual void OnClientConnectionLost(EventArgs e) => ClientConnectionLost?.Invoke(this, e);

		public event EventHandler ClientReconnecting;

		protected virtual void OnClientReconnecting(EventArgs e) => ClientReconnecting?.Invoke(this, e);

		public event EventHandler ServerStarted;

		protected virtual void OnServerStarted(EventArgs e) => ServerStarted?.Invoke(this, e);

		public event EventHandler ServerConnected;

		protected virtual void OnServerConnected(EventArgs e) => ServerConnected?.Invoke(this, e);

		public event EventHandler ServerDisconnected;

		protected virtual void OnServerDisconnected(EventArgs e) => ServerDisconnected?.Invoke(this, e);

		public event EventHandler ServerConnectionLost;

		protected virtual void OnServerConnectionLost(EventArgs e) => ServerConnectionLost?.Invoke(this, e);

		static Dictionary<Guid?, int> guid2LastAggressivity = new Dictionary<Guid?, int>();

		public static Guid HugeOperationStart(string targetAggressiveness = "DISABLED")
		{
			RTC_RPC.SendToKillSwitch("FREEZE");

			var token = Guid.NewGuid();
			if (RTC_Core.isStandalone)
			{
				guid2LastAggressivity.Add(token, RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex);

				if (targetAggressiveness == "DISABLED")
					RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex = RTC_Core.sForm.cbNetCoreCommandTimeout.Items.Count - 1;
				else
					RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex = RTC_Core.sForm.cbNetCoreCommandTimeout.Items.Count - 2;
			}
			return token;
		}

		public static void HugeOperationEnd(Guid? operationGuid = null)
		{
			RTC_RPC.SendToKillSwitch("UNFREEZE");

			if (RTC_Core.isStandalone)
			{
				if (operationGuid == null || !(guid2LastAggressivity.ContainsKey(operationGuid)))
					return;

				RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex = guid2LastAggressivity[operationGuid];
				guid2LastAggressivity.Remove(operationGuid);
			}
		}

		public static void HugeOperationReset()
		{
			RTC_RPC.SendToKillSwitch("UNFREEZE");

			if (RTC_Core.isStandalone)
			{
				RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex = 0;
				guid2LastAggressivity.Clear();

			}
		}

		public Socket KillableAcceptSocket(TcpListener listener)
		{
			Socket socket = null;

			try
			{
				ManualResetEvent clientConnected = new ManualResetEvent(false);
				listener.Start();
				clientConnected.Reset();
				var iasyncResult = listener.BeginAcceptSocket((ar) =>
				{
					try
					{
						socket = listener.EndAcceptSocket(ar);
						clientConnected.Set();
					}
					catch (Exception ex) { OutputException(ex); }
				}, null);
				clientConnected.WaitOne();

				return socket;
			}
			catch (Exception exception)
			{
				throw exception;
			}
			finally
			{
				listener.Stop();
			}
		}

		public void StoreCommands(NetworkStream providedStream, bool dontCreateNetworkStream = false)
		{
			var binaryFormatter = new BinaryFormatter();

			TcpListener server = null;
			Socket socket = null;
			NetworkStream networkStream = null;

			if (providedStream != null)
				networkStream = providedStream;

			try
			{
				if (networkStream == null && !dontCreateNetworkStream)
				{
					server = new TcpListener(IPAddress.Any, port);
					server.Start();
					socket = KillableAcceptSocket(server);
					networkStream = new NetworkStream(socket);
					server.Stop();
				}

				networkStream.ReadTimeout = DefaultNetworkStreamTimeout;
				networkStream.WriteTimeout = DefaultNetworkStreamTimeout;

				if (side == NetworkSide.CLIENT)
					SendCommand(new RTC_Command(CommandType.HI), false, true);

				while (true)
				{
					if (networkStream != null && networkStream.DataAvailable)
					{
						RTC_Command cmd;

						try
						{
							cmd = (RTC_Command)binaryFormatter.Deserialize(networkStream);
						}
						catch (Exception ex)
						{
							throw ex;
						}

						if (cmd != null)
						{
							if (cmd.Type == CommandType.RETURNVALUE)
								ReturnWatch.SyncReturns.Add((Guid)cmd.requestGuid, cmd.objectValue);
							else
								CommandQueue.AddLast(cmd);
						}
					}

					while (PeerCommandQueue.Count > 0)
					{
						RTC_Command backCmd;

						lock (CommandQueueLock)
						{
							backCmd = PeerCommandQueue.First.Value;
							PeerCommandQueue.RemoveFirst();
						}

						try
						{
							using (MemoryStream ms = new MemoryStream())
							{
								binaryFormatter.Serialize(ms, backCmd);
								byte[] buf = ms.ToArray();
								networkStream.Write(buf, 0, buf.Length);
							}

							//binaryFormatter.Serialize(networkStream, backCmd);
						}
						catch (Exception ex)
						{
							throw ex;
						}

						if (backCmd.Type == CommandType.BYE)
						{
							CommandQueue.AddFirst(new RTC_Command(CommandType.SAIDBYE));
							PeerCommandQueue.Clear();
							throw new Exception("SAIDBYE");
						}

						if (side == NetworkSide.DISCONNECTED || side == NetworkSide.CONNECTIONLOST)
						{
							if (side == NetworkSide.DISCONNECTED)
							{
								CommandQueue.Clear();
								PeerCommandQueue.Clear();
							}

							throw new Exception(side.ToString());
						}
					}

					Thread.Sleep(5);
				}
			}
			catch (Exception ex)
			{
				OutputException(ex);
				
				if (side == NetworkSide.CLIENT || side == NetworkSide.SERVER)
					side = NetworkSide.CONNECTIONLOST;
			}
			finally
			{
				if (networkStream != null)
				{
					try
					{
						networkStream.Close();
						networkStream.Dispose();
					}
					catch (Exception ex2) { OutputException(ex2); }
				}

				if (socket != null)
				{
					//socket.Close();
					try
					{
						socket.Shutdown(SocketShutdown.Both);
						socket.Dispose();
					}
					catch (Exception ex2) { OutputException(ex2); }
				}

				if (server != null)
				{
					try
					{
						server.Stop();
					}
					catch (Exception ex2) { OutputException(ex2); }
				}

				isStreamReadingThreadAlive = false;
			}
		}

		public void OutputException(Exception ex)
		{
			//Discarded
			Console.WriteLine(expectedSide.ToString() + " -> " + ex.ToString());
		}

		public void StopNetworking(bool fromBye = false, bool stayConnected = false)
		{
			//if ((side == NetworkSide.CONNECTIONLOST || side == NetworkSide.DISCONNECTED) && stayConnected)
			//	return;

			//supposedToBeConnected = stayConnected;

			if (!fromBye)
			{
				SendCommand(new RTC_Command(CommandType.SAYBYE), true, true);
				return;
			}

			if (!stayConnected)
				expectingSomeone = false;

			if (expectedSide == NetworkSide.CLIENT)
				if (stayConnected)
					OnClientConnectionLost(null);
				else
					OnClientDisconnected(null);
			else if (expectedSide == NetworkSide.SERVER)
				if (!stayConnected)
					OnServerDisconnected(null);

			side = (stayConnected ? NetworkSide.CONNECTIONLOST : NetworkSide.DISCONNECTED);

			if (side == NetworkSide.DISCONNECTED)
			{
				CommandQueue.Clear();
				PeerCommandQueue.Clear();
			}

			if (streamReadingThread != null)
			{
				streamReadingThread.Abort();
				streamReadingThread = null;
			}

			if (clientStream != null)
			{
				clientStream.Close();
				clientStream = null;
			}

			if (client != null)
			{
				client.Close();
				client = null;
			}

			/*
			if (CommandQueueProcessorTimer != null)
			{
				CommandQueueProcessorTimer.Stop();
				CommandQueueProcessorTimer = null;
			}
			*/

			if (!stayConnected)
			{
				if (KeepAliveTimer != null)
				{
					KeepAliveTimer.Stop();
					KeepAliveTimer = null;
				}
			}

			if (!stayConnected)
				supposedToBeConnected = false;
		}

		private bool StartClient(bool clientDefaultReconnect = false)
		{
			try
			{
				client = new TcpClient();

				var result = client.BeginConnect(address, port, null, null);
				var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(DefaultMaxRetries));

				if (!success)
					throw new Exception("Failed to connect.");

				client.EndConnect(result);
				clientStream = client.GetStream();

				streamReadingThread = new Thread(() => StoreCommands(clientStream));
				streamReadingThread.Name = "CLIENT";
				streamReadingThread.Start();
				isStreamReadingThreadAlive = true;
			}
			catch (Exception ex)
			{
				OutputException(ex);

				if (clientStream != null)
				{
					clientStream.Close();
					clientStream = null;
				}

				if (client != null)
				{
					client.Close();
					client = null;
				}

				if (clientDefaultReconnect)
					supposedToBeConnected = true;

				if (!supposedToBeConnected)
					MessageBox.Show("Could not connect to Server (Server did not respond in time)\n\n" + ex.ToString());

				if (clientDefaultReconnect)
					return true;

				return false;
			}

			return true;
		}

		private bool StartServer(bool dontUseNetworkStream = false)
		{
			try
			{
				streamReadingThread = new Thread(() => StoreCommands(null, dontUseNetworkStream));
				streamReadingThread.Name = "SERVER";
				streamReadingThread.Start();
				isStreamReadingThreadAlive = true;
			}
			catch (Exception ex)
			{
				OutputException(ex);
				return false;
			}

			return true;
		}

		public void ClearNetowrkCache()
		{
			var peercmd_bck = PeerCommandQueue.ToArray();
			PeerCommandQueue.Clear();

			var md_bck = CommandQueue.ToArray();
			CommandQueue.Clear();
		}

		public bool StartNetworking(NetworkSide _side, bool clientDefaultReconnect = false, bool dontUseNetworkStream = false)
		{
			if (supposedToBeConnected && !(side == NetworkSide.DISCONNECTED || side == NetworkSide.CONNECTIONLOST))
				return false;

			//if (side != NetworkSide.CONNECTIONLOST)
			//{
			//	CommandQueue.Clear();
			//	PeerCommandQueue.Clear();
			//}

			if (CommandQueueProcessorTimer == null)
			{
				CommandQueueProcessorTimer = new System.Windows.Forms.Timer();
				CommandQueueProcessorTimer.Interval = 5;
				CommandQueueProcessorTimer.Tick += CommandQueueProcessorTimer_Tick;
				CommandQueueProcessorTimer.Start();
			}

			if (_side == NetworkSide.CLIENT)
			{
				side = NetworkSide.CLIENT;
				expectedSide = NetworkSide.CLIENT;
				if (!StartClient(clientDefaultReconnect))
					return false;

				OnClientConnecting(null);
			}
			else if (_side == NetworkSide.SERVER)
			{
				side = NetworkSide.SERVER;
				expectedSide = NetworkSide.SERVER;

				StartServer(dontUseNetworkStream);

				OnServerStarted(null);
			}
			else
			{
				return false;
			}

			KeepAliveCounter = DefaultKeepAliveCounter;

			if (KeepAliveTimer == null)
			{
				KeepAliveTimer = new System.Windows.Forms.Timer();
				KeepAliveTimer.Interval = 666;
				KeepAliveTimer.Tick += KeepAliveTimer_Tick;
				KeepAliveTimer.Start();
			}

			if (_side == NetworkSide.SERVER)
				expectingSomeone = false;

			supposedToBeConnected = true;

			return true;
		}

		private void KeepAliveTimer_Tick(object sender, EventArgs e)
		{
			if (!supposedToBeConnected)
				return;

			if (expectedSide == NetworkSide.SERVER && !expectingSomeone)
				return;

			if (expectedSide == NetworkSide.CLIENT)
				KeepAliveCounter--;
			else if (expectedSide == NetworkSide.SERVER)
				KeepAliveCounter--;

			SendCommand(new RTC_Command(CommandType.BOOP), false, true);

			if (KeepAliveCounter <= 0 && side != NetworkSide.CONNECTIONLOST)
			{
				StopNetworking(true, true);
			}

			if (KeepAliveCounter < -1 && side == NetworkSide.CONNECTIONLOST)
			{
				if (expectedSide == NetworkSide.CLIENT)
				{
					OnClientReconnecting(null);
					StartNetworking(expectedSide);

					KeepAliveCounter = DefaultKeepAliveCounter;
				}
				else if (expectedSide == NetworkSide.SERVER)
				{
					OnServerConnectionLost(null);
					StartNetworking(expectedSide);
				}
			}
		}

		private void CommandQueueProcessorTimer_Tick(object sender, EventArgs e)
		{
			ProcessQueue(CommandQueue);
		}

		public void ProcessQueueNow(LinkedList<RTC_Command> queue)
		{
			ProcessQueue(queue);
		}

		public object ProcessQueue(LinkedList<RTC_Command> cmdQueue, bool snatchReturn = false)
		{
			while (cmdQueue.Count > 0 && cmdQueue.First != null)
			{
				RTC_Command cmd = cmdQueue.First.Value;

				if (cmd.Type == CommandType.PUSHSCREEN)
				{
					cmd = GetLatestScreenFrame(cmdQueue);
				}
				else
				{
					try
					{
						cmdQueue.RemoveFirst();
					}
					catch (Exception ex)
					{
						MessageBox.Show("NetCore had a thread collision and threw up.\nIn theory this should fix itself after you close this window.\n\n" + ex.ToString());
						ReturnWatch.SyncReturns.Clear();
						return null;
					}
				}

				if (cmd == null)
				{
					continue;
				}

				RTC_Command cmdBack = null;

				if (!showBoops && cmd.Type.ToString() != "BOOP")
					Console.WriteLine(expectedSide.ToString() + ":ProcessQueue -> " + cmd.Type.ToString());

				switch (cmd.Type)
				{
					// NetCore Commands

					case CommandType.HI:
						if (side == NetworkSide.SERVER)
						{
							OnServerConnected(null);
							expectingSomeone = true;
							SendCommand(new RTC_Command(CommandType.HI), false, true);
						}
						else if (side == NetworkSide.CLIENT)
						{
							OnClientConnected(null);
							expectingSomeone = true;
						}

						break;

					case CommandType.SAYBYE:
						SendCommand(new RTC_Command(CommandType.BYE), !(isStreamReadingThreadAlive && expectingSomeone), true);
						//if not connected, send to self directly
						break;

					case CommandType.SAIDBYE:
					case CommandType.BYE:
						StopNetworking(true);
						return null;

					case CommandType.CONNECTIONLOST:
						StopNetworking(true, true);
						break;

					case CommandType.BOOP:
						KeepAliveCounter = DefaultKeepAliveCounter;
						break;

					case CommandType.AGGRESSIVENESS:
						RTC_NetCoreSettings.ChangeNetCoreSettings((string)cmd.objectValue);
						break;

					case CommandType.GETAGGRESSIVENESS:
						string setting = RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedItem.ToString().ToUpper();
						RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.AGGRESSIVENESS) { objectValue = setting });
						break;

					default:
						// NetCore Extension Commands (Comment to detach Netcore from Extensions)
						cmdBack = Process_RTCExtensions(cmd);
						break;
				}

				// Create backcommand if a sync request was issued
				if (cmdBack == null && cmd.requestGuid != null)
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);

				//send command back or return value if from bizhawk to bizhawk
				if (cmdBack != null)
				{
					if (snatchReturn)
						return cmdBack.objectValue;

					cmdBack.ReturnedFrom = cmd.Type;
					cmdBack.requestGuid = cmd.requestGuid;
					SendCommand(cmdBack, false);
				}
			}

			return null;
		}

		public object SendCommand(RTC_Command cmd, bool self, bool priority = false)
		{
			if (!expectingSomeone && cmd.Type == CommandType.PUSHSCREEN) //Don't pile up video frames in the queue if considered disconnected
				return null;

			if (CommandQueueProcessorTimer == null)
			{
				if (!self)
					return null;

				LinkedList<RTC_Command> tempQueue = new LinkedList<RTC_Command>();
				tempQueue.AddLast(cmd);

				if (!showBoops && cmd.Type.ToString() != "BOOP")
					Console.WriteLine($"{expectedSide.ToString()}:SendCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}");
				ProcessQueue(tempQueue);
				return null;
			}

			if (!RTC_Core.isStandalone && !RTC_Hooks.isRemoteRTC && cmd.Type == CommandType.RETURNVALUE)
			{
				ReturnWatch.SyncReturns.Add((Guid)cmd.requestGuid, cmd.objectValue);
				return null;
			}

			var cmdQueue = PeerCommandQueue;

			if (self)
				cmdQueue = CommandQueue;

			if (!showBoops && cmd.Type.ToString() != "BOOP")
				Console.WriteLine($"{expectedSide.ToString()}:SendCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}");

			if (priority)
				cmdQueue.AddFirst(cmd);
			else
				cmdQueue.AddLast(cmd);

			return null;
		}

		public object SendSyncCommand(RTC_Command cmd, bool self, bool priority = false)
		{
			if (!expectingSomeone && cmd.Type == CommandType.PUSHSCREEN)
				return null;

			cmd.requestGuid = Guid.NewGuid();

			if (CommandQueueProcessorTimer == null || (!RTC_Core.isStandalone && !RTC_Hooks.isRemoteRTC) || RTC_Hooks.isRemoteRTC)
			{
				if (!self)
					return null;

				if (!RTC_Hooks.isRemoteRTC)
				{
					LinkedList<RTC_Command> tempQueue = new LinkedList<RTC_Command>(new[] { cmd });
					Console.WriteLine($"{expectedSide.ToString()}:SendSyncCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}");
					ProcessQueue(tempQueue);

					Console.WriteLine(expectedSide.ToString() + " -> GETVALUE");
					return ReturnWatch.GetValue((Guid)cmd.requestGuid, cmd.Type);
				}
				else
				{
					LinkedList<RTC_Command> tempQueue = new LinkedList<RTC_Command>(new[] { cmd });
					Console.WriteLine($"{expectedSide.ToString()}:SendSyncCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}");
					return ProcessQueue(tempQueue, true);
				}
			}

			var cmdQueue = PeerCommandQueue;

			if (self)
				cmdQueue = CommandQueue;

			Console.WriteLine($"{expectedSide.ToString()}:SendSyncCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}");

			if (priority)
				cmdQueue.AddFirst(cmd);
			else
				cmdQueue.AddLast(cmd);

			Console.WriteLine(expectedSide.ToString() + " -> GETVALUE");
			return ReturnWatch.GetValue((Guid)cmd.requestGuid, cmd.Type);
		}
	}

	public static class ReturnWatch
	{
		public static volatile Dictionary<Guid, object> SyncReturns = new Dictionary<Guid, object>();
		public static volatile int maxtries = 0;

		public static object GetValue(Guid WatchedGuid, CommandType type)
		{
			//await Task.Factory.StartNew(() => WaitForValue(WatchedGuid));
			Console.WriteLine("GetValue:Awaiting -> " + type.ToString());

			maxtries = 0;

			while (!SyncReturns.ContainsKey(WatchedGuid) && maxtries < RTC_NetCore.DefaultMaxRetries)
			{
				maxtries++;
				//WaitMiliseconds(2);

				if (maxtries % 100 == 0)
				{
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BOOP));
					System.Windows.Forms.Application.DoEvents();
				}

				Thread.Sleep(2);
			}

			if (maxtries >= RTC_NetCore.DefaultMaxRetries)
			{
				//MessageBox.Show("An inter-thread synchronous method has timed before a response arrived. Aborting current procedure.");
				return null;
			}

			object ret = SyncReturns[WatchedGuid];
			SyncReturns.Remove(WatchedGuid);

			Console.WriteLine("GetValue:Returned -> " + type.ToString());

			return ret;
		}

		private static void WaitMiliseconds(int ms)
		{
			DateTime _desired = DateTime.Now.AddMilliseconds(ms);
			while (DateTime.Now < _desired)
			{
				System.Windows.Forms.Application.DoEvents();
			}
		}
	}

	public enum NetworkSide
	{
		DISCONNECTED,
		CONNECTIONLOST,
		PENDINGCLIENT,
		CLIENT,
		SERVER
	}

	[Serializable()]
	public enum CommandType
	{
		//===============================================
		//NetCore commands
		//===============================================
		HI,

		BYE,
		SAYBYE,
		SAIDBYE,
		CONNECTIONLOST,
		RETURNVALUE,
		BOOP,
		AGGRESSIVENESS,
		GETAGGRESSIVENESS,

		//===============================================
		//Extension commands
		//===============================================

		//General RTC commands
		BLAST,

		ASYNCBLAST,
		STASHKEY,
		BLASTGENERATOR_BLAST,

		REMOTE_KEY_PUSHSAVESTATEDICO,
		REMOTE_KEY_GETSYSTEMNAME,
		REMOTE_KEY_GETSYSTEMCORE,
		REMOTE_KEY_GETGAMENAME,
		REMOTE_KEY_GETSYNCSETTINGS,
		REMOTE_KEY_PUTSYNCSETTINGS,
		REMOTE_KEY_GETOPENROMFILENAME,
		REMOTE_KEY_GETRAWBLASTLAYER,
		REMOTE_KEY_GETBLASTBYTEBACKUPLAYER,
		REMOTE_DOMAIN_GETDOMAINS,
		REMOTE_DOMAIN_VMD_ADD,
		REMOTE_DOMAIN_VMD_REMOVE,
		REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP,
		REMOTE_DOMAIN_SETSELECTEDDOMAINS,
		REMOTE_DOMAIN_SYSTEM,
		REMOTE_DOMAIN_SYSTEMPREFIX,
		REMOTE_DOMAIN_PEEKBYTE,
		REMOTE_DOMAIN_POKEBYTE,
		REMOTE_DOMAIN_GETSIZE,
		REMOTE_PUSHPARAMS,
		REMOTE_PUSHVMDS,
		REMOTE_LOADROM,
		REMOTE_LOADSTATE,
		REMOTE_SAVESTATE,
		REMOTE_MERGECONFIG,
		REMOTE_IMPORTKEYBINDS,
		REMOTE_BACKUPKEY_REQUEST,
		REMOTE_BACKUPKEY_STASH,
		REMOTE_EVENT_LOADGAMEDONE_NEWGAME,
		REMOTE_EVENT_LOADGAMEDONE_SAMEGAME,
		REMOTE_EVENT_CLOSEBIZHAWK,
		REMOTE_EVENT_SAVEBIZHAWKCONFIG,
		REMOTE_EVENT_BIZHAWKSTARTED,

		//Multiplayer commands
		PULLROM,

		PUSHROM,
		PULLSTATE,
		PUSHSTATE,
		PULLSWAPSTATE,
		PUSHSWAPSTATE,
		PUSHSCREEN,
		PULLSCREEN,
		REQUESTSTREAM,
		GAMEOFSWAPSTART,
		GAMEOFSWAPSTOP,

		//Bizhawk Overrides
		BIZHAWK_SET_OSDDISABLED,

		//Bizhawk Shortcuts
		BIZHAWK_OPEN_HEXEDITOR_ADDRESS,

		//General Corruption settings
		REMOTE_SET_SAVESTATEBOX,

		REMOTE_SET_AUTOCORRUPT,
		REMOTE_SET_CUSTOMPRECISION,
		REMOTE_SET_INTENSITY,
		REMOTE_SET_ERRORDELAY,
		REMOTE_SET_ENGINE,
		REMOTE_SET_BLASTRADIUS,
		REMOTE_SET_RESTOREBLASTLAYERBACKUP,

		// Corruption Core settings and commands
		REMOTE_SET_NIGHTMARE_TYPE,
		REMOTE_SET_NIGHTMARE_MINVALUE,
		REMOTE_SET_NIGHTMARE_MAXVALUE,

		REMOTE_SET_HELLGENIE_MAXCHEATS,
		REMOTE_SET_HELLGENIE_MINVALUE,
		REMOTE_SET_HELLGENIE_MAXVALUE,
		REMOTE_SET_HELLGENIE_CLEARALLCHEATS,
		REMOTE_SET_HELLGENIE_REMOVEEXCESSCHEATS,
		REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND,
		REMOTE_SET_DISTORTION_DELAY,
		REMOTE_SET_DISTORTION_RESYNC,
		REMOTE_SET_PIPE_MAXPIPES,
		REMOTE_SET_PIPE_TILTVALUE,

		REMOTE_SET_PIPE_CLEARPIPES,
		REMOTE_SET_PIPE_LOCKPIPES,
		REMOTE_SET_PIPE_CHAINEDPIPES,
		REMOTE_SET_PIPE_PROCESSONSTEP,
		REMOTE_SET_PIPE_CLEARPIPESREWIND,
		REMOTE_SET_VECTOR_LIMITER,
		REMOTE_SET_VECTOR_VALUES,

		REMOTE_HOTKEY_MANUALBLAST,
		REMOTE_HOTKEY_AUTOCORRUPTTOGGLE,
		REMOTE_HOTKEY_ERRORDELAYDECREASE,
		REMOTE_HOTKEY_ERRORDELAYINCREASE,
		REMOTE_HOTKEY_INTENSITYDECREASE,
		REMOTE_HOTKEY_INTENSITYINCREASE,
		REMOTE_HOTKEY_GHLOADCORRUPT,
		REMOTE_HOTKEY_GHCORRUPT,
		REMOTE_HOTKEY_GHLOAD,
		REMOTE_HOTKEY_GHSAVE,
		REMOTE_HOTKEY_GHSTASHTOSTOCKPILE,
		REMOTE_HOTKEY_BLASTRAWSTASH,
		REMOTE_HOTKEY_SENDRAWSTASH,
		REMOTE_HOTKEY_BLASTLAYERTOGGLE,
		REMOTE_HOTKEY_BLASTLAYERREBLAST,

		REMOTE_RENDER_START,
		REMOTE_RENDER_STOP,
		REMOTE_RENDER_SETTYPE,
		REMOTE_RENDER_STARTED,
		REMOTE_RENDER_RENDERATLOAD,
	}
}

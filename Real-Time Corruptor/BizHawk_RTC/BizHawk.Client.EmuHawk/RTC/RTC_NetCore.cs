using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Net;
using BizHawk.Client.EmuHawk;
using System.Windows;
using System.IO;
using BizHawk.Client.Common;
using System.Threading.Tasks;

namespace RTC
{
	public class RTC_NetCore
	{
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

		static int KeepAliveCounter = 5;
        public static int DefaultKeepAliveCounter = 5;
        public static int DefaultNetworkStreamTimeout = 2000;
        public static int DefaultMaxRetries = 666;

        System.Windows.Forms.Timer KeepAliveTimer = null;

		static volatile Dictionary<string, bool> TransferedRomFilenames = new Dictionary<string, bool>();

		private static object CommandQueueLock = new object();



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
					catch(Exception ex) { OutputException(ex); }

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
						RTC_Command cmd = (RTC_Command)binaryFormatter.Deserialize(networkStream);
						if (cmd != null)
						{
							if(cmd.Type == CommandType.RETURNVALUE)
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

						binaryFormatter.Serialize(networkStream, backCmd);

						/*
						if (backCmd.stashkey != null)
						{
							if (backCmd.Type == CommandType.PULLSWAPSTATE || backCmd.Type == CommandType.PUSHSWAPSTATE)
								if(RTC_StockpileManager.SavestateStashkeyDico.ContainsKey(backCmd.stashkey.Key))
									RTC_StockpileManager.SavestateStashkeyDico.Remove(backCmd.stashkey.Key);

							if (backCmd.stashkey.stateData != null)
								backCmd.stashkey.stateData = null;

							if (backCmd.stashkey.RomData != null)
								backCmd.stashkey.RomData = null;
						}
						*/

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


		public bool PeerHasRom(string RomFilename)
		{
			if (TransferedRomFilenames.ContainsKey(RomFilename))
				return true;

			TransferedRomFilenames.Add(RomFilename, true);
			return false;
		}

		public void SwapGameState()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			RTC_Command cmd = new RTC_Command(CommandType.PULLSWAPSTATE);

			string romFullFilename = GlobalWin.MainForm.CurrentlyOpenRom;
			cmd.romFilename = romFullFilename.Substring(romFullFilename.LastIndexOf("\\") + 1, romFullFilename.Length - (romFullFilename.LastIndexOf("\\") + 1));

			if (!PeerHasRom(cmd.romFilename))
				cmd.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

			StashKey sk = RTC_StockpileManager.SaveState(false);
			cmd.stashkey = sk;
			sk.EmbedState();

			cmd.Priority = true;
			SendCommand(cmd, false);
		}

		public void SendStashkey()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			if (RTC_StockpileManager.currentStashkey == null)
			{
				MessageBox.Show("Couldn't fetch Stashkey from RTC_StockpileManager.currentStashkey");
				return;
			}

			RTC_Command cmd = new RTC_Command(CommandType.STASHKEY);

			cmd.romFilename = ShortenFilename(GlobalWin.MainForm.CurrentlyOpenRom);

			if (!PeerHasRom(cmd.romFilename))
				cmd.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

			cmd.stashkey = RTC_StockpileManager.currentStashkey;
			cmd.stashkey.EmbedState();

			cmd.Priority = true;

			SendCommand(cmd, false);
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

		private RTC_Command GetLatestScreenFrame(LinkedList<RTC_Command> cmdQueue)
		{
			RTC_Command cmd = null;

			try
			{
				Stack<RTC_Command> vidcmds = new Stack<RTC_Command>();

				foreach (var vidcmd in cmdQueue.ToArray())
					if (vidcmd.Type == CommandType.PUSHSCREEN)
						vidcmds.Push(vidcmd);

				cmd = vidcmds.Peek();

				foreach (var vidcmd in vidcmds)
					cmdQueue.Remove(vidcmd);
			}
			catch (Exception ex)
			{
				OutputException(ex);
			}

			return cmd;
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
                    catch(Exception ex)
                    {
                        MessageBox.Show("NetCore had a thread collision and threw up.\nIn theory this should fix itself after you close this window.\n\n" + ex.ToString());
                        return null;
                    }
                }

				if (cmd == null)
				{
					continue;
				}

				RTC_Command cmdBack = null;

				Console.WriteLine(expectedSide.ToString() + ":ProcessQueue -> " + cmd.Type.ToString());

				switch (cmd.Type)
				{

					#region Netcore commands

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
                        RTC_ConnectionStatus_Form.changeNetCoreSettings((string)cmd.objectValue);
                        break;

                    case CommandType.GETAGGRESSIVENESS:
                        string setting = RTC_Core.csForm.cbNetCoreCommandTimeout.SelectedItem.ToString().ToUpper();
                        RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.AGGRESSIVENESS) { objectValue = setting });
                        break;

                    #endregion
                    case CommandType.ASYNCBLAST:
						{
							BlastLayer bl = RTC_Core.Blast(null, RTC_MemoryDomains.SelectedDomains);
							if (bl != null)
								bl.Apply();
						}
						break;

					case CommandType.BLAST:
						{
							BlastLayer bl = null;
							string[] _domains = (string[])cmd.objectValue;

							if (_domains == null)
								_domains = RTC_MemoryDomains.SelectedDomains;

							if (cmd.blastlayer != null)
							{
								cmd.blastlayer.Apply(cmd.isReplay);
							}
							else
							{
								bl = RTC_Core.Blast(null, _domains);
							}

							if (cmd.requestGuid != null)
							{
								cmdBack = new RTC_Command(CommandType.RETURNVALUE);
								cmdBack.objectValue = bl;
							}


						}

						break;

					case CommandType.STASHKEY:

						if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename))
							File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename, cmd.romData);

						cmd.stashkey.RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + ShortenFilename(cmd.romFilename);

						cmd.stashkey.DeployState();

						cmd.stashkey.Run();

						break;

					case CommandType.PULLROM:
						cmdBack = new RTC_Command(CommandType.PUSHROM);
						cmdBack.romFilename = ShortenFilename(GlobalWin.MainForm.CurrentlyOpenRom);

						if (!PeerHasRom(cmdBack.romFilename))
							cmdBack.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

						break;

					case CommandType.PUSHROM:
						if (cmd.romData != null)
						{
							cmd.romFilename = ShortenFilename(cmd.romFilename);
							if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename))
								File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename, cmd.romData);
						}

						RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename);
						break;

					case CommandType.PULLSTATE:
						cmdBack = new RTC_Command(CommandType.PUSHSTATE);
						StashKey sk_PULLSTATE = RTC_StockpileManager.SaveState(false);
						cmdBack.stashkey = sk_PULLSTATE;
						sk_PULLSTATE.EmbedState();

						break;

					case CommandType.PUSHSTATE:
						cmd.stashkey.DeployState();
						RTC_StockpileManager.LoadState(cmd.stashkey, false);

						if (RTC_Core.multiForm.cbPullStateToGlitchHarvester.Checked)
						{
							StashKey sk_PUSHSTATE = RTC_StockpileManager.SaveState(true, cmd.stashkey);
							sk_PUSHSTATE.RomFilename = GlobalWin.MainForm.CurrentlyOpenRom;
						}

						break;

					case CommandType.PULLSWAPSTATE:

						cmdBack = new RTC_Command(CommandType.PUSHSWAPSTATE);
						cmdBack.romFilename = ShortenFilename(GlobalWin.MainForm.CurrentlyOpenRom);

						if (!PeerHasRom(cmdBack.romFilename))
							cmdBack.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

						StashKey sk_PULLSWAPSTATE = RTC_StockpileManager.SaveState(false);
						cmdBack.stashkey = sk_PULLSWAPSTATE;
						sk_PULLSWAPSTATE.EmbedState();

						cmd.romFilename = ShortenFilename(cmd.romFilename);

						if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename))
							File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename, cmd.romData);
						RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename);

						cmd.stashkey.DeployState();
						RTC_StockpileManager.LoadState(cmd.stashkey, false);

						if (RTC_Core.multiForm.GameOfSwapTimer != null)
							RTC_Core.multiForm.GameOfSwapCounter = 64;

						break;

					case CommandType.PUSHSWAPSTATE:

						cmd.romFilename = ShortenFilename(cmd.romFilename);

						if (cmd.romData != null)
							if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename))
								File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename, cmd.romData);

						RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename);

						cmd.stashkey.DeployState();
						RTC_StockpileManager.LoadState(cmd.stashkey, false);

						if (RTC_Core.multiForm.GameOfSwapTimer != null)
							RTC_Core.multiForm.GameOfSwapCounter = 64;

						break;
					case CommandType.PULLSCREEN:
						cmdBack = new RTC_Command(CommandType.PUSHSCREEN);
						cmdBack.screen = MainForm.MakeScreenshotImage().ToSysdrawingBitmap();
						break;

					case CommandType.REQUESTSTREAM:
						RTC_Core.multiForm.cbStreamScreenToPeer.Checked = true;
						break;

					case CommandType.PUSHSCREEN:
						UpdatePeerScreen(cmd.screen);
						break;

					case CommandType.GAMEOFSWAPSTART:
						RTC_Core.multiForm.StartGameOfSwap(false);
						break;

					case CommandType.GAMEOFSWAPSTOP:
						RTC_Core.multiForm.StopGameOfSwap(true);
						break;

					case CommandType.REMOTE_PUSHPARAMS:
						(cmd.objectValue as RTC_Params).Deploy();
						break;

					case CommandType.REMOTE_LOADROM:
						RTC_Core.LoadRom_NET(cmd.romFilename);
						break;
					case CommandType.REMOTE_LOADSTATE:
						{
							StashKey sk = (StashKey)(cmd.objectValue as object[])[0];
							bool reloadRom = (bool)(cmd.objectValue as object[])[1];
							bool runBlastLayer = (bool)(cmd.objectValue as object[])[2];

							bool returnValue = RTC_StockpileManager.LoadState_NET(sk, reloadRom);

							RTC_MemoryDomains.RefreshDomains(false);
							RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true });

							cmdBack = new RTC_Command(CommandType.RETURNVALUE);
							cmdBack.objectValue = returnValue;

						}
						break;

                    case CommandType.REMOTE_MERGECONFIG:
                        Stockpile.MergeBizhawkConfig_NET();
                        break;

					case CommandType.REMOTE_SAVESTATE:
						{
							StashKey sk = RTC_StockpileManager.SaveState_NET((bool)(cmd.objectValue as object[])[0], (StashKey)(cmd.objectValue as object[])[1]);
							if (cmd.requestGuid != null)
							{
								cmdBack = new RTC_Command(CommandType.RETURNVALUE);
								cmdBack.objectValue = sk;
							}
						}
						break;

					case CommandType.REMOTE_BACKUPKEY_REQUEST:
						{
							if (!RTC_Hooks.isNormalAdvance)
								break;

							cmdBack = new RTC_Command(CommandType.REMOTE_BACKUPKEY_STASH);

							bool multiThread = true;
							if (new string[] {
								"SNES",
							}.Contains(Global.Game.System.ToString().ToUpper()))
								multiThread = false;

								cmdBack.objectValue = RTC_StockpileManager.SaveState_NET(false, null, multiThread);
							break;
						}

					case CommandType.REMOTE_BACKUPKEY_STASH:
						RTC_StockpileManager.backupedState = (StashKey)cmd.objectValue;
						RTC_StockpileManager.allBackupStates.Push((StashKey)cmd.objectValue);
						RTC_Core.coreForm.btnGpJumpBack.Visible = true;
						RTC_Core.coreForm.btnGpJumpNow.Visible = true;
						break;

					case CommandType.REMOTE_DOMAIN_PEEKBYTE:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = RTC_MemoryDomains.getInterface((string)(cmd.objectValue as object[])[0]).PeekByte((long)(cmd.objectValue as object[])[1]);
						break;

					case CommandType.REMOTE_DOMAIN_POKEBYTE:
						RTC_MemoryDomains.getInterface((string)(cmd.objectValue as object[])[0]).PokeByte((long)(cmd.objectValue as object[])[1], (byte)(cmd.objectValue as object[])[2]);
						break;

					case CommandType.REMOTE_DOMAIN_GETDOMAINS:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = RTC_MemoryDomains.getProxies();

						break;

					case CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS:
							RTC_MemoryDomains.UpdateSelectedDomains((string[])cmd.objectValue);
						break;

					case CommandType.REMOTE_DOMAIN_SYSTEM:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = Global.Game.System.ToString().ToUpper();
						break;

					case CommandType.REMOTE_DOMAIN_SYSTEMPREFIX:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = PathManager.SaveStatePrefix(Global.Game);
						break;


					case CommandType.REMOTE_KEY_APPLY:
						RTC_StockpileManager.ApplyStashkey((StashKey)cmd.objectValue);
						break;

					case CommandType.REMOTE_KEY_INJECT:
						RTC_StockpileManager.InjectFromStashkey((StashKey)cmd.objectValue, false);
						break;

					case CommandType.REMOTE_KEY_ORIGINAL:
						RTC_StockpileManager.OriginalFromStashkey((StashKey)cmd.objectValue);
						break;

					case CommandType.REMOTE_KEY_PUSHSAVESTATEDICO:
						RTC_StockpileManager.SavestateStashkeyDico[(string)(cmd.objectValue as object[])[1]] = (StashKey)((cmd.objectValue as object[])[0]);
                        RTC_Core.ghForm.refreshSavestateTextboxes();
                        break;

					case CommandType.REMOTE_KEY_GETPATHENTRY:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = (Global.Config.PathEntries[Global.Game.System, "Savestates"] ?? Global.Config.PathEntries[Global.Game.System, "Base"]).SystemDisplayName;
						break;

					case CommandType.REMOTE_KEY_GETSYSTEMCORE:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = StashKey.getCoreName((string)cmd.objectValue);
						break;

					case CommandType.REMOTE_KEY_GETGAMENAME:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = PathManager.FilesystemSafeName(Global.Game);
						break;

					case CommandType.REMOTE_KEY_GETOPENROMFILENAME:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = GlobalWin.MainForm.CurrentlyOpenRom;
						break;

					case CommandType.REMOTE_KEY_GETRAWBLASTLAYER:
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = RTC_StockpileManager.getRawBlastlayer();
						break;

					case CommandType.REMOTE_SET_SAVESTATEBOX:
						RTC_StockpileManager.currentSavestateKey = (string)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_AUTOCORRUPT:
						RTC_Core.AutoCorrupt = (bool)cmd.objectValue;
						break;

                    case CommandType.REMOTE_SET_INTENSITY:
						RTC_Core.Intensity = (int)cmd.objectValue;
						break;
					case CommandType.REMOTE_SET_ERRORDELAY:
						RTC_Core.ErrorDelay = (int)cmd.objectValue;
						break;
					case CommandType.REMOTE_SET_BLASTRADIUS:
						RTC_Core.Radius = (BlastRadius)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_RESTOREBLASTLAYERBACKUP:
						if (RTC_StockpileManager.lastBlastLayerBackup != null)
							RTC_StockpileManager.lastBlastLayerBackup.Apply(true);
						break;

					case CommandType.REMOTE_SET_NIGHTMARE_TYPE:
						RTC_NightmareEngine.Algo = (BlastByteAlgo)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS:
						RTC_HellgenieEngine.MaxCheats = (int)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND:
						RTC_Core.ClearCheatsOnRewind = (bool)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_HELLGENIE_CLEARALLCHEATS:
						if (Global.CheatList != null)
							Global.CheatList.Clear();
						break;
					case CommandType.REMOTE_SET_HELLGENIE_REMOVEEXCESSCHEATS:
						while (Global.CheatList.Count > RTC_HellgenieEngine.MaxCheats)
							Global.CheatList.Remove(Global.CheatList[0]);
						break;

					case CommandType.REMOTE_SET_PIPE_MAXPIPES:
						RTC_PipeEngine.MaxPipes = (int)cmd.objectValue;
						break;

                    case CommandType.REMOTE_SET_PIPE_TILTVALUE:
						RTC_PipeEngine.TiltValue = (int)cmd.objectValue;
						break;


					case CommandType.REMOTE_SET_PIPE_CLEARPIPES:
						RTC_PipeEngine.AllBlastPipes.Clear();
						RTC_PipeEngine.lastDomain = null;
						break;

					case CommandType.REMOTE_SET_PIPE_LOCKPIPES:
						RTC_PipeEngine.LockPipes = (bool)cmd.objectValue;
						break;

                    case CommandType.REMOTE_SET_PIPE_CHAINEDPIPES:
						RTC_PipeEngine.ChainedPipes = (bool)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_PIPE_PROCESSONSTEP:
						RTC_PipeEngine.ProcessOnStep = (bool)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_PIPE_CLEARPIPESREWIND:
						RTC_Core.ClearPipesOnRewind = (bool)cmd.objectValue;
						break;


					case CommandType.REMOTE_SET_ENGINE:
						RTC_Core.SelectedEngine = (CorruptionEngine)cmd.objectValue;
						break;

					case CommandType.REMOTE_SET_DISTORTION_DELAY:
						RTC_DistortionEngine.MaxAge = (int)cmd.objectValue;
						RTC_DistortionEngine.CurrentAge = 0;
						RTC_DistortionEngine.AllDistortionBytes.Clear();
						break;

					case CommandType.REMOTE_SET_DISTORTION_RESYNC:
						RTC_DistortionEngine.CurrentAge = 0;
						RTC_DistortionEngine.AllDistortionBytes.Clear();
						break;

					case CommandType.REMOTE_SET_VECTOR_LIMITER:
						RTC_VectorEngine.limiterList = (string[])cmd.objectValue;
						break;
					case CommandType.REMOTE_SET_VECTOR_VALUES:
						RTC_VectorEngine.valueList = (string[])cmd.objectValue;
						break;

					case CommandType.REMOTE_EVENT_LOADGAMEDONE_NEWGAME:

						if (RTC_Core.isStandalone && RTC_GameProtection.isRunning)
							RTC_GameProtection.Reset();

						RTC_Core.AutoCorrupt = false;
						//RTC_StockpileManager.isCorruptionApplied = false;
						RTC_Core.coreForm.RefreshDomains();
						RTC_Core.coreForm.setMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
						break;
					case CommandType.REMOTE_EVENT_LOADGAMEDONE_SAMEGAME:
						//RTC_StockpileManager.isCorruptionApplied = false;
						RTC_Core.coreForm.RefreshDomainsAndKeepSelected();
						break;

					case CommandType.REMOTE_EVENT_CLOSEBIZHAWK:
						GlobalWin.MainForm.Close();
						break;

					case CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG:
						GlobalWin.MainForm.SaveConfig();
						break;

					case CommandType.REMOTE_EVENT_BIZHAWKSTARTED:

						if (RTC_StockpileManager.backupedState == null)
							RTC_Core.coreForm.AutoCorrupt = false;

						RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHPARAMS) { objectValue = new RTC_Params() }, true, true);

						Thread.Sleep(100);

                        if(RTC_StockpileManager.backupedState != null)
                            RTC_Core.coreForm.RefreshDomainsAndKeepSelected(RTC_StockpileManager.backupedState.SelectedDomains.ToArray());

                        if (RTC_Core.coreForm.cbUseGameProtection.Checked)
							RTC_GameProtection.Start();

						break;


					case CommandType.REMOTE_HOTKEY_MANUALBLAST:
						RTC_Core.coreForm.btnManualBlast_Click(null, null);
						break;

					case CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE:
						RTC_Core.coreForm.btnAutoCorrupt_Click(null, null);
						break;
					case CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE:
						if (RTC_Core.coreForm.nmErrorDelay.Value > 1)
							RTC_Core.coreForm.nmErrorDelay.Value--;
						break;

					case CommandType.REMOTE_HOTKEY_ERRORDELAYINCREASE:
						if (RTC_Core.coreForm.nmErrorDelay.Value < RTC_Core.coreForm.track_ErrorDelay.Maximum)
							RTC_Core.coreForm.nmErrorDelay.Value++;
						break;

					case CommandType.REMOTE_HOTKEY_INTENSITYDECREASE:
						if (RTC_Core.coreForm.nmIntensity.Value > 1)
							RTC_Core.coreForm.nmIntensity.Value--;
						break;

					case CommandType.REMOTE_HOTKEY_INTENSITYINCREASE:
						if (RTC_Core.coreForm.nmIntensity.Value < RTC_Core.coreForm.track_Intensity.Maximum)
							RTC_Core.coreForm.nmIntensity.Value++;
						break;

					case CommandType.REMOTE_HOTKEY_GHLOADCORRUPT:
						RTC_Core.ghForm.cbAutoLoadState.Checked = true;
						RTC_Core.ghForm.btnCorrupt_Click(null, null);
						break;

					case CommandType.REMOTE_HOTKEY_GHCORRUPT:
						{
							bool isload = RTC_Core.ghForm.cbAutoLoadState.Checked;
							RTC_Core.ghForm.cbAutoLoadState.Checked = false;
							RTC_Core.ghForm.btnCorrupt_Click(null, null);
							RTC_Core.ghForm.cbAutoLoadState.Checked = isload;
						}
						break;

					case CommandType.REMOTE_HOTKEY_GHLOAD:
						RTC_Core.ghForm.btnSaveLoad.Text = "LOAD";
						RTC_Core.ghForm.btnSaveLoad_Click(null, null);
						break;
					case CommandType.REMOTE_HOTKEY_GHSAVE:
						RTC_Core.ghForm.btnSaveLoad.Text = "SAVE";
						RTC_Core.ghForm.btnSaveLoad_Click(null, null);
						break;
                    case CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE:
                        RTC_Core.ghForm.AddStashToStockpile(false);
                        break;

                    case CommandType.REMOTE_HOTKEY_SENDRAWSTASH:
						RTC_Core.ghForm.btnSendRaw_Click(null, null);
						break;

                    case CommandType.REMOTE_HOTKEY_BLASTRAWSTASH:
                        RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.ASYNCBLAST));
                        RTC_Core.ghForm.btnSendRaw_Click(null, null);
				    break;
					case CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE:
						RTC_Core.ghForm.btnBlastToggle_Click(null, null);
						break;
					case CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST:

						if (RTC_StockpileManager.currentStashkey == null || RTC_StockpileManager.currentStashkey.BlastLayer.Layer.Count == 0)
						{
							RTC_Core.ghForm.IsCorruptionApplied = false;
							break;
						}

							RTC_Core.ghForm.IsCorruptionApplied = true;
							RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BLAST) { blastlayer = RTC_StockpileManager.currentStashkey.BlastLayer });
						break;

					case CommandType.REMOTE_RENDER_START:
						RTC_Render.StartRender_NET();
						break;

					case CommandType.REMOTE_RENDER_STOP:
						RTC_Render.StopRender_NET();
						break;

					case CommandType.REMOTE_RENDER_SETTYPE:
						RTC_Render.lastType = (RENDERTYPE)cmd.objectValue;
						break;

					case CommandType.REMOTE_RENDER_STARTED:
						RTC_Core.ghForm.btnRender.Text = "Stop Render";
						RTC_Core.ghForm.btnRender.ForeColor = Color.GreenYellow;
						break;

					case CommandType.REMOTE_RENDER_RENDERATLOAD:
						RTC_StockpileManager.renderAtLoad = (bool)cmd.objectValue;
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

		public static string ShortenFilename(string longFilename)
		{
			if (longFilename.Contains("\\"))
				return longFilename.Substring(longFilename.LastIndexOf("\\") + 1);
			else
				return longFilename;
		}

		public void UpdatePeerScreen(Image img)
		{
			if (RTC_Core.multiForm.btnPopoutPeerGameScreen.Visible == false)
				RTC_Core.multipeerpopoutForm.pbPeerScreen.Image = img;
			else
				RTC_Core.multiForm.pbPeerScreen.Image = img;

		}

		public void SendBlastlayer()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			if (RTC_StockpileManager.currentStashkey == null || RTC_StockpileManager.currentStashkey.BlastLayer == null)
			{
				MessageBox.Show("Couldn't fetch BlastLayer from RTC_StockpileManager.currentStashkey");
				return;
			}

			RTC_Command cmd = new RTC_Command(CommandType.BLAST);
			cmd.blastlayer = RTC_StockpileManager.currentStashkey.BlastLayer;

			SendCommand(cmd, false);
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
				Console.WriteLine($"{expectedSide.ToString()}:SendCommand self:{self.ToString()} priority:{priority.ToString()} -> {cmd.Type.ToString()}" );
				ProcessQueue(tempQueue);
				return null;
			}

			if(!RTC_Core.isStandalone && !RTC_Hooks.isRemoteRTC && cmd.Type == CommandType.RETURNVALUE)
			{
				ReturnWatch.SyncReturns.Add((Guid)cmd.requestGuid, cmd.objectValue);
				return null;
			}

			var cmdQueue = PeerCommandQueue;

			if (self)
				cmdQueue = CommandQueue;

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

		public static object GetValue(Guid WatchedGuid, CommandType type)
		{
			//await Task.Factory.StartNew(() => WaitForValue(WatchedGuid));
			Console.WriteLine("GetValue:Awaiting -> " + type.ToString());

			int maxtries = 0;

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


	[Serializable()]
	public enum CommandType
	{
		//NetCore commands
		HI,
		BYE,
		SAYBYE,
		SAIDBYE,
		CONNECTIONLOST,
		RETURNVALUE,
		BOOP,
        AGGRESSIVENESS,
        GETAGGRESSIVENESS,

        //General RTC commands
        BLAST,
		ASYNCBLAST,
		STASHKEY,
		
		REMOTE_KEY_APPLY, //REFACTORED : TO BE REMOVED
		REMOTE_KEY_INJECT, //REFACTORED : TO BE REMOVED
		REMOTE_KEY_ORIGINAL, //REFACTORED : TO BE REMOVED
		REMOTE_KEY_PUSHSAVESTATEDICO,
		REMOTE_KEY_GETPATHENTRY,
		REMOTE_KEY_GETSYSTEMCORE,
		REMOTE_KEY_GETGAMENAME,
		REMOTE_KEY_GETOPENROMFILENAME,
		REMOTE_KEY_GETRAWBLASTLAYER,
		REMOTE_DOMAIN_GETDOMAINS,
		REMOTE_DOMAIN_SETSELECTEDDOMAINS,
		REMOTE_DOMAIN_SYSTEM,
		REMOTE_DOMAIN_SYSTEMPREFIX,
		REMOTE_DOMAIN_PEEKBYTE,
		REMOTE_DOMAIN_POKEBYTE,
		REMOTE_DOMAIN_GETSIZE,
		REMOTE_PUSHPARAMS,
		REMOTE_LOADROM,
		REMOTE_LOADSTATE,
		REMOTE_SAVESTATE,
        REMOTE_MERGECONFIG,
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

		//General Corruption settings
		REMOTE_SET_SAVESTATEBOX,
		REMOTE_SET_AUTOCORRUPT,
		REMOTE_SET_INTENSITY,
		REMOTE_SET_ERRORDELAY,
		REMOTE_SET_ENGINE,
		REMOTE_SET_BLASTRADIUS,
		REMOTE_SET_RESTOREBLASTLAYERBACKUP,

		// Corruption Core settings and commands
		REMOTE_SET_NIGHTMARE_TYPE,
		REMOTE_SET_HELLGENIE_MAXCHEATS,
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

	public enum NetworkSide
	{
		DISCONNECTED,
		CONNECTIONLOST,
		PENDINGCLIENT,
		CLIENT,
		SERVER
	}

}

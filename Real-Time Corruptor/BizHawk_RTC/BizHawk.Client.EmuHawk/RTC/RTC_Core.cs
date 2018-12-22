using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;

/*
isRemoteRTC = Bool stating that the process is emuhawk.exe in detached
isStandalone = Bool stating the process is standalonertc in detached
*/

namespace RTC
{
	public static class RTC_Core
	{
		public static string[] args;
		public static Random RND = new Random();
		public static List<ProblematicProcess> ProblematicProcesses;

		//General RTC Values
		public static string RtcVersion = "3.30";

		//Directories
		public static string bizhawkDir = Directory.GetCurrentDirectory();

		public static string rtcDir = bizhawkDir + "\\RTC\\";
		public static string workingDir = rtcDir + "\\WORKING\\";
		public static string assetsDir = rtcDir + "\\ASSETS\\";
		public static string paramsDir = rtcDir + "\\PARAMS\\";
		public static string listsDir = rtcDir + "\\LISTS\\";

		//Engine Values
		public static CorruptionEngine SelectedEngine = CorruptionEngine.NIGHTMARE;

		public static BindingList<Object> LimiterListBindingSource = new BindingList<Object>();
		public static BindingList<Object> ValueListBindingSource = new BindingList<Object>();

		private static int customPrecision;
		public static int CustomPrecision
		{
			get { return customPrecision; }
			set
			{
				customPrecision = value;
				CurrentPrecision = value;
			}
	}
		public static int CurrentPrecision = 1;
		public static int Intensity = 1;
		public static int ErrorDelay = 1;
		public static BlastRadius Radius = BlastRadius.SPREAD;
		public static bool AutoCorrupt = false;

		public static bool ClearStepActionsOnRewind = false;
		public static bool ExtractBlastLayer = false;
		public static string lastOpenRom = null;
		public static int lastLoaderRom = 0;

		//RTC Settings
		public static bool BizhawkOsdDisabled = true;
		public static bool UseHexadecimal = true;
		public static bool AllowCrossCoreCorruption = false;
		public static bool DontCleanSavestatesOnQuit = false;

		//Note Box Settings
		public static System.Drawing.Point NoteBoxPosition;
		public static System.Drawing.Size NoteBoxSize;

		//RTC Main Forms
		//public static Color generalColor = Color.FromArgb(60, 45, 70);
		public static Color GeneralColor = Color.LightSteelBlue;


		//All RTC forms
		public static Form[] AllRtcForms
		{
			get
			{
				//This fetches all singletons of interface IAutoColorized

				List<Form> all = new List<Form>();

				foreach (Type t in Assembly.GetAssembly(typeof(S)).GetTypes())
					if (typeof(IAutoColorize).IsAssignableFrom(t) && t != typeof(IAutoColorize))
						all.Add((Form)S.GET(Type.GetType(t.ToString())));

				return all.ToArray();
				
			}
		}

		//NetCores
		public static RTC_NetCore Multiplayer = null;

		public static RTC_NetCore RemoteRTC = null;

		public static bool isStandalone = false;
		public static bool RemoteRTC_SupposedToBeConnected = false;

		public static bool FirstConnection = true;

		public static volatile bool isClosing = false;

		public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
		{
			if (isClosing)
				return;

			isClosing = true;

			if (RTC_Core.Multiplayer != null && RTC_Core.Multiplayer.streamReadingThread != null)
				RTC_Core.Multiplayer.streamReadingThread.Abort();

			if (RTC_Core.RemoteRTC != null && RTC_Core.RemoteRTC.streamReadingThread != null)
				RTC_Core.RemoteRTC.streamReadingThread.Abort();

			foreach (Form frm in AllRtcForms)
			{
				if (frm != null)
					frm.Close();
			}

			if (S.GET<RTC_Standalone_Form>() != null)
				S.GET<RTC_Standalone_Form>().Close();

			//Clean out the working folders
			if (!RTC_Hooks.isRemoteRTC && !RTC_Core.DontCleanSavestatesOnQuit)
			{
				Stockpile.EmptyFolder("\\WORKING\\");
			}

			Application.Exit();
		}


		public static void DownloadProblematicProcesses()
		{
			string LocalPath = RTC_Core.paramsDir + "\\BADPROCESSES";
			string json = "";
			try
			{
				if (File.Exists(LocalPath))
				{
					DateTime lastModified = File.GetLastWriteTime(LocalPath);
					if (lastModified.Date == DateTime.Today)
						return;
				}
				WebClientTimeout client = new WebClientTimeout();
				client.Headers[HttpRequestHeader.Accept] = "text/html, image/png, image/jpeg, image/gif, */*;q=0.1";
				client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
				json = client.DownloadString("https://raw.githubusercontent.com/ircluzar/RTC3/master/ProblematicProcesses.json");
				File.WriteAllText(LocalPath, json);
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					//Couldn't download the new one so just fall back to the old one if it's there
					Console.WriteLine(ex.ToString());
					if (File.Exists(LocalPath))
					{
						try
						{
							json = File.ReadAllText(LocalPath);
						}
						catch (Exception _ex)
						{
							Console.WriteLine("Couldn't read BADPROCESSES\n\n" + _ex.ToString());
							return;
						}
					}
					else
						return;
				}
				else
				{
					Console.WriteLine(ex.ToString());
				}
			}

			try
			{
				ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				if (File.Exists(LocalPath))
					File.Delete(LocalPath);
				return;
			}
		}

		//Checks if any problematic processes are found
		public static volatile bool Warned = false;
		public static void CheckForProblematicProcesses()
		{
			if (Warned || ProblematicProcesses == null)
				return;

			try
			{
				var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

				//Warn based on loaded processes
				foreach (var item in ProblematicProcesses)
				{
					if (processes.Contains(item.Name))
					{
						MessageBox.Show(item.Message, "Incompatible Program Detected!");
						Warned = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}
		}

		public static void StartSound()
		{
			RTC_Hooks.BIZHAWK_STARTSOUND();
		}

		public static void StopSound()
		{
			RTC_Hooks.BIZHAWK_STOPSOUND();
		}

		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			//Timed releases. Only for exceptionnal cases.
			bool Expires = false;
			DateTime ExpiringDate = DateTime.Parse("2017-03-03");
			if (Expires && DateTime.Now > ExpiringDate)
			{
				RTC_RPC.SendToKillSwitch("CLOSE");
				MessageBox.Show("This version has expired");
				RTC_Hooks.BIZHAWK_MAINFORM_CLOSE();
				S.GET<RTC_Core_Form>().Close();
				S.GET<RTC_GlitchHarvester_Form>().Close();
				Application.Exit();
				return;
			}
			/*
			S.GET<RTC_Core_Form>() = new RTC_Core_Form();
			
			cecForm = new RTC_CustomEngineConfig_Form();
			S.GET<RTC_StockpilePlayer_Form>() = new RTC_StockpilePlayer_Form();
			S.GET<RTC_GlitchHarvester_Form>() = new RTC_GlitchHarvester_Form();
			
			hotkeyForm = new RTC_HotkeyConfig_Form();

			multiForm = new RTC_Multiplayer_Form();
			multipeerpopoutForm = new RTC_MultiPeerPopout_Form();
			sbForm = new RTC_StockpileBlastBoard_Form();
			beForm = new RTC_BlastEditor_Form();
			S.GET<RTC_BlastGenerator_Form>() = new RTC_BlastGenerator_Form();


			gpForm = new RTC_GeneralParameters_Form();
			mdForm = new RTC_MemoryDomains_Form();
			S.GET<RTC_CorruptionEngine_Form>() = new RTC_CorruptionEngine_Form();
			vmdPoolForm = new RTC_VmdPool_Form();
			vmdGenForm = new RTC_VmdGen_Form();
			S.GET<RTC_VmdAct_Form>() = new RTC_VmdAct_Form();
			vmdNoToolForm = new RTC_VmdNoTool_Form();
			ecForm = new RTC_EngineConfig_Form();

			saForm = new RTC_SettingsAestethics_Form();
			sgForm = new RTC_SettingsGeneral_Form();
			sncForm = new RTC_SettingsNetCore_Form();
			sForm = new RTC_Settings_Form();
			*/

			S.SET<RTC_Standalone_Form>(_standaloneForm);
			//standaloneForm = _standaloneForm;

			if (!Directory.Exists(RTC_Core.workingDir))
				Directory.CreateDirectory(RTC_Core.workingDir);

			if (!Directory.Exists(RTC_Core.workingDir + "\\TEMP\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\TEMP\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SKS\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SKS\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SSK\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SSK\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SESSION\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SESSION\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\MEMORYDUMPS\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\MEMORYDUMPS\\");

			if (!Directory.Exists(RTC_Core.assetsDir + "\\CRASHSOUNDS\\"))
				Directory.CreateDirectory(RTC_Core.assetsDir + "\\CRASHSOUNDS\\");

			if (!Directory.Exists(RTC_Core.rtcDir + "\\PARAMS\\"))
				Directory.CreateDirectory(RTC_Core.rtcDir + "\\PARAMS\\");

			if (!Directory.Exists(RTC_Core.rtcDir + "\\LISTS\\"))
				Directory.CreateDirectory(RTC_Core.rtcDir + "\\LISTS\\");


			//Loading RTC Params
			RTC_Params.LoadRTCColor();
			S.GET<RTC_SettingsGeneral_Form>().cbDisableBizhawkOSD.Checked = !RTC_Params.IsParamSet("ENABLE_BIZHAWK_OSD");
			S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTC_Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
			S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTC_Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");

			//Load and initialize Hotkeys
			//RTC_Hotkeys.InitializeHotkeySystem();
			//RTC_Params.LoadHotkeys();
			//	RTC_Hotkeys.Test("None", "D", "REMOTE_HOTKEY_MANUALBLAST");

			//Initiation of loopback TCP, only in DETACHED MODE
			if (RTC_Hooks.isRemoteRTC || RTC_Core.isStandalone)
			{
				RemoteRTC = new RTC_NetCore();
				RemoteRTC.port = 42042;
				RemoteRTC.address = "";
			}

			//Initialize RemoteRTC server
			if (RTC_Hooks.isRemoteRTC && !RTC_Core.isStandalone)
			{
				//Bizhawk has started in REMOTERTC mode, no RTC form will be loaded
				RemoteRTC.StartNetworking(NetworkSide.CLIENT, true);
				RemoteRTC.SendCommand(new RTC_Command(CommandType.REMOTE_EVENT_BIZHAWKSTARTED), false, true);
			}
			else
			{
				//Setup of Detached-exclusive features
				if (RTC_Core.isStandalone)
				{
					S.GET<RTC_Core_Form>().Text = "RTC : Detached Mode";

					if (S.ISNULL<RTC_ConnectionStatus_Form>())
						S.SET(new RTC_ConnectionStatus_Form());

					S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());

					RemoteRTC.ServerStarted += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerStarted");

						if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>().IsDisposed)
						{
							if (S.ISNULL<RTC_ConnectionStatus_Form>())
								S.SET(new RTC_ConnectionStatus_Form());

							S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
						}

						if (!S.ISNULL<RTC_GlitchHarvester_Form>() && !S.GET<RTC_GlitchHarvester_Form>().IsDisposed)
						{
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.BringToFront();
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Show();
						}
					});

					RemoteRTC.ServerConnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = true;
						Console.WriteLine("RemoteRTC.ServerConnected");
						S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text = "Connection status: Connected";

						if (FirstConnection)
						{
							FirstConnection = false;
							S.GET<RTC_Core_Form>().btnEngineConfig_Click(ob, ev);
						}
						else
							S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);

						S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Size = S.GET<RTC_GlitchHarvester_Form>().Size;
						S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Hide();
						S.GET<RTC_ConnectionStatus_Form>().btnStartEmuhawkDetached.Text = "Restart BizHawk";

						RTC_RPC.Heartbeat = true;
						RTC_RPC.Freeze = false;
					});

					RemoteRTC.ServerConnectionLost += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerConnectionLost");

						if (S.GET<RTC_ConnectionStatus_Form>() != null && !S.GET<RTC_ConnectionStatus_Form>().IsDisposed)
						{
							S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
							S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());
						}

						if (S.GET<RTC_GlitchHarvester_Form>() != null && !S.GET<RTC_GlitchHarvester_Form>().IsDisposed)
						{
							S.GET<RTC_GlitchHarvester_Form>().lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.BringToFront();
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Show();
						}

						RTC_GameProtection.Stop();
						//Kill the active table autodumps
						S.GET<RTC_VmdAct_Form>().cbAutoAddDump.Checked = false;
					});

					RemoteRTC.ServerDisconnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerDisconnected");
						S.GET<RTC_ConnectionStatus_Form>().lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						S.GET<RTC_GlitchHarvester_Form>().lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_ConnectionStatus_Form>());

						S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.BringToFront();
						S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Show();

						RTC_GameProtection.Stop();
						//Kill the active table autodumps
						S.GET<RTC_VmdAct_Form>().cbAutoAddDump.Checked = false;
					});

					RemoteRTC.StartNetworking(NetworkSide.SERVER, false, false);
				}
				else if (RTC_Hooks.isRemoteRTC)
				{ //WILL THIS EVER HAPPEN? TO BE REMOVED IF NOT
					RemoteRTC.StartNetworking(NetworkSide.SERVER, false, true);
				}

				// Show the main RTC Form
				S.GET<RTC_Core_Form>().Show();
			}

			//Starting UDP loopback for Killswitch 
			RTC_RPC.Start();

			//Refocus on Bizhawk
			RTC_Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			if (!File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
				RTC_Hooks.BIZHAWK_SAVE_CONFIG();

			//Fetch NetCore aggressiveness
			if (RTC_Hooks.isRemoteRTC)
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));
		}

		public static object SendCommandToBizhawk(RTC_Command cmd, bool sync = false, bool priority = false)
		{
			//This is a NetCore wrapper that guarantees a NetCore command is sent to BizHawk no matter which mode.
			//It can query a value in sync or async

			if (RemoteRTC == null)
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
				if ((!RTC_Hooks.isRemoteRTC && !RTC_Core.isStandalone) || RemoteRTC.side == NetworkSide.CLIENT)
				{
					if (sync)
						return RemoteRTC.SendSyncCommand(cmd, true, priority);
					else
						return RemoteRTC.SendCommand(cmd, true, priority);
				}
				else
				{
					if (sync)
						return RemoteRTC.SendSyncCommand(cmd, false, priority);
					else
						return RemoteRTC.SendCommand(cmd, false, priority);
				}
			}
		}

		public static void SendCommandToRTC(RTC_Command cmd)
		{
			//This is a NetCore wrapper that guarantees a NetCore command is sent to RTC no matter which mode.
			//It CANNOT query a value

			if (RTC_Core.RemoteRTC == null)
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
				RemoteRTC.SendCommand(cmd, false);
			}
		}

		public static string EmuFolderCheck(string SystemDisplayName)
		{
			//Workaround for Bizhawk's folder name quirk

			if (SystemDisplayName.Contains("(INTERIM)"))
			{
				char[] delimiters = {'(', ' ', ')'};

				string temp = SystemDisplayName.Split(delimiters)[0];
					SystemDisplayName = temp + "_INTERIM";
			}
			switch (SystemDisplayName)
			{
				case "Playstation":
					return "PSX";
				case "GG":
					return "Game Gear";
				case "Commodore 64":
					return "C64";
				case "SG":
					return "SG-1000";
				default:
					return SystemDisplayName;
			}
		}

		public static BlastUnit GetBlastUnit(string _domain, long _address, int precision)
		{
			//Will generate a blast unit depending on which Corruption Engine is currently set.
			//Some engines like Distortion may not return an Unit depending on the current state on things.

			BlastUnit bu = null;

			switch (SelectedEngine)
			{
				case CorruptionEngine.NIGHTMARE:
					bu = RTC_NightmareEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.HELLGENIE:
					bu = RTC_HellgenieEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.DISTORTION:
					bu = RTC_DistortionEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.FREEZE:
					bu = RTC_FreezeEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.PIPE:
					bu = RTC_PipeEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.VECTOR:
					bu = RTC_VectorEngine.GenerateUnit(_domain, _address);
					break;
				case CorruptionEngine.CUSTOM:
					bu = RTC_CustomEngine.GenerateUnit(_domain, _address, precision);
					break;
				case CorruptionEngine.NONE:
					return null;
			}

			return bu;
		}

		//Generates or applies a blast layer using one of the multiple BlastRadius algorithms
		public static BlastLayer Blast(BlastLayer _layer, string[] _selectedDomains)
		{
			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;
			BlastUnit bu;
			BlastLayer bl;

			try
			{
				if (_layer != null)
				{
					_layer.Apply(); //If the BlastLayer was provided, there's no need to generate a new one.

					return _layer;
				}
				else if (RTC_Core.SelectedEngine == CorruptionEngine.BLASTGENERATORENGINE)
				{
					//It will query a BlastLayer generated by the Blast Generator
					bl = RTC_BlastGeneratorEngine.GetBlastLayer();
					if (bl == null)
						//We return an empty blastlayer so when it goes to apply it, it doesn't find a null blastlayer and try and apply to the domains which aren't enabled resulting in an exception
						return new BlastLayer();
					else
						return bl;
				}
				else
				{
					bl = new BlastLayer();

					if (_selectedDomains == null || _selectedDomains.Count() == 0)
						return null;
					
					// Capping intensity at engine-specific maximums

					int _Intensity = Intensity; //general RTC intensity

					if ((RTC_Core.SelectedEngine == CorruptionEngine.HELLGENIE || RTC_Core.SelectedEngine == CorruptionEngine.FREEZE || RTC_Core.SelectedEngine == CorruptionEngine.PIPE) && _Intensity > RTC_StepActions.MaxInfiniteBlastUnits)
						_Intensity = RTC_StepActions.MaxInfiniteBlastUnits; //Capping for cheat max

					switch (Radius) //Algorithm branching
					{
						case BlastRadius.SPREAD: //Randomly spreads all corruption bytes to all selected domains

							for (int i = 0; i < _Intensity; i++)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
								RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

								bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain

							Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

							MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

							for (int i = 0; i < _Intensity; i++)
							{
								RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

								bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
								if (bu != null)
									bl.Layer.Add(bu);
							}

							break;

						case BlastRadius.BURST: // 10 shots of 10% chunk

							for (int j = 0; j < 10; j++)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;

								for (int i = 0; i < (int)((double)_Intensity / 10); i++)
								{
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NORMALIZED: // Blasts based on the size of the largest selected domain. Intensity =  Intensity / (domainSize[largestdomain]/domainSize[currentdomain])

							//Find the smallest domain and base our normalization around it
							//Domains aren't IComparable so I used keys

							long[] domainSize = new long[_selectedDomains.Length];
							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];
								domainSize[i] = RTC_MemoryDomains.GetInterface(Domain).Size;
							}
							//Sort the arrays
							Array.Sort(domainSize, _selectedDomains);

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								//Get the intensity divider. The size of the largest domain divided by the size of the current domain
								long normalized = ((domainSize[_selectedDomains.Length - 1] / (domainSize[i])));

								for (int j = 0; j < (_Intensity / normalized); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.PROPORTIONAL: //Blasts proportionally based on the total size of all selected domains

							long totalSize = _selectedDomains.Select(it => RTC_MemoryDomains.GetInterface(it).Size).Sum(); //Gets the total size of all selected domains

							long[] normalizedIntensity = new long[_selectedDomains.Length]; //matches the index of selectedDomains
							for (int i = 0; i < _selectedDomains.Length; i++)
							{   //calculates the proportionnal normalized Intensity based on total selected domains size
								double proportion = (double)RTC_MemoryDomains.GetInterface(_selectedDomains[i]).Size / (double)totalSize;
								normalizedIntensity[i] = Convert.ToInt64((double)_Intensity * proportion);
							}

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < normalizedIntensity[i]; j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.EVEN: //Evenly distributes the blasts through all selected domains

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < (_Intensity / _selectedDomains.Length); j++)
								{
									MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = GetBlastUnit(Domain, RandomAddress, RTC_Core.CurrentPrecision);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.NONE: //Shouldn't ever happen but handled anyway
							return null;
					}

					if (bl.Layer.Count == 0)
						return null;
					else
						return bl;
				}
			}
			catch (Exception ex)
			{
				string additionalInfo = "";

				if (RTC_MemoryDomains.GetInterface(Domain) == null)
				{
					additionalInfo = "Unable to get an interface to the selected memory domain! Try clicking the Auto-Select Domains button to refresh the domains!\n\n";
				}

				DialogResult dr = MessageBox.Show("Something went wrong in the RTC Core. \n" +
					additionalInfo +
					"This is an RTC error, so you should probably send this to the RTC devs.\n\n" +
				"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				(S.GET<RTC_Core_Form>().AutoCorrupt ? ">> STOP AUTOCORRUPT ?.\n\n" : "") +
				$"domain:{Domain?.ToString()} maxaddress:{MaxAddress.ToString()} randomaddress:{RandomAddress.ToString()} \n\n" +
				ex.ToString(), "Error", (S.GET<RTC_Core_Form>().AutoCorrupt ? MessageBoxButtons.YesNo : MessageBoxButtons.OK));

				if (dr == DialogResult.Yes || dr == DialogResult.OK)
					S.GET<RTC_Core_Form>().AutoCorrupt = false;

				return null;
			}
		}

		public static BlastTarget GetBlastTarget()
		{
			//Standalone version of BlastRadius SPREAD

			string Domain = null;
			long MaxAddress = -1;
			long RandomAddress = -1;

			string[] _selectedDomains = RTC_MemoryDomains.SelectedDomains;

			Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

			MaxAddress = RTC_MemoryDomains.GetInterface(Domain).Size;
			RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

			return new BlastTarget(Domain, RandomAddress);
		}

		public static string GetRandomKey()
		{
			//Generates unique string ids that are human-readable, unlike GUIDs
			string Key = RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString();
			return Key;
		}

		public static void LoadDefaultRom()
		{
			//Loads a NES-based title screen.
			//Can be overriden by putting a file named "overridedefault.nes" in the ASSETS folder

			int newNumber = lastLoaderRom;

			while (newNumber == lastLoaderRom)
			{
				int nbNesFiles = Directory.GetFiles(RTC_Core.assetsDir, "*.nes").Length;
				
				newNumber = RTC_Core.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(RTC_Core.assetsDir + "overridedefault.nes"))
						RTC_Core.LoadRom(RTC_Core.assetsDir + "overridedefault.nes");
					//Please ignore
					else if (RTC_Core.RND.Next(0, 420) == 7)
						RTC_Core.LoadRom(RTC_Core.assetsDir + "gd.fds");
					else
						RTC_Core.LoadRom(RTC_Core.assetsDir + newNumber.ToString() + "default.nes");

					lastLoaderRom = newNumber;
					break;
				}
			}
		}

		public static void LoadRom(string RomFile, bool sync = false)
		{
			// Safe method for loading a Rom file from any process

			SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADROM)
			{
				romFilename = RomFile
			}, sync);
		}

		public static void LoadRom_NET(string RomFile)
		{
			var loadRomWatch = System.Diagnostics.Stopwatch.StartNew();
			// -> EmuHawk Process only
			//Loads a rom inside Bizhawk from a Filename.

			RTC_Core.StopSound();

			var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM(); ;


			RTC_Hooks.AllowCaptureRewindState = false;
			RTC_Hooks.BIZHAWK_LOADROM(RomFile);
			RTC_Hooks.AllowCaptureRewindState = true;

			RTC_Core.StartSound();
			loadRomWatch.Stop();
			Console.WriteLine($"Time taken for LoadRom_NET: {0}ms", loadRomWatch.ElapsedMilliseconds);
		}

		public static string SaveSavestate_NET(string Key, bool threadSave = false)
		{
			// -> EmuHawk Process only
			//Creates a new savestate and returns the key to it.

			if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE())
				return null;

			string quickSlotName = Key + ".timejump";

			string prefix = RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX();
			prefix = prefix.Substring(prefix.LastIndexOf('\\') + 1);

			var path = RTC_Core.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";

			var file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();


			if (threadSave)
			{
				(new Thread(() =>
				{
					try
					{
						RTC_Hooks.BIZHAWK_SAVESTATE(path, quickSlotName);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Thread collision ->\n" + ex.ToString());
					}
				})).Start();
			}
			else
				RTC_Hooks.BIZHAWK_SAVESTATE(path, quickSlotName);

			return path;
		}

		public static bool LoadSavestate_NET(string Key)
		{
			try
			{

				// -> EmuHawk Process only
				// Loads a Savestate from a key

				if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE())
					return false;

				string quickSlotName = Key + ".timejump";

				string prefix = RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX();
				prefix = prefix.Substring(prefix.LastIndexOf('\\') + 1);

				var path = RTC_Core.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";


				if (File.Exists(path) == false)
				{
					RTC_Hooks.BIZHAWK_OSDMESSAGE("Unable to load " + quickSlotName + ".State");
					return false;
				}

				RTC_Hooks.BIZHAWK_LOADSTATE(path, quickSlotName);

				return true;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
		}

		public static void SetEngineByName(string name)
		{
			//Selects an engine from a given string name

			for (int i = 0; i < S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items.Count; i++)
				if (S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.Items[i].ToString() == name)
				{
					S.GET<RTC_CorruptionEngine_Form>().cbSelectedEngine.SelectedIndex = i;
					break;
				}
		}


		public static void SetRTCHexadecimal(bool useHex, Form form = null)
		{
			//Sets the interface to use Hex across the board

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in AllRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else
			{
				allControls.AddRange(form.Controls.getControlsWithTag());
				allControls.Add(form);
			}

			var hexadecimal = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("hex"));

			foreach (NumericUpDownHexFix updown in hexadecimal)
				updown.Hexadecimal = true;

			foreach(DataGridView dgv in hexadecimal)
				foreach (DataGridViewColumn column in dgv.Columns)
				{
					if (column.CellType.Name == "DataGridViewNumericUpDownCell")
					{
						DataGridViewNumericUpDownColumn _column = column as DataGridViewNumericUpDownColumn;
						_column.Hexadecimal = useHex;
					}
				}
		}


		public static void SetRTCColor(Color color, Form form = null)
		{
			//Recolors all the RTC Forms using the general skin color

			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				foreach (Form targetForm in AllRtcForms)
				{
					if (targetForm != null)
					{
						allControls.AddRange(targetForm.Controls.getControlsWithTag());
						allControls.Add(targetForm);
					}
				}
			}
			else
			{
				allControls.AddRange(form.Controls.getControlsWithTag());
				allControls.Add(form);
			}

			var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));
			var darkererColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darkerer"));

			foreach (Control c in lightColorControls)
				if(c is Label)
					c.ForeColor = color.ChangeColorBrightness(0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(0.30f);

			foreach (Control c in normalColorControls)
				if (c is Label)
					c.ForeColor = color;
				else
					c.BackColor = color;

			S.GET<RTC_StockpilePlayer_Form>().dgvStockpile.BackgroundColor = color;
			S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.BackgroundColor = color;
			

			//TODO
			//beForm.dgvBlastLayer.BackgroundColor = color;

			S.GET<RTC_BlastGenerator_Form>().dgvBlastGenerator.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.30f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.30f);

			foreach (Control c in darkerColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.75f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.75f);

			foreach (Control c in darkererColorControls)
				if (c is Label)
					c.ForeColor = color.ChangeColorBrightness(-0.825f);
				else
					c.BackColor = color.ChangeColorBrightness(-0.825f);
		}

		public static void SelectRTCColor()
		{
			// Show the color dialog.
			Color color;
			ColorDialog cd = new ColorDialog();
			DialogResult result = cd.ShowDialog();
			// See if user pressed ok.
			if (result == DialogResult.OK)
			{
				// Set form background to the selected color.
				color = cd.Color;
			}
			else
				return;

			SetRTCColor(color);

			RTC_Params.SaveRTCColor(color);
		}
	}
}

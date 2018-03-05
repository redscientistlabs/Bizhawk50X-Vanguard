using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Diagnostics;

namespace RTC
{

    public static class RTC_Core
    {
        public static string[] args;
        public static Random RND = new Random();

        //General RTC Values
        public static string RtcVersion = "3.10b";

        //Directories
        public static string bizhawkDir = Directory.GetCurrentDirectory();
        public static string rtcDir = bizhawkDir + "\\RTC";
        public static string paramsDir = rtcDir + "\\PARAMS";

        //Engine Values
        public static CorruptionEngine SelectedEngine = CorruptionEngine.NIGHTMARE;
		public static int CustomPrecision = 1;
		public static int Intensity = 1;
		public static int ErrorDelay = 1;
        public static BlastRadius Radius = BlastRadius.SPREAD;
        public static bool AutoCorrupt = false;

        public static bool ClearCheatsOnRewind = false;
		public static bool ClearPipesOnRewind = false;
		public static bool ExtractBlastLayer = false;
        public static string lastOpenRom = null;
        public static int lastLoaderRom = 0;

        //RTC Settings
        public static bool BizhawkOsdDisabled = true;
        public static bool AllowCrossCoreCorruption = false;




        //RTC Main Forms
        public static Color generalColor = Color.LightSteelBlue;
		public static RTC_Core_Form coreForm = null;
        public static RTC_EngineConfig_Form ecForm = null;
        public static RTC_StockpilePlayer_Form spForm = null;
        public static RTC_GlitchHarvester_Form ghForm = null;
        public static RTC_Settings_Form sForm = null;

        //RTC Extension Forms
        public static RTC_Multiplayer_Form multiForm;
		public static RTC_MultiPeerPopout_Form multipeerpopoutForm = null;
		public static RTC_StockpileBlastBoard_Form sbForm = null;
		public static RTC_ConnectionStatus_Form csForm = null;
		public static RTC_BlastEditor_Form beForm = null;

		public static Form standaloneForm = null;
        
        //RTC Advanced Tool Forms
        public static RTC_VmdPool_Form vmdPoolForm = null;
        public static RTC_VmdGen_Form vmdGenForm = null;
        public static RTC_VmdAct_Form vmdActForm = null;

        //All RTC forms
        public static Form[] allRtcForms
        {
            get
            {
                return new Form[]
                    {
                        coreForm,
                        sForm,
                        ecForm,
                        ghForm,
                        spForm,

                        multiForm,
                        multipeerpopoutForm,
                        sbForm,
                        beForm,

                        vmdActForm,
                        vmdGenForm,
                        vmdPoolForm,

                        standaloneForm,
                    };
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

            foreach(Form frm in allRtcForms)
            {
                if (frm != null)
                    frm.Close();
            }

            if (RTC_Core.standaloneForm != null)
                RTC_Core.standaloneForm.Close();


            if (!RTC_Hooks.isRemoteRTC && File.Exists(RTC_Core.bizhawkDir + "\\StateClean.bat")) //We force useless savestates to clear on quit to prevent disk usage to inflate too much
			{
				Process p = new Process();
				p.StartInfo.FileName = RTC_Core.bizhawkDir + "\\StateClean.bat";
				p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				p.StartInfo.CreateNoWindow = true;
				p.Start();
			}

			Application.Exit();
		}


        //Process-safe methods for starting/stopping sound in Bizhawk
		public static void StartSound(){
            if (GlobalWin.MainForm != null) { GlobalWin.Sound.StartSound(); }
		}
		public static void StopSound(){
            if (GlobalWin.MainForm != null) { GlobalWin.Sound.StopSound(); }
		}

        //Checks if any problematic processes are found
        public static bool Warned = false;
        public static void CheckForProblematicProcesses()
        {
            if (Warned)
                return;

            /*
            // PROCESS CHECKUP IS NO LONGER AVAILABLE WITH BIZHAWK 2.2
            try
            {
                var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

                if (processes.Contains("XGS32") || processes.Contains("XGS64"))
                {
                    MessageBox.Show("XSplit Game Capture detected. XSplit's Game Capture is incompatible with BizHawk's N64 Emulator. Disable it via the XSplit options and restart XSplit.");
                    Warned = true;
                }

                if (processes.Contains("RTSS") || processes.Contains("RIVATUNERSTATISTICSSERVER"))
                {
                    MessageBox.Show("Rivatuner Statistics Server OSD Detected. The RTSS OSD is incompatible with BizHawk's N64 Emulator. If you're using any GPU overclocking software such as MSI Afterburner, disable the OSD while running the RTC or blacklist emuhawk.exe in the RTSS Settings");
                    Warned = true;
                }

                if (processes.Contains("PRECISIONXSERVER"))
                {
                    MessageBox.Show("EVGA Precision X OSD detected. The OSD is incompatible with BizHawk's N64 Emulator. Disable the OSD in the Precision X menu or blacklist emuhawk.exe in PrecisionX Server.");
                    Warned = true;
                }
                

            }
            catch
            {
                //Let's just do nothing in that case.
            }
            */

        }

        //This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(Form _standaloneForm = null)
        {

            //Timed releases. Only for exceptionnal cases.
            bool Expires = false;
            DateTime ExpiringDate = DateTime.Parse("2017-03-03");
            if (Expires && DateTime.Now > ExpiringDate)
            {
                RTC_RPC.SendToKillSwitch("CLOSE");
                MessageBox.Show("This version has expired");
                GlobalWin.MainForm.Close();
                RTC_Core.coreForm.Close();
                RTC_Core.ghForm.Close();
                Application.Exit();
                return;
            }

			coreForm = new RTC_Core_Form();
            ecForm = new RTC_EngineConfig_Form();
            spForm = new RTC_StockpilePlayer_Form();
            ghForm = new RTC_GlitchHarvester_Form();
            sForm = new RTC_Settings_Form();
			
			multiForm = new RTC_Multiplayer_Form();
			multipeerpopoutForm = new RTC_MultiPeerPopout_Form();
			sbForm = new RTC_StockpileBlastBoard_Form();
			beForm = new RTC_BlastEditor_Form();
            vmdPoolForm = new RTC_VmdPool_Form();
            vmdGenForm = new RTC_VmdGen_Form();
            vmdActForm = new RTC_VmdAct_Form();

			standaloneForm = _standaloneForm;


            if (!Directory.Exists(RTC_Core.rtcDir + "\\TEMP\\"))
                Directory.CreateDirectory(RTC_Core.rtcDir + "\\TEMP\\");

            if (!Directory.Exists(RTC_Core.rtcDir + "\\TEMP2\\"))
                Directory.CreateDirectory(RTC_Core.rtcDir + "\\TEMP2\\");

            if (!Directory.Exists(RTC_Core.rtcDir + "\\TEMP3\\"))
                Directory.CreateDirectory(RTC_Core.rtcDir + "\\TEMP3\\");

            if (!Directory.Exists(RTC_Core.rtcDir + "\\TEMP4\\"))
                Directory.CreateDirectory(RTC_Core.rtcDir + "\\TEMP4\\");


            //Loading RTC PArams
            RTC_Params.LoadRTCColor();
            RTC_Core.sForm.cbDisableBizhawkOSD.Checked = !RTC_Params.IsParamSet("ENABLE_BIZHAWK_OSD");
            RTC_Core.sForm.cbAllowCrossCoreCorruption.Checked = RTC_Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");

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
					coreForm.Text = "RTC : Detached Mode";

                    if(csForm == null)
					    csForm = new RTC_ConnectionStatus_Form();

                    RTC_Core.coreForm.showPanelForm(csForm);

					RemoteRTC.ServerStarted += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerStarted");


                        if (csForm != null && !csForm.IsDisposed)
                        {
                            if (RTC_Core.csForm == null)
                                csForm = new RTC_ConnectionStatus_Form();

                            RTC_Core.coreForm.showPanelForm(csForm);
                        }

                        if (ghForm != null && !ghForm.IsDisposed)
                        {
                            ghForm.pnHideGlitchHarvester.BringToFront();
                            ghForm.pnHideGlitchHarvester.Show();
                        }
					});

					RemoteRTC.ServerConnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = true;
						Console.WriteLine("RemoteRTC.ServerConnected");
						csForm.lbConnectionStatus.Text = "Connection status: Connected";

                        if (FirstConnection)
                        {
                            FirstConnection = false;
                            coreForm.btnEngineConfig_Click(ob, ev);
                        }
                        else
                            coreForm.showPanelForm(coreForm.previousForm, false);

						ghForm.pnHideGlitchHarvester.Hide();
						csForm.btnStartEmuhawkDetached.Text = "Restart BizHawk";

						RTC_RPC.Heartbeat = true;
						RTC_RPC.Freeze = false;

					});


					RemoteRTC.ServerConnectionLost += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerConnectionLost");


                        if (csForm != null && !csForm.IsDisposed)
                        {
                            csForm.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
                            coreForm.showPanelForm(csForm);
                        }

                        if (ghForm != null && !ghForm.IsDisposed)
                        {
                            ghForm.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
                            ghForm.pnHideGlitchHarvester.BringToFront();
                            ghForm.pnHideGlitchHarvester.Show();
                        }

						RTC_GameProtection.Stop();

					});

					RemoteRTC.ServerDisconnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerDisconnected");
						csForm.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						ghForm.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
                        coreForm.showPanelForm(csForm);

						ghForm.pnHideGlitchHarvester.BringToFront();
						ghForm.pnHideGlitchHarvester.Show();

						RTC_GameProtection.Stop();

					});

					RemoteRTC.StartNetworking(NetworkSide.SERVER,false,false);
				}
				else if (RTC_Hooks.isRemoteRTC)
				{ //WILL THIS EVER HAPPEN? TO BE REMOVED IF NOT
					RemoteRTC.StartNetworking(NetworkSide.SERVER,false,true);
				}
				
                // Show the main RTC Form
				coreForm.Show();

			}

            //Starting UDP loopback for Killswitch and External Rom Plugins
            RTC_RPC.Start();


            //Refocus on Bizhawk
			if (GlobalWin.MainForm != null)
				GlobalWin.MainForm.Focus();


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

			if(RemoteRTC == null)
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

            switch (SystemDisplayName)
            {
                case "Playstation":
                    return"PSX";
                case "GG":
                    return "Game Gear";
                case "Commodore 64":
                    return "C64";
                default:
					return SystemDisplayName;
			}
            
        }

        public static BlastUnit getBlastUnit(string _domain, long _address)
        {
            //Will generate a blast unit depending on which Corruption Engine is currently set.
            //Some engines like Distortion may not return an Unit depending on the current state on things.

            BlastUnit bu = null;

            switch(SelectedEngine)
            {
                case CorruptionEngine.NIGHTMARE:
                    bu = RTC_NightmareEngine.GenerateUnit(_domain, _address);
                    break;
                case CorruptionEngine.HELLGENIE:
                    bu = RTC_HellgenieEngine.GenerateUnit(_domain, _address);
                    break;
                case CorruptionEngine.DISTORTION:
                    RTC_DistortionEngine.AddUnit(RTC_DistortionEngine.GenerateUnit(_domain, _address));
                    bu = RTC_DistortionEngine.GetUnit();
                    break;
                case CorruptionEngine.FREEZE:
                    bu = RTC_FreezeEngine.GenerateUnit(_domain, _address);
                    break;
				case CorruptionEngine.PIPE:
					bu = RTC_PipeEngine.GenerateUnit(_domain, _address);
					break;
				case CorruptionEngine.VECTOR:
					bu = RTC_VectorEngine.GenerateUnit(_domain, _address);
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
                else if (RTC_Core.SelectedEngine == CorruptionEngine.EXTERNALROM) 
                {   //External ROM Plugin: Bypasses domains and uses an alternative algorithm to fetch corruption.
                    //It will query a BlastLayer generated from a differential between an original and corrupted rom.
                    bl = RTC_ExternalRomPlugin.GetBlastLayer();
                    if (bl == null)
                        return null;
                    else
                        return bl;

                }
                else
                {
                    bl = new BlastLayer();

                    if (_selectedDomains == null || _selectedDomains.Count() == 0)
						return null;

					// Age distortion BlastBytes
                    if (RTC_Core.SelectedEngine == CorruptionEngine.DISTORTION && RTC_DistortionEngine.CurrentAge < RTC_DistortionEngine.MaxAge)
                        RTC_DistortionEngine.CurrentAge++;

					//Run Pipes on Corrupt Step if required
					if (RTC_Core.SelectedEngine == CorruptionEngine.PIPE && !RTC_PipeEngine.ProcessOnStep)
						RTC_PipeEngine.ExecutePipes();


					// Capping intensity at engine-specific maximums

					int _Intensity = Intensity; //general RTC intensity

					if ((RTC_Core.SelectedEngine == CorruptionEngine.HELLGENIE || RTC_Core.SelectedEngine == CorruptionEngine.FREEZE) && _Intensity > RTC_HellgenieEngine.MaxCheats)
						_Intensity = RTC_HellgenieEngine.MaxCheats; //Capping for cheat max

					if (RTC_Core.SelectedEngine == CorruptionEngine.PIPE && _Intensity > RTC_PipeEngine.MaxPipes)
						_Intensity = RTC_PipeEngine.MaxPipes; //Capping for pipe max


					switch (Radius) //Algorithm branching
                    {
                        case BlastRadius.SPREAD: //Randomly spreads all corruption bytes to all selected domains

                            for (int i = 0; i < _Intensity; i++) 
                            {
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

                                MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
                                RandomAddress = RTC_Core.RND.RandomLong(MaxAddress -1);

                                bu = getBlastUnit(Domain, RandomAddress);
                                if (bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

                        case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain

							Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

							MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;

                            for (int i = 0; i < _Intensity; i++)
                            {
                                RandomAddress = RTC_Core.RND.RandomLong(MaxAddress -1);

                                bu = getBlastUnit(Domain, RandomAddress);
                                if(bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

						case BlastRadius.BURST: // 10 shots of 10% chunk

							for (int j = 0; j < 10; j++)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];

								MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;

								for (int i = 0; i < (int)((double)_Intensity / 10); i++)
								{
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress);
									if (bu != null)
										bl.Layer.Add(bu);
								}

							}

							break;

						case BlastRadius.NORMALIZED: // Blasts based on the size of the largest selected domain. Intensity =  Intensity / (domainSize[largestdomain]/domainSize[currentdomain])

                            
							//Find the smallest domain and base our normalization around it
							//Domains aren't IComparable so I used keys

							long[] domainSize = new long [_selectedDomains.Length];
							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];
								domainSize[i] = RTC_MemoryDomains.getInterface(Domain).Size;
							}
							//Sort the arrays
							Array.Sort(domainSize, _selectedDomains);

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								//Get the intensity divider. The size of the largest domain divided by the size of the current domain
								long normalized = ((domainSize[_selectedDomains.Length-1] / (domainSize[i])));

								for (int j = 0; j < (_Intensity / normalized); j++)
								{
									MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.PROPORTIONAL: //Blasts proportionally based on the total size of all selected domains

							long totalSize = _selectedDomains.Select(it => RTC_MemoryDomains.getInterface(it).Size).Sum(); //Gets the total size of all selected domains

							long[] normalizedIntensity = new long[_selectedDomains.Length]; //matches the index of selectedDomains
							for (int i = 0; i < _selectedDomains.Length; i++)
							{   //calculates the proportionnal normalized Intensity based on total selected domains size
								double proportion = (double)RTC_MemoryDomains.getInterface(_selectedDomains[i]).Size / (double)totalSize;
								normalizedIntensity[i] = Convert.ToInt64((double)_Intensity * proportion);
							}

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < normalizedIntensity[i]; j++)
								{
									MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress);
									if (bu != null)
										bl.Layer.Add(bu);
								}
							}

							break;

						case BlastRadius.EVEN: //Evenly distributes the blasts through all selected domains 

							for (int i = 0; i < _selectedDomains.Length; i++)
							{
								Domain = _selectedDomains[i];

								for (int j = 0; j < (_Intensity/_selectedDomains.Length); j++)
								{
									MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
									RandomAddress = RTC_Core.RND.RandomLong(MaxAddress - 1);

									bu = getBlastUnit(Domain, RandomAddress);
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
				DialogResult dr = MessageBox.Show("Something went wrong in the RTC Core. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
				"If you know the steps to reproduce this error it would be greatly appreaciated.\n\n" +
				(RTC_Core.coreForm.AutoCorrupt ? ">> STOP AUTOCORRUPT ?.\n\n" : "") +
				$"domain:{Domain.ToString()} maxaddress:{MaxAddress.ToString()} randomaddress:{RandomAddress.ToString()} \n\n" +
				ex.ToString(), "Error", (RTC_Core.coreForm.AutoCorrupt ? MessageBoxButtons.YesNo : MessageBoxButtons.OK));

				if (dr == DialogResult.Yes || dr == DialogResult.OK)
					RTC_Core.coreForm.AutoCorrupt = false;

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

            if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
                Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];
            else
                Domain = RTC_MemoryDomains.MainDomain.ToString();

            MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
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
                newNumber = RTC_Core.RND.Next(1, 17);

                if (newNumber != lastLoaderRom)
                {
                    if(File.Exists(RTC_Core.rtcDir + "\\ASSETS\\" + "overridedefault.nes"))
                        RTC_Core.LoadRom(RTC_Core.rtcDir + "\\ASSETS\\" + "overridedefault.nes");
                    else
                        RTC_Core.LoadRom(RTC_Core.rtcDir + "\\ASSETS\\" + newNumber.ToString() + "default.nes");
                    
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
            // -> EmuHawk Process only
            //Loads a rom inside Bizhawk from a Filename.

            RTC_Core.StopSound();

			var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = GlobalWin.MainForm.CurrentlyOpenRom;

			var lra = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs { OpenAdvanced = new OpenAdvanced_OpenRom { Path = RomFile } };

			RTC_Hooks.AllowCaptureRewindState = false;
			GlobalWin.MainForm.LoadRom(RomFile, lra);
			RTC_Hooks.AllowCaptureRewindState = true;

			RTC_Core.StartSound();

		}

        public static string SaveSavestate_NET(string Key, bool threadSave = false)
        {
            // -> EmuHawk Process only
            //Creates a new savestate and returns the key to it.

            if (Global.Emulator is NullEmulator)
                return null;

			string quickSlotName = Key + ".timejump";


			//string prefix = (string)SendCommandRTC(new RTC_Command(CommandType.REMOTE_DOMAINS_SYSTEMPREFIX), true);
			string prefix = PathManager.SaveStatePrefix(Global.Game);

			var path = prefix + "." + quickSlotName + ".State";

            var file = new FileInfo(path);
            if (file.Directory != null && file.Directory.Exists == false)
                file.Directory.Create();


            //Filtering out filename parts 
            path = path.Replace(".Performance.", ".");
            path = path.Replace(".Compatibility.", ".");
            path = path.Replace(".QuickNes.", ".");
            path = path.Replace(".NesHawk.", ".");
            path = path.Replace(".VBA-Next.", ".");
			path = path.Replace(".mGBA.", ".");
            path = path.Replace(".Snes9x.", ".");

            if (threadSave)
			{
				(new Thread(() => {
					try
					{
						GlobalWin.MainForm.SaveState(path, quickSlotName, false);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Thread collision ->\n" + ex.ToString());
					}

				})).Start();
			}
			else
				GlobalWin.MainForm.SaveState(path, quickSlotName, false);


			return path;
        }

        public static void LoadSavestate_NET(string Key, bool fromLua = false)
        {
            // -> EmuHawk Process only
            // Loads a Savestate from a key

            if (Global.Emulator is NullEmulator)
                return;

			string quickSlotName = Key + ".timejump";

			//string prefix = (string)SendCommandRTC(new RTC_Command(CommandType.REMOTE_DOMAINS_SYSTEMPREFIX), true);
			string prefix = PathManager.SaveStatePrefix(Global.Game);

			var path = prefix + "." + quickSlotName + ".State";

            //Filtering out parts 
            path = path.Replace(".Performance.", ".");
            path = path.Replace(".Compatibility.", ".");
            path = path.Replace(".QuickNes.", ".");
            path = path.Replace(".NesHawk.", ".");
            path = path.Replace(".VBA-Next.", ".");
			path = path.Replace(".mGBA.", ".");
            path = path.Replace(".Snes9x.", ".");

            if (File.Exists(path) == false)
            {
                GlobalWin.OSD.AddMessage("Unable to load " + quickSlotName + ".State");
                return;
            }

            GlobalWin.MainForm.LoadState(path, quickSlotName, fromLua);
        }

        public static void SetEngineByName(string name)
        {
            //Selects an engine from a given string name

            for (int i = 0; i < ecForm.cbSelectedEngine.Items.Count; i++)
                if (ecForm.cbSelectedEngine.Items[i].ToString() == name)
                {
                    ecForm.cbSelectedEngine.SelectedIndex = i;
                    break;
                }

        }



		public static void SetRTCColor(Color color, Form form = null)
		{
            //Recolors all the RTC Forms using the general skin color

			List<Control> allControls = new List<Control>();

			if (form == null)
			{

                foreach(Form targetForm in allRtcForms)
				    if (targetForm != null)
				    {
					    allControls.AddRange(targetForm.Controls.getControlsWithTag());
					    allControls.Add(targetForm);
				    }
            }
			else
				allControls.AddRange(form.Controls.getControlsWithTag());

			var lightColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => ((it.Tag as string) ?? "").Contains("color:darker"));

			foreach (Control c in lightColorControls)
				c.BackColor = color.ChangeColorBrightness(0.30f);

			foreach (Control c in normalColorControls)
				c.BackColor = color;

			spForm.dgvStockpile.BackgroundColor = color;
			ghForm.dgvStockpile.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				c.BackColor = color.ChangeColorBrightness(-0.30f);

			foreach (Control c in darkerColorControls)
				c.BackColor = color.ChangeColorBrightness(-0.75f);

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

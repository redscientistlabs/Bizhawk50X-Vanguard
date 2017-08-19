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
		public static string RtcVersion = "2.87";
		
        public static Random RND = new Random();
        public static string[] args;

        //Values
        public static CorruptionEngine SelectedEngine = CorruptionEngine.NIGHTMARE;
		public static int Intensity = 1;
		public static int ErrorDelay = 1;
        public static BlastRadius Radius = BlastRadius.SPREAD;

        public static bool ClearCheatsOnRewind = false;
		public static bool ClearPipesOnRewind = false;
		public static bool ExtractBlastLayer = false;
        public static string lastOpenRom = null;
        public static int lastLoaderRom = 0;

        public static bool AutoCorrupt = false;

        //General Values
        public static string bizhawkDir = Directory.GetCurrentDirectory();
        public static string rtcDir = bizhawkDir + "\\RTC";

		//Forms
		public static Color generalColor = Color.LightSteelBlue;
		public static RTC_Form coreForm = null;
		public static RTC_GH_Form ghForm = null;
        public static RTC_SP_Form spForm = null;
		public static RTC_Multi_Form multiForm;
		public static RTC_MultiPeerPopout_Form multipeerpopoutForm = null;
		public static RTC_StockpileBlastBoard sbForm = null;
		public static RTC_ConnectionStatus_Form csForm = null;
		public static RTC_BlastEditorForm beForm = null;
		public static Form standaloneForm = null;

		//Bizhawk Overrides
		public static bool Bizhawk_OSD_Enabled = false;

		//NetCores
		public static RTC_NetCore Multiplayer = null;
		public static RTC_NetCore RemoteRTC = null;

		public static bool isStandalone = false;
		public static bool RemoteRTC_SupposedToBeConnected = false;

		public static volatile bool isClosing = false;
		public static void CloseAllRtcForms()
		{
			if (isClosing)
				return;

			isClosing = true;

			if (RTC_Core.Multiplayer != null && RTC_Core.Multiplayer.streamReadingThread != null)
				RTC_Core.Multiplayer.streamReadingThread.Abort();
		
			if (RTC_Core.RemoteRTC != null && RTC_Core.RemoteRTC.streamReadingThread != null)
				RTC_Core.RemoteRTC.streamReadingThread.Abort();

			if (coreForm != null)
				coreForm.Close();
			if (ghForm != null)
				ghForm.Close();
			if (spForm != null)
				spForm.Close();
			if (multiForm != null)
				multiForm.Close();
			if (multipeerpopoutForm != null)
				multipeerpopoutForm.Close();
			if (sbForm != null)
				sbForm.Close();
			if (csForm != null)
				csForm.Close();
			if (standaloneForm != null)
				standaloneForm.Close();

			if (!RTC_Hooks.isRemoteRTC)
			{
				Process p = new Process();
				p.StartInfo.FileName = RTC_Core.bizhawkDir + "\\StateClean.bat";
				p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				p.StartInfo.CreateNoWindow = true;
				p.Start();
			}

			Application.Exit();
		}

		public static void StartSound()
		{
			if(GlobalWin.MainForm != null)
				GlobalWin.Sound.StartSound();
		}

		public static void StopSound()
		{
			if (GlobalWin.MainForm != null)
				GlobalWin.Sound.StopSound();
		}

		public static void Start(Form _standaloneForm = null)
        {

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

			coreForm = new RTC_Form();
			ghForm = new RTC_GH_Form();
			spForm = new RTC_SP_Form();
			multiForm = new RTC_Multi_Form();
			multipeerpopoutForm = new RTC_MultiPeerPopout_Form();
			sbForm = new RTC_StockpileBlastBoard();
			beForm = new RTC_BlastEditorForm();

			standaloneForm = _standaloneForm;


			if (isStandalone || !RTC_Hooks.isRemoteRTC)
			{
				if (File.Exists(rtcDir + "\\params\\COLOR.TXT"))
				{
					string[] bytes = File.ReadAllText(rtcDir + "\\params\\COLOR.TXT").Split(',');
					SetRTCColor(Color.FromArgb(Convert.ToByte(bytes[0]), Convert.ToByte(bytes[1]), Convert.ToByte(bytes[2])));
				}
				else
					SetRTCColor(Color.FromArgb(110, 150, 193));
			}

			if (RTC_Hooks.isRemoteRTC || RTC_Core.isStandalone)
			{
				RemoteRTC = new RTC_NetCore();
				RemoteRTC.port = 42042;
				RemoteRTC.address = "";
			}

			//Initialize RemoteRTC server
			if (RTC_Hooks.isRemoteRTC && !RTC_Core.isStandalone)
			{
				RemoteRTC.StartNetworking(NetworkSide.CLIENT, true);
				RemoteRTC.SendCommand(new RTC_Command(CommandType.REMOTE_EVENT_BIZHAWKSTARTED), false, true);

			}
			else
			{

				if (RTC_Core.isStandalone)
				{
					coreForm.Text = "RTC : Detached Mode";

					csForm = new RTC_ConnectionStatus_Form();
					csForm.TopLevel = false;
					csForm.Location = new Point(0, 0);
					coreForm.Controls.Add(csForm);
					csForm.TopMost = true;

					RemoteRTC.ServerStarted += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerStarted");
						coreForm.pnEngineConfig.Hide();
						coreForm.pnLeftPanel.Hide();
						csForm.btnReturnToSession.Visible = false;
						csForm.Show();

						ghForm.pnHideGlitchHarvester.BringToFront();
						ghForm.pnHideGlitchHarvester.Show();
					});

					RemoteRTC.ServerConnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = true;
						Console.WriteLine("RemoteRTC.ServerConnected");
						coreForm.pnEngineConfig.Show();
						coreForm.pnLeftPanel.Show();
						csForm.lbConnectionStatus.Text = "Connection status: Connected";
						csForm.btnReturnToSession.Visible = true;
						csForm.Hide();

						ghForm.pnHideGlitchHarvester.Hide();
						csForm.btnStartEmuhawkDetached.Text = "Restart BizHawk";

						RTC_RPC.Heartbeat = true;
						RTC_RPC.Freeze = false;

					});


					RemoteRTC.ServerConnectionLost += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerConnectionLost");
						coreForm.pnEngineConfig.Hide();
						coreForm.pnLeftPanel.Hide();
						csForm.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
						csForm.btnReturnToSession.Visible = false;
						ghForm.lbConnectionStatus.Text = "Connection status: Bizhawk timed out";
						csForm.Show();

						ghForm.pnHideGlitchHarvester.BringToFront();
						ghForm.pnHideGlitchHarvester.Show();

						RTC_GameProtection.Stop();

					});

					RemoteRTC.ServerDisconnected += new EventHandler((ob, ev) =>
					{
						RemoteRTC_SupposedToBeConnected = false;
						Console.WriteLine("RemoteRTC.ServerDisconnected");
						coreForm.pnEngineConfig.Hide();
						coreForm.pnLeftPanel.Hide();
						csForm.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						csForm.btnReturnToSession.Visible = false;
						ghForm.lbConnectionStatus.Text = "Connection status: NetCore Shutdown";
						csForm.Show();

						ghForm.pnHideGlitchHarvester.BringToFront();
						ghForm.pnHideGlitchHarvester.Show();

						RTC_GameProtection.Stop();

					});

					RemoteRTC.StartNetworking(NetworkSide.SERVER,false,false);
				}
				else if (RTC_Hooks.isRemoteRTC)
				{
					RemoteRTC.StartNetworking(NetworkSide.SERVER,false,true);
				}
				
				coreForm.Show();

				
			}

            RTC_RPC.Start();


			if (GlobalWin.MainForm != null)
				GlobalWin.MainForm.Focus();


            //Force create bizhawk config file if it doesn't exist
            if (!File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
                RTC_Hooks.BIZHAWK_SAVE_CONFIG();

            if (RTC_Hooks.isRemoteRTC)
                RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));

        }
			
			
		

		public static object SendCommandToBizhawk(RTC_Command cmd, bool sync = false, bool priority = false)
		{
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


		public static long RandomLong(long max)
        {
			return RND.RandomLong(0, max);
		}

        public static string EmuFolderCheck(string SystemDisplayName)
        {
            switch (SystemDisplayName)
            {
                case "Playstation":
                    return"PSX";
                case "GG":
                    return "Game Gear";
				default:
					return SystemDisplayName;
			}
            
        }

        public static BlastUnit getBlastUnit(string _domain, long _address)
        {

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

        //Generates or queries a blast layer then applies it.
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
                {
                    BlastLayer romLayer = RTC_ExternalRomPlugin.GetLayer();
                    if (romLayer == null)
                    {
                        return null;
                    }
                    else
                    {
                        //romLayer.Apply();
                        return romLayer;
                    }
                }
                else
                {
                    bl = new BlastLayer();

                    if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE && (_selectedDomains == null || _selectedDomains.Count() == 0))
						return null;

					// Age distortion BlastBytes
                    if (RTC_Core.SelectedEngine == CorruptionEngine.DISTORTION && RTC_DistortionEngine.CurrentAge < RTC_DistortionEngine.MaxAge)
                        RTC_DistortionEngine.CurrentAge++;

					//Run Pipes on Corrupt Step if required
					if (RTC_Core.SelectedEngine == CorruptionEngine.PIPE && !RTC_PipeEngine.ProcessOnStep)
						RTC_PipeEngine.ExecutePipes();


					// Capping intensity at maximums
					int _Intensity = Intensity;

					if ((RTC_Core.SelectedEngine == CorruptionEngine.HELLGENIE || RTC_Core.SelectedEngine == CorruptionEngine.FREEZE) && _Intensity > RTC_HellgenieEngine.MaxCheats)
						_Intensity = RTC_HellgenieEngine.MaxCheats;

					if (RTC_Core.SelectedEngine == CorruptionEngine.PIPE && _Intensity > RTC_PipeEngine.MaxPipes)
						_Intensity = RTC_PipeEngine.MaxPipes;

					switch (Radius)
                    {
                        case BlastRadius.SPREAD:

                            for (int i = 0; i < _Intensity; i++) //Randomly spreads all corruption bytes to all selected domains
                            {
								if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
								{
									Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];
								}
								else
								{
									Domain = RTC_MemoryDomains._domain.ToString();
								}

                                MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
                                RandomAddress = RandomLong(MaxAddress -1);

                                bu = getBlastUnit(Domain, RandomAddress);
                                if (bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

                        case BlastRadius.CHUNK: //Randomly spreads the corruption bytes in one randomly selected domain

							if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
							{
								Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];
							}
							else
							{
								Domain = RTC_MemoryDomains._domain.ToString();
							}

							MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;

                            for (int i = 0; i < _Intensity; i++)
                            {
                                RandomAddress = RandomLong(MaxAddress -1);

                                bu = getBlastUnit(Domain, RandomAddress);
                                if(bu != null)
                                    bl.Layer.Add(bu);
                            }

                            break;

                        case BlastRadius.BURST:

                            for (int j = 0; j < 10; j++) // 10 shots of 10% chunk
                            {
								if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
								{
									Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];
								}
								else
								{
									Domain = RTC_MemoryDomains._domain.ToString();
								}

								MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;

                                for (int i = 0; i < (int)((double)_Intensity / 10); i++)
                                {
                                    RandomAddress = RandomLong(MaxAddress -1);

                                    bu = getBlastUnit(Domain, RandomAddress);
                                    if (bu != null)
                                        bl.Layer.Add(bu);
                                }

                            }

                            break;

                        case BlastRadius.NONE:
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
            string Domain = null;
            long MaxAddress = -1;
            long RandomAddress = -1;

            string[] _selectedDomains = RTC_MemoryDomains.SelectedDomains;

            if (RTC_Core.SelectedEngine != CorruptionEngine.FREEZE)
            {
                Domain = _selectedDomains[RND.Next(_selectedDomains.Length)];
            }
            else
            {
                Domain = RTC_MemoryDomains._domain.ToString();
            }

            MaxAddress = RTC_MemoryDomains.getInterface(Domain).Size;
            RandomAddress = RandomLong(MaxAddress - 1);

            return new BlastTarget(Domain, RandomAddress);

        }

        public static string GetRandomKey()
        {
            string Key = RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString() + RND.Next(1, 9999).ToString();
            return Key;
        }

        public static void LoadDefaultRom()
        {
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
			SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADROM)
			{
				romFilename = RomFile
			}, sync);
		}


		public static void LoadRom_NET(string RomFile)
		{

			RTC_Core.StopSound();

			var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = GlobalWin.MainForm.CurrentlyOpenRom;

			var lra = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs { OpenAdvanced = new OpenAdvanced_OpenRom { Path = RomFile } };

			RTC_Hooks.AllowCaptureRewindState = false;
			GlobalWin.MainForm.LoadRom(RomFile, lra);
			RTC_Hooks.AllowCaptureRewindState = true;

			RTC_Core.StartSound();

			//coreForm.RefreshDomains(); //refresh and reload domains


		}

        public static string SaveSavestate(string Key, bool threadSave = false)
        {
            if (Global.Emulator is NullEmulator)
                return null;

			string quickSlotName = Key + ".timejump";


			//string prefix = (string)SendCommandRTC(new RTC_Command(CommandType.REMOTE_DOMAINS_SYSTEMPREFIX), true);
			string prefix = PathManager.SaveStatePrefix(Global.Game);

			var path = prefix + "." + quickSlotName + ".State";

            var file = new FileInfo(path);
            if (file.Directory != null && file.Directory.Exists == false)
                file.Directory.Create();


            //Filtering out parts 
            path = path.Replace(".Performance.", ".");
            path = path.Replace(".Compatibility.", ".");
            path = path.Replace(".QuickNes.", ".");
            path = path.Replace(".NesHawk.", ".");
            path = path.Replace(".VBA-Next.", ".");
			path = path.Replace(".mGBA.", ".");

			if(threadSave)
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

            if (File.Exists(path) == false)
            {
                GlobalWin.OSD.AddMessage("Unable to load " + quickSlotName + ".State");
                return;
            }

            GlobalWin.MainForm.LoadState(path, quickSlotName, fromLua);
        }

        public static void SetEngineByName(string name)
        {
            for (int i = 0; i < coreForm.cbSelectedEngine.Items.Count; i++)
                if (coreForm.cbSelectedEngine.Items[i].ToString() == name)
                {
                    coreForm.cbSelectedEngine.SelectedIndex = i;
                    break;
                }

        }

		/// <summary>
		/// Creates color with corrected brightness.
		/// </summary>
		/// <param name="color">Color to correct.</param>
		/// <param name="correctionFactor">The brightness correction factor. Must be between -1 and 1. 
		/// Negative values produce darker colors.</param>
		/// <returns>
		/// Corrected <see cref="Color"/> structure.
		/// </returns>
		public static Color ChangeColorBrightness(Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}

		private static List<Control> FindTag(Control.ControlCollection controls)
		{
			List<Control> allControls = new List<Control>();

			foreach (Control c in controls)
			{
				if (c.Tag != null)
					allControls.Add(c);

					if (c.HasChildren)
						allControls.AddRange(FindTag(c.Controls)); //Recursively check all children controls as well; ie groupboxes or tabpages
			}

			return allControls;
		}

		public static void SetRTCColor(Color color, Form form = null)
		{
			List<Control> allControls = new List<Control>();

			if (form == null)
			{
				if (coreForm != null)
				{
					allControls.AddRange(FindTag(coreForm.Controls));
					allControls.Add(coreForm);
				}
				if (ghForm != null)
				{
					allControls.AddRange(FindTag(ghForm.Controls));
					allControls.Add(ghForm);
				}
				if (spForm != null)
				{
					allControls.AddRange(FindTag(spForm.Controls));
					allControls.Add(spForm);
				}
				if (multiForm != null)
				{
					allControls.AddRange(FindTag(multiForm.Controls));
					allControls.Add(multiForm);
				}
				if (multipeerpopoutForm != null)
				{
					allControls.AddRange(FindTag(multipeerpopoutForm.Controls));
					allControls.Add(multipeerpopoutForm);
				}
				if (sbForm != null)
				{
					allControls.AddRange(FindTag(sbForm.Controls));
					allControls.Add(sbForm);
				}
				if (beForm != null)
				{
					allControls.AddRange(FindTag(beForm.Controls));
					allControls.Add(beForm);
				}

			}
			else
				allControls.AddRange(FindTag(form.Controls));

			var lightColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:light"));
			var normalColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:normal"));
			var darkColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:dark"));
			var darkerColorControls = allControls.FindAll(it => (it.Tag as string).Contains("color:darker"));

			foreach (Control c in lightColorControls)
				c.BackColor = ChangeColorBrightness(color, 0.30f);

			foreach (Control c in normalColorControls)
				c.BackColor = color;

			spForm.dgvStockpile.BackgroundColor = color;
			ghForm.dgvStockpile.BackgroundColor = color;

			foreach (Control c in darkColorControls)
				c.BackColor = ChangeColorBrightness(color, -0.30f);

			foreach (Control c in darkerColorControls)
				c.BackColor = ChangeColorBrightness(color, -0.75f);

		}

		public static void SetAndSaveColorRTC()
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

			if (File.Exists(rtcDir + "\\params\\COLOR.TXT"))
				File.Delete(rtcDir + "\\params\\COLOR.TXT");

			File.WriteAllText(rtcDir + "\\params\\COLOR.TXT", color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString());
		}
	}


	static class RandomExtensions
	{
		public static long RandomLong(this Random rnd)
		{
			byte[] buffer = new byte[8];
			rnd.NextBytes(buffer);
			return BitConverter.ToInt64(buffer, 0);
		}

		public static long RandomLong(this Random rnd, long min, long max)
		{
			EnsureMinLEQMax(ref min, ref max);
			long numbersInRange = unchecked(max - min + 1);
			if (numbersInRange < 0)
				throw new ArgumentException("Size of range between min and max must be less than or equal to Int64.MaxValue");

			long randomOffset = RandomLong(rnd);
			if (IsModuloBiased(randomOffset, numbersInRange))
				return RandomLong(rnd, min, max); // Try again
			else
				return min + PositiveModuloOrZero(randomOffset, numbersInRange);
		}

		static bool IsModuloBiased(long randomOffset, long numbersInRange)
		{
			long greatestCompleteRange = numbersInRange * (long.MaxValue / numbersInRange);
			return randomOffset > greatestCompleteRange;
		}

		static long PositiveModuloOrZero(long dividend, long divisor)
		{
			long mod;
			Math.DivRem(dividend, divisor, out mod);
			if (mod < 0)
				mod += divisor;
			return mod;
		}

		static void EnsureMinLEQMax(ref long min, ref long max)
		{
			if (min <= max)
				return;
			long temp = min;
			min = max;
			max = temp;
		}
	}

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Media;
using System.Threading;

namespace RTC
{
    public partial class RTC_Form : Form // replace by : UserControl for panel
    {
        SoundPlayer simpleSound = new SoundPlayer(RTC_Core.rtcDir + "\\ASSETS\\quack.wav"); //QUACK

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
				if (RTC_Core.isStandalone)
					return base.CreateParams;

				CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

		public bool AutoCorrupt
		{
			get
			{
				return RTC_Core.AutoCorrupt;
			}
			set
			{
				if (value)
					btnAutoCorrupt.Text = "Stop Auto-Corrupt";
				else
					btnAutoCorrupt.Text = "Start Auto-Corrupt";

				RTC_Core.AutoCorrupt = value;

				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_AUTOCORRUPT) {
					objectValue = value
				});
			}
		}

		public bool DontUpdateIntensity = false;
		public int Intensity
		{
			get {
				return RTC_Core.Intensity;
			}
			set {
				if (DontUpdateIntensity)
					return;

				RTC_Core.Intensity = value;
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_INTENSITY) { objectValue = RTC_Core.Intensity });

				DontUpdateIntensity = true;

				if (nmIntensity.Value != RTC_Core.Intensity)
					nmIntensity.Value = RTC_Core.Intensity;

				if (RTC_Core.ghForm.nmIntensity.Value != RTC_Core.Intensity)
					RTC_Core.ghForm.nmIntensity.Value = RTC_Core.Intensity;


				int _fx = Convert.ToInt32(Math.Sqrt(value) * 2000d);

				if (track_Intensity.Value != _fx)
					track_Intensity.Value = _fx;

				if (RTC_Core.ghForm.track_Intensity.Value != _fx)
					RTC_Core.ghForm.track_Intensity.Value = _fx;

				DontUpdateIntensity = false;
			}
		}

		public bool DontUpdateErrorDelay = false;
		public int ErrorDelay
		{
			get
			{
				return RTC_Core.ErrorDelay;
			}
			set
			{
				if (DontUpdateErrorDelay)
					return;

				RTC_Core.ErrorDelay = Convert.ToInt32(value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_ERRORDELAY) { objectValue = RTC_Core.ErrorDelay });

				DontUpdateErrorDelay = true;

				if (nmErrorDelay.Value != RTC_Core.ErrorDelay)
					nmErrorDelay.Value = RTC_Core.ErrorDelay;

				int _fx = Convert.ToInt32(Math.Sqrt(value) * 2000d);

				if (track_ErrorDelay.Value != _fx)
					track_ErrorDelay.Value = _fx;

				DontUpdateErrorDelay = false;

			}
		}

		public RTC_Form()
        {
            InitializeComponent();

			if (RTC_Core.isStandalone)
				pnCrashProtection.Visible = true;
		}

		public void setMemoryZonesSelectedDomains(string[] _domains)
		{
			lbMemoryZones_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryZones.Items.Count; i++)
				if (_domains.Contains(lbMemoryZones.Items[i].ToString()))
					lbMemoryZones.SetSelected(i, true);
				else
					lbMemoryZones.SetSelected(i, false);

			lbMemoryZones_DontExecute_SelectedIndexChanged = false;
			lbMemoryZones_SelectedIndexChanged(null, null);
		}

		public void setMemoryZonesAllButSelectedDomains(string[] _blacklistedDomains)
		{
			lbMemoryZones_DontExecute_SelectedIndexChanged = true;

			for (
				int i = 0; i < lbMemoryZones.Items.Count; i++)
				if (_blacklistedDomains.Contains(lbMemoryZones.Items[i].ToString()))
					lbMemoryZones.SetSelected(i, false);
				else
					lbMemoryZones.SetSelected(i, true);

			lbMemoryZones_DontExecute_SelectedIndexChanged = false;
			lbMemoryZones_SelectedIndexChanged(null, null);
		}

		private void btnRefreshZones_Click(object sender, EventArgs e)
        {
            RefreshDomains();

            //RTC_Restore.SaveRestore();
        }

        public void btnManualBlast_Click(object sender, EventArgs e)
        {
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.ASYNCBLAST));
		}

        private void btnClearCheats_Click(object sender, EventArgs e)
        {
            RTC_HellgenieEngine.ClearCheats();

            //RTC_Restore.SaveRestore();
        }

        public void btnAutoCorrupt_Click(object sender, EventArgs e)
        {
            if (btnAutoCorrupt.ForeColor == Color.Silver)
                return;

            if (!this.AutoCorrupt)
				this.AutoCorrupt = true;
            else
				this.AutoCorrupt = false;

        }

        private void RTC_Form_Load(object sender, EventArgs e)
        {
			btnLogo.Text = "    Version " + RTC_Core.RtcVersion;

			//As of 0.71+ HexEditor must be loaded to hook memory domains
			//thx bizhawk devs for increasing complexity
			//GlobalWin.Tools.Load<HexEditor>();

			Controls.Remove(gbNightmareEngine);
			pnCorruptionEngine.Controls.Add(gbNightmareEngine);
			gbNightmareEngine.Location = new Point(gbSelectedEngine.Location.X,gbSelectedEngine.Location.Y);

			Controls.Remove(gbHellgenieEngine);
			pnCorruptionEngine.Controls.Add(gbHellgenieEngine);
			gbHellgenieEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbDistortionEngine);
			pnCorruptionEngine.Controls.Add(gbDistortionEngine);
			gbDistortionEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbFreezeEngine);
			pnCorruptionEngine.Controls.Add(gbFreezeEngine);
			gbFreezeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbExternalRomPlugin);
			pnCorruptionEngine.Controls.Add(gbExternalRomPlugin);
			gbExternalRomPlugin.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbPipeEngine);
			pnCorruptionEngine.Controls.Add(gbPipeEngine);
			gbPipeEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);

			Controls.Remove(gbVectorEngine);
			pnCorruptionEngine.Controls.Add(gbVectorEngine);
			gbVectorEngine.Location = new Point(gbSelectedEngine.Location.X, gbSelectedEngine.Location.Y);


			foreach (string item in Directory.GetDirectories(RTC_Core.rtcDir + "\\PLUGINS"))
                cbExternalSelectedPlugin.Items.Add(item.Substring(item.LastIndexOf("\\") + 1));

			cbSelectedEngine.SelectedIndex = 0;
			cbVectorLimiterList.SelectedIndex = 0;
			cbVectorValueList.SelectedIndex = 0;
			cbBlastRadius.SelectedIndex = 0;
			cbBlastType.SelectedIndex = 0;
			cbMemoryDomainTool.SelectedIndex = 0;
			cbExternalSelectedPlugin.SelectedIndex = 0;


			if (File.Exists(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.TXT"))
			{
				MessageBox.Show(File.ReadAllText(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.TXT").Replace("[ver]",RTC_Core.RtcVersion), "RTC", MessageBoxButtons.OK, MessageBoxIcon.Information);

				if (File.Exists(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.OLD"))
					File.Delete(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.OLD");

				File.Move(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.TXT", RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER.OLD");
			}

		}

        public void track_ErrorDelay_Scroll(object sender, EventArgs e)
        {
			double fx = Math.Ceiling(Math.Pow((track_ErrorDelay.Value * 0.0005d), 2));
			int _fx = Convert.ToInt32(fx);

			if (_fx != ErrorDelay)
				ErrorDelay = _fx;

		}

        public void nmErrorDelay_ValueChanged(object sender, EventArgs e)
        {


			int _fx = Convert.ToInt32(nmErrorDelay.Value);

			if (_fx != ErrorDelay)
				ErrorDelay = _fx;

		}

        public void track_Intensity_Scroll(object sender, EventArgs e)
        {
			double fx = Math.Floor(Math.Pow((track_Intensity.Value * 0.0005d), 2));
			int _fx = Convert.ToInt32(fx);

			if (_fx != Intensity)
				Intensity = _fx;

		}

        public void nmIntensity_ValueChanged(object sender, EventArgs e)
        {
			int _fx = Convert.ToInt32(nmIntensity.Value);

			if (Intensity != _fx)
				Intensity = _fx;

        }

        private void cbBlastRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastRadius.SelectedItem.ToString())
            {
                case "SPREAD":
                    RTC_Core.Radius = BlastRadius.SPREAD;
                    break;

                case "CHUNK":
					RTC_Core.Radius = BlastRadius.CHUNK;
                    break;

                case "BURST":
					RTC_Core.Radius = BlastRadius.BURST;
                    break;
            }

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_BLASTRADIUS) { objectValue = RTC_Core.Radius });

		}

        private void cbBlastType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastType.SelectedItem.ToString())
            {
                case "RANDOM":
					
                    RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOM;
                    break;

                case "RANDOMTILT":
					RTC_NightmareEngine.Algo = BlastByteAlgo.RANDOMTILT;
                    break;

                case "TILT":
					RTC_NightmareEngine.Algo = BlastByteAlgo.TILT;
                    break;
            }

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_NIGHTMARE_TYPE) { objectValue = RTC_NightmareEngine.Algo });

		}

        private void nmMaxCheats_ValueChanged(object sender, EventArgs e)
        {
			if (Convert.ToInt32(nmMaxCheats.Value) != RTC_HellgenieEngine.MaxCheats)
			{
				RTC_HellgenieEngine.MaxCheats = Convert.ToInt32(nmMaxCheats.Value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS) { objectValue = RTC_HellgenieEngine.MaxCheats });
			}

			if (nmMaxCheats.Value != nmMaxFreezes.Value)
				nmMaxFreezes.Value = nmMaxCheats.Value;
		}

		public bool lbMemoryZones_DontExecute_SelectedIndexChanged = false;
        private void lbMemoryZones_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (lbMemoryZones_DontExecute_SelectedIndexChanged)
				return;

			string[] selectedDomains = lbMemoryZones.SelectedItems.Cast<string>().ToArray();

			RTC_MemoryDomains.UpdateSelectedDomains(selectedDomains, true);

            //RTC_Restore.SaveRestore();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            RefreshDomains();

			lbMemoryZones_DontExecute_SelectedIndexChanged = true;

			for (int i = 0; i < lbMemoryZones.Items.Count; i++)
                lbMemoryZones.SetSelected(i, true);

			lbMemoryZones_DontExecute_SelectedIndexChanged = false;

			lbMemoryZones_SelectedIndexChanged(null, null);

		}

        private void btnAutoSelectZones_Click(object sender, EventArgs e)
        {
			RefreshDomains();
			setMemoryZonesAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());

        }

        private void btnGlitchHarvester_Click(object sender, EventArgs e)
        {
            RTC_Core.ghForm.Show();
			RTC_Core.ghForm.Focus();
		}

        private void cbClearCheatsOnRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
                cbClearFreezesOnRewind.Checked = cbClearCheatsOnRewind.Checked;

            RTC_Core.ClearCheatsOnRewind = cbClearCheatsOnRewind.Checked;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND) { objectValue = RTC_Core.ClearCheatsOnRewind });

		}

        private void RTC_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!RTC_Core.isStandalone && e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.Hide();
				return;
            }
			else if(RTC_Core.isStandalone)
			{
				if (RTC_Core.RemoteRTC_SupposedToBeConnected)
				{
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_CLOSEBIZHAWK));
					Thread.Sleep(1000);
				}

				RTC_Core.CloseAllRtcForms();
			}
        }

        private void nmDistortionDelay_ValueChanged(object sender, EventArgs e)
        {
            RTC_DistortionEngine.MaxAge = Convert.ToInt32(nmDistortionDelay.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_DISTORTION_DELAY) { objectValue = RTC_DistortionEngine.MaxAge });
		}

        private void btnResyncDistortionEngine_Click(object sender, EventArgs e)
        {
			RTC_DistortionEngine.Resync();

        }

        public void btnReboot_Click(object sender, EventArgs e)
        {
            if (File.Exists(RTC_Core.rtcDir + "\\SESSION\\Restore.dat"))
                File.Delete(RTC_Core.rtcDir + "\\SESSION\\Restore.dat");

            Process.Start("KillSwitchRestart.bat");

        }

        public void btnFactoryClean_Click(object sender, EventArgs e)
        {
            Process.Start($"FactoryClean{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
        }

        public void btnAutoKillSwitch_Click(object sender, EventArgs e)
        {
            Process.Start("AutoKillSwitch.exe");
        }

        private void RTC_Form_ResizeEnd(object sender, EventArgs e)
        {
            //RTC_Restore.SaveRestore();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnExternalOpenWindow_Click(object sender, EventArgs e)
        {
            RTC_ExternalRomPlugin.OpenWindow();
        }

        private void nmIntensity_ValueChanged(object sender, KeyPressEventArgs e)
        {

        }

        private void nmIntensity_ValueChanged(object sender, KeyEventArgs e)
        {

        }

        public void btnEasyModeCurrent_Click(object sender, EventArgs e)
        {
            StartEasyMode(false);
        }

        public void btnEasyModeTemplate_Click(object sender, EventArgs e)
        {
            StartEasyMode(true);
        }

		public void RefreshDomains()
		{
			RTC_MemoryDomains.RefreshDomains();

			lbMemoryZones.Items.Clear();
			lbMemoryZones.Items.AddRange(RTC_MemoryDomains.MemoryDomainProxies.Keys.ToArray());
		}

		public void RefreshDomainsAndKeepSelected(string[] overrideDomains = null)
		{
			string[] copy = RTC_MemoryDomains.lastSelectedDomains;

            if (overrideDomains != null)
                copy = overrideDomains;

            RefreshDomains(); //refresh and reload zones

			RTC_MemoryDomains.UpdateSelectedDomains(copy);

			RTC_Core.coreForm.setMemoryZonesSelectedDomains(copy);


		}

		public void StartEasyMode(bool useTemplate)
        {
			if (RTC_Core.isStandalone && !RTC_Core.coreForm.cbUseGameProtection.Checked)
				RTC_Core.coreForm.cbUseGameProtection.Checked = true;


			if (useTemplate)
            {


                //Put Console templates HERE
                string thisSystem = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SYSTEM), true);

				switch (thisSystem)
                {

                    case "NES":     //Nintendo Entertainment system
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.coreForm.Intensity = 2;
                        RTC_Core.coreForm.ErrorDelay = 1;
                        break;


                    case "GB":      //Gameboy
                    case "GBC":     //Gameboy Color
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.coreForm.Intensity = 1;
                        RTC_Core.coreForm.ErrorDelay = 4;
                        break;

                    case "SNES":    //Super Nintendo
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.coreForm.Intensity = 4;
                        RTC_Core.coreForm.ErrorDelay = 8;
                        break;


                    case "GBA":     //Gameboy Advance
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.coreForm.Intensity = 1;
                        RTC_Core.coreForm.ErrorDelay = 1;
                        break;

                    case "N64":     //Nintendo 64
                        RTC_Core.SetEngineByName("Vector Engine");
                        RTC_Core.coreForm.Intensity = 100;
                        RTC_Core.coreForm.ErrorDelay = 1;
                        break;

                    case "SG":      //Sega SG-1000
                    case "GG":      //Sega GameGear
                    case "SMS":     //Sega Master System
                    case "GEN":     //Sega Genesis and CD
                    case "PCE":     //PC-Engine / Turbo Grafx
                    case "PSX":     //Sony Playstation 1
                    case "A26":     //Atari 2600
                    case "A78":     //Atari 7800
                    case "LYNX":    //Atari Lynx
                    case "INTV":    //Intellivision
                    case "PCECD":   //related to PC-Engine / Turbo Grafx
                    case "SGX":     //related to PC-Engine / Turbo Grafx
                    case "TI83":    //Ti-83 Calculator
                    case "WSWAN":   //Wonderswan
                    case "C64":     //Commodore 64
                    case "Coleco":  //Colecovision
                    case "SGB":     //Super Gameboy
                    case "SAT":     //Sega Saturn
                    case "DGB": 
                        MessageBox.Show("WARNING: No Easy-Mode template was made for this system. Please configure it manually and use the current settings.");
                        break;

                    //TODO: Add more zones like gamegear, atari, turbo graphx
                }




            }

            this.AutoCorrupt = true;

        }

        private void cbExternalSelectedPlugin_SelectedIndexChanged(object sender, EventArgs e)
        {
            RTC_ExternalRomPlugin.SelectedPlugin = (sender as ComboBox).SelectedItem.ToString();

        }

        private void cbUseGameProtection_CheckedChanged(object sender, EventArgs e)
        {

            if (cbUseGameProtection.Checked)
            {
                RTC_GameProtection.Start();
				RTC_Core.csForm.pnDisableGameProtection.Visible = true;

			}
            else
            {
                RTC_GameProtection.Stop();
				RTC_StockpileManager.backupedState = null;
				RTC_StockpileManager.allBackupStates.Clear();
				btnGpJumpBack.Visible = false;
				btnGpJumpNow.Visible = false;
				RTC_Core.csForm.pnDisableGameProtection.Visible = false;
			}

        }

        private void nmTimeStackDelay_ValueChanged(object sender, EventArgs e)
        {
			UpdateGameProtectionDelay();

		}

		public void UpdateGameProtectionDelay()
		{
			RTC_GameProtection.BackupInterval = Convert.ToInt32(nmGameProtectionDelay.Value);
			if (RTC_GameProtection.isRunning)
				RTC_GameProtection.Reset();

		}

		private void cbSelectedEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbNightmareEngine.Visible = false;
            gbHellgenieEngine.Visible = false;
            gbDistortionEngine.Visible = false;
            gbFreezeEngine.Visible = false;
			gbPipeEngine.Visible = false;
			gbVectorEngine.Visible = false;
			gbExternalRomPlugin.Visible = false;

			btnAutoCorrupt.Visible = true;
			RTC_Core.ghForm.pnIntensity.Visible = true;

            switch (cbSelectedEngine.SelectedItem.ToString())
            {
                case "Nightmare Engine":
                    RTC_Core.SelectedEngine = CorruptionEngine.NIGHTMARE;
                    gbNightmareEngine.Visible = true;
                    break;

                case "Hellgenie Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.HELLGENIE;
                    gbHellgenieEngine.Visible = true;
                    break;

                case "Distortion Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.DISTORTION;
                    gbDistortionEngine.Visible = true;
                    break;

                case "Freeze Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.FREEZE;
                    gbFreezeEngine.Visible = true;
                    break;

				case "Pipe Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.PIPE;
					gbPipeEngine.Visible = true;
					break;

				case "Vector Engine":
					RTC_Core.SelectedEngine = CorruptionEngine.VECTOR;
					gbVectorEngine.Visible = true;
					break;

				case "External ROM Plugin":
					
					RTC_Core.SelectedEngine = CorruptionEngine.EXTERNALROM;
                    gbExternalRomPlugin.Visible = true;

                    this.AutoCorrupt = false;
					btnAutoCorrupt.Visible = false;

                    RTC_Core.ghForm.pnIntensity.Visible = false;
                    break;

                default:
                    break;
            }

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_ENGINE) { objectValue = RTC_Core.SelectedEngine });

			if (cbSelectedEngine.SelectedItem.ToString() == "External ROM Plugin")
            {
                labelBlastRadius.Visible = false;
                labelIntensity.Visible = false;
                labelIntensityTimes.Visible = false;
                labelErrorDelay.Visible = false;
                labelErrorDelaySteps.Visible = false;
                nmErrorDelay.Visible = false;
                nmIntensity.Visible = false;
                track_ErrorDelay.Visible = false;
                track_Intensity.Visible = false;
                cbBlastRadius.Visible = false;
            }
            else if (cbSelectedEngine.SelectedItem.ToString() == "Freeze Engine")
            {
                labelBlastRadius.Visible = true;
                labelIntensity.Visible = true;
                labelIntensityTimes.Visible = true;
                labelErrorDelay.Visible = true;
                labelErrorDelaySteps.Visible = true;
                nmErrorDelay.Visible = true;
                nmIntensity.Visible = true;
                track_ErrorDelay.Visible = true;
                track_Intensity.Visible = true;
                cbBlastRadius.Visible = true;
            }
            else
            {
                labelBlastRadius.Visible = true;
                labelIntensity.Visible = true;
                labelIntensityTimes.Visible = true;
                labelErrorDelay.Visible = true;
                labelErrorDelaySteps.Visible = true;
                nmErrorDelay.Visible = true;
                nmIntensity.Visible = true;
                track_ErrorDelay.Visible = true;
                track_Intensity.Visible = true;
                cbBlastRadius.Visible = true;
            }

            RTC_HellgenieEngine.ClearCheats();
			RTC_PipeEngine.ClearPipes();

        }

        private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e) => UpdateGameProtectionDelay();
        private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e) => UpdateGameProtectionDelay();

        private void cbClearFreezesOnRewind_CheckedChanged(object sender, EventArgs e)
        {
            if (cbClearFreezesOnRewind.Checked != cbClearCheatsOnRewind.Checked)
                cbClearCheatsOnRewind.Checked = cbClearFreezesOnRewind.Checked;


            RTC_Core.ClearCheatsOnRewind = cbClearFreezesOnRewind.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND) { objectValue = RTC_Core.ClearCheatsOnRewind });

		}

        private void nmMaxFreezes_ValueChanged(object sender, EventArgs e)
        {

			if (Convert.ToInt32(nmMaxFreezes.Value) != RTC_HellgenieEngine.MaxCheats)
			{
				RTC_HellgenieEngine.MaxCheats = Convert.ToInt32(nmMaxFreezes.Value);
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS) { objectValue = RTC_HellgenieEngine.MaxCheats });
			}

			if (nmMaxCheats.Value != nmMaxFreezes.Value)
				nmMaxCheats.Value = nmMaxFreezes.Value;

		}

        private void btnLogo_MouseClick(object sender, MouseEventArgs e)
        {
			if (RTC_Core.isStandalone)
			{
				RTC_Core.coreForm.pnEngineConfig.Hide();
				RTC_Core.coreForm.pnLeftPanel.Hide();
				RTC_Core.csForm.Show();
			}
			else
			{
				simpleSound.Play();
			}
		}

        private void btnActiveTableDumpsReset_Click(object sender, EventArgs e)
        {
            RTC_FreezeEngine.ResetActiveTable();
        }

        private void btnActiveTableAddDump_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_FreezeEngine.AddDump();
        }

        private void btnActiveTableGenerate_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_FreezeEngine.GenerateActiveTable();
        }

        private void cbAutoAddDump_CheckedChanged(object sender, EventArgs e)
        {
            if (RTC_FreezeEngine.ActiveTableAutodump != null)
            {
                RTC_FreezeEngine.ActiveTableAutodump.Stop();
                RTC_FreezeEngine.ActiveTableAutodump = null;
            }

            if (cbAutoAddDump.Checked)
            {
                RTC_FreezeEngine.ActiveTableAutodump = new System.Windows.Forms.Timer();
                RTC_FreezeEngine.ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
                RTC_FreezeEngine.ActiveTableAutodump.Tick += new EventHandler(btnActiveTableAddDump_Click);
                RTC_FreezeEngine.ActiveTableAutodump.Start();
            }

        }

        private void nmAutoAddSec_ValueChanged(object sender, EventArgs e)
        {
            if(RTC_FreezeEngine.ActiveTableAutodump != null)
                RTC_FreezeEngine.ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
        }

        private void track_ActiveTableActivityThreshold_Scroll(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
                return;

            nmActiveTableActivityThreshold.Value = Convert.ToDecimal((double)track_ActiveTableActivityThreshold.Value / 100);
            RTC_FreezeEngine.ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);
        }

        private void nmActiveTableActivityThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
                return;

            track_ActiveTableActivityThreshold.Value = Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100);
            RTC_FreezeEngine.ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);
        }

        private void btnActiveTableSubstractFile_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.FirstInit)
                return;

            RTC_FreezeEngine.SubstractActiveTable();
        }

        private void btnActiveTableSaveAs_Click(object sender, EventArgs e)
        {
            if (!RTC_FreezeEngine.ActiveTableReady)
                return;

            RTC_FreezeEngine.SaveActiveTable(false);
        }

        private void btnActiveTableLoad_Click(object sender, EventArgs e)
        {
            RTC_FreezeEngine.LoadActiveTable();
        }

        private void btnActiveTableQuickSave_Click(object sender, EventArgs e)
        {
            if (btnActiveTableQuickSave.ForeColor != Color.Silver)
            {
                RTC_FreezeEngine.SaveActiveTable(true);
            }
        }

        private void btnReboot_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Button).Location.X + e.Location.X, (sender as Button).Location.Y + e.Location.Y);

			ContextMenuStrip ParamsButtonMenu = new ContextMenuStrip();
			ParamsButtonMenu.Items.Add("Change RTC skin color", null, new EventHandler((ev, ob)=> { RTC_Core.SetAndSaveColorRTC(); }));
			ParamsButtonMenu.Items.Add("Reset RTC Parameters", null, null).Enabled = false;
			ParamsButtonMenu.Items.Add("Reset RTC Parameters + Window Parameters", null, null).Enabled = false;
			ParamsButtonMenu.Items.Add(new ToolStripSeparator());
			ParamsButtonMenu.Items.Add("Start AutoKillSwitch", null, new EventHandler(btnAutoKillSwitch_Click));
			ParamsButtonMenu.Items.Add("RTC Factory Clean", null, new EventHandler(btnFactoryClean_Click));
			ParamsButtonMenu.Show(this, locate);
        }

        private void btnEasyMode_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Button).Location.X + e.Location.X, (sender as Button).Location.Y + e.Location.Y);

			ContextMenuStrip EasyButtonMenu = new ContextMenuStrip();
			EasyButtonMenu.Items.Add("Start with Recommended Settings", null, new EventHandler(btnEasyModeTemplate_Click));
			EasyButtonMenu.Items.Add(new ToolStripSeparator());
			EasyButtonMenu.Items.Add("Watch a tutorial video", null, new EventHandler((ob,ev) => Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw"))).Enabled = false;
			EasyButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("http://cc.r5x.cc/dokuwiki/")));
			EasyButtonMenu.Show(this, locate);
        }

		private void btnStockPilePlayer_Click(object sender, EventArgs e)
		{
			if (this.Controls.Contains(RTC_Core.multiForm))
				this.Controls.Remove(RTC_Core.multiForm);

			if (!this.Controls.Contains(RTC_Core.spForm))
			{
				RTC_Core.spForm.TopLevel = false;
				RTC_Core.spForm.Location = new Point(150, 0);
				this.Controls.Add(RTC_Core.spForm);
				
			}


			pnEngineConfig.Visible = false;
			RTC_Core.spForm.Show();


		}

		private void btnRTCMultiplayer_Click(object sender, EventArgs e)
		{
			if(RTC_Core.isStandalone)
			{
				MessageBox.Show("Multiplayer unsupported in Detached mode");
				return;
			}

			if (this.Controls.Contains(RTC_Core.spForm))
				this.Controls.Remove(RTC_Core.spForm);

			if (!this.Controls.Contains(RTC_Core.multiForm))
			{
				RTC_Core.multiForm.TopLevel = false;
				RTC_Core.multiForm.Location = new Point(150, 0);
				this.Controls.Add(RTC_Core.multiForm);
			}

			pnEngineConfig.Visible = false;

			RTC_Core.multiForm.Show();
		}

		private void btnEngineConfig_Click(object sender, EventArgs e)
		{
			if(this.Controls.Contains(RTC_Core.multiForm))
				this.Controls.Remove(RTC_Core.multiForm);

			if (this.Controls.Contains(RTC_Core.spForm))
				this.Controls.Remove(RTC_Core.spForm);

			pnEngineConfig.Visible = true;
		}

		private void nmMaxPipes_ValueChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.MaxPipes = Convert.ToInt32(nmMaxPipes.Value);
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_MAXPIPES) { objectValue = RTC_PipeEngine.MaxPipes });

		}

		private void btnClearPipes_Click(object sender, EventArgs e)
		{
			RTC_PipeEngine.ClearPipes();
		}

		private void cbLockPipes_CheckedChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.LockPipes = cbLockPipes.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_LOCKPIPES) { objectValue = RTC_PipeEngine.LockPipes });
		}

		private void cbProcessOnStep_CheckedChanged(object sender, EventArgs e)
		{
			RTC_PipeEngine.ProcessOnStep = cbProcessOnStep.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_PROCESSONSTEP) { objectValue = RTC_PipeEngine.ProcessOnStep });
		}

		private void cbClearPipesOnRewind_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Core.ClearPipesOnRewind = cbClearPipesOnRewind.Checked;
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_CLEARPIPESREWIND) { objectValue = RTC_Core.ClearPipesOnRewind });
		}

		private void cbVectorLimiterList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = (sender as ComboBox).SelectedItem.ToString();

			switch (selectedText)
			{
				case "Extended":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.extendedListOfConstants;
					break;
				case "Extended+":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfPositiveConstants;
					break;
				case "Extended-":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfNegativeConstants;
					break;
				case "Whole":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfWholeConstants;
					break;
				case "Whole+":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfWholePositiveConstants;
					break;
				case "Tiny":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.listOfTinyConstants;
					break;
				case "One":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantPositiveOne;
					break;
				case "One*":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantOne;
					break;
				case "Two":
					RTC_VectorEngine.limiterList = RTC_VectorEngine.constantPositiveTwo;
					break;

					
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_LIMITER) { objectValue = RTC_VectorEngine.limiterList });

		}

		private void cbVectorValueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedText = (sender as ComboBox).SelectedItem.ToString();

			switch (selectedText)
			{
				case "Extended":
					RTC_VectorEngine.valueList = RTC_VectorEngine.extendedListOfConstants;
					break;
				case "Extended+":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfPositiveConstants;
					break;
				case "Extended-":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfNegativeConstants;
					break;
				case "Whole":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfWholeConstants;
					break;
				case "Whole+":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfWholePositiveConstants;
					break;
				case "Tiny":
					RTC_VectorEngine.valueList = RTC_VectorEngine.listOfTinyConstants;
					break;
				case "One":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantPositiveOne;
					break;
				case "One*":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantOne;
					break;
				case "Two":
					RTC_VectorEngine.valueList = RTC_VectorEngine.constantPositiveTwo;
					break;
				case "AnyFloat":
					RTC_VectorEngine.valueList = null;
					break;
			}

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_VECTOR_VALUES) { objectValue = RTC_VectorEngine.valueList });
		}

		private void btnGpJumpBack_Click(object sender, EventArgs e)
		{
			try
			{
				btnGpJumpBack.Visible = false;

				if (RTC_StockpileManager.allBackupStates.Count == 0)
					return;

				StashKey sk = RTC_StockpileManager.allBackupStates.Pop();

				if (sk != null)
					sk.Run();

				RTC_GameProtection.Reset();
			}

			finally
			{
				if (RTC_StockpileManager.allBackupStates.Count != 0)
					btnGpJumpBack.Visible = true;
			}

		}

		private void btnGpJumpNow_Click(object sender, EventArgs e)
		{
			try
			{
				btnGpJumpNow.Visible = false;

				if (RTC_StockpileManager.backupedState != null)
					RTC_StockpileManager.backupedState.Run();

				RTC_GameProtection.Reset();
			}
			finally
			{
				btnGpJumpNow.Visible = true;
			}
		}

        private void nmTiltPipeValue_ValueChanged(object sender, EventArgs e)
        {
            RTC_PipeEngine.TiltValue = Convert.ToInt32(nmTiltPipeValue.Value);
            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_TILTVALUE) { objectValue = RTC_PipeEngine.TiltValue });
        }

        private void btnManualBlast_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

                ContextMenuStrip columnsMenu = new ContextMenuStrip();
                columnsMenu.Items.Add("Blast + Send RAW To Stash (Glitch Harvester)", null, new EventHandler((ob, ev) => {
                    RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH));
                }));
                columnsMenu.Show(this, locate);
            }
        }

        private void cbGenerateChainedPipes_CheckedChanged(object sender, EventArgs e)
        {
            RTC_PipeEngine.ChainedPipes = cbGenerateChainedPipes.Checked;
            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_CHAINEDPIPES) { objectValue = RTC_PipeEngine.ChainedPipes });
        }
    }

}

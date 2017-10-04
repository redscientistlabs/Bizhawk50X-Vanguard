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
    public partial class RTC_Core_Form : Form // replace by : UserControl for panel
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

                RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_AUTOCORRUPT)
                {
                    objectValue = value
                });
            }
        }

        public RTC_Core_Form()
        {
            InitializeComponent();

			if (RTC_Core.isStandalone)
				pnCrashProtection.Visible = true;
		}

        public void btnManualBlast_Click(object sender, EventArgs e)
        {
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.ASYNCBLAST));
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


			if (!File.Exists(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER_READ"))
			{
				MessageBox.Show(File.ReadAllText(RTC_Core.rtcDir + "\\LICENSES\\DISCLAIMER.TXT").Replace("[ver]",RTC_Core.RtcVersion), "RTC", MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.WriteAllText(RTC_Core.rtcDir + "\\PARAMS\\DISCLAIMER_READ", "");
			}

            RTC_Core.CheckForProblematicProcesses();

            btnEngineConfig_Click(sender, e);
		}

        private void btnGlitchHarvester_Click(object sender, EventArgs e)
        {
            RTC_Core.ghForm.Show();
			RTC_Core.ghForm.Focus();
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
                if(RTC_StockpileManager.unsavedEdits && !RTC_Core.isClosing && MessageBox.Show("You have unsaved edits in the Glitch Harvester Stockpile. \n\n Are you sure you want to close RTC without saving?", "Unsaved edits in Stockpile", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

				if (RTC_Core.RemoteRTC_SupposedToBeConnected)
				{
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_CLOSEBIZHAWK));
					Thread.Sleep(1000);
				}

				RTC_Core.CloseAllRtcForms();
			}
        }

        public void btnFactoryClean_Click(object sender, EventArgs e)
        {
            Process.Start($"FactoryClean{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
        }

        public void btnAutoKillSwitch_Click(object sender, EventArgs e)
        {
            Process.Start("AutoKillSwitch.exe");
        }


        public void btnEasyModeCurrent_Click(object sender, EventArgs e)
        {
            StartEasyMode(false);
        }

        public void btnEasyModeTemplate_Click(object sender, EventArgs e)
        {
            StartEasyMode(true);
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
                        RTC_Core.ecForm.Intensity = 2;
                        RTC_Core.ecForm.ErrorDelay = 1;
                        break;


                    case "GB":      //Gameboy
                    case "GBC":     //Gameboy Color
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.ecForm.Intensity = 1;
                        RTC_Core.ecForm.ErrorDelay = 4;
                        break;

                    case "SNES":    //Super Nintendo
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.ecForm.Intensity = 1;
                        RTC_Core.ecForm.ErrorDelay = 2;
                        break;


                    case "GBA":     //Gameboy Advance
                        RTC_Core.SetEngineByName("Nightmare Engine");
                        RTC_Core.ecForm.Intensity = 1;
                        RTC_Core.ecForm.ErrorDelay = 1;
                        break;

                    case "N64":     //Nintendo 64
                        RTC_Core.SetEngineByName("Vector Engine");
                        RTC_Core.ecForm.Intensity = 75;
                        RTC_Core.ecForm.ErrorDelay = 1;
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
                        return;

                    //TODO: Add more domains for systems like gamegear, atari, turbo graphx
                }




            }

            this.AutoCorrupt = true;

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


        private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e) => UpdateGameProtectionDelay();
        private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e) => UpdateGameProtectionDelay();


        private void btnLogo_MouseClick(object sender, MouseEventArgs e)
        {
			if (RTC_Core.isStandalone)
			{
				RTC_Core.coreForm.pnLeftPanel.Hide();
				RTC_Core.csForm.Show();
			}
			else
			{
				simpleSound.Play();
			}
		}

        /*
        
         //Legacy Active Table code

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

        */

        private void btnReboot_MouseDown(object sender, MouseEventArgs e)
        {
            Point locate = new Point((sender as Button).Location.X + e.Location.X, (sender as Button).Location.Y + e.Location.Y);

			ContextMenuStrip ParamsButtonMenu = new ContextMenuStrip();
            ParamsButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("https://corrupt.wiki/")));
            ParamsButtonMenu.Items.Add(new ToolStripSeparator());
            ParamsButtonMenu.Items.Add("Change RTC skin color", null, new EventHandler((ev, ob)=> { RTC_Core.SelectRTCColor(); }));
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
			//EasyButtonMenu.Items.Add("Watch a tutorial video", null, new EventHandler((ob,ev) => Process.Start("https://www.youtube.com/watch?v=sIELpn4-Umw"))).Enabled = false;
			EasyButtonMenu.Items.Add("Open the online wiki", null, new EventHandler((ob, ev) => Process.Start("https://corrupt.wiki/")));
			EasyButtonMenu.Show(this, locate);
        }

		private void btnStockPilePlayer_Click(object sender, EventArgs e)
		{
            btnEngineConfig.Text = btnEngineConfig.Text.Replace("● ", "");
            btnStockpilePlayer.Text = btnStockpilePlayer.Text.Replace("● ", "");
            btnRTCMultiplayer.Text = btnRTCMultiplayer.Text.Replace("● ", "");

            btnStockpilePlayer.Text = "● " + btnStockpilePlayer.Text;

            if (this.Controls.Contains(RTC_Core.multiForm))
				this.Controls.Remove(RTC_Core.multiForm);

            if (this.Controls.Contains(RTC_Core.ecForm))
                this.Controls.Remove(RTC_Core.ecForm);

            if (!this.Controls.Contains(RTC_Core.spForm))
			{
				RTC_Core.spForm.TopLevel = false;
				RTC_Core.spForm.Location = new Point(150, 0);
				this.Controls.Add(RTC_Core.spForm);
				
			}

			RTC_Core.spForm.Show();

		}

		private void btnRTCMultiplayer_Click(object sender, EventArgs e)
		{
			if(RTC_Core.isStandalone)
			{
				MessageBox.Show("Multiplayer unsupported in Detached mode");
				return;
			}

            btnEngineConfig.Text = btnEngineConfig.Text.Replace("● ", "");
            btnStockpilePlayer.Text = btnStockpilePlayer.Text.Replace("● ", "");
            btnRTCMultiplayer.Text = btnRTCMultiplayer.Text.Replace("● ", "");

            btnRTCMultiplayer.Text = "● " + btnRTCMultiplayer.Text;

            if (this.Controls.Contains(RTC_Core.spForm))
				this.Controls.Remove(RTC_Core.spForm);

            if (this.Controls.Contains(RTC_Core.ecForm))
                this.Controls.Remove(RTC_Core.ecForm);

            if (!this.Controls.Contains(RTC_Core.multiForm))
			{
				RTC_Core.multiForm.TopLevel = false;
				RTC_Core.multiForm.Location = new Point(150, 0);
				this.Controls.Add(RTC_Core.multiForm);
			}

			RTC_Core.multiForm.Show();
		}

		public void btnEngineConfig_Click(object sender, EventArgs e)
		{
            btnEngineConfig.Text = btnEngineConfig.Text.Replace("● ", "");
            btnStockpilePlayer.Text = btnStockpilePlayer.Text.Replace("● ", "");
            btnRTCMultiplayer.Text = btnRTCMultiplayer.Text.Replace("● ", "");

            btnEngineConfig.Text = "● " + btnEngineConfig.Text;

            if (this.Controls.Contains(RTC_Core.multiForm))
				this.Controls.Remove(RTC_Core.multiForm);

			if (this.Controls.Contains(RTC_Core.spForm))
				this.Controls.Remove(RTC_Core.spForm);

            if (!this.Controls.Contains(RTC_Core.ecForm))
            {
                RTC_Core.ecForm.TopLevel = false;
                RTC_Core.ecForm.Location = new Point(150, 0);
                this.Controls.Add(RTC_Core.ecForm);
            }

            RTC_Core.ecForm.Show();
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

    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_ConnectionStatus_Form : Form
	{
		SoundPlayer simpleSound = null;
		SoundPlayer coldSound = null;
		bool isClassicCrash = true;
		bool rotateRandomSounds = false;

		public RTC_ConnectionStatus_Form()
		{
			InitializeComponent();
		}

		private void RTC_ConnectionStatus_Form_Load(object sender, EventArgs e)
		{
			cbCrashSoundEffect.SelectedIndex = 0;
            cbNetCoreCommandTimeout.SelectedIndex = 0;

            coldSound = new SoundPlayer("RTC\\ASSETS\\cold.wav");

            if(File.Exists(RTC_Core.bizhawkDir + "\\WGH\\WindowsGlitchHarvester.exe"))
            {
                lbWindowsGlitchHarvester.Visible = true;
                pnWindowsGlitchHarvester.Visible = true;
            }
		}

		public void btnStartEmuhawkDetached_Click(object sender, EventArgs e)
		{
			lbBizhawkAttached.Visible = false;
			lbBizhawkEmulatorAttached.Visible = false;
			pnBizhawkAttached.Visible = false;

			pnDisableGameProtection.Location = new Point(pnBizhawkAttached.Location.X, pnBizhawkAttached.Location.Y);

			RTC.RTC_RPC.Heartbeat = false;
			RTC.RTC_Core.csForm.pbTimeout.Value = RTC.RTC_Core.csForm.pbTimeout.Maximum;
			RTC.RTC_RPC.Freeze = true;

			if (rotateRandomSounds)
				simpleSound = new SoundPlayer(getRandomSound());

			if (simpleSound != null && btnStartEmuhawkDetached.Text == "Restart BizHawk")
			{
				if (isClassicCrash)
				{
					if (RTC_Core.RND.Next(9999) == 666)
						coldSound.Play();
					else
						simpleSound.Play();
				}
				else
					simpleSound.Play();
			}


			Process.Start("RESTARTDETACHEDRTC.bat");
		}

		private string getRandomSound()
		{
			string[] files = Directory.GetFiles("RTC\\ASSETS\\CRASHSOUNDS\\");

			if (files == null || files.Length == 0)
				return "RTC\\ASSETS\\crash.wav";

			return files[RTC_Core.RND.Next(files.Length)];
		}

		private void cbCrashSoundEffect_SelectedIndexChanged(object sender, EventArgs e)
		{
			rotateRandomSounds = false;
			isClassicCrash = false;

			switch (cbCrashSoundEffect.SelectedIndex)
			{
				case 0:
					isClassicCrash = true;
					simpleSound = new SoundPlayer("RTC\\ASSETS\\crash.wav");
					break;
				case 1:
					simpleSound = new SoundPlayer("RTC\\ASSETS\\quack.wav");
					break;

				case 2:
					simpleSound = null;
				break;
				case 3:
					rotateRandomSounds = true;
				break;
			}

		}

		private void btnReturnToSession_Click(object sender, EventArgs e)
		{
			RTC_Core.coreForm.pnEngineConfig.Show();
			RTC_Core.coreForm.pnLeftPanel.Show();
			RTC_Core.csForm.Hide();

		}

		private void btnStopGameProtection_Click(object sender, EventArgs e)
		{
			RTC_Core.coreForm.cbUseGameProtection.Checked = false;
		}

		private void btnStartEmuhawkAttached_Click(object sender, EventArgs e)
		{
			Process.Start(RTC_Core.bizhawkDir + "\\RESTARTATTACHEDRTC.bat");
		}

		private void btnStartWGH_Click(object sender, EventArgs e)
		{
			ProcessStartInfo psi = new ProcessStartInfo(RTC_Core.bizhawkDir + "\\WGH\\WindowsGlitchHarvester.exe");
			psi.WorkingDirectory = RTC_Core.bizhawkDir + "\\WGH";
			Process.Start(psi);
		}

		private void btnStartVrun_Click(object sender, EventArgs e)
		{
			Process.Start("http://virus.run/");
		}

        private void cbNetCoreCommandTimeout_SelectedIndexChanged(object sender, EventArgs e)
        {
            string setting = cbNetCoreCommandTimeout.SelectedItem.ToString().ToUpper();
            changeNetCoreSettings(setting);
            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.AGGRESSIVENESS) { objectValue = setting });

        }
        public static void changeNetCoreSettings(string setting)
        {
            switch (setting)
            {
                case "STANDARD":
                    RTC_NetCore.DefaultKeepAliveCounter = 5;
                    RTC_NetCore.DefaultNetworkStreamTimeout = 2000;
                    RTC_NetCore.DefaultMaxRetries = 666;

                    if(RTC_Core.isStandalone)
                        RTC_Core.csForm.pbTimeout.Maximum = 13;

                    break;
                case "LAZY":
                    RTC_NetCore.DefaultKeepAliveCounter = 15;
                    RTC_NetCore.DefaultNetworkStreamTimeout = 6000;
                    RTC_NetCore.DefaultMaxRetries = 6666;

                    if (RTC_Core.isStandalone)
                        RTC_Core.csForm.pbTimeout.Maximum = 20;

                    break;

                case "DISABLED":
                    RTC_NetCore.DefaultKeepAliveCounter = int.MaxValue;
                    RTC_NetCore.DefaultNetworkStreamTimeout = int.MaxValue;
                    RTC_NetCore.DefaultMaxRetries = int.MaxValue;

                    if (RTC_Core.isStandalone)
                        RTC_Core.csForm.pbTimeout.Maximum = int.MaxValue;

                    break;
            }
        }
    }
}

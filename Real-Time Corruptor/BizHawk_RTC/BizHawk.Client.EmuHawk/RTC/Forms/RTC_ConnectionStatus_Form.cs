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

		public RTC_ConnectionStatus_Form()
		{
			InitializeComponent();
        }

		private void RTC_ConnectionStatus_Form_Load(object sender, EventArgs e)
		{

			RTC_Core.sForm.cbCrashSoundEffect.SelectedIndex = 0;
            RTC_Core.sForm.cbNetCoreCommandTimeout.SelectedIndex = 0;

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

			RTC.RTC_RPC.Heartbeat = false;
			RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Value = RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum;
			RTC.RTC_RPC.Freeze = true;

            RTC_NetCoreSettings.PlayCrashSound();


			Process.Start("RESTARTDETACHEDRTC.bat");
		}

		private void btnReturnToSession_Click(object sender, EventArgs e)
		{
            RTC_Core.coreForm.showPanelForm(RTC_Core.coreForm.previousForm);

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

    }
}

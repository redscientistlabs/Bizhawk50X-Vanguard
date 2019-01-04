using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_ConnectionStatus_Form : Form, IAutoColorize
	{
		public RTC_ConnectionStatus_Form()
		{
			InitializeComponent();
		}

		private void RTC_ConnectionStatus_Form_Load(object sender, EventArgs e)
		{
			int crashSound = 0;

			if (RTC_Params.IsParamSet("CRASHSOUND"))
				crashSound = Convert.ToInt32(RTC_Params.ReadParam("CRASHSOUND"));

			S.GET<RTC_SettingsNetCore_Form>().cbCrashSoundEffect.SelectedIndex = crashSound;

			S.GET<RTC_SettingsNetCore_Form>().cbNetCoreCommandTimeout.SelectedIndex = 0;

			if (File.Exists(RTC_Core.bizhawkDir + Path.DirectorySeparatorChar + "WGH\\WindowsGlitchHarvester.exe"))
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
			RTC.S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = RTC.S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum;
			RTC.RTC_RPC.Freeze = true;

			RTC_NetCoreSettings.PlayCrashSound();

			Process.Start("RESTARTDETACHEDRTC.bat");
		}

		private void btnReturnToSession_Click(object sender, EventArgs e)
		{
			S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm);
		}

		private void btnStopGameProtection_Click(object sender, EventArgs e)
		{
			S.GET<RTC_Core_Form>().cbUseGameProtection.Checked = false;
		}

		private void btnStartEmuhawkAttached_Click(object sender, EventArgs e)
		{
			Process.Start(RTC_Core.bizhawkDir + Path.DirectorySeparatorChar + "RESTARTATTACHEDRTC.bat");
		}

		private void btnStartWGH_Click(object sender, EventArgs e)
		{
			ProcessStartInfo psi =
				new ProcessStartInfo(RTC_Core.bizhawkDir + Path.DirectorySeparatorChar + "WGH\\WindowsGlitchHarvester.exe")
				{
					WorkingDirectory = RTC_Core.bizhawkDir + Path.DirectorySeparatorChar + "WGH"
				};
			Process.Start(psi);
		}

		private void btnStartVrun_Click(object sender, EventArgs e)
		{
			Process.Start("http://virus.run/");
		}
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Media;
using System.Diagnostics;
using CorruptCore;
using UI;
using static UI.UI_Extensions;

namespace UI
{
	public partial class RTC_SettingsNetCore_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_SettingsNetCore_Form()
		{
			InitializeComponent();
		}

		private void cbCrashSoundEffect_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (cbCrashSoundEffect.SelectedIndex)
			{
				/*
				case 0:
					var PlatesHdFiles = Directory.GetFiles(RTC_EmuCore.assetsDir + Path.DirectorySeparatorChar + "PLATESHD" + Path.DirectorySeparatorChar);
					RTC_NetCoreSettings.LoadedSounds = PlatesHdFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;
				case 1:
					RTC_NetCoreSettings.LoadedSounds = new SoundPlayer[] { new SoundPlayer(RTC_EmuCore.assetsDir + Path.DirectorySeparatorChar + "crash.wav") };
					break;

				case 2:
					RTC_NetCoreSettings.LoadedSounds = null;
					break;
				case 3:
					var CrashSoundsFiles = Directory.GetFiles(RTC_EmuCore.assetsDir + Path.DirectorySeparatorChar + "CRASHSOUNDS");
					RTC_NetCoreSettings.LoadedSounds = CrashSoundsFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;*/
			}

			//RTC_Params.SetParam("CRASHSOUND", cbCrashSoundEffect.SelectedIndex.ToString());
		}

		public void cbNetCoreCommandTimeout_SelectedIndexChanged(object sender, EventArgs e)
		{
			string setting = cbNetCoreCommandTimeout.SelectedItem.ToString().ToUpper();
			//RTC_NetCoreSettings.ChangeNetCoreSettings(setting);
			//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.AGGRESSIVENESS) { objectValue = setting }, true);
		}

		private void btnStartAutoKillSwitch_Click(object sender, EventArgs e)
		{
			Process.Start("AutoKillSwitch.exe");
		}

		private void nmGameProtectionDelay_ValueChanged(object sender, KeyPressEventArgs e) => UpdateGameProtectionDelay();

		private void nmGameProtectionDelay_ValueChanged(object sender, KeyEventArgs e) => UpdateGameProtectionDelay();

		private void nmGameProtectionDelay_ValueChanged(object sender, EventArgs e) => UpdateGameProtectionDelay();

		public void UpdateGameProtectionDelay()
		{
			RTC_GameProtection.BackupInterval = Convert.ToInt32(S.GET<RTC_SettingsNetCore_Form>().nmGameProtectionDelay.Value);
			if (RTC_GameProtection.isRunning)
				RTC_GameProtection.Reset();
		}

		private void RTC_SettingsNetCore_Form_Load(object sender, EventArgs e)
		{
				pnDetachedModeSettings.Controls.Clear();
				lbDetachedModeSettings.ForeColor = Color.Gray;
		}
	}
}

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
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
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
				case 0:
					var PlatesHdFiles = Directory.GetFiles(RTC_Corruptcore.assetsDir + Path.DirectorySeparatorChar + "PLATESHD" + Path.DirectorySeparatorChar);
					RTC_AutoKillSwitch.LoadedSounds = PlatesHdFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;
				case 1:
					RTC_AutoKillSwitch.LoadedSounds = new SoundPlayer[] { new SoundPlayer(RTC_Corruptcore.assetsDir + Path.DirectorySeparatorChar + "crash.wav") };
					break;

				case 2:
					RTC_AutoKillSwitch.LoadedSounds = null;
					break;
				case 3:
					var CrashSoundsFiles = Directory.GetFiles(RTC_Corruptcore.assetsDir + Path.DirectorySeparatorChar + "CRASHSOUNDS");
					RTC_AutoKillSwitch.LoadedSounds = CrashSoundsFiles.Select(it => new SoundPlayer(it)).ToArray();
					break;
			}

			RTC_Params.SetParam("CRASHSOUND", cbCrashSoundEffect.SelectedIndex.ToString());
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

		}
	}
}

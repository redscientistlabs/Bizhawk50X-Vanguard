using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RTC
{
	public partial class RTC_SettingsGeneral_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_SettingsGeneral_Form()
		{
			InitializeComponent();

			popoutAllowed = false;
		}

		private void btnImportKeyBindings_Click(object sender, EventArgs e)
		{
			if (NetCoreImplementation.isRemoteRTC && (NetCoreImplementation.RemoteRTC != null ? NetCoreImplementation.RemoteRTC.expectedSide != NetworkSide.SERVER : false))
			{
				MessageBox.Show("Can't import keybindings when not connected to Bizhawk!");
				return;
			}

			try
			{
				RTC_EmuCore.StopSound();

				if (RTC_EmuCore.bizhawkDir.Contains("\\VERSIONS\\"))
				{
					var bizhawkFolder = new DirectoryInfo(RTC_EmuCore.bizhawkDir);
					var LauncherVersFolder = bizhawkFolder.Parent.Parent;

					var versions = LauncherVersFolder.GetDirectories().Reverse().ToArray();

					var prevVersion = versions[1].Name;

					var dr = MessageBox.Show(
						"RTC Launcher detected,\n" +
						$"Do you want to import Controller/Hotkey bindings from version {prevVersion}"
						, $"Import config from previous version ?", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

					if (dr == DialogResult.Yes)
						Stockpile.LoadBizhawkKeyBindsFromIni(versions[1].FullName + "\\BizHawk\\config.ini");
					else
						Stockpile.LoadBizhawkKeyBindsFromIni();
				}
				else
					Stockpile.LoadBizhawkKeyBindsFromIni();
			}
			finally
			{
				RTC_EmuCore.StartSound();
			}
		}

		private void btnOpenOnlineWiki_Click(object sender, EventArgs e)
		{
			Process.Start("https://corrupt.wiki/");
		}

		private void btnChangeRTCColor_Click(object sender, EventArgs e)
		{
			RTC_UICore.SelectRTCColor();
		}

		private void cbDisableBizhawkOSD_CheckedChanged(object sender, EventArgs e)
		{
			if (cbDisableBizhawkOSD.Checked)
				RTC_Params.RemoveParam("ENABLE_BIZHAWK_OSD");
			else
				RTC_Params.SetParam("ENABLE_BIZHAWK_OSD");

			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BIZHAWK_SET_OSDDISABLED) { objectValue = cbDisableBizhawkOSD.Checked });
		}

		private void cbAllowCrossCoreCorruption_CheckedChanged(object sender, EventArgs e)
		{
			if (cbAllowCrossCoreCorruption.Checked)
				RTC_Params.SetParam("ALLOW_CROSS_CORE_CORRUPTION");
			else
				RTC_Params.RemoveParam("ALLOW_CROSS_CORE_CORRUPTION");

			RTC_EmuCore.AllowCrossCoreCorruption = cbAllowCrossCoreCorruption.Checked;
		}

		private void cbDontCleanAtQuit_CheckedChanged(object sender, EventArgs e)
		{
			if (cbDontCleanAtQuit.Checked)
				RTC_Params.SetParam("DONT_CLEAN_SAVESTATES_AT_QUIT");
			else
				RTC_Params.RemoveParam("DONT_CLEAN_SAVESTATES_AT_QUIT");

			NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BIZHAWK_SET_DONT_CLEAN_SAVESTATES_AT_QUIT) { objectValue = cbDontCleanAtQuit.Checked });
		}


	}
}

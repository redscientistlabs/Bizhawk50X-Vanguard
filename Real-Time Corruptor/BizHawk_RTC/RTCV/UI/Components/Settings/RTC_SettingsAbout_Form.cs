﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;
using RTCV.NetCore.StaticTools;

namespace RTCV.UI
{
	public partial class RTC_SettingsAbout_Form : ComponentForm, IAutoColorize
	{
		public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
		public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

		public RTC_SettingsAbout_Form()
		{
			InitializeComponent();

			this.undockedSizable = false;
		}

		private void RTC_SettingsAbout_Form_Load(object sender, EventArgs e)
		{
			lbVersion.Text += RTC_CorruptCore.RtcVersion;
			lbProcess.Text += (RTC_CorruptCore.Attached ? "Attached mode" : "Detached mode");
			lbConnectedTo.Text += "BizHawk Emulator";
		}
	}
}
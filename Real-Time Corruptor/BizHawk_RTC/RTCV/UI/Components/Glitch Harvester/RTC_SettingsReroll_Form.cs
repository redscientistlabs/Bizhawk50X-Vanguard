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
using RTCV.CorruptCore;

namespace RTCV.UI
{
	public partial class RTC_SettingsReroll_Form : Form, IAutoColorize
	{

		public RTC_SettingsReroll_Form()
		{
			InitializeComponent();

			cbRerollAddress.CheckedChanged += cbRerollAddress_CheckedChanged;
			cbRerollSourceAddress.CheckedChanged += cbRerollSourceAddress_CheckedChanged;

			RTC_UICore.SetRTCColor(RTC_UICore.GeneralColor, this);
			Load += RTC_SettingRerollForm_Load;
		}

		private void RTC_SettingRerollForm_Load(object sender, EventArgs e)
		{
			cbRerollAddress.Checked = RTC_Corruptcore.RerollAddress;
			cbRerollSourceAddress.Checked = RTC_Corruptcore.RerollSourceAddress;
		}

		private void cbRerollSourceAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Corruptcore.RerollSourceAddress = cbRerollSourceAddress.Checked;
			RTC_Params.SetParam("REROLL_SOURCEADDRESS", cbRerollSourceAddress.Checked.ToString());
		}

		private void cbRerollAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Corruptcore.RerollAddress = cbRerollAddress.Checked;
			RTC_Params.SetParam("REROLL_ADDRESS", cbRerollAddress.Checked.ToString());
		}
	}
}

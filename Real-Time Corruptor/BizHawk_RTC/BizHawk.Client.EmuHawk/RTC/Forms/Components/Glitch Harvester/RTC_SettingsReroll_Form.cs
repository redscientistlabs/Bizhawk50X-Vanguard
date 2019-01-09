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
	public partial class RTC_SettingsReroll_Form : Form, IAutoColorize
	{

		public RTC_SettingsReroll_Form()
		{
			InitializeComponent();

			cbRerollAddress.CheckedChanged += cbRerollAddress_CheckedChanged;
			cbRerollSourceAddress.CheckedChanged += cbRerollSourceAddress_CheckedChanged;

			RTC_Core.SetRTCColor(RTC_Core.GeneralColor, this);
			Load += RTC_SettingRerollForm_Load;
		}

		private void RTC_SettingRerollForm_Load(object sender, EventArgs e)
		{
			cbRerollAddress.Checked = RTC_Core.RerollAddress;
			cbRerollSourceAddress.Checked = RTC_Core.RerollSourceAddress;
		}

		private void cbRerollSourceAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Core.RerollSourceAddress = cbRerollSourceAddress.Checked;
			RTC_Params.SetParam("REROLL_SOURCEADDRESS", cbRerollSourceAddress.Checked.ToString());
		}

		private void cbRerollAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Core.RerollAddress = cbRerollAddress.Checked;
			RTC_Params.SetParam("REROLL_ADDRESS", cbRerollAddress.Checked.ToString());
		}
	}
}

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
			cbRerollAddress.Checked = (bool)RTC_Unispec.RTCSpec[RTCSPEC.REROLL_ADDRESS.ToString()];
			cbRerollSourceAddress.Checked = (bool)RTC_Unispec.RTCSpec[RTCSPEC.REROLL_SOURCEADDRESS.ToString()];

			cbRerollAddress.CheckedChanged += cbRerollAddress_CheckedChanged;
			cbRerollSourceAddress.CheckedChanged += cbRerollSourceAddress_CheckedChanged;

			RTC_Core.SetRTCColor(RTC_Core.GeneralColor, this);
		}

		private void cbRerollSourceAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.REROLL_SOURCEADDRESS.ToString(), cbRerollSourceAddress.Checked);
			RTC_Params.SetParam("REROLL_SOURCEADDRESS", cbRerollSourceAddress.Checked.ToString());
		}

		private void cbRerollAddress_CheckedChanged(object sender, EventArgs e)
		{
			RTC_Unispec.RTCSpec.Update(RTCSPEC.REROLL_ADDRESS.ToString(), cbRerollAddress.Checked);
			RTC_Params.SetParam("REROLL_ADDRESS", cbRerollAddress.Checked.ToString());
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorruptCore;
using RTCV.NetCore;
using static RTCV.CorruptCore.RTC_Corruptcore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public partial class RTC_Debug_Form : Form, IAutoColorize
	{
		public RTC_Debug_Form()
		{
			InitializeComponent();
			this.FormClosing += RTC_Debug_Form_FormClosing;
		}

		private void RTC_Debug_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			tbRTC.Text = CloudDebug.getRTCInfo();
		}



		private void button2_Click(object sender, EventArgs e)
		{
				richTextBox2.Text = CloudDebug.getEmuInfo();
		}



	}
}

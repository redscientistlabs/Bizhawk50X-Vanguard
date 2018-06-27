using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StandaloneRTC
{
	public partial class Loader : Form
	{
		Timer t;

		public Loader(string[] args)
		{
			InitializeComponent();

			RTC.RTC_Core.isStandalone = true;

			if (args.Contains("-CONSOLE"))
				BizHawk.Client.EmuHawk.LogConsole.CreateConsole();

			RTC.RTC_Core.Start(this);
			this.Hide();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			t = new Timer();
			t.Interval = 400;
			t.Tick += new EventHandler(CheckHeartbeat);
			t.Start();

			RTC.RTC_RPC.StartKillswitch();
		}

		private void CheckHeartbeat(object sender, EventArgs e)
		{
			if ((RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Value == RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum && !RTC.RTC_RPC.Heartbeat) || RTC.RTC_RPC.Freeze || !RTC.RTC_Core.coreForm.cbUseAutoKillSwitch.Checked)
				return;

			if (!RTC.RTC_RPC.Heartbeat)
			{
				RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.PerformStep();

				if (RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Value == RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Maximum)
				{
					//this.Focused = false;
					RTC.RTC_Core.csForm.btnStartEmuhawkDetached_Click(null, null);
					//this.Focused = false;
				}


			}
			else
			{
				RTC.RTC_Core.coreForm.pbAutoKillSwitchTimeout.Value = 0;
				RTC.RTC_RPC.Heartbeat = false;
			}



		}


		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Visible = false;
		}
	}
}

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
	public partial class Loader : RTC.RTC_Standalone_Form
	{ 
		Timer t;

		public Loader(string[] args)
		{
			InitializeComponent();

			RTC.NetCoreImplementation.isStandalone = true;

			if (args.Contains("-CONSOLE"))
			{
				RTC.RTC_Hooks.ShowConsole = true;
				BizHawk.Client.EmuHawk.LogConsole.CreateConsole();
			}

			RTC.RTC_Core.Start(this);
			this.Hide();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			t = new Timer();
			t.Interval = 400;
			//t.Tick += new EventHandler(CheckHeartbeat);
			t.Start();

		}


		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Visible = false;
		}
	}
}

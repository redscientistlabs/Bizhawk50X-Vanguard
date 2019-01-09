using System;
using System.Linq;
using System.Windows.Forms;
using RTC.Legacy;

namespace StandaloneRTC
{
	public partial class Loader : RTC.RTC_Standalone_Form
	{ 
		Timer t;

		public Loader(string[] args)
		{
			InitializeComponent();

			RTC_NetcoreImplementation.isStandaloneUI = true;
			RTC.LogConsole.CreateConsole();

			if (args.Contains("-CONSOLE"))
			{
				RTC.LogConsole.ShowConsole();
			}
			else
			{
				RTC.LogConsole.HideConsole();
			}

			RTC.RTC_EmuCore.Start(this);
			Hide();

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			t = new Timer
			{
				Interval = 400
			};
			t.Tick += CheckHeartbeat;
			t.Start();

			RTC.RTC_RPC.StartKillswitch();
		}

		private void CheckHeartbeat(object sender, EventArgs e)
		{
			if ((RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.Value == RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum && !RTC.RTC_RPC.Heartbeat) || RTC.RTC_RPC.Freeze || !RTC.S.GET<RTC.RTC_Core_Form>().cbUseAutoKillSwitch.Checked)
				return;

			if (!RTC.RTC_RPC.Heartbeat)
			{
				RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.PerformStep();

				if (RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.Value == RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.Maximum)
				{
					//this.Focused = false;
					RTC.S.GET<RTC.RTC_ConnectionStatus_Form>().btnStartEmuhawkDetached_Click(null, null);
					//this.Focused = false;
				}


			}
			else
			{
				RTC.S.GET<RTC.RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = 0;
				RTC.RTC_RPC.Heartbeat = false;
			}
		}


		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			Visible = false;
		}
	}
}

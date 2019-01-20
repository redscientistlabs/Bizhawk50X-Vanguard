using System;
using System.Linq;
using System.Windows.Forms;
using RTCV.UI;

namespace StandaloneRTC
{
	public partial class Loader : UI_Extensions.RTC_Standalone_Form
	{ 
		Timer t;

		public Loader(string[] args)
		{
			InitializeComponent();

			RTC.LogConsole.CreateConsole();

			if (args.Contains("-CONSOLE"))
			{
				RTC.LogConsole.ShowConsole();
			}
			else
			{
				RTC.LogConsole.HideConsole();
			}

		//	RTC.LogConsole.ShowConsole();
			RTC_UICore.Start(this);
		}

		private void Form1_Load(object sender, EventArgs e)
		{

		}


		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			Visible = false;
		}
	}
}

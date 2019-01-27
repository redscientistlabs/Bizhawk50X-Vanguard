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

			LogConsole.CreateConsole();

			if (args.Contains("-CONSOLE"))
			{
				LogConsole.ShowConsole();
			}
			else
			{
				LogConsole.HideConsole();
			}

		//	RTC.LogConsole.ShowConsole();
			UICore.Start(this);
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

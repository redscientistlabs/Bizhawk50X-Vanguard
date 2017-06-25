using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StandaloneRTC
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Loader());
			//RTC.RTC_Core.coreForm = new RTC.RTC_Form();
			//RTC.RTC_Core.isStandalone = true;
			//RTC.RTC_Core.Start();
			//Application.Run(RTC.RTC_Core.coreForm);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

            int nbInstances = 0;
            foreach (var prc in processes)
                if (prc == "STANDALONERTC")
                    nbInstances++;

           
            if (nbInstances > 1)
            {
                MessageBox.Show("RTC cannot run more than once at the time in Detached mode.\nLoading aborted","StandaloneRTC.exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

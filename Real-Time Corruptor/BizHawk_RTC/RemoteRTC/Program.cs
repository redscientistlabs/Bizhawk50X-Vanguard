using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RemoteRTC
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			List<string> allArgs = new List<string>(args);
			allArgs.Add("-REMOTE");

			return BizHawk.Client.EmuHawk.Program.Main(allArgs.ToArray());
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
		}
	}
}

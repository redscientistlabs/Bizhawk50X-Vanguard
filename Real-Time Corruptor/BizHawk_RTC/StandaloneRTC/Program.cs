using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StandaloneRTC
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

			//in case assembly resolution fails, such as if we moved them into the dll subdiretory, this event handler can reroute to them
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

			int nbInstances = processes.Count(prc => prc == "STANDALONERTC");

			if (nbInstances > 1)
			{
				MessageBox.Show("RTC cannot run more than once at the time in Detached mode.\nLoading aborted", "StandaloneRTC.exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Loader(args));
			//RTC.S.GET<RTC.RTC_Core_Form>() = new RTC.RTC_Form();
			//RTC.RTC_Core.isStandalone = true;
			//RTC.RTC_Core.Start();
			//Application.Run(RTC.S.GET<RTC.RTC_Core_Form>());
		}

		//Lifted from Bizhawk
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string requested = args.Name;

			lock (AppDomain.CurrentDomain)
			{
				var asms = AppDomain.CurrentDomain.GetAssemblies();
				foreach (var asm in asms)
					if (asm.FullName == requested)
						return asm;

				//load missing assemblies by trying to find them in the dll directory
				string dllname = new AssemblyName(requested).Name + ".dll";
				string directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dll");
				string simpleName = new AssemblyName(requested).Name;
				string fname = Path.Combine(directory, dllname);
				if (!File.Exists(fname)) return null;

				//it is important that we use LoadFile here and not load from a byte array; otherwise mixed (managed/unamanged) assemblies can't load
				return Assembly.LoadFile(fname);
			}
		}
	}
}

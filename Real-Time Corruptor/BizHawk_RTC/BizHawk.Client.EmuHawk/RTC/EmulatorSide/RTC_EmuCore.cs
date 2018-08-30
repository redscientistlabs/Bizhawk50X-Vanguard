using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using Newtonsoft.Json;
using RTCV.NetCore;

/*
isRemoteRTC = Bool stating that the process is emuhawk.exe in detached
isStandalone = Bool stating the process is standalonertc in detached
*/

namespace RTC
{
	public static class RTC_EmuCore
	{ 
		//General RTC Values
		public static string RtcVersion = "3.30";
		public static string[] args;

		public static FullSpec spec = null;
		
		public static List<ProblematicProcess> ProblematicProcesses;


		//Directories
		public static string bizhawkDir = Directory.GetCurrentDirectory();

		public static string rtcDir = bizhawkDir + "\\RTC\\";
		public static string workingDir = rtcDir + "\\WORKING\\";
		public static string assetsDir = rtcDir + "\\ASSETS\\";
		public static string paramsDir = rtcDir + "\\PARAMS\\";
		public static string listsDir = rtcDir + "\\LISTS\\";



		public static BindingList<Object> LimiterListBindingSource = new BindingList<Object>();
		public static BindingList<Object> ValueListBindingSource = new BindingList<Object>();


		//RTC Settings
		public static bool OsdDisabled
		{
			get { return (bool)spec["OsdDisabled"]; }
			set { spec.Update(new PartialSpec("EmuCore", "OsdDisabled", value)); }
		}
		public static bool AllowCrossCoreCorruption
		{
			get { return (bool)spec["AllowCrossCoreCorruption"]; }
			set { spec.Update(new PartialSpec("EmuCore", "AllowCrossCoreCorruption", value)); }
		}
		public static bool DontCleanSavestatesOnQuit
		{
			get { return (bool)spec["DontCleanSavestatesOnQuit"]; }
			set { spec.Update(new PartialSpec("EmuCore", "DontCleanSavestatesOnQuit", value)); }
		}
		public static string currentGameSystem
		{
			get { return (string)spec["currentGameSystem"]; }
			set { spec.Update(new PartialSpec("EmuCore", "currentGameSystem", value)); }
		}
		public static string currentGameName
		{
			get { return (string)spec["currentGameName"]; }
			set { spec.Update(new PartialSpec("EmuCore", "currentGameName", value)); }
		}
		public static string lastOpenRom
		{
			get { return (string)spec["lastOpenRom"]; }
			set { spec.Update(new PartialSpec("EmuCore", "lastOpenRom", value)); }
		}
		public static int lastLoaderRom
		{
			get { return (int)spec["lastLoaderRom"]; }
			set { spec.Update(new PartialSpec("EmuCore", "lastLoaderRom", value)); }
		}


		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			//Spawn a console for StandaloneRTC.
			//If we're in attached mode, we can't do this as the emulator itself may have something overriding stdout (Bizhawk)
			if (NetCoreImplementation.isStandaloneUI)
			{
				LogConsole.CreateConsole();
				if (!RTC_Hooks.ShowConsole)
					LogConsole.HideConsole();
			}

			//Timed releases. Only for exceptionnal cases.
			bool Expires = false;
			DateTime ExpiringDate = DateTime.Parse("2017-03-03");
			if (Expires && DateTime.Now > ExpiringDate)
			{
				//RTC_RPC.SendToKillSwitch("CLOSE");
				MessageBox.Show("This version has expired");
				RTC_Hooks.BIZHAWK_MAINFORM_CLOSE();
				S.GET<RTC_Core_Form>().Close();
				S.GET<RTC_GlitchHarvester_Form>().Close();
				Application.Exit();
				return;
			}


			S.SET(_standaloneForm);

				RTC_Extensions.DirectoryRequired(new string[] {
					workingDir,
					workingDir + "\\TEMP\\",
					workingDir + "\\SKS\\",
					workingDir + "\\SSK\\",
					workingDir + "\\SESSION\\",
					workingDir + "\\MEMORYDUMPS\\",
					assetsDir + "\\CRASHSOUNDS\\",
					rtcDir + "\\PARAMS\\",
					rtcDir + "\\LISTS\\",
					});

			//Set initial spec for EmuCore
			PartialSpec partial = new PartialSpec("EmuCore");

			partial["OsdDisabled"] = true;
			partial["AllowCrossCoreCorruption"] = false;
			partial["DontCleanSavestatesOnQuit"] = false;
			partial["currentGameSystem"] = null;
			partial["currentGameName"] = null;
			partial["lastOpenRom"] = null;
			partial["lastLoaderRom"] = 0;
			partial["domains"] = new MemoryDomainProto[0];
			RTC_EmuCore.spec = new FullSpec(partial);


			spec.SpecUpdated += (object o, SpecUpdateEventArgs e) =>
			{
				if (!NetCoreImplementation.isStandaloneUI && NetCoreImplementation.isStandaloneEmu)
					NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.SPECUPDATE) { objectValue = e.partialSpec});

				if(NetCoreImplementation.isStandaloneUI || NetCoreImplementation.isAttached)
				{
					S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
				}

			};
				


			//RTC_EmuCore.spec.Update(partial);


			//Start other components here
			RTC_CorruptCore.Start();
			RTC_UICore.Start();

			//Loading RTC Params
			RTC_Params.LoadRTCColor();
			S.GET<RTC_SettingsGeneral_Form>().cbDisableBizhawkOSD.Checked = !RTC_Params.IsParamSet("ENABLE_BIZHAWK_OSD");
			S.GET<RTC_SettingsGeneral_Form>().cbAllowCrossCoreCorruption.Checked = RTC_Params.IsParamSet("ALLOW_CROSS_CORE_CORRUPTION");
			S.GET<RTC_SettingsGeneral_Form>().cbDontCleanAtQuit.Checked = RTC_Params.IsParamSet("DONT_CLEAN_SAVESTATES_AT_QUIT");

			//Load and initialize Hotkeys
			//RTC_Hotkeys.InitializeHotkeySystem();
			//RTC_Params.LoadHotkeys();
			//	RTC_Hotkeys.Test("None", "D", "REMOTE_HOTKEY_MANUALBLAST");



			NetCoreImplementation.Start();

			if (NetCoreImplementation.isStandaloneUI || NetCoreImplementation.isAttached)
				S.GET<RTC_Core_Form>().Show();

			//Refocus on Bizhawk
			RTC_Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			if (!File.Exists(RTC_EmuCore.bizhawkDir + "\\config.ini"))
				RTC_Hooks.BIZHAWK_SAVE_CONFIG();

			//Fetch NetCore aggressiveness
			if (NetCoreImplementation.isStandaloneEmu)
				NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));
		}


		public static void DownloadProblematicProcesses()
		{
			string LocalPath = RTC_EmuCore.paramsDir + "\\BADPROCESSES";
			string json = "";
			try
			{
				if (File.Exists(LocalPath))
				{
					DateTime lastModified = File.GetLastWriteTime(LocalPath);
					if (lastModified.Date == DateTime.Today)
						return;
				}
				WebClientTimeout client = new WebClientTimeout();
				client.Headers[HttpRequestHeader.Accept] = "text/html, image/png, image/jpeg, image/gif, */*;q=0.1";
				client.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows; U; Windows NT 6.1; de; rv:1.9.2.12) Gecko/20101026 Firefox/3.6.12";
				json = client.DownloadString("https://raw.githubusercontent.com/ircluzar/RTC3/master/ProblematicProcesses.json");
				File.WriteAllText(LocalPath, json);
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					//Couldn't download the new one so just fall back to the old one if it's there
					Console.WriteLine(ex.ToString());
					if (File.Exists(LocalPath))
					{
						try
						{
							json = File.ReadAllText(LocalPath);
						}
						catch (Exception _ex)
						{
							Console.WriteLine("Couldn't read BADPROCESSES\n\n" + _ex.ToString());
							return;
						}
					}
					else
						return;
				}
				else
				{
					Console.WriteLine(ex.ToString());
				}
			}

			try
			{
				ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(json);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				if (File.Exists(LocalPath))
					File.Delete(LocalPath);
				return;
			}
		}

		//Checks if any problematic processes are found
		public static volatile bool Warned = false;

		public static void CheckForProblematicProcesses()
		{
			if (Warned || ProblematicProcesses == null)
				return;

			try
			{
				var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

				//Warn based on loaded processes
				foreach (var item in ProblematicProcesses)
				{
					if (processes.Contains(item.Name))
					{
						MessageBox.Show(item.Message, "Incompatible Program Detected!");
						Warned = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return;
			}
		}

		public static void StartSound()
		{
			RTC_Hooks.BIZHAWK_STARTSOUND();
		}
		public static void StopSound()
		{
			RTC_Hooks.BIZHAWK_STOPSOUND();
		}

		public static string EmuFolderCheck(string SystemDisplayName)
		{
			//Workaround for Bizhawk's folder name quirk

			if (SystemDisplayName.Contains("(INTERIM)"))
			{
				char[] delimiters = {'(', ' ', ')'};

				string temp = SystemDisplayName.Split(delimiters)[0];
					SystemDisplayName = temp + "_INTERIM";
			}
			switch (SystemDisplayName)
			{
				case "Playstation":
					return "PSX";
				case "GG":
					return "Game Gear";
				case "Commodore 64":
					return "C64";
				case "SG":
					return "SG-1000";
				default:
					return SystemDisplayName;
			}
		}

		public static void LoadRom_NET(string RomFile)
		{
			var loadRomWatch = System.Diagnostics.Stopwatch.StartNew();
			// -> EmuHawk Process only
			//Loads a rom inside Bizhawk from a Filename.

			RTC_EmuCore.StopSound();

			var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM(); ;


			RTC_Hooks.AllowCaptureRewindState = false;
			RTC_Hooks.BIZHAWK_LOADROM(RomFile);
			RTC_Hooks.AllowCaptureRewindState = true;

			RTC_EmuCore.StartSound();
			loadRomWatch.Stop();
			Console.WriteLine($"Time taken for LoadRom_NET: {0}ms", loadRomWatch.ElapsedMilliseconds);
		}

		public static string SaveSavestate_NET(string Key, bool threadSave = false)
		{
			// -> EmuHawk Process only
			//Creates a new savestate and returns the key to it.

			if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE())
				return null;

			string quickSlotName = Key + ".timejump";

			string prefix = RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX();
			prefix = prefix.Substring(prefix.LastIndexOf('\\') + 1);

			var path = RTC_EmuCore.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";

			var file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();


			if (threadSave)
			{
				(new Thread(() =>
				{
					try
					{
						RTC_Hooks.BIZHAWK_SAVESTATE(path, quickSlotName);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Thread collision ->\n" + ex.ToString());
					}
				})).Start();
			}
			else
				RTC_Hooks.BIZHAWK_SAVESTATE(path, quickSlotName);

			return path;
		}

		public static bool LoadSavestate_NET(string Key)
		{
			try
			{

				// -> EmuHawk Process only
				// Loads a Savestate from a key

				if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE())
					return false;

				string quickSlotName = Key + ".timejump";

				string prefix = RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX();
				prefix = prefix.Substring(prefix.LastIndexOf('\\') + 1);

				var path = RTC_EmuCore.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";


				if (File.Exists(path) == false)
				{
					RTC_Hooks.BIZHAWK_OSDMESSAGE("Unable to load " + quickSlotName + ".State");
					return false;
				}

				RTC_Hooks.BIZHAWK_LOADSTATE(path, quickSlotName);

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
		}

		public static void LoadDefaultRom()
		{
			//Loads a NES-based title screen.
			//Can be overriden by putting a file named "overridedefault.nes" in the ASSETS folder

			int newNumber = lastLoaderRom;

			while (newNumber == lastLoaderRom)
			{
				int nbNesFiles = Directory.GetFiles(RTC_EmuCore.assetsDir, "*.nes").Length;

				newNumber = RTC_CorruptCore.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(RTC_EmuCore.assetsDir + "overridedefault.nes"))
						LoadRom_NET(RTC_EmuCore.assetsDir + "overridedefault.nes");
					//Please ignore
					else if (RTC_CorruptCore.RND.Next(0, 420) == 7)
						LoadRom_NET(RTC_EmuCore.assetsDir + "gd.fds");
					else
						LoadRom_NET(RTC_EmuCore.assetsDir + newNumber.ToString() + "default.nes");

					lastLoaderRom = newNumber;
					break;
				}
			}
		}

	}
}

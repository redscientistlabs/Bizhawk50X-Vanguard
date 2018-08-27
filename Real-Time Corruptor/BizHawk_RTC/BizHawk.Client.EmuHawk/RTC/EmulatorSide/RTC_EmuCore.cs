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
using Newtonsoft.Json;

/*
isRemoteRTC = Bool stating that the process is emuhawk.exe in detached
isStandalone = Bool stating the process is standalonertc in detached
*/

namespace RTC
{
	public static class RTC_Core
	{
		public static string[] args;
		
		public static List<ProblematicProcess> ProblematicProcesses;

		//General RTC Values
		public static string RtcVersion = "3.30";

		//Directories
		public static string bizhawkDir = Directory.GetCurrentDirectory();

		public static string rtcDir = bizhawkDir + "\\RTC\\";
		public static string workingDir = rtcDir + "\\WORKING\\";
		public static string assetsDir = rtcDir + "\\ASSETS\\";
		public static string paramsDir = rtcDir + "\\PARAMS\\";
		public static string listsDir = rtcDir + "\\LISTS\\";

		//Engine Values


		public static BindingList<Object> LimiterListBindingSource = new BindingList<Object>();
		public static BindingList<Object> ValueListBindingSource = new BindingList<Object>();



		public static bool ClearStepActionsOnRewind = false;
		public static bool ExtractBlastLayer = false;
		public static string lastOpenRom = null;
		public static int lastLoaderRom = 0;

		//RTC Settings
		public static bool BizhawkOsdDisabled = true;
		public static bool UseHexadecimal = true;
		public static bool AllowCrossCoreCorruption = false;
		public static bool DontCleanSavestatesOnQuit = false;

		//Note Box Settings
		public static System.Drawing.Point NoteBoxPosition;
		public static System.Drawing.Size NoteBoxSize;

		//RTC Main Forms
		public static Color generalColor = Color.LightSteelBlue;


		//All RTC forms
		public static Form[] allRtcForms
		{
			get
			{
				//This fetches all singletons of interface IAutoColorized

				List<Form> all = new List<Form>();

				foreach (Type t in Assembly.GetAssembly(typeof(S)).GetTypes())
					if (typeof(IAutoColorize).IsAssignableFrom(t) && t != typeof(IAutoColorize))
						all.Add((Form)S.GET(Type.GetType(t.ToString())));

				return all.ToArray();
				
			}
		}


		public static volatile bool isClosing = false;

		public static void CloseAllRtcForms() //This allows every form to get closed to prevent RTC from hanging
		{
			if (isClosing)
				return;

			isClosing = true;

			if (NetCoreImplementation.Multiplayer != null && NetCoreImplementation.Multiplayer.streamReadingThread != null)
				NetCoreImplementation.Multiplayer.streamReadingThread.Abort();

			if (NetCoreImplementation.RemoteRTC != null && NetCoreImplementation.RemoteRTC.streamReadingThread != null)
				NetCoreImplementation.RemoteRTC.streamReadingThread.Abort();

			foreach (Form frm in allRtcForms)
			{
				if (frm != null)
					frm.Close();
			}

			if (S.GET<RTC_Standalone_Form>() != null)
				S.GET<RTC_Standalone_Form>().Close();

			//Clean out the working folders
			if (!NetCoreImplementation.isRemoteRTC && !RTC_Core.DontCleanSavestatesOnQuit)
			{
				Stockpile.EmptyFolder("\\WORKING\\");
			}

			Application.Exit();
		}


		public static void DownloadProblematicProcesses()
		{
			string LocalPath = RTC_Core.paramsDir + "\\BADPROCESSES";
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

		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			//Spawn a console for StandaloneRTC.
			//If we're in attached mode, we can't do this as the emulator itself may have something overriding stdout (Bizhawk)
			if (NetCoreImplementation.isStandalone)
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
			

			S.SET<RTC_Standalone_Form>(_standaloneForm);


			if (!Directory.Exists(RTC_Core.workingDir))
				Directory.CreateDirectory(RTC_Core.workingDir);

			if (!Directory.Exists(RTC_Core.workingDir + "\\TEMP\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\TEMP\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SKS\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SKS\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SSK\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SSK\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\SESSION\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\SESSION\\");

			if (!Directory.Exists(RTC_Core.workingDir + "\\MEMORYDUMPS\\"))
				Directory.CreateDirectory(RTC_Core.workingDir + "\\MEMORYDUMPS\\");

			if (!Directory.Exists(RTC_Core.assetsDir + "\\CRASHSOUNDS\\"))
				Directory.CreateDirectory(RTC_Core.assetsDir + "\\CRASHSOUNDS\\");

			if (!Directory.Exists(RTC_Core.rtcDir + "\\PARAMS\\"))
				Directory.CreateDirectory(RTC_Core.rtcDir + "\\PARAMS\\");

			if (!Directory.Exists(RTC_Core.rtcDir + "\\LISTS\\"))
				Directory.CreateDirectory(RTC_Core.rtcDir + "\\LISTS\\");


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

			if (NetCoreImplementation.isRemoteRTC && !NetCoreImplementation.isStandalone)
				S.GET<RTC_Core_Form>().Show();

			//Refocus on Bizhawk
			RTC_Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			if (!File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
				RTC_Hooks.BIZHAWK_SAVE_CONFIG();

			//Fetch NetCore aggressiveness
			if (NetCoreImplementation.isRemoteRTC)
				NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));
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

			RTC_Core.StopSound();

			var args = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM(); ;


			RTC_Hooks.AllowCaptureRewindState = false;
			RTC_Hooks.BIZHAWK_LOADROM(RomFile);
			RTC_Hooks.AllowCaptureRewindState = true;

			RTC_Core.StartSound();
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

			var path = RTC_Core.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";

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

				var path = RTC_Core.workingDir + "\\SESSION\\" + prefix + "." + quickSlotName + ".State";


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
				int nbNesFiles = Directory.GetFiles(RTC_Core.assetsDir, "*.nes").Length;

				newNumber = RTC_CorruptCore.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(RTC_Core.assetsDir + "overridedefault.nes"))
						LoadRom_NET(RTC_Core.assetsDir + "overridedefault.nes");
					//Please ignore
					else if (RTC_CorruptCore.RND.Next(0, 420) == 7)
						LoadRom_NET(RTC_Core.assetsDir + "gd.fds");
					else
						LoadRom_NET(RTC_Core.assetsDir + newNumber.ToString() + "default.nes");

					lastLoaderRom = newNumber;
					break;
				}
			}
		}

	}
}

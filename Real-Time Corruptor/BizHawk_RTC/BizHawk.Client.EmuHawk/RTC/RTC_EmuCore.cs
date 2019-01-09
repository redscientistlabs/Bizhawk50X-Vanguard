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
using RTC.Legacy;
using RTCV.NetCore;
using static RTC.RTC_Unispec;

namespace RTC
{
	public static class RTC_EmuCore
	{
		public static string[] args;

		public static List<ProblematicProcess> ProblematicProcesses;

		//Directories
		public static string bizhawkDir = Directory.GetCurrentDirectory();

		public static string rtcDir = bizhawkDir + Path.DirectorySeparatorChar + "RTC" + Path.DirectorySeparatorChar;
		public static string workingDir = rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar;
		public static string assetsDir = rtcDir + Path.DirectorySeparatorChar + "ASSETS" + Path.DirectorySeparatorChar;
		public static string paramsDir = rtcDir + Path.DirectorySeparatorChar + "PARAMS" + Path.DirectorySeparatorChar;
		public static string listsDir = rtcDir + Path.DirectorySeparatorChar + "LISTS" + Path.DirectorySeparatorChar;


		public static string CurrentGameSystem
		{
			get => (string)EmuSpec[EMUSPEC.STOCKPILE_CURRENTGAMESYSTEM.ToString()];
			set => EmuSpec.Update(EMUSPEC.STOCKPILE_CURRENTGAMESYSTEM.ToString(), value);
		}
		public static string CurrentGameName
		{
			get => (string)EmuSpec[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()];
			set => EmuSpec.Update(EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString(), value);
		}
		public static string LastOpenRom
		{
			get => (string)EmuSpec[EMUSPEC.CORE_LASTOPENROM.ToString()];
			set => EmuSpec.Update(EMUSPEC.CORE_LASTOPENROM.ToString(), value);
		}
		public static int LastLoaderRom
		{
			get => (int)EmuSpec[EMUSPEC.CORE_LASTLOADERROM.ToString()];
			set => EmuSpec.Update(EMUSPEC.CORE_LASTLOADERROM.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");


			partial[EMUSPEC.STOCKPILE_CURRENTGAMESYSTEM.ToString()] = null;
			partial[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()] = null;
			partial[EMUSPEC.CORE_LASTOPENROM.ToString()] = null;
			partial[EMUSPEC.CORE_LASTLOADERROM.ToString()] = -1;


			return partial;
		}

		public static volatile FullSpec EmuSpec;


		public static void RegisterEmuhawkSpec()
		{
			PartialSpec emuSpecTemplate = new PartialSpec("EmuSpec");

			emuSpecTemplate.Insert(RTC_EmuCore.getDefaultPartial());

			EmuSpec = new FullSpec(emuSpecTemplate); //You have to feed a partial spec as a template


			EmuSpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;

				//Only send the update if we're connected
				if (RTC_NetcoreImplementation.RemoteRTC_SupposedToBeConnected)
					RTC_NetcoreImplementation.SendCommandToBizhawk(
						new RTC_Command(CommandType.REMOTE_PUSHEMUSPECUPDATE) { objectValue = partial }, true);
			};
		}

		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			//Spawn a console for StandaloneRTC && initialize spec
			if (RTC_NetcoreImplementation.isStandaloneUI)
			{
				RTC_Corruptcore.Start();

				LogConsole.CreateConsole();
				if (!RTC_Corruptcore.ShowConsole)
					LogConsole.HideConsole();
			}

			//Register the RTC spec in attached mode
			if (!RTC_NetcoreImplementation.isStandaloneUI && !RTC_NetcoreImplementation.isStandaloneEmu)
			{
				RTC_Corruptcore.Start();
			}

			S.SET(_standaloneForm);

			RTC_Extensions.DirectoryRequired(new string[] {
				workingDir,
				workingDir + "\\TEMP\\",
				workingDir + "\\SKS\\",
				workingDir + "\\SSK\\",
				workingDir + "\\SESSION\\",
				workingDir + "\\MEMORYDUMPS\\",
				workingDir + "\\MP\\",
				assetsDir + "\\CRASHSOUNDS\\",
				rtcDir + "\\PARAMS\\",
				rtcDir + "\\LISTS\\",
			});


			RTC_EmuCore.RegisterEmuhawkSpec();
			RTC_NetcoreImplementation.Start();

			// Show the main RTC Form

			if (RTC_NetcoreImplementation.isStandaloneUI || RTC_NetcoreImplementation.isAttached)
				S.GET<RTC_Core_Form>().Show();

			//Refocus on Bizhawk
			RTC_Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			if (!File.Exists(RTC_EmuCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"))
				RTC_Hooks.BIZHAWK_SAVE_CONFIG();

			//Fetch NetCore aggressiveness
			if (RTC_NetcoreImplementation.isStandaloneEmu)
				RTC_NetcoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));

		}

		public static void DownloadProblematicProcesses()
		{
			string LocalPath = RTC_EmuCore.paramsDir + Path.DirectorySeparatorChar + "BADPROCESSES";
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
				RTC_EmuCore.ProblematicProcesses = JsonConvert.DeserializeObject<List<ProblematicProcess>>(json);
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
			if (Warned || RTC_EmuCore.ProblematicProcesses == null)
				return;

			try
			{
				var processes = Process.GetProcesses().Select(it => $"{it.ProcessName.ToUpper()}").OrderBy(x => x).ToArray();

				//Warn based on loaded processes
				foreach (var item in RTC_EmuCore.ProblematicProcesses)
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
				char[] delimiters = { '(', ' ', ')' };

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

		public static void LoadDefaultRom()
		{
			//Loads a NES-based title screen.
			//Can be overriden by putting a file named "overridedefault.nes" in the ASSETS folder

			int lastLoaderRom = RTC_EmuCore.LastLoaderRom;
			int newNumber = RTC_EmuCore.LastLoaderRom;

			while (newNumber == lastLoaderRom)
			{
				int nbNesFiles = Directory.GetFiles(RTC_EmuCore.assetsDir, "*.nes").Length;

				newNumber = RTC_Corruptcore.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(RTC_EmuCore.assetsDir + "overridedefault.nes"))
						LoadRom(RTC_EmuCore.assetsDir + "overridedefault.nes");
					//Please ignore
					else if (RTC_Corruptcore.RND.Next(0, 420) == 7)
						LoadRom(RTC_EmuCore.assetsDir + "gd.fds");
					else
						LoadRom(RTC_EmuCore.assetsDir + newNumber.ToString() + "default.nes");

					lastLoaderRom = newNumber;
					break;
				}
			}
		}

		public static void LoadRom(string RomFile, bool sync = false)
		{
			// Safe method for loading a Rom file from any process

			RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADROM)
			{
				romFilename = RomFile
			}, sync);
		}

		public static void LoadRom_NET(string RomFile)
		{
			var loadRomWatch = Stopwatch.StartNew();
			// -> EmuHawk Process only
			//Loads a rom inside Bizhawk from a Filename.

			StopSound();

			//var args = new MainForm.LoadRomArgs();

			if (RomFile == null)
				RomFile = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM(); ;


			RTC_Hooks.AllowCaptureRewindState = false;
			RTC_Hooks.BIZHAWK_LOADROM(RomFile);
			RTC_Hooks.AllowCaptureRewindState = true;

			StartSound();
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

			var path = RTC_EmuCore.workingDir + Path.DirectorySeparatorChar + "SESSION" + Path.DirectorySeparatorChar + prefix + "." + quickSlotName + ".State";

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

		public static bool LoadSavestate_NET(string Key, StashKeySavestateLocation stateLocation)
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

				var path = RTC_EmuCore.workingDir + stateLocation + Path.DirectorySeparatorChar + prefix + "." + quickSlotName + ".State";


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
	}
}

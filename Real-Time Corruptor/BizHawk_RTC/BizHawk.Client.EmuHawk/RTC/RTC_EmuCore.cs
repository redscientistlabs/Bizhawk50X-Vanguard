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
using BizHawk.Client.EmuHawk;
using CorruptCore;
using Newtonsoft.Json;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.Vanguard;

namespace RTC
{
	public static class RTC_EmuCore
	{
		public static string[] args;

		public static string System
		{
			get => (string)EmuSpec[VSPEC.SYSTEM.ToString()];
			set => EmuSpec.Update(VSPEC.SYSTEM.ToString(), value);
		}
		public static string GameName
		{
			get => (string)EmuSpec[VSPEC.GAMENAME.ToString()];
			set => EmuSpec.Update(VSPEC.GAMENAME.ToString(), value);
		}
		public static string SystemPrefix
		{
			get => (string)EmuSpec[VSPEC.SYSTEMPREFIX.ToString()];
			set => EmuSpec.Update(VSPEC.SYSTEMPREFIX.ToString(), value);
		}
		public static string SystemCore
		{
			get => (string)EmuSpec[VSPEC.SYSTEMCORE.ToString()];
			set => EmuSpec.Update(VSPEC.SYSTEMCORE.ToString(), value);
		}
		public static string SyncSettings
		{
			get => (string)EmuSpec[VSPEC.SYNCSETTINGS.ToString()];
			set => EmuSpec.Update(VSPEC.SYNCSETTINGS.ToString(), value);
		}
		public static string OpenRomFilename
		{
			get => (string)EmuSpec[VSPEC.OPENROMFILENAME.ToString()];
			set => EmuSpec.Update(VSPEC.OPENROMFILENAME.ToString(), value);
		}
		public static int LastLoaderRom
		{
			get => (int)EmuSpec[VSPEC.CORE_LASTLOADERROM.ToString()];
			set => EmuSpec.Update(VSPEC.CORE_LASTLOADERROM.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial[VSPEC.SYSTEM.ToString()] = String.Empty;
			partial[VSPEC.GAMENAME.ToString()] = String.Empty;
			partial[VSPEC.SYSTEMPREFIX.ToString()] = String.Empty;
			partial[VSPEC.OPENROMFILENAME.ToString()] = String.Empty;
			partial[VSPEC.SYNCSETTINGS.ToString()] = String.Empty;
			partial[VSPEC.OPENROMFILENAME.ToString()] = String.Empty;
			partial[VSPEC.CORE_LASTLOADERROM.ToString()] = -1;

			return partial;
		}

		public static volatile FullSpec EmuSpec;


		public static void RegisterEmuhawkSpec()
		{
			PartialSpec emuSpecTemplate = new PartialSpec("EmuSpec");

			emuSpecTemplate.Insert(RTC_EmuCore.getDefaultPartial());

			EmuSpec = new FullSpec(emuSpecTemplate); //You have to feed a partial spec as a template


			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHEMUSPEC, emuSpecTemplate, true);


			EmuSpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;
				RTC_Corruptcore.VanguardSpec = EmuSpec;
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHEMUSPECUPDATE, partial, true);
			};
		}

		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			VanguardImplementation.StartClient(GlobalWin.MainForm);
			RTC_EmuCore.RegisterEmuhawkSpec();


			//S.SET(_standaloneForm);

			RTC_Extensions.DirectoryRequired(new string[] {
				RTC_Corruptcore.workingDir, RTC_Corruptcore.workingDir + "\\TEMP\\", RTC_Corruptcore.workingDir + "\\SKS\\", RTC_Corruptcore.workingDir + "\\SSK\\", RTC_Corruptcore.workingDir + "\\SESSION\\", RTC_Corruptcore.workingDir + "\\MEMORYDUMPS\\", RTC_Corruptcore.workingDir + "\\MP\\", RTC_Corruptcore.assetsDir + "\\CRASHSOUNDS\\", RTC_Corruptcore.rtcDir + "\\PARAMS\\", RTC_Corruptcore.rtcDir + "\\LISTS\\",
			});


			// Show the main RTC Form


			//Todo
			//if (RTC_NetcoreImplementation.isStandaloneUI || RTC_NetcoreImplementation.isAttached)
			//S.GET<RTC_Core_Form>().Show();

			//Refocus on Bizhawk
			RTC_Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			if (!File.Exists(RTC_Corruptcore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"))
				RTC_Hooks.BIZHAWK_MAINFORM_SAVECONFIG();

			//Fetch NetCore aggressiveness
		//	if (RTC_NetcoreImplementation.isStandaloneEmu)
		//		RTC_NetcoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.GETAGGRESSIVENESS));

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
				int nbNesFiles = Directory.GetFiles(RTC_Corruptcore.assetsDir, "*.nes").Length;

				newNumber = RTC_Corruptcore.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(RTC_Corruptcore.assetsDir + "overridedefault.nes"))
						LoadRom_NET(RTC_Corruptcore.assetsDir + "overridedefault.nes");
					//Please ignore
					else if (RTC_Corruptcore.RND.Next(0, 420) == 7)
						LoadRom_NET(RTC_Corruptcore.assetsDir + "gd.fds");
					else
						LoadRom_NET(RTC_Corruptcore.assetsDir + newNumber.ToString() + "default.nes");

					lastLoaderRom = newNumber;
					break;
				}
			}
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

			var path = RTC_Corruptcore.workingDir + Path.DirectorySeparatorChar + "SESSION" + Path.DirectorySeparatorChar + prefix + "." + quickSlotName + ".State";

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

		public static bool LoadSavestate_NET(string path, StashKeySavestateLocation stateLocation)
		{
			try
			{
				if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE())
					return false;


				if (File.Exists(path) == false)
				{
					RTC_Hooks.BIZHAWK_OSDMESSAGE("Unable to load " + Path.GetFileName(path) + " from " + stateLocation);
					return false;
				}

				RTC_Hooks.BIZHAWK_LOADSTATE(path);

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
		}

		public static void LoadBizhawkWindowState()
		{
			if (RTC_Params.IsParamSet("BIZHAWK_SIZE"))
			{
				string[] size = RTC_Params.ReadParam("BIZHAWK_SIZE").Split(',');
				RTC_Hooks.BIZHAWK_GETSET_MAINFORMSIZE = new Size(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
				string[] location = RTC_Params.ReadParam("BIZHAWK_LOCATION").Split(',');
				RTC_Hooks.BIZHAWK_GETSET_MAINFORMLOCATION = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
			}
		}

		public static void SaveBizhawkWindowState()
		{
			var size = RTC_Hooks.BIZHAWK_GETSET_MAINFORMSIZE;
			var location = RTC_Hooks.BIZHAWK_GETSET_MAINFORMLOCATION;

			RTC_Params.SetParam("BIZHAWK_SIZE", $"{size.Width},{size.Height}");
			RTC_Params.SetParam("BIZHAWK_LOCATION", $"{location.X},{location.Y}");
		}

		public static void LoadDefaultAndShowBizhawkForm()
		{

			RTC_EmuCore.LoadDefaultRom();

			RTC_EmuCore.LoadBizhawkWindowState();

			GlobalWin.MainForm.Focus();

			//Yell at the user if they're using audio throttle as it's buggy
			//We have to do this in the bizhawk process
			if (Global.Config.SoundThrottle)
			{
				MessageBox.Show("Sound throttle is buggy and can result in crashes.\nSwapping to clock throttle.");
				Global.Config.SoundThrottle = false;
				Global.Config.ClockThrottle = true;
				RTC_Hooks.BIZHAWK_MAINFORM_SAVECONFIG();
			}
		}

	}
}

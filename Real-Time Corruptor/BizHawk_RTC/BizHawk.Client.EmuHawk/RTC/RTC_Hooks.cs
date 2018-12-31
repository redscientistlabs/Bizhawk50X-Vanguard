using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static RTC.RTC_Unispec;

namespace RTC
{
	public static class RTC_Hooks
	{
		//Instead of writing code inside bizhawk, hooks are placed inside of it so will be easier
		//to upgrade BizHawk when they'll release a new version.

		// Here are the keywords for searching hooks and fixes: //RTC_HIJACK

		static bool disableRTC;
		public static bool isRemoteRTC = false;
		public static bool isNormalAdvance = false;

		private static Guid? loadGameToken = null;
		private static Guid? loadSavestateToken = null;

		public static System.Diagnostics.Stopwatch watch = null;

		public static volatile bool BIZHAWK_ALLOWED_DOUBLECLICK_FULLSCREEN = true;

		static int CPU_STEP_Count = 0;

		public static void CPU_STEP(bool isRewinding, bool isFastForwarding, bool isBeforeStep = false)
		{
			if (disableRTC || Global.Emulator is NullEmulator)
				return;
			
			//Return out if it's being called from before the step and we're not on frame 0. If we're on frame 0, then we go as normal
			//If we can't get runbefore, just assume we don't want to run before
			if (isBeforeStep && CPU_STEP_Count != 0 && ((bool)(RTC_Unispec.RTCSpec?[RTCSPEC.STEP_RUNBEFORE.ToString()] ?? false)) == false)
				return;

			isNormalAdvance = !(isRewinding || isFastForwarding);

			// Unique step hooks
			if (!isRewinding && !isFastForwarding)
				STEP_FORWARD();
			else if (isRewinding)
				STEP_REWIND();
			else if (isFastForwarding)
				STEP_FASTFORWARD();

			//Any step hook for corruption
			STEP_CORRUPT(isRewinding, isFastForwarding);
		}

		private static void STEP_FORWARD()
		{
			if (disableRTC) return;
		}

		private static void STEP_REWIND()
		{
			if (disableRTC) return;

			if ((bool)RTCSpec[RTCSPEC.CORE_CLEARSTEPACTIONSONREWIND.ToString()])
				RTC_StepActions.ClearStepBlastUnits();
		}

		private static void STEP_FASTFORWARD()
		{
			if (disableRTC) return;
		}

		private static void STEP_CORRUPT(bool _isRewinding, bool _isFastForwarding)
		{
			if (disableRTC) return;

			if (!_isRewinding)
			{
				RTC_StepActions.Execute();
			}

			if (_isRewinding || _isFastForwarding)
				return;

			CPU_STEP_Count++;

			bool autoCorrupt = (bool)(RTCSpec?[RTCSPEC.CORE_AUTOCORRUPT.ToString()] ?? false);
			int intensity = (int)(RTCSpec?[RTCSPEC.CORE_ERRORDELAY.ToString()] ?? -1);
			if (autoCorrupt && CPU_STEP_Count >= intensity)
			{
				CPU_STEP_Count = 0;
				BlastLayer bl = RTC_Core.Blast(null, (string[])RTC_Unispec.RTCSpec[RTCSPEC.MEMORYDOMAINS_SELECTEDDOMAINS.ToString()]);
				if (bl != null)
					bl.Apply();
			}
		}

		public static void MAIN_BIZHAWK(string[] args)
		{
			//MessageBox.Show("ATTACH DEBUGGER NOW");

			if (!System.Environment.Is64BitOperatingSystem)
			{
				MessageBox.Show("32-bit operating system detected. Bizhawk requires 64-bit to run. Program will shut down");
				Application.Exit();
			}
			RTC_Unispec.RegisterEmuhawkSpec();
			
			
			RTC_Core.args = args;

			disableRTC = RTC_Core.args.Contains("-DISABLERTC");
			isRemoteRTC = RTC_Core.args.Contains("-REMOTERTC");

			//RTC_Unispec.RTCSpec.Update(Spec.HOOKS_SHOWCONSOLE.ToString(), RTC_Core.args.Contains("-CONSOLE"));
		}

		public static void MAINFORM_FORM_LOAD_END()
		{
			if (disableRTC) return;

			//RTC_Hooks.LOAD_GAME_DONE();

			RTC_Core.Start();

			RTC_Core.LoadDefaultRom();

			RTC_Params.LoadBizhawkWindowState();

			GlobalWin.MainForm.Focus();

			//Yell at the user if they're using audio throttle as it's buggy
			//We have to do this in the bizhawk process
			if (Global.Config.SoundThrottle)
			{
				MessageBox.Show("Sound throttle is buggy and can result in crashes.\nSwapping to clock throttle.");
				Global.Config.SoundThrottle = false;
				Global.Config.ClockThrottle = true;
				RTC_Hooks.BIZHAWK_SAVE_CONFIG();
			}

		}

		public static void MAINFORM_RESIZEEND()
		{
			if (disableRTC) return;

			RTC_Params.SaveBizhawkWindowState();
		}

		public static void MAINFORM_CLOSING()
		{
			if (disableRTC) return;

			RTC_Core.CloseAllRtcForms();
		}

		public static void BIZHAWK_SAVE_CONFIG()
		{
			if (disableRTC) return;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG));
		}

		public static void LOAD_GAME_BEGIN()
		{
			if (disableRTC) return;

			isNormalAdvance = false;

			loadGameToken = RTC_NetCore.HugeOperationStart();

			RTC_StepActions.ClearStepBlastUnits();
			CPU_STEP_Count = 0;
		}

		static string lastGameName = "";

		public static void LOAD_GAME_DONE()
		{
			if (disableRTC) return;

			//RTC_HellgenieEngine.ClearCheats();
			//RTC_PipeEngine.ClearPipes();

			//Glitch Harvester warning for archives

			string uppercaseFilename = GlobalWin.MainForm.CurrentlyOpenRom.ToUpper();
			if (S.GET<RTC_GlitchHarvester_Form>().Visible && (uppercaseFilename.Contains(".ZIP") || uppercaseFilename.Contains(".7Z")))
				MessageBox.Show($"The rom {RTC_Extensions.getShortFilenameFromPath(uppercaseFilename)} is in an archive and can't be added to a Stockpile");

			//Load Game vars into RTC_Core
			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];



			PartialSpec gameDone = new PartialSpec("EmuSpec");
			gameDone[EMUSPEC.STOCKPILE_CURRENTGAMESYSTEM.ToString()] = RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName);
			gameDone[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()] = PathManager.FilesystemSafeName(Global.Game);
			gameDone[EMUSPEC.CORE_LASTOPENROM.ToString()] = GlobalWin.MainForm.CurrentlyOpenRom;
			EmuSpec.Update(gameDone);
			
			//Sleep for 10ms in case Bizhawk hung for a moment after the game loaded
			System.Threading.Thread.Sleep(10);
			//prepare memory domains in advance on bizhawk side
			RTC_MemoryDomains.RefreshDomains(false);

			if (RTC_Unispec.EmuSpec[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()].ToString() != lastGameName)
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_EVENT_LOADGAMEDONE_NEWGAME));
			}
			else
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_EVENT_LOADGAMEDONE_SAMEGAME));
			}

			lastGameName = (string)RTC_Unispec.EmuSpec[EMUSPEC.STOCKPILE_CURRENTGAMENAME.ToString()];

			//RTC_Restore.SaveRestore();

			RTC_NetCore.HugeOperationEnd(loadGameToken);
		}

		public static void LOAD_GAME_FAILED()
		{
			if (disableRTC) return;

			RTC_NetCore.HugeOperationEnd(loadGameToken);
		}

		static bool CLOSE_GAME_loop_flag = false;

		public static bool AllowCaptureRewindState = true;

		public static void CLOSE_GAME(bool loadDefault = false)
		{
			if (disableRTC) return;

			if (CLOSE_GAME_loop_flag == true)
				return;

			CLOSE_GAME_loop_flag = true;

			//RTC_Core.AutoCorrupt = false;

			RTC_StepActions.ClearStepBlastUnits();

			if(!RTC_Hooks.isRemoteRTC)
				RTC_MemoryDomains.Clear();

			RTC_Unispec.EmuSpec.Update(EMUSPEC.CORE_LASTOPENROM.ToString(), null);

			if (loadDefault)
				RTC_Core.LoadDefaultRom();

			//RTC_RPC.SendToKillSwitch("UNFREEZE");

			CLOSE_GAME_loop_flag = false;
		}

		public static void RESET()
		{
			if (disableRTC) return;
		}

		public static void LOAD_SAVESTATE_BEGIN()
		{
			if (disableRTC) return;

			loadSavestateToken = RTC_NetCore.HugeOperationStart();
		}

		public static void LOAD_SAVESTATE_END()
		{
			if (disableRTC) return;
			

			RTC_NetCore.HugeOperationEnd(loadSavestateToken);
		}

		public static void EMU_CRASH(string msg)
		{
			if (disableRTC) return;

			RTC_RPC.Stop();
			MessageBox.Show("SORRY EMULATOR CRASHED\n\n" + msg);
		}

		public static bool HOTKEY_CHECK(string trigger)
		{// You can go to the injected Hotkey Hijack by searching #HotkeyHijack
			if (disableRTC) return false;

			if (watch != null)
			{
				long elapsedMs = watch.ElapsedMilliseconds;
				if (elapsedMs > 3000)
				{
					watch.Stop();
					watch = null;
				}
			}

			switch (trigger)
			{
				default:
					return false;

				case "Manual Blast":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_MANUALBLAST));
					break;

				case "Auto-Corrupt":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE));
					break;

				case "Error Delay--":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE));
					break;

				case "Error Delay++":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_ERRORDELAYINCREASE));
					break;

				case "Intensity--":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYDECREASE));
					break;

				case "Intensity++":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_INTENSITYINCREASE));
					break;

				case "GH Load and Corrupt":
					watch = System.Diagnostics.Stopwatch.StartNew();
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOADCORRUPT));
					break;

				case "GH Just Corrupt":
					watch = System.Diagnostics.Stopwatch.StartNew();
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHCORRUPT));
					break;

				case "GH Load":
					watch = System.Diagnostics.Stopwatch.StartNew();
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHLOAD));
					break;

				case "GH Save":
					watch = System.Diagnostics.Stopwatch.StartNew();
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSAVE));
					break;

				case "Stash->Stockpile":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE));
					break;

				case "Induce KS Crash":
					RTC_RPC.Stop();
					break;

				case "Blast+RawStash":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTRAWSTASH));
					break;

				case "Send Raw to Stash":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_SENDRAWSTASH));
					break;

				case "BlastLayer Toggle":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE));
					break;

				case "BlastLayer Re-Blast":
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST));
					break;
			}
			return true;
		}

		public static bool IsAllowedBackgroundInputForm(Form activeForm)
		{
			if (disableRTC) return false;

			return (activeForm is RTC.RTC_Core_Form ||
					activeForm is RTC.RTC_Settings_Form ||
					activeForm is RTC.RTC_GlitchHarvester_Form ||
					activeForm is RTC.RTC_StockpilePlayer_Form ||
					activeForm is RTC.RTC_Multiplayer_Form ||
					activeForm is RTC.RTC_MultiPeerPopout_Form ||
					activeForm is RTC.RTC_StockpileBlastBoard_Form ||
					activeForm is RTC.RTC_ConnectionStatus_Form ||
					activeForm is RTC.RTC_BlastEditor_Form ||
					activeForm is RTC.RTC_VmdPool_Form ||
					activeForm is RTC.RTC_VmdGen_Form ||
					activeForm is RTC.RTC_VmdAct_Form
					);
		}

		public static Bitmap BIZHAWK_GET_SCREENSHOT()
		{
			return GlobalWin.MainForm.MakeScreenshotImage().ToSysdrawingBitmap();
		}


		public static string BIZHAWK_GET_FILESYSTEMCORENAME()
		{
			//This returns the folder name of the currently loaded system core

			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];

			return pathEntry.SystemDisplayName;
		}

		public static string BIZHAWK_GET_FILESYSTEMGAMENAME()
		{
			//This returns the filename of the currently loaded game before extension

			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];

			return PathManager.FilesystemSafeName(Global.Game);
		}

		public static string BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME()
		{
			//returns the currently loaded core's name

			return Global.Game.System;
		}

		public static string BIZHAWK_GET_CURRENTLYOPENEDROM()
		{
			//This returns the filename of the currently opened rom

			return GlobalWin.MainForm.CurrentlyOpenRom;
		}

		public static bool BIZHAWK_ISNULLEMULATORCORE()
		{
			//This checks if the currently loaded emulator core is the Null emulator

			return Global.Emulator is NullEmulator;
		}

		public static bool BIZHAWK_ISMAINFORMVISIBLE()
		{
			return GlobalWin.MainForm.Visible;
		}

		public static void BIZHAWK_LOADROM(string RomFile)
		{
			var lra = new BizHawk.Client.EmuHawk.MainForm.LoadRomArgs { OpenAdvanced = new OpenAdvanced_OpenRom { Path = RomFile } };
			GlobalWin.MainForm.LoadRom(RomFile, lra);
		}

		public static void BIZHAWK_OPEN_HEXEDITOR_ADDRESS(MemoryDomainProxy mdp, long address)
		{
			GlobalWin.Tools.Load<HexEditor>();
			GlobalWin.Tools.HexEditor.SetDomain(mdp.MD);
			GlobalWin.Tools.HexEditor.GoToAddress(address);
		}

		public static Size BIZHAWK_GETSET_MAINFORMSIZE
		{
			get
			{
				return GlobalWin.MainForm.Size;
			}
			set
			{
				GlobalWin.MainForm.Size = value;
			}
		}

		public static Point BIZHAWK_GETSET_MAINFORMLOCATION
		{
			get
			{
				return GlobalWin.MainForm.Location;
			}
			set
			{
				GlobalWin.MainForm.Location = value;
			}
		}


		public static void BIZHAWK_STARTSOUND()
		{
			if (GlobalWin.MainForm != null) { GlobalWin.Sound.StartSound(); }
		}

		public static void BIZHAWK_STOPSOUND()
		{
			if (GlobalWin.MainForm != null) { GlobalWin.Sound.StopSound(); }
		}

		public static void BIZHAWK_MAINFORM_CLOSE()
		{
			GlobalWin.MainForm?.Close();
		}

		public static void BIZHAWK_MAINFORM_FOCUS()
		{
			GlobalWin.MainForm?.Focus();
		}

		public static void BIZHAWK_MAINFORM_SAVECONFIG()
		{
			GlobalWin.MainForm?.SaveConfig();
		}

		public static string BIZHAWK_GET_SAVESTATEPREFIX()
		{
			return PathManager.FilesystemSafeName(Global.Game);
		}

		public static void BIZHAWK_LOADSTATE(string path, string quickSlotName)
		{
			GlobalWin.MainForm.LoadState(path, quickSlotName, false);
		}

		public static void BIZHAWK_SAVESTATE(string path, string quickSlotName)
		{
			GlobalWin.MainForm.SaveState(path, quickSlotName, false);
		}

		public static void BIZHAWK_OSDMESSAGE(string message)
		{
			GlobalWin.OSD.AddMessage(message);
		}

		public static void BIZHAWK_MERGECONFIGINI(string backupConfigPath, string stockpileConfigPath)
		{
			Config bc;
			Config sc;

			FileInfo fileBc = new FileInfo(backupConfigPath);
			FileInfo fileSc = new FileInfo(stockpileConfigPath);

			using (StreamReader reader = fileBc.OpenText())
			{
				JsonSerializer serializer = new JsonSerializer();

				JsonTextReader r = new JsonTextReader(reader);
				bc = (Config)serializer.Deserialize(r, typeof(Config));
			}

			using (StreamReader reader = fileSc.OpenText())
			{
				JsonSerializer serializer = new JsonSerializer();
				JsonTextReader r = new JsonTextReader(reader);
				sc = (Config)serializer.Deserialize(r, typeof(Config));
			}

			//bc = (JObject)JsonConvert.DeserializeObject(backupConfig);
			//sc = (JObject)JsonConvert.DeserializeObject(stockpileConfig);

			sc.HotkeyBindings = bc.HotkeyBindings;
			sc.AllTrollers = bc.AllTrollers;
			sc.AllTrollersAutoFire = bc.AllTrollersAutoFire;
			sc.AllTrollersAnalog = bc.AllTrollersAnalog;

			if (File.Exists(stockpileConfigPath))
				File.Delete(stockpileConfigPath);

			try
			{
				using (StreamWriter writer = fileSc.CreateText())
				{
					JsonSerializer serializer = new JsonSerializer();
					JsonTextWriter w = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
					serializer.Serialize(w, sc);
				}
			}
			catch
			{
				/* Eat it */
			}
		}

		public static void BIZHAWK_IMPORTCONFIGINI(string importConfigPath, string stockpileConfigPath)
		{
			Config bc;
			Config sc;

			FileInfo fileBc = new FileInfo(importConfigPath);
			FileInfo fileSc = new FileInfo(stockpileConfigPath);

			JsonSerializer serializer = new JsonSerializer();

			using (StreamReader reader = fileBc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				bc = (Config)serializer.Deserialize(r, typeof(Config));
			}

			using (StreamReader reader = fileSc.OpenText())
			{
				JsonTextReader r = new JsonTextReader(reader);
				sc = (Config)serializer.Deserialize(r, typeof(Config));
			}

			//bc = (JObject)JsonConvert.DeserializeObject(backupConfig);
			//sc = (JObject)JsonConvert.DeserializeObject(stockpileConfig);

			sc.HotkeyBindings = bc.HotkeyBindings;
			sc.AllTrollers = bc.AllTrollers;
			sc.AllTrollersAutoFire = bc.AllTrollersAutoFire;
			sc.AllTrollersAnalog = bc.AllTrollersAnalog;

			if (File.Exists(stockpileConfigPath))
				File.Delete(stockpileConfigPath);

			try
			{
				using (StreamWriter writer = fileSc.CreateText())
				{
					JsonTextWriter w = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
					serializer.Serialize(w, sc);
				}
			}
			catch
			{
				/* Eat it */
			}
		}

		public static void BIZHAWK_SET_SYSTEMCORE(string systemName, string systemCore)
		{
			switch (systemName.ToUpper())
			{
				case "GAMEBOY":
					Global.Config.GB_AsSGB = systemCore == "sameboy";
					Global.Config.SGB_UseBsnes = false;
					Global.Config.GB_UseGBHawk = systemCore == "gbhawk";

					break;

				case "NES":
					Global.Config.NES_InQuickNES = systemCore == "quicknes";
					break;

				case "SNES":

					if (systemCore == "bsnes_SGB")
					{
						Global.Config.GB_AsSGB = true;
						Global.Config.SGB_UseBsnes = true;
					}
					else
						Global.Config.SNES_InSnes9x = systemCore == "snes9x";

					break;

				case "GBA":
					Global.Config.GBA_UsemGBA = systemCore == "mgba";
					break;

			}
		}

		public static string BIZHAWK_GET_SYSTEMCORENAME(string systemName)
		{
			try
			{

				SettingsAdapter settable = new SettingsAdapter(Global.Emulator);

				switch (systemName.ToUpper())
				{
					case "GAMEBOY":

						if (Global.Config.GB_AsSGB)
							return "sameboy";
						else if (Global.Config.GB_UseGBHawk)
							return "gbhawk";
						else
							return "gambatte";

					case "NES":
						return (Global.Config.NES_InQuickNES ? "quicknes" : "neshawk");

					case "SNES":

						if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("SGB WRAM"))
							return "bsnes_SGB";

						return (Global.Config.SNES_InSnes9x ? "snes9x" : "bsnes");

					case "GBA":
						return (Global.Config.GBA_UsemGBA ? "mgba" : "vba-next");

					case "N64":

						N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
											 ?? new N64SyncSettings();

						return $"{ss.VideoPlugin}/{ss.Rsp}/{ss.Core}/{(ss.DisableExpansionSlot ? "NoExp" : "Exp")}";
					default:
						break;
				}

				return systemName;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public static string BIZHAWK_GETSET_SYNCSETTINGS
		{
			get
			{
				SettingsAdapter settable = new SettingsAdapter(Global.Emulator);
				if (settable.HasSyncSettings)
				{
					string ss = ConfigService.SaveWithType(settable.GetSyncSettings());
					return ss;
				}
				return null;
			}
			set
			{
				SettingsAdapter settable = new SettingsAdapter(Global.Emulator);
				if (settable.HasSyncSettings)
				{
					settable.PutSyncSettings(ConfigService.LoadWithType(value));
				}
			}
		}

		public static void BIZHAWK_STARTRECORDAV(string videowritername, string filename, bool unattended)
		{
			GlobalWin.MainForm.RecordAvBase(videowritername, filename, unattended);
		}

		public static void BIZHAWK_STOPRECORDAV()
		{
			GlobalWin.MainForm.StopAv();
		}
	}
}

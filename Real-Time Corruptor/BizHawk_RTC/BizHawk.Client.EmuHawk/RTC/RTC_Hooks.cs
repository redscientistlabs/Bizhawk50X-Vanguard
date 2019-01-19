using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static CorruptCore.NetcoreCommands;

namespace RTC
{
	public static class RTC_Hooks
	{
		//Instead of writing code inside bizhawk, hooks are placed inside of it so will be easier
		//to upgrade BizHawk when they'll release a new version.

		// Here are the keywords for searching hooks and fixes: //RTC_HIJACK

		static bool disableRTC;
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
			if (isBeforeStep && CPU_STEP_Count != 0 && ((bool)(RTC_Corruptcore.CorruptCoreSpec?[RTCSPEC.STEP_RUNBEFORE.ToString()] ?? false)) == false)
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

			if (RTC_StepActions.ClearStepActionsOnRewind)
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

			bool autoCorrupt = RTC_Corruptcore.AutoCorrupt;
			int errorDelay = RTC_Corruptcore.ErrorDelay;
			if (autoCorrupt && CPU_STEP_Count >= errorDelay)
			{
				CPU_STEP_Count = 0;
				BlastLayer bl = RTC_Corruptcore.GenerateBlastLayer((string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"]);
				if (bl != null)
					bl.Apply(false);
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
			
			
			RTC_EmuCore.args = args;

			disableRTC = RTC_EmuCore.args.Contains("-DISABLERTC");
			//RTC_E.isStandaloneEmu = RTC_EmuCore.args.Contains("-REMOTERTC");

			//RTC_Unispec.RTCSpec.Update(Spec.HOOKS_SHOWCONSOLE.ToString(), RTC_Core.args.Contains("-CONSOLE"));
		}

		public static void MAINFORM_FORM_LOAD_END()
		{
			if (disableRTC) return;
			
			RTC_EmuCore.Start();
		}



		public static void MAINFORM_RESIZEEND()
		{
			if (disableRTC) return;

			RTC_EmuCore.SaveBizhawkWindowState();
		}

		public static void MAINFORM_CLOSING()
		{
			if (disableRTC) return;

			//Todo
			//RTC_UICore.CloseAllRtcForms();
		}


		public static void LOAD_GAME_BEGIN()
		{
			if (disableRTC) return;

			isNormalAdvance = false;
			
			RTC_StepActions.ClearStepBlastUnits();
			CPU_STEP_Count = 0;
		}

		static string lastGameName = "";

		public static void LOAD_GAME_DONE()
		{
			if (disableRTC) return;


			//Glitch Harvester warning for archives

			string uppercaseFilename = GlobalWin.MainForm.CurrentlyOpenRom.ToUpper();
			if (uppercaseFilename.Contains(".ZIP") || uppercaseFilename.Contains(".7Z"))
				MessageBox.Show($"The rom {RTC_Extensions.getShortFilenameFromPath(uppercaseFilename)} is in an archive and can't be added to a Stockpile");

			//Load Game vars into RTC_Core
			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];



			PartialSpec gameDone = new PartialSpec("EmuSpec");
			gameDone[VSPEC.SYSTEM.ToString()] =		  BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper();
			gameDone[VSPEC.GAMENAME.ToString()] =	  BIZHAWK_GET_FILESYSTEMGAMENAME();
			gameDone[VSPEC.SYSTEMPREFIX.ToString()] = BIZHAWK_GET_SAVESTATEPREFIX();
			gameDone[VSPEC.SYSTEMCORE.ToString()] =	  BIZHAWK_GET_SYSTEMCORENAME(Global.Game.System);
			gameDone[VSPEC.SYNCSETTINGS.ToString()] = BIZHAWK_GETSET_SYNCSETTINGS;
			gameDone[VSPEC.OPENROMFILENAME.ToString()] = GlobalWin.MainForm.CurrentlyOpenRom;
			gameDone[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS.ToString()] = RTC_EmuCore.GetBlacklistedDomains(BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper());
			RTC_EmuCore.EmuSpec.Update(gameDone);

			//RTC_MemoryDomains.RefreshDomains(false);

			//prepare memory domains in advance on bizhawk side

			RefreshDomains();

			if (RTC_EmuCore.GameName != lastGameName)
			{
			}
			else
			{
			}

			lastGameName = RTC_EmuCore.GameName;
		}

		public static void LOAD_GAME_FAILED()
		{
			if (disableRTC) return;

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

			RTC_MemoryDomains.Clear();

			RTC_EmuCore.OpenRomFilename = null;

			if (loadDefault)
				RTC_EmuCore.LoadDefaultRom();

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

		}

		public static void LOAD_SAVESTATE_END()
		{
			if (disableRTC) return;
			

		}

		public static void EMU_CRASH(string msg)
		{
			if (disableRTC) return;

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
					LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_HOTKEY_MANUALBLAST);
					break;

				case "Auto-Corrupt":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_AUTOCORRUPTTOGGLE);
					break;

				case "Error Delay--":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_ERRORDELAYDECREASE);
					break;

				case "Error Delay++":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_ERRORDELAYINCREASE);
					break;

				case "Intensity--":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_INTENSITYDECREASE);
					break;

				case "Intensity++":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_INTENSITYINCREASE);
					break;

				case "GH Load and Corrupt":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_GHLOADCORRUPT);
					break;

				case "GH Just Corrupt":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_GHCORRUPT);
					break;

				case "GH Load":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_GHLOAD);
					break;

				case "GH Save":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_GHSAVE);
					break;

				case "Stash->Stockpile":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_GHSTASHTOSTOCKPILE);
					break;

				case "Induce KS Crash":
					break;

				case "Blast+RawStash":
					var x = LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_BLASTRAWSTASH);
					break;

				case "Send Raw to Stash":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_SENDRAWSTASH);
					break;

				case "BlastLayer Toggle":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_BLASTLAYERTOGGLE);
					break;

				case "BlastLayer Re-Blast":
					LocalNetCoreRouter.Route(UI, REMOTE_HOTKEY_BLASTLAYERREBLAST);
					break;
			}
			return true;
		}

		public static bool IsAllowedBackgroundInputForm(Form activeForm)
		{
			if (disableRTC) return false;

			return false;

			//todo
			/*
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
					*/
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
			GlobalWin.Tools.HexEditor.SetDomain(((VanguardImplementation.BizhawkMemoryDomain)(mdp.MD)).MD);
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

		public static void BIZHAWK_LOADSTATE(string path)
		{
			GlobalWin.MainForm.LoadState(path, Path.GetFileName(path), false);
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

		public class MemoryDomainRTCInterface
		{
			[RequiredService]
			public IMemoryDomains MemoryDomains { get; set; }

			[RequiredService]
			private IEmulator Emulator { get; set; }
		}

		public static volatile MemoryDomainRTCInterface MDRI = new MemoryDomainRTCInterface();

		public static bool RefreshDomains()
		{
			if (Global.Emulator is NullEmulator)
				return false;

			//Compare the old to the new. If the name and sizes are all the same, don't push that there were changes.
			//We need to compare like this because the domains can change from syncsettings.
			//We only check name and size as those are the only things that can change on the fly
			var oldInterfaces = RTC_EmuCore.MemoryInterfacees;
			var newInterfaces = GetInterfaces();
			bool domainsChanged = false;

			if (oldInterfaces.Length != newInterfaces.Length)
				domainsChanged = true;

			for (int i = 0; i < oldInterfaces.Length; i++)
			{
				if (domainsChanged)
					break;
				if (oldInterfaces[i].Name != newInterfaces[i].Name)
					domainsChanged = true;
				if (oldInterfaces[i].Size != newInterfaces[i].Size)
					domainsChanged = true;
			}

			//We gotta push this no matter what since it's new underlying objects
			RTC_EmuCore.EmuSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES.ToString(), GetInterfaces());
			LocalNetCoreRouter.Route(CORRUPTCORE, REMOTE_EVENT_DOMAINSUPDATED, domainsChanged,true);
			return true;
		}

		public static MemoryDomainProxy[] GetInterfaces()
		{
			Console.WriteLine($" getInterfaces()");

			List<MemoryDomainProxy> interfaces = new List<MemoryDomainProxy>();

			if (Global.Emulator?.ServiceProvider == null)
				return null;

			ServiceInjector.UpdateServices(Global.Emulator.ServiceProvider, MDRI);

			foreach (MemoryDomain _domain in MDRI.MemoryDomains)
					interfaces.Add(new MemoryDomainProxy(new VanguardImplementation.BizhawkMemoryDomain(_domain)));


			return interfaces.ToArray();
		}


	}
}

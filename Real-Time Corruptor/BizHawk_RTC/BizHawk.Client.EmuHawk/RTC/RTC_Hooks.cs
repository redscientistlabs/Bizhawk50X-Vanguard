using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_Hooks
	{
		//Instead of writing code inside bizhawk, hooks are placed inside of it so will be easier
		//to upgrade BizHawk when they'll release a new version.

		// Here are the keywords for searching hooks and fixes: //RTC_HIJACK

		static bool DisableRTC;
		public static bool isRemoteRTC = false;
		public static bool isNormalAdvance = false;

		public static System.Diagnostics.Stopwatch watch = null;

		public static volatile bool BIZHAWK_ALLOWED_DOUBLECLICK_FULLSCREEN = true;

		static int CPU_STEP_Count = 0;

		public static void CPU_STEP(bool _isRewinding, bool _isFastForwarding, bool _isPaused)
		{
			if (DisableRTC || Global.Emulator is NullEmulator)
				return;

			isNormalAdvance = !(_isRewinding || _isFastForwarding || _isPaused);

			// Unique step hooks
			if (_isPaused)
				STEP_PAUSED();
			else if (!_isRewinding && !_isFastForwarding)
				STEP_FORWARD();
			else if (_isRewinding)
				STEP_REWIND();
			else if (_isFastForwarding)
				STEP_FASTFORWARD();

			//Any step hook for corruption
			STEP_CORRUPT(_isRewinding, _isFastForwarding, _isPaused);
		}

		private static void STEP_PAUSED()
		{
			if (DisableRTC) return;
		}

		private static void STEP_FORWARD()
		{
			if (DisableRTC) return;
		}

		private static void STEP_REWIND()
		{
			if (DisableRTC) return;

			if (RTC_Core.ClearCheatsOnRewind)
				RTC_HellgenieEngine.ClearCheats();

			if (RTC_Core.ClearPipesOnRewind)
				RTC_PipeEngine.ClearPipes();
		}

		private static void STEP_FASTFORWARD()
		{
			if (DisableRTC) return;
		}

		private static void STEP_CORRUPT(bool _isRewinding, bool _isFastForwarding, bool _isPaused)
		{
			if (DisableRTC) return;

			if (!_isRewinding && !_isPaused)
				if (RTC_PipeEngine.ProcessOnStep)
					RTC_PipeEngine.ExecutePipes();

			if (_isRewinding || _isFastForwarding || _isPaused)
				return;

			CPU_STEP_Count++;

			if (RTC_Core.AutoCorrupt && CPU_STEP_Count >= RTC_Core.ErrorDelay)
			{
				CPU_STEP_Count = 0;
				BlastLayer bl = RTC_Core.Blast(null, RTC_MemoryDomains.SelectedDomains);
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

			RTC_Core.args = args;

			DisableRTC = RTC_Core.args.Contains("-DISABLERTC");
			isRemoteRTC = RTC_Core.args.Contains("-REMOTERTC");
		}

		public static void MAINFORM_FORM_LOAD_END()
		{
			if (DisableRTC) return;

			//RTC_Hooks.LOAD_GAME_DONE();

			RTC_Core.Start();

			RTC_Core.LoadDefaultRom();

			RTC_Params.LoadBizhawkWindowState();

			GlobalWin.MainForm.Focus();
		}

		public static void MAINFORM_RESIZEEND()
		{
			if (DisableRTC) return;

			RTC_Params.SaveBizhawkWindowState();
		}

		public static void MAINFORM_CLOSING()
		{
			if (DisableRTC) return;

			RTC_Core.CloseAllRtcForms();
		}

		public static void BIZHAWK_SAVE_CONFIG()
		{
			if (DisableRTC) return;

			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG));
		}

		public static void LOAD_GAME_BEGIN()
		{
			if (DisableRTC) return;

			isNormalAdvance = false;

			RTC_NetCore.HugeOperationStart();

			RTC_HellgenieEngine.ClearCheats(true);
			RTC_PipeEngine.ClearPipes(true);
		}

		static string lastGameName = "";

		public static void LOAD_GAME_DONE()
		{
			if (DisableRTC) return;

			//RTC_HellgenieEngine.ClearCheats();
			//RTC_PipeEngine.ClearPipes();

			//Glitch Harvester warning for archives

			string uppercaseFilename = GlobalWin.MainForm.CurrentlyOpenRom.ToUpper();
			if (RTC_Core.ghForm.Visible && (uppercaseFilename.Contains(".ZIP") || uppercaseFilename.Contains(".7Z")))
				MessageBox.Show($"The rom {RTC_Extensions.getShortFilenameFromPath(uppercaseFilename)} is in an archive and can't be added to a Stockpile");

			//Load Game vars into RTC_Core
			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];

			RTC_StockpileManager.currentGameSystem = RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName);
			RTC_StockpileManager.currentGameName = PathManager.FilesystemSafeName(Global.Game);
			RTC_Core.lastOpenRom = GlobalWin.MainForm.CurrentlyOpenRom;
			RTC_RPC.RefreshPlugin();

			//Sleep for 10ms in case Bizhawk hung for a moment after the game loaded
			System.Threading.Thread.Sleep(10);
			//prepare memory domains in advance on bizhawk side
			RTC_MemoryDomains.RefreshDomains(false);

			if (RTC_StockpileManager.currentGameName != lastGameName)
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_EVENT_LOADGAMEDONE_NEWGAME));
			}
			else
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_EVENT_LOADGAMEDONE_SAMEGAME));
			}

			lastGameName = RTC_StockpileManager.currentGameName;

			//RTC_Restore.SaveRestore();

			RTC_NetCore.HugeOperationEnd();
		}

		public static void LOAD_GAME_FAILED()
		{
			if (DisableRTC) return;

			RTC_NetCore.HugeOperationEnd();
		}

		static bool CLOSE_GAME_loop_flag = false;

		public static bool AllowCaptureRewindState = true;

		public static void CLOSE_GAME(bool loadDefault = false)
		{
			if (DisableRTC) return;

			if (CLOSE_GAME_loop_flag == true)
				return;

			CLOSE_GAME_loop_flag = true;

			//RTC_Core.AutoCorrupt = false;

			RTC_PipeEngine.ClearPipes();
			RTC_MemoryDomains.Clear();

			RTC_Core.lastOpenRom = null;

			if (loadDefault)
				RTC_Core.LoadDefaultRom();

			//RTC_RPC.SendToKillSwitch("UNFREEZE");

			CLOSE_GAME_loop_flag = false;
		}

		public static void RESET()
		{
			if (DisableRTC) return;
		}

		public static void LOAD_SAVESTATE_BEGIN()
		{
			if (DisableRTC) return;

			RTC_NetCore.HugeOperationStart();
		}

		public static void LOAD_SAVESTATE_END()
		{
			if (DisableRTC) return;

			RTC_NetCore.HugeOperationEnd();
		}

		public static void EMU_CRASH(string msg)
		{
			if (DisableRTC) return;

			RTC_RPC.Stop();
			MessageBox.Show("SORRY EMULATOR CRASHED\n\n" + msg);
		}

		public static bool HOTKEY_CHECK(string trigger)
		{// You can go to the injected Hotkey Hijack by searching #HotkeyHijack
			if (DisableRTC) return false;

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

		public static bool IsAllowedBackgroundInputForm(Form ActiveForm)
		{
			if (DisableRTC) return false;

			return (ActiveForm is RTC.RTC_Core_Form ||
					ActiveForm is RTC.RTC_Settings_Form ||
					ActiveForm is RTC.RTC_GlitchHarvester_Form ||
					ActiveForm is RTC.RTC_StockpilePlayer_Form ||
					ActiveForm is RTC.RTC_Multiplayer_Form ||
					ActiveForm is RTC.RTC_MultiPeerPopout_Form ||
					ActiveForm is RTC.RTC_StockpileBlastBoard_Form ||
					ActiveForm is RTC.RTC_ConnectionStatus_Form ||
					ActiveForm is RTC.RTC_NewBlastEditor_Form ||
					ActiveForm is RTC.RTC_VmdPool_Form ||
					ActiveForm is RTC.RTC_VmdGen_Form ||
					ActiveForm is RTC.RTC_VmdAct_Form
					);
		}
	}
}

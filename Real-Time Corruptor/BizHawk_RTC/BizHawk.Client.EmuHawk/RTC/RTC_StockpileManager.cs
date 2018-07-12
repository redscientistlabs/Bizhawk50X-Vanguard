using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_StockpileManager
	{
		//Object references
		public static Stockpile currentStockpile = null;

		public static StashKey currentStashkey { get; set; } = null;

		public static StashKey backupedState = null;
		public static Stack<StashKey> allBackupStates = new Stack<StashKey>();
		public static BlastLayer lastBlastLayerBackup = null;
		public static string currentGameSystem = "";
		public static string currentGameName = "";

		public static bool unsavedEdits = false;

		private static bool _isCorruptionApplied = false;

		public static bool isCorruptionApplied
		{
			get
			{
				return _isCorruptionApplied;
			}
			set
			{
				_isCorruptionApplied = value;

				RTC_Core.ghForm.IsCorruptionApplied = value;
			}
		}

		public static bool stashAfterOperation = true;

		public static volatile List<StashKey> StashHistory = new List<StashKey>();

		// key: some key or guid, value: [0] savestate key [1] rom file
		public static volatile Dictionary<string, StashKey> SavestateStashkeyDico = new Dictionary<string, StashKey>();

		public static volatile string currentSavestateKey = null;
		public static bool renderAtLoad = false;

		private static void PreApplyStashkey()
		{
			RTC_StepActions.ClearStepBlastUnits();
		}

		private static void PostApplyStashkey()
		{
			if (renderAtLoad)
			{
				RTC_Render.StartRender();
			}
		}

		public static StashKey getCurrentSavestateStashkey()
		{
			if (currentSavestateKey == null || !SavestateStashkeyDico.ContainsKey(currentSavestateKey))
				return null;

			return SavestateStashkeyDico[currentSavestateKey];
		}

		public static bool ApplyStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			var token = RTC_NetCore.HugeOperationStart("LAZY");

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(sk))
				{
					RTC_NetCore.HugeOperationEnd(token);
					return isCorruptionApplied;
				}
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true });

			RTC_NetCore.HugeOperationEnd(token);

			isCorruptionApplied = (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0);

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			var token = RTC_NetCore.HugeOperationStart("LAZY");

			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null)
			{
				RTC_Core.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_Core.StartSound();
				RTC_NetCore.HugeOperationEnd(token);
				return false;
			}

			string currentGame = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			if (currentGame == null || psk.GameName != currentGame) 
			{
				RTC_Core.LoadRom(psk.RomFilename, true);
				RTC_Core.ecForm.RefreshDomains();
				RTC_Core.ecForm.SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();
			BlastLayer bl = (BlastLayer)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { objectValue = RTC_MemoryDomains.SelectedDomains }, true);
			watch.Stop();
			Console.WriteLine($"It took " + watch.ElapsedMilliseconds + " ms to blastlayer");

			currentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, bl);
			currentStashkey.RomFilename = psk.RomFilename;
			currentStashkey.SystemName = psk.SystemName;
			currentStashkey.SystemCore = psk.SystemCore;
			currentStashkey.GameName = psk.GameName;

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(currentStashkey))
				{
					RTC_NetCore.HugeOperationEnd(token);
					return isCorruptionApplied;
				}
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = bl });

			isCorruptionApplied = (bl != null);

			if (stashAfterOperation && bl != null)
			{
				StashHistory.Add(currentStashkey);
				RTC_Core.ghForm.RefreshStashHistory();
				RTC_Core.ghForm.dgvStockpile.ClearSelection();

				RTC_Core.ghForm.lbStashHistory.ClearSelected();

				RTC_Core.ghForm.DontLoadSelectedStash = true;
				RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;
			}

			RTC_NetCore.HugeOperationEnd(token);

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null)
			{
				RTC_Core.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_Core.StartSound();
				return false;
			}

			if (psk.SystemCore != sk.SystemCore && !RTC_Core.AllowCrossCoreCorruption)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return isCorruptionApplied;
			}

			currentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, sk.BlastLayer);
			currentStashkey.RomFilename = psk.RomFilename;
			currentStashkey.SystemName = psk.SystemName;
			currentStashkey.SystemCore = psk.SystemCore;
			currentStashkey.GameName = psk.GameName;

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(currentStashkey))
					return false;
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer }, true);

			isCorruptionApplied = (sk.BlastLayer != null);

			if (stashAfterOperation)
			{
				StashHistory.Add(currentStashkey);
				RTC_Core.ghForm.RefreshStashHistory();
			}

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool OriginalFromStashkey(StashKey sk)
		{
			PreApplyStashkey();

			if (sk == null)
			{
				MessageBox.Show("No StashKey could be loaded");
				return false;
			}

			if (!LoadState(sk, true, false))
				return isCorruptionApplied;

			isCorruptionApplied = false;

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool MergeStashkeys(List<StashKey> sks, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			if (sks != null && sks.Count > 1)
			{
				var token = RTC_NetCore.HugeOperationStart();

				StashKey master = sks[0];

				string masterSystemCore = master.SystemCore;
				bool allCoresIdentical = true;

				foreach (StashKey item in sks)
					if (item.SystemCore != master.SystemCore)
					{
						allCoresIdentical = false;
						break;
					}

				if (!allCoresIdentical && !RTC_Core.AllowCrossCoreCorruption)
				{
					MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));
					RTC_NetCore.HugeOperationEnd(token);

					return false;
				}

				foreach (StashKey item in sks)
					if (item.GameName != master.GameName)
					{
						MessageBox.Show("Merge attempt failed: game mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));
						RTC_NetCore.HugeOperationEnd(token);

						return false;
					}



				BlastLayer bl = new BlastLayer();

				foreach (StashKey item in sks)
					bl.Layer.AddRange(item.BlastLayer.Layer);

				bl.Layer = bl.Layer.Distinct().ToList();

				currentStashkey = new StashKey(RTC_Core.GetRandomKey(), master.ParentKey, bl);
				currentStashkey.RomFilename = master.RomFilename;
				currentStashkey.SystemName = master.SystemName;
				currentStashkey.SystemCore = master.SystemCore;
				currentStashkey.GameName = master.GameName;
				currentStashkey.SyncSettings = master.SyncSettings;

				//RTC_NetCore.HugeOperationEnd(token);
				//  token = RTC_NetCore.HugeOperationStart("LAZY");

				if (_loadBeforeOperation)
				{
					if (!LoadStateAndBlastLayer(currentStashkey))
					{
						RTC_NetCore.HugeOperationEnd(token);
						return isCorruptionApplied;
					}
				}
				else
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = currentStashkey.BlastLayer });

				isCorruptionApplied = (currentStashkey.BlastLayer != null && currentStashkey.BlastLayer.Layer.Count > 0);

				if (stashAfterOperation)
				{
					StashHistory.Add(currentStashkey);
					RTC_Core.ghForm.RefreshStashHistory();
				}

				RTC_NetCore.HugeOperationEnd(token);

				PostApplyStashkey();
				return true;
			}
			else
			{
				MessageBox.Show("You need 2 or more items for Merging");
				return false;
			}
		}

		public static bool LoadState(StashKey sk, bool ReloadRom = true, bool applyBlastLayer = true)
		{
			return (bool)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, ReloadRom, false, applyBlastLayer }
			}, true);
		}

		public static bool LoadStateAndBlastLayer(StashKey sk, bool ReloadRom = true)
		{
			object returnValue = RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, ReloadRom, (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0) }
			}, true);

			return (returnValue != null ? (bool)returnValue : false);
		}

		public static bool LoadState_NET(StashKey sk, bool ReloadRom = true)
		{
			var loadStateWatch = System.Diagnostics.Stopwatch.StartNew();
			if (sk == null)
				return false;

			StashKey.SetCore(sk);
			string GameSystem = sk.SystemName;
			string GameName = sk.GameName;
			string Key = sk.ParentKey;
			bool GameHasChanged = false;
			string TheoricalSaveStateFilename;

			RTC_Core.StopSound();

			if (ReloadRom)
			{
				string ss = null;
				RTC_Core.LoadRom_NET(sk.RomFilename);

				var ssWatch = System.Diagnostics.Stopwatch.StartNew();
				ss = StashKey.getSyncSettings_NET(ss);
				ssWatch.Stop();
				Console.WriteLine($"Time taken to get the SyncSettings: {0}ms", ssWatch.ElapsedMilliseconds);

				//If the syncsettings are different, update them and load it again. Otheriwse, leave as is
				if (sk.SyncSettings != ss && sk.SyncSettings != null)
				{
					StashKey.putSyncSettings_NET(sk.SyncSettings);
					RTC_Core.LoadRom_NET(sk.RomFilename);
				}
				GameHasChanged = true;
			}

			RTC_Core.StartSound();

			PathEntry pathEntry = Global.Config.PathEntries[Global.Game.System, "Savestates"] ??
			Global.Config.PathEntries[Global.Game.System, "Base"];

			if (!GameHasChanged)
				TheoricalSaveStateFilename = RTC_Core.bizhawkDir + "\\" + GameSystem + "\\State\\" + GameName + "." + Key + ".timejump.State";
			else
				TheoricalSaveStateFilename = RTC_Core.bizhawkDir + "\\" + RTC_Core.EmuFolderCheck(pathEntry.SystemDisplayName) + "\\State\\" + PathManager.FilesystemSafeName(Global.Game) + "." + Key + ".timejump.State";

			if (File.Exists(TheoricalSaveStateFilename))
			{
				if (!RTC_Core.LoadSavestate_NET(Key))
				{
					RTC_Core.StopSound();
					MessageBox.Show($"Error loading savestate : An internal Bizhawk error has occurred.\n Are you sure your savestate matches the game, your syncsettings match, and the savestate is supported by this version of Bizhawk?");
					RTC_Core.StartSound();
					return false;
				}
			}
			else
			{
				RTC_Core.StopSound();
				MessageBox.Show($"Error loading savestate : (File {Key + ".timejump"} not found)");
				RTC_Core.StartSound();
				return false;
			}

			loadStateWatch.Stop();
			Console.WriteLine($"Time taken for LoadState_NET: {0}ms", loadStateWatch.ElapsedMilliseconds);
			return true;
		}

		public static StashKey SaveState(bool SendToStashDico, StashKey _sk = null, bool sync = true)
		{
			return (StashKey)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SAVESTATE) { objectValue = new object[] { SendToStashDico, _sk } }, sync);
		}

		public static StashKey SaveState_NET(bool SendToStashDico, StashKey _sk = null, bool threadSave = false)
		{
			string Key = RTC_Core.GetRandomKey();
			string statePath;

			StashKey sk;

			if (_sk == null)
			{
				Key = RTC_Core.GetRandomKey();
				statePath = RTC_Core.SaveSavestate_NET(Key, threadSave);
				sk = new StashKey(Key, Key, null);
			}
			else
			{
				Key = _sk.Key;
				statePath = _sk.StateFilename;
				sk = _sk;
			}

			if (statePath == null)
				return null;

			sk.StateShortFilename = statePath.Substring(statePath.LastIndexOf("\\") + 1, statePath.Length - (statePath.LastIndexOf("\\") + 1));
			sk.StateFilename = statePath;

			if (SendToStashDico)
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_KEY_PUSHSAVESTATEDICO) { objectValue = new object[] { sk, currentSavestateKey } });

				if (RTC_Hooks.isRemoteRTC)
					RTC_StockpileManager.SavestateStashkeyDico[currentSavestateKey] = sk;
			}

			return sk;
		}

		public static bool ChangeGameWarning(string rom, bool dontask = false)
		{
			if (Global.Emulator is NullEmulator || GlobalWin.MainForm.CurrentlyOpenRom.ToString().Contains("default.nes") || dontask)
				return true;

			if (rom == null)
				return false;

			string currentFilename = GlobalWin.MainForm.CurrentlyOpenRom.ToString();

			if (currentFilename.IndexOf("\\") != -1)
				currentFilename = currentFilename.Substring(currentFilename.LastIndexOf("\\") + 1);

			string btnFilename = rom;

			if (btnFilename.IndexOf("\\") != -1)
				btnFilename = btnFilename.Substring(btnFilename.LastIndexOf("\\") + 1);

			if (btnFilename != currentFilename)
			{
				string cctext =
					"Loading this savestate will change the game\n" +
					"\n" +
										"Current Rom: " + currentFilename + "\n" +
					"Target Rom: " + rom + "\n" +
					"\n" +
					"Do you wish to continue ? (Yes/No)";

				if (MessageBox.Show(cctext, "Switching to another game", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					return false;
			}

			return true;
		}

		public static void StockpileChanged()
		{
			RTC_Core.sbForm.RefreshButtons();
		}

		public static StashKey getRawBlastlayer()
		{
			RTC_Core.StopSound();

			StashKey sk = RTC_StockpileManager.SaveState_NET(false);

			BlastLayer bl = new BlastLayer();


			bl.Layer.AddRange(RTC_StepActions.GetRawBlastLayer().Layer);

			string thisSystem = Global.Game.System;
			string romFilename = GlobalWin.MainForm.CurrentlyOpenRom;

			var rp = RTC_MemoryDomains.GetRomParts(thisSystem, romFilename);

			if (rp.Error == null)
			{
				if (rp.PrimaryDomain != null)
				{
					List<byte> addData = new List<byte>();

					if (rp.SkipBytes != 0)
					{
						byte[] padding = new byte[rp.SkipBytes];
						for (int i = 0; i < rp.SkipBytes; i++)
							padding[i] = 0;

						addData.AddRange(padding);
					}

					addData.AddRange(RTC_MemoryDomains.GetDomainData(rp.PrimaryDomain));
					if (rp.SecondDomain != null)
						addData.AddRange(RTC_MemoryDomains.GetDomainData(rp.SecondDomain));

					byte[] corrupted = addData.ToArray();
					byte[] original = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("32X FB")) //Flip 16-bit words on 32X rom
						original = original.FlipWords(2);
					else if (thisSystem.ToUpper() == "N64")
						original = BizHawk.Client.Common.RomGame.MutateSwapN64(original);
					else if (GlobalWin.MainForm.CurrentlyOpenRom.ToUpper().Contains(".SMD"))
						original = BizHawk.Client.Common.RomGame.DeInterleaveSMD(original);

					for (int i = 0; i < rp.SkipBytes; i++)
						original[i] = 0;

					BlastLayer romBlast = RTC_BlastTools.GetBlastLayerFromDiff(original, corrupted);

					if (romBlast != null && romBlast.Layer.Count > 0)
						bl.Layer.AddRange(romBlast.Layer);
				}
			}

			sk.BlastLayer = bl;
			RTC_Core.StartSound();

			return sk;
		}

		public static void AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			isCorruptionApplied = (currentStashkey.BlastLayer != null && currentStashkey.BlastLayer.Layer.Count > 0);

			if (stashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(currentStashkey);
				RTC_Core.ghForm.RefreshStashHistory();
			}
		}
	}
}

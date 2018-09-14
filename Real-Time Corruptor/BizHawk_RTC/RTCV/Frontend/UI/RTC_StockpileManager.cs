using RTCV.CorruptCore;
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

				S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = value;
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

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(sk))
				{
					return isCorruptionApplied;
				}
			}
			else
				NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true });

			isCorruptionApplied = (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0);

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null)
			{
				RTC_EmuCore.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_EmuCore.StartSound();
				return false;
			}

			string currentGame = (string)NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			if (currentGame == null || psk.GameName != currentGame) 
			{
				RTC_UICore.LoadRom(psk.RomFilename, true);
				S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
				S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();
			BlastLayer bl = (BlastLayer)NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { objectValue = RTC_MemoryDomains.SelectedDomains }, true);
			watch.Stop();
			Console.WriteLine($"It took " + watch.ElapsedMilliseconds + " ms to blastlayer");

			currentStashkey = new StashKey(RTC_CorruptCore.GetRandomKey(), psk.ParentKey, bl);
			currentStashkey.RomFilename = psk.RomFilename;
			currentStashkey.SystemName = psk.SystemName;
			currentStashkey.SystemCore = psk.SystemCore;
			currentStashkey.GameName = psk.GameName;

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(currentStashkey))
				{
					return isCorruptionApplied;
				}
			}
			else
				NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = bl });

			isCorruptionApplied = (bl != null);

			if (stashAfterOperation && bl != null)
			{
				StashHistory.Add(currentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();

				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

				S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
			}

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null)
			{
				RTC_EmuCore.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_EmuCore.StartSound();
				return false;
			}

			if (psk.SystemCore != sk.SystemCore && !RTC_EmuCore.AllowCrossCoreCorruption)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return isCorruptionApplied;
			}

			currentStashkey = new StashKey(RTC_CorruptCore.GetRandomKey(), psk.ParentKey, sk.BlastLayer);
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
				NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer }, true);

			isCorruptionApplied = (sk.BlastLayer != null);

			if (stashAfterOperation)
			{
				StashHistory.Add(currentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
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
				StashKey master = sks[0];

				string masterSystemCore = master.SystemCore;
				bool allCoresIdentical = true;

				foreach (StashKey item in sks)
					if (item.SystemCore != master.SystemCore)
					{
						allCoresIdentical = false;
						break;
					}

				if (!allCoresIdentical && !RTC_EmuCore.AllowCrossCoreCorruption)
				{
					MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));

					return false;
				}

				foreach (StashKey item in sks)
					if (item.GameName != master.GameName)
					{
						MessageBox.Show("Merge attempt failed: game mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));

						return false;
					}



				BlastLayer bl = new BlastLayer();

				foreach (StashKey item in sks)
					bl.Layer.AddRange(item.BlastLayer.Layer);

				bl.Layer = bl.Layer.Distinct().ToList();

				currentStashkey = new StashKey(RTC_CorruptCore.GetRandomKey(), master.ParentKey, bl);
				currentStashkey.RomFilename = master.RomFilename;
				currentStashkey.SystemName = master.SystemName;
				currentStashkey.SystemCore = master.SystemCore;
				currentStashkey.GameName = master.GameName;
				currentStashkey.SyncSettings = master.SyncSettings;

				if (_loadBeforeOperation)
				{
					if (!LoadStateAndBlastLayer(currentStashkey))
					{
						return isCorruptionApplied;
					}
				}
				else
					NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = currentStashkey.BlastLayer });

				isCorruptionApplied = (currentStashkey.BlastLayer != null && currentStashkey.BlastLayer.Layer.Count > 0);

				if (stashAfterOperation)
				{
					StashHistory.Add(currentStashkey);
					S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				}

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
			object returnValue = (bool)NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, ReloadRom, false, applyBlastLayer }
			}, true);

			return (returnValue != null ? (bool)returnValue : false);
		}

		public static bool LoadStateAndBlastLayer(StashKey sk, bool ReloadRom = true)
		{
			object returnValue = NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
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
			StashKeySavestateLocation StateLocation = sk.StateLocation;

			string TheoricalSaveStateFilename;

			RTC_EmuCore.StopSound();

			if (ReloadRom)
			{
				RTC_EmuCore.LoadRom_NET(sk.RomFilename);

				var ssWatch = System.Diagnostics.Stopwatch.StartNew();
				string ss = RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS;
				ssWatch.Stop();
				Console.WriteLine($"Time taken to get the SyncSettings: {0}ms", ssWatch.ElapsedMilliseconds);

				//If the syncsettings are different, update them and load it again. Otheriwse, leave as is
				if (sk.SyncSettings != ss && sk.SyncSettings != null)
				{
					RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS = sk.SyncSettings;
					RTC_EmuCore.LoadRom_NET(sk.RomFilename);
				}
			}

			RTC_EmuCore.StartSound();

			TheoricalSaveStateFilename = RTC_EmuCore.workingDir + "\\" + StateLocation.ToString() + "\\" + GameName + "." + Key + ".timejump.State";

			if (File.Exists(TheoricalSaveStateFilename))
			{
				if (!RTC_EmuCore.LoadSavestate_NET(Key))
				{
					RTC_EmuCore.StopSound();
					MessageBox.Show($"Error loading savestate : An internal Bizhawk error has occurred.\n Are you sure your savestate matches the game, your syncsettings match, and the savestate is supported by this version of Bizhawk?");
					RTC_EmuCore.StartSound();
					return false;
				}
			}
			else
			{
				RTC_EmuCore.StopSound();
				MessageBox.Show($"Error loading savestate : (File {Key + ".timejump"} not found)");
				RTC_EmuCore.StartSound();
				return false;
			}

			loadStateWatch.Stop();
			Console.WriteLine($"Time taken for LoadState_NET: {0}ms", loadStateWatch.ElapsedMilliseconds);
			return true;
		}

		public static StashKey SaveState(bool SendToStashDico, StashKey _sk = null, bool sync = true)
		{
			return (StashKey)NetCoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SAVESTATE) { objectValue = new object[] { SendToStashDico, _sk } }, sync);
		}

		public static StashKey SaveState_NET(bool SendToStashDico, StashKey _sk = null, bool threadSave = false)
		{
			string Key = RTC_CorruptCore.GetRandomKey();
			string statePath;

			StashKey sk;

			if (_sk == null)
			{
				Key = RTC_CorruptCore.GetRandomKey();
				statePath = RTC_EmuCore.SaveSavestate_NET(Key, threadSave);
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
				NetCoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_KEY_PUSHSAVESTATEDICO) { objectValue = new object[] { sk, currentSavestateKey } });

				if (NetCoreImplementation.isStandaloneEmu)
					RTC_StockpileManager.SavestateStashkeyDico[currentSavestateKey] = sk;
			}

			return sk;
		}

		public static bool ChangeGameWarning(string rom, bool dontask = false)
		{
			if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE() || RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM().Contains("default.nes") || dontask)
				return true;

			if (rom == null)
				return false;

			string currentFilename = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM();

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
			S.GET<RTC_StockpileBlastBoard_Form>().RefreshButtons();
		}

		public static StashKey getRawBlastlayer()
		{
			RTC_EmuCore.StopSound();

			StashKey sk = RTC_StockpileManager.SaveState_NET(false);

			BlastLayer bl = new BlastLayer();


			bl.Layer.AddRange(RTC_StepActions.GetRawBlastLayer().Layer);

			string thisSystem = RTC_Hooks.BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME();
			string romFilename = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM();

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
					byte[] original = File.ReadAllBytes(romFilename);

					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("32X FB")) //Flip 16-bit words on 32X rom
						original = original.FlipWords(2);
					else if (thisSystem.ToUpper() == "N64")
						original = BizHawk.Client.Common.RomGame.MutateSwapN64(original);
					else if (romFilename.ToUpper().Contains(".SMD"))
						original = BizHawk.Client.Common.RomGame.DeInterleaveSMD(original);

					for (int i = 0; i < rp.SkipBytes; i++)
						original[i] = 0;

					BlastLayer romBlast = RTC_BlastTools.GetBlastLayerFromDiff(original, corrupted);

					if (romBlast != null && romBlast.Layer.Count > 0)
						bl.Layer.AddRange(romBlast.Layer);
				}
			}

			sk.BlastLayer = bl;
			RTC_EmuCore.StartSound();

			return sk;
		}

		public static void AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			isCorruptionApplied = (currentStashkey.BlastLayer != null && currentStashkey.BlastLayer.Layer.Count > 0);

			if (stashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(currentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			}
		}
	}
}

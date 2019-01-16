using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_StockpileManager_UISide
	{
		//Object references
		public static Stockpile CurrentStockpile { get; set; }

		public static StashKey CurrentStashkey { get; set; }

		public static Stack<StashKey> AllBackupStates { get; set; } = new Stack<StashKey>();


		public static string CurrentSavestateKey;
		public static StashKey BackupedState;

		public static bool StashAfterOperation = true;

		public static volatile List<StashKey> StashHistory = new List<StashKey>();

		// key: some key or guid, value: [0] savestate key [1] rom file
		public static volatile Dictionary<string, StashKey> SavestateStashkeyDico = new Dictionary<string, StashKey>();



		private static void PreApplyStashkey()
		{
			RTC_StepActions.ClearStepBlastUnits();
		}

		private static void PostApplyStashkey()
		{
			if (RTC_StockpileManager_EmuSide.RenderAtLoad)
			{
				RTC_Render_CorruptCore.StartRender();
			}
		}

		public static StashKey GetCurrentSavestateStashkey()
		{
			if (CurrentSavestateKey == null || !SavestateStashkeyDico.ContainsKey(CurrentSavestateKey))
				return null;

			return SavestateStashkeyDico[CurrentSavestateKey];
		}

		public static bool ApplyStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			bool isCorruptionApplied = sk?.BlastLayer?.Layer?.Count > 0;

			if (_loadBeforeOperation)
			{
				if (!LoadState(sk))
				{
					return isCorruptionApplied;
				}
			}
			else
			{
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] {CurrentStashkey.BlastLayer, true}, true);
			}

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();


			StashKey psk = RTC_StockpileManager_UISide.GetCurrentSavestateStashkey();

			if (psk == null)
			{
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				return false;
			}

			string currentGame = (string)RTC_Corruptcore.VanguardSpec[VSPEC.GAMENAME.ToString()];
			if (currentGame == null || psk.GameName != currentGame) 
			{
				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, psk.RomFilename, true);
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();
			BlastLayer bl = LocalNetCoreRouter.QueryRoute<BlastLayer>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.GENERATEBLASTLAYER, (string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"], true);
			watch.Stop();
			Console.WriteLine($"It took " + watch.ElapsedMilliseconds + " ms to blastlayer");

			CurrentStashkey = new StashKey(RTC_Corruptcore.GetRandomKey(), psk.ParentKey, bl)
			{
				RomFilename = psk.RomFilename,
				SystemName = psk.SystemName,
				SystemCore = psk.SystemCore,
				GameName = psk.GameName,
				SyncSettings = psk.SyncSettings,
				StateLocation = psk.StateLocation
			};

			bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

			if (_loadBeforeOperation)
			{
				if (!LoadState(CurrentStashkey, true, true))
				{
					return isCorruptionApplied;
				}
			}
			else
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);


			if (StashAfterOperation && bl != null)
			{
				StashHistory.Add(CurrentStashkey);

				//todo
				/*
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();

				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

				S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
				*/
			}


			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager_UISide.GetCurrentSavestateStashkey();

			if (psk == null)
			{
				MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				return false;
			}

			if (psk.SystemCore != sk.SystemCore && !RTC_Corruptcore.AllowCrossCoreCorruption)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return false;
			}

			CurrentStashkey = new StashKey(RTC_Corruptcore.GetRandomKey(), psk.ParentKey, sk.BlastLayer)
			{
				RomFilename = psk.RomFilename,
				SystemName = psk.SystemName,
				SystemCore = psk.SystemCore,
				GameName = psk.GameName,
				SyncSettings = psk.SyncSettings,
				StateLocation = psk.StateLocation
			};

			if (_loadBeforeOperation)
			{
				if (!LoadState(CurrentStashkey))
					return false;
			}
			else
				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);

			bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

			if (StashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				//todo
				//S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
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

			bool isCorruptionApplied = false;

			if (!LoadState(sk, true, false))
				return isCorruptionApplied;

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

				if (!allCoresIdentical && !RTC_Corruptcore.AllowCrossCoreCorruption)
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

				CurrentStashkey = new StashKey(RTC_Corruptcore.GetRandomKey(), master.ParentKey, bl)
				{
					RomFilename = master.RomFilename,
					SystemName = master.SystemName,
					SystemCore = master.SystemCore,
					GameName = master.GameName,
					SyncSettings = master.SyncSettings,
					StateLocation = master.StateLocation
				};


				bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

				if (_loadBeforeOperation)
				{
					if (!LoadState(CurrentStashkey))
					{
						return isCorruptionApplied;
					}
				}
				else
				{
					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.APPLYBLASTLAYER, new object[] { CurrentStashkey.BlastLayer, true }, true);
				}


				if (StashAfterOperation)
				{
					StashHistory.Add(CurrentStashkey);
				}


				PostApplyStashkey();
				return true;
			}
			MessageBox.Show("You need 2 or more items for Merging");
			return false;
		}

		public static bool LoadState(StashKey sk, bool reloadRom = true, bool applyBlastLayer = true)
		{
			LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_LOADSTATE, new object[] {sk, true, applyBlastLayer }, true);
			return true;
		}

		public static StashKey SaveState(bool sendToStashDico, StashKey sk = null, bool threadSave = false)
		{
			StashKey _sk = LocalNetCoreRouter.QueryRoute<StashKey>(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_SAVESTATE, sk, true);
			if (sendToStashDico)
			{
				var currentkey = CurrentSavestateKey;
				if (currentkey != null)
					SavestateStashkeyDico[currentkey] = _sk;
			}

			return _sk;
		}


		public static void StockpileChanged()
		{
			//S.GET<RTC_StockpileBlastBoard_Form>().RefreshButtons();
		}


		public static bool AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			bool isCorruptionApplied = CurrentStashkey?.BlastLayer?.Layer?.Count > 0;

			if (StashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				//todo
				//S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			}

			return isCorruptionApplied;
		}

	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_StockpileManager
	{
		//Object references
		public static Stockpile CurrentStockpile { get; set; }

		public static StashKey CurrentStashkey { get; set; }

		public static Stack<StashKey> AllBackupStates { get; set; } = new Stack<StashKey>();
		public static BlastLayer LastBlastLayerBackup { get; set; } = null;


		private static bool isCorruptionApplied = false;

		public static bool IsCorruptionApplied
		{
			get
			{
				return isCorruptionApplied;
			}
			set
			{
				isCorruptionApplied = value;

			//	S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = value;
			}
		}

		public static string CurrentSavestateKey
		{
			get => (string)RTC_Corruptcore.CorruptCoreSpec[RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString()];
			set => RTC_Corruptcore.CorruptCoreSpec.Update(RTCSPEC.CORE_EXTRACTBLASTLAYER.ToString(), value);
		}

		public static StashKey BackupedState
		{
			get => (StashKey)RTC_Corruptcore.CorruptCoreSpec[RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString()];
			set => RTC_Corruptcore.CorruptCoreSpec.Update(RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()] = null;
			partial[RTCSPEC.STOCKPILE_BACKUPEDSTATE.ToString()] = null;

			return partial;
		}

		public static bool StashAfterOperation = true;

		public static volatile List<StashKey> StashHistory = new List<StashKey>();

		// key: some key or guid, value: [0] savestate key [1] rom file
		public static volatile Dictionary<string, StashKey> SavestateStashkeyDico = new Dictionary<string, StashKey>();

		public static bool RenderAtLoad = false;

		private static void PreApplyStashkey()
		{
			RTC_StepActions.ClearStepBlastUnits();
		}

		private static void PostApplyStashkey()
		{
			if (RenderAtLoad)
			{
			//	RTC_Render.StartRender();
			//	RTC_Render.StartRender();
			}
		}

		public static StashKey GetCurrentSavestateStashkey()
		{
			if (RTC_StockpileManager.CurrentSavestateKey == null || !SavestateStashkeyDico.ContainsKey(RTC_StockpileManager.CurrentSavestateKey))
				return null;

			return SavestateStashkeyDico[RTC_StockpileManager.CurrentSavestateKey];
		}

		public static bool ApplyStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();


			if (_loadBeforeOperation)
			{
				if (!LoadState(sk))
				{
					return IsCorruptionApplied;
				}
			}
			RTC_Corruptcore.ApplyBlastLayer(sk.BlastLayer);


			IsCorruptionApplied = (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0);

			PostApplyStashkey();
			return IsCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();


			StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();

			if (psk == null)
			{
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				return false;
			}

			string currentGame = (string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_GETGAMENAME, true);
			if (currentGame == null || psk.GameName != currentGame) 
			{
				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, psk.RomFilename, true);
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();
			BlastLayer bl = RTC_Corruptcore.GenerateBlastLayer((string[])RTC_Corruptcore.UISpec["SELECTEDDOMAINS"]);
			watch.Stop();
			Console.WriteLine($"It took " + watch.ElapsedMilliseconds + " ms to blastlayer");

			CurrentStashkey = new StashKey(RTC_Corruptcore.GetRandomKey(), psk.ParentKey, bl)
			{
				RomFilename = psk.RomFilename,
				SystemName = psk.SystemName,
				SystemCore = psk.SystemCore,
				GameName = psk.GameName,
				SyncSettings = psk.SyncSettings
			};

			if (_loadBeforeOperation)
			{
				if (!LoadState(CurrentStashkey, true, true))
				{
					return IsCorruptionApplied;
				}
			}
			else
				RTC_Corruptcore.ApplyBlastLayer(bl);

			IsCorruptionApplied = (bl != null);

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
			return IsCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();

			if (psk == null)
			{
				MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				return false;
			}

			if (psk.SystemCore != sk.SystemCore && !RTC_Corruptcore.AllowCrossCoreCorruption)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return IsCorruptionApplied;
			}

			CurrentStashkey = new StashKey(RTC_Corruptcore.GetRandomKey(), psk.ParentKey, sk.BlastLayer)
			{
				RomFilename = psk.RomFilename,
				SystemName = psk.SystemName,
				SystemCore = psk.SystemCore,
				GameName = psk.GameName
			};

			if (_loadBeforeOperation)
			{
				if (!LoadState(CurrentStashkey))
					return false;
			}
			else
				RTC_Corruptcore.ApplyBlastLayer(sk.BlastLayer);

			IsCorruptionApplied = (sk.BlastLayer != null);

			if (StashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				//todo
				//S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			}

			PostApplyStashkey();
			return IsCorruptionApplied;
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
				return IsCorruptionApplied;

			IsCorruptionApplied = false;

			PostApplyStashkey();
			return IsCorruptionApplied;
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
					SyncSettings = master.SyncSettings
				};

				//RTC_NetCore.HugeOperationEnd(token);
				//  token = RTC_NetCore.HugeOperationStart("LAZY");

				if (_loadBeforeOperation)
				{
					if (!LoadState(CurrentStashkey))
					{
						return IsCorruptionApplied;
					}
				}
				else
				{
					RTC_Corruptcore.ApplyBlastLayer(CurrentStashkey.BlastLayer);
				}

				IsCorruptionApplied = (CurrentStashkey.BlastLayer != null && CurrentStashkey.BlastLayer.Layer.Count > 0);

				if (StashAfterOperation)
				{
					StashHistory.Add(CurrentStashkey);
					//todo
					//S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				}


				PostApplyStashkey();
				return true;
			}
			MessageBox.Show("You need 2 or more items for Merging");
			return false;
		}

		public static bool LoadState(StashKey sk, bool reloadRom = true, bool applyBlastLayer = true)
		{

			var loadStateWatch = System.Diagnostics.Stopwatch.StartNew();
			if (sk == null)
				return false;

			StashKey.SetCore(sk);
			string gameSystem = sk.SystemName;
			string gameName = sk.GameName;
			string key = sk.ParentKey;
			StashKeySavestateLocation stateLocation = sk.StateLocation;


			if (reloadRom)
			{
				LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, sk.RomFilename, true);

				var ssWatch = System.Diagnostics.Stopwatch.StartNew();
				string ss = (string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_GETSETSYNCSETTINGS, true);
				ssWatch.Stop();
				Console.WriteLine($"Time taken to get the SyncSettings: {0}ms", ssWatch.ElapsedMilliseconds);

				//If the syncsettings are different, update them and load it again. Otheriwse, leave as is
				if (sk.SyncSettings != ss && sk.SyncSettings != null)
				{
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_GETSETSYNCSETTINGS, sk.SyncSettings, true);
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_LOADROM, sk.RomFilename, true);
				}
			}


			string theoreticalSaveStateFilename = RTC_Corruptcore.workingDir + Path.DirectorySeparatorChar + stateLocation.ToString() + Path.DirectorySeparatorChar + gameName + "." + key + ".timejump.State";

			if (File.Exists(theoreticalSaveStateFilename))
			{
				if (!LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.VANGUARD, NetcoreCommands.LOADSAVESTATE, new object[] {key, stateLocation}, true))
				{
					MessageBox.Show($"Error loading savestate : An internal Bizhawk error has occurred.\n Are you sure your savestate matches the game, your syncsettings match, and the savestate is supported by this version of Bizhawk?");
					return false;
				}
			}
			else
			{
				MessageBox.Show($"Error loading savestate : (File {key + ".timejump"} not found)");
				return false;
			}

			loadStateWatch.Stop();
			Console.WriteLine($"Time taken for LoadState_NET: {0}ms", loadStateWatch.ElapsedMilliseconds);
			return true;
		}

		public static StashKey SaveState(bool sendToStashDico, StashKey _sk = null, bool sync = true)
		{
			return (StashKey)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.SAVESAVESTATE, new object[] { sendToStashDico, _sk }, sync);
		}

		public static StashKey SaveState_NET(bool sendToStashDico, StashKey _sk = null, bool threadSave = false)
		{
			string Key = RTC_Corruptcore.GetRandomKey();
			string statePath;

			StashKey sk;

			if (_sk == null)
			{
				Key = RTC_Corruptcore.GetRandomKey();
				statePath = (string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.SAVESAVESTATE, Key);
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

			//sk.StateShortFilename = statePath.Substring(statePath.LastIndexOf(Path.DirectorySeparatorChar) + 1, statePath.Length - (statePath.LastIndexOf(Path.DirectorySeparatorChar) + 1));
			sk.StateShortFilename = Path.GetFileName(statePath);
			sk.StateFilename = statePath;

			if (sendToStashDico)
			{
				//RTC_NetcoreImplementation.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_KEY_PUSHSAVESTATEDICO) { objectValue = new object[] { sk, RTC_StockpileManager.CurrentSavestateKey } });

				var currentkey = RTC_StockpileManager.CurrentSavestateKey;
				if(currentkey != null)
					RTC_StockpileManager.SavestateStashkeyDico[currentkey] = sk;
			}

			return sk;
		}

		/*
		public static bool ChangeGameWarning(string rom, bool dontask = false)
		{
			if (RTC_Hooks.BIZHAWK_ISNULLEMULATORCORE() || RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM().Contains("default.nes") || dontask)
				return true;

			if (rom == null)
				return false;

			string currentFilename = RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM();
			currentFilename = Path.GetFileName(currentFilename);

			string btnFilename = rom;
			btnFilename = Path.GetFileName(btnFilename);

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
		}*/

		public static void StockpileChanged()
		{
			//S.GET<RTC_StockpileBlastBoard_Form>().RefreshButtons();
		}

		public static StashKey GetRawBlastlayer()
		{

			StashKey sk = RTC_StockpileManager.SaveState_NET(false);

			BlastLayer bl = new BlastLayer();


			bl.Layer.AddRange(RTC_StepActions.GetRawBlastLayer().Layer);

			string thisSystem = (string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_SYSTEM, true);
			string romFilename =(string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_KEY_GETOPENROMFILENAME, true);

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
						original = MutateSwapN64(original);
					else if (romFilename.ToUpper().Contains(".SMD"))
						original = DeInterleaveSMD(original);

					for (int i = 0; i < rp.SkipBytes; i++)
						original[i] = 0;

					BlastLayer romBlast = RTC_BlastTools.GetBlastLayerFromDiff(original, corrupted);

					if (romBlast != null && romBlast.Layer.Count > 0)
						bl.Layer.AddRange(romBlast.Layer);
				}
			}

			sk.BlastLayer = bl;

			return sk;
		}

		public static void AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			IsCorruptionApplied = (CurrentStashkey.BlastLayer != null && CurrentStashkey.BlastLayer.Layer.Count > 0);

			if (StashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				//todo
				//S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			}
		}

		public static byte[] DeInterleaveSMD(byte[] source)
		{
			// SMD files are interleaved in pages of 16k, with the first 8k containing all 
			// odd bytes and the second 8k containing all even bytes.
			int size = source.Length;
			if (size > 0x400000)
			{
				size = 0x400000;
			}

			int pages = size / 0x4000;
			var output = new byte[size];

			for (int page = 0; page < pages; page++)
			{
				for (int i = 0; i < 0x2000; i++)
				{
					output[(page * 0x4000) + (i * 2) + 0] = source[(page * 0x4000) + 0x2000 + i];
					output[(page * 0x4000) + (i * 2) + 1] = source[(page * 0x4000) + 0x0000 + i];
				}
			}

			return output;
		}

		public static unsafe byte[] MutateSwapN64(byte[] source)
		{
			// N64 roms are in one of the following formats:
			//  .Z64 = No swapping
			//  .N64 = Word Swapped
			//  .V64 = Byte Swapped

			// File extension does not always match the format
			int size = source.Length;

			// V64 format
			fixed (byte* pSource = &source[0])
			{
				if (pSource[0] == 0x37)
				{
					for (int i = 0; i < size; i += 2)
					{
						byte temp = pSource[i];
						pSource[i] = pSource[i + 1];
						pSource[i + 1] = temp;
					}
				}

				// N64 format
				else if (pSource[0] == 0x40)
				{
					for (int i = 0; i < size; i += 4)
					{
						// output[i] = source[i + 3];
						// output[i + 3] = source[i];
						// output[i + 1] = source[i + 2];
						// output[i + 2] = source[i + 1];
						byte temp = pSource[i];
						pSource[i] = source[i + 3];
						pSource[i + 3] = temp;

						temp = pSource[i + 1];
						pSource[i + 1] = pSource[i + 2];
						pSource[i + 2] = temp;
					}
				}
				else // Z64 format (or some other unknown format)
				{
				}
			}

			return source;
		}
	}
}

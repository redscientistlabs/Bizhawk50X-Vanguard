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
		public static Stockpile CurrentStockpile { get; set; }

		public static StashKey CurrentStashkey { get; set; }

		public static Stack<StashKey> AllBackupStates { get; set; } = new Stack<StashKey>();
		public static BlastLayer LastBlastLayerBackup { get; set; } = null;

		public static bool UnsavedEdits = false;

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

				S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = value;
			}
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
				RTC_Render.StartRender();
			}
		}

		public static StashKey GetCurrentSavestateStashkey()
		{
			if (RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()] == null || !SavestateStashkeyDico.ContainsKey(RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()]?.ToString()))
				return null;

			return SavestateStashkeyDico[RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()].ToString()];
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
					return IsCorruptionApplied;
				}
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true });

			RTC_NetCore.HugeOperationEnd(token);

			IsCorruptionApplied = (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0);

			PostApplyStashkey();
			return IsCorruptionApplied;
		}

		public static bool Corrupt(bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			var token = RTC_NetCore.HugeOperationStart("LAZY");

			StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();

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
				S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
				S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();
			BlastLayer bl = (BlastLayer)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { objectValue = RTC_Unispec.RTCSpec[RTCSPEC.MEMORYDOMAINS_SELECTEDDOMAINS.ToString()] }, true);
			watch.Stop();
			Console.WriteLine($"It took " + watch.ElapsedMilliseconds + " ms to blastlayer");

			CurrentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, bl);
			CurrentStashkey.RomFilename = psk.RomFilename;
			CurrentStashkey.SystemName = psk.SystemName;
			CurrentStashkey.SystemCore = psk.SystemCore;
			CurrentStashkey.GameName = psk.GameName;
			CurrentStashkey.SyncSettings = psk.SyncSettings;

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(CurrentStashkey))
				{
					RTC_NetCore.HugeOperationEnd(token);
					return IsCorruptionApplied;
				}
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = bl });

			IsCorruptionApplied = (bl != null);

			if (StashAfterOperation && bl != null)
			{
				StashHistory.Add(CurrentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
				S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();

				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

				S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
				S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
			}

			RTC_NetCore.HugeOperationEnd(token);

			PostApplyStashkey();
			return IsCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.GetCurrentSavestateStashkey();

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
				return IsCorruptionApplied;
			}

			CurrentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, sk.BlastLayer);
			CurrentStashkey.RomFilename = psk.RomFilename;
			CurrentStashkey.SystemName = psk.SystemName;
			CurrentStashkey.SystemCore = psk.SystemCore;
			CurrentStashkey.GameName = psk.GameName;

			if (_loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(CurrentStashkey))
					return false;
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer }, true);

			IsCorruptionApplied = (sk.BlastLayer != null);

			if (StashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
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

				CurrentStashkey = new StashKey(RTC_Core.GetRandomKey(), master.ParentKey, bl)
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
					if (!LoadStateAndBlastLayer(CurrentStashkey))
					{
						RTC_NetCore.HugeOperationEnd(token);
						return IsCorruptionApplied;
					}
				}
				else
				{
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = CurrentStashkey.BlastLayer });
				}

				IsCorruptionApplied = (CurrentStashkey.BlastLayer != null && CurrentStashkey.BlastLayer.Layer.Count > 0);

				if (StashAfterOperation)
				{
					StashHistory.Add(CurrentStashkey);
					S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
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

		public static bool LoadState(StashKey sk, bool reloadRom = true, bool applyBlastLayer = true)
		{
			object returnValue = RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, reloadRom, false, applyBlastLayer }
			}, true);

			return (returnValue != null && (bool)returnValue);
		}

		public static bool LoadStateAndBlastLayer(StashKey sk, bool reloadRom = true)
		{
			object returnValue = RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, reloadRom, (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0) }
			}, true);

			return (returnValue != null && (bool)returnValue);
		}

		public static bool LoadState_NET(StashKey sk, bool reloadRom = true)
		{
			var loadStateWatch = System.Diagnostics.Stopwatch.StartNew();
			if (sk == null)
				return false;

			StashKey.SetCore(sk);
			string gameSystem = sk.SystemName;
			string gameName = sk.GameName;
			string key = sk.ParentKey;
			StashKeySavestateLocation stateLocation = sk.StateLocation;

			RTC_Core.StopSound();

			if (reloadRom)
			{
				RTC_Core.LoadRom_NET(sk.RomFilename);

				var ssWatch = System.Diagnostics.Stopwatch.StartNew();
				string ss = RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS;
				ssWatch.Stop();
				Console.WriteLine($"Time taken to get the SyncSettings: {0}ms", ssWatch.ElapsedMilliseconds);

				//If the syncsettings are different, update them and load it again. Otheriwse, leave as is
				if (sk.SyncSettings != ss && sk.SyncSettings != null)
				{
					RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS = sk.SyncSettings;
					RTC_Core.LoadRom_NET(sk.RomFilename);
				}
			}

			RTC_Core.StartSound();

			string theoreticalSaveStateFilename = RTC_Core.workingDir + "\\" + stateLocation.ToString() + "\\" + gameName + "." + key + ".timejump.State";

			if (File.Exists(theoreticalSaveStateFilename))
			{
				if (!RTC_Core.LoadSavestate_NET(key, stateLocation))
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
				MessageBox.Show($"Error loading savestate : (File {key + ".timejump"} not found)");
				RTC_Core.StartSound();
				return false;
			}

			loadStateWatch.Stop();
			Console.WriteLine($"Time taken for LoadState_NET: {0}ms", loadStateWatch.ElapsedMilliseconds);
			return true;
		}

		public static StashKey SaveState(bool sendToStashDico, StashKey _sk = null, bool sync = true)
		{
			return (StashKey)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SAVESTATE) { objectValue = new object[] { sendToStashDico, _sk } }, sync);
		}

		public static StashKey SaveState_NET(bool sendToStashDico, StashKey _sk = null, bool threadSave = false)
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

			if (sendToStashDico)
			{
				RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.REMOTE_KEY_PUSHSAVESTATEDICO) { objectValue = new object[] { sk, RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()] } });

				if (RTC_Hooks.isRemoteRTC)
				{
					var currentkey = RTC_Unispec.RTCSpec[RTCSPEC.STOCKPILE_CURRENTSAVESTATEKEY.ToString()]?.ToString();
					if(currentkey != null)
						RTC_StockpileManager.SavestateStashkeyDico[currentkey] = sk;
				}
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

		public static StashKey GetRawBlastlayer()
		{
			RTC_Core.StopSound();

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
			RTC_Core.StartSound();

			return sk;
		}

		public static void AddCurrentStashkeyToStash(bool _stashAfterOperation = true)
		{
			IsCorruptionApplied = (CurrentStashkey.BlastLayer != null && CurrentStashkey.BlastLayer.Layer.Count > 0);

			if (StashAfterOperation && _stashAfterOperation)
			{
				StashHistory.Add(CurrentStashkey);
				S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			}
		}
	}
}

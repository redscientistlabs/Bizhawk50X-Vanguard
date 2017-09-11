using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_StockpileManager
	{

		//Object references
		public static Stockpile currentStockpile = null;

		public static StashKey currentStashkey
        {
            get
            {
                return _currentStashkey;
            }
            set
            {
                _currentStashkey = value;

                GC.Collect();
                GC.WaitForPendingFinalizers();

            }
        }
        private static StashKey _currentStashkey = null;

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
		public static bool loadBeforeOperation = true;

		public static volatile List<StashKey> StashHistory = new List<StashKey>();

		// key: some key or guid, value: [0] savestate key [1] rom file
		public static volatile Dictionary<string, StashKey> SavestateStashkeyDico = new Dictionary<string, StashKey>();
		public static volatile string currentSavestateKey = null;
		public static bool renderAtLoad = false;

		private static void PreApplyStashkey()
		{
			RTC_HellgenieEngine.ClearCheats(true);
			RTC_PipeEngine.ClearPipes(true);
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

			if (loadBeforeOperation && _loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(sk))
					return isCorruptionApplied;
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true});

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
				RTC_Core.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_Core.StartSound();
				return false;
			}

			string currentGame = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);
			if (psk.GameName != currentGame)
			{
				RTC_Core.LoadRom(psk.RomFilename, true);
				RTC_Core.coreForm.RefreshDomains();
				RTC_Core.coreForm.setMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
			}


			BlastLayer bl = (BlastLayer)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { objectValue = RTC_MemoryDomains.SelectedDomains }, true);

			currentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, bl);
			currentStashkey.RomFilename = psk.RomFilename;
			currentStashkey.SystemName = psk.SystemName;
			currentStashkey.SystemCore = psk.SystemCore;
			currentStashkey.GameName = psk.GameName;

			if (loadBeforeOperation && _loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(currentStashkey))
					return isCorruptionApplied;
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = bl });

			isCorruptionApplied = (bl != null);
			
				if (stashAfterOperation && bl != null)
				{
					StashHistory.Add(currentStashkey);
					RTC_Core.ghForm.RefreshStashHistory();
					RTC_Core.ghForm.dgvStockpile.ClearSelection();
					RTC_Core.ghForm.DontLoadSelectedStash = true;
					RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;
				}

			PostApplyStashkey();
			return isCorruptionApplied;
		}

		public static bool InjectFromStashkey(StashKey sk, bool _loadBeforeOperation = true)
		{
			PreApplyStashkey();

			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null && loadBeforeOperation && _loadBeforeOperation)
			{
				RTC_Core.StopSound();
                MessageBox.Show("The Glitch Harvester could not perform the INJECT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
                RTC_Core.StartSound();
				return false;
			}


			if (psk.SystemCore != sk.SystemCore)
			{
				MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + $"{psk.GameName} -> {psk.SystemName} -> {psk.SystemCore}\n{sk.GameName} -> {sk.SystemName} -> {sk.SystemCore}");
				return isCorruptionApplied;
			}


			currentStashkey = new StashKey(RTC_Core.GetRandomKey(), psk.ParentKey, sk.BlastLayer);
			currentStashkey.RomFilename = psk.RomFilename;
			currentStashkey.SystemName = psk.SystemName;
			currentStashkey.SystemCore = psk.SystemCore;
			currentStashkey.GameName = psk.GameName;

			if (loadBeforeOperation && _loadBeforeOperation)
			{
				if (!LoadStateAndBlastLayer(currentStashkey))
					return false;
			}
			else
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer}, true);

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

            if(sk == null)
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

		public static bool MergeStashkeys(List<StashKey> sks, bool _stashAfterOperation = true)
		{
			PreApplyStashkey();

			if (sks != null && sks.Count > 1)
			{
                RTC_RPC.SendToKillSwitch("FREEZE");
                RTC_NetCore.HugeOperationStart();


                StashKey master = sks[0];

				string masterSystemCore = master.SystemCore;
				bool allCoresIdentical = true;

				foreach (StashKey item in sks)
					if(item.SystemCore != master.SystemCore)
					{
						allCoresIdentical = false;
						break;
					}

				if(!allCoresIdentical)
				{
					MessageBox.Show("Merge attempt failed: Core mismatch\n\n" + string.Join("\n", sks.Select(it => $"{it.GameName} -> {it.SystemName} -> {it.SystemCore}")));
                    RTC_RPC.SendToKillSwitch("UNFREEZE");
                    RTC_NetCore.HugeOperationEnd();

                    return false;
				}

				BlastLayer bl = new BlastLayer();

				foreach (StashKey item in sks)
					bl.Layer.AddRange(item.BlastLayer.Layer);

                RTC_RPC.SendToKillSwitch("UNFREEZE");
                RTC_NetCore.HugeOperationEnd();

                currentStashkey = new StashKey(RTC_Core.GetRandomKey(), master.ParentKey, bl);
				currentStashkey.RomFilename = master.RomFilename;
				currentStashkey.SystemName = master.SystemName;
				currentStashkey.SystemCore = master.SystemCore;
				currentStashkey.GameName = master.GameName;

				if (loadBeforeOperation)
				{
					if (!LoadStateAndBlastLayer(currentStashkey))
						return isCorruptionApplied;
				}
				else
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = currentStashkey.BlastLayer}, true);

				isCorruptionApplied = (currentStashkey.BlastLayer != null && currentStashkey.BlastLayer.Layer.Count > 0);

				if (stashAfterOperation && _stashAfterOperation)
				{
					StashHistory.Add(currentStashkey);
					RTC_Core.ghForm.RefreshStashHistory();
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
			return (bool)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE)
			{
				objectValue = new object[] { sk, ReloadRom, false, applyBlastLayer }
			},true);
		}

		public static bool LoadStateAndBlastLayer(StashKey sk, bool ReloadRom = true)
		{
			object returnValue = RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_LOADSTATE){
				objectValue = new object[] { sk, ReloadRom, (sk.BlastLayer != null && sk.BlastLayer.Layer.Count > 0) }
			}, true);

			return (returnValue != null ? (bool)returnValue : false);
		}

		public static bool LoadState_NET(StashKey sk, bool ReloadRom = true)
		{
			if (sk == null)
				return false;

			string GameSystem = sk.SystemName;
			string GameName = sk.GameName;
			string Key = sk.ParentKey;
			bool GameHasChanged = false;
			string TheoricalSaveStateFilename;

			RTC_Core.StopSound();

			if (ReloadRom)
			{
					RTC_Core.LoadRom_NET(sk.RomFilename);
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
				RTC_Core.LoadSavestate_NET(Key);
			}
			else
			{
				RTC_Core.StopSound();
				MessageBox.Show($"Error loading savestate : (File {Key + ".timejump"} not found)");
				RTC_Core.StartSound();
				return false;
			}

			return true;
		}

		public static StashKey SaveState(bool SendToStashDico, StashKey _sk = null, bool sync = true)
		{
			return (StashKey)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SAVESTATE) { objectValue = new object[] { SendToStashDico, _sk} }, sync);
		}

		public static StashKey SaveState_NET(bool SendToStashDico, StashKey _sk = null, bool threadSave = false)
		{
			string Key = RTC_Core.GetRandomKey();
			string statePath;

			StashKey sk;

			if (_sk == null) {
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

				if(RTC_Hooks.isRemoteRTC)
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

			foreach (var item in Global.CheatList)
			{
				string[] disassembleCheat = item.Name.Split('|');

				if (disassembleCheat[0] == "RTC Cheat")
				{
					string _domain = disassembleCheat[1];
					long _address = Convert.ToInt64(disassembleCheat[2]);

					BizHawk.Client.Common.DisplayType _displayType = BizHawk.Client.Common.DisplayType.Unsigned;

					bool _bigEndian = Convert.ToBoolean(disassembleCheat[4]);
					int _value = Convert.ToInt32(disassembleCheat[5]);
					bool _isEnabled = Convert.ToBoolean(disassembleCheat[6]);
					bool _isFreeze = Convert.ToBoolean(disassembleCheat[7]);

					bl.Layer.Add(new BlastCheat(_domain, _address, _displayType, _bigEndian, _value, _isEnabled, _isFreeze));
				}
			}

			bl.Layer.AddRange(RTC_PipeEngine.AllBlastPipes);

            string thisSystem = Global.Game.System;
            string _primarydomain = null;
            string _seconddomain = null;
            int skipbytes = 0;

            switch (thisSystem.ToUpper())
            {
                case "NES":
                    _primarydomain = "PRG ROM";
                    _seconddomain = "CHR VROM";
                    skipbytes = 16;
                    break;

                case "SNES":
                    _primarydomain = "CARTROM";
                    break;

                case "A78":
                    _primarydomain = "HSC ROM";
                    break;

                case "LYNX":
                    _primarydomain = "Cart A";
                    skipbytes = 64;
                    break;

                case "N64":
                case "GB":
                case "GBC":
                case "SMS":
                case "GEN":
                case "GBA":
                case "PCE":
                case "GG":
                case "SG":
                case "SGX":
                case "WSWAN":
                    _primarydomain = "ROM";
                    break;


                case "PCECD":
                case "SAT":
                case "PSX":
                    MessageBox.Show("Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...");
                    return null;

            }


            if (_primarydomain != null)
            {
                List<byte> addData = new List<byte>();
                for (int i = 0; i < skipbytes; i++)
                    addData.Add((byte)0);

                addData.AddRange(RTC_MemoryDomains.getDomainData(_primarydomain));
                if(_seconddomain != null)
                    addData.AddRange(RTC_MemoryDomains.getDomainData(_seconddomain));

                byte[] corrupted = addData.ToArray();
                byte[] original = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

                for (int i = 0; i < skipbytes; i++)
                    original[i] = (byte)0;

                BlastLayer romBlast = RTC_ExternalRomPlugin.GetBlastLayer(original, corrupted);

                if (romBlast != null && romBlast.Layer.Count > 0)
                    bl.Layer.AddRange(romBlast.Layer);

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

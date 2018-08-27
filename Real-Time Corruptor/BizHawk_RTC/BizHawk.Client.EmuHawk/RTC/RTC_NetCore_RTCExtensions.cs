using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_NetCore
	{
		static volatile Dictionary<string, bool> TransferedRomFilenames = new Dictionary<string, bool>();

		public RTC_Command Process_RTCExtensions(RTC_Command cmd)
		{
			RTC_Command cmdBack = null;

			switch (cmd.Type)
			{
				case CommandType.ASYNCBLAST:
					{
						BlastLayer bl = RTC_Core.Blast(null, RTC_MemoryDomains.SelectedDomains);
						if (bl != null)
							bl.Apply();
					}
					break;

				case CommandType.BLAST:
				{
					BlastLayer bl = null;
					string[] _domains = (string[])cmd.objectValue;

					if (_domains == null)
						_domains = RTC_MemoryDomains.SelectedDomains;

					if (cmd.blastlayer != null)
					{
						cmd.blastlayer.Apply(cmd.isReplay);
					}
					else
					{
						bl = RTC_Core.Blast(null, _domains);
					}

					if (cmd.requestGuid != null)
					{
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = bl;
					}
				}

				break;

				case CommandType.STASHKEY:

					if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename))
						File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP\\" + cmd.romFilename, cmd.romData);

					cmd.stashkey.RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + RTC_Extensions.getShortFilenameFromPath(cmd.romFilename);

					cmd.stashkey.DeployState();

					cmd.stashkey.Run();

					break;

				case CommandType.PULLROM:
					cmdBack = new RTC_Command(CommandType.PUSHROM);
					cmdBack.romFilename = RTC_Extensions.getShortFilenameFromPath(GlobalWin.MainForm.CurrentlyOpenRom);

					if (!PeerHasRom(cmdBack.romFilename))
						cmdBack.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

					break;

				case CommandType.PUSHROM:
					if (cmd.romData != null)
					{
						cmd.romFilename = RTC_Extensions.getShortFilenameFromPath(cmd.romFilename);
						if (!File.Exists(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename))
							File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename, cmd.romData);
					}

					RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename);
					break;

				case CommandType.PULLSTATE:
					cmdBack = new RTC_Command(CommandType.PUSHSTATE);
					StashKey sk_PULLSTATE = RTC_StockpileManager.SaveState(false);
					cmdBack.stashkey = sk_PULLSTATE;
					sk_PULLSTATE.EmbedState();

					break;

				case CommandType.PUSHSTATE:
					cmd.stashkey.DeployState();
					RTC_StockpileManager.LoadState(cmd.stashkey, true);

					if (RTC_Core.multiForm.cbPullStateToGlitchHarvester.Checked)
					{
						StashKey sk_PUSHSTATE = RTC_StockpileManager.SaveState(true, cmd.stashkey);
						sk_PUSHSTATE.RomFilename = GlobalWin.MainForm.CurrentlyOpenRom;
					}

					break;

				case CommandType.PULLSWAPSTATE:

					cmdBack = new RTC_Command(CommandType.PUSHSWAPSTATE);
					cmdBack.romFilename = RTC_Extensions.getShortFilenameFromPath(GlobalWin.MainForm.CurrentlyOpenRom);

					if (!PeerHasRom(cmdBack.romFilename))
						cmdBack.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

					StashKey sk_PULLSWAPSTATE = RTC_StockpileManager.SaveState(false);
					cmdBack.stashkey = sk_PULLSWAPSTATE;
					sk_PULLSWAPSTATE.EmbedState();

					cmd.romFilename = RTC_Extensions.getShortFilenameFromPath(cmd.romFilename);

					if (!File.Exists(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename))
						File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename, cmd.romData);
					RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename);

					cmd.stashkey.DeployState();
					RTC_StockpileManager.LoadState(cmd.stashkey, false);

					if (RTC_Core.multiForm.GameOfSwapTimer != null)
						RTC_Core.multiForm.GameOfSwapCounter = 64;

					break;

				case CommandType.PUSHSWAPSTATE:

					cmd.romFilename = RTC_Extensions.getShortFilenameFromPath(cmd.romFilename);

					if (cmd.romData != null)
						if (!File.Exists(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename))
							File.WriteAllBytes(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename, cmd.romData);

					RTC_Core.LoadRom(RTC_Core.rtcDir + "\\TEMP5\\" + cmd.romFilename);

					cmd.stashkey.DeployState();
					RTC_StockpileManager.LoadState(cmd.stashkey, false);

					if (RTC_Core.multiForm.GameOfSwapTimer != null)
						RTC_Core.multiForm.GameOfSwapCounter = 64;

					break;
				case CommandType.PULLSCREEN:
					cmdBack = new RTC_Command(CommandType.PUSHSCREEN);
					cmdBack.screen = GlobalWin.MainForm.MakeScreenshotImage().ToSysdrawingBitmap();
					break;

				case CommandType.REQUESTSTREAM:
					RTC_Core.multiForm.cbStreamScreenToPeer.Checked = true;
					break;

				case CommandType.PUSHSCREEN:
					UpdatePeerScreen(cmd.screen);
					break;

				case CommandType.GAMEOFSWAPSTART:
					RTC_Core.multiForm.StartGameOfSwap(false);
					break;

				case CommandType.GAMEOFSWAPSTOP:
					RTC_Core.multiForm.StopGameOfSwap(true);
					break;

				case CommandType.REMOTE_PUSHPARAMS:
					(cmd.objectValue as RTC_Params).Deploy();
					break;

				case CommandType.REMOTE_PUSHVMDS:
					RTC_MemoryDomains.VmdPool.Clear();
					foreach (var proto in (cmd.objectValue as VmdPrototype[]))
						RTC_MemoryDomains.AddVMD(proto);
					break;

				case CommandType.BLASTGENERATOR_BLAST:
				{
					List<BlastGeneratorProto> returnList;

					List<BlastGeneratorProto> blastGeneratorProtos =
						(List<BlastGeneratorProto>)(cmd.objectValue as object);
					StashKey _sk = (StashKey)(cmd.stashkey as object);

					returnList = RTC_BlastTools.GenerateBlastLayersFromBlastGeneratorProtos(blastGeneratorProtos, _sk);

					if (cmd.requestGuid != null)
					{
						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = returnList;
					}

					break;
				}

				case CommandType.REMOTE_LOADROM:
					RTC_Core.LoadRom_NET(cmd.romFilename);
					break;
				case CommandType.REMOTE_LOADSTATE:
					{
						StashKey sk = (StashKey)(cmd.objectValue as object[])[0];
						bool reloadRom = (bool)(cmd.objectValue as object[])[1];
						bool runBlastLayer = (bool)(cmd.objectValue as object[])[2];

						bool returnValue = RTC_StockpileManager.LoadState_NET(sk, reloadRom);

						RTC_MemoryDomains.RefreshDomains(false);

						if (runBlastLayer)
							RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST) { blastlayer = sk.BlastLayer, isReplay = true });

						cmdBack = new RTC_Command(CommandType.RETURNVALUE);
						cmdBack.objectValue = returnValue;
					}
					break;

				case CommandType.REMOTE_MERGECONFIG:
					Stockpile.MergeBizhawkConfig_NET();
					break;

				case CommandType.REMOTE_IMPORTKEYBINDS:
					Stockpile.ImportBizhawkKeybinds_NET();
					break;

				case CommandType.REMOTE_SAVESTATE:
					{
						StashKey sk = RTC_StockpileManager.SaveState_NET((bool)(cmd.objectValue as object[])[0], (StashKey)(cmd.objectValue as object[])[1]);
						if (cmd.requestGuid != null)
						{
							cmdBack = new RTC_Command(CommandType.RETURNVALUE);
							cmdBack.objectValue = sk;
						}
					}
					break;

				case CommandType.REMOTE_BACKUPKEY_REQUEST:
					{
						if (!RTC_Hooks.isNormalAdvance)
							break;

						cmdBack = new RTC_Command(CommandType.REMOTE_BACKUPKEY_STASH);

						bool multiThread = false;

						// apparently multithread savestates doesn't work well right now.
						// We can try again in a future version of bizhawk
						/*
                        if (new string[] {
                             "SNES", "GB", "GBC", "GBA",
                        }.Contains(Global.Game.System.ToString().ToUpper()))
                            multiThread = false;
                        */

						cmdBack.objectValue = RTC_StockpileManager.SaveState_NET(false, null, multiThread);
						break;
					}

				case CommandType.REMOTE_BACKUPKEY_STASH:
					RTC_StockpileManager.backupedState = (StashKey)cmd.objectValue;
					RTC_StockpileManager.allBackupStates.Push((StashKey)cmd.objectValue);
					RTC_Core.coreForm.btnGpJumpBack.Visible = true;
					RTC_Core.coreForm.btnGpJumpNow.Visible = true;
					break;

				case CommandType.REMOTE_RASTERIZE_PIPES:
					RTC_PipeEngine.RasterizePipes();
					break;

				case CommandType.REMOTE_DOMAIN_PEEKBYTE:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = RTC_MemoryDomains.GetInterface((string)(cmd.objectValue as object[])[0]).PeekByte((long)(cmd.objectValue as object[])[1]);
					break;

				case CommandType.REMOTE_DOMAIN_POKEBYTE:
					RTC_MemoryDomains.GetInterface((string)(cmd.objectValue as object[])[0]).PokeByte((long)(cmd.objectValue as object[])[1], (byte)(cmd.objectValue as object[])[2]);
					break;

				case CommandType.REMOTE_DOMAIN_GETDOMAINS:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = RTC_MemoryDomains.GetInterfaces();

					break;

				case CommandType.REMOTE_DOMAIN_VMD_ADD:
					RTC_MemoryDomains.AddVMD((cmd.objectValue as VmdPrototype));
					break;

				case CommandType.REMOTE_DOMAIN_VMD_REMOVE:
					RTC_MemoryDomains.RemoveVMD((cmd.objectValue as string));
					break;

				case CommandType.REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP:
					RTC_MemoryDomains.generateActiveTableDump((string)(cmd.objectValue as object[])[0], (string)(cmd.objectValue as object[])[1]);
					break;

				case CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS:
					RTC_MemoryDomains.UpdateSelectedDomains((string[])cmd.objectValue);
					break;

				case CommandType.REMOTE_DOMAIN_SYSTEM:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = Global.Game.System.ToString().ToUpper();
					break;

				case CommandType.REMOTE_DOMAIN_SYSTEMPREFIX:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = PathManager.SaveStatePrefix(Global.Game);
					break;

				case CommandType.REMOTE_KEY_PUSHSAVESTATEDICO:
					{
						var key = (string)(cmd.objectValue as object[])[1];
						var sk = (StashKey)((cmd.objectValue as object[])[0]);
						RTC_StockpileManager.SavestateStashkeyDico[key] = sk;
						RTC_Core.ghForm.RefreshSavestateTextboxes();
					}
					break;

				case CommandType.REMOTE_KEY_GETSYSTEMNAME:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = (Global.Config.PathEntries[Global.Game.System, "Savestates"] ?? Global.Config.PathEntries[Global.Game.System, "Base"]).SystemDisplayName;
					break;

				case CommandType.REMOTE_KEY_GETSYSTEMCORE:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = StashKey.getCoreName_NET((string)cmd.objectValue);
					break;

				case CommandType.REMOTE_KEY_GETGAMENAME:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = PathManager.FilesystemSafeName(Global.Game);
					break;

				case CommandType.REMOTE_KEY_GETSYNCSETTINGS:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = StashKey.getSyncSettings_NET((string)cmd.objectValue);
					break;

				case CommandType.REMOTE_KEY_PUTSYNCSETTINGS:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					break;

				case CommandType.REMOTE_KEY_GETOPENROMFILENAME:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = GlobalWin.MainForm.CurrentlyOpenRom;
					break;

				case CommandType.REMOTE_KEY_GETRAWBLASTLAYER:
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = RTC_StockpileManager.getRawBlastlayer();
					break;
				case CommandType.REMOTE_KEY_GETBLASTBYTESETFROMLAYER:
				{
					//We need a stashkey to load the game 
					BlastLayer _bl = cmd.blastlayer;
					var sk = cmd.stashkey;
					cmdBack = new RTC_Command(CommandType.RETURNVALUE);
					cmdBack.objectValue = RTC_BlastTools.GetAppliedBackupLayer(_bl, sk);
					break;
				}

				case CommandType.BIZHAWK_SET_OSDDISABLED:
					RTC_Core.BizhawkOsdDisabled = (bool)cmd.objectValue;
					break;

				case CommandType.BIZHAWK_SET_DONT_CLEAN_SAVESTATES_AT_QUIT:
					RTC_Core.DontCleanSavestatesOnQuit = (bool)cmd.objectValue;
					break;

				case CommandType.ENABLE_CONSOLE:
					RTC_Hooks.ShowConsole = (bool)cmd.objectValue;
					break;

				case CommandType.BIZHAWK_OPEN_HEXEDITOR_ADDRESS:
				{

					GlobalWin.Tools.Load<HexEditor>();
					string domain = (string)(cmd.objectValue as object[])[0];
					long address = (long)(cmd.objectValue as object[])[1];

					long realAddress = RTC_MemoryDomains.GetRealAddress(domain, address);

					MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
					GlobalWin.Tools.HexEditor.SetDomain(mdp.md);
					GlobalWin.Tools.HexEditor.GoToAddress(realAddress);
					break;
				}

				case CommandType.REMOTE_SET_SAVESTATEBOX:
					RTC_StockpileManager.currentSavestateKey = (string)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_AUTOCORRUPT:
					RTC_Core.AutoCorrupt = (bool)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_CUSTOMPRECISION:
					RTC_Core.CustomPrecision = (int)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_INTENSITY:
					RTC_Core.Intensity = (int)cmd.objectValue;
					break;
				case CommandType.REMOTE_SET_ERRORDELAY:
					RTC_Core.ErrorDelay = (int)cmd.objectValue;
					break;
				case CommandType.REMOTE_SET_BLASTRADIUS:
					RTC_Core.Radius = (BlastRadius)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_RESTOREBLASTLAYERBACKUP:
					if (RTC_StockpileManager.lastBlastLayerBackup != null)
						RTC_StockpileManager.lastBlastLayerBackup.Apply(true);
					break;

				case CommandType.REMOTE_SET_NIGHTMARE_TYPE:
					RTC_NightmareEngine.Algo = (BlastByteAlgo)cmd.objectValue;
					break;
				case CommandType.REMOTE_SET_NIGHTMARE_MINVALUE:
				{
					int precision = (int)(cmd.objectValue as object[])[0];
					long value = (long)(cmd.objectValue as object[])[1];

					switch (precision)
					{
						case 1:
							RTC_NightmareEngine.MinValue8Bit = value;
							break;
						case 2:
							RTC_NightmareEngine.MinValue16Bit = value;
							break;
						case 4:
							RTC_NightmareEngine.MinValue32Bit = value;
							break;
					}
					break;
				}
				case CommandType.REMOTE_SET_NIGHTMARE_MAXVALUE:
				{
					int precision = (int)(cmd.objectValue as object[])[0];
					long value = (long)(cmd.objectValue as object[])[1];

					switch (precision)
					{
						case 1:
							RTC_NightmareEngine.MaxValue8Bit = value;
							break;
						case 2:
							RTC_NightmareEngine.MaxValue16Bit = value;
							break;
						case 4:
							RTC_NightmareEngine.MaxValue32Bit = value;
							break;
					}
					break;
				}
				case CommandType.REMOTE_SET_HELLGENIE_MAXCHEATS:
					RTC_HellgenieEngine.MaxCheats = (int)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_HELLGENIE_MINVALUE:
				{
					int precision = (int)(cmd.objectValue as object[])[0];
					long value = (long)(cmd.objectValue as object[])[1];

					switch (precision)
					{
						case 1:
							RTC_HellgenieEngine.MinValue8Bit = value;
							break;
						case 2:
							RTC_HellgenieEngine.MinValue16Bit = value;
							break;
						case 4:
							RTC_HellgenieEngine.MinValue32Bit = value;
							break;
					}
					break;
				}
				case CommandType.REMOTE_SET_HELLGENIE_MAXVALUE:
				{
					int precision = (int)(cmd.objectValue as object[])[0];
					long value = (long)(cmd.objectValue as object[])[1];

					switch (precision)
					{
						case 1:
							RTC_HellgenieEngine.MaxValue8Bit = value;
							break;
						case 2:
							RTC_HellgenieEngine.MaxValue16Bit = value;
							break;
						case 4:
							RTC_HellgenieEngine.MaxValue32Bit = value;
							break;
					}
					break;
				}

				case CommandType.REMOTE_SET_HELLGENIE_CHEARCHEATSREWIND:
					RTC_Core.ClearCheatsOnRewind = (bool)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_HELLGENIE_CLEARALLCHEATS:
					if (Global.CheatList != null)
						Global.CheatList.Clear();
					break;
				case CommandType.REMOTE_SET_HELLGENIE_REMOVEEXCESSCHEATS:
					while (Global.CheatList.Count > RTC_HellgenieEngine.MaxCheats)
						Global.CheatList.Remove(Global.CheatList[0]);
					break;

				case CommandType.REMOTE_SET_PIPE_MAXPIPES:
					RTC_PipeEngine.MaxPipes = (int)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_PIPE_TILTVALUE:
					RTC_PipeEngine.TiltValue = (int)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_PIPE_CLEARPIPES:
					RTC_PipeEngine.AllBlastPipes.Clear();
					RTC_PipeEngine.LastDomain = null;
					break;

				case CommandType.REMOTE_SET_PIPE_LOCKPIPES:
					RTC_PipeEngine.LockPipes = (bool)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_PIPE_CHAINEDPIPES:
					RTC_PipeEngine.ChainedPipes = (bool)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_PIPE_CLEARPIPESREWIND:
					RTC_Core.ClearPipesOnRewind = (bool)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_ENGINE:
					RTC_Core.SelectedEngine = (CorruptionEngine)cmd.objectValue;
					break;

				case CommandType.REMOTE_SET_DISTORTION_DELAY:
					RTC_DistortionEngine.MaxAge = (int)cmd.objectValue;
					RTC_DistortionEngine.CurrentAge = 0;
					RTC_DistortionEngine.AllDistortionBytes.Clear();
					break;

				case CommandType.REMOTE_SET_DISTORTION_RESYNC:
					RTC_DistortionEngine.CurrentAge = 0;
					RTC_DistortionEngine.AllDistortionBytes.Clear();
					break;

				case CommandType.REMOTE_SET_VECTOR_LIMITER:
					RTC_VectorEngine.LimiterList = (string[])cmd.objectValue;
					break;
				case CommandType.REMOTE_SET_VECTOR_VALUES:
					RTC_VectorEngine.ValueList = (string[])cmd.objectValue;
					break;

				case CommandType.REMOTE_EVENT_LOADGAMEDONE_NEWGAME:

					if (RTC_Core.isStandalone && RTC_GameProtection.isRunning)
						RTC_GameProtection.Reset();

					RTC_Core.AutoCorrupt = false;
					//RTC_StockpileManager.isCorruptionApplied = false;
					RTC_Core.ecForm.RefreshDomains();
					RTC_Core.ecForm.SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
					RTC_Core.ecForm.UpdateDefaultPrecision();
					break;

				case CommandType.REMOTE_EVENT_LOADGAMEDONE_SAMEGAME:
					//RTC_StockpileManager.isCorruptionApplied = false;
					RTC_Core.ecForm.RefreshDomainsAndKeepSelected();
					RTC_Core.ecForm.UpdateDefaultPrecision();
					break;

				case CommandType.REMOTE_EVENT_CLOSEBIZHAWK:
					GlobalWin.MainForm.Close();
					break;

				case CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG:
					GlobalWin.MainForm.SaveConfig();
					break;

				case CommandType.REMOTE_EVENT_BIZHAWKSTARTED:

					if (RTC_StockpileManager.backupedState == null)
						RTC_Core.coreForm.AutoCorrupt = false;

					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHPARAMS) { objectValue = new RTC_Params() }, true, true);

					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_PUSHVMDS) { objectValue = RTC_MemoryDomains.VmdPool.Values.Select(it => (it as VirtualMemoryDomain).Proto).ToArray() }, true, true);

					Thread.Sleep(100);

					if (RTC_StockpileManager.backupedState != null)
						RTC_Core.ecForm.RefreshDomainsAndKeepSelected(RTC_StockpileManager.backupedState.SelectedDomains.ToArray());

					if (RTC_Core.coreForm.cbUseGameProtection.Checked)
						RTC_GameProtection.Start();

					break;

				case CommandType.REMOTE_HOTKEY_MANUALBLAST:
					RTC_Core.coreForm.btnManualBlast_Click(null, null);
					break;

				case CommandType.REMOTE_HOTKEY_AUTOCORRUPTTOGGLE:
					RTC_Core.coreForm.btnAutoCorrupt_Click(null, null);
					break;
				case CommandType.REMOTE_HOTKEY_ERRORDELAYDECREASE:
					if (RTC_Core.ecForm.nmErrorDelay.Value > 1)
						RTC_Core.ecForm.nmErrorDelay.Value--;
					break;

				case CommandType.REMOTE_HOTKEY_ERRORDELAYINCREASE:
					if (RTC_Core.ecForm.nmErrorDelay.Value < RTC_Core.ecForm.track_ErrorDelay.Maximum)
						RTC_Core.ecForm.nmErrorDelay.Value++;
					break;

				case CommandType.REMOTE_HOTKEY_INTENSITYDECREASE:
					if (RTC_Core.ecForm.nmIntensity.Value > 1)
						RTC_Core.ecForm.nmIntensity.Value--;
					break;

				case CommandType.REMOTE_HOTKEY_INTENSITYINCREASE:
					if (RTC_Core.ecForm.nmIntensity.Value < RTC_Core.ecForm.track_Intensity.Maximum)
						RTC_Core.ecForm.nmIntensity.Value++;
					break;

				case CommandType.REMOTE_HOTKEY_GHLOADCORRUPT:
					if (!RTC_NetCore.NetCoreCommandSynclock)
					{
						RTC_NetCore.NetCoreCommandSynclock = true;

						RTC_Core.ghForm.cbAutoLoadState.Checked = true;
						RTC_Core.ghForm.btnCorrupt_Click(null, null);

						RTC_NetCore.NetCoreCommandSynclock = false;
					}
					break;

				case CommandType.REMOTE_HOTKEY_GHCORRUPT:
					if (!RTC_NetCore.NetCoreCommandSynclock)
					{
						RTC_NetCore.NetCoreCommandSynclock = true;

						bool isload = RTC_Core.ghForm.cbAutoLoadState.Checked;
						RTC_Core.ghForm.cbAutoLoadState.Checked = false;
						RTC_Core.ghForm.btnCorrupt_Click(null, null);
						RTC_Core.ghForm.cbAutoLoadState.Checked = isload;

						RTC_NetCore.NetCoreCommandSynclock = false;
					}
					break;

				case CommandType.REMOTE_HOTKEY_GHLOAD:
					RTC_Core.ghForm.btnSaveLoad.Text = "LOAD";
					RTC_Core.ghForm.btnSaveLoad_Click(null, null);
					break;
				case CommandType.REMOTE_HOTKEY_GHSAVE:
					RTC_Core.ghForm.btnSaveLoad.Text = "SAVE";
					RTC_Core.ghForm.btnSaveLoad_Click(null, null);
					break;
				case CommandType.REMOTE_HOTKEY_GHSTASHTOSTOCKPILE:
					RTC_Core.ghForm.AddStashToStockpile(false);
					break;

				case CommandType.REMOTE_HOTKEY_SENDRAWSTASH:
					RTC_Core.ghForm.btnSendRaw_Click(null, null);
					break;

				case CommandType.REMOTE_HOTKEY_BLASTRAWSTASH:
					RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.ASYNCBLAST));
					RTC_Core.ghForm.btnSendRaw_Click(null, null);
					break;
				case CommandType.REMOTE_HOTKEY_BLASTLAYERTOGGLE:
					RTC_Core.ghForm.btnBlastToggle_Click(null, null);
					break;
				case CommandType.REMOTE_HOTKEY_BLASTLAYERREBLAST:

					if (RTC_StockpileManager.currentStashkey == null || RTC_StockpileManager.currentStashkey.BlastLayer.Layer.Count == 0)
					{
						RTC_Core.ghForm.IsCorruptionApplied = false;
						break;
					}

					RTC_Core.ghForm.IsCorruptionApplied = true;
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BLAST) { blastlayer = RTC_StockpileManager.currentStashkey.BlastLayer });
					break;

				case CommandType.REMOTE_RENDER_START:
					RTC_Render.StartRender_NET();
					break;

				case CommandType.REMOTE_RENDER_STOP:
					RTC_Render.StopRender_NET();
					break;

				case CommandType.REMOTE_RENDER_SETTYPE:
					RTC_Render.lastType = (RENDERTYPE)cmd.objectValue;
					break;

				case CommandType.REMOTE_RENDER_STARTED:
					RTC_Core.ghForm.btnRender.Text = "Stop Render";
					RTC_Core.ghForm.btnRender.ForeColor = Color.GreenYellow;
					break;

				case CommandType.REMOTE_RENDER_RENDERATLOAD:
					RTC_StockpileManager.renderAtLoad = (bool)cmd.objectValue;
					break;

			}

			return cmdBack;
		}

		public bool PeerHasRom(string RomFilename)
		{
			if (TransferedRomFilenames.ContainsKey(RomFilename))
				return true;

			TransferedRomFilenames.Add(RomFilename, true);
			return false;
		}

		public void SwapGameState()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			RTC_Command cmd = new RTC_Command(CommandType.PULLSWAPSTATE);

			string romFullFilename = GlobalWin.MainForm.CurrentlyOpenRom;
			cmd.romFilename = romFullFilename.Substring(romFullFilename.LastIndexOf("\\") + 1, romFullFilename.Length - (romFullFilename.LastIndexOf("\\") + 1));

			if (!PeerHasRom(cmd.romFilename))
				cmd.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

			StashKey sk = RTC_StockpileManager.SaveState(false);
			cmd.stashkey = sk;
			sk.EmbedState();

			cmd.Priority = true;
			SendCommand(cmd, false);
		}

		public void SendStashkey()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			if (RTC_StockpileManager.currentStashkey == null)
			{
				MessageBox.Show("Couldn't fetch Stashkey from RTC_StockpileManager.currentStashkey");
				return;
			}

			RTC_Command cmd = new RTC_Command(CommandType.STASHKEY);

			cmd.romFilename = RTC_Extensions.getShortFilenameFromPath(GlobalWin.MainForm.CurrentlyOpenRom);

			if (!PeerHasRom(cmd.romFilename))
				cmd.romData = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

			cmd.stashkey = RTC_StockpileManager.currentStashkey;
			cmd.stashkey.EmbedState();

			cmd.Priority = true;

			SendCommand(cmd, false);
		}

		private RTC_Command GetLatestScreenFrame(LinkedList<RTC_Command> cmdQueue)
		{
			RTC_Command cmd = null;

			try
			{
				Stack<RTC_Command> vidcmds = new Stack<RTC_Command>();

				foreach (var vidcmd in cmdQueue.ToArray())
					if (vidcmd.Type == CommandType.PUSHSCREEN)
						vidcmds.Push(vidcmd);

				cmd = vidcmds.Peek();

				foreach (var vidcmd in vidcmds)
					cmdQueue.Remove(vidcmd);
			}
			catch (Exception ex)
			{
				OutputException(ex);
			}

			return cmd;
		}

		public void UpdatePeerScreen(Image img)
		{
			if (RTC_Core.multiForm.btnPopoutPeerGameScreen.Visible == false)
				RTC_Core.multipeerpopoutForm.pbPeerScreen.Image = img;
			else
				RTC_Core.multiForm.pbPeerScreen.Image = img;
		}

		public void SendBlastlayer()
		{
			if (side == NetworkSide.DISCONNECTED)
				return;

			if (RTC_StockpileManager.currentStashkey == null || RTC_StockpileManager.currentStashkey.BlastLayer == null)
			{
				MessageBox.Show("Couldn't fetch BlastLayer from RTC_StockpileManager.currentStashkey");
				return;
			}

			RTC_Command cmd = new RTC_Command(CommandType.BLAST);
			cmd.blastlayer = RTC_StockpileManager.currentStashkey.BlastLayer;

			SendCommand(cmd, false);
		}
	}
}

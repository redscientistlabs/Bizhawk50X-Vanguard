using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using RTC.Legacy;
using RTCV.NetCore;

namespace RTC
{
	public class CorruptCoreConnector : IRoutable
	{

		public CorruptCoreConnector(FullSpec spec)
		{

		}

		public object OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			//Use setReturnValue to handle returns

			var message = e.message;
			var advancedMessage = message as NetCoreAdvancedMessage;

			switch (e.message.Type)
			{
				//HANDLE MESSAGES HERE
				case "ADVANCED MESSAGE THAT CONTAINS A VALUE":
				{
					object value = advancedMessage.objectValue; //This is how you get the value from a message
					break;
				}

				case "#!RETURNTEST": //ADVANCED MESSAGE (SYNCED) WANTS A RETURN VALUE
					e.setReturnValue(new Random(666));
					break;

				case "ASYNCBLAST":
				{
					BlastLayer bl = RTC_Corruptcore.Blast(null, RTC_MemoryDomains.SelectedDomains);
					if (bl != null)
						bl.Apply();
				}
				break;
				case "BLAST":
				{
					object[] value = advancedMessage.objectValue as object[];
					BlastLayer bl = (BlastLayer)value[0];
					string[] _domains = (string[])value[1];
					bool isReplay = (bool)value[2];

						if (_domains == null)
						_domains = RTC_MemoryDomains.SelectedDomains;

					if (bl != null)
					{
						bl.Apply(isReplay);
					}
					else
					{
						bl = RTC_Corruptcore.Blast(null, _domains);
					}

					if (advancedMessage.requestGuid != null)
					{
						e.setReturnValue(bl);
					}
				}
				break;

				case "STASHKEY":
				{
					var temp = advancedMessage.objectValue as object[];

					var sk = temp[0] as StashKey;
					var romFilename = temp[1] as String;
					var romData = temp[2] as Byte[];

					if (!File.Exists(RTC_EmuCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename))
					File.WriteAllBytes(RTC_EmuCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename, romData);

					sk.RomFilename = RTC_EmuCore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + RTC_Extensions.getShortFilenameFromPath(romFilename);
					sk.DeployState();
					sk.Run();
				}
				break;


				case "REMOTE_PUSHRTCSPEC":
					RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				case "REMOTE_PUSHEMUSPEC":
					RTC_EmuCore.EmuSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				case "REMOTE_PUSHRTCSPECUPDATE":
					RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;

				case "REMOTE_PUSHEMUSPECUPDATE":
					RTC_EmuCore.EmuSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;

				case "REMOTE_PUSHVMDS":
					RTC_MemoryDomains.VmdPool.Clear();
					foreach (var proto in (advancedMessage.objectValue as VmdPrototype[]))
						RTC_MemoryDomains.AddVMD(proto);
					break;

				case "BLASTGENERATOR_BLAST":
				{
					var temp = advancedMessage.objectValue as object[];
					var blastGeneratorProtos = (List<BlastGeneratorProto>)(temp[0]);
					var sk = (StashKey)(temp[1]);

					List<BlastGeneratorProto> returnList = RTC_BlastTools.GenerateBlastLayersFromBlastGeneratorProtos(blastGeneratorProtos, sk);

					if (advancedMessage.requestGuid != null)
						{
							e.setReturnValue(returnList);
						}
						break;
					}

				case "REMOTE_LOADROM":
				{
					var fileName = advancedMessage.objectValue as String;
					RTC_EmuCore.LoadRom_NET(fileName);
				}
				break;

				case "REMOTE_LOADSTATE":
					{
						StashKey sk = (StashKey)(advancedMessage.objectValue as object[])[0];
						bool reloadRom = (bool)(advancedMessage.objectValue as object[])[1];
						bool runBlastLayer = (bool)(advancedMessage.objectValue as object[])[2];

						bool returnValue = RTC_StockpileManager.LoadState_NET(sk, reloadRom);

						RTC_MemoryDomains.RefreshDomains(false);

						if (runBlastLayer)
						{
							//TODO
							//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command(CommandType.BLAST, { blastlayer = sk.BlastLayer, isReplay = true });
							RTC_Hooks.CPU_STEP(false, false, true);
						}


						e.setReturnValue(returnValue);
					}
					break;

				case "REMOTE_MERGECONFIG":
					Stockpile.MergeBizhawkConfig_NET();
					break;

				case "REMOTE_IMPORTKEYBINDS":
					Stockpile.ImportBizhawkKeybinds_NET();
					break;

				case "REMOTE_SAVESTATE":
					{
						StashKey sk = RTC_StockpileManager.SaveState_NET((bool)(advancedMessage.objectValue as object[])[0], (StashKey)(advancedMessage.objectValue as object[])[1]);
						if (advancedMessage.requestGuid != null)
						{
							e.setReturnValue(sk);
						}
					}
					break;

				case "REMOTE_BACKUPKEY_REQUEST":
					{
						if (!RTC_Hooks.isNormalAdvance)
							break;
						e.setReturnValue(new NetCoreAdvancedMessage("REMOTE_BACKUPKEY_STASH", RTC_StockpileManager.SaveState_NET(false, null, false)));
						break;
					}

				case "REMOTE_BACKUPKEY_STASH":
					RTC_StockpileManager.BackupedState = (StashKey)advancedMessage.objectValue;
					RTC_StockpileManager.AllBackupStates.Push((StashKey)advancedMessage.objectValue);
					S.GET<RTC_Core_Form>().btnGpJumpBack.Visible = true;
					S.GET<RTC_Core_Form>().btnGpJumpNow.Visible = true;
					break;

				case "REMOTE_DOMAIN_PEEKBYTE":
					e.setReturnValue(RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PeekByte((long)(advancedMessage.objectValue as object[])[1]));
					break;

				case "REMOTE_DOMAIN_POKEBYTE":
					RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PokeByte((long)(advancedMessage.objectValue as object[])[1], (byte)(advancedMessage.objectValue as object[])[2]);
					break;

				case "REMOTE_DOMAIN_GETDOMAINS":
					e.setReturnValue(RTC_MemoryDomains.GetInterfaces());
					break;

				case "REMOTE_DOMAIN_VMD_ADD":
					RTC_MemoryDomains.AddVMD((advancedMessage.objectValue as VmdPrototype));
					break;

				case "REMOTE_DOMAIN_VMD_REMOVE":
					RTC_MemoryDomains.RemoveVMD((advancedMessage.objectValue as string));
					break;

				case "REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP":
					RTC_MemoryDomains.GenerateActiveTableDump((string)(advancedMessage.objectValue as object[])[0], (string)(advancedMessage.objectValue as object[])[1]);
					break;

				case "REMOTE_DOMAIN_SETSELECTEDDOMAINS":
					RTC_MemoryDomains.UpdateSelectedDomains((string[])advancedMessage.objectValue);
					break;

				case "REMOTE_DOMAIN_SYSTEM":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper());
					break;

				case "REMOTE_DOMAIN_SYSTEMPREFIX":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX());
					break;

				case "REMOTE_KEY_PUSHSAVESTATEDICO":
					{
						var key = (string)(advancedMessage.objectValue as object[])[1];
						var sk = (StashKey)((advancedMessage.objectValue as object[])[0]);
						RTC_StockpileManager.SavestateStashkeyDico[key] = sk;
						S.GET<RTC_GlitchHarvester_Form>().RefreshSavestateTextboxes();
					}
					break;

				case "REMOTE_KEY_GETSYSTEMNAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_FILESYSTEMCORENAME());
					break;

				case "REMOTE_KEY_GETSYSTEMCORE":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_SYSTEMCORENAME((string)advancedMessage.objectValue));
					break;

				case "REMOTE_KEY_GETGAMENAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_FILESYSTEMGAMENAME());
					break;

				case "REMOTE_KEY_GETSETSYNCSETTINGS":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS);
					break;

				case "REMOTE_KEY_GETOPENROMFILENAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM());
					break;

				case "REMOTE_KEY_GETRAWBLASTLAYER":
					e.setReturnValue(RTC_StockpileManager.GetRawBlastlayer());
					break;
				case "REMOTE_KEY_GETBAKEDLAYER":
					{
						//We need a stashkey to load the game 
						var temp = advancedMessage.objectValue as object[];
						var _bl = temp[0] as BlastLayer;
						var sk = temp[1] as StashKey;
						e.setReturnValue(RTC_BlastTools.GetAppliedBackupLayer(_bl, sk));
						break;
					}


				case "BIZHAWK_OPEN_HEXEDITOR_ADDRESS":
					{
						var temp = advancedMessage.objectValue as object[];
						string domain = (string)temp[0];
						long address = (long)temp[1];

						MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
						long realAddress = RTC_MemoryDomains.GetRealAddress(domain, address);

						RTC_Hooks.BIZHAWK_OPEN_HEXEDITOR_ADDRESS(mdp, realAddress);

						break;
					}

				case "REMOTE_SET_RESTOREBLASTLAYERBACKUP":
					if (RTC_StockpileManager.LastBlastLayerBackup != null)
						RTC_StockpileManager.LastBlastLayerBackup.Apply(true);
					break;

				case "REMOTE_SET_STEPACTIONS_CLEARALLBLASTUNITS":
					RTC_StepActions.ClearStepBlastUnits();
					break;
				case "REMOTE_SET_STEPACTIONS_REMOVEEXCESSINFINITEUNITS":
					RTC_StepActions.RemoveExcessInfiniteStepUnits();
					break;

				case "REMOTE_EVENT_LOADGAMEDONE_NEWGAME":

					if (RTC_NetcoreImplementation.isStandaloneUI && RTC_GameProtection.isRunning)
						RTC_GameProtection.Reset();

					RTC_Corruptcore.AutoCorrupt = false;
					//RTC_StockpileManager.isCorruptionApplied = false;
					S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
					S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
					S.GET<RTC_CorruptionEngine_Form>().UpdateDefaultPrecision();
					break;
				case "REMOTE_EVENT_LOADGAMEDONE_SAMEGAME":
					//RTC_StockpileManager.isCorruptionApplied = false;
					S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
					S.GET<RTC_CorruptionEngine_Form>().UpdateDefaultPrecision();
					break;

				case "REMOTE_EVENT_CLOSEBIZHAWK":
					RTC_Hooks.BIZHAWK_MAINFORM_CLOSE();
					break;

				case "REMOTE_EVENT_SAVEBIZHAWKCONFIG":
					RTC_Hooks.BIZHAWK_MAINFORM_SAVECONFIG();
					break;

				case "REMOTE_EVENT_BIZHAWKSTARTED":
					if (RTC_StockpileManager.BackupedState == null)
						S.GET<RTC_Core_Form>().AutoCorrupt = false;


					//Todo
					//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command("REMOTE_PUSHVMDS) { objectValue = RTC_MemoryDomains.VmdPool.Values.Select(it => (it as VirtualMemoryDomain).Proto).ToArray() }, true, true);

					Thread.Sleep(100);

					if (RTC_StockpileManager.BackupedState != null)
						S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(RTC_StockpileManager.BackupedState.SelectedDomains.ToArray());

					if (S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
						RTC_GameProtection.Start();

					break;

				case "REMOTE_HOTKEY_MANUALBLAST":
					S.GET<RTC_Core_Form>().btnManualBlast_Click(null, null);
					break;

				case "REMOTE_HOTKEY_AUTOCORRUPTTOGGLE":
					S.GET<RTC_Core_Form>().btnAutoCorrupt_Click(null, null);
					break;
				case "REMOTE_HOTKEY_ERRORDELAYDECREASE":
					if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value > 1)
						S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value--;
					break;

				case "REMOTE_HOTKEY_ERRORDELAYINCREASE":
					if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value < S.GET<RTC_GeneralParameters_Form>().track_ErrorDelay.Maximum)
						S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value++;
					break;

				case "REMOTE_HOTKEY_INTENSITYDECREASE":
					if (S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value > 1)
						S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value--;
					break;

				case "REMOTE_HOTKEY_INTENSITYINCREASE":
					if (S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value < S.GET<RTC_GeneralParameters_Form>().track_Intensity.Maximum)
						S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value++;
					break;

				case "REMOTE_HOTKEY_GHLOADCORRUPT":
					if (!RTC_NetCore.NetCoreCommandSynclock)
					{
						RTC_NetCore.NetCoreCommandSynclock = true;

						S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = true;
						S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);

						RTC_NetCore.NetCoreCommandSynclock = false;
					}
					break;

				case "REMOTE_HOTKEY_GHCORRUPT":
					if (!RTC_NetCore.NetCoreCommandSynclock)
					{
						RTC_NetCore.NetCoreCommandSynclock = true;
						RTC_Corruptcore.CorruptCoreSpec.Update(CCSPEC.STEP_RUNBEFORE.ToString(), true);


						bool isload = S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked;
						S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = false;
						S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);
						S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = isload;

						RTC_NetCore.NetCoreCommandSynclock = false;
					}
					break;

				case "REMOTE_HOTKEY_GHLOAD":
					S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "LOAD";
					S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
					break;
				case "REMOTE_HOTKEY_GHSAVE":
					S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "SAVE";
					S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
					break;
				case "REMOTE_HOTKEY_GHSTASHTOSTOCKPILE":
					S.GET<RTC_GlitchHarvester_Form>().AddStashToStockpile(false);
					break;

				case "REMOTE_HOTKEY_SENDRAWSTASH":
					S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
					break;

				case "REMOTE_HOTKEY_BLASTRAWSTASH":
					RTC_Corruptcore.CorruptCoreSpec.Update(CCSPEC.STEP_RUNBEFORE.ToString(), true);
	
					//Todo
					//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command("ASYNCBLAST));

					S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
					break;
				case "REMOTE_HOTKEY_BLASTLAYERTOGGLE":
					S.GET<RTC_GlitchHarvester_Form>().btnBlastToggle_Click(null, null);
					break;
				case "REMOTE_HOTKEY_BLASTLAYERREBLAST":

					if (RTC_StockpileManager.CurrentStashkey == null || RTC_StockpileManager.CurrentStashkey.BlastLayer.Layer.Count == 0)
					{
						S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = false;
						break;
					}

					S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = true;
					//Todo
					//RTC_NetcoreImplementation.SendCommandToRTC(new RTC_Command("BLAST) { blastlayer = RTC_StockpileManager.CurrentStashkey.BlastLayer });
					break;

				case "REMOTE_RENDER_START":
					RTC_Render.StartRender_NET();
					break;

				case "REMOTE_RENDER_STOP":
					RTC_Render.StopRender_NET();
					break;

				case "REMOTE_RENDER_SETTYPE":
					RTC_Render.lastType = (RENDERTYPE)advancedMessage.objectValue;
					break;

				case "REMOTE_RENDER_STARTED":
					S.GET<RTC_GlitchHarvester_Form>().btnRender.Text = "Stop Render";
					S.GET<RTC_GlitchHarvester_Form>().btnRender.ForeColor = Color.GreenYellow;
					break;

				case "REMOTE_RENDER_RENDERATLOAD":
					RTC_StockpileManager.RenderAtLoad = (bool)advancedMessage.objectValue;
					break;

				default:
					new object();
					break;
			}

			return e.returnMessage;
		}


		public void Kill()
		{

		}
	}
}

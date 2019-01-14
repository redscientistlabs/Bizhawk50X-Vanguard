using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CorruptCore;
using RTCV.NetCore;
using static CorruptCore.NetcoreCommands;

namespace RTCV.CorruptCore
{
	public class CorruptCoreConnector : IRoutable
	{

		public CorruptCoreConnector()
		{
			//spec.Side = RTCV.NetCore.NetworkSide.CLIENT;
		}

		public object OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			//Use setReturnValue to handle returns

			var message = e.message;
			var advancedMessage = message as NetCoreAdvancedMessage;

			switch (e.message.Type)
			{
				//UI sent its spec
				case REMOTE_PUSHUISPEC:
					RTC_Corruptcore.UISpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				//UI sent a spec update
				case REMOTE_PUSHUISPECUPDATE:
					RTC_Corruptcore.UISpec?.Update((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				//Vanguard sent a copy of its spec
				case REMOTE_PUSHEMUSPEC:
					RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				//Vanguard sent a spec update
				case REMOTE_PUSHEMUSPECUPDATE:
					RTC_Corruptcore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;

				//UI sent a copy of the CorruptCore spec
				case REMOTE_PUSHCORRUPTCORESPEC:
					RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					RTC_Corruptcore.CorruptCoreSpec.SpecUpdated += (o, ea) =>
					{
						PartialSpec partial = ea.partialSpec;

						LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
					};
					e.setReturnValue(true);
					break;

				//UI sent an update of the CorruptCore spec
				case REMOTE_PUSHCORRUPTCORESPECUPDATE:
					RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					e.setReturnValue(true);
					break;

				case REMOTE_EVENT_DOMAINSUPDATED:
					RTC_MemoryDomains.RefreshDomains();
				//	LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED);
					break;

				case ASYNCBLAST:
					{
						RTC_Corruptcore.ASyncGenerateAndBlast();
					}
					break;

				case GENERATEBLASTLAYER:
				{
					string[] domains = advancedMessage.objectValue as string[];
					BlastLayer bl = RTC_Corruptcore.GenerateBlastLayer(domains);
					if (advancedMessage.requestGuid != null)
					{
						e.setReturnValue(bl);
					}

					break;
				}
				case APPLYBLASTLAYER:
					{
						BlastLayer bl = advancedMessage.objectValue as BlastLayer;
						bl.Apply();
					}
					break;

				case STASHKEY:
					{
						var temp = advancedMessage.objectValue as object[];

						var sk = temp[0] as StashKey;
						var romFilename = temp[1] as String;
						var romData = temp[2] as Byte[];

						if (!File.Exists(RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename))
							File.WriteAllBytes(RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + romFilename, romData);

						sk.RomFilename = RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "WORKING" + Path.DirectorySeparatorChar + "SKS" + Path.DirectorySeparatorChar + RTC_Extensions.getShortFilenameFromPath(romFilename);
						sk.DeployState();
						sk.Run();
					}
					break;


				case REMOTE_PUSHRTCSPEC:
					RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				case REMOTE_PUSHRTCSPECUPDATE:
					RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;


				case REMOTE_PUSHVMDS:
					RTC_MemoryDomains.VmdPool.Clear();
					foreach (var proto in (advancedMessage.objectValue as VmdPrototype[]))
						RTC_MemoryDomains.AddVMD(proto);
					break;

				case BLASTGENERATOR_BLAST:
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


				case REMOTE_MERGECONFIG:
					Stockpile.MergeBizhawkConfig_NET();
					break;

				case REMOTE_IMPORTKEYBINDS:
					Stockpile.ImportBizhawkKeybinds_NET();
					break;


				case REMOTE_LOADSTATE:
				{
					StashKey sk = (StashKey)(advancedMessage.objectValue as object[])[0];
					bool reloadRom = (bool)(advancedMessage.objectValue as object[])[1];
					bool runBlastLayer = (bool)(advancedMessage.objectValue as object[])[2];

					bool returnValue = RTC_StockpileManager.LoadState_NET(sk, reloadRom, runBlastLayer);

					e.setReturnValue(returnValue);
				}
					break;
				case REMOTE_SAVESTATE:
					{
						StashKey sk = RTC_StockpileManager.SaveState_NET(advancedMessage.objectValue as StashKey); //Has to be nullable cast
						e.setReturnValue(sk);
					}
					break;

				case REMOTE_BACKUPKEY_REQUEST:
					{
					//	if (!RTC_Hooks.isNormalAdvance)
					//		break;
						e.setReturnValue(new NetCoreAdvancedMessage("REMOTE_BACKUPKEY_STASH", RTC_StockpileManager.SaveState_NET()));
						break;
					}

				case REMOTE_BACKUPKEY_STASH:
					RTC_StockpileManager.BackupedState = (StashKey)advancedMessage.objectValue;
					RTC_StockpileManager.AllBackupStates.Push((StashKey)advancedMessage.objectValue);
				//	S.GET<RTC_Core_Form>().btnGpJumpBack.Visible = true;
					//S.GET<RTC_Core_Form>().btnGpJumpNow.Visible = true;
					break;

				case REMOTE_DOMAIN_PEEKBYTE:
					e.setReturnValue(RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PeekByte((long)(advancedMessage.objectValue as object[])[1]));
					break;

				case REMOTE_DOMAIN_POKEBYTE:
					RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PokeByte((long)(advancedMessage.objectValue as object[])[1], (byte)(advancedMessage.objectValue as object[])[2]);
					break;

				case REMOTE_DOMAIN_GETDOMAINS:
					e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_GETDOMAINS, true));
					break;

				case REMOTE_DOMAIN_VMD_ADD:
					RTC_MemoryDomains.AddVMD((advancedMessage.objectValue as VmdPrototype));
					break;

				case REMOTE_DOMAIN_VMD_REMOVE:
					RTC_MemoryDomains.RemoveVMD((advancedMessage.objectValue as string));
					break;

				case REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP:
					RTC_MemoryDomains.GenerateActiveTableDump((string)(advancedMessage.objectValue as object[])[0], (string)(advancedMessage.objectValue as object[])[1]);
					break;

				/*
				case "REMOTE_DOMAIN_SETSELECTEDDOMAINS":
					RTC_MemoryDomains.UpdateSelectedDomains((string[])advancedMessage.objectValue);
					break;
					*/

				case REMOTE_KEY_PUSHSAVESTATEDICO:
					{
						var key = (string)(advancedMessage.objectValue as object[])[1];
						var sk = (StashKey)((advancedMessage.objectValue as object[])[0]);
						RTC_StockpileManager.SavestateStashkeyDico[key] = sk;
						//S.GET<RTC_GlitchHarvester_Form>().RefreshSavestateTextboxes();
					}
					break;

				case REMOTE_KEY_GETRAWBLASTLAYER:
					e.setReturnValue(RTC_StockpileManager.GetRawBlastlayer());
					break;


				case REMOTE_SET_RESTOREBLASTLAYERBACKUP:
					if (RTC_StockpileManager.LastBlastLayerBackup != null)
						RTC_StockpileManager.LastBlastLayerBackup.Apply(true);
					break;

				case REMOTE_SET_STEPACTIONS_CLEARALLBLASTUNITS:
					RTC_StepActions.ClearStepBlastUnits();
					break;
				case REMOTE_SET_STEPACTIONS_REMOVEEXCESSINFINITEUNITS:
					RTC_StepActions.RemoveExcessInfiniteStepUnits();
					break;


				case REMOTE_EVENT_LOADGAMEDONE_NEWGAME:
				{

					break;
				}

				/*
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
						RTC_Corruptcore.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE.ToString(), true);


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
					RTC_Corruptcore.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE.ToString(), true);

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
					*/

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

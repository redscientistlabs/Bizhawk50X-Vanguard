using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CorruptCore;
using NetCore;
using RTCV.CorruptCore;
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

				case "GETSPECDUMPS":
					StringBuilder sb = new StringBuilder();
					sb.AppendLine("Spec Dump from CorruptCore");
					sb.AppendLine();
					RTC_Corruptcore.UISpec?.GetDump().ForEach(x => sb.AppendLine(x));
					RTC_Corruptcore.CorruptCoreSpec?.GetDump().ForEach(x => sb.AppendLine(x));
					RTC_Corruptcore.VanguardSpec?.GetDump().ForEach(x => sb.AppendLine(x));

					return sb.ToString();
					break;
				//UI sent its spec
				case REMOTE_PUSHUISPEC:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.UISpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					}); 
					e.setReturnValue(true);
					break;

				//UI sent a spec update
				case REMOTE_PUSHUISPECUPDATE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.UISpec?.Update((PartialSpec)advancedMessage.objectValue);
					});
					e.setReturnValue(true);
					break;

				//Vanguard sent a copy of its spec
				case REMOTE_PUSHEMUSPEC:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					});
					e.setReturnValue(true);
					break;

				//Vanguard sent a spec update
				case REMOTE_PUSHEMUSPECUPDATE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					});
					break;

				//UI sent a copy of the CorruptCore spec
				case REMOTE_PUSHCORRUPTCORESPEC:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
						RTC_Corruptcore.CorruptCoreSpec.SpecUpdated += (ob, eas) =>
						{
							PartialSpec partial = eas.partialSpec;

							LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPECUPDATE, partial, true);
						};
					});
					e.setReturnValue(true);
					break;

				//UI sent an update of the CorruptCore spec
				case REMOTE_PUSHCORRUPTCORESPECUPDATE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					});
					e.setReturnValue(true);
					break;

				case REMOTE_EVENT_DOMAINSUPDATED:
					var domainsChanged = (bool)advancedMessage.objectValue;
					RTC_MemoryDomains.RefreshDomains();
					LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED, domainsChanged, true);
					break;

				case ASYNCBLAST:
					{
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							RTC_Corruptcore.ASyncGenerateAndBlast();
						});
					}
					break;

				case GENERATEBLASTLAYER:
				{
					string[] domains = advancedMessage.objectValue as string[];

					BlastLayer bl = null;
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						bl = RTC_Corruptcore.GenerateBlastLayer(domains);
					});
					if (advancedMessage.requestGuid != null)
					{
						e.setReturnValue(bl);
					}
					break;
				}
				case APPLYBLASTLAYER:
				{
					var temp = advancedMessage.objectValue as object[];
					BlastLayer bl = (BlastLayer)temp[0];
					bool backup = (bool)temp[1];
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						bl.Apply(backup);
					});
				}
				break;

				/*
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
					*/


				case REMOTE_PUSHRTCSPEC:
					RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				case REMOTE_PUSHRTCSPECUPDATE:
					RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;


				case BLASTGENERATOR_BLAST:
					{
						var temp = advancedMessage.objectValue as object[];
						var blastGeneratorProtos = (List<BlastGeneratorProto>)(temp[0]);
						var sk = (StashKey)(temp[1]);

						List<BlastGeneratorProto> returnList = null;
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							returnList = RTC_BlastTools.GenerateBlastLayersFromBlastGeneratorProtos(blastGeneratorProtos, sk);
						});

						if (advancedMessage.requestGuid != null)
						{
							e.setReturnValue(returnList);
						}
						break;
					}

				case REMOTE_LOADSTATE:
				{
					StashKey sk = (StashKey)(advancedMessage.objectValue as object[])[0];
					bool reloadRom = (bool)(advancedMessage.objectValue as object[])[1];
					bool runBlastLayer = (bool)(advancedMessage.objectValue as object[])[2];
					bool returnValue = false;
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						returnValue = RTC_StockpileManager_EmuSide.LoadState_NET(sk, reloadRom, runBlastLayer);
					});

					e.setReturnValue(returnValue);
				}
				break;
				case REMOTE_SAVESTATE:
				{
					StashKey sk = null;
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						sk = RTC_StockpileManager_EmuSide.SaveState_NET(advancedMessage.objectValue as StashKey); //Has to be nullable cast
					});
					e.setReturnValue(sk);
				}
					break;

				case REMOTE_BACKUPKEY_REQUEST:
					{
						//We don't store this in the spec as it'd be horrible to push it to the UI and it doesn't care
						if (!LocalNetCoreRouter.QueryRoute<bool>(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ISNORMALADVANCE))
							break;
						StashKey sk = null;
						//We send an unsynced command back
						SyncObjectSingleton.FormExecute((o, ea) =>
							{
								sk = RTC_StockpileManager_EmuSide.SaveState_NET();
							});

						LocalNetCoreRouter.Route(NetcoreCommands.UI, REMOTE_BACKUPKEY_STASH, sk, false);
						break;
					}
					

				case REMOTE_DOMAIN_PEEKBYTE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						e.setReturnValue(RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PeekByte((long)(advancedMessage.objectValue as object[])[1]));
					});
					break;

				case REMOTE_DOMAIN_POKEBYTE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_MemoryDomains.GetInterface((string)(advancedMessage.objectValue as object[])[0]).PokeByte((long)(advancedMessage.objectValue as object[])[1], (byte)(advancedMessage.objectValue as object[])[2]);
					});
					break;

				case REMOTE_DOMAIN_GETDOMAINS:
					e.setReturnValue(LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_GETDOMAINS, true));
					break;


				case REMOTE_PUSHVMDPROTOS:
					RTC_MemoryDomains.VmdPool.Clear();
					foreach (var proto in (advancedMessage.objectValue as VmdPrototype[]))
						RTC_MemoryDomains.AddVMD(proto);
					break;

				case REMOTE_DOMAIN_VMD_ADD:
					RTC_MemoryDomains.AddVMD_NET((advancedMessage.objectValue as VmdPrototype));
					break;

				case REMOTE_DOMAIN_VMD_REMOVE:
					RTC_MemoryDomains.RemoveVMD_NET((advancedMessage.objectValue as string));
					break;

				case REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_MemoryDomains.GenerateActiveTableDump((string)(advancedMessage.objectValue as object[])[0], (string)(advancedMessage.objectValue as object[])[1]);
					}); 
					break;

				/*
				case "REMOTE_DOMAIN_SETSELECTEDDOMAINS":
					RTC_MemoryDomains.UpdateSelectedDomains((string[])advancedMessage.objectValue);
					break;
					*/

				case REMOTE_KEY_PUSHSAVESTATEDICO:
					{
						//var key = (string)(advancedMessage.objectValue as object[])[1];
						//var sk = (StashKey)((advancedMessage.objectValue as object[])[0]);
						//RTC_StockpileManager.SavestateStashkeyDico[key] = sk;
						//S.GET<RTC_GlitchHarvester_Form>().RefreshSavestateTextboxes();
					}
					break;

				case REMOTE_KEY_GETRAWBLASTLAYER:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						e.setReturnValue(RTC_StockpileManager_EmuSide.GetRawBlastlayer());
					});
					break;


				case REMOTE_SET_APPLYUNCORRUPTBL:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (RTC_StockpileManager_EmuSide.UnCorruptBL != null)
							RTC_StockpileManager_EmuSide.UnCorruptBL.Apply(true);
					});
					break;

				case REMOTE_SET_APPLYCORRUPTBL:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (RTC_StockpileManager_EmuSide.CorruptBL != null)
							RTC_StockpileManager_EmuSide.CorruptBL.Apply(false);
					});
					break;


				case REMOTE_SET_STEPACTIONS_CLEARALLBLASTUNITS:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_StepActions.ClearStepBlastUnits();
					});

					break;
				case REMOTE_SET_STEPACTIONS_REMOVEEXCESSINFINITEUNITS:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_StepActions.RemoveExcessInfiniteStepUnits();
					});
					break;


				case REMOTE_EVENT_CLOSEEMULATOR:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						Application.Exit();
					});
					break;

					
				case REMOTE_HOTKEY_MANUALBLAST:
					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, ASYNCBLAST);
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

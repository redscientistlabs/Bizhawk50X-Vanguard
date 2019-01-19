using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorruptCore;
using NetCore;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;
using static CorruptCore.NetcoreCommands;

namespace UI
{
	public static class UI_VanguardImplementation
	{
			public static UIConnector connector = null;



			public static void StartServer()
			{
				ConsoleEx.WriteLine("Starting UI Vanguard Implementation");

				var spec = new NetCoreReceiver();
				spec.MessageReceived += OnMessageReceived;

				spec.Attached = RTC_Corruptcore.Attached;

				connector = new UIConnector(spec);
			}

			public static void RestartServer()
			{
				connector.Kill();
				connector = null;
				StartServer();
			}

			private static void OnMessageReceived(object sender, NetCoreEventArgs e)
			{
				var message = e.message;
				var simpleMessage = message as NetCoreSimpleMessage;
				var advancedMessage = message as NetCoreAdvancedMessage;

				switch (message.Type) //Handle received messages here
				{
				case REMOTE_PUSHEMUSPEC:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue, !RTC_Corruptcore.Attached);
						e.setReturnValue(true);

						//Push the UI and CorruptCore spec (since we're master)
						LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPEC, RTC_Corruptcore.UISpec.GetPartialSpec(), true);
						LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPEC, RTC_Corruptcore.CorruptCoreSpec.GetPartialSpec(), true);

						//Specs are all set up so UI is clear.
						LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ALLSPECSSENT, true);
					});
					break;


				case REMOTE_ALLSPECSSENT:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_Core_Form>().Show();
						//S.GET<RTC_Test_Form>().Show();
						if (RTC_UICore.FirstConnect)
						{
							RTC_UICore.FirstConnect = false;
							S.GET<RTC_Core_Form>().btnEngineConfig_Click(null, null);
						}
						else
						{
							//Push the VMDs since we store them out of spec
							var vmdProtos = RTC_MemoryDomains.VmdPool.Values.Cast<VirtualMemoryDomain>().Select(x => x.Proto).ToArray();
							LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHVMDPROTOS, vmdProtos, true);

							//Return to the main form
							S.GET<RTC_Core_Form>().ShowPanelForm(S.GET<RTC_Core_Form>().previousForm, false);

							//Unhide the GH
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Size = S.GET<RTC_GlitchHarvester_Form>().Size;
							S.GET<RTC_GlitchHarvester_Form>().pnHideGlitchHarvester.Hide();
						}

						S.GET<RTC_Core_Form>().pbAutoKillSwitchTimeout.Value = 0;

						if(!RTC_Corruptcore.Attached)
						RTC_AutoKillSwitch.Enabled = true;
					});
					break;

				case REMOTE_PUSHEMUSPECUPDATE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
					});
					e.setReturnValue(true);
					break;

				//CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
				case REMOTE_PUSHCORRUPTCORESPECUPDATE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					});
					e.setReturnValue(true);
					break;

				case REMOTE_EVENT_DOMAINSUPDATED:

					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						var domainsChanged = advancedMessage?.objectValue as bool?; // We want to be able to send this unsynced so use a nullable cast
						if (domainsChanged != null && domainsChanged == true)
						{
							S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
							S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains((string[])RTC_Corruptcore.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS.ToString()]);
						}
						else
						{

							S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
						}
					});
					break;
				case ERROR_DISABLE_AUTOCORRUPT:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_Core_Form>().AutoCorrupt = false;
					});
					break;
					

				case REMOTE_HOTKEY_AUTOCORRUPTTOGGLE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_Core_Form>().btnAutoCorrupt_Click(null, null);
					});
					break;
				case REMOTE_HOTKEY_ERRORDELAYDECREASE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value > 1)
							S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value--;
					});
					break;

				case REMOTE_HOTKEY_ERRORDELAYINCREASE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value < S.GET<RTC_GeneralParameters_Form>().track_ErrorDelay.Maximum)
							S.GET<RTC_GeneralParameters_Form>().nmErrorDelay.Value++;
					});
					break;

				case REMOTE_HOTKEY_INTENSITYDECREASE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value > 1)
							S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value--;
					});
					break;

				case REMOTE_HOTKEY_INTENSITYINCREASE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value < S.GET<RTC_GeneralParameters_Form>().track_Intensity.Maximum)
							S.GET<RTC_GeneralParameters_Form>().nmIntensity.Value++;
					});
					break;

				case REMOTE_HOTKEY_GHLOADCORRUPT:
					{
						SyncObjectSingleton.FormExecute((o, ea) =>
						{
							S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = true;
							S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);
						});
					}
					break;

				case REMOTE_HOTKEY_GHCORRUPT:
					RTC_Corruptcore.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE.ToString(), true);

					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						bool isload = S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked;
						S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = false;
						S.GET<RTC_GlitchHarvester_Form>().btnCorrupt_Click(null, null);
						S.GET<RTC_GlitchHarvester_Form>().cbAutoLoadState.Checked = isload;
					});

					break;

				case REMOTE_HOTKEY_GHLOAD:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "LOAD";
						S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
					});
					break;
				case REMOTE_HOTKEY_GHSAVE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad.Text = "SAVE";
						S.GET<RTC_GlitchHarvester_Form>().btnSaveLoad_Click(null, null);
					});
					break;
				case REMOTE_HOTKEY_GHSTASHTOSTOCKPILE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_GlitchHarvester_Form>().AddStashToStockpile(false);
					});
					break;

				case REMOTE_HOTKEY_SENDRAWSTASH:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
					});
					break;

				case REMOTE_HOTKEY_BLASTRAWSTASH:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						RTC_Corruptcore.CorruptCoreSpec.Update(VSPEC.STEP_RUNBEFORE.ToString(), true);
						LocalNetCoreRouter.Route(CORRUPTCORE, ASYNCBLAST, null, true);

						S.GET<RTC_GlitchHarvester_Form>().btnSendRaw_Click(null, null);
					});
					break;
				case REMOTE_HOTKEY_BLASTLAYERTOGGLE:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						S.GET<RTC_GlitchHarvester_Form>().btnBlastToggle_Click(null, null);
					});
					break;
				case REMOTE_HOTKEY_BLASTLAYERREBLAST:
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						if (RTC_StockpileManager_UISide.CurrentStashkey == null || RTC_StockpileManager_UISide.CurrentStashkey.BlastLayer.Layer.Count == 0)
						{
							S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = false;
							return;
						}
						S.GET<RTC_GlitchHarvester_Form>().IsCorruptionApplied = true;
						RTC_StockpileManager_UISide.ApplyStashkey(RTC_StockpileManager_UISide.CurrentStashkey, false);
					});
					break;

				case REMOTE_BACKUPKEY_STASH:
					RTC_StockpileManager_UISide.BackupedState = (StashKey)advancedMessage.objectValue;
					RTC_StockpileManager_UISide.AllBackupStates.Push((StashKey)advancedMessage.objectValue);
					SyncObjectSingleton.FormExecute((o, ea) =>
					{
						UI_Extensions.S.GET<RTC_Core_Form>().btnGpJumpBack.Visible = true;
						UI_Extensions.S.GET<RTC_Core_Form>().btnGpJumpNow.Visible = true;
					});
					break;

				case KILLSWITCH_PULSE:
					RTC_AutoKillSwitch.Pulse();
					break;

			}
		}
	}
}

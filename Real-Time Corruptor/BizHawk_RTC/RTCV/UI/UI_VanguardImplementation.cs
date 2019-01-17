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
						RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
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

			}
		}
	}
}

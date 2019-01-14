using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorruptCore;
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



			public static void StartServer(ISynchronizeInvoke syncObject)
			{
				ConsoleEx.WriteLine("Starting UI Vanguard Implementation");

				var spec = new NetCoreReceiver();
				spec.MessageReceived += OnMessageReceived;

				connector = new UIConnector(spec, syncObject);
			}

			public static void RestartServer()
			{
				ISynchronizeInvoke temp = connector.netConn.spec.syncObject;
				connector.Kill();
				connector = null;
				StartServer(temp);
			}

			private static void OnMessageReceived(object sender, NetCoreEventArgs e)
			{
				var message = e.message;
				var simpleMessage = message as NetCoreSimpleMessage;
				var advancedMessage = message as NetCoreAdvancedMessage;

				switch (message.Type) //Handle received messages here
				{
				case REMOTE_PUSHEMUSPEC:
					RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);

					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHUISPEC, RTC_Corruptcore.UISpec.GetPartialSpec(), true);
					LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPEC, RTC_Corruptcore.CorruptCoreSpec.GetPartialSpec(), true);
					LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_ALLSPECSSENT,true);
					break;


				case REMOTE_ALLSPECSSENT:
					S.GET<RTC_Core_Form>().Show();
					break;

				case REMOTE_PUSHEMUSPECUPDATE:
						RTC_Corruptcore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
						e.setReturnValue(true);
						break;

				//CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
				case REMOTE_PUSHCORRUPTCORESPECUPDATE:
						RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
						e.setReturnValue(true);
						break;

				case REMOTE_EVENT_DOMAINSUPDATED:
					var domainsChanged = advancedMessage?.objectValue as bool?; // We want to be able to send this unsynced so use a nullable cast
					if (domainsChanged != null && domainsChanged ==  true)
					{
						S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
						S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains((string[])RTC_Corruptcore.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS.ToString()]);
					}
					else
					{
						S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
					}
						break;

			}
		}
	}
}

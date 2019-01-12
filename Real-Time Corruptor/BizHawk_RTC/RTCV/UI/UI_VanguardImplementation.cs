using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.UI;
using static RTCV.UI.UI_Extensions;

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
					case "REMOTE_PUSHEMUSPEC":
						RTC_Corruptcore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
						e.setReturnValue(true);
						break;

					case "REMOTE_PUSHEMUSPECUPDATE":
						RTC_Corruptcore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
						e.setReturnValue(true);
						break;

					case "REMOTE_PUSHCORRUPTCORESPEC":
						if (RTC_Corruptcore.CorruptCoreSpec != null)
						{
							LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_PUSHCORRUPTCORESPEC", RTC_Corruptcore.CorruptCoreSpec.GetPartialSpec(), true);
						}
						//If we have a copy of the corruptcore spec, send it over since it means there's an active session and we want to be the authority
						else
						{
							RTC_Corruptcore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);

							//Register the update action
							RTC_Corruptcore.CorruptCoreSpec.SpecUpdated += (o, ea) =>
							{
								PartialSpec partial = ea.partialSpec;

								LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_PUSHCORRUPTCORESPECUPDATE", partial, true);
							};
						}
						e.setReturnValue(true);
						break;

					//CorruptCore pushed its spec. Note the false on propogate (since we don't want a recursive loop)
					case "REMOTE_PUSHCORRUPTCORESPECUPDATE":
						RTC_Corruptcore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
						e.setReturnValue(true);
						break;

					case "REMOTE_EVENT_DOMAINSUPDATED":
						S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
						break;

			}
		}
	}
}

using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RTC
{
	public static class VanguardImplementation
	{
		public static RTCV.Vanguard.VanguardConnector connector = null;

		public static void StartClient()
		{
			Thread.Sleep(500); //When starting in Multiple Startup Project, the first try will be uncessful since
							   //the server takes a bit more time to start then the client.

			var spec = new NetCoreReceiver();
			spec.MessageReceived += OnMessageReceived;

			connector = new RTCV.Vanguard.VanguardConnector(spec);
		}

		public static void RestartClient()
		{
			connector.Kill();
			connector = null;
			StartClient();
		}

		private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			// This is where you implement interaction.
			// Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

			var message = e.message;
			var simpleMessage = message as NetCoreSimpleMessage;
			var advancedMessage = message as NetCoreAdvancedMessage;

			switch (message.Type) //Handle received messages here
			{

				case "JUST A MESSAGE":
					//do something
					break;

				case "ADVANCED MESSAGE THAT CONTAINS A VALUE":
					object value = advancedMessage.objectValue; //This is how you get the value from a message
					break;

				case "#!RETURNTEST": //ADVANCED MESSAGE (SYNCED) WANTS A RETURN VALUE
					e.setReturnValue(new Random(666));
					break;

				case "#!WAIT":
					ConsoleEx.WriteLine("Simulating 20 sec of workload");
					Thread.Sleep(20000);
					break;

				case "#!HANG":
					ConsoleEx.WriteLine("Hanging forever");
					Thread.Sleep(int.MaxValue);
					break;

				default:
					ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
					break;
			}

		}
	}
}

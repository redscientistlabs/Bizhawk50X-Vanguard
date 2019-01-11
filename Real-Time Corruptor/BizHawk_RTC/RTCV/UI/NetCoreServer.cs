using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using RTCV.NetCore;
using UI;

namespace RTCV.NetCore
{
    public static class NetCoreServer
    {
        public static NetCore.NetCoreConnector loopbackConnector = null;
        static NetCore.NetCoreConnector multiplayerConnector = null;
        
        public static void StartLoopback()
        {
            var spec = new NetCore.NetCoreSpec();
            spec.Side = NetCore.NetworkSide.SERVER;
            spec.Loopback = true;
            //spec.IP = "127.0.0.1";
            //spec.Port = 42069;
            spec.MessageReceived += OnMessageReceived;
            loopbackConnector = new NetCore.NetCoreConnector(spec);
        }

		public static void RestartLoopback()
		{
			loopbackConnector.Kill();
			loopbackConnector = null;
			StartLoopback();
		}

		public static void StopLoopback()
		{
			loopbackConnector.Stop(true);
			loopbackConnector = null;
		}

		public static void StartMultiplayer(int _Port)
        {
            var spec = new NetCore.NetCoreSpec();
            //spec.Side = NetCore.NetworkSide.SERVER;
            spec.Loopback = false;
            //spec.IP = "";
            spec.Port = _Port;

            multiplayerConnector = new NetCore.NetCoreConnector(spec);
        }

        private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{ // This is where you implement interaction
			// Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

			var message = e.message;
			var simpleMessage = message as NetCoreSimpleMessage;
			var advancedMessage = message as NetCoreAdvancedMessage;

			switch (message.Type) //Handle received messages here
			{
				case "REMOTE_PUSHEMUSPEC":
					RTC_UICore.VanguardSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				case "REMOTE_PUSHEMUSPECUPDATE":
					RTC_UICore.VanguardSpec?.Update((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				case "REMOTE_PUSHCORRUPTCORESPEC":
					RTC_UICore.CorruptCoreSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;

				case "REMOTE_PUSHCORRUPTCORECUPDATE":
					RTC_UICore.CorruptCoreSpec?.Update((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				default:
					ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
					break;
			}

		}
	}
}

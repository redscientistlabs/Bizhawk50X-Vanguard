using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
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
			spec.syncObject = S.GET<RTC_Core_Form>();
			//spec.ServerConnected += Spec_ServerConnected;
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
				default:
					ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
					break;
			}

		}
	}
}

using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CorruptCore;
using RTCV.CorruptCore;
using NetworkSide = RTCV.NetCore.NetworkSide;
using static RTCV.CorruptCore.RTC_Corruptcore;

namespace RTCV.Vanguard
{
    public class VanguardConnector : IRoutable
    {
        NetCoreReceiver receiver;

        CorruptCoreConnector corruptConn;
        NetCoreConnector netConn;

        public VanguardConnector(NetCoreReceiver _receiver, Form syncObject)
        {
            receiver = _receiver;

			LocalNetCoreRouter.registerEndpoint(this, "VANGUARD");
			corruptConn = new CorruptCoreConnector();
			LocalNetCoreRouter.registerEndpoint(corruptConn, "CORRUPTCORE");

			var netCoreSpec = new NetCoreSpec();
            netCoreSpec.Side = NetworkSide.CLIENT;
            netCoreSpec.MessageReceived += OnMessageReceivedProxy;
			netCoreSpec.ClientConnected += NetCoreSpec_ClientConnected;
			netCoreSpec.syncObject = syncObject;
			netConn = new NetCoreConnector(netCoreSpec);
			//netConn = LocalNetCoreRouter.registerEndpoint(new NetCoreConnector(netCoreSpec), "UI");
			LocalNetCoreRouter.registerEndpoint(netConn, "DEFAULT"); //Will send mesages to netcore if can't find the destination

			RTC_Corruptcore.Start();
		}


		private void NetCoreSpec_ClientConnected(object sender, EventArgs e)
		{
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHCORRUPTCORESPEC, RTC_Corruptcore.CorruptCoreSpec.GetPartialSpec(), true);

			//LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHVANGUARDSPEC, RTC_Corruptcore.CorruptCoreSpec.GetPartialSpec(), true);
		}

		public void OnMessageReceivedProxy(object sender, NetCoreEventArgs e) => OnMessageReceived(sender, e);
        public object OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            //No implementation here, we simply route and return

            if (e.message.Type.Contains('|'))
            {   //This needs to be routed

                var msgParts = e.message.Type.Split('|');
                string endpoint = msgParts[0];
                e.message.Type = msgParts[1]; //remove endpoint from type

                return NetCore.LocalNetCoreRouter.Route(endpoint, e);
            }
            else
            {   //This is for the Vanguard Implementation
                receiver.OnMessageReceived(e);
                return e.returnMessage;
            }

            
        }

        //Ship everything to netcore, any needed routing will be handled in there
        public void SendMessage(string message) => netConn.SendMessage(message);
        public void SendMessage(string message, object value) => netConn.SendMessage(message,value);
        public object SendSyncedMessage(string message) { return netConn.SendSyncedMessage(message); }
        public object SendSyncedMessage(string message, object value) { return netConn.SendSyncedMessage(message, value); }

        public void Kill()
        {

        }
    }
}

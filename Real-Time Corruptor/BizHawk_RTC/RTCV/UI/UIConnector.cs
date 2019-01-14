﻿using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CorruptCore;
using RTCV.CorruptCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	public class UIConnector : IRoutable
	{
		NetCoreReceiver receiver;
		public NetCoreConnector netConn;

		public UIConnector(NetCoreReceiver _receiver, ISynchronizeInvoke syncObject)
		{
			receiver = _receiver;

			LocalNetCoreRouter.registerEndpoint(this, NetcoreCommands.UI);

			var netCoreSpec = new NetCore.NetCoreSpec();
			netCoreSpec.Side = NetCore.NetworkSide.SERVER;
			netCoreSpec.Loopback = true;
			netCoreSpec.syncObject = syncObject;
			netCoreSpec.MessageReceived += OnMessageReceivedProxy;
			netCoreSpec.ServerConnected += Spec_ServerConnected;

			netConn = new NetCoreConnector(netCoreSpec);
			LocalNetCoreRouter.registerEndpoint(netConn, "VANGUARD");
			LocalNetCoreRouter.registerEndpoint(netConn, "DEFAULT"); //Will send mesages to netcore if can't find the destination
		}

		private static void Spec_ServerConnected(object sender, EventArgs e)
		{
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
		public void SendMessage(string message, object value) => netConn.SendMessage(message, value);
		public object SendSyncedMessage(string message) { return netConn.SendSyncedMessage(message); }
		public object SendSyncedMessage(string message, object value) { return netConn.SendSyncedMessage(message, value); }

		public void Kill()
		{

		}
	}
}

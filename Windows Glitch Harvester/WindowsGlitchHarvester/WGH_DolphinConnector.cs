using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsGlitchHarvester
{
    public class WGH_DolphinConnector
    {

        WGH_SavestateInfoForm ssiForm;
        public NetCoreConnector connector = null;
        public NetCoreSpec spec = null;

        public static object peekedAddress;

        public WGH_DolphinConnector(WGH_SavestateInfoForm _form)
        {
            ssiForm = _form;
        }

        public void StartServer()
        {
            spec = new NetCoreSpec();

            spec.syncObject = ssiForm;

            spec.Side = NetworkSide.SERVER;
            spec.MessageReceived += OnMessageReceived;

            spec.ServerConnected += updateFormStatusLabel;
            spec.ServerConnectionLost += updateFormStatusLabel;
            spec.ServerDisconnected += updateFormStatusLabel;
            spec.ServerListening += updateFormStatusLabel;

            connector = new NetCoreConnector(spec);
        }

        public void updateFormStatusLabel(object sender, EventArgs e)
        {
            string text = $"Status: {spec.Connector.status}";

            ssiForm.lbStatus.Text = text;
        }
        

        public void StopServer(bool graceful = false)
        {
            if (graceful)
            {
                connector.Stop();
                connector = null;
            }
            else
            {
                connector.Kill();
                connector = null;
            }
        }

        public void RestartServer()
        {
            connector.Kill();
            connector = null;
            StartServer();
        }

        private void OnMessageReceived(object sender, NetCoreEventArgs e)
        {
            // This is where you implement interaction
            // Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

            var message = e.message;
            var simpleMessage = message as NetCoreSimpleMessage;
            var advancedMessage = message as NetCoreAdvancedMessage;    

            switch (message.Type) //Handle received messages here
            {

                case "WII":
                    WGH_SavestateInfoForm.isWii = true;
                    break;

                case "GAMECUBE":
                    WGH_SavestateInfoForm.isWii = false;
                    break;

                default:
                    ConsoleEx.WriteLine($"Received unassigned {(message is NetCoreAdvancedMessage ? "advanced " : "")}message \"{message.Type}\"");
                    break;
            }

        }

    }
}

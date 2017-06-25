using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.Xml.Serialization;

namespace RTC
{
    public static class RTC_GameProtection
    {
		static Timer t;
		public static int BackupInterval = 15;
		public static bool isRunning = false;

        public static void Start()
        {
			if (t == null)
			{
				t = new Timer();
				t.Tick += new EventHandler(Tick);
			}

			t.Interval = Convert.ToInt32(BackupInterval) * 1000;
            t.Start();

			isRunning = true;

			if (RTC_Core.RemoteRTC.PeerCommandQueue.Count > 0)
				foreach (var item in RTC_Core.RemoteRTC.PeerCommandQueue)
					if (item.Type == CommandType.REMOTE_BACKUPKEY_REQUEST)
						RTC_Core.RemoteRTC.PeerCommandQueue.Remove(item);

		}

        public static void Stop()
        {
            if (t != null)
                t.Stop();

			isRunning = false;

		}

        public static void Reset()
        {
			Stop();
			Start();
		}

        

        static void Tick(object Sender, EventArgs e)
        {
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_BACKUPKEY_REQUEST));
        }


    }

}

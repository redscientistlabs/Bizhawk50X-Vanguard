using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace RTC
{


    public static class RTC_RPC
    {
        private static string ip = "127.0.0.1";
        private static int listenPort = 56666;
        private static UdpClient publisher = new UdpClient(ip, listenPort);

        public static void Send(string msg, string[] args)
        {
            foreach (string item in args)
            {
                try
                {
                    // Uses the full filename received in arguments and copies the rom to the current folder as CorruptedROM.rom
                    string filename = item.Substring(item.LastIndexOf('\\') + 1, item.Length - (item.LastIndexOf('\\') + 1));
                    string fullpath = AppDomain.CurrentDomain.BaseDirectory + "CorruptedROM.rom";

                    if (item == fullpath)
                        continue;

                    if (File.Exists(fullpath))
                        File.Delete(fullpath);

                    File.Copy(item, fullpath);
                }
                catch { } //srsly no
            }


            Byte[] sdata = Encoding.ASCII.GetBytes(msg);
            publisher.Send(sdata, sdata.Length);
        }

    }
}

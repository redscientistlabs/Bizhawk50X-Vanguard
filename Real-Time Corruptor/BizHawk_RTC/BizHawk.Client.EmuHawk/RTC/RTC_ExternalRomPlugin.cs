using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using BizHawk.Client.EmuHawk;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace RTC
{

    public static class RTC_ExternalRomPlugin
    {
        public static string CorruptedRom = "CorruptedROM.rom";
        public static string SelectedPlugin = null;
        public static string PluginFilename = null;

        public static string LastOpenedPlugin = null;

        public static BlastUnit GetUnit()
        {
            return null;
        }

        public static BlastLayer GetBlastLayer()
        {
            if (!File.Exists(CorruptedRom))
            {
                MessageBox.Show("Null Plugin: You must have CorruptedROM.rom in your BizHawk folder");
                return null;
            }

            byte[] Corrupt = File.ReadAllBytes("CorruptedROM.rom");
            byte[] Original = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);

            if (Original.Length != Corrupt.Length)
            {
                MessageBox.Show("Error: The corrupted rom isn't the same size as the original one");
                return null;
            }

            return (GetBlastLayer(Original, Corrupt));

        }

        public static BlastLayer GetBlastLayer(byte[] Original, byte[] Corrupt)
        {


            BlastLayer bl = new BlastLayer();

            string thisSystem = Global.Game.System;
            string _primarydomain = "";
            string _seconddomain = "";
            int skipbytes = 0;



            switch (thisSystem.ToUpper())
            {
                case "NES":
                    _primarydomain = "PRG ROM";
                    _seconddomain = "CHR VROM";
                    skipbytes = 16;
                    break;

                case "SNES":
                    _primarydomain = "CARTROM";
                    break;

                case "A78":
                    _primarydomain = "HSC ROM";
                    break;

                case "LYNX":
                    _primarydomain = "Cart A";
                    skipbytes = 64;
                    break;

                case "N64":
                case "GB":
                case "GBC":
                case "SMS":
                case "GEN":
                case "GBA":
                case "PCE":
                case "GG":
                case "SG":
                case "SGX":
                case "WSWAN":
                    _primarydomain = "ROM";
                    break;


                case "PCECD":
                case "SAT":
                case "PSX":
                    MessageBox.Show("Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...");
                    return null;

            }

            if(Original.Length != Corrupt.Length)
            {
                MessageBox.Show("ERROR, ROM SIZE MISMATCH");
                return null;
            }

            long maxaddress = RTC_MemoryDomains.getInterface(_primarydomain).Size;

            for (int i = 0; i < Original.Length; i++)
            {
                if (Original[i] != Corrupt[i] && i >= skipbytes)
                {
                    if (i - skipbytes >= maxaddress)
                        bl.Layer.Add(new BlastByte(_seconddomain, (i - skipbytes) - maxaddress, BlastByteType.SET, Convert.ToInt32(Corrupt[i]), true));
                    else
                        bl.Layer.Add(new BlastByte(_primarydomain, i - skipbytes, BlastByteType.SET, Convert.ToInt32(Corrupt[i]), true));
                }
            }


            if (bl.Layer.Count == 0)
                return null;
            else
                return bl;



        }

        public static void KillLastPlugin()
        {
            if (LastOpenedPlugin != null)
            {
                ProcessStartInfo psi = new ProcessStartInfo("taskkill", "/F /IM \"" + LastOpenedPlugin + "\"");
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                Process.Start(psi);
            }
        }

        public static void OpenWindow()
        {
            if (SelectedPlugin == null || SelectedPlugin == "NULL")
            {
				RTC_Core.StopSound();
                MessageBox.Show("The Null Plugin allows you to manually bind an external ROM corruptor to RTC. \n" +
                    "\n" +
                    "Way 1: Set the corrupted ROM to be created in the Bizhawk folder and have its filename called CorruptedROM.rom and blast to convert it to a BlastLayer\n" +
                    "\n" +
                    "Way 2: You can chain the ROM corruptor to the Glitch Harvester by putting ExternalCorrupt.exe as the emulator in the ROM corruptor.\n" +
                    "\n" +
                    "You'll have to make sure that the " +
                    "rom that is being corrupted is also running in Bizhawk and that the selected Savestate in " +
                    "the Glitch Harvester corresponds to the game that is being corrupted.\n" +
                    "\n" +
                    "To INSTALL an compatible external corruptor as a plugin, put the folder that contains RTC.dat in the PLUGINS folder."
                    , "Null Plugin info", MessageBoxButtons.OK, MessageBoxIcon.Information);
				RTC_Core.StartSound();
            }
            else
            {
                if (File.Exists(RTC_Core.rtcDir + "\\PLUGINS\\" + SelectedPlugin + "\\RTC.dat"))
                    PluginFilename = File.ReadAllText(RTC_Core.rtcDir + "\\PLUGINS\\" + SelectedPlugin + "\\RTC.dat");

                string PluginExeFilename = RTC_Core.rtcDir + "\\PLUGINS\\" + SelectedPlugin + "\\" + PluginFilename;

                ProcessStartInfo psi = new ProcessStartInfo("taskkill", "/F /IM \"" + PluginFilename + "\"");
                psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                Process.Start(psi);
                Thread.Sleep(300);

                Process.Start(PluginExeFilename, "-RTC");

                LastOpenedPlugin = PluginFilename;
            }
        }

        public static bool IsPluginRunning()
        {
            if (PluginFilename == null || PluginFilename == "" || PluginFilename == "NULL")
                return true;

            if (Process.GetProcessesByName(PluginFilename).Length > 0)
                return true;
            else
                return false;
        }

    }
}

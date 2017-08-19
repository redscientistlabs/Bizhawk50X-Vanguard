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
        public static string CorruptedRom = null;
        public static string SelectedPlugin = null;
        public static string PluginFilename = null;

        public static BlastUnit GetUnit()
        {
            return null;
        }

        public static BlastLayer GetLayer()
        {
            

            if (!File.Exists("CorruptedROM.rom"))
                {
                    MessageBox.Show("Null Plugin: You must have CorruptedROM.rom in your BizHawk folder");
                    return null;
                }

            CorruptedRom = "CorruptedROM.rom";
            
            BlastLayer bl = new BlastLayer();

            string thisSystem = Global.Game.System;
            string _domain = "";
            string _seconddomain = "";
            int skipbytes = 0;

            switch (thisSystem) 
            { 
                case "NES":
                    _domain = "PRG ROM";
                    _seconddomain = "CHR VROM";
                    skipbytes = 16;
                    break;
                case "SNES":
                    _domain = "CARTROM";
                    break;
                case "N64":
                    _domain = "ROM";
                    break;
                case "GB":
                case "GBC":
                    _domain = "ROM";
                    break;
                case "SMS": // Sega Master System
					_domain = "ROM";
					return null;
                case "GEN": // Sega Genesis
                    _domain = "MD CART";
                    break;
				case "PSX": // PlayStation
					MessageBox.Show("Unfortunately, Bizhawk doesn't support editing the PSX's ISO while it is running. Maybe in a future version...");
					return null;
				case "INTV":
                case "SG":
                case "GG":
                case "PCECD":
                case "PCE":
                case "SGX":
                case "TI83":
                case "A26":
                case "A78":
                case "C64":
                case "Coleco":
                case "GBA":
                case "SAT":
                case "DGB":
                default:
                    MessageBox.Show("The selected system doesn't appear to have bridge configurations yet. This will not work. You could ask the devs to add it though.");
                    break;
            }


            byte[] Original = File.ReadAllBytes(GlobalWin.MainForm.CurrentlyOpenRom);
            byte[] Corrupt = File.ReadAllBytes("CorruptedROM.rom");

            if (Original.Length != Corrupt.Length)
            {
                MessageBox.Show("Error: The corrupted rom isn't the same size as the original one");
                return null;
            }

            long maxaddress = RTC_MemoryDomains.getInterface(_domain).Size;

            for (int i = 0; i < Original.Length; i++)
            {
                if (Original[i] != Corrupt[i] && i >= skipbytes)
                {
                    if (i - skipbytes >= maxaddress)
                        bl.Layer.Add(new BlastByte(_seconddomain, (i - skipbytes) - maxaddress, BlastByteType.SET, Convert.ToInt32(Corrupt[i]), true));
                    else
                        bl.Layer.Add(new BlastByte(_domain, i - skipbytes, BlastByteType.SET, Convert.ToInt32(Corrupt[i]), true));
                }
            }

            if (bl.Layer.Count == 0)
                return null;
            else
                return bl;



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

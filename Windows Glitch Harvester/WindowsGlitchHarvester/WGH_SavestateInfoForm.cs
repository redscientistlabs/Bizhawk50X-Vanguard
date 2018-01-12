using RTCV.NetCore;
using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGlitchHarvester
{
    public partial class WGH_SavestateInfoForm : Form
    {
        WGH_DolphinConnector dolphinConn;

        public static volatile Queue<string> lazyCrossThreadConsoleQueue = new Queue<string>();
<<<<<<< HEAD
        public static volatile Queue<string> lazyCrossThreadStatusQueue = new Queue<string>();
        public static volatile Queue<string> lazyCrossThreadPeekQueue = new Queue<string>();
=======
>>>>>>> syncobject
        System.Windows.Forms.Timer lazyCrossThreadTimer = new System.Windows.Forms.Timer();

        public WGH_SavestateInfoForm()
        {
            InitializeComponent();
            dolphinConn = new WGH_DolphinConnector(this);
            ConsoleEx.singularity.ConsoleWritten += OnConsoleWritten;

            this.Show();
            
            lazyCrossThreadTimer.Interval = 666;
            lazyCrossThreadTimer.Tick += (o, e) => {
                while (lazyCrossThreadConsoleQueue.Count != 0)
                {
                    lbNetCoreConsole.Items.Add(lazyCrossThreadConsoleQueue.Dequeue());
                    lbNetCoreConsole.SelectedIndex = lbNetCoreConsole.Items.Count - 1;
                }

<<<<<<< HEAD
                while (lazyCrossThreadStatusQueue.Count != 0)
                    lbStatus.Text = lazyCrossThreadStatusQueue.Dequeue();
                while (lazyCrossThreadPeekQueue.Count != 0)
                    peekedValue.Text = lazyCrossThreadPeekQueue.Dequeue();
=======
                //while (lazyCrossThreadStatusQueue.Count != 0)
                //    lbStatus.Text = lazyCrossThreadStatusQueue.Dequeue();
>>>>>>> syncobject
            };
            lazyCrossThreadTimer.Start();
            
        }

        private void OnConsoleWritten(object sender, NetCoreEventArgs e)
        {
            lazyCrossThreadConsoleQueue.Enqueue(e.message.Type);
            /*
            try
            {
                lbNetCoreConsole.Items.Add(e.message.Type);
            }
            catch { }
            */

            //lbNetCoreConsole.Items.Add(e.message.Type);
        }

        private void WGH_SavestateInfoForm_Load(object sender, EventArgs e)
        {
            GetSavestateInfo();

        }


        public void GetSavestateInfo()
        {

            MemoryInterface mi = WGH_Core.currentMemoryInterface;

            if (mi == null)
            {
                this.Close();
                return;
            }


            if (mi.isDolphinSavestate())
            {
                byte[] bytes;
                byte[] sram_pattern = { 0x5B, 0x43, 0x6F, 0x72, 0x65, 0x54, 0x69, 0x6D, 0x69, 0x6E, 0x67, 0x5D };
                byte[] aram_pattern = { 0x5B, 0x50, 0x72, 0x6F, 0x63, 0x65, 0x73, 0x73, 0x6F, 0x72, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x66, 0x61, 0x63, 0x65, 0x40, 0x5D };
                byte[] exram_pattern = { 0x5B, 0x4D, 0x65, 0x6D, 0x6F, 0x72, 0x79, 0x20, 0x46, 0x61, 0x6B, 0x65, 0x56, 0x4D, 0x45, 0x4D, 0x40, 0x40, 0x40, 0x5D };
                int sram_offset = 0;
                int aram_offset = 0;
                int exram_offset = 0;
                int sram_size = 0;
                int aram_size = 0;
                int exram_size = 0;

                //getMemorySize() returns a long whereas peekbytes uses an int. Dolphin savestates should never be 2GB large so this shouldn't be a problem
                bytes = mi.PeekBytes(0, Convert.ToInt32(mi.getMemorySize()));

                int offset = 0;

                //Search for the sram
                for (int i = 0; i < bytes.Length - sram_pattern.Length; i++)
                {
                    bool found = true;
                    for (offset = 0; offset < sram_pattern.Length; offset++)
                    {
                        if (bytes[i + offset] != sram_pattern[offset])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        sram_offset = i + offset;
                        break;
                    }
                }


                //Search for the aram
                for (int i = 0; i < bytes.Length - aram_pattern.Length; i++)
                {
                    bool found = true;
                    for (offset = 0; offset < aram_pattern.Length; offset++)
                    {
                        if (bytes[i + offset] != aram_pattern[offset])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        aram_offset = i + offset;
                        break;
                    }
                }


                //Search for the sram
                for (int i = 0; i < bytes.Length - exram_pattern.Length; i++)
                {
                    bool found = true;
                    for (offset = 0; offset < exram_pattern.Length; offset++)
                    {
                        if (bytes[i + offset] != exram_pattern[offset])
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        exram_offset = i + offset;
                        break;
                    }
                }


                sramOffset.Text = sram_offset.ToString();
                sramAlignment.Text = (sram_offset % 4).ToString();

                //Sram is always 24MB so we can make this assumption
                sram_size = 25165824;

                //Check for the exram. If it doesn't exist, we have gamecube so it's aram
                if ((Encoding.Default.GetString(mi.PeekBytes(exram_offset + 4, 14)) == "[Memory EXRAM]"))
                {
                    aram_size = 16777216;
                    aramexramLabel.Text = "ARAM";
                    aramexramOffset.Text = aram_offset.ToString();
                    aramexramAlignment.Text = (aram_offset % 4).ToString();
                }
                else
                {
                    exram_size = 67108864;
                    aramexramLabel.Text = "EXRAM";
                    aramexramOffset.Text = exram_offset.ToString();
                    aramexramAlignment.Text = (exram_offset % 4).ToString();
                }                
            }
            else
            {
                MessageBox.Show("The currently loaded file is not a Dolphin Narry's Mod v0.1.3 savestate.");
                this.Close();
            }
        }

        private void btnStartNetCore_Click(object sender, EventArgs e)
        {
            if(btnStartNetCore.Text.Contains("Restart"))
            {
                dolphinConn.RestartServer();
            }
            else
            {
                dolphinConn.StartServer();
                btnStartNetCore.Text = "Restart NetCore Server";
            }
            
            
        }

        private void btnLoadState_Click(object sender, EventArgs e)
        {
            dolphinConn.connector.SendMessage("LOADSTATE", WGH_Core.currentTargetFullName);
            Console.WriteLine(WGH_Core.currentTargetFullName);
            //This will send a NetCoreAdvancedMessage
        }

        private void btnSaveState_Click(object sender, EventArgs e)
        {
            //dolphinConn.connector.SendMessage("SAVESTATE");
            //This will send a NetCoreSimpleMessage

            dolphinConn.connector.SendMessage("SAVESTATE", WGH_Core.currentTargetFullName);
            //If you want to send an advanced message or if you want to specify a filename for example
        }

        private void btnPokeByte_Click(object sender, EventArgs e)
        {
            Object[] message = new Object[2];
            message[0] = addressNum.Value;
            message[1] = valueNum.Value;

            dolphinConn.connector.SendMessage("POKEBYTE", message);
        }
        

        private void btnPeekByte_Click(object sender, EventArgs e)
        {

            peekedValue.Text = (dolphinConn.connector.SendSyncedMessage("PEEKBYTES", (Object)addressNum.Value)).ToString();


        }

        private void btnPokeBytes_Click(object sender, EventArgs e)
        {
            Object[] message = new Object[3];
            message[0] = addressNum.Value;
            message[1] = 4;
            message[2] = BitConverter.GetBytes((Int64)valueNum.Value);

            dolphinConn.connector.SendMessage("POKEBYTES", message);

        }

        private void btnPeekBytes_Click(object sender, EventArgs e)
        {
            Object[] message = new Object[2];
            message[0] = addressNum.Value;
            message[1] = 4;

            Byte[] returned = (Byte[])(dolphinConn.connector.SendSyncedMessage("PEEKBYTES", message));

            peekedValue.Text = "";
            foreach (Byte _byte in returned)
                peekedValue.Text += (_byte).ToString();
        }


    }
}

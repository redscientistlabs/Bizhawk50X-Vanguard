using System;
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
        public WGH_SavestateInfoForm()
        {
            InitializeComponent();
            this.Show();
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
                byte[] aram_pattern = { 0x5B, 0x50, 0x72, 0x6F, 0x63, 0x65, 0x73, 0x73, 0x6F, 0x72, 0x49, 0x6E, 0x74, 0x65, 0x72, 0x66, 0x61, 0x63, 0x65, 0x5D };
                byte[] exram_pattern = { 0x5B, 0x4D, 0x65, 0x6D, 0x6F, 0x72, 0x79, 0x20, 0x46, 0x61, 0x6B, 0x65, 0x56, 0x4D, 0x45, 0x4D, 0x5D };
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
                MessageBox.Show("The currently loaded file is not a Dolphin Narry's Mod v0.1 savestate.");
                this.Close();
            }
        }
    }
}

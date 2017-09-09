using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChompAndFriends
{
    public partial class BlooperUI : UserControl
    {
        public BlooperUI()
        {
            InitializeComponent();
            logicComboBox.SelectedIndex = 0;
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                blooperToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                blooperToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }

        private void logicComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (logicComboBox.SelectedIndex)
            {
                case 0:
                    blooperToolTip.SetToolTip(logicComboBox, "Bitwise AND with the next byte to be corrupted");
                    break;
                case 1:
                    blooperToolTip.SetToolTip(logicComboBox, "Bitwise OR with the next byte to be corrupted");
                    break;
                case 2:
                    blooperToolTip.SetToolTip(logicComboBox, "Bitwise XOR with the next byte to be corrupted");
                    break;
                case 3:
                    blooperToolTip.SetToolTip(logicComboBox, "Bitshift left");
                    break;
                case 4:
                    blooperToolTip.SetToolTip(logicComboBox, "Bitshift right");
                    break;
                default:
                    break;
            }
        }
    }
}

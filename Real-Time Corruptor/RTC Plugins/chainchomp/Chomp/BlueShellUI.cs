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
    public partial class BlueShellUI : UserControl
    {
        public BlueShellUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                blueShellToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                blueShellToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }

        private void byteFind_ValueChanged(object sender, EventArgs e)
        {
            blueShellToolTip.SetToolTip(byteFind, string.Format("Corrupt if byte equals ${0:X} ({0})", (int)byteFind.Value));
        }

        private void byteReplace_ValueChanged(object sender, EventArgs e)
        {
            blueShellToolTip.SetToolTip(byteReplace, string.Format("Set byte to ${0:X} ({0})", (int)byteReplace.Value));
        }
    }
}

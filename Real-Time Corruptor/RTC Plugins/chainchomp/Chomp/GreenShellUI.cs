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
    public partial class GreenShellUI : UserControl
    {
        public GreenShellUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                greenShellToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                greenShellToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }

        private void shift_ValueChanged(object sender, EventArgs e)
        {
            int i = (int)shift.Value;
            int j = Math.Abs(i);
            if (i < 0)
            {
                greenShellToolTip.SetToolTip(shift, string.Format("Shift bytes {0} place{1} to the left", j, j > 1 ? "s" : ""));
            }
            else if (i > 0)
            {
                greenShellToolTip.SetToolTip(shift, string.Format("Shift bytes {0} place{1} to the right", j, j > 1 ? "s" : ""));
            }
            else
            {
                greenShellToolTip.SetToolTip(shift, "A value of 0 has no effect!");
            }
        }
    }
}

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
    public partial class BobombUI : UserControl
    {
        public BobombUI()
        {
            InitializeComponent();
        }

        private void targetOffset_ValueChanged(object sender, EventArgs e)
        {
            bobombToolTip.SetToolTip(targetOffset, string.Format("Centre blast at 0x{0:X} ({0} bytes from the start)", (int)targetOffset.Value));
        }

        private void blastRadius_ValueChanged(object sender, EventArgs e)
        {
            bobombToolTip.SetToolTip(blastRadius, string.Format("Corrupt ${0:X} ({0}) bytes either side of target", (int)blastRadius.Value));
        }

        private void power_ValueChanged(object sender, EventArgs e)
        {
            bobombToolTip.SetToolTip(power, string.Format("Set corrupted bytes between ${0:X} ({0}) and original values depending on distance from blast centre", (int)power.Value));
        }
    }
}

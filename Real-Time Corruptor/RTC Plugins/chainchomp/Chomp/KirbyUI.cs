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
    public partial class KirbyUI : UserControl
    {
        public KirbyUI()
        {
            InitializeComponent();
        }

        private void skipOver_ValueChanged(object sender, EventArgs e)
        {
            kirbyToolTip.SetToolTip(skipOver, string.Format("Copy offset = 0x{0:X} (ignore the first {0} bytes)", (int)skipOver.Value));
        }

        private void sampleSize_ValueChanged(object sender, EventArgs e)
        {
            kirbyToolTip.SetToolTip(sampleSize, string.Format("Sample length = ${0:X} (copy the next {0} bytes)", (int)sampleSize.Value));
        }

        private void pasteAt_ValueChanged(object sender, EventArgs e)
        {
            kirbyToolTip.SetToolTip(pasteAt, string.Format("Paste offset = 0x{0:X} (paste after the next {0} bytes)", (int)pasteAt.Value));
        }

        private void loop_CheckedChanged(object sender, EventArgs e)
        {
            if (loop.Checked)
            {
                kirbyToolTip.SetToolTip(loop, "Copy multiple times (best for small sizes)");

            }
            else
            {
                kirbyToolTip.SetToolTip(loop, "Copy once (best for large sizes)");
            }
        }
    }
}

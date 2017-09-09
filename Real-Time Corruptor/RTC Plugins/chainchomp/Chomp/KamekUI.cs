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
    public partial class KamekUI : UserControl
    {
        public KamekUI()
        {
            InitializeComponent();
            seed.MaxLength = int.MaxValue; //allow full int range
        }

        private void filterAlpha(KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        
        private void seed_KeyPress(object sender, KeyPressEventArgs e)
        {
            filterAlpha(e);
        }

        private void resetOnRun_CheckedChanged(object sender, EventArgs e)
        {
            if (resetOnRun.Checked)
            {
                kamekToolTip.SetToolTip(resetOnRun, "Reset the generator state each time the chain runs (more predictable)");
            }
            else
            {
                kamekToolTip.SetToolTip(resetOnRun, "Continue from the previous generator state (less predictable)");
            }
        }

    }
}

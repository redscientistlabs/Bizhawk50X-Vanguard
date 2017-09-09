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
    public partial class SignpostUI : UserControl
    {
        public SignpostUI()
        {
            InitializeComponent();
        }

        private void offset_ValueChanged(object sender, EventArgs e)
        {
            signpostToolTip.SetToolTip(offset, string.Format("Modify the value at 0x{0:X}", (int)offset.Value));
        }
    }
}

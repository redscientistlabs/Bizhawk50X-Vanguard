using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace ChompAndFriends
{
    public partial class RedShellUI : UserControl
    {

        public RedShellUI()
        {
            InitializeComponent();
            
        }

        private void offset_ValueChanged(object sender, EventArgs e)
        {
            redShellToolTip.SetToolTip(offset, string.Format("Modify the value at 0x{0:X}", (int)offset.Value));
        }

        private void byteValue_ValueChanged(object sender, EventArgs e)
        {
            redShellToolTip.SetToolTip(byteValue, string.Format("Set byte to ${0:X} ({0})", (int)byteValue.Value));

        }


    }
}

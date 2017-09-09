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
    public partial class PSwitchUI : UserControl
    {
        public PSwitchUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                pSwitchToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                pSwitchToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }
    }
}

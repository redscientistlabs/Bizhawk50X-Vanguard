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
    public partial class ChompUI : UserControl
    {
        public ChompUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                chompToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                chompToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }

        }

        private void byteValue_ValueChanged(object sender, EventArgs e)
        {
            chompToolTip.SetToolTip(byteValue, string.Format("Set byte to ${0:X} ({0})", (int)byteValue.Value));
        }
    }
}

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
    public partial class MushroomUI : UserControl
    {
        public MushroomUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                mushroomToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                mushroomToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }

        private void incAmount_ValueChanged(object sender, EventArgs e)
        {
            mushroomToolTip.SetToolTip(incAmount, string.Format("Add ${0:X} ({0}) to byte", (int)incAmount.Value));
        }

    }
}

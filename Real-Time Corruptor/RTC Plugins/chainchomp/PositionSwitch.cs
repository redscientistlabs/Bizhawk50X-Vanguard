using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChainChomp
{
    public partial class PositionSwitch : UserControl
    {
        public EventHandler UpButtonClick;
        public EventHandler DownButtonClick;

        public PositionSwitch()
        {
            InitializeComponent();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (DownButtonClick != null)
            {
                DownButtonClick(this, e);
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (UpButtonClick != null)
            {
                UpButtonClick(this, e);
            }
        }

        

    }
}

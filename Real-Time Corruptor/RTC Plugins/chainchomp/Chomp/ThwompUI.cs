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
    public partial class ThwompUI : UserControl
    {
        private int _waveform = 0;

        //property that sets BG when changed
        public int waveform
        {
            get
            {
                return _waveform;
            }
            set
            {
                _waveform = value;
                waveformPreview.BackgroundImage = waveformBackgrounds[value];
                thwompToolTip.SetToolTip(waveformPreview, descriptions[value]);
            }
        }

        private static Bitmap[] waveformBackgrounds = new Bitmap[]
        {
            ChompAndFriends.Properties.Resources.sineWave,
            ChompAndFriends.Properties.Resources.triangleWave,
            ChompAndFriends.Properties.Resources.squareWave,
            ChompAndFriends.Properties.Resources.sawtoothWave,
            ChompAndFriends.Properties.Resources.absoluteSineWave
        };

        private static string[] descriptions = new string[]
        {
            "Sine wave",
            "Triangle wave",
            "Square wave",
            "Sawtooh wave",
            "Absolute Sine wave"
        };

        public ThwompUI()
        {
            InitializeComponent();
        }

        private void byteInterval_ValueChanged(object sender, EventArgs e)
        {
            if (byteInterval.Value > 1)
            {
                thwompToolTip.SetToolTip(byteInterval, string.Format("Corrupt every {0} byte", ChainChompCorruptor.PluginUtility.AddOrdinal((int)byteInterval.Value)));
            }
            else
            {
                thwompToolTip.SetToolTip(byteInterval, "Corrupt every byte");
            }
        }

        private void prevWaveButton_Click(object sender, EventArgs e)
        {
            waveform = waveform - 1 > -1 ? waveform - 1 : waveformBackgrounds.Length - 1;
        }

        private void nextWaveButton_Click(object sender, EventArgs e)
        {
            waveform = waveform + 1 < waveformBackgrounds.Length ? waveform + 1 : 0;
        }
    }
}

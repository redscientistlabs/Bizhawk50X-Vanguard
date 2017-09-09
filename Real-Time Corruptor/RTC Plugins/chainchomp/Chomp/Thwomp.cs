using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Thwomp : IChainChompCorruptor
    {
        private ThwompUI rackControl;

        public Thwomp()
        {
            rackControl = new ThwompUI();

        }

        public string Name
        {
            get
            {
                return "Thwomp";
            }
        }

        public string Author
        {
            get
            {
                return "DaleJ";
            }
        }

        public string Version
        {
            get
            {
                return "1.0";
            }
        }

        public string PresetExt
        {
            get
            {
                return "thwomp";
            }
        }

        [SaveableSetting]
        public int interval
        {
            get
            {
                return (int)rackControl.byteInterval.Value;
            }
            set
            {
                rackControl.byteInterval.Value = value;
            }
        }

        [SaveableSetting]
        public bool useOffset //bool is enough information for binary radio list
        {
            get
            {
                return rackControl.offsetRadioButton.Checked;
            }

            set
            {
                if (value)
                {
                    rackControl.offsetRadioButton.Checked = true;
                }
                else
                {
                    rackControl.byteValueRadioButton.Checked = true;
                }
            }
        }

        [SaveableSetting]
        public int waveform
        {
            get
            {
                return rackControl.waveform;
            }
            set
            {
                rackControl.waveform = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            switch (waveform)
            {
                case 0:
                    //SINE WAVE
                    input = input.Select((n, i) => i % interval == 0 ? (byte)(255 * 0.5 * (Math.Sin((useOffset ? i : n) * (2 * Math.PI / 256)) + 1)) : n);
                    return input;
                case 1:
                    //TRIANGLE WAVE
                    input = input.Select((n, i) => i % interval == 0 ? (byte)(255 * (255 - Math.Abs((useOffset ? i : n) % 510) / 255)) : n);
                    return input;
                case 2:
                    //SQUARE WAVE
                    input = input.Select((n, i) => i % interval == 0 ? (byte)(255 * Math.Round(0.5 * (Math.Sin((useOffset ? i : n) * (2 * Math.PI / 256)) + 1), 0, MidpointRounding.AwayFromZero)) : n);
                    return input;
                case 3:
                    //SAWTOOTH WAVE
                    input = input.Select((n, i) => i % interval == 0 ? (byte)((useOffset ? i : n) % 255) : n);
                    return input;
                case 4:
                    //ABSOLUTE SINE WAVE
                    input = input.Select((n, i) => i % interval == 0 ? (byte)Math.Abs(255 * Math.Sin((useOffset ? i : n) * (2 * Math.PI / 256))) : n);
                    return input;
                default:
                    break;
            }
            return input;
        }

        public IComponent GetUserControl()
        {
            return rackControl;
        }

        public IComponent GetBackgroundContainer()
        {
            return rackControl.GetNextControl(rackControl, true);
        }

        public IEnumerable<byte> GetFactoryPresets()
        {
            return ChompAndFriends.Properties.Resources.thwompPresets;
        }
    }
}

using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Signpost : IChainChompCorruptor
    {
        private SignpostUI rackControl;

        public Signpost()
        {
            rackControl = new SignpostUI();

        }

        public string Name
        {
            get
            {
                return "Signpost";
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
                return "post";
            }
        }

        [SaveableSetting]
        public int offset
        {
            get
            {
                return (int)rackControl.offset.Value;
            }
            set
            {
                rackControl.offset.Value = value;
            }
        }

        [SaveableSetting]
        public string text
        {
            get
            {
                return rackControl.textBox.Text;
            }
            set
            {
                rackControl.textBox.Text = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            byte[] ascii = System.Text.Encoding.ASCII.GetBytes(text);
            List<byte> sample = input.ToList();
            if (offset < sample.Count)
            {
                for (int i = offset; i < ascii.Count(); i++)
                {
                    if (i < sample.Count)
                    {
                        sample[i] = ascii[i - offset];
                    }
                }
            }

            return sample;
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
            return ChompAndFriends.Properties.Resources.signpostPresets;
        }
    }
}

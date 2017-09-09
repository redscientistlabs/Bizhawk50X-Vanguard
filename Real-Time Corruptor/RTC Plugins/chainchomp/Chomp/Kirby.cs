using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Kirby : IChainChompCorruptor
    {
        private KirbyUI rackControl;

        public Kirby()
        {
            rackControl = new KirbyUI();

        }

        public string Name
        {
            get
            {
                return "Kirby";
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
                return "kirby";
            }
        }

        [SaveableSetting]
        public int skipOver
        {
            get
            {
                return (int)rackControl.skipOver.Value;
            }
            set
            {
                rackControl.skipOver.Value = value;
            }
        }

        [SaveableSetting]
        public int sampleSize
        {
            get
            {
                return (int)rackControl.sampleSize.Value;
            }
            set
            {
                rackControl.sampleSize.Value = value;
            }
        }

        [SaveableSetting]
        public int pasteAt
        {
            get
            {
                return (int)rackControl.pasteAt.Value;
            }
            set
            {
                rackControl.pasteAt.Value = value;
            }
        }

        [SaveableSetting]
        public bool loop
        {
            get
            {
                return rackControl.loop.Checked;
            }
            set
            {
                rackControl.loop.Checked = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            int total = skipOver + sampleSize * 2 + pasteAt;
            byte[] source = input.ToArray();

            if (loop)
            {
                input = input.Select((n, i) => (i / total) * total + skipOver - 1 < i - pasteAt - sampleSize ? source[i - pasteAt - sampleSize] : n);
            }
            else
            {
                input = input.Select((n, i) => i - pasteAt - sampleSize > skipOver -1 && i < total ? source[i - pasteAt - sampleSize] : n);
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
            return ChompAndFriends.Properties.Resources.kirbyPresets;
        }
    }
}

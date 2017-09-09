using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class PSwitch : IChainChompCorruptor
    {
        private PSwitchUI rackControl;

        public PSwitch()
        {
            rackControl = new PSwitchUI();

        }

        public string Name
        {
            get
            {
                return "P Switch";
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
                return "p";
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

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            input = input.Select((n, i) => i % interval == 0 ? (byte)~n : n);
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
            return ChompAndFriends.Properties.Resources.pSwitchPresets;
        }
    }
}

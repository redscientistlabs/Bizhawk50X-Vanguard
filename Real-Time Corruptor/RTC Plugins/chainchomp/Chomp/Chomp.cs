using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChainChompCorruptor;
using System.ComponentModel;

namespace ChompAndFriends
{
    public class Chomp : IChainChompCorruptor
    {
        private ChompUI rackControl;

        public Chomp()
        {
            rackControl = new ChompUI();

        }

        public string Name
        {
            get
            {
                return "Chomp";
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
                return "chomp";
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
        public byte setTo
        {
            get
            {
                return (byte)rackControl.byteValue.Value;
            }

            set
            {
                rackControl.byteValue.Value = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            input = input.Select((x, i) => i % interval == 0 ? setTo : x);
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
            return ChompAndFriends.Properties.Resources.chompPresets;
        }

    }
}

using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Bobomb : IChainChompCorruptor
    {
        private BobombUI rackControl;

        public Bobomb()
        {
            rackControl = new BobombUI();

        }

        public string Name
        {
            get
            {
                return "Bob-omb";
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
                return "bob";
            }
        }

        [SaveableSetting]
        public int target
        {
            get
            {
                return (int)rackControl.targetOffset.Value;
            }
            set
            {
                rackControl.targetOffset.Value = value;
            }
        }

        [SaveableSetting]
        public int radius
        {
            get
            {
                return (int)rackControl.blastRadius.Value;
            }
            set
            {
                rackControl.blastRadius.Value = value;
            }
        }

        [SaveableSetting]
        public byte power
        {
            get
            {
                return (byte)rackControl.power.Value;
            }
            set
            {
                rackControl.power.Value = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
           input = input.Select((n, i) => Math.Abs(target - i) <= radius ? (byte)(n + power * (byte)Math.Abs(target - i)/radius) : n);
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
            return ChompAndFriends.Properties.Resources.bobombPresets;
        }
    }
}

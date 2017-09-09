using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Mushroom : IChainChompCorruptor
    {
        private MushroomUI rackControl;

        public Mushroom()
        {
            rackControl = new MushroomUI();

        }

        public string Name
        {
            get
            {
                return "Mushroom";
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
                return "shroom";
            }
        }

        [SaveableSetting]
        public int byteInterval
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
        public byte incAmount
        {
            get
            {
                return (byte)rackControl.incAmount.Value;
            }

            set
            {
                rackControl.incAmount.Value = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            input = input.Select((n, i) => i % byteInterval == 0 ? (byte)(incAmount + n) : n);
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
            return ChompAndFriends.Properties.Resources.mushroomPresets;
        }
    }
}

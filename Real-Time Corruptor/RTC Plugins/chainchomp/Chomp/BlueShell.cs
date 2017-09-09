using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class BlueShell : IChainChompCorruptor
    {
        private BlueShellUI rackControl;

        public BlueShell()
        {
            rackControl = new BlueShellUI();

        }

        public string Name
        {
            get
            {
                return "Blue Shell";
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
                return "bsh";
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
        public byte find
        {
            get
            {
                return (byte)rackControl.byteFind.Value;
            }
            set
            {
                rackControl.byteFind.Value = value;
            }
        }

        [SaveableSetting]
        public byte replace
        {
            get
            {
                return (byte)rackControl.byteReplace.Value;
            }
            set
            {
                rackControl.byteReplace.Value = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            input = input.Select((n, i) => i % interval == 0 && n == find ? replace : n);
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
            return ChompAndFriends.Properties.Resources.blueShellPresets;
        }
    }
}

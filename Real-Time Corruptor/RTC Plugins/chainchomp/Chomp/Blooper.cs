using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Blooper : IChainChompCorruptor
    {
        private BlooperUI rackControl;

        public Blooper()
        {
            rackControl = new BlooperUI();

        }

        public string Name
        {
            get
            {
                return "Blooper";
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
                return "bloop";
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
        public int logic
        {
            get
            {
                return rackControl.logicComboBox.SelectedIndex;
            }
            set
            {
                rackControl.logicComboBox.SelectedIndex = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            byte[] source = input.ToArray();

            input = input.Select((n, i) =>
            {
                if (i % interval == 0)
                {
                    byte x = i + interval < source.Count() ? source[0] : (byte)0xAA;

                    switch (logic)
                    {
                        case 0:
                            //AND
                            return (byte)(n & x);
                        case 1:
                            //OR
                            return (byte)(n | x);
                        case 2:
                            //XOR
                            return (byte)(n ^ x);
                        case 3:
                            //<<
                            return (byte)(n <<= n);
                        case 4:
                            //>>
                            return (byte)(n >>= n);
                        default:
                            break;
                    }
                }
                return n;
            });
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
            return ChompAndFriends.Properties.Resources.blooperPresets;
        }
    }
}

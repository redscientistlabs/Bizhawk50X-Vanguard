using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class GreenShell : IChainChompCorruptor
    {
        private GreenShellUI rackControl;

        public GreenShell()
        {
            rackControl = new GreenShellUI();

        }

        public string Name
        {
            get
            {
                return "Green Shell";
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
                return "gsh";
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
        public int shift
        {
            get
            {
                return (int)rackControl.shift.Value;
            }
            set
            {
                rackControl.shift.Value = value;
            }
        }

        private List<byte> source;

        private byte CopyByte(int index)
        {
            index = Math.Max(0, Math.Min(source.Count - 1, index));
            return source[index];
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            source = input.ToList();

            input = input.Select((n, i) => (i - shift) % interval == 0 ? CopyByte(i - shift) : n);
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
            return ChompAndFriends.Properties.Resources.greenShellPresets;
        }
    }
}

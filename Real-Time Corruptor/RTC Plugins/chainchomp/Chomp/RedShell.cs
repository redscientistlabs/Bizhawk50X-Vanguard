using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChompAndFriends
{
    [KnownType(typeof(List<string[]>))]
    public class RedShell : IChainChompCorruptor
    {
        private RedShellUI rackControl;

        public RedShell()
        {
            rackControl = new RedShellUI();

        }

        public string Name
        {
            get
            {
                return "Red Shell";
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
                return "rsh";
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
        public byte byteValue
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

        [SaveableSetting]
        public string note
        {
            get
            {
                return rackControl.note.Text;
            }
            set
            {
                rackControl.note.Text = value;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            if (offset < input.Count() && input.Count() != 0)
            {
                List<byte> sample = input.ToList();
                sample[offset] = byteValue;
                return sample;
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
            return ChompAndFriends.Properties.Resources.redShellPresets;
        }
    }
}

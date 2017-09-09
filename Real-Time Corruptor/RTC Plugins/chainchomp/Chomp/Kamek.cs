using ChainChompCorruptor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChompAndFriends
{
    public class Kamek : IChainChompCorruptor
    {
        private KamekUI rackControl;
        private Random rnd;

        public Kamek()
        {
            rackControl = new KamekUI();

        }

        public string Name
        {
            get
            {
                return "Kamek";
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
                return "kamek";
            }
        }

        [SaveableSetting]
        public string seed
        {
            get
            {
                return rackControl.seed.Text;
            }
            set
            {
                rackControl.seed.Text = value;
            }
        }

        [SaveableSetting]
        public string maxOffset
        {
            get
            {
                return rackControl.maxOffset.Text;
            }
            set
            {
                
                rackControl.maxOffset.Text = value;
            }
        }

        [SaveableSetting]
        public bool resetOnRun
        {
            get
            {
                return rackControl.resetOnRun.Checked;
            }
            set
            {
                rackControl.resetOnRun.Checked = value;
            }
        }

        [SaveableSetting]
        public bool jumpIsVariable
        {
            get
            {
                return rackControl.jumpVariableRadioButton.Checked;
            }
            set
            {
                if (value)
                {
                    rackControl.jumpVariableRadioButton.Checked = true;
                }
                else
                {
                    rackControl.jumpFixedRadioButton.Checked = true;
                }
            }
        }

        [SaveableSetting]
        public bool valueIsVariable
        {
            get
            {
                return rackControl.valueVariableRadioButton.Checked;
            }
            set
            {
                if (value)
                {
                    rackControl.valueVariableRadioButton.Checked = true;
                }
                else
                {
                    rackControl.valueFixedRadioButton.Checked = true;
                }
            }
        }

        private int fixedJump;
        private byte fixedValue;
        private int nextJump;
        private byte nextValue
        {
            get
            {
                if (jumpIsVariable)
                {
                    nextJump = rnd.Next(1, maxJump + 1);
                }
                else
                {
                    nextJump = fixedJump;
                }
                return valueIsVariable ? (byte)rnd.Next(0, 256) : fixedValue;
            }
        }

        private int maxJump
        {
            get
            {
                int i;
                bool valid = int.TryParse(maxOffset, out i); //make sure the text entered was valid
                return valid ? i + 1 : 257;
            }
        }

        public IEnumerable<byte> Corrupt(IEnumerable<byte> input)
        {
            if (rackControl.resetOnRun.Checked || rnd == null)
            {
                int newSeed;
                bool valid = int.TryParse(seed, out newSeed); //make sure the text entered was valid
                newSeed = valid ? newSeed : 1337;
                rnd = new Random(newSeed);
            }

            //set fixed & initial values
            fixedJump = rnd.Next(1, maxJump);
            fixedValue = (byte)rnd.Next(0, 256);
            nextJump = fixedJump;

            input = input.Select((n, i) => i % nextJump == 0 ? nextValue : n);

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
            return ChompAndFriends.Properties.Resources.kamekPresets;
        }
    }
}

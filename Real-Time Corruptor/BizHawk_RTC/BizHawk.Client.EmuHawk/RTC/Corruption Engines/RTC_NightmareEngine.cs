using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{

    public static class RTC_NightmareEngine
    {
        public static BlastByteAlgo Algo = BlastByteAlgo.RANDOM;

        public static BlastUnit GenerateUnit(string _domain, long _address)
        {

            // Randomly selects a memory operation according to the selected algorithm

            try
            {
                BlastByteType Type = BlastByteType.NONE;

                switch (Algo)
                {
                    case BlastByteAlgo.RANDOM: //RANDOM always sets a random value
                        Type = BlastByteType.SET;
                        break;

                    case BlastByteAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
                        int result = RTC_Core.RND.Next(1, 4);
                        switch (result)
                        {
                            case 1:
                                Type = BlastByteType.ADD;
                                break;
                            case 2:
                                Type = BlastByteType.SUBSTRACT;
                                break;
                            case 3:
                                Type = BlastByteType.SET;
                                break;
                            default:
                                MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) RANDOMTILT)");
                                return null;
                        }

                        break;

                    case BlastByteAlgo.TILT: //TILT can either add 1 or substract 1
                        result = RTC_Core.RND.Next(1, 3);
                        switch (result)
                        {
                            case 1:
                                Type = BlastByteType.ADD;
                                break;

                            case 2:
                                Type = BlastByteType.SUBSTRACT;
                                break;

                            default:
                                MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) TILT)");
                                return null;
                        }
                        break;

                }


                byte[] Value = new byte[] { 1 }; //1 by default because Add(1) or Substract(1) but more is still possible
                if (Type == BlastByteType.SET)
                {
                    if (RTC_Core.CustomPrecision == -1)
                    {
                        var settings = new RamSearchEngine.Settings(RTC_MemoryDomains.MDRI.MemoryDomains);
                        Value = new byte[(int)settings.Size];
                    }
                    else
                        Value = new byte[RTC_Core.CustomPrecision];

                    for(int i=0; i<Value.Length; i++)
                        Value[i] = (byte)RTC_Core.RND.Next(0, 255);
                }

                return new BlastByte(_domain, _address, Type, Value, true);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Nightmare Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{

    public static class RTC_DistortionEngine
    {
        public static int MaxAge = 50;
        public static int CurrentAge = 0;
        public static Queue<BlastUnit> AllDistortionBytes = new Queue<BlastUnit>();

        public static BlastUnit GetUnit()
        {

            if (CurrentAge >= MaxAge)
                return AllDistortionBytes.Dequeue();
            else
                return null;
        }

        public static void AddUnit(BlastUnit bu)
        {
            AllDistortionBytes.Enqueue(bu);
        }


        public static BlastUnit GenerateUnit(string _domain, long _address)
        {

            // Randomly selects a memory operation according to the selected algorithm

            try
            {
                BlastByteType Type = BlastByteType.SET;
                int Value = 1;
                BlastByte bb = new BlastByte(_domain, _address, Type, Value, true);
                return bb.GetBackup();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Distortion Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

		public static void Resync()
		{
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_DISTORTION_RESYNC));

		}
	}
}

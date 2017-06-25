using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;

namespace RTC
{

    public static class RTC_PipeEngine
    {
        public static int MaxPipes = 20;
        public static Queue<BlastUnit> AllBlastPipes = new Queue<BlastUnit>();

		public static string lastDomain = null;
		public static long lastAddress = 0;

		public static bool LockPipes = false;
		public static bool ProcessOnStep = true;

        public static void ExecutePipes()
        {
			foreach (BlastPipe pipe in AllBlastPipes)
				pipe.Execute();

		}

		public static void ClearPipes(bool sync = false)
		{
			if (!LockPipes)
			{
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_PIPE_CLEARPIPES), sync);
			}
		}

		public static void AddUnit(BlastUnit bu)
        {
			if (!LockPipes)
			{

				if (bu != null)
					AllBlastPipes.Enqueue(bu);

				RemoveExcessPipes();
			}

		}

		public static void RemoveExcessPipes()
		{
				while (AllBlastPipes.Count > MaxPipes)
				AllBlastPipes.Dequeue();
		}

        public static BlastUnit GenerateUnit(string _domain, long _address)
        {


            // Randomly selects a memory operation according to the selected algorithm
			
            try
            {
				if(lastDomain == null) // The first unit will always be null
				{
					lastDomain = _domain;
					lastAddress = _address;
					return null;
				}
				else
				{
					BlastPipe bp = new BlastPipe(_domain, _address, lastDomain, lastAddress, true);
					lastDomain = _domain;
					lastAddress = _address;
					return bp;

				}

			}
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Datapipe Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

    }
}

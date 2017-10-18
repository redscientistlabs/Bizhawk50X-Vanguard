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
        public static int tiltValue = 0;
        public static Queue<BlastUnit> AllBlastPipes = new Queue<BlastUnit>();

        public static bool ChainedPipes = true;

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
                
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
                int pipeSize;

                if (RTC_Core.CustomPrecision == -1)
                    pipeSize = mdp.WordSize;
                else
                    pipeSize = RTC_Core.CustomPrecision;

                long safeAddress = _address - (_address % pipeSize);

                if (ChainedPipes)
                {
                    if (lastDomain == null) // The first unit will always be null
                    {
                        lastDomain = _domain;
                        lastAddress = safeAddress;
                        return null;
                    }
                    else
                    {
                        BlastPipe bp = new BlastPipe(_domain, safeAddress, lastDomain, lastAddress, tiltValue, pipeSize, true);
                        lastDomain = _domain;
                        lastAddress = safeAddress;
                        return bp;

                    }
                }
                else
                {
                    var pipeEnd = RTC_Core.GetBlastTarget();
                    long safepipeEndAddress = pipeEnd.address - (pipeEnd.address % pipeSize);

                    BlastPipe bp = new BlastPipe(_domain, safeAddress, pipeEnd.domain, safepipeEndAddress, tiltValue, pipeSize, true);
                    lastDomain = _domain;
                    lastAddress = safeAddress;
                    return bp;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Pipe Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }

    }
}

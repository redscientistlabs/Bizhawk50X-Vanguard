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

		public static BlastPipeAlgo Algo = BlastPipeAlgo.VALUE;


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

			try
            {
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);

				BlastPipeType Type = BlastPipeType.SET;

				switch (Algo)
				{
					case BlastPipeAlgo.VALUE: //RANDOM always sets a random value
						Type = BlastPipeType.SET;
						break;

					case BlastPipeAlgo.TILTVALUE: //RANDOMTILT may add 1,substract 1 or set a random value
						int result = RTC_Core.RND.Next(1, 4);
						switch (result)
						{
							case 1:
								Type = BlastPipeType.ADD;
								break;
							case 2:
								Type = BlastPipeType.SUBSTRACT;
								break;
							case 3:
								Type = BlastPipeType.SET;
								break;
							default:
								MessageBox.Show("Random returned an unexpected value (RTC_PipeEngine switch(Algo) RANDOMTILT)");
								return null;
						}

						break;

					case BlastPipeAlgo.TILT: //TILT can either add 1 or substract 1
						result = RTC_Core.RND.Next(1, 3);
						switch (result)
						{
							case 1:
								Type = BlastPipeType.ADD;
								break;

							case 2:
								Type = BlastPipeType.SUBSTRACT;
								break;

							default:
								MessageBox.Show("Random returned an unexpected value (RTC_PipeEngine switch(Algo) TILT)");
								return null;
						}
						break;
				}


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
                        BlastPipe bp = new BlastPipe(_domain, safeAddress, lastDomain, lastAddress, tiltValue, pipeSize, Type, mdp.BigEndian, true);
                        lastDomain = _domain;
                        lastAddress = safeAddress;
                        return bp;

                    }
                }
                else
                {
                    var pipeEnd = RTC_Core.GetBlastTarget();
                    long safepipeEndAddress = pipeEnd.address - (pipeEnd.address % pipeSize);

                    BlastPipe bp = new BlastPipe(_domain, safeAddress, pipeEnd.domain, safepipeEndAddress, tiltValue, pipeSize, Type, mdp.BigEndian, true);
                    lastDomain = _domain;
                    lastAddress = safeAddress;
                    return bp;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Pipe Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
                return null;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_PipeEngine
	{
		public static int MaxPipes = 20;
		public static int TiltValue = 0;
		public static Queue<BlastUnit> AllBlastPipes = new Queue<BlastUnit>();

		public static bool ChainedPipes = true;

		public static string LastDomain = null;
		public static long LastAddress = 0;

		public static bool LockPipes = false;

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
				if (_domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(_domain, _address);

				int pipeSize = RTC_Core.CustomPrecision == -1 ? mdp.WordSize : RTC_Core.CustomPrecision;

				long safeAddress = _address - (_address % pipeSize);

				if (ChainedPipes)
				{
					if (LastDomain == null) // The first unit will always be null
					{
						LastDomain = _domain;
						LastAddress = safeAddress;
						return null;
					}
					else
					{
						BlastPipe bp = new BlastPipe(_domain, safeAddress, LastDomain, LastAddress, TiltValue, pipeSize, mdp.BigEndian, true);
						LastDomain = _domain;
						LastAddress = safeAddress;
						return bp;
					}
				}
				else
				{
					BlastTarget pipeEnd = RTC_Core.GetBlastTarget();
					long safepipeEndAddress = pipeEnd.address - (pipeEnd.address % pipeSize);

					BlastPipe bp = new BlastPipe(_domain, safeAddress, pipeEnd.domain, safepipeEndAddress, TiltValue, pipeSize, mdp.BigEndian, true);
					LastDomain = _domain;
					LastAddress = safeAddress;
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

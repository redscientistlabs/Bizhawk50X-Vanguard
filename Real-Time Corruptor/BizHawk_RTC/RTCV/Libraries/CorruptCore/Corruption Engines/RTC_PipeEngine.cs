using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_PipeEngine
	{
		public static PartialSpec getDefaultPartial()
		{
			return null;
		}

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				if (domain == null)
					return null;
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);

				long safeAddress = address - (address % precision);

				BlastTarget pipeStart = RTC_CorruptCore.GetBlastTarget();
				long safePipeStartAddress = pipeStart.Address - (pipeStart.Address % precision);

				return new BlastUnit(StoreType.CONTINUOUS, ActionTime.PREEXECUTE, domain, safeAddress, pipeStart.Domain, safePipeStartAddress, precision, mi.BigEndian, 0, 0);
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

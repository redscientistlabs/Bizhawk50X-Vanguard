using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CorruptCore
{
	public static class RTC_PipeEngine
	{

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				
				long safeAddress = address - (address % precision);

				BlastTarget pipeStart = RTC_Corruptcore.GetBlastTarget();
				long safePipeStartAddress = pipeStart.Address - (pipeStart.Address % precision);

				return new BlastUnit(StoreType.CONTINUOUS, StoreTime.PREEXECUTE, domain, safeAddress, pipeStart.Domain, safePipeStartAddress, precision, mdp.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("Pipe Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}

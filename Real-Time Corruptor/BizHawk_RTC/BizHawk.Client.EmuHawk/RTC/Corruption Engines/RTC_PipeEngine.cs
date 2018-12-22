using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
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

				BlastTarget pipeStart = RTC_Core.GetBlastTarget();
				long safePipeStartAddress = pipeStart.Address - (pipeStart.Address % precision);

				return new BlastUnit(StoreType.CONTINUOUS, ActionTime.PREEXECUTE, domain, safeAddress, pipeStart.Domain, safePipeStartAddress, precision, mdp.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("Pipe Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}

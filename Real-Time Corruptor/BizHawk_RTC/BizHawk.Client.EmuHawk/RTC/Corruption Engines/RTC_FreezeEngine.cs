using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_FreezeEngine
	{
		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, StoreTime.PREEXECUTE, domain, safeAddress, domain, safeAddress, precision, mdp.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("Freeze Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}

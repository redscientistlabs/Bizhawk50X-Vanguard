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
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);
				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, ActionTime.PREEXECUTE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Freeze Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}
	}
}

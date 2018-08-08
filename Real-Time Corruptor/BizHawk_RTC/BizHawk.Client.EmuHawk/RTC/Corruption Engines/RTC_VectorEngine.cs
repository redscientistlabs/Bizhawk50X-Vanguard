using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_VectorEngine
	{
		public static MD5 currentLimiterList = null;
		public static MD5 currentValueList = null;

		public static BlastUnit GenerateUnit(string domain, long address)
		{
			if (domain == null)
				return null;

			long safeAddress = address - (address % 4); //32-bit trunk

			MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, safeAddress);
			if (mdp == null)
				return null;

			try
			{
				//Enforce the safeaddress at generation
				if (RTC_Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, currentLimiterList, mdp))
					return new BlastUnit(RTC_Filtering.GetRandomConstant(currentValueList), domain, safeAddress, 4, mdp.BigEndian);
				return null;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Vector Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}


	}
}

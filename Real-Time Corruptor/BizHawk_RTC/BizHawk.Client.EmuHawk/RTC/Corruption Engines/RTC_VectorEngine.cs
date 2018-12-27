using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_VectorEngine
	{

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
				if (RTC_Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, domain, (string)RTC_Unispec.RTCSpec[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()], mdp))
					return new BlastUnit(RTC_Filtering.GetRandomConstant((string)RTC_Unispec.RTCSpec[RTCSPEC.VECTOR_VALUELISTHASH.ToString()], 4), domain, safeAddress, 4, mdp.BigEndian);
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Vector Engine GenerateUnit Threw Up" + ex);
			}
		}


	}
}

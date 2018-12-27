using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_DistortionEngine
	{
		public static int Delay = (int)RTC_Unispec.RTCSpec[Spec.DISTORTION_DELAY.ToString()];
		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm
			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, ActionTime.IMMEDIATE, domain, safeAddress, domain, safeAddress, precision, mdp.BigEndian, Delay, 1);
			}
			catch (Exception ex)
			{
				throw new Exception("Distortion Engine GenerateUnit Threw Up" + ex);
			}
		}

	}
}

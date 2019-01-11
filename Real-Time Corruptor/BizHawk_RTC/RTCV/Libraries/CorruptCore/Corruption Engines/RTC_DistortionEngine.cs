using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTC
{
	public static class RTC_DistortionEngine
	{
		public static int Delay
		{
			get => (int)RTC_Corruptcore.CorruptCoreSpec[RTCSPEC.DISTORTION_DELAY.ToString()];
			set => RTC_Corruptcore.CorruptCoreSpec.Update(RTCSPEC.DISTORTION_DELAY.ToString(), value);
		}
		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");
			partial[RTCSPEC.DISTORTION_DELAY.ToString()] = 50;

			return partial;
		}

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm
			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, StoreTime.IMMEDIATE, domain, safeAddress, domain, safeAddress, precision, mdp.BigEndian, Delay, 1);
			}
			catch (Exception ex)
			{
				throw new Exception("Distortion Engine GenerateUnit Threw Up" + ex);
			}
		}

	}
}

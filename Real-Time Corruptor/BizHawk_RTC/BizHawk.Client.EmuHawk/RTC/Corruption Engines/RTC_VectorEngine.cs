using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using RTCV.NetCore;

namespace RTC
{
	public static class RTC_VectorEngine
	{

		public static string LimiterListHash
		{
			get => (string)RTC_Corruptcore.RTCSpec[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()];
			set => RTC_Corruptcore.RTCSpec.Update(RTCSPEC.VECTOR_LIMITERLISTHASH.ToString(), value);
		}

		public static string ValueListHash
		{
			get => (string)RTC_Corruptcore.RTCSpec[RTCSPEC.VECTOR_VALUELISTHASH.ToString()];
			set => RTC_Corruptcore.RTCSpec.Update(RTCSPEC.VECTOR_VALUELISTHASH.ToString(), value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial[RTCSPEC.VECTOR_LIMITERLISTHASH.ToString()] = String.Empty;
			partial[RTCSPEC.VECTOR_VALUELISTHASH.ToString()] = String.Empty;

			return partial;
		}


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
				if (RTC_Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, domain, LimiterListHash, mdp))
					return new BlastUnit(RTC_Filtering.GetRandomConstant(ValueListHash, 4), domain, safeAddress, 4, mdp.BigEndian);
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("Vector Engine GenerateUnit Threw Up" + ex);
			}
		}


	}
}

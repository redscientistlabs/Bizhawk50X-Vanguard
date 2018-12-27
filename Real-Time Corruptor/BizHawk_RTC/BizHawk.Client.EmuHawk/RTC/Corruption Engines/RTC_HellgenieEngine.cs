using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);

				Byte[] value = new Byte[precision];

				long safeAddress = address - (address % precision);

				long randomValue = -1;
				switch (precision)
				{
					case (1):
						randomValue = RTC_Core.RND.RandomLong((long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MINVALUE8BIT.ToString()], (long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MAXVALUE8BIT.ToString()]);
						break;
					case (2):
						randomValue = RTC_Core.RND.RandomLong((long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MINVALUE16BIT.ToString()], (long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MAXVALUE16BIT.ToString()]);
						break;
					case (4):
						randomValue = RTC_Core.RND.RandomLong((long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MINVALUE32BIT.ToString()], (long)RTC_Unispec.RTCSpec[Spec.HELLGENIE_MAXVALUE32BIT.ToString()]);
						break;
				}

				if (randomValue != -1)
				{
					value = RTC_Extensions.GetByteArrayValue(precision, randomValue, true);
				}
				else
				{
					for (int i = 0; i < precision; i++)
					{
						value[i] = (byte)RTC_Core.RND.Next();
					}
				}

				return new BlastUnit(value, domain, safeAddress, precision, mdp.BigEndian, 0, 0);
			}
			catch (Exception ex)
			{
				throw new Exception("HellGenie Engine GenerateUnit Threw Up" + ex);
			}
		}

	}
}

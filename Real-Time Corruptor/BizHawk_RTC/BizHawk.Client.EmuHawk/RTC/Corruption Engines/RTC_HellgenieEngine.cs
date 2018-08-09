using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{

		public static long MinValue8Bit = 0;
		public static long MaxValue8Bit = 0xFF;

		public static long MinValue16Bit = 0;
		public static long MaxValue16Bit = 0xFFFF;

		public static long MinValue32Bit = 0;
		public static long MaxValue32Bit = 0xFFFFFFFF;

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
						randomValue = RTC_Core.RND.RandomLong(MinValue8Bit, MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_Core.RND.RandomLong(MinValue16Bit, MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_Core.RND.RandomLong(MinValue32Bit, MaxValue32Bit);
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
				MessageBox.Show("Something went wrong in the RTC Hellgenie Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

	}
}

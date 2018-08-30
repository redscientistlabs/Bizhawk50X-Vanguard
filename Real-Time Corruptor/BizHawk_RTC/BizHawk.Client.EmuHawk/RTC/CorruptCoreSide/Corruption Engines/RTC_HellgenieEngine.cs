using RTCV.NetCore;
using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{

		public static long MinValue8Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MinValue8Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MinValue8Bit", value)); }
		}
		public static long MaxValue8Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MaxValue8Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MaxValue8Bit", value)); }
		}

		public static long MinValue16Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MinValue16Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MinValue16Bit", value)); }
		}
		public static long MaxValue16Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MaxValue16Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MaxValue16Bit", value)); }
		}

		public static long MinValue32Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MinValue32Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MinValue32Bit", value)); }
		}
		public static long MaxValue32Bit
		{
			get { return (long)RTC_CorruptCore.spec["HellgenieEngine_MaxValue32Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "HellgenieEngine_MaxValue32Bit", value)); }
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["HellgenieEngine_MinValue8Bit"] = 0L;
			partial["HellgenieEngine_MaxValue8Bit"] = 0xFFL;

			partial["HellgenieEngine_MinValue16Bit"] = 0L;
			partial["HellgenieEngine_MaxValue16Bit"] = 0xFFFFL;

			partial["HellgenieEngine_MinValue32Bit"] = 0L;
			partial["HellgenieEngine_MaxValue32Bit"] = 0xFFFFFFFFL;

			return partial;
		}

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);

				Byte[] value = new Byte[precision];

				long safeAddress = address - (address % precision);

				long randomValue = -1;
				switch (precision)
				{
					case (1):
						randomValue = RTC_CorruptCore.RND.RandomLong(MinValue8Bit, MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_CorruptCore.RND.RandomLong(MinValue16Bit, MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_CorruptCore.RND.RandomLong(MinValue32Bit, MaxValue32Bit);
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
						value[i] = (byte)RTC_CorruptCore.RND.Next();
					}
				}

				return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 0);
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

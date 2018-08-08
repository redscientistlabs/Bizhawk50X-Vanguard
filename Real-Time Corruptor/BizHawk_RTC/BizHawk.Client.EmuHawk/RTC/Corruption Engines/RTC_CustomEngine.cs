using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_CustomEngine
	{
		public static long MinValue8Bit = 0;
		public static long MaxValue8Bit = 0xFF;

		public static long MinValue16Bit = 0;
		public static long MaxValue16Bit = 0xFFFF;

		public static long MinValue32Bit = 0;
		public static long MaxValue32Bit = 0xFFFFFFFF;

		public static BlastUnitSource Source = BlastUnitSource.VALUE;

		public static StoreType StoreType = StoreType.ONCE;
		public static ActionTime StoreTime = ActionTime.IMMEDIATE;
		public static CustomStoreAddress StoreAddress = CustomStoreAddress.RANDOM;

		public static int Delay = 0;
		public static int Lifetime = 1;

		public static BigInteger TiltValue = 1;

		public static ActionTime LimiterTime = ActionTime.GENERATE;

		public static bool UseLimiterList = false;

		public static bool Loop = false;
		
		public static CustomValueSource ValueSource = CustomValueSource.RANDOM;
		
		public static MD5 LimiterList = null;
		public static MD5 ValueList = null;

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;

				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);

				byte[] value = new byte[precision];
				long safeAddress = address - (address % precision);

				//If it's generation time limiter, return null if it's not on the list
				if (UseLimiterList && LimiterTime == ActionTime.GENERATE &&
				    !RTC_Filtering.LimiterPeekBytes(safeAddress, safeAddress + precision, LimiterList, mdp))
					return null;

				BlastUnit bu = new BlastUnit();

				switch (Source)
				{
					case BlastUnitSource.VALUE:
					{
						switch (ValueSource)
						{
							case CustomValueSource.VALUELIST:
							{
								value = RTC_Filtering.GetRandomConstant(ValueList);
							}
							break;

							case CustomValueSource.RANGE:
							{
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
									value = RTC_Extensions.GetByteArrayValue(precision, randomValue, true);
								else
									for (int i = 0; i < precision; i++)
										value[i] = (byte)RTC_Core.RND.Next();
							}
							break;

							case CustomValueSource.RANDOM:
							{
								for (int i = 0; i < precision; i++)
									value[i] = (byte)RTC_Core.RND.Next();
							}
							break;
								
						}
					}
					break;

					case BlastUnitSource.STORE:
					{
						bu.StoreType = StoreType;
						bu.StoreTime = StoreTime;

						switch (StoreAddress)
						{
							case CustomStoreAddress.RANDOM:
							{
								BlastTarget bt = RTC_Core.GetBlastTarget();
								long safeStartAddress = bt.Address - (bt.Address % precision);
								bu.SourceDomain = bt.Domain;
								bu.SourceAddress = safeStartAddress;
							}
							break;
							case CustomStoreAddress.SAME:
							{
								bu.SourceDomain = domain;
								bu.SourceAddress = safeAddress;
							}
							break;
						}
					}
					break;
				}
				bu.Value = value;
				bu.Address = safeAddress;
				bu.Domain = domain;
				bu.Source = Source;
				bu.ExecuteFrame = Delay;
				bu.Lifetime = Lifetime;
				bu.Precision = precision;
				bu.LimiterList = LimiterList;
				bu.LimiterTime = LimiterTime;
				bu.Loop = Loop;

				return bu;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Nightmare Engine. \n" +
				                "This is an RTC error, so you should probably send this to the RTC devs.\n" +
				                "If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				                ex.ToString());
				return null;
			}
		}


		public static bool IsConstant(byte[] bytes, string[] list, bool bigEndian)
		{
			if (list == null)
				return true;
			if (!bigEndian)
				return list.Contains(ByteArrayToString(bytes));
			else
			{
				Array.Reverse(bytes);
				return list.Contains(ByteArrayToString(bytes));
			}
		}

		public static string ByteArrayToString(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}

		public static byte[] GetRandomConstant(string[] list)
		{
			if (list == null)
			{
				byte[] buffer = new byte[4];
				RTC_Core.RND.NextBytes(buffer);
				return buffer;
			}

			return StringToByteArray(list[RTC_Core.RND.Next(list.Length)]);
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}
	}
}

using System;
using System.Linq;
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
		public static StoreTime StoreTime = StoreTime.IMMEDIATE;

		public static int Delay = 0;
		public static int Lifetime = 1;
		public static bool UseLimiterList = false;
		public static bool UseValueList = false;


		public static string[] LimiterList = null;
		public static string[] ValueList = null;
		#region constant lists

		public static string[] listOfTinyConstants = new string[]
		{
			"0000003c", //
			"0000803d", //
			"0000003e", //
			"0000803e", // = 0.25
			"0000003f", // = 0.50
			"0000403f", // = 0.75
			"000000bc", //
			"000080bd", //
			"000000be", //
			"000080be", // = -0.25
			"000000bf", // = -0.50
			"000040bf", // = -0.75
		};

		public static string[] constantOne = new string[]
		{
			"0000803f", // = 1
			"000080bf" // = -1
		};

		public static string[] constantPositiveOne = new string[]
		{
			"0000803f" // = 1
		};

		public static string[] constantPositiveTwo = new string[]
		{
			"00000040" // = 2
		};

		public static string[] listOfWholeConstants = new string[]
		{
			"0000803f", // = 1
			"00000040", // = 2
			"00004040", // = 3
			"00008040", // = 4
			"0000a040", // = 5
			"00000041", // = 8
			"00002041", // = 10
			"00008041", // = 16
			"00000042", // = 32
			"00008042", //
			"00000043", //
			"00008043", //
			"00000044", //
			"00008044", //
			"00000045", //
			"00008045", //
			"00000046", //
			"00008046", //
			"00000047", //
			"00008047", // = 65536
			"000080bf", // = -1
			"000000c0", // = -2
			"000040c0", // = -3
			"000080c0", // = -4
			"0000a0c0", // = -5
			"000000c1", // = -8
			"000020c1", // = -10
			"000080c1", // = -16
			"000000c2", // = -32
			"000080c2", //
			"000000c3", //
			"000080c3", //
			"000000c4", //
			"000080c4", //
			"000000c5", //
			"000080c5", //
			"000000c6", //
			"000080c6", //
			"000000c7", //
			"000080c7" // = -65536
		};

		public static string[] listOfWholePositiveConstants = new string[]
		{
			"0000803f", // = 1
			"00000040", // = 2
			"00004040", // = 3
			"00008040", // = 4
			"0000a040", // = 5
			"00000041", // = 8
			"00002041", // = 10
			"00008041", // = 16
			"00000042", // = 32
			"00008042", //
			"00000043", //
			"00008043", //
			"00000044", //
			"00008044", //
			"00000045", //
			"00008045", //
			"00000046", //
			"00008046", //
			"00000047", //
			"00008047", // = 65536
		};

		public static string[] listOfPositiveConstants = new string[]
		{
			"0000003c", //
			"0000803d", //
			"0000003e", //
			"0000803e", // = 0.25
			"0000003f", // = 0.50
			"0000403f", // = 0.75
			"0000803f", // = 1
			"00000040", // = 2
			"00004040", // = 3
			"00008040", // = 4
			"0000a040", // = 5
			"00000041", // = 8
			"00002041", // = 10
			"00008041", // = 16
			"00000042", // = 32
			"00008042", //
			"00000043", //
			"00008043", //
			"00000044", //
			"00008044", //
			"00000045", //
			"00008045", //
			"00000046", //
			"00008046", //
			"00000047", //
			"00008047" // = 65536
		};

		public static string[] listOfNegativeConstants = new string[]
		{
			"000000bc", //
			"000080bd", //
			"000000be", //
			"000080be", // = -0.25
			"000000bf", // = -0.50
			"000040bf", // = -0.75
			"000080bf", // = -1
			"000000c0", // = -2
			"000040c0", // = -3
			"000080c0", // = -4
			"0000a0c0", // = -5
			"000000c1", // = -8
			"000020c1", // = -10
			"000080c1", // = -16
			"000000c2", // = -32
			"000080c2", //
			"000000c3", //
			"000080c3", //
			"000000c4", //
			"000080c4", //
			"000000c5", //
			"000080c5", //
			"000000c6", //
			"000080c6", //
			"000000c7", //
			"000080c7" // = -65536
		};

		public static string[] extendedListOfConstants = new string[]
		{
			"0000003c", //
			"0000803d", //
			"0000003e", //
			"0000803e", // = 0.25
			"0000003f", // = 0.50
			"0000403f", // = 0.75
			"0000803f", // = 1
			"00000040", // = 2
			"00004040", // = 3
			"00008040", // = 4
			"0000a040", // = 5
			"00000041", // = 8
			"00002041", // = 10
			"00008041", // = 16
			"00000042", // = 32
			"00008042", //
			"00000043", //
			"00008043", //
			"00000044", //
			"00008044", //
			"00000045", //
			"00008045", //
			"00000046", //
			"00008046", //
			"00000047", //
			"00008047", // = 65536
			"000000bc", //
			"000080bd", //
			"000000be", //
			"000080be", // = -0.25
			"000000bf", // = -0.50
			"000040bf", // = -0.75
			"000080bf", // = -1
			"000000c0", // = -2
			"000040c0", // = -3
			"000080c0", // = -4
			"0000a0c0", // = -5
			"000000c1", // = -8
			"000020c1", // = -10
			"000080c1", // = -16
			"000000c2", // = -32
			"000080c2", //
			"000000c3", //
			"000080c3", //
			"000000c4", //
			"000080c4", //
			"000000c5", //
			"000080c5", //
			"000000c6", //
			"000080c6", //
			"000000c7", //
			"000080c7" // = -65536
		};

		#endregion constant lists

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;

				BlastUnit bu = new BlastUnit();

				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);

				byte[] value = new byte[precision];
				long safeAddress = address - (address % precision);

				switch (Source)
				{
					case BlastUnitSource.VALUE:
					{
						if (UseValueList)
						{
							//todo
						}
						else
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
					}
					break;

					case BlastUnitSource.STORE:
					{
						BlastTarget bt = RTC_Core.GetBlastTarget();
						long safeStartAddress = bt.Address - (bt.Address % precision);

						bu.StoreType = StoreType;
						bu.ActionTime = StoreTime;
						bu.SourceDomain = bt.Domain;
						bu.SourceAddress = safeStartAddress;
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

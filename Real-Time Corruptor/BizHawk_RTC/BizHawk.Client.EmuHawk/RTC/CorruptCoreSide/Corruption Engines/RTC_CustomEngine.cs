using RTCV.NetCore;
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
		public static long MinValue8Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MinValue8Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MinValue8Bit", value)); }
		}
		public static long MaxValue8Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MaxValue8Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MaxValue8Bit", value)); }
		}

		public static long MinValue16Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MinValue16Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MinValue16Bit", value)); }
		}
		public static long MaxValue16Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MaxValue16Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MaxValue16Bit", value)); }
		}

		public static long MinValue32Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MinValue32Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MinValue32Bit", value)); }
		}
		public static long MaxValue32Bit
		{
			get { return (long)RTC_CorruptCore.spec["CustomEngine_MaxValue32Bit"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_MaxValue32Bit", value)); }
		}

		public static BlastUnitSource Source
		{
			get { return (BlastUnitSource)RTC_CorruptCore.spec["CustomEngine_Source"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_Source", value)); }
		}

		public static StoreType StoreType
		{
			get { return (StoreType)RTC_CorruptCore.spec["CustomEngine_StoreType"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_StoreType", value)); }
		}
		public static ActionTime StoreTime
		{
			get { return (ActionTime)RTC_CorruptCore.spec["CustomEngine_StoreTime"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_StoreTime", value)); }
		}
		public static CustomStoreAddress StoreAddress
		{
			get { return (CustomStoreAddress)RTC_CorruptCore.spec["CustomEngine_StoreAddress"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_StoreAddress", value)); }
		}

		public static int Delay
		{
			get { return (int)RTC_CorruptCore.spec["CustomEngine_Delay"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_Delay", value)); }
		}
		public static int Lifetime
		{
			get { return (int)RTC_CorruptCore.spec["CustomEngine_Lifetime"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_Lifetime", value)); }
		}

		public static BigInteger TiltValue
		{
			get { return (BigInteger)RTC_CorruptCore.spec["CustomEngine_TiltValue"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "TCustomEngine_iltValue", value)); }
		}

		public static ActionTime LimiterTime
		{
			get { return (ActionTime)RTC_CorruptCore.spec["CustomEngine_LimiterTime"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_LimiterTime", value)); }
		}
		public static bool LimiterInverted
		{
			get { return (bool)RTC_CorruptCore.spec["CustomEngine_LimiterInverted"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_LimiterInverted", value)); }
		}


		public static bool Loop
		{
			get { return (bool)RTC_CorruptCore.spec["CustomEngine_Loop"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_Loop", value)); }
		}

		public static CustomValueSource ValueSource
		{
			get { return (CustomValueSource)RTC_CorruptCore.spec["CustomEngine_ValueSource"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_ValueSource", value)); }
		}

		public static string LimiterListHash
		{
			get { return (string)RTC_CorruptCore.spec["CustomEngine_LimiterListHash"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_LimiterListHash", value)); }
		}

		public static string ValueListHash
		{
			get { return (string)RTC_CorruptCore.spec["CustomEngine_ValueListHash"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "CustomEngine_ValueListHash", value)); }
		}


		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["CustomEngine_MinValue8Bit"] = 0L;
			partial["CustomEngine_MaxValue8Bit"] = 0xFFL;
					 
			partial["CustomEngine_MinValue16Bit"] = 0;
			partial["CustomEngine_MaxValue16Bit"] = 0xFFFFL;
					 
			partial["CustomEngine_MinValue32Bit"] = 0L;
			partial["CustomEngine_MaxValue32Bit"] = 0xFFFFFFFFL;
					 
			partial["CustomEngine_Source"] = BlastUnitSource.VALUE;
					 
			partial["CustomEngine_StoreType"] = StoreType.ONCE;
			partial["CustomEngine_StoreTime"] = ActionTime.IMMEDIATE;
			partial["CustomEngine_StoreAddress"] = CustomStoreAddress.RANDOM;
					 
			partial["CustomEngine_Delay"] = 0;
			partial["CustomEngine_Lifetime"] = 1;
					 
			partial["CustomEngine_TiltValue"] = 1;
					 
			partial["CustomEngine_LimiterTime"] = ActionTime.NONE;
			partial["CustomEngine_LimiterInverted"] = false;
					 
					 
			partial["CustomEngine_Loop"] = false;
					 
			partial["CustomEngine_ValueSource"] = CustomValueSource.RANDOM;
					 
			partial["CustomEngine_LimiterListHash"] = null;
			partial["CustomEngine_ValueListHash"] = null;

			return partial;
		}


		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			try
			{
				if (domain == null)
					return null;

				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);


				byte[] value = new byte[precision];
				long safeAddress = address - (address % precision);

			
				BlastUnit bu = new BlastUnit();

				switch (Source)
				{
					case BlastUnitSource.VALUE:
					{
						switch (ValueSource)
						{
							case CustomValueSource.VALUELIST:
							{
								value = RTC_Filtering.GetRandomConstant(ValueListHash);
							}
							break;

							case CustomValueSource.RANGE:
							{
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
									value = RTC_Extensions.GetByteArrayValue(precision, randomValue, true);
								else
									for (int i = 0; i < precision; i++)
										value[i] = (byte)RTC_CorruptCore.RND.Next();
							}
							break;

							case CustomValueSource.RANDOM:
							{
								for (int i = 0; i < precision; i++)
									value[i] = (byte)RTC_CorruptCore.RND.Next();
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
								BlastTarget bt = RTC_CorruptCore.GetBlastTarget();
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
				bu.LimiterTime = LimiterTime;
				bu.Loop = Loop;
				bu.InvertLimiter = LimiterInverted;

				//Only set a list if it's used to save on memory
				if (LimiterTime != ActionTime.NONE)
					bu.LimiterListHash = LimiterListHash;

				//Limiter handling
				if (LimiterTime == ActionTime.GENERATE)
				{
					if (LimiterInverted)
					{
						//If it's store, we need to use the sourceaddress and sourcedomain
						if (Source == BlastUnitSource.STORE && RTC_Filtering.LimiterPeekBytes(bu.SourceAddress,
							    bu.SourceAddress + bu.Precision, bu.SourceDomain, LimiterListHash, mdp))
							return null;
						//If it's VALUE, we need to use the address and domain
						else if (Source == BlastUnitSource.VALUE && RTC_Filtering.LimiterPeekBytes(bu.Address,
							         bu.Address + bu.Precision, bu.Domain, LimiterListHash, mdp))
							return null;
					}
					else
					{
						//If it's store, we need to use the sourceaddress and sourcedomain
						if (Source == BlastUnitSource.STORE && !RTC_Filtering.LimiterPeekBytes(bu.SourceAddress,
							    bu.SourceAddress + bu.Precision, bu.SourceDomain, LimiterListHash, mdp))
							return null;
						//If it's VALUE, we need to use the address and domain
						else if (Source == BlastUnitSource.VALUE && !RTC_Filtering.LimiterPeekBytes(bu.Address,
							         bu.Address + bu.Precision, bu.Domain, LimiterListHash, mdp))
							return null;
					}
				}

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
				RTC_CorruptCore.RND.NextBytes(buffer);
				return buffer;
			}

			return StringToByteArray(list[RTC_CorruptCore.RND.Next(list.Length)]);
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

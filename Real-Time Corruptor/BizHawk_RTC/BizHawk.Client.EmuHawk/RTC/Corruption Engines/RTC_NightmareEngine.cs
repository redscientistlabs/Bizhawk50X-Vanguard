using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_NightmareEngine
	{

		public static NightmareAlgo Algo = NightmareAlgo.RANDOM;
		private static NightmareType type = NightmareType.SET;

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				switch (Algo)
				{
					case NightmareAlgo.RANDOM: //RANDOM always sets a random value
						type = NightmareType.SET;
						break;

					case NightmareAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
						int result = RTC_Core.RND.Next(1, 4);
						switch (result)
						{
							case 1:
								type = NightmareType.ADD;
								break;
							case 2:
								type = NightmareType.SUBTRACT;
								break;
							case 3:
								type = NightmareType.SET;
								break;
							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) RANDOMTILT)");
								return null;
						}

						break;

					case NightmareAlgo.TILT: //TILT can either add 1 or substract 1
						result = RTC_Core.RND.Next(1, 3);
						switch (result)
						{
							case 1:
								type = NightmareType.ADD;
								break;

							case 2:
								type = NightmareType.SUBTRACT;
								break;

							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) TILT)");
								return null;
						}
						break;
				}


				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);

				byte[] value = new byte[precision];

				long safeAddress = address - (address % precision);

				if (type == NightmareType.SET)
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

					return new BlastUnit(value, domain, safeAddress, precision, mdp.BigEndian, 0, 1);
				}
				//Tilt. Backup with a + or -
				else
				{
					BlastUnit bu = new BlastUnit(StoreType.ONCE, ActionTime.GENERATE, domain, safeAddress, domain, safeAddress, precision, mdp.BigEndian);
					if (type == NightmareType.ADD)
						bu.TiltValue = 1;
					else
						bu.TiltValue = 0;
					return bu;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Nightmare Engine GenerateUnit Threw Up" + ex);
			}
		}
	}
}

using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_NightmareEngine
	{
		public static BlastByteAlgo Algo = BlastByteAlgo.RANDOM;
		public static long MinValue = 0;
		public static long MaxValue = 255;

		public static BlastUnit GenerateUnit(string domain, long address)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				BlastByteType type = BlastByteType.NONE;

				switch (Algo)
				{
					case BlastByteAlgo.RANDOM: //RANDOM always sets a random value
						type = BlastByteType.SET;
						break;

					case BlastByteAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
						int result = RTC_Core.RND.Next(1, 4);
						switch (result)
						{
							case 1:
								type = BlastByteType.ADD;
								break;
							case 2:
								type = BlastByteType.SUBSTRACT;
								break;
							case 3:
								type = BlastByteType.SET;
								break;
							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) RANDOMTILT)");
								return null;
						}

						break;

					case BlastByteAlgo.TILT: //TILT can either add 1 or substract 1
						result = RTC_Core.RND.Next(1, 3);
						switch (result)
						{
							case 1:
								type = BlastByteType.ADD;
								break;

							case 2:
								type = BlastByteType.SUBSTRACT;
								break;

							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) TILT)");
								return null;
						}
						break;
				}

				byte[] value = RTC_Core.CustomPrecision == -1 ? new byte[mdp.WordSize] : new byte[RTC_Core.CustomPrecision];

				long safeAddress = address - (address % value.Length);

				if (type == BlastByteType.SET)
				{
					long randomValue = RTC_Core.RND.RandomLong(MinValue, MaxValue);

					value = RTC_Extensions.GetByteArrayValue(value.Length, randomValue, true);
				}
				else //ADD, SUBSTRACT
				{
					//Add and subtract only need the last byte set as we're only incrementing by 1. If we set all the bytes, it'd tilt far more than 1.
					//1 by default because Add(1) or Substract(1) but more is still possible
					value[value.Length - 1] = 1;
				}

				return new BlastByte(domain, safeAddress, type, value, mdp.BigEndian, true);
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
	}
}

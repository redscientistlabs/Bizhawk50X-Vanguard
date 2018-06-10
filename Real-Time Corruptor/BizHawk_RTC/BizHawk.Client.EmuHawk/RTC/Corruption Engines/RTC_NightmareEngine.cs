using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_NightmareEngine
	{
		public static BlastByteAlgo Algo = BlastByteAlgo.RANDOM;

		public static BlastUnit GenerateUnit(string _domain, long _address)
		{
			// Randomly selects a memory operation according to the selected algorithm

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
				BlastByteType Type = BlastByteType.NONE;

				switch (Algo)
				{
					case BlastByteAlgo.RANDOM: //RANDOM always sets a random value
						Type = BlastByteType.SET;
						break;

					case BlastByteAlgo.RANDOMTILT: //RANDOMTILT may add 1,substract 1 or set a random value
						int result = RTC_Core.RND.Next(1, 4);
						switch (result)
						{
							case 1:
								Type = BlastByteType.ADD;
								break;
							case 2:
								Type = BlastByteType.SUBSTRACT;
								break;
							case 3:
								Type = BlastByteType.SET;
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
								Type = BlastByteType.ADD;
								break;

							case 2:
								Type = BlastByteType.SUBSTRACT;
								break;

							default:
								MessageBox.Show("Random returned an unexpected value (RTC_NightmareEngine switch(Algo) TILT)");
								return null;
						}
						break;
				}

				byte[] _value;
				if (RTC_Core.CustomPrecision == -1)
					_value = new byte[mdp.WordSize];
				else
					_value = new byte[RTC_Core.CustomPrecision];

				long safeAddress = _address - (_address % _value.Length);

				if (Type == BlastByteType.SET)
				{
					for (int i = 0; i < _value.Length; i++)
						_value[i] = (byte)RTC_Core.RND.Next(0, 255);
				}
				else //ADD, SUBSTRACT
				{
					//Add and subtract only need the first byte set as we're only incrementing by 1. If we set all the bytes, it'd tilt far more than 1.
					//1 by default because Add(1) or Substract(1) but more is still possible
					if (!mdp.BigEndian)
						_value[0] = 1;
					else
						_value[_value.Length - 1] = 1;
				}

				return new BlastByte(_domain, safeAddress, Type, _value, mdp.BigEndian, true);
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

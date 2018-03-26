using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{

    public static class RTC_NightmareEngine
    {
		public static string[] limiterList = null;
		public static string[] valueList = null;

		public static string lastDomain = null;
		public static byte[] lastValues = null;

		#region constant lists

		public static string[] listTest = new string[]
		{
			"1",
			"2",
		};
		#endregion

		public static BlastByteAlgo Algo = BlastByteAlgo.RANDOM;

        public static BlastUnit GenerateUnit(string _domain, long _address)
        {

			// Randomly selects a memory operation according to the selected algorithm
			bool isFiltered = false;
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
					case BlastByteAlgo.FILTERED: //Filtered replace
						isFiltered = true;
						Type = BlastByteType.SET;
						break;

				}


				if (!isFiltered)
				{

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
						for (int i = 0; i < _value.Length; i++)  //1 by default because Add(1) or Substract(1) but more is still possible
							_value[i] = 1;
					}

					return new BlastByte(_domain, safeAddress, Type, _value, true);
				}
				else
				{

					BlastByte bu = null;
					int size;
					byte[] _value;

					if (RTC_Core.CustomPrecision == -1)
						size = mdp.WordSize;
					else
						size = RTC_Core.CustomPrecision;

					_value = new byte[size];

					long safeAddress = _address - (_address % _value.Length);

					lastValues = readBytes(mdp, safeAddress, size);
					lastDomain = _domain;


					if (isConstant(lastValues, limiterList))
						bu = new BlastByte(_domain, safeAddress, Type, getRandomConstant(valueList), true);
					return bu;
				}

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Nightmare Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }

		}
		public static byte[] readBytes(MemoryDomainProxy mdp, long address, int size)
		{
			byte[] _bytes = new byte[size];

			for (int i = 0; i < size; i++)
			{
				_bytes[i] = mdp.PeekByte(address + i);
			}

			return _bytes;
		}
		public static bool isConstant(byte[] bytes, string[] list)
		{
			if (list == null)
				return true;

			return list.Contains(ByteArrayToString(bytes));
		}

		public static string ByteArrayToString(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", "").ToLower();
		}

		public static byte[] getRandomConstant(string[] list)
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
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;

namespace RTC
{

	public static class RTC_BitEngine
	{

		public static string lastDomain = null;
		public static byte[] lastValues = null;

		public static string[] limiterList = null;
		public static string[] valueList = null;

		#region constant lists

		public static string[] listOfTinyConstants = new string[]
		{
			"3c000000", //
			"3d800000", //
			"3e000000", //
			"3e800000", // = 0.25
			"3f000000", // = 0.50
			"3f400000", // = 0.75
			"bc000000", //
			"bd800000", //
			"be000000", //
			"be800000", // = -0.25
			"bf000000", // = -0.50
			"bf400000", // = -0.75
		};

		public static string[] constantOne = new string[]
		{
			"3f800000", // = 1
			"bf800000" // = -1
		};

		public static string[] constantPositiveOne = new string[]
		{
			"3f800000" // = 1
		};


		public static string[] constantPositiveTwo = new string[]
		{
			"40000000" // = 2
		};

		public static string[] listOfWholeConstants = new string[]
		{
			"3f800000", // = 1
			"40000000", // = 2
			"40400000", // = 3
			"40800000", // = 4
			"40a00000", // = 5
			"41000000", // = 8
			"41200000", // = 10
			"41800000", // = 16
			"42000000", // = 32
			"42800000", //
			"43000000", //
			"43800000", //
			"44000000", //
			"44800000", //
			"45000000", //
			"45800000", //
			"46000000", //
			"46800000", //
			"47000000", //
			"47800000", // = 65536
			"bf800000", // = -1
			"c0000000", // = -2
			"c0400000", // = -3
			"c0800000", // = -4
			"c0a00000", // = -5
			"c1000000", // = -8
			"c1200000", // = -10
			"c1800000", // = -16
			"c2000000", // = -32
			"c2800000", //
			"c3000000", //
			"c3800000", //
			"c4000000", //
			"c4800000", //
			"c5000000", //
			"c5800000", //
			"c6000000", //
			"c6800000", //
			"c7000000", //
			"c7800000" // = -65536
		};

		public static string[] listOfWholePositiveConstants = new string[]
		{
			"3f800000", // = 1
			"40000000", // = 2
			"40400000", // = 3
			"40800000", // = 4
			"40a00000", // = 5
			"41000000", // = 8
			"41200000", // = 10
			"41800000", // = 16
			"42000000", // = 32
			"42800000", //
			"43000000", //
			"43800000", //
			"44000000", //
			"44800000", //
			"45000000", //
			"45800000", //
			"46000000", //
			"46800000", //
			"47000000", //
			"47800000", // = 65536
		};

		public static string[] listOfPositiveConstants = new string[]
		{
			"3c000000", //
			"3d800000", //
			"3e000000", //
			"3e800000", // = 0.25
			"3f000000", // = 0.50
			"3f400000", // = 0.75
			"3f800000", // = 1
			"40000000", // = 2
			"40400000", // = 3
			"40800000", // = 4
			"40a00000", // = 5
			"41000000", // = 8
			"41200000", // = 10
			"41800000", // = 16
			"42000000", // = 32
			"42800000", //
			"43000000", //
			"43800000", //
			"44000000", //
			"44800000", //
			"45000000", //
			"45800000", //
			"46000000", //
			"46800000", //
			"47000000", //
			"47800000" // = 65536
		};

		public static string[] listOfNegativeConstants = new string[]
		{
			"bc000000", //
			"bd800000", //
			"be000000", //
			"be800000", // = -0.25
			"bf000000", // = -0.50
			"bf400000", // = -0.75
			"bf800000", // = -1
			"c0000000", // = -2
			"c0400000", // = -3
			"c0800000", // = -4
			"c0a00000", // = -5
			"c1000000", // = -8
			"c1200000", // = -10
			"c1800000", // = -16
			"c2000000", // = -32
			"c2800000", //
			"c3000000", //
			"c3800000", //
			"c4000000", //
			"c4800000", //
			"c5000000", //
			"c5800000", //
			"c6000000", //
			"c6800000", //
			"c7000000", //
			"c7800000" // = -65536
		};

		public static string[] extendedListOfConstants = new string[]
		{
			"3c000000", //
			"3d800000", //
			"3e000000", //
			"3e800000", // = 0.25
			"3f000000", // = 0.50
			"3f400000", // = 0.75
			"3f800000", // = 1
			"40000000", // = 2
			"40400000", // = 3
			"40800000", // = 4
			"40a00000", // = 5
			"41000000", // = 8
			"41200000", // = 10
			"41800000", // = 16
			"42000000", // = 32
			"42800000", //
			"43000000", //
			"43800000", //
			"44000000", //
			"44800000", //
			"45000000", //
			"45800000", //
			"46000000", //
			"46800000", //
			"47000000", //
			"47800000", // = 65536
			"bc000000", //
			"bd800000", //
			"be000000", //
			"be800000", // = -0.25
			"bf000000", // = -0.50
			"bf400000", // = -0.75
			"bf800000", // = -1
			"c0000000", // = -2
			"c0400000", // = -3
			"c0800000", // = -4
			"c0a00000", // = -5
			"c1000000", // = -8
			"c1200000", // = -10
			"c1800000", // = -16
			"c2000000", // = -32
			"c2800000", //
			"c3000000", //
			"c3800000", //
			"c4000000", //
			"c4800000", //
			"c5000000", //
			"c5800000", //
			"c6000000", //
			"c6800000", //
			"c7000000", //
			"c7800000" // = -65536
		};

		#endregion


		public static BlastByteAlgo Algo = BlastByteAlgo.RANDOM;
		public static BlastUnit GenerateUnit(string _domain, long _address)
		{

			// Randomly selects a memory operation according to the selected algorithm

			//long safeAddress = _address - (_address % 8); //64-bit trunk

			try
			{
				long safeAddress = _address - (_address % 4); //32-bit trunk
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, safeAddress);
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
				_value = new byte[1];


				//Corrupts one of the first three bits
				if (Type == BlastByteType.SET)
				{
					BitArray _bits = new BitArray(BitConverter.GetBytes(mdp.PeekByte(safeAddress)).ToArray());
                    _bits[0] = !(_bits[0]);
                    _value[0] = BitArrayToByte(_bits);
				}
				else //ADD, SUBSTRACT
				{
					for (int i = 0; i < _value.Length; i++)  //1 by default because Add(1) or Substract(1) but more is still possible
						_value[i] = 1;
				}

				return new BlastByte(_domain, safeAddress, Type, _value, true);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Bit Engine. \n" +
								"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
								ex.ToString());
				return null;
			}
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

		public static byte[] read32bits(MemoryDomainProxy mdp, long address)
		{
			return new byte[] { mdp.PeekByte(address),
								mdp.PeekByte(address + 1),
								mdp.PeekByte(address + 2),
								mdp.PeekByte(address + 3)
			};
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
        public static byte BitArrayToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGlitchHarvester
{
	public static class WGH_VectorEngine
	{

		public static string lastDomain = null;
		public static byte[] lastValues = null;

		public static string[] limiterList = null;
		public static string[] valueList = null;

        public static long vectorOffset = 0;
        public static bool vectorAligned = true;

        public static int limiterMin = 0;
        public static int limiterMax = 0;
        public static int valueMin = 0;
        public static int valueMax = 0;
        public static bool customWholeNumbers = false;

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
            "477FFF00", // 65535
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

        public static string[] superExtendedListOfConstants = new string[]
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
			"c7800000", // = -65536
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

        public static string[] customList = new string[]
        {
            "custom"
		};

        public static bool BigEndian = false;

		#endregion

		public static BlastUnit GenerateUnit(string _domain, long _address)
		{
            long safeAddress;

            // Randomly seleclong safeAddress ts a memory operation according to the selected algorithm
            if (vectorAligned)
                safeAddress = (_address - ((_address % 4 )) + vectorOffset);
            else
                safeAddress = (_address + vectorOffset);


            MemoryInterface mi = WGH_Core.currentMemoryInterface;
			//MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(_domain);

			if (mi == null)
				return null;


			try
			{

				BlastVector bv = null;

				lastValues = read32bits(mi, safeAddress);
				lastDomain = _domain;


                if (isConstant(lastValues, limiterList))
					bv = new BlastVector(_domain, _address, getRandomConstant(valueList), true);

				return bv;




			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Vector Engine. \n" +
								"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
								ex.ToString());
				return null;
			}
		}

		public static bool isConstant(byte[] bytes, string[] list)
		{
            if (list == null)
                return true;

            if (list[0] == "custom")
            {
                float value;

                if (WGH_VectorEngine.BigEndian)
                {
                    value = ByteArrayToFloat(FlipBytes(bytes));
                }
                else
                    value = ByteArrayToFloat(bytes);

                if (value == 0)
                   return false;
                if (customWholeNumbers)
                {

                    if ((Math.Abs(value % 1) < Double.Epsilon) && value <= limiterMax && value >= limiterMin)
                    {
                        return true;
                    }
                    return false;
                }

                if (value <= limiterMax && value >= limiterMin)
                    return true;

                return false;
            }

            if (!WGH_VectorEngine.BigEndian)
			{
                return list.Contains(ByteArrayToString(FlipBytes(bytes)));
            }

            return list.Contains(ByteArrayToString(bytes));
		}

        public static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        public static float ByteArrayToFloat(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        public static byte[] read32bits(MemoryInterface mi, long address)
		{
			return mi.PeekBytes(address, 4);
		}

		public static byte[] getRandomConstant(string[] list)
        {
            byte[] bytes;
            if (list == null)
            {
                byte[] buffer = new byte[4];
                WGH_Core.RND.NextBytes(buffer);
                return buffer;
            }
            if (list[0] == "custom")
            {

                if (customWholeNumbers)
                {
                    int buffer = WGH_Core.RND.Next(valueMin, valueMax);
                    bytes = BitConverter.GetBytes((float)buffer);
                }
                else
                {
                    float buffer = WGH_Core.RND.Next(valueMin, valueMax) + (float)WGH_Core.RND.NextDouble();
                    bytes = BitConverter.GetBytes(buffer);
                }

                //This generates a little endian byte array. The normal byte flip check assumes the input is big endian, so handle it separately instead of flipping the bytes twice
                if (WGH_VectorEngine.BigEndian)
                {
                    return FlipBytes(bytes);
                }
                return bytes;
            }

            bytes = StringToByteArray(list[WGH_Core.RND.Next(list.Length)]);

            //Assume we have big endian input for some reason. If it's little endian, flip the bytes. 
            if (!WGH_VectorEngine.BigEndian)
            {
                return FlipBytes(bytes);
            }

            return bytes;
		}

        public static byte[] FlipBytes(byte[] bytes)
        {
            byte[] _bytes = (byte[])bytes.Clone();

            _bytes[0] = bytes[3];
            _bytes[1] = bytes[2];
            _bytes[2] = bytes[1];
            _bytes[3] = bytes[0];

            return _bytes;
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

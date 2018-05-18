using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;

namespace RTC
{

    public static class RTC_VectorEngine
    {

		public static string lastDomain = null;
		public static byte[] lastValues = null;

		public static string[] limiterList = null;
		public static string[] valueList = null;
	
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

		#endregion

		public static BlastUnit GenerateUnit(string _domain, long _address)
        {

            // Randomly selects a memory operation according to the selected algorithm

            //long safeAddress = _address - (_address % 8); //64-bit trunk
            long safeAddress = _address - (_address % 4); //32-bit trunk

            MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, safeAddress);
			if (mdp == null)
				return null;


			try
			{

				BlastByte bu = null;

				lastValues = mdp.PeekBytes(safeAddress, safeAddress + 4);
				lastDomain = _domain;

				//Enforce the safeaddress at generation
				if (isConstant(lastValues, limiterList, mdp.BigEndian))
					bu = new BlastByte(_domain, safeAddress, BlastByteType.VECTOR, getRandomConstant(valueList), mdp.BigEndian, true);

				return bu;

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Vector Engine. \n" +
								"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
								ex.ToString());
				return null;
			}
        }

		public static bool isConstant(byte[] bytes, string[] list, bool BigEndian)
		{
            if (list == null)
                return true;
			if(!BigEndian)
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

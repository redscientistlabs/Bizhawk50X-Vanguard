using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RTC
{
	public static class RTC_Filtering
	{

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

		private static Dictionary<MD5, String[]> hash2LimiterDico = new Dictionary<MD5, string[]>();
		private static Dictionary<MD5, String[]> hash2ValueDico = new Dictionary<MD5, string[]>();

		

		public static void RegisterList(String[] list, bool isValueList)
		{
			//Make one giant string to hash
			string _list = String.Empty;
			foreach (String str in list)
				String.Concat(_list, str);

			//Hash it
			MD5 hash = MD5.Create();
			hash.ComputeHash(_list.GetBytes());

			if (isValueList)
			{
				if (!hash2ValueDico.ContainsKey(hash))
					hash2ValueDico.Add(hash, list);
			}				
			else
			{
				if (!hash2LimiterDico.ContainsKey(hash))
					hash2LimiterDico.Add(hash, list);
			}
		}

		public static bool LimiterPeekBytes(long startAddress, long endAddress, MD5 hash, MemoryDomainProxy mdp)
		{
			byte[] values = mdp.PeekBytes(startAddress, endAddress);
			//The compare is done as big endian
			if (!mdp.BigEndian)
				values = values.FlipBytes();

			if (LimiterContainsValue(values, hash))
				return true;

			return false;
		}

		public static bool LimiterContainsValue(byte[] bytes, MD5 hash)
		{
			if (!hash2LimiterDico.ContainsKey(hash))
				return false;

			string str = BitConverter.ToString(bytes).Replace("-", "").ToUpper();

			return hash2LimiterDico[hash].Contains(str);
		}


		public static byte[] GetRandomConstant(MD5 hash)
		{
			if (!hash2ValueDico.ContainsKey(hash))
			{
				return null;
			}

			return StringToByteArray(hash2ValueDico[hash][RTC_Core.RND.Next(hash2ValueDico[hash].Length)]);
		}

		private static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
							 .Where(x => x % 2 == 0)
							 .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
							 .ToArray();
		}
	}
}
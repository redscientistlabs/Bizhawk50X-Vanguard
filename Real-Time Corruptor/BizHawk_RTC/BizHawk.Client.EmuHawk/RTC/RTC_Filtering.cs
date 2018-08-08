using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RTC
{
	public static class RTC_Filtering
	{

		private static Dictionary<MD5, String[]> hash2LimiterDico = new Dictionary<MD5, string[]>();
		private static Dictionary<MD5, String[]> hash2ValueDico = new Dictionary<MD5, string[]>();

		
		public static List<MD5> LoadListsFromPaths(string[] paths)
		{
			List<MD5> md5s = new List<MD5>();

			foreach(string path in paths)
			{
				md5s.Add(LoadListFromPath(path));
			}
			return md5s;
		}

		public static MD5 LoadListFromPath(string path)
		{
			string[] temp = File.ReadAllLines(path);
			bool flipBytes = path.StartsWith("_");

			for (int i = 0; i < temp.Length; i++)
			{
				temp[i].Trim();
				temp[i].ToUpper();
				//If it's big endian, flip it. this is ugly and slow but it works
				if (flipBytes)
				{
					byte[] bytes = StringToByteArray(temp[i]);
					bytes = bytes.FlipBytes();
					temp[i] = bytes.ToString();
				}
			}

			return RegisterList(temp);
		}

		private static MD5 RegisterList(String[] list)
		{
			//Make one giant string to hash
			string _list = String.Empty;
			foreach (String str in list)
				String.Concat(_list, str);

			//Hash it
			MD5 hash = MD5.Create();
			hash.ComputeHash(_list.GetBytes());

			if (!hash2ValueDico.ContainsKey(hash))
				hash2ValueDico.Add(hash, list);
			if (!hash2LimiterDico.ContainsKey(hash))
				hash2LimiterDico.Add(hash, list);

			return hash;
		}

		public static bool LimiterPeekBytes(long startAddress, long endAddress, MD5 hash, MemoryDomainProxy mdp)
		{
			byte[] values = mdp.PeekBytes(startAddress, endAddress);
			//The compare is done as little endian
			if (mdp.BigEndian)
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
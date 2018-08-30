using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
namespace RTC
{
	public static class RTC_Filtering
	{

		public static SerializableDico<string, String[]> Hash2LimiterDico
		{
			get { return (SerializableDico<string, String[]>)RTC_CorruptCore.spec["Filtering_Hash2LimiterDico"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "Filtering_Hash2LimiterDico", value)); }
		}
		public static SerializableDico<string, String[]> Hash2ValueDico
		{
			get { return (SerializableDico<string, String[]>)RTC_CorruptCore.spec["Filtering_Hash2ValueDico"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "Filtering_Hash2ValueDico", value)); }
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["Filtering_Hash2LimiterDico"] = new SerializableDico<string, string[]>();
			partial["Filtering_Hash2ValueDico"] = new SerializableDico<string, string[]>();

			return partial;
		}



		public static List<string> LoadListsFromPaths(string[] paths)
		{
			List<string> md5s = new List<string>();

			foreach(string path in paths)
			{
				md5s.Add(LoadListFromPath(path));
			}

			var partial = new PartialSpec("CorruptCore");
			partial["Hash2LimiterDico"] = Hash2LimiterDico;
			partial["Hash2ValueDico"] = Hash2ValueDico;
			RTC_CorruptCore.spec.Update(partial);

			return md5s;
		}

		//This is private as it won't update the netcore. The netcore call is in LoadListsFromPaths. Use that
		private static string LoadListFromPath(string path)
		{
			string[] temp = File.ReadAllLines(path);
			bool flipBytes = path.StartsWith("_");

			for (int i = 0; i < temp.Length; i++)
			{
				temp[i] = temp[i].Trim();
				temp[i] = temp[i].ToUpper();
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

		private static string RegisterList(String[] list)
		{
			//Make one giant string to hash
			string _list = String.Empty;
			foreach (String str in list)
				String.Concat(_list, str);

			//Hash it
			MD5 hash = MD5.Create();
			hash.ComputeHash(_list.GetBytes());
			string hashStr = Convert.ToBase64String(hash.Hash);

			if (!Hash2ValueDico.ContainsKey(hashStr))
				Hash2ValueDico[hashStr] = list;
			if (!Hash2LimiterDico.ContainsKey(hashStr))
				Hash2LimiterDico[hashStr] = list;

			return hashStr;
		}

		public static bool LimiterPeekBytes(long startAddress, long endAddress, string domain, string hash, MemoryInterface mi)
		{
			long precision = endAddress - startAddress;
			byte[] values = new byte[precision];

			for (long i = 0; i < precision; i++)
			{
				values[i] = mi.PeekByte(startAddress + i);
			}

			//The compare is done as little endian
			if (mi.BigEndian)
				values = values.FlipBytes();

			if (LimiterContainsValue(values, hash))
				return true;

			return false;
		}

		public static bool LimiterContainsValue(byte[] bytes, string hash)
		{
			if (!Hash2LimiterDico.ContainsKey(hash))
				return false;

			string str = BitConverter.ToString(bytes).Replace("-", "").ToUpper();

			return Hash2LimiterDico[hash].Contains(str);
		}


		public static byte[] GetRandomConstant(string hash)
		{
			if (!Hash2ValueDico.ContainsKey(hash))
			{
				return null;
			}

			return StringToByteArray(Hash2ValueDico[hash][RTC_CorruptCore.RND.Next(Hash2ValueDico[hash].Length)]);
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
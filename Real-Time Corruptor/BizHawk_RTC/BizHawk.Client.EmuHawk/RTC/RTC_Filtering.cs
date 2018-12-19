using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_Filtering
	{

		public static Dictionary<string, String[]> Hash2LimiterDico = new Dictionary<string, string[]>();
		public static Dictionary<string, String[]> Hash2ValueDico = new Dictionary<string, string[]>();

		public static List<string> LoadListsFromPaths(string[] paths)
		{
			List<string> md5s = new List<string>();

			foreach (string path in paths)
			{
				md5s.Add(loadListFromPath(path));
			}
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_UPDATE_FILTERING_DICTIONARIES) { objectValue = new object[] { RTC_Filtering.Hash2LimiterDico, RTC_Filtering.Hash2ValueDico } });
			return md5s;
		}

		//This is private as it won't update the netcore. The netcore call is in LoadListsFromPaths. Use that
		private static string loadListFromPath(string path)
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



		private static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}
		private static string RegisterList(String[] list)
		{
			//Make one giant string to hash
			string concat = String.Empty;
			foreach (String str in list)
				concat = String.Concat(concat, str);

			//Hash it
			MD5 hash = MD5.Create();
			hash.ComputeHash(concat.GetBytes());
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


		public static byte[] GetRandomConstant(string hash, int precision)
		{
			if (!Hash2ValueDico.ContainsKey(hash))
			{
				return null;
			}
			return RTC_Extensions.GetByteArrayFromContentsPadLeft(Hash2ValueDico[hash][RTC_Core.RND.Next(Hash2ValueDico[hash].Length)], precision);
		}

		public static List<String[]> GetAllLimiterListsFromStockpile(Stockpile sks)
		{
			sks.MissingLimiter = false;
			List<String> hashList = new List<string>();
			List<String[]> returnList = new List<String[]>();

			foreach (StashKey sk in sks.StashKeys)
			{
				foreach (BlastUnit bu in sk.BlastLayer.Layer)
				{
					if (!hashList.Contains(bu.LimiterListHash))
						hashList.Add(bu.LimiterListHash);
				}
			}

			foreach (var s in hashList)
			{
				if(Hash2LimiterDico.ContainsKey(s))
					returnList.Add(Hash2LimiterDico[s]);
				else
				{
					DialogResult dr = MessageBox.Show("Couldn't find Limiter List " + s +
						" If you continue saving, any blastunit using this list will ignore the limiter on playback if the list still cannot be found.\nDo you want to continue?", "Couldn't Find Limiter List",
						MessageBoxButtons.YesNo);

					if (dr == DialogResult.No)
						return null;

					sks.MissingLimiter = true;
				}
			}

			return returnList;
		}
	}
}
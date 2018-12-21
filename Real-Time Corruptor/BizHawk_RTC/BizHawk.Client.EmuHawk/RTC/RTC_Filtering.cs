using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_Filtering
	{

		public static Dictionary<string, HashSet<Byte[]>> Hash2LimiterDico = new Dictionary<string, HashSet<Byte[]>> ();
		public static Dictionary<string, List<Byte[]>> Hash2ValueDico = new Dictionary<string, List<Byte[]>>();



		public static List<string> LoadListsFromPaths(string[] paths)
		{
			List<string> md5s = new List<string>();

			foreach (string path in paths)
			{
				md5s.Add(loadListFromPath(path, false));
			}
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_UPDATE_FILTERING_DICTIONARIES) { objectValue = new object[] { RTC_Filtering.Hash2LimiterDico, RTC_Filtering.Hash2ValueDico } });
			return md5s;
		}

		private static string loadListFromPath(string path, bool syncListViaNetcore)
		{
			string[] temp = File.ReadAllLines(path);
			bool flipBytes = path.StartsWith("_");

			List<Byte[]> byteList = new List<byte[]>();
			foreach (string t in temp)
			{
				byte[] bytes = RTC_Extensions.StringToByteArray(t);
				//If it's big endian, flip it
				if (flipBytes)
				{
					bytes.FlipBytes();
				}
				byteList.Add(bytes);
			}

			return RegisterList(byteList.Distinct().ToList(), syncListViaNetcore);
		}

		public static string RegisterList(List<Byte[]> list, bool syncListsViaNetcore)
		{
			//Make one giant string to hash
			string concat = String.Empty;
			foreach (byte[] line in list)
			{
				StringBuilder sb = new StringBuilder();
				foreach (var b in line)
					sb.Append(b.ToString());

				concat = String.Concat(concat, sb.ToString());
			}

			//Hash it. We don't use GetHashCode because we want something consistent
			MD5 hash = MD5.Create();
			hash.ComputeHash(concat.GetBytes());
			string hashStr = Convert.ToBase64String(hash.Hash);

			if (!Hash2ValueDico.ContainsKey(hashStr))
				Hash2ValueDico[hashStr] = list;
			if (!Hash2LimiterDico.ContainsKey(hashStr))
				Hash2LimiterDico[hashStr] = new HashSet<byte[]>(list, new ByteArrayComparer());

			if (syncListsViaNetcore)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_UPDATE_FILTERING_DICTIONARIES) { objectValue = new object[] { RTC_Filtering.Hash2LimiterDico, RTC_Filtering.Hash2ValueDico } });

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
			HashSet<Byte[]> hs = null;
			if (Hash2LimiterDico.TryGetValue(hash, out hs))
			{
				return hs.Contains(bytes);
			}

			return false;

		}


		public static byte[] GetRandomConstant(string hash, int precision)
		{
			if (!Hash2ValueDico.ContainsKey(hash))
			{
				return null;
			}

			int line = RTC_Core.RND.Next(Hash2ValueDico[hash].Count);
			Byte[] t = Hash2ValueDico[hash][line];
			if(t.Length < precision)
				t.PadLeft(precision);
			return t;
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
				if (Hash2LimiterDico.ContainsKey(s))
				{
					List<String> strList = new List<string>();
					foreach (var line in Hash2LimiterDico[s])
					{
						StringBuilder sb = new StringBuilder();
						foreach (var b in line)
							sb.Append(b.ToString());
						strList.Add(sb.ToString());
					}
					returnList.Add(strList.ToArray());
				}

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
	[Serializable]
	public class ByteArrayComparer : IEqualityComparer<byte[]>
	{
		public bool Equals(byte[] a, byte[] b)
		{
			if (a.Length != b.Length) return false;
			for (int i = 0; i < a.Length; i++)
				if (a[i] != b[i]) return false;
			return true;
		}
		public int GetHashCode(byte[] a)
		{
			uint b = 0;
			for (int i = 0; i < a.Length; i++)
				b = ((b << 23) | (b >> 9)) ^ a[i];
			return unchecked((int)b);
		}
	}
}
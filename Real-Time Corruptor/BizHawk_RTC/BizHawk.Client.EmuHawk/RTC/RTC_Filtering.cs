using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Ceras;

namespace RTC
{
	public static class RTC_Filtering
	{

		public static List<string> LoadListsFromPaths(string[] paths)
		{
			List<string> md5s = new List<string>();

			foreach (string path in paths)
			{
				md5s.Add(loadListFromPath(path, false));
			}

			//We do this because we're adding to the lists not replacing them. It's a bit odd but it's needed
			PartialSpec update = new PartialSpec("RTCSpec");
			update[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()]; 
			update[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()]; 
			RTC_Unispec.RTCSpec.Update(update);

			return md5s;
		}

		private static string loadListFromPath(string path, bool syncListViaNetcore)
		{
			string[] temp = File.ReadAllLines(path);
			bool flipBytes = path.StartsWith("_");

			List<Byte[]> byteList = new List<byte[]>();
			for (int i = 0; i < temp.Length; i++)
			{
				string t = temp[i];
				byte[] bytes = null;
				try
				{
					bytes = RTC_Extensions.StringToByteArray(t);
				}
				catch (Exception e)
				{
					MessageBox.Show("Error reading list " + Path.GetFileName(path) + ". Error at line " + (i+1) + ".\nValue: " + t + "\n. Valid format is a list of raw byte values.");
					break;
				}

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

			var limiterDico = (Dictionary<string, RTC_Extensions.HashSetByteArrayComparator>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()];
			var valueDico = (Dictionary<string, List<Byte[]>>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()];

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

			if (!valueDico?.ContainsKey(hashStr) ?? false)
				valueDico[hashStr] = list;
			if (!limiterDico?.ContainsKey(hashStr) ?? false)
				limiterDico[hashStr] = new RTC_Extensions.HashSetByteArrayComparator(list);

			if (syncListsViaNetcore)
			{
				PartialSpec update = new PartialSpec("RTCSpec");
				update[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()] = limiterDico;
				update[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] = valueDico;
				RTC_Unispec.RTCSpec.Update(update);
			}
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_UPDATE_FILTERING_DICTIONARIES) { objectValue = new object[] { RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()], RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()] } });

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
			RTC_Extensions.HashSetByteArrayComparator hs = null;
			var limiterDico = (Dictionary<string, RTC_Extensions.HashSetByteArrayComparator>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()];
			if (limiterDico == null)
				return false;

			if (limiterDico.TryGetValue(hash, out hs))
			{
				return hs.Contains(bytes);
			}

			return false;
		}


		public static byte[] GetRandomConstant(string hash, int precision)
		{
			var valueDico = (Dictionary<string, List<Byte[]>>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()];
			if (valueDico == null)
				return null;

			if (!valueDico.ContainsKey(hash))
			{
				return null;
			}

			int line = RTC_Core.RND.Next(valueDico[hash].Count);
			Byte[] t = valueDico[hash][line];

			//If the list is shorter than the current precision, left pad it
			if (t.Length < precision)
				t.PadLeft(precision);
			//If the list is longer than the current precision, truncate it. Lists are stored little endian so truncate from the right
			else if (t.Length > precision)
			{
				//It'd probably be faster to do this via bitshifting but it's 4am and I want to be able to read this code in the future so...
				
				//Flip the bytes (stored as little endian), truncate, then flip them back
				t = t.FlipBytes();
				Array.Resize(ref t, precision);
				t = t.FlipBytes();
			}

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

			var limiterDico = (Dictionary<string, HashSet<Byte[]>>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2LIMITERDICO.ToString()];
			var valueDico = (Dictionary<string, List<Byte[]>>)RTC_Unispec.RTCSpec[RTCSPEC.FILTERING_HASH2VALUEDICO.ToString()];

			foreach (var s in hashList)
			{
				if (s != null && valueDico.ContainsKey(s))
				{
					List<String> strList = new List<string>();
					foreach (var line in limiterDico[s])
					{
						StringBuilder sb = new StringBuilder();
						foreach (var b in line)
							sb.Append(b.ToString());
						strList.Add(sb.ToString());
					}
					returnList.Add(strList.ToArray());
				}

				else if(s != null)
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
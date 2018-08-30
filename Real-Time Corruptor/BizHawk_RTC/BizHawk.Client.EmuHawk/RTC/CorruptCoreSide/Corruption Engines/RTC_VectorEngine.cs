using RTCV.NetCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_VectorEngine
	{
		public static string LimiterListHash
		{
			get { return (string)RTC_CorruptCore.spec["VectorEngine_LimiterListHash"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "VectorEngine_LimiterListHash", value)); }
		}
		public static string ValueListHash
		{
			get { return (string)RTC_CorruptCore.spec["VectorEngine_ValueListHash"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "VectorEngine_ValueListHash", value)); }
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["VectorEngine_LimiterListHash"] = null;
			partial["VectorEngine_ValueListHash"] = null;

			return partial;
		}

		public static BlastUnit GenerateUnit(string domain, long address)
		{
			if (domain == null)
				return null;

			long safeAddress = address - (address % 4); //32-bit trunk


			MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);

			if (mi == null)
				return null;


			try
			{
				//Enforce the safeaddress at generation
				if (RTC_Filtering.LimiterPeekBytes(safeAddress, safeAddress + 4, domain, LimiterListHash, mi))
					return new BlastUnit(RTC_Filtering.GetRandomConstant(ValueListHash), domain, safeAddress, 4, mi.BigEndian);
				return null;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Vector Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}


	}
}

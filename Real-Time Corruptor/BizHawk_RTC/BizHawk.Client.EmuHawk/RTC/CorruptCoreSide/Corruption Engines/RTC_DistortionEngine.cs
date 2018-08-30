using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_DistortionEngine
	{
		public static int Delay
		{
			get { return (int)RTC_CorruptCore.spec["DistortionEngine_Delay"]; }
			set { RTC_CorruptCore.spec.Update(new PartialSpec("CorruptCore", "DistortionEngine_Delay", value)); }
		}


		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("CorruptCore");

			partial["DistortionEngine_Delay"] = 50;

			return partial;
		}

		public static BlastUnit GenerateUnit(string domain, long address, int precision)
		{
			// Randomly selects a memory operation according to the selected algorithm
			try
			{
				if (domain == null)
					return null;

				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);

				long safeAddress = address - (address % precision);
				return new BlastUnit(StoreType.ONCE, ActionTime.IMMEDIATE, domain, safeAddress, domain, safeAddress, precision, mi.BigEndian, Delay, 1);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Distortion Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

	}
}

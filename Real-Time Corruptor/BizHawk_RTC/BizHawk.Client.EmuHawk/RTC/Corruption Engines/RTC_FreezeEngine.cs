using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_FreezeEngine
	{
		//The freeze engine is very similar to the Hellgenie and shares common functions with it. See RTC_HellgenieEngine.cs for cheat-related methods.

		public static BlastCheat GenerateUnit(string domain, long address)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);
				BizHawk.Client.Common.DisplayType displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

				byte[] value = RTC_Core.CustomPrecision == -1 ? new byte[mi.WordSize] : new byte[RTC_Core.CustomPrecision];

				long safeAddress = address - (address % value.Length);

				for (int i = 0; i < value.Length; i++)
					value[i] = 0;

				return new BlastCheat(domain, safeAddress, displaytype, mi.BigEndian, value, true, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Freeze Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}
	}
}

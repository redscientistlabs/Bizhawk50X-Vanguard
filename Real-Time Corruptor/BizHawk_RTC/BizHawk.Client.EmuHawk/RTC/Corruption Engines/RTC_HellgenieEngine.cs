using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{
		public static int MaxCheats = 50;

		public static long MinValue = 0;
		public static long MaxValue = 255;

		public static BlastCheat GenerateUnit(string domain, long address)
		{
			try
			{
				if (domain == null)
					return null;
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				BizHawk.Client.Common.DisplayType displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

				byte[] value = RTC_Core.CustomPrecision == -1 ? new byte[mdp.WordSize] : new byte[RTC_Core.CustomPrecision];

				long safeAddress = address - (address % value.Length);

				long randomValue = RTC_Core.RND.RandomLong(MinValue, MaxValue);
				value = RTC_Extensions.GetByteArrayValue(value.Length, randomValue, true);

				return new BlastCheat(domain, safeAddress, displaytype, mdp.BigEndian, value, true, false);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Hellgenie Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

		public static void RemoveExcessCheats()
		{
			RTC_Command cmd = new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_REMOVEEXCESSCHEATS);
			RTC_Core.SendCommandToBizhawk(cmd);
		}

		public static void ClearCheats(bool sync = false)
		{
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_SET_HELLGENIE_CLEARALLCHEATS), sync);
		}
	}
}

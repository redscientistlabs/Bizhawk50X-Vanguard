using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{
		public static int MaxCheats = 50;

		public static long MinValue8Bit = 0;
		public static long MaxValue8Bit = 0xFF;

		public static long MinValue16Bit = 0;
		public static long MaxValue16Bit = 0xFFFF;

		public static long MinValue32Bit = 0;
		public static long MaxValue32Bit = 0xFFFFFFFF;

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

				long randomValue = 0;
				switch (value.Length)
				{
					case (1):
						randomValue = RTC_Core.RND.RandomLong(MinValue8Bit, MaxValue8Bit);
						break;
					case (2):
						randomValue = RTC_Core.RND.RandomLong(MinValue16Bit, MaxValue16Bit);
						break;
					case (4):
						randomValue = RTC_Core.RND.RandomLong(MinValue32Bit, MaxValue32Bit);
						break;
				}
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

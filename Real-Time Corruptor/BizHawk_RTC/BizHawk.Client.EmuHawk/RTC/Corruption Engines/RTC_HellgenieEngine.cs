using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_HellgenieEngine
	{
		public static int MaxCheats = 50;

		public static BlastCheat GenerateUnit(string _domain, long _address)
		{
			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(_domain, _address);
				BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

				byte[] _value;
				if (RTC_Core.CustomPrecision == -1)
					_value = new byte[mdp.WordSize];
				else
					_value = new byte[RTC_Core.CustomPrecision];

				long safeAddress = _address - (_address % _value.Length);

				long randomValue = RTC_Core.RND.RandomLong(Convert.ToInt64(RTC_Core.ecForm.nmMinValueHellgenie.Value), Convert.ToInt64(RTC_Core.ecForm.nmMaxValueHellgenie.Value));
				_value = RTC_Extensions.getByteArrayValue(_value.Length, randomValue, true);

				return new BlastCheat(_domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, false);
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

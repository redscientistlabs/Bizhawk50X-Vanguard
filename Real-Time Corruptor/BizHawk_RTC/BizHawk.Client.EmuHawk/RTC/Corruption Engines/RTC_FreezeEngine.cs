using System;
using System.Windows.Forms;

namespace RTC
{
	public static class RTC_FreezeEngine
	{
		//The freeze engine is very similar to the Hellgenie and shares common functions with it. See RTC_HellgenieEngine.cs for cheat-related methods.

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

				for (int i = 0; i < _value.Length; i++)
					_value[i] = 0;
				//_value[i] = mdp.PeekByte(safeAddress + i);

				return new BlastCheat(_domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, true);
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

using System;
using System.Linq;
using System.Windows.Forms;
using BizHawk.Client.Common;

namespace RTC
{
	public class RTC_BlastCheatGenerator
	{
		public BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, BGBlastCheatModes mode)
		{
			BlastLayer bl = new BlastLayer();

			//We need to clear any cheats out first
			RTC_HellgenieEngine.ClearCheats();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, precision, mode, note);
				if (bu != null)
					bl.Layer.Add(bu);
			}

			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision,
			BGBlastCheatModes mode, string note)
		{
			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);

				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];
				bool freeze = false;
				DisplayType _displaytype = DisplayType.Unsigned;

				long safeAddress = address - address % _value.Length;

				//Use >= as Size is 1 indexed whereas address is 0 indexed
				if (safeAddress + _value.Length > mdp.Size)
					return null;

				switch (mode)
				{
					case BGBlastCheatModes.ADD:
						_value = RTC_Extensions.AddValueToByteArray(
							mdp.PeekBytes(safeAddress, safeAddress + _value.Length), param1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.SUBTRACT:
						_value = RTC_Extensions.AddValueToByteArray(
							mdp.PeekBytes(safeAddress, safeAddress + _value.Length), param1 * -1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.RANDOM:
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) RTC_Core.RND.Next(0, 255);
						break;
					case BGBlastCheatModes.RANDOM_RANGE:
						long temp = RTC_Core.RND.RandomLong(param1, param2);
						_value = RTC_Extensions.GetByteArrayValue(precision, temp, true);
						break;
					case BGBlastCheatModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(safeAddress, safeAddress + precision)
							.SequenceEqual(RTC_Extensions.GetByteArrayValue(precision, param1, true)))
							_value = RTC_Extensions.GetByteArrayValue(precision, param2, true);
						else
							return null;
						break;
					case BGBlastCheatModes.SET:
						_value = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGBlastCheatModes.SHIFT_RIGHT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress += param1;
						if (safeAddress >= mdp.Size)
							safeAddress = mdp.Size - _value.Length;
						break;
					case BGBlastCheatModes.SHIFT_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress -= param1;
						if (safeAddress < 0)
							safeAddress = 0;
						break;

					//Bitwise operations
					case BGBlastCheatModes.BITWISE_AND:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_OR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] | _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_XOR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] ^ _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftLeft(_value);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_RIGHT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftRight(_value);
						break;
					case BGBlastCheatModes.BITWISE_ROTATE_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateLeft(_value);
						break;
					case BGBlastCheatModes.BITWISE_ROTATE_RIGHT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateRight(_value);
						break;
					case BGBlastCheatModes.FREEZE:
						freeze = true;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
				}

				return new BlastCheat(domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, freeze, note);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastCheat Generator. \n\n" +
				                "Make sure the domain selected is a valid domain for the core!\n\n" +
								"This is an RTC error, so you should probably send this to the RTC devs.\n" +
				                "If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				                ex);
				throw;
			}
		}
	}
}
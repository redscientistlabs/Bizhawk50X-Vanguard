using System;
using System.Linq;
using System.Windows.Forms;
using BizHawk.Client.Common;

namespace RTC
{
	public class RTC_BlastCheatGenerator
	{
		public static BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, int seed, BGBlastCheatModes mode)
		{
			BlastLayer bl = new BlastLayer();


			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, precision, mode, note, new Random(seed));
				if (bu != null)
					bl.Layer.Add(bu);
			}

			return bl;
		}

		private static BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision,
			BGBlastCheatModes mode, string note, Random rand)
		{
			try
			{
				string targetDomain = RTC_MemoryDomains.GetRealDomain(domain, address);
				long targetAddress = RTC_MemoryDomains.GetRealAddress(domain, address);
				//We use an mdp rather than an mi because of the potential of non-contiguous vmds causing problem with BlastCheat
				MemoryInterface mdp = RTC_MemoryDomains.GetProxy(targetDomain, targetAddress);

				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];
				bool freeze = false;
				DisplayType _displaytype = DisplayType.Unsigned;

				long safeAddress = address - address % _value.Length;
				long safeTargetAddress = targetAddress - targetAddress % _value.Length;

				//Use >= as Size is 1 indexed whereas address is 0 indexed
				if (safeTargetAddress + _value.Length > mdp.Size)
					return null;

				switch (mode)
				{
					case BGBlastCheatModes.ADD:
						_value = RTC_Extensions.AddValueToByteArray(
							mdp.PeekBytes(safeTargetAddress, safeTargetAddress + _value.Length), param1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.SUBTRACT:
						_value = RTC_Extensions.AddValueToByteArray(
							mdp.PeekBytes(safeTargetAddress, safeTargetAddress + _value.Length), param1 * -1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.RANDOM:
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) rand.Next(0, 255);
						break;
					case BGBlastCheatModes.RANDOM_RANGE:
						long temp = rand.RandomLong(param1, param2);
						_value = RTC_Extensions.GetByteArrayValue(precision, temp, true);
						break;
					case BGBlastCheatModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision)
							.SequenceEqual(RTC_Extensions.GetByteArrayValue(precision, param1, true)))
							_value = RTC_Extensions.GetByteArrayValue(precision, param2, true);
						else
							return null;
						break;
					case BGBlastCheatModes.SET:
						_value = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGBlastCheatModes.SHIFT_RIGHT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						safeTargetAddress += param1;
						if (safeTargetAddress >= mdp.Size)
							safeTargetAddress = mdp.Size - _value.Length;
						break;
					case BGBlastCheatModes.SHIFT_LEFT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						safeTargetAddress -= param1;
						if (safeTargetAddress < 0)
							safeTargetAddress = 0;
						break;

					//Bitwise operations
					case BGBlastCheatModes.BITWISE_AND:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_OR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] | _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_XOR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte) (_value[i] ^ _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_LEFT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftLeft(_value);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_RIGHT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftRight(_value);
						break;
					case BGBlastCheatModes.BITWISE_ROTATE_LEFT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateLeft(_value);
						break;
					case BGBlastCheatModes.BITWISE_ROTATE_RIGHT:
						_value = mdp.PeekBytes(safeTargetAddress, safeTargetAddress + precision);
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
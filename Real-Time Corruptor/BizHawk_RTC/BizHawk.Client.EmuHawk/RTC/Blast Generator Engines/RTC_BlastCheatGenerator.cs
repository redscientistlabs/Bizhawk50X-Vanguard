using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace RTC
{

	public class RTC_BlastCheatGenerator
	{
		public BlastLayer GenerateLayer(string Domain, long StepSize, long StartAddress, long EndAddress, long Param1, long Param2, int Precision, BGBlastCheatModes Mode)
		{
			BlastLayer bl = new BlastLayer();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long Address = StartAddress; Address < EndAddress; Address = (Address + StepSize + Precision - 1))
			{
				bl.Layer.Add(GenerateUnit(Domain, Address, Param1, Param2, Precision, Mode));
			}
			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision, BGBlastCheatModes mode)
		{

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(domain, address);
				

				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];
				bool freeze = false;
				BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;

				long safeAddress = address - (address % _value.Length);

				switch (mode)
				{
					case BGBlastCheatModes.ADD:
							_value = RTC_Extensions.addValueToByteArray(mdp.PeekBytes(safeAddress, safeAddress + _value.Length), RTC_Extensions.getDecimalValue(_value, mdp.BigEndian), mdp.BigEndian);
						break;
					case BGBlastCheatModes.SUBTRACT:
						_value = RTC_Extensions.addValueToByteArray(mdp.PeekBytes(safeAddress, safeAddress + _value.Length), RTC_Extensions.getDecimalValue(_value, mdp.BigEndian) * -1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.RANDOM:
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)RTC_Core.RND.Next(0, 255);
						break;
					case BGBlastCheatModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(safeAddress, safeAddress + precision) == RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian))
							_value = RTC_Extensions.getByteArrayValue(precision, param2, mdp.BigEndian);
						break;
					case BGBlastCheatModes.SET:
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;
					case BGBlastCheatModes.SHIFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress += param1;
						break;

					//Bitwise operations
					case BGBlastCheatModes.BITWISE_AND:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_OR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] | _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_XOR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] ^ _temp[i]);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftLeft(_value);
						break;
					case BGBlastCheatModes.BITWISE_SHIFT_RIGHT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
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
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateRight(_value);
						break;
					case BGBlastCheatModes.FREEZE:
						freeze = true;
						break;
				}

				return new BlastCheat(domain, safeAddress, _displaytype, mdp.BigEndian, _value, true, freeze);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastCheat Generator. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

	}
}

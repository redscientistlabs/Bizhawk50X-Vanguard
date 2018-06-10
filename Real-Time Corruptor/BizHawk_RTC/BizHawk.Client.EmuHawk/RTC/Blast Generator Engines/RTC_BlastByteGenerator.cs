using System;
using System.Windows.Forms;

namespace RTC
{
	public class RTC_BlastByteGenerator
	{
		public BlastLayer GenerateLayer(string Domain, long StepSize, long StartAddress, long EndAddress, long Param1, long Param2, int Precision, BGBlastByteModes Mode)
		{
			BlastLayer bl = new BlastLayer();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long Address = StartAddress; Address < EndAddress; Address = (Address + StepSize + Precision - 1))
			{
				bl.Layer.Add(GenerateUnit(Domain, Address, Param1, Param2, Precision, Mode));
			}
			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision, BGBlastByteModes mode)
		{
			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(domain, address);
				BlastByteType Type = BlastByteType.SET;

				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];

				long safeAddress = address - (address % _value.Length);

				//Use >= as Size is 1 indexed whereas address is 0 indexed
				if (safeAddress + _value.Length > mdp.Size)
					return null;

				switch (mode)
				{
					case BGBlastByteModes.ADD:
						Type = BlastByteType.ADD;
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;
					case BGBlastByteModes.SUBTRACT:
						Type = BlastByteType.SUBSTRACT;
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;
					case BGBlastByteModes.RANDOM:
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)RTC_Core.RND.Next(0, 255);
						break;
					case BGBlastByteModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(safeAddress, safeAddress + precision) == RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian))
							_value = RTC_Extensions.getByteArrayValue(precision, param2, mdp.BigEndian);
						break;
					case BGBlastByteModes.SET:
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;
					case BGBlastByteModes.SHIFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress += param1;
						break;

					//Bitwise operations
					case BGBlastByteModes.BITWISE_AND:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_OR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] | _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_XOR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] ^ _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_SHIFT_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftLeft(_value);
						break;
					case BGBlastByteModes.BITWISE_SHIFT_RIGHT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftRight(_value);
						break;
					case BGBlastByteModes.BITWISE_ROTATE_LEFT:
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateLeft(_value);
						break;
					case BGBlastByteModes.BITWISE_ROTATE_RIGHT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						_value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateRight(_value);
						break;
				}

				return new BlastByte(domain, safeAddress, Type, _value, mdp.BigEndian, true);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastByte Generator. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}
	}
}

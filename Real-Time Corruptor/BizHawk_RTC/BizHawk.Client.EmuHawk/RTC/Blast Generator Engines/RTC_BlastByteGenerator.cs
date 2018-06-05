using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace RTC
{

	public class RTC_BlastByteGenerator
	{
		public BlastLayer GenerateLayer(string Domain, long StepSize, long StartAddress, long EndAddress, long Param1, long Param2, int Precision, BGBlastModes Mode)
		{
			BlastLayer bl = new BlastLayer();

			for (long Address = StartAddress; Address < EndAddress; Address = (Address + Precision + StepSize))
			{
				bl.Layer.Add(GenerateUnit(Domain, Address, Param1, Param2, Precision, Mode));
			}
			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision, BGBlastModes mode)
		{

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(domain, address);
				BlastByteType Type = BlastByteType.SET;
				
				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];

				long safeAddress = address - (address % _value.Length);

				switch (mode)
				{							
					case BGBlastModes.ADD:
						Type = BlastByteType.ADD;
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						if (mdp.BigEndian) ;
						_value.FlipBytes();
						break;
					case BGBlastModes.SUBTRACT:
						Type = BlastByteType.SUBSTRACT;
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						if (mdp.BigEndian) ;
						_value.FlipBytes();
						break;
					case BGBlastModes.RANDOM:
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)RTC_Core.RND.Next(0, 255);
						break;
					case BGBlastModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(address, address + precision) == RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian))
							_value = RTC_Extensions.getByteArrayValue(precision, param2, mdp.BigEndian);
						break;
					case BGBlastModes.SET:
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;
					case BGBlastModes.SHIFT:
						safeAddress += param1;
						_value = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						break;

					//Bitwise operations
					case BGBlastModes.BITWISE_AND:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] & _temp[i]);
						break;
					case BGBlastModes.BITWISE_OR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] | _temp[i]);
						break;
					case BGBlastModes.BITWISE_XOR:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] ^ _temp[i]);
						break;
					case BGBlastModes.BITWISE_ROTATE_LEFT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] << _temp[i]);
						break;
					case BGBlastModes.BITWISE_ROTATE_RIGHT:
						_temp = RTC_Extensions.getByteArrayValue(precision, param1, mdp.BigEndian);
						for (int i = 0; i < _value.Length; i++)
							_value[i] = (byte)(_value[i] >> _temp[i]);
						break;
				}

				return new BlastByte(domain, safeAddress, Type, _value, mdp.BigEndian, true);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC Nightmare Engine. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

	}
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public class RTC_BlastByteGenerator
	{
		public BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, BGBlastByteModes mode)
		{
			BlastLayer bl = new BlastLayer();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, precision, mode, note);
				if (bu != null)
					bl.Layer.Add(bu);
			}

			return bl;
		}

		//As the param is a long, it's little endian. We have to account for this whenever the param is going to be used as a value for a byte array
		//If it's an address, we can leave it as is.
		//If it's something such as SET or Replace X with Y, we always flip as we need to go to big endian
		//If it's something like a bitwise operation, we read the values from left to right when pulling them from memory. As such, we also always convert to big endian
		private static BlastUnit GenerateUnit(string domain, long address, long param1, long param2, int precision,
			BGBlastByteModes mode, string note)
		{
			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
				BlastUnitSource type = BlastUnitSource.SET;

				byte[] value = new byte[precision];
				byte[] _temp = new byte[precision];

				long safeAddress = address - address % value.Length;

				//Use >= as Size is 1 indexed whereas address is 0 indexed
				if (safeAddress + value.Length > mdp.Size)
					return null;

				switch (mode)
				{
					case BGBlastByteModes.ADD:
						type = BlastUnitSource.ADD;
						value = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGBlastByteModes.SUBTRACT:
						type = BlastUnitSource.SUBTRACT;
						value = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGBlastByteModes.RANDOM:
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte) RTC_Core.RND.Next(0, 255);
						break;
					case BGBlastByteModes.RANDOM_RANGE:
						long temp = RTC_Core.RND.RandomLong(param1, param2);
						value = RTC_Extensions.GetByteArrayValue(precision, temp, true);
						break;
					case BGBlastByteModes.REPLACE_X_WITH_Y:
						if (mdp.PeekBytes(safeAddress, safeAddress + precision)
							.SequenceEqual(RTC_Extensions.GetByteArrayValue(precision, param1, true)))
							value = RTC_Extensions.GetByteArrayValue(precision, param2, true);
						else
							return null;
						break;
					case BGBlastByteModes.SET:
						value = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGBlastByteModes.SHIFT_RIGHT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress += param1;
						if (safeAddress >= mdp.Size)
							safeAddress = mdp.Size - value.Length;
						break;
					case BGBlastByteModes.SHIFT_LEFT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						safeAddress -= param1;
						if (safeAddress < 0)
							safeAddress = 0;
						break;


					//Bitwise operations
					case BGBlastByteModes.BITWISE_AND:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte) (value[i] & _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_COMPLEMENT:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte) (value[i] & _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_OR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte) (value[i] | _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_XOR:
						_temp = RTC_Extensions.GetByteArrayValue(precision, param1, true);
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte) (value[i] ^ _temp[i]);
						break;
					case BGBlastByteModes.BITWISE_SHIFT_LEFT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftLeft(value);
						break;
					case BGBlastByteModes.BITWISE_SHIFT_RIGHT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.ShiftRight(value);
						break;
					case BGBlastByteModes.BITWISE_ROTATE_LEFT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateLeft(value);
						break;
					case BGBlastByteModes.BITWISE_ROTATE_RIGHT:
						value = mdp.PeekBytes(safeAddress, safeAddress + precision);
						for (int i = 0; i < param1; i++)
							RTC_Extensions.RotateRight(value);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
				}

				return new BlastByte(domain, safeAddress, type, value, mdp.BigEndian, true, note);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastByte Generator. \n\n" +
				                "Make sure the domain selected is a valid domain for the core!\n\n" +
				                "This is an RTC error, so you should probably send this to the RTC devs.\n" +
				                "If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				                ex);
				throw;
			}
		}
	}
}
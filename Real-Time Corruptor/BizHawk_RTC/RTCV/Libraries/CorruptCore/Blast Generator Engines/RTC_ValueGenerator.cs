using System;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace RTCV.CorruptCore
{
	public static class RTC_ValueGenerator
	{
		private static byte[] param1Bytes;
		private static byte[] param2Bytes;

		public static BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, int seed, BGValueModes mode)
		{
			BlastLayer bl = new BlastLayer();

			Random rand = new Random(seed);

			param1Bytes = null;
			param2Bytes = null;

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, precision, mode, note, rand);
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
			BGValueModes mode, string note, Random rand)
		{
			try
			{
				if (!MemoryDomains.MemoryInterfaces.ContainsKey(domain))
					return null;

				MemoryInterface mi = MemoryDomains.GetInterface(domain);

				byte[] value = new byte[precision];
				byte[] _temp = new byte[precision];
				BigInteger tiltValue;

				if (param1Bytes == null)
					param1Bytes = CorruptCore_Extensions.GetByteArrayValue(precision, param1, true);
				if (param2Bytes == null)
					param2Bytes = CorruptCore_Extensions.GetByteArrayValue(precision, param2, true);

				long safeAddress = address - address % value.Length;

				//Use >= as Size is 1 indexed whereas address is 0 indexed
				if (safeAddress + value.Length > mi.Size)
					return null;

				switch (mode)
				{
					case BGValueModes.ADD:
						tiltValue = new BigInteger(param1Bytes);
						break;
					case BGValueModes.SUBTRACT:
						tiltValue = new BigInteger(param1Bytes) * -1;
						break;
					case BGValueModes.RANDOM:
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte)rand.Next(0, 255);
						break;
					case BGValueModes.RANDOM_RANGE:
						long temp = rand.RandomLong(param1, param2);
						value = CorruptCore_Extensions.GetByteArrayValue(precision, temp, true);
						break;
					case BGValueModes.REPLACE_X_WITH_Y:
						if (mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian)
							.SequenceEqual(param1Bytes))
							value = param2Bytes;
						else
							return null;
						break;
					case BGValueModes.SET:
						value = CorruptCore_Extensions.GetByteArrayValue(precision, param1, true);
						break;
					case BGValueModes.SHIFT_RIGHT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						safeAddress += param1;
						if (safeAddress >= mi.Size)
							safeAddress = mi.Size - value.Length;
						break;
					case BGValueModes.SHIFT_LEFT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						safeAddress -= param1;
						if (safeAddress < 0)
							safeAddress = 0;
						break;


					//Bitwise operations
					case BGValueModes.BITWISE_AND:
						_temp = param1Bytes;
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte)(value[i] & _temp[i]);
						break;
					case BGValueModes.BITWISE_COMPLEMENT:
						_temp = param1Bytes;
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte)(value[i] & _temp[i]);
						break;
					case BGValueModes.BITWISE_OR:
						_temp = param1Bytes;
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte)(value[i] | _temp[i]);
						break;
					case BGValueModes.BITWISE_XOR:
						_temp = param1Bytes;
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < value.Length; i++)
							value[i] = (byte)(value[i] ^ _temp[i]);
						break;
					case BGValueModes.BITWISE_SHIFT_LEFT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < param1; i++)
							CorruptCore_Extensions.ShiftLeft(value);
						break;
					case BGValueModes.BITWISE_SHIFT_RIGHT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < param1; i++)
							CorruptCore_Extensions.ShiftRight(value);
						break;
					case BGValueModes.BITWISE_ROTATE_LEFT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < param1; i++)
							CorruptCore_Extensions.RotateLeft(value);
						break;
					case BGValueModes.BITWISE_ROTATE_RIGHT:
						value = mi.PeekBytes(safeAddress, safeAddress + precision, mi.BigEndian);
						for (int i = 0; i < param1; i++)
							CorruptCore_Extensions.RotateRight(value);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
				}

				return new BlastUnit(value, domain, safeAddress, precision, mi.BigEndian, 0, 1, note);
			}
			catch (Exception ex)
			{
				throw new NetCore.CustomException("Something went wrong in the RTC ValueGenerator Generator. " + ex.Message, ex.StackTrace);
			}
		}
	}
}
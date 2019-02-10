using System;
using System.Windows.Forms;

namespace RTCV.CorruptCore
{
	public class RTC_StoreGenerator
	{
		public static BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, int seed, BGStoreModes mode)
		{
			BlastLayer bl = new BlastLayer();

			Random rand = new Random(seed);
			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, stepSize, precision, mode, note, rand);
				if (bu != null)
					bl.Layer.Add(bu);
			}

			return bl;
		}

		private static BlastUnit GenerateUnit(string domain, long address, long param1, long param2, long stepSize,
			int precision, BGStoreModes mode, string note, Random rand)
		{
			try
			{
				if (!MemoryDomains.MemoryInterfaces.ContainsKey(domain))
					return null;
				MemoryInterface mi = MemoryDomains.GetInterface(domain);

				byte[] value = new byte[precision];
				long destAddress = 0;
				StoreType storeType = StoreType.CONTINUOUS;

				long safeAddress = address - address % precision;

				if (safeAddress >= mi.Size)
					return null;

				switch (mode)
				{
					case BGStoreModes.CHAINED:
						long temp = safeAddress + stepSize;
						if (temp <= mi.Size)
							destAddress = temp;
						else
							destAddress = mi.Size - 1;
						break;
					case BGStoreModes.SOURCE_RANDOM:
						destAddress = safeAddress;
						safeAddress = rand.Next(0, Convert.ToInt32(mi.Size - 1));
						break;
					case BGStoreModes.SOURCE_SET:
						destAddress = safeAddress;
						safeAddress = param1;
						break;
					case BGStoreModes.DEST_RANDOM:
						destAddress = rand.Next(0, Convert.ToInt32(mi.Size - 1));
						break;
					case BGStoreModes.FREEZE:
						destAddress = address;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
				}

				if (destAddress >= mi.Size)
					return null;

				return new BlastUnit(storeType, StoreTime.PREEXECUTE, domain, destAddress, domain, safeAddress, precision, mi.BigEndian, 0, 0, note);
			}
			catch (Exception ex)
			{
				throw new NetCore.CustomException("Something went wrong in the RTC StoreGenerator Generator. " + ex.Message, ex.StackTrace);
			}
		}
	}
}
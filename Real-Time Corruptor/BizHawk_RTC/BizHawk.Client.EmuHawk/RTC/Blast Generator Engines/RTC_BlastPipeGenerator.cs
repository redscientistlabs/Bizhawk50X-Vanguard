using System;
using System.Windows.Forms;

namespace RTC
{
	public class RTC_BlastPipeGenerator
	{
		public BlastLayer GenerateLayer(string note, string domain, long stepSize, long startAddress, long endAddress,
			long param1, long param2, int precision, BGBlastPipeModes mode)
		{
			BlastLayer bl = new BlastLayer();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long address = startAddress; address < endAddress; address = address + stepSize + precision - 1)
			{
				BlastUnit bu = GenerateUnit(domain, address, param1, param2, stepSize, precision, mode, note);
				if (bu != null)
					bl.Layer.Add(bu);
			}

			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, long stepSize,
			int precision, BGBlastPipeModes mode, string note)
		{
			try
			{

				MemoryInterface mi = RTC_MemoryDomains.GetInterface(domain);

				byte[] value = new byte[precision];
				long destAddress = 0;


				long safeAddress = address - address % value.Length;

				if (safeAddress >= mi.Size)
					return null;

				switch (mode)
				{
					case BGBlastPipeModes.CHAINED:
						long temp = safeAddress + stepSize;
						if (temp <= mi.Size)
							destAddress = temp;
						else
							destAddress = mi.Size - 1;
						break;
					case BGBlastPipeModes.SOURCE_RANDOM:
						destAddress = safeAddress;
						safeAddress = RTC_Core.RND.Next(0, Convert.ToInt32(mi.Size - 1));
						break;
					case BGBlastPipeModes.SOURCE_SET:
						destAddress = safeAddress;
						safeAddress = param1;
						break;
					case BGBlastPipeModes.DEST_RANDOM:
						destAddress = RTC_Core.RND.Next(0, Convert.ToInt32(mi.Size - 1));
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
				}

				if (destAddress >= mi.Size)
					return null;

				return new BlastPipe(domain, safeAddress, domain, destAddress, 0, precision, mi.BigEndian, true, note);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastPipe Generator. \n\n" +
				                "Make sure the domain selected is a valid domain for the core!\n\n" +
								"This is an RTC error, so you should probably send this to the RTC devs.\n" +
				                "If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
				                ex);
				throw;
			}
		}
	}
}
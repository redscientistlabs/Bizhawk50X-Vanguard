using BizHawk.Client.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace RTC
{

	public class RTC_BlastPipeGenerator
	{
		public BlastLayer GenerateLayer(string Domain, long StepSize, long StartAddress, long EndAddress, long Param1, long Param2, int Precision, BGBlastPipeModes Mode)
		{
			BlastLayer bl = new BlastLayer();

			//We subtract 1 at the end as precision is 1,2,4, and we need to go 0,1,3
			for (long Address = StartAddress; Address < EndAddress; Address = (Address + StepSize + Precision - 1))
			{
				bl.Layer.Add(GenerateUnit(Domain, Address, Param1, Param2, StepSize, Precision, Mode));
			}
			return bl;
		}

		private BlastUnit GenerateUnit(string domain, long address, long param1, long param2, long stepSize, int precision, BGBlastPipeModes mode)
		{

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(domain, address);

				byte[] _value = new byte[precision];
				byte[] _temp = new byte[precision];
				long destAddress = 0;

				long safeAddress = address - (address % _value.Length);

				switch (mode)
				{
					case BGBlastPipeModes.CHAINED:
						long temp = safeAddress + stepSize;
						if (temp <= mdp.Size)
						{
							destAddress = temp;
						}
						else
							destAddress = mdp.Size;
						break;
					case BGBlastPipeModes.RANDOM:
						destAddress = safeAddress;
						safeAddress = RTC_Core.RND.Next(0, Convert.ToInt32(mdp.Size-1));
						break;
					case BGBlastPipeModes.SETSOURCE:
						destAddress = safeAddress;
						safeAddress = param1;
						break;
				}

				return new BlastPipe(domain, safeAddress, domain, destAddress, 0, precision, mdp.BigEndian, true);

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in the RTC BlastPipe Generator. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs.\n" +
					"If you know the steps to reproduce this error it would be greatly appreciated.\n\n" +
								ex.ToString());
				return null;
			}
		}

	}
}

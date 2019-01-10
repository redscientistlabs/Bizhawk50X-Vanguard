using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using RTC;

namespace BizHawk.Client.EmuHawk.RTC
{
	public static class RTC_EmuCoreObjects
	{
		public class BizhawkMemoryDomain : IMemoryDomain
		{
			public MemoryDomain MD;
			public string Name => MD.Name;
			private long size => MD.Size;
			public long Size
			{
				get
				{
					//Bizhawk always displays 8MB of ram even if only 4 are in use.
					if (Global.Emulator is N64 && !(Global.Emulator as N64).UsingExpansionSlot && Name == "RDRAM")
						return size / 2;
					return size;
				}
			}
			public int WordSize => MD.WordSize;
			public bool BigEndian => MD.EndianType == MemoryDomain.Endian.Big;

			public BizhawkMemoryDomain(MemoryDomain md)
			{
				MD = md;
			}
			public byte PeekByte(long addr)
			{
				return MD.PeekByte(addr);
			}
			public void PokeByte(long addr, byte val)
			{
				MD.PokeByte(addr, val);
			}

			public override string ToString()
			{
				return MD.Name;
			}
		}

		public class MemoryDomainRTCInterface
		{
			[RequiredService]
			public IMemoryDomains MemoryDomains { get; set; }

			[RequiredService]
			private IEmulator Emulator { get; set; }
		}

	}
}

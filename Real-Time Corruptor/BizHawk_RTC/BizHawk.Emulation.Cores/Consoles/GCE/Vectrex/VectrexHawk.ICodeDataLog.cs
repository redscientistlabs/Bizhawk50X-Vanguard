using System;
using System.IO;
using System.Collections.Generic;

using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Components.M6502;

namespace BizHawk.Emulation.Cores.Consoles.Vectrex
{
	public sealed partial class VectrexHawk : ICodeDataLogger
	{
		public void SetCDL(ICodeDataLog cdl)
		{
			CDL = cdl;
		}

		public void NewCDL(ICodeDataLog cdl)
		{
			cdl["RAM"] = new byte[MemoryDomains["RAM"].Size];

			if (MemoryDomains.Has("Save RAM"))
			{
				cdl["Save RAM"] = new byte[MemoryDomains["Save RAM"].Size];
			}

			if (MemoryDomains.Has("Battery RAM"))
			{
				cdl["Battery RAM"] = new byte[MemoryDomains["Battery RAM"].Size];
			}

			if (MemoryDomains.Has("Battery RAM"))
			{
				cdl["Battery RAM"] = new byte[MemoryDomains["Battery RAM"].Size];
			}

			cdl.SubType = "VIC20";
			cdl.SubVer = 0;
		}

		[FeatureNotImplemented]
		public void DisassembleCDL(Stream s, ICodeDataLog cdl)
		{

		}

		private enum CDLog_AddrType
		{
			None,
			ROM,
			MainRAM,
			SaveRAM,
		}

		[Flags]
		private enum CDLog_Flags
		{
			ExecFirst = 0x01,
			ExecOperand = 0x02,
			Data = 0x04
		};

		private struct CDLog_MapResults
		{
			public CDLog_AddrType Type;
			public int Address;
		}

		private delegate CDLog_MapResults MapMemoryDelegate(ushort addr, bool write);
		private MapMemoryDelegate MapMemory;
		private ICodeDataLog CDL;

		private void RunCDL(ushort address, CDLog_Flags flags)
		{
			if (MapMemory != null)
			{
				CDLog_MapResults results = MapMemory(address, false);
				switch (results.Type)
				{
					case CDLog_AddrType.None: break;
					case CDLog_AddrType.MainRAM: CDL["Main RAM"][results.Address] |= (byte)flags; break;
					case CDLog_AddrType.SaveRAM: CDL["Save RAM"][results.Address] |= (byte)flags; break;
				}
			}
		}

		/// <summary>
		/// A wrapper for FetchMemory which inserts CDL logic
		/// </summary>
		private byte FetchMemory_CDL(ushort address)
		{
			RunCDL(address, CDLog_Flags.ExecFirst);
			return PeekMemory(address);
		}

		/// <summary>
		/// A wrapper for ReadMemory which inserts CDL logic
		/// </summary>
		private byte ReadMemory_CDL(ushort address)
		{
			RunCDL(address, CDLog_Flags.Data);
			return ReadMemory(address);
		}
	}
}
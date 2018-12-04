using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using NLua;

using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Common.IEmulatorExtensions;

namespace BizHawk.Client.EmuHawk
{
	[Description("A library for interfacing with RTC extensions")]
	public sealed class RTCLuaLibrary : LuaMemoryBase
	{
		public RTCLuaLibrary(Lua lua)
			: base(lua) { }

		public RTCLuaLibrary(Lua lua, Action<string> logOutputCallback)
			: base(lua, logOutputCallback) { }

		public override string Name => "rtc";

		protected override MemoryDomain Domain
		{
			get
			{
				if (MemoryDomainCore != null)
				{
					return MemoryDomainCore.MainMemory;
				}
				else
				{
					var error = $"Error: {Emulator.Attributes().CoreName} does not implement memory domains";
					Log(error);
					throw new NotImplementedException(error);
				}
			}
		}


		private uint RDRAMBase = 0x80000000;
		private uint RDRAMSize = 0x800000;
		private uint RDRAMEnd = 0x80800000;

		[LuaMethodExample("getTexturePointers(\"TextureTable\");")]
		[LuaMethod("getTexturePointers", "Does your stuff in c#")]
		public void getTexturePointers(LuaTable textureTable, LuaTable rangeTable)
		{
			foreach (var count in rangeTable.Keys)
			{
				var start = Convert.ToInt32((double)((LuaTable)rangeTable[count])[1]);
				var end =  Convert.ToInt32((double)((LuaTable) rangeTable[count])[2] - 4);
				for (int address = start; address < end; address += 4)
				{
					uint? value = dereferencePointer(address);
					if (value != null)
					{
						if (textureTable[value] != null)
						{
							((LuaTable)(textureTable[value]))["references"] = Convert.ToInt32(((LuaTable)(textureTable[value]))["references"]) + 1;
							((LuaTable)(((LuaTable)(textureTable[value]))["referenceAddresses"]))[address] = address;
						}
					}
				}
			}
		}

		bool isRDRAM(uint value)
		{
			return (value < RDRAMSize);
		}
		uint? dereferencePointer(int address)
		{
			if (address < (RDRAMSize - 4))
			{
				uint value = ReadUnsignedBig(address, 4);
				if(value >= RDRAMBase && value < RDRAMEnd)
					return value - RDRAMBase;
			}
			return null;
		}
	}

}

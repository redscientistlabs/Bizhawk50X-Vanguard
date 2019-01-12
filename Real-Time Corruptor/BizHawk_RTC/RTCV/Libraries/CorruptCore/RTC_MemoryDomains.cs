using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Ceras;
using CorruptCore;
using RTCV.NetCore;

namespace RTCV.CorruptCore
{
	public static class RTC_MemoryDomains
	{

		public static Dictionary<string, MemoryInterface> MemoryInterfaces
		{
			get => (Dictionary<string, MemoryInterface>)RTC_Corruptcore.CorruptCoreSpec["MEMORYINTERFACES"];
			set => RTC_Corruptcore.CorruptCoreSpec.Update("MEMORYINTERFACES", value);
		}
		public static Dictionary<string, MemoryInterface> VmdPool
		{
			get => (Dictionary<string, MemoryInterface>)RTC_Corruptcore.CorruptCoreSpec["VMDPOOL"];
			set => RTC_Corruptcore.CorruptCoreSpec.Update("VMDPOOL", value);
		}

		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("RTCSpec");

			partial["MEMORYINTERFACES"] = new Dictionary<string, MemoryInterface>();
			partial["VMDPOOL"] = new Dictionary<string, MemoryInterface>();

			return partial;
		}


		public static void RefreshDomains()
		{
			//var returns = LocalNetCoreRouter.QueryRoute<MemoryInterface[]>("VANGUARD", "REMOTE_DOMAIN_GETDOMAINS");

			var temp = new Dictionary<string, MemoryInterface>();
			foreach (MemoryInterface mi in (MemoryInterface[])RTC_Corruptcore.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES.ToString()])
				temp.Add(mi.ToString(), mi);
			MemoryInterfaces = temp;
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED);

		}


		public static void Clear()
		{
			MemoryInterfaces.Clear();
		}

		public static MemoryDomainProxy GetProxy(string domain, long address)
		{
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

			if (MemoryInterfaces.ContainsKey(domain))
			{
				MemoryInterface mi = MemoryInterfaces[domain];
				return (MemoryDomainProxy)mi;
			}
			else if (VmdPool.ContainsKey(domain))
			{
				MemoryInterface mi = VmdPool[domain];
				if (mi is VirtualMemoryDomain vmd)
					return GetProxy(vmd.GetRealDomain(address), vmd.GetRealAddress(address));
			}
			return null;
		}

		public static MemoryInterface GetInterface(string _domain)
		{
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

			if (MemoryInterfaces.ContainsKey(_domain))
				return MemoryInterfaces[_domain];

			if (VmdPool.ContainsKey(_domain))
				return VmdPool[_domain];

			return null;
		}


		public static long GetRealAddress(string domain, long address)
		{
			if (domain.Contains("[V]"))
			{
				MemoryInterface mi = VmdPool[domain];
				VirtualMemoryDomain vmd = ((VirtualMemoryDomain)mi);
				return vmd.GetRealAddress(address);
			}
			else
				return address;
		}

		public static string GetRealDomain(string domain, long address)
		{
			if (domain.Contains("[V]"))
			{
				MemoryInterface mi = VmdPool[domain];
				VirtualMemoryDomain vmd = ((VirtualMemoryDomain)mi);
				return vmd.GetRealDomain(address);
			}
			else
				return domain;
		}

		public static void GenerateVmdFromStashkey(StashKey sk)
		{
			VmdPrototype proto = new VmdPrototype(sk.BlastLayer);
			AddVMD(proto);

			//Todo
			//S.GET<RTC_VmdPool_Form>().RefreshVMDs();
		}

		public static void AddVMD(VmdPrototype proto) => AddVMD(proto.Generate());

		public static void AddVMD(VirtualMemoryDomain VMD)
		{
			RTC_MemoryDomains.VmdPool[VMD.ToString()] = VMD;


			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED, true);
		}

		public static void RemoveVMD(VirtualMemoryDomain VMD) => RemoveVMD(VMD.ToString());

		public static void RemoveVMD(string vmdName)
		{
			if (RTC_MemoryDomains.VmdPool.ContainsKey(vmdName))
				RTC_MemoryDomains.VmdPool.Remove(vmdName);

			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_EVENT_DOMAINSUPDATED, true);
		}

		public static void RenameVMD(VirtualMemoryDomain VMD) => RenameVMD(VMD.ToString());

		public static void RenameVMD(string vmdName)
		{
			throw new NotImplementedException("Move input to UI");
			/*
			if (!RTC_MemoryDomains.VmdPool.ContainsKey(vmdName))
				return;

			string name = "";
			string value = "";
			if (UI_Extensions.GetInputBox("BlastLayer to VMD", "Enter the new VMD name:", ref value) == DialogResult.OK)
			{
				name = value.Trim();
			}
			else
			{
				return;
			}

			if (string.IsNullOrWhiteSpace(name))
				name = RTC_Corruptcore.GetRandomKey();

			VirtualMemoryDomain VMD = (VirtualMemoryDomain)RTC_MemoryDomains.VmdPool[vmdName];

			RemoveVMD(VMD);
			VMD.Name = name;
			VMD.Proto.VmdName = name;
			AddVMD(VMD);*/
		}

		public static void GenerateActiveTableDump(string domain, string key)
		{
			MemoryInterface mi = MemoryInterfaces[domain];

			byte[] dump = mi.GetDump();

			File.WriteAllBytes(RTC_Corruptcore.rtcDir + Path.DirectorySeparatorChar + "MEMORYDUMPS" + Path.DirectorySeparatorChar + key + ".dmp", dump.ToArray());
		}

		public static byte[] GetDomainData(string domain)
		{
			MemoryInterface mi = domain.Contains("[V]") ? VmdPool[domain] : MemoryInterfaces[domain];
			return mi.GetDump();
		}

		public static string[] GetBlacklistedDomains()
		{
			// Returns the list of Domains that can't be rewinded and/or are just not good to use

			List<string> domainBlacklist = new List<string>();

			string systemName = (string)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.REMOTE_DOMAIN_SYSTEM, true);

			switch (systemName)
			{
				case "NES":     //Nintendo Entertainment system

					domainBlacklist.Add("System Bus");
					domainBlacklist.Add("PRG ROM");
					domainBlacklist.Add("PALRAM"); //Color Memory (Useless and disgusting)
					domainBlacklist.Add("CHR VROM"); //Cartridge
					domainBlacklist.Add("Battery RAM"); //Cartridge Save Data
					domainBlacklist.Add("FDS Side"); //ROM data for the FDS. Sadly uncorruptable.
					break;

				case "GB":      //Gameboy
				case "GBC":     //Gameboy Color
					domainBlacklist.Add("ROM"); //Cartridge
					domainBlacklist.Add("System Bus");
					domainBlacklist.Add("OBP"); //SGB dummy domain doesn't do anything in sameboy
					domainBlacklist.Add("BGP");  //SGB dummy domain doesn't do anything in sameboy
					domainBlacklist.Add("BOOTROM"); //Sameboy SGB Bootrom
					break;

				case "SNES":    //Super Nintendo

					domainBlacklist.Add("CARTROM"); //Cartridge
					domainBlacklist.Add("APURAM"); //SPC700 memory
					domainBlacklist.Add("CGRAM"); //Color Memory (Useless and disgusting)
					domainBlacklist.Add("System Bus"); // maxvalue is not representative of chip (goes ridiculously high)
					domainBlacklist.Add("SGB CARTROM"); // Supergameboy cartridge

					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("SGB CARTROM"))
					{
						domainBlacklist.Add("VRAM");
						domainBlacklist.Add("WRAM");
						domainBlacklist.Add("CARTROM");
					}

					break;

				case "N64":     //Nintendo 64
					domainBlacklist.Add("System Bus");
					domainBlacklist.Add("PI Register");
					domainBlacklist.Add("EEPROM");
					domainBlacklist.Add("ROM");
					domainBlacklist.Add("SI Register");
					domainBlacklist.Add("VI Register");
					domainBlacklist.Add("RI Register");
					domainBlacklist.Add("AI Register");
					break;

				case "PCE":     //PC Engine / Turbo Grafx
				case "SGX":     //Super Grafx
					domainBlacklist.Add("ROM");
					domainBlacklist.Add("System Bus"); //BAD THINGS HAPPEN WITH THIS DOMAIN
					domainBlacklist.Add("System Bus (21 bit)");
					break;

				case "GBA":     //Gameboy Advance
					domainBlacklist.Add("OAM");
					domainBlacklist.Add("BIOS");
					domainBlacklist.Add("PALRAM");
					domainBlacklist.Add("ROM");
					domainBlacklist.Add("System Bus");
					break;

				case "SMS":     //Sega Master System
					domainBlacklist.Add("System Bus"); // the game cartridge appears to be on the system bus
					domainBlacklist.Add("ROM");
					break;

				case "GG":      //Sega GameGear
					domainBlacklist.Add("System Bus"); // the game cartridge appears to be on the system bus
					domainBlacklist.Add("ROM");
					break;

				case "SG":      //Sega SG-1000
					domainBlacklist.Add("System Bus");
					domainBlacklist.Add("ROM");
					break;

				case "32X_INTERIM":
				case "GEN":     //Sega Genesis and CD
					domainBlacklist.Add("MD CART");
					domainBlacklist.Add("CRAM"); //Color Ram
					domainBlacklist.Add("VSRAM"); //Vertical scroll ram. Do you like glitched scrolling? Have a dedicated domain...
					domainBlacklist.Add("SRAM"); //Save Ram
					domainBlacklist.Add("BOOT ROM"); //Genesis Boot Rom
					domainBlacklist.Add("32X FB"); //32X Sprinkles
					domainBlacklist.Add("CD BOOT ROM"); //Sega CD boot rom
					domainBlacklist.Add("S68K BUS");
					domainBlacklist.Add("M68K BUS");
					break;

				case "PSX":     //Sony Playstation 1
					domainBlacklist.Add("BiosROM");
					domainBlacklist.Add("PIOMem");
					break;

				case "A26":     //Atari 2600
					domainBlacklist.Add("System Bus");
					break;

				case "A78":     //Atari 7800
					domainBlacklist.Add("System Bus");
					break;

				case "LYNX":    //Atari Lynx
					domainBlacklist.Add("Save RAM");
					domainBlacklist.Add("Cart B");
					domainBlacklist.Add("Cart A");
					break;

				case "WSWAN":   //Wonderswan
					domainBlacklist.Add("ROM");
					break;

				case "Coleco":  //Colecovision
					domainBlacklist.Add("System Bus");
					break;

				case "VB":      //Virtualboy
					domainBlacklist.Add("ROM");
					break;

				case "SAT":     //Sega Saturn
					domainBlacklist.Add("Backup RAM");
					domainBlacklist.Add("Boot Rom");
					domainBlacklist.Add("Backup Cart");
					domainBlacklist.Add("VDP1 Framebuffer"); //Sprinkles
					domainBlacklist.Add("VDP2 CRam"); //VDP 2 color ram (pallettes)
					domainBlacklist.Add("Sound Ram"); //90% chance of killing the audio
					break;

				case "INTV": //Intellivision
					domainBlacklist.Add("Graphics ROM");
					domainBlacklist.Add("System ROM");
					domainBlacklist.Add("Executive Rom"); //??????
					break;

				case "APPLEII": //Apple II
					domainBlacklist.Add("System Bus");
					break;

				case "C64":     //Commodore 64
					domainBlacklist.Add("System Bus");
					domainBlacklist.Add("1541 Bus");
					break;

				case "PCECD":   //PC-Engine CD / Turbo Grafx CD
				case "TI83":    //Ti-83 Calculator
				case "SGB":     //Super Gameboy
				case "DGB":
					break;

					//TODO: Add more domains for cores like gamegear, atari, turbo graphx
			}
			return domainBlacklist.ToArray();
		}

		private static bool CheckNesHeader(string filename)
		{
			byte[] buffer = new byte[4];
			using (Stream fs = File.OpenRead(filename))
			{
				fs.Read(buffer, 0, buffer.Length);
			}
			if (!buffer.SequenceEqual(Encoding.ASCII.GetBytes("NES\x1A")))
				return false;
			return true;
		}

		public static RomParts GetRomParts(string thisSystem, string romFilename)
		{
			RomParts rp = new RomParts();

			switch (thisSystem.ToUpper())
			{
				case "NES":     //Nintendo Entertainment System

					//There's no easy way to discern NES from FDS so just check for the domain name
					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("PRG ROM"))
						rp.PrimaryDomain = "PRG ROM";
					else
					{
						rp.Error = "Unfortunately, Bizhawk doesn't support editing the ROM (FDS Side) domain of FDS games. Maybe in a future version...";
						break;
					}

					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("CHR VROM"))
						rp.SecondDomain = "CHR VROM";
					//Skip the first 16 bytes if there's an iNES header
					if (CheckNesHeader(romFilename))
						rp.SkipBytes = 16;
					break;

				case "SNES":    //Super Nintendo
					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("SGB CARTROM")) //BSNES SGB Mode
						rp.PrimaryDomain = "SGB CARTROM";
					else
					{
						rp.PrimaryDomain = "CARTROM";

						long filesize = new System.IO.FileInfo(romFilename).Length;

						if (filesize % 1024 != 0)
							rp.SkipBytes = 512;
					}

					break;

				case "LYNX":    //Atari Lynx
					rp.PrimaryDomain = "Cart A";
					rp.SkipBytes = 64;
					break;

				case "N64":     //Nintendo 64
				case "GB":      //Gameboy
				case "GBC":     //Gameboy Color
				case "SMS":     //Sega Master System
				case "GBA":     //Game Boy Advance
				case "PCE":     //PC Engine
				case "GG":      //Game Gear
				case "SG":      //SG-1000
				case "SGX":     //PC Engine SGX
				case "WSWAN":   //Wonderswan
				case "VB":      //Virtualboy
				case "NGP":     //Neo Geo Pocket
					rp.PrimaryDomain = "ROM";
					break;

				case "GEN":     // Sega Genesis
					if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("MD CART"))  //If it's regular Genesis or 32X
					{
						rp.PrimaryDomain = "MD CART";

						if (romFilename.ToUpper().Contains(".SMD"))
							rp.SkipBytes = 512;
					}
					else
					{    //If it's in Sega CD mode
						rp.Error = "Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...";
					}
					break;

				case "PCFX":    //PCFX
				case "PCECD":   //PC Engine CD
				case "SAT":     //Sega Saturn
				case "PSX":     //Playstation
					rp.Error = "Unfortunately, Bizhawk doesn't support editing the ISOs while it is running. Maybe in a future version...";
					break;
				default:
					rp.Error = "The RTC devs haven't added support for this system. Go yell at them to make it work.";
					break;
			}

			return rp;
		}


	}

	public class RomParts
	{
		public string Error { get; set; }
		public string PrimaryDomain { get; set; }
		public string SecondDomain { get; set; }
		public int SkipBytes { get; set; }
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public abstract class MemoryInterface
	{
		public abstract long Size { get; set; }
		public int WordSize { get; set; }
		public string Name { get; set; }
		public bool BigEndian { get; set; }

		public abstract byte[] GetDump();

		public abstract byte[] PeekBytes(long startAddress, long endAddress, bool bigEndian);

		public abstract byte PeekByte(long address);

		public abstract void PokeByte(long address, byte value);

		public MemoryInterface()
		{

		}
	}

	[XmlInclude(typeof(BlastLayer))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class VmdPrototype
	{
		public string VmdName { get; set; }
		public string GenDomain { get; set; }
		public bool BigEndian { get; set; }
		public int WordSize { get; set; }
		public long PointerSpacer { get; set; }

		public long Padding { get; set; }

		public List<long> AddSingles { get; set; } = new List<long>();
		public List<long> RemoveSingles { get; set; } = new List<long>();

		public List<long[]> AddRanges { get; set; } = new List<long[]>();
		public List<long[]> RemoveRanges { get; set; } = new List<long[]>();

		public BlastLayer SuppliedBlastLayer = null;

		public VmdPrototype()
		{

		}

		public VmdPrototype(BlastLayer bl)
		{
			VmdName = RTC_Corruptcore.GetRandomKey();
			GenDomain = "Hybrid";

			BlastUnit bu = bl.Layer[0];
			MemoryInterface mi = RTC_MemoryDomains.GetInterface(bu.Domain);
			BigEndian = mi.BigEndian;
			WordSize = mi.WordSize;
			SuppliedBlastLayer = bl;
		}

		public VirtualMemoryDomain Generate()
		{
			VirtualMemoryDomain VMD = new VirtualMemoryDomain
			{
				Proto = this,
				Name = VmdName,
				BigEndian = BigEndian,
				WordSize = WordSize
			};


			if (SuppliedBlastLayer != null)
			{
				VMD.AddFromBlastLayer(SuppliedBlastLayer);
				return VMD;
			}

			int addressCount = 0;
			for (int i = 0; i < Padding; i++)
			{
				VMD.PointerDomains.Add(GenDomain);
				VMD.PointerAddresses.Add(i);
			}

			foreach (long[] range in AddRanges)
			{
				long start = range[0];
				long end = range[1];

				for (long i = start; i < end; i++)
				{
					if (!IsAddressInRanges(i, RemoveSingles, RemoveRanges))
						if (PointerSpacer == 1 || addressCount % PointerSpacer == 0)
						{
							//VMD.MemoryPointers.Add(new Tuple<string, long>(Domain, i));
							VMD.PointerDomains.Add(GenDomain);
							VMD.PointerAddresses.Add(i);
						}
					addressCount++;
				}
			}

			foreach (long single in AddSingles)
			{
				//VMD.MemoryPointers.Add(new Tuple<string, long>(Domain, single));
				VMD.PointerDomains.Add(GenDomain);
				VMD.PointerAddresses.Add(single);
				addressCount++;
			}

			return VMD;
		}

		public bool IsAddressInRanges(long address, List<long> singles, List<long[]> ranges)
		{
			if (singles.Contains(address))
				return true;

			foreach (long[] range in ranges)
			{
				long start = range[0];
				long end = range[1];

				if (address >= start && address < end)
					return true;
			}

			return false;
		}
	}

	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public class VirtualMemoryDomain : MemoryInterface
	{
		public List<string> PointerDomains { get; set; }  =  new List<string>();
		public List<long> PointerAddresses { get; set; } = new List<long>();
		public VmdPrototype Proto { get; set; }


		public VirtualMemoryDomain()
		{

		}

		public override long Size { get => PointerDomains.Count;
			set { } }

		public void AddFromBlastLayer(BlastLayer bl)
		{
			if (bl == null)
				return;

			foreach (BlastUnit bu in bl.Layer)
			{
				PointerDomains.Add(bu.Domain);
				PointerAddresses.Add(bu.Address);
			}
		}

		public string GetRealDomain(long address)
		{
			if (address < 0 || address > PointerDomains.Count)
				return null;

			return PointerDomains[(int)address];
		}

		public long GetRealAddress(long address)
		{
			if (address < 0 || address > PointerAddresses.Count || address < Proto.Padding)
				return 0;
			return PointerAddresses[(int)address];
		}

		public byte[] ToData()
		{
			VirtualMemoryDomain VMD = this;

			using (MemoryStream serialized = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(serialized, VMD);

				using (MemoryStream input = new MemoryStream(serialized.ToArray()))
				using (MemoryStream output = new MemoryStream())
				{
					using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
					{
						input.CopyTo(zip);
					}

					return output.ToArray();
				}
			}
		}

		public static VirtualMemoryDomain FromData(byte[] data)
		{
			using (MemoryStream input = new MemoryStream(data))
			using (MemoryStream output = new MemoryStream())
			{
				using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
				{
					zip.CopyTo(output);
				}

				var binaryFormatter = new BinaryFormatter();

				using (MemoryStream serialized = new MemoryStream(output.ToArray()))
				{
					VirtualMemoryDomain VMD = (VirtualMemoryDomain)binaryFormatter.Deserialize(serialized);
					return VMD;
				}
			}
		}

		public override string ToString()
		{
			//Virtual Memory Domains always start with [V]
			return "[V]" + Name;
		}

		public override byte[] GetDump()
		{
			return PeekBytes(0, Size);
		}

		public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
		{
			//endAddress is exclusive
			List<byte> data = new List<byte>();
			for (long i = startAddress; i < endAddress; i++)
				data.Add(PeekByte(i));

			if (raw || BigEndian)
				return data.ToArray();
			else
				return data.ToArray().FlipBytes();
		}

		public override byte PeekByte(long address)
		{
			if (address < this.Proto.Padding)
				return (byte)0;
			string targetDomain = GetRealDomain(address);
			long targetAddress = GetRealAddress(address);

			MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(targetDomain, targetAddress);

			return mdp?.PeekByte(targetAddress) ?? 0;
		}

		public override void PokeByte(long address, byte value)
		{
			if (address < this.Proto.Padding)
				return;

			string targetDomain = GetRealDomain(address);
			long targetAddress = GetRealAddress(address);

			MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(targetDomain, targetAddress);

			mdp?.PokeByte(targetAddress, value);
		}
	}


	[Serializable]
	[Ceras.MemberConfig(TargetMember.All)]
	public sealed class MemoryDomainProxy : MemoryInterface
	{
		[NonSerialized , Ceras.Ignore]
		public IMemoryDomain MD = null;
		public override long Size { get; set; }
		public MemoryDomainProxy(IMemoryDomain _md)
		{
			MD = _md;
			Size = MD.Size;

			Name = MD.ToString();


			WordSize = MD.WordSize;
			Name = MD.ToString();
			BigEndian = MD.BigEndian;
		}
		public MemoryDomainProxy()
		{
		}
		public override string ToString()
		{
			return Name;
		}

		public override byte[] GetDump()
		{
			return PeekBytes(0, Size);
		}

		public override byte[] PeekBytes(long startAddress, long endAddress, bool raw = true)
		{
			//endAddress is exclusive
			List<byte> data = new List<byte>();
			for (long i = startAddress; i < endAddress; i++)
				data.Add(PeekByte(i));

			if(raw || BigEndian)
				return data.ToArray();
			else
				return data.ToArray().FlipBytes();
		}

		public override byte PeekByte(long address)
		{
		//	if (MD == null)
			//	return (byte)LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.PEEKBYTE, new object[] { Name, address }, true);
		//	else
				return MD.PeekByte(address);
		}

		public override void PokeByte(long address, byte value)
		{
		//	if (MD == null)
		//		LocalNetCoreRouter.Route(NetcoreCommands.VANGUARD, NetcoreCommands.POKEBYTE, new object[] { Name, address }, true);
		//	else
				MD.PokeByte(address, value);
		}
	}

	public interface IMemoryDomain
	{
		string Name { get; }
		long Size { get; }
		int WordSize { get; }
		bool BigEndian { get; }

		byte PeekByte(long addr);
		void PokeByte(long addr, byte val);
	}

}

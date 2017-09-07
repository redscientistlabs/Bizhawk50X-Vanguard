using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Client.EmuHawk;
using System.Windows.Forms;
using System.Threading;
using BizHawk.Emulation.Cores.Nintendo.N64;
using System.Xml.Serialization;

namespace RTC
{

    public static class RTC_MemoryDomains
    {
		
		public static volatile MemoryDomainRTCInterface MDRI = new MemoryDomainRTCInterface();
		public static volatile Dictionary<string,MemoryInterface> MemoryInterfaces = new Dictionary<string, MemoryInterface>();
        public static volatile Dictionary<string, MemoryInterface> VmdPool = new Dictionary<string, MemoryInterface>();

        public static string _domain = null;
		public static bool BigEndian { get; set; }
		public static int DataSize { get; set; }
		public static WatchSize WatchSize
		{
			get
			{
				return (WatchSize)DataSize;
			}
		}

		public static string[] SelectedDomains = new string[] { };
		public static string[] lastSelectedDomains = new string[] { };


        public static string[] getSelectedDomains()
        {
            return SelectedDomains;
        }

        public static void UpdateSelectedDomains(string[] _domains, bool sync = false)
        {

			SelectedDomains = _domains;
			lastSelectedDomains = _domains;

			if (RTC_Core.isStandalone)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS) { objectValue = SelectedDomains }, sync);

			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide} -> Selected {_domains.Count().ToString()} domains \n{string.Join(" | ", _domains)}");

		}

		public static void ClearSelectedDomains()
		{
			lastSelectedDomains = SelectedDomains;

			SelectedDomains = new string[] { };

			if (RTC_Core.isStandalone)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SETSELECTEDDOMAINS) { objectValue = SelectedDomains });

			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide} -> Cleared selected domains");
		}

        public static string[] GetBlacklistedDomains()
        {
            // Returns the list of Domains that can't be rewinded.

            List<string> DomainBlacklist = new List<string>();

			string SystemName;
			
			if(RTC_Core.isStandalone)
				SystemName = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_SYSTEM), true);
			else
				SystemName = Global.Game.System.ToString().ToUpper();

			switch (SystemName)
            {

                case "NES":     //Nintendo Entertainment system

                    DomainBlacklist.Add("System Bus");
                    DomainBlacklist.Add("PRG ROM");
					DomainBlacklist.Add("PALRAM"); //Color Memory (Useless and disgusting)
					DomainBlacklist.Add("CHR VROM"); //Cartridge
                    DomainBlacklist.Add("Battery RAM"); //Cartridge Save Data
                    break;


                case "GB":      //Gameboy
                case "GBC":     //Gameboy Color
                    DomainBlacklist.Add("ROM"); //Cartridge
                    DomainBlacklist.Add("System Bus");
                    break;

                case "SNES":    //Super Nintendo

                    DomainBlacklist.Add("CARTROM"); //Cartridge
                    DomainBlacklist.Add("CARTRAM"); //Cartridge Save data
                    DomainBlacklist.Add("APURAM"); //SPC700 memory
                    DomainBlacklist.Add("CGRAM"); //Color Memory (Useless and disgusting)
					DomainBlacklist.Add("System Bus"); // maxvalue is not representative of chip (goes ridiculously high)
					break;

                case "N64":     //Nintendo 64
                    DomainBlacklist.Add("System Bus");
                    DomainBlacklist.Add("PI Register");
                    DomainBlacklist.Add("EEPROM");
                    DomainBlacklist.Add("ROM");
                    DomainBlacklist.Add("SI Register");
					DomainBlacklist.Add("VI Register");
					DomainBlacklist.Add("RI Register");
					DomainBlacklist.Add("AI Register");
					break;

                case "PCE":     //PC Engine / Turbo Grafx
                    DomainBlacklist.Add("ROM");
                    break;


                case "GBA":     //Gameboy Advance
                    DomainBlacklist.Add("OAM");
                    DomainBlacklist.Add("BIOS");
                    DomainBlacklist.Add("PALRAM");
                    DomainBlacklist.Add("ROM");
                    break;

                case "SG":      //Sega SG-1000
                    //everything okay
                    break;

                case "SMS":     //Sega Master System
                    DomainBlacklist.Add("System Bus"); // the game cartridge appears to be on the system bus
                    break;

                case "GG":      //Sega GameGear
                    //everything okay
                    break;

                case "GEN":     //Sega Genesis and CD
                    DomainBlacklist.Add("MD CART");
                    break;


                case "PSX":     //Sony Playstation 1
                    DomainBlacklist.Add("MainRAM");
                    DomainBlacklist.Add("BiosROM");
                    DomainBlacklist.Add("PIOMem");
                    break;

                case "A26":     //Atari 2600
                    break;

                case "A78":     //Atari 7800
                    DomainBlacklist.Add("BIOS ROM");
                    DomainBlacklist.Add("HSC ROM");
                    break;

                case "LYNX":    //Atari Lynx
                    DomainBlacklist.Add("Save RAM");
                    DomainBlacklist.Add("Cart B");
                    DomainBlacklist.Add("Cart A");
                    break;


                case "INTV":    //Intellivision

                case "PCECD":   //related to PC-Engine / Turbo Grafx
                case "SGX":     //related to PC-Engine / Turbo Grafx
                case "TI83":    //Ti-83 Calculator
                case "WSWAN":   //Wonderswan
                case "C64":     //Commodore 64
                case "Coleco":  //Colecovision
                case "SGB":     //Super Gameboy
                case "SAT":     //Sega Saturn
                case "DGB": 
                    MessageBox.Show("WARNING: The selected system appears to be supported by Bizhawk Emulator.\n " +
                    "However, no corruption Template is available yet for this system.\n " +
                    "You'll have to manually select the Memory Domains to corrupt.");
                    break;

                    //TODO: Add more domains for cores like gamegear, atari, turbo graphx
            }


			return DomainBlacklist.ToArray();

        }


        public static void RefreshDomains(bool clearSelected = true)
        {

			if (Global.Emulator is NullEmulator)
				return;

			object[] returns;

			if (!RTC_Core.isStandalone)
				returns = (object[])getInterfaces();
			else
				returns = (object[])RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_GETDOMAINS), true);

			if (returns == null)
			{
				Console.WriteLine($"{RTC_Core.RemoteRTC.expectedSide} -> RefreshDomains() FAILED");
				return;
			}

			if(clearSelected)
				ClearSelectedDomains();

			if (RTC_Core.isStandalone)
			{
				MemoryInterfaces.Clear();

				foreach (MemoryInterface mi in (MemoryInterface[])returns[0])
					MemoryInterfaces.Add(mi.ToString(), mi);

				_domain = (string)returns[1];
				DataSize = MemoryInterfaces[_domain].WordSize;
				BigEndian = MemoryInterfaces[_domain].BigEndian;

			}

		}

		public static object getInterfaces()
		{
			Console.WriteLine($"{RTC_Core.RemoteRTC?.expectedSide.ToString()} -> getInterfaces()");

			MemoryInterfaces.Clear();

			ServiceInjector.UpdateServices(Global.Emulator.ServiceProvider, MDRI);

			foreach (MemoryDomain _domain in MDRI.MemoryDomains)
				MemoryInterfaces.Add(_domain.ToString(), new MemoryDomainProxy(_domain));

			_domain = MDRI.MemoryDomains.MainMemory.ToString();
			DataSize = (MemoryInterfaces[_domain] as MemoryDomainProxy).md.WordSize;
			BigEndian = (MemoryInterfaces[_domain] as MemoryDomainProxy).md.EndianType == MemoryDomain.Endian.Big;

			//RefreshDomains();

            /*
            if(VmdPool.Count > 0)
                foreach (string VmdKey in VmdPool.Keys)
                    MemoryInterfaces.Add(VmdKey, VmdPool[VmdKey]);
            */

            return new object[] { MemoryInterfaces.Values.ToArray(), _domain };
		}

        public static void Clear()
        {

            MemoryInterfaces.Clear();

			lastSelectedDomains = SelectedDomains;
			SelectedDomains = new string[] { };

			if(RTC_Core.coreForm != null)
				RTC_Core.coreForm.lbMemoryDomains.Items.Clear();

        }

        public static MemoryDomainProxy getProxy(string _domain, long _address)
        {
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

            if (MemoryInterfaces.ContainsKey(_domain))
            {
                MemoryInterface mi = MemoryInterfaces[_domain];
                return (MemoryDomainProxy)mi;
            }
            else if(VmdPool.ContainsKey(_domain))
            {
                MemoryInterface mi = VmdPool[_domain];
                var vmd = (mi as VirtualMemoryDomain);
                return getProxy(vmd.getRealDomain(_address), vmd.getRealAddress(_address));
            }
            else
                return null;
        }

        public static MemoryInterface getInterface(string _domain)
        {
			if (MemoryInterfaces.Count == 0)
				RefreshDomains();

            if (MemoryInterfaces.ContainsKey(_domain))
				return MemoryInterfaces[_domain];

            if (VmdPool.ContainsKey(_domain))
                return VmdPool[_domain];

			return null;
        }

		public static void UnFreezeAddress(long address)
		{
			if (address >= 0)
			{
				// TODO: can't unfreeze address 0??
				Global.CheatList.RemoveRange(
					Global.CheatList.Where(x => x.Contains(address)).ToList());
			}

		}

		public static void FreezeAddress(long address, string freezename = "")
		{
			if (address >= 0)
			{
				var watch = Watch.GenerateWatch(
					getProxy(_domain, address).md,
					address,
					WatchSize,
					BizHawk.Client.Common.DisplayType.Hex,
					BigEndian,
					//RTC_HIJACK : change string.empty to freezename
					freezename);

				Global.CheatList.Add(new Cheat(
					watch,
					watch.Value));
			}
		}

	}

    [Serializable()]
    public abstract class MemoryInterface
    {
        public abstract long Size { get; set; }
        public int WordSize { get; set; }
        public string name { get; set; }
        public bool BigEndian { get; set; }

        public abstract byte PeekByte(long address);
        public abstract void PokeByte(long address, byte value);
    }

    [XmlInclude(typeof(MemoryPointer))]
    [Serializable()]
    public class VirtualMemoryDomain : MemoryInterface
    {
        public List<MemoryPointer> MemoryPointers = new List<MemoryPointer>();

        public override long Size { get { return MemoryPointers.Count; } set { } }

        public void AddFromBlastLayer(BlastLayer bl)
        {
            if (bl == null)
                return;

            foreach(BlastUnit bu in bl.Layer)
            {
                MemoryPointers.Add(new MemoryPointer(bu.Domain, bu.Address));
            }
        }

        public string getRealDomain(long address)
        {
            if (address < 0 || address >= MemoryPointers.Count)
                return null;

            return MemoryPointers[(int)address].Domain;
        }

        public long getRealAddress(long address)
        {
            if (address < 0 || address >= MemoryPointers.Count)
                return 0;

            return MemoryPointers[(int)address].Address;
        }

        public override string ToString()
        {
            return "[V]" + name;
            //Virtual Memory Domains always start with [V]
        }

        public override byte PeekByte(long address)
        {
            string targetDomain = getRealDomain(address);
            long targetAddress = getRealAddress(address);

            var mdp = RTC_MemoryDomains.getProxy(targetDomain, targetAddress);

            if(mdp == null)
                return 0;

            return mdp.PeekByte(targetAddress);
        }

        public override void PokeByte(long address, byte value)
        {
            string targetDomain = getRealDomain(address);
            long targetAddress = getRealAddress(address);

            var mdp = RTC_MemoryDomains.getProxy(targetDomain, targetAddress);

            if (mdp == null)
                return;

            mdp.PokeByte(targetAddress, value);
        }
    }

    [Serializable()]
    [XmlType("MP")]
    public class MemoryPointer
    {
        [XmlElement("D")]
        public string DomainData { get; set; }
        [XmlIgnore]
        public string Domain { get { return (IsEnabled ? DomainData : ""); }}
        [XmlElement("A")]
        public long AddressData { get; set; }
        [XmlIgnore]
        public long Address { get { return (IsEnabled ? AddressData : 0); } }
        [XmlElement("E")]
        public bool IsEnabled { get; set; }

        public MemoryPointer(string _domain, long _address)
        {
            DomainData = _domain;
            AddressData = _address;
            IsEnabled = true;
        }

        public MemoryPointer()
        {
        }

        public override string ToString()
        {
            return $"[{(IsEnabled ? "x" : " ")}] {DomainData}({AddressData})";

            //Gives: [x] ROM(123)
        }

        

    }

    [Serializable()]
	public class MemoryDomainProxy : MemoryInterface
    {

		[NonSerialized]
		public MemoryDomain md = null;

		//public long Size;
		//public int WordSize;
		//public string name;
		//public bool BigEndian;

        public override long Size { get; set; }

		public MemoryDomainProxy(MemoryDomain _md)
		{
			md = _md;
			Size = md.Size;

            name = md.ToString();

            if (Global.Emulator is N64 && !(Global.Emulator as N64).UsingExpansionSlot && name == "RDRAM")
				Size = Size / 2;

			WordSize = md.WordSize;
			name = md.ToString();
			BigEndian = _md.EndianType == MemoryDomain.Endian.Big;
		}

		public override string ToString()
		{
			return name;
		}

		public void Detach()
		{
			md = null;
		}

		public void Reattach()
		{
			md = RTC_MemoryDomains.MDRI.MemoryDomains.FirstOrDefault(it => it.ToString() == name);
			Size = md.Size;
			WordSize = md.WordSize;
			name = md.ToString();
			BigEndian = md.EndianType == MemoryDomain.Endian.Big;
		}

		public override byte PeekByte(long address)
		{
			if (md == null)
				return (byte)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_PEEKBYTE) { objectValue = new object[] { name, address } }, true);
			else
				return md.PeekByte(address);
		}

		public override void PokeByte(long address, byte value)
		{
			if (md == null)
				RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_POKEBYTE) { objectValue = new object[] { name, address, value } });
			else
				md.PokeByte(address, value);
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

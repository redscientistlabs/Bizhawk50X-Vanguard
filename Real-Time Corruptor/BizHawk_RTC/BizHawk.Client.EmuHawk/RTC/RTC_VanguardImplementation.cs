using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using RTCV;
using RTCV.CorruptCore;

namespace RTC
{
	public static class VanguardImplementation
	{
		public static RTCV.Vanguard.VanguardConnector connector = null;

		public static void StartClient()
		{
			ConsoleEx.WriteLine("Starting Vanguard Client");
			Thread.Sleep(500); //When starting in Multiple Startup Project, the first try will be uncessful since
			//the server takes a bit more time to start then the client.

			var spec = new NetCoreReceiver();
			spec.MessageReceived += OnMessageReceived;

			connector = new RTCV.Vanguard.VanguardConnector(spec);
		}

		public static void RestartClient()
		{
			connector.Kill();
			connector = null;
			StartClient();
		}

		private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			// This is where you implement interaction.
			// Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

			var message = e.message;
			var simpleMessage = message as NetCoreSimpleMessage;
			var advancedMessage = message as NetCoreAdvancedMessage;

			ConsoleEx.WriteLine(message.Type);
			switch (message.Type) //Handle received messages here
			{
				case "POKEBYTE":
				{
					break;
				}
				case "SAVESAVESTATE":
				{
					e.setReturnValue(RTC_EmuCore.SaveSavestate_NET(advancedMessage.objectValue as string));
					break;
				}

				case "LOADSAVESTATE":
				{
					var cmd = advancedMessage.objectValue as object[];
					var path = cmd[0] as string;
					var location = (StashKeySavestateLocation)cmd[1];
					e.setReturnValue(RTC_EmuCore.LoadSavestate_NET(path, location));
					break;
				}

				case "REMOTE_PUSHEMUSPEC":
					RTC_EmuCore.EmuSpec = new FullSpec((PartialSpec)advancedMessage.objectValue);
					e.setReturnValue(true);
					break;


				case "REMOTE_PUSHEMUSPECUPDATE":
					RTC_EmuCore.EmuSpec?.Update((PartialSpec)advancedMessage.objectValue, false);
					break;

				case "REMOTE_LOADROM":
				{
					var fileName = advancedMessage.objectValue as String;
					RTC_EmuCore.LoadRom_NET(fileName);
				}
					break;

				case "REMOTE_DOMAIN_GETDOMAINS":
					e.setReturnValue(BizhawkMemoryDomain.GetInterfaces());
					break;

				case "REMOTE_DOMAIN_SYSTEM":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_CURRENTLYLOADEDSYSTEMNAME().ToUpper());
					break;

				case "REMOTE_DOMAIN_SYSTEMPREFIX":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_SAVESTATEPREFIX());
					break;

				case "REMOTE_KEY_GETSYSTEMNAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_FILESYSTEMCORENAME());
					break;

				case "REMOTE_KEY_GETSYSTEMCORE":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_SYSTEMCORENAME((string)advancedMessage.objectValue));
					break;

				case "REMOTE_KEY_GETGAMENAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_FILESYSTEMGAMENAME());
					break;

				case "REMOTE_KEY_GETSETSYNCSETTINGS":
					if (advancedMessage.objectValue != null)
						RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS = (string)advancedMessage.objectValue;
					else
						e.setReturnValue(RTC_Hooks.BIZHAWK_GETSET_SYNCSETTINGS);
					break;

				case "REMOTE_KEY_GETOPENROMFILENAME":
					e.setReturnValue(RTC_Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM());
					break;

				case "REMOTE_KEY_SETSYSTEMCORE":
				{
					var cmd = advancedMessage.objectValue as object[];
					var systemName = (string)cmd[0];
					var systemCore = (string)cmd[1];
					RTC_Hooks.BIZHAWK_SET_SYSTEMCORE(systemName, systemCore);
				}
					break;

				case "BIZHAWK_OPEN_HEXEDITOR_ADDRESS":
				{
					var temp = advancedMessage.objectValue as object[];
					string domain = (string)temp[0];
					long address = (long)temp[1];

					MemoryDomainProxy mdp = RTC_MemoryDomains.GetProxy(domain, address);
					long realAddress = RTC_MemoryDomains.GetRealAddress(domain, address);

					RTC_Hooks.BIZHAWK_OPEN_HEXEDITOR_ADDRESS(mdp, realAddress);

					break;
				}

				case "REMOTE_EVENT_LOADGAMEDONE_NEWGAME":


					RTC_Corruptcore.AutoCorrupt = false;
					//					S.GET<RTC_MemoryDomains_Form>().RefreshDomains();
					//				S.GET<RTC_MemoryDomains_Form>().SetMemoryDomainsAllButSelectedDomains(RTC_MemoryDomains.GetBlacklistedDomains());
					break;
				case "REMOTE_EVENT_LOADGAMEDONE_SAMEGAME":
					//RTC_StockpileManager.isCorruptionApplied = false;
					//				S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
					break;

				case "REMOTE_EVENT_BIZHAWK_MAINFORM_CLOSE":
					RTC_Hooks.BIZHAWK_MAINFORM_CLOSE();
					break;

				case "REMOTE_EVENT_SAVEBIZHAWKCONFIG":
					RTC_Hooks.BIZHAWK_MAINFORM_SAVECONFIG();
					break;

				case "REMOTE_EVENT_BIZHAWKSTARTED":
					//				if (RTC_StockpileManager.BackupedState == null)
					//					S.GET<RTC_Core_Form>().AutoCorrupt = false;


					//Todo
					//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command("REMOTE_PUSHVMDS) { objectValue = RTC_MemoryDomains.VmdPool.Values.Select(it => (it as VirtualMemoryDomain).Proto).ToArray() }, true, true);

					Thread.Sleep(100);

					//		if (RTC_StockpileManager.BackupedState != null)
					//			S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(RTC_StockpileManager.BackupedState.SelectedDomains.ToArray());

					//		if (S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
					//			RTC_GameProtection.Start();

					break;

			}
		}



		public class MemoryDomainRTCInterface
		{
			[RequiredService]
			public IMemoryDomains MemoryDomains { get; set; }

			[RequiredService]
			private IEmulator Emulator { get; set; }
		}


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


			public static volatile MemoryDomainRTCInterface MDRI = new MemoryDomainRTCInterface();

			public static void RefreshDomains(bool clearSelected = true)
			{
				if (Global.Emulator is NullEmulator)
					return;

				RTC_EmuCore.EmuSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES.ToString(), (object[])GetInterfaces());
				LocalNetCoreRouter.Route("CORRUPTCORE", "REMOTE_EVENT_DOMAINSUPDATED");

			}

			public static object GetInterfaces()
			{
				Console.WriteLine($" getInterfaces()");

				Dictionary<String, MemoryInterface> interfaces = new Dictionary<string, MemoryInterface>();

				if (Global.Emulator?.ServiceProvider == null)
					return null; ;

				ServiceInjector.UpdateServices(Global.Emulator.ServiceProvider, MDRI);

				foreach (MemoryDomain _domain in MDRI.MemoryDomains)
					if (!interfaces.ContainsKey(_domain.ToString()))
						interfaces.Add(_domain.ToString(), new MemoryDomainProxy(new BizhawkMemoryDomain(_domain)));


				return interfaces.Values.ToArray();
			}
		}
	}
}

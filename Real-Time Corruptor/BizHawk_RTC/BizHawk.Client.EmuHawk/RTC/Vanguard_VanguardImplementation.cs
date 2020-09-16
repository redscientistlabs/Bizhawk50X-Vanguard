using RTCV.NetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using BizHawk.Emulation.Cores.Nintendo.N64;
using RTCV;
using RTCV.CorruptCore;

namespace RTCV.BizhawkVanguard
{
	public static class VanguardImplementation
	{
		public static RTCV.Vanguard.VanguardConnector connector = null;


		public static void StartClient()
		{
			try
			{
				ConsoleEx.WriteLine("Starting Vanguard Client");
				Thread.Sleep(500); //When starting in Multiple Startup Project, the first try will be uncessful since
				//the server takes a bit more time to start then the client.

				var spec = new NetCoreReceiver();
				spec.Attached = VanguardCore.attached;
				spec.MessageReceived += OnMessageReceived;

				connector = new RTCV.Vanguard.VanguardConnector(spec);

			}
			catch (Exception ex)
			{
				if (VanguardCore.ShowErrorDialog(ex, true) == DialogResult.Abort)
					throw new RTCV.NetCore.AbortEverythingException();
			}
		}

		public static void RestartClient()
		{
			connector?.Kill();
			connector = null;
			StartClient();
		}

		private static void OnMessageReceived(object sender, NetCoreEventArgs e)
		{
			try
			{
				// This is where you implement interaction.
				// Warning: Any error thrown in here will be caught by NetCore and handled by being displayed in the console.

				var message = e.message;
				var simpleMessage = message as NetCoreSimpleMessage;
				var advancedMessage = message as NetCoreAdvancedMessage;

				ConsoleEx.WriteLine(message.Type);
				switch (message.Type) //Handle received messages here
				{

					case RTCV.NetCore.Commands.Remote.AllSpecSent:
					{
						if (VanguardCore.FirstConnect)
						{
							SyncObjectSingleton.FormExecute(() =>
							{
								VanguardCore.LoadDefaultAndShowBizhawkForm();
							});
							VanguardCore.FirstConnect = false;
						}
					}
						break;
					case RTCV.NetCore.Commands.Basic.SaveSavestate:
						SyncObjectSingleton.FormExecute(() =>
						{
							e.setReturnValue(VanguardCore.SaveSavestate_NET(advancedMessage.objectValue as string));
						});
						break;

					case RTCV.NetCore.Commands.Basic.LoadSavestate:
						{
							var cmd = advancedMessage.objectValue as object[];
							var path = cmd[0] as string;
							var location = (StashKeySavestateLocation)cmd[1];
							SyncObjectSingleton.FormExecute(() =>
							{
								e.setReturnValue(VanguardCore.LoadSavestate_NET(path, location));
							});
							break;
						}

					case RTCV.NetCore.Commands.Remote.LoadROM:
						{
							var fileName = advancedMessage.objectValue as String;
							SyncObjectSingleton.FormExecute(() =>
							{
								VanguardCore.LoadRom_NET(fileName);
							});

						}
						break;
					case RTCV.NetCore.Commands.Remote.CloseGame:
						{
							SyncObjectSingleton.FormExecute(() =>
							{
								Hooks.CLOSE_GAME(true);
							});
						}
						break;

					case RTCV.NetCore.Commands.Remote.DomainGetDomains:
						SyncObjectSingleton.FormExecute(() =>
						{
							e.setReturnValue(Hooks.GetInterfaces());
						});
						break;

					case RTCV.NetCore.Commands.Remote.KeySetSyncSettings:
						SyncObjectSingleton.FormExecute(() =>
						{
							Hooks.BIZHAWK_GETSET_SYNCSETTINGS = (string)advancedMessage.objectValue;
						});
						break;

					case RTCV.NetCore.Commands.Remote.KeySetSystemCore:
						{
							var cmd = advancedMessage.objectValue as object[];
							var systemName = (string)cmd[0];
							var systemCore = (string)cmd[1];
							SyncObjectSingleton.FormExecute(() =>
							{
								Hooks.BIZHAWK_SET_SYSTEMCORE(systemName, systemCore);
							});
						}
						break;

					case RTCV.NetCore.Commands.Remote.OpenHexEditor:
						SyncObjectSingleton.FormExecute(() => Hooks.BIZHAWK_OPEN_HEXEDITOR());
						break;

					case RTCV.NetCore.Commands.Emulator.GetRealtimeAPI:
						e.setReturnValue(VanguardCore.RTE_API);
						break;

					case RTCV.NetCore.Commands.Emulator.GetScreenshot:
						e.setReturnValue(Hooks.BIZHAWK_GET_SCREENSHOT());
						break;

					case RTCV.NetCore.Commands.Emulator.OpenHexEditorAddress:
						{
							var temp = advancedMessage.objectValue as object[];
							string domain = (string)temp[0];
							long address = (long)temp[1];

							MemoryDomainProxy mdp = MemoryDomains.GetProxy(domain, address);
							long realAddress = MemoryDomains.GetRealAddress(domain, address);

							SyncObjectSingleton.FormExecute(() =>
							{
								Hooks.BIZHAWK_OPEN_HEXEDITOR_ADDRESS(mdp, realAddress);
							});

							break;
						}
					case RTCV.NetCore.Commands.Remote.EventEmuManiformClose:
						SyncObjectSingleton.FormExecute(() =>
						{
							Hooks.BIZHAWK_MAINFORM_CLOSE();
						});
						break;
					case RTCV.NetCore.Commands.Remote.RenderStart:
						SyncObjectSingleton.FormExecute(() =>
						{
							BizhawkRender.StartRender_NET();
						});
						break;

					case RTCV.NetCore.Commands.Remote.RenderStop:
						SyncObjectSingleton.FormExecute(() =>
						{
							BizhawkRender.StopRender_NET();
						});
						break;

					case RTCV.NetCore.Commands.Remote.EventEmuStarted:
						//if (RTC_StockpileManager.BackupedState == null)
						//S.GET<RTC_Core_Form>().AutoCorrupt = false;


						//Todo
						//RTC_NetcoreImplementation.SendCommandToBizhawk(new RTC_Command("REMOTE_PUSHVMDS) { objectValue = MemoryDomains.VmdPool.Values.Select(it => (it as VirtualMemoryDomain).Proto).ToArray() }, true, true);

						//Thread.Sleep(100);

						//		if (RTC_StockpileManager.BackupedState != null)
						//			S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected(RTC_StockpileManager.BackupedState.SelectedDomains.ToArray());

						//		if (S.GET<RTC_Core_Form>().cbUseGameProtection.Checked)
						//			RTC_GameProtection.Start();

						break;
					case RTCV.NetCore.Commands.Remote.IsNormalAdvance:
						e.setReturnValue(Hooks.isNormalAdvance);
						break;

					case RTCV.NetCore.Commands.Remote.EventCloseEmulator:
						Environment.Exit(-1);
						break;
				}
			}
			catch (Exception ex)
			{
				VanguardCore.ShowErrorDialog(ex,true);
			}
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
				//Ensure we stay within range
				return addr < MD.Size ? MD.PeekByte(addr) : (byte)0;
			}

			public byte[] PeekBytes(long addr, int range)
			{
				//Ensure we stay within range
				if (addr >= (MD.Size + range))
					return new byte[range];

				byte[] returnArray = new byte[range];

				for (int i = 0; i < range; i++)
					returnArray[i] = MD.PeekByte(addr + i);

				return returnArray;
			}

			public void PokeByte(long addr, byte val)
			{
				if(addr <= MD.Size - 1)
					MD.PokeByte(addr, val);
			}

			public override string ToString()
			{
				return MD.Name;
			}

		}
	}
}

using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using RTCV.CorruptCore;
using RTCV.NetCore;
using RTCV.Vanguard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace Vanguard
{
	public static class VanguardCore
	{
		public static string[] args;

		internal static DialogResult ShowErrorDialog(Exception exception, bool canContinue = false)
		{
			return new RTCV.NetCore.CloudDebug(exception, canContinue).Start();
		}


		/// <summary>
		/// Global exceptions in Non User Interfarce(other thread) antipicated error
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = (Exception)e.ExceptionObject;
			Form error = new RTCV.NetCore.CloudDebug(ex);
			var result = error.ShowDialog();

		}

		/// <summary>
		/// Global exceptions in User Interfarce antipicated error
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
		{
			Exception ex = e.Exception;
			Form error = new RTCV.NetCore.CloudDebug(ex);
			var result = error.ShowDialog();

			Form loaderObject = (sender as Form);

			if (result == DialogResult.Abort)
			{
				if (loaderObject != null)
					RTCV.NetCore.SyncObjectSingleton.SyncObjectExecute(loaderObject, (o, ea) =>
					{
						loaderObject.Close();
					});
			}
		}

		public static bool attached = false;

		public static string System
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEM];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEM, value);
		}
		public static string GameName
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.GAMENAME];
			set => AllSpec.VanguardSpec.Update(VSPEC.GAMENAME, value);
		}
		public static string SystemPrefix
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEMPREFIX];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEMPREFIX, value);
		}
		public static string SystemCore
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYSTEMCORE];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYSTEMCORE, value);
		}
		public static string SyncSettings
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.SYNCSETTINGS];
			set => AllSpec.VanguardSpec.Update(VSPEC.SYNCSETTINGS, value);
		}
		public static string OpenRomFilename
		{
			get => (string)AllSpec.VanguardSpec[VSPEC.OPENROMFILENAME];
			set => AllSpec.VanguardSpec.Update(VSPEC.OPENROMFILENAME, value);
		}
		public static int LastLoaderRom
		{
			get => (int)AllSpec.VanguardSpec[VSPEC.CORE_LASTLOADERROM];
			set => AllSpec.VanguardSpec.Update(VSPEC.CORE_LASTLOADERROM, value);
		}
		public static string[] BlacklistedDomains
		{
			get => (string[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS];
			set => AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS, value);
		}
		public static MemoryDomainProxy[] MemoryInterfacees
		{
			get => (MemoryDomainProxy[])AllSpec.VanguardSpec[VSPEC.MEMORYDOMAINS_INTERFACES];
			set => AllSpec.VanguardSpec.Update(VSPEC.MEMORYDOMAINS_INTERFACES, value);
		}

		public static string emuDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		public static string logPath = Path.Combine(emuDir, "EMU_LOG.txt");


		public static PartialSpec getDefaultPartial()
		{
			var partial = new PartialSpec("VanguardSpec");

			partial[VSPEC.NAME] = "Bizhawk";
			partial[VSPEC.SYSTEM] = String.Empty;
			partial[VSPEC.GAMENAME] = String.Empty;
			partial[VSPEC.SYSTEMPREFIX] = String.Empty;
			partial[VSPEC.OPENROMFILENAME] = String.Empty;
			partial[VSPEC.SYNCSETTINGS] = String.Empty;
			partial[VSPEC.MEMORYDOMAINS_BLACKLISTEDDOMAINS] = new string[] { };
			partial[VSPEC.MEMORYDOMAINS_INTERFACES] = new MemoryDomainProxy[] { };
			partial[VSPEC.CORE_LASTLOADERROM] = -1;
			partial[VSPEC.SUPPORTS_RENDERING] = true;
			partial[VSPEC.SUPPORTS_CONFIG_MANAGEMENT] = true;
			partial[VSPEC.SUPPORTS_CONFIG_HANDOFF] = true;
			partial[VSPEC.SUPPORTS_KILLSWITCH] = true;
			partial[VSPEC.SUPPORTS_REALTIME] = true;
			partial[VSPEC.SUPPORTS_SAVESTATES] = true;
			partial[VSPEC.SUPPORTS_MIXED_STOCKPILE] = true;
			partial[VSPEC.CONFIG_PATHS] = new[] {Path.Combine(emuDir, "config.ini")};

			return partial;
		}



		public static void RegisterEmuhawkSpec()
		{
			PartialSpec emuSpecTemplate = new PartialSpec("VanguardSpec");

			emuSpecTemplate.Insert(VanguardCore.getDefaultPartial());

			AllSpec.VanguardSpec = new FullSpec(emuSpecTemplate, !CorruptCore.Attached); //You have to feed a partial spec as a template

			if (VanguardCore.attached)
				RTCV.Vanguard.VanguardConnector.PushVanguardSpecRef(AllSpec.VanguardSpec);

			LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHVANGUARDSPEC, emuSpecTemplate, true);
			LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHVANGUARDSPEC, emuSpecTemplate, true);


			AllSpec.VanguardSpec.SpecUpdated += (o, e) =>
			{
				PartialSpec partial = e.partialSpec;


				LocalNetCoreRouter.Route(NetcoreCommands.CORRUPTCORE, NetcoreCommands.REMOTE_PUSHVANGUARDSPECUPDATE, partial, true);
				LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_PUSHVANGUARDSPECUPDATE, partial, true);
			};
		}

		//This is the entry point of RTC. Without this method, nothing will load.
		public static void Start(RTC_Standalone_Form _standaloneForm = null)
		{
			//Grab an object on the main thread to use for netcore invokes
			SyncObjectSingleton.SyncObject = GlobalWin.MainForm;
			SyncObjectSingleton.EmuThreadIsMainThread = true;

			//Start everything
			VanguardImplementation.StartClient();
			VanguardCore.RegisterEmuhawkSpec();
			CorruptCore.StartEmuSide();

			//Refocus on Bizhawk
			Hooks.BIZHAWK_MAINFORM_FOCUS();

			//Force create bizhawk config file if it doesn't exist
			//if (!File.Exists(CorruptCore.bizhawkDir + Path.DirectorySeparatorChar + "config.ini"))
				//Hooks.BIZHAWK_MAINFORM_SAVECONFIG();

			//If it's attached, lie to vanguard
			if (VanguardCore.attached)
				VanguardConnector.ImplyClientConnected();
		}


		public static void StartSound()
		{
			Hooks.BIZHAWK_STARTSOUND();
		}

		public static void StopSound()
		{
			Hooks.BIZHAWK_STOPSOUND();
		}


		/// <summary>
		/// Loads a NES-based title screen.
		/// Can be overriden by putting a file named "overridedefault.nes" in the ASSETS folder
		/// </summary>
		public static void LoadDefaultRom()
		{
			int lastLoaderRom = VanguardCore.LastLoaderRom;
			int newNumber = VanguardCore.LastLoaderRom;

			while (newNumber == lastLoaderRom)
			{
				int nbNesFiles = Directory.GetFiles(CorruptCore.assetsDir, "*.nes").Length;

				newNumber = CorruptCore.RND.Next(1, nbNesFiles + 1);

				if (newNumber != lastLoaderRom)
				{
					if (File.Exists(Path.Combine(CorruptCore.assetsDir, "overridedefault.nes") ))
						LoadRom_NET(Path.Combine(CorruptCore.assetsDir + "overridedefault.nes"));
					//Please ignore
					else if (CorruptCore.RND.Next(0, 420) == 7)
						LoadRom_NET(Path.Combine(CorruptCore.assetsDir + "gd.fds"));
					else
						LoadRom_NET(Path.Combine(CorruptCore.assetsDir, newNumber.ToString() + "default.nes"));

					lastLoaderRom = newNumber;
					break;
				}
			}
		}

		/// <summary>
		/// Loads a rom within Bizhawk. To be called from within Bizhawk only
		/// </summary>
		/// <param name="RomFile"></param>
		public static void LoadRom_NET(string RomFile)
		{
			var loadRomWatch = Stopwatch.StartNew();

			StopSound();

			if (RomFile == null)
				RomFile = Hooks.BIZHAWK_GET_CURRENTLYOPENEDROM(); ;


			//Stop capturing rewind while we load
			Hooks.AllowCaptureRewindState = false;
			Hooks.BIZHAWK_LOADROM(RomFile);
			Hooks.AllowCaptureRewindState = true;

			StartSound();
			loadRomWatch.Stop();
			Console.WriteLine($"Time taken for LoadRom_NET: {0}ms", loadRomWatch.ElapsedMilliseconds);
		}

		/// <summary>
		/// Creates a savestate using a key as the filename and returns the path.
		/// Bizhawk process only.
		/// </summary>
		/// <param name="Key"></param>
		/// <param name="threadSave"></param>
		/// <returns></returns>
		public static string SaveSavestate_NET(string Key, bool threadSave = false)
		{
			//Don't state if we don't have a core
			if (Hooks.BIZHAWK_ISNULLEMULATORCORE())
				return null;

			//Build the shortname
			string quickSlotName = Key + ".timejump";

			//Get the prefix for the state
			string prefix = Hooks.BIZHAWK_GET_SAVESTATEPREFIX();
			prefix = prefix.Substring(prefix.LastIndexOf('\\') + 1);

			//Build up our path
			var path = Path.Combine(CorruptCore.workingDir, "SESSION", prefix + "." + quickSlotName + ".State");

			//If the path doesn't exist, make it
			var file = new FileInfo(path);
			if (file.Directory != null && file.Directory.Exists == false)
				file.Directory.Create();

			//Savestates on a new thread. Doesn't work properly as Bizhawk doesn't support threaded states
			if (threadSave)
			{
				(new Thread(() =>
				{
					try
					{
						Hooks.BIZHAWK_SAVESTATE(path, quickSlotName);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Thread collision ->\n" + ex.ToString());
					}
				})).Start();
			}
			else
				Hooks.BIZHAWK_SAVESTATE(path, quickSlotName); //savestate

			return path;
		}

		/// <summary>
		/// Loads a savestate from a path. 
		/// </summary>
		/// <param name="path">The path of the state</param>
		/// <param name="stateLocation">Where the state is located in a stashkey (used for errors, not required)</param>
		/// <returns></returns>
		public static bool LoadSavestate_NET(string path, StashKeySavestateLocation stateLocation = StashKeySavestateLocation.DEFAULTVALUE)
		{
			try
			{
				//If we don't have a core just exit out
				if (Hooks.BIZHAWK_ISNULLEMULATORCORE())
					return false;

				//If we can't find the file, throw a message
				if (File.Exists(path) == false)
				{
					Hooks.BIZHAWK_OSDMESSAGE("Unable to load " + Path.GetFileName(path) + " from " + stateLocation);
					return false;
				}

				Hooks.BIZHAWK_LOADSTATE(path);

				RTCV.NetCore.AllSpec.CorruptCoreSpec.Update(RTCSPEC.STEP_RUNBEFORE.ToString(), true);

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// Loads the window size/position from a param
		/// </summary>
		public static void LoadBizhawkWindowState()
		{
			if (RTCV.NetCore.Params.IsParamSet("BIZHAWK_SIZE"))
			{
				string[] size = RTCV.NetCore.Params.ReadParam("BIZHAWK_SIZE").Split(',');
				Hooks.BIZHAWK_GETSET_MAINFORMSIZE = new Size(Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
				string[] location = RTCV.NetCore.Params.ReadParam("BIZHAWK_LOCATION").Split(',');
				Hooks.BIZHAWK_GETSET_MAINFORMLOCATION = new Point(Convert.ToInt32(location[0]), Convert.ToInt32(location[1]));
			}
		}
		/// <summary>
		/// Saves the window size/position to a param
		/// </summary>
		public static void SaveBizhawkWindowState()
		{
			var size = Hooks.BIZHAWK_GETSET_MAINFORMSIZE;
			var location = Hooks.BIZHAWK_GETSET_MAINFORMLOCATION;

			RTCV.NetCore.Params.SetParam("BIZHAWK_SIZE", $"{size.Width},{size.Height}");
			RTCV.NetCore.Params.SetParam("BIZHAWK_LOCATION", $"{location.X},{location.Y}");
		}

		/// <summary>
		/// Loads the default rom and shows bizhawk
		/// </summary>
		public static void LoadDefaultAndShowBizhawkForm()
		{

			VanguardCore.LoadDefaultRom();
			VanguardCore.LoadBizhawkWindowState();
			GlobalWin.MainForm.Focus();

			//Yell at the user if they're using audio throttle as it's buggy
			if (Global.Config.SoundThrottle)
			{
				MessageBox.Show("Sound throttle is buggy and can result in crashes.\nSwapping to clock throttle.");
				Global.Config.SoundThrottle = false;
				Global.Config.ClockThrottle = true;
				Hooks.BIZHAWK_MAINFORM_SAVECONFIG();
			}
		}


		/// <summary>
		/// Returns the list of domains that are blacklisted from being auto-selected
		/// </summary>
		/// <param name="systemName"></param>
		/// <returns></returns>
		public static string[] GetBlacklistedDomains(string systemName)
		{
			// Returns the list of Domains that can't be rewinded and/or are just not good to use

			List<string> domainBlacklist = new List<string>();
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

					if (MemoryDomains.MemoryInterfaces.ContainsKey("SGB CARTROM"))
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

			return domainBlacklist.ToArray(); ;
		}

	}
}

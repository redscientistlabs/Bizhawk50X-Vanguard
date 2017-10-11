using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.ComponentModel;
using System.Xml.Serialization;
using BizHawk.Emulation.Cores.Nintendo.SNES;
using BizHawk.Emulation.Cores.Nintendo.N64;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.IO.Compression;

namespace RTC
{

	[XmlInclude(typeof(StashKey))]
	[XmlInclude(typeof(BlastLayer))]
	[XmlInclude(typeof(BlastCheat))]
	[XmlInclude(typeof(BlastByte))]
	[XmlInclude(typeof(BlastPipe))]
	[XmlInclude(typeof(BlastVector))]
	[XmlInclude(typeof(BlastUnit))]
	[Serializable()]
	public class Stockpile
    {
        public List<StashKey> StashKeys = new List<StashKey>();

		public string Name;
		public string Filename = null;
        public string ShortFilename;
		public string RtcVersion;


        public Stockpile(DataGridView dgvStockpile)
        {

			foreach (DataGridViewRow row in dgvStockpile.Rows)
                StashKeys.Add((StashKey)row.Cells[0].Value);

        }

		public Stockpile()
		{

		}

        public override string ToString()
        {
            return (Name != null ? Name : "");
        }

		public void Save(bool IsQuickSave = false)
		{
			Save(this, IsQuickSave);
		}

        public static bool Save(Stockpile sks, bool IsQuickSave = false)
        {
            if (sks.StashKeys.Count == 0)
            {
                MessageBox.Show("Can't save because the Current Stockpile is empty");
                return false;
            }

            if (!IsQuickSave)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "sks";
                saveFileDialog1.Title = "Save Stockpile File";
                saveFileDialog1.Filter = "SKS files|*.sks";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    sks.Filename = saveFileDialog1.FileName;
                    sks.ShortFilename = sks.Filename.Substring(sks.Filename.LastIndexOf("\\") + 1, sks.Filename.Length - (sks.Filename.LastIndexOf("\\") + 1));
                }
                else
                    return false;
            }
            else
            {
                sks.Filename = RTC_StockpileManager.currentStockpile.Filename;
                sks.ShortFilename = RTC_StockpileManager.currentStockpile.ShortFilename;
            }

			//Backuping bizhawk settings
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_EVENT_SAVEBIZHAWKCONFIG), true);

			//Watermarking RTC Version
			sks.RtcVersion = RTC_Core.RtcVersion;

            List<string> AllRoms = new List<string>();

			//populating Allroms array
			foreach (StashKey key in sks.StashKeys)
				if (!AllRoms.Contains(key.RomFilename))
				{
					AllRoms.Add(key.RomFilename);

                    if (key.RomFilename.ToUpper().Contains(".CUE"))
                    {
                        string cueFolder = RTC_Extensions.getLongDirectoryFromPath(key.RomFilename);
                        string[] cueLines = File.ReadAllLines(key.RomFilename);
                        List<string> binFiles = new List<string>();

                        foreach(var line in cueLines)
                            if(line.Contains ("FILE") && line.Contains("BINARY"))
                            {
                                int startFilename = line.IndexOf('"') + 1;
                                int endFilename = line.LastIndexOf('"');

                                binFiles.Add(line.Substring(startFilename, endFilename - startFilename));
                            }
                        
                        AllRoms.AddRange(binFiles.Select(it => cueFolder + it));
                    }

                    if (key.RomFilename.ToUpper().Contains(".CCD"))
                    {
                        List<string> binFiles = new List<string>();

                        if(File.Exists(RTC_Extensions.removeFileExtension(key.RomFilename) + ".sub"))
                            binFiles.Add(RTC_Extensions.removeFileExtension(key.RomFilename) + ".sub");

                        if (File.Exists(RTC_Extensions.removeFileExtension(key.RomFilename) + ".img"))
                            binFiles.Add(RTC_Extensions.removeFileExtension(key.RomFilename) + ".img");

                        AllRoms.AddRange(binFiles);
                    }

                }

            //clean temp2 folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Delete(file);


            //populating temp2 folder with roms
            for (int i = 0; i < AllRoms.Count; i++)
            {
                string rom = AllRoms[i];
                string romTempfilename = RTC_Core.rtcDir + "\\TEMP2\\" + (rom.Substring(rom.LastIndexOf("\\") + 1, rom.Length - (rom.LastIndexOf("\\") + 1)));

				if (!rom.Contains("\\"))
					rom = RTC_Core.rtcDir + "\\TEMP\\" + rom;


				if (File.Exists(romTempfilename))
                {
                    File.Delete(romTempfilename);
                    File.Copy(rom, romTempfilename);
                }
                else
                    File.Copy(rom, romTempfilename);


            }

            //clean temp folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP"))
                File.Delete(file);

            //sending back filtered files from temp2 folder to temp
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Move(file, RTC_Core.rtcDir + "\\TEMP\\" + (file.Substring(file.LastIndexOf("\\") + 1, file.Length - (file.LastIndexOf("\\") + 1))));

            //clean temp2 folder again
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP2"))
                File.Delete(file);

            foreach (StashKey key in sks.StashKeys)
            {
                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + statefilename))
                    File.Copy(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename, RTC_Core.rtcDir + "\\TEMP\\" + statefilename); // copy savestates to temp folder

            }

			if(File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
				File.Copy(RTC_Core.bizhawkDir + "\\config.ini", RTC_Core.rtcDir + "\\TEMP\\config.ini");


			for (int i = 0; i < sks.StashKeys.Count; i++) //changes RomFile to short filename
				sks.StashKeys[i].RomFilename = RTC_Extensions.getShortFilenameFromPath(sks.StashKeys[i].RomFilename);

			FileStream FS;
			XmlSerializer xs = new XmlSerializer(typeof(Stockpile));

			//creater stockpile.xml to temp folder from stockpile object
			FS = File.Open(RTC_Core.rtcDir + "\\TEMP\\stockpile.xml", FileMode.OpenOrCreate);
			xs.Serialize(FS, sks);
            FS.Close();

            //7z the temp folder to destination filename
            //string[] stringargs = { "-c", sks.Filename, RTC_Core.rtcDir + "\\TEMP\\" };
            //FastZipProgram.Exec(stringargs);

            string tempFilename = sks.Filename + ".temp";

            var comp = System.IO.Compression.CompressionLevel.Fastest;

            if (!RTC_Core.ghForm.cbCompressStockpiles.Checked)
                comp = System.IO.Compression.CompressionLevel.NoCompression;

            System.IO.Compression.ZipFile.CreateFromDirectory(RTC_Core.rtcDir + "\\TEMP\\", tempFilename, comp, false);

            if (File.Exists(sks.Filename))
                File.Delete(sks.Filename);

            File.Move(tempFilename, sks.Filename);


			RTC_StockpileManager.currentStockpile = sks;

            RTC_StockpileManager.unsavedEdits = false;


            return true;
        }

		public static bool Load(DataGridView dgvStockpile, string Filename = null)
		{

			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.DefaultExt = "sks";
				ofd.Title = "Open Stockpile File";
				ofd.Filter = "SKS files|*.sks";
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName.ToString();
				}
				else
					return false;
			}

			if (!File.Exists(Filename))
			{
				MessageBox.Show("The Stockpile file wasn't found");
				return false;
			}

            var token = RTC_NetCore.HugeOperationStart();

            Extract(Filename, "TEMP", "stockpile.xml");

			FileStream FS;
			XmlSerializer xs = new XmlSerializer(typeof(Stockpile));
			Stockpile sks;

			try
			{
				FS = File.Open(RTC_Core.rtcDir + "\\TEMP\\stockpile.xml", FileMode.OpenOrCreate);
				sks = (Stockpile)xs.Deserialize(FS);
				FS.Close();
			}
			catch
			{
				MessageBox.Show("The Stockpile file could not be loaded");
                RTC_NetCore.HugeOperationEnd(token);
                return false;
			}

			RTC_StockpileManager.currentStockpile = sks;

			// repopulating savestates out of temp folder
			foreach (StashKey key in sks.StashKeys)
			{

				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                string SystemFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName;
                string SystemStateFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\";

                if (!Directory.Exists(SystemFolder))
                    Directory.CreateDirectory(SystemFolder);

                if (!Directory.Exists(SystemStateFolder))
                    Directory.CreateDirectory(SystemStateFolder);

                if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
			}


			for (int i = 0; i < sks.StashKeys.Count; i++)
			{
				sks.StashKeys[i].RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + sks.StashKeys[i].RomFilename;
			}


			//fill list controls
			dgvStockpile.Rows.Clear();

			foreach (StashKey key in sks.StashKeys)
				dgvStockpile.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);

			RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.ghForm.dgvStockpile);
			RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.spForm.dgvStockpile);

			sks.Filename = Filename;

			CheckCompatibility(sks);


            RTC_NetCore.HugeOperationEnd(token);

            return true;

		}

		public static void CheckCompatibility(Stockpile sks)
		{
			List<string> ErrorMessages = new List<string>();

			if (sks.RtcVersion != RTC_Core.RtcVersion)
			{
				if (sks.RtcVersion == null)
					ErrorMessages.Add("You have loaded a broken stockpile that didn't contain an RTC Version number\n. There is no reason to believe that these items will work.");
				else
					ErrorMessages.Add("You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RTC_Core.RtcVersion + "\n Items might not appear identical to how they when they were created or it is possible that they don't work if BizHawk was upgraded.");
			}

            /*
            // We switch cores on the fly now, no more need for checking core mismatch

            Dictionary<string, string> StashkeySystemNameToCurrentCore = new Dictionary<string, string>();

			foreach(StashKey sk in sks.StashKeys)
			{
                string currentCore;
                string systemName = sk.SystemName;

                if (StashkeySystemNameToCurrentCore.ContainsKey(systemName))
                    currentCore = StashkeySystemNameToCurrentCore[systemName];
                else
                {
                    currentCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = systemName }, true);
                    StashkeySystemNameToCurrentCore.Add(systemName, currentCore);
                }

				if(sk.SystemCore != currentCore)
				{
					string errorMessage = $"Core mismatch for System [{sk.SystemName}]\n Current Bizhawk core -> {currentCore}\n Stockpile core -> {sk.SystemCore}";

					if (!ErrorMessages.Contains(errorMessage))
						ErrorMessages.Add(errorMessage);
				}

			}
            */

			if (ErrorMessages.Count == 0)
				return;

			string message = "The loaded stockpile returned the following errors:\n\n";

			foreach (string line in ErrorMessages)
				message += $"•  {line} \n\n";


			MessageBox.Show(message, "Compatibility Checker", MessageBoxButtons.OK, MessageBoxIcon.Warning);

		}

        public static void Extract(string Filename, string Folder, string MasterFile)
        {
            //clean temp folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + $"\\{Folder}"))
                File.Delete(file);

            //7z extract part

            //string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + $"\\{Folder}\\" };
            //FastZipProgram.Exec(stringargs);

			System.IO.Compression.ZipFile.ExtractToDirectory(Filename, RTC_Core.rtcDir + $"\\{Folder}\\");


			if (!File.Exists(RTC_Core.rtcDir + $"\\{Folder}\\{MasterFile}"))
            {
                MessageBox.Show("The file could not be read properly");
                return;
            }
        }

		public static void LoadBizhawkConfig(string Filename = null)
		{

			if (Filename == null)
			{
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.DefaultExt = "sks";
				ofd.Title = "Open Stockpile File";
				ofd.Filter = "SKS files|*.sks";
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					Filename = ofd.FileName.ToString();
				}
				else
					return;
			}

			if (File.Exists(RTC_Core.bizhawkDir + "\\backup_config.ini"))
			{
				if (MessageBox.Show("Do you want to overwrite the previous Config Backup with the current Bizhawk Config?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					File.Delete(RTC_Core.bizhawkDir + "\\backup_config.ini");
					File.Copy((RTC_Core.bizhawkDir + "\\config.ini"), (RTC_Core.bizhawkDir + "\\backup_config.ini"));
				}
			}
			else
				File.Copy((RTC_Core.bizhawkDir + "\\config.ini"), (RTC_Core.bizhawkDir + "\\backup_config.ini"));

			Extract(Filename, "TEMP", "stockpile.xml");

            if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
                File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");
            File.Copy((RTC_Core.rtcDir + "\\TEMP\\config.ini"), (RTC_Core.bizhawkDir + "\\stockpile_config.ini"));


            RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_MERGECONFIG), true);

            Process.Start(RTC_Core.bizhawkDir + $"\\StockpileConfig{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");

		}

        public static void MergeBizhawkConfig_NET()
        {

            Config bc;
            Config sc;

            var fileBc = new FileInfo(RTC_Core.bizhawkDir + "\\backup_config.ini");
            var fileSc = new FileInfo(RTC_Core.bizhawkDir + "\\stockpile_config.ini");


            using (var reader = fileBc.OpenText())
            {
                var r = new JsonTextReader(reader);
                bc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
            }

            using (var reader = fileSc.OpenText())
            {
                var r = new JsonTextReader(reader);
                sc = (Config)ConfigService.Serializer.Deserialize(r, typeof(Config));
            }

            //bc = (JObject)JsonConvert.DeserializeObject(backupConfig);
            //sc = (JObject)JsonConvert.DeserializeObject(stockpileConfig);


            sc.HotkeyBindings = bc.HotkeyBindings;
            sc.AllTrollers = bc.AllTrollers;
            sc.AllTrollersAutoFire = bc.AllTrollersAutoFire;
            sc.AllTrollersAnalog = bc.AllTrollersAnalog;

            if (File.Exists(RTC_Core.bizhawkDir + "\\stockpile_config.ini"))
                File.Delete(RTC_Core.bizhawkDir + "\\stockpile_config.ini");

           
            try
            {
                using (var writer = fileSc.CreateText())
                {
                    var w = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
                    ConfigService.Serializer.Serialize(w, sc);
                }
            }
            catch
            {
                /* Eat it */
            }
        }

		public static void RestoreBizhawkConfig()
		{

			Process.Start(RTC_Core.bizhawkDir + $"\\RestoreConfig{(RTC_Core.isStandalone ? "DETACHED" : "ATTACHED")}.bat");
		}

		public static void Import()
        {
            Import(null, false);
        }

        public static void Import(string Filename, bool CorruptCloud)
        {

            //clean temp folder
            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3"))
                File.Delete(file);

            if (Filename == null)
            {
                OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
                OpenFileDialog1.DefaultExt = "sks";
                OpenFileDialog1.Title = "Open Stockpile File";
                OpenFileDialog1.Filter = "SKS files|*.sks";
                OpenFileDialog1.RestoreDirectory = true;
                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Filename = OpenFileDialog1.FileName.ToString();
                }
                else
                    return;
            }

            if (!File.Exists(Filename))
            {
                MessageBox.Show("The Stockpile file wasn't found");
                return;
            }

            //7z extract part

            //string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + "\\TEMP3\\" };
            //FastZipProgram.Exec(stringargs);

			System.IO.Compression.ZipFile.ExtractToDirectory(Filename, RTC_Core.rtcDir + $"\\TEMP3\\");

			if (!File.Exists(RTC_Core.rtcDir + "\\TEMP3\\stockpile.xml"))
            {
                MessageBox.Show("The file could not be read properly");
                return;
            }



            //stockpile se deserialization
            FileStream FS;
			XmlSerializer xs = new XmlSerializer(typeof(Stockpile));
            Stockpile sks;

            try
            {
                FS = File.Open(RTC_Core.rtcDir + "\\TEMP3\\stockpile.xml", FileMode.OpenOrCreate);
				sks = (Stockpile)xs.Deserialize(FS);
				FS.Close();
            }
            catch
            {
                MessageBox.Show("The Stockpile file could not be loaded");
                return;
            }

            // repopulating savestates out of temp folder
            foreach (StashKey key in sks.StashKeys)
            {
                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                string SystemFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName;
                string SystemStateFolder = RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\";

                if (!Directory.Exists(SystemFolder))
                    Directory.CreateDirectory(SystemFolder);

                if (!Directory.Exists(SystemStateFolder))
                    Directory.CreateDirectory(SystemStateFolder);

                if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
                    File.Copy(RTC_Core.rtcDir + "\\TEMP3\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
            }

            for (int i = 0; i < sks.StashKeys.Count; i++)
            {
                sks.StashKeys[i].RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + sks.StashKeys[i].RomFilename;
            }

            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3\\"))
                if(!file.Contains(".sk") && !file.Contains(".timejump.State"))
                    try
                    {
                        File.Copy(file, RTC_Core.rtcDir + "\\TEMP\\" + file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'))); // copy roms to temp
                    }
                    catch{}

            //fill list controls

            foreach (StashKey sk in sks.StashKeys)
            {
				var dataRow = RTC_Core.ghForm.dgvStockpile.Rows[RTC_Core.ghForm.dgvStockpile.Rows.Add()];
				dataRow.Cells["Item"].Value = sk;
				dataRow.Cells["GameName"].Value = sk.GameName;
				dataRow.Cells["SystemName"].Value = sk.SystemName;
				dataRow.Cells["SystemCore"].Value = sk.SystemCore;
            }

            RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.ghForm.dgvStockpile);
            CheckCompatibility(sks);


            RTC_StockpileManager.StockpileChanged();

        }

    }
    

    
    [Serializable()]
    public class StashKey : ICloneable
    {

        public string RomFilename;
		public byte[] RomData = null;

		public string StateShortFilename = null;
		public string StateFilename = null;
		public byte[] StateData = null;

		public string SystemName;
        public string SystemDeepName;
		public string SystemCore;
		public List<string> SelectedDomains = new List<string>();
        public string GameName;
		public string Note = null;


		public string Key;
        public string ParentKey = null;
        public BlastLayer BlastLayer = null;

        public string Alias
        {
            get
            {
                    return _Alias ?? Key;
            }
            set
            {
                _Alias = value;
            }
        }

        private string _Alias;

        public StashKey(string _key, string _parentkey, BlastLayer _blastlayer)
        {

			Key = _key;
            ParentKey = _parentkey;
            BlastLayer = _blastlayer;


			RomFilename = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true);
			SystemName = RTC_Core.EmuFolderCheck((string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMNAME), true));
            SystemCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = SystemName }, true);
			GameName = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);

			this.SelectedDomains.AddRange(RTC_MemoryDomains.SelectedDomains);

        }

		public StashKey()
		{

		}

		public object Clone()
		{
            object sk = ObjectCopier.Clone(this);
            (sk as StashKey).Key = RTC_Core.GetRandomKey();
            (sk as StashKey).Alias = null;
            return sk;
		}

        public static void setCore(StashKey sk) => setCore(sk.SystemName, sk.SystemCore);
        public static void setCore(string _systemName, string _systemCore)
        {
            switch (_systemName.ToUpper())
            {
                case "GAMEBOY":
                    Global.Config.GB_AsSGB = _systemCore == "sameboy";
                    Global.Config.SGB_UseBsnes = false;
                    break;
                case "NES":
                    Global.Config.NES_InQuickNES = _systemCore == "quicknes";
                    break;
                case "SNES":

                    if(_systemCore == "bsnes_SGB")
                    {
                        Global.Config.GB_AsSGB = true;
                        Global.Config.SGB_UseBsnes = true;
                    }
                    else
                        Global.Config.SNES_InSnes9x = _systemCore == "snes9x";

                    break;
                case "GBA":
                    Global.Config.GBA_UsemGBA = _systemCore == "mgba";
                    break;
                case "N64":

                    string[] coreParts = _systemCore.Split('/');

                    N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
                    ?? new N64SyncSettings();

                    ss.VideoPlugin = (PluginType)Enum.Parse(typeof(PluginType), coreParts[0], true);
                    ss.Rsp = (N64SyncSettings.RspType)Enum.Parse(typeof(N64SyncSettings.RspType), coreParts[1], true);
                    ss.Core = (N64SyncSettings.CoreType)Enum.Parse(typeof(N64SyncSettings.CoreType), coreParts[2], true);
                    ss.DisableExpansionSlot = (coreParts[3] == "NoExp");

                    N64VideoPluginconfig.PutSyncSettings(ss);

                    break;
            }
        }

        public static string getCoreName_NET(string _systemName)
        {

            switch (_systemName.ToUpper())
            {
                case "GAMEBOY":
                    return (Global.Config.GB_AsSGB ? "sameboy" : "gambatte");

                case "NES":
                    return (Global.Config.NES_InQuickNES ? "quicknes" : "neshawk");

                case "SNES":

                    if (RTC_MemoryDomains.MemoryInterfaces.ContainsKey("SGB WRAM"))
                        return "bsnes_SGB";

                    return (Global.Config.SNES_InSnes9x ? "snes9x" : "bsnes");

                case "GBA":
                    return (Global.Config.GBA_UsemGBA ? "mgba" : "vba-next");

                case "N64":
                    N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
                    ?? new N64SyncSettings();

                    return $"{ss.VideoPlugin}/{ss.Rsp}/{ss.Core}/{(ss.DisableExpansionSlot ? "NoExp" : "Exp")}";
            }

            return _systemName;
        }

        public override string ToString()
        {
            return Alias;
        }

		public bool Run()
		{
			RTC_StockpileManager.currentStashkey = this;
			return RTC_StockpileManager.ApplyStashkey(this);

		}

        public void RunOriginal()
        {
			RTC_StockpileManager.currentStashkey = this;
            RTC_StockpileManager.OriginalFromStashkey(this);
        }

		public byte[] EmbedState()
		{
			if (StateFilename == null)
				return null;

			if (this.StateData != null)
				return this.StateData;

			byte[] stateData = File.ReadAllBytes(StateFilename);
			this.StateData = stateData;

			return stateData;
		}

		public bool DeployState()
		{
			if (StateShortFilename == null || this.StateData == null)
				return false;

			string deployedStatePath = getSavestateFullPath();

			if (File.Exists(deployedStatePath))
				return true;

			File.WriteAllBytes(deployedStatePath, this.StateData);

			return true;
		}

		public string getSavestateFullPath()
		{
			return RTC_Core.bizhawkDir + "\\" + this.SystemName + "\\State\\" + this.GameName + "." + this.ParentKey + ".timejump.State"; // get savestate name
		}
	}
    
	[Serializable()]
	public class SaveStateKeys
	{
		public StashKey[] StashKeys = new StashKey[41];
		public string[] Text = new string[41];
	}

    [Serializable()]
    public class BlastTarget
    {

        public string domain = null;
        public long address = 0;

        public BlastTarget(string _domain, long _address)
        {
            domain = _domain;
            address = _address;
        }
    }

    [Serializable()]
	public class BlastLayer : ICloneable
	{
		public List<BlastUnit> Layer;

		public BlastLayer()
		{
			Layer = new List<BlastUnit>();
		}

		public BlastLayer(List<BlastUnit> _layer)
		{
			Layer = _layer;
		}

        public object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public void Apply(bool ignoreMaximums = false)
		{
			if(RTC_Core.isStandalone)
			{
				RTC_Core.SendCommandToBizhawk(new RTC.RTC_Command(CommandType.BLAST) { blastlayer = this});
				return;
			}

            /*
			if (this != RTC_StockpileManager.lastBlastLayerBackup &&
				RTC_Core.SelectedEngine != CorruptionEngine.HELLGENIE &&
				RTC_Core.SelectedEngine != CorruptionEngine.FREEZE &&
				RTC_Core.SelectedEngine != CorruptionEngine.PIPE)
				RTC_StockpileManager.lastBlastLayerBackup = GetBackup();
            */

            if (this != RTC_StockpileManager.lastBlastLayerBackup)
                RTC_StockpileManager.lastBlastLayerBackup = GetBackup();

            bool success;

			try
			{

				foreach (BlastUnit bb in Layer)
				{

                    if (bb == null) //BlastCheat getBackup() always returns null so they can happen and they are valid
                        success = true;
                    else
					    success = bb.Apply();


					if (!success)
						throw new Exception(
						"One of the BlastUnits in the BlastLayer failed to Apply().\n\n" +
						"The operation was cancelled");

				}

			}

			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while applying a BlastLayer to the game.\n\n" +
							"The operation was cancelled\n\n" +
							ex.ToString()
							);

			}
			finally
			{
				if (!ignoreMaximums)
				{
					
					RTC_HellgenieEngine.RemoveExcessCheats();
					RTC_PipeEngine.RemoveExcessPipes();
				}
			}
		}
	

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>(); ;
            
			BackupLayer.AddRange(Layer.Select(it => it.GetBackup()));

            return new BlastLayer(BackupLayer);
		}

		public void Reroll()
		{
			foreach (BlastUnit bu in Layer)
				bu.Reroll();
		}

        public void Rasterize()
        {
            foreach (BlastUnit bu in Layer)
                bu.Rasterize();
        }
    }

    [Serializable()]
    public abstract class BlastUnit
    {
        public abstract bool Apply();
        public abstract BlastUnit GetBackup();
		public abstract void Reroll();

        public abstract void Rasterize();

        public abstract bool IsEnabled { get; set; }

        public abstract string Domain { get; set; }
        public abstract long Address { get; set; }
    }

    [Serializable()]
    public class BlastByte : BlastUnit
    {
        public override string Domain { get; set; }
        public override long Address { get; set; }
        public BlastByteType Type;
        public int Value;
        public override bool IsEnabled { get; set; }

		public BlastByte(string _domain, long _address, BlastByteType _type, int _value, bool _isEnabled)
        {
            Domain = _domain;
            Address = _address;
            Type = _type;
            Value = _value;
            IsEnabled = _isEnabled;
        }

        public BlastByte()
		{

		}

        public override void Rasterize()
        {
            if(Domain.Contains("[V]"))
            {
                /*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

                string _domain = (string)Domain.Clone();
                long _address = Address;

                Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
                Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
            }
        }

        public override bool Apply()
        {
            if (!IsEnabled)
                return true;

            try
            {
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);

				if (mdp == null)
					return true;

                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);

                switch (Type)
                {
                    case BlastByteType.SET:
                        mdp.PokeByte(targetAddress, (byte)Value);
                        break;

                    case BlastByteType.ADD:
                        mdp.PokeByte(targetAddress, (byte)(mdp.PeekByte(targetAddress) + Value));
                        break;

                    case BlastByteType.SUBSTRACT:
                        mdp.PokeByte(targetAddress, (byte)(mdp.PeekByte(targetAddress) - Value));
                        break;

                    case BlastByteType.NONE:
                        return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("The BlastByte apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
                ex.ToString());
            }

			return true;

        }

        public override BlastUnit GetBackup()
        {
            if (!IsEnabled)
                return null;

            try
            {
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);
                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);

                if (mdp == null || Type == BlastByteType.NONE)
                    return null;

                return new BlastByte(Domain, Address, BlastByteType.SET, mdp.PeekByte(targetAddress), true);

            }
            catch (Exception ex)
            {
                throw new Exception("The BlastByte GetBackup() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
                ex.ToString());
            }

        }

		public override void Reroll()
		{
			if(Type == BlastByteType.SET)
			{
				Value = RTC_Core.RND.Next(0, 255);
			}
			else if(Type == BlastByteType.ADD || Type == BlastByteType.SUBSTRACT)
			{
				var result = RTC_Core.RND.Next(1, 3);
				switch (result)
				{
					case 1:
						Type = BlastByteType.ADD;
						break;

					case 2:
						Type = BlastByteType.SUBSTRACT;
						break;
				}
				
			}
		}

        public override string ToString()
        {
            string EnabledString = "[ ] BlastByte -> ";
            if (IsEnabled)
                EnabledString = "[x] BlastByte -> ";

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Type.ToString() + "(" + Value.ToString() + ")");
        }
    }

	[Serializable()]
	public class BlastVector : BlastUnit
	{
		public override string Domain { get; set; }
        public override long Address { get; set; }
        public BlastByteType Type;

		public byte[] Values;

		public override bool IsEnabled { get; set; }

		public BlastVector(string _domain, long _address, byte[] _values, bool _isEnabled)
		{
			Domain = _domain;
			Address = (_address - (_address % 4));
			Values = _values;
			IsEnabled = _isEnabled;
		}

		public BlastVector()
		{

		}

        public override void Rasterize()
        {
            if (Domain.Contains("[V]"))
            {
                /*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

                string _domain = (string)Domain.Clone();
                long _address = Address;

                Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
                Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
            }
        }

        public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);

				if (mdp == null)
					return true;

                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);

                mdp.PokeByte(targetAddress, Values[0]);
				mdp.PokeByte(targetAddress + 1, Values[1]);
				mdp.PokeByte(targetAddress + 2, Values[2]);
				mdp.PokeByte(targetAddress + 3, Values[3]);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
				ex.ToString());
			}

			return true;

		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);
				if (mdp == null)
					return null;

                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);

                return new BlastVector(Domain, Address, RTC_VectorEngine.read32bits(mdp, targetAddress), true);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector GetBackup() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
				ex.ToString());
			}

		}

		public override void Reroll()
		{
			Values = RTC_VectorEngine.getRandomConstant(RTC_VectorEngine.valueList);
		}

		public override string ToString()
		{
			string EnabledString = "[ ] BlastVector -> ";
			if (IsEnabled)
				EnabledString = "[x] BlastVector -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Type.ToString() + "(" + RTC_VectorEngine.ByteArrayToString(Values) + ")");
		}
	}


	[Serializable()]
	public class BlastPipe : BlastUnit
	{
		public override string Domain { get; set; }
        public override long Address { get; set; }
        public string PipeDomain;
		public long PipeAddress;
        public int TiltValue;

        public override bool IsEnabled { get; set; }

		public BlastPipe(string _domain, long _address, string _pipeDomain, long _pipeAddress, int _tiltValue, bool _isEnabled)
		{
			Domain = _domain;
			Address = _address;
			PipeDomain = _pipeDomain;
			PipeAddress = _pipeAddress;
			IsEnabled = _isEnabled;
            TiltValue = _tiltValue;
		}

		public BlastPipe()
		{
            new object();
		}

        public void Execute()
		{
			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);
				MemoryDomainProxy mdp2 = RTC_MemoryDomains.getProxy(PipeDomain, PipeAddress);

				if (mdp == null || mdp2 == null)
					throw new Exception($"Memory Domain error, MD1 -> {mdp.ToString()}, md2 -> {mdp2.ToString()}");

                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);
                long targetPipeAddress = RTC_MemoryDomains.getRealAddress(PipeDomain, PipeAddress);

                int currentValue = (int)mdp.PeekByte(targetAddress);

                int newValue = currentValue + TiltValue;

                if (newValue < 0)
                    newValue = 0;
                else if (newValue > 255)
                    newValue = 255;

                mdp2.PokeByte(targetPipeAddress,  (byte)newValue);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastPipe apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
				ex.ToString());
			}
		}

        public override void Rasterize()
        {
            if (Domain.Contains("[V]"))
            {
                /*
                Tuple<string,long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp != null)
                {
                    Domain = mp.Item1;
                    Address = mp.Item2;
                }
                */

                string _domain = (string)Domain.Clone();
                long _address = Address;

                Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
                Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
            }

            if (PipeDomain.Contains("[V]"))
            {
                /*
                Tuple<string, long> mp = (RTC_MemoryDomains.VmdPool[PipeDomain] as VirtualMemoryDomain)?.MemoryPointers[(int)PipeAddress];
                if (mp != null)
                {
                    PipeDomain = mp.Item1;
                    PipeAddress = mp.Item2;
                }
                */
                string _domain = (string)PipeDomain.Clone();
                long _address = PipeAddress;

                PipeDomain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
                PipeAddress = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
            }
        }

        public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			RTC_PipeEngine.AddUnit(this);

			return true;

		}

		public override BlastUnit GetBackup()
		{
			if (!IsEnabled)
				return null;

			try
			{
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(PipeDomain, PipeAddress);

				if (mdp == null)
					return null;

				return new BlastByte(PipeDomain, PipeAddress, BlastByteType.SET, mdp.PeekByte(PipeAddress), true);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastPipe GetBackup() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
				ex.ToString());
			}

		}


		public override void Reroll()
		{
            var pipeEnd = RTC_Core.GetBlastTarget();

            PipeDomain = pipeEnd.domain;
            PipeAddress = pipeEnd.address;
        }

		public override string ToString()
		{
			string EnabledString = "[ ] BlastPipe -> ";
			if (IsEnabled)
				EnabledString = "[x] BlastPipe -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			string cleanDomainName2 = PipeDomain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")piped->" + cleanDomainName2 + "(" + Convert.ToInt32(PipeAddress).ToString() + "), tilt->" + TiltValue.ToString());
		}
	}


	[Serializable()]
    public class BlastCheat : BlastUnit
    {
        public override string Domain { get; set; }
        public override long Address { get; set; }
        public BizHawk.Client.Common.DisplayType DisplayType;
        public bool BigEndian;
        public int Value;
        public bool IsFreeze;

		public override bool IsEnabled { get; set; }

		public BlastCheat(string _domain, long _address, BizHawk.Client.Common.DisplayType _displayType, bool _bigEndian, int _value, bool _isEnabled, bool _isFreeze)
        {
			//because of this, blastcheats can't be generated on standalone side.
            var settings = new RamSearchEngine.Settings(RTC_MemoryDomains.MDRI.MemoryDomains);

            Domain = _domain;
			Address = _address - (_address % (int)settings.Size);

            
            DisplayType = settings.Type;
            BigEndian = settings.BigEndian;

            Value = _value;
            IsEnabled = _isEnabled;
            IsFreeze = _isFreeze;
            
        }

		public BlastCheat()
		{
		}

        public override void Rasterize()
        {
            if (Domain.Contains("[V]"))
            {
                /*
                Tuple<string, long> mp = (RTC_MemoryDomains.VmdPool[Domain] as VirtualMemoryDomain)?.MemoryPointers[(int)Address];
                if (mp == null)
                    return;

                Domain = mp.Item1;
                Address = mp.Item2;
                */

                string _domain = (string)Domain.Clone();
                long _address = Address;

                Domain = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerDomains[(int)_address] ?? "ERROR";
                Address = (RTC_MemoryDomains.VmdPool[_domain] as VirtualMemoryDomain)?.PointerAddresses[(int)_address] ?? -1;
            }
        }

        public override bool Apply()
        {
            try
            {
                if (!IsEnabled)
                    return true;

                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(Domain, Address);
                var settings = new RamSearchEngine.Settings(RTC_MemoryDomains.MDRI.MemoryDomains);

                if (mdp == null)
					return true;

                string targetDomain = RTC_MemoryDomains.getRealDomain(Domain, Address);
                long targetAddress = RTC_MemoryDomains.getRealAddress(Domain, Address);

                string cheatName = "RTC Cheat|" + targetDomain + "|" + targetAddress.ToString() + "|" + DisplayType.ToString() + "|" + BigEndian.ToString() + "|" + Value.ToString() + "|" + IsEnabled.ToString() + "|" + IsFreeze.ToString();

                if (!IsFreeze)
                {
                    Watch somewatch = Watch.GenerateWatch(mdp.md, targetAddress, settings.Size, DisplayType, BigEndian, cheatName, Value, 0,0);
                    Cheat ch = new Cheat(somewatch, Value, null, true);
                    Global.CheatList.Add(ch);
                }
                else
                {
                    int _value = mdp.PeekByte(targetAddress);

                    Watch somewatch = Watch.GenerateWatch(mdp.md, targetAddress, settings.Size, DisplayType, BigEndian, cheatName, _value, 0, 0);
                    Cheat ch = new Cheat(somewatch, Value, null, true);
                    Global.CheatList.Add(ch);
                }
                    //RTC_MemoryDomains.FreezeAddress(Address, cheatName);

                
            }
            catch (Exception ex)
            {
                throw new Exception("The BlastCheat apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\nif you are able to reproduce this bug in a consistant manner\n\n" +
                ex.ToString());
            }

			return true;
        }

        public override BlastUnit GetBackup()
        {
            return null;
        }

		public override void Reroll()
		{
			Value = RTC_Core.RND.Next(255);
		}

		public override string ToString()
        {
            string EnabledString = $"[ ] BlastCheat{(IsFreeze ? ":Freeze" : "")} -> ";
            if (IsEnabled)
                EnabledString = $"[x] BlastCheat{(IsFreeze ? ":Freeze" : "")} -> ";

            string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"

            //RTC_TODO: Rewrite the toString method for this
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + DisplayType.ToString() + "(" + Value.ToString() + ")");
        }
    }

    [Serializable()]
    public class ActiveTableObject
    {
        public long[] data;

        public ActiveTableObject(long[] _data)
        {
            data = _data;
        }
    }



}

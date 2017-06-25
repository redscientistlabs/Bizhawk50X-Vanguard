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

namespace RTC
{

    public class LabelPassthrough : Label
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, this.Text.ToString(), this.Font, ClientRectangle, ForeColor);
        }

    }

	public class RefreshingListBox : ListBox
	{
		public void RefreshItemsReal()
		{
			base.RefreshItems();
		}
	}

	public class MenuButton : Button
    {
        [DefaultValue(null)]
        public ContextMenuStrip Menu { get; set; }

        public void SetMenu(ContextMenuStrip _menu)
        {
            Menu = _menu;
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);

            if (Menu != null && mevent.Button == MouseButtons.Left)
            {
                Menu.Show(this, mevent.Location);
            }
        }
    }


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
        public List<StashKey> stashkeys = new List<StashKey>();

		public string Name;
		public string Filename = null;
        public string ShortFilename;
		public string RtcVersion;


        public Stockpile(DataGridView dgvStockpile)
        {

			foreach (DataGridViewRow row in dgvStockpile.Rows)
                stashkeys.Add((StashKey)row.Cells[0].Value);

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
            if (sks.stashkeys.Count == 0)
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
			foreach (StashKey key in sks.stashkeys)
				if (!AllRoms.Contains(key.RomFilename))
				{
					AllRoms.Add(key.RomFilename);

					if(key.RomFilename.ToUpper().Contains(".CUE"))
						AllRoms.Add(key.RomFilename.Substring(0, key.RomFilename.Length -4) + ".bin");
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

            foreach (StashKey key in sks.stashkeys)
            {
                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				if (!File.Exists(RTC_Core.rtcDir + "\\TEMP\\" + statefilename))
                    File.Copy(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename, RTC_Core.rtcDir + "\\TEMP\\" + statefilename); // copy savestates to temp folder

            }

			if(File.Exists(RTC_Core.bizhawkDir + "\\config.ini"))
				File.Copy(RTC_Core.bizhawkDir + "\\config.ini", RTC_Core.rtcDir + "\\TEMP\\config.ini");


			for (int i = 0; i < sks.stashkeys.Count; i++) //changes RomFile to short filename
				sks.stashkeys[i].RomFilename = RTC_NetCore.ShortenFilename(sks.stashkeys[i].RomFilename);

			FileStream FS;
			XmlSerializer xs = new XmlSerializer(typeof(Stockpile));

			//creater stockpile.xml to temp folder from stockpile object
			FS = File.Open(RTC_Core.rtcDir + "\\TEMP\\stockpile.xml", FileMode.OpenOrCreate);
			xs.Serialize(FS, sks);
            FS.Close();

            //7z the temp folder to destination filename
            string[] stringargs = { "-c", sks.Filename, RTC_Core.rtcDir + "\\TEMP\\" };
            FastZipProgram.Exec(stringargs);

			RTC_StockpileManager.currentStockpile = sks;

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
				return false;
			}

			RTC_StockpileManager.currentStockpile = sks;

			// repopulating savestates out of temp folder
			foreach (StashKey key in sks.stashkeys)
			{

				string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

				if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
					File.Copy(RTC_Core.rtcDir + "\\TEMP\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
			}


			for (int i = 0; i < sks.stashkeys.Count; i++)
			{
				sks.stashkeys[i].RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + sks.stashkeys[i].RomFilename;
			}


			//fill list controls
			dgvStockpile.Rows.Clear();

			foreach (StashKey key in sks.stashkeys)
				dgvStockpile.Rows.Add(key, key.GameName, key.SystemName, key.SystemCore, key.Note);

			RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.ghForm.dgvStockpile);
			RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.spForm.dgvStockpile);

			sks.Filename = Filename;

			CheckCompatibility(sks);

			return true;

		}

		public static void CheckCompatibility(Stockpile sks)
		{
			List<string> ErrorMessages = new List<string>();

			if (sks.RtcVersion != RTC_Core.RtcVersion)
			{
				if (sks.RtcVersion == null)
					ErrorMessages.Add("You have loaded a broken stockpile using RTC " + RTC_Core.RtcVersion + "\n Items might not appear identical to how they when they were created.");
				else
					ErrorMessages.Add("You have loaded a stockpile created with RTC " + sks.RtcVersion + " using RTC " + RTC_Core.RtcVersion + "\n Items might not appear identical to how they when they were created.");
			}


			foreach(StashKey sk in sks.stashkeys)
			{
				string currentCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = sk.SystemName }, true);
				if(sk.SystemCore != currentCore)
				{
					string errorMessage = $"Core mismatch for System [{sk.SystemName}]\n Current Bizhawk core -> {currentCore}\n Stockpile core -> {sk.SystemCore}";

					if (!ErrorMessages.Contains(errorMessage))
						ErrorMessages.Add(errorMessage);
				}

			}

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

			string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + $"\\{Folder}\\" };

			FastZipProgram.Exec(stringargs);

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

			File.Copy((RTC_Core.rtcDir + "\\TEMP\\config.ini"), (RTC_Core.bizhawkDir + "\\stockpile_config.ini"));

			Process.Start(RTC_Core.bizhawkDir + "\\SwitchToStockpileConfig.bat");

		}

		public static void RestoreBizhawkConfig()
		{

			Process.Start(RTC_Core.bizhawkDir + "\\RestoreConfig.bat");
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

            string[] stringargs = { "-x", Filename, RTC_Core.rtcDir + "\\TEMP3\\" };

            FastZipProgram.Exec(stringargs);

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
            foreach (StashKey key in sks.stashkeys)
            {
                string statefilename = key.GameName + "." + key.ParentKey + ".timejump.State"; // get savestate name

                if (!File.Exists(RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename))
                    File.Copy(RTC_Core.rtcDir + "\\TEMP3\\" + statefilename, RTC_Core.bizhawkDir + "\\" + key.SystemName + "\\State\\" + statefilename); // copy savestates to temp folder
            }

            for (int i = 0; i < sks.stashkeys.Count; i++)
            {
                sks.stashkeys[i].RomFilename = RTC_Core.rtcDir + "\\TEMP\\" + sks.stashkeys[i].RomFilename;
            }

            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\TEMP3\\"))
                if(!file.Contains(".sk") && !file.Contains(".timejump.State"))
                    try
                    {
                        File.Copy(file, RTC_Core.rtcDir + "\\TEMP\\" + file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'))); // copy roms to temp
                    }
                    catch{}

            //fill list controls

            foreach (StashKey sk in sks.stashkeys)
            {
				var dataRow = RTC_Core.ghForm.dgvStockpile.Rows[RTC_Core.ghForm.dgvStockpile.Rows.Add()];
				dataRow.Cells["Item"].Value = sk;
				dataRow.Cells["GameName"].Value = sk.GameName;
				dataRow.Cells["SystemName"].Value = sk.SystemName;
				dataRow.Cells["SystemCore"].Value = sk.SystemCore;
            }

			RTC_Core.ghForm.RefreshNoteIcons(RTC_Core.ghForm.dgvStockpile);


			RTC_StockpileManager.StockpileChanged();

        }

    }
    

    
    [Serializable()]
    public class StashKey : ICloneable
    {

        public string RomFilename;
		public byte[] RomData = null;

		public string stateShortFilename = null;
		public string stateFilename = null;
		public byte[] stateData = null;

		public string SystemName;
		public string SystemCore;
		public List<string> SelectedDomains = new List<string>();
        public string GameName;
		public string Note = null;


		public string Key;
        public string ParentKey = null;
        public BlastLayer blastlayer = null;

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
            blastlayer = _blastlayer;


			RomFilename = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETOPENROMFILENAME), true);
			SystemName = RTC_Core.EmuFolderCheck((string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETPATHENTRY), true));
			SystemCore = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETSYSTEMCORE) { objectValue = SystemName }, true);
			GameName = (string)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_KEY_GETGAMENAME), true);

			this.SelectedDomains.AddRange(RTC_MemoryDomains.SelectedDomains);

        }

		public StashKey()
		{

		}

		public object Clone()
		{
			return ObjectCopier.Clone(this);
		}

		public static string getCoreName(string _SystemName)
		{

			switch(_SystemName)
			{
				case "NES":
					return (Global.Config.NES_InQuickNES ? "quicknes" : "neshawk");

				case "SNES":
					if (!Global.Config.SNES_InSnes9x)
					{
						var snesSettings = BizHawk.Client.EmuHawk.ProfileConfig.GetSyncSettings<LibsnesCore, LibsnesCore.SnesSyncSettings>();
						return $"bsnes:{snesSettings.Profile}";
					}
					else
						return "snes9x";

				case "GBA":
					return (Global.Config.GBA_UsemGBA ? "mgba" : "vba-next");

				case "N64":
					N64SyncSettings ss = (N64SyncSettings)Global.Config.GetCoreSyncSettings<N64>()
					?? new N64SyncSettings();

					return $"{ss.VideoPlugin}/{ss.Rsp}/{ss.Core}";


			}

			return _SystemName;
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
			if (stateFilename == null)
				return null;

			if (this.stateData != null)
				return this.stateData;

			byte[] stateData = File.ReadAllBytes(stateFilename);
			this.stateData = stateData;

			return stateData;
		}

		public bool DeployState()
		{
			if (stateShortFilename == null || this.stateData == null)
				return false;

			string deployedStatePath = getSavestateFullPath();

			if (File.Exists(deployedStatePath))
				return true;

			File.WriteAllBytes(deployedStatePath, this.stateData);

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
		public StashKey[] Stashkeys = new StashKey[41];
		public string[] Text = new string[41];
	}

	[Serializable()]
	public class BlastLayer
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

		public void Apply(bool ignoreMaximums = false)
		{
			if(RTC_Core.isStandalone)
			{
				RTC_Core.SendCommandToBizhawk(new RTC.RTC_Command(CommandType.BLAST) { blastlayer = this});
				return;
			}


			if (this != RTC_StockpileManager.lastBlastLayerBackup &&
				RTC_Core.SelectedEngine != CorruptionEngine.HELLGENIE &&
				RTC_Core.SelectedEngine != CorruptionEngine.FREEZE &&
				RTC_Core.SelectedEngine != CorruptionEngine.PIPE)
				RTC_StockpileManager.lastBlastLayerBackup = GetBackup();

			bool success;

			try
			{

				foreach (BlastUnit bb in Layer)
				{

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

	}

    [Serializable()]
    public abstract class BlastUnit
    {
        public abstract bool Apply();
        public abstract BlastUnit GetBackup();
		public abstract void Reroll();
		public abstract bool IsEnabled { get; set; }
	}

    [Serializable()]
    public class BlastByte : BlastUnit
    {
        public string Domain;
        public long Address;
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

        public override bool Apply()
        {
            if (!IsEnabled)
                return true;

            try
            {
                MemoryDomainProxy mdp = RTC_MemoryDomains.getProxyFromString(Domain);

				if (mdp == null)
					return true;

                switch (Type)
                {
                    case BlastByteType.SET:
                        mdp.PokeByte(Address, (byte)Value);
                        break;

                    case BlastByteType.ADD:
                        mdp.PokeByte(Address, (byte)(mdp.PeekByte(Address) + Value));
                        break;

                    case BlastByteType.SUBSTRACT:
                        mdp.PokeByte(Address, (byte)(mdp.PeekByte(Address) - Value));
                        break;

                    case BlastByteType.NONE:
                        return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("The BlastByte apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
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
                MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);

                if (md == null || Type == BlastByteType.NONE)
                    return null;

                return new BlastByte(Domain, Address, BlastByteType.SET, md.PeekByte(Address), true);

            }
            catch (Exception ex)
            {
                throw new Exception("The BlastByte GetBackup() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
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
		public string Domain;
		public long Address;
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

		public override bool Apply()
		{
			if (!IsEnabled)
				return true;

			try
			{
				MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);

				if (md == null)
					return true;
	
				md.PokeByte(Address, Values[0]);
				md.PokeByte(Address + 1, Values[1]);
				md.PokeByte(Address + 2, Values[2]);
				md.PokeByte(Address + 3, Values[3]);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector apply() function threw up. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
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
				MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);

				if (md == null)
					return null;

				return new BlastVector(Domain, Address, RTC_VectorEngine.read32bits(md, Address), true);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector GetBackup() function threw up. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
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
		public string Domain;
		public long Address;
		public string pipeDomain;
		public long PipeAddress;

		public override bool IsEnabled { get; set; }

		public BlastPipe(string _domain, long _address, string _pipeDomain, long _pipeAddress, bool _isEnabled)
		{
			Domain = _domain;
			Address = _address;
			pipeDomain = _pipeDomain;
			PipeAddress = _pipeAddress;
			IsEnabled = _isEnabled;
		}

		public BlastPipe()
		{

		}

		public void Execute()
		{
			try
			{
				MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);
				MemoryDomainProxy md2 = RTC_MemoryDomains.getProxyFromString(pipeDomain);

				if (md == null || md2 == null)
					throw new Exception($"Memory Domain error, MD1 -> {md.ToString()}, md2 -> {md2.ToString()}");

				md2.PokeByte(PipeAddress, md.PeekByte(Address));

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastPipe apply() function threw up. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
				ex.ToString());
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
				MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(pipeDomain);

				if (md == null)
					return null;

				return new BlastByte(pipeDomain, PipeAddress, BlastByteType.SET, md.PeekByte(PipeAddress), true);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastPipe GetBackup() function threw up. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
				ex.ToString());
			}

		}


		public override void Reroll()
		{
			//Pipes can't be rerolled. just do nothing
			//throw (new Exception("Reroll impossible on BlastPipe units"));
		}

		public override string ToString()
		{
			string EnabledString = "[ ] BlastPipe -> ";
			if (IsEnabled)
				EnabledString = "[x] BlastPipe -> ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			string cleanDomainName2 = pipeDomain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")piped->" + cleanDomainName2 + "(" + Convert.ToInt32(PipeAddress).ToString() + ")");
		}
	}


	[Serializable()]
    public class BlastCheat : BlastUnit
    {
        public string Domain;
        public long Address;
        public BizHawk.Client.Common.DisplayType displayType;
        public bool bigEndian;
        public int Value;
        public bool IsFreeze;
        WatchSize size;

		public override bool IsEnabled { get; set; }

		public BlastCheat(string _domain, long _address, BizHawk.Client.Common.DisplayType _displayType, bool _bigEndian, int _value, bool _isEnabled, bool _isFreeze)
        {
			//because of this, blastcheats can't be generated on standalone side.
            var settings = new RamSearchEngine.Settings(RTC_MemoryDomains.MDRI.MemoryDomains);

            Domain = _domain;
			size = settings.Size;

			Address = _address - (_address % (int)size);

            
            displayType = settings.Type;
            bigEndian = settings.BigEndian;

            Value = _value;
            IsEnabled = _isEnabled;
            IsFreeze = _isFreeze;
            
        }

		public BlastCheat()
		{
		}

        public override bool Apply()
        {
            try
            {
                if (!IsEnabled)
                    return true;

                MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);

				if (md == null)
					return true;

                string cheatName = "RTC Cheat|" + Domain + "|" + Address.ToString() + "|" + displayType.ToString() + "|" + bigEndian.ToString() + "|" + Value.ToString() + "|" + IsEnabled.ToString() + "|" + IsFreeze.ToString();

                if (!IsFreeze)
                {
                    Watch somewatch = Watch.GenerateWatch(md.md, Address, size, displayType, bigEndian, cheatName, Value, 0,0);
                    Cheat ch = new Cheat(somewatch, Value, null, true);
                    Global.CheatList.Add(ch);
                }
                else
                    RTC_MemoryDomains.FreezeAddress(Address, cheatName);

                
            }
            catch (Exception ex)
            {
                throw new Exception("The BlastCheat apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
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
            return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + displayType.ToString() + "(" + Value.ToString() + ")");
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

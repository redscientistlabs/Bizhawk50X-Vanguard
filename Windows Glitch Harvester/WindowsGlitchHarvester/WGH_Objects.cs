using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.IO;

namespace WindowsGlitchHarvester
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

        /*
        protected override void OnPaint(PaintEventArgs pevent)
        {

            base.OnPaint(pevent);
            
            int arrowX = ClientRectangle.Width - 14;
            int arrowY = ClientRectangle.Height / 2 - 1;

            Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            Point[] arrows = new Point[] { new Point(arrowX, arrowY), new Point(arrowX + 7, arrowY), new Point(arrowX + 3, arrowY + 4) };
            pevent.Graphics.FillPolygon(brush, arrows);
             
        }
        */
    }

    [Serializable()]
    public class Stockpile
    {
        public List<StashKey> stashkeys = new List<StashKey>();

        public string Filename;
        public string ShortFilename;
		public string WghVersion;

		public string descrip = "";

        public string Name;
        public string CloudCorruptID = null;

        public List<string> ComputerSerials = new List<string>();
        public List<string> MakersName = new List<string>();
        public List<string> MakersID = new List<string>();


        public Stockpile(ListBox lbStockpile)
        {
            foreach (StashKey sk in lbStockpile.Items)
            {
                stashkeys.Add(sk);
            }
        }

        public override string ToString()
        {
            return (Name != null ? Name : "");
        }


        public static void Save(Stockpile sks)
        {
            Stockpile.Save(sks, false);
        }

        public static void Save(Stockpile sks, bool IsQuickSave)
        {
            if (sks.stashkeys.Count == 0)
            {
                MessageBox.Show("Can't save because the Current Stockpile is empty");
                return;
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
                    return;
            }
            else
            {
                sks.Filename = WGH_Core.currentStockpile.Filename;
                sks.ShortFilename = WGH_Core.currentStockpile.ShortFilename;
            }

			//Watermarking WGH Version
			sks.WghVersion = WGH_Core.WghVersion;


			System.IO.FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            //creater master.sk to temp folder from stockpile object

			if(File.Exists(WGH_Core.currentDir + "\\TEMP\\master.sk"))
				FS = File.Open(WGH_Core.currentDir + "\\TEMP\\master.sk", FileMode.Open);
			else
				FS = File.Open(WGH_Core.currentDir + "\\TEMP\\master.sk", FileMode.Create);

			bformatter.Serialize(FS, sks);
            FS.Close();


            //7z the temp folder to destination filename
            string[] stringargs = { "-c", sks.Filename, WGH_Core.currentDir + "\\TEMP\\" };
            FastZipProgram.Exec(stringargs);

            Load(sks.Filename); //Reload file after for test and clean

        }

        public static void Load()
        {
            Load(null);
        }

        public static void Load(string Filename)
        {

            //clean temp folder
            foreach (string file in Directory.GetFiles(WGH_Core.currentDir + "\\TEMP"))
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

            string[] stringargs = { "-x", Filename, WGH_Core.currentDir + "\\TEMP\\" };

            FastZipProgram.Exec(stringargs);

            if (!File.Exists(WGH_Core.currentDir + "\\TEMP\\master.sk"))
            {
                MessageBox.Show("The file could not be read properly");
                return;
            }



            //stockpile part
            System.IO.FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            Stockpile sks;
            bformatter = new BinaryFormatter();

            try
            {
                FS = File.Open(WGH_Core.currentDir + "\\TEMP\\master.sk", FileMode.Open);
                sks = (Stockpile)bformatter.Deserialize(FS);
                FS.Close();
            }
            catch
            {
                MessageBox.Show("The Stockpile file could not be loaded");
                return;
            }

            WGH_Core.currentStockpile = sks;


            //fill list controls
            WGH_Core.ghForm.lbStockpile.Items.Clear();

            foreach (StashKey key in sks.stashkeys)
            {
                WGH_Core.ghForm.lbStockpile.Items.Add(key);
            }


            WGH_Core.ghForm.btnSaveStockpile.Enabled = true;
            WGH_Core.ghForm.btnSaveStockpile.BackColor = Color.Tomato;
            sks.Filename = Filename;

			if (sks.WghVersion != WGH_Core.WghVersion)
			{
				if (sks.WghVersion == null)
					MessageBox.Show("WARNING: You have loaded a pre-0.09 stockpile using WGH " + WGH_Core.WghVersion + "\n Items might not appear identical to how they when they were created.");
				else
					MessageBox.Show("WARNING: You have loaded a stockpile created with WGH " + sks.WghVersion + " using WGH " + WGH_Core.WghVersion + "\n Items might not appear identical to how they when they were created.");
			}

		}

        public static void Import()
        {
            Import(null, false);
        }

        public static void Import(string Filename, bool CorruptCloud)
        {

            //clean temp folder
            foreach (string file in Directory.GetFiles(WGH_Core.currentDir + "\\TEMP3"))
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

            string[] stringargs = { "-x", Filename, WGH_Core.currentDir + "\\TEMP3\\" };

            FastZipProgram.Exec(stringargs);

            if (!File.Exists(WGH_Core.currentDir + "\\TEMP3\\master.sk"))
            {
                MessageBox.Show("The file could not be read properly");
                return;
            }



            //stockpile part
            System.IO.FileStream FS;
            BinaryFormatter bformatter = new BinaryFormatter();

            Stockpile sks;
            bformatter = new BinaryFormatter();

            try
            {
                FS = File.Open(WGH_Core.currentDir + "\\TEMP3\\master.sk", FileMode.Open);
                sks = (Stockpile)bformatter.Deserialize(FS);
                FS.Close();
            }
            catch
            {
                MessageBox.Show("The Stockpile file could not be loaded");
                return;
            }


            //fill list controls

            foreach (StashKey key in sks.stashkeys)
            {
                WGH_Core.ghForm.lbStockpile.Items.Add(key);
            }

        }

    }



    [Serializable()]
    public class StashKey
    {

        public String TargetId;
        public String TargetType;
        public List<string> MemoryZones = new List<string>();
        public String TargetName;

        public String Key;
        public String ParentKey = null;
        public BlastLayer blastlayer = null;

        public String Alias
        {
            get
            {
                if (_Alias != null)
                    return _Alias;
                else
                    return Key;
            }
            set
            {
                _Alias = value;
            }
        }

        private String _Alias;

        public StashKey(String _key, BlastLayer _blastlayer)
        {

            Key = _key;
            blastlayer = _blastlayer;
            TargetId = WGH_Core.currentTargetName;
            TargetType = WGH_Core.currentTargetType;
            TargetName = WGH_Core.currentTargetName;

        }

        public override string ToString()
        {
            return Alias;
        }

        public bool Run()
        {
            WGH_Core.Blast(blastlayer);
            return (blastlayer.Layer.Count > 0);
        }

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

        public void Apply()
        {
            WGH_Core.lastBlastLayerBackup = GetBackup();

			foreach (BlastUnit bb in Layer)
				if (bb == null || !bb.Apply())
					return;
                
        }

        public BlastLayer GetBackup()
        {
            List<BlastUnit> BackupLayer = new List<BlastUnit>(); ;

			foreach (BlastUnit bb in Layer)
			{
				var bu = bb.GetBackup();
				if(bu != null)
					BackupLayer.Add(bu);
			}

            BlastLayer Recovery = new BlastLayer(BackupLayer);

            return Recovery;
        }

    }

    [Serializable()]
    public abstract class BlastUnit
    {
        public abstract bool Apply();
        public abstract BlastUnit GetBackup();
    }

    [Serializable()]
    public class BlastByte : BlastUnit
    {
        public string Domain;
        public long Address;
        public BlastByteType Type;
        public int Value;
        public bool IsEnabled;

        public BlastByte(string _domain, long _address, BlastByteType _type, int _value, bool _isEnabled)
        {
            Domain = _domain;
            Address = _address;
            Type = _type;
            Value = _value;
            IsEnabled = _isEnabled;
        }

        public override bool Apply()
        {
            if (!IsEnabled)
                return false;

            try
            {
                MemoryInterface mi = WGH_Core.currentMemoryInterface;

                if (mi == null)
                    return false;

                switch (Type)
                {
                    case BlastByteType.SET:
                        mi.PokeByte(Address, (byte)Value);
                        break;

                    case BlastByteType.ADD:
                        mi.PokeByte(Address, (byte)(mi.PeekByte(Address) + Value));
                        break;

                    case BlastByteType.SUBSTRACT:
                        mi.PokeByte(Address, (byte)(mi.PeekByte(Address) - Value));
                        break;

                    case BlastByteType.NONE:
                        return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("The BlastByte apply() function threw up. \n\n" +
                ex.ToString());
                return false;
            }

			return true;
        }

        public override BlastUnit GetBackup()
        {
            if (!IsEnabled)
                return null;

            try
            {
                MemoryInterface mi = WGH_Core.currentMemoryInterface;

                if (mi == null || Type == BlastByteType.NONE || mi is ProcessInterface)
                    return null;

                return new BlastByte(Domain, Address, BlastByteType.SET, (int)mi.PeekByte(Address), true);

            }
            catch (Exception ex)
            {
                MessageBox.Show("The BlastByte apply() function threw up. \n" +
                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                ex.ToString());
                return null;
            }

        }

        public override string ToString()
        {
            string EnabledString = "[ ] ";
            if (IsEnabled)
                EnabledString = "[x] ";

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


		public bool IsEnabled;

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
				MemoryInterface mi = WGH_Core.currentMemoryInterface;
				//MemoryDomainProxy md = RTC_MemoryDomains.getProxyFromString(Domain);

				if (mi == null)
					return true;

				mi.PokeByte(Address, Values[0]);
				mi.PokeByte(Address + 1, Values[1]);
				mi.PokeByte(Address + 2, Values[2]);
				mi.PokeByte(Address + 3, Values[3]);

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
				//MemoryInterface md = RTC_MemoryDomains.getProxyFromString(Domain);
				MemoryInterface mi = WGH_Core.currentMemoryInterface;

				if (mi == null)
					return null;

				return new BlastVector(Domain, Address, WGH_VectorEngine.read32bits(mi, Address), true);

			}
			catch (Exception ex)
			{
				throw new Exception("The BlastVector GetBackup() function threw up. \n" +
				"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
				ex.ToString());
			}

		}
		/*
		public override void Reroll()
		{
			Values = WGH_VectorEngine.getRandomConstant(WGH_VectorEngine.valueList);
		}
		*/
		public override string ToString()
		{
			string EnabledString = "[ ] ";
			if (IsEnabled)
				EnabledString = "[x] ";

			string cleanDomainName = Domain.Replace("(nametables)", ""); //Shortens the domain name if it contains "(nametables)"
			return (EnabledString + cleanDomainName + "(" + Convert.ToInt32(Address).ToString() + ")." + Type.ToString() + "(" + WGH_VectorEngine.ByteArrayToString(Values) + ")");
		}
	}


	[Serializable()]
    public abstract class MemoryInterface
    {
        public abstract byte[] getMemoryDump();
        public abstract byte[] lastMemoryDump { get; set; }
        public abstract long getMemorySize();
        public abstract long? lastMemorySize{ get; set; }

        public abstract void PokeByte(long address, byte data);
        public abstract void PokeBytes(long address, byte[] data);
        public abstract byte? PeekByte(long address);
        public abstract byte[] PeekBytes(long address, int range);

        public abstract void SetBackup();
        public abstract void ResetBackup(bool askConfirmation = true);
        public abstract void RestoreBackup(bool announce = true);
        public abstract void ResetWorkingFile();
        public abstract void ApplyWorkingFile();

    }

    [Serializable()]
    public class FileInterface : MemoryInterface
    {
        public string filename;
        public string shortFilename;
        bool writeDirectly = false;
        System.IO.Stream stream = null;

        public FileInterface(string _targetId)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                filename = targetId[1];
                shortFilename = filename.Substring(filename.LastIndexOf("\\") + 1, filename.Length - (filename.LastIndexOf("\\") + 1));

                SetBackup();

                //getMemoryDump();
                getMemorySize();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"FileInterface failed to load something \n\n" + "Culprit file: " + filename + "\n\n" + ex.ToString());

                if(WGH_Core.ghForm.rbTargetMultipleFiles.Checked)
                    throw;
            }
        }

        public string getCompositeFilename(string prefix)
        {
            return $"{prefix.Trim().ToUpper()}^{filename.ToBase64()}^{shortFilename}";
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            if(overrideWriteCopyMode || WGH_Core.writeCopyMode)
                return WGH_Core.currentDir + "\\TEMP\\" + getCompositeFilename("CORRUPT");
            else
                return filename;
        }
        public string getBackupFilename()
        {
            return WGH_Core.currentDir + "\\TEMP\\" + getCompositeFilename("BACKUP");
        }

        public override void ResetWorkingFile()
        {

        tryDeleteResetWorkingFileAgain:
            try
            {
                if (File.Exists(getCorruptFilename()))
                    File.Delete(getCorruptFilename());
            }
            catch
            {
                MessageBox.Show($"Could not get access to {getCorruptFilename()}\n\nClose the file then press OK", "WARNING");
                    goto tryDeleteResetWorkingFileAgain;
            }
            

            SetWorkingFile();
        }

        public string SetWorkingFile()
        {
            if (!File.Exists(getCorruptFilename()))
                File.Copy(getBackupFilename(), getCorruptFilename(), true);

            return getCorruptFilename();
        }

        public override void ApplyWorkingFile()
        {
            if(stream != null)
            {
                stream.Close();
                stream = null;
            }

            if(WGH_Core.writeCopyMode)
            {

            tryApplyWorkingFileAgain:
                try
                {
                    if (File.Exists(filename))
                        File.Delete(filename);

                    if (File.Exists(getCorruptFilename()))
                        File.Move(getCorruptFilename(), filename);
                }
                catch
                {
                    MessageBox.Show($"Could not get access to {filename} because some other program is probably using it. \n\nClose the file then press OK to try again", "WARNING");
                    goto tryApplyWorkingFileAgain;
                }

            }
        }

        public override void SetBackup()
        {
            if (!File.Exists(getBackupFilename()))
                File.Copy(filename, getBackupFilename(), true);
        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target file?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            if (File.Exists(getBackupFilename()))
                File.Delete(getBackupFilename());

            SetBackup();

        }

        public override void RestoreBackup(bool announce = true)
        {

            if (File.Exists(getBackupFilename()))
            {
                File.Delete(filename);
                File.Copy(getBackupFilename(), filename, true);

                if (announce)
                    MessageBox.Show("Backup of " + shortFilename + " was restored");
            }
            else
            {
                MessageBox.Show("Couldn't find backup of " + shortFilename);
            }
        }

        public override byte[] getMemoryDump()
        {
            lastMemoryDump = File.ReadAllBytes(getBackupFilename());
            return lastMemoryDump.ToArray();
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            if (lastMemorySize != null)
                return (long)lastMemorySize;

            lastMemorySize = new FileInfo(filename).Length;
            return (long)lastMemorySize;
            
        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {
            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open);

            stream.Position = address;
            stream.Write(data, 0, data.Length);

            if (lastMemoryDump != null)
            for (int i = 0; i<data.Length; i++)
                lastMemoryDump[address + i] = data[i];

        }

        public override void PokeByte(long address, byte data)
        {
            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open);

            stream.Position = address;
            stream.WriteByte(data);

            if (lastMemoryDump != null)
                lastMemoryDump[address] = data;
        }

        public override byte? PeekByte(long address)
        {

            if (lastMemoryDump != null)
                return lastMemoryDump[address];

            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open);

            byte[] readBytes = new byte[1];
            stream.Position = address;
            stream.Read(readBytes, 0, 1);

            //fs.Close();

            return readBytes[0];

        }

        public override byte[] PeekBytes(long address, int range)
        {

            if (lastMemoryDump != null)
                return lastMemoryDump.SubArray(address, range);

            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open);

            byte[] readBytes = new byte[range];
            stream.Position = address;
            stream.Read(readBytes, 0, range);

            //fs.Close();

            return readBytes;


        }

    }


    [Serializable()]
    public class MultipleFileInterface : MemoryInterface
    {
        public string filename;
        public string shortFilename;
        //bool writeDirectly = false;
        //Stream stream = null;

        public List<FileInterface> fileInterfaces = new List<FileInterface>();

        public MultipleFileInterface(string _targetId)
        {
            try
            {
                string[] targetId = _targetId.Split('|');

                for (int i = 0; i < targetId.Length; i++)
                    fileInterfaces.Add(new FileInterface("File|" + targetId[i]));

                filename = "MultipleFiles";
                shortFilename = "MultipleFiles";

                //SetBackup();

                //getMemoryDump();
                getMemorySize();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"MultipleFileInterface failed to load something \n\n" + ex.ToString());
            }
        }

        public string getCompositeFilename(string prefix)
        {
            return string.Join("|", fileInterfaces.Select(it => it.getCompositeFilename(prefix)));

        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            return string.Join("|", fileInterfaces.Select(it => it.getCorruptFilename(overrideWriteCopyMode)));

        }

        public string getBackupFilename()
        {
            return string.Join("|", fileInterfaces.Select(it => it.getBackupFilename()));
        }

        public override void ResetWorkingFile()
        {
            foreach (var fi in fileInterfaces)
                fi.ResetWorkingFile();

        }

        public string SetWorkingFile()
        {
            return string.Join("|", fileInterfaces.Select(it => it.SetWorkingFile()));

        }

        public override void ApplyWorkingFile()
        {
            foreach (var fi in fileInterfaces)
                fi.ApplyWorkingFile();

        }

        public override void SetBackup()
        {
            foreach (var fi in fileInterfaces)
                fi.SetBackup();

        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target files?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            foreach (var fi in fileInterfaces)
                fi.ResetBackup(false);

        }

        public override void RestoreBackup(bool announce = true)
        {

            foreach (var fi in fileInterfaces)
                fi.RestoreBackup(false);

            if(announce)
                MessageBox.Show("Backups of " + string.Join(",",fileInterfaces.Select(it => (it as FileInterface).shortFilename)) + " were restored");

        }

        public override byte[] getMemoryDump()
        {

            List<byte> allBytes = new List<byte>();

            foreach(var fi in fileInterfaces)
                allBytes.AddRange(fi.getMemoryDump());

            lastMemoryDump = allBytes.ToArray();
            return lastMemoryDump;

        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            long size = 0;

            foreach (var fi in fileInterfaces)
                size += fi.getMemorySize();

            lastMemorySize = size;
            return (long)lastMemorySize;

        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {
            long addressPad = 0;
            FileInterface targetInterface = null;

            foreach(var fi in fileInterfaces)
            {
                if (addressPad + fi.getMemorySize() > address)
                {
                    targetInterface = fi;
                    break;
                }

                addressPad += fi.getMemorySize();
                
            }

            if (targetInterface != null)
                targetInterface.PokeBytes(address - addressPad, data);

            if (lastMemoryDump != null)
                for (int i = 0; i < data.Length; i++)
                    lastMemoryDump[address + i] = data[i];

        }

        public override void PokeByte(long address, byte data)
        {

            long addressPad = 0;
            FileInterface targetInterface = null;

            foreach (var fi in fileInterfaces)
            {
                if (addressPad + fi.getMemorySize() > address)
                {
                    targetInterface = fi;
                    break;
                }

                addressPad += fi.getMemorySize();
                
            }

            if (targetInterface != null)
                targetInterface.PokeByte(address - addressPad, data);

            if (lastMemoryDump != null)
                lastMemoryDump[address] = data;
        }

        public override byte? PeekByte(long address)
        {

            if (lastMemoryDump != null)
                return lastMemoryDump[address];

            long addressPad = 0;
            FileInterface targetInterface = null;

            foreach (var fi in fileInterfaces)
            {
                if (addressPad + fi.getMemorySize() > address)
                {
                    targetInterface = fi;
                    break;
                }

                addressPad += fi.getMemorySize();

            }

            if (targetInterface != null)
                return targetInterface.PeekByte(address - addressPad);
            else
                return null;


        }

        public override byte[] PeekBytes(long address, int range)
        {
            
            if (lastMemoryDump != null)
                return lastMemoryDump.SubArray(address, range);
            

            long addressPad = 0;
            FileInterface targetInterface = null;

            foreach (var fi in fileInterfaces)
            {
                if (addressPad + fi.getMemorySize() > address)
                {
                    targetInterface = fi;
                    break;
                }

                addressPad += fi.getMemorySize();

            }

            if (targetInterface != null)
                return targetInterface.PeekBytes(address - addressPad,range);
            else
                return null;

        }

    }


    [Serializable()]
    public class ProcessInterface : MemoryInterface
    {
        public string processName;
		ProcessHijacker hijack = null;
		public bool Hooked;

		public bool useCaching = false;

        public ProcessInterface(string _processName)
        {
			hijack = new ProcessHijacker(_processName);
			Hooked = hijack.Hooked;
			processName = _processName;

            //getMemoryDump();
            getMemorySize();
        }

        public override byte[] getMemoryDump()
        {
			lastMemoryDump = hijack.ReadAllData();
			return lastMemoryDump;
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            if (hijack == null)
                return 0;

			hijack.refreshProcessSize();
			lastMemorySize = hijack.processSize;

            return (long)lastMemorySize;

        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {

            //HOOK CHEATENGINE API HERE
            /*
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                stream.Position = address;
                stream.Write(data, 0, data.Length);
            }
            */

           // if (lastMemoryDump == null)
           //     getMemoryDump();

            if (hijack == null)
                return;

			hijack.WriteBytes(data, address);

            //for (int i = 0; i < data.Length; i++)
           // {
              //  lastMemoryDump[address + i] = data[i];
            //}


        }

        public override void PokeByte(long address, byte data)
        {

            //HOOK CHEATENGINE API HERE
            /*
            using (Stream stream = File.Open(filename, FileMode.Open))
            {
                stream.Position = address;
                stream.WriteByte(data);
            }
            */

            //if (hijack == null)
                //getMemoryDump();

            if (hijack == null)
                return;

			hijack.WriteByte(data, address);
        }

        public override byte? PeekByte(long address)
        {
            if (hijack == null)
                return null;

			if (useCaching)
			{
				if (lastMemoryDump == null)
					getMemoryDump();

				if (lastMemoryDump == null)
					return null;

				return lastMemoryDump[address];
			}
			else
				return hijack.ReadByte(address);
		}

        public override byte[] PeekBytes(long address, int range)
        {
			if (hijack == null)
				return null;

			if(useCaching)
			{
				if (lastMemoryDump == null)
					getMemoryDump();

				if (lastMemoryDump == null)
					return null;

				return lastMemoryDump.SubArray(address, range);
			}
			else
				return hijack.ReadBytes(address, range);
		}

        public override void SetBackup()
        {
            //CAN'T DO THAT WITH PROCESSES
        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            //CAN'T DO THAT WITH PROCESSES
        }

        public override void RestoreBackup(bool announce = true)
        {
            //CAN'T DO THAT WITH PROCESSES
        }

        public override void ResetWorkingFile()
        {
            //CAN'T DO THAT WITH PROCESSES
        }

        public override void ApplyWorkingFile()
        {
            //CAN'T DO THAT WITH PROCESSES
        }

		public void RefreshSize()
		{
			getMemorySize();
		}
	}
}

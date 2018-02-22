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
        public List<StashKey> StashKeys = new List<StashKey>();

        public string Filename;
        public string ShortFilename;
		public string WghVersion;

        public Stockpile(ListBox lbStockpile)
        {
            foreach (StashKey sk in lbStockpile.Items)
            {
                StashKeys.Add(sk);
            }
        }

        public static void Save(Stockpile sks)
        {
            Stockpile.Save(sks, false);
        }

        public static void Save(Stockpile sks, bool IsQuickSave)
        {
            if (sks.StashKeys.Count == 0)
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

			if(File.Exists(WGH_Core.currentDir + "\\TEMP2\\master.sk"))
				FS = File.Open(WGH_Core.currentDir + "\\TEMP2\\master.sk", FileMode.Open);
			else
				FS = File.Open(WGH_Core.currentDir + "\\TEMP2\\master.sk", FileMode.Create);

			bformatter.Serialize(FS, sks);
            FS.Close();


            //7z the temp folder to destination filename
            string[] stringargs = { "-c", sks.Filename, WGH_Core.currentDir + "\\TEMP2\\" };
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
            foreach (string file in Directory.GetFiles(WGH_Core.currentDir + "\\TEMP2"))
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

            string[] stringargs = { "-x", Filename, WGH_Core.currentDir + "\\TEMP2\\" };

            FastZipProgram.Exec(stringargs);

            if (!File.Exists(WGH_Core.currentDir + "\\TEMP2\\master.sk"))
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
                FS = File.Open(WGH_Core.currentDir + "\\TEMP2\\master.sk", FileMode.Open);
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

            foreach (StashKey key in sks.StashKeys)
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
            foreach (string file in Directory.GetFiles(WGH_Core.currentDir + "\\TEMP2"))
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

            foreach (StashKey key in sks.StashKeys)
            {
                WGH_Core.ghForm.lbStockpile.Items.Add(key);
            }

        }

    }



    [Serializable()]
    public class StashKey : ICloneable
    {

        public String TargetId;
        public String TargetType;
        public List<string> MemoryZones = new List<string>();
        public String TargetName;

        public String Key;
        public String ParentKey = null;
        public BlastLayer BlastLayer = null;

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
            BlastLayer = _blastlayer;
            TargetId = WGH_Core.currentTargetName;
            TargetType = WGH_Core.currentTargetType;
            TargetName = WGH_Core.currentTargetName;

        }

        public override string ToString()
        {
            return Alias;
        }

        public object Clone()
        {
            return ObjectCopier.Clone(this);
        }

        public bool Run()
        {
            WGH_Core.Blast(BlastLayer);
            return (BlastLayer.Layer.Count > 0);
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
        long vectorOffset = WGH_VectorEngine.vectorOffset;
		public byte[] Values;


		public override bool IsEnabled { get; set; }

		public BlastVector(string _domain, long _address, byte[] _values, bool _isEnabled)
		{
			Domain = _domain;
            if (WGH_VectorEngine.vectorAligned)
                Address = ((_address - (_address % 4)) + vectorOffset);
            else
               Address = (_address + vectorOffset);
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

    interface ICachable
    {
        

    }


    [Serializable()]
    public abstract class MemoryInterface
    {
        public abstract byte[] getMemoryDump();
        public abstract bool isDolphinSavestate();
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

        public System.IO.Stream stream = null;
    }

    [Serializable()]
    public class FileInterface : MemoryInterface
    {
        public string Filename;
        public string ShortFilename;


        public FileInterface(string _targetId)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                Filename = targetId[1];
                ShortFilename = Filename.Substring(Filename.LastIndexOf("\\") + 1, Filename.Length - (Filename.LastIndexOf("\\") + 1));

                SetBackup();

                //getMemoryDump();
                getMemorySize();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"FileInterface failed to load something \n\n" + "Culprit file: " + Filename + "\n\n" + ex.ToString());

                if(WGH_Core.ghForm.rbTargetMultipleFiles.Checked)
                    throw;
            }
        }

        public string getCompositeFilename(string prefix)
        {
            return $"{prefix.Trim().ToUpper()}^{Filename.ToBase64()}^{ShortFilename}";
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            if(overrideWriteCopyMode || WGH_Core.writeCopyMode)
                return WGH_Core.currentDir + "\\TEMP\\" + getCompositeFilename("CORRUPT");
            else
                return Filename;
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

                try
                {
                    if (File.Exists(Filename))
                        File.Delete(Filename);

                    if (File.Exists(getCorruptFilename()))
                        File.Move(getCorruptFilename(), Filename);
                }
                catch
                {
                    MessageBox.Show($"Could not get access to {Filename} because some other program is probably using it. \n\nClose the file then press OK to try again", "WARNING");
                }

            }
        }

        public override void SetBackup()
        {
            if (!File.Exists(getBackupFilename()))
                File.Copy(Filename, getBackupFilename(), true);
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
                File.Delete(Filename);
                File.Copy(getBackupFilename(), Filename, true);

                if (announce)
                    MessageBox.Show("Backup of " + ShortFilename + " was restored");
            }
            else
            {
                MessageBox.Show("Couldn't find backup of " + ShortFilename);
            }
        }

        public override byte[] getMemoryDump()
        {
            lastMemoryDump = File.ReadAllBytes(getBackupFilename());
            return lastMemoryDump;
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            if (lastMemorySize != null)
                return (long)lastMemorySize;

            lastMemorySize = new FileInfo(Filename).Length;
            return (long)lastMemorySize;
            
        }

        public override bool isDolphinSavestate()
        {

            string a = "Dolphin Narry's Mod";
            string b  = Encoding.Default.GetString(PeekBytes(32, 19));

            if (a == b)
                return true;
            else
                return false;
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
        public string Filename;
        public string ShortFilename;

        public List<FileInterface> FileInterfaces = new List<FileInterface>();

        public MultipleFileInterface(string _targetId)
        {
            try
            {
                string[] targetId = _targetId.Split('|');

                for (int i = 0; i < targetId.Length; i++)
                    FileInterfaces.Add(new FileInterface("File|" + targetId[i]));

                Filename = "MultipleFiles";
                ShortFilename = "MultipleFiles";

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
            return string.Join("|", FileInterfaces.Select(it => it.getCompositeFilename(prefix)));

        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            return string.Join("|", FileInterfaces.Select(it => it.getCorruptFilename(overrideWriteCopyMode)));

        }

        public string getBackupFilename()
        {
            return string.Join("|", FileInterfaces.Select(it => it.getBackupFilename()));
        }

        public override void ResetWorkingFile()
        {
            foreach (var fi in FileInterfaces)
                fi.ResetWorkingFile();

        }

        public string SetWorkingFile()
        {
            return string.Join("|", FileInterfaces.Select(it => it.SetWorkingFile()));

        }

        public override void ApplyWorkingFile()
        {
            foreach (var fi in FileInterfaces)
                fi.ApplyWorkingFile();

        }

        public override void SetBackup()
        {
            foreach (var fi in FileInterfaces)
                fi.SetBackup();

        }

        public override void ResetBackup(bool askConfirmation = true)
        {
            if (askConfirmation && MessageBox.Show("Are you sure you want to reset the backup using the target files?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            foreach (var fi in FileInterfaces)
                fi.ResetBackup(false);

        }

        public override void RestoreBackup(bool announce = true)
        {

            foreach (var fi in FileInterfaces)
                fi.RestoreBackup(false);

            if(announce)
                MessageBox.Show("Backups of " + string.Join(",",FileInterfaces.Select(it => (it as FileInterface).ShortFilename)) + " were restored");

        }

        public override byte[] getMemoryDump()
        {

            List<byte> allBytes = new List<byte>();

            foreach(var fi in FileInterfaces)
                allBytes.AddRange(fi.getMemoryDump());

            lastMemoryDump = allBytes.ToArray();
            return lastMemoryDump;

        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            long size = 0;

            foreach (var fi in FileInterfaces)
                size += fi.getMemorySize();

            lastMemorySize = size;
            return (long)lastMemorySize;

        }

        public override bool isDolphinSavestate()
        {
            //Not supported for multiple files
            return false;
        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {
            long addressPad = 0;
            FileInterface targetInterface = null;

            foreach(var fi in FileInterfaces)
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

            foreach (var fi in FileInterfaces)
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

            foreach (var fi in FileInterfaces)
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

            foreach (var fi in FileInterfaces)
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
        public string ProcessName;
		ProcessHijacker Hijack = null;

		public bool Hooked;
		public bool UseCaching = false;

        public ProcessInterface(string _processName)
        {
			Hijack = new ProcessHijacker(_processName);
			Hooked = Hijack.Hooked;
			ProcessName = _processName;

            //getMemoryDump();
            getMemorySize();
        }

        public override byte[] getMemoryDump()
        {
			lastMemoryDump = Hijack.ReadAllData();
			return lastMemoryDump;
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            if (Hijack == null)
                return 0;

			Hijack.refreshProcessSize();
			lastMemorySize = Hijack.processSize;

            return (long)lastMemorySize;

        }

        public override bool isDolphinSavestate()
        {  
            //Not applicable for processs corruption
            return false;
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

            if (Hijack == null)
                return;

			Hijack.WriteBytes(data, address);

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

            if (Hijack == null)
                return;

			Hijack.WriteByte(data, address);
        }

        public override byte? PeekByte(long address)
        {
            if (Hijack == null)
                return null;

			if (UseCaching)
			{
				if (lastMemoryDump == null)
					getMemoryDump();

				if (lastMemoryDump == null)
					return null;

				return lastMemoryDump[address];
			}
			else
				return Hijack.ReadByte(address);
		}

        public override byte[] PeekBytes(long address, int range)
        {
			if (Hijack == null)
				return null;

			if(UseCaching)
			{
				if (lastMemoryDump == null)
					getMemoryDump();

				if (lastMemoryDump == null)
					return null;

				return lastMemoryDump.SubArray(address, range);
			}
			else
				return Hijack.ReadBytes(address, range);
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



    [Serializable()]
    public class DolphinInterface : MemoryInterface
    {
        public string Filename;
        public string ShortFilename;


        public DolphinInterface(string _targetId)
        {
            try
            {
                string[] targetId = _targetId.Split('|');
                Filename = targetId[1];
                ShortFilename = Filename.Substring(Filename.LastIndexOf("\\") + 1, Filename.Length - (Filename.LastIndexOf("\\") + 1));

                SetBackup();

                //getMemoryDump();
                getMemorySize();


            }
            catch (Exception ex)
            {
                MessageBox.Show($"DolphinInterface failed to load something \n\n" + "Culprit file: " + Filename + "\n\n" + ex.ToString());

                if (WGH_Core.ghForm.rbTargetMultipleFiles.Checked)
                    throw;
            }
        }

        public string getCompositeFilename(string prefix)
        {
            return $"{prefix.Trim().ToUpper()}^{Filename.ToBase64()}^{ShortFilename}";
        }

        public string getCorruptFilename(bool overrideWriteCopyMode = false)
        {
            if (overrideWriteCopyMode || WGH_Core.writeCopyMode)
                return WGH_Core.currentDir + "\\TEMP\\" + getCompositeFilename("CORRUPT");
            else
                return Filename;
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
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }

            if (WGH_Core.writeCopyMode)
            {

                tryApplyWorkingFileAgain:
                try
                {
                    if (File.Exists(Filename))
                        File.Delete(Filename);

                    if (File.Exists(getCorruptFilename()))
                        File.Move(getCorruptFilename(), Filename);
                }
                catch
                {
                    MessageBox.Show($"Could not get access to {Filename} because some other program is probably using it. \n\nClose the file then press OK to try again", "WARNING");
                    goto tryApplyWorkingFileAgain;
                }

            }
        }

        public override void SetBackup()
        {
            if (!File.Exists(getBackupFilename()))
                File.Copy(Filename, getBackupFilename(), true);
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
                File.Delete(Filename);
                File.Copy(getBackupFilename(), Filename, true);

                if (announce)
                    MessageBox.Show("Backup of " + ShortFilename + " was restored");
            }
            else
            {
                MessageBox.Show("Couldn't find backup of " + ShortFilename);
            }
        }

        public override byte[] getMemoryDump()
        {
            lastMemoryDump = File.ReadAllBytes(getBackupFilename());
            return lastMemoryDump;
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            if (lastMemorySize != null)
                return (long)lastMemorySize;

            lastMemorySize = new FileInfo(Filename).Length;
            return (long)lastMemorySize;

        }

        public override bool isDolphinSavestate()
        {

            string a = "Dolphin Narry's Mod";
            string b = Encoding.Default.GetString(PeekBytes(32, 19));

            if (a == b)
                return true;
            else
                return false;
        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {
            if (stream == null)
                stream = File.Open(SetWorkingFile(), FileMode.Open);

            stream.Position = address;
            stream.Write(data, 0, data.Length);

            if (lastMemoryDump != null)
                for (int i = 0; i < data.Length; i++)
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
    //Actual dolphinInterface saved just in case
    /*
    [Serializable()]
    public class DolphinInterface : MemoryInterface , ICachable
    {
        public string ProcessName;
        ProcessHijacker Hijack = null;

        public bool Hooked;
        public bool UseCaching = false;

        public DolphinInterface(string _processName)
        {
            getMemorySize();
        }

        public override byte[] getMemoryDump()
        {
            lastMemoryDump = null;
            return lastMemoryDump;
        }
        public override byte[] lastMemoryDump { get; set; } = null;

        public override long getMemorySize()
        {
            lastMemorySize = WGH_SavestateInfoForm.getMemorySize();
            return WGH_SavestateInfoForm.getMemorySize();
        }

        public override bool isDolphinSavestate()
        {
            return false;
        }

        public override long? lastMemorySize { get; set; }

        public override void PokeBytes(long address, byte[] data)
        {
            WGH_Core.ssForm.PokeBytes(address, data);
        }

        public override void PokeByte(long address, byte data)
        {
            WGH_Core.ssForm.PokeByte(address, data);
        }

        public override byte? PeekByte(long address)
        {
            return WGH_Core.ssForm.PeekByte(address);
        }

        public override byte[] PeekBytes(long address, int range)
        {
            return WGH_Core.ssForm.PeekBytes(address, range);
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
        }*/
}

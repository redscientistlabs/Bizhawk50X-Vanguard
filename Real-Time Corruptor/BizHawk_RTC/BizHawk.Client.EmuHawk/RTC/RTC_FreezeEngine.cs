using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizHawk.Client.Common;
using BizHawk.Emulation.Common;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using BizHawk.Client.EmuHawk;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace RTC
{
    public static class RTC_FreezeEngine
    {
        public static bool FirstInit = false;

        public static bool _activeTableReady = false;
        public static bool ActiveTableReady
        {
            get
            {
                return _activeTableReady;
            }
            set
            {
                if (value)
                {
                    RTC_Core.coreForm.lbFreezeEngineActiveStatus.Text = "Active table status: READY";
                    RTC_Core.coreForm.btnActiveTableSubstractFile.ForeColor = Color.MediumPurple;
                    RTC_Core.coreForm.btnActiveTableSaveAs.ForeColor = Color.OrangeRed;
                }
                else
                {
                    RTC_Core.coreForm.lbFreezeEngineActiveStatus.Text = "Active table status: NOT READY";
                    RTC_Core.coreForm.btnActiveTableSubstractFile.ForeColor = Color.Silver;
                    RTC_Core.coreForm.btnActiveTableSaveAs.ForeColor = Color.Silver;
                    
                }

                if (ActiveTableGenerated != null && ActiveTableGenerated.Length > 0)
                    _activeTableReady = value;
                else
                    _activeTableReady = false;
            }
        }


        public static bool UseActiveTable = false;
        public static List<string> ActiveTableDumps = null;
        public static long[] ActiveTableActivity = null;
        public static long[] ActiveTableGenerated = null;
        public static double ActivityThreshold = 0;
        public static Timer ActiveTableAutodump = null;

        public static string currentFilename{
            get { return _currentFilename; }
            set {
                if (value == null)
                {
                    RTC_Core.coreForm.btnActiveTableQuickSave.ForeColor = Color.Silver;
                }
                else
                {
                    RTC_Core.coreForm.btnActiveTableQuickSave.ForeColor = Color.OrangeRed;
                }
                _currentFilename = value;
            }
        }
        public static string _currentFilename = null;

        public static void LoadActiveTable()
        {
			RTC_Core.StopSound();

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.DefaultExt = "act";
            OpenFileDialog1.Title = "Open ActiveTable File";
            OpenFileDialog1.Filter = "ACT files|*.act";
            OpenFileDialog1.RestoreDirectory = true;
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFilename = OpenFileDialog1.FileName.ToString();
            }
            else
            {
				RTC_Core.StartSound();
                return;
            }

            FileStream FS;
			//BinaryFormatter bformatter = new BinaryFormatter();
			XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
            FS = File.Open(currentFilename, FileMode.OpenOrCreate);
			//ActiveTableObject act = (ActiveTableObject)bformatter.Deserialize(FS);
			ActiveTableObject act = (ActiveTableObject)xs.Deserialize(FS);
			FS.Close();

            SetActiveTable(act);


			RTC_Core.StartSound();
        }

        public static void SaveActiveTable()
        {
            SaveActiveTable(true);
        }

        public static void SaveActiveTable(bool IsQuickSave)
        {
			RTC_Core.StopSound();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            if (!IsQuickSave)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.DefaultExt = "act";
                saveFileDialog1.Title = "Save ActiveTable File";
                saveFileDialog1.Filter = "ACT files|*.act";
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFilename = saveFileDialog1.FileName;
                    //sks.ShortFilename = sks.Filename.Substring(sks.Filename.LastIndexOf("\\") + 1, sks.Filename.Length - (sks.Filename.LastIndexOf("\\") + 1));
                }
                else
                {
					RTC_Core.StartSound();
                    return;
                }
            }

            ActiveTableObject act = new ActiveTableObject(ActiveTableGenerated);

            FileStream FS;
			//BinaryFormatter bformatter = new BinaryFormatter();
			XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
            FS = File.Open(currentFilename, FileMode.OpenOrCreate);
			//bformatter.Serialize(FS, act);
			xs.Serialize(FS, act);
			FS.Close();

            GC.Collect();
            GC.WaitForPendingFinalizers();

			RTC_Core.StartSound();
        }

        public static void SubstractActiveTable()
        {
			RTC_Core.StopSound();

            string tempFilename;

            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.DefaultExt = "act";
            OpenFileDialog1.Title = "Open ActiveTable File";
            OpenFileDialog1.Filter = "ACT files|*.act";
            OpenFileDialog1.RestoreDirectory = true;
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tempFilename = OpenFileDialog1.FileName.ToString();
            }
            else
                return;

            FileStream FS;
			//BinaryFormatter bformatter = new BinaryFormatter();
			XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
            FS = File.Open(tempFilename, FileMode.OpenOrCreate);
            //ActiveTableObject act = (ActiveTableObject)bformatter.Deserialize(FS);
			ActiveTableObject act = (ActiveTableObject)xs.Deserialize(FS);
			FS.Close();

            long[] substractiveActiveTable = act.data;

            List<long> newActiveTable = new List<long>();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            foreach (long item in ActiveTableGenerated)
                if (!substractiveActiveTable.Contains(item))
                    newActiveTable.Add(item);


            ActiveTableGenerated = newActiveTable.ToArray();
            RTC_Core.coreForm.lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();

            GC.Collect();
            GC.WaitForPendingFinalizers();

			RTC_Core.StartSound();
        }


        public static void SetActiveTable(ActiveTableObject act)
        {
            FirstInit = true;
            ActiveTableGenerated = act.data;
            ActiveTableReady = true;
            RTC_Core.coreForm.lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();
            RTC_Core.coreForm.cbFreezeEngineActive.Checked = true;
        }

        public static void ResetActiveTable(){

            if (!FirstInit)
            {
                FirstInit = true;
                RTC_Core.coreForm.btnActiveTableAddDump.ForeColor = Color.FromArgb(192, 255, 192);
                RTC_Core.coreForm.cbAutoAddDump.Enabled = true;
                RTC_Core.coreForm.btnActiveTableGenerate.ForeColor = Color.OrangeRed;
            }

            RTC_Core.coreForm.lbFreezeEngineDomainAddressSize.Text = "Domain size: " + RTC_MemoryDomains.getInterface(RTC_MemoryDomains._domain).Size.ToString();
            RTC_Core.coreForm.lbFreezeEngineNbDumps.Text = "Memory dumps collected: 0";
            RTC_Core.coreForm.lbFreezeEngineActiveTableSize.Text = "Active table size: 0";
            ActiveTableReady = false;

            ActiveTableGenerated = null;

            ActiveTableDumps = new List<string>();

            foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\MEMORYDUMPS"))
                File.Delete(file);

            currentFilename = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void AddDump()
        {
            MemoryStream ms = new MemoryStream();


            if (ActiveTableDumps == null)
                return;

            List<byte> newDump = new List<byte>();
            for (long i = 0; i < RTC_MemoryDomains.getInterface(RTC_MemoryDomains._domain).Size; i++)
            {
				newDump.Add((byte)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_PEEKBYTE) {
					objectValue = new object[] { RTC_MemoryDomains._domain.ToString(), i }
				}, true));
				//newDump.Add(RTC_MemoryDomains._domain.PeekByte(i));
			}

            string key = RTC_Core.GetRandomKey();
            File.WriteAllBytes(RTC_Core.rtcDir + "\\MEMORYDUMPS\\" + key + ".dmp", newDump.ToArray());
            ActiveTableDumps.Add(key);

            RTC_Core.coreForm.lbFreezeEngineNbDumps.Text = "Memory dumps collected: " + ActiveTableDumps.Count.ToString();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static byte[] GetDumpFromFile(string key)
        {
            return File.ReadAllBytes(RTC_Core.rtcDir + "\\MEMORYDUMPS\\" + key + ".dmp");
        }

        public static void GenerateActiveTable()
        {
            if(!ComputeActiveTableActivity())
                return; //exit generation if activity computation failed

            List<long> newActiveTable = new List<long>();
            double computedThreshold = (double)ActiveTableDumps.Count * (ActivityThreshold / 100d) + 1d;
            bool ExcludeEverchanging = RTC_Core.coreForm.cbActiveTableExclude100percent.Checked;


            for (int i = 0; i < ActiveTableActivity.Length; i++)
            {
                if ((double)ActiveTableActivity[i] > computedThreshold && (!ExcludeEverchanging || ActiveTableActivity[i] != (long)ActiveTableDumps.Count))
                    newActiveTable.Add(i);
            }

            long[] tempActiveTable = newActiveTable.ToArray();


            if (RTC_Core.coreForm.cbActiveTableCapSize.Checked && RTC_Core.coreForm.nmActiveTableCapSize.Value < tempActiveTable.Length)
                ActiveTableGenerated = CapActiveTable(tempActiveTable);
            else
                ActiveTableGenerated = tempActiveTable;

            RTC_Core.coreForm.lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();
            RTC_Core.coreForm.cbFreezeEngineActive.Checked = true;

            ActiveTableReady = true;
            currentFilename = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        public static long[] CapActiveTable(long[] tempActiveTable)
        {
            List<long> cappedActiveTable = new List<long>();

            int CapSize = Convert.ToInt32(RTC_Core.coreForm.nmActiveTableCapSize.Value);
            int Offset = Convert.ToInt32(RTC_Core.coreForm.nmActiveTableCapOffset.Value);
            bool DuplicateFound = true;

            if(RTC_Core.coreForm.rbActiveTableCapRandom.Checked)
            {

                for (int i = 0; i < CapSize; i++)
                {
                    DuplicateFound = true;

                    while (DuplicateFound)
                    {
                        long queryAdress = tempActiveTable[RTC_Core.RandomLong(tempActiveTable.Length -1)];

                        if (!cappedActiveTable.Contains(queryAdress))
                        {
                            DuplicateFound = false;
                            cappedActiveTable.Add(queryAdress);
                        }
                        else
                            DuplicateFound = true;
                    }
                }

            }
            else if(RTC_Core.coreForm.rbActiveTableCapBlockStart.Checked)
            {
                for (int i = 0; i < CapSize || i + Offset >= tempActiveTable.Length; i++)
                {
                    cappedActiveTable.Add(tempActiveTable[i + Offset]);
                }
            }
            else if(RTC_Core.coreForm.rbActiveTableCapBlockEnd.Checked)
            {
                for (int i = 0; i < CapSize || (tempActiveTable.Length - 1) - (i + Offset) < 0; i++)
                {
                    cappedActiveTable.Add(tempActiveTable[(tempActiveTable.Length - 1) - (i + Offset)]);
                }
            }

            return cappedActiveTable.ToArray();
        }

        public static bool ComputeActiveTableActivity()
        {
            if(ActiveTableDumps.Count < 2)
            {
                MessageBox.Show("Not enough dumps for generation");
                return false;
            }

            List<long> newActiveTableActivity = new List<long>();

			long domainSize = (long)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_GETSIZE) { objectValue = RTC_MemoryDomains._domain }, true);

			for (long i = 0; i < RTC_MemoryDomains.getInterface(RTC_MemoryDomains._domain).Size; i++)
            {
                newActiveTableActivity.Add(0);
            }

            ActiveTableActivity = newActiveTableActivity.ToArray();

            byte[] lastDump = null;

            for(int i = 0; i<ActiveTableDumps.Count; i++)
            {

                if (i == 0)
                {
                    lastDump = GetDumpFromFile(ActiveTableDumps[i]);
                    continue;
                }

                byte[] currentDump = GetDumpFromFile(ActiveTableDumps[i]);

                for (int j = 0; j < ActiveTableActivity.Length; j++)
                {
                    if (lastDump[j] != currentDump[j])
                        ActiveTableActivity[j]++;
                }

            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }

        public static long GetAdressFromActiveTable()
        {
            if (_activeTableReady)
            {
                return ActiveTableGenerated[RTC_Core.RND.Next(ActiveTableGenerated.Length - 1)];
            }
            else
                return 0;
        }

        public static BlastCheat GenerateUnit(string _domain, long _address)
        {
            try
            {
                BizHawk.Client.Common.DisplayType _displaytype = BizHawk.Client.Common.DisplayType.Unsigned;
                bool _bigEndian = false;
                int _value = 0;
                long Address = 0;
				MemoryDomainProxy mdp = RTC_MemoryDomains.getProxy(RTC_MemoryDomains._domain, _address);

                if (RTC_Core.coreForm.cbFreezeEngineActive.Checked && ActiveTableReady)
                    Address = GetAdressFromActiveTable();
                else
                    Address = RTC_Core.RandomLong(mdp.Size -1);

				long safeAddress = Address - (Address % mdp.WordSize);

                return new BlastCheat(_domain, safeAddress, _displaytype, _bigEndian, _value, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong in the RTC Freeze Engine. \n" +
                                "This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
                                ex.ToString());
                return null;
            }
        }


    }
}

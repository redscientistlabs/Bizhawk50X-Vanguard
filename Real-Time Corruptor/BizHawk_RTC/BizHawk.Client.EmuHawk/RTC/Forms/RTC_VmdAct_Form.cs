using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace RTC
{
    public partial class RTC_VmdAct_Form : Form
    {
        public RTC_VmdAct_Form()
        {
            InitializeComponent();
        }

		public bool ActLoadedFromFile = false;
		public bool FirstInit = false;
		public bool _activeTableReady = false;
		public bool ActiveTableReady
		{
			get
			{
				return _activeTableReady;
			}
			set
			{
				if (value)
				{
					lbFreezeEngineActiveStatus.Text = "Active table status: READY";
					btnActiveTableSubstractFile.ForeColor = Color.DarkGreen;
					btnActiveTableAddFile.ForeColor = Color.DarkGreen;
					btnActiveTableSubstractFile.Enabled = true;
					btnActiveTableAddFile.Enabled = true;
				}
				else
				{
					lbFreezeEngineActiveStatus.Text = "Active table status: NOT READY";
					btnActiveTableSubstractFile.ForeColor = Color.Red;
					btnActiveTableAddFile.ForeColor = Color.Red;
					btnActiveTableSubstractFile.Enabled = false;
					btnActiveTableAddFile.Enabled = false;
				}

				if (ActiveTableGenerated != null && ActiveTableGenerated.Length > 0)
					_activeTableReady = value;
				else
					_activeTableReady = false;
			}
		}


		public bool UseActiveTable = false;
		public List<string> ActiveTableDumps = null;
		public long[] ActiveTableActivity = null;
		public long[] ActiveTableGenerated = null;
		public double ActivityThreshold = 0;
		public Timer ActiveTableAutodump = null;

		public string currentFilename
		{
			get { return _currentFilename; }
			set
			{
				if (value == null)
				{
					btnActiveTableQuickSave.ForeColor = Color.Black;
				}
				else
				{
					btnActiveTableQuickSave.ForeColor = Color.OrangeRed;
				}
				_currentFilename = value;
			}
		}
		public string _currentFilename = null;
		

		public void SaveActiveTable(bool IsQuickSave)
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

			using(FileStream FS = File.Open(currentFilename, FileMode.OpenOrCreate))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
				//bformatter.Serialize(FS, act);
				xs.Serialize(FS, act);
				FS.Close();
			}
			
			GC.Collect();
			GC.WaitForPendingFinalizers();

			RTC_Core.StartSound();
		}
		

		public void SetActiveTable(ActiveTableObject act)
		{
			FirstInit = true;
			ActiveTableGenerated = act.data;
			ActiveTableReady = true;
			lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();
		}
		
		public byte[] GetDumpFromFile(string key)
		{
			return File.ReadAllBytes(RTC_Core.rtcDir + "\\MEMORYDUMPS\\" + key + ".dmp");
		}

		public long[] CapActiveTable(long[] tempActiveTable)
		{
			List<long> cappedActiveTable = new List<long>();

			int CapSize = Convert.ToInt32(nmActiveTableCapSize.Value);
			int Offset = Convert.ToInt32(nmActiveTableCapOffset.Value);
			bool DuplicateFound = true;

			if (rbActiveTableCapRandom.Checked)
			{

				for (int i = 0; i < CapSize; i++)
				{
					DuplicateFound = true;

					while (DuplicateFound)
					{
						long queryAdress = tempActiveTable[RTC_Core.RND.RandomLong(tempActiveTable.Length - 1)];

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
			else if (rbActiveTableCapBlockStart.Checked)
			{
				for (int i = 0; i < CapSize || i + Offset >= tempActiveTable.Length; i++)
				{
					cappedActiveTable.Add(tempActiveTable[i + Offset]);
				}
			}
			else if (rbActiveTableCapBlockEnd.Checked)
			{
				for (int i = 0; i < CapSize || (tempActiveTable.Length - 1) - (i + Offset) < 0; i++)
				{
					cappedActiveTable.Add(tempActiveTable[(tempActiveTable.Length - 1) - (i + Offset)]);
				}
			}

			return cappedActiveTable.ToArray();
		}

		public bool ComputeActiveTableActivity()
		{
			if (ActiveTableDumps.Count < 2)
			{
				MessageBox.Show("Not enough dumps for generation");
				return false;
			}

			List<long> newActiveTableActivity = new List<long>();

		//	long domainSize = (long)RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_GETSIZE) { objectValue = cbSelectedMemoryDomain.SelectedItem.ToString()}, true);

			for (long i = 0; i < RTC_MemoryDomains.getInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size; i++)
			{
				newActiveTableActivity.Add(0);
			}

			ActiveTableActivity = newActiveTableActivity.ToArray();

			byte[] lastDump = null;

			for (int i = 0; i < ActiveTableDumps.Count; i++)
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

		public long GetAdressFromActiveTable()
		{
			if (_activeTableReady)
			{
				return ActiveTableGenerated[RTC_Core.RND.Next(ActiveTableGenerated.Length - 1)];
			}
			else
				return 0;
		}


		private void btnActiveTableAddDump_Click(object sender, EventArgs e)
		{
			if (cbSelectedMemoryDomain == null || RTC_MemoryDomains.getInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString() == null)
			{
				MessageBox.Show("Select a valid domain before continuing!");
				return;
			}
			if (ActiveTableDumps == null)
				return;

			string key = RTC_Core.GetRandomKey();
			RTC_Core.SendCommandToBizhawk(new RTC_Command(CommandType.REMOTE_DOMAIN_ACTIVETABLE_MAKEDUMP)
			{
				objectValue = new object[] { cbSelectedMemoryDomain.SelectedItem.ToString(), key }
			}, true);

			ActiveTableDumps.Add(key);
			lbFreezeEngineNbDumps.Text = "Memory dumps collected: " + ActiveTableDumps.Count.ToString();

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void btnActiveTableDumpsReset_Click(object sender, EventArgs e)
		{
			ActLoadedFromFile = false;

			if (!FirstInit)
			{
				FirstInit = true;
				btnActiveTableDumpsReset.Text = "Reset";
				btnActiveTableDumpsReset.ForeColor = Color.Black;

				btnActiveTableAddDump.ForeColor = Color.FromArgb(192, 255, 192);
				btnActiveTableGenerate.Enabled = true;
				btnActiveTableAddDump.Enabled = true;
				btnActiveTableLoad.Enabled = true;
				btnActiveTableQuickSave.Enabled = true;
				cbAutoAddDump.Enabled = true;
			}

			lbFreezeEngineDomainAddressSize.Text = "Domain size: " + RTC_MemoryDomains.getInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString();
			lbFreezeEngineNbDumps.Text = "Memory dumps collected: 0";
			lbFreezeEngineActiveTableSize.Text = "Active table size: 0";
			ActiveTableReady = false;

			ActiveTableGenerated = null;

			ActiveTableDumps = new List<string>();

			foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\MEMORYDUMPS"))
				File.Delete(file);

			currentFilename = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}


		private void btnActiveTableLoad_Click(object sender, EventArgs e)
		{

			RTC_Core.StopSound();


			try
			{
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

				using(FileStream FS = File.Open(currentFilename, FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
					ActiveTableObject act = (ActiveTableObject)xs.Deserialize(FS);
					FS.Close();
					SetActiveTable(act);
					ActLoadedFromFile = true;
				}
				RTC_Core.StartSound();
			}
			catch
			{
				MessageBox.Show($"The ACT xml file {currentFilename} could not be loaded.");
			}
		}

		private void btnActiveTableQuickSave_Click(object sender, EventArgs e)
		{
			if (currentFilename == null)
				SaveActiveTable(false);
			else
				SaveActiveTable(true);
		}

		private void btnActiveTableSubstractFile_Click(object sender, EventArgs e)
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
			ActiveTableObject act = null;
			using (FileStream FS = File.Open(tempFilename, FileMode.OpenOrCreate))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
				act = (ActiveTableObject)xs.Deserialize(FS);
				FS.Close();
			}
			long[] substractiveActiveTable = act.data;

			List<long> newActiveTable = new List<long>();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			foreach (long item in ActiveTableGenerated)
				if (!substractiveActiveTable.Contains(item))
					newActiveTable.Add(item);


			ActiveTableGenerated = newActiveTable.ToArray();
			lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			RTC_Core.StartSound();
		}

		private void btnActiveTableAddFile_Click(object sender, EventArgs e)
		{
			try
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

				ActiveTableObject act = null;

				using (FileStream FS = File.Open(tempFilename, FileMode.OpenOrCreate))
				{
					XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
					act = (ActiveTableObject)xs.Deserialize(FS);
					FS.Close();
				}
				long[] additiveActiveTable = act.data;

				List<long> newActiveTable = new List<long>();

				GC.Collect();
				GC.WaitForPendingFinalizers();

				foreach (long item in ActiveTableGenerated)
					if (additiveActiveTable.Contains(item))
						newActiveTable.Add(item);


				ActiveTableGenerated = newActiveTable.ToArray();
				lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();

				GC.Collect();
				GC.WaitForPendingFinalizers();

				RTC_Core.StartSound();
			}
			catch
			{
				MessageBox.Show($"Unable to add the active table! Are you sure an existing table was loaded?");
			}
		}

		private void generateActiveTable()
		{
			try { 
				if (!ComputeActiveTableActivity())
					return; //exit generation if activity computation failed

				List<long> newActiveTable = new List<long>();
				double computedThreshold = (double)ActiveTableDumps.Count * (ActivityThreshold / 100d) + 1d;
				bool ExcludeEverchanging = cbActiveTableExclude100percent.Checked;


				for (int i = 0; i < ActiveTableActivity.Length; i++)
				{
					if ((double)ActiveTableActivity[i] > computedThreshold && (!ExcludeEverchanging || ActiveTableActivity[i] != (long)ActiveTableDumps.Count))
						newActiveTable.Add(i);
				}

				long[] tempActiveTable = newActiveTable.ToArray();


				if (cbActiveTableCapSize.Checked && nmActiveTableCapSize.Value < tempActiveTable.Length)
					ActiveTableGenerated = CapActiveTable(tempActiveTable);
				else
					ActiveTableGenerated = tempActiveTable;

				lbFreezeEngineActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();


				ActiveTableReady = true;
				currentFilename = null;

				GC.Collect();
				GC.WaitForPendingFinalizers();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in when generating the active table. \n" +
								"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
								ex.ToString());
				return;
			}
		}

		private void generateVMD() {

			try { 
				var token = RTC_NetCore.HugeOperationStart("LAZY");
				MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
				VirtualMemoryDomain VMD = new VirtualMemoryDomain();
				VmdPrototype proto = new VmdPrototype();

				proto.GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString();
				proto.VmdName = RTC_Core.GetRandomKey();
				proto.BigEndian = mi.BigEndian;
				proto.WordSize = mi.WordSize;
				proto.PointerSpacer = 1;
				foreach (int address in ActiveTableGenerated)
					proto.addSingles.Add(address);
				VMD = proto.Generate();
				if (VMD.PointerAddresses.Count == 0)
				{
					MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
					RTC_NetCore.HugeOperationEnd(token);
					return;
				}

				RTC_MemoryDomains.AddVMD(VMD);

				RTC_Core.vmdPoolForm.RefreshVMDs();

				GC.Collect();
				GC.WaitForPendingFinalizers();

				RTC_NetCore.HugeOperationEnd(token);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in when generating the VMD table. \n" +
								"This is not a BizHawk error so you should probably send a screenshot of this to the devs\n\n" +
								ex.ToString());
				return;
			}
		}


		private void btnActiveTableGenerate_Click(object sender, EventArgs e)
		{

			if (cbSelectedMemoryDomain == null || RTC_MemoryDomains.getInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString() == null)
			{
				MessageBox.Show("Select a valid domain before continuing!");
				return;
			}

			if (!ActLoadedFromFile)
				generateActiveTable();
			generateVMD();
		}

		private void btnLoadDomains_Click(object sender, EventArgs e)
		{
			RTC_Core.ecForm.RefreshDomainsAndKeepSelected();

			cbSelectedMemoryDomain.Items.Clear();
			cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

			cbSelectedMemoryDomain.SelectedIndex = 0;

			btnActiveTableDumpsReset.Enabled = true;
			btnActiveTableDumpsReset.ForeColor = Color.DarkGreen;
		}

		private void nmActiveTableActivityThreshold_ValueChanged(object sender, EventArgs e)
		{
			if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
				return;

			track_ActiveTableActivityThreshold.Value = Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100);
			ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);

		}

		private void track_ActiveTableActivityThreshold_Scroll(object sender, EventArgs e)
		{

			if (Convert.ToInt32(track_ActiveTableActivityThreshold.Value) == Convert.ToInt32(nmActiveTableActivityThreshold.Value * 100))
				return;

			nmActiveTableActivityThreshold.Value = Convert.ToDecimal((double)track_ActiveTableActivityThreshold.Value / 100);
			ActivityThreshold = Convert.ToDouble(nmActiveTableActivityThreshold.Value);

		}

		private void cbAutoAddDump_CheckedChanged(object sender, EventArgs e)
		{

			if (ActiveTableAutodump != null)
			{
				ActiveTableAutodump.Stop();
				ActiveTableAutodump = null;
			}

			if (cbAutoAddDump.Checked)
			{
				ActiveTableAutodump = new Timer();
				ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
				ActiveTableAutodump.Tick += new EventHandler(btnActiveTableAddDump_Click);
				ActiveTableAutodump.Start();
			}
		}

		private void nmAutoAddSec_ValueChanged(object sender, EventArgs e)
		{
			if (ActiveTableAutodump != null)
				ActiveTableAutodump.Interval = Convert.ToInt32(nmAutoAddSec.Value) * 1000;
		}
	}
}

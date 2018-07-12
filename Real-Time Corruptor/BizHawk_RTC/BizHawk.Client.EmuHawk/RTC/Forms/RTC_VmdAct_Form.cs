using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

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
					lbActiveStatus.Text = "Active table status: READY";

					btnActiveTableSubtractFile.Font = new Font("Segoe UI Semibold", 8);
					btnActiveTableAddFile.Font = new Font("Segoe UI Semibold", 8);
					btnActiveTableSubtractFile.Enabled = true;
					btnActiveTableAddFile.Enabled = true;
				}
				else
				{
					lbActiveStatus.Text = "Active table status: NOT READY";
					btnActiveTableSubtractFile.Font = new Font("Segoe UI", 8);
					btnActiveTableAddFile.Font = new Font("Segoe UI", 8);
					btnActiveTableSubtractFile.Enabled = false;
					btnActiveTableAddFile.Enabled = false;
				}

				if (ActiveTableGenerated != null && ActiveTableGenerated.Length > 0)
					_activeTableReady = value;
				else
					_activeTableReady = false;
			}
		}

		public bool UseActiveTable = false;
		public bool UseCorePrecision = false;
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

			using (FileStream FS = File.Open(currentFilename, FileMode.OpenOrCreate))
			{
				XmlSerializer xs = new XmlSerializer(typeof(ActiveTableObject));
				//bformatter.Serialize(FS, act);
				xs.Serialize(FS, act);
				FS.Close();
			}


			RTC_Core.StartSound();
		}

		public void SetActiveTable(ActiveTableObject act)
		{
			FirstInit = true;
			ActiveTableGenerated = act.data;
			ActiveTableReady = true;
			lbActiveTableSize.Text = "Active table size (0x" + ActiveTableGenerated.Length.ToString("X") + ")";
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

			MemoryInterface mi = RTC_MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString());
			if(mi == null)
			{
				MessageBox.Show("The currently selected domain doesn't exist!\nMake sure you have the correct core loaded and you've refreshed the domains.");
					return false;
			}

			long domainSize = RTC_MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size;
			for (long i = 0; i < domainSize; i++)
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
			if (cbSelectedMemoryDomain == null || RTC_MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString()).Size.ToString() == null)
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

		}

		private void btnActiveTableDumpsReset_Click(object sender, EventArgs e)
		{
			ActLoadedFromFile = false;

			if (!FirstInit)
			{
				FirstInit = true;
				btnActiveTableDumpsReset.Text = "Reset";
				btnActiveTableDumpsReset.ForeColor = Color.Black;

				btnActiveTableAddDump.Font = new Font("Segoe UI Semibold", 8);
				btnActiveTableGenerate.Enabled = true;
				btnActiveTableAddDump.Enabled = true;
				btnActiveTableLoad.Enabled = true;
				btnActiveTableQuickSave.Enabled = true;
				cbAutoAddDump.Enabled = true;
			}
			MemoryInterface mi = RTC_MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString());
			if (mi == null)
			{
				MessageBox.Show("The currently selected domain doesn't exist!\nMake sure you have the correct core loaded and have refreshed the domains.");
				return;
			}
			decimal memoryDomainSize = mi.Size;
			
			//Verify they want to continue if the domain size is larger than 32MB
			if (memoryDomainSize > 0x2000000)
			{
				DialogResult result = MessageBox.Show("The domain you have selected is larger than 32MB\n The domain size is " + (memoryDomainSize / 1024) + "MB.\n Are you sure you want to continue?", "Large Domain Detected", MessageBoxButtons.YesNo);
				if (result == DialogResult.No)
					return;
			}

			lbDomainAddressSize.Text = "Domain size: 0x" + mi.Size.ToString("X");
			lbFreezeEngineNbDumps.Text = "Memory dumps collected: 0";
			lbActiveTableSize.Text = "Active table size: 0x0";
			ActiveTableReady = false;

			ActiveTableGenerated = null;

			ActiveTableDumps = new List<string>();

			foreach (string file in Directory.GetFiles(RTC_Core.rtcDir + "\\MEMORYDUMPS"))
				File.Delete(file);

			currentFilename = null;
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

				using (FileStream FS = File.Open(currentFilename, FileMode.OpenOrCreate))
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

		private void btnActiveTableSubtractFile_Click(object sender, EventArgs e)
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
			long[] subtractiveActiveTable = act.data;

			List<long> newActiveTable = new List<long>();


			foreach (long item in ActiveTableGenerated)
				if (!subtractiveActiveTable.Contains(item))
					newActiveTable.Add(item);

			ActiveTableGenerated = newActiveTable.ToArray();
			lbActiveTableSize.Text = "Active table size (0x" + ActiveTableGenerated.Length.ToString("X") + ")";

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


				foreach (long item in ActiveTableGenerated)
					if (additiveActiveTable.Contains(item))
						newActiveTable.Add(item);

				ActiveTableGenerated = newActiveTable.ToArray();
				lbActiveTableSize.Text = "Active table size (0x" + ActiveTableGenerated.Length.ToString("X") + ")";


				RTC_Core.StartSound();
			}
			catch
			{
				MessageBox.Show($"Unable to add the active table! Are you sure an existing table was loaded?");
			}
		}

		private bool generateActiveTable()
		{
			var token = RTC_NetCore.HugeOperationStart("DISABLED");
			try
			{
				if (!ComputeActiveTableActivity())
					return false; //exit generation if activity computation failed

				List<long> newActiveTable = new List<long>();
				double computedThreshold = (double)ActiveTableDumps.Count * (ActivityThreshold / 100d) + 1d;
				bool ExcludeEverchanging = cbActiveTableExclude100percent.Checked;

				for (int i = 0; i < ActiveTableActivity.Length; i++)
				{
					if ((double)ActiveTableActivity[i] >= computedThreshold && (!ExcludeEverchanging || ActiveTableActivity[i] != ((long)ActiveTableDumps.Count - 1)))
						newActiveTable.Add(i);
				}

				long[] tempActiveTable = newActiveTable.ToArray();

				if (cbActiveTableCapSize.Checked && nmActiveTableCapSize.Value < tempActiveTable.Length)
					ActiveTableGenerated = CapActiveTable(tempActiveTable);
				else
					ActiveTableGenerated = tempActiveTable;

				lbActiveTableSize.Text = "Active table size: " + ActiveTableGenerated.Length.ToString();

				ActiveTableReady = true;
				currentFilename = null;
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in when generating the active table. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs with instructions on what you did to cause it.\n\n" +
								ex.ToString());
				return false;
			}
			finally
			{
				RTC_NetCore.HugeOperationEnd(token);
			}
		}

		private void generateVMD()
		{
            if (ActiveTableGenerated == null || ActiveTableGenerated.Length == 0)
                return;

			var token = RTC_NetCore.HugeOperationStart("DISABLED");
			try
			{
				MemoryInterface mi = RTC_MemoryDomains.MemoryInterfaces[cbSelectedMemoryDomain.SelectedItem.ToString()];
				VirtualMemoryDomain VMD = new VirtualMemoryDomain();
				VmdPrototype proto = new VmdPrototype();

				int lastaddress = -1;

				proto.GenDomain = cbSelectedMemoryDomain.SelectedItem.ToString();
				proto.VmdName = mi.Name + " " + RTC_Core.GetRandomKey();
				proto.BigEndian = mi.BigEndian;
				proto.WordSize = mi.WordSize;
				proto.PointerSpacer = 1;

				if (UseCorePrecision)
				{
					foreach (int address in ActiveTableGenerated)
					{
						int safeaddress = (address - (address % 4));
						if (safeaddress != lastaddress)
						{
							lastaddress = safeaddress;
							for (int i = 0; i < mi.WordSize; i++)
							{
								proto.addSingles.Add(safeaddress + i);
							}
							//[] _addresses = { safeaddress, safeaddress + mi.WordSize };
							//	proto.addRanges.Add(_addresses);
						}
					}
				}
				else
				{
					foreach (int address in ActiveTableGenerated)
						proto.addSingles.Add(address);
				}

				VMD = proto.Generate();
				if (VMD.PointerAddresses.Count == 0)
				{
					MessageBox.Show("The resulting VMD had no pointers so the operation got cancelled.");
					RTC_NetCore.HugeOperationEnd(token);
					return;
				}

				RTC_MemoryDomains.AddVMD(VMD);

				RTC_Core.vmdPoolForm.RefreshVMDs();

				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong in when generating the VMD table. \n" +
					"This is an RTC error, so you should probably send this to the RTC devs with instructions on what you did to cause it.\n\n" +
								ex.ToString());
				return;
			}
			finally
			{
				RTC_NetCore.HugeOperationEnd(token);
			}
		}

		private void btnActiveTableGenerate_Click(object sender, EventArgs e)
		{
			if (cbSelectedMemoryDomain == null || RTC_MemoryDomains.GetInterface(cbSelectedMemoryDomain.SelectedItem.ToString())?.Size.ToString() == null)
			{
				MessageBox.Show("Select a valid domain before continuing!");
				return;
			}

			btnActiveTableGenerate.Enabled = false;

			//If they didn't load from file, compute then generate. Otherwise just generate
			if (!ActLoadedFromFile)
			{
				if (generateActiveTable())
					generateVMD();
			}
			else
				generateVMD();

			btnActiveTableGenerate.Enabled = true;
		}
		private void RefreshDomains()
		{
			RTC_Core.ecForm.RefreshDomainsAndKeepSelected();
			var temp = cbSelectedMemoryDomain.SelectedItem;

			cbSelectedMemoryDomain.Items.Clear();
			cbSelectedMemoryDomain.Items.AddRange(RTC_MemoryDomains.MemoryInterfaces.Keys.Where(it => !it.Contains("[V]")).ToArray());

			if (temp != null && cbSelectedMemoryDomain.Items.Contains(temp))
				cbSelectedMemoryDomain.SelectedItem = temp;
			else
				cbSelectedMemoryDomain.SelectedIndex = 0;
		}

		private void btnLoadDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
			btnActiveTableDumpsReset.Enabled = true;
			btnActiveTableDumpsReset.Font = new Font("Segoe UI Semibold", 8);
			btnLoadDomains.Text = "Refresh Domains";
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

		private void cbUseCorePrecision_CheckedChanged(object sender, EventArgs e)
		{
			UseCorePrecision = cbUseCorePrecision.Checked;
		}
	}
}

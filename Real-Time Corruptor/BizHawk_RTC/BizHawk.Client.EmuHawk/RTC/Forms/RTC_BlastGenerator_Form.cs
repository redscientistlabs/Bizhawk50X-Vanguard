using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	// 0  dgvBlastLayerReference
	// 1  dgvRowDirty
	// 2  dgvEnabled
	// 3  dgvDomain
	// 4  dgvPrecision
	// 5  dgvType
	// 6  dgvMode
	// 7  dgvStepSize
	// 8  dgvStartAddress
	// 9  dgvEndAddress
	// 10  dgvParam1
	// 11  dgvParam2

	//TYPE = BLASTUNITTYPE
	//MODE = GENERATIONMODE

	public partial class RTC_BlastGenerator_Form : Form
	{
		private enum BlastGeneratorColumn
		{
			dgvBlastLayerReference,
			dgvRowDirty,
			dgvEnabled,
			dgvDomain,
			dgvPrecision,
			dgvType,
			dgvMode,
			dgvStepSize,
			dgvStartAddress,
			dgvEndAddress,
			dgvParam1,
			dgvParam2
		}

		public static BlastLayer currentBlastLayer = null;
		private bool openedFromBlastEditor = false;
		private StashKey sk = null;
		private ContextMenuStrip cms = new ContextMenuStrip();
		private bool initialized = false;

		private Dictionary<string, MemoryDomainProxy> domainToMDPDico = new Dictionary<string, MemoryDomainProxy>();
		public string[] domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			this.dgvBlastGenerator.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastGenerator_MouseClick);
			this.dgvBlastGenerator.CellValueChanged += new DataGridViewCellEventHandler(dgvBlastGenerator_CellValueChanged);
			RTC_Core.SetRTCColor(RTC_Core.generalColor, this);
		}

		public void LoadNoStashKey()
		{
			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = false;
			btnSendTo.Text = "Send to Stash";
			initialized = true;

			this.Show();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();
			sk.BlastLayer = new BlastLayer();

			RefreshDomains();
			AddDefaultRow();
			PopulateModeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = true;
			btnSendTo.Text = "Send to Blast Editor";
			initialized = true;

			this.Show();
		}

		private void AddDefaultRow()
		{
			try
			{
				//Add an empty row and populate with default values
				dgvBlastGenerator.Rows.Add();
				int lastrow = dgvBlastGenerator.RowCount - 1;
				//Set up the DGV based on the current state of Bizhawk
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvRowDirty"]).Value = true;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"]).Value = true;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvPrecision"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvPrecision"] as DataGridViewComboBoxCell).Items[0];
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvType"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvType"] as DataGridViewComboBoxCell).Items[0];

				//These can't be null or else things go bad when trying to save and load them from a file
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvStartAddress"] as DataGridViewNumericUpDownCell).Value = 0;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvEndAddress"] as DataGridViewNumericUpDownCell).Value = 1;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvParam1"] as DataGridViewNumericUpDownCell).Value = 0;
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvParam2"] as DataGridViewNumericUpDownCell).Value = 0;

				PopulateDomainCombobox(dgvBlastGenerator.Rows[lastrow]);
				PopulateModeCombobox(dgvBlastGenerator.Rows[lastrow]);
				// (dgvBlastGenerator.Rows[lastrow].Cells["dgvMode"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Items[0];

				//For some reason, setting the minimum on the DGV to 1 doesn't change the fact it inserts with a count of 0
				(dgvBlastGenerator.Rows[lastrow].Cells["dgvStepSize"]).Value = 1;
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while adding a new row.\n\n" +
							"Your session is probably broken\n" +
							ex.ToString()
							);
			}
		}

		private bool PopulateDomainCombobox(DataGridViewRow row)
		{
			try
			{
				DataGridViewComboBoxCell _cell = row.Cells["dgvDomain"] as DataGridViewComboBoxCell;

				int temp = _cell.Items.Count;
				string currentValue = "";
				if (_cell.Value != null)
					currentValue = _cell.Value.ToString();

				//So this combobox is annoying. You need to have something selected or else the dgv throws up
				//The (bad) solution I'm using is to insert a row at the beginning as a holdover until it's re-populated, then removing that row.

				_cell.Items.Insert(0, "NONE");
				_cell.Value = _cell.Items[0];

				for (int i = temp; i > 0; i--)
					_cell.Items.RemoveAt(1);

				foreach (string domain in domains)
				{
					_cell.Items.Add(domain);
				}

				if (_cell.Items.Contains(currentValue))
					_cell.Value = currentValue;
				else
					_cell.Value = _cell.Items[1];

				_cell.Items.Remove("NONE");

				UpdateAddressRange(row);

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void UpdateAddressRange(DataGridViewRow row)
		{
			try
			{
				if (row.Cells["dgvDomain"].Value.ToString() == "NONE")
					return;

				long size = domainToMDPDico[row.Cells["dgvDomain"].Value.ToString()].Size;

				(row.Cells["dgvStartAddress"] as DataGridViewNumericUpDownCell).Maximum = size;
				(row.Cells["dgvEndAddress"] as DataGridViewNumericUpDownCell).Maximum = size;
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void PopulateModeCombobox(DataGridViewRow row)
		{
			DataGridViewComboBoxCell _cell = row.Cells["dgvMode"] as DataGridViewComboBoxCell;

			int temp = _cell.Items.Count;

			//So this combobox is annoying. You need to have something selected or else the dgv throws up
			//The (bad) solution I'm using is to insert a row at the beginning as a holdover until it's re-populated, then removing that row.

			string currentValue = "";
			if (_cell.Value != null)
				currentValue = _cell.Value.ToString();

			_cell.Items.Insert(0, "NONE");
			_cell.Value = _cell.Items[0];

			for (int i = temp; i > 0; i--)
				_cell.Items.RemoveAt(1);

			switch (row.Cells["dgvType"].Value.ToString())
			{
				case "BlastByte":
					foreach (BGBlastByteModes type in Enum.GetValues(typeof(BGBlastByteModes)))
					{
						_cell.Items.Add(type.ToString());
					}
					break;
				case "BlastCheat":
					foreach (BGBlastCheatModes type in Enum.GetValues(typeof(BGBlastCheatModes)))
					{
						_cell.Items.Add(type.ToString());
					}
					break;
				case "BlastPipe":
					foreach (BGBlastPipeModes type in Enum.GetValues(typeof(BGBlastPipeModes)))
					{
						_cell.Items.Add(type.ToString());
					}
					break;
			}
			if (_cell.Items.Contains(currentValue))
				_cell.Value = currentValue;
			else
				_cell.Value = _cell.Items[1];

			_cell.Items.Remove("NONE");
		}

		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{
			BlastLayer bl = GenerateBlastLayers();
			if (bl != null)
				(bl.Clone() as BlastLayer).Apply();
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			if (sk == null)
			{
				StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();
				if (psk == null)
				{
					RTC_Core.StopSound();
					MessageBox.Show("Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
					RTC_Core.StartSound();
					return;
				}
				sk = (StashKey)psk.Clone();
			}

			StashKey newSk = (StashKey)sk.Clone();

			BlastLayer bl = GenerateBlastLayers(true);
			if (bl == null)
				return;
			newSk.BlastLayer = bl;

			GC.Collect();
			GC.WaitForPendingFinalizers();

			newSk.Run();
		}

		private void btnSendTo_Click(object sender, EventArgs e)
		{
			if (sk == null)
			{
				StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();
				if (psk == null)
				{
					RTC_Core.StopSound();
					MessageBox.Show("Could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
					RTC_Core.StartSound();
					return;
				}
				sk = (StashKey)psk.Clone();
			}

			StashKey newSk = (StashKey)sk.Clone();
			BlastLayer bl = GenerateBlastLayers(true);
			if (bl == null)
				return;
			newSk.BlastLayer = bl;

			if (openedFromBlastEditor)
			{
				if (RTC_Core.beForm != null)
				{
					RTC_Core.beForm.ImportBlastLayer(newSk.BlastLayer);
				}
			}
			else
			{
				RTC_StockpileManager.StashHistory.Add(newSk);
				RTC_Core.ghForm.RefreshStashHistory();
				RTC_Core.ghForm.dgvStockpile.ClearSelection();
				RTC_Core.ghForm.lbStashHistory.ClearSelected();

				RTC_Core.ghForm.DontLoadSelectedStash = true;
				RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void cbUseHex_CheckedChanged(object sender, EventArgs e)
		{
			updownNudgeEndAddress.Hexadecimal = cbUseHex.Checked;
			updownNudgeStartAddress.Hexadecimal = cbUseHex.Checked;
			updownNudgeParam1.Hexadecimal = cbUseHex.Checked;
			updownNudgeParam2.Hexadecimal = cbUseHex.Checked;

			foreach (DataGridViewColumn column in dgvBlastGenerator.Columns)
			{
				if (column.CellType.Name == "DataGridViewNumericUpDownCell")
				{
					DataGridViewNumericUpDownColumn _column = column as DataGridViewNumericUpDownColumn;
					_column.Hexadecimal = cbUseHex.Checked;
				}
			}
		}

		private void dgvBlastGenerator_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastGenerator == null)
				return;

			dgvBlastGenerator.Rows[e.RowIndex].Cells["dgvRowDirty"].Value = true;

			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.dgvType)
			{
				PopulateModeCombobox(dgvBlastGenerator.Rows[e.RowIndex]);
			}
			if ((BlastGeneratorColumn)e.ColumnIndex == BlastGeneratorColumn.dgvDomain)
			{
				UpdateAddressRange(dgvBlastGenerator.Rows[e.RowIndex]);
			}
		}

		private void dgvBlastGenerator_MouseClick(object sender, MouseEventArgs e)
		{
			int currentMouseOverColumn = dgvBlastGenerator.HitTest(e.X, e.Y).ColumnIndex;
			int currentMouseOverRow = dgvBlastGenerator.HitTest(e.X, e.Y).RowIndex;

			if (e.Button == MouseButtons.Left)
			{
				if (currentMouseOverRow == -1)
				{
					dgvBlastGenerator.EndEdit();
					dgvBlastGenerator.ClearSelection();
				}
			}
			/*
			else if (e.Button == MouseButtons.Right)
			{
				//Column header
				if (currentMouseOverRow == -1)
				{
					cmsBlastEditor.Items.Clear();
					PopulateColumnHeaderContextMenu(currentMouseOverColumn);
					cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}
			}
				*/
		}

		public BlastLayer GenerateBlastLayers(bool useStashkey = false)
		{
			try
			{
				BlastLayer bl = new BlastLayer();

				if (useStashkey)
				{
					//If opened from engine config, use the GH state
					if (!openedFromBlastEditor)
					{
						StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();
						if (psk == null)
						{
							RTC_Core.StopSound();
							MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
							RTC_Core.StartSound();
							return null;
						}
						sk = (StashKey)psk.Clone();
					}
					sk.RunOriginal();
				}

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
				{
					if ((bool)row.Cells["dgvRowDirty"].Value == true)
					{
						BlastGeneratorProto proto = CreateProtoFromRow(row);
						row.Cells["dgvBlastLayerReference"].Value = proto.GenerateBlastLayer();
						bl.Layer.AddRange(((BlastLayer)row.Cells["dgvBlastLayerReference"].Value).Layer);
						row.Cells["dgvRowDirty"].Value = true;
					}
					else
					{
						bl.Layer.AddRange(((BlastLayer)row.Cells["dgvBlastLayerReference"].Value).Layer);
					}
				}
				return bl;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Something went wrong when generating the blastlayers. \n" +
					"Are you sure your input is correct and you have the correct core loaded?\n\n" +
					ex.ToString());
				return null;
			}
		}

		//We don't always know how the parameters are going to be used
		//Because of this, we create the prototype using the raw value and flip the bytes as needed inside the generator
		//An example would be that the param could be an address, or a value
		//If we flipped to big endian here, that'd goof the handling of addresses
		//As such, we always create the prototype with the raw value.
		private BlastGeneratorProto CreateProtoFromRow(DataGridViewRow row)
		{
			string domain = row.Cells["dgvDomain"].Value.ToString();
			string type = row.Cells["dgvType"].Value.ToString();
			string mode = row.Cells["dgvMode"].Value.ToString();
			int precision = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
			int stepSize = Convert.ToInt32(row.Cells["dgvStepSize"].Value);
			long startAddress = Convert.ToInt64(row.Cells["dgvStartAddress"].Value);
			long endAddress = Convert.ToInt64(row.Cells["dgvEndAddress"].Value);
			long param1 = Convert.ToInt64(row.Cells["dgvParam1"].Value);
			long param2 = Convert.ToInt64(row.Cells["dgvParam2"].Value);

			return new BlastGeneratorProto(type, domain, mode, precision, stepSize, startAddress, endAddress, param1, param2);
		}

		private string GetPrecisionNameFromSize(int precision)
		{
			switch (precision)
			{
				case 1:
					return "8-bit";

				case 2:
					return "16-bit";

				case 4:
					return "32-bit";

				default:
					return null;
			}
		}

		private int GetPrecisionSizeFromName(string precision)
		{
			switch (precision)
			{
				case "8-bit":
					return 1;

				case "16-bit":
					return 2;

				case "32-bit":
					return 4;

				default:
					return -1;
			}
		}

		private void btnAddRow_Click(object sender, EventArgs e)
		{
			AddDefaultRow();
		}

		private void btnNudgeStartAddressUp_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvStartAddress", updownNudgeStartAddress.Value);
		}

		private void btnNudgeStartAddressDown_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvStartAddress", updownNudgeStartAddress.Value, true);
		}

		private void btnNudgeEndAddressUp_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvEndAddress", updownNudgeEndAddress.Value);
		}

		private void btnNudgeEndAddressDown_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvEndAddress", updownNudgeEndAddress.Value, true);
		}

		private void btnNudgeParam1Up_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvParam1", updownNudgeParam1.Value);
		}

		private void btnNudgeParam1Down_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvParam1", updownNudgeParam1.Value, true);
		}

		private void btnNudgeParam2Up_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvParam2", updownNudgeParam2.Value);
		}

		private void btnNudgeParam2Down_Click(object sender, EventArgs e)
		{
			nudgeParams("dgvParam2", updownNudgeParam2.Value, true);
		}

		private void nudgeParams(string column, decimal amount, bool shiftDown = false)
		{
			if (shiftDown)
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) >= 0)

						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) - amount;
					else
						selected.Cells[column].Value = 0;
				}
			else
			{
				foreach (DataGridViewRow selected in dgvBlastGenerator.SelectedRows)
				{
					decimal max = (selected.Cells[column] as DataGridViewNumericUpDownCell).Maximum;

					if ((Convert.ToDecimal(selected.Cells[column].Value) - amount) <= max)
						selected.Cells[column].Value = Convert.ToDecimal(selected.Cells[column].Value) + amount;
					else
						selected.Cells[column].Value = max;
				}
			}
		}

		private void btnHideSidebar_Click(object sender, EventArgs e)
		{
			if (btnHideSidebar.Text == "▶")
			{
				panelSidebar.Visible = false;
				btnHideSidebar.Text = "◀";
			}
			else
			{
				panelSidebar.Visible = true;
				btnHideSidebar.Text = "▶";
			}
		}

		private void RefreshDomains()
		{
			try
			{
				domainToMDPDico.Clear();
				domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
				for (int i = 0; i < domains.Length; i++)
				{
					domainToMDPDico.Add(domains[i], RTC_MemoryDomains.getProxy(domains[i], 0));
				}

				foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
					PopulateDomainCombobox(row);
			}
			catch (Exception ex)
			{
				throw new Exception(
							"An error occurred in RTC while refreshing the domains\n" +
							"Are you sure you don't have an invalid domain selected?\n" +
							"Make sure any VMDs are loaded and you have the correct core loaded in Bizhawk\n" +
							ex.ToString()
							);
			}
		}

		private void btnRefreshDomains_Click(object sender, EventArgs e)
		{
			RefreshDomains();
		}

		private void saveDataGridView(DataGridView dgv)
		{
			var dt = new DataTable();
			foreach (DataGridViewColumn column in dgv.Columns)
			{
				dt.Columns.Add();
			}

			object[] cellValues = new object[dgv.Columns.Count];
			foreach (DataGridViewRow row in dgv.Rows)
			{
				for (int i = 0; i < row.Cells.Count; i++)
				{
					cellValues[i] = row.Cells[i].Value;
				}
				dt.Rows.Add(cellValues);
			}

			DataSet ds = new DataSet();
			ds.Tables.Add(dt);
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "bg|*.bg";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					ds.Tables[0].WriteXml(sfd.FileName);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		private bool importDataGridView(DataGridView dgv)
		{
			return loadDataGridView(dgv, true);
		}

		private bool loadDataGridView(DataGridView dgv, bool import = false)
		{
			DataSet ds = new DataSet();
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "bg|*.bg";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				try
				{
					var dt = new DataTable();
					ds.Tables.Add(dt);
					foreach (DataGridViewColumn column in dgv.Columns)
						dt.Columns.Add();
					ds.Tables[0].ReadXml(ofd.FileName);
					if (!import)
					{
						foreach (DataGridViewRow row in dgv.Rows)
							dgv.Rows.Remove(row);
					}

					foreach (DataRow row in ds.Tables[0].Rows)
					{
						dgv.Rows.Add();

						int lastrow = dgvBlastGenerator.RowCount - 1;

						//We need to populate the comboboxes or else things go bad
						//To do this, we load the type first so we can populate the modes, then add the domain to the domain combobox.
						//If it's invalid, they'll know on generation. If they load the correct core and press refresh, it'll maintain its selection
						dgv[(int)BlastGeneratorColumn.dgvType, lastrow].Value = row.ItemArray[(int)BlastGeneratorColumn.dgvType];

						PopulateModeCombobox(dgv.Rows[lastrow]);
						(dgv.Rows[lastrow].Cells["dgvDomain"] as DataGridViewComboBoxCell).Items.Add(row.ItemArray[(int)BlastGeneratorColumn.dgvDomain]);

						for (int i = 0; i < dgv.Rows[lastrow].Cells.Count; i++)
						{
							dgv.Rows[lastrow].Cells[i].Value = row.ItemArray[i];
						}
						//Override these two
						dgv[(int)BlastGeneratorColumn.dgvBlastLayerReference, lastrow].Value = null;
						dgv[(int)BlastGeneratorColumn.dgvRowDirty, lastrow].Value = true;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					return false;
				}
			}
			return true;
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			loadDataGridView(dgvBlastGenerator);
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveDataGridView(dgvBlastGenerator);
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			importDataGridView(dgvBlastGenerator);
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show(
			@"Blast Generator instructions help and examples
			-----------------------------------------------
			>Endianess is always handled as little endian, or right -> left
			This is completely agnostic of core endianess
			That means that:
			10 on 16-bit precision will be treated as 00 10
			1000 on 16-bit precision will be treated as 10 00
			etc...

			> Ranges are exclusive, meaning that the last
			address is excluded from the range.
			This means that:
			Start Address of 10, End address of 16, step size of 1
			would generate blasts for addresses 10,11,12,13,14,15"
			);
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

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

		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			this.dgvBlastGenerator.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastGenerator_MouseClick);
		}

		public void LoadGHStashkey()
		{
			StashKey psk = RTC_StockpileManager.getCurrentSavestateStashkey();

			if (psk == null)
			{
				RTC_Core.StopSound();
				MessageBox.Show("The Glitch Harvester could not perform the CORRUPT action\n\nEither no Savestate Box was selected in the Savestate Manager\nor the Savetate Box itself is empty.");
				RTC_Core.StartSound();
				return;
			}

			sk = (StashKey)psk.Clone();
			
			PopulateDomainCombobox(dgvBlastGenerator.Columns["dgvDomain"] as DataGridViewComboBoxColumn);
			AddDefaultRow();
			PopulateTypeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = false;

			this.Show();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();
			sk.BlastLayer = null;

			PopulateDomainCombobox(dgvBlastGenerator.Columns["dgvDomain"] as DataGridViewComboBoxColumn);
			AddDefaultRow();
			PopulateTypeCombobox(dgvBlastGenerator.Rows[0]);
			openedFromBlastEditor = true;

			this.Show();
		}

		private void AddDefaultRow()
		{

			//Add an empty row and populate with default values
			dgvBlastGenerator.Rows.Add();
			int lastrow = dgvBlastGenerator.RowCount - 1;
			//Set up the DGV based on the current state of Bizhawk
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvRowDirty"]).Value = true;
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvEnabled"]).Value = true;
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvPrecision"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvPrecision"] as DataGridViewComboBoxCell).Items[0];
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvDomain"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvDomain"] as DataGridViewComboBoxCell).Items[0];
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvType"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvType"] as DataGridViewComboBoxCell).Items[0];
			(dgvBlastGenerator.Rows[lastrow].Cells["dgvMode"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Items[0];

			PopulateTypeCombobox(dgvBlastGenerator.Rows[lastrow]);
		}

		private bool PopulateDomainCombobox(DataGridViewComboBoxColumn dgvColumn)
		{
			string[] domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

			foreach (string domain in domains)
			{
				dgvColumn.Items.Add(domain);
			}

			return false;
		}


		private void PopulateTypeCombobox(DataGridViewRow row)
		{
			DataGridViewComboBoxCell dgvType = (row.Cells["dgvMode"] as DataGridViewComboBoxCell);
			dgvType.Items.Clear();

			switch (row.Cells["dgvType"].Value.ToString())
			{
				case "BlastByte":
					foreach (BGBlastModes type in Enum.GetValues(typeof(BGBlastModes)))
					{
						dgvType.Items.Add(type.ToString());
					}
					break;
				case "BlastCheat":
					foreach (BGBlastModes type in Enum.GetValues(typeof(BGBlastModes)))
					{
						dgvType.Items.Add(type.ToString());
					}
					foreach (BGBlastCheatModes type in Enum.GetValues(typeof(BGBlastCheatModes)))
					{
						dgvType.Items.Add(type.ToString());
					}
					break;
				case "BlasePipe":
					foreach (BGBlastPipeModes type in Enum.GetValues(typeof(BGBlastPipeModes)))
					{
						dgvType.Items.Add(type.ToString());
					}
					break;
			}
		}


		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{

		}

		private void btnSendTo_Click(object sender, EventArgs e)
		{
			sk.BlastLayer = GenerateBlastLayers();

			StashKey newSk = (StashKey)sk.Clone();

			if (openedFromBlastEditor)
			{
				if(RTC_Core.beForm != null)
				{
					RTC_Core.beForm.ImportBlastLayer(sk.BlastLayer);
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

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{

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
			else if (e.Button == MouseButtons.Right)
			{
				/*	//Column header
					if (currentMouseOverRow == -1)
					{
						cmsBlastEditor.Items.Clear();
						PopulateColumnHeaderContextMenu(currentMouseOverColumn);
						cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
					}

					//dgvType
					if (currentMouseOverColumn == (int)BlastGeneratorColumn.dgvType)
					{
						PopulateBlastUnitModeContextMenu(currentMouseOverColumn, currentMouseOverRow);
						cms.Show(dgvBlastGenerator, new Point(e.X, e.Y));
					}
					*/
			}
		}

		public BlastLayer GenerateBlastLayers()
		{
			BlastLayer bl = new BlastLayer();
			sk.RunOriginal();
		

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

		private BlastGeneratorProto CreateProtoFromRow(DataGridViewRow row)
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

		//	try
		//	{

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

		//	}catch(Exception ex)
		//	{
		//		throw;
		//	}
		}

		private void btnShiftBlastLayerDown_Click(object sender, EventArgs e)
		{

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
	}
}

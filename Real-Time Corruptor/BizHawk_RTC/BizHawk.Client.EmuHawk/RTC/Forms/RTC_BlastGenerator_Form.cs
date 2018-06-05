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

		private StashKey sk = null;
		private ContextMenuStrip cms = new ContextMenuStrip();

		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
			//Fill in a default row
			//dgvBlastGenerator.Rows[0].Cells["dgvEnabled"].Value = true;

			this.dgvBlastGenerator.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastGenerator_MouseClick);

		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();

			//Set up the DGV based on the current state of Bizhawk
			PopulateDomainCombobox(dgvBlastGenerator.Columns["dgvDomain"] as DataGridViewComboBoxColumn);
			(dgvBlastGenerator.Rows[0].Cells["dgvRowDirty"]).Value = true;
			(dgvBlastGenerator.Rows[0].Cells["dgvDomain"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvDomain"] as DataGridViewComboBoxCell).Items[0];
			(dgvBlastGenerator.Rows[0].Cells["dgvType"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvType"] as DataGridViewComboBoxCell).Items[0];
			(dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Value = (dgvBlastGenerator.Rows[0].Cells["dgvMode"] as DataGridViewComboBoxCell).Items[0];
			PopulateTypeCombobox(dgvBlastGenerator.Rows[0]);

			this.Show();
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

		private void btnSendBlastLayerToEditor_Click(object sender, EventArgs e)
		{

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

		private void GenerateBlastLayers()
		{
			BlastLayer bl = new BlastLayer();

			//foreach (DataGridViewRow row in dgvBlastGenerator.Rows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvRowDirty"].Value == true))
			foreach (DataGridViewRow row in dgvBlastGenerator.Rows)
			{
				if(true)
				//if ((bool)row.Cells["dgvRowDirty"].Value == true)
				{
					BlastGeneratorProto proto = CreateProtoFromRow(row);
					row.Cells["dgvBlastLayerReference"].Value = proto.GenerateBlastLayer().Layer;
					bl.Layer.Concat(((BlastLayer)row.Cells["dgvBlastLayerReference"].Value).Layer);
				}
				else
				{
					bl.Layer.Concat(((BlastLayer)row.Cells["dgvBlastLayerReference"].Value).Layer);
				}
			}
			sk.BlastLayer = bl;
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

			string domain = row.Cells["dgvDomain"].Value.ToString();
			string type = row.Cells["dgvType"].Value.ToString();
			string mode = row.Cells["dgvMode"].Value.ToString();
			int precision = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
			int stepSize = Convert.ToInt32(row.Cells["dgvDomain"].Value);
			long startAddress = Convert.ToInt64(row.Cells["dgvStartAddress"].Value);
			long endAddress = Convert.ToInt64(row.Cells["dgvEndAddress"].Value);
			long param1 = Convert.ToInt64(row.Cells["dgvParam1"].Value);
			long param2 = Convert.ToInt64(row.Cells["dgvParam2"].Value);

			return new BlastGeneratorProto(type, domain, mode, precision, stepSize, startAddress, endAddress, param1, param2);

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
	}
}

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
	// 0  dgvEnabled
	// 1  dgvDomain
	// 2  dgvType
	// 3  dgvMode
	// 4  dgvStepSize
	// 5  dgvStartAddress
	// 6  dgvEndAddress
	// 7  dgvParam1
	// 8  dgvParam2
	public partial class RTC_BlastGenerator_Form : Form
	{
		private enum BlastGeneratorColumn
		{
			dgvEnabled,
			dgvDomain,
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


		private void btnShiftBlastLayerDown_Click(object sender, EventArgs e)
		{

		}
	}
}

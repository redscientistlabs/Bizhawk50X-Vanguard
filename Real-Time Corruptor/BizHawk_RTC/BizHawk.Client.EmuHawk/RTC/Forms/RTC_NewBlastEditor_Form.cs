using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	/* 
	 * Column Indexes - Updated 4/8/2018
	 * 
	 * 0 dgvBlastUnitReference
	 * 1 dgvEnabled
	 * 2 dgvPrecision
	 * 3 dgvBlastUnitType
	 * 4 dgvBUMode
	 * 5 dgvParam1Domain
	 * 6 dvgParam1
	 * 7 dvgParam2Domain
	 * 8 dvgParam2
	 */
	public partial class RTC_NewBlastEditor_Form : Form
	{
		StashKey sk = null;
		bool initialized = false;
		bool CurrentlyUpdating = false;
		ContextMenuStrip cmsDomain = null;

		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();
			//set its location and size to fit the cell
			//	dtp.Location = dgvBlastLayer.GetCellDisplayRectangle(0, 3,true).Location;
			//dtp.Size = dgvBlastLayer.GetCellDisplayRectangle(0, 3,true).Size;
		}

		private void RTC_BlastEditorForm_Load(object sender, EventArgs e)
		{
			RTC_Core.SetRTCColor(RTC_Core.generalColor, this);

			this.dgvBlastLayer.CellValidating += new DataGridViewCellValidatingEventHandler(dgvBlastLayer_CellValidating);
			this.dgvBlastLayer.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastLayer_MouseClick);
			this.dgvBlastLayer.Sorted += dgvBlastLayer_Sorted;
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null || _sk.BlastLayer == null || _sk.BlastLayer.Layer == null)
				return;
			sk = (StashKey)_sk.Clone();
			RefreshBlastLayer();
			this.Show();
		}

		public void RefreshBlastLayer()
		{
			initialized = false;
			//Clear out whatever is there
			dgvBlastLayer.Rows.Clear();
			//Populate the different rows.
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				/*
				 param1
					BlastByte  = Address
					BlastCheat = Address
					BlastPipe  = Source Address
					BlastVector = Address
				 param2
					BlastByte  = Value
					BlastCheat = Value
					BlastPipe  = Destination Address
					BlastBector = Value
				*/

				//Valid for all types
				bool enabled = bu.IsEnabled;
				string blastType = Convert.ToString(bu.GetType());

				//Dependent on blast type
				int precision = -1;
				string sourceDomain = "";
				string destDomain = "";
				Decimal sourceAddress = 0;

				//This is both the value and the dest address depending on the engine
				Decimal destAddress = 0;
				string blastMode = "";

				//add DateTimePicker into the control collection of the DataGridView

				if (bu is BlastByte)
				{
					BlastByte bb = bu as BlastByte;
					precision = bb.Value.Length;
					sourceAddress = Convert.ToDecimal(bb.Address);
					sourceDomain = bb.Domain;
					destAddress = (Decimal)RTC_Extensions.getDecimalValue(bb.Value);
					blastMode = Convert.ToString(bb.Type);
				}

				else if (bu is BlastCheat)
				{
					BlastCheat bc = bu as BlastCheat;
					precision = bc.Value.Length;
					sourceAddress = bc.Address;
					sourceDomain = bc.Domain;
					//destAddress = RTC_Extensions.getDecimalValue(bc.Value);
					if (bc.IsFreeze)
						blastMode = "Freeze";
					else
						blastMode = "HellGenie";

				}
				else if (bu is BlastPipe)
				{
					BlastPipe bp = bu as BlastPipe;
					precision = bp.PipeSize;

					sourceAddress = bp.Address;
					sourceDomain = bp.Domain;
					destDomain = bp.PipeDomain;
					destAddress = bp.PipeAddress;
					blastMode = Convert.ToString(bp.TiltValue);
				}
				if (bu is BlastVector)
				{
					BlastVector bv = bu as BlastVector;
					precision = bv.Values.Length;
					sourceAddress = Convert.ToDecimal(bv.Address);
					sourceDomain = bv.Domain;
					destAddress = (Decimal)RTC_Extensions.getDecimalValue(bv.Values);
					blastMode = Convert.ToString(bv.Type);
				}

				dgvBlastLayer.Rows.Add(bu, enabled, GetPrecisionNameFromSize(precision), blastType, blastMode, sourceDomain, sourceAddress, destDomain, destAddress);
				//Update the precision
				ValidatePrecision(dgvBlastLayer.Rows[dgvBlastLayer.Rows.Count - 1]);
			}
			//Param1 and Param2 only need to accept Decimal
			dgvBlastLayer.Columns[6].ValueType = typeof(Decimal);
			dgvBlastLayer.Columns[8].ValueType = typeof(Decimal);

			PopulateDomainContextMenu();
			lbBlastLayerSize.Text = "BlastLayer size: " + sk.BlastLayer.Layer.Count.ToString();
			initialized = true;
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				bu.IsEnabled = true;

			foreach (BlastUnit bu in sk.BlastLayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.BlastLayer.Layer.Count / 2))
				bu.IsEnabled = false;

			RefreshBlastLayer();
		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < sk.BlastLayer.Layer.Count(); i++)
				sk.BlastLayer.Layer[i].IsEnabled = !sk.BlastLayer.Layer[i].IsEnabled;

			RefreshBlastLayer();
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> newLayer = new List<BlastUnit>();

			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				if (bu.IsEnabled)
					newLayer.Add(bu);

			sk.BlastLayer.Layer = newLayer;

			RefreshBlastLayer();
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			BlastLayer bl = new BlastLayer();

			foreach (DataGridViewRow row in dgvBlastLayer.Rows)
			{
				BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
				if (bu.IsEnabled)
					bl.Layer.Add(bu);
			}


			StashKey newSk = (StashKey)sk.Clone();
			newSk.BlastLayer = (BlastLayer)bl.Clone();

			GC.Collect();
			GC.WaitForPendingFinalizers();

			newSk.Run();
		}

		private void btnCorrupt_Click(object sender, EventArgs e)
		{
			BlastLayer bl = new BlastLayer();

			foreach (DataGridViewRow row in dgvBlastLayer.Rows)
			{
				BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
				if (bu.IsEnabled)
					bl.Layer.Add(bu);
			}
			GC.Collect();
			GC.WaitForPendingFinalizers();

			(bl.Clone() as BlastLayer).Apply();
		}

		private void btnSendToStash_Click(object sender, EventArgs e)
		{
			StashKey newSk = (StashKey)sk.Clone();
			//newSk.Key = RTC_Core.GetRandomKey();
			//newSk.Alias = null;

			RTC_StockpileManager.StashHistory.Add(newSk);
			RTC_Core.ghForm.RefreshStashHistory();
			RTC_Core.ghForm.dgvStockpile.ClearSelection();
			RTC_Core.ghForm.DontLoadSelectedStash = true;
			RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}


		private void btnDisableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				bu.IsEnabled = false;

			RefreshBlastLayer();
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				bu.IsEnabled = true;

			RefreshBlastLayer();
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			if (dgvBlastLayer.CurrentCell.RowIndex == -1)
			{
				MessageBox.Show("No unit was selected. Cannot duplicate.");
				return;
			}

			int pos = dgvBlastLayer.CurrentCell.RowIndex;

			BlastUnit bu = sk.BlastLayer.Layer[pos];
			BlastUnit bu2 = ObjectCopier.Clone(bu);
			sk.BlastLayer.Layer.Insert(pos, bu2);


			RefreshBlastLayer();
		}

		private void btnSanitizeDuplicates_Click(object sender, EventArgs e)
		{
			List<BlastUnit> bul = new List<BlastUnit>(sk.BlastLayer.Layer.ToArray().Reverse());
			List<long> usedAddresses = new List<long>();

			foreach (BlastUnit bu in bul)
			{
				if (!usedAddresses.Contains(bu.Address))
					usedAddresses.Add(bu.Address);
				else
					sk.BlastLayer.Layer.Remove(bu);
			}

			RefreshBlastLayer();
		}

		private void dgvBlastLayer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastLayer == null || dgvBlastLayer.SelectedCells == null)
				return;

			if (!CurrentlyUpdating)
			{
				CurrentlyUpdating = true;
				DataGridViewRow row = dgvBlastLayer.Rows[dgvBlastLayer.CurrentCell.RowIndex];
				UpdateBlastUnitFromRow(row);
			}
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

		private void UpdateBlastUnitFromRow(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value)
			{
				case "RTC.BlastByte":
					BlastByte bb = (BlastByte)row.Cells["dgvBlastUnitReference"].Value;
					bb.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bb.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bb.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam2"].Value));
					bb.Domain = Convert.ToString(row.Cells["dgvParam1Domain"].Value);
					Enum.TryParse(row.Cells["dgvBUMode"].Value.ToString().ToUpper(), out bb.Type);
					row.Cells["dgvBlastUnitReference"].Value = bb;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastCheat":
					BlastCheat bc = (BlastCheat)row.Cells["dgvBlastUnitReference"].Value;
					bc.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bc.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bc.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam2"].Value));
					bc.Domain = Convert.ToString(row.Cells["dgvParam1Domain"].Value);
					if (row.Cells["dgvBUMode"].Value.ToString() == "Freeze") ;
					bc.IsFreeze = true;
					row.Cells["dgvBlastUnitReference"].Value = bc;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastPipe":
					BlastPipe bp = (BlastPipe)row.Cells["dgvBlastUnitReference"].Value;
					bp.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bp.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bp.PipeAddress = Convert.ToInt64(row.Cells["dgvParam2"].Value);
					bp.Domain = Convert.ToString(row.Cells["dgvParam1Domain"].Value);
					bp.PipeDomain = Convert.ToString(row.Cells["dgvParam2Domain"].Value);
					bp.TiltValue = Convert.ToInt32(row.Cells["dgvBUMode"].Value.ToString());
					row.Cells["dgvBlastUnitReference"].Value = bp;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastVector":
					BlastVector bv = (BlastVector)row.Cells["dgvBlastUnitReference"].Value;
					bv.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bv.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bv.Values = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam2"].Value));
					bv.Domain = Convert.ToString(row.Cells["dgvParam1Domain"].Value);
					Enum.TryParse(row.Cells["dgvBUMode"].Value.ToString().ToUpper(), out bv.Type);
					row.Cells["dgvBlastUnitReference"].Value = bv;
					CurrentlyUpdating = false;
					break;
				default:
					MessageBox.Show("You had an invalid blast unit type! Check your input. The invalid unit is: " + row.Cells["dgvBlastUnitType"].Value);
					CurrentlyUpdating = false;
					break;
			}
		}



		//This is gonna be ugly because some engines re-use boxes. Sorry -Narry
		private void ValidatePrecision(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value.ToString())
			{
				case "RTC.BlastByte":
				case "RTC.BlastCheat":
					if (row.Cells["dgvPrecision"].Value.ToString() == "8-bit")
					{
						(row.Cells["dgvParam2"] as DataGridViewNumericUpDownCell).Maximum = 255;
					}
					else if (row.Cells["dgvPrecision"].Value.ToString() == "16-bit")
					{
						(row.Cells["dgvParam2"] as DataGridViewNumericUpDownCell).Maximum = 65535;
					}
					else
					{
						(row.Cells["dgvParam2"] as DataGridViewNumericUpDownCell).Maximum = 4294967295;
					}
					break;
				case "RTC.BlastPipe":
						//Todo set this to the valid maximum address
						(row.Cells["dgvParam2"] as DataGridViewNumericUpDownCell).Maximum = 2147483647;
						break;
				default:
					break;
			}
		}

		private void dgvBlastLayer_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DataGridViewRow row = dgvBlastLayer.Rows[e.RowIndex];
			DataGridViewColumn column = dgvBlastLayer.Columns[e.ColumnIndex];
			
			//Make sure the max and min is updated if the precision changed
			if (column.HeaderText.Equals("Precision"))
				ValidatePrecision(dgvBlastLayer.Rows[e.RowIndex]);

			if (!ValidateBlastUnitFromRow(row))
			{
				dgvBlastLayer.Rows[e.RowIndex].ErrorText = "There's something wrong with your input!";
				e.Cancel = true;
			}

		}

		//Validates that a blast unit is valid from a dgv row
		bool ValidateBlastUnitFromRow(DataGridViewRow row)
		{
			return true;
		}

		private void PopulateDomainContextMenu()
		{
			String[] domains = RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray();

			if (cmsDomain == null)
				cmsDomain = new ContextMenuStrip();

			cmsDomain.Items.Clear();

			foreach (string domain in domains)
			{
				//cmsDomain.Items.Add(domain, null, );
				(cmsDomain.Items.Add(domain, null, new EventHandler((ob, ev) =>
				{
					dgvBlastLayer.SelectedCells[0].Value = domain;
				})) as ToolStripMenuItem).Enabled = true;
			}
		}

		private void dgvBlastLayer_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int currentMouseOverColumn = dgvBlastLayer.HitTest(e.X, e.Y).ColumnIndex;
				if ((dgvBlastLayer.CurrentCell.ColumnIndex == 5 && currentMouseOverColumn == 5) || (dgvBlastLayer.CurrentCell.ColumnIndex == 7 && currentMouseOverColumn == 7))
						cmsDomain.Show(dgvBlastLayer, new Point(e.X, e.Y));
			}
		}

		private void dgvBlastLayer_Sorted(object sender, EventArgs e)
		{
			switch (dgvBlastLayer.SortedColumn.Name)
			{
				case "dgvParam1Domain":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Domain).ToList();
					break;
				case "dgvParam1":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Address).ToList();
					break;
				case "dgvParam2Domain":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParam2Domain(it)).ToList();
					break;
				case "dgvParam2":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParam2Value(it)).ToList();
					break;
				default:
					break;
			}
		}

		private long GetParam2Value(BlastUnit bu)
		{
			if(bu is BlastByte || bu is BlastCheat)
			{
				BlastByte bb = bu as BlastByte;
				decimal value = RTC_Extensions.getDecimalValue(bb.Value);				
				return (long)value;
			}
			if (bu is BlastPipe)
			{
				BlastPipe bp = bu as BlastPipe;
				return bp.PipeAddress;
			}
			return 0;
		}
		private string GetParam2Domain(BlastUnit bu)
		{
			if (bu is BlastPipe)
			{
				BlastPipe bp = bu as BlastPipe;
				return bp.PipeDomain;
			}
			return null;
		}

		private void cbUseHex_CheckedChanged(object sender, EventArgs e)
		{
			MessageBox.Show("Not yet implemented.");
		}

	}
}
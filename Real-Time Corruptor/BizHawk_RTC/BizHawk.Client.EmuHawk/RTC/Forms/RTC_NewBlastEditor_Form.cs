using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RTC
{
	public partial class RTC_NewBlastEditor_Form : Form
	{
		/* 
		 * Column Indexes - Updated 4/8/2018
		 * 
		 * 0 dgvLocked
		 * 1 dgvBlastUnitReference
		 * 2 dgvEnabled
		 * 3 dgvPrecision
		 * 4 dgvBlastUnitType
		 * 5 dgvBUMode
		 * 6 dgvSourceAddressDomain
		 * 7 dvgSourceAddress
		 * 8 dvgParamDomain
		 * dvgParam
		 */
		private enum BlastEditorColumn
		{
			Locked,
			BlastUnitReference,
			Enabled,
			Precision,
			BlastUnitType,
			BUMode,
			SourceAddressDomain,
			SourceAddress,
			ParamDomain,
			Param
		}

		StashKey sk = null;
		StashKey originalsk = null;
		bool initialized = false;
		bool CurrentlyUpdating = false;
		ContextMenuStrip cmsDomain = new ContextMenuStrip();
		ContextMenuStrip cmsBlastUnitMode = new ContextMenuStrip();
		ContextMenuStrip cmsBlastUnitType = new ContextMenuStrip();
		ContextMenuStrip cmsColumnHeader = new ContextMenuStrip();
		String[] domains;

		Dictionary<BlastUnit, DataGridViewRow> bu2RowDico = new Dictionary<BlastUnit, DataGridViewRow>();

		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();

			dgvBlastLayer.DoubleBuffered(true);
			this.dgvBlastLayer.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;


			dgvBlastLayer.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);

			//set its location and size to fit the cell
			//	dtp.Location = dgvBlastLayer.GetCellDisplayRectangle(0, 3,true).Location;
			//dtp.Size = dgvBlastLayer.GetCellDisplayRectangle(0, 3,true).Size;
		}

		private void RTC_BlastEditorForm_Load(object sender, EventArgs e)
		{
			RTC_Core.SetRTCColor(RTC_Core.generalColor, this);

			this.dgvBlastLayer.CellValidating += new DataGridViewCellValidatingEventHandler(dgvBlastLayer_CellValidating);
			this.dgvBlastLayer.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastLayer_MouseClick);
			this.dgvBlastLayer.RowsAdded += new DataGridViewRowsAddedEventHandler(dgvBlastLayer_RowsAdded);
			this.dgvBlastLayer.RowsRemoved += new DataGridViewRowsRemovedEventHandler(dgvBlastLayer_RowsRemoved);
			this.dgvBlastLayer.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgvBlastLayer_UserDeletingRow);
			this.dgvBlastLayer.Sorted += dgvBlastLayer_Sorted;
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null || _sk.BlastLayer == null || _sk.BlastLayer.Layer == null)
				return;

			originalsk = (StashKey)_sk.Clone();
			sk = originalsk;
			RefreshBlastLayer();

			domains = RTC_MemoryDomains.MemoryInterfaces.Keys.ToArray();

			this.Show();
		}

		public void UpdateBlastLayerSize()
		{
			lbBlastLayerSize.Text = "BlastLayer size: " + sk.BlastLayer.Layer.Count.ToString();
		}

		public void RefreshBlastLayer()
		{
			initialized = false;
			//Clear out whatever is there
			dgvBlastLayer.Rows.Clear();
			//Populate the different rows.
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				AddBlastUnitToDGV(bu);
			}
			//SourceAddress and Param only need to accept Decimal
			dgvBlastLayer.Columns[(int)BlastEditorColumn.SourceAddress].ValueType = typeof(Decimal);
			dgvBlastLayer.Columns[(int)BlastEditorColumn.Param].ValueType = typeof(Decimal);

			UpdateBlastLayerSize();
			initialized = true;
		}

		private void AddBlastUnitToDGV(BlastUnit bu)
		{

			/*
			 SourceAddress
				BlastByte  = Address
				BlastCheat = Address
				BlastPipe  = Source Address
				BlastVector = Address
			 Param
				BlastByte  = Value
				BlastCheat = Value
				BlastPipe  = Destination Address
				BlastBector = Value
			*/

			//Valid for all types
			bool enabled = bu.IsEnabled;
			bool locked = bu.IsLocked;
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

			dgvBlastLayer.Rows.Add(bu, locked, enabled, GetPrecisionNameFromSize(precision), blastType, blastMode, sourceDomain, sourceAddress, destDomain, destAddress);

			//update the BlastUnit to Cell dico.
			var row = dgvBlastLayer.Rows[dgvBlastLayer.Rows.Count - 1];
			bu2RowDico[bu] = row;

			//Update the precision
			ValidatePrecision(dgvBlastLayer.Rows[dgvBlastLayer.Rows.Count - 1]);
		}


		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				bu2RowDico[bu].Cells[1].Value = true;
				CurrentlyUpdating = false;
				//bu.IsEnabled = true;
			}

			foreach (BlastUnit bu in sk.BlastLayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.BlastLayer.Layer.Count / 2))
			{
				bu2RowDico[bu].Cells[1].Value = false;
				CurrentlyUpdating = false;
				//bu.IsEnabled = false;
			}

		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < sk.BlastLayer.Layer.Count(); i++)
			{
				var bu = sk.BlastLayer.Layer[i];
				bu2RowDico[bu].Cells[1].Value = !((bool)bu2RowDico[bu].Cells[1].Value);
				CurrentlyUpdating = false;
			}
			//sk.BlastLayer.Layer[i].IsEnabled = !sk.BlastLayer.Layer[i].IsEnabled;
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> buToRemove = new List<BlastUnit>();

			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				if (!bu.IsEnabled)
					buToRemove.Add(bu);

			foreach (BlastUnit bu in buToRemove)
			{
				sk.BlastLayer.Layer.Remove(bu);
				dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
				CurrentlyUpdating = false;
			}
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
			{
				bu2RowDico[bu].Cells[1].Value = false;
				CurrentlyUpdating = false;
			}
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				bu2RowDico[bu].Cells[1].Value = true;
				CurrentlyUpdating = false;
			}
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			if (dgvBlastLayer.SelectedRows.Count < 1)
			{
				MessageBox.Show("No rows were selected. Cannot duplicate.");
				return;
			}
			foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
			{
				int pos = row.Index;

				BlastUnit bu = sk.BlastLayer.Layer[pos];
				BlastUnit bu2 = ObjectCopier.Clone(bu);
				sk.BlastLayer.Layer.Insert(pos, bu2);

				AddBlastUnitToDGV(bu2);
			}

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
				{
					sk.BlastLayer.Layer.Remove(bu);
					dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
					CurrentlyUpdating = false;
				}
			}
		}

		private void dgvBlastLayer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastLayer == null || dgvBlastLayer.SelectedCells == null)
				return;

			if (!CurrentlyUpdating)
			{
				CurrentlyUpdating = true;
				UpdateBlastUnitFromRow(dgvBlastLayer.Rows[e.RowIndex]);
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
					bb.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bb.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value));
					bb.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					Enum.TryParse(row.Cells["dgvBUMode"].Value.ToString().ToUpper(), out bb.Type);
					row.Cells["dgvBlastUnitReference"].Value = bb;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastCheat":
					BlastCheat bc = (BlastCheat)row.Cells["dgvBlastUnitReference"].Value;
					bc.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bc.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bc.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value));
					bc.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					if (row.Cells["dgvBUMode"].Value.ToString() == "Freeze")
						bc.IsFreeze = true;
					row.Cells["dgvBlastUnitReference"].Value = bc;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastPipe":
					BlastPipe bp = (BlastPipe)row.Cells["dgvBlastUnitReference"].Value;
					bp.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bp.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bp.PipeAddress = Convert.ToInt64(row.Cells["dgvParam"].Value);
					bp.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					bp.PipeDomain = Convert.ToString(row.Cells["dgvParamDomain"].Value);
					bp.TiltValue = Convert.ToInt32(row.Cells["dgvBUMode"].Value.ToString());
					row.Cells["dgvBlastUnitReference"].Value = bp;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastVector":
					BlastVector bv = (BlastVector)row.Cells["dgvBlastUnitReference"].Value;
					bv.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bv.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bv.Values = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value));
					bv.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
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
						(row.Cells["dgvParam"] as DataGridViewNumericUpDownCell).Maximum = Byte.MaxValue;
					}
					else if (row.Cells["dgvPrecision"].Value.ToString() == "16-bit")
					{
						(row.Cells["dgvParam"] as DataGridViewNumericUpDownCell).Maximum = UInt16.MaxValue;
					}
					else
					{
						(row.Cells["dgvParam"] as DataGridViewNumericUpDownCell).Maximum = UInt32.MaxValue;
					}
					break;
				case "RTC.BlastPipe":
					//Todo set this to the valid maximum address
					(row.Cells["dgvParam"] as DataGridViewNumericUpDownCell).Maximum = Int32.MaxValue;
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

		private void PopulateDomainContextMenu(int column, int row)
		{
			cmsDomain.Items.Clear();

			foreach (string domain in domains)
			{
				//cmsDomain.Items.Add(domain, null, );
				(cmsDomain.Items.Add(domain, null, new EventHandler((ob, ev) =>
				{
					dgvBlastLayer[column, row].Value = domain;
				})) as ToolStripMenuItem).Enabled = true;
			}
		}

		private void PopulateBlastUnitModeContextMenu(int column, int row)
		{
			cmsBlastUnitMode.Items.Clear();

			foreach (BlastByteType type in Enum.GetValues(typeof(BlastByteType)))
			{
				//cmsDomain.Items.Add(domain, null, );
				(cmsBlastUnitMode.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
				{
					dgvBlastLayer[column, row].Value = type.ToString();
				})) as ToolStripMenuItem).Enabled = true;
			}
			cmsBlastUnitMode.Items.Add(new ToolStripSeparator());
			foreach (BlastCheatType type in Enum.GetValues(typeof(BlastCheatType)))
			{
				//cmsDomain.Items.Add(domain, null, );
				(cmsBlastUnitMode.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
				{
					dgvBlastLayer[column, row].Value = type.ToString();
				})) as ToolStripMenuItem).Enabled = true;
			}
		}
		private void PopulateBlastUnitTypeContextMenu(int column, int row)
		{
			cmsBlastUnitType.Items.Clear();

			//Adding these by hand

			(cmsBlastUnitType.Items.Add("RTC.BlastByte", null, new EventHandler((ob, ev) =>
			{
				dgvBlastLayer[column, row].Value = "RTC.BlastByte";
			})) as ToolStripMenuItem).Enabled = true;
			(cmsBlastUnitType.Items.Add("RTC.BlastCheat", null, new EventHandler((ob, ev) =>
			{
				dgvBlastLayer[column, row].Value = "RTC.BlastByte";
			})) as ToolStripMenuItem).Enabled = true;
			(cmsBlastUnitType.Items.Add("RTC.BlastPipe", null, new EventHandler((ob, ev) =>
			{
				dgvBlastLayer[column, row].Value = "RTC.BlastByte";
			})) as ToolStripMenuItem).Enabled = true;

		}

		private void PopulateColumnHeaderContextMenu(int column)
		{
			cmsColumnHeader.Items.Clear();

			//Adding these by hand

			(cmsBlastUnitType.Items.Add("Change Selected Rows", null, new EventHandler((ob, ev) =>
			{
			})) as ToolStripMenuItem).Enabled = true;
		}

		private void dgvBlastLayer_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int currentMouseOverColumn = dgvBlastLayer.HitTest(e.X, e.Y).ColumnIndex;
				int currentMouseOverRow = dgvBlastLayer.HitTest(e.X, e.Y).RowIndex;

				//BlastUnitType
				if (currentMouseOverColumn == (int)BlastEditorColumn.BlastUnitType)
				{
					PopulateBlastUnitTypeContextMenu(currentMouseOverColumn, currentMouseOverRow);
					cmsBlastUnitType.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}

				//BlastUnitMode
				if (currentMouseOverColumn == (int)BlastEditorColumn.BUMode)
				{

					PopulateBlastUnitModeContextMenu(currentMouseOverColumn, currentMouseOverRow);
					cmsBlastUnitMode.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}

				//Domain1 Domain2
				if (currentMouseOverColumn == (int)BlastEditorColumn.SourceAddressDomain || currentMouseOverColumn == (int)BlastEditorColumn.ParamDomain)
				{
					PopulateDomainContextMenu(currentMouseOverColumn, currentMouseOverRow);
					cmsDomain.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}
			}
		}

		private void dgvBlastLayer_Sorted(object sender, EventArgs e)
		{
			switch (dgvBlastLayer.SortedColumn.Name)
			{
				case "dgvSourceAddressDomain":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Domain).ToList();
					break;
				case "dgvSourceAddress":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Address).ToList();
					break;
				case "dgvParamDomain":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParamDomain(it)).ToList();
					break;
				case "dgvParam":
					sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParamValue(it)).ToList();
					break;
				default:
					break;
			}
		}

		private long GetParamValue(BlastUnit bu)
		{
			if (bu is BlastByte || bu is BlastCheat)
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
		private string GetParamDomain(BlastUnit bu)
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
			foreach (DataGridViewColumn column in dgvBlastLayer.Columns)
			{
				if (column.CellType.Name == "DataGridViewNumericUpDownCell")
				{
					DataGridViewNumericUpDownColumn _column = column as DataGridViewNumericUpDownColumn;
					_column.Hexadecimal = cbUseHex.Checked;
				}
			}
		}

		private void dgvBlastLayer_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateBlastLayerSize();
		}
		private void dgvBlastLayer_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateBlastLayerSize();
		}
		private void dgvBlastLayer_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
		{
			sk.BlastLayer.Layer.Remove((BlastUnit)e.Row.Cells[0].Value);
			UpdateBlastLayerSize();
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			if (dgvBlastLayer.SelectedRows.Count < 1)
			{
				MessageBox.Show("No rows were selected. Cannot remove.");
				return;
			}
			foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
			{
				int pos = row.Index;

				BlastUnit bu = sk.BlastLayer.Layer[pos];

				sk.BlastLayer.Layer.Remove(bu);
				dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
				CurrentlyUpdating = false;
			}
		}
		public DialogResult getSearchBox(string title, string promptText, ref decimal value)
		{
			Form form = new Form();
			Label label = new Label();
			NumericUpDown updown = new NumericUpDown();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			updown.Value = value;



			updown.Maximum = Int32.MaxValue;
			updown.Hexadecimal = cbUseHex.Checked;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			updown.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			updown.Anchor = updown.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, updown, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = updown.Value;
			return dialogResult;
		}

		private void btnSearchSourceAddress_Click(object sender, EventArgs e)
		{
			decimal value = -1;
			if (this.getSearchBox("Search for an address", "Enter an address:	", ref value) == DialogResult.OK)
			{

			}
		}

		private void saveToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RTC_BlastLayerTools.SaveBlastLayerToFile(sk.BlastLayer);
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastLayer temp = RTC_BlastLayerTools.LoadBlastLayerFromFile();
			if (temp != null)
			{
				sk.BlastLayer = temp;
				RefreshBlastLayer();
			}
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastLayer temp = RTC_BlastLayerTools.LoadBlastLayerFromFile();
			if (temp != null)
			{
				foreach (BlastUnit bu in temp.Layer)
					sk.BlastLayer.Layer.Add(bu);
				RefreshBlastLayer();
			}
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void runOriginalSavestateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			originalsk.RunOriginal();
		}

		private void replaceSavestateFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StashKey temp = RTC_StockpileManager.getCurrentSavestateStashkey();
			if(temp == null)
			{
				MessageBox.Show("There is no savestate selected in the glitch harvester, or the current selected box is empty");
				return;
			}
			sk.StateFilename = temp.StateFilename;
			sk.StateShortFilename = temp.StateShortFilename;
			sk.StateData = temp.StateData;
		}

		private void replaceSavestateFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = "state";
			ofd.Title = "Open Savestate File";
			ofd.Filter = "state files|*.state";
			ofd.RestoreDirectory = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;
			sk.StateFilename = filename;
			sk.StateShortFilename = filename.Substring(filename.LastIndexOf("\\") + 1, filename.Length - (filename.LastIndexOf("\\") + 1));
		}

		private void saveSavestateToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;
			SaveFileDialog ofd = new SaveFileDialog();
			ofd.DefaultExt = "state";
			ofd.Title = "Save Savestate File";
			ofd.Filter = "state files|*.state";
			ofd.RestoreDirectory = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;
			File.Copy(sk.StateFilename, filename);
		}

		private void runRomWithoutBlastlayerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RTC_Core.LoadRom(sk.RomFilename, true);
		}

		private void replaceRomFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StashKey temp = RTC_StockpileManager.getCurrentSavestateStashkey();
			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the glitch harvester, or the current selected box is empty");
				return;
			}
			sk.RomFilename = temp.RomFilename;
			sk.RomData = temp.RomData;
		}

		private void replaceRomFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.DefaultExt = "state";
			ofd.Title = "Save Savestate File";
			ofd.Filter = "state files|*.state";
			ofd.RestoreDirectory = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;

			sk.RomFilename = filename;
		}

		private void bakeROMBlastunitsToFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.DefaultExt = "rom";
			sfd.Title = "Save rom File";
			sfd.Filter = "rom files|*.rom";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				filename = sfd.FileName.ToString();
			}
			else
				return;
			RomParts rp = RTC_MemoryDomains.GetRomParts(sk.SystemName, sk.RomFilename);
			

			File.Copy(sk.RomFilename, filename);
			using (FileStream output = new FileStream(filename, FileMode.Open))
				foreach (BlastByte bu in sk.BlastLayer.Layer)
				{
					if(bu.Domain == rp.primarydomain || bu.Domain == rp.seconddomain)
					{
						output.Position = bu.Address;
						output.Write(bu.Value, 0, bu.Value.Length);
					}
						
				}
		}
	}
}
/*
 * The re-written blast editor for RTC 3.20
 * Allows for direct manipulation of the blastunits within a blastlayer
 * While I really should have used a data bound DGV, I went in blind not knowing about them
 * There's a hidden column in the dgv (index 0) which contains a reference to the blastunit
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_BlastEditor_Form : Form
	{
		/*
		 * Column Indexes - Updated 4/8/2018
		 *
		 * 0 DgvBlastUnitReference
		 * 1 DgvBlastUnitLocked.
		 * 2 DgvBlastUnitEnabled
		 * 3 DgvPrecision
		 * 4 DgvBlastUnitType
		 * 5 DgvBlastUnitMode
		 * 6 DgvSourceAddressDomain
		 * 7 DgvSourceAddress
		 * 8 DgvParamDomain
		 * 9 DgvParam 
		 * 10 DgvNoteButton
		 */

		private enum BlastEditorColumn
		{
			DgvBlastUnitReference,
			DgvBlastUnitLocked,
			DgvBlastUnitEnabled,
			DgvPrecision,
			DgvBlastUnitType,
			DgvBlastUnitMode,
			DgvSourceAddressDomain,
			DgvSourceAddress,
			DgvParamDomain,
			DgvParam,
			DvgNoteButton,
		}

		private StashKey sk = null;
		private StashKey originalSk = null;
		private bool initialized = false;
		private bool currentlyUpdating = false;
		private ContextMenuStrip cmsBlastEditor = new ContextMenuStrip();
		private string[] domains;
		private int searchOffset = 0;
		private string searchValue = "";
		private string searchColumn = "";
		public string CurrentBlastLayerFile = "";
		private Guid? scrollToken = null;

		private Dictionary<BlastUnit, DataGridViewRow> bu2RowDico = null;

		public RTC_BlastEditor_Form()
		{
			InitializeComponent();

			dgvBlastLayer.DoubleBuffered(true);
			this.dgvBlastLayer.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;

		}

		private void RTC_BlastEditorForm_Load(object sender, EventArgs e)
		{
			RTC_Core.SetRTCColor(RTC_Core.generalColor, this);

			KeyDown += RTC_NewBlastEditor_Form_KeyDown;
			dgvBlastLayer.CellValidating += dgvBlastLayer_CellValidating;
			dgvBlastLayer.CellValueChanged += dgvBlastLayer_CellValueChanged;
			dgvBlastLayer.MouseClick += dgvBlastLayer_MouseClick;
			dgvBlastLayer.CellClick += dgvBlastLayer_CellClick;
			dgvBlastLayer.RowsAdded += dgvBlastLayer_RowsAdded;
			dgvBlastLayer.RowsRemoved += dgvBlastLayer_RowsRemoved;
			dgvBlastLayer.UserDeletingRow += dgvBlastLayer_UserDeletingRow;
			dgvBlastLayer.UserDeletedRow += dgvBlastLayer_UserDeletedRow;
			dgvBlastLayer.Sorted += dgvBlastLayer_Sorted;


			//For the combobox for shifting blast units, we want an object that contains the column name and the readable name
			//Do this here as I can't get it working in the designer
			cbShiftBlastlayer.DisplayMember = "Text";
			cbShiftBlastlayer.ValueMember = "Value";
			cbShiftBlastlayer.Items.Add(new { Text = "Source Address", Value = "dgvSourceAddress" });
			cbShiftBlastlayer.Items.Add(new { Text = "Parameter Value", Value = "dgvParam" });
			cbShiftBlastlayer.SelectedIndex = 0;
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk?.BlastLayer?.Layer == null)
				return;

			bu2RowDico = new Dictionary<BlastUnit, DataGridViewRow>();
			originalSk = (StashKey)_sk.Clone();
			sk = (StashKey)_sk.Clone();
			RefreshBlastLayer();
			domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

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

			dgvBlastLayer.SuspendLayout();

			bu2RowDico = new Dictionary<BlastUnit, DataGridViewRow>();

			//Populate the different rows.
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				AddBlastUnitToDgv(bu);
			}
			//SourceAddress and Param only need to accept Decimal
			if (dgvBlastLayer != null)
			{
				dgvBlastLayer.Columns["dgvSourceAddress"].ValueType = typeof(decimal);
				dgvBlastLayer.Columns["dgvParam"].ValueType = typeof(decimal);
			}

			UpdateBlastLayerSize();
			RefreshNoteIcons();

			dgvBlastLayer.ResumeLayout();
			initialized = true;
		}

		private void InsertBlastUnitToBlastLayerAndDgv(int index, BlastUnit bu)
		{
			sk.BlastLayer.Layer.Add(bu);
			InsertBlastUnitToDgv(index, bu);
		}

		private void AddBlastUnitToDgv(BlastUnit bu)
		{
			InsertBlastUnitToDgv(dgvBlastLayer.Rows.Count, bu);
		}

		private void InsertBlastUnitToDgv(int index, BlastUnit bu)
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
			decimal sourceAddress = 0;

			//This is both the value and the dest address depending on the engine
			decimal param = 0;
			string blastMode = "";

			//add DateTimePicker into the control collection of the DataGridView

			switch (bu)
			{
				case BlastByte bb:
					precision = bb.Value.Length;
					sourceAddress = Convert.ToDecimal(bb.Address);
					sourceDomain = bb.Domain;
					param = RTC_Extensions.GetDecimalValue(bb.Value, true);
					blastMode = Convert.ToString(bb.Type);
					break;
				case BlastCheat bc:
					precision = bc.Value.Length;
					sourceAddress = bc.Address;
					sourceDomain = bc.Domain;
					param = RTC_Extensions.GetDecimalValue(bc.Value, true);
					blastMode = bc.IsFreeze ? "FREEZE" : "HELLGENIE";
					break;
				case BlastPipe bp:
					precision = bp.PipeSize;
					sourceAddress = bp.Address;
					sourceDomain = bp.Domain;
					destDomain = bp.PipeDomain;
					param = bp.PipeAddress;
					blastMode = Convert.ToString(bp.TiltValue);
					break;
				case BlastVector bv:
					precision = bv.Values.Length;
					sourceAddress = Convert.ToDecimal(bv.Address);
					sourceDomain = bv.Domain;
					param = RTC_Extensions.GetDecimalValue(bv.Values, bv.BigEndian);
					blastMode = Convert.ToString(bv.Type);
					break;
			}

			dgvBlastLayer.Rows.Insert(index, bu, locked, enabled, GetPrecisionNameFromSize(precision), blastType, blastMode, sourceDomain, sourceAddress, destDomain, param);

			//update the BlastUnit to Cell dico.
			DataGridViewRow row = dgvBlastLayer.Rows[index];
			bu2RowDico[bu] = row;

			//Update the precision
			UpdateRowPrecision(dgvBlastLayer.Rows[index]);
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
				{
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = true;
					currentlyUpdating = false;
				}

			}
			foreach (BlastUnit bu in sk.BlastLayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.BlastLayer.Layer.Count / 2))
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
				{
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = false;
					currentlyUpdating = false;
				}
			}
		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < sk.BlastLayer.Layer.Count(); i++)
			{
				BlastUnit bu = sk.BlastLayer.Layer[i];
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = !((bool)bu2RowDico[bu].Cells["dgvBlastEnabled"].Value);
			}
			//sk.BlastLayer.Layer[i].IsEnabled = !sk.BlastLayer.Layer[i].IsEnabled;
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> buToRemove = new List<BlastUnit>();

			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				if (!bu.IsEnabled && !bu.IsLocked)
					buToRemove.Add(bu);

			foreach (BlastUnit bu in buToRemove)
			{
				RemoveBlastUnitFromBlastLayerAndDgv(bu);
			}
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			if (sk.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to be able to load.");
				return;
			}
			BlastLayer bl = new BlastLayer();

			foreach (DataGridViewRow row in dgvBlastLayer.Rows)
			{
				BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
				if (bu.IsEnabled)
					bl.Layer.Add(bu);
			}

			StashKey newSk = (StashKey)sk.Clone();
			newSk.BlastLayer = (BlastLayer)bl.Clone();

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

			(bl.Clone() as BlastLayer)?.Apply();
		}

		private void btnSendToStash_Click(object sender, EventArgs e)
		{
			if (sk.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to send this to the stash.");
				return;
			}
			StashKey newSk = (StashKey)sk.Clone();
			//newSk.Key = RTC_Core.GetRandomKey();
			//newSk.Alias = null;
			
			RTC_StockpileManager.StashHistory.Add(newSk);

			RTC_Core.ghForm.RefreshStashHistory();
			RTC_Core.ghForm.dgvStockpile.ClearSelection();
			RTC_Core.ghForm.lbStashHistory.ClearSelected();

			RTC_Core.ghForm.DontLoadSelectedStash = true;   
			RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;
			RTC_StockpileManager.currentStashkey = RTC_StockpileManager.StashHistory[RTC_Core.ghForm.lbStashHistory.SelectedIndex];

		}

		private void btnDisableEverything_Click(object sender, EventArgs e)
		{
			dgvBlastLayer.ClearSelection();
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = false;
			}
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			dgvBlastLayer.ClearSelection();
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = true;
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
				sk.BlastLayer.Layer.Insert(pos + 1, bu2);
				InsertBlastUnitToDgv(pos + 1, bu2);
				SetNoteIcon(bu2);

				//AddBlastUnitToDGV(bu2);
			}
		}

		private void dgvBlastLayer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastLayer == null)
				return;

			if (!currentlyUpdating)
			{
				currentlyUpdating = true;

				try
				{
					if((BlastEditorColumn)e.ColumnIndex == BlastEditorColumn.DgvPrecision)
						UpdateRowPrecision(dgvBlastLayer.Rows[e.RowIndex]);
					//Changing the blast unit type
					if ((BlastEditorColumn)e.ColumnIndex == BlastEditorColumn.DgvBlastUnitType)
						ReplaceBlastUnitFromRow(dgvBlastLayer.Rows[e.RowIndex]);
					else
						UpdateBlastUnitFromRow(dgvBlastLayer.Rows[e.RowIndex]);
				}
				catch (Exception ex)
				{
					currentlyUpdating = false;

					throw new System.Exception("Something went wrong in when updating the blastunit \n" +
					"Make sure your input is valid.\n\n" +
					"If your input was valid, you should probably send a copy of this error and what you did to cause it to the RTC devs.\n\n" +
					ex.ToString());
				}
			}
		}

		private void dgvBlastLayer_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			/*
			DataGridViewRow row = dgvBlastLayer.Rows[e.RowIndex];
			DataGridViewColumn column = dgvBlastLayer.Columns[e.ColumnIndex];

			//Make sure the max and min is updated if the precision changed
			if (column.HeaderText.Equals("Precision"))
				UpdateRowPrecision(dgvBlastLayer.Rows[e.RowIndex]);

			*/
		}

		//Validates that a blast unit is valid from a dgv row
		private bool ValidateBlastUnitFromRow(DataGridViewRow row)
		{
			return true;
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

		private void ReplaceBlastUnitFromRow(DataGridViewRow row)
		{
			BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
			string column = row.Cells["dgvBlastUnitType"].Value.ToString();
			int index = row.Index;
			bu2RowDico[bu] = row;
			try
			{
				switch (column)
				{
					case "RTC.BlastByte":
						BlastByte bb = (BlastByte)bu.ConvertBlastUnit(typeof(BlastByte));
						sk.BlastLayer.Layer.Insert(index, bb);
						InsertBlastUnitToDgv(index, bb);
						currentlyUpdating = false;
						break;

					case "RTC.BlastCheat":
						BlastCheat bc = (BlastCheat)bu.ConvertBlastUnit(typeof(BlastCheat));
						sk.BlastLayer.Layer.Insert(index, bc);
						InsertBlastUnitToDgv(index, bc);
						currentlyUpdating = false;
						break;

					case "RTC.BlastPipe":
						BlastPipe bp = (BlastPipe)bu.ConvertBlastUnit(typeof(BlastPipe));
						sk.BlastLayer.Layer.Insert(index, bp);
						InsertBlastUnitToDgv(index, bp);
						currentlyUpdating = false;
						break;

					case "RTC.BlastVector":
						BlastVector bv = (BlastVector)bu.ConvertBlastUnit(typeof(BlastVector));
						sk.BlastLayer.Layer.Insert(index, bv);
						InsertBlastUnitToDgv(index, bv);
						currentlyUpdating = false;
						break;

					default:
						currentlyUpdating = false;
						throw new Exception("Invalid BlastUnit Type");
				}
				//Remove the old one
				RemoveBlastUnitFromBlastLayerAndDgv(bu);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void RemoveBlastUnitFromBlastLayerAndDgv(BlastUnit bu)
		{
			//Remove it from the dgv
			dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
			//Remove it from the dictionary
			bu2RowDico.Remove(bu);
			//Remove it from the blastlayer
			sk.BlastLayer.Layer.Remove(bu);

			UpdateBlastLayerSize();
		}

		private void RemoveBlastUnitFromBlastLayerAndDgv(DataGridViewRow row)
		{
			BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
			//Remove it from the blastlayer
			sk.BlastLayer.Layer.Remove(bu);
			//Remove it from the dictionary
			bu2RowDico.Remove(bu);
			//Remove it from the dgv
			dgvBlastLayer.Rows.Remove(row);

			UpdateBlastLayerSize();
		}

		private void RemoveBlastUnitFromBlastLayer(DataGridViewRow row)
		{
			BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
			//Remove it from the blastlayer
			sk.BlastLayer.Layer.Remove(bu);
			//Remove it from the dictionary
			bu2RowDico.Remove(bu);

			UpdateBlastLayerSize();
		}

		private void UpdateBlastUnitFromRow(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value)
			{
				case "RTC.BlastByte":
					BlastByte bb = (BlastByte)row.Cells["dgvBlastUnitReference"].Value;
					bb.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bb.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bb.Value = RTC_Extensions.GetByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), true);
					bb.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					Enum.TryParse(row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper(), out bb.Type);
					bb.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));

					row.Cells["dgvBlastUnitReference"].Value = bb;
					currentlyUpdating = false;
					break;

				case "RTC.BlastCheat":
					BlastCheat bc = (BlastCheat)row.Cells["dgvBlastUnitReference"].Value;
					bc.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bc.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bc.Value = RTC_Extensions.GetByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), true);
					bc.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					if (row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper() == "FREEZE")
						bc.IsFreeze = true;
					else
						bc.IsFreeze = false;
					bc.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));

					row.Cells["dgvBlastUnitReference"].Value = bc;
					currentlyUpdating = false;
					break;

				case "RTC.BlastPipe":
					BlastPipe bp = (BlastPipe)row.Cells["dgvBlastUnitReference"].Value;
					bp.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bp.PipeSize = GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString());
					bp.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bp.PipeAddress = Convert.ToInt64(row.Cells["dgvParam"].Value);
					bp.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					bp.PipeDomain = Convert.ToString(row.Cells["dgvParamDomain"].Value);
					bp.TiltValue = Convert.ToInt32(row.Cells["dgvBlastUnitMode"].Value.ToString());
					bp.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));

					row.Cells["dgvBlastUnitReference"].Value = bp;
					currentlyUpdating = false;
					break;

				case "RTC.BlastVector":
					BlastVector bv = (BlastVector)row.Cells["dgvBlastUnitReference"].Value;
					bv.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bv.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bv.Values = RTC_Extensions.GetByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), true);
					bv.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					bv.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));
					Enum.TryParse(row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper(), out bv.Type);

					row.Cells["dgvBlastUnitReference"].Value = bv;
					currentlyUpdating = false;
					break;

				default:
					MessageBox.Show("You had an invalid blast unit type! Check your input. The invalid unit is: " + row.Cells["dgvBlastUnitType"].Value);
					currentlyUpdating = false;
					break;
			}
		}

		private void UpdateRowPrecision(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value.ToString())
			{
				case "RTC.BlastByte":
				case "RTC.BlastCheat":
					switch (row.Cells["dgvPrecision"].Value.ToString())
					{
						case "8-bit":
							((DataGridViewNumericUpDownCell)row.Cells["dgvParam"]).Maximum = Byte.MaxValue;
							break;
						case "16-bit":
							((DataGridViewNumericUpDownCell)row.Cells["dgvParam"]).Maximum = UInt16.MaxValue;
							break;
						case "32-bit":
							((DataGridViewNumericUpDownCell)row.Cells["dgvParam"]).Maximum = UInt32.MaxValue;
							break;
					}
					break;
				case "RTC.BlastPipe":
					//Todo set this to the valid maximum address
					((DataGridViewNumericUpDownCell)row.Cells["dgvParam"]).Maximum = UInt32.MaxValue;
					break;

				default:
					break;
			}
		}

		private UInt32 GetPrecisionMaxValue(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value.ToString())
			{
				case "RTC.BlastByte":
				case "RTC.BlastCheat":
					switch (row.Cells["dgvPrecision"].Value.ToString())
					{
						case "8-bit":
							return Byte.MaxValue;
						case "16-bit":
							return UInt16.MaxValue;
						case "32-bit":
							return UInt32.MaxValue;
						default:
							break;
					}
					break;
				case "RTC.BlastPipe":
					//Todo set this to the valid maximum address
					return Int32.MaxValue;
				default:
					break;
			}
			//No precision set?
			return UInt32.MaxValue;
		}

		private void PopulateDomainContextMenu(int column)
		{
			domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

			foreach (string domain in domains)
			{
				((ToolStripMenuItem)cmsBlastEditor.Items.Add(domain, null, new EventHandler((ob, ev) =>
				{
					foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
						selected.Cells[column].Value = domain;
				}))).Enabled = true;
			}
		}

		private void PopulateAddressContextMenu(int column, int row)
		{
			if (row == -1)
			{
				return;
			}
			string domain = dgvBlastLayer["dgvSourceAddressDomain", row].Value.ToString();
			long address = Convert.ToInt64(dgvBlastLayer["dgvSourceAddress", row].Value);

			{
				((ToolStripMenuItem)cmsBlastEditor.Items.Add("Open Selected Address in Hex Editor", null, new EventHandler((ob, ev) =>
				{
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BIZHAWK_OPEN_HEXEDITOR_ADDRESS) { objectValue = new object[] { domain, address } });
				}))).Enabled = true;
			}
			
		}

		private void PopulateParamContextMenu(int column, int row)
		{
			if (row == -1)
			{
				return;
			}

			if (dgvBlastLayer["dgvBlastUnitType", row].Value.ToString() == "RTC.BlastPipe")
			{
				string domain = dgvBlastLayer["dgvParamDomain", row].Value.ToString();
				long address = Convert.ToInt64(dgvBlastLayer["dgvParam", row].Value);

				{
					((ToolStripMenuItem)cmsBlastEditor.Items.Add("Open Selected Address in Hex Editor", null, new EventHandler((ob, ev) =>
					{
						RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BIZHAWK_OPEN_HEXEDITOR_ADDRESS) { objectValue = new object[] { domain, address } });
					}))).Enabled = true;
				}
			}

			{
				((ToolStripMenuItem)cmsBlastEditor.Items.Add("Flip Param Bytes in Selected Rows", null, new EventHandler((ob, ev) =>
				{
					foreach (DataGridViewRow _row in dgvBlastLayer.SelectedRows)
					{
						byte[] temp = RTC_Extensions.GetByteArrayValue(GetPrecisionSizeFromName(_row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(_row.Cells["dgvParam"].Value.ToString()), true);
						temp.FlipBytes();
						_row.Cells["dgvParam"].Value = RTC_Extensions.GetDecimalValue(temp, true);
					}
				}))).Enabled = true;

				((ToolStripMenuItem)cmsBlastEditor.Items.Add("Reroll Param in Selected Rows", null, new EventHandler((ob, ev) =>
				{
					foreach (DataGridViewRow _row in dgvBlastLayer.SelectedRows)
					{
						//Since we want the reroll to obey the engine params, we use the blastunit's reroll functionality
						BlastUnit bu = ((BlastUnit)(_row.Cells["dgvBlastUnitReference"].Value));
						bu.Reroll();
						_row.Cells["dgvParam"].Value = GetParamValue(bu);

						//If it's a BlastPipe, we need to make sure we also update the domain
						if (bu is BlastPipe bp)
							_row.Cells["dgvParamDomain"].Value = bp.PipeDomain;


					}
				}))).Enabled = true;
			}
		}

		private void PopulateBlastUnitModeContextMenu(int column, int row)
		{
			if (row == -1)
			{
				return;
			}

			if (dgvBlastLayer["dgvBlastUnitType", row].Value.ToString() == "RTC.BlastByte")
			{
				foreach (BlastByteType type in Enum.GetValues(typeof(BlastByteType)))
				{
					//cmsDomain.Items.Add(domain, null, );
					((ToolStripMenuItem)cmsBlastEditor.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
					{
						foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
							selected.Cells[column].Value = type.ToString();
					}))).Enabled = true;
				}
			}
			else if (dgvBlastLayer["dgvBlastUnitType", row].Value.ToString() == "RTC.BlastCheat")
			{
				foreach (BlastCheatType type in Enum.GetValues(typeof(BlastCheatType)))
				{
					//cmsDomain.Items.Add(domain, null, );
					((ToolStripMenuItem)cmsBlastEditor.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
					{
						foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
							selected.Cells[column].Value = type.ToString();
					}))).Enabled = true;
				}
			}
		}

		private void PopulateBlastUnitTypeContextMenu(int column, int row)
		{
			if (row == -1)
			{
				return;
			}

			//Adding these by hand

			((ToolStripMenuItem)cmsBlastEditor.Items.Add("RTC.BlastByte", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastByte";
			}))).Enabled = true;
			((ToolStripMenuItem)cmsBlastEditor.Items.Add("RTC.BlastCheat", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastCheat";
			}))).Enabled = true;
			((ToolStripMenuItem)cmsBlastEditor.Items.Add("RTC.BlastPipe", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastPipe";
			}))).Enabled = true;
		}

		private void PopulateColumnHeaderContextMenu(int column)
		{
			//Adding these by hand
			//See file header for column details

			((ToolStripMenuItem)cmsBlastEditor.Items.Add("Change Selected Rows", null, new EventHandler((ob, ev) =>
			{
				string newvalue = "";
				decimal decimalvalue = 0;

				switch (column)
				{
					//Blast unit locked
					//Blast unit enabled
					case (int)BlastEditorColumn.DgvBlastUnitLocked:
					case (int)BlastEditorColumn.DgvBlastUnitEnabled:
						foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
						{
							row.Cells[column].Value = !(bool)(row.Cells[column].Value);
						}
						break;

					case (int)BlastEditorColumn.DgvPrecision:
						string[] options = { "8-bit", "16-bit", "32-bit" };
						if (RTC_Extensions.GetComboInputBox("Replace Selected Rows", "Replacement input: ", options, ref newvalue) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = newvalue;
							}
						break;

					/* 4 
					 * 5 
					 * 6 
					 * 8 
					 */
					case (int)BlastEditorColumn.DgvBlastUnitType:
					case (int)BlastEditorColumn.DgvBlastUnitMode:
					case (int)BlastEditorColumn.DgvSourceAddressDomain:
					case (int)BlastEditorColumn.DgvParamDomain:
						if (RTC_Extensions.GetInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref newvalue) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = newvalue;
							}
						break;
					//7 dvgSourceAddress
					case (int)BlastEditorColumn.DgvSourceAddress:
						if (RTC_Extensions.GetInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref decimalvalue, RTC_Core.UseHexadecimal) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = decimalvalue;
							}
						break;
					//9 dvgParam
					//Needs to be capped at the precision so it's separate from case 7 (the address)
					case (int)BlastEditorColumn.DgvParam:
						if (RTC_Extensions.GetInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref decimalvalue, RTC_Core.UseHexadecimal) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								uint precision = GetPrecisionMaxValue(row);
								if (decimalvalue > precision)
									decimalvalue = precision;
								row.Cells[column].Value = decimalvalue;
							}
						break;

					default:
						break;
				}
			}))).Enabled = true;
		}

		private void dgvBlastLayer_MouseClick(object sender, MouseEventArgs e)
		{
			int currentMouseOverColumn = dgvBlastLayer.HitTest(e.X, e.Y).ColumnIndex;
			int currentMouseOverRow = dgvBlastLayer.HitTest(e.X, e.Y).RowIndex;

			if (e.Button == MouseButtons.Left)
			{
				if (currentMouseOverRow == -1)
				{
					dgvBlastLayer.EndEdit();
					dgvBlastLayer.ClearSelection();
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				cmsBlastEditor = new ContextMenuStrip();

				PopulateColumnHeaderContextMenu(currentMouseOverColumn);

				switch (currentMouseOverColumn)
				{	//BlastUnitType
					case (int)BlastEditorColumn.DgvBlastUnitType:
						cmsBlastEditor.Items.Add(new ToolStripSeparator());
						PopulateBlastUnitTypeContextMenu(currentMouseOverColumn, currentMouseOverRow);
						break;
					//BlastUnitMode
					case (int)BlastEditorColumn.DgvBlastUnitMode:
						cmsBlastEditor.Items.Add(new ToolStripSeparator());
						PopulateBlastUnitModeContextMenu(currentMouseOverColumn, currentMouseOverRow);
						break;

					//Domain1 Domain2
					case (int)BlastEditorColumn.DgvSourceAddressDomain:
					case (int)BlastEditorColumn.DgvParamDomain:
						cmsBlastEditor.Items.Add(new ToolStripSeparator());
						PopulateDomainContextMenu(currentMouseOverColumn);
						break;
					//Source Address
					case (int)BlastEditorColumn.DgvSourceAddress:
						cmsBlastEditor.Items.Add(new ToolStripSeparator());
						PopulateAddressContextMenu(currentMouseOverColumn, currentMouseOverRow);
						break;
					//Param
					case (int)BlastEditorColumn.DgvParam:
						cmsBlastEditor.Items.Add(new ToolStripSeparator());
						PopulateParamContextMenu(currentMouseOverColumn, currentMouseOverRow);
						break;
				}


				cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
			}
		}

		//We manually sort the blastlayer alongside the DGV
		private void dgvBlastLayer_Sorted(object sender, EventArgs e)
		{
			if (dgvBlastLayer.SortOrder == SortOrder.Ascending)
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
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(GetParamDomain).ToList();
						break;

					case "dgvParam":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(GetParamValue).ToList();
						break;

					default:
						break;
				}
			}
			else
			{
				switch (dgvBlastLayer.SortedColumn.Name)
				{
					case "dgvSourceAddressDomain":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Domain).Reverse().ToList();
						break;

					case "dgvSourceAddress":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => it.Address).Reverse().ToList();
						break;

					case "dgvParamDomain":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(GetParamDomain).Reverse().ToList();
						break;

					case "dgvParam":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(GetParamValue).Reverse().ToList();
						break;

					default:
						break;
				}
			}
		}

		private long GetParamValue(BlastUnit bu)
		{
			switch (bu)
			{
				case BlastByte bb:
				{
					decimal value = RTC_Extensions.GetDecimalValue(bb.Value, true);
					return (long)value;
				}
				case BlastCheat bc:
				{
					decimal value = RTC_Extensions.GetDecimalValue(bc.Value, true);
					return (long)value;
				}
				case BlastPipe bp:
					return bp.PipeAddress;
				case BlastVector bv:
				{
					decimal value = RTC_Extensions.GetDecimalValue(bv.Values, bv.BigEndian);
					return (long)value;
				}
			}

			return 0;
		}

		private string GetParamDomain(BlastUnit bu)
		{
			return bu is BlastPipe bp ? bp.PipeDomain : null;
		}

		public void SetHexadecimal (bool useHex)
		{
			updownShiftBlastLayerAmount.Hexadecimal = useHex;
			foreach (DataGridViewColumn column in dgvBlastLayer.Columns)
			{
				if (column.CellType.Name == "DataGridViewNumericUpDownCell")
				{
					if (column is DataGridViewNumericUpDownColumn _column)
						_column.Hexadecimal = useHex;
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
			if ((bool)e.Row.Cells["dgvBlastUnitLocked"].Value)
			{
				e.Cancel = true;
				return;
			}
			//Make sure the row gets removed from the blastlayer as well
			RemoveBlastUnitFromBlastLayer(e.Row);
		}

		private void dgvBlastLayer_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			if (dgvBlastLayer.SelectedRows.Count < 1)
			{
				MessageBox.Show("No rows were selected. Cannot remove.");
				return;
			}
			if (Control.ModifierKeys == Keys.Control || (MessageBox.Show("Are you sure you want to remove the selected row(s)?", "Remove Rows", MessageBoxButtons.YesNo) == DialogResult.Yes))
			{
				foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
				{
					if ((bool)row.Cells["dgvBlastUnitLocked"].Value)
						return;

					RemoveBlastUnitFromBlastLayerAndDgv(row);
					currentlyUpdating = false;
				}
			}
		}

		public DialogResult GetSearchBox(string title, string promptText, bool filterColumn = false)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox input = new TextBox();

			Button buttonOk = new Button();
			Button buttonCancel = new Button();
			ComboBox column = new ComboBox();
			//Only draw the column combobox if the user wants the column
			column.Hide();

			if (filterColumn)
			{
				column.DisplayMember = "Text";
				column.ValueMember = "Value";
				column.Items.Add(new { Text = "Source Address", Value = "dgvSourceAddress" });
				column.Items.Add(new { Text = "Parameter Value", Value = "dgvParam" });
				column.Items.Add(new { Text = "Source Address Domain", Value = "dgvSourceAddressDomain" });
				column.Items.Add(new { Text = "Parameter Domain", Value = "dgvParamDomain" });
				column.Items.Add(new { Text = "Blast Unit Type", Value = "dgvBlastUnitType" });
				column.Items.Add(new { Text = "Blast Unit Mode", Value = "dgvBlastUnitMode" });
				column.SelectedIndex = 0;
				column.SetBounds(72, 64, 164, 20);
				column.Show();
			}

			form.Text = title;
			label.Text = promptText;
			//input.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(64, 15, 164, 16);
			input.SetBounds(48, 36, 164, 20);
			buttonOk.SetBounds(96, 98, 75, 23);
			buttonCancel.SetBounds(172, 98, 75, 23);

			label.TextAlign = ContentAlignment.MiddleCenter;
			label.AutoSize = true;
			input.Anchor = input.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(256, 128);
			form.Controls.AddRange(new Control[] { label, input, column, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();

			searchValue = input.Text.ToUpper();
			if (filterColumn)
				searchColumn = (column.SelectedItem as dynamic).Value;
			return dialogResult;
		}

		//Provides a dialog box that searches for a row in the DGV
		private void btnSearchRow_Click(object sender, EventArgs e)
		{
			if (GetSearchBox("Search for a value", "Choose a column and enter a value", true) == DialogResult.OK)
			{
				dgvBlastLayer.ClearSelection();
				SearchDataGridView(dgvBlastLayer);
			}
		}

		private void btnSearchAgain_Click(object sender, EventArgs e)
		{
			dgvBlastLayer.ClearSelection();
			SearchDataGridView(dgvBlastLayer, true);
		}

		private void SearchDataGridView(DataGridView dgv, bool findAgain = false)
		{
			if (searchColumn == "")
				return;

			if (!findAgain)
				searchOffset = 0;

			//Search for the formatted value and return the row
			//We use the formatted value and expect the user to input the correct type.
			while (searchOffset < dgv.RowCount)
			{
				if (dgv[searchColumn, searchOffset].FormattedValue?.ToString() == searchValue)
				{
					dgvBlastLayer.CurrentCell = dgvBlastLayer.Rows[searchOffset].Cells[searchColumn];
					return;
				}
				searchOffset++;
			}

			MessageBox.Show("Reached the end of the Blastlayer and didn't find anything");
			searchOffset = 0;
			return;
		}

		private void saveToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//If there's no blastlayer file already set, don't quicksave
			if (CurrentBlastLayerFile == "")
				RTC_BlastTools.SaveBlastLayerToFile(sk.BlastLayer);
			else
				RTC_BlastTools.SaveBlastLayerToFile(sk.BlastLayer, true);
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RTC_BlastTools.SaveBlastLayerToFile(sk.BlastLayer, false);
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastLayer temp = RTC_BlastTools.LoadBlastLayerFromFile();
			if (temp != null)
			{
				sk.BlastLayer = temp;
				RefreshBlastLayer();
			}
		}

		private void importBlastlayerblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BlastLayer temp = RTC_BlastTools.LoadBlastLayerFromFile();
			ImportBlastLayer(temp);
		}

		public void ImportBlastLayer(BlastLayer bl)
		{
			if (bl != null)
			{
				foreach (BlastUnit bu in bl.Layer)
					sk.BlastLayer.Layer.Add(bu);
				RefreshBlastLayer();
			}
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;

			if (sk.BlastLayer.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty is empty");
				return;
			}

			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.DefaultExt = "csv";
			saveFileDialog1.Title = "Export to csv";
			saveFileDialog1.Filter = "csv files|*.csv";
			saveFileDialog1.RestoreDirectory = true;

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filename = saveFileDialog1.FileName;
			}
			else
				return;
			CSVGenerator csv = new CSVGenerator();
			File.WriteAllText(filename, csv.GenerateFromDGV(dgvBlastLayer), Encoding.UTF8);
		}

		private void runOriginalSavestateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			originalSk.RunOriginal();
		}

		private void rasterizeVMDsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				bu.Rasterize();
			RefreshBlastLayer();
		}

		private void replaceSavestateFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StashKey temp = RTC_StockpileManager.getCurrentSavestateStashkey();
			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the glitch harvester, or the current selected box is empty");
				return;
			}

			//If the core doesn't match, abort
			if (sk.SystemCore != temp.SystemCore)
			{
				MessageBox.Show("The core associated with the current ROM and the core associated with the selected savestate don't match. Aborting!");
				return;
			}

			//If the game name differs, make sure they know what they're doing
			//There are times it'd be fine with a differing name yet savestates would still work (romhacks)
			if (sk.GameName != temp.GameName)
			{
				DialogResult dialogResult = MessageBox.Show(
					"You're attempting to replace a savestate associated with " +
					sk.GameName +
					" with a savestate associated with " +
					temp.GameName + ".\n" +
					"This probably won't work unless you also update the ROM.\n" +
					"Updating the ROM will invalidate the savestate, so if you're changing both ROM and state, do that first.\n\n" +
					"Are you sure you want to continue?", "Game mismatch", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.No)
				{
					return;
				}
			}

			//We only need the ParentKey and the SyncSettings here as everything else will match
			sk.ParentKey = temp.ParentKey;
			sk.SyncSettings = temp.SyncSettings;
		}

		private void replaceSavestateFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;

			OpenFileDialog ofd = new OpenFileDialog
			{
				DefaultExt = "state",
				Title = "Open Savestate File",
				Filter = "state files|*.state",
				RestoreDirectory = true
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;

			string oldKey = sk.ParentKey;
			string oldSS = sk.SyncSettings;

			//Get a new key
			sk.ParentKey = RTC_Core.GetRandomKey();

			//Let's hope the game name is correct!
			File.Copy(filename, sk.GetSavestateFullPath(), true);

			//Null the syncsettings out
			sk.SyncSettings = null;


			//Attempt to load and if it fails, don't let them update it.
			if (!RTC_StockpileManager.LoadState(sk))
			{
				sk.ParentKey = oldKey;
				sk.SyncSettings = oldSS;
				return;
			}

			//Grab the syncsettings
			StashKey temp = new StashKey(RTC_Core.GetRandomKey(), sk.ParentKey, sk.BlastLayer);
			sk.SyncSettings = temp.SyncSettings;
		}

		private void saveSavestateToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string filename;
			SaveFileDialog ofd = new SaveFileDialog
			{
				DefaultExt = "state",
				Title = "Save Savestate File",
				Filter = "state files|*.state",
				RestoreDirectory = true
			};
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				filename = ofd.FileName.ToString();
			}
			else
				return;

			File.Copy(sk.GetSavestateFullPath(), filename, true);
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
			sk.ParentKey = null;
			sk.RomFilename = temp.RomFilename;
			sk.RomData = temp.RomData;
			sk.GameName = temp.GameName;
			sk.SystemName = temp.SystemName;
			sk.SystemDeepName = temp.SystemDeepName;
			sk.SystemCore = temp.SystemCore;
			sk.SyncSettings = temp.SyncSettings;
		}

		private void replaceRomFromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = MessageBox.Show("Loading this rom will invalidate the associated savestate. You'll need to set a new savestate for the Blastlayer. Continue?", "Invalidate State?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				string filename;
				OpenFileDialog ofd = new OpenFileDialog
				{
					Title = "Open ROM File",
					Filter = "any file|*.*",
					RestoreDirectory = true
				};
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					filename = ofd.FileName.ToString();
				}
				else
					return;
				RTC_Core.LoadRom(filename, true);

				StashKey temp = new StashKey(RTC_Core.GetRandomKey(), sk.ParentKey, sk.BlastLayer);

				// We have to null this as to properly create a stashkey, we need to use it in the constructor,
				// but then the user needs to provide a savestate
				sk.ParentKey = null;

				sk.RomFilename = temp.RomFilename;
				sk.GameName = temp.GameName;
				sk.SystemName = temp.SystemName;
				sk.SystemDeepName = temp.SystemDeepName;
				sk.SystemCore = temp.SystemCore;
				sk.SyncSettings = temp.SyncSettings;
			}
		}

		private void bakeROMBlastunitsToFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string[] originalFilename = sk.RomFilename.Split('\\');
			string filename;
			SaveFileDialog sfd = new SaveFileDialog();
			//sfd.DefaultExt = "rom";
			sfd.FileName = originalFilename[originalFilename.Length - 1];
			sfd.Title = "Save Rom File";
			sfd.Filter = "rom files|*.*";
			sfd.RestoreDirectory = true;
			if (sfd.ShowDialog() == DialogResult.OK)
				filename = sfd.FileName.ToString();
			else
				return;
			RomParts rp = RTC_MemoryDomains.GetRomParts(sk.SystemName, sk.RomFilename);

			File.Copy(sk.RomFilename, filename, true);
			using (FileStream output = new FileStream(filename, FileMode.Open))
			{
				foreach (BlastUnit bu in sk.BlastLayer.Layer)
				{
					if (bu is BlastByte bb)
					{
						if (bb.Domain == rp.PrimaryDomain)
						{
							output.Position = bb.Address + rp.SkipBytes;
							output.Write(bb.Value, 0, bb.Value.Length);
						}
						else if (bb.Domain == rp.SecondDomain)
						{
							output.Position = bb.Address + RTC_MemoryDomains.MemoryInterfaces[rp.SecondDomain].Size + rp.SkipBytes;
							output.Write(bb.Value, 0, bb.Value.Length);
						}
					}
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

		private void RTC_NewBlastEditor_Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.F)
			{
				btnSearchRow.PerformClick();
			}
			if (e.Control && e.KeyCode == Keys.G)
			{
				btnSearchAgain.PerformClick();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				saveToFileblToolStripMenuItem.PerformClick();
			}
			if (e.Control && e.KeyCode == Keys.C)
			{
				Clipboard.SetDataObject(dgvBlastLayer.CurrentCell.Value.ToString(), false);
			}
		}

		private void btnShiftBlastLayerDown_Click(object sender, EventArgs e)
		{
			ShiftBlastLayer(true);
		}

		private void btnShiftBlastLayerUp_Click(object sender, EventArgs e)
		{
			ShiftBlastLayer(false);
		}

		private void ShiftBlastLayer(bool shiftDown = false)
		{
			if (shiftDown)
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where((item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true)))
				{
					if (((decimal)selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value - updownShiftBlastLayerAmount.Value) >= 0)
						selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value = (decimal)selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value - updownShiftBlastLayerAmount.Value;
					else
						selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value = 0;
				}
			else
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where((item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true)))
				{
					decimal max = selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Maximum;

					if (((decimal)selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value + (decimal)updownShiftBlastLayerAmount.Value) <= max)
						selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value = (decimal)selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value + updownShiftBlastLayerAmount.Value;
					else
						selected.Cells[(cbShiftBlastlayer.SelectedItem as dynamic).Value].Value = max;
				}
			}
		}

		private void bakeBlastByteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var token = RTC_NetCore.HugeOperationStart("DISABLED");
			try
			{
				//Generate a blastlayer from the current selected rows
				BlastLayer bl = new BlastLayer();
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true)))
				{
					BlastUnit bu = (BlastUnit)selected.Cells["dgvBlastUnitReference"].Value;
					if (!bu.IsLocked)
					{
						//They have to be enabled to get a backup
						bu.IsEnabled = true;
						bl.Layer.Add(bu);
					}
				}

				//Bake them
				BlastLayer newBlastLayer = RTC_BlastTools.BakeBlastUnitsToSet(sk, bl);

				int i = 0;
				//Insert the new one where the old row was, then remove the old row.
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item =>
					((bool)item.Cells["dgvBlastUnitLocked"].Value != true)))
				{
					InsertBlastUnitToBlastLayerAndDgv(selected.Index, newBlastLayer.Layer[i]);
					i++;
					RemoveBlastUnitFromBlastLayerAndDgv((BlastUnit)selected.Cells["dgvBlastUnitReference"].Value);
				}
			}
			catch (Exception ex)
			{
				throw new System.Exception("Something went wrong in when baking to SET.\n" +
				                           "Your blast editor session may be broke depending on when it failed.\n" +
				                           "You should probably send a copy of this error and what you did to cause it to the RTC devs.\n\n" +
				                           ex.ToString());
			}
			finally
			{
				RTC_NetCore.HugeOperationEnd(token);
			}
		}

		private void sanitizeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<BlastUnit> bul = new List<BlastUnit>(sk.BlastLayer.Layer.ToArray().Reverse());
			List<long> usedAddresses = new List<long>();
			List<long> usedPipeAddresses = new List<long>();

			foreach (BlastUnit bu in bul)
			{
				//Optimize by doing everything besides blastpipe first
				if (!(bu is BlastPipe))
				{
					if (!usedAddresses.Contains(bu.Address) && !bu.IsLocked)
						usedAddresses.Add(bu.Address);
					else
					{
						RemoveBlastUnitFromBlastLayerAndDgv(bu);
						currentlyUpdating = false;
					}
				}
				else
				{
					BlastPipe bp = bu as BlastPipe;
					if (!usedPipeAddresses.Contains(bp.PipeAddress) && !bu.IsLocked)
						usedPipeAddresses.Add(bp.PipeAddress);
					else
					{
						RemoveBlastUnitFromBlastLayerAndDgv(bu);
						currentlyUpdating = false;
					}
				}
			}
		}

		private void openBlastLayerGeneratorToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (RTC_Core.bgForm != null)
				RTC_Core.bgForm.Close();
			RTC_Core.bgForm = new RTC_BlastGenerator_Form();
			RTC_Core.bgForm.LoadStashkey(sk);
		}
		private void dgvBlastLayer_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Note handling
			if (e != null)
			{
				DataGridView senderGrid = (DataGridView)sender;

				if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
				{
					DataGridViewRow row = dgvBlastLayer.Rows[e.RowIndex];
					BlastUnit bu = (BlastUnit)row.Cells["dgvBlastUnitReference"].Value;
					if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm == null)
					{
						RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(bu.Note, "BlastEditor", bu);
					}
					else
					{
						if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Visible)
							RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Close();

						RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(bu.Note, "BlastEditor", bu);
					}

					return;
				}
			}
		}

		private void SetNoteIcon(BlastUnit bu)
		{
			bu2RowDico[bu].Cells["dgvNoteButton"].Value = string.IsNullOrWhiteSpace(bu.Note) ? string.Empty : "📝";
		}

		public void RefreshNoteIcons()
		{
			if(sk != null)
			{
				foreach (BlastUnit bu in sk.BlastLayer.Layer)
					SetNoteIcon(bu);
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			System.Diagnostics.ProcessStartInfo sInfo = new System.Diagnostics.ProcessStartInfo("https://corrupt.wiki/corruptors/rtc-real-time-corruptor/blast-editor.html");
			System.Diagnostics.Process.Start(sInfo);
		}
	}
}

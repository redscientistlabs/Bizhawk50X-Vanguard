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
	public partial class RTC_NewBlastEditor_Form : Form
	{
		/*
		 * Column Indexes - Updated 4/8/2018
		 *
		 * 0 dgvBlastUnitReference
		 * 1 dgvBlastUnitLocked.
		 * 2 dgvBlastEnabled
		 * 3 dgvPrecision
		 * 4 dgvBlastUnitType
		 * 5 dgvBlastUnitMode
		 * 6 dgvSourceAddressDomain
		 * 7 dvgSourceAddress
		 * 8 dvgParamDomain
		 * 9 dvgParam
		 */

		private enum BlastEditorColumn
		{
			dgvBlastUnitReference,
			dgvBlastUnitLocked,
			dgvBlastEnabled,
			dgvPrecision,
			dgvBlastUnitType,
			dgvBlastUnitMode,
			dgvSourceAddressDomain,
			dgvSourceAddress,
			dgvParamDomain,
			dgvParam
		}

		private StashKey sk = null;
		private StashKey originalsk = null;
		private bool initialized = false;
		private bool CurrentlyUpdating = false;
		private ContextMenuStrip cmsBlastEditor = new ContextMenuStrip();
		private String[] domains;
		private int searchOffset = 0;
		private string searchValue = "";
		private string searchColumn = "";
		public string currentBlastLayerFile = "";

		private Dictionary<BlastUnit, DataGridViewRow> bu2RowDico = new Dictionary<BlastUnit, DataGridViewRow>();

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

			this.KeyDown += new KeyEventHandler(RTC_NewBlastEditor_Form_KeyDown);
			this.dgvBlastLayer.CellValidating += new DataGridViewCellValidatingEventHandler(dgvBlastLayer_CellValidating);
			this.dgvBlastLayer.MouseClick += new System.Windows.Forms.MouseEventHandler(dgvBlastLayer_MouseClick);
			this.dgvBlastLayer.RowsAdded += new DataGridViewRowsAddedEventHandler(dgvBlastLayer_RowsAdded);
			this.dgvBlastLayer.RowsRemoved += new DataGridViewRowsRemovedEventHandler(dgvBlastLayer_RowsRemoved);
			this.dgvBlastLayer.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgvBlastLayer_UserDeletingRow);
			this.dgvBlastLayer.Sorted += new EventHandler(dgvBlastLayer_Sorted);
			this.dgvBlastLayer.SelectionChanged += new EventHandler(dgvBlastLayer_SelectionChanged);
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null || _sk.BlastLayer == null || _sk.BlastLayer.Layer == null)
				return;

			originalsk = (StashKey)_sk.Clone();
			sk = originalsk;
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
			//Populate the different rows.
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				AddBlastUnitToDGV(bu);
			}
			//SourceAddress and Param only need to accept Decimal
			dgvBlastLayer.Columns["dgvSourceAddress"].ValueType = typeof(Decimal);
			dgvBlastLayer.Columns["dgvParam"].ValueType = typeof(Decimal);

			UpdateBlastLayerSize();
			UpdateSelectedBlastUnitInfo();

			initialized = true;
		}

		private void AddBlastUnitToDGV(BlastUnit bu)
		{
			InsertBlastUnitToDGV(dgvBlastLayer.Rows.Count, bu);
		}

		private void InsertBlastUnitToDGV(int index, BlastUnit bu)
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
				destAddress = (Decimal)RTC_Extensions.getDecimalValue(bb.Value, bb.BigEndian);
				blastMode = Convert.ToString(bb.Type);
			}
			else if (bu is BlastCheat)
			{
				BlastCheat bc = bu as BlastCheat;
				precision = bc.Value.Length;
				sourceAddress = bc.Address;
				sourceDomain = bc.Domain;
				destAddress = RTC_Extensions.getDecimalValue(bc.Value, bc.BigEndian);
				if (bc.IsFreeze)
					blastMode = "FREEZE";
				else
					blastMode = "HELLGENIE";
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
				destAddress = (Decimal)RTC_Extensions.getDecimalValue(bv.Values, bv.BigEndian);
				blastMode = Convert.ToString(bv.Type);
			}

			dgvBlastLayer.Rows.Insert(index, bu, locked, enabled, GetPrecisionNameFromSize(precision), blastType, blastMode, sourceDomain, sourceAddress, destDomain, destAddress);

			//update the BlastUnit to Cell dico.
			var row = dgvBlastLayer.Rows[index];
			bu2RowDico[bu] = row;

			//Update the precision
			UpdateRowPrecision(dgvBlastLayer.Rows[dgvBlastLayer.Rows.Count - 1]);
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = true;
				CurrentlyUpdating = false;
			}
			foreach (BlastUnit bu in sk.BlastLayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.BlastLayer.Layer.Count / 2))
			{
				if (!(bool)bu2RowDico[bu].Cells["dgvBlastUnitLocked"].Value)
					bu2RowDico[bu].Cells["dgvBlastEnabled"].Value = false;
				CurrentlyUpdating = false;
			}
		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < sk.BlastLayer.Layer.Count(); i++)
			{
				var bu = sk.BlastLayer.Layer[i];
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
				sk.BlastLayer.Layer.Remove(bu);
				dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
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
				sk.BlastLayer.Layer.Add(bu2);
				AddBlastUnitToDGV(bu2);
			}
		}

		//For BlastPipe, sanitize duplicare valyes instead of addresses
		private void btnSanitizeDuplicates_Click(object sender, EventArgs e)
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
						sk.BlastLayer.Layer.Remove(bu);
						dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
						CurrentlyUpdating = false;
					}
				}
				else
				{
					BlastPipe bp = bu as BlastPipe;
					if (!usedPipeAddresses.Contains(bp.PipeAddress) && !bu.IsLocked)
						usedPipeAddresses.Add(bp.PipeAddress);
					else
					{
						sk.BlastLayer.Layer.Remove(bu);
						dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
						CurrentlyUpdating = false;
					}
				}
			}
		}

		private void dgvBlastLayer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (!initialized || dgvBlastLayer == null)
				return;


			if (!CurrentlyUpdating)
			{
				CurrentlyUpdating = true;

				try
				{
					UpdateRowPrecision(dgvBlastLayer.Rows[e.RowIndex]);
					//Changing the blast unit type
					if (e.ColumnIndex == 4)
						ReplaceBlastUnitFromRow(dgvBlastLayer.Rows[e.RowIndex]);
					else
						UpdateBlastUnitFromRow(dgvBlastLayer.Rows[e.RowIndex]);
				}
				catch (Exception ex)
				{
					CurrentlyUpdating = false;

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
						BlastByte bb = (BlastByte)RTC_BlastTools.ConvertBlastUnit(bu, typeof(BlastByte));
						sk.BlastLayer.Layer.Insert(index, bb);
						InsertBlastUnitToDGV(index, bb);
						CurrentlyUpdating = false;
						break;

					case "RTC.BlastCheat":
						BlastCheat bc = (BlastCheat)RTC_BlastTools.ConvertBlastUnit(bu, typeof(BlastCheat));
						sk.BlastLayer.Layer.Insert(index, bc);
						InsertBlastUnitToDGV(index, bc);
						CurrentlyUpdating = false;
						break;

					case "RTC.BlastPipe":
						BlastPipe bp = (BlastPipe)RTC_BlastTools.ConvertBlastUnit(bu, typeof(BlastPipe));
						sk.BlastLayer.Layer.Insert(index, bp);
						InsertBlastUnitToDGV(index, bp);
						CurrentlyUpdating = false;
						break;

					case "RTC.BlastVector":
						BlastVector bv = (BlastVector)RTC_BlastTools.ConvertBlastUnit(bu, typeof(BlastVector));
						sk.BlastLayer.Layer.Insert(index, bv);
						InsertBlastUnitToDGV(index, bv);
						CurrentlyUpdating = false;
						break;

					default:
						CurrentlyUpdating = false;
						throw new Exception("Invalid BlastUnit Type");
				}
				//Remove the old one
				RemoveBlastUnitFromBlastLayerAndDGV(bu);
			}catch(Exception ex)
			{
				throw;
			}
		}

		private void RemoveBlastUnitFromBlastLayerAndDGV(BlastUnit bu)
		{
			//Remove it from the dgv
			dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
			//Remove it from the dictionary
			bu2RowDico.Remove(bu);
			//Remove it from the blastlayer
			sk.BlastLayer.Layer.Remove(bu);

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
					bb.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), bb.BigEndian);
					bb.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					Enum.TryParse(row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper(), out bb.Type);
					bb.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));

					row.Cells["dgvBlastUnitReference"].Value = bb;
					CurrentlyUpdating = false;
					break;

				case "RTC.BlastCheat":
					BlastCheat bc = (BlastCheat)row.Cells["dgvBlastUnitReference"].Value;
					bc.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bc.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bc.Value = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), bc.BigEndian);
					bc.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					if (row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper() == "FREEZE")
						bc.IsFreeze = true;
					else
						bc.IsFreeze = false;
					bc.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));

					row.Cells["dgvBlastUnitReference"].Value = bc;
					CurrentlyUpdating = false;
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
					CurrentlyUpdating = false;
					break;

				case "RTC.BlastVector":
					BlastVector bv = (BlastVector)row.Cells["dgvBlastUnitReference"].Value;
					bv.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bv.Address = Convert.ToInt64(row.Cells["dgvSourceAddress"].Value);
					bv.Values = RTC_Extensions.getByteArrayValue(GetPrecisionSizeFromName(row.Cells["dgvPrecision"].Value.ToString()), Convert.ToDecimal(row.Cells["dgvParam"].Value), bv.BigEndian);
					bv.Domain = Convert.ToString(row.Cells["dgvSourceAddressDomain"].Value);
					bv.IsLocked = Convert.ToBoolean((row.Cells["dgvBlastUnitLocked"].Value));
					Enum.TryParse(row.Cells["dgvBlastUnitMode"].Value.ToString().ToUpper(), out bv.Type);

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
		private void UpdateRowPrecision(DataGridViewRow row)
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

		private UInt32 GetPrecisionMaxValue(DataGridViewRow row)
		{
			switch (row.Cells["dgvBlastUnitType"].Value.ToString())
			{
				case "RTC.BlastByte":
				case "RTC.BlastCheat":
					if (row.Cells["dgvPrecision"].Value.ToString() == "8-bit")
					{
						return Byte.MaxValue;
					}
					else if (row.Cells["dgvPrecision"].Value.ToString() == "16-bit")
					{
						return UInt16.MaxValue;
					}
					else
					{
						return UInt32.MaxValue;
					}
				case "RTC.BlastPipe":
					//Todo set this to the valid maximum address
					return Int32.MaxValue;
			}
			//No precision set?
			return UInt32.MaxValue;
		}

		private void PopulateDomainContextMenu(int column)
		{
			cmsBlastEditor.Items.Clear();

			domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();

			foreach (string domain in domains)
			{
				(cmsBlastEditor.Items.Add(domain, null, new EventHandler((ob, ev) =>
				{
					foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
						selected.Cells[column].Value = domain;
				})) as ToolStripMenuItem).Enabled = true;
			}
		}

		private void PopulateBlastUnitModeContextMenu(int column, int row)
		{
			cmsBlastEditor.Items.Clear();

			if (dgvBlastLayer[4, row].Value.ToString() == "RTC.BlastByte")
			{
				foreach (BlastByteType type in Enum.GetValues(typeof(BlastByteType)))
				{
					//cmsDomain.Items.Add(domain, null, );
					(cmsBlastEditor.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
					{
						foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
							selected.Cells[column].Value = type.ToString();
					})) as ToolStripMenuItem).Enabled = true;
				}
			}
			else if (dgvBlastLayer[4, row].Value.ToString() == "RTC.BlastCheat")
			{
				foreach (BlastCheatType type in Enum.GetValues(typeof(BlastCheatType)))
				{
					//cmsDomain.Items.Add(domain, null, );
					(cmsBlastEditor.Items.Add(type.ToString(), null, new EventHandler((ob, ev) =>
					{
						foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
							selected.Cells[column].Value = type.ToString();
					})) as ToolStripMenuItem).Enabled = true;
				}
			}
		}

		private void PopulateBlastUnitTypeContextMenu(int column, int row)
		{
			cmsBlastEditor.Items.Clear();

			//Adding these by hand

			(cmsBlastEditor.Items.Add("RTC.BlastByte", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastByte";
			})) as ToolStripMenuItem).Enabled = true;
			(cmsBlastEditor.Items.Add("RTC.BlastCheat", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastCheat";
			})) as ToolStripMenuItem).Enabled = true;
			(cmsBlastEditor.Items.Add("RTC.BlastPipe", null, new EventHandler((ob, ev) =>
			{
				foreach (DataGridViewRow selected in dgvBlastLayer.SelectedRows.Cast<DataGridViewRow>().Where(item => (bool)item.Cells["dgvBlastUnitLocked"].Value != true))
					selected.Cells[column].Value = "RTC.BlastPipe";
			})) as ToolStripMenuItem).Enabled = true;
		}

		private void PopulateColumnHeaderContextMenu(int column)
		{
			cmsBlastEditor.Items.Clear();

			//Adding these by hand

			/*
			*1 dgvBlastUnitLocked.

		   * 0 dgvBlastUnitReference
		   * 2 dgvBlastEnabled
		   * 3 dgvPrecision
		   * 4 dgvBlastUnitType
		   * 5 dgvBlastUnitMode
		   * 6 dgvSourceAddressDomain
		   * 7 dvgSourceAddress
		   * 8 dvgParamDomain
		   * 9 dvgParam*/

			(cmsBlastEditor.Items.Add("Change Selected Rows", null, new EventHandler((ob, ev) =>
			{
				string newvalue = "";
				decimal decimalvalue = 0;

				switch (column)
				{
					//Blast unit locked
					//Blast unit enabled
					case 1:
					case 2:
						foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
						{
							row.Cells[column].Value = !(bool)(row.Cells[column].Value);
						}
						break;

					case 3:
						string[] options = { "8-bit", "16-bit", "32-bit" };
						if (RTC_Extensions.getComboInputBox("Replace Selected Rows", "Replacement input: ", options, ref newvalue) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = newvalue;
							}
						break;

					/* 4 dgvBlastUnitType
					 * 5 dgvBlastUnitMode
					 * 6 dgvSourceAddressDomain
					 * 8 dvgParamDomain
					 */
					case 4:
					case 5:
					case 6:
					case 8:
						if (RTC_Extensions.getInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref newvalue) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = newvalue;
							}
						break;
					//7 dvgSourceAddress
					case 7:
						if (RTC_Extensions.getInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref decimalvalue, cbUseHex.Checked) == DialogResult.OK)
							foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
							{
								row.Cells[column].Value = decimalvalue;
							}
						break;
					//9 dvgParam
					//Needs to be capped at the precision so it's separate from case 7 (the address)
					case 9:
						if (RTC_Extensions.getInputBox("Replace Selected Rows", "Replacement input (make sure it's valid): ", ref decimalvalue, cbUseHex.Checked) == DialogResult.OK)
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
			})) as ToolStripMenuItem).Enabled = true;
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
				//Column header
				if (currentMouseOverRow == -1)
				{
					PopulateColumnHeaderContextMenu(currentMouseOverColumn);
					cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}

				//BlastUnitType
				else if (currentMouseOverColumn == (int)BlastEditorColumn.dgvBlastUnitType)
				{
					PopulateBlastUnitTypeContextMenu(currentMouseOverColumn, currentMouseOverRow);
					cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}

				//BlastUnitMode
				else if (currentMouseOverColumn == (int)BlastEditorColumn.dgvBlastUnitMode)
				{
					PopulateBlastUnitModeContextMenu(currentMouseOverColumn, currentMouseOverRow);
					cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}

				//Domain1 Domain2
				else if (currentMouseOverColumn == (int)BlastEditorColumn.dgvSourceAddressDomain || currentMouseOverColumn == (int)BlastEditorColumn.dgvParamDomain)
				{
					PopulateDomainContextMenu(currentMouseOverColumn);
					cmsBlastEditor.Show(dgvBlastLayer, new Point(e.X, e.Y));
				}
			}
		}

		private void dgvBlastLayer_Sorted(object sender, EventArgs e)
		{

			if(dgvBlastLayer.SortOrder == SortOrder.Ascending)
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
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParamDomain(it)).Reverse().ToList();
						break;

					case "dgvParam":
						sk.BlastLayer.Layer = sk.BlastLayer.Layer.OrderBy(it => GetParamValue(it)).Reverse().ToList();
						break;

					default:
						break;
				}
			
			}
		}

		private long GetParamValue(BlastUnit bu)
		{
			if (bu is BlastByte)
			{
				BlastByte bb = bu as BlastByte;
				decimal value = RTC_Extensions.getDecimalValue(bb.Value, bb.BigEndian);
				return (long)value;
			}
			else if (bu is BlastCheat)
			{
				BlastCheat bc = bu as BlastCheat;
				decimal value = RTC_Extensions.getDecimalValue(bc.Value, bc.BigEndian);
				return (long)value;
			}
			else if (bu is BlastPipe)
			{
				BlastPipe bp = bu as BlastPipe;
				return bp.PipeAddress;
			}
			else if (bu is BlastVector)
			{
				BlastVector bv = bu as BlastVector;
				decimal value = RTC_Extensions.getDecimalValue(bv.Values, bv.BigEndian);
				return (long)value;
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
			UpdateSelectedBlastUnitInfo();
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
			if (MessageBox.Show("Are you sure you want to remove the selected rows?", "Remove Rows", MessageBoxButtons.YesNo) == DialogResult.Yes)
				foreach (DataGridViewRow row in dgvBlastLayer.SelectedRows)
				{
					if ((bool)row.Cells["dgvBlastUnitLocked"].Value)
						return;

					int pos = row.Index;
					BlastUnit bu = sk.BlastLayer.Layer[pos];

					sk.BlastLayer.Layer.Remove(bu);
					dgvBlastLayer.Rows.Remove(bu2RowDico[bu]);
					CurrentlyUpdating = false;
				}
		}

		public DialogResult getSearchBox(string title, string promptText, bool filterColumn = false)
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

			searchValue = input.Text;
			if (filterColumn)
				searchColumn = (column.SelectedItem as dynamic).Value;
			return dialogResult;
		}

		//Provides a dialog box that searches for a row in the DGV
		private void btnSearchRow_Click(object sender, EventArgs e)
		{
			if (this.getSearchBox("Search for a value", "Choose a column and enter a value", true) == DialogResult.OK)
			{
				dgvBlastLayer.ClearSelection();
				DataGridViewRow row = SearchDataGridView(dgvBlastLayer);
				if (row != null)
					row.Selected = true;
			}
		}

		private void btnSearchAgain_Click(object sender, EventArgs e)
		{
			dgvBlastLayer.ClearSelection();
			DataGridViewRow row = SearchDataGridView(dgvBlastLayer, true);
			if (row != null)
				row.Selected = true;
		}

		private DataGridViewRow SearchDataGridView(DataGridView dgv, bool findAgain = false)
		{
			if (searchColumn == "")
				return null;

			if (!findAgain)
				searchOffset = 0;

			//Search for the formatted value and return the row
			//We use the formatted value and expect the user to input the correct type.
			while (searchOffset < dgv.RowCount)
			{
				if (dgv[searchColumn, searchOffset].FormattedValue.ToString() == searchValue)
					return dgv.Rows[searchOffset++];
				searchOffset++;
			}

			MessageBox.Show("Reached the end of the Blastlayer and didn't find anything");
			searchOffset = 0;
			return null;
		}

		private void saveToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//If there's no blastlayer file already set, don't quicksave
			if (currentBlastLayerFile == "")
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
			if (temp != null)
			{
				foreach (BlastUnit bu in temp.Layer)
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
			originalsk.RunOriginal();
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
			if (sk.GameName != temp.GameName)
			{
				DialogResult dialogResult = MessageBox.Show("You're attempting to replace a savestate associated with " + sk.GameName + " with a savestate associated with " + temp.GameName + ".\n This probably won't work unless you also update the rom.\n Are you sure you want to continue?", "Game mismatch", MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.Yes)
				{
					sk.ParentKey = temp.ParentKey;
				}
				else if (dialogResult == DialogResult.No)
				{
					return;
				}
			}
			else
				sk.ParentKey = temp.ParentKey;
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
			//Get a new key
			sk.ParentKey = RTC_Core.GetRandomKey();

			//Let's hope the game name is correct!
			File.Copy(filename, sk.getSavestateFullPath());
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

			File.Copy(sk.getSavestateFullPath(), filename);
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
			DialogResult dialogResult = MessageBox.Show("Loading this rom will invalidate the associated savestate. You'll need to set a new savestate for the Blastlayer. Continue?", "Invalidate State?", MessageBoxButtons.YesNo);
			if (dialogResult == DialogResult.Yes)
			{
				string filename;
				OpenFileDialog ofd = new OpenFileDialog();
				ofd.Title = "Open ROM File";
				ofd.Filter = "any file|*.*";
				ofd.RestoreDirectory = true;
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					filename = ofd.FileName.ToString();
				}
				else
					return;
				RTC_Core.LoadRom(filename, true);
				StashKey temp = new StashKey();

				sk.RomFilename = temp.RomFilename;
				sk.GameName = temp.GameName;
				sk.SystemName = temp.SystemName;
				sk.SystemDeepName = temp.SystemDeepName;
				sk.SystemCore = temp.SystemCore;
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
			{
				filename = sfd.FileName.ToString();
			}
			else
				return;
			RomParts rp = RTC_MemoryDomains.GetRomParts(sk.SystemName, sk.RomFilename);

			File.Copy(sk.RomFilename, filename);
			using (FileStream output = new FileStream(filename, FileMode.Open))
			{
				foreach (BlastByte bu in sk.BlastLayer.Layer)
				{
					if (bu.Domain == rp.primarydomain)
					{
						output.Position = bu.Address + rp.skipbytes;
						output.Write(bu.Value, 0, bu.Value.Length);
					}
					else if (bu.Domain == rp.seconddomain)
					{
						output.Position = bu.Address + RTC_MemoryDomains.MemoryInterfaces["CHR VROM"].Size + rp.skipbytes;
						output.Write(bu.Value, 0, bu.Value.Length);
					}
				}
			}
		}

		private void btnHideSidebar_Click(object sender, EventArgs e)
		{
			if (btnHideSidebar.Text == "▶")
			{
				panelSidebar.Visible = false;
				//this.Width = this.Width - 176;
				//dgvBlastLayer.Width = dgvBlastLayer.Width + 176;
				btnHideSidebar.Text = "◀";
			}
			else
			{
				panelSidebar.Visible = true;
				//this.Width = this.Width + 176;
				//dgvBlastLayer.Width = dgvBlastLayer.Width - 176;
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

		private void UpdateSelectedBlastUnitInfo()
		{
			if (dgvBlastLayer.SelectedRows.Count >= 1)
			{
				lbSelectedParam1Info.Text = dgvBlastLayer[7, dgvBlastLayer.CurrentRow.Index].FormattedValue.ToString();
				lbSelectedParam2Info.Text = dgvBlastLayer[9, dgvBlastLayer.CurrentRow.Index].FormattedValue.ToString();
			}
		}

		private void dgvBlastLayer_SelectionChanged(object sender, EventArgs e)
		{
			UpdateSelectedBlastUnitInfo();
		}
	}
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using BizHawk.Common.NumberExtensions;
using Newtonsoft.Json.Serialization;

/**
 * The DataGridView is bound to the blastlayer
 * All validation is done within the dgv
 * The boxes at the bottom are unbound and manipulate the selected rows in the dgv, and thus, the validation is handled by the dgv
 * No maxmimum is set in the numericupdowns at the bottom as the dgv validates
 **/

/*
Applies in all cases & should be editable
 * bool IsEnabled 
 * bool IsLocked
 * 
 * string Domain 
 * long Address 
 * int Precision 
 * BlastUnitSource Source 

 * BigInteger TiltValue 
 * 
 * int ExecuteFrame 
 * int Lifetime 
 * bool Loop 
 * 
 * ActionTime LimiterTime 
 * string LimiterListHash 
 * bool InvertLimiter 
 *
 * string Note 


Applies for Store & should be editable
 * ActionTime StoreTime 
 * StoreType StoreType 
 * string SourceDomain 
 * long SourceAddress 


Applies for Value & should be editable
 * byte[] Value */

namespace RTC
{
	public partial class RTC_NewBlastEditor_Form : Form, IAutoColorize
	{

		private static Dictionary<string, MemoryInterface> domainToMiDico = new Dictionary<string, MemoryInterface>();
		private string[] domains = RTC_MemoryDomains.MemoryInterfaces?.Keys?.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
		private string mainDomain = RTC_MemoryDomains.MDRI?.MemoryDomains?.MainMemory?.ToString();
		private string searchValue, searchColumn;
		public List<String> VisibleColumns;
		private string CurrentBlastLayerFile = "";

		private int searchOffset = 0;
		private IEnumerable<BlastUnit> searchEnumerable;
		BindingList<BlastUnit> selectedBUs = new BindingList<BlastUnit>();
		ContextMenuStrip headerStrip;
		ContextMenuStrip cms;

		Dictionary<String, Control> property2ControlDico;


		int buttonFillWeight = 20;
		int checkBoxFillWeight = 25;
		int comboBoxFillWeight = 40;
		int textBoxFillWeight = 30;
		int numericUpDownFillWeight = 35;

		private enum buProperty
		{
			isEnabled,
			isLocked,
			Domain,
			Address,
			Precision,
			ValueString,
			Source,
			ExecuteFrame,
			Lifetime,
			Loop,
			LimiterTime,
			LimiterListHash,
			InvertLimiter,
			StoreTime,
			StoreType,
			SourceDomain,
			SourceAddress,
			Note
		}
		//We gotta cache this stuff outside of the scope of InitializeDGV
		//	private object actionTimeValues = 


		public RTC_NewBlastEditor_Form()
		{
			try
			{
				InitializeComponent();

				dgvBlastEditor.DataError += dgvBlastLayer_DataError;
				dgvBlastEditor.AutoGenerateColumns = false;
				dgvBlastEditor.SelectionChanged += dgvBlastEditor_SelectionChanged;
				dgvBlastEditor.ColumnHeaderMouseClick += dgvBlastEditor_ColumnHeaderMouseClick;
				dgvBlastEditor.CellValueChanged += dgvBlastEditor_CellValueChanged;
				dgvBlastEditor.CellMouseClick += dgvBlastEditor_CellMouseClick;
				dgvBlastEditor.RowsAdded += DgvBlastEditor_RowsAdded;
				dgvBlastEditor.RowsRemoved += DgvBlastEditor_RowsRemoved;

				tbFilter.TextChanged += tbFilter_TextChanged;

				cbEnabled.Validated += cbEnabled_Validated;
				cbLocked.Validated += CbLocked_Validated;
				cbBigEndian.Validated += CbBigEndian_Validated;
				cbLoop.Validated += CbLoop_Validated;

				cbDomain.Validated += cbDomain_Validated;
				upDownAddress.Validated += UpDownAddress_Validated;
				upDownPrecision.Validated += UpDownPrecision_Validated;
				tbTiltValue.Validated += TbTiltValue_Validated;


				upDownExecuteFrame.Validated += UpDownExecuteFrame_Validated;
				upDownLifetime.Validated += UpDownLifetime_Validated;

				cbSource.Validated += CbSource_Validated;
				tbValue.Validated += TbValue_Validated;

				cbInvertLimiter.Validated += CbInvertLimiter_Validated;
				cbLimiterTime.Validated += CbLimiterTime_Validated;
				cbLimiterList.Validated += CbLimiterList_Validated;

				upDownSourceAddress.Validated += UpDownSourceAddress_Validated;
				cbStoreTime.Validated += CbStoreTime_Validated;
				cbStoreType.Validated += CbStoreType_Validated;
				cbSourceDomain.Validated += CbSourceDomain_Validated;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
		private void RTC_NewBlastEditorForm_Load(object sender, EventArgs e)
		{
			RTC_Core.SetRTCColor(RTC_Core.GeneralColor, this);
		}

		private void dgvBlastEditor_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			// Note handling
			if (e != null && e.RowIndex != -1)
			{
				if (e.ColumnIndex == dgvBlastEditor.Columns[buProperty.Note.ToString()]?.Index)
				{
					BlastUnit bu = dgvBlastEditor.Rows[e.RowIndex].DataBoundItem as BlastUnit;
					if (bu != null)
						new RTC_NoteEditor_Form(bu, dgvBlastEditor[e.ColumnIndex, e.RowIndex]);
				}
			}

			if (e.Button == MouseButtons.Left)
			{
				if (e.RowIndex == -1)
				{
					dgvBlastEditor.EndEdit();
					dgvBlastEditor.ClearSelection();
				}
			}
			else if (e.Button == MouseButtons.Right)
			{
				//End the edit if they're right clicking somewhere else
				if (dgvBlastEditor.CurrentCell.ColumnIndex != e.ColumnIndex)
				{
					dgvBlastEditor.EndEdit();
				}

				cms = new ContextMenuStrip();

				if (e.RowIndex != -1 && e.ColumnIndex != -1)
				{
					//Can't use a switch statement because dynamic
					if (dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[buProperty.Address.ToString()] ||
						dgvBlastEditor.Columns[e.ColumnIndex] == dgvBlastEditor.Columns[buProperty.SourceAddress.ToString()])
					{
						cms.Items.Add(new ToolStripSeparator());
						PopulateAddressContextMenu(dgvBlastEditor[e.ColumnIndex, e.RowIndex]);
					}
					cms.Show(dgvBlastEditor, dgvBlastEditor.PointToClient(Cursor.Position));
				}
			}
		}
		
		private void PopulateAddressContextMenu(DataGridViewCell cell)
		{
			BlastUnit bu = (BlastUnit)dgvBlastEditor.Rows[cell.RowIndex].DataBoundItem;

			((ToolStripMenuItem)cms.Items.Add("Open Selected Address in Hex Editor", null, new EventHandler((ob, ev) =>
			{
				if (cell.OwningColumn == dgvBlastEditor.Columns[buProperty.Address.ToString()])
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BIZHAWK_OPEN_HEXEDITOR_ADDRESS) { objectValue = new object[] { bu.Domain, bu.Address } });

				if (cell.OwningColumn == dgvBlastEditor.Columns[buProperty.SourceAddress.ToString()])
					RTC_Core.SendCommandToRTC(new RTC_Command(CommandType.BIZHAWK_OPEN_HEXEDITOR_ADDRESS) { objectValue = new object[] { bu.SourceDomain, bu.SourceAddress } });
			}))).Enabled = true;
		}

		private void dgvBlastEditor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewColumn changedColumn = dgvBlastEditor.Columns[e.ColumnIndex];

			//If the Domain or SourceDomain changed update the Maximum Value
			if (changedColumn.Name == buProperty.Domain.ToString())
			{
				updateMaximum(dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.Address.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.Domain.ToString()].Value.ToString());
			}
			else if (changedColumn.Name == buProperty.SourceDomain.ToString())
			{
				updateMaximum(dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell, dgvBlastEditor.Rows[e.RowIndex].Cells[buProperty.SourceDomain.ToString()].Value.ToString());
			}
			UpdateBottom();
		}

		private void CbSourceDomain_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.SourceDomain.ToString()].Value = cbSourceDomain.SelectedItem;
			UpdateBottom();
		}

		private void CbStoreType_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.StoreType.ToString()].Value = cbStoreType.SelectedItem;UpdateBottom();
			UpdateBottom();
		}

		private void CbStoreTime_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.StoreTime.ToString()].Value = cbStoreTime.SelectedItem;
			UpdateBottom();
		}

		private void CbLimiterList_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.LimiterListHash.ToString()].Value = ((ComboBoxItem<String>)(cbLimiterList?.SelectedItem))?.Value ?? null; // We gotta use the value
			UpdateBottom();
		}

		private void CbBigEndian_Validated(object sender, EventArgs e)
		{
			//Big Endian isn't available in the DGV so we operate on the actual BU then refresh
			//Todo - change this?
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				(row.DataBoundItem as BlastUnit).BigEndian = cbBigEndian.Checked;
			}
			dgvBlastEditor.Refresh();
			UpdateBottom();
		}

		private void TbValue_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.ValueString.ToString()].Value = tbValue.Text;
			UpdateBottom();
		}

		private void CbSource_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Source.ToString()].Value = cbSource.SelectedItem;
			UpdateBottom();
		}

		private void TbTiltValue_Validated(object sender, EventArgs e)
		{
			//Tilt isn't stored within the DGV so operate on the BUs. No validation neccesary as it's a bigint
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				(row.DataBoundItem as BlastUnit).TiltValue = System.Numerics.BigInteger.Parse(tbTiltValue.Text);
			}
			UpdateBottom();
		}
		private void UpDownLifetime_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Lifetime.ToString()].Value = upDownLifetime.Value;

			UpdateBottom();
			dgvBlastEditor.Refresh();
		}
		private void UpDownExecuteFrame_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.ExecuteFrame.ToString()].Value = upDownExecuteFrame.Value;

			UpdateBottom();
			dgvBlastEditor.Refresh();
		}

		private void UpDownPrecision_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Precision.ToString()].Value = upDownPrecision.Value;

			UpdateBottom();
			dgvBlastEditor.Refresh();
		}

		private void UpDownAddress_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Address.ToString()].Value = upDownAddress.Value;
			UpdateBottom();
		}

		private void UpDownSourceAddress_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.SourceAddress.ToString()].Value = upDownSourceAddress.Value;
			UpdateBottom();
		}

		private void CbLocked_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.isLocked.ToString()].Value = cbLocked.Checked;
			UpdateBottom();
		}

		private void CbLimiterTime_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.LimiterTime.ToString()].Value = cbLimiterTime.SelectedItem;
			UpdateBottom();
		}

		private void CbInvertLimiter_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.InvertLimiter.ToString()].Value = cbInvertLimiter.Checked;
			UpdateBottom();
		}
		private void cbEnabled_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.isEnabled.ToString()].Value = cbEnabled.Checked;
			UpdateBottom();
		}

		private void cbDomain_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Domain.ToString()].Value = cbDomain.SelectedItem;
			UpdateBottom();
		}


		private void CbLoop_Validated(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				row.Cells[buProperty.Loop.ToString()].Value = cbLoop.Checked;
			UpdateBottom();
		}


		private void dgvBlastEditor_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				headerStrip = new ContextMenuStrip();
				headerStrip.Items.Add("Select columns to show", null, new EventHandler((ob, ev) =>
				{
					ColumnSelector cs = new ColumnSelector();
					cs.LoadColumnSelector(dgvBlastEditor.Columns);
				}));

				headerStrip.Show(MousePosition);
			}
		}


		private void updateMaximum(DataGridViewSelectedRowCollection rows)
		{
			foreach (DataGridViewRow row in rows)
			{
				BlastUnit bu = row.DataBoundItem as BlastUnit;
				string domain = bu.Domain;
				string sourceDomain = bu.SourceDomain;

				if(domain != null)
					(row.Cells[buProperty.Address.ToString()] as DataGridViewNumericUpDownCell).Maximum = domainToMiDico[domain].Size;
				if(sourceDomain != null)
					(row.Cells[buProperty.SourceAddress.ToString()] as DataGridViewNumericUpDownCell).Maximum = domainToMiDico[sourceDomain].Size;
			}
		}

		private void updateMaximum(DataGridViewNumericUpDownCell cell, String domain)
		{
			if (domainToMiDico.ContainsKey(domain))
				cell.Maximum = domainToMiDico[domain]
					.Size;
			else
				cell.Maximum = Int32.MaxValue;

		}
		
		private void UpdateBottom()
		{
			if (dgvBlastEditor.SelectedRows.Count > 0)
			{
				var lastRow = dgvBlastEditor.SelectedRows[dgvBlastEditor.SelectedRows.Count - 1];

				/*
				cbDomain.SelectedItem = (String)(lastRow.Cells[buProperty.Domain.ToString()].Value);
				cbEnabled.Checked = (bool)(lastRow.Cells[buProperty.isEnabled.ToString()].Value);
				cbLocked.Checked = (bool)(lastRow.Cells[buProperty.isLocked.ToString()].Value);
				upDownAddress.Value = (long)(lastRow.Cells[buProperty.Address.ToString()].Value);
				upDownPrecision.Value = (int)(lastRow.Cells[buProperty.Precision.ToString()].Value);
				tbValue.Text = (String)(lastRow.Cells[buProperty.ValueString.ToString()].Value);
				upDownExecuteFrame.Value = (int)(lastRow.Cells[buProperty.ExecuteFrame.ToString()].Value);
				upDownLifetime.Value = (int)(lastRow.Cells[buProperty.Lifetime.ToString()].Value);
				cbLoop.Checked = (bool)(lastRow.Cells[buProperty.Loop.ToString()].Value);
				cbLimiterTime.SelectedItem = (ActionTime)(lastRow.Cells[buProperty.LimiterTime.ToString()].Value);
				cbLimiterList.SelectedItem = (String)(lastRow.Cells[buProperty.LimiterHash.ToString()].Value);
				cbInvertLimiter.Checked = (bool)(lastRow.Cells[buProperty.InvertLimiter.ToString()].Value);
				cbStoreTime.SelectedItem = (ActionTime)(lastRow.Cells[buProperty.StoreTime.ToString()].Value);
				cbStoreType.SelectedItem = (StoreType)(lastRow.Cells[buProperty.StoreType.ToString()].Value);
				cbSourceDomain.SelectedItem = (String)(lastRow.Cells[buProperty.SourceDomain.ToString()].Value);
				cbSource.SelectedItem = (BlastUnitSource)(lastRow.Cells[buProperty.Source.ToString()].Value);
				upDownSourceAddress.Value = (long)(lastRow.Cells[buProperty.SourceAddress.ToString()].Value);

				tbTiltValue.Text = (lastRow.DataBoundItem as BlastUnit).TiltValue.ToString();*/
				BlastUnit bu = (BlastUnit)lastRow.DataBoundItem;



				if (domainToMiDico.ContainsKey(bu.Domain ?? String.Empty))
					upDownAddress.Maximum = domainToMiDico[bu.Domain].Size;
				else
					upDownAddress.Maximum = Int32.MaxValue;

				if (domainToMiDico.ContainsKey(bu.SourceDomain ?? String.Empty))
					upDownSourceAddress.Maximum = domainToMiDico[bu.SourceDomain].Size;
				else
					upDownSourceAddress.Maximum = Int32.MaxValue;


				cbDomain.SelectedItem = bu.Domain;
				cbEnabled.Checked = bu.IsEnabled;
				cbLocked.Checked = bu.IsLocked;

				upDownAddress.Value = bu.Address;
				upDownPrecision.Value = bu.Precision;
				tbValue.Text = bu.ValueString;
				upDownExecuteFrame.Value = bu.ExecuteFrame;
				upDownLifetime.Value = bu.Lifetime;
				cbLoop.Checked = bu.Loop;
				cbLimiterTime.SelectedItem = bu.LimiterTime;

				cbLimiterList.SelectedItem = RTC_Core.LimiterListBindingSource.FirstOrDefault(x => x.Value == bu.LimiterListHash);

				cbInvertLimiter.Checked = bu.InvertLimiter;
				cbStoreTime.SelectedItem = bu.StoreTime;
				cbStoreType.SelectedItem = bu.StoreType;
				cbSourceDomain.SelectedItem = bu.SourceDomain;
				cbSource.SelectedItem = bu.Source;
				upDownSourceAddress.Value = bu.SourceAddress;


				tbTiltValue.Text = bu.TiltValue.ToString();

			}
		}

		private void dgvBlastEditor_SelectionChanged(object sender, EventArgs e)
		{
			UpdateBottom();
			//Rather than setting all these values at load, we set it on the fly
			updateMaximum(dgvBlastEditor.SelectedRows);
		}

		private void tbFilter_TextChanged(object sender, EventArgs e)
		{

			if (tbFilter.Text.Length == 0)
			{
				dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer;
				return;;
			}
				

			string value = ((ComboBoxItem<String>)cbFilterColumn?.SelectedItem)?.Value;
			if (value == null)
				return;

			switch (((ComboBoxItem<String>)cbFilterColumn.SelectedItem).Name)
			{
				//If it's an address or a source address we want decimal
				case "Address":
					dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer.Where(x => x.Address.ToString("X").ToUpper().Substring(0, tbFilter.Text.Length.Clamp(0, x.Address.ToString("X").Length)) == tbFilter.Text.ToUpper()).ToList();
					break;
				case "SourceAddress":
					dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer.Where(x => x.SourceAddress.ToString("X").ToUpper().Substring(0, tbFilter.Text.Length.Clamp(0, x.SourceAddress.ToString("X").Length)) == tbFilter.Text.ToUpper()).ToList();
					break;
				default: //Otherwise just use reflection and dig it out
					dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer.Where(x => x?.GetType()?.GetProperty(value)?.GetValue(x) != null && (x.GetType()?.GetProperty(value)?.GetValue(x).ToString().ToUpper().Substring(0, tbFilter.Text.Length) == tbFilter.Text.ToUpper())).ToList();
					break;
			}
		}
	
		private void InitializeBottom()
		{
			property2ControlDico = new Dictionary<string, Control>();

			var storeType = Enum.GetValues(typeof(StoreType));
			var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));

			cbDomain.BindingContext = new BindingContext();
			cbDomain.DataSource = domains;

			cbSourceDomain.BindingContext = new BindingContext();
			cbSourceDomain.DataSource = domains;

			foreach (var item in Enum.GetValues(typeof(LimiterTime)))
			{
				cbLimiterTime.Items.Add(item);
			}
			foreach (var item in Enum.GetValues(typeof(StoreTime)))
			{
				cbStoreTime.Items.Add(item);
			}
			foreach (var item in blastUnitSource)
			{
				cbSource.Items.Add(item);
			}

			cbLimiterList.DataSource = RTC_Core.LimiterListBindingSource;
			cbLimiterList.DisplayMember = "Name";
			cbLimiterList.ValueMember = "Value";

			cbStoreType.DataSource = storeType;

			property2ControlDico.Add(buProperty.Address.ToString(), upDownAddress);
			property2ControlDico.Add(buProperty.Domain.ToString(), cbDomain);
			property2ControlDico.Add(buProperty.Source.ToString(), cbSource);
			property2ControlDico.Add(buProperty.ExecuteFrame.ToString(), upDownExecuteFrame);
			property2ControlDico.Add(buProperty.InvertLimiter.ToString(), cbInvertLimiter);
			property2ControlDico.Add(buProperty.isEnabled.ToString(), cbEnabled);
			property2ControlDico.Add(buProperty.isLocked.ToString(), cbLocked);
			property2ControlDico.Add(buProperty.Lifetime.ToString(), upDownLifetime);
			property2ControlDico.Add(buProperty.LimiterListHash.ToString(), cbLimiterList);
			property2ControlDico.Add(buProperty.LimiterTime.ToString(), cbLimiterTime);
			property2ControlDico.Add(buProperty.Loop.ToString(), cbLoop);
			property2ControlDico.Add(buProperty.Note.ToString(), btnNote);
			property2ControlDico.Add(buProperty.Precision.ToString(), upDownPrecision);
			property2ControlDico.Add(buProperty.SourceAddress.ToString(), upDownSourceAddress);
			property2ControlDico.Add(buProperty.SourceDomain.ToString(), cbSourceDomain);
			property2ControlDico.Add(buProperty.StoreTime.ToString(), cbStoreTime);
			property2ControlDico.Add(buProperty.StoreType.ToString(), cbStoreType);
			property2ControlDico.Add(buProperty.ValueString.ToString(), tbValue);
		}



		private void InitializeDGV()
		{

			VisibleColumns = new List<string>();
			var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));


			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.isEnabled.ToString(), buProperty.isEnabled.ToString(), "Enabled", new DataGridViewCheckBoxColumn()));
			

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.isLocked.ToString(), buProperty.isLocked.ToString(), "Locked", new DataGridViewCheckBoxColumn()));

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn source = CreateColumn(buProperty.Source.ToString(), buProperty.Source.ToString(), "Source", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in blastUnitSource)
				source.Items.Add(item);
			dgvBlastEditor.Columns.Add(source);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn domain = CreateColumn(buProperty.Domain.ToString(), buProperty.Domain.ToString(), "Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			domain.DataSource = domains;
			dgvBlastEditor.Columns.Add(domain);

			DataGridViewNumericUpDownColumn address = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.Address.ToString(), buProperty.Address.ToString(), "Address", new DataGridViewNumericUpDownColumn());
			address.Hexadecimal = true;
			dgvBlastEditor.Columns.Add(address);



			
			
			DataGridViewNumericUpDownColumn precision = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.Precision.ToString(), buProperty.Precision.ToString(), "Precision", new DataGridViewNumericUpDownColumn());
			precision.Minimum = 1;
			precision.Maximum = Int32.MaxValue;
			dgvBlastEditor.Columns.Add(precision);

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.ValueString.ToString(), buProperty.ValueString.ToString(), "Value", new DataGridViewTextBoxColumn()));


			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.ExecuteFrame.ToString(), buProperty.ExecuteFrame.ToString(), "Execute Frame", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.Lifetime.ToString(), buProperty.Lifetime.ToString(), "Lifetime", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.Loop.ToString(), buProperty.Loop.ToString(), "Loop", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn limiterTime = CreateColumn(buProperty.LimiterTime.ToString(), buProperty.LimiterTime.ToString(), "Limiter Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in Enum.GetValues(typeof(LimiterTime)))
				limiterTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(limiterTime);

			DataGridViewComboBoxColumn limiterHash = CreateColumn(buProperty.LimiterListHash.ToString(), buProperty.LimiterListHash.ToString(), "Limiter List", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			limiterHash.DataSource = RTC_Core.LimiterListBindingSource;
			limiterHash.DisplayMember = "Name";
			limiterHash.ValueMember = "Value";
			dgvBlastEditor.Columns.Add(limiterHash);

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.InvertLimiter.ToString(), buProperty.InvertLimiter.ToString(), "Invert Limiter", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn storeTime = CreateColumn(buProperty.StoreTime.ToString(), buProperty.StoreTime.ToString(), "Store Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in Enum.GetValues(typeof(StoreTime)))
				storeTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(storeTime);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn storeType = CreateColumn(buProperty.StoreType.ToString(), buProperty.StoreType.ToString(), "Store Type", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			storeType.DataSource = Enum.GetValues(typeof(StoreType));
			dgvBlastEditor.Columns.Add(storeType);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn sourceDomain = CreateColumn(buProperty.SourceDomain.ToString(), buProperty.SourceDomain.ToString(), "Source Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			sourceDomain.DataSource = domains;
			dgvBlastEditor.Columns.Add(sourceDomain);

			DataGridViewNumericUpDownColumn sourceAddress = (DataGridViewNumericUpDownColumn)CreateColumn(buProperty.SourceAddress.ToString(), buProperty.SourceAddress.ToString(), "Source Address", new DataGridViewNumericUpDownColumn());
			sourceAddress.Hexadecimal = true;
			dgvBlastEditor.Columns.Add(sourceAddress);


			dgvBlastEditor.Columns.Add(CreateColumn("", buProperty.Note.ToString(), "Note", new DataGridViewButtonColumn()));


			VisibleColumns.Add(buProperty.isEnabled.ToString());
			VisibleColumns.Add(buProperty.isLocked.ToString());
			VisibleColumns.Add(buProperty.Source.ToString());
			VisibleColumns.Add(buProperty.Domain.ToString());
			VisibleColumns.Add(buProperty.Address.ToString());
			VisibleColumns.Add(buProperty.Precision.ToString());
			VisibleColumns.Add(buProperty.ValueString.ToString());
			VisibleColumns.Add(buProperty.Note.ToString());

			RefreshVisibleColumns();

			PopulateFilterCombobox();
			PopulateShiftCombobox();
		}


		private void PopulateFilterCombobox()
		{
			cbFilterColumn.SelectedItem = null;
			cbFilterColumn.Items.Clear();

			//Populate the filter ComboBox
			cbFilterColumn.DisplayMember = "Name";
			cbFilterColumn.ValueMember = "Value";
			foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
			{
				//Exclude button and checkbox
				if (!(column is DataGridViewCheckBoxColumn || column is DataGridViewButtonColumn))// && column.Visible)
					cbFilterColumn.Items.Add(new ComboBoxItem<String>(column.HeaderText, column.Name));
			}
		}

		private void PopulateShiftCombobox()
		{
			cbShiftBlastlayer.SelectedItem = null;
			cbShiftBlastlayer.Items.Clear();

			//Populate the filter ComboBox
			cbShiftBlastlayer.DisplayMember = "Name";
			cbShiftBlastlayer.ValueMember = "Value";

			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>(buProperty.Address.ToString(), buProperty.Address.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Source Address", buProperty.SourceAddress.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Value", buProperty.ValueString.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>(buProperty.Lifetime.ToString(), buProperty.Lifetime.ToString()));
			cbShiftBlastlayer.Items.Add(new ComboBoxItem<String>("Execute Frame", buProperty.ExecuteFrame.ToString()));
			cbShiftBlastlayer.SelectedIndex = 0;
		}


		public void RefreshVisibleColumns()
		{	
			foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
			{
				if (VisibleColumns.Contains(column.Name))
					column.Visible = true;
				else
					column.Visible = false;
			}
			dgvBlastEditor.Refresh();
		}



		private DataGridViewColumn CreateColumn(string dataPropertyName, string columnName, string displayName,
			DataGridViewColumn column, int fillWeight = -1)
		{

			if(fillWeight == -1)
			{

				switch (column)
				{
					case DataGridViewButtonColumn s:
						s.FillWeight = buttonFillWeight;
						break;
					case DataGridViewCheckBoxColumn s:
						s.FillWeight = checkBoxFillWeight;
						break;
					case DataGridViewComboBoxColumn s:
						s.FillWeight = comboBoxFillWeight;
						break;
					case DataGridViewTextBoxColumn s:
						s.FillWeight = textBoxFillWeight;
						break;
					case DataGridViewNumericUpDownColumn s:
						s.FillWeight = numericUpDownFillWeight;
						break;
				}
			}
			else
			{
				column.FillWeight = fillWeight;
			}


			column.DataPropertyName = dataPropertyName;
			column.Name = columnName;

			column.HeaderText = displayName;

			return column;
		}


		private DataGridViewColumn CreateColumnUnbound(string columnName, string displayName,
			DataGridViewColumn column, int fillWeight = -1)
		{
			return CreateColumn(String.Empty, columnName, displayName, column, fillWeight);
		}

		StashKey originalSK = null;
		StashKey currentSK = null;
		BindingSource bs = null;

		public void LoadStashkey(StashKey sk)
		{
			originalSK = sk;
			currentSK = sk.Clone() as StashKey;

			if (!RefreshDomains())
			{
				MessageBox.Show("Loading domains failed! Aborting load. Check to make sure the RTC and Bizhawk are connected.");
				this.Close();
				return;
			}


			bs = new BindingSource {DataSource = currentSK.BlastLayer.Layer};
		
			dgvBlastEditor.DataSource = bs;
			InitializeDGV();
			InitializeBottom();
			RefreshAllNoteIcons();

			this.Show();
		}


		private bool RefreshDomains()
		{
			try
			{
				S.GET<RTC_MemoryDomains_Form>().RefreshDomainsAndKeepSelected();
				domainToMiDico.Clear();
				domains = RTC_MemoryDomains.MemoryInterfaces.Keys.Concat(RTC_MemoryDomains.VmdPool.Values.Select(it => it.ToString())).ToArray();
				foreach (string domain in domains)
				{
					domainToMiDico.Add(domain, RTC_MemoryDomains.GetInterface(domain));
				}
				if(domainToMiDico.Keys.Count > 0)
					return true;
				return false;
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

		private void dgvBlastLayer_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBox.Show(e.Exception.ToString() + "\nRow:" + e.RowIndex + "\nColumn" + e.ColumnIndex + "\n" + e.Context + "\n" + dgvBlastEditor[e.ColumnIndex, e.RowIndex].Value?.ToString());
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => x.IsLocked == false))
			{
				bu.IsEnabled = true;
			}

			foreach (BlastUnit bu in currentSK.BlastLayer.Layer
				.Where(x => x.IsLocked == false)
				.OrderBy(x => RTC_Core.RND.Next())
				.Take(currentSK.BlastLayer.Layer.Count / 2))
			{
				bu.IsEnabled = false;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => x.IsLocked == false))
			{
				bu.IsEnabled = !bu.IsEnabled;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> buToRemove = new List<BlastUnit>();

			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => 
				x.IsLocked == false &&
				x.IsEnabled == false))
			{
				buToRemove.Add(bu);
			}

			foreach (BlastUnit bu in buToRemove)
			{
				bs.Remove(bu);
			}
		}

		private void btnDisableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x =>
				x.IsLocked == false))
			{
				bu.IsEnabled = false;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x =>
				x.IsLocked == false))
			{
				bu.IsEnabled = true;
			}
			dgvBlastEditor.Refresh();
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			foreach(DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
					bs.Remove(row.DataBoundItem as BlastUnit);
			}
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
				{
					BlastUnit bu = ((row.DataBoundItem as BlastUnit).Clone() as BlastUnit);
					bs.Add(bu);
				}
			}
		}

		private void btnSendToStash_Click(object sender, EventArgs e)
		{
			if (currentSK.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to send this to the stash.");
				return;
			}
			StashKey newSk = (StashKey)currentSK.Clone();
			//newSk.Key = RTC_Core.GetRandomKey();
			//newSk.Alias = null;

			RTC_StockpileManager.StashHistory.Add(newSk);

			S.GET<RTC_GlitchHarvester_Form>().RefreshStashHistory();
			S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.ClearSelection();
			S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.ClearSelected();

			S.GET<RTC_GlitchHarvester_Form>().DontLoadSelectedStash = true;
			S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex = S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.Items.Count - 1;
			RTC_StockpileManager.CurrentStashkey = RTC_StockpileManager.StashHistory[S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex];

		}

		private void btnSearchAgain_Click(object sender, EventArgs e)
		{
			if (searchEnumerable.Count() != 0 && searchOffset < searchEnumerable.Count())
				bs.Position = bs.IndexOf(searchEnumerable.ElementAt(searchOffset));
			else
			{ 
				MessageBox.Show("Reached end of list without finding anything.");
			}
			searchOffset++;
		}

		private void btnNote_Click(object sender, EventArgs e)
		{
			if (dgvBlastEditor.SelectedRows.Count > 0)
			{
				BlastLayer temp = new BlastLayer();
				List<DataGridViewCell> cellList = new List<DataGridViewCell>();
				foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				{
					if (row.DataBoundItem is BlastUnit bu)
					{
						temp.Layer.Add(bu);
						cellList.Add(row.Cells[buProperty.Note.ToString()]);
					}					
				}
				new RTC_NoteEditor_Form(temp, cellList);
			}
		}

		private void sanitizeDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<BlastUnit> bul = new List<BlastUnit>(currentSK.BlastLayer.Layer.ToArray().Reverse());
			List<long> usedAddresses = new List<long>();

			foreach (BlastUnit bu in bul)
			{
				if (!usedAddresses.Contains(bu.Address) && !bu.IsLocked)
					usedAddresses.Add(bu.Address);
				else
				{
					currentSK.BlastLayer.Layer.Remove(bu);
				}
			}
		}

		private void rasterizeVMDsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer)
			{
				bu.RasterizeVMDs();
			}
		}

		private void runRomWithoutBlastlayerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			currentSK.RunOriginal();
		}

		private void replaceRomFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{

			StashKey temp = RTC_StockpileManager.GetCurrentSavestateStashkey();

			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the Glitch Harvester, or the current selected box is empty");
				return;
			}
			currentSK.ParentKey = null;
			currentSK.RomFilename = temp.RomFilename;
			currentSK.RomData = temp.RomData;
			currentSK.GameName = temp.GameName;
			currentSK.SystemName = temp.SystemName;
			currentSK.SystemDeepName = temp.SystemDeepName;
			currentSK.SystemCore = temp.SystemCore;
			currentSK.SyncSettings = temp.SyncSettings;
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

				StashKey temp = new StashKey(RTC_Core.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);

				// We have to null this as to properly create a stashkey, we need to use it in the constructor,
				// but then the user needs to provide a savestate
				currentSK.ParentKey = null;
				
				currentSK.RomFilename = temp.RomFilename;
				currentSK.GameName = temp.GameName;
				currentSK.SystemName = temp.SystemName;
				currentSK.SystemDeepName = temp.SystemDeepName;
				currentSK.SystemCore = temp.SystemCore;
				currentSK.SyncSettings = temp.SyncSettings;
			}
		}

		private void bakeROMBlastunitsToFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//todo
			throw new NotImplementedException();
		}

		private void runOriginalSavestateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			originalSK.RunOriginal();
		}

		private void replaceSavestateFromGHToolStripMenuItem_Click(object sender, EventArgs e)
		{

			StashKey temp = RTC_StockpileManager.GetCurrentSavestateStashkey();
			if (temp == null)
			{
				MessageBox.Show("There is no savestate selected in the glitch harvester, or the current selected box is empty");
				return;
			}

			//If the core doesn't match, abort
			if (currentSK.SystemCore != temp.SystemCore)
			{
				MessageBox.Show("The core associated with the current ROM and the core associated with the selected savestate don't match. Aborting!");
				return;
			}

			//If the game name differs, make sure they know what they're doing
			//There are times it'd be fine with a differing name yet savestates would still work (romhacks)
			if (currentSK.GameName != temp.GameName)
			{
				DialogResult dialogResult = MessageBox.Show(
					"You're attempting to replace a savestate associated with " +
					currentSK.GameName +
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
			currentSK.ParentKey = temp.ParentKey;
			currentSK.SyncSettings = temp.SyncSettings;
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

			string oldKey = currentSK.ParentKey;
			string oldSS = currentSK.SyncSettings;

			//Get a new key
			currentSK.ParentKey = RTC_Core.GetRandomKey();
			//Null the syncsettings out
			currentSK.SyncSettings = null;

			//Let's hope the game name is correct!
			File.Copy(filename, currentSK.GetSavestateFullPath(), true);

			//Attempt to load and if it fails, don't let them update it.
			if (!RTC_StockpileManager.LoadState(currentSK))
			{
				currentSK.ParentKey = oldKey;
				currentSK.SyncSettings = oldSS;
				return;
			}

			//Grab the syncsettings
			StashKey temp = new StashKey(RTC_Core.GetRandomKey(), currentSK.ParentKey, currentSK.BlastLayer);
			currentSK.SyncSettings = temp.SyncSettings;
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
				filename = ofd.FileName;
			}
			else
				return;

			File.Copy(currentSK.GetSavestateFullPath(), filename, true);
		}

		private void loadFromFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{

			BlastLayer temp = RTC_BlastTools.LoadBlastLayerFromFile();
			if (temp != null)
			{
				currentSK.BlastLayer = temp;
			}
		}

		private void saveToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//If there's no blastlayer file already set, don't quicksave
			if (CurrentBlastLayerFile == "")
				RTC_BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
			else
				RTC_BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer, CurrentBlastLayerFile);

			CurrentBlastLayerFile = RTC_BlastTools.LastBlastLayerSavePath;
		}

		private void saveAsToFileblToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RTC_BlastTools.SaveBlastLayerToFile(currentSK.BlastLayer);
			CurrentBlastLayerFile = RTC_BlastTools.LastBlastLayerSavePath;
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
					currentSK.BlastLayer.Layer.Add(bu);
			}
		}

		private void exportToCSVToolStripMenuItem_Click(object sender, EventArgs e)
		{

			string filename;

			if (currentSK.BlastLayer.Layer.Count == 0)
			{
				MessageBox.Show("Can't save because the provided blastlayer is empty.");
				return;
			}

			SaveFileDialog saveFileDialog1 = new SaveFileDialog
			{
				DefaultExt = "csv",
				Title = "Export to csv",
				Filter = "csv files|*.csv",
				RestoreDirectory = true
			};

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				filename = saveFileDialog1.FileName;
			}
			else
				return;
			CSVGenerator csv = new CSVGenerator();
			File.WriteAllText(filename, csv.GenerateFromDGV(dgvBlastEditor), Encoding.UTF8);
		}

		private void bakeBlastunitsToVALUEToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var token = RTC_NetCore.HugeOperationStart("DISABLED");
			try
			{
				//Generate a blastlayer from the current selected rows
				BlastLayer bl = new BlastLayer();
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
				{
					BlastUnit bu = (BlastUnit)selected.DataBoundItem;
					
					//They have to be enabled to get a backup
					bu.IsEnabled = true;
					bl.Layer.Add(bu);
				}

				//Bake them
				BlastLayer newBlastLayer = RTC_BlastTools.BakeBlastUnitsToSet(currentSK, bl);

				int i = 0;
				//Insert the new one where the old row was, then remove the old row.
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
				{
					currentSK.BlastLayer.Layer.Insert(selected.Index, newBlastLayer.Layer[i]);
					i++;
					currentSK.BlastLayer.Layer.Remove((BlastUnit)selected.DataBoundItem);
				}
			}
			catch (Exception ex)
			{
				throw new System.Exception("Something went wrong in when baking to VALUE.\n" +
				                           "Your blast editor session may be broke depending on when it failed.\n" +
				                           "You should probably send a copy of this error and what you did to cause it to the RTC devs.\n\n" +
				                           ex.ToString());
			}
			finally
			{
				RTC_NetCore.HugeOperationEnd(token);
			}
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{

			if (currentSK.ParentKey == null)
			{
				MessageBox.Show("There's no savestate associated with this Stashkey!\nAssociate one in the menu to be able to load.");
				return;
			}
			
			StashKey newSk = (StashKey)currentSK.Clone();
			newSk.Run();
		}

		private void btnCorrupt_Click(object sender, EventArgs e)
		{
			StashKey newSk = (StashKey)currentSK.Clone();
			newSk.BlastLayer?.Apply();
		}

		public void RefreshAllNoteIcons()
		{
			foreach (DataGridViewRow row in dgvBlastEditor.Rows)
			{
				DataGridViewCell buttonCell = row.Cells[buProperty.Note.ToString()];
				buttonCell.Value = string.IsNullOrWhiteSpace((row.DataBoundItem as BlastUnit)?.Note) ? string.Empty : "📝";
			}
		}


		private void DgvBlastEditor_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			UpdateLayerSize();
		}

		private void DgvBlastEditor_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateLayerSize();
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
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>()
					.Where((item => ((BlastUnit)item.DataBoundItem).IsLocked == false)))
				{
					var cell = selected.Cells[((ComboBoxItem<String>)cbShiftBlastlayer.SelectedItem).Value];

					//Can't use a switch statement because tostring is evaluated at runtime
					if (cell.OwningColumn.Name == buProperty.SourceAddress.ToString() ||
						cell.OwningColumn.Name == buProperty.Address.ToString() ||
						cell.OwningColumn.Name == buProperty.Lifetime.ToString() ||
						cell.OwningColumn.Name == buProperty.ExecuteFrame.ToString())
					{
						var u = (DataGridViewNumericUpDownCell)cell;
						if (shiftDown)
							{
								if (((long)u.Value - updownShiftBlastLayerAmount.Value) >= 0)
									u.Value = (long)u.Value - updownShiftBlastLayerAmount.Value;
								else
									u.Value = 0;

						}
						else
						{
							if (((long)u.Value + updownShiftBlastLayerAmount.Value) <= u.Maximum)
								u.Value = (long)u.Value + updownShiftBlastLayerAmount.Value;
							else
								u.Value = u.Maximum;
						}
					}
					else if (cell.OwningColumn.Name == buProperty.ValueString.ToString())
					{
						//Unlike addresses, we want values to roll over so we work on the underlying value and use existing code 
						BlastUnit bu = (BlastUnit)cell.OwningRow.DataBoundItem;

						if (shiftDown)
							bu.Value = RTC_Extensions.AddValueToByteArrayUnchecked(bu.Value, new BigInteger(0 - updownShiftBlastLayerAmount.Value), bu.BigEndian);
						else
							bu.Value = RTC_Extensions.AddValueToByteArrayUnchecked(bu.Value, new BigInteger(updownShiftBlastLayerAmount.Value), bu.BigEndian);

					}
					else
					{
						throw new NotImplementedException("Invalid column type.");
					}

				}
			dgvBlastEditor.Refresh();
			UpdateBottom();
		}

		private void UpdateLayerSize()
		{
			lbBlastLayerSize.Text = "Size: " + currentSK.BlastLayer.Layer.Count;
		}
	}
}

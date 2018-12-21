using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
			LimiterHash,
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
				dgvBlastEditor.CellClick += dgvBlastEditor_CellClick;
				
				tbFilter.TextChanged += tbFilter_TextChanged;

				cbEnabled.Validated += cbEnabled_Validated;
				cbLocked.Validated += CbLocked_Validated;
				cbBigEndian.Validated += CbBigEndian_Validated;

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

		private void dgvBlastEditor_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// Note handling
			if (e != null && e.RowIndex != -1)
			{
				if (e.ColumnIndex == dgvBlastEditor.Columns[buProperty.Note.ToString()]?.Index )
				{
					BlastUnit bu = dgvBlastEditor.Rows[e.RowIndex].DataBoundItem as BlastUnit;
					if (bu != null)
						 new RTC_NoteEditor_Form(bu, dgvBlastEditor[e.ColumnIndex, e.RowIndex]);
				}
			}
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
				row.Cells[buProperty.LimiterHash.ToString()].Value = cbLimiterList.SelectedItem;
			UpdateBottom();
		}

		private void CbBigEndian_Validated(object sender, EventArgs e)
		{
			//Big Endian isn't available in the DGV so we operate on the actual BU then refresh
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
				row.Cells[buProperty.isLocked.ToString()].Value = cbInvertLimiter.Checked;
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
				cell.Maximum = domainToMiDico[domain].Size;
		}
		
		private void UpdateBottom()
		{
			if (dgvBlastEditor.SelectedRows.Count > 0)
			{
				var lastRow = dgvBlastEditor.SelectedRows[dgvBlastEditor.SelectedRows.Count - 1];
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

				tbTiltValue.Text = (lastRow.DataBoundItem as BlastUnit).TiltValue.ToString();
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
			string value = (cbFilterColumn.SelectedItem as dynamic).Value;
			dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer.Where(x => (x.GetType()
			.GetProperty(value)
			.GetValue(x)
			.ToString() == tbFilter.Text)).ToList();

			if (tbFilter.TextLength == 0)
				dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer;
		}
	
		private void InitializeBottom()
		{
			property2ControlDico = new Dictionary<string, Control>();

			var actionTime = Enum.GetValues(typeof(ActionTime));
			var storeType = Enum.GetValues(typeof(StoreType));
			var blastUnitSource = Enum.GetValues(typeof(BlastUnitSource));

			cbDomain.DataSource = domains;
			cbSourceDomain.DataSource = domains;

			foreach (var item in actionTime)
			{
				cbLimiterTime.Items.Add(item);
				cbStoreTime.Items.Add(item);
			}
			foreach (var item in blastUnitSource)
			{
				cbSource.Items.Add(item);
			}

			cbLimiterList.DataSource = RTC_Core.LimiterListBindingSource;
			cbLimiterList.DisplayMember = "Text";
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
			property2ControlDico.Add(buProperty.LimiterHash.ToString(), cbLimiterList);
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
			var actionTime = Enum.GetValues(typeof(ActionTime));
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
			precision.Maximum = Int32.MaxValue;
			dgvBlastEditor.Columns.Add(precision);

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.ValueString.ToString(), buProperty.ValueString.ToString(), "Value", new DataGridViewTextBoxColumn()));


			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.ExecuteFrame.ToString(), buProperty.ExecuteFrame.ToString(), "Execute Frame", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.Lifetime.ToString(), buProperty.Lifetime.ToString(), "Lifetime", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.Loop.ToString(), buProperty.Loop.ToString(), "Loop", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn limiterTime = CreateColumn(buProperty.LimiterTime.ToString(), buProperty.LimiterTime.ToString(), "Limiter Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in actionTime)
				limiterTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(limiterTime);

			DataGridViewComboBoxColumn limiterHash = CreateColumn(buProperty.LimiterHash.ToString(), buProperty.LimiterHash.ToString(), "Limiter List", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			limiterHash.DataSource = RTC_Core.LimiterListBindingSource;
			limiterHash.DisplayMember = "Text";
			limiterHash.ValueMember = "Value";
			dgvBlastEditor.Columns.Add(limiterHash);

			dgvBlastEditor.Columns.Add(CreateColumn(buProperty.InvertLimiter.ToString(), buProperty.InvertLimiter.ToString(), "Invert Limiter", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn storeTime = CreateColumn(buProperty.StoreTime.ToString(), buProperty.StoreTime.ToString(), "Store Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in actionTime)
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


			//Populate the filter ComboBox
			cbFilterColumn.DisplayMember = "Text";
			cbFilterColumn.ValueMember = "Value";
			foreach (DataGridViewColumn column in dgvBlastEditor.Columns)
			{
				//Exclude button and checkbox
				if (!(column is DataGridViewCheckBoxColumn || column is DataGridViewButtonColumn) && column.Visible)
					cbFilterColumn.Items.Add(new { Text = column.HeaderText, Value = column.Name });
			}

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
			originalSK = sk.Clone() as StashKey;
			currentSK = sk.Clone() as StashKey;
			RefreshDomains();

			bs = new BindingSource {DataSource = currentSK.BlastLayer.Layer};
		
			dgvBlastEditor.DataSource = bs;
			InitializeDGV();
			InitializeBottom();
			RefreshAllNoteIcons();

			this.Show();
		}


		private void RefreshDomains()
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
				foreach(DataGridViewColumn item in dgvBlastEditor.Columns)
				{
					//Exclude button and checkbox
					if (!(item is DataGridViewCheckBoxColumn || item is DataGridViewButtonColumn))
						column.Items.Add(new { Text = item.HeaderText, Value = item.Name });					
				}
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
			label.Anchor =  AnchorStyles.Bottom | AnchorStyles.Left;
			input.Anchor = input.Anchor | AnchorStyles.Left;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

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
			else
				searchColumn = string.Empty;
			return dialogResult;
		}

		//Provides a dialog box that searches for a row in the DGV
		private void btnSearchRow_Click(object sender, EventArgs e)
		{
			if (GetSearchBox("Search for a value", "Choose a column and enter a value", true) == DialogResult.OK)
			{
				if(searchColumn != null)
				{
					searchOffset = 0;
					searchEnumerable = currentSK.BlastLayer.Layer.Where(x => (x.GetType().GetProperty(searchColumn).GetValue(x).ToString()) == searchValue);
					
					if (searchEnumerable.Count() != 0)
						bs.Position = bs.IndexOf(searchEnumerable.ElementAt(searchOffset));
					else
						MessageBox.Show("Reached end of list without finding anything.");
					searchOffset++;					
					
				}
				else
				{
					List<string> metaList = (List<string>)bs.DataSource;
					metaList.FindIndex(s => string.Equals(s, searchValue, StringComparison.CurrentCultureIgnoreCase));
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
			RTC_StockpileManager.currentStashkey = RTC_StockpileManager.StashHistory[S.GET<RTC_GlitchHarvester_Form>().lbStashHistory.SelectedIndex];

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

			StashKey temp = RTC_StockpileManager.getCurrentSavestateStashkey();

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

			StashKey temp = RTC_StockpileManager.getCurrentSavestateStashkey();
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
				filename = ofd.FileName.ToString();
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
				foreach (DataGridViewRow selected in dgvBlastEditor.SelectedRows.Cast<DataGridViewRow>().Where(item =>
					((bool)item.Cells["dgvBlastUnitLocked"].Value != true)))
				{
					currentSK.BlastLayer.Layer.Insert(selected.Index, newBlastLayer.Layer[i]);
					i++;
					currentSK.BlastLayer.Layer.Remove((BlastUnit)selected.DataBoundItem);
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

		public void RefreshAllNoteIcons()
		{
			foreach (DataGridViewRow row in dgvBlastEditor.Rows)
			{
				DataGridViewCell buttonCell = row.Cells[buProperty.Note.ToString()];
				buttonCell.Value = string.IsNullOrWhiteSpace((row.DataBoundItem as BlastUnit)?.Note) ? string.Empty : "📝";

			}
		}
	}
}

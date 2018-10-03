using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

		private int searchOffset = 0;
		private IEnumerable<BlastUnit> searchEnumerable;
		BindingList<BlastUnit> selectedBUs = new BindingList<BlastUnit>();
		ContextMenuStrip headerStrip;
		



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
				dgvBlastEditor.ColumnHeaderMouseClick += dgvBlastEditor_ColumnHeaderMouseClick; ;
				tbFilter.TextChanged += tbFilter_TextChanged;
				upDownAddress.Validated += OnValidated;
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
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

		private void OnValidated(object sender, EventArgs e)
		{
			dynamic bad = (sender as dynamic);
			if((bad?.DataBindings?.Count ?? 0) > 0)
			{
				PropertyInfo property = bad.DataBindings[0].DataSource.GetType().GetProperty(bad.DataBindings[0].BindingMemberInfo.BindingField);
				dynamic value = property.GetValue(bad.DataBindings[0].DataSource);

				foreach (BlastUnit bu in selectedBUs)
				{
					property.SetValue(bu, value);
				}
			}
			dgvBlastEditor.Refresh();
		}

		private void dgvBlastEditor_SelectionChanged(object sender, EventArgs e)
		{
			selectedBUs = new BindingList<BlastUnit>();

			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
				selectedBUs.Add(row.DataBoundItem as BlastUnit);

			upDownAddress.DataBindings.Clear();
			upDownPrecision.DataBindings.Clear();
			if (selectedBUs.Count > 0) {
				upDownAddress.DataBindings.Add("Text", selectedBUs[selectedBUs.Count - 1], "Address", true, DataSourceUpdateMode.OnValidation);
				upDownPrecision.DataBindings.Add("Value", selectedBUs[selectedBUs.Count - 1], "Precision", true, DataSourceUpdateMode.OnValidation);
			}
		}

		private void selectedBUs_ListChanged(object sender, ListChangedEventArgs e)
		{
			if(e.ListChangedType == ListChangedType.ItemChanged)
			{
				PropertyDescriptor property = e.PropertyDescriptor;
				BlastUnit changed = selectedBUs[e.NewIndex];
				//Get the value of the changed property
				object value = property.GetValue(changed);
				foreach (BlastUnit bu in selectedBUs)
					property.SetValue(bu, value);
			}
		}

		private void tbFilter_TextChanged(object sender, EventArgs e)
		{
			string value = (cbFilterColumn.SelectedItem as dynamic).Value;
			dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer.Where(x => (x.GetType().GetProperty(value).GetValue(x).ToString() == tbFilter.Text)).ToList();

			if (tbFilter.TextLength == 0)
				dgvBlastEditor.DataSource = currentSK.BlastLayer.Layer;
		}

		private void InitializeDGV()
		{

			VisibleColumns = new List<string>();
			var actionTime = Enum.GetValues(typeof(ActionTime));

			dgvBlastEditor.Columns.Add(CreateColumn("isEnabled", "Enabled", new DataGridViewCheckBoxColumn()));
			

			dgvBlastEditor.Columns.Add(CreateColumn("isLocked", "Locked", new DataGridViewCheckBoxColumn()));
			

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn domain = CreateColumn("Domain", "Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			domain.DataSource = domains;
			dgvBlastEditor.Columns.Add(domain);

			dgvBlastEditor.Columns.Add(CreateColumn("Address", "Address", new DataGridViewNumericUpDownColumn()));
			
			dgvBlastEditor.Columns.Add(CreateColumn("Precision", "Precision", new DataGridViewNumericUpDownColumn()));

			dgvBlastEditor.Columns.Add(CreateColumn("ValueString", "Value", new DataGridViewTextBoxColumn()));


			dgvBlastEditor.Columns.Add(CreateColumn("ExecuteFrame", "Execute Frame", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn("Lifetime", "Lifetime", new DataGridViewNumericUpDownColumn()));
			dgvBlastEditor.Columns.Add(CreateColumn("Loop", "Loop", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn limiterTime = CreateColumn("LimiterTime", "Limiter Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in actionTime)
				limiterTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(limiterTime);

			DataGridViewComboBoxColumn limiterHash = CreateColumn("LimiterHash", "Limiter List", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			limiterHash.DataSource = RTC_Core.LimiterListBindingSource;
			limiterHash.DisplayMember = "Text";
			limiterHash.ValueMember = "Value";
			dgvBlastEditor.Columns.Add(limiterHash);

			dgvBlastEditor.Columns.Add(CreateColumn("InvertLimiter", "Invert Limiter", new DataGridViewCheckBoxColumn()));


			DataGridViewComboBoxColumn storeTime = CreateColumn("StoreTime", "Store Time", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			foreach (var item in actionTime)
				storeTime.Items.Add(item);
			dgvBlastEditor.Columns.Add(storeTime);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn storeType = CreateColumn("StoreType", "Store Type", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			storeType.DataSource = Enum.GetValues(typeof(StoreType));
			dgvBlastEditor.Columns.Add(storeType);

			//Do this one separately as we need to populate the Combobox
			DataGridViewComboBoxColumn sourceDomain = CreateColumn("SourceDomain", "Source Domain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			sourceDomain.DataSource = domains;
			dgvBlastEditor.Columns.Add(sourceDomain);

			dgvBlastEditor.Columns.Add(CreateColumn("SourceAddress", "Source Address", new DataGridViewNumericUpDownColumn()));


			dgvBlastEditor.Columns.Add(CreateColumn("Note", "Note", new DataGridViewButtonColumn()));


			VisibleColumns.Add("isEnabled");
			VisibleColumns.Add("isLocked");
			VisibleColumns.Add("Domain");
			VisibleColumns.Add("Address");
			VisibleColumns.Add("Precision");
			VisibleColumns.Add("ValueString");
			VisibleColumns.Add("Note");

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

		private DataGridViewColumn CreateColumn(string dataPropertyName, string displayName, DataGridViewColumn column, int fillWeight = -1)
		{

			if(fillWeight == -1)
			{
				int buttonFillWeight = 20;
				int checkBoxFillWeight = 25;
				int comboBoxFillWeight = 40;
				int textBoxFillWeight = 30;
				int numericUpDownFillWeight = 35;

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
			column.Name = dataPropertyName;

			column.HeaderText = displayName;

			return column;
		}


		StashKey currentSK = null;
		BindingSource bs = null;

		public void LoadStashkey(StashKey sk)
		{
			currentSK = sk.Clone() as StashKey;
			RefreshDomains();

			bs = new BindingSource(currentSK.BlastLayer.Layer, "");
		
			dgvBlastEditor.DataSource = bs;
			InitializeDGV();

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
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{

			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x => 
				x.IsLocked == false &&
				x.IsEnabled == false))
			{
				currentSK.BlastLayer.Layer.Remove(bu);
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
		}

		private void btnEnableEverything_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in currentSK.BlastLayer.Layer.
				Where(x =>
				x.IsLocked == false))
			{
				bu.IsEnabled = true;
			}
		}

		private void btnRemoveSelected_Click(object sender, EventArgs e)
		{
			foreach(DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
					currentSK.BlastLayer.Layer.Remove(row.DataBoundItem as BlastUnit);
			}
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			foreach (DataGridViewRow row in dgvBlastEditor.SelectedRows)
			{
				if ((row.DataBoundItem as BlastUnit).IsLocked == false)
				{
					BlastUnit bu = ((row.DataBoundItem as BlastUnit).Clone() as BlastUnit);
					currentSK.BlastLayer.Layer.Add(bu);
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



	}
}

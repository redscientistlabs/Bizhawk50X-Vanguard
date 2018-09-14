using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

		//We gotta cache this stuff outside of the scope of InitializeDGV
	//	private object actionTimeValues = 


		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();
			dgvBlastEditor.AutoGenerateColumns = false;

			dgvBlastEditor.DataError += dgvBlastLayer_DataError;

		}


		private void InitializeDGV()
		{
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
			DataGridViewComboBoxColumn sourceDomain = CreateColumn("SourceDomain", "SourceDomain", new DataGridViewComboBoxColumn()) as DataGridViewComboBoxColumn;
			sourceDomain.DataSource = domains;
			dgvBlastEditor.Columns.Add(sourceDomain);

			dgvBlastEditor.Columns.Add(CreateColumn("SourceAddress", "Source Address", new DataGridViewNumericUpDownColumn()));


			dgvBlastEditor.Columns.Add(CreateColumn("Note", "Note", new DataGridViewButtonColumn()));

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

			bs = new BindingSource();
			bs.DataSource = sk.BlastLayer.Layer;
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
	}
}

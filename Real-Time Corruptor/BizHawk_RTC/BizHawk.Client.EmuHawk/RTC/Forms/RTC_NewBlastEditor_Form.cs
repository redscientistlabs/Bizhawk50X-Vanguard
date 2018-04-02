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
	public partial class RTC_NewBlastEditor_Form : Form
	{
		StashKey sk = null;
		bool initialized = false;
		bool CurrentlyUpdating = false;

		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();
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
					Nightmare  = Address
					BlastCheat = Address
					BlastPipe  = Source Address
				 param2
					Nightmare  = Value
					BlastCheat = Value
					BlastPipe  = Destination Address

				*/

				//Valid for all types
				bool enabled = bu.IsEnabled;
				string blastType = Convert.ToString(bu.GetType());

				//Dependent on blast type
				int precision = -1;
				string sourceDomain = "";
				string destDomain = "";
				long sourceAddress = -1;
				decimal destAddress = -1;
				string blastMode = "";

				if (bu is BlastByte)
				{
					BlastByte bb = bu as BlastByte;
					precision = bb.Value.Length;
					sourceAddress = bb.Address;
					sourceDomain = bb.Domain;
					destAddress = RTC_Extensions.getDecimalValue(bb.Value);
					blastMode = Convert.ToString(bb.Type);
				}

				else if (bu is BlastCheat)
				{
					BlastCheat bc = bu as BlastCheat;
					precision = bc.Value.Length;
					sourceAddress = bc.Address;
					sourceDomain = bc.Domain;
					destAddress = RTC_Extensions.getDecimalValue(bc.Value);
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
					blastMode = "Tilt: " + Convert.ToString(bp.TiltValue);
				}
				dgvBlastLayer.Rows.Add(bu, enabled, precision, blastType, blastMode, sourceDomain, sourceAddress, destDomain, destAddress);
			}
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

		public DialogResult getInputBox(string title, string promptText, ref string value)
		{
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}


		private void RTC_BlastEditorForm_Load(object sender, EventArgs e)
		{
			ContextMenu contextMenu = new ContextMenu();
			//this.nmAddressEdit.ContextMenu = contextMenu;
			//this.nmValueEdit.ContextMenu = contextMenu;
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

				switch (row.Cells["dgvBlastUnitType"].Value)
				{
				case "RTC.BlastByte":
					BlastByte bb = (BlastByte)row.Cells["dgvBlastUnitReference"].Value;
					bb.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bb.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bb.Value = RTC_Extensions.getByteArrayValue(Convert.ToInt32(row.Cells["dgvPrecision"].Value), Convert.ToDecimal(row.Cells["dgvParam2"].Value));
					bb.Domain = Convert.ToString(row.Cells["dgvParam1Domain"].Value);
					Enum.TryParse(row.Cells["dgvBUMode"].Value.ToString().ToUpper(), out bb.Type);
					row.Cells["dgvBlastUnitReference"].Value = bb;
					CurrentlyUpdating = false;
					break;
				case "RTC.BlastCheat":
					BlastCheat bc = (BlastCheat)row.Cells["dgvBlastUnitReference"].Value;
					bc.IsEnabled = Convert.ToBoolean((row.Cells["dgvBlastEnabled"].Value));
					bc.Address = Convert.ToInt64(row.Cells["dgvParam1"].Value);
					bc.Value = RTC_Extensions.getByteArrayValue(Convert.ToInt32(row.Cells["dgvPrecision"].Value), Convert.ToDecimal(row.Cells["dgvParam2"].Value));
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
				default:
					MessageBox.Show("You had an invalid blast unit type! Check your input. The invalid unit is: " + row.Cells["dgvBlastUnitType"].Value);
					CurrentlyUpdating = false;
					break;

				}
			}
		}
	}
}

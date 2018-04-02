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

		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null || _sk.BlastLayer == null || _sk.BlastLayer.Layer == null)
				return;

			//lbBlastLayer.Items.Clear();
			sk = (StashKey)_sk.Clone();

			//lbBlastLayer.DataSource = sk.BlastLayer.Layer;

			//foreach (var item in sk.blastlayer.Layer)
			//	lbBlastLayer.Items.Add(item);

			RefreshBlastLayer();

			this.Show();
		}

		public void RefreshBlastLayer()
		{
			lbBlastLayer.DataSource = null;
			lbBlastLayer.DataSource = sk.BlastLayer.Layer;

			lbBlastLayerSize.Text = "BlastLayer size: " + sk.BlastLayer.Layer.Count.ToString();
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.BlastLayer.Layer)
				bu.IsEnabled = true;

			foreach (BlastUnit bu in sk.BlastLayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.BlastLayer.Layer.Count / 2))
				bu.IsEnabled = false;

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

			foreach (var item in lbBlastLayer.Items)
			{
				BlastUnit bu = (item as BlastUnit);
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

			foreach (var item in lbBlastLayer.Items)
			{
				BlastUnit bu = (item as BlastUnit);
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

		private void lbBlastLayer_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (lbBlastLayer.SelectedIndex == -1)
				return;

			(lbBlastLayer.SelectedItem as BlastUnit).IsEnabled = !(lbBlastLayer.SelectedItem as BlastUnit).IsEnabled;

			RefreshBlastLayer();
		}

		private void btnInvertDisabled_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < sk.BlastLayer.Layer.Count(); i++)
				sk.BlastLayer.Layer[i].IsEnabled = !sk.BlastLayer.Layer[i].IsEnabled;

			RefreshBlastLayer();
		}

		private void lbBlastLayer_SelectedIndexChanged(object sender, EventArgs e)
		{
			gbAddressEdit.Visible = false;
			gbValueEdit.Visible = false;

			lbAddressEdit.Text = "Address Edit:";
			lbValueEdit.Text = "Value Edit:";

			if (lbBlastLayer.SelectedIndex == -1)
				return;

			BlastUnit bu = (BlastUnit)lbBlastLayer.SelectedItem;
			nmValueEdit.Value = 0;
			nmValueEdit.Maximum = Int64.MaxValue;

			if (lbBlastLayer.SelectedItem is BlastByte)
			{
				gbAddressEdit.Visible = true;
				gbValueEdit.Visible = true;
				nmAddressEdit.Value = (lbBlastLayer.SelectedItem as BlastByte).Address;

				var bb = (bu as BlastByte);
				nmValueEdit.Maximum = RTC_Extensions.getNumericMaxValue(bb.Value);
				nmValueEdit.Value = RTC_Extensions.getDecimalValue(bb.Value);
			}
			else if (lbBlastLayer.SelectedItem is BlastCheat)
			{
				gbAddressEdit.Visible = true;
				nmAddressEdit.Value = (lbBlastLayer.SelectedItem as BlastCheat).Address;
				var bc = (bu as BlastCheat);
				if (!bc.IsFreeze)
				{
					gbValueEdit.Visible = true;

					nmValueEdit.Maximum = RTC_Extensions.getNumericMaxValue(bc.Value);
					nmValueEdit.Value = RTC_Extensions.getDecimalValue(bc.Value);
				}
			}
			else if (lbBlastLayer.SelectedItem is BlastPipe)
			{
				lbAddressEdit.Text = "Address Edit:";
				lbValueEdit.Text = "PipeAddress Edit:";

				gbAddressEdit.Visible = true;
				gbValueEdit.Visible = true;
				nmAddressEdit.Value = (bu as BlastPipe).Address;
				nmValueEdit.Value = (bu as BlastPipe).PipeAddress;
				nmValueEdit.Maximum = nmAddressEdit.Maximum;
			}
		}

		private void btnAdressUpdate_Click(object sender, EventArgs e)
		{
			if (lbBlastLayer.SelectedItem is BlastByte)
				(lbBlastLayer.SelectedItem as BlastByte).Address = Convert.ToInt64(nmAddressEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastCheat)
				(lbBlastLayer.SelectedItem as BlastCheat).Address = Convert.ToInt64(nmAddressEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastPipe)
				(lbBlastLayer.SelectedItem as BlastPipe).Address = Convert.ToInt64(nmAddressEdit.Value);

			RefreshBlastLayer();
		}

		private void btnValueUpdate_Click(object sender, EventArgs e)
		{
			if (lbBlastLayer.SelectedItem is BlastByte)
				(lbBlastLayer.SelectedItem as BlastByte).Value = RTC_Extensions.getByteArrayValue((lbBlastLayer.SelectedItem as BlastByte).Value, nmValueEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastCheat)
				(lbBlastLayer.SelectedItem as BlastCheat).Value = RTC_Extensions.getByteArrayValue((lbBlastLayer.SelectedItem as BlastCheat).Value, nmValueEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastPipe)
				(lbBlastLayer.SelectedItem as BlastPipe).PipeAddress = Convert.ToInt64(nmValueEdit.Value);

			RefreshBlastLayer();
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


		private void nmAddressEdit_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X + gbAddressEdit.Location.X, (sender as Control).Location.Y + e.Location.Y + gbAddressEdit.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Import from Hex", null, new EventHandler((ob, ev) => {
					string value = "";
					if (this.getInputBox("Import from Hex", "Enter Hexadecimal number:", ref value) == DialogResult.OK)
					{
						int newValue = int.Parse(value, NumberStyles.HexNumber);
						nmAddressEdit.Value = Convert.ToDecimal(newValue);
					}
				}));
				columnsMenu.Show(this, locate);
			}
		}

		private void nmValueEdit_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X + gbValueEdit.Location.X, (sender as Control).Location.Y + e.Location.Y + gbValueEdit.Location.Y);

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				columnsMenu.Items.Add("Import from Hex", null, new EventHandler((ob, ev) => {
					string value = "";
					if (this.getInputBox("Import from Hex", "Enter Hexadecimal number:", ref value) == DialogResult.OK)
					{
						int newValue = int.Parse(value, NumberStyles.HexNumber);
						nmValueEdit.Value = Convert.ToDecimal(newValue);
					}
				}));
				columnsMenu.Show(this, locate);
			}
		}

		private void RTC_BlastEditorForm_Load(object sender, EventArgs e)
		{
			ContextMenu contextMenu = new ContextMenu();
			this.nmAddressEdit.ContextMenu = contextMenu;
			this.nmValueEdit.ContextMenu = contextMenu;
		}

		private void btnDuplicateSelected_Click(object sender, EventArgs e)
		{
			if (lbBlastLayer.SelectedIndex == -1)
			{
				MessageBox.Show("No unit was selected. Cannot duplicate.");
				return;
			}

			int pos = lbBlastLayer.SelectedIndex;

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
	}
}

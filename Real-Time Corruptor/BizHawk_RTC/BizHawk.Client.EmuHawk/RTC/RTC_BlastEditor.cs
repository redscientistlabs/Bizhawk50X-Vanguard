using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_BlastEditorForm : Form
	{
		StashKey sk = null;

		public RTC_BlastEditorForm()
		{
			InitializeComponent();
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			lbBlastLayer.Items.Clear();
			sk = (StashKey)_sk.Clone();

			lbBlastLayer.DataSource = sk.blastlayer.Layer;

			//foreach (var item in sk.blastlayer.Layer)
			//	lbBlastLayer.Items.Add(item);

			this.Show();
		}

		public void RefreshBlastLayer()
		{
			lbBlastLayer.DataSource = null;
			lbBlastLayer.DataSource = sk.blastlayer.Layer;
		}

		private void btnDisable50_Click(object sender, EventArgs e)
		{
			foreach (BlastUnit bu in sk.blastlayer.Layer)
				bu.IsEnabled = true;

			foreach (BlastUnit bu in sk.blastlayer.Layer.OrderBy(x => RTC_Core.RND.Next()).Take(sk.blastlayer.Layer.Count / 2))
				bu.IsEnabled = false;

			RefreshBlastLayer();
		}

		private void btnRemoveDisabled_Click(object sender, EventArgs e)
		{
			List<BlastUnit> newLayer = new List<BlastUnit>();

			foreach (BlastUnit bu in sk.blastlayer.Layer)
				if (bu.IsEnabled)
					newLayer.Add(bu);

			sk.blastlayer.Layer = newLayer;

			RefreshBlastLayer();
		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{
			BlastLayer bl = new BlastLayer();

			foreach(var item in lbBlastLayer.Items)
			{
				BlastUnit bu = (item as BlastUnit);
				if (bu.IsEnabled)
					bl.Layer.Add(bu);
			}

			StashKey newSk = (StashKey)sk.Clone();

			newSk.blastlayer = bl;

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

			bl.Apply();
		}

		private void btnSendToStash_Click(object sender, EventArgs e)
		{
			StashKey newSk = (StashKey)sk.Clone();
			newSk.Key = RTC_Core.GetRandomKey();
			newSk.Alias = null;

			RTC_StockpileManager.StashHistory.Add(newSk);
			RTC_Core.ghForm.RefreshStashHistory();
			RTC_Core.ghForm.dgvStockpile.ClearSelection();
			RTC_Core.ghForm.DontLoadSelectedStash = true;
			RTC_Core.ghForm.lbStashHistory.SelectedIndex = RTC_Core.ghForm.lbStashHistory.Items.Count - 1;
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
			for (int i = 0; i < sk.blastlayer.Layer.Count(); i++)
				sk.blastlayer.Layer[i].IsEnabled = !sk.blastlayer.Layer[i].IsEnabled;

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

			if(lbBlastLayer.SelectedItem is BlastByte)
			{
				gbAddressEdit.Visible = true;
				gbValueEdit.Visible = true;
				nmAddressEdit.Value = (lbBlastLayer.SelectedItem as BlastByte).Address;
				nmValueEdit.Value = (lbBlastLayer.SelectedItem as BlastByte).Value;
			}
			else if (lbBlastLayer.SelectedItem is BlastCheat)
			{
				gbAddressEdit.Visible = true;
				nmAddressEdit.Value = (lbBlastLayer.SelectedItem as BlastCheat).Address;
				if(!(lbBlastLayer.SelectedItem as BlastCheat).IsFreeze)
				{
					gbValueEdit.Visible = true;
					nmValueEdit.Value = (lbBlastLayer.SelectedItem as BlastCheat).Value;
				}
			}
			else if (lbBlastLayer.SelectedItem is BlastPipe)
			{
				lbAddressEdit.Text = "Address Edit:";
				lbValueEdit.Text = "PipeAddress Edit:";

				gbAddressEdit.Visible = true;
				gbValueEdit.Visible = true;
				nmAddressEdit.Value = (lbBlastLayer.SelectedItem as BlastPipe).Address;
				nmValueEdit.Value = (lbBlastLayer.SelectedItem as BlastPipe).PipeAddress;
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
				(lbBlastLayer.SelectedItem as BlastByte).Value = Convert.ToInt32(nmValueEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastCheat)
				(lbBlastLayer.SelectedItem as BlastCheat).Value = Convert.ToInt32(nmValueEdit.Value);
			else if (lbBlastLayer.SelectedItem is BlastPipe)
				(lbBlastLayer.SelectedItem as BlastPipe).PipeAddress = Convert.ToInt64(nmValueEdit.Value);

			RefreshBlastLayer();
		}
	}
}

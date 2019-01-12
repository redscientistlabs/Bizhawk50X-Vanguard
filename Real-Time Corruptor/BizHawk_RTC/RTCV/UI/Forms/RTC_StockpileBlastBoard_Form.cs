using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
	/*
	public partial class RTC_StockpileBlastBoard_Form : Form, IAutoColorize
	{
		List<Button> allButtons = new List<Button>();
		int btnWidth = 200;
		int btnHeight = 50;
		int horizontalMargin = 12;
		int verticalMargin = 12;
		double maxColumns = 0;

		public RTC_StockpileBlastBoard_Form()
		{
			InitializeComponent();
		}

		private void RTC_StockpileBoard_Load(object sender, EventArgs e) => RefreshButtons();

		private void RTC_StockpileBoard_Shown(object sender, EventArgs e) => RefreshButtons();

		private void RTC_StockpileBoard_ResizeEnd(object sender, EventArgs e) => RefreshButtons();

		public void RefreshButtons()
		{
			if (!this.Visible)
				return;

			foreach (Button btn in allButtons)
				this.Controls.Remove(btn);

			allButtons.Clear();

			maxColumns = calculateMaxColumns();

			if (S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.Rows.Count == 0)
				lbEmptyStockpile.Visible = true;
			else
				lbEmptyStockpile.Visible = false;

			double i = 0;
			foreach (DataGridViewRow dataRow in S.GET<RTC_GlitchHarvester_Form>().dgvStockpile.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;

				int row = Convert.ToInt32(Math.Floor(i / maxColumns));
				int column = Convert.ToInt32(i % maxColumns);

				Button btn = new Button
				{
					Text = sk.ToString(),
					Tag = sk
				};
				btn.Click += new EventHandler(ButtonClicked);
				btn.Size = new Size(btnWidth, btnHeight);
				btn.Location = new Point((horizontalMargin + (column * (btn.Size.Width + horizontalMargin))), (verticalMargin + (row * (btn.Size.Height + verticalMargin))));
				btn.BackColor = Color.FromArgb(224, 224, 224);
				btn.FlatStyle = FlatStyle.Flat;
				this.Controls.Add(btn);
				allButtons.Add(btn);

				i++;
			}
		}

		public double calculateMaxColumns()
		{
			return Math.Floor(Convert.ToDouble(this.Width - horizontalMargin) / Convert.ToDouble(btnWidth + horizontalMargin));
		}

		public void ButtonClicked(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			StashKey sk = (StashKey)btn.Tag;
			RTC_Command cmd = new RTC_Command(CommandType.BLAST)
			{
				blastlayer = sk.BlastLayer
			};
			RTC_NetcoreImplementation.Multiplayer.SendCommand(cmd, false, true);
		}

		private void RTC_StockpileBoard_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void RTC_StockpileBoard_Resize(object sender, EventArgs e)
		{
			if (maxColumns != calculateMaxColumns())
				RefreshButtons();
		}
	}
	*/
}

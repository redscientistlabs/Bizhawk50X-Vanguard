using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_StockpilePlayer_Form : Form
	{
		public bool DontLoadSelectedStockpile = false;

		public RTC_StockpilePlayer_Form()
		{
			InitializeComponent();
		}

		private void RTC_BE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
				e.Cancel = true;
				this.Hide();
			}
		}

		private void btnPreviousItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnPreviousItem.Visible = false;

				if (dgvStockpile.SelectedRows.Count == 0)
					return;

				int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

				if (CurrentSelectedIndex == 0)
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[dgvStockpile.Rows.Count - 1].Selected = true;
				}
				else
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[CurrentSelectedIndex - 1].Selected = true;
				}

				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnPreviousItem.Visible = true;
			}
		}

		private void btnNextItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnNextItem.Visible = false;

				if (dgvStockpile.SelectedRows.Count == 0)
					return;

				int CurrentSelectedIndex = dgvStockpile.SelectedRows[0].Index;

				if (CurrentSelectedIndex == dgvStockpile.Rows.Count - 1)
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[0].Selected = true;
				}
				else
				{
					dgvStockpile.ClearSelection();
					dgvStockpile.Rows[CurrentSelectedIndex + 1].Selected = true;
				}

				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnNextItem.Visible = true;
			}
		}

		private void btnReloadItem_Click(object sender, EventArgs e)
		{
			try
			{
				btnReloadItem.Visible = false;
				dgvStockpile_CellClick(null, null);
			}
			finally
			{
				btnReloadItem.Visible = true;
			}
		}

		private void btnBlastToggle_Click(object sender, EventArgs e)
		{
			RTC_Core.ghForm.btnBlastToggle_Click(null, null);
		}

		private void dgvStockpile_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

				ToolStripSeparator stripSeparator = new ToolStripSeparator();
				stripSeparator.Paint += stripSeparator_Paint;

				ContextMenuStrip columnsMenu = new ContextMenuStrip();
				(columnsMenu.Items.Add("Show Item Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Item"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Item"].Visible;
				(columnsMenu.Items.Add("Show Game Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["GameName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["GameName"].Visible;
				(columnsMenu.Items.Add("Show System Name", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemName"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemName"].Visible;
				(columnsMenu.Items.Add("Show System Core", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["SystemCore"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["SystemCore"].Visible;
				(columnsMenu.Items.Add("Show Note", null, new EventHandler((ob, ev) => { dgvStockpile.Columns["Note"].Visible ^= true; })) as ToolStripMenuItem).Checked = dgvStockpile.Columns["Note"].Visible;
				columnsMenu.Items.Add(stripSeparator);
				(columnsMenu.Items.Add("Load on Select", null, new EventHandler((ob, ev) => { RTC_Core.ghForm.cbLoadOnSelect.Checked ^= true; })) as ToolStripMenuItem).Checked = RTC_Core.ghForm.cbLoadOnSelect.Checked;
				(columnsMenu.Items.Add("Clear Cheats/Freezes on Rewind", null, new EventHandler((ob, ev) => { RTC_Core.ecForm.cbClearCheatsOnRewind.Checked ^= true; })) as ToolStripMenuItem).Checked = RTC_Core.ecForm.cbClearCheatsOnRewind.Checked;
				(columnsMenu.Items.Add("Clear Pipes on Rewind", null, new EventHandler((ob, ev) => { RTC_Core.ecForm.cbClearPipesOnRewind.Checked ^= true; })) as ToolStripMenuItem).Checked = RTC_Core.ecForm.cbClearPipesOnRewind.Checked;

				columnsMenu.Show(this, locate);
			}
		}

		private void stripSeparator_Paint(object sender, PaintEventArgs e)
		{
			ToolStripSeparator stripSeparator = sender as ToolStripSeparator;
			ContextMenuStrip menuStrip = stripSeparator.Owner as ContextMenuStrip;
			e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, stripSeparator.Width, stripSeparator.Height));
			using (Pen pen = new Pen(Color.LightGray, 1))
			{
				e.Graphics.DrawLine(pen, new Point(23, stripSeparator.Height / 2), new Point(menuStrip.Width, stripSeparator.Height / 2));
			}
		}

		private void btnLoadStockpile_MouseDown(object sender, MouseEventArgs e)
		{
			Point locate = new Point((sender as Control).Location.X + e.Location.X, (sender as Control).Location.Y + e.Location.Y);

			ContextMenuStrip LoadMenuItems = new ContextMenuStrip();
			LoadMenuItems.Items.Add("Load Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					DontLoadSelectedStockpile = true;

					if (Stockpile.Load(dgvStockpile))
						RTC_Core.ghForm.dgvStockpile.Rows.Clear();
					dgvStockpile.ClearSelection();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Loading Failure ->\n\n" + ex.ToString());
				}
			}));

			LoadMenuItems.Items.Add("Load Bizhawk settings from Stockpile", null, new EventHandler((ob, ev) =>
			{
				try
				{
					Stockpile.LoadBizhawkConfigFromStockpile();
				}
				catch (Exception ex)
				{
					MessageBox.Show("Loading Settings Failure ->\n\n" + ex.ToString());
				}
			}));

			LoadMenuItems.Items.Add("Restore Bizhawk config Backup", null, new EventHandler((ob, ev) =>
			{
				try
				{
					RTC_Core.StopSound();
					Stockpile.RestoreBizhawkConfig();
				}
				finally
				{
					RTC_Core.StartSound();
				}
			})).Enabled = (File.Exists(RTC_Core.bizhawkDir + "\\backup_config.ini"));

			LoadMenuItems.Show(this, locate);
		}

		private void dgvStockpile_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				dgvStockpile.Enabled = false;

				if (e != null)
				{
					var senderGrid = (DataGridView)sender;

					StashKey sk = (StashKey)senderGrid.Rows[e.RowIndex].Cells["Item"].Value;

					if (sk.Note != null)
						tbNoteBox.Text = sk.Note.Replace("\n", Environment.NewLine);
					else
						tbNoteBox.Text = "";

					if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
						e.RowIndex >= 0)
					{
						if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm == null)
						{
							RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(sk.Note, "StockpilePlayer", sk);
						}
						else
						{
							if (RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Visible)
								RTC_NoteEditor_Form.CurrentlyOpenNoteForm.Close();

							RTC_NoteEditor_Form.CurrentlyOpenNoteForm = new RTC_NoteEditor_Form(sk.Note, "StockpilePlayer", sk);
						}

						return;
					}
				}

				if ((sender == null) && dgvStockpile.SelectedRows.Count > 0)
				{
					RTC_Core.ghForm.rbCorrupt.Checked = true;
					RTC_StockpileManager.currentStashkey = (dgvStockpile.SelectedRows[0].Cells[0].Value as StashKey);
					RTC_StockpileManager.ApplyStashkey(RTC_StockpileManager.currentStashkey);

					RTC_Core.ghForm.lbStashHistory.ClearSelected();
					RTC_Core.ghForm.dgvStockpile.ClearSelection();

					RTC_Core.ghForm.IsCorruptionApplied = !(RTC_StockpileManager.currentStashkey.BlastLayer == null || RTC_StockpileManager.currentStashkey.BlastLayer.Layer.Count == 0);
				}
			}
			finally
			{
				dgvStockpile.Enabled = true;
			}
		}

		public void RefreshNoteIcons()
		{
			foreach (DataGridViewRow dataRow in dgvStockpile.Rows)
			{
				StashKey sk = (StashKey)dataRow.Cells["Item"].Value;
				if (sk.Note == null)
				{
					dataRow.Cells["Note"].Value = "";
				}
				else
				{
					dataRow.Cells["Note"].Value = "📝";
				}
			}
		}
	}
}

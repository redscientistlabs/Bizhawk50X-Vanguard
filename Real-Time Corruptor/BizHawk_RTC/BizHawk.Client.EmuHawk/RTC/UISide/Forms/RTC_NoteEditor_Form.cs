using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_NoteEditor_Form : Form
	{
		private string note;
		private string source;
		private object item;


		public static Form CurrentlyOpenNoteForm = null;

		public RTC_NoteEditor_Form(string _note, string _source, object _item)
		{
			KeyDown += RTC_NE_Form_KeyDown;

			note = _note;
			source = _source;
			item = _item;
			InitializeComponent();
			this.Show();
		}

		private void RTC_NE_Form_Load(object sender, EventArgs e)
		{
			if (note != null)
				tbNote.Text = note.Replace("\n", Environment.NewLine);

			// Set window location
			if (RTC_Core.NoteBoxPosition != new Point(0, 0))
			{
				this.Location = RTC_Core.NoteBoxPosition;
			}
			if (RTC_Core.NoteBoxSize != new Size(0,0))
			{
				this.Size = RTC_Core.NoteBoxSize;
			}
		}


		private void RTC_NE_Form_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S)
			{
				this.Close();
			}
			if (e.KeyCode == Keys.Escape)
			{
				this.Close();
			}
		}

		private void RTC_NE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			CurrentlyOpenNoteForm = null;

			RTC_Core.NoteBoxSize = this.Size;
			RTC_Core.NoteBoxPosition = this.Location;

			string cleanText = string.Join("\n", tbNote.Lines.Select(it => it.Trim()));

			note = "";

			if (cleanText != "")
			{
				note = cleanText;
			}
			switch (source)
			{
				//We update a stashkey for the GH and Stockpile Player
				case ("GlitchHarvester"):
					if (S.GET<RTC_GlitchHarvester_Form>().Visible)
					{
						((StashKey)item).Note = note;
						S.GET<RTC_GlitchHarvester_Form>().RefreshNoteIcons();
					}
					break;
				case ("StockpilePlayer"):
					if (S.GET<RTC_StockpilePlayer_Form>().Visible)
					{
						((StashKey)item).Note = note;
						S.GET<RTC_StockpilePlayer_Form>().RefreshNoteIcons();
					}
					break;
				//We update individual blastunits for the blast editor
				case ("BlastEditor"):
					if (S.GET<RTC_BlastEditor_Form>().Visible)
					{
						((BlastUnit)item).Note = note;
						//TODO
						//S.GET<RTC_BlastEditor_Form>().RefreshNoteIcons();
					}
					break;
				//We update a DGV cell for the Blast Generator
				case ("BlastGenerator"):
					if (S.GET<RTC_BlastGenerator_Form>().Visible)
					{
						((DataGridViewCell)item).Value = note;
						S.GET<RTC_BlastGenerator_Form>().RefreshNoteIcons();
					}
						
					break;
			}
		}

		private void RTC_NE_Form_Shown(object sender, EventArgs e)
		{
			tbNote.DeselectAll();
		}
	}
}

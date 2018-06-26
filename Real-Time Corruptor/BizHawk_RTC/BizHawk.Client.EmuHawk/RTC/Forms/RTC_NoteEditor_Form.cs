using System;
using System.Data;
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
		}

		private void RTC_NE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			CurrentlyOpenNoteForm = null;

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
					if (RTC_Core.ghForm.Visible)
					{
						((StashKey)item).Note = note;
						RTC_Core.ghForm.RefreshNoteIcons();
					}
					break;
				case ("StockpilePlayer"):
					if (RTC_Core.spForm.Visible)
					{
						((StashKey)item).Note = note;
						RTC_Core.spForm.RefreshNoteIcons();
					}
					break;
				//We update individual blastunits for the blast editor
				case ("BlastEditor"):
					if (RTC_Core.beForm.Visible)
					{
						((BlastUnit)item).Note = note;
						RTC_Core.beForm.RefreshNoteIcons();
					}
					break;
				//We update a DGV cell for the Blast Generator
				case ("BlastGenerator"):
					if (RTC_Core.bgForm.Visible)
					{
						((DataGridViewCell)item).Value = note;
						RTC_Core.bgForm.RefreshNoteIcons();
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

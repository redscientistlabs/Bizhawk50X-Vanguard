using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_NoteEditor_Form : Form
	{
		string Note;
		string Source;
		object Item;
		

		public static Form currentlyOpenNoteForm = null;

		public RTC_NoteEditor_Form(string _note, string _source, object _item)
		{
			Note = _note;
			Source = _source;
			Item = _item;
			InitializeComponent();
			this.Show();
		}

		private void RTC_NE_Form_Load(object sender, EventArgs e)
		{
			if (Note != null)
				tbNote.Text = Note.Replace("\n", Environment.NewLine);
		}

		private void RTC_NE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			currentlyOpenNoteForm = null;

			string cleanText = string.Join("\n", tbNote.Lines.Select(it => it.Trim()));

			Note = "";

			if (cleanText != "")
			{
				Note = cleanText;
			}
			switch (Source)
			{
				//We update a stashkey for the GH and Stockpile Player
				case ("GlitchHarvester"):
					if (RTC_Core.ghForm.Visible)
					{
						(Item as StashKey).Note = Note;
						RTC_Core.ghForm.RefreshNoteIcons();
					}
					break;
				case ("StockpilePlayer"):
					if (RTC_Core.spForm.Visible)
					{
						(Item as StashKey).Note = Note;
						RTC_Core.spForm.RefreshNoteIcons();
					}
					break;
				//We update individual blastunits for the blast editor
				case ("BlastEditor"):
					if (RTC_Core.beForm.Visible)
					{
						(Item as BlastUnit).Note = Note;
						RTC_Core.beForm.RefreshNoteIcons();
					}
					break;
				//We update a DGV cell for the Blast Generator
				case ("BlastGenerator"):
					if (RTC_Core.bgForm.Visible)
					{
						(Item as DataGridViewCell).Value = Note;
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

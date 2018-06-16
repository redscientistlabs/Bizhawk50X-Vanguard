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
			if(Note != null)
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
				case ("GlitchHarvester"):
					(Item as StashKey).Note = Note;
					RTC_Core.ghForm.RefreshNoteIcons();
					break;
				case ("StockpilePlayer"):
					(Item as StashKey).Note = Note;
					RTC_Core.spForm.RefreshNoteIcons();
					break;
				case ("BlastEditor"):
					(Item as BlastUnit).Note = Note;
					RTC_Core.beForm.RefreshNoteIcons();
					break;
			}
		}

		private void RTC_NE_Form_Shown(object sender, EventArgs e)
		{
			tbNote.DeselectAll();
		}
	}
}

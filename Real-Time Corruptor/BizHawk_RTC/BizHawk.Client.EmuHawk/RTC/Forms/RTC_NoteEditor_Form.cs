using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_NoteEditor_Form : Form
	{
		DataGridView dgv;
		StashKey sk;

		public static Form currentlyOpenNoteForm = null;

		public RTC_NoteEditor_Form(StashKey _sk, DataGridView _dgv)
		{
			sk = _sk;
			dgv = _dgv;

			InitializeComponent();

			this.Show();
		}

		private void RTC_NE_Form_Load(object sender, EventArgs e)
		{
			if (sk.Note != null)
				tbNote.Text = sk.Note.Replace("\n", Environment.NewLine);
		}

		private void RTC_NE_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			currentlyOpenNoteForm = null;

			string cleanText = string.Join("\n", tbNote.Lines.Select(it => it.Trim()));

			sk.Note = null;

			if (cleanText != "")
			{
				sk.Note = cleanText;
			}

			RTC_Core.ghForm.RefreshNoteIcons(dgv);
		}

		private void RTC_NE_Form_Shown(object sender, EventArgs e)
		{
			tbNote.DeselectAll();
		}
	}
}

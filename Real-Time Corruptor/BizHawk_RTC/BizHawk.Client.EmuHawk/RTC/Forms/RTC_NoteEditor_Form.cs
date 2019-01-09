using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace RTC
{
	public partial class RTC_NoteEditor_Form : Form
	{
		private INote note;

		private List<DataGridViewCell> cells;


		public RTC_NoteEditor_Form(INote noteObject, DataGridViewCell _cell)
		{
			KeyDown += RTC_NE_Form_KeyDown;

			note = noteObject;
			cells = new List<DataGridViewCell>
			{
				_cell
			};
			InitializeComponent();
			this.Show();
		}

		public RTC_NoteEditor_Form(INote noteObject, List<DataGridViewCell> _cells)
		{
			KeyDown += RTC_NE_Form_KeyDown;

			note = noteObject;
			cells = _cells;
			InitializeComponent();
			this.Show();
		}


		private void RTC_NE_Form_Load(object sender, EventArgs e)
		{
			if (note.Note != null)
				tbNote.Text = note.Note.Replace("\n", Environment.NewLine);

			// Set window location
			if (RTC_UICore.NoteBoxPosition != new Point(0, 0))
			{
				this.Location = RTC_UICore.NoteBoxPosition;
			}
			if (RTC_UICore.NoteBoxSize != new Size(0,0))
			{
				this.Size = RTC_UICore.NoteBoxSize;
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
			RTC_UICore.NoteBoxSize = this.Size;
			RTC_UICore.NoteBoxPosition = this.Location;

			string cleanText = string.Join("\n", tbNote.Lines.Select(it => it.Trim()));

			if(cleanText == "[DIFFERENT]")
				return;

			if (String.IsNullOrEmpty(cleanText))
			{
				note.Note = String.Empty;
				if (cells != null)
					foreach (DataGridViewCell cell in cells)
						cell.Value = String.Empty;
			}
			else
			{
				note.Note = cleanText;
				if(cells != null)
					foreach (DataGridViewCell cell in cells)
						cell.Value = "📝";
			}

		}

		private void RTC_NE_Form_Shown(object sender, EventArgs e)
		{
			tbNote.DeselectAll();
		}
	}
}

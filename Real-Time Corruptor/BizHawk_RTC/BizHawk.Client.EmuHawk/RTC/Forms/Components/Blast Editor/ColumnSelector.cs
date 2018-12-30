using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTC
{
	public partial class ColumnSelector : Form, IAutoColorize
	{
		public ColumnSelector()
		{
			InitializeComponent();
			RTC_Core.SetRTCColor(RTC_Core.GeneralColor, this);
			this.FormClosing += this.ColumnSelector_Closing;
		}

		public void LoadColumnSelector(DataGridViewColumnCollection columns)
		{
			foreach(DataGridViewColumn column in columns)
			{
				CheckBox cb = new CheckBox();
				cb.Text = column.HeaderText;
				cb.Name = column.Name;
				cb.Checked = column.Visible;
				tablePanel.Controls.Add(cb);
			}
			this.Show();
		}
		private void ColumnSelector_Closing(object sender, FormClosingEventArgs e)
		{
			if (S.GET<RTC_NewBlastEditor_Form>() != null)
			{
				List<String> temp = new List<String>();
				foreach (CheckBox cb in tablePanel.Controls.Cast<CheckBox>().Where(item => item.Checked))
				{
					temp.Add(cb.Name);
				}
				S.GET<RTC_NewBlastEditor_Form>().VisibleColumns = temp;
				S.GET<RTC_NewBlastEditor_Form>().RefreshVisibleColumns();
			}
		}
	}
}

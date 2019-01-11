using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CorruptCore;

namespace UI
{
	public partial class ColumnSelector : Form, UI_Extensions.IAutoColorize
	{
		public ColumnSelector()
		{
			InitializeComponent();
			RTC_UICore.SetRTCColor(RTC_UICore.GeneralColor, this);
			this.FormClosing += this.ColumnSelector_Closing;
		}

		public void LoadColumnSelector(DataGridViewColumnCollection columns)
		{
			foreach(DataGridViewColumn column in columns)
			{
				CheckBox cb = new CheckBox
				{
					AutoSize = true,
					Text = column.HeaderText,
					Name = column.Name,
					Checked = column.Visible
				};
				tablePanel.Controls.Add(cb);
			}
			this.Show();
		}
		private void ColumnSelector_Closing(object sender, FormClosingEventArgs e)
		{
				List<String> temp = new List<String>();
				StringBuilder sb = new StringBuilder();
				foreach (CheckBox cb in tablePanel.Controls.Cast<CheckBox>().Where(item => item.Checked))
				{
					temp.Add(cb.Name);

					sb.Append(cb.Name);
					sb.Append(",");
				}
			if (UI_Extensions.S.GET<RTC_NewBlastEditor_Form>() != null)
			{
				UI_Extensions.S.GET<RTC_NewBlastEditor_Form>().VisibleColumns = temp;
				UI_Extensions.S.GET<RTC_NewBlastEditor_Form>().RefreshVisibleColumns();
			}
			RTC_Params.SetParam("BLASTEDITOR_VISIBLECOLUMNS", sb.ToString());

		}
	}
}

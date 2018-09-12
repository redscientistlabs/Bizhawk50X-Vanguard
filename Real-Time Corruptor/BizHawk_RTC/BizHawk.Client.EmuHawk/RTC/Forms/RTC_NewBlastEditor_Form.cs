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
	public partial class RTC_NewBlastEditor_Form : Form, IAutoColorize
	{
		public RTC_NewBlastEditor_Form()
		{
			InitializeComponent();
			InitializeDGV();
		}

		private void InitializeDGV()
		{
			dgvBlastEditor.AutoGenerateColumns = false;


			//We can't bind to Byte Arrays directly 
			DataGridViewColumn column = new DataGridViewNumericUpDownColumn();
			column.DataPropertyName = "Value";

			dgvBlastEditor.Columns.Add(column);

		}

		StashKey currentSK = null;
		BindingSource bs = null;

		public void LoadStashkey(StashKey sk)
		{
			currentSK = sk;

			bs = new BindingSource();
			bs.DataSource = sk.BlastLayer.Layer;

			dgvBlastEditor.DataSource = bs;
			

			this.Show();
		}

	}
}

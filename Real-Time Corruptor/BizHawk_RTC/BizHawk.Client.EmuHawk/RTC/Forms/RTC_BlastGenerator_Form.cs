using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace RTC
{
	public partial class RTC_BlastGenerator_Form : Form
	{
		private StashKey sk = null;


		public RTC_BlastGenerator_Form()
		{
			InitializeComponent();
		}

		private void RTC_BlastGeneratorForm_Load(object sender, EventArgs e)
		{
		}

		public void LoadStashkey(StashKey _sk)
		{
			if (_sk == null)
				return;

			sk = (StashKey)_sk.Clone();

			this.Show();
		}


		private void btnJustCorrupt_Click(object sender, EventArgs e)
		{

		}

		private void btnSendBlastLayerToEditor_Click(object sender, EventArgs e)
		{

		}

		private void btnLoadCorrupt_Click(object sender, EventArgs e)
		{

		}

		private void cbUseHex_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void btnShiftBlastLayerDown_Click(object sender, EventArgs e)
		{

		}
	}
}

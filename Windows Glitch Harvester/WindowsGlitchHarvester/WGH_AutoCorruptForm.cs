using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsGlitchHarvester
{
	public partial class WGH_AutoCorruptForm : Form
	{
		public Timer AutoCorruptTimer = new Timer();

		public WGH_AutoCorruptForm()
		{
			InitializeComponent();
		}

		private bool _AutoCorrupt = false;
		public bool AutoCorrupt {
			get
			{
				return _AutoCorrupt;
			}
			set
			{
				_AutoCorrupt = value;

				if (AutoCorruptTimer != null)
					AutoCorruptTimer.Stop();

				if (value)
				{
					AutoCorruptTimer = new Timer();
					AutoCorruptTimer.Interval = WGH_Core.ErrorDelay;
					AutoCorruptTimer.Tick += new EventHandler((ov,ev) => { WGH_Core.ghForm.BlastTarget(1, false, false); });
					AutoCorruptTimer.Start();
				}

			}
		}


		private void nmAutoCorruptDelay_ValueChanged(object sender, EventArgs e)
		{
			WGH_Core.ErrorDelay = (int)nmAutoCorruptDelay.Value;

		}

		private void btnStartAutoCorrupt_Click(object sender, EventArgs e)
		{
			if(btnStartAutoCorrupt.Text == "Start Auto-Corrupt")
			{
				btnStartAutoCorrupt.Text = "Stop Auto-Corrupt";
				WGH_Core.acForm.AutoCorrupt = true;
			}
			else
			{
				btnStartAutoCorrupt.Text = "Start Auto-Corrupt";
				WGH_Core.acForm.AutoCorrupt = false;
			}
		}
	}
}

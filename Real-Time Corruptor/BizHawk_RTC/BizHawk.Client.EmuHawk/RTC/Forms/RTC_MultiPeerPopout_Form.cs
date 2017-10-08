using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RTC
{
	public partial class RTC_MultiPeerPopout_Form : Form
	{

		Size BizhawkMainformLastSize;
		Point BizhawkMainformLastLocation;
        public bool BizhawkMainformDispFixScaleInteger;
        public bool BizhawkMainformDispFixAspectRatio;

        public RTC_MultiPeerPopout_Form()
		{
			InitializeComponent();
        }

		private void RTC_MultiPeerPopout_Form_Load(object sender, EventArgs e)
		{

		}

		private void RTC_MultiPeerPopout_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.FormOwnerClosing)
			{
                if (RTC_Core.multiForm.btnSplitscreen.ForeColor == Color.Red)
                    RTC_Core.multiForm.btnSplitscreen_Click(sender, e);

                RTC_Core.multiForm.btnPopoutPeerGameScreen.Visible = true;
                RTC_Core.multiForm.pnPeerRedBar.Visible = true;
                RTC_Core.multiForm.pbPeerScreen.Visible = true;

                e.Cancel = true;
				this.Hide();
			}

		}

		private void RTC_MultiPeerPopout_Form_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			SwitchWindowType();
		}

		public void pbPeerScreen_DoubleClick(object sender, EventArgs e)
		{
			SwitchWindowType();
		}

		private void pnPeerRedBar_DoubleClick(object sender, EventArgs e)
		{
			SwitchWindowType();
		}

		public void SetSplitscreen(bool state)
		{
			if(state)
			{
				this.Size = new Size(554, 297);
				this.pbPeerScreen.Size = new Size(256, 224);

				BizhawkMainformLastSize = GlobalWin.MainForm.Size;
				BizhawkMainformLastLocation = GlobalWin.MainForm.Location;
                BizhawkMainformDispFixAspectRatio = Global.Config.DispFixAspectRatio;
                BizhawkMainformDispFixScaleInteger = Global.Config.DispFixScaleInteger;

                GlobalWin.MainForm.Hide();
				GlobalWin.MainForm.MainformMenu.Visible = false;
				GlobalWin.MainForm.MainStatusBar.Visible = false;
				GlobalWin.MainForm.FormBorderStyle = FormBorderStyle.None;

				RTC_Hooks.BIZHAWK_ALLOWED_DOUBLECLICK_FULLSCREEN = false;

				Global.Config.DispFixAspectRatio = false;
				Global.Config.DispFixScaleInteger = true;

				GlobalWin.MainForm.TopLevel = false;
				this.Controls.Add(GlobalWin.MainForm);
				GlobalWin.MainForm.Location = this.pnPlacer.Location;
				GlobalWin.MainForm.Size = this.pnPlacer.Size;
				//Top, Bottom, Left, Right
				GlobalWin.MainForm.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
				GlobalWin.MainForm.Show();

				this.RTC_MultiPeerPopout_Form_Resize(null, null);
				this.pbPeerScreen.Location = new Point(12 + 256, 19);

			}
			else
			{

				GlobalWin.MainForm.FormBorderStyle = FormBorderStyle.Sizable;

				this.Controls.Remove(GlobalWin.MainForm);
				GlobalWin.MainForm.Anchor = ((AnchorStyles)AnchorStyles.Top | AnchorStyles.Left);
				GlobalWin.MainForm.TopLevel = true;
				

				GlobalWin.MainForm.Size = BizhawkMainformLastSize;
				GlobalWin.MainForm.Location = BizhawkMainformLastLocation;
				GlobalWin.MainForm.MainformMenu.Visible = true;
				GlobalWin.MainForm.MainStatusBar.Visible = true;

				GlobalWin.MainForm.Show();

                Global.Config.DispFixAspectRatio = BizhawkMainformDispFixAspectRatio;
                Global.Config.DispFixScaleInteger = BizhawkMainformDispFixScaleInteger;

                this.pbPeerScreen.Size = new Size(512, 448);
				RTC_Hooks.BIZHAWK_ALLOWED_DOUBLECLICK_FULLSCREEN = true;

				this.pbPeerScreen.Location = new Point(12, 19);
				this.Size = new Size(550, 520);
				this.pbPeerScreen.Size = new Size(512, 448);
			}
		}

		public void SwitchWindowType()
		{
			if (this.FormBorderStyle == FormBorderStyle.None)
			{
				this.FormBorderStyle = FormBorderStyle.Sizable;
				this.TopMost = false;
			}
			else
			{
				this.FormBorderStyle = FormBorderStyle.None;
				this.TopMost = true;
			}
		}

		public void RTC_MultiPeerPopout_Form_Resize(object sender, EventArgs e)
		{
			if (this.FormBorderStyle == FormBorderStyle.None)
				return;

			bool dualScreens = this.Controls.Contains(GlobalWin.MainForm);

			double Width = pnPlacer.Size.Width / (dualScreens ? 2f : 1f);
			double ratio = 224f / 256f;
			double Height = Width * ratio;

			int FullWidth = pnPlacer.Size.Width;
			int FullHeight = pnPlacer.Size.Height;

			if (dualScreens)
			{
				Size newsize = new Size(Convert.ToInt32(Width), Convert.ToInt32(Height));
				pbPeerScreen.Size = new Size(Convert.ToInt32(Width), Convert.ToInt32(Height));
				GlobalWin.MainForm.Size = newsize;

				GlobalWin.MainForm.Location = new Point(10, 20);
				pbPeerScreen.Location = new Point(10 + Convert.ToInt32(Width), 20);

			}

			this.Size = new Size(FullWidth + 38, Convert.ToInt32(Height) + 72);


		}
	}
}

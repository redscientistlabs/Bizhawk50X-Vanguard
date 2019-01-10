namespace RTC
{
	partial class RTC_MultiPeerPopout_Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnPeerRedBar = new System.Windows.Forms.Panel();
			this.pbPeerScreen = new System.Windows.Forms.PictureBox();
			this.pnPlacer = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.pbPeerScreen)).BeginInit();
			this.SuspendLayout();
			// 
			// pnPeerRedBar
			// 
			this.pnPeerRedBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnPeerRedBar.BackColor = System.Drawing.Color.Red;
			this.pnPeerRedBar.Location = new System.Drawing.Point(12, 8);
			this.pnPeerRedBar.Name = "pnPeerRedBar";
			this.pnPeerRedBar.Size = new System.Drawing.Size(508, 5);
			this.pnPeerRedBar.TabIndex = 145;
			this.pnPeerRedBar.DoubleClick += new System.EventHandler(this.pnPeerRedBar_DoubleClick);
			// 
			// pbPeerScreen
			// 
			this.pbPeerScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbPeerScreen.BackColor = System.Drawing.Color.Black;
			this.pbPeerScreen.Location = new System.Drawing.Point(10, 20);
			this.pbPeerScreen.Name = "pbPeerScreen";
			this.pbPeerScreen.Size = new System.Drawing.Size(512, 448);
			this.pbPeerScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pbPeerScreen.TabIndex = 19;
			this.pbPeerScreen.TabStop = false;
			this.pbPeerScreen.DoubleClick += new System.EventHandler(this.pbPeerScreen_DoubleClick);
			// 
			// pnPlacer
			// 
			this.pnPlacer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnPlacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
			this.pnPlacer.Location = new System.Drawing.Point(10, 19);
			this.pnPlacer.Name = "pnPlacer";
			this.pnPlacer.Size = new System.Drawing.Size(512, 448);
			this.pnPlacer.TabIndex = 146;
			this.pnPlacer.Visible = false;
			// 
			// RTC_MultiPeerPopout_Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gray;
			this.ClientSize = new System.Drawing.Size(534, 481);
			this.Controls.Add(this.pnPlacer);
			this.Controls.Add(this.pnPeerRedBar);
			this.Controls.Add(this.pbPeerScreen);
			this.Name = "RTC_MultiPeerPopout_Form";
			this.Tag = "color:normal";
			this.Text = "RTC Multiplayer Screen";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_MultiPeerPopout_Form_FormClosing);
			this.Load += new System.EventHandler(this.RTC_MultiPeerPopout_Form_Load);
			this.ResizeEnd += new System.EventHandler(this.RTC_MultiPeerPopout_Form_Resize);
			this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RTC_MultiPeerPopout_Form_MouseDoubleClick);
			this.Resize += new System.EventHandler(this.RTC_MultiPeerPopout_Form_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pbPeerScreen)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.Panel pnPeerRedBar;
		public System.Windows.Forms.PictureBox pbPeerScreen;
		public System.Windows.Forms.Panel pnPlacer;
	}
}
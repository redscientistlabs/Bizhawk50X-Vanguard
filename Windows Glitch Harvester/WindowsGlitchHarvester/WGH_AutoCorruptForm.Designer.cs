namespace WindowsGlitchHarvester
{
	partial class WGH_AutoCorruptForm
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
			this.btnStartAutoCorrupt = new System.Windows.Forms.Button();
			this.nmAutoCorruptDelay = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pbRTC = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.nmAutoCorruptDelay)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRTC)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStartAutoCorrupt
			// 
			this.btnStartAutoCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartAutoCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.btnStartAutoCorrupt.FlatAppearance.BorderSize = 0;
			this.btnStartAutoCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStartAutoCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
			this.btnStartAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
			this.btnStartAutoCorrupt.Location = new System.Drawing.Point(6, 9);
			this.btnStartAutoCorrupt.Name = "btnStartAutoCorrupt";
			this.btnStartAutoCorrupt.Size = new System.Drawing.Size(312, 28);
			this.btnStartAutoCorrupt.TabIndex = 1;
			this.btnStartAutoCorrupt.TabStop = false;
			this.btnStartAutoCorrupt.Tag = "color:darker";
			this.btnStartAutoCorrupt.Text = "Start Auto-Corrupt";
			this.btnStartAutoCorrupt.UseVisualStyleBackColor = false;
			this.btnStartAutoCorrupt.Click += new System.EventHandler(this.btnStartAutoCorrupt_Click);
			// 
			// nmAutoCorruptDelay
			// 
			this.nmAutoCorruptDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nmAutoCorruptDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.nmAutoCorruptDelay.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.nmAutoCorruptDelay.ForeColor = System.Drawing.Color.White;
			this.nmAutoCorruptDelay.Location = new System.Drawing.Point(145, 43);
			this.nmAutoCorruptDelay.Name = "nmAutoCorruptDelay";
			this.nmAutoCorruptDelay.Size = new System.Drawing.Size(86, 25);
			this.nmAutoCorruptDelay.TabIndex = 20;
			this.nmAutoCorruptDelay.TabStop = false;
			this.nmAutoCorruptDelay.Tag = "color:dark";
			this.nmAutoCorruptDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nmAutoCorruptDelay.ValueChanged += new System.EventHandler(this.nmAutoCorruptDelay_ValueChanged);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(3, 45);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(138, 17);
			this.label5.TabIndex = 21;
			this.label5.Text = "Corrupt process every";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(239, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 17);
			this.label1.TabIndex = 22;
			this.label1.Text = "milliseconds";
			// 
			// pbRTC
			// 
			this.pbRTC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.pbRTC.Image = global::WindowsGlitchHarvester.Properties.Resources.logortc;
			this.pbRTC.Location = new System.Drawing.Point(337, 9);
			this.pbRTC.Name = "pbRTC";
			this.pbRTC.Size = new System.Drawing.Size(66, 69);
			this.pbRTC.TabIndex = 23;
			this.pbRTC.TabStop = false;
			// 
			// WGH_AutoCorruptForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gray;
			this.ClientSize = new System.Drawing.Size(641, 80);
			this.Controls.Add(this.pbRTC);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nmAutoCorruptDelay);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.btnStartAutoCorrupt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "WGH_AutoCorruptForm";
			this.Tag = "color:normal";
			this.Text = "WGH_AutoCorruptForm";
			((System.ComponentModel.ISupportInitialize)(this.nmAutoCorruptDelay)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbRTC)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStartAutoCorrupt;
		private System.Windows.Forms.NumericUpDown nmAutoCorruptDelay;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbRTC;
	}
}
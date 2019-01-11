namespace RTCV.UI
{
	partial class RTC_ConnectionStatus_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_ConnectionStatus_Form));
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.btnStartEmuhawkDetached = new System.Windows.Forms.Button();
            this.pnCorruptionEngine = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnBizhawkAttached = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStartEmuhawkAttached = new System.Windows.Forms.Button();
            this.lbBizhawkEmulatorAttached = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbBizhawkAttached = new System.Windows.Forms.Label();
            this.lbWindowsGlitchHarvester = new System.Windows.Forms.Label();
            this.pnWindowsGlitchHarvester = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStartWGH = new System.Windows.Forms.Button();
            this.lbVrun = new System.Windows.Forms.Label();
            this.pnVrun = new System.Windows.Forms.Panel();
            this.btnStartVrun = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lbVrunAddress = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbRTCver = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnCorruptionEngine.SuspendLayout();
            this.pnBizhawkAttached.SuspendLayout();
            this.pnWindowsGlitchHarvester.SuspendLayout();
            this.pnVrun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lbConnectionStatus.Location = new System.Drawing.Point(65, 74);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(247, 19);
            this.lbConnectionStatus.TabIndex = 1;
            this.lbConnectionStatus.Text = "Connection status: Waiting for Bizhawk";
            // 
            // btnStartEmuhawkDetached
            // 
            this.btnStartEmuhawkDetached.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartEmuhawkDetached.FlatAppearance.BorderSize = 0;
            this.btnStartEmuhawkDetached.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartEmuhawkDetached.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartEmuhawkDetached.ForeColor = System.Drawing.Color.Black;
            this.btnStartEmuhawkDetached.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartEmuhawkDetached.Location = new System.Drawing.Point(24, 18);
            this.btnStartEmuhawkDetached.Name = "btnStartEmuhawkDetached";
            this.btnStartEmuhawkDetached.Size = new System.Drawing.Size(184, 49);
            this.btnStartEmuhawkDetached.TabIndex = 2;
            this.btnStartEmuhawkDetached.Text = "  Start BizHawk";
            this.btnStartEmuhawkDetached.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartEmuhawkDetached.UseVisualStyleBackColor = false;
            this.btnStartEmuhawkDetached.Click += new System.EventHandler(this.btnStartEmuhawkDetached_Click);
            // 
            // pnCorruptionEngine
            // 
            this.pnCorruptionEngine.BackColor = System.Drawing.Color.Transparent;
            this.pnCorruptionEngine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnCorruptionEngine.Controls.Add(this.label2);
            this.pnCorruptionEngine.Controls.Add(this.btnStartEmuhawkDetached);
            this.pnCorruptionEngine.Location = new System.Drawing.Point(22, 181);
            this.pnCorruptionEngine.Name = "pnCorruptionEngine";
            this.pnCorruptionEngine.Size = new System.Drawing.Size(232, 115);
            this.pnCorruptionEngine.TabIndex = 114;
            this.pnCorruptionEngine.Tag = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(34, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 30);
            this.label2.TabIndex = 131;
            this.label2.Text = "Detached Mode protects your\r\nsession if Bizhawk crashes.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(15, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 21);
            this.label11.TabIndex = 116;
            this.label11.Text = "BizHawk Emulator";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(31, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 32);
            this.panel1.TabIndex = 117;
            // 
            // pnBizhawkAttached
            // 
            this.pnBizhawkAttached.BackColor = System.Drawing.Color.Transparent;
            this.pnBizhawkAttached.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBizhawkAttached.Controls.Add(this.label1);
            this.pnBizhawkAttached.Controls.Add(this.label3);
            this.pnBizhawkAttached.Controls.Add(this.btnStartEmuhawkAttached);
            this.pnBizhawkAttached.Location = new System.Drawing.Point(22, 331);
            this.pnBizhawkAttached.Name = "pnBizhawkAttached";
            this.pnBizhawkAttached.Size = new System.Drawing.Size(232, 159);
            this.pnBizhawkAttached.TabIndex = 120;
            this.pnBizhawkAttached.Tag = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 30);
            this.label1.TabIndex = 130;
            this.label1.Text = "Only use attached mode if you have\r\na good reason to not use detached.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(11, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(205, 15);
            this.label3.TabIndex = 129;
            this.label3.Text = "* Multiplayer requires Attached Mode";
            // 
            // btnStartEmuhawkAttached
            // 
            this.btnStartEmuhawkAttached.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartEmuhawkAttached.FlatAppearance.BorderSize = 0;
            this.btnStartEmuhawkAttached.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartEmuhawkAttached.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartEmuhawkAttached.ForeColor = System.Drawing.Color.Black;
            this.btnStartEmuhawkAttached.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartEmuhawkAttached.Location = new System.Drawing.Point(24, 32);
            this.btnStartEmuhawkAttached.Name = "btnStartEmuhawkAttached";
            this.btnStartEmuhawkAttached.Size = new System.Drawing.Size(184, 29);
            this.btnStartEmuhawkAttached.TabIndex = 19;
            this.btnStartEmuhawkAttached.Text = "  Start BizHawk";
            this.btnStartEmuhawkAttached.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartEmuhawkAttached.UseVisualStyleBackColor = false;
            this.btnStartEmuhawkAttached.Click += new System.EventHandler(this.btnStartEmuhawkAttached_Click);
            // 
            // lbBizhawkEmulatorAttached
            // 
            this.lbBizhawkEmulatorAttached.AutoSize = true;
            this.lbBizhawkEmulatorAttached.BackColor = System.Drawing.Color.Transparent;
            this.lbBizhawkEmulatorAttached.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbBizhawkEmulatorAttached.ForeColor = System.Drawing.Color.White;
            this.lbBizhawkEmulatorAttached.Location = new System.Drawing.Point(18, 309);
            this.lbBizhawkEmulatorAttached.Name = "lbBizhawkEmulatorAttached";
            this.lbBizhawkEmulatorAttached.Size = new System.Drawing.Size(137, 21);
            this.lbBizhawkEmulatorAttached.TabIndex = 121;
            this.lbBizhawkEmulatorAttached.Text = "Bizhawk Emulator";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(161, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 15);
            this.label4.TabIndex = 122;
            this.label4.Text = "Detached Mode";
            // 
            // lbBizhawkAttached
            // 
            this.lbBizhawkAttached.AutoSize = true;
            this.lbBizhawkAttached.BackColor = System.Drawing.Color.Transparent;
            this.lbBizhawkAttached.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lbBizhawkAttached.ForeColor = System.Drawing.Color.White;
            this.lbBizhawkAttached.Location = new System.Drawing.Point(167, 314);
            this.lbBizhawkAttached.Name = "lbBizhawkAttached";
            this.lbBizhawkAttached.Size = new System.Drawing.Size(93, 15);
            this.lbBizhawkAttached.TabIndex = 123;
            this.lbBizhawkAttached.Text = "Attached Mode";
            // 
            // lbWindowsGlitchHarvester
            // 
            this.lbWindowsGlitchHarvester.AutoSize = true;
            this.lbWindowsGlitchHarvester.BackColor = System.Drawing.Color.Transparent;
            this.lbWindowsGlitchHarvester.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbWindowsGlitchHarvester.ForeColor = System.Drawing.Color.White;
            this.lbWindowsGlitchHarvester.Location = new System.Drawing.Point(398, 158);
            this.lbWindowsGlitchHarvester.Name = "lbWindowsGlitchHarvester";
            this.lbWindowsGlitchHarvester.Size = new System.Drawing.Size(199, 21);
            this.lbWindowsGlitchHarvester.TabIndex = 125;
            this.lbWindowsGlitchHarvester.Text = "Windows Glitch Harvester";
            this.lbWindowsGlitchHarvester.Visible = false;
            // 
            // pnWindowsGlitchHarvester
            // 
            this.pnWindowsGlitchHarvester.BackColor = System.Drawing.Color.Transparent;
            this.pnWindowsGlitchHarvester.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnWindowsGlitchHarvester.Controls.Add(this.label8);
            this.pnWindowsGlitchHarvester.Controls.Add(this.btnStartWGH);
            this.pnWindowsGlitchHarvester.Location = new System.Drawing.Point(401, 181);
            this.pnWindowsGlitchHarvester.Name = "pnWindowsGlitchHarvester";
            this.pnWindowsGlitchHarvester.Size = new System.Drawing.Size(232, 115);
            this.pnWindowsGlitchHarvester.TabIndex = 124;
            this.pnWindowsGlitchHarvester.Tag = "";
            this.pnWindowsGlitchHarvester.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(18, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(197, 39);
            this.label8.TabIndex = 120;
            this.label8.Text = "This program is a standalone version \nof RTC\'s Glitch Harvester which \ncan corrup" +
    "t Windows Files/Processes";
            // 
            // btnStartWGH
            // 
            this.btnStartWGH.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartWGH.FlatAppearance.BorderSize = 0;
            this.btnStartWGH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartWGH.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartWGH.ForeColor = System.Drawing.Color.Black;
            this.btnStartWGH.Image = ((System.Drawing.Image)(resources.GetObject("btnStartWGH.Image")));
            this.btnStartWGH.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartWGH.Location = new System.Drawing.Point(22, 65);
            this.btnStartWGH.Name = "btnStartWGH";
            this.btnStartWGH.Size = new System.Drawing.Size(185, 29);
            this.btnStartWGH.TabIndex = 19;
            this.btnStartWGH.Text = "  Start WGH";
            this.btnStartWGH.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartWGH.UseVisualStyleBackColor = false;
            this.btnStartWGH.Click += new System.EventHandler(this.btnStartWGH_Click);
            // 
            // lbVrun
            // 
            this.lbVrun.AutoSize = true;
            this.lbVrun.BackColor = System.Drawing.Color.Transparent;
            this.lbVrun.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbVrun.ForeColor = System.Drawing.Color.White;
            this.lbVrun.Location = new System.Drawing.Point(398, 309);
            this.lbVrun.Name = "lbVrun";
            this.lbVrun.Size = new System.Drawing.Size(99, 21);
            this.lbVrun.TabIndex = 127;
            this.lbVrun.Text = "VRUN Game";
            // 
            // pnVrun
            // 
            this.pnVrun.BackColor = System.Drawing.Color.Transparent;
            this.pnVrun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnVrun.Controls.Add(this.btnStartVrun);
            this.pnVrun.Controls.Add(this.label9);
            this.pnVrun.Location = new System.Drawing.Point(401, 331);
            this.pnVrun.Name = "pnVrun";
            this.pnVrun.Size = new System.Drawing.Size(232, 159);
            this.pnVrun.TabIndex = 126;
            this.pnVrun.Tag = "";
            // 
            // btnStartVrun
            // 
            this.btnStartVrun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartVrun.FlatAppearance.BorderSize = 0;
            this.btnStartVrun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartVrun.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartVrun.ForeColor = System.Drawing.Color.Black;
            this.btnStartVrun.Image = ((System.Drawing.Image)(resources.GetObject("btnStartVrun.Image")));
            this.btnStartVrun.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartVrun.Location = new System.Drawing.Point(22, 64);
            this.btnStartVrun.Name = "btnStartVrun";
            this.btnStartVrun.Size = new System.Drawing.Size(185, 29);
            this.btnStartVrun.TabIndex = 121;
            this.btnStartVrun.Text = "  Play VRUN";
            this.btnStartVrun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartVrun.UseVisualStyleBackColor = false;
            this.btnStartVrun.Click += new System.EventHandler(this.btnStartVrun_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(19, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(179, 39);
            this.label9.TabIndex = 121;
            this.label9.Text = "Corrupt everything in this online\nrandomized platforming shooter. \nRuns best in G" +
    "oogle Chrome.";
            // 
            // lbVrunAddress
            // 
            this.lbVrunAddress.AutoSize = true;
            this.lbVrunAddress.BackColor = System.Drawing.Color.Transparent;
            this.lbVrunAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbVrunAddress.ForeColor = System.Drawing.Color.White;
            this.lbVrunAddress.Location = new System.Drawing.Point(546, 313);
            this.lbVrunAddress.Name = "lbVrunAddress";
            this.lbVrunAddress.Size = new System.Drawing.Size(93, 15);
            this.lbVrunAddress.TabIndex = 128;
            this.lbVrunAddress.Text = "http://virus.run/";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(596, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 129;
            this.label6.Text = "(Alpha)";
            // 
            // lbRTCver
            // 
            this.lbRTCver.AutoSize = true;
            this.lbRTCver.BackColor = System.Drawing.Color.Transparent;
            this.lbRTCver.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lbRTCver.ForeColor = System.Drawing.Color.White;
            this.lbRTCver.Location = new System.Drawing.Point(491, 32);
            this.lbRTCver.Name = "lbRTCver";
            this.lbRTCver.Size = new System.Drawing.Size(0, 37);
            this.lbRTCver.TabIndex = 131;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(19, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 50);
            this.pictureBox1.TabIndex = 132;
            this.pictureBox1.TabStop = false;
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lbRTCver);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbVrunAddress);
            this.Controls.Add(this.lbVrun);
            this.Controls.Add(this.pnVrun);
            this.Controls.Add(this.lbWindowsGlitchHarvester);
            this.Controls.Add(this.pnWindowsGlitchHarvester);
            this.Controls.Add(this.lbBizhawkAttached);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbBizhawkEmulatorAttached);
            this.Controls.Add(this.pnBizhawkAttached);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pnCorruptionEngine);
            this.Controls.Add(this.lbConnectionStatus);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ConnectionStatus_Form";
            this.Text = "RTC_ConnectionStatus_Form";
            this.Load += new System.EventHandler(this.RTC_ConnectionStatus_Form_Load);
            this.pnCorruptionEngine.ResumeLayout(false);
            this.pnCorruptionEngine.PerformLayout();
            this.pnBizhawkAttached.ResumeLayout(false);
            this.pnBizhawkAttached.PerformLayout();
            this.pnWindowsGlitchHarvester.ResumeLayout(false);
            this.pnWindowsGlitchHarvester.PerformLayout();
            this.pnVrun.ResumeLayout(false);
            this.pnVrun.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.Label lbConnectionStatus;
		public System.Windows.Forms.Button btnStartEmuhawkDetached;
		private System.Windows.Forms.Panel pnCorruptionEngine;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.Panel pnBizhawkAttached;
		public System.Windows.Forms.Button btnStartEmuhawkAttached;
		private System.Windows.Forms.Label lbBizhawkEmulatorAttached;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbBizhawkAttached;
		private System.Windows.Forms.Label lbWindowsGlitchHarvester;
		public System.Windows.Forms.Panel pnWindowsGlitchHarvester;
		private System.Windows.Forms.Label label8;
		public System.Windows.Forms.Button btnStartWGH;
		private System.Windows.Forms.Label lbVrun;
		public System.Windows.Forms.Panel pnVrun;
		private System.Windows.Forms.Label label9;
		public System.Windows.Forms.Button btnStartVrun;
		private System.Windows.Forms.Label lbVrunAddress;
		private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbRTCver;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}
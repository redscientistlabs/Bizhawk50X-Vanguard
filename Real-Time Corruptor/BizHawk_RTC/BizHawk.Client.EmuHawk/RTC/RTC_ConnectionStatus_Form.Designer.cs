namespace RTC
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
            this.lbRTC = new System.Windows.Forms.Label();
            this.lbConnectionStatus = new System.Windows.Forms.Label();
            this.btnStartEmuhawkDetached = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCrashSoundEffect = new System.Windows.Forms.ComboBox();
            this.pbTimeout = new System.Windows.Forms.ProgressBar();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.pnCorruptionEngine = new System.Windows.Forms.Panel();
            this.cbNetCoreCommandTimeout = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReturnToSession = new System.Windows.Forms.Button();
            this.pnDisableGameProtection = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStopGameProtection = new System.Windows.Forms.Button();
            this.pnBizhawkAttached = new System.Windows.Forms.Panel();
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnStartVrun = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.lbVrunAddress = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnCorruptionEngine.SuspendLayout();
            this.pnDisableGameProtection.SuspendLayout();
            this.pnBizhawkAttached.SuspendLayout();
            this.pnWindowsGlitchHarvester.SuspendLayout();
            this.pnVrun.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbRTC
            // 
            this.lbRTC.AutoSize = true;
            this.lbRTC.Font = new System.Drawing.Font("Segoe UI Semibold", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRTC.Location = new System.Drawing.Point(19, 12);
            this.lbRTC.Name = "lbRTC";
            this.lbRTC.Size = new System.Drawing.Size(477, 65);
            this.lbRTC.TabIndex = 0;
            this.lbRTC.Text = "Real-Time Corruptor";
            // 
            // lbConnectionStatus
            // 
            this.lbConnectionStatus.AutoSize = true;
            this.lbConnectionStatus.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbConnectionStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lbConnectionStatus.Location = new System.Drawing.Point(77, 85);
            this.lbConnectionStatus.Name = "lbConnectionStatus";
            this.lbConnectionStatus.Size = new System.Drawing.Size(279, 21);
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
            this.btnStartEmuhawkDetached.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.CorpHawkSmall;
            this.btnStartEmuhawkDetached.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartEmuhawkDetached.Location = new System.Drawing.Point(22, 14);
            this.btnStartEmuhawkDetached.Name = "btnStartEmuhawkDetached";
            this.btnStartEmuhawkDetached.Size = new System.Drawing.Size(163, 29);
            this.btnStartEmuhawkDetached.TabIndex = 2;
            this.btnStartEmuhawkDetached.Text = "  Start BizHawk";
            this.btnStartEmuhawkDetached.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartEmuhawkDetached.UseVisualStyleBackColor = false;
            this.btnStartEmuhawkDetached.Click += new System.EventHandler(this.btnStartEmuhawkDetached_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(19, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Crash sound effect:";
            // 
            // cbCrashSoundEffect
            // 
            this.cbCrashSoundEffect.BackColor = System.Drawing.Color.Black;
            this.cbCrashSoundEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCrashSoundEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCrashSoundEffect.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbCrashSoundEffect.ForeColor = System.Drawing.Color.White;
            this.cbCrashSoundEffect.FormattingEnabled = true;
            this.cbCrashSoundEffect.Items.AddRange(new object[] {
            "Breaking plates",
            "Quack",
            "None",
            "CRASHSOUNDS folder"});
            this.cbCrashSoundEffect.Location = new System.Drawing.Point(22, 126);
            this.cbCrashSoundEffect.Name = "cbCrashSoundEffect";
            this.cbCrashSoundEffect.Size = new System.Drawing.Size(163, 25);
            this.cbCrashSoundEffect.TabIndex = 16;
            this.cbCrashSoundEffect.SelectedIndexChanged += new System.EventHandler(this.cbCrashSoundEffect_SelectedIndexChanged);
            // 
            // pbTimeout
            // 
            this.pbTimeout.Location = new System.Drawing.Point(22, 74);
            this.pbTimeout.MarqueeAnimationSpeed = 1;
            this.pbTimeout.Maximum = 13;
            this.pbTimeout.Name = "pbTimeout";
            this.pbTimeout.Size = new System.Drawing.Size(163, 23);
            this.pbTimeout.Step = 1;
            this.pbTimeout.TabIndex = 17;
            this.pbTimeout.Tag = "17";
            this.pbTimeout.Value = 13;
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.ForeColor = System.Drawing.Color.White;
            this.cbEnabled.Location = new System.Drawing.Point(22, 51);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(165, 17);
            this.cbEnabled.TabIndex = 18;
            this.cbEnabled.Text = "Automatically restart BizHawk";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // pnCorruptionEngine
            // 
            this.pnCorruptionEngine.BackColor = System.Drawing.Color.Transparent;
            this.pnCorruptionEngine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnCorruptionEngine.Controls.Add(this.cbNetCoreCommandTimeout);
            this.pnCorruptionEngine.Controls.Add(this.label5);
            this.pnCorruptionEngine.Controls.Add(this.btnStartEmuhawkDetached);
            this.pnCorruptionEngine.Controls.Add(this.cbCrashSoundEffect);
            this.pnCorruptionEngine.Controls.Add(this.label1);
            this.pnCorruptionEngine.Controls.Add(this.cbEnabled);
            this.pnCorruptionEngine.Controls.Add(this.pbTimeout);
            this.pnCorruptionEngine.Location = new System.Drawing.Point(20, 148);
            this.pnCorruptionEngine.Name = "pnCorruptionEngine";
            this.pnCorruptionEngine.Size = new System.Drawing.Size(208, 221);
            this.pnCorruptionEngine.TabIndex = 114;
            this.pnCorruptionEngine.Tag = "";
            // 
            // cbNetCoreCommandTimeout
            // 
            this.cbNetCoreCommandTimeout.BackColor = System.Drawing.Color.Black;
            this.cbNetCoreCommandTimeout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNetCoreCommandTimeout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbNetCoreCommandTimeout.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbNetCoreCommandTimeout.ForeColor = System.Drawing.Color.White;
            this.cbNetCoreCommandTimeout.FormattingEnabled = true;
            this.cbNetCoreCommandTimeout.Items.AddRange(new object[] {
            "Standard",
            "Lazy",
            "Disabled"});
            this.cbNetCoreCommandTimeout.Location = new System.Drawing.Point(22, 181);
            this.cbNetCoreCommandTimeout.Name = "cbNetCoreCommandTimeout";
            this.cbNetCoreCommandTimeout.Size = new System.Drawing.Size(163, 25);
            this.cbNetCoreCommandTimeout.TabIndex = 20;
            this.cbNetCoreCommandTimeout.SelectedIndexChanged += new System.EventHandler(this.cbNetCoreCommandTimeout_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(19, 163);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "NetCore/KillSwitch Aggressiveness";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(17, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 19);
            this.label11.TabIndex = 116;
            this.label11.Text = "BizHawk Emulator";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(43, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 32);
            this.panel1.TabIndex = 117;
            // 
            // btnReturnToSession
            // 
            this.btnReturnToSession.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReturnToSession.FlatAppearance.BorderSize = 0;
            this.btnReturnToSession.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturnToSession.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnReturnToSession.ForeColor = System.Drawing.Color.Black;
            this.btnReturnToSession.Location = new System.Drawing.Point(578, 20);
            this.btnReturnToSession.Name = "btnReturnToSession";
            this.btnReturnToSession.Size = new System.Drawing.Size(208, 29);
            this.btnReturnToSession.TabIndex = 19;
            this.btnReturnToSession.Text = "Return to Session";
            this.btnReturnToSession.UseVisualStyleBackColor = false;
            this.btnReturnToSession.Visible = false;
            this.btnReturnToSession.Click += new System.EventHandler(this.btnReturnToSession_Click);
            // 
            // pnDisableGameProtection
            // 
            this.pnDisableGameProtection.BackColor = System.Drawing.Color.Transparent;
            this.pnDisableGameProtection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnDisableGameProtection.Controls.Add(this.label2);
            this.pnDisableGameProtection.Controls.Add(this.btnStopGameProtection);
            this.pnDisableGameProtection.Location = new System.Drawing.Point(578, 64);
            this.pnDisableGameProtection.Name = "pnDisableGameProtection";
            this.pnDisableGameProtection.Size = new System.Drawing.Size(208, 81);
            this.pnDisableGameProtection.TabIndex = 118;
            this.pnDisableGameProtection.Tag = "";
            this.pnDisableGameProtection.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 13);
            this.label2.TabIndex = 119;
            this.label2.Text = "Game Protection is currently enabled";
            // 
            // btnStopGameProtection
            // 
            this.btnStopGameProtection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStopGameProtection.FlatAppearance.BorderSize = 0;
            this.btnStopGameProtection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopGameProtection.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStopGameProtection.ForeColor = System.Drawing.Color.Black;
            this.btnStopGameProtection.Location = new System.Drawing.Point(22, 36);
            this.btnStopGameProtection.Name = "btnStopGameProtection";
            this.btnStopGameProtection.Size = new System.Drawing.Size(163, 28);
            this.btnStopGameProtection.TabIndex = 2;
            this.btnStopGameProtection.Text = "Stop";
            this.btnStopGameProtection.UseVisualStyleBackColor = false;
            this.btnStopGameProtection.Click += new System.EventHandler(this.btnStopGameProtection_Click);
            // 
            // pnBizhawkAttached
            // 
            this.pnBizhawkAttached.BackColor = System.Drawing.Color.Transparent;
            this.pnBizhawkAttached.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBizhawkAttached.Controls.Add(this.label3);
            this.pnBizhawkAttached.Controls.Add(this.btnStartEmuhawkAttached);
            this.pnBizhawkAttached.Location = new System.Drawing.Point(20, 415);
            this.pnBizhawkAttached.Name = "pnBizhawkAttached";
            this.pnBizhawkAttached.Size = new System.Drawing.Size(208, 72);
            this.pnBizhawkAttached.TabIndex = 120;
            this.pnBizhawkAttached.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(20, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(165, 13);
            this.label3.TabIndex = 129;
            this.label3.Text = "* Multiplayer requires Attached";
            // 
            // btnStartEmuhawkAttached
            // 
            this.btnStartEmuhawkAttached.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartEmuhawkAttached.FlatAppearance.BorderSize = 0;
            this.btnStartEmuhawkAttached.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartEmuhawkAttached.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStartEmuhawkAttached.ForeColor = System.Drawing.Color.Black;
            this.btnStartEmuhawkAttached.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.CorpHawkSmall;
            this.btnStartEmuhawkAttached.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartEmuhawkAttached.Location = new System.Drawing.Point(22, 13);
            this.btnStartEmuhawkAttached.Name = "btnStartEmuhawkAttached";
            this.btnStartEmuhawkAttached.Size = new System.Drawing.Size(163, 29);
            this.btnStartEmuhawkAttached.TabIndex = 19;
            this.btnStartEmuhawkAttached.Text = "  Start BizHawk";
            this.btnStartEmuhawkAttached.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartEmuhawkAttached.UseVisualStyleBackColor = false;
            this.btnStartEmuhawkAttached.Click += new System.EventHandler(this.btnStartEmuhawkAttached_Click);
            // 
            // lbBizhawkEmulatorAttached
            // 
            this.lbBizhawkEmulatorAttached.AutoSize = true;
            this.lbBizhawkEmulatorAttached.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbBizhawkEmulatorAttached.ForeColor = System.Drawing.Color.White;
            this.lbBizhawkEmulatorAttached.Location = new System.Drawing.Point(17, 393);
            this.lbBizhawkEmulatorAttached.Name = "lbBizhawkEmulatorAttached";
            this.lbBizhawkEmulatorAttached.Size = new System.Drawing.Size(121, 19);
            this.lbBizhawkEmulatorAttached.TabIndex = 121;
            this.lbBizhawkEmulatorAttached.Text = "Bizhawk Emulator";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(167, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 122;
            this.label4.Text = "(Detached)";
            // 
            // lbBizhawkAttached
            // 
            this.lbBizhawkAttached.AutoSize = true;
            this.lbBizhawkAttached.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbBizhawkAttached.ForeColor = System.Drawing.Color.White;
            this.lbBizhawkAttached.Location = new System.Drawing.Point(169, 396);
            this.lbBizhawkAttached.Name = "lbBizhawkAttached";
            this.lbBizhawkAttached.Size = new System.Drawing.Size(63, 15);
            this.lbBizhawkAttached.TabIndex = 123;
            this.lbBizhawkAttached.Text = "(Attached)";
            // 
            // lbWindowsGlitchHarvester
            // 
            this.lbWindowsGlitchHarvester.AutoSize = true;
            this.lbWindowsGlitchHarvester.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbWindowsGlitchHarvester.ForeColor = System.Drawing.Color.White;
            this.lbWindowsGlitchHarvester.Location = new System.Drawing.Point(575, 155);
            this.lbWindowsGlitchHarvester.Name = "lbWindowsGlitchHarvester";
            this.lbWindowsGlitchHarvester.Size = new System.Drawing.Size(174, 19);
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
            this.pnWindowsGlitchHarvester.Location = new System.Drawing.Point(578, 178);
            this.pnWindowsGlitchHarvester.Name = "pnWindowsGlitchHarvester";
            this.pnWindowsGlitchHarvester.Size = new System.Drawing.Size(208, 115);
            this.pnWindowsGlitchHarvester.TabIndex = 124;
            this.pnWindowsGlitchHarvester.Tag = "";
            this.pnWindowsGlitchHarvester.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(6, 12);
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
            this.btnStartWGH.Size = new System.Drawing.Size(163, 29);
            this.btnStartWGH.TabIndex = 19;
            this.btnStartWGH.Text = "  Start WGH";
            this.btnStartWGH.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartWGH.UseVisualStyleBackColor = false;
            this.btnStartWGH.Click += new System.EventHandler(this.btnStartWGH_Click);
            // 
            // lbVrun
            // 
            this.lbVrun.AutoSize = true;
            this.lbVrun.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbVrun.ForeColor = System.Drawing.Color.White;
            this.lbVrun.Location = new System.Drawing.Point(576, 306);
            this.lbVrun.Name = "lbVrun";
            this.lbVrun.Size = new System.Drawing.Size(88, 19);
            this.lbVrun.TabIndex = 127;
            this.lbVrun.Text = "VRUN Game";
            // 
            // pnVrun
            // 
            this.pnVrun.BackColor = System.Drawing.Color.Transparent;
            this.pnVrun.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnVrun.Controls.Add(this.button1);
            this.pnVrun.Controls.Add(this.btnStartVrun);
            this.pnVrun.Controls.Add(this.label9);
            this.pnVrun.Location = new System.Drawing.Point(578, 328);
            this.pnVrun.Name = "pnVrun";
            this.pnVrun.Size = new System.Drawing.Size(208, 159);
            this.pnVrun.TabIndex = 126;
            this.pnVrun.Tag = "";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.ToolBox;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(22, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 29);
            this.button1.TabIndex = 122;
            this.button1.Text = "  VRUN Editor";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
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
            this.btnStartVrun.Size = new System.Drawing.Size(163, 29);
            this.btnStartVrun.TabIndex = 121;
            this.btnStartVrun.Text = "  Play VRUN";
            this.btnStartVrun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStartVrun.UseVisualStyleBackColor = false;
            this.btnStartVrun.Click += new System.EventHandler(this.btnStartVrun_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(7, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(179, 39);
            this.label9.TabIndex = 121;
            this.label9.Text = "Corrupt everything in this online\nrandomized platforming shooter. \nRuns best in G" +
    "oogle Chrome.";
            // 
            // lbVrunAddress
            // 
            this.lbVrunAddress.AutoSize = true;
            this.lbVrunAddress.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lbVrunAddress.ForeColor = System.Drawing.Color.White;
            this.lbVrunAddress.Location = new System.Drawing.Point(696, 310);
            this.lbVrunAddress.Name = "lbVrunAddress";
            this.lbVrunAddress.Size = new System.Drawing.Size(93, 15);
            this.lbVrunAddress.TabIndex = 128;
            this.lbVrunAddress.Text = "http://virus.run/";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(746, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 129;
            this.label6.Text = "(Alpha)";
            // 
            // RTC_ConnectionStatus_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(811, 516);
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
            this.Controls.Add(this.pnDisableGameProtection);
            this.Controls.Add(this.btnReturnToSession);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pnCorruptionEngine);
            this.Controls.Add(this.lbConnectionStatus);
            this.Controls.Add(this.lbRTC);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_ConnectionStatus_Form";
            this.Text = "RTC_ConnectionStatus_Form";
            this.Load += new System.EventHandler(this.RTC_ConnectionStatus_Form_Load);
            this.pnCorruptionEngine.ResumeLayout(false);
            this.pnCorruptionEngine.PerformLayout();
            this.pnDisableGameProtection.ResumeLayout(false);
            this.pnDisableGameProtection.PerformLayout();
            this.pnBizhawkAttached.ResumeLayout(false);
            this.pnBizhawkAttached.PerformLayout();
            this.pnWindowsGlitchHarvester.ResumeLayout(false);
            this.pnWindowsGlitchHarvester.PerformLayout();
            this.pnVrun.ResumeLayout(false);
            this.pnVrun.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Label lbRTC;
		public System.Windows.Forms.Label lbConnectionStatus;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ComboBox cbCrashSoundEffect;
		public System.Windows.Forms.Button btnStartEmuhawkDetached;
		public System.Windows.Forms.ProgressBar pbTimeout;
		private System.Windows.Forms.Panel pnCorruptionEngine;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.CheckBox cbEnabled;
		public System.Windows.Forms.Button btnReturnToSession;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.Button btnStopGameProtection;
		public System.Windows.Forms.Panel pnDisableGameProtection;
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
        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.ComboBox cbNetCoreCommandTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
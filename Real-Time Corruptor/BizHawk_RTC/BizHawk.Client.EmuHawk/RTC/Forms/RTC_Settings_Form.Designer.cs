namespace RTC
{
    partial class RTC_Settings_Form
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
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.nmGameProtectionDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenOnlineWiki = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbNetCoreCommandTimeout = new System.Windows.Forms.ComboBox();
            this.cbCrashSoundEffect = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnChangeRTCColor = new System.Windows.Forms.Button();
            this.btnRtcFactoryClean = new System.Windows.Forms.Button();
            this.pnDetachedModeSettings = new System.Windows.Forms.Panel();
            this.lbDetachedModeSettings = new System.Windows.Forms.Label();
            this.pnAttachedModeSettings = new System.Windows.Forms.Panel();
            this.btnStartAutoKillSwitch = new System.Windows.Forms.Button();
            this.lbAttachedModeSettings = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.pnDetachedModeSettings.SuspendLayout();
            this.pnAttachedModeSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(14, 45);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(33, 13);
            this.label21.TabIndex = 117;
            this.label21.Text = "every";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(15, 24);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(129, 13);
            this.label20.TabIndex = 116;
            this.label20.Text = "Backups the game state";
            // 
            // nmGameProtectionDelay
            // 
            this.nmGameProtectionDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmGameProtectionDelay.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmGameProtectionDelay.ForeColor = System.Drawing.Color.White;
            this.nmGameProtectionDelay.Location = new System.Drawing.Point(54, 42);
            this.nmGameProtectionDelay.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmGameProtectionDelay.Name = "nmGameProtectionDelay";
            this.nmGameProtectionDelay.Size = new System.Drawing.Size(37, 25);
            this.nmGameProtectionDelay.TabIndex = 114;
            this.nmGameProtectionDelay.Tag = "color:dark";
            this.nmGameProtectionDelay.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmGameProtectionDelay.ValueChanged += new System.EventHandler(this.nmGameProtectionDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmGameProtectionDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nmGameProtectionDelay_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(95, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 115;
            this.label3.Text = "seconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 47);
            this.label2.TabIndex = 118;
            this.label2.Text = "Settings and tools";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.nmGameProtectionDelay);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(15, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 81);
            this.groupBox1.TabIndex = 124;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Game Protection";
            // 
            // btnOpenOnlineWiki
            // 
            this.btnOpenOnlineWiki.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOpenOnlineWiki.FlatAppearance.BorderSize = 0;
            this.btnOpenOnlineWiki.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOnlineWiki.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnOpenOnlineWiki.ForeColor = System.Drawing.Color.Black;
            this.btnOpenOnlineWiki.Location = new System.Drawing.Point(401, 23);
            this.btnOpenOnlineWiki.Name = "btnOpenOnlineWiki";
            this.btnOpenOnlineWiki.Size = new System.Drawing.Size(232, 29);
            this.btnOpenOnlineWiki.TabIndex = 125;
            this.btnOpenOnlineWiki.Tag = "color:light";
            this.btnOpenOnlineWiki.Text = "Open the online wiki";
            this.btnOpenOnlineWiki.UseVisualStyleBackColor = false;
            this.btnOpenOnlineWiki.Click += new System.EventHandler(this.btnOpenOnlineWiki_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbNetCoreCommandTimeout);
            this.groupBox2.Controls.Add(this.cbCrashSoundEffect);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(15, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(201, 133);
            this.groupBox2.TabIndex = 125;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "NetCore";
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
            this.cbNetCoreCommandTimeout.Location = new System.Drawing.Point(18, 93);
            this.cbNetCoreCommandTimeout.Name = "cbNetCoreCommandTimeout";
            this.cbNetCoreCommandTimeout.Size = new System.Drawing.Size(163, 25);
            this.cbNetCoreCommandTimeout.TabIndex = 129;
            this.cbNetCoreCommandTimeout.SelectedIndexChanged += new System.EventHandler(this.cbNetCoreCommandTimeout_SelectedIndexChanged);
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
            this.cbCrashSoundEffect.Location = new System.Drawing.Point(18, 42);
            this.cbCrashSoundEffect.Name = "cbCrashSoundEffect";
            this.cbCrashSoundEffect.Size = new System.Drawing.Size(163, 25);
            this.cbCrashSoundEffect.TabIndex = 127;
            this.cbCrashSoundEffect.SelectedIndexChanged += new System.EventHandler(this.cbCrashSoundEffect_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(15, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 13);
            this.label5.TabIndex = 128;
            this.label5.Text = "NetCore/KillSwitch Aggressiveness";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(15, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 126;
            this.label1.Text = "Crash sound effect:";
            // 
            // btnChangeRTCColor
            // 
            this.btnChangeRTCColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnChangeRTCColor.FlatAppearance.BorderSize = 0;
            this.btnChangeRTCColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeRTCColor.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnChangeRTCColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeRTCColor.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.paintbrush_icon;
            this.btnChangeRTCColor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnChangeRTCColor.Location = new System.Drawing.Point(19, 84);
            this.btnChangeRTCColor.Name = "btnChangeRTCColor";
            this.btnChangeRTCColor.Size = new System.Drawing.Size(274, 45);
            this.btnChangeRTCColor.TabIndex = 126;
            this.btnChangeRTCColor.Tag = "color:light";
            this.btnChangeRTCColor.Text = "   Change RTC\'s color theme";
            this.btnChangeRTCColor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnChangeRTCColor.UseVisualStyleBackColor = false;
            this.btnChangeRTCColor.Click += new System.EventHandler(this.btnChangeRTCColor_Click);
            // 
            // btnRtcFactoryClean
            // 
            this.btnRtcFactoryClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRtcFactoryClean.FlatAppearance.BorderSize = 0;
            this.btnRtcFactoryClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtcFactoryClean.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnRtcFactoryClean.ForeColor = System.Drawing.Color.Black;
            this.btnRtcFactoryClean.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
            this.btnRtcFactoryClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRtcFactoryClean.Location = new System.Drawing.Point(401, 465);
            this.btnRtcFactoryClean.Name = "btnRtcFactoryClean";
            this.btnRtcFactoryClean.Size = new System.Drawing.Size(232, 29);
            this.btnRtcFactoryClean.TabIndex = 127;
            this.btnRtcFactoryClean.Tag = "color:light";
            this.btnRtcFactoryClean.Text = "  RTC Factory Clean";
            this.btnRtcFactoryClean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRtcFactoryClean.UseVisualStyleBackColor = false;
            this.btnRtcFactoryClean.Click += new System.EventHandler(this.btnRtcFactoryClean_Click);
            // 
            // pnDetachedModeSettings
            // 
            this.pnDetachedModeSettings.BackColor = System.Drawing.Color.Gray;
            this.pnDetachedModeSettings.Controls.Add(this.groupBox2);
            this.pnDetachedModeSettings.Controls.Add(this.groupBox1);
            this.pnDetachedModeSettings.Location = new System.Drawing.Point(401, 84);
            this.pnDetachedModeSettings.Name = "pnDetachedModeSettings";
            this.pnDetachedModeSettings.Size = new System.Drawing.Size(232, 244);
            this.pnDetachedModeSettings.TabIndex = 128;
            this.pnDetachedModeSettings.Tag = "color:normal";
            // 
            // lbDetachedModeSettings
            // 
            this.lbDetachedModeSettings.AutoSize = true;
            this.lbDetachedModeSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbDetachedModeSettings.ForeColor = System.Drawing.Color.White;
            this.lbDetachedModeSettings.Location = new System.Drawing.Point(405, 63);
            this.lbDetachedModeSettings.Name = "lbDetachedModeSettings";
            this.lbDetachedModeSettings.Size = new System.Drawing.Size(163, 19);
            this.lbDetachedModeSettings.TabIndex = 129;
            this.lbDetachedModeSettings.Text = "Detached Mode Settings";
            // 
            // pnAttachedModeSettings
            // 
            this.pnAttachedModeSettings.BackColor = System.Drawing.Color.Gray;
            this.pnAttachedModeSettings.Controls.Add(this.btnStartAutoKillSwitch);
            this.pnAttachedModeSettings.Location = new System.Drawing.Point(401, 357);
            this.pnAttachedModeSettings.Name = "pnAttachedModeSettings";
            this.pnAttachedModeSettings.Size = new System.Drawing.Size(232, 60);
            this.pnAttachedModeSettings.TabIndex = 130;
            this.pnAttachedModeSettings.Tag = "color:normal";
            // 
            // btnStartAutoKillSwitch
            // 
            this.btnStartAutoKillSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartAutoKillSwitch.FlatAppearance.BorderSize = 0;
            this.btnStartAutoKillSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAutoKillSwitch.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStartAutoKillSwitch.ForeColor = System.Drawing.Color.Black;
            this.btnStartAutoKillSwitch.Location = new System.Drawing.Point(15, 15);
            this.btnStartAutoKillSwitch.Name = "btnStartAutoKillSwitch";
            this.btnStartAutoKillSwitch.Size = new System.Drawing.Size(201, 29);
            this.btnStartAutoKillSwitch.TabIndex = 126;
            this.btnStartAutoKillSwitch.Tag = "color:light";
            this.btnStartAutoKillSwitch.Text = "Start Classic AutoKillSwitch";
            this.btnStartAutoKillSwitch.UseVisualStyleBackColor = false;
            this.btnStartAutoKillSwitch.Click += new System.EventHandler(this.btnStartAutoKillSwitch_Click);
            // 
            // lbAttachedModeSettings
            // 
            this.lbAttachedModeSettings.AutoSize = true;
            this.lbAttachedModeSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbAttachedModeSettings.ForeColor = System.Drawing.Color.White;
            this.lbAttachedModeSettings.Location = new System.Drawing.Point(405, 336);
            this.lbAttachedModeSettings.Name = "lbAttachedModeSettings";
            this.lbAttachedModeSettings.Size = new System.Drawing.Size(160, 19);
            this.lbAttachedModeSettings.TabIndex = 131;
            this.lbAttachedModeSettings.Text = "Attached Mode Settings";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(19, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 29);
            this.button1.TabIndex = 127;
            this.button1.Tag = "color:light";
            this.button1.Text = " Close Settings";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RTC_Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pnAttachedModeSettings);
            this.Controls.Add(this.lbAttachedModeSettings);
            this.Controls.Add(this.pnDetachedModeSettings);
            this.Controls.Add(this.lbDetachedModeSettings);
            this.Controls.Add(this.btnRtcFactoryClean);
            this.Controls.Add(this.btnChangeRTCColor);
            this.Controls.Add(this.btnOpenOnlineWiki);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_Settings_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Settings";
            this.Load += new System.EventHandler(this.RTC_Settings_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnDetachedModeSettings.ResumeLayout(false);
            this.pnAttachedModeSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.NumericUpDown nmGameProtectionDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button btnOpenOnlineWiki;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ComboBox cbNetCoreCommandTimeout;
        public System.Windows.Forms.ComboBox cbCrashSoundEffect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnChangeRTCColor;
        public System.Windows.Forms.Button btnRtcFactoryClean;
        private System.Windows.Forms.Panel pnDetachedModeSettings;
        private System.Windows.Forms.Label lbDetachedModeSettings;
        private System.Windows.Forms.Panel pnAttachedModeSettings;
        public System.Windows.Forms.Button btnStartAutoKillSwitch;
        private System.Windows.Forms.Label lbAttachedModeSettings;
        public System.Windows.Forms.Button button1;
    }
}
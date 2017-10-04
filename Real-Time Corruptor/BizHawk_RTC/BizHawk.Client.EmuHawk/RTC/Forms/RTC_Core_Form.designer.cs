namespace RTC
{
    partial class RTC_Core_Form
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_Core_Form));
            this.btnEasyMode = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.nmGameProtectionDelay = new System.Windows.Forms.NumericUpDown();
            this.cbUseGameProtection = new System.Windows.Forms.CheckBox();
            this.btnAutoCorrupt = new System.Windows.Forms.Button();
            this.btnManualBlast = new System.Windows.Forms.Button();
            this.pnLeftPanel = new System.Windows.Forms.Panel();
            this.pnCrashProtection = new System.Windows.Forms.Panel();
            this.btnGpJumpNow = new System.Windows.Forms.Button();
            this.btnGpJumpBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.pnCrashProtectionUnavailable = new System.Windows.Forms.Panel();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.btnEngineConfig = new System.Windows.Forms.Button();
            this.btnLogo = new System.Windows.Forms.Button();
            this.btnReboot = new System.Windows.Forms.Button();
            this.btnStockpilePlayer = new System.Windows.Forms.Button();
            this.btnRTCMultiplayer = new System.Windows.Forms.Button();
            this.btnGlitchHarvester = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).BeginInit();
            this.pnLeftPanel.SuspendLayout();
            this.pnCrashProtection.SuspendLayout();
            this.pnCrashProtectionUnavailable.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEasyMode
            // 
            this.btnEasyMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEasyMode.FlatAppearance.BorderSize = 0;
            this.btnEasyMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEasyMode.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnEasyMode.ForeColor = System.Drawing.Color.Black;
            this.btnEasyMode.Image = ((System.Drawing.Image)(resources.GetObject("btnEasyMode.Image")));
            this.btnEasyMode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEasyMode.Location = new System.Drawing.Point(5, 37);
            this.btnEasyMode.Name = "btnEasyMode";
            this.btnEasyMode.Size = new System.Drawing.Size(140, 50);
            this.btnEasyMode.TabIndex = 85;
            this.btnEasyMode.TabStop = false;
            this.btnEasyMode.Tag = "color:light";
            this.btnEasyMode.Text = " Easy Start";
            this.btnEasyMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEasyMode.UseVisualStyleBackColor = false;
            this.btnEasyMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEasyMode_MouseDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(86, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "seconds";
            // 
            // nmGameProtectionDelay
            // 
            this.nmGameProtectionDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmGameProtectionDelay.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmGameProtectionDelay.ForeColor = System.Drawing.Color.White;
            this.nmGameProtectionDelay.Location = new System.Drawing.Point(45, 77);
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
            this.nmGameProtectionDelay.TabIndex = 17;
            this.nmGameProtectionDelay.Tag = "color:dark";
            this.nmGameProtectionDelay.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmGameProtectionDelay.ValueChanged += new System.EventHandler(this.nmTimeStackDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nmGameProtectionDelay_ValueChanged);
            this.nmGameProtectionDelay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.nmGameProtectionDelay_ValueChanged);
            // 
            // cbUseGameProtection
            // 
            this.cbUseGameProtection.AutoSize = true;
            this.cbUseGameProtection.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseGameProtection.ForeColor = System.Drawing.Color.White;
            this.cbUseGameProtection.Location = new System.Drawing.Point(13, 35);
            this.cbUseGameProtection.Name = "cbUseGameProtection";
            this.cbUseGameProtection.Size = new System.Drawing.Size(68, 17);
            this.cbUseGameProtection.TabIndex = 76;
            this.cbUseGameProtection.Text = "Enabled";
            this.cbUseGameProtection.UseVisualStyleBackColor = true;
            this.cbUseGameProtection.CheckedChanged += new System.EventHandler(this.cbUseGameProtection_CheckedChanged);
            // 
            // btnAutoCorrupt
            // 
            this.btnAutoCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnAutoCorrupt.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAutoCorrupt.FlatAppearance.BorderSize = 0;
            this.btnAutoCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnAutoCorrupt.Location = new System.Drawing.Point(5, 276);
            this.btnAutoCorrupt.Name = "btnAutoCorrupt";
            this.btnAutoCorrupt.Size = new System.Drawing.Size(140, 53);
            this.btnAutoCorrupt.TabIndex = 8;
            this.btnAutoCorrupt.TabStop = false;
            this.btnAutoCorrupt.Tag = "color:darker";
            this.btnAutoCorrupt.Text = "Start Auto-Corrupt";
            this.btnAutoCorrupt.UseVisualStyleBackColor = false;
            this.btnAutoCorrupt.Click += new System.EventHandler(this.btnAutoCorrupt_Click);
            // 
            // btnManualBlast
            // 
            this.btnManualBlast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnManualBlast.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnManualBlast.FlatAppearance.BorderSize = 0;
            this.btnManualBlast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualBlast.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnManualBlast.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnManualBlast.Location = new System.Drawing.Point(5, 232);
            this.btnManualBlast.Name = "btnManualBlast";
            this.btnManualBlast.Size = new System.Drawing.Size(140, 40);
            this.btnManualBlast.TabIndex = 7;
            this.btnManualBlast.TabStop = false;
            this.btnManualBlast.Tag = "color:darker";
            this.btnManualBlast.Text = "Manual Blast";
            this.btnManualBlast.UseVisualStyleBackColor = false;
            this.btnManualBlast.Click += new System.EventHandler(this.btnManualBlast_Click);
            this.btnManualBlast.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnManualBlast_MouseDown);
            // 
            // pnLeftPanel
            // 
            this.pnLeftPanel.BackColor = System.Drawing.Color.Gray;
            this.pnLeftPanel.Controls.Add(this.pnCrashProtection);
            this.pnLeftPanel.Controls.Add(this.pnCrashProtectionUnavailable);
            this.pnLeftPanel.Controls.Add(this.btnEngineConfig);
            this.pnLeftPanel.Controls.Add(this.btnLogo);
            this.pnLeftPanel.Controls.Add(this.btnReboot);
            this.pnLeftPanel.Controls.Add(this.btnEasyMode);
            this.pnLeftPanel.Controls.Add(this.btnStockpilePlayer);
            this.pnLeftPanel.Controls.Add(this.btnRTCMultiplayer);
            this.pnLeftPanel.Controls.Add(this.btnGlitchHarvester);
            this.pnLeftPanel.Controls.Add(this.btnManualBlast);
            this.pnLeftPanel.Controls.Add(this.btnAutoCorrupt);
            this.pnLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.pnLeftPanel.Name = "pnLeftPanel";
            this.pnLeftPanel.Size = new System.Drawing.Size(150, 515);
            this.pnLeftPanel.TabIndex = 70;
            this.pnLeftPanel.Tag = "color:normal";
            // 
            // pnCrashProtection
            // 
            this.pnCrashProtection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnCrashProtection.Controls.Add(this.btnGpJumpNow);
            this.pnCrashProtection.Controls.Add(this.btnGpJumpBack);
            this.pnCrashProtection.Controls.Add(this.label2);
            this.pnCrashProtection.Controls.Add(this.label21);
            this.pnCrashProtection.Controls.Add(this.label20);
            this.pnCrashProtection.Controls.Add(this.cbUseGameProtection);
            this.pnCrashProtection.Controls.Add(this.nmGameProtectionDelay);
            this.pnCrashProtection.Controls.Add(this.label3);
            this.pnCrashProtection.Location = new System.Drawing.Point(5, 333);
            this.pnCrashProtection.Name = "pnCrashProtection";
            this.pnCrashProtection.Size = new System.Drawing.Size(140, 146);
            this.pnCrashProtection.TabIndex = 116;
            this.pnCrashProtection.Tag = "color:darker";
            this.pnCrashProtection.Visible = false;
            // 
            // btnGpJumpNow
            // 
            this.btnGpJumpNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGpJumpNow.FlatAppearance.BorderSize = 0;
            this.btnGpJumpNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGpJumpNow.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpNow.ForeColor = System.Drawing.Color.Black;
            this.btnGpJumpNow.Location = new System.Drawing.Point(75, 113);
            this.btnGpJumpNow.Name = "btnGpJumpNow";
            this.btnGpJumpNow.Size = new System.Drawing.Size(60, 26);
            this.btnGpJumpNow.TabIndex = 117;
            this.btnGpJumpNow.TabStop = false;
            this.btnGpJumpNow.Tag = "color:light";
            this.btnGpJumpNow.Text = "Now ⏩";
            this.btnGpJumpNow.UseVisualStyleBackColor = false;
            this.btnGpJumpNow.Visible = false;
            this.btnGpJumpNow.Click += new System.EventHandler(this.btnGpJumpNow_Click);
            // 
            // btnGpJumpBack
            // 
            this.btnGpJumpBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGpJumpBack.FlatAppearance.BorderSize = 0;
            this.btnGpJumpBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGpJumpBack.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpBack.ForeColor = System.Drawing.Color.Black;
            this.btnGpJumpBack.Location = new System.Drawing.Point(5, 113);
            this.btnGpJumpBack.Name = "btnGpJumpBack";
            this.btnGpJumpBack.Size = new System.Drawing.Size(61, 26);
            this.btnGpJumpBack.TabIndex = 116;
            this.btnGpJumpBack.TabStop = false;
            this.btnGpJumpBack.Tag = "color:light";
            this.btnGpJumpBack.Text = "⏪ Back";
            this.btnGpJumpBack.UseVisualStyleBackColor = false;
            this.btnGpJumpBack.Visible = false;
            this.btnGpJumpBack.Click += new System.EventHandler(this.btnGpJumpBack_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(9, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 20);
            this.label2.TabIndex = 111;
            this.label2.Text = "Game Protection";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(5, 80);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(33, 13);
            this.label21.TabIndex = 113;
            this.label21.Text = "every";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(6, 59);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(129, 13);
            this.label20.TabIndex = 112;
            this.label20.Text = "Backups the game state";
            // 
            // pnCrashProtectionUnavailable
            // 
            this.pnCrashProtectionUnavailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.pnCrashProtectionUnavailable.Controls.Add(this.label24);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label23);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label22);
            this.pnCrashProtectionUnavailable.Location = new System.Drawing.Point(11, 368);
            this.pnCrashProtectionUnavailable.Name = "pnCrashProtectionUnavailable";
            this.pnCrashProtectionUnavailable.Size = new System.Drawing.Size(124, 68);
            this.pnCrashProtectionUnavailable.TabIndex = 119;
            this.pnCrashProtectionUnavailable.Tag = "color:darker";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(13, 43);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(98, 17);
            this.label24.TabIndex = 17;
            this.label24.Text = "Attached Mode";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(12, 24);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(100, 17);
            this.label23.TabIndex = 16;
            this.label23.Text = "is unavailable in";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(11, 5);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(106, 17);
            this.label22.TabIndex = 15;
            this.label22.Text = "Game protection";
            // 
            // btnEngineConfig
            // 
            this.btnEngineConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEngineConfig.FlatAppearance.BorderSize = 0;
            this.btnEngineConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEngineConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnEngineConfig.ForeColor = System.Drawing.Color.Black;
            this.btnEngineConfig.Location = new System.Drawing.Point(5, 92);
            this.btnEngineConfig.Name = "btnEngineConfig";
            this.btnEngineConfig.Size = new System.Drawing.Size(140, 30);
            this.btnEngineConfig.TabIndex = 118;
            this.btnEngineConfig.TabStop = false;
            this.btnEngineConfig.Tag = "color:light";
            this.btnEngineConfig.Text = "Engine Config";
            this.btnEngineConfig.UseVisualStyleBackColor = false;
            this.btnEngineConfig.Click += new System.EventHandler(this.btnEngineConfig_Click);
            // 
            // btnLogo
            // 
            this.btnLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLogo.FlatAppearance.BorderSize = 0;
            this.btnLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogo.ForeColor = System.Drawing.Color.White;
            this.btnLogo.Image = ((System.Drawing.Image)(resources.GetObject("btnLogo.Image")));
            this.btnLogo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLogo.Location = new System.Drawing.Point(0, 0);
            this.btnLogo.Name = "btnLogo";
            this.btnLogo.Size = new System.Drawing.Size(155, 33);
            this.btnLogo.TabIndex = 117;
            this.btnLogo.TabStop = false;
            this.btnLogo.Tag = "color:darker";
            this.btnLogo.Text = "    Version 0.00";
            this.btnLogo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogo.UseVisualStyleBackColor = false;
            this.btnLogo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnLogo_MouseClick);
            // 
            // btnReboot
            // 
            this.btnReboot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReboot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnReboot.FlatAppearance.BorderSize = 0;
            this.btnReboot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReboot.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnReboot.ForeColor = System.Drawing.Color.White;
            this.btnReboot.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.add;
            this.btnReboot.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReboot.Location = new System.Drawing.Point(0, 482);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(155, 33);
            this.btnReboot.TabIndex = 86;
            this.btnReboot.TabStop = false;
            this.btnReboot.Tag = "color:darker";
            this.btnReboot.Text = "   RTC Settings ";
            this.btnReboot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReboot.UseVisualStyleBackColor = false;
            this.btnReboot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnReboot_MouseDown);
            // 
            // btnStockpilePlayer
            // 
            this.btnStockpilePlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpilePlayer.FlatAppearance.BorderSize = 0;
            this.btnStockpilePlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpilePlayer.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnStockpilePlayer.ForeColor = System.Drawing.Color.Black;
            this.btnStockpilePlayer.Location = new System.Drawing.Point(5, 162);
            this.btnStockpilePlayer.Name = "btnStockpilePlayer";
            this.btnStockpilePlayer.Size = new System.Drawing.Size(140, 30);
            this.btnStockpilePlayer.TabIndex = 109;
            this.btnStockpilePlayer.TabStop = false;
            this.btnStockpilePlayer.Tag = "color:light";
            this.btnStockpilePlayer.Text = "Stockpile Player";
            this.btnStockpilePlayer.UseVisualStyleBackColor = false;
            this.btnStockpilePlayer.Click += new System.EventHandler(this.btnStockPilePlayer_Click);
            // 
            // btnRTCMultiplayer
            // 
            this.btnRTCMultiplayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRTCMultiplayer.FlatAppearance.BorderSize = 0;
            this.btnRTCMultiplayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRTCMultiplayer.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnRTCMultiplayer.ForeColor = System.Drawing.Color.Black;
            this.btnRTCMultiplayer.Location = new System.Drawing.Point(5, 197);
            this.btnRTCMultiplayer.Name = "btnRTCMultiplayer";
            this.btnRTCMultiplayer.Size = new System.Drawing.Size(140, 30);
            this.btnRTCMultiplayer.TabIndex = 108;
            this.btnRTCMultiplayer.TabStop = false;
            this.btnRTCMultiplayer.Tag = "color:light";
            this.btnRTCMultiplayer.Text = "RTC Multiplayer";
            this.btnRTCMultiplayer.UseVisualStyleBackColor = false;
            this.btnRTCMultiplayer.Click += new System.EventHandler(this.btnRTCMultiplayer_Click);
            // 
            // btnGlitchHarvester
            // 
            this.btnGlitchHarvester.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGlitchHarvester.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvester.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvester.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnGlitchHarvester.ForeColor = System.Drawing.Color.Black;
            this.btnGlitchHarvester.Location = new System.Drawing.Point(5, 127);
            this.btnGlitchHarvester.Name = "btnGlitchHarvester";
            this.btnGlitchHarvester.Size = new System.Drawing.Size(140, 30);
            this.btnGlitchHarvester.TabIndex = 107;
            this.btnGlitchHarvester.TabStop = false;
            this.btnGlitchHarvester.Tag = "color:light";
            this.btnGlitchHarvester.Text = "Glitch Harvester";
            this.btnGlitchHarvester.UseVisualStyleBackColor = false;
            this.btnGlitchHarvester.Click += new System.EventHandler(this.btnGlitchHarvester_Click);
            // 
            // RTC_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(804, 515);
            this.Controls.Add(this.pnLeftPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RTC_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Attached Mode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmGameProtectionDelay)).EndInit();
            this.pnLeftPanel.ResumeLayout(false);
            this.pnCrashProtection.ResumeLayout(false);
            this.pnCrashProtection.PerformLayout();
            this.pnCrashProtectionUnavailable.ResumeLayout(false);
            this.pnCrashProtectionUnavailable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnManualBlast;
        public System.Windows.Forms.Panel pnLeftPanel;
        private System.Windows.Forms.Button btnGlitchHarvester;
        public System.Windows.Forms.Button btnAutoCorrupt;
        public System.Windows.Forms.CheckBox cbUseGameProtection;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown nmGameProtectionDelay;
        public System.Windows.Forms.Button btnEasyMode;
        public System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.Button btnRTCMultiplayer;
        private System.Windows.Forms.Button btnStockpilePlayer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnCrashProtection;
		public System.Windows.Forms.Button btnLogo;
		private System.Windows.Forms.Button btnEngineConfig;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Panel pnCrashProtectionUnavailable;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label22;
		public System.Windows.Forms.Button btnGpJumpNow;
		public System.Windows.Forms.Button btnGpJumpBack;
    }
}


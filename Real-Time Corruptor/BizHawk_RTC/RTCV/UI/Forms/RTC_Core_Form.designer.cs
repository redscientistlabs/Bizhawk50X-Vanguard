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
            this.cbUseGameProtection = new System.Windows.Forms.CheckBox();
            this.btnAutoCorrupt = new System.Windows.Forms.Button();
            this.btnManualBlast = new System.Windows.Forms.Button();
            this.pnLeftPanel = new System.Windows.Forms.Panel();
            this.pnAutoKillSwitch = new System.Windows.Forms.Panel();
            this.btnAutoKillSwitchExecute = new System.Windows.Forms.Button();
            this.cbAutoKillSwitchExecuteAction = new System.Windows.Forms.ComboBox();
            this.cbUseAutoKillSwitch = new System.Windows.Forms.CheckBox();
            this.pbAutoKillSwitchTimeout = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.pnCrashProtection = new System.Windows.Forms.Panel();
            this.btnGpJumpNow = new System.Windows.Forms.Button();
            this.btnGpJumpBack = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnEngineConfig = new System.Windows.Forms.Button();
            this.btnLogo = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnStockpilePlayer = new System.Windows.Forms.Button();
            this.btnRTCMultiplayer = new System.Windows.Forms.Button();
            this.btnGlitchHarvester = new System.Windows.Forms.Button();
            this.pnCrashProtectionUnavailable = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.pnLeftPanel.SuspendLayout();
            this.pnAutoKillSwitch.SuspendLayout();
            this.pnCrashProtection.SuspendLayout();
            this.pnCrashProtectionUnavailable.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEasyMode
            // 
            this.btnEasyMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEasyMode.FlatAppearance.BorderSize = 0;
            this.btnEasyMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEasyMode.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnEasyMode.ForeColor = System.Drawing.Color.Black;
            this.btnEasyMode.Image = ((System.Drawing.Image)(resources.GetObject("btnEasyMode.Image")));
            this.btnEasyMode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEasyMode.Location = new System.Drawing.Point(4, 52);
            this.btnEasyMode.Name = "btnEasyMode";
            this.btnEasyMode.Size = new System.Drawing.Size(140, 44);
            this.btnEasyMode.TabIndex = 85;
            this.btnEasyMode.TabStop = false;
            this.btnEasyMode.Tag = "color:light";
            this.btnEasyMode.Text = " Easy Start";
            this.btnEasyMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEasyMode.UseVisualStyleBackColor = false;
            this.btnEasyMode.Visible = false;
            this.btnEasyMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEasyMode_MouseDown);
            // 
            // cbUseGameProtection
            // 
            this.cbUseGameProtection.AutoSize = true;
            this.cbUseGameProtection.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseGameProtection.ForeColor = System.Drawing.Color.White;
            this.cbUseGameProtection.Location = new System.Drawing.Point(9, 30);
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
            this.btnAutoCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnAutoCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnAutoCorrupt.Location = new System.Drawing.Point(4, 259);
            this.btnAutoCorrupt.Name = "btnAutoCorrupt";
            this.btnAutoCorrupt.Size = new System.Drawing.Size(140, 29);
            this.btnAutoCorrupt.TabIndex = 8;
            this.btnAutoCorrupt.TabStop = false;
            this.btnAutoCorrupt.Tag = "color:darker";
            this.btnAutoCorrupt.Text = "Start Auto-Corrupt";
            this.btnAutoCorrupt.UseVisualStyleBackColor = false;
            this.btnAutoCorrupt.Visible = false;
            this.btnAutoCorrupt.Click += new System.EventHandler(this.btnAutoCorrupt_Click);
            // 
            // btnManualBlast
            // 
            this.btnManualBlast.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnManualBlast.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnManualBlast.FlatAppearance.BorderSize = 0;
            this.btnManualBlast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnManualBlast.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnManualBlast.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnManualBlast.Location = new System.Drawing.Point(4, 227);
            this.btnManualBlast.Name = "btnManualBlast";
            this.btnManualBlast.Size = new System.Drawing.Size(140, 29);
            this.btnManualBlast.TabIndex = 7;
            this.btnManualBlast.TabStop = false;
            this.btnManualBlast.Tag = "color:darker";
            this.btnManualBlast.Text = "Manual Blast";
            this.btnManualBlast.UseVisualStyleBackColor = false;
            this.btnManualBlast.Visible = false;
            this.btnManualBlast.Click += new System.EventHandler(this.btnManualBlast_Click);
            this.btnManualBlast.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnManualBlast_MouseDown);
            // 
            // pnLeftPanel
            // 
            this.pnLeftPanel.BackColor = System.Drawing.Color.Gray;
            this.pnLeftPanel.Controls.Add(this.pnAutoKillSwitch);
            this.pnLeftPanel.Controls.Add(this.pnCrashProtection);
            this.pnLeftPanel.Controls.Add(this.btnEngineConfig);
            this.pnLeftPanel.Controls.Add(this.btnLogo);
            this.pnLeftPanel.Controls.Add(this.btnSettings);
            this.pnLeftPanel.Controls.Add(this.btnEasyMode);
            this.pnLeftPanel.Controls.Add(this.btnStockpilePlayer);
            this.pnLeftPanel.Controls.Add(this.btnRTCMultiplayer);
            this.pnLeftPanel.Controls.Add(this.btnGlitchHarvester);
            this.pnLeftPanel.Controls.Add(this.btnManualBlast);
            this.pnLeftPanel.Controls.Add(this.btnAutoCorrupt);
            this.pnLeftPanel.Controls.Add(this.pnCrashProtectionUnavailable);
            this.pnLeftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnLeftPanel.Location = new System.Drawing.Point(0, 0);
            this.pnLeftPanel.Name = "pnLeftPanel";
            this.pnLeftPanel.Size = new System.Drawing.Size(150, 515);
            this.pnLeftPanel.TabIndex = 70;
            this.pnLeftPanel.Tag = "color:normal";
            // 
            // pnAutoKillSwitch
            // 
            this.pnAutoKillSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnAutoKillSwitch.Controls.Add(this.btnAutoKillSwitchExecute);
            this.pnAutoKillSwitch.Controls.Add(this.cbAutoKillSwitchExecuteAction);
            this.pnAutoKillSwitch.Controls.Add(this.cbUseAutoKillSwitch);
            this.pnAutoKillSwitch.Controls.Add(this.pbAutoKillSwitchTimeout);
            this.pnAutoKillSwitch.Controls.Add(this.label4);
            this.pnAutoKillSwitch.Location = new System.Drawing.Point(4, 378);
            this.pnAutoKillSwitch.Name = "pnAutoKillSwitch";
            this.pnAutoKillSwitch.Size = new System.Drawing.Size(140, 101);
            this.pnAutoKillSwitch.TabIndex = 118;
            this.pnAutoKillSwitch.Tag = "color:dark";
            this.pnAutoKillSwitch.Visible = false;
            // 
            // btnAutoKillSwitchExecute
            // 
            this.btnAutoKillSwitchExecute.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAutoKillSwitchExecute.FlatAppearance.BorderSize = 0;
            this.btnAutoKillSwitchExecute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoKillSwitchExecute.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnAutoKillSwitchExecute.ForeColor = System.Drawing.Color.Black;
            this.btnAutoKillSwitchExecute.Location = new System.Drawing.Point(9, 65);
            this.btnAutoKillSwitchExecute.Name = "btnAutoKillSwitchExecute";
            this.btnAutoKillSwitchExecute.Size = new System.Drawing.Size(106, 25);
            this.btnAutoKillSwitchExecute.TabIndex = 118;
            this.btnAutoKillSwitchExecute.TabStop = false;
            this.btnAutoKillSwitchExecute.Tag = "color:light";
            this.btnAutoKillSwitchExecute.Text = "Kill + Restart";
            this.btnAutoKillSwitchExecute.UseVisualStyleBackColor = false;
            this.btnAutoKillSwitchExecute.Click += new System.EventHandler(this.btnAutoKillSwitchExecute_Click);
            // 
            // cbAutoKillSwitchExecuteAction
            // 
            this.cbAutoKillSwitchExecuteAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAutoKillSwitchExecuteAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbAutoKillSwitchExecuteAction.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cbAutoKillSwitchExecuteAction.FormattingEnabled = true;
            this.cbAutoKillSwitchExecuteAction.Items.AddRange(new object[] {
            "Kill",
            "Kill + Restart",
            "Restart + Reset"});
            this.cbAutoKillSwitchExecuteAction.Location = new System.Drawing.Point(9, 65);
            this.cbAutoKillSwitchExecuteAction.Name = "cbAutoKillSwitchExecuteAction";
            this.cbAutoKillSwitchExecuteAction.Size = new System.Drawing.Size(123, 25);
            this.cbAutoKillSwitchExecuteAction.TabIndex = 119;
            this.cbAutoKillSwitchExecuteAction.SelectedIndexChanged += new System.EventHandler(this.cbAutoKillSwitchExecuteAction_SelectedIndexChanged);
            // 
            // cbUseAutoKillSwitch
            // 
            this.cbUseAutoKillSwitch.AutoSize = true;
            this.cbUseAutoKillSwitch.Checked = true;
            this.cbUseAutoKillSwitch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseAutoKillSwitch.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseAutoKillSwitch.ForeColor = System.Drawing.Color.White;
            this.cbUseAutoKillSwitch.Location = new System.Drawing.Point(9, 28);
            this.cbUseAutoKillSwitch.Name = "cbUseAutoKillSwitch";
            this.cbUseAutoKillSwitch.Size = new System.Drawing.Size(68, 17);
            this.cbUseAutoKillSwitch.TabIndex = 120;
            this.cbUseAutoKillSwitch.Text = "Enabled";
            this.cbUseAutoKillSwitch.UseVisualStyleBackColor = true;
            this.cbUseAutoKillSwitch.CheckedChanged += new System.EventHandler(this.cbUseAutoKillSwitch_CheckedChanged);
            // 
            // pbAutoKillSwitchTimeout
            // 
            this.pbAutoKillSwitchTimeout.Location = new System.Drawing.Point(9, 48);
            this.pbAutoKillSwitchTimeout.MarqueeAnimationSpeed = 1;
            this.pbAutoKillSwitchTimeout.Maximum = 13;
            this.pbAutoKillSwitchTimeout.Name = "pbAutoKillSwitchTimeout";
            this.pbAutoKillSwitchTimeout.Size = new System.Drawing.Size(123, 10);
            this.pbAutoKillSwitchTimeout.Step = 1;
            this.pbAutoKillSwitchTimeout.TabIndex = 119;
            this.pbAutoKillSwitchTimeout.Tag = "17";
            this.pbAutoKillSwitchTimeout.Value = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(8, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 19);
            this.label4.TabIndex = 111;
            this.label4.Text = "Auto-KillSwitch";
            // 
            // pnCrashProtection
            // 
            this.pnCrashProtection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnCrashProtection.Controls.Add(this.btnGpJumpNow);
            this.pnCrashProtection.Controls.Add(this.btnGpJumpBack);
            this.pnCrashProtection.Controls.Add(this.label2);
            this.pnCrashProtection.Controls.Add(this.cbUseGameProtection);
            this.pnCrashProtection.Location = new System.Drawing.Point(4, 291);
            this.pnCrashProtection.Name = "pnCrashProtection";
            this.pnCrashProtection.Size = new System.Drawing.Size(140, 84);
            this.pnCrashProtection.TabIndex = 116;
            this.pnCrashProtection.Tag = "color:dark";
            this.pnCrashProtection.Visible = false;
            // 
            // btnGpJumpNow
            // 
            this.btnGpJumpNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGpJumpNow.FlatAppearance.BorderSize = 0;
            this.btnGpJumpNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGpJumpNow.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpNow.ForeColor = System.Drawing.Color.Black;
            this.btnGpJumpNow.Location = new System.Drawing.Point(73, 49);
            this.btnGpJumpNow.Name = "btnGpJumpNow";
            this.btnGpJumpNow.Size = new System.Drawing.Size(59, 23);
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
            this.btnGpJumpBack.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnGpJumpBack.ForeColor = System.Drawing.Color.Black;
            this.btnGpJumpBack.Location = new System.Drawing.Point(9, 49);
            this.btnGpJumpBack.Name = "btnGpJumpBack";
            this.btnGpJumpBack.Size = new System.Drawing.Size(59, 23);
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
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 19);
            this.label2.TabIndex = 111;
            this.label2.Text = "Game Protection";
            // 
            // btnEngineConfig
            // 
            this.btnEngineConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEngineConfig.FlatAppearance.BorderSize = 0;
            this.btnEngineConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEngineConfig.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnEngineConfig.ForeColor = System.Drawing.Color.Black;
            this.btnEngineConfig.Location = new System.Drawing.Point(4, 99);
            this.btnEngineConfig.Name = "btnEngineConfig";
            this.btnEngineConfig.Size = new System.Drawing.Size(140, 29);
            this.btnEngineConfig.TabIndex = 118;
            this.btnEngineConfig.TabStop = false;
            this.btnEngineConfig.Tag = "color:light";
            this.btnEngineConfig.Text = "Engine Config";
            this.btnEngineConfig.UseVisualStyleBackColor = false;
            this.btnEngineConfig.Visible = false;
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
            this.btnLogo.Size = new System.Drawing.Size(150, 49);
            this.btnLogo.TabIndex = 117;
            this.btnLogo.TabStop = false;
            this.btnLogo.Tag = "color:darker";
            this.btnLogo.Text = "   Version 0.00";
            this.btnLogo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogo.UseVisualStyleBackColor = false;
            this.btnLogo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnLogo_MouseClick);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettings.Location = new System.Drawing.Point(0, 482);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(150, 33);
            this.btnSettings.TabIndex = 86;
            this.btnSettings.TabStop = false;
            this.btnSettings.Tag = "color:darker";
            this.btnSettings.Text = "   Settings and tools";
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnStockpilePlayer
            // 
            this.btnStockpilePlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpilePlayer.FlatAppearance.BorderSize = 0;
            this.btnStockpilePlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpilePlayer.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnStockpilePlayer.ForeColor = System.Drawing.Color.Black;
            this.btnStockpilePlayer.Location = new System.Drawing.Point(4, 163);
            this.btnStockpilePlayer.Name = "btnStockpilePlayer";
            this.btnStockpilePlayer.Size = new System.Drawing.Size(140, 29);
            this.btnStockpilePlayer.TabIndex = 109;
            this.btnStockpilePlayer.TabStop = false;
            this.btnStockpilePlayer.Tag = "color:light";
            this.btnStockpilePlayer.Text = "Stockpile Player";
            this.btnStockpilePlayer.UseVisualStyleBackColor = false;
            this.btnStockpilePlayer.Visible = false;
            this.btnStockpilePlayer.Click += new System.EventHandler(this.btnStockPilePlayer_Click);
            // 
            // btnRTCMultiplayer
            // 
            this.btnRTCMultiplayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRTCMultiplayer.FlatAppearance.BorderSize = 0;
            this.btnRTCMultiplayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRTCMultiplayer.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnRTCMultiplayer.ForeColor = System.Drawing.Color.Black;
            this.btnRTCMultiplayer.Location = new System.Drawing.Point(4, 195);
            this.btnRTCMultiplayer.Name = "btnRTCMultiplayer";
            this.btnRTCMultiplayer.Size = new System.Drawing.Size(140, 29);
            this.btnRTCMultiplayer.TabIndex = 108;
            this.btnRTCMultiplayer.TabStop = false;
            this.btnRTCMultiplayer.Tag = "color:light";
            this.btnRTCMultiplayer.Text = "RTC Multiplayer";
            this.btnRTCMultiplayer.UseVisualStyleBackColor = false;
            this.btnRTCMultiplayer.Visible = false;
            this.btnRTCMultiplayer.Click += new System.EventHandler(this.btnRTCMultiplayer_Click);
            // 
            // btnGlitchHarvester
            // 
            this.btnGlitchHarvester.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGlitchHarvester.FlatAppearance.BorderSize = 0;
            this.btnGlitchHarvester.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGlitchHarvester.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnGlitchHarvester.ForeColor = System.Drawing.Color.Black;
            this.btnGlitchHarvester.Location = new System.Drawing.Point(4, 131);
            this.btnGlitchHarvester.Name = "btnGlitchHarvester";
            this.btnGlitchHarvester.Size = new System.Drawing.Size(140, 29);
            this.btnGlitchHarvester.TabIndex = 107;
            this.btnGlitchHarvester.TabStop = false;
            this.btnGlitchHarvester.Tag = "color:light";
            this.btnGlitchHarvester.Text = "Glitch Harvester";
            this.btnGlitchHarvester.UseVisualStyleBackColor = false;
            this.btnGlitchHarvester.Visible = false;
            this.btnGlitchHarvester.Click += new System.EventHandler(this.btnGlitchHarvester_Click);
            // 
            // pnCrashProtectionUnavailable
            // 
            this.pnCrashProtectionUnavailable.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnCrashProtectionUnavailable.Controls.Add(this.label7);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label6);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label5);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label3);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label1);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label24);
            this.pnCrashProtectionUnavailable.Controls.Add(this.label23);
            this.pnCrashProtectionUnavailable.Location = new System.Drawing.Point(5, 291);
            this.pnCrashProtectionUnavailable.Name = "pnCrashProtectionUnavailable";
            this.pnCrashProtectionUnavailable.Size = new System.Drawing.Size(140, 188);
            this.pnCrashProtectionUnavailable.TabIndex = 119;
            this.pnCrashProtectionUnavailable.Tag = "color:dark";
            this.pnCrashProtectionUnavailable.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label7.ForeColor = System.Drawing.Color.Gainsboro;
            this.label7.Location = new System.Drawing.Point(7, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 13);
            this.label7.TabIndex = 116;
            this.label7.Text = "KillSwitch from settings";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label6.ForeColor = System.Drawing.Color.Gainsboro;
            this.label6.Location = new System.Drawing.Point(3, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 13);
            this.label6.TabIndex = 115;
            this.label6.Text = "You can open the Classic";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(15, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 19);
            this.label5.TabIndex = 114;
            this.label5.Text = "Auto-KillSwitch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(17, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 113;
            this.label3.Text = "and the integrated";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 19);
            this.label1.TabIndex = 112;
            this.label1.Text = "Game Protection";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label24.ForeColor = System.Drawing.Color.Gainsboro;
            this.label24.Location = new System.Drawing.Point(25, 91);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(86, 13);
            this.label24.TabIndex = 17;
            this.label24.Text = "Attached Mode";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label23.ForeColor = System.Drawing.Color.Gainsboro;
            this.label23.Location = new System.Drawing.Point(20, 76);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(98, 13);
            this.label23.TabIndex = 16;
            this.label23.Text = "are unavailable in";
            // 
            // RTC_Core_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(804, 515);
            this.Controls.Add(this.pnLeftPanel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RTC_Core_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Attached Mode";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_Form_Load);
            this.pnLeftPanel.ResumeLayout(false);
            this.pnAutoKillSwitch.ResumeLayout(false);
            this.pnAutoKillSwitch.PerformLayout();
            this.pnCrashProtection.ResumeLayout(false);
            this.pnCrashProtection.PerformLayout();
            this.pnCrashProtectionUnavailable.ResumeLayout(false);
            this.pnCrashProtectionUnavailable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnManualBlast;
        public System.Windows.Forms.Panel pnLeftPanel;
        public System.Windows.Forms.Button btnAutoCorrupt;
        public System.Windows.Forms.CheckBox cbUseGameProtection;
        public System.Windows.Forms.Button btnEasyMode;
        public System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnRTCMultiplayer;
        private System.Windows.Forms.Button btnStockpilePlayer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnCrashProtection;
		public System.Windows.Forms.Button btnLogo;
		private System.Windows.Forms.Button btnEngineConfig;
		private System.Windows.Forms.Panel pnCrashProtectionUnavailable;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label23;
		public System.Windows.Forms.Button btnGpJumpNow;
		public System.Windows.Forms.Button btnGpJumpBack;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnAutoKillSwitch;
        public System.Windows.Forms.CheckBox cbUseAutoKillSwitch;
        public System.Windows.Forms.ProgressBar pbAutoKillSwitchTimeout;
        public System.Windows.Forms.Button btnAutoKillSwitchExecute;
        private System.Windows.Forms.ComboBox cbAutoKillSwitchExecuteAction;
        public System.Windows.Forms.Button btnGlitchHarvester;
    }
}


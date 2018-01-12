namespace WindowsGlitchHarvester
{
    public partial class WGH_MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WGH_MainForm));
            this.btnBlastTarget = new System.Windows.Forms.Button();
            this.btnBrowseTarget = new System.Windows.Forms.Button();
            this.lbStashHistory = new System.Windows.Forms.ListBox();
            this.btnLoadStockpile = new System.Windows.Forms.Button();
            this.btnClearStashHistory = new System.Windows.Forms.Button();
            this.btnSaveStockpileAs = new System.Windows.Forms.Button();
            this.btnSaveStockpile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearStockpile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTarget = new System.Windows.Forms.Label();
            this.btnStockpileMoveDown = new System.Windows.Forms.Button();
            this.btnStockpileMoveUp = new System.Windows.Forms.Button();
            this.nmIntensity = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tbIntensity = new System.Windows.Forms.TrackBar();
            this.tbStartingAddress = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.nmStartingAddress = new System.Windows.Forms.NumericUpDown();
            this.tbBlastRange = new System.Windows.Forms.TrackBar();
            this.nmBlastRange = new System.Windows.Forms.NumericUpDown();
            this.btnAddStashToStockpile = new System.Windows.Forms.Button();
            this.cbBlastRange = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnRestoreFileBackup = new System.Windows.Forms.Button();
            this.gbNightmareEngineSettings = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBlastType = new System.Windows.Forms.ComboBox();
            this.cbCorruptionEngine = new System.Windows.Forms.ComboBox();
            this.rbTargetFile = new System.Windows.Forms.RadioButton();
            this.rbTargetProcess = new System.Windows.Forms.RadioButton();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnImportStockpile = new System.Windows.Forms.Button();
            this.btnStockpileUp = new System.Windows.Forms.Button();
            this.btnStockpileDown = new System.Windows.Forms.Button();
            this.btnStashHistoryUp = new System.Windows.Forms.Button();
            this.btnStashHistoryDown = new System.Windows.Forms.Button();
            this.pnTargetPanel = new System.Windows.Forms.Panel();
            this.savestateInfoButton = new System.Windows.Forms.Button();
            this.rbTargetMultipleFiles = new System.Windows.Forms.RadioButton();
            this.btnClearAllBackups = new System.Windows.Forms.Button();
            this.btnResetBackup = new System.Windows.Forms.Button();
            this.cbWriteCopyMode = new System.Windows.Forms.CheckBox();
            this.btnInjectSelected = new System.Windows.Forms.Button();
            this.btnDisableAutoUncorrupt = new System.Windows.Forms.Button();
            this.btnEnableCaching = new System.Windows.Forms.Button();
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnKillProcess = new System.Windows.Forms.Button();
            this.lbArgs = new System.Windows.Forms.Label();
            this.tbArgs = new System.Windows.Forms.TextBox();
            this.btnBlastUntilEffect = new System.Windows.Forms.Button();
            this.cbTerminateOnReExec = new System.Windows.Forms.CheckBox();
            this.rbExecuteWith = new System.Windows.Forms.RadioButton();
            this.rbExecuteScript = new System.Windows.Forms.RadioButton();
            this.rbExecuteOtherProgram = new System.Windows.Forms.RadioButton();
            this.btnRerollInject = new System.Windows.Forms.Button();
            this.rbExecuteCorruptedFile = new System.Windows.Forms.RadioButton();
            this.rbNoExecution = new System.Windows.Forms.RadioButton();
            this.btnEditExec = new System.Windows.Forms.Button();
            this.lbExecution = new System.Windows.Forms.Label();
            this.cbInjectOnSelect = new System.Windows.Forms.CheckBox();
            this.btnRenameSelected = new System.Windows.Forms.Button();
            this.gbVectorEngineSettings = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.limiterMax = new System.Windows.Forms.NumericUpDown();
            this.limiterMin = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.customWholeNumbers = new System.Windows.Forms.CheckBox();
            this.valueMax = new System.Windows.Forms.NumericUpDown();
            this.valueMin = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.cbVectorAligned = new System.Windows.Forms.CheckBox();
            this.vectorOffset = new System.Windows.Forms.NumericUpDown();
            this.cbBigEndian = new System.Windows.Forms.CheckBox();
            this.cbVectorValueList = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cbVectorLimiterList = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.gbDefaultSettings = new System.Windows.Forms.GroupBox();
            this.pnCorruptionEngine = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lbStockpile = new System.Windows.Forms.ListBox();
            this.rbTargetDolphin = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStartingAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStartingAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBlastRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBlastRange)).BeginInit();
            this.gbNightmareEngineSettings.SuspendLayout();
            this.pnTargetPanel.SuspendLayout();
            this.pnBottom.SuspendLayout();
            this.gbVectorEngineSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limiterMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.limiterMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorOffset)).BeginInit();
            this.pnCorruptionEngine.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBlastTarget
            // 
            this.btnBlastTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastTarget.FlatAppearance.BorderSize = 0;
            this.btnBlastTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastTarget.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.btnBlastTarget.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnBlastTarget.Location = new System.Drawing.Point(8, 6);
            this.btnBlastTarget.Name = "btnBlastTarget";
            this.btnBlastTarget.Size = new System.Drawing.Size(118, 39);
            this.btnBlastTarget.TabIndex = 0;
            this.btnBlastTarget.TabStop = false;
            this.btnBlastTarget.Tag = "color:darker";
            this.btnBlastTarget.Text = "Blast Target";
            this.btnBlastTarget.UseVisualStyleBackColor = false;
            this.btnBlastTarget.Click += new System.EventHandler(this.btnBlastTarget_Click);
            this.btnBlastTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBlastTarget_MouseDown);
            // 
            // btnBrowseTarget
            // 
            this.btnBrowseTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBrowseTarget.FlatAppearance.BorderSize = 0;
            this.btnBrowseTarget.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseTarget.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnBrowseTarget.ForeColor = System.Drawing.Color.Gold;
            this.btnBrowseTarget.Location = new System.Drawing.Point(10, 34);
            this.btnBrowseTarget.Name = "btnBrowseTarget";
            this.btnBrowseTarget.Size = new System.Drawing.Size(71, 23);
            this.btnBrowseTarget.TabIndex = 1;
            this.btnBrowseTarget.TabStop = false;
            this.btnBrowseTarget.Tag = "color:darker";
            this.btnBrowseTarget.Text = "Browse";
            this.btnBrowseTarget.UseVisualStyleBackColor = false;
            this.btnBrowseTarget.Click += new System.EventHandler(this.btnBrowseTarget_Click);
            // 
            // lbStashHistory
            // 
            this.lbStashHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbStashHistory.BackColor = System.Drawing.Color.Gray;
            this.lbStashHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbStashHistory.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbStashHistory.ForeColor = System.Drawing.Color.White;
            this.lbStashHistory.FormattingEnabled = true;
            this.lbStashHistory.IntegralHeight = false;
            this.lbStashHistory.ItemHeight = 17;
            this.lbStashHistory.Location = new System.Drawing.Point(229, 96);
            this.lbStashHistory.Name = "lbStashHistory";
            this.lbStashHistory.ScrollAlwaysVisible = true;
            this.lbStashHistory.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbStashHistory.Size = new System.Drawing.Size(147, 216);
            this.lbStashHistory.TabIndex = 3;
            this.lbStashHistory.TabStop = false;
            this.lbStashHistory.Tag = "color:normal";
            this.lbStashHistory.SelectedIndexChanged += new System.EventHandler(this.lbStashHistory_SelectedIndexChanged);
            this.lbStashHistory.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbStashHistory_MouseDown);
            // 
            // btnLoadStockpile
            // 
            this.btnLoadStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadStockpile.BackColor = System.Drawing.Color.Orange;
            this.btnLoadStockpile.FlatAppearance.BorderSize = 0;
            this.btnLoadStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnLoadStockpile.Location = new System.Drawing.Point(571, 72);
            this.btnLoadStockpile.Name = "btnLoadStockpile";
            this.btnLoadStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnLoadStockpile.TabIndex = 4;
            this.btnLoadStockpile.TabStop = false;
            this.btnLoadStockpile.Text = "Load";
            this.btnLoadStockpile.UseVisualStyleBackColor = false;
            this.btnLoadStockpile.Click += new System.EventHandler(this.btnLoadStockpile_Click);
            // 
            // btnClearStashHistory
            // 
            this.btnClearStashHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearStashHistory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearStashHistory.FlatAppearance.BorderSize = 0;
            this.btnClearStashHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStashHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnClearStashHistory.ForeColor = System.Drawing.Color.Black;
            this.btnClearStashHistory.Location = new System.Drawing.Point(229, 315);
            this.btnClearStashHistory.Name = "btnClearStashHistory";
            this.btnClearStashHistory.Size = new System.Drawing.Size(147, 32);
            this.btnClearStashHistory.TabIndex = 5;
            this.btnClearStashHistory.TabStop = false;
            this.btnClearStashHistory.Tag = "color:light";
            this.btnClearStashHistory.Text = "Clear History";
            this.btnClearStashHistory.UseVisualStyleBackColor = false;
            this.btnClearStashHistory.Click += new System.EventHandler(this.btnClearStashHistory_Click);
            // 
            // btnSaveStockpileAs
            // 
            this.btnSaveStockpileAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStockpileAs.BackColor = System.Drawing.Color.Firebrick;
            this.btnSaveStockpileAs.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpileAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpileAs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpileAs.Location = new System.Drawing.Point(623, 72);
            this.btnSaveStockpileAs.Name = "btnSaveStockpileAs";
            this.btnSaveStockpileAs.Size = new System.Drawing.Size(59, 22);
            this.btnSaveStockpileAs.TabIndex = 6;
            this.btnSaveStockpileAs.TabStop = false;
            this.btnSaveStockpileAs.Text = "Save as";
            this.btnSaveStockpileAs.UseVisualStyleBackColor = false;
            this.btnSaveStockpileAs.Click += new System.EventHandler(this.btnSaveStockpileAs_Click);
            // 
            // btnSaveStockpile
            // 
            this.btnSaveStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveStockpile.BackColor = System.Drawing.Color.LightGray;
            this.btnSaveStockpile.Enabled = false;
            this.btnSaveStockpile.FlatAppearance.BorderSize = 0;
            this.btnSaveStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSaveStockpile.Location = new System.Drawing.Point(684, 72);
            this.btnSaveStockpile.Name = "btnSaveStockpile";
            this.btnSaveStockpile.Size = new System.Drawing.Size(44, 22);
            this.btnSaveStockpile.TabIndex = 8;
            this.btnSaveStockpile.TabStop = false;
            this.btnSaveStockpile.Text = "Save";
            this.btnSaveStockpile.UseVisualStyleBackColor = false;
            this.btnSaveStockpile.Click += new System.EventHandler(this.btnSaveStockpile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(227, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 19);
            this.label1.TabIndex = 9;
            this.label1.Text = "Stash History";
            // 
            // btnClearStockpile
            // 
            this.btnClearStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearStockpile.FlatAppearance.BorderSize = 0;
            this.btnClearStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearStockpile.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnClearStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnClearStockpile.Location = new System.Drawing.Point(419, 457);
            this.btnClearStockpile.Name = "btnClearStockpile";
            this.btnClearStockpile.Size = new System.Drawing.Size(107, 32);
            this.btnClearStockpile.TabIndex = 10;
            this.btnClearStockpile.TabStop = false;
            this.btnClearStockpile.Tag = "color:light";
            this.btnClearStockpile.Text = "Clear Stockpile";
            this.btnClearStockpile.UseVisualStyleBackColor = false;
            this.btnClearStockpile.Click += new System.EventHandler(this.btnClearStockpile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(419, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 21);
            this.label2.TabIndex = 11;
            this.label2.Text = "Stockpile Manager";
            // 
            // lbTarget
            // 
            this.lbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbTarget.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbTarget.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbTarget.ForeColor = System.Drawing.Color.Gold;
            this.lbTarget.Location = new System.Drawing.Point(85, 34);
            this.lbTarget.Name = "lbTarget";
            this.lbTarget.Padding = new System.Windows.Forms.Padding(3, 6, 1, 1);
            this.lbTarget.Size = new System.Drawing.Size(655, 23);
            this.lbTarget.TabIndex = 12;
            this.lbTarget.Tag = "color:dark";
            this.lbTarget.Text = "No target selected";
            this.lbTarget.Click += new System.EventHandler(this.lbTargetName_Click);
            // 
            // btnStockpileMoveDown
            // 
            this.btnStockpileMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileMoveDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileMoveDown.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnStockpileMoveDown.Location = new System.Drawing.Point(815, 457);
            this.btnStockpileMoveDown.Name = "btnStockpileMoveDown";
            this.btnStockpileMoveDown.Size = new System.Drawing.Size(39, 32);
            this.btnStockpileMoveDown.TabIndex = 13;
            this.btnStockpileMoveDown.TabStop = false;
            this.btnStockpileMoveDown.Tag = "color:light";
            this.btnStockpileMoveDown.Text = "▼▼";
            this.btnStockpileMoveDown.UseVisualStyleBackColor = false;
            this.btnStockpileMoveDown.Click += new System.EventHandler(this.btnStockpileMoveDown_Click);
            // 
            // btnStockpileMoveUp
            // 
            this.btnStockpileMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileMoveUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileMoveUp.FlatAppearance.BorderSize = 0;
            this.btnStockpileMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileMoveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnStockpileMoveUp.Location = new System.Drawing.Point(774, 457);
            this.btnStockpileMoveUp.Name = "btnStockpileMoveUp";
            this.btnStockpileMoveUp.Size = new System.Drawing.Size(39, 32);
            this.btnStockpileMoveUp.TabIndex = 14;
            this.btnStockpileMoveUp.TabStop = false;
            this.btnStockpileMoveUp.Tag = "color:light";
            this.btnStockpileMoveUp.Text = "▲▲";
            this.btnStockpileMoveUp.UseVisualStyleBackColor = false;
            this.btnStockpileMoveUp.Click += new System.EventHandler(this.btnStockpileMoveUp_Click);
            // 
            // nmIntensity
            // 
            this.nmIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmIntensity.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmIntensity.ForeColor = System.Drawing.Color.White;
            this.nmIntensity.Location = new System.Drawing.Point(114, 12);
            this.nmIntensity.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmIntensity.Name = "nmIntensity";
            this.nmIntensity.Size = new System.Drawing.Size(86, 25);
            this.nmIntensity.TabIndex = 15;
            this.nmIntensity.TabStop = false;
            this.nmIntensity.Tag = "color:dark";
            this.nmIntensity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nmIntensity.ValueChanged += new System.EventHandler(this.nmIntensity_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(8, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Intensity :";
            // 
            // tbIntensity
            // 
            this.tbIntensity.Location = new System.Drawing.Point(9, 41);
            this.tbIntensity.Maximum = 5000000;
            this.tbIntensity.Name = "tbIntensity";
            this.tbIntensity.Size = new System.Drawing.Size(191, 45);
            this.tbIntensity.TabIndex = 17;
            this.tbIntensity.TabStop = false;
            this.tbIntensity.TickFrequency = 0;
            this.tbIntensity.Value = 100;
            this.tbIntensity.Scroll += new System.EventHandler(this.tbIntensity_Scroll);
            // 
            // tbStartingAddress
            // 
            this.tbStartingAddress.Location = new System.Drawing.Point(9, 106);
            this.tbStartingAddress.Maximum = 100;
            this.tbStartingAddress.Name = "tbStartingAddress";
            this.tbStartingAddress.Size = new System.Drawing.Size(191, 45);
            this.tbStartingAddress.TabIndex = 20;
            this.tbStartingAddress.TabStop = false;
            this.tbStartingAddress.TickFrequency = 0;
            this.tbStartingAddress.Value = 100;
            this.tbStartingAddress.Scroll += new System.EventHandler(this.tbStartingAddress_Scroll);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Starting address:";
            // 
            // nmStartingAddress
            // 
            this.nmStartingAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmStartingAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmStartingAddress.ForeColor = System.Drawing.Color.White;
            this.nmStartingAddress.Location = new System.Drawing.Point(114, 78);
            this.nmStartingAddress.Name = "nmStartingAddress";
            this.nmStartingAddress.Size = new System.Drawing.Size(86, 25);
            this.nmStartingAddress.TabIndex = 18;
            this.nmStartingAddress.TabStop = false;
            this.nmStartingAddress.Tag = "color:dark";
            this.nmStartingAddress.ValueChanged += new System.EventHandler(this.nmStartingAddress_ValueChanged);
            // 
            // tbBlastRange
            // 
            this.tbBlastRange.Location = new System.Drawing.Point(9, 175);
            this.tbBlastRange.Maximum = 100;
            this.tbBlastRange.Name = "tbBlastRange";
            this.tbBlastRange.Size = new System.Drawing.Size(191, 45);
            this.tbBlastRange.TabIndex = 23;
            this.tbBlastRange.TabStop = false;
            this.tbBlastRange.TickFrequency = 0;
            this.tbBlastRange.Value = 100;
            this.tbBlastRange.Scroll += new System.EventHandler(this.tbBlastRange_Scroll);
            // 
            // nmBlastRange
            // 
            this.nmBlastRange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmBlastRange.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmBlastRange.ForeColor = System.Drawing.Color.White;
            this.nmBlastRange.Location = new System.Drawing.Point(114, 144);
            this.nmBlastRange.Name = "nmBlastRange";
            this.nmBlastRange.Size = new System.Drawing.Size(86, 25);
            this.nmBlastRange.TabIndex = 21;
            this.nmBlastRange.TabStop = false;
            this.nmBlastRange.Tag = "color:dark";
            this.nmBlastRange.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nmBlastRange.ValueChanged += new System.EventHandler(this.nmBlastRange_ValueChanged);
            // 
            // btnAddStashToStockpile
            // 
            this.btnAddStashToStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddStashToStockpile.FlatAppearance.BorderSize = 0;
            this.btnAddStashToStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStashToStockpile.Font = new System.Drawing.Font("Webdings", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnAddStashToStockpile.Location = new System.Drawing.Point(383, 216);
            this.btnAddStashToStockpile.Name = "btnAddStashToStockpile";
            this.btnAddStashToStockpile.Padding = new System.Windows.Forms.Padding(0, 0, 0, 13);
            this.btnAddStashToStockpile.Size = new System.Drawing.Size(28, 114);
            this.btnAddStashToStockpile.TabIndex = 24;
            this.btnAddStashToStockpile.TabStop = false;
            this.btnAddStashToStockpile.Tag = "color:light";
            this.btnAddStashToStockpile.Text = "4";
            this.btnAddStashToStockpile.UseVisualStyleBackColor = false;
            this.btnAddStashToStockpile.Click += new System.EventHandler(this.btnAddStashToStockpile_Click);
            // 
            // cbBlastRange
            // 
            this.cbBlastRange.AutoSize = true;
            this.cbBlastRange.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbBlastRange.ForeColor = System.Drawing.Color.White;
            this.cbBlastRange.Location = new System.Drawing.Point(9, 145);
            this.cbBlastRange.Name = "cbBlastRange";
            this.cbBlastRange.Size = new System.Drawing.Size(95, 21);
            this.cbBlastRange.TabIndex = 25;
            this.cbBlastRange.TabStop = false;
            this.cbBlastRange.Text = "Blast range:";
            this.cbBlastRange.UseVisualStyleBackColor = true;
            this.cbBlastRange.CheckedChanged += new System.EventHandler(this.cbBlastRange_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(12, 328);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 19);
            this.label7.TabIndex = 26;
            this.label7.Text = "Corruption Engine";
            // 
            // btnRestoreFileBackup
            // 
            this.btnRestoreFileBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestoreFileBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRestoreFileBackup.FlatAppearance.BorderSize = 0;
            this.btnRestoreFileBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestoreFileBackup.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRestoreFileBackup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnRestoreFileBackup.Location = new System.Drawing.Point(633, 7);
            this.btnRestoreFileBackup.Name = "btnRestoreFileBackup";
            this.btnRestoreFileBackup.Size = new System.Drawing.Size(107, 23);
            this.btnRestoreFileBackup.TabIndex = 27;
            this.btnRestoreFileBackup.TabStop = false;
            this.btnRestoreFileBackup.Tag = "color:darker";
            this.btnRestoreFileBackup.Text = "Restore Backup";
            this.btnRestoreFileBackup.UseVisualStyleBackColor = false;
            this.btnRestoreFileBackup.Click += new System.EventHandler(this.btnRestoreFileBackup_Click);
            // 
            // gbNightmareEngineSettings
            // 
            this.gbNightmareEngineSettings.Controls.Add(this.label3);
            this.gbNightmareEngineSettings.Controls.Add(this.cbBlastType);
            this.gbNightmareEngineSettings.ForeColor = System.Drawing.Color.White;
            this.gbNightmareEngineSettings.Location = new System.Drawing.Point(885, 228);
            this.gbNightmareEngineSettings.Name = "gbNightmareEngineSettings";
            this.gbNightmareEngineSettings.Size = new System.Drawing.Size(345, 130);
            this.gbNightmareEngineSettings.TabIndex = 28;
            this.gbNightmareEngineSettings.TabStop = false;
            this.gbNightmareEngineSettings.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 41;
            this.label3.Text = "Blast Type:";
            // 
            // cbBlastType
            // 
            this.cbBlastType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlastType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastType.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbBlastType.ForeColor = System.Drawing.Color.White;
            this.cbBlastType.FormattingEnabled = true;
            this.cbBlastType.Items.AddRange(new object[] {
            "RANDOM",
            "RANDOMTILT",
            "TILT"});
            this.cbBlastType.Location = new System.Drawing.Point(77, 20);
            this.cbBlastType.Name = "cbBlastType";
            this.cbBlastType.Size = new System.Drawing.Size(97, 25);
            this.cbBlastType.TabIndex = 40;
            this.cbBlastType.TabStop = false;
            this.cbBlastType.Tag = "color:dark";
            this.cbBlastType.SelectedIndexChanged += new System.EventHandler(this.cbBlastType_SelectedIndexChanged);
            // 
            // cbCorruptionEngine
            // 
            this.cbCorruptionEngine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbCorruptionEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCorruptionEngine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbCorruptionEngine.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbCorruptionEngine.ForeColor = System.Drawing.Color.White;
            this.cbCorruptionEngine.FormattingEnabled = true;
            this.cbCorruptionEngine.Items.AddRange(new object[] {
            "Nightmare Engine",
            "Vector Engine"});
            this.cbCorruptionEngine.Location = new System.Drawing.Point(10, 12);
            this.cbCorruptionEngine.Name = "cbCorruptionEngine";
            this.cbCorruptionEngine.Size = new System.Drawing.Size(345, 25);
            this.cbCorruptionEngine.TabIndex = 29;
            this.cbCorruptionEngine.TabStop = false;
            this.cbCorruptionEngine.Tag = "color:dark";
            this.cbCorruptionEngine.SelectedIndexChanged += new System.EventHandler(this.cbCorruptionEngine_SelectedIndexChanged);
            // 
            // rbTargetFile
            // 
            this.rbTargetFile.AutoSize = true;
            this.rbTargetFile.Checked = true;
            this.rbTargetFile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbTargetFile.ForeColor = System.Drawing.Color.White;
            this.rbTargetFile.Location = new System.Drawing.Point(9, 9);
            this.rbTargetFile.Name = "rbTargetFile";
            this.rbTargetFile.Size = new System.Drawing.Size(77, 17);
            this.rbTargetFile.TabIndex = 30;
            this.rbTargetFile.TabStop = true;
            this.rbTargetFile.Text = "Target File";
            this.rbTargetFile.UseVisualStyleBackColor = true;
            this.rbTargetFile.CheckedChanged += new System.EventHandler(this.rbTargetFile_CheckedChanged);
            // 
            // rbTargetProcess
            // 
            this.rbTargetProcess.AutoSize = true;
            this.rbTargetProcess.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbTargetProcess.ForeColor = System.Drawing.Color.White;
            this.rbTargetProcess.Location = new System.Drawing.Point(324, 9);
            this.rbTargetProcess.Name = "rbTargetProcess";
            this.rbTargetProcess.Size = new System.Drawing.Size(180, 17);
            this.rbTargetProcess.TabIndex = 31;
            this.rbTargetProcess.Text = "Target Process (EXPERIMENTAL)";
            this.rbTargetProcess.UseVisualStyleBackColor = true;
            this.rbTargetProcess.CheckedChanged += new System.EventHandler(this.rbTargetProcess_CheckedChanged);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveSelected.FlatAppearance.BorderSize = 0;
            this.btnRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRemoveSelected.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveSelected.Location = new System.Drawing.Point(540, 457);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(117, 32);
            this.btnRemoveSelected.TabIndex = 33;
            this.btnRemoveSelected.TabStop = false;
            this.btnRemoveSelected.Tag = "color:light";
            this.btnRemoveSelected.Text = "Remove Item";
            this.btnRemoveSelected.UseVisualStyleBackColor = false;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnImportStockpile
            // 
            this.btnImportStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnImportStockpile.FlatAppearance.BorderSize = 0;
            this.btnImportStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportStockpile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnImportStockpile.Location = new System.Drawing.Point(739, 72);
            this.btnImportStockpile.Name = "btnImportStockpile";
            this.btnImportStockpile.Size = new System.Drawing.Size(50, 22);
            this.btnImportStockpile.TabIndex = 34;
            this.btnImportStockpile.TabStop = false;
            this.btnImportStockpile.Text = "Import";
            this.btnImportStockpile.UseVisualStyleBackColor = false;
            this.btnImportStockpile.Click += new System.EventHandler(this.btnImportStockpile_Click);
            // 
            // btnStockpileUp
            // 
            this.btnStockpileUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileUp.FlatAppearance.BorderSize = 0;
            this.btnStockpileUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileUp.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileUp.Location = new System.Drawing.Point(802, 71);
            this.btnStockpileUp.Name = "btnStockpileUp";
            this.btnStockpileUp.Size = new System.Drawing.Size(25, 23);
            this.btnStockpileUp.TabIndex = 36;
            this.btnStockpileUp.TabStop = false;
            this.btnStockpileUp.Tag = "color:light";
            this.btnStockpileUp.Text = "▲";
            this.btnStockpileUp.UseVisualStyleBackColor = false;
            this.btnStockpileUp.Click += new System.EventHandler(this.btnStockpileUp_Click);
            // 
            // btnStockpileDown
            // 
            this.btnStockpileDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStockpileDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStockpileDown.FlatAppearance.BorderSize = 0;
            this.btnStockpileDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStockpileDown.ForeColor = System.Drawing.Color.Black;
            this.btnStockpileDown.Location = new System.Drawing.Point(829, 71);
            this.btnStockpileDown.Name = "btnStockpileDown";
            this.btnStockpileDown.Size = new System.Drawing.Size(25, 23);
            this.btnStockpileDown.TabIndex = 35;
            this.btnStockpileDown.TabStop = false;
            this.btnStockpileDown.Tag = "color:light";
            this.btnStockpileDown.Text = "▼";
            this.btnStockpileDown.UseVisualStyleBackColor = false;
            this.btnStockpileDown.Click += new System.EventHandler(this.btnStockpileDown_Click);
            // 
            // btnStashHistoryUp
            // 
            this.btnStashHistoryUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStashHistoryUp.FlatAppearance.BorderSize = 0;
            this.btnStashHistoryUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStashHistoryUp.ForeColor = System.Drawing.Color.Black;
            this.btnStashHistoryUp.Location = new System.Drawing.Point(324, 71);
            this.btnStashHistoryUp.Name = "btnStashHistoryUp";
            this.btnStashHistoryUp.Size = new System.Drawing.Size(25, 23);
            this.btnStashHistoryUp.TabIndex = 38;
            this.btnStashHistoryUp.TabStop = false;
            this.btnStashHistoryUp.Tag = "color:light";
            this.btnStashHistoryUp.Text = "▲";
            this.btnStashHistoryUp.UseVisualStyleBackColor = false;
            this.btnStashHistoryUp.Click += new System.EventHandler(this.btnStashHistoryUp_Click);
            // 
            // btnStashHistoryDown
            // 
            this.btnStashHistoryDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStashHistoryDown.FlatAppearance.BorderSize = 0;
            this.btnStashHistoryDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStashHistoryDown.ForeColor = System.Drawing.Color.Black;
            this.btnStashHistoryDown.Location = new System.Drawing.Point(351, 71);
            this.btnStashHistoryDown.Name = "btnStashHistoryDown";
            this.btnStashHistoryDown.Size = new System.Drawing.Size(25, 23);
            this.btnStashHistoryDown.TabIndex = 37;
            this.btnStashHistoryDown.TabStop = false;
            this.btnStashHistoryDown.Tag = "color:light";
            this.btnStashHistoryDown.Text = "▼";
            this.btnStashHistoryDown.UseVisualStyleBackColor = false;
            this.btnStashHistoryDown.Click += new System.EventHandler(this.btnStashHistoryDown_Click);
            // 
            // pnTargetPanel
            // 
            this.pnTargetPanel.BackColor = System.Drawing.Color.Gray;
            this.pnTargetPanel.Controls.Add(this.rbTargetDolphin);
            this.pnTargetPanel.Controls.Add(this.savestateInfoButton);
            this.pnTargetPanel.Controls.Add(this.rbTargetMultipleFiles);
            this.pnTargetPanel.Controls.Add(this.btnClearAllBackups);
            this.pnTargetPanel.Controls.Add(this.btnResetBackup);
            this.pnTargetPanel.Controls.Add(this.rbTargetFile);
            this.pnTargetPanel.Controls.Add(this.rbTargetProcess);
            this.pnTargetPanel.Controls.Add(this.btnBrowseTarget);
            this.pnTargetPanel.Controls.Add(this.lbTarget);
            this.pnTargetPanel.Controls.Add(this.btnRestoreFileBackup);
            this.pnTargetPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTargetPanel.Location = new System.Drawing.Point(0, 0);
            this.pnTargetPanel.Name = "pnTargetPanel";
            this.pnTargetPanel.Size = new System.Drawing.Size(864, 64);
            this.pnTargetPanel.TabIndex = 39;
            this.pnTargetPanel.Tag = "color:normal";
            // 
            // savestateInfoButton
            // 
            this.savestateInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.savestateInfoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.savestateInfoButton.FlatAppearance.BorderSize = 0;
            this.savestateInfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.savestateInfoButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.savestateInfoButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.savestateInfoButton.Location = new System.Drawing.Point(502, 7);
            this.savestateInfoButton.Name = "savestateInfoButton";
            this.savestateInfoButton.Size = new System.Drawing.Size(125, 23);
            this.savestateInfoButton.TabIndex = 35;
            this.savestateInfoButton.TabStop = false;
            this.savestateInfoButton.Tag = "color:darker";
            this.savestateInfoButton.Text = "Dolphin Mod Tools";
            this.savestateInfoButton.UseVisualStyleBackColor = false;
            this.savestateInfoButton.Click += new System.EventHandler(this.savestateInfoButton_Click);
            // 
            // rbTargetMultipleFiles
            // 
            this.rbTargetMultipleFiles.AutoSize = true;
            this.rbTargetMultipleFiles.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbTargetMultipleFiles.ForeColor = System.Drawing.Color.White;
            this.rbTargetMultipleFiles.Location = new System.Drawing.Point(90, 9);
            this.rbTargetMultipleFiles.Name = "rbTargetMultipleFiles";
            this.rbTargetMultipleFiles.Size = new System.Drawing.Size(128, 17);
            this.rbTargetMultipleFiles.TabIndex = 34;
            this.rbTargetMultipleFiles.Text = "Target Multiple Files";
            this.rbTargetMultipleFiles.UseVisualStyleBackColor = true;
            this.rbTargetMultipleFiles.CheckedChanged += new System.EventHandler(this.rbTargetMultipleFiles_CheckedChanged);
            // 
            // btnClearAllBackups
            // 
            this.btnClearAllBackups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAllBackups.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnClearAllBackups.FlatAppearance.BorderSize = 0;
            this.btnClearAllBackups.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearAllBackups.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearAllBackups.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnClearAllBackups.Location = new System.Drawing.Point(745, 34);
            this.btnClearAllBackups.Name = "btnClearAllBackups";
            this.btnClearAllBackups.Size = new System.Drawing.Size(108, 23);
            this.btnClearAllBackups.TabIndex = 33;
            this.btnClearAllBackups.TabStop = false;
            this.btnClearAllBackups.Tag = "color:darker";
            this.btnClearAllBackups.Text = "Clear all Backups";
            this.btnClearAllBackups.UseVisualStyleBackColor = false;
            this.btnClearAllBackups.Click += new System.EventHandler(this.btnClearAllBackups_Click);
            // 
            // btnResetBackup
            // 
            this.btnResetBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnResetBackup.FlatAppearance.BorderSize = 0;
            this.btnResetBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetBackup.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnResetBackup.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnResetBackup.Location = new System.Drawing.Point(745, 7);
            this.btnResetBackup.Name = "btnResetBackup";
            this.btnResetBackup.Size = new System.Drawing.Size(108, 23);
            this.btnResetBackup.TabIndex = 32;
            this.btnResetBackup.TabStop = false;
            this.btnResetBackup.Tag = "color:darker";
            this.btnResetBackup.Text = "Reset Backup";
            this.btnResetBackup.UseVisualStyleBackColor = false;
            this.btnResetBackup.Click += new System.EventHandler(this.btnResetBackup_Click);
            // 
            // cbWriteCopyMode
            // 
            this.cbWriteCopyMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbWriteCopyMode.AutoSize = true;
            this.cbWriteCopyMode.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbWriteCopyMode.ForeColor = System.Drawing.Color.White;
            this.cbWriteCopyMode.Location = new System.Drawing.Point(350, 57);
            this.cbWriteCopyMode.Name = "cbWriteCopyMode";
            this.cbWriteCopyMode.Size = new System.Drawing.Size(126, 17);
            this.cbWriteCopyMode.TabIndex = 34;
            this.cbWriteCopyMode.TabStop = false;
            this.cbWriteCopyMode.Text = "Write in copy mode";
            this.cbWriteCopyMode.UseVisualStyleBackColor = true;
            this.cbWriteCopyMode.CheckedChanged += new System.EventHandler(this.cbWriteCopyMode_CheckedChanged);
            // 
            // btnInjectSelected
            // 
            this.btnInjectSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnInjectSelected.FlatAppearance.BorderSize = 0;
            this.btnInjectSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInjectSelected.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnInjectSelected.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnInjectSelected.Location = new System.Drawing.Point(129, 7);
            this.btnInjectSelected.Name = "btnInjectSelected";
            this.btnInjectSelected.Size = new System.Drawing.Size(100, 38);
            this.btnInjectSelected.TabIndex = 40;
            this.btnInjectSelected.TabStop = false;
            this.btnInjectSelected.Tag = "color:darker";
            this.btnInjectSelected.Text = "Inject selected item";
            this.btnInjectSelected.UseVisualStyleBackColor = false;
            this.btnInjectSelected.Click += new System.EventHandler(this.btnInjectSelected_Click);
            // 
            // btnDisableAutoUncorrupt
            // 
            this.btnDisableAutoUncorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnDisableAutoUncorrupt.FlatAppearance.BorderSize = 0;
            this.btnDisableAutoUncorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisableAutoUncorrupt.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnDisableAutoUncorrupt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnDisableAutoUncorrupt.Location = new System.Drawing.Point(84, 11);
            this.btnDisableAutoUncorrupt.Name = "btnDisableAutoUncorrupt";
            this.btnDisableAutoUncorrupt.Size = new System.Drawing.Size(142, 22);
            this.btnDisableAutoUncorrupt.TabIndex = 36;
            this.btnDisableAutoUncorrupt.TabStop = false;
            this.btnDisableAutoUncorrupt.Tag = "color:darker";
            this.btnDisableAutoUncorrupt.Text = "Disable Auto-Uncorrupt";
            this.btnDisableAutoUncorrupt.UseVisualStyleBackColor = false;
            this.btnDisableAutoUncorrupt.Click += new System.EventHandler(this.btnDisableAutoUncorrupt_Click);
            // 
            // btnEnableCaching
            // 
            this.btnEnableCaching.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEnableCaching.FlatAppearance.BorderSize = 0;
            this.btnEnableCaching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableCaching.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnEnableCaching.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnEnableCaching.Location = new System.Drawing.Point(235, 11);
            this.btnEnableCaching.Name = "btnEnableCaching";
            this.btnEnableCaching.Size = new System.Drawing.Size(189, 21);
            this.btnEnableCaching.TabIndex = 35;
            this.btnEnableCaching.TabStop = false;
            this.btnEnableCaching.Tag = "color:darker";
            this.btnEnableCaching.Text = "Enable caching on current target";
            this.btnEnableCaching.UseVisualStyleBackColor = false;
            this.btnEnableCaching.Click += new System.EventHandler(this.btnEnableCaching_Click);
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.Color.Gray;
            this.pnBottom.Controls.Add(this.btnKillProcess);
            this.pnBottom.Controls.Add(this.lbArgs);
            this.pnBottom.Controls.Add(this.tbArgs);
            this.pnBottom.Controls.Add(this.btnBlastUntilEffect);
            this.pnBottom.Controls.Add(this.cbTerminateOnReExec);
            this.pnBottom.Controls.Add(this.cbWriteCopyMode);
            this.pnBottom.Controls.Add(this.rbExecuteWith);
            this.pnBottom.Controls.Add(this.rbExecuteScript);
            this.pnBottom.Controls.Add(this.rbExecuteOtherProgram);
            this.pnBottom.Controls.Add(this.btnRerollInject);
            this.pnBottom.Controls.Add(this.rbExecuteCorruptedFile);
            this.pnBottom.Controls.Add(this.rbNoExecution);
            this.pnBottom.Controls.Add(this.btnEditExec);
            this.pnBottom.Controls.Add(this.lbExecution);
            this.pnBottom.Controls.Add(this.cbInjectOnSelect);
            this.pnBottom.Controls.Add(this.btnBlastTarget);
            this.pnBottom.Controls.Add(this.btnInjectSelected);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 545);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(864, 80);
            this.pnBottom.TabIndex = 40;
            this.pnBottom.Tag = "color:normal";
            // 
            // btnKillProcess
            // 
            this.btnKillProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKillProcess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnKillProcess.FlatAppearance.BorderSize = 0;
            this.btnKillProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKillProcess.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnKillProcess.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnKillProcess.Location = new System.Drawing.Point(764, 29);
            this.btnKillProcess.Name = "btnKillProcess";
            this.btnKillProcess.Size = new System.Drawing.Size(87, 23);
            this.btnKillProcess.TabIndex = 55;
            this.btnKillProcess.TabStop = false;
            this.btnKillProcess.Tag = "color:darker";
            this.btnKillProcess.Text = "Kill Process";
            this.btnKillProcess.UseVisualStyleBackColor = false;
            this.btnKillProcess.Click += new System.EventHandler(this.btnKillProcess_Click);
            // 
            // lbArgs
            // 
            this.lbArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbArgs.AutoSize = true;
            this.lbArgs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbArgs.ForeColor = System.Drawing.Color.White;
            this.lbArgs.Location = new System.Drawing.Point(690, 57);
            this.lbArgs.Name = "lbArgs";
            this.lbArgs.Size = new System.Drawing.Size(29, 13);
            this.lbArgs.TabIndex = 54;
            this.lbArgs.Text = "args";
            this.lbArgs.Visible = false;
            // 
            // tbArgs
            // 
            this.tbArgs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbArgs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbArgs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbArgs.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbArgs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbArgs.Location = new System.Drawing.Point(720, 58);
            this.tbArgs.Name = "tbArgs";
            this.tbArgs.Size = new System.Drawing.Size(131, 15);
            this.tbArgs.TabIndex = 53;
            this.tbArgs.Visible = false;
            // 
            // btnBlastUntilEffect
            // 
            this.btnBlastUntilEffect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastUntilEffect.FlatAppearance.BorderSize = 0;
            this.btnBlastUntilEffect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastUntilEffect.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBlastUntilEffect.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnBlastUntilEffect.Location = new System.Drawing.Point(8, 48);
            this.btnBlastUntilEffect.Name = "btnBlastUntilEffect";
            this.btnBlastUntilEffect.Size = new System.Drawing.Size(118, 24);
            this.btnBlastUntilEffect.TabIndex = 50;
            this.btnBlastUntilEffect.TabStop = false;
            this.btnBlastUntilEffect.Tag = "color:darker";
            this.btnBlastUntilEffect.Text = "Blast until effect";
            this.btnBlastUntilEffect.UseVisualStyleBackColor = false;
            this.btnBlastUntilEffect.Click += new System.EventHandler(this.btnBlastUntilEffect_Click);
            this.btnBlastUntilEffect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBlastUntilEffect_MouseDown);
            // 
            // cbTerminateOnReExec
            // 
            this.cbTerminateOnReExec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTerminateOnReExec.AutoSize = true;
            this.cbTerminateOnReExec.Checked = true;
            this.cbTerminateOnReExec.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTerminateOnReExec.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbTerminateOnReExec.ForeColor = System.Drawing.Color.White;
            this.cbTerminateOnReExec.Location = new System.Drawing.Point(483, 57);
            this.cbTerminateOnReExec.Name = "cbTerminateOnReExec";
            this.cbTerminateOnReExec.Size = new System.Drawing.Size(196, 17);
            this.cbTerminateOnReExec.TabIndex = 48;
            this.cbTerminateOnReExec.TabStop = false;
            this.cbTerminateOnReExec.Text = "Terminate Target on Re-Execution";
            this.cbTerminateOnReExec.UseVisualStyleBackColor = true;
            // 
            // rbExecuteWith
            // 
            this.rbExecuteWith.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbExecuteWith.AutoSize = true;
            this.rbExecuteWith.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbExecuteWith.ForeColor = System.Drawing.Color.White;
            this.rbExecuteWith.Location = new System.Drawing.Point(494, 7);
            this.rbExecuteWith.Name = "rbExecuteWith";
            this.rbExecuteWith.Size = new System.Drawing.Size(90, 17);
            this.rbExecuteWith.TabIndex = 47;
            this.rbExecuteWith.Text = "Execute with";
            this.rbExecuteWith.UseVisualStyleBackColor = true;
            // 
            // rbExecuteScript
            // 
            this.rbExecuteScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbExecuteScript.AutoSize = true;
            this.rbExecuteScript.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbExecuteScript.ForeColor = System.Drawing.Color.White;
            this.rbExecuteScript.Location = new System.Drawing.Point(737, 7);
            this.rbExecuteScript.Name = "rbExecuteScript";
            this.rbExecuteScript.Size = new System.Drawing.Size(54, 17);
            this.rbExecuteScript.TabIndex = 46;
            this.rbExecuteScript.Text = "Script";
            this.rbExecuteScript.UseVisualStyleBackColor = true;
            this.rbExecuteScript.Visible = false;
            this.rbExecuteScript.CheckedChanged += new System.EventHandler(this.rbExecuteScript_CheckedChanged);
            // 
            // rbExecuteOtherProgram
            // 
            this.rbExecuteOtherProgram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbExecuteOtherProgram.AutoSize = true;
            this.rbExecuteOtherProgram.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbExecuteOtherProgram.ForeColor = System.Drawing.Color.White;
            this.rbExecuteOtherProgram.Location = new System.Drawing.Point(590, 7);
            this.rbExecuteOtherProgram.Name = "rbExecuteOtherProgram";
            this.rbExecuteOtherProgram.Size = new System.Drawing.Size(142, 17);
            this.rbExecuteOtherProgram.TabIndex = 45;
            this.rbExecuteOtherProgram.Text = "Execute other program";
            this.rbExecuteOtherProgram.UseVisualStyleBackColor = true;
            this.rbExecuteOtherProgram.CheckedChanged += new System.EventHandler(this.rbExecuteOtherProgram_CheckedChanged);
            // 
            // btnRerollInject
            // 
            this.btnRerollInject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnRerollInject.FlatAppearance.BorderSize = 0;
            this.btnRerollInject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRerollInject.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRerollInject.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnRerollInject.Location = new System.Drawing.Point(129, 48);
            this.btnRerollInject.Name = "btnRerollInject";
            this.btnRerollInject.Size = new System.Drawing.Size(100, 24);
            this.btnRerollInject.TabIndex = 49;
            this.btnRerollInject.TabStop = false;
            this.btnRerollInject.Tag = "color:darker";
            this.btnRerollInject.Text = "Reroll + Inject";
            this.btnRerollInject.UseVisualStyleBackColor = false;
            this.btnRerollInject.Click += new System.EventHandler(this.btnRerollInject_Click);
            // 
            // rbExecuteCorruptedFile
            // 
            this.rbExecuteCorruptedFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbExecuteCorruptedFile.AutoSize = true;
            this.rbExecuteCorruptedFile.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbExecuteCorruptedFile.ForeColor = System.Drawing.Color.White;
            this.rbExecuteCorruptedFile.Location = new System.Drawing.Point(350, 7);
            this.rbExecuteCorruptedFile.Name = "rbExecuteCorruptedFile";
            this.rbExecuteCorruptedFile.Size = new System.Drawing.Size(137, 17);
            this.rbExecuteCorruptedFile.TabIndex = 44;
            this.rbExecuteCorruptedFile.Text = "Execute corrupted file";
            this.rbExecuteCorruptedFile.UseVisualStyleBackColor = true;
            this.rbExecuteCorruptedFile.CheckedChanged += new System.EventHandler(this.rbExecuteCorruptedFile_CheckedChanged);
            // 
            // rbNoExecution
            // 
            this.rbNoExecution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbNoExecution.AutoSize = true;
            this.rbNoExecution.Checked = true;
            this.rbNoExecution.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbNoExecution.ForeColor = System.Drawing.Color.White;
            this.rbNoExecution.Location = new System.Drawing.Point(245, 7);
            this.rbNoExecution.Name = "rbNoExecution";
            this.rbNoExecution.Size = new System.Drawing.Size(93, 17);
            this.rbNoExecution.TabIndex = 43;
            this.rbNoExecution.TabStop = true;
            this.rbNoExecution.Text = "No execution";
            this.rbNoExecution.UseVisualStyleBackColor = true;
            this.rbNoExecution.CheckedChanged += new System.EventHandler(this.rbNoExecution_CheckedChanged);
            // 
            // btnEditExec
            // 
            this.btnEditExec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditExec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnEditExec.FlatAppearance.BorderSize = 0;
            this.btnEditExec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditExec.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnEditExec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnEditExec.Location = new System.Drawing.Point(690, 29);
            this.btnEditExec.Name = "btnEditExec";
            this.btnEditExec.Size = new System.Drawing.Size(67, 23);
            this.btnEditExec.TabIndex = 35;
            this.btnEditExec.TabStop = false;
            this.btnEditExec.Tag = "color:darker";
            this.btnEditExec.Text = "Edit Exec";
            this.btnEditExec.UseVisualStyleBackColor = false;
            this.btnEditExec.Click += new System.EventHandler(this.btnEditExec_Click);
            // 
            // lbExecution
            // 
            this.lbExecution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbExecution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbExecution.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbExecution.ForeColor = System.Drawing.Color.Gold;
            this.lbExecution.Location = new System.Drawing.Point(242, 29);
            this.lbExecution.Name = "lbExecution";
            this.lbExecution.Padding = new System.Windows.Forms.Padding(2, 5, 1, 1);
            this.lbExecution.Size = new System.Drawing.Size(440, 23);
            this.lbExecution.TabIndex = 42;
            this.lbExecution.Tag = "color:dark";
            this.lbExecution.Text = "No execution set";
            // 
            // cbInjectOnSelect
            // 
            this.cbInjectOnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbInjectOnSelect.AutoSize = true;
            this.cbInjectOnSelect.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbInjectOnSelect.ForeColor = System.Drawing.Color.White;
            this.cbInjectOnSelect.Location = new System.Drawing.Point(243, 57);
            this.cbInjectOnSelect.Name = "cbInjectOnSelect";
            this.cbInjectOnSelect.Size = new System.Drawing.Size(103, 17);
            this.cbInjectOnSelect.TabIndex = 41;
            this.cbInjectOnSelect.TabStop = false;
            this.cbInjectOnSelect.Text = "Inject on select";
            this.cbInjectOnSelect.UseVisualStyleBackColor = true;
            // 
            // btnRenameSelected
            // 
            this.btnRenameSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenameSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRenameSelected.FlatAppearance.BorderSize = 0;
            this.btnRenameSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnRenameSelected.ForeColor = System.Drawing.Color.Black;
            this.btnRenameSelected.Location = new System.Drawing.Point(659, 457);
            this.btnRenameSelected.Name = "btnRenameSelected";
            this.btnRenameSelected.Size = new System.Drawing.Size(99, 32);
            this.btnRenameSelected.TabIndex = 42;
            this.btnRenameSelected.TabStop = false;
            this.btnRenameSelected.Tag = "color:light";
            this.btnRenameSelected.Text = "Rename Item";
            this.btnRenameSelected.UseVisualStyleBackColor = false;
            this.btnRenameSelected.Click += new System.EventHandler(this.btnRenameSelected_Click);
            // 
            // gbVectorEngineSettings
            // 
            this.gbVectorEngineSettings.Controls.Add(this.label14);
            this.gbVectorEngineSettings.Controls.Add(this.label15);
            this.gbVectorEngineSettings.Controls.Add(this.limiterMax);
            this.gbVectorEngineSettings.Controls.Add(this.limiterMin);
            this.gbVectorEngineSettings.Controls.Add(this.label12);
            this.gbVectorEngineSettings.Controls.Add(this.label11);
            this.gbVectorEngineSettings.Controls.Add(this.customWholeNumbers);
            this.gbVectorEngineSettings.Controls.Add(this.valueMax);
            this.gbVectorEngineSettings.Controls.Add(this.valueMin);
            this.gbVectorEngineSettings.Controls.Add(this.label10);
            this.gbVectorEngineSettings.Controls.Add(this.cbVectorAligned);
            this.gbVectorEngineSettings.Controls.Add(this.vectorOffset);
            this.gbVectorEngineSettings.Controls.Add(this.cbBigEndian);
            this.gbVectorEngineSettings.Controls.Add(this.cbVectorValueList);
            this.gbVectorEngineSettings.Controls.Add(this.label18);
            this.gbVectorEngineSettings.Controls.Add(this.cbVectorLimiterList);
            this.gbVectorEngineSettings.Controls.Add(this.label13);
            this.gbVectorEngineSettings.ForeColor = System.Drawing.Color.White;
            this.gbVectorEngineSettings.Location = new System.Drawing.Point(885, 85);
            this.gbVectorEngineSettings.Name = "gbVectorEngineSettings";
            this.gbVectorEngineSettings.Size = new System.Drawing.Size(345, 130);
            this.gbVectorEngineSettings.TabIndex = 42;
            this.gbVectorEngineSettings.TabStop = false;
            this.gbVectorEngineSettings.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label14.Location = new System.Drawing.Point(265, 54);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 17);
            this.label14.TabIndex = 100;
            this.label14.Text = "Limiter Max";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label15.Location = new System.Drawing.Point(265, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 17);
            this.label15.TabIndex = 99;
            this.label15.Text = "Limiter Min";
            // 
            // limiterMax
            // 
            this.limiterMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.limiterMax.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.limiterMax.ForeColor = System.Drawing.Color.White;
            this.limiterMax.Location = new System.Drawing.Point(268, 73);
            this.limiterMax.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.limiterMax.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.limiterMax.Name = "limiterMax";
            this.limiterMax.Size = new System.Drawing.Size(62, 25);
            this.limiterMax.TabIndex = 98;
            this.limiterMax.Tag = "color:dark";
            this.limiterMax.ValueChanged += new System.EventHandler(this.limiterMax_ValueChanged);
            // 
            // limiterMin
            // 
            this.limiterMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.limiterMin.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.limiterMin.ForeColor = System.Drawing.Color.White;
            this.limiterMin.Location = new System.Drawing.Point(268, 28);
            this.limiterMin.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.limiterMin.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.limiterMin.Name = "limiterMin";
            this.limiterMin.Size = new System.Drawing.Size(62, 25);
            this.limiterMin.TabIndex = 97;
            this.limiterMin.Tag = "color:dark";
            this.limiterMin.ValueChanged += new System.EventHandler(this.limiterMin_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label12.Location = new System.Drawing.Point(188, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 17);
            this.label12.TabIndex = 96;
            this.label12.Text = "Value Max";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label11.Location = new System.Drawing.Point(188, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 17);
            this.label11.TabIndex = 95;
            this.label11.Text = "Value Min";
            // 
            // customWholeNumbers
            // 
            this.customWholeNumbers.AutoSize = true;
            this.customWholeNumbers.Checked = true;
            this.customWholeNumbers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customWholeNumbers.Cursor = System.Windows.Forms.Cursors.Default;
            this.customWholeNumbers.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.customWholeNumbers.ForeColor = System.Drawing.SystemColors.Window;
            this.customWholeNumbers.Location = new System.Drawing.Point(191, 105);
            this.customWholeNumbers.Name = "customWholeNumbers";
            this.customWholeNumbers.Size = new System.Drawing.Size(152, 21);
            this.customWholeNumbers.TabIndex = 94;
            this.customWholeNumbers.TabStop = false;
            this.customWholeNumbers.Text = "Whole Numbers Only";
            this.customWholeNumbers.UseVisualStyleBackColor = true;
            this.customWholeNumbers.CheckedChanged += new System.EventHandler(this.customWholeNumbers_CheckedChanged);
            // 
            // valueMax
            // 
            this.valueMax.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.valueMax.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.valueMax.ForeColor = System.Drawing.Color.White;
            this.valueMax.Location = new System.Drawing.Point(191, 73);
            this.valueMax.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.valueMax.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.valueMax.Name = "valueMax";
            this.valueMax.Size = new System.Drawing.Size(62, 25);
            this.valueMax.TabIndex = 91;
            this.valueMax.Tag = "color:dark";
            this.valueMax.ValueChanged += new System.EventHandler(this.valueMax_ValueChanged);
            // 
            // valueMin
            // 
            this.valueMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.valueMin.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.valueMin.ForeColor = System.Drawing.Color.White;
            this.valueMin.Location = new System.Drawing.Point(191, 28);
            this.valueMin.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.valueMin.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.valueMin.Name = "valueMin";
            this.valueMin.Size = new System.Drawing.Size(62, 25);
            this.valueMin.TabIndex = 90;
            this.valueMin.Tag = "color:dark";
            this.valueMin.ValueChanged += new System.EventHandler(this.valueMin_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label10.Location = new System.Drawing.Point(121, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 17);
            this.label10.TabIndex = 89;
            this.label10.Text = "Alignment";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // cbVectorAligned
            // 
            this.cbVectorAligned.AutoSize = true;
            this.cbVectorAligned.Checked = true;
            this.cbVectorAligned.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVectorAligned.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbVectorAligned.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbVectorAligned.ForeColor = System.Drawing.Color.White;
            this.cbVectorAligned.Location = new System.Drawing.Point(103, 103);
            this.cbVectorAligned.Name = "cbVectorAligned";
            this.cbVectorAligned.Size = new System.Drawing.Size(71, 21);
            this.cbVectorAligned.TabIndex = 88;
            this.cbVectorAligned.TabStop = false;
            this.cbVectorAligned.Text = "Aligned";
            this.cbVectorAligned.UseVisualStyleBackColor = true;
            this.cbVectorAligned.CheckedChanged += new System.EventHandler(this.cbVectorAligned_CheckedChanged);
            // 
            // vectorOffset
            // 
            this.vectorOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.vectorOffset.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.vectorOffset.ForeColor = System.Drawing.Color.White;
            this.vectorOffset.Location = new System.Drawing.Point(136, 28);
            this.vectorOffset.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.vectorOffset.Name = "vectorOffset";
            this.vectorOffset.Size = new System.Drawing.Size(34, 25);
            this.vectorOffset.TabIndex = 87;
            this.vectorOffset.Tag = "color:dark";
            this.vectorOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.vectorOffset.ValueChanged += new System.EventHandler(this.vectorOffset_ValueChanged);
            // 
            // cbBigEndian
            // 
            this.cbBigEndian.AutoSize = true;
            this.cbBigEndian.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbBigEndian.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbBigEndian.ForeColor = System.Drawing.Color.White;
            this.cbBigEndian.Location = new System.Drawing.Point(16, 103);
            this.cbBigEndian.Name = "cbBigEndian";
            this.cbBigEndian.Size = new System.Drawing.Size(88, 21);
            this.cbBigEndian.TabIndex = 86;
            this.cbBigEndian.TabStop = false;
            this.cbBigEndian.Text = "Big Endian";
            this.cbBigEndian.UseVisualStyleBackColor = true;
            this.cbBigEndian.CheckedChanged += new System.EventHandler(this.cbBigEndian_CheckedChanged);
            // 
            // cbVectorValueList
            // 
            this.cbVectorValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbVectorValueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVectorValueList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbVectorValueList.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbVectorValueList.ForeColor = System.Drawing.Color.White;
            this.cbVectorValueList.FormattingEnabled = true;
            this.cbVectorValueList.Items.AddRange(new object[] {
            "Extended",
            "Extended+",
            "Extended-",
            "SuperExtended",
            "Whole",
            "Whole+",
            "Tiny",
            "One",
            "One*",
            "Two",
            "Custom",
            "AnyFloat"});
            this.cbVectorValueList.Location = new System.Drawing.Point(16, 73);
            this.cbVectorValueList.Name = "cbVectorValueList";
            this.cbVectorValueList.Size = new System.Drawing.Size(106, 25);
            this.cbVectorValueList.TabIndex = 85;
            this.cbVectorValueList.TabStop = false;
            this.cbVectorValueList.Tag = "color:dark";
            this.cbVectorValueList.SelectedIndexChanged += new System.EventHandler(this.cbVectorValueList_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label18.Location = new System.Drawing.Point(15, 57);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 17);
            this.label18.TabIndex = 84;
            this.label18.Text = "Value list";
            // 
            // cbVectorLimiterList
            // 
            this.cbVectorLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbVectorLimiterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVectorLimiterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbVectorLimiterList.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbVectorLimiterList.ForeColor = System.Drawing.Color.White;
            this.cbVectorLimiterList.FormattingEnabled = true;
            this.cbVectorLimiterList.Items.AddRange(new object[] {
            "Extended",
            "Extended+",
            "Extended-",
            "SuperExtended",
            "Whole",
            "Whole+",
            "Tiny",
            "One",
            "One*",
            "Two",
            "Custom",
            "AnyFloat"});
            this.cbVectorLimiterList.Location = new System.Drawing.Point(16, 28);
            this.cbVectorLimiterList.Name = "cbVectorLimiterList";
            this.cbVectorLimiterList.Size = new System.Drawing.Size(106, 25);
            this.cbVectorLimiterList.TabIndex = 82;
            this.cbVectorLimiterList.TabStop = false;
            this.cbVectorLimiterList.Tag = "color:dark";
            this.cbVectorLimiterList.SelectedIndexChanged += new System.EventHandler(this.cbVectorLimiterList_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label13.Location = new System.Drawing.Point(15, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 17);
            this.label13.TabIndex = 83;
            this.label13.Text = "Limiter list";
            // 
            // gbDefaultSettings
            // 
            this.gbDefaultSettings.ForeColor = System.Drawing.Color.White;
            this.gbDefaultSettings.Location = new System.Drawing.Point(10, 43);
            this.gbDefaultSettings.Name = "gbDefaultSettings";
            this.gbDefaultSettings.Size = new System.Drawing.Size(345, 130);
            this.gbDefaultSettings.TabIndex = 42;
            this.gbDefaultSettings.TabStop = false;
            this.gbDefaultSettings.Visible = false;
            this.gbDefaultSettings.Enter += new System.EventHandler(this.gbDefaultSettings_Enter);
            // 
            // pnCorruptionEngine
            // 
            this.pnCorruptionEngine.BackColor = System.Drawing.Color.Gray;
            this.pnCorruptionEngine.Controls.Add(this.cbCorruptionEngine);
            this.pnCorruptionEngine.Controls.Add(this.gbDefaultSettings);
            this.pnCorruptionEngine.Location = new System.Drawing.Point(9, 352);
            this.pnCorruptionEngine.Name = "pnCorruptionEngine";
            this.pnCorruptionEngine.Size = new System.Drawing.Size(367, 184);
            this.pnCorruptionEngine.TabIndex = 114;
            this.pnCorruptionEngine.Tag = "color:normal";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.nmIntensity);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nmStartingAddress);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nmBlastRange);
            this.panel1.Controls.Add(this.tbBlastRange);
            this.panel1.Controls.Add(this.cbBlastRange);
            this.panel1.Controls.Add(this.tbIntensity);
            this.panel1.Controls.Add(this.tbStartingAddress);
            this.panel1.Location = new System.Drawing.Point(9, 96);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 216);
            this.panel1.TabIndex = 114;
            this.panel1.Tag = "color:normal";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(14, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 19);
            this.label6.TabIndex = 115;
            this.label6.Text = "General Parameters";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.Gray;
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.btnDisableAutoUncorrupt);
            this.panel3.Controls.Add(this.btnEnableCaching);
            this.panel3.Location = new System.Drawing.Point(418, 495);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(436, 42);
            this.panel3.TabIndex = 115;
            this.panel3.Tag = "color:normal";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(12, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 17);
            this.label8.TabIndex = 37;
            this.label8.Text = "Debug";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(688, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 17);
            this.label9.TabIndex = 54;
            this.label9.Text = "args";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(379, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lbStockpile
            // 
            this.lbStockpile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStockpile.BackColor = System.Drawing.Color.Gray;
            this.lbStockpile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbStockpile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbStockpile.ForeColor = System.Drawing.Color.White;
            this.lbStockpile.FormattingEnabled = true;
            this.lbStockpile.IntegralHeight = false;
            this.lbStockpile.ItemHeight = 17;
            this.lbStockpile.Location = new System.Drawing.Point(419, 96);
            this.lbStockpile.Name = "lbStockpile";
            this.lbStockpile.ScrollAlwaysVisible = true;
            this.lbStockpile.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbStockpile.Size = new System.Drawing.Size(435, 360);
            this.lbStockpile.TabIndex = 2;
            this.lbStockpile.TabStop = false;
            this.lbStockpile.Tag = "color:normal";
            this.lbStockpile.SelectedIndexChanged += new System.EventHandler(this.lbStockpile_SelectedIndexChanged);
            this.lbStockpile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbStockpile_MouseDown);
            // 
            // rbTargetDolphin
            // 
            this.rbTargetDolphin.AutoSize = true;
            this.rbTargetDolphin.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rbTargetDolphin.ForeColor = System.Drawing.Color.White;
            this.rbTargetDolphin.Location = new System.Drawing.Point(219, 9);
            this.rbTargetDolphin.Name = "rbTargetDolphin";
            this.rbTargetDolphin.Size = new System.Drawing.Size(101, 17);
            this.rbTargetDolphin.TabIndex = 36;
            this.rbTargetDolphin.Text = "Target Dolphin";
            this.rbTargetDolphin.UseVisualStyleBackColor = true;
            this.rbTargetDolphin.CheckedChanged += new System.EventHandler(this.rbTargetDolphin_CheckedChanged);
            // 
            // WGH_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(864, 625);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnClearStashHistory);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnCorruptionEngine);
            this.Controls.Add(this.gbVectorEngineSettings);
            this.Controls.Add(this.btnRenameSelected);
            this.Controls.Add(this.pnBottom);
            this.Controls.Add(this.pnTargetPanel);
            this.Controls.Add(this.btnStashHistoryUp);
            this.Controls.Add(this.btnStashHistoryDown);
            this.Controls.Add(this.btnStockpileUp);
            this.Controls.Add(this.btnStockpileDown);
            this.Controls.Add(this.btnImportStockpile);
            this.Controls.Add(this.btnRemoveSelected);
            this.Controls.Add(this.gbNightmareEngineSettings);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnAddStashToStockpile);
            this.Controls.Add(this.btnStockpileMoveUp);
            this.Controls.Add(this.btnStockpileMoveDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnClearStockpile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSaveStockpile);
            this.Controls.Add(this.btnSaveStockpileAs);
            this.Controls.Add(this.btnLoadStockpile);
            this.Controls.Add(this.lbStockpile);
            this.Controls.Add(this.lbStashHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(880, 664);
            this.Name = "WGH_MainForm";
            this.Tag = "color:dark";
            this.Text = "Windows Glitch Harvester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WGH_MainForm_FormClosing);
            this.Load += new System.EventHandler(this.WGH_MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbStartingAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmStartingAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbBlastRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmBlastRange)).EndInit();
            this.gbNightmareEngineSettings.ResumeLayout(false);
            this.gbNightmareEngineSettings.PerformLayout();
            this.pnTargetPanel.ResumeLayout(false);
            this.pnTargetPanel.PerformLayout();
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.gbVectorEngineSettings.ResumeLayout(false);
            this.gbVectorEngineSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limiterMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.limiterMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vectorOffset)).EndInit();
            this.pnCorruptionEngine.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListBox lbStockpile;
        private System.Windows.Forms.Button btnBlastTarget;
        private System.Windows.Forms.Button btnBrowseTarget;
        private System.Windows.Forms.Button btnLoadStockpile;
        private System.Windows.Forms.Button btnClearStashHistory;
        private System.Windows.Forms.Button btnSaveStockpileAs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearStockpile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStockpileMoveDown;
        private System.Windows.Forms.Button btnStockpileMoveUp;
        private System.Windows.Forms.NumericUpDown nmIntensity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tbIntensity;
        private System.Windows.Forms.TrackBar tbStartingAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nmStartingAddress;
        private System.Windows.Forms.TrackBar tbBlastRange;
        private System.Windows.Forms.NumericUpDown nmBlastRange;
        private System.Windows.Forms.Button btnAddStashToStockpile;
        private System.Windows.Forms.CheckBox cbBlastRange;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnRestoreFileBackup;
        private System.Windows.Forms.GroupBox gbNightmareEngineSettings;
        private System.Windows.Forms.ComboBox cbCorruptionEngine;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnImportStockpile;
        private System.Windows.Forms.Button btnStockpileUp;
        private System.Windows.Forms.Button btnStockpileDown;
        private System.Windows.Forms.Button btnStashHistoryUp;
        private System.Windows.Forms.Button btnStashHistoryDown;
        public System.Windows.Forms.ListBox lbStashHistory;
        public System.Windows.Forms.Button btnSaveStockpile;
        public System.Windows.Forms.RadioButton rbTargetFile;
        public System.Windows.Forms.RadioButton rbTargetProcess;
        private System.Windows.Forms.Panel pnTargetPanel;
        public System.Windows.Forms.Label lbTarget;
        private System.Windows.Forms.Button btnClearAllBackups;
        private System.Windows.Forms.Button btnResetBackup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBlastType;
        private System.Windows.Forms.CheckBox cbWriteCopyMode;
        private System.Windows.Forms.Button btnInjectSelected;
        private System.Windows.Forms.CheckBox cbInjectOnSelect;
        public System.Windows.Forms.RadioButton rbExecuteOtherProgram;
        public System.Windows.Forms.RadioButton rbExecuteCorruptedFile;
        public System.Windows.Forms.RadioButton rbNoExecution;
        private System.Windows.Forms.Button btnEditExec;
        public System.Windows.Forms.Label lbExecution;
        public System.Windows.Forms.RadioButton rbExecuteScript;
        public System.Windows.Forms.RadioButton rbExecuteWith;
        private System.Windows.Forms.CheckBox cbTerminateOnReExec;
        public System.Windows.Forms.RadioButton rbTargetMultipleFiles;
        public System.Windows.Forms.Button btnEnableCaching;
        private System.Windows.Forms.Button btnDisableAutoUncorrupt;
        private System.Windows.Forms.Button btnRenameSelected;
        private System.Windows.Forms.Button btnRerollInject;
        private System.Windows.Forms.GroupBox gbVectorEngineSettings;
        public System.Windows.Forms.ComboBox cbVectorValueList;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.ComboBox cbVectorLimiterList;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox gbDefaultSettings;
        private System.Windows.Forms.Panel pnCorruptionEngine;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnBlastUntilEffect;
        private System.Windows.Forms.CheckBox cbBigEndian;
        public System.Windows.Forms.Panel pnBottom;
        public System.Windows.Forms.TextBox tbArgs;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnKillProcess;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label lbArgs;
        private System.Windows.Forms.NumericUpDown vectorOffset;
        private System.Windows.Forms.CheckBox cbVectorAligned;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button savestateInfoButton;
        private System.Windows.Forms.NumericUpDown valueMax;
        private System.Windows.Forms.NumericUpDown valueMin;
        private System.Windows.Forms.CheckBox customWholeNumbers;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown limiterMax;
        private System.Windows.Forms.NumericUpDown limiterMin;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.RadioButton rbTargetDolphin;
    }
}


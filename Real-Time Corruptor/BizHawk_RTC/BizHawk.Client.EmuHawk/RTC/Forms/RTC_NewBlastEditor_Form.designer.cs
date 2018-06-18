﻿namespace RTC
{
	partial class RTC_NewBlastEditor_Form
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_NewBlastEditor_Form));
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.btnLoadCorrupt = new System.Windows.Forms.Button();
            this.btnSendToStash = new System.Windows.Forms.Button();
            this.btnDisable50 = new System.Windows.Forms.Button();
            this.btnRemoveDisabled = new System.Windows.Forms.Button();
            this.btnInvertDisabled = new System.Windows.Forms.Button();
            this.btnDisableEverything = new System.Windows.Forms.Button();
            this.btnEnableEverything = new System.Windows.Forms.Button();
            this.btnDuplicateSelected = new System.Windows.Forms.Button();
            this.lbBlastLayerSize = new System.Windows.Forms.Label();
            this.dgvBlastLayer = new System.Windows.Forms.DataGridView();
            this.dgvBlastUnitReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBlastUnitLocked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvBlastEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvPrecision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvBlastUnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBlastUnitMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSourceAddressDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSourceAddress = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvParamDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvParam = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvNoteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnMemoryTargetting = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbShiftBlastlayer = new System.Windows.Forms.ComboBox();
            this.btnShiftBlastLayerDown = new System.Windows.Forms.Button();
            this.btnShiftBlastLayerUp = new System.Windows.Forms.Button();
            this.updownShiftBlastLayerAmount = new RTC.NumericUpDownHexFix();
            this.cbUseHex = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn1 = new RTC.DataGridViewNumericUpDownColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn2 = new RTC.DataGridViewNumericUpDownColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnSearchRow = new System.Windows.Forms.Button();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnSearchAgain = new System.Windows.Forms.Button();
            this.menuStripEx1 = new MenuStripEx();
            this.blastLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBlastlayerblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveStateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runOriginalSavestateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSavestateFromGHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceSavestateFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSavestateToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rOMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runRomWithoutBlastlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceRomFromGHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceRomFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bakeROMBlastunitsToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sanitizeDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rasterizeVMDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bakeBlastByteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openBlastLayerGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnHideSidebar = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastLayer)).BeginInit();
            this.pnMemoryTargetting.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownShiftBlastLayerAmount)).BeginInit();
            this.panelSidebar.SuspendLayout();
            this.menuStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Location = new System.Drawing.Point(9, 403);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(157, 23);
            this.btnCorrupt.TabIndex = 13;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:darker";
            this.btnCorrupt.Text = "Apply Corruption";
            this.btnCorrupt.UseVisualStyleBackColor = false;
            this.btnCorrupt.Click += new System.EventHandler(this.btnCorrupt_Click);
            // 
            // btnLoadCorrupt
            // 
            this.btnLoadCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnLoadCorrupt.FlatAppearance.BorderSize = 0;
            this.btnLoadCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnLoadCorrupt.Location = new System.Drawing.Point(9, 377);
            this.btnLoadCorrupt.Name = "btnLoadCorrupt";
            this.btnLoadCorrupt.Size = new System.Drawing.Size(157, 23);
            this.btnLoadCorrupt.TabIndex = 14;
            this.btnLoadCorrupt.TabStop = false;
            this.btnLoadCorrupt.Tag = "color:darker";
            this.btnLoadCorrupt.Text = "Load + Corrupt";
            this.btnLoadCorrupt.UseVisualStyleBackColor = false;
            this.btnLoadCorrupt.Click += new System.EventHandler(this.btnLoadCorrupt_Click);
            // 
            // btnSendToStash
            // 
            this.btnSendToStash.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendToStash.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSendToStash.FlatAppearance.BorderSize = 0;
            this.btnSendToStash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendToStash.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendToStash.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnSendToStash.Location = new System.Drawing.Point(9, 429);
            this.btnSendToStash.Name = "btnSendToStash";
            this.btnSendToStash.Size = new System.Drawing.Size(157, 23);
            this.btnSendToStash.TabIndex = 12;
            this.btnSendToStash.TabStop = false;
            this.btnSendToStash.Tag = "color:darker";
            this.btnSendToStash.Text = "Send To Stash";
            this.btnSendToStash.UseVisualStyleBackColor = false;
            this.btnSendToStash.Click += new System.EventHandler(this.btnSendToStash_Click);
            // 
            // btnDisable50
            // 
            this.btnDisable50.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisable50.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDisable50.FlatAppearance.BorderSize = 0;
            this.btnDisable50.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisable50.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisable50.ForeColor = System.Drawing.Color.Black;
            this.btnDisable50.Location = new System.Drawing.Point(9, 152);
            this.btnDisable50.Name = "btnDisable50";
            this.btnDisable50.Size = new System.Drawing.Size(157, 23);
            this.btnDisable50.TabIndex = 114;
            this.btnDisable50.TabStop = false;
            this.btnDisable50.Tag = "color:light";
            this.btnDisable50.Text = "Random Disable 50%";
            this.btnDisable50.UseVisualStyleBackColor = false;
            this.btnDisable50.Click += new System.EventHandler(this.btnDisable50_Click);
            // 
            // btnRemoveDisabled
            // 
            this.btnRemoveDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveDisabled.FlatAppearance.BorderSize = 0;
            this.btnRemoveDisabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveDisabled.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRemoveDisabled.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveDisabled.Location = new System.Drawing.Point(9, 204);
            this.btnRemoveDisabled.Name = "btnRemoveDisabled";
            this.btnRemoveDisabled.Size = new System.Drawing.Size(157, 23);
            this.btnRemoveDisabled.TabIndex = 115;
            this.btnRemoveDisabled.TabStop = false;
            this.btnRemoveDisabled.Tag = "color:light";
            this.btnRemoveDisabled.Text = "Remove Disabled";
            this.btnRemoveDisabled.UseVisualStyleBackColor = false;
            this.btnRemoveDisabled.Click += new System.EventHandler(this.btnRemoveDisabled_Click);
            // 
            // btnInvertDisabled
            // 
            this.btnInvertDisabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInvertDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnInvertDisabled.FlatAppearance.BorderSize = 0;
            this.btnInvertDisabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInvertDisabled.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnInvertDisabled.ForeColor = System.Drawing.Color.Black;
            this.btnInvertDisabled.Location = new System.Drawing.Point(9, 178);
            this.btnInvertDisabled.Name = "btnInvertDisabled";
            this.btnInvertDisabled.Size = new System.Drawing.Size(157, 23);
            this.btnInvertDisabled.TabIndex = 116;
            this.btnInvertDisabled.TabStop = false;
            this.btnInvertDisabled.Tag = "color:light";
            this.btnInvertDisabled.Text = "Invert Disabled";
            this.btnInvertDisabled.UseVisualStyleBackColor = false;
            this.btnInvertDisabled.Click += new System.EventHandler(this.btnInvertDisabled_Click);
            // 
            // btnDisableEverything
            // 
            this.btnDisableEverything.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableEverything.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDisableEverything.FlatAppearance.BorderSize = 0;
            this.btnDisableEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisableEverything.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisableEverything.ForeColor = System.Drawing.Color.Black;
            this.btnDisableEverything.Location = new System.Drawing.Point(9, 233);
            this.btnDisableEverything.Name = "btnDisableEverything";
            this.btnDisableEverything.Size = new System.Drawing.Size(157, 23);
            this.btnDisableEverything.TabIndex = 128;
            this.btnDisableEverything.TabStop = false;
            this.btnDisableEverything.Tag = "color:light";
            this.btnDisableEverything.Text = "Disable Everything";
            this.btnDisableEverything.UseVisualStyleBackColor = false;
            this.btnDisableEverything.Click += new System.EventHandler(this.btnDisableEverything_Click);
            // 
            // btnEnableEverything
            // 
            this.btnEnableEverything.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnableEverything.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnEnableEverything.FlatAppearance.BorderSize = 0;
            this.btnEnableEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnableEverything.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnEnableEverything.ForeColor = System.Drawing.Color.Black;
            this.btnEnableEverything.Location = new System.Drawing.Point(9, 259);
            this.btnEnableEverything.Name = "btnEnableEverything";
            this.btnEnableEverything.Size = new System.Drawing.Size(157, 23);
            this.btnEnableEverything.TabIndex = 129;
            this.btnEnableEverything.TabStop = false;
            this.btnEnableEverything.Tag = "color:light";
            this.btnEnableEverything.Text = "Enable Everything";
            this.btnEnableEverything.UseVisualStyleBackColor = false;
            this.btnEnableEverything.Click += new System.EventHandler(this.btnEnableEverything_Click);
            // 
            // btnDuplicateSelected
            // 
            this.btnDuplicateSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuplicateSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDuplicateSelected.FlatAppearance.BorderSize = 0;
            this.btnDuplicateSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuplicateSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnDuplicateSelected.ForeColor = System.Drawing.Color.Black;
            this.btnDuplicateSelected.Location = new System.Drawing.Point(9, 319);
            this.btnDuplicateSelected.Name = "btnDuplicateSelected";
            this.btnDuplicateSelected.Size = new System.Drawing.Size(157, 23);
            this.btnDuplicateSelected.TabIndex = 130;
            this.btnDuplicateSelected.TabStop = false;
            this.btnDuplicateSelected.Tag = "color:light";
            this.btnDuplicateSelected.Text = "Duplicate Selected Rows";
            this.btnDuplicateSelected.UseVisualStyleBackColor = false;
            this.btnDuplicateSelected.Click += new System.EventHandler(this.btnDuplicateSelected_Click);
            // 
            // lbBlastLayerSize
            // 
            this.lbBlastLayerSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlastLayerSize.AutoSize = true;
            this.lbBlastLayerSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbBlastLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbBlastLayerSize.Location = new System.Drawing.Point(2, 5);
            this.lbBlastLayerSize.Name = "lbBlastLayerSize";
            this.lbBlastLayerSize.Size = new System.Drawing.Size(58, 13);
            this.lbBlastLayerSize.TabIndex = 132;
            this.lbBlastLayerSize.Text = "Layer size:";
            // 
            // dgvBlastLayer
            // 
            this.dgvBlastLayer.AllowUserToAddRows = false;
            this.dgvBlastLayer.AllowUserToResizeRows = false;
            this.dgvBlastLayer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBlastLayer.BackgroundColor = System.Drawing.Color.Gray;
            this.dgvBlastLayer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBlastLayer.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBlastLayer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBlastLayer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlastLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvBlastUnitReference,
            this.dgvBlastUnitLocked,
            this.dgvBlastEnabled,
            this.dgvPrecision,
            this.dgvBlastUnitType,
            this.dgvBlastUnitMode,
            this.dgvSourceAddressDomain,
            this.dgvSourceAddress,
            this.dgvParamDomain,
            this.dgvParam,
            this.dgvNoteButton});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBlastLayer.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvBlastLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBlastLayer.GridColor = System.Drawing.Color.Black;
            this.dgvBlastLayer.Location = new System.Drawing.Point(0, 24);
            this.dgvBlastLayer.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBlastLayer.Name = "dgvBlastLayer";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBlastLayer.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBlastLayer.RowHeadersVisible = false;
            this.dgvBlastLayer.RowTemplate.Height = 24;
            this.dgvBlastLayer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBlastLayer.Size = new System.Drawing.Size(748, 457);
            this.dgvBlastLayer.TabIndex = 133;
            this.dgvBlastLayer.Tag = "color:normal";
            // 
            // dgvBlastUnitReference
            // 
            this.dgvBlastUnitReference.HeaderText = "dgvBlastUnitReference";
            this.dgvBlastUnitReference.Name = "dgvBlastUnitReference";
            this.dgvBlastUnitReference.Visible = false;
            // 
            // dgvBlastUnitLocked
            // 
            this.dgvBlastUnitLocked.FillWeight = 12F;
            this.dgvBlastUnitLocked.HeaderText = "🔒";
            this.dgvBlastUnitLocked.MinimumWidth = 30;
            this.dgvBlastUnitLocked.Name = "dgvBlastUnitLocked";
            // 
            // dgvBlastEnabled
            // 
            this.dgvBlastEnabled.FillWeight = 28.07027F;
            this.dgvBlastEnabled.HeaderText = "Enabled";
            this.dgvBlastEnabled.Name = "dgvBlastEnabled";
            // 
            // dgvPrecision
            // 
            this.dgvPrecision.FillWeight = 31.57906F;
            this.dgvPrecision.HeaderText = "Precision";
            this.dgvPrecision.Items.AddRange(new object[] {
            "8-bit",
            "16-bit",
            "32-bit"});
            this.dgvPrecision.Name = "dgvPrecision";
            this.dgvPrecision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvBlastUnitType
            // 
            this.dgvBlastUnitType.FillWeight = 42.10541F;
            this.dgvBlastUnitType.HeaderText = "BlastUnit Type";
            this.dgvBlastUnitType.Name = "dgvBlastUnitType";
            this.dgvBlastUnitType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvBlastUnitMode
            // 
            this.dgvBlastUnitMode.FillWeight = 38.40865F;
            this.dgvBlastUnitMode.HeaderText = "BlastUnit Mode";
            this.dgvBlastUnitMode.Name = "dgvBlastUnitMode";
            this.dgvBlastUnitMode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvSourceAddressDomain
            // 
            this.dgvSourceAddressDomain.FillWeight = 46.14497F;
            this.dgvSourceAddressDomain.HeaderText = "Source Domain";
            this.dgvSourceAddressDomain.Name = "dgvSourceAddressDomain";
            // 
            // dgvSourceAddress
            // 
            this.dgvSourceAddress.FillWeight = 46.14497F;
            this.dgvSourceAddress.HeaderText = "Source Address";
            this.dgvSourceAddress.Hexadecimal = true;
            this.dgvSourceAddress.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvSourceAddress.Name = "dgvSourceAddress";
            this.dgvSourceAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvParamDomain
            // 
            this.dgvParamDomain.FillWeight = 46.14497F;
            this.dgvParamDomain.HeaderText = "Parameter Domain";
            this.dgvParamDomain.Name = "dgvParamDomain";
            // 
            // dgvParam
            // 
            this.dgvParam.FillWeight = 46.14497F;
            this.dgvParam.HeaderText = "Parameter Value";
            this.dgvParam.Hexadecimal = true;
            this.dgvParam.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.dgvParam.Name = "dgvParam";
            this.dgvParam.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvNoteButton
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvNoteButton.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNoteButton.FillWeight = 12F;
            this.dgvNoteButton.HeaderText = "📝";
            this.dgvNoteButton.MinimumWidth = 10;
            this.dgvNoteButton.Name = "dgvNoteButton";
            // 
            // pnMemoryTargetting
            // 
            this.pnMemoryTargetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMemoryTargetting.BackColor = System.Drawing.Color.Gray;
            this.pnMemoryTargetting.Controls.Add(this.lbBlastLayerSize);
            this.pnMemoryTargetting.Location = new System.Drawing.Point(9, 20);
            this.pnMemoryTargetting.Name = "pnMemoryTargetting";
            this.pnMemoryTargetting.Size = new System.Drawing.Size(136, 24);
            this.pnMemoryTargetting.TabIndex = 134;
            this.pnMemoryTargetting.Tag = "color:normal";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(9, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "BlastLayer Info";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(9, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 136;
            this.label4.Text = "Shift Selected Rows";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.cbShiftBlastlayer);
            this.panel1.Controls.Add(this.btnShiftBlastLayerDown);
            this.panel1.Controls.Add(this.btnShiftBlastLayerUp);
            this.panel1.Controls.Add(this.updownShiftBlastLayerAmount);
            this.panel1.Location = new System.Drawing.Point(9, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 60);
            this.panel1.TabIndex = 137;
            this.panel1.Tag = "color:normal";
            // 
            // cbShiftBlastlayer
            // 
            this.cbShiftBlastlayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbShiftBlastlayer.ForeColor = System.Drawing.Color.White;
            this.cbShiftBlastlayer.FormattingEnabled = true;
            this.cbShiftBlastlayer.Location = new System.Drawing.Point(22, 6);
            this.cbShiftBlastlayer.Name = "cbShiftBlastlayer";
            this.cbShiftBlastlayer.Size = new System.Drawing.Size(114, 21);
            this.cbShiftBlastlayer.TabIndex = 148;
            this.cbShiftBlastlayer.Tag = "color:dark";
            // 
            // btnShiftBlastLayerDown
            // 
            this.btnShiftBlastLayerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShiftBlastLayerDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerDown.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerDown.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerDown.Location = new System.Drawing.Point(22, 33);
            this.btnShiftBlastLayerDown.Name = "btnShiftBlastLayerDown";
            this.btnShiftBlastLayerDown.Size = new System.Drawing.Size(21, 20);
            this.btnShiftBlastLayerDown.TabIndex = 147;
            this.btnShiftBlastLayerDown.TabStop = false;
            this.btnShiftBlastLayerDown.Tag = "color:light";
            this.btnShiftBlastLayerDown.Text = "◀";
            this.btnShiftBlastLayerDown.UseVisualStyleBackColor = false;
            this.btnShiftBlastLayerDown.Click += new System.EventHandler(this.btnShiftBlastLayerDown_Click);
            // 
            // btnShiftBlastLayerUp
            // 
            this.btnShiftBlastLayerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShiftBlastLayerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerUp.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerUp.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerUp.Location = new System.Drawing.Point(115, 33);
            this.btnShiftBlastLayerUp.Name = "btnShiftBlastLayerUp";
            this.btnShiftBlastLayerUp.Size = new System.Drawing.Size(21, 20);
            this.btnShiftBlastLayerUp.TabIndex = 146;
            this.btnShiftBlastLayerUp.TabStop = false;
            this.btnShiftBlastLayerUp.Tag = "color:light";
            this.btnShiftBlastLayerUp.Text = "▶";
            this.btnShiftBlastLayerUp.UseVisualStyleBackColor = false;
            this.btnShiftBlastLayerUp.Click += new System.EventHandler(this.btnShiftBlastLayerUp_Click);
            // 
            // updownShiftBlastLayerAmount
            // 
            this.updownShiftBlastLayerAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownShiftBlastLayerAmount.ForeColor = System.Drawing.Color.White;
            this.updownShiftBlastLayerAmount.Hexadecimal = true;
            this.updownShiftBlastLayerAmount.Location = new System.Drawing.Point(50, 33);
            this.updownShiftBlastLayerAmount.Name = "updownShiftBlastLayerAmount";
            this.updownShiftBlastLayerAmount.Size = new System.Drawing.Size(59, 20);
            this.updownShiftBlastLayerAmount.TabIndex = 145;
            this.updownShiftBlastLayerAmount.Tag = "color:dark";
            // 
            // cbUseHex
            // 
            this.cbUseHex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUseHex.AutoSize = true;
            this.cbUseHex.Checked = true;
            this.cbUseHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseHex.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseHex.ForeColor = System.Drawing.Color.White;
            this.cbUseHex.Location = new System.Drawing.Point(9, 132);
            this.cbUseHex.Name = "cbUseHex";
            this.cbUseHex.Size = new System.Drawing.Size(100, 17);
            this.cbUseHex.TabIndex = 138;
            this.cbUseHex.Tag = "";
            this.cbUseHex.Text = "Display As Hex";
            this.cbUseHex.UseVisualStyleBackColor = true;
            this.cbUseHex.CheckedChanged += new System.EventHandler(this.cbUseHex_CheckedChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "dgvBlastUnitReference";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "BlastUnit Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 90F;
            this.dataGridViewTextBoxColumn3.HeaderText = "BlastUnit Mode";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 90;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 90F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Param 1 Domain";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 90;
            // 
            // dataGridViewNumericUpDownColumn1
            // 
            this.dataGridViewNumericUpDownColumn1.FillWeight = 90F;
            this.dataGridViewNumericUpDownColumn1.HeaderText = "Param 1 Value";
            this.dataGridViewNumericUpDownColumn1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.dataGridViewNumericUpDownColumn1.Name = "dataGridViewNumericUpDownColumn1";
            this.dataGridViewNumericUpDownColumn1.Width = 90;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 90F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Param 1 Value";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 90;
            // 
            // dataGridViewNumericUpDownColumn2
            // 
            this.dataGridViewNumericUpDownColumn2.FillWeight = 90F;
            this.dataGridViewNumericUpDownColumn2.HeaderText = "Param 2 Value";
            this.dataGridViewNumericUpDownColumn2.Name = "dataGridViewNumericUpDownColumn2";
            this.dataGridViewNumericUpDownColumn2.Width = 90;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 90F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Param 2 Domain";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 90;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveSelected.FlatAppearance.BorderSize = 0;
            this.btnRemoveSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnRemoveSelected.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveSelected.Location = new System.Drawing.Point(9, 289);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(157, 23);
            this.btnRemoveSelected.TabIndex = 139;
            this.btnRemoveSelected.TabStop = false;
            this.btnRemoveSelected.Tag = "color:light";
            this.btnRemoveSelected.Text = "Remove Selected Rows";
            this.btnRemoveSelected.UseVisualStyleBackColor = false;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnSearchRow
            // 
            this.btnSearchRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSearchRow.FlatAppearance.BorderSize = 0;
            this.btnSearchRow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchRow.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSearchRow.ForeColor = System.Drawing.Color.Black;
            this.btnSearchRow.Location = new System.Drawing.Point(9, 345);
            this.btnSearchRow.Name = "btnSearchRow";
            this.btnSearchRow.Size = new System.Drawing.Size(130, 23);
            this.btnSearchRow.TabIndex = 140;
            this.btnSearchRow.TabStop = false;
            this.btnSearchRow.Tag = "color:light";
            this.btnSearchRow.Text = "Search For Row";
            this.btnSearchRow.UseVisualStyleBackColor = false;
            this.btnSearchRow.Click += new System.EventHandler(this.btnSearchRow_Click);
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.btnHelp);
            this.panelSidebar.Controls.Add(this.btnSearchAgain);
            this.panelSidebar.Controls.Add(this.label3);
            this.panelSidebar.Controls.Add(this.btnSearchRow);
            this.panelSidebar.Controls.Add(this.btnLoadCorrupt);
            this.panelSidebar.Controls.Add(this.btnRemoveSelected);
            this.panelSidebar.Controls.Add(this.btnCorrupt);
            this.panelSidebar.Controls.Add(this.cbUseHex);
            this.panelSidebar.Controls.Add(this.btnSendToStash);
            this.panelSidebar.Controls.Add(this.panel1);
            this.panelSidebar.Controls.Add(this.btnRemoveDisabled);
            this.panelSidebar.Controls.Add(this.label4);
            this.panelSidebar.Controls.Add(this.btnDisable50);
            this.panelSidebar.Controls.Add(this.btnInvertDisabled);
            this.panelSidebar.Controls.Add(this.pnMemoryTargetting);
            this.panelSidebar.Controls.Add(this.btnDisableEverything);
            this.panelSidebar.Controls.Add(this.btnEnableEverything);
            this.panelSidebar.Controls.Add(this.btnDuplicateSelected);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Location = new System.Drawing.Point(748, 24);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(176, 457);
            this.panelSidebar.TabIndex = 142;
            // 
            // btnSearchAgain
            // 
            this.btnSearchAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchAgain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSearchAgain.FlatAppearance.BorderSize = 0;
            this.btnSearchAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchAgain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSearchAgain.ForeColor = System.Drawing.Color.Black;
            this.btnSearchAgain.Location = new System.Drawing.Point(144, 345);
            this.btnSearchAgain.Name = "btnSearchAgain";
            this.btnSearchAgain.Size = new System.Drawing.Size(21, 23);
            this.btnSearchAgain.TabIndex = 141;
            this.btnSearchAgain.TabStop = false;
            this.btnSearchAgain.Tag = "color:light";
            this.btnSearchAgain.Text = "▶";
            this.btnSearchAgain.UseVisualStyleBackColor = false;
            this.btnSearchAgain.Click += new System.EventHandler(this.btnSearchAgain_Click);
            // 
            // menuStripEx1
            // 
            this.menuStripEx1.ClickThrough = true;
            this.menuStripEx1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blastLayerToolStripMenuItem,
            this.saveStateToolStripMenuItem,
            this.rOMToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.openBlastLayerGeneratorToolStripMenuItem});
            this.menuStripEx1.Location = new System.Drawing.Point(0, 0);
            this.menuStripEx1.Name = "menuStripEx1";
            this.menuStripEx1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStripEx1.Size = new System.Drawing.Size(924, 24);
            this.menuStripEx1.TabIndex = 141;
            this.menuStripEx1.Tag = "";
            this.menuStripEx1.Text = "menuStripEx1";
            // 
            // blastLayerToolStripMenuItem
            // 
            this.blastLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromFileblToolStripMenuItem,
            this.saveToFileblToolStripMenuItem,
            this.saveAsToFileblToolStripMenuItem,
            this.importBlastlayerblToolStripMenuItem,
            this.exportToCSVToolStripMenuItem});
            this.blastLayerToolStripMenuItem.Name = "blastLayerToolStripMenuItem";
            this.blastLayerToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.blastLayerToolStripMenuItem.Tag = "";
            this.blastLayerToolStripMenuItem.Text = "BlastLayer";
            // 
            // loadFromFileblToolStripMenuItem
            // 
            this.loadFromFileblToolStripMenuItem.Name = "loadFromFileblToolStripMenuItem";
            this.loadFromFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.loadFromFileblToolStripMenuItem.Text = "&Load From File (.bl)";
            this.loadFromFileblToolStripMenuItem.Click += new System.EventHandler(this.loadFromFileblToolStripMenuItem_Click);
            // 
            // saveToFileblToolStripMenuItem
            // 
            this.saveToFileblToolStripMenuItem.Name = "saveToFileblToolStripMenuItem";
            this.saveToFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToFileblToolStripMenuItem.Text = "&Save to File (.bl)";
            this.saveToFileblToolStripMenuItem.Click += new System.EventHandler(this.saveToFileblToolStripMenuItem_Click);
            // 
            // saveAsToFileblToolStripMenuItem
            // 
            this.saveAsToFileblToolStripMenuItem.Name = "saveAsToFileblToolStripMenuItem";
            this.saveAsToFileblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveAsToFileblToolStripMenuItem.Text = "&Save As to File (.bl)";
            this.saveAsToFileblToolStripMenuItem.Click += new System.EventHandler(this.saveAsToFileblToolStripMenuItem_Click);
            // 
            // importBlastlayerblToolStripMenuItem
            // 
            this.importBlastlayerblToolStripMenuItem.Name = "importBlastlayerblToolStripMenuItem";
            this.importBlastlayerblToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.importBlastlayerblToolStripMenuItem.Text = "&Import Blastlayer (.bl)";
            this.importBlastlayerblToolStripMenuItem.Click += new System.EventHandler(this.importBlastlayerblToolStripMenuItem_Click);
            // 
            // exportToCSVToolStripMenuItem
            // 
            this.exportToCSVToolStripMenuItem.Name = "exportToCSVToolStripMenuItem";
            this.exportToCSVToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exportToCSVToolStripMenuItem.Text = "&Export to CSV";
            this.exportToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportToCSVToolStripMenuItem_Click);
            // 
            // saveStateToolStripMenuItem
            // 
            this.saveStateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runOriginalSavestateToolStripMenuItem,
            this.replaceSavestateFromGHToolStripMenuItem,
            this.replaceSavestateFromFileToolStripMenuItem,
            this.saveSavestateToToolStripMenuItem});
            this.saveStateToolStripMenuItem.Name = "saveStateToolStripMenuItem";
            this.saveStateToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.saveStateToolStripMenuItem.Tag = "";
            this.saveStateToolStripMenuItem.Text = "SaveState";
            // 
            // runOriginalSavestateToolStripMenuItem
            // 
            this.runOriginalSavestateToolStripMenuItem.Name = "runOriginalSavestateToolStripMenuItem";
            this.runOriginalSavestateToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.runOriginalSavestateToolStripMenuItem.Text = "Run Original Savestate";
            this.runOriginalSavestateToolStripMenuItem.Click += new System.EventHandler(this.runOriginalSavestateToolStripMenuItem_Click);
            // 
            // replaceSavestateFromGHToolStripMenuItem
            // 
            this.replaceSavestateFromGHToolStripMenuItem.Name = "replaceSavestateFromGHToolStripMenuItem";
            this.replaceSavestateFromGHToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.replaceSavestateFromGHToolStripMenuItem.Text = "Replace Savestate from GH";
            this.replaceSavestateFromGHToolStripMenuItem.Click += new System.EventHandler(this.replaceSavestateFromGHToolStripMenuItem_Click);
            // 
            // replaceSavestateFromFileToolStripMenuItem
            // 
            this.replaceSavestateFromFileToolStripMenuItem.Name = "replaceSavestateFromFileToolStripMenuItem";
            this.replaceSavestateFromFileToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.replaceSavestateFromFileToolStripMenuItem.Text = "Replace Savestate from File";
            this.replaceSavestateFromFileToolStripMenuItem.Click += new System.EventHandler(this.replaceSavestateFromFileToolStripMenuItem_Click);
            // 
            // saveSavestateToToolStripMenuItem
            // 
            this.saveSavestateToToolStripMenuItem.Name = "saveSavestateToToolStripMenuItem";
            this.saveSavestateToToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.saveSavestateToToolStripMenuItem.Text = "Save Savestate to";
            this.saveSavestateToToolStripMenuItem.Click += new System.EventHandler(this.saveSavestateToToolStripMenuItem_Click);
            // 
            // rOMToolStripMenuItem
            // 
            this.rOMToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runRomWithoutBlastlayerToolStripMenuItem,
            this.replaceRomFromGHToolStripMenuItem,
            this.replaceRomFromFileToolStripMenuItem,
            this.bakeROMBlastunitsToFileToolStripMenuItem});
            this.rOMToolStripMenuItem.Name = "rOMToolStripMenuItem";
            this.rOMToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.rOMToolStripMenuItem.Tag = "";
            this.rOMToolStripMenuItem.Text = "ROM";
            // 
            // runRomWithoutBlastlayerToolStripMenuItem
            // 
            this.runRomWithoutBlastlayerToolStripMenuItem.Name = "runRomWithoutBlastlayerToolStripMenuItem";
            this.runRomWithoutBlastlayerToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.runRomWithoutBlastlayerToolStripMenuItem.Text = "Run Rom Without Blastlayer";
            this.runRomWithoutBlastlayerToolStripMenuItem.Click += new System.EventHandler(this.runRomWithoutBlastlayerToolStripMenuItem_Click);
            // 
            // replaceRomFromGHToolStripMenuItem
            // 
            this.replaceRomFromGHToolStripMenuItem.Name = "replaceRomFromGHToolStripMenuItem";
            this.replaceRomFromGHToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.replaceRomFromGHToolStripMenuItem.Text = "Replace Rom from GH";
            this.replaceRomFromGHToolStripMenuItem.Click += new System.EventHandler(this.replaceRomFromGHToolStripMenuItem_Click);
            // 
            // replaceRomFromFileToolStripMenuItem
            // 
            this.replaceRomFromFileToolStripMenuItem.Name = "replaceRomFromFileToolStripMenuItem";
            this.replaceRomFromFileToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.replaceRomFromFileToolStripMenuItem.Text = "Replace Rom from File";
            this.replaceRomFromFileToolStripMenuItem.Click += new System.EventHandler(this.replaceRomFromFileToolStripMenuItem_Click);
            // 
            // bakeROMBlastunitsToFileToolStripMenuItem
            // 
            this.bakeROMBlastunitsToFileToolStripMenuItem.Name = "bakeROMBlastunitsToFileToolStripMenuItem";
            this.bakeROMBlastunitsToFileToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.bakeROMBlastunitsToFileToolStripMenuItem.Text = "Bake ROM Blastunits to File";
            this.bakeROMBlastunitsToFileToolStripMenuItem.Click += new System.EventHandler(this.bakeROMBlastunitsToFileToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sanitizeDuplicatesToolStripMenuItem,
            this.rasterizeVMDsToolStripMenuItem,
            this.bakeBlastByteToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Tag = "";
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // sanitizeDuplicatesToolStripMenuItem
            // 
            this.sanitizeDuplicatesToolStripMenuItem.Name = "sanitizeDuplicatesToolStripMenuItem";
            this.sanitizeDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.sanitizeDuplicatesToolStripMenuItem.Text = "Sanitize Duplicates";
            this.sanitizeDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.sanitizeDuplicatesToolStripMenuItem_Click);
            // 
            // rasterizeVMDsToolStripMenuItem
            // 
            this.rasterizeVMDsToolStripMenuItem.Name = "rasterizeVMDsToolStripMenuItem";
            this.rasterizeVMDsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.rasterizeVMDsToolStripMenuItem.Text = "Rasterize VMDs";
            this.rasterizeVMDsToolStripMenuItem.Click += new System.EventHandler(this.rasterizeVMDsToolStripMenuItem_Click);
            // 
            // bakeBlastByteToolStripMenuItem
            // 
            this.bakeBlastByteToolStripMenuItem.Name = "bakeBlastByteToolStripMenuItem";
            this.bakeBlastByteToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.bakeBlastByteToolStripMenuItem.Text = "Bake Selected BlastBytes to Set";
            this.bakeBlastByteToolStripMenuItem.Click += new System.EventHandler(this.bakeBlastByteToolStripMenuItem_Click);
            // 
            // openBlastLayerGeneratorToolStripMenuItem
            // 
            this.openBlastLayerGeneratorToolStripMenuItem.Name = "openBlastLayerGeneratorToolStripMenuItem";
            this.openBlastLayerGeneratorToolStripMenuItem.Size = new System.Drawing.Size(159, 20);
            this.openBlastLayerGeneratorToolStripMenuItem.Text = "Open BlastLayer Generator";
            this.openBlastLayerGeneratorToolStripMenuItem.Click += new System.EventHandler(this.openBlastLayerGeneratorToolStripMenuItem_Click_1);
            // 
            // btnHideSidebar
            // 
            this.btnHideSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHideSidebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnHideSidebar.Location = new System.Drawing.Point(896, 0);
            this.btnHideSidebar.Name = "btnHideSidebar";
            this.btnHideSidebar.Size = new System.Drawing.Size(28, 24);
            this.btnHideSidebar.TabIndex = 144;
            this.btnHideSidebar.Text = "▶";
            this.btnHideSidebar.UseVisualStyleBackColor = true;
            this.btnHideSidebar.Click += new System.EventHandler(this.btnHideSidebar_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Help;
            this.btnHelp.Location = new System.Drawing.Point(148, 6);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(27, 22);
            this.btnHelp.TabIndex = 176;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:dark";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // RTC_NewBlastEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(924, 481);
            this.Controls.Add(this.btnHideSidebar);
            this.Controls.Add(this.dgvBlastLayer);
            this.Controls.Add(this.panelSidebar);
            this.Controls.Add(this.menuStripEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripEx1;
            this.MinimumSize = new System.Drawing.Size(600, 520);
            this.Name = "RTC_NewBlastEditor_Form";
            this.Tag = "color:dark";
            this.Text = "Blast Editor";
            this.Load += new System.EventHandler(this.RTC_BlastEditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastLayer)).EndInit();
            this.pnMemoryTargetting.ResumeLayout(false);
            this.pnMemoryTargetting.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updownShiftBlastLayerAmount)).EndInit();
            this.panelSidebar.ResumeLayout(false);
            this.panelSidebar.PerformLayout();
            this.menuStripEx1.ResumeLayout(false);
            this.menuStripEx1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnCorrupt;
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Button btnSendToStash;
		private System.Windows.Forms.Button btnDisable50;
		private System.Windows.Forms.Button btnRemoveDisabled;
		private System.Windows.Forms.Button btnInvertDisabled;
		private System.Windows.Forms.Button btnDisableEverything;
		private System.Windows.Forms.Button btnEnableEverything;
		private System.Windows.Forms.Button btnDuplicateSelected;
		private System.Windows.Forms.Label lbBlastLayerSize;
		public System.Windows.Forms.DataGridView dgvBlastLayer;
		private System.Windows.Forms.Panel pnMemoryTargetting;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox cbUseHex;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn1;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn2;
		private System.Windows.Forms.Button btnRemoveSelected;
		private System.Windows.Forms.Button btnSearchRow;
		private System.Windows.Forms.Panel panelSidebar;
		private MenuStripEx menuStripEx1;
		private System.Windows.Forms.ToolStripMenuItem blastLayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadFromFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveStateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rOMToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runRomWithoutBlastlayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceRomFromGHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceRomFromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bakeROMBlastunitsToFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBlastlayerblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToCSVToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem runOriginalSavestateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceSavestateFromGHToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem replaceSavestateFromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveSavestateToToolStripMenuItem;
		private System.Windows.Forms.Button btnSearchAgain;
		private System.Windows.Forms.Button btnHideSidebar;
		private System.Windows.Forms.ToolStripMenuItem saveAsToFileblToolStripMenuItem;
		private System.Windows.Forms.ComboBox cbShiftBlastlayer;
		private System.Windows.Forms.Button btnShiftBlastLayerDown;
		private System.Windows.Forms.Button btnShiftBlastLayerUp;
		private NumericUpDownHexFix updownShiftBlastLayerAmount;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rasterizeVMDsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bakeBlastByteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sanitizeDuplicatesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openBlastLayerGeneratorToolStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastUnitReference;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvBlastUnitLocked;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvBlastEnabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvPrecision;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastUnitType;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastUnitMode;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvSourceAddressDomain;
		private DataGridViewNumericUpDownColumn dgvSourceAddress;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvParamDomain;
		private DataGridViewNumericUpDownColumn dgvParam;
		private System.Windows.Forms.DataGridViewButtonColumn dgvNoteButton;
		private System.Windows.Forms.Button btnHelp;
	}
}
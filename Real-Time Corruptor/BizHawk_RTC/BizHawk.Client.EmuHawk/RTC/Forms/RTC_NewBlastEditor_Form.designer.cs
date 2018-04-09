namespace RTC
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
            this.btnSanitizeDuplicates = new System.Windows.Forms.Button();
            this.lbBlastLayerSize = new System.Windows.Forms.Label();
            this.dgvBlastLayer = new System.Windows.Forms.DataGridView();
            this.dgvBlastUnitReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBlastEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvPrecision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvBlastUnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvBUMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvParam1Domain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvParam1 = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvParam2Domain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvParam2 = new RTC.DataGridViewNumericUpDownColumn();
            this.pnMemoryTargetting = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbUseHex = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn1 = new RTC.DataGridViewNumericUpDownColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewNumericUpDownColumn2 = new RTC.DataGridViewNumericUpDownColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastLayer)).BeginInit();
            this.pnMemoryTargetting.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.btnCorrupt.Location = new System.Drawing.Point(759, 444);
            this.btnCorrupt.Name = "btnCorrupt";
            this.btnCorrupt.Size = new System.Drawing.Size(157, 23);
            this.btnCorrupt.TabIndex = 13;
            this.btnCorrupt.TabStop = false;
            this.btnCorrupt.Tag = "color:darker";
            this.btnCorrupt.Text = "Corrupt";
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
            this.btnLoadCorrupt.Location = new System.Drawing.Point(759, 418);
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
            this.btnSendToStash.Location = new System.Drawing.Point(759, 470);
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
            this.btnDisable50.Location = new System.Drawing.Point(758, 227);
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
            this.btnRemoveDisabled.Location = new System.Drawing.Point(758, 279);
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
            this.btnInvertDisabled.Location = new System.Drawing.Point(758, 253);
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
            this.btnDisableEverything.Location = new System.Drawing.Point(758, 331);
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
            this.btnEnableEverything.Location = new System.Drawing.Point(758, 357);
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
            this.btnDuplicateSelected.Location = new System.Drawing.Point(758, 383);
            this.btnDuplicateSelected.Name = "btnDuplicateSelected";
            this.btnDuplicateSelected.Size = new System.Drawing.Size(157, 23);
            this.btnDuplicateSelected.TabIndex = 130;
            this.btnDuplicateSelected.TabStop = false;
            this.btnDuplicateSelected.Tag = "color:light";
            this.btnDuplicateSelected.Text = "Duplicate Selected";
            this.btnDuplicateSelected.UseVisualStyleBackColor = false;
            this.btnDuplicateSelected.Click += new System.EventHandler(this.btnDuplicateSelected_Click);
            // 
            // btnSanitizeDuplicates
            // 
            this.btnSanitizeDuplicates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSanitizeDuplicates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSanitizeDuplicates.FlatAppearance.BorderSize = 0;
            this.btnSanitizeDuplicates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSanitizeDuplicates.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSanitizeDuplicates.ForeColor = System.Drawing.Color.Black;
            this.btnSanitizeDuplicates.Location = new System.Drawing.Point(758, 305);
            this.btnSanitizeDuplicates.Name = "btnSanitizeDuplicates";
            this.btnSanitizeDuplicates.Size = new System.Drawing.Size(157, 23);
            this.btnSanitizeDuplicates.TabIndex = 131;
            this.btnSanitizeDuplicates.TabStop = false;
            this.btnSanitizeDuplicates.Tag = "color:light";
            this.btnSanitizeDuplicates.Text = "Sanitize Duplicates";
            this.btnSanitizeDuplicates.UseVisualStyleBackColor = false;
            this.btnSanitizeDuplicates.Click += new System.EventHandler(this.btnSanitizeDuplicates_Click);
            // 
            // lbBlastLayerSize
            // 
            this.lbBlastLayerSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlastLayerSize.AutoSize = true;
            this.lbBlastLayerSize.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbBlastLayerSize.ForeColor = System.Drawing.Color.White;
            this.lbBlastLayerSize.Location = new System.Drawing.Point(5, 5);
            this.lbBlastLayerSize.Name = "lbBlastLayerSize";
            this.lbBlastLayerSize.Size = new System.Drawing.Size(58, 13);
            this.lbBlastLayerSize.TabIndex = 132;
            this.lbBlastLayerSize.Text = "Layer size:";
            // 
            // dgvBlastLayer
            // 
            this.dgvBlastLayer.AllowUserToAddRows = false;
            this.dgvBlastLayer.AllowUserToResizeRows = false;
            this.dgvBlastLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBlastLayer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlastLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvBlastUnitReference,
            this.dgvBlastEnabled,
            this.dgvPrecision,
            this.dgvBlastUnitType,
            this.dgvBUMode,
            this.dgvParam1Domain,
            this.dgvParam1,
            this.dgvParam2Domain,
            this.dgvParam2});
            this.dgvBlastLayer.Location = new System.Drawing.Point(9, 10);
            this.dgvBlastLayer.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBlastLayer.MultiSelect = false;
            this.dgvBlastLayer.Name = "dgvBlastLayer";
            this.dgvBlastLayer.RowHeadersVisible = false;
            this.dgvBlastLayer.RowTemplate.Height = 24;
            this.dgvBlastLayer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBlastLayer.Size = new System.Drawing.Size(740, 483);
            this.dgvBlastLayer.TabIndex = 133;
            this.dgvBlastLayer.Tag = "color:normal";
            this.dgvBlastLayer.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBlastLayer_CellValueChanged);
            // 
            // dgvBlastUnitReference
            // 
            this.dgvBlastUnitReference.HeaderText = "dgvBlastUnitReference";
            this.dgvBlastUnitReference.Name = "dgvBlastUnitReference";
            this.dgvBlastUnitReference.Visible = false;
            // 
            // dgvBlastEnabled
            // 
            this.dgvBlastEnabled.HeaderText = "Enabled";
            this.dgvBlastEnabled.Name = "dgvBlastEnabled";
            this.dgvBlastEnabled.Width = 75;
            // 
            // dgvPrecision
            // 
            this.dgvPrecision.HeaderText = "Precision";
            this.dgvPrecision.Items.AddRange(new object[] {
            "8-bit",
            "16-bit",
            "32-bit"});
            this.dgvPrecision.Name = "dgvPrecision";
            this.dgvPrecision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPrecision.Width = 60;
            // 
            // dgvBlastUnitType
            // 
            this.dgvBlastUnitType.HeaderText = "BlastUnit Type";
            this.dgvBlastUnitType.Name = "dgvBlastUnitType";
            // 
            // dgvBUMode
            // 
            this.dgvBUMode.FillWeight = 90F;
            this.dgvBUMode.HeaderText = "BlastUnit Mode";
            this.dgvBUMode.Name = "dgvBUMode";
            this.dgvBUMode.Width = 90;
            // 
            // dgvParam1Domain
            // 
            this.dgvParam1Domain.FillWeight = 90F;
            this.dgvParam1Domain.HeaderText = "Param 1 Domain";
            this.dgvParam1Domain.Name = "dgvParam1Domain";
            this.dgvParam1Domain.Width = 90;
            // 
            // dgvParam1
            // 
            this.dgvParam1.FillWeight = 90F;
            this.dgvParam1.HeaderText = "Param 1 Value";
            this.dgvParam1.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.dgvParam1.Name = "dgvParam1";
            this.dgvParam1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvParam1.Width = 90;
            // 
            // dgvParam2Domain
            // 
            this.dgvParam2Domain.FillWeight = 90F;
            this.dgvParam2Domain.HeaderText = "Param 2 Domain";
            this.dgvParam2Domain.Name = "dgvParam2Domain";
            this.dgvParam2Domain.Width = 90;
            // 
            // dgvParam2
            // 
            this.dgvParam2.FillWeight = 90F;
            this.dgvParam2.HeaderText = "Param 2 Value";
            this.dgvParam2.Name = "dgvParam2";
            this.dgvParam2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvParam2.Width = 90;
            // 
            // pnMemoryTargetting
            // 
            this.pnMemoryTargetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMemoryTargetting.BackColor = System.Drawing.Color.Gray;
            this.pnMemoryTargetting.Controls.Add(this.lbBlastLayerSize);
            this.pnMemoryTargetting.Location = new System.Drawing.Point(758, 27);
            this.pnMemoryTargetting.Name = "pnMemoryTargetting";
            this.pnMemoryTargetting.Size = new System.Drawing.Size(158, 82);
            this.pnMemoryTargetting.TabIndex = 134;
            this.pnMemoryTargetting.Tag = "color:normal";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(760, 10);
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
            this.label4.Location = new System.Drawing.Point(760, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 136;
            this.label4.Text = "Selected BlastUnit Info";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(758, 133);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 60);
            this.panel1.TabIndex = 137;
            this.panel1.Tag = "color:normal";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 133;
            this.label6.Text = "Param 2:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 132;
            this.label5.Text = "Param 1:";
            // 
            // cbUseHex
            // 
            this.cbUseHex.AutoSize = true;
            this.cbUseHex.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUseHex.ForeColor = System.Drawing.Color.White;
            this.cbUseHex.Location = new System.Drawing.Point(758, 200);
            this.cbUseHex.Name = "cbUseHex";
            this.cbUseHex.Size = new System.Drawing.Size(100, 17);
            this.cbUseHex.TabIndex = 138;
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
            // RTC_NewBlastEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(930, 504);
            this.Controls.Add(this.cbUseHex);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnMemoryTargetting);
            this.Controls.Add(this.dgvBlastLayer);
            this.Controls.Add(this.btnSanitizeDuplicates);
            this.Controls.Add(this.btnDuplicateSelected);
            this.Controls.Add(this.btnEnableEverything);
            this.Controls.Add(this.btnDisableEverything);
            this.Controls.Add(this.btnInvertDisabled);
            this.Controls.Add(this.btnDisable50);
            this.Controls.Add(this.btnRemoveDisabled);
            this.Controls.Add(this.btnSendToStash);
            this.Controls.Add(this.btnCorrupt);
            this.Controls.Add(this.btnLoadCorrupt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(530, 530);
            this.Name = "RTC_NewBlastEditor_Form";
            this.Tag = "color:dark";
            this.Text = "Blast Editor";
            this.Load += new System.EventHandler(this.RTC_BlastEditorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastLayer)).EndInit();
            this.pnMemoryTargetting.ResumeLayout(false);
            this.pnMemoryTargetting.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
		private System.Windows.Forms.Button btnSanitizeDuplicates;
		private System.Windows.Forms.Label lbBlastLayerSize;
		private System.Windows.Forms.DataGridView dgvBlastLayer;
		private System.Windows.Forms.Panel pnMemoryTargetting;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox cbUseHex;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn1;
		private DataGridViewNumericUpDownColumn dataGridViewNumericUpDownColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastUnitReference;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvBlastEnabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvPrecision;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBlastUnitType;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvBUMode;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvParam1Domain;
		private DataGridViewNumericUpDownColumn dgvParam1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvParam2Domain;
		private DataGridViewNumericUpDownColumn dgvParam2;
	}
}
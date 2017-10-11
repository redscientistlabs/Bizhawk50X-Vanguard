namespace RTC
{
	partial class RTC_BlastEditor_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_BlastEditor_Form));
            this.lbBlastLayer = new System.Windows.Forms.ListBox();
            this.btnCorrupt = new System.Windows.Forms.Button();
            this.btnLoadCorrupt = new System.Windows.Forms.Button();
            this.btnSendToStash = new System.Windows.Forms.Button();
            this.btnDisable50 = new System.Windows.Forms.Button();
            this.btnRemoveDisabled = new System.Windows.Forms.Button();
            this.btnInvertDisabled = new System.Windows.Forms.Button();
            this.gbValueEdit = new System.Windows.Forms.GroupBox();
            this.lbValueEdit = new System.Windows.Forms.Label();
            this.nmValueEdit = new System.Windows.Forms.NumericUpDown();
            this.btnValueUpdate = new System.Windows.Forms.Button();
            this.gbAddressEdit = new System.Windows.Forms.GroupBox();
            this.lbAddressEdit = new System.Windows.Forms.Label();
            this.nmAddressEdit = new System.Windows.Forms.NumericUpDown();
            this.btnAdressUpdate = new System.Windows.Forms.Button();
            this.btnDisableEverything = new System.Windows.Forms.Button();
            this.btnEnableEverything = new System.Windows.Forms.Button();
            this.btnDuplicateSelected = new System.Windows.Forms.Button();
            this.btnSanitizeDuplicates = new System.Windows.Forms.Button();
            this.lbBlastLayerSize = new System.Windows.Forms.Label();
            this.gbValueEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmValueEdit)).BeginInit();
            this.gbAddressEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmAddressEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // lbBlastLayer
            // 
            this.lbBlastLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlastLayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbBlastLayer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbBlastLayer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbBlastLayer.ForeColor = System.Drawing.Color.White;
            this.lbBlastLayer.FormattingEnabled = true;
            this.lbBlastLayer.IntegralHeight = false;
            this.lbBlastLayer.Location = new System.Drawing.Point(14, 14);
            this.lbBlastLayer.Margin = new System.Windows.Forms.Padding(5);
            this.lbBlastLayer.Name = "lbBlastLayer";
            this.lbBlastLayer.ScrollAlwaysVisible = true;
            this.lbBlastLayer.Size = new System.Drawing.Size(342, 507);
            this.lbBlastLayer.TabIndex = 11;
            this.lbBlastLayer.Tag = "color:dark";
            this.lbBlastLayer.SelectedIndexChanged += new System.EventHandler(this.lbBlastLayer_SelectedIndexChanged);
            this.lbBlastLayer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbBlastLayer_MouseDoubleClick);
            // 
            // btnCorrupt
            // 
            this.btnCorrupt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnCorrupt.FlatAppearance.BorderSize = 0;
            this.btnCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnCorrupt.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnCorrupt.Location = new System.Drawing.Point(369, 475);
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
            this.btnLoadCorrupt.Location = new System.Drawing.Point(369, 449);
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
            this.btnSendToStash.Location = new System.Drawing.Point(369, 501);
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
            this.btnDisable50.Location = new System.Drawing.Point(369, 41);
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
            this.btnRemoveDisabled.Location = new System.Drawing.Point(369, 93);
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
            this.btnInvertDisabled.Location = new System.Drawing.Point(369, 67);
            this.btnInvertDisabled.Name = "btnInvertDisabled";
            this.btnInvertDisabled.Size = new System.Drawing.Size(157, 23);
            this.btnInvertDisabled.TabIndex = 116;
            this.btnInvertDisabled.TabStop = false;
            this.btnInvertDisabled.Tag = "color:light";
            this.btnInvertDisabled.Text = "Invert Disabled";
            this.btnInvertDisabled.UseVisualStyleBackColor = false;
            this.btnInvertDisabled.Click += new System.EventHandler(this.btnInvertDisabled_Click);
            // 
            // gbValueEdit
            // 
            this.gbValueEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbValueEdit.Controls.Add(this.lbValueEdit);
            this.gbValueEdit.Controls.Add(this.nmValueEdit);
            this.gbValueEdit.Controls.Add(this.btnValueUpdate);
            this.gbValueEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.gbValueEdit.ForeColor = System.Drawing.Color.White;
            this.gbValueEdit.Location = new System.Drawing.Point(369, 330);
            this.gbValueEdit.Name = "gbValueEdit";
            this.gbValueEdit.Size = new System.Drawing.Size(157, 112);
            this.gbValueEdit.TabIndex = 117;
            this.gbValueEdit.TabStop = false;
            this.gbValueEdit.Visible = false;
            // 
            // lbValueEdit
            // 
            this.lbValueEdit.AutoSize = true;
            this.lbValueEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbValueEdit.ForeColor = System.Drawing.Color.White;
            this.lbValueEdit.Location = new System.Drawing.Point(14, 16);
            this.lbValueEdit.Name = "lbValueEdit";
            this.lbValueEdit.Size = new System.Drawing.Size(74, 19);
            this.lbValueEdit.TabIndex = 126;
            this.lbValueEdit.Text = "Value Edit:";
            // 
            // nmValueEdit
            // 
            this.nmValueEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmValueEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmValueEdit.ForeColor = System.Drawing.Color.White;
            this.nmValueEdit.Location = new System.Drawing.Point(18, 39);
            this.nmValueEdit.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmValueEdit.Name = "nmValueEdit";
            this.nmValueEdit.Size = new System.Drawing.Size(118, 25);
            this.nmValueEdit.TabIndex = 119;
            this.nmValueEdit.TabStop = false;
            this.nmValueEdit.Tag = "color:dark";
            this.nmValueEdit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nmValueEdit_MouseDown);
            // 
            // btnValueUpdate
            // 
            this.btnValueUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnValueUpdate.FlatAppearance.BorderSize = 0;
            this.btnValueUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnValueUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnValueUpdate.ForeColor = System.Drawing.Color.Black;
            this.btnValueUpdate.Location = new System.Drawing.Point(18, 70);
            this.btnValueUpdate.Name = "btnValueUpdate";
            this.btnValueUpdate.Size = new System.Drawing.Size(118, 28);
            this.btnValueUpdate.TabIndex = 118;
            this.btnValueUpdate.TabStop = false;
            this.btnValueUpdate.Tag = "color:light";
            this.btnValueUpdate.Text = "Update";
            this.btnValueUpdate.UseVisualStyleBackColor = false;
            this.btnValueUpdate.Click += new System.EventHandler(this.btnValueUpdate_Click);
            // 
            // gbAddressEdit
            // 
            this.gbAddressEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAddressEdit.Controls.Add(this.lbAddressEdit);
            this.gbAddressEdit.Controls.Add(this.nmAddressEdit);
            this.gbAddressEdit.Controls.Add(this.btnAdressUpdate);
            this.gbAddressEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.gbAddressEdit.ForeColor = System.Drawing.Color.White;
            this.gbAddressEdit.Location = new System.Drawing.Point(369, 220);
            this.gbAddressEdit.Name = "gbAddressEdit";
            this.gbAddressEdit.Size = new System.Drawing.Size(157, 112);
            this.gbAddressEdit.TabIndex = 127;
            this.gbAddressEdit.TabStop = false;
            this.gbAddressEdit.Visible = false;
            // 
            // lbAddressEdit
            // 
            this.lbAddressEdit.AutoSize = true;
            this.lbAddressEdit.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbAddressEdit.ForeColor = System.Drawing.Color.White;
            this.lbAddressEdit.Location = new System.Drawing.Point(14, 16);
            this.lbAddressEdit.Name = "lbAddressEdit";
            this.lbAddressEdit.Size = new System.Drawing.Size(89, 19);
            this.lbAddressEdit.TabIndex = 126;
            this.lbAddressEdit.Text = "Address Edit:";
            // 
            // nmAddressEdit
            // 
            this.nmAddressEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmAddressEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmAddressEdit.ForeColor = System.Drawing.Color.White;
            this.nmAddressEdit.Location = new System.Drawing.Point(18, 39);
            this.nmAddressEdit.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmAddressEdit.Name = "nmAddressEdit";
            this.nmAddressEdit.Size = new System.Drawing.Size(118, 25);
            this.nmAddressEdit.TabIndex = 119;
            this.nmAddressEdit.TabStop = false;
            this.nmAddressEdit.Tag = "color:dark";
            this.nmAddressEdit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.nmAddressEdit_MouseDown);
            // 
            // btnAdressUpdate
            // 
            this.btnAdressUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAdressUpdate.FlatAppearance.BorderSize = 0;
            this.btnAdressUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdressUpdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAdressUpdate.ForeColor = System.Drawing.Color.Black;
            this.btnAdressUpdate.Location = new System.Drawing.Point(18, 70);
            this.btnAdressUpdate.Name = "btnAdressUpdate";
            this.btnAdressUpdate.Size = new System.Drawing.Size(118, 28);
            this.btnAdressUpdate.TabIndex = 118;
            this.btnAdressUpdate.TabStop = false;
            this.btnAdressUpdate.Tag = "color:light";
            this.btnAdressUpdate.Text = "Update";
            this.btnAdressUpdate.UseVisualStyleBackColor = false;
            this.btnAdressUpdate.Click += new System.EventHandler(this.btnAdressUpdate_Click);
            // 
            // btnDisableEverything
            // 
            this.btnDisableEverything.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisableEverything.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDisableEverything.FlatAppearance.BorderSize = 0;
            this.btnDisableEverything.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisableEverything.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnDisableEverything.ForeColor = System.Drawing.Color.Black;
            this.btnDisableEverything.Location = new System.Drawing.Point(369, 145);
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
            this.btnEnableEverything.Location = new System.Drawing.Point(370, 171);
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
            this.btnDuplicateSelected.Location = new System.Drawing.Point(370, 197);
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
            this.btnSanitizeDuplicates.Location = new System.Drawing.Point(370, 119);
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
            this.lbBlastLayerSize.Location = new System.Drawing.Point(366, 18);
            this.lbBlastLayerSize.Name = "lbBlastLayerSize";
            this.lbBlastLayerSize.Size = new System.Drawing.Size(83, 13);
            this.lbBlastLayerSize.TabIndex = 132;
            this.lbBlastLayerSize.Text = "BlastLayer size:";
            // 
            // RTC_BlastEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(539, 536);
            this.Controls.Add(this.lbBlastLayerSize);
            this.Controls.Add(this.btnSanitizeDuplicates);
            this.Controls.Add(this.btnDuplicateSelected);
            this.Controls.Add(this.btnEnableEverything);
            this.Controls.Add(this.btnDisableEverything);
            this.Controls.Add(this.gbAddressEdit);
            this.Controls.Add(this.gbValueEdit);
            this.Controls.Add(this.btnInvertDisabled);
            this.Controls.Add(this.btnDisable50);
            this.Controls.Add(this.btnRemoveDisabled);
            this.Controls.Add(this.lbBlastLayer);
            this.Controls.Add(this.btnSendToStash);
            this.Controls.Add(this.btnCorrupt);
            this.Controls.Add(this.btnLoadCorrupt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(532, 532);
            this.Name = "RTC_BlastEditor_Form";
            this.Tag = "color:normal";
            this.Text = "Blast Editor";
            this.Load += new System.EventHandler(this.RTC_BlastEditorForm_Load);
            this.gbValueEdit.ResumeLayout(false);
            this.gbValueEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmValueEdit)).EndInit();
            this.gbAddressEdit.ResumeLayout(false);
            this.gbAddressEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmAddressEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.ListBox lbBlastLayer;
		private System.Windows.Forms.Button btnCorrupt;
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Button btnSendToStash;
		private System.Windows.Forms.Button btnDisable50;
		private System.Windows.Forms.Button btnRemoveDisabled;
		private System.Windows.Forms.Button btnInvertDisabled;
		private System.Windows.Forms.GroupBox gbValueEdit;
		private System.Windows.Forms.Button btnValueUpdate;
		public System.Windows.Forms.NumericUpDown nmValueEdit;
		private System.Windows.Forms.Label lbValueEdit;
		private System.Windows.Forms.GroupBox gbAddressEdit;
		private System.Windows.Forms.Label lbAddressEdit;
		public System.Windows.Forms.NumericUpDown nmAddressEdit;
		private System.Windows.Forms.Button btnAdressUpdate;
        private System.Windows.Forms.Button btnDisableEverything;
        private System.Windows.Forms.Button btnEnableEverything;
        private System.Windows.Forms.Button btnDuplicateSelected;
        private System.Windows.Forms.Button btnSanitizeDuplicates;
        private System.Windows.Forms.Label lbBlastLayerSize;
    }
}
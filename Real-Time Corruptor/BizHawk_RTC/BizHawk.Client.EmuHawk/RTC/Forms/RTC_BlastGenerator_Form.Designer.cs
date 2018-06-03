namespace RTC
{
	partial class RTC_BlastGenerator_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_BlastGenerator_Form));
            this.btnHelp = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tbCustomAddresses = new System.Windows.Forms.TextBox();
            this.btnLoadCorrupt = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.cbSelectedMemoryDomain = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnJustCorrupt = new System.Windows.Forms.Button();
            this.btnSendBlastLayerToEditor = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbEndianTypeValue = new System.Windows.Forms.Label();
            this.lbWordSizeValue = new System.Windows.Forms.Label();
            this.lbDomainSizeValue = new System.Windows.Forms.Label();
            this.lbEndianTypeLabel = new System.Windows.Forms.Label();
            this.lbWordSizeLabel = new System.Windows.Forms.Label();
            this.lbDomainSizeLabel = new System.Windows.Forms.Label();
            this.lbBlastUnitToGenerate = new System.Windows.Forms.Label();
            this.cbBlastUnitMode = new System.Windows.Forms.ComboBox();
            this.gbBlastByteGenerator = new System.Windows.Forms.GroupBox();
            this.updownBlastByteValue = new RTC.NumericUpDownHexFix();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbBlastByteModes = new System.Windows.Forms.ComboBox();
            this.cbUseHex = new System.Windows.Forms.CheckBox();
            this.gbBlastCheatGenerator = new System.Windows.Forms.GroupBox();
            this.updownBlastCheatValue = new RTC.NumericUpDownHexFix();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbBlastCheatModes = new System.Windows.Forms.ComboBox();
            this.gbBlastPipeGenerator = new System.Windows.Forms.GroupBox();
            this.updownBlastPipeTilt = new RTC.NumericUpDownHexFix();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbBlastPipeMode = new System.Windows.Forms.ComboBox();
            this.pnEngineMode = new System.Windows.Forms.Panel();
            this.updownStepSize = new RTC.NumericUpDownHexFix();
            this.lbUsedDomains = new System.Windows.Forms.ListBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnAddStashToStockpile = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.gbBlastByteGenerator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastByteValue)).BeginInit();
            this.gbBlastCheatGenerator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastCheatValue)).BeginInit();
            this.gbBlastPipeGenerator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastPipeTilt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownStepSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHelp
            // 
            this.btnHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHelp.FlatAppearance.BorderSize = 0;
            this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHelp.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnHelp.ForeColor = System.Drawing.Color.Black;
            this.btnHelp.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.Help;
            this.btnHelp.Location = new System.Drawing.Point(398, 117);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(19, 18);
            this.btnHelp.TabIndex = 147;
            this.btnHelp.TabStop = false;
            this.btnHelp.Tag = "color:light";
            this.btnHelp.UseVisualStyleBackColor = false;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(253, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 15);
            this.label5.TabIndex = 144;
            this.label5.Text = "Remove/Add addresses";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // tbCustomAddresses
            // 
            this.tbCustomAddresses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbCustomAddresses.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbCustomAddresses.ForeColor = System.Drawing.Color.White;
            this.tbCustomAddresses.Location = new System.Drawing.Point(256, 138);
            this.tbCustomAddresses.Multiline = true;
            this.tbCustomAddresses.Name = "tbCustomAddresses";
            this.tbCustomAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCustomAddresses.Size = new System.Drawing.Size(161, 149);
            this.tbCustomAddresses.TabIndex = 143;
            this.tbCustomAddresses.Tag = "color:dark";
            // 
            // btnLoadCorrupt
            // 
            this.btnLoadCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadCorrupt.FlatAppearance.BorderSize = 0;
            this.btnLoadCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadCorrupt.ForeColor = System.Drawing.Color.Black;
            this.btnLoadCorrupt.Location = new System.Drawing.Point(256, 297);
            this.btnLoadCorrupt.Name = "btnLoadCorrupt";
            this.btnLoadCorrupt.Size = new System.Drawing.Size(161, 30);
            this.btnLoadCorrupt.TabIndex = 139;
            this.btnLoadCorrupt.TabStop = false;
            this.btnLoadCorrupt.Tag = "color:light";
            this.btnLoadCorrupt.Text = "Load + Corrupt";
            this.btnLoadCorrupt.UseVisualStyleBackColor = false;
            this.btnLoadCorrupt.Click += new System.EventHandler(this.btnLoadCorrupt_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(98, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(84, 19);
            this.label17.TabIndex = 137;
            this.label17.Text = "Domain List";
            // 
            // btnLoadDomains
            // 
            this.btnLoadDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadDomains.FlatAppearance.BorderSize = 0;
            this.btnLoadDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDomains.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadDomains.ForeColor = System.Drawing.Color.Black;
            this.btnLoadDomains.Location = new System.Drawing.Point(12, 12);
            this.btnLoadDomains.Name = "btnLoadDomains";
            this.btnLoadDomains.Size = new System.Drawing.Size(80, 47);
            this.btnLoadDomains.TabIndex = 136;
            this.btnLoadDomains.TabStop = false;
            this.btnLoadDomains.Tag = "color:light";
            this.btnLoadDomains.Text = "Refresh Domains";
            this.btnLoadDomains.UseVisualStyleBackColor = false;
            this.btnLoadDomains.Click += new System.EventHandler(this.btnLoadDomains_Click);
            // 
            // cbSelectedMemoryDomain
            // 
            this.cbSelectedMemoryDomain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbSelectedMemoryDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedMemoryDomain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedMemoryDomain.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbSelectedMemoryDomain.ForeColor = System.Drawing.Color.White;
            this.cbSelectedMemoryDomain.FormattingEnabled = true;
            this.cbSelectedMemoryDomain.Location = new System.Drawing.Point(102, 31);
            this.cbSelectedMemoryDomain.Name = "cbSelectedMemoryDomain";
            this.cbSelectedMemoryDomain.Size = new System.Drawing.Size(120, 25);
            this.cbSelectedMemoryDomain.TabIndex = 135;
            this.cbSelectedMemoryDomain.Tag = "color:dark";
            this.cbSelectedMemoryDomain.SelectedIndexChanged += new System.EventHandler(this.cbSelectedMemoryDomain_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 19);
            this.label1.TabIndex = 149;
            this.label1.Text = "Step Size:";
            // 
            // btnJustCorrupt
            // 
            this.btnJustCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnJustCorrupt.FlatAppearance.BorderSize = 0;
            this.btnJustCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJustCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnJustCorrupt.ForeColor = System.Drawing.Color.Black;
            this.btnJustCorrupt.Location = new System.Drawing.Point(256, 333);
            this.btnJustCorrupt.Name = "btnJustCorrupt";
            this.btnJustCorrupt.Size = new System.Drawing.Size(161, 30);
            this.btnJustCorrupt.TabIndex = 150;
            this.btnJustCorrupt.TabStop = false;
            this.btnJustCorrupt.Tag = "color:light";
            this.btnJustCorrupt.Text = "Corrupt";
            this.btnJustCorrupt.UseVisualStyleBackColor = false;
            this.btnJustCorrupt.Click += new System.EventHandler(this.btnJustCorrupt_Click);
            // 
            // btnSendBlastLayerToEditor
            // 
            this.btnSendBlastLayerToEditor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSendBlastLayerToEditor.FlatAppearance.BorderSize = 0;
            this.btnSendBlastLayerToEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendBlastLayerToEditor.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSendBlastLayerToEditor.ForeColor = System.Drawing.Color.Black;
            this.btnSendBlastLayerToEditor.Location = new System.Drawing.Point(256, 369);
            this.btnSendBlastLayerToEditor.Name = "btnSendBlastLayerToEditor";
            this.btnSendBlastLayerToEditor.Size = new System.Drawing.Size(161, 30);
            this.btnSendBlastLayerToEditor.TabIndex = 151;
            this.btnSendBlastLayerToEditor.TabStop = false;
            this.btnSendBlastLayerToEditor.Tag = "color:light";
            this.btnSendBlastLayerToEditor.Text = "Send BlastLayer to Editor";
            this.btnSendBlastLayerToEditor.UseVisualStyleBackColor = false;
            this.btnSendBlastLayerToEditor.Click += new System.EventHandler(this.btnSendBlastLayerToEditor_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbEndianTypeValue);
            this.groupBox1.Controls.Add(this.lbWordSizeValue);
            this.groupBox1.Controls.Add(this.lbDomainSizeValue);
            this.groupBox1.Controls.Add(this.lbEndianTypeLabel);
            this.groupBox1.Controls.Add(this.lbWordSizeLabel);
            this.groupBox1.Controls.Add(this.lbDomainSizeLabel);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 80);
            this.groupBox1.TabIndex = 152;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Domain summary";
            // 
            // lbEndianTypeValue
            // 
            this.lbEndianTypeValue.AutoSize = true;
            this.lbEndianTypeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbEndianTypeValue.ForeColor = System.Drawing.Color.White;
            this.lbEndianTypeValue.Location = new System.Drawing.Point(82, 58);
            this.lbEndianTypeValue.Name = "lbEndianTypeValue";
            this.lbEndianTypeValue.Size = new System.Drawing.Size(42, 13);
            this.lbEndianTypeValue.TabIndex = 92;
            this.lbEndianTypeValue.Text = "#####";
            // 
            // lbWordSizeValue
            // 
            this.lbWordSizeValue.AutoSize = true;
            this.lbWordSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbWordSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbWordSizeValue.Location = new System.Drawing.Point(82, 39);
            this.lbWordSizeValue.Name = "lbWordSizeValue";
            this.lbWordSizeValue.Size = new System.Drawing.Size(42, 13);
            this.lbWordSizeValue.TabIndex = 91;
            this.lbWordSizeValue.Text = "#####";
            // 
            // lbDomainSizeValue
            // 
            this.lbDomainSizeValue.AutoSize = true;
            this.lbDomainSizeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbDomainSizeValue.ForeColor = System.Drawing.Color.White;
            this.lbDomainSizeValue.Location = new System.Drawing.Point(82, 22);
            this.lbDomainSizeValue.Name = "lbDomainSizeValue";
            this.lbDomainSizeValue.Size = new System.Drawing.Size(42, 13);
            this.lbDomainSizeValue.TabIndex = 90;
            this.lbDomainSizeValue.Text = "#####";
            // 
            // lbEndianTypeLabel
            // 
            this.lbEndianTypeLabel.AutoSize = true;
            this.lbEndianTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbEndianTypeLabel.ForeColor = System.Drawing.Color.White;
            this.lbEndianTypeLabel.Location = new System.Drawing.Point(2, 54);
            this.lbEndianTypeLabel.Name = "lbEndianTypeLabel";
            this.lbEndianTypeLabel.Size = new System.Drawing.Size(81, 17);
            this.lbEndianTypeLabel.TabIndex = 88;
            this.lbEndianTypeLabel.Text = "Endian Type:";
            // 
            // lbWordSizeLabel
            // 
            this.lbWordSizeLabel.AutoSize = true;
            this.lbWordSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbWordSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbWordSizeLabel.Location = new System.Drawing.Point(2, 36);
            this.lbWordSizeLabel.Name = "lbWordSizeLabel";
            this.lbWordSizeLabel.Size = new System.Drawing.Size(70, 17);
            this.lbWordSizeLabel.TabIndex = 87;
            this.lbWordSizeLabel.Text = "Word Size:";
            // 
            // lbDomainSizeLabel
            // 
            this.lbDomainSizeLabel.AutoSize = true;
            this.lbDomainSizeLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbDomainSizeLabel.ForeColor = System.Drawing.Color.White;
            this.lbDomainSizeLabel.Location = new System.Drawing.Point(2, 18);
            this.lbDomainSizeLabel.Name = "lbDomainSizeLabel";
            this.lbDomainSizeLabel.Size = new System.Drawing.Size(83, 17);
            this.lbDomainSizeLabel.TabIndex = 86;
            this.lbDomainSizeLabel.Text = "Domain Size:";
            // 
            // lbBlastUnitToGenerate
            // 
            this.lbBlastUnitToGenerate.AutoSize = true;
            this.lbBlastUnitToGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbBlastUnitToGenerate.ForeColor = System.Drawing.Color.White;
            this.lbBlastUnitToGenerate.Location = new System.Drawing.Point(13, 147);
            this.lbBlastUnitToGenerate.Name = "lbBlastUnitToGenerate";
            this.lbBlastUnitToGenerate.Size = new System.Drawing.Size(41, 19);
            this.lbBlastUnitToGenerate.TabIndex = 154;
            this.lbBlastUnitToGenerate.Text = "Type:";
            // 
            // cbBlastUnitMode
            // 
            this.cbBlastUnitMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastUnitMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlastUnitMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastUnitMode.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbBlastUnitMode.ForeColor = System.Drawing.Color.White;
            this.cbBlastUnitMode.FormattingEnabled = true;
            this.cbBlastUnitMode.Items.AddRange(new object[] {
            "BlastByte",
            "BlastCheat",
            "BlastPipe"});
            this.cbBlastUnitMode.Location = new System.Drawing.Point(60, 147);
            this.cbBlastUnitMode.Name = "cbBlastUnitMode";
            this.cbBlastUnitMode.Size = new System.Drawing.Size(97, 25);
            this.cbBlastUnitMode.TabIndex = 153;
            this.cbBlastUnitMode.Tag = "color:dark";
            this.cbBlastUnitMode.SelectedIndexChanged += new System.EventHandler(this.cbBlastUnitMode_SelectedIndexChanged);
            // 
            // gbBlastByteGenerator
            // 
            this.gbBlastByteGenerator.Controls.Add(this.updownBlastByteValue);
            this.gbBlastByteGenerator.Controls.Add(this.label2);
            this.gbBlastByteGenerator.Controls.Add(this.label3);
            this.gbBlastByteGenerator.Controls.Add(this.cbBlastByteModes);
            this.gbBlastByteGenerator.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.gbBlastByteGenerator.ForeColor = System.Drawing.Color.White;
            this.gbBlastByteGenerator.Location = new System.Drawing.Point(883, 34);
            this.gbBlastByteGenerator.Name = "gbBlastByteGenerator";
            this.gbBlastByteGenerator.Size = new System.Drawing.Size(210, 150);
            this.gbBlastByteGenerator.TabIndex = 156;
            this.gbBlastByteGenerator.TabStop = false;
            this.gbBlastByteGenerator.Text = "BlastByte Generator";
            // 
            // updownBlastByteValue
            // 
            this.updownBlastByteValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownBlastByteValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updownBlastByteValue.Cursor = System.Windows.Forms.Cursors.Cross;
            this.updownBlastByteValue.ForeColor = System.Drawing.Color.White;
            this.updownBlastByteValue.Location = new System.Drawing.Point(57, 50);
            this.updownBlastByteValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownBlastByteValue.Name = "updownBlastByteValue";
            this.updownBlastByteValue.Size = new System.Drawing.Size(147, 25);
            this.updownBlastByteValue.TabIndex = 157;
            this.updownBlastByteValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 19);
            this.label2.TabIndex = 157;
            this.label2.Text = "Mode:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 19);
            this.label3.TabIndex = 158;
            this.label3.Text = "Value:";
            // 
            // cbBlastByteModes
            // 
            this.cbBlastByteModes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastByteModes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastByteModes.ForeColor = System.Drawing.Color.White;
            this.cbBlastByteModes.FormattingEnabled = true;
            this.cbBlastByteModes.Items.AddRange(new object[] {
            "Shift",
            "Swap",
            "Add",
            "Set",
            "Random",
            "Bitwise Rotate Left",
            "Bitwise Rotate Right",
            "Bitwise AND",
            "Bitwise OR",
            "Bitwise XOR",
            "Bitwise Complement"});
            this.cbBlastByteModes.Location = new System.Drawing.Point(57, 21);
            this.cbBlastByteModes.Name = "cbBlastByteModes";
            this.cbBlastByteModes.Size = new System.Drawing.Size(147, 25);
            this.cbBlastByteModes.TabIndex = 0;
            this.cbBlastByteModes.SelectedIndexChanged += new System.EventHandler(this.cbBlastByteModes_SelectedIndexChanged);
            // 
            // cbUseHex
            // 
            this.cbUseHex.AutoSize = true;
            this.cbUseHex.Checked = true;
            this.cbUseHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseHex.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.cbUseHex.ForeColor = System.Drawing.Color.White;
            this.cbUseHex.Location = new System.Drawing.Point(17, 389);
            this.cbUseHex.Name = "cbUseHex";
            this.cbUseHex.Size = new System.Drawing.Size(117, 19);
            this.cbUseHex.TabIndex = 157;
            this.cbUseHex.Text = "Use Hexadecimal";
            this.cbUseHex.UseVisualStyleBackColor = true;
            this.cbUseHex.CheckedChanged += new System.EventHandler(this.cbUseHex_CheckedChanged);
            // 
            // gbBlastCheatGenerator
            // 
            this.gbBlastCheatGenerator.Controls.Add(this.updownBlastCheatValue);
            this.gbBlastCheatGenerator.Controls.Add(this.label4);
            this.gbBlastCheatGenerator.Controls.Add(this.label6);
            this.gbBlastCheatGenerator.Controls.Add(this.cbBlastCheatModes);
            this.gbBlastCheatGenerator.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.gbBlastCheatGenerator.ForeColor = System.Drawing.Color.White;
            this.gbBlastCheatGenerator.Location = new System.Drawing.Point(1128, 35);
            this.gbBlastCheatGenerator.Name = "gbBlastCheatGenerator";
            this.gbBlastCheatGenerator.Size = new System.Drawing.Size(210, 150);
            this.gbBlastCheatGenerator.TabIndex = 159;
            this.gbBlastCheatGenerator.TabStop = false;
            this.gbBlastCheatGenerator.Text = "BlastCheat Generator";
            // 
            // updownBlastCheatValue
            // 
            this.updownBlastCheatValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownBlastCheatValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updownBlastCheatValue.Cursor = System.Windows.Forms.Cursors.Cross;
            this.updownBlastCheatValue.ForeColor = System.Drawing.Color.White;
            this.updownBlastCheatValue.Location = new System.Drawing.Point(57, 50);
            this.updownBlastCheatValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownBlastCheatValue.Name = "updownBlastCheatValue";
            this.updownBlastCheatValue.Size = new System.Drawing.Size(147, 25);
            this.updownBlastCheatValue.TabIndex = 157;
            this.updownBlastCheatValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 19);
            this.label4.TabIndex = 157;
            this.label4.Text = "Mode:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(6, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 19);
            this.label6.TabIndex = 158;
            this.label6.Text = "Value:";
            // 
            // cbBlastCheatModes
            // 
            this.cbBlastCheatModes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastCheatModes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastCheatModes.ForeColor = System.Drawing.Color.White;
            this.cbBlastCheatModes.FormattingEnabled = true;
            this.cbBlastCheatModes.Items.AddRange(new object[] {
            "Shift",
            "Swap",
            "Add",
            "Set",
            "Random",
            "Bitwise Rotate Left",
            "Bitwise Rotate Right",
            "Bitwise AND",
            "Bitwise OR",
            "Bitwise XOR",
            "Bitwise Complement"});
            this.cbBlastCheatModes.Location = new System.Drawing.Point(57, 21);
            this.cbBlastCheatModes.Name = "cbBlastCheatModes";
            this.cbBlastCheatModes.Size = new System.Drawing.Size(147, 25);
            this.cbBlastCheatModes.TabIndex = 0;
            // 
            // gbBlastPipeGenerator
            // 
            this.gbBlastPipeGenerator.Controls.Add(this.updownBlastPipeTilt);
            this.gbBlastPipeGenerator.Controls.Add(this.label7);
            this.gbBlastPipeGenerator.Controls.Add(this.label8);
            this.gbBlastPipeGenerator.Controls.Add(this.cbBlastPipeMode);
            this.gbBlastPipeGenerator.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.gbBlastPipeGenerator.ForeColor = System.Drawing.Color.White;
            this.gbBlastPipeGenerator.Location = new System.Drawing.Point(1118, 201);
            this.gbBlastPipeGenerator.Name = "gbBlastPipeGenerator";
            this.gbBlastPipeGenerator.Size = new System.Drawing.Size(210, 150);
            this.gbBlastPipeGenerator.TabIndex = 160;
            this.gbBlastPipeGenerator.TabStop = false;
            this.gbBlastPipeGenerator.Text = "BlastPipe Generator";
            // 
            // updownBlastPipeTilt
            // 
            this.updownBlastPipeTilt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownBlastPipeTilt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updownBlastPipeTilt.Cursor = System.Windows.Forms.Cursors.Cross;
            this.updownBlastPipeTilt.ForeColor = System.Drawing.Color.White;
            this.updownBlastPipeTilt.Location = new System.Drawing.Point(57, 50);
            this.updownBlastPipeTilt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownBlastPipeTilt.Name = "updownBlastPipeTilt";
            this.updownBlastPipeTilt.Size = new System.Drawing.Size(147, 25);
            this.updownBlastPipeTilt.TabIndex = 157;
            this.updownBlastPipeTilt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(6, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 19);
            this.label7.TabIndex = 157;
            this.label7.Text = "Mode:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(6, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 19);
            this.label8.TabIndex = 158;
            this.label8.Text = "Tilt:";
            // 
            // cbBlastPipeMode
            // 
            this.cbBlastPipeMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastPipeMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastPipeMode.ForeColor = System.Drawing.Color.White;
            this.cbBlastPipeMode.FormattingEnabled = true;
            this.cbBlastPipeMode.Items.AddRange(new object[] {
            "Shift",
            "Swap",
            "Add",
            "Set",
            "Random",
            "Bitwise Rotate Left",
            "Bitwise Rotate Right",
            "Bitwise AND",
            "Bitwise OR",
            "Bitwise XOR",
            "Bitwise Complement"});
            this.cbBlastPipeMode.Location = new System.Drawing.Point(57, 21);
            this.cbBlastPipeMode.Name = "cbBlastPipeMode";
            this.cbBlastPipeMode.Size = new System.Drawing.Size(147, 25);
            this.cbBlastPipeMode.TabIndex = 0;
            // 
            // pnEngineMode
            // 
            this.pnEngineMode.Location = new System.Drawing.Point(17, 213);
            this.pnEngineMode.Name = "pnEngineMode";
            this.pnEngineMode.Size = new System.Drawing.Size(210, 150);
            this.pnEngineMode.TabIndex = 161;
            // 
            // updownStepSize
            // 
            this.updownStepSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updownStepSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updownStepSize.ForeColor = System.Drawing.Color.White;
            this.updownStepSize.Location = new System.Drawing.Point(84, 179);
            this.updownStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updownStepSize.Name = "updownStepSize";
            this.updownStepSize.Size = new System.Drawing.Size(73, 20);
            this.updownStepSize.TabIndex = 148;
            this.updownStepSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbUsedDomains
            // 
            this.lbUsedDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbUsedDomains.ForeColor = System.Drawing.Color.White;
            this.lbUsedDomains.FormattingEnabled = true;
            this.lbUsedDomains.Location = new System.Drawing.Point(256, 31);
            this.lbUsedDomains.Name = "lbUsedDomains";
            this.lbUsedDomains.Size = new System.Drawing.Size(161, 82);
            this.lbUsedDomains.TabIndex = 162;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(253, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 15);
            this.label9.TabIndex = 163;
            this.label9.Text = "Used Domains";
            // 
            // btnAddStashToStockpile
            // 
            this.btnAddStashToStockpile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAddStashToStockpile.FlatAppearance.BorderSize = 0;
            this.btnAddStashToStockpile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddStashToStockpile.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.btnAddStashToStockpile.ForeColor = System.Drawing.Color.Black;
            this.btnAddStashToStockpile.Location = new System.Drawing.Point(228, 31);
            this.btnAddStashToStockpile.Name = "btnAddStashToStockpile";
            this.btnAddStashToStockpile.Size = new System.Drawing.Size(22, 25);
            this.btnAddStashToStockpile.TabIndex = 164;
            this.btnAddStashToStockpile.TabStop = false;
            this.btnAddStashToStockpile.Tag = "color:light";
            this.btnAddStashToStockpile.Text = "▶";
            this.btnAddStashToStockpile.UseVisualStyleBackColor = false;
            // 
            // RTC_BlastGenerator_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1398, 482);
            this.Controls.Add(this.btnAddStashToStockpile);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lbUsedDomains);
            this.Controls.Add(this.gbBlastPipeGenerator);
            this.Controls.Add(this.gbBlastCheatGenerator);
            this.Controls.Add(this.cbUseHex);
            this.Controls.Add(this.gbBlastByteGenerator);
            this.Controls.Add(this.updownStepSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbBlastUnitToGenerate);
            this.Controls.Add(this.cbBlastUnitMode);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSendBlastLayerToEditor);
            this.Controls.Add(this.btnJustCorrupt);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbCustomAddresses);
            this.Controls.Add(this.btnLoadCorrupt);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.cbSelectedMemoryDomain);
            this.Controls.Add(this.pnEngineMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(420, 350);
            this.Name = "RTC_BlastGenerator_Form";
            this.Tag = "color:dark";
            this.Text = "BlastLayer Generator";
            this.Load += new System.EventHandler(this.RTC_BlastGeneratorForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbBlastByteGenerator.ResumeLayout(false);
            this.gbBlastByteGenerator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastByteValue)).EndInit();
            this.gbBlastCheatGenerator.ResumeLayout(false);
            this.gbBlastCheatGenerator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastCheatValue)).EndInit();
            this.gbBlastPipeGenerator.ResumeLayout(false);
            this.gbBlastPipeGenerator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownBlastPipeTilt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownStepSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbCustomAddresses;
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Button btnLoadDomains;
		public System.Windows.Forms.ComboBox cbSelectedMemoryDomain;
		private RTC.NumericUpDownHexFix updownStepSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnJustCorrupt;
		private System.Windows.Forms.Button btnSendBlastLayerToEditor;
		private System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.Label lbEndianTypeValue;
		public System.Windows.Forms.Label lbWordSizeValue;
		public System.Windows.Forms.Label lbDomainSizeValue;
		public System.Windows.Forms.Label lbEndianTypeLabel;
		public System.Windows.Forms.Label lbWordSizeLabel;
		public System.Windows.Forms.Label lbDomainSizeLabel;
		private System.Windows.Forms.Label lbBlastUnitToGenerate;
		public System.Windows.Forms.ComboBox cbBlastUnitMode;
		private System.Windows.Forms.GroupBox gbBlastByteGenerator;
		private System.Windows.Forms.ComboBox cbBlastByteModes;
		private System.Windows.Forms.Label label2;
		private NumericUpDownHexFix updownBlastByteValue;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox cbUseHex;
		private System.Windows.Forms.GroupBox gbBlastCheatGenerator;
		private NumericUpDownHexFix updownBlastCheatValue;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox cbBlastCheatModes;
		private System.Windows.Forms.GroupBox gbBlastPipeGenerator;
		private NumericUpDownHexFix updownBlastPipeTilt;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cbBlastPipeMode;
		private System.Windows.Forms.Panel pnEngineMode;
		private System.Windows.Forms.ListBox lbUsedDomains;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button btnAddStashToStockpile;
	}
}
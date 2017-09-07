namespace RTC
{
    partial class RTC_VmdGen_Form
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
            this.cbSelectedEngine = new System.Windows.Forms.ComboBox();
            this.btnLoadDomains = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.nmStartingAddress = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nmRangeSize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbEndianTypeValue = new System.Windows.Forms.Label();
            this.lbWordSizeValue = new System.Windows.Forms.Label();
            this.lbDomainSizeValue = new System.Windows.Forms.Label();
            this.lbEndianTypeLabel = new System.Windows.Forms.Label();
            this.lbWordSizeLabel = new System.Windows.Forms.Label();
            this.lbDomainSizeLabel = new System.Windows.Forms.Label();
            this.btnGenerateVMD = new System.Windows.Forms.Button();
            this.cbUsePointerSpacer = new System.Windows.Forms.CheckBox();
            this.nmPointerSpacer = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCustomAddresses = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVmdName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmStartingAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRangeSize)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPointerSpacer)).BeginInit();
            this.SuspendLayout();
            // 
            // cbSelectedEngine
            // 
            this.cbSelectedEngine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbSelectedEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedEngine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedEngine.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.cbSelectedEngine.ForeColor = System.Drawing.Color.White;
            this.cbSelectedEngine.FormattingEnabled = true;
            this.cbSelectedEngine.Location = new System.Drawing.Point(96, 29);
            this.cbSelectedEngine.Name = "cbSelectedEngine";
            this.cbSelectedEngine.Size = new System.Drawing.Size(132, 25);
            this.cbSelectedEngine.TabIndex = 16;
            this.cbSelectedEngine.Tag = "color:dark";
            this.cbSelectedEngine.SelectedIndexChanged += new System.EventHandler(this.cbSelectedEngine_SelectedIndexChanged);
            // 
            // btnLoadDomains
            // 
            this.btnLoadDomains.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadDomains.FlatAppearance.BorderSize = 0;
            this.btnLoadDomains.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDomains.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadDomains.ForeColor = System.Drawing.Color.Black;
            this.btnLoadDomains.Location = new System.Drawing.Point(6, 7);
            this.btnLoadDomains.Name = "btnLoadDomains";
            this.btnLoadDomains.Size = new System.Drawing.Size(80, 47);
            this.btnLoadDomains.TabIndex = 17;
            this.btnLoadDomains.TabStop = false;
            this.btnLoadDomains.Tag = "color:light";
            this.btnLoadDomains.Text = "Load Domains";
            this.btnLoadDomains.UseVisualStyleBackColor = false;
            this.btnLoadDomains.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(92, 7);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(115, 19);
            this.label17.TabIndex = 117;
            this.label17.Text = "Memory Domain";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(6, 68);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(98, 13);
            this.label21.TabIndex = 120;
            this.label21.Text = "Starting Address :";
            // 
            // nmStartingAddress
            // 
            this.nmStartingAddress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmStartingAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmStartingAddress.ForeColor = System.Drawing.Color.White;
            this.nmStartingAddress.Location = new System.Drawing.Point(114, 63);
            this.nmStartingAddress.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmStartingAddress.Name = "nmStartingAddress";
            this.nmStartingAddress.Size = new System.Drawing.Size(114, 25);
            this.nmStartingAddress.TabIndex = 118;
            this.nmStartingAddress.Tag = "color:dark";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 121;
            this.label1.Text = "Range size (bytes) :";
            // 
            // nmRangeSize
            // 
            this.nmRangeSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmRangeSize.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmRangeSize.ForeColor = System.Drawing.Color.White;
            this.nmRangeSize.Location = new System.Drawing.Point(114, 94);
            this.nmRangeSize.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nmRangeSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmRangeSize.Name = "nmRangeSize";
            this.nmRangeSize.Size = new System.Drawing.Size(114, 25);
            this.nmRangeSize.TabIndex = 122;
            this.nmRangeSize.Tag = "color:dark";
            this.nmRangeSize.Value = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
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
            this.groupBox1.Location = new System.Drawing.Point(235, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(148, 81);
            this.groupBox1.TabIndex = 123;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Domain summary";
            // 
            // lbEndianTypeValue
            // 
            this.lbEndianTypeValue.AutoSize = true;
            this.lbEndianTypeValue.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbEndianTypeValue.ForeColor = System.Drawing.Color.White;
            this.lbEndianTypeValue.Location = new System.Drawing.Point(83, 59);
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
            this.lbWordSizeValue.Location = new System.Drawing.Point(83, 40);
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
            this.lbDomainSizeValue.Location = new System.Drawing.Point(83, 23);
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
            this.lbEndianTypeLabel.Location = new System.Drawing.Point(3, 55);
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
            this.lbWordSizeLabel.Location = new System.Drawing.Point(3, 37);
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
            this.lbDomainSizeLabel.Location = new System.Drawing.Point(3, 19);
            this.lbDomainSizeLabel.Name = "lbDomainSizeLabel";
            this.lbDomainSizeLabel.Size = new System.Drawing.Size(83, 17);
            this.lbDomainSizeLabel.TabIndex = 86;
            this.lbDomainSizeLabel.Text = "Domain Size:";
            // 
            // btnGenerateVMD
            // 
            this.btnGenerateVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGenerateVMD.FlatAppearance.BorderSize = 0;
            this.btnGenerateVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGenerateVMD.ForeColor = System.Drawing.Color.Black;
            this.btnGenerateVMD.Location = new System.Drawing.Point(6, 211);
            this.btnGenerateVMD.Name = "btnGenerateVMD";
            this.btnGenerateVMD.Size = new System.Drawing.Size(222, 31);
            this.btnGenerateVMD.TabIndex = 124;
            this.btnGenerateVMD.TabStop = false;
            this.btnGenerateVMD.Tag = "color:light";
            this.btnGenerateVMD.Text = "Generate VMD";
            this.btnGenerateVMD.UseVisualStyleBackColor = false;
            this.btnGenerateVMD.Click += new System.EventHandler(this.btnGenerateVMD_Click);
            // 
            // cbUsePointerSpacer
            // 
            this.cbUsePointerSpacer.AutoSize = true;
            this.cbUsePointerSpacer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbUsePointerSpacer.ForeColor = System.Drawing.Color.White;
            this.cbUsePointerSpacer.Location = new System.Drawing.Point(7, 134);
            this.cbUsePointerSpacer.Name = "cbUsePointerSpacer";
            this.cbUsePointerSpacer.Size = new System.Drawing.Size(124, 17);
            this.cbUsePointerSpacer.TabIndex = 125;
            this.cbUsePointerSpacer.Text = "Make pointer every";
            this.cbUsePointerSpacer.UseVisualStyleBackColor = true;
            // 
            // nmPointerSpacer
            // 
            this.nmPointerSpacer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmPointerSpacer.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.nmPointerSpacer.ForeColor = System.Drawing.Color.White;
            this.nmPointerSpacer.Location = new System.Drawing.Point(128, 130);
            this.nmPointerSpacer.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nmPointerSpacer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmPointerSpacer.Name = "nmPointerSpacer";
            this.nmPointerSpacer.Size = new System.Drawing.Size(52, 25);
            this.nmPointerSpacer.TabIndex = 126;
            this.nmPointerSpacer.Tag = "color:dark";
            this.nmPointerSpacer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmPointerSpacer.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(181, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 127;
            this.label4.Text = "address";
            // 
            // tbCustomAddresses
            // 
            this.tbCustomAddresses.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbCustomAddresses.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbCustomAddresses.ForeColor = System.Drawing.Color.White;
            this.tbCustomAddresses.Location = new System.Drawing.Point(235, 124);
            this.tbCustomAddresses.Multiline = true;
            this.tbCustomAddresses.Name = "tbCustomAddresses";
            this.tbCustomAddresses.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbCustomAddresses.Size = new System.Drawing.Size(148, 118);
            this.tbCustomAddresses.TabIndex = 128;
            this.tbCustomAddresses.Tag = "color:dark";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(233, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 19);
            this.label5.TabIndex = 129;
            this.label5.Text = "Remove/Add addresses";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(236, 107);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 13);
            this.label9.TabIndex = 130;
            this.label9.Text = "(One per line, in Decimal)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 131;
            this.label2.Text = "VMD Name:";
            // 
            // tbVmdName
            // 
            this.tbVmdName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbVmdName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVmdName.ForeColor = System.Drawing.Color.White;
            this.tbVmdName.Location = new System.Drawing.Point(79, 168);
            this.tbVmdName.Name = "tbVmdName";
            this.tbVmdName.Size = new System.Drawing.Size(149, 24);
            this.tbVmdName.TabIndex = 132;
            this.tbVmdName.Tag = "color:dark";
            // 
            // RTC_VmdGen_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.tbVmdName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbCustomAddresses);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nmPointerSpacer);
            this.Controls.Add(this.cbUsePointerSpacer);
            this.Controls.Add(this.btnGenerateVMD);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.nmRangeSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.nmStartingAddress);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.btnLoadDomains);
            this.Controls.Add(this.cbSelectedEngine);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdGen_Form";
            this.Text = "RTC_VmdGen_Form";
            ((System.ComponentModel.ISupportInitialize)(this.nmStartingAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmRangeSize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmPointerSpacer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ComboBox cbSelectedEngine;
        private System.Windows.Forms.Button btnLoadDomains;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label21;
        public System.Windows.Forms.NumericUpDown nmStartingAddress;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown nmRangeSize;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Label lbWordSizeLabel;
        public System.Windows.Forms.Label lbDomainSizeLabel;
        public System.Windows.Forms.Label lbEndianTypeLabel;
        private System.Windows.Forms.Button btnGenerateVMD;
        public System.Windows.Forms.CheckBox cbUsePointerSpacer;
        public System.Windows.Forms.NumericUpDown nmPointerSpacer;
        public System.Windows.Forms.Label lbDomainSizeValue;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbEndianTypeValue;
        public System.Windows.Forms.Label lbWordSizeValue;
        private System.Windows.Forms.TextBox tbCustomAddresses;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVmdName;
    }
}
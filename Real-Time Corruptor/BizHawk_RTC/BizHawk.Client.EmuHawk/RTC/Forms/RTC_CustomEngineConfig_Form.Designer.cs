namespace RTC
{
	partial class RTC_CustomEngineConfig_Form
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
            this.cbClearUnitsOnRewind = new System.Windows.Forms.CheckBox();
            this.btnClearActive = new System.Windows.Forms.Button();
            this.nmMaxInfinite = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.pnMinMax = new System.Windows.Forms.Panel();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.nmMaxValueHellgenie = new RTC.NumericUpDownHexFix();
            this.nmMinValueHellgenie = new RTC.NumericUpDownHexFix();
            this.pnValueList = new System.Windows.Forms.Panel();
            this.cbValueList = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.pnValueSource = new System.Windows.Forms.Panel();
            this.radioButton12 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbValueList = new System.Windows.Forms.RadioButton();
            this.rbMinMax = new System.Windows.Forms.RadioButton();
            this.pnBackupSource = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButton11 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.cbLimiterList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxInfinite)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.pnMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxValueHellgenie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinValueHellgenie)).BeginInit();
            this.pnValueList.SuspendLayout();
            this.pnValueSource.SuspendLayout();
            this.pnBackupSource.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbClearUnitsOnRewind
            // 
            this.cbClearUnitsOnRewind.AutoSize = true;
            this.cbClearUnitsOnRewind.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbClearUnitsOnRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearUnitsOnRewind.Location = new System.Drawing.Point(230, 14);
            this.cbClearUnitsOnRewind.Name = "cbClearUnitsOnRewind";
            this.cbClearUnitsOnRewind.Size = new System.Drawing.Size(79, 17);
            this.cbClearUnitsOnRewind.TabIndex = 151;
            this.cbClearUnitsOnRewind.Text = "Lock Units";
            this.cbClearUnitsOnRewind.UseVisualStyleBackColor = true;
            // 
            // btnClearActive
            // 
            this.btnClearActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearActive.FlatAppearance.BorderSize = 0;
            this.btnClearActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearActive.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearActive.ForeColor = System.Drawing.Color.Black;
            this.btnClearActive.Location = new System.Drawing.Point(12, 267);
            this.btnClearActive.Name = "btnClearActive";
            this.btnClearActive.Size = new System.Drawing.Size(159, 24);
            this.btnClearActive.TabIndex = 150;
            this.btnClearActive.TabStop = false;
            this.btnClearActive.Tag = "color:light";
            this.btnClearActive.Text = "Clear all active units";
            this.btnClearActive.UseVisualStyleBackColor = false;
            // 
            // nmMaxInfinite
            // 
            this.nmMaxInfinite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxInfinite.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.nmMaxInfinite.ForeColor = System.Drawing.Color.White;
            this.nmMaxInfinite.Location = new System.Drawing.Point(130, 29);
            this.nmMaxInfinite.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmMaxInfinite.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmMaxInfinite.Name = "nmMaxInfinite";
            this.nmMaxInfinite.Size = new System.Drawing.Size(70, 22);
            this.nmMaxInfinite.TabIndex = 149;
            this.nmMaxInfinite.Tag = "color:dark";
            this.nmMaxInfinite.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nmMaxInfinite.ValueChanged += new System.EventHandler(this.nmMaxInfinite_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(127, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 148;
            this.label1.Text = "Max Infinite Units";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(106, 54);
            this.groupBox2.TabIndex = 161;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Unit Source";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.radioButton1.Location = new System.Drawing.Point(6, 31);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(64, 19);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Backup";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.radioButton2.Location = new System.Drawing.Point(6, 14);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(54, 19);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Value";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // pnMinMax
            // 
            this.pnMinMax.Controls.Add(this.label26);
            this.pnMinMax.Controls.Add(this.label27);
            this.pnMinMax.Controls.Add(this.nmMaxValueHellgenie);
            this.pnMinMax.Controls.Add(this.nmMinValueHellgenie);
            this.pnMinMax.Location = new System.Drawing.Point(124, 73);
            this.pnMinMax.Name = "pnMinMax";
            this.pnMinMax.Size = new System.Drawing.Size(154, 69);
            this.pnMinMax.TabIndex = 166;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(9, 37);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(60, 13);
            this.label26.TabIndex = 169;
            this.label26.Text = "Max Value";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(9, 13);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(59, 13);
            this.label27.TabIndex = 166;
            this.label27.Text = "Min Value";
            // 
            // nmMaxValueHellgenie
            // 
            this.nmMaxValueHellgenie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxValueHellgenie.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMaxValueHellgenie.ForeColor = System.Drawing.Color.White;
            this.nmMaxValueHellgenie.Hexadecimal = true;
            this.nmMaxValueHellgenie.Location = new System.Drawing.Point(75, 36);
            this.nmMaxValueHellgenie.Name = "nmMaxValueHellgenie";
            this.nmMaxValueHellgenie.Size = new System.Drawing.Size(70, 20);
            this.nmMaxValueHellgenie.TabIndex = 168;
            this.nmMaxValueHellgenie.Tag = "color:dark";
            this.nmMaxValueHellgenie.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // nmMinValueHellgenie
            // 
            this.nmMinValueHellgenie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMinValueHellgenie.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMinValueHellgenie.ForeColor = System.Drawing.Color.White;
            this.nmMinValueHellgenie.Hexadecimal = true;
            this.nmMinValueHellgenie.Location = new System.Drawing.Point(75, 12);
            this.nmMinValueHellgenie.Name = "nmMinValueHellgenie";
            this.nmMinValueHellgenie.Size = new System.Drawing.Size(70, 20);
            this.nmMinValueHellgenie.TabIndex = 167;
            this.nmMinValueHellgenie.Tag = "color:dark";
            // 
            // pnValueList
            // 
            this.pnValueList.Controls.Add(this.cbValueList);
            this.pnValueList.Controls.Add(this.label18);
            this.pnValueList.Location = new System.Drawing.Point(284, 73);
            this.pnValueList.Name = "pnValueList";
            this.pnValueList.Size = new System.Drawing.Size(154, 69);
            this.pnValueList.TabIndex = 167;
            // 
            // cbValueList
            // 
            this.cbValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbValueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValueList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbValueList.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbValueList.ForeColor = System.Drawing.Color.White;
            this.cbValueList.FormattingEnabled = true;
            this.cbValueList.Location = new System.Drawing.Point(12, 25);
            this.cbValueList.Name = "cbValueList";
            this.cbValueList.Size = new System.Drawing.Size(130, 21);
            this.cbValueList.TabIndex = 87;
            this.cbValueList.Tag = "color:dark";
			this.cbValueList.DataSource = RTC_Core.filterListsComboSource;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(9, 9);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 13);
            this.label18.TabIndex = 84;
            this.label18.Text = "Value list:";
            // 
            // pnValueSource
            // 
            this.pnValueSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnValueSource.Controls.Add(this.radioButton12);
            this.pnValueSource.Controls.Add(this.label2);
            this.pnValueSource.Controls.Add(this.rbValueList);
            this.pnValueSource.Controls.Add(this.rbMinMax);
            this.pnValueSource.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pnValueSource.ForeColor = System.Drawing.Color.White;
            this.pnValueSource.Location = new System.Drawing.Point(12, 73);
            this.pnValueSource.Name = "pnValueSource";
            this.pnValueSource.Size = new System.Drawing.Size(106, 85);
            this.pnValueSource.TabIndex = 169;
            // 
            // radioButton12
            // 
            this.radioButton12.AutoSize = true;
            this.radioButton12.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton12.Location = new System.Drawing.Point(6, 54);
            this.radioButton12.Name = "radioButton12";
            this.radioButton12.Size = new System.Drawing.Size(74, 17);
            this.radioButton12.TabIndex = 180;
            this.radioButton12.TabStop = true;
            this.radioButton12.Text = "Value List";
            this.radioButton12.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 179;
            this.label2.Text = "Value Source";
            // 
            // rbValueList
            // 
            this.rbValueList.AutoSize = true;
            this.rbValueList.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbValueList.Location = new System.Drawing.Point(6, 36);
            this.rbValueList.Name = "rbValueList";
            this.rbValueList.Size = new System.Drawing.Size(74, 17);
            this.rbValueList.TabIndex = 177;
            this.rbValueList.TabStop = true;
            this.rbValueList.Text = "Value List";
            this.rbValueList.UseVisualStyleBackColor = true;
            // 
            // rbMinMax
            // 
            this.rbMinMax.AutoSize = true;
            this.rbMinMax.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbMinMax.Location = new System.Drawing.Point(6, 18);
            this.rbMinMax.Name = "rbMinMax";
            this.rbMinMax.Size = new System.Drawing.Size(99, 17);
            this.rbMinMax.TabIndex = 176;
            this.rbMinMax.TabStop = true;
            this.rbMinMax.Text = "MinMax Boxes";
            this.rbMinMax.UseVisualStyleBackColor = true;
            // 
            // pnBackupSource
            // 
            this.pnBackupSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBackupSource.Controls.Add(this.panel4);
            this.pnBackupSource.Controls.Add(this.panel2);
            this.pnBackupSource.Controls.Add(this.panel1);
            this.pnBackupSource.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pnBackupSource.ForeColor = System.Drawing.Color.White;
            this.pnBackupSource.Location = new System.Drawing.Point(12, 164);
            this.pnBackupSource.Name = "pnBackupSource";
            this.pnBackupSource.Size = new System.Drawing.Size(321, 85);
            this.pnBackupSource.TabIndex = 180;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.radioButton7);
            this.panel4.Controls.Add(this.radioButton8);
            this.panel4.Location = new System.Drawing.Point(217, 13);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(100, 55);
            this.panel4.TabIndex = 183;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 182;
            this.label5.Text = "Store Type";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton7.Location = new System.Drawing.Point(6, 33);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(64, 17);
            this.radioButton7.TabIndex = 181;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "Execute";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton8.Location = new System.Drawing.Point(6, 16);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(52, 17);
            this.radioButton8.TabIndex = 180;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Once";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.radioButton5);
            this.panel2.Controls.Add(this.radioButton6);
            this.panel2.Location = new System.Drawing.Point(111, 13);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 55);
            this.panel2.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 182;
            this.label4.Text = "Store Address";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton5.Location = new System.Drawing.Point(6, 33);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(68, 17);
            this.radioButton5.TabIndex = 181;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Random";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton6.Location = new System.Drawing.Point(6, 16);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(52, 17);
            this.radioButton6.TabIndex = 180;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Same";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Location = new System.Drawing.Point(1, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(104, 55);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 182;
            this.label3.Text = "Start Storing";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton3.Location = new System.Drawing.Point(6, 33);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(89, 17);
            this.radioButton3.TabIndex = 181;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "First Execute";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton4.Location = new System.Drawing.Point(6, 16);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(78, 17);
            this.radioButton4.TabIndex = 180;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Immediate";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.radioButton11);
            this.panel5.Controls.Add(this.label7);
            this.panel5.Controls.Add(this.radioButton9);
            this.panel5.Controls.Add(this.radioButton10);
            this.panel5.Controls.Add(this.cbLimiterList);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.panel5.ForeColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(341, 164);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(154, 127);
            this.panel5.TabIndex = 181;
            // 
            // radioButton11
            // 
            this.radioButton11.AutoSize = true;
            this.radioButton11.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton11.Location = new System.Drawing.Point(12, 96);
            this.radioButton11.Name = "radioButton11";
            this.radioButton11.Size = new System.Drawing.Size(64, 17);
            this.radioButton11.TabIndex = 186;
            this.radioButton11.TabStop = true;
            this.radioButton11.Text = "Execute";
            this.radioButton11.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 185;
            this.label7.Text = "Limiter Time";
            // 
            // radioButton9
            // 
            this.radioButton9.AutoSize = true;
            this.radioButton9.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton9.Location = new System.Drawing.Point(12, 80);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(89, 17);
            this.radioButton9.TabIndex = 184;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "First Execute";
            this.radioButton9.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.radioButton10.Location = new System.Drawing.Point(12, 63);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(72, 17);
            this.radioButton10.TabIndex = 183;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "Generate";
            this.radioButton10.UseVisualStyleBackColor = true;
            // 
            // cbLimiterList
            // 
            this.cbLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbLimiterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimiterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLimiterList.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbLimiterList.ForeColor = System.Drawing.Color.White;
            this.cbLimiterList.FormattingEnabled = true;
            this.cbLimiterList.Location = new System.Drawing.Point(12, 25);
            this.cbLimiterList.Name = "cbLimiterList";
            this.cbLimiterList.Size = new System.Drawing.Size(130, 21);
            this.cbLimiterList.TabIndex = 87;
            this.cbLimiterList.Tag = "color:dark";
			this.cbLimiterList.DataSource = RTC_Core.filterListsComboSource;

			// 
			// label6
			// 
			this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(9, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 84;
            this.label6.Text = "Limiter List";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(230, 33);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(141, 17);
            this.checkBox1.TabIndex = 182;
            this.checkBox1.Text = "Clear Units on Rewind";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // RTC_CustomEngineConfig_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(501, 303);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.pnBackupSource);
            this.Controls.Add(this.pnValueSource);
            this.Controls.Add(this.pnMinMax);
            this.Controls.Add(this.pnValueList);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cbClearUnitsOnRewind);
            this.Controls.Add(this.btnClearActive);
            this.Controls.Add(this.nmMaxInfinite);
            this.Controls.Add(this.label1);
            this.Name = "RTC_CustomEngineConfig_Form";
            this.Text = "Custom Engine Config";
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxInfinite)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.pnMinMax.ResumeLayout(false);
            this.pnMinMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxValueHellgenie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinValueHellgenie)).EndInit();
            this.pnValueList.ResumeLayout(false);
            this.pnValueList.PerformLayout();
            this.pnValueSource.ResumeLayout(false);
            this.pnValueSource.PerformLayout();
            this.pnBackupSource.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.CheckBox cbClearUnitsOnRewind;
		private System.Windows.Forms.Button btnClearActive;
		public System.Windows.Forms.NumericUpDown nmMaxInfinite;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.Panel pnMinMax;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		public NumericUpDownHexFix nmMaxValueHellgenie;
		public NumericUpDownHexFix nmMinValueHellgenie;
		private System.Windows.Forms.Panel pnValueList;
		public System.Windows.Forms.ComboBox cbValueList;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Panel pnValueSource;
		private System.Windows.Forms.RadioButton rbValueList;
		private System.Windows.Forms.RadioButton rbMinMax;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnBackupSource;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.RadioButton radioButton12;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton radioButton7;
		private System.Windows.Forms.RadioButton radioButton8;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.RadioButton radioButton11;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton radioButton9;
		private System.Windows.Forms.RadioButton radioButton10;
		public System.Windows.Forms.ComboBox cbLimiterList;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.CheckBox checkBox1;
	}
}
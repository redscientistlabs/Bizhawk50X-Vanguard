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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_CustomEngineConfig_Form));
            this.cbLockUnits = new System.Windows.Forms.CheckBox();
            this.btnClearActive = new System.Windows.Forms.Button();
            this.nmMaxInfinite = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbUnitSource = new System.Windows.Forms.GroupBox();
            this.rbUnitSourceStore = new System.Windows.Forms.RadioButton();
            this.rbUnitSourceValue = new System.Windows.Forms.RadioButton();
            this.pnMinMax = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.nmMaxValue = new RTC.NumericUpDownHexFix();
            this.nmMinValue = new RTC.NumericUpDownHexFix();
            this.pnValueList = new System.Windows.Forms.Panel();
            this.cbValueList = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.pnValueSource = new System.Windows.Forms.Panel();
            this.rbRange = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbValueList = new System.Windows.Forms.RadioButton();
            this.rbRandom = new System.Windows.Forms.RadioButton();
            this.pnBackupSource = new System.Windows.Forms.Panel();
            this.pnStoreType = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.rbStoreStep = new System.Windows.Forms.RadioButton();
            this.rbStoreOnce = new System.Windows.Forms.RadioButton();
            this.pnStoreAddress = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.rbStoreRandom = new System.Windows.Forms.RadioButton();
            this.rbStoreSame = new System.Windows.Forms.RadioButton();
            this.pnStoreTime = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.rbStoreFirstExecute = new System.Windows.Forms.RadioButton();
            this.rbStoreImmediate = new System.Windows.Forms.RadioButton();
            this.pnLimiterList = new System.Windows.Forms.Panel();
            this.rbLimiterNone = new System.Windows.Forms.RadioButton();
            this.rbLimiterExecute = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.rbLimiterFirstExecute = new System.Windows.Forms.RadioButton();
            this.rbLimiterGenerate = new System.Windows.Forms.RadioButton();
            this.cbLimiterList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbClearRewind = new System.Windows.Forms.CheckBox();
            this.cbLoopUnit = new System.Windows.Forms.CheckBox();
            this.pnStepSettings = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nmDelay = new System.Windows.Forms.NumericUpDown();
            this.nmLifetime = new System.Windows.Forms.NumericUpDown();
            this.pnCheckBoxes = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.nmTilt = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxInfinite)).BeginInit();
            this.gbUnitSource.SuspendLayout();
            this.pnMinMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinValue)).BeginInit();
            this.pnValueList.SuspendLayout();
            this.pnValueSource.SuspendLayout();
            this.pnBackupSource.SuspendLayout();
            this.pnStoreType.SuspendLayout();
            this.pnStoreAddress.SuspendLayout();
            this.pnStoreTime.SuspendLayout();
            this.pnLimiterList.SuspendLayout();
            this.pnStepSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLifetime)).BeginInit();
            this.pnCheckBoxes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmTilt)).BeginInit();
            this.SuspendLayout();
            // 
            // cbLockUnits
            // 
            this.cbLockUnits.AutoSize = true;
            this.cbLockUnits.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbLockUnits.ForeColor = System.Drawing.Color.White;
            this.cbLockUnits.Location = new System.Drawing.Point(3, 7);
            this.cbLockUnits.Name = "cbLockUnits";
            this.cbLockUnits.Size = new System.Drawing.Size(79, 17);
            this.cbLockUnits.TabIndex = 151;
            this.cbLockUnits.Text = "Lock Units";
            this.cbLockUnits.UseVisualStyleBackColor = true;
            this.cbLockUnits.CheckedChanged += new System.EventHandler(this.cbLockUnits_CheckedChanged);
            // 
            // btnClearActive
            // 
            this.btnClearActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearActive.FlatAppearance.BorderSize = 0;
            this.btnClearActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearActive.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearActive.ForeColor = System.Drawing.Color.Black;
            this.btnClearActive.Location = new System.Drawing.Point(12, 277);
            this.btnClearActive.Name = "btnClearActive";
            this.btnClearActive.Size = new System.Drawing.Size(123, 24);
            this.btnClearActive.TabIndex = 150;
            this.btnClearActive.TabStop = false;
            this.btnClearActive.Tag = "color:light";
            this.btnClearActive.Text = "Clear all active units";
            this.btnClearActive.UseVisualStyleBackColor = false;
            this.btnClearActive.Click += new System.EventHandler(this.btnClearActive_Click);
            // 
            // nmMaxInfinite
            // 
            this.nmMaxInfinite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxInfinite.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.nmMaxInfinite.ForeColor = System.Drawing.Color.White;
            this.nmMaxInfinite.Location = new System.Drawing.Point(154, 277);
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
            this.label1.Location = new System.Drawing.Point(227, 283);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 148;
            this.label1.Text = "Max Infinite Units";
            // 
            // gbUnitSource
            // 
            this.gbUnitSource.Controls.Add(this.rbUnitSourceStore);
            this.gbUnitSource.Controls.Add(this.rbUnitSourceValue);
            this.gbUnitSource.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.gbUnitSource.ForeColor = System.Drawing.Color.White;
            this.gbUnitSource.Location = new System.Drawing.Point(12, 13);
            this.gbUnitSource.Name = "gbUnitSource";
            this.gbUnitSource.Size = new System.Drawing.Size(106, 54);
            this.gbUnitSource.TabIndex = 161;
            this.gbUnitSource.TabStop = false;
            this.gbUnitSource.Text = "Unit Source";
            // 
            // rbUnitSourceStore
            // 
            this.rbUnitSourceStore.AutoSize = true;
            this.rbUnitSourceStore.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.rbUnitSourceStore.Location = new System.Drawing.Point(6, 31);
            this.rbUnitSourceStore.Name = "rbUnitSourceStore";
            this.rbUnitSourceStore.Size = new System.Drawing.Size(52, 19);
            this.rbUnitSourceStore.TabIndex = 1;
            this.rbUnitSourceStore.Text = "Store";
            this.rbUnitSourceStore.UseVisualStyleBackColor = true;
            this.rbUnitSourceStore.CheckedChanged += new System.EventHandler(this.rbUnitSourceStore_CheckedChanged);
            // 
            // rbUnitSourceValue
            // 
            this.rbUnitSourceValue.AutoSize = true;
            this.rbUnitSourceValue.Checked = true;
            this.rbUnitSourceValue.Font = new System.Drawing.Font("Segoe UI Symbol", 9F);
            this.rbUnitSourceValue.Location = new System.Drawing.Point(6, 14);
            this.rbUnitSourceValue.Name = "rbUnitSourceValue";
            this.rbUnitSourceValue.Size = new System.Drawing.Size(54, 19);
            this.rbUnitSourceValue.TabIndex = 0;
            this.rbUnitSourceValue.TabStop = true;
            this.rbUnitSourceValue.Text = "Value";
            this.rbUnitSourceValue.UseVisualStyleBackColor = true;
            this.rbUnitSourceValue.CheckedChanged += new System.EventHandler(this.rbUnitSourceValue_CheckedChanged);
            // 
            // pnMinMax
            // 
            this.pnMinMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnMinMax.Controls.Add(this.label8);
            this.pnMinMax.Controls.Add(this.label26);
            this.pnMinMax.Controls.Add(this.label27);
            this.pnMinMax.Controls.Add(this.nmMaxValue);
            this.pnMinMax.Controls.Add(this.nmMinValue);
            this.pnMinMax.Location = new System.Drawing.Point(154, 106);
            this.pnMinMax.Name = "pnMinMax";
            this.pnMinMax.Size = new System.Drawing.Size(154, 69);
            this.pnMinMax.TabIndex = 166;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(9, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 13);
            this.label8.TabIndex = 170;
            this.label8.Text = "Range:";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label26.ForeColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(9, 45);
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
            this.label27.Location = new System.Drawing.Point(9, 27);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(59, 13);
            this.label27.TabIndex = 166;
            this.label27.Text = "Min Value";
            // 
            // nmMaxValue
            // 
            this.nmMaxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMaxValue.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMaxValue.ForeColor = System.Drawing.Color.White;
            this.nmMaxValue.Hexadecimal = true;
            this.nmMaxValue.Location = new System.Drawing.Point(75, 45);
            this.nmMaxValue.Name = "nmMaxValue";
            this.nmMaxValue.Size = new System.Drawing.Size(70, 20);
            this.nmMaxValue.TabIndex = 168;
            this.nmMaxValue.Tag = "color:dark";
            this.nmMaxValue.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nmMaxValue.ValueChanged += new System.EventHandler(this.nmMaxValue_ValueChanged);
            // 
            // nmMinValue
            // 
            this.nmMinValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmMinValue.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.nmMinValue.ForeColor = System.Drawing.Color.White;
            this.nmMinValue.Hexadecimal = true;
            this.nmMinValue.Location = new System.Drawing.Point(75, 21);
            this.nmMinValue.Name = "nmMinValue";
            this.nmMinValue.Size = new System.Drawing.Size(70, 20);
            this.nmMinValue.TabIndex = 167;
            this.nmMinValue.Tag = "color:dark";
            this.nmMinValue.ValueChanged += new System.EventHandler(this.nmMinValue_ValueChanged);
            // 
            // pnValueList
            // 
            this.pnValueList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnValueList.Controls.Add(this.cbValueList);
            this.pnValueList.Controls.Add(this.label18);
            this.pnValueList.Location = new System.Drawing.Point(341, 89);
            this.pnValueList.Name = "pnValueList";
            this.pnValueList.Size = new System.Drawing.Size(154, 69);
            this.pnValueList.TabIndex = 167;
            // 
            // cbValueList
            // 
            this.cbValueList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbValueList.DataSource = ((object)(resources.GetObject("cbValueList.DataSource")));
            this.cbValueList.DisplayMember = "Text";
            this.cbValueList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbValueList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbValueList.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbValueList.ForeColor = System.Drawing.Color.White;
            this.cbValueList.FormattingEnabled = true;
            this.cbValueList.Location = new System.Drawing.Point(12, 24);
            this.cbValueList.Name = "cbValueList";
            this.cbValueList.Size = new System.Drawing.Size(130, 21);
            this.cbValueList.TabIndex = 87;
            this.cbValueList.Tag = "color:dark";
            this.cbValueList.ValueMember = "Value";
            this.cbValueList.SelectedIndexChanged += new System.EventHandler(this.cbValueList_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(9, 4);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 13);
            this.label18.TabIndex = 84;
            this.label18.Text = "Value list:";
            // 
            // pnValueSource
            // 
            this.pnValueSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnValueSource.Controls.Add(this.rbRange);
            this.pnValueSource.Controls.Add(this.label2);
            this.pnValueSource.Controls.Add(this.rbValueList);
            this.pnValueSource.Controls.Add(this.rbRandom);
            this.pnValueSource.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pnValueSource.ForeColor = System.Drawing.Color.White;
            this.pnValueSource.Location = new System.Drawing.Point(12, 73);
            this.pnValueSource.Name = "pnValueSource";
            this.pnValueSource.Size = new System.Drawing.Size(106, 85);
            this.pnValueSource.TabIndex = 169;
            // 
            // rbRange
            // 
            this.rbRange.AutoSize = true;
            this.rbRange.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbRange.Location = new System.Drawing.Point(6, 54);
            this.rbRange.Name = "rbRange";
            this.rbRange.Size = new System.Drawing.Size(58, 17);
            this.rbRange.TabIndex = 180;
            this.rbRange.Text = "Range";
            this.rbRange.UseVisualStyleBackColor = true;
            this.rbRange.CheckedChanged += new System.EventHandler(this.rbRange_CheckedChanged);
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
            this.rbValueList.Text = "Value List";
            this.rbValueList.UseVisualStyleBackColor = true;
            this.rbValueList.CheckedChanged += new System.EventHandler(this.rbValueList_CheckedChanged);
            // 
            // rbRandom
            // 
            this.rbRandom.AutoSize = true;
            this.rbRandom.Checked = true;
            this.rbRandom.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbRandom.Location = new System.Drawing.Point(6, 18);
            this.rbRandom.Name = "rbRandom";
            this.rbRandom.Size = new System.Drawing.Size(68, 17);
            this.rbRandom.TabIndex = 176;
            this.rbRandom.TabStop = true;
            this.rbRandom.Text = "Random";
            this.rbRandom.UseVisualStyleBackColor = true;
            this.rbRandom.CheckedChanged += new System.EventHandler(this.rbRandom_CheckedChanged);
            // 
            // pnBackupSource
            // 
            this.pnBackupSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnBackupSource.Controls.Add(this.pnStoreType);
            this.pnBackupSource.Controls.Add(this.pnStoreAddress);
            this.pnBackupSource.Controls.Add(this.pnStoreTime);
            this.pnBackupSource.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pnBackupSource.ForeColor = System.Drawing.Color.White;
            this.pnBackupSource.Location = new System.Drawing.Point(12, 177);
            this.pnBackupSource.Name = "pnBackupSource";
            this.pnBackupSource.Size = new System.Drawing.Size(321, 85);
            this.pnBackupSource.TabIndex = 180;
            // 
            // pnStoreType
            // 
            this.pnStoreType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnStoreType.Controls.Add(this.label5);
            this.pnStoreType.Controls.Add(this.rbStoreStep);
            this.pnStoreType.Controls.Add(this.rbStoreOnce);
            this.pnStoreType.Location = new System.Drawing.Point(217, 13);
            this.pnStoreType.Name = "pnStoreType";
            this.pnStoreType.Size = new System.Drawing.Size(100, 55);
            this.pnStoreType.TabIndex = 183;
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
            // rbStoreStep
            // 
            this.rbStoreStep.AutoSize = true;
            this.rbStoreStep.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreStep.Location = new System.Drawing.Point(6, 33);
            this.rbStoreStep.Name = "rbStoreStep";
            this.rbStoreStep.Size = new System.Drawing.Size(86, 17);
            this.rbStoreStep.TabIndex = 181;
            this.rbStoreStep.Text = "Continuous";
            this.rbStoreStep.UseVisualStyleBackColor = true;
            this.rbStoreStep.CheckedChanged += new System.EventHandler(this.rbStoreStep_CheckedChanged);
            // 
            // rbStoreOnce
            // 
            this.rbStoreOnce.AutoSize = true;
            this.rbStoreOnce.Checked = true;
            this.rbStoreOnce.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreOnce.Location = new System.Drawing.Point(6, 16);
            this.rbStoreOnce.Name = "rbStoreOnce";
            this.rbStoreOnce.Size = new System.Drawing.Size(52, 17);
            this.rbStoreOnce.TabIndex = 180;
            this.rbStoreOnce.TabStop = true;
            this.rbStoreOnce.Text = "Once";
            this.rbStoreOnce.UseVisualStyleBackColor = true;
            this.rbStoreOnce.CheckedChanged += new System.EventHandler(this.rbStoreOnce_CheckedChanged);
            // 
            // pnStoreAddress
            // 
            this.pnStoreAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnStoreAddress.Controls.Add(this.label4);
            this.pnStoreAddress.Controls.Add(this.rbStoreRandom);
            this.pnStoreAddress.Controls.Add(this.rbStoreSame);
            this.pnStoreAddress.Location = new System.Drawing.Point(111, 13);
            this.pnStoreAddress.Name = "pnStoreAddress";
            this.pnStoreAddress.Size = new System.Drawing.Size(100, 55);
            this.pnStoreAddress.TabIndex = 1;
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
            // rbStoreRandom
            // 
            this.rbStoreRandom.AutoSize = true;
            this.rbStoreRandom.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreRandom.Location = new System.Drawing.Point(6, 33);
            this.rbStoreRandom.Name = "rbStoreRandom";
            this.rbStoreRandom.Size = new System.Drawing.Size(68, 17);
            this.rbStoreRandom.TabIndex = 181;
            this.rbStoreRandom.Text = "Random";
            this.rbStoreRandom.UseVisualStyleBackColor = true;
            this.rbStoreRandom.CheckedChanged += new System.EventHandler(this.rbStoreRandom_CheckedChanged);
            // 
            // rbStoreSame
            // 
            this.rbStoreSame.AutoSize = true;
            this.rbStoreSame.Checked = true;
            this.rbStoreSame.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreSame.Location = new System.Drawing.Point(6, 16);
            this.rbStoreSame.Name = "rbStoreSame";
            this.rbStoreSame.Size = new System.Drawing.Size(52, 17);
            this.rbStoreSame.TabIndex = 180;
            this.rbStoreSame.TabStop = true;
            this.rbStoreSame.Text = "Same";
            this.rbStoreSame.UseVisualStyleBackColor = true;
            this.rbStoreSame.CheckedChanged += new System.EventHandler(this.rbStoreSame_CheckedChanged);
            // 
            // pnStoreTime
            // 
            this.pnStoreTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnStoreTime.Controls.Add(this.label3);
            this.pnStoreTime.Controls.Add(this.rbStoreFirstExecute);
            this.pnStoreTime.Controls.Add(this.rbStoreImmediate);
            this.pnStoreTime.Location = new System.Drawing.Point(1, 13);
            this.pnStoreTime.Name = "pnStoreTime";
            this.pnStoreTime.Size = new System.Drawing.Size(104, 55);
            this.pnStoreTime.TabIndex = 0;
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
            // rbStoreFirstExecute
            // 
            this.rbStoreFirstExecute.AutoSize = true;
            this.rbStoreFirstExecute.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreFirstExecute.Location = new System.Drawing.Point(6, 33);
            this.rbStoreFirstExecute.Name = "rbStoreFirstExecute";
            this.rbStoreFirstExecute.Size = new System.Drawing.Size(89, 17);
            this.rbStoreFirstExecute.TabIndex = 181;
            this.rbStoreFirstExecute.Text = "First Execute";
            this.rbStoreFirstExecute.UseVisualStyleBackColor = true;
            this.rbStoreFirstExecute.CheckedChanged += new System.EventHandler(this.rbStoreFirstExecute_CheckedChanged);
            // 
            // rbStoreImmediate
            // 
            this.rbStoreImmediate.AutoSize = true;
            this.rbStoreImmediate.Checked = true;
            this.rbStoreImmediate.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbStoreImmediate.Location = new System.Drawing.Point(6, 16);
            this.rbStoreImmediate.Name = "rbStoreImmediate";
            this.rbStoreImmediate.Size = new System.Drawing.Size(78, 17);
            this.rbStoreImmediate.TabIndex = 180;
            this.rbStoreImmediate.TabStop = true;
            this.rbStoreImmediate.Text = "Immediate";
            this.rbStoreImmediate.UseVisualStyleBackColor = true;
            this.rbStoreImmediate.CheckedChanged += new System.EventHandler(this.rbStoreImmediate_CheckedChanged);
            // 
            // pnLimiterList
            // 
            this.pnLimiterList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnLimiterList.Controls.Add(this.rbLimiterNone);
            this.pnLimiterList.Controls.Add(this.rbLimiterExecute);
            this.pnLimiterList.Controls.Add(this.label7);
            this.pnLimiterList.Controls.Add(this.rbLimiterFirstExecute);
            this.pnLimiterList.Controls.Add(this.rbLimiterGenerate);
            this.pnLimiterList.Controls.Add(this.cbLimiterList);
            this.pnLimiterList.Controls.Add(this.label6);
            this.pnLimiterList.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.pnLimiterList.ForeColor = System.Drawing.Color.White;
            this.pnLimiterList.Location = new System.Drawing.Point(341, 164);
            this.pnLimiterList.Name = "pnLimiterList";
            this.pnLimiterList.Size = new System.Drawing.Size(154, 134);
            this.pnLimiterList.TabIndex = 181;
            // 
            // rbLimiterNone
            // 
            this.rbLimiterNone.AutoSize = true;
            this.rbLimiterNone.Checked = true;
            this.rbLimiterNone.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbLimiterNone.Location = new System.Drawing.Point(12, 64);
            this.rbLimiterNone.Name = "rbLimiterNone";
            this.rbLimiterNone.Size = new System.Drawing.Size(53, 17);
            this.rbLimiterNone.TabIndex = 187;
            this.rbLimiterNone.TabStop = true;
            this.rbLimiterNone.Text = "None";
            this.rbLimiterNone.UseVisualStyleBackColor = true;
            this.rbLimiterNone.CheckedChanged += new System.EventHandler(this.rbLimiterNone_CheckedChanged);
            // 
            // rbLimiterExecute
            // 
            this.rbLimiterExecute.AutoSize = true;
            this.rbLimiterExecute.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbLimiterExecute.Location = new System.Drawing.Point(12, 112);
            this.rbLimiterExecute.Name = "rbLimiterExecute";
            this.rbLimiterExecute.Size = new System.Drawing.Size(64, 17);
            this.rbLimiterExecute.TabIndex = 186;
            this.rbLimiterExecute.Text = "Execute";
            this.rbLimiterExecute.UseVisualStyleBackColor = true;
            this.rbLimiterExecute.CheckedChanged += new System.EventHandler(this.rbLimiterExecute_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 13);
            this.label7.TabIndex = 185;
            this.label7.Text = "Limiter Time";
            // 
            // rbLimiterFirstExecute
            // 
            this.rbLimiterFirstExecute.AutoSize = true;
            this.rbLimiterFirstExecute.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbLimiterFirstExecute.Location = new System.Drawing.Point(12, 96);
            this.rbLimiterFirstExecute.Name = "rbLimiterFirstExecute";
            this.rbLimiterFirstExecute.Size = new System.Drawing.Size(89, 17);
            this.rbLimiterFirstExecute.TabIndex = 184;
            this.rbLimiterFirstExecute.Text = "First Execute";
            this.rbLimiterFirstExecute.UseVisualStyleBackColor = true;
            this.rbLimiterFirstExecute.CheckedChanged += new System.EventHandler(this.rbLimiterFirstExecute_CheckedChanged);
            // 
            // rbLimiterGenerate
            // 
            this.rbLimiterGenerate.AutoSize = true;
            this.rbLimiterGenerate.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.rbLimiterGenerate.Location = new System.Drawing.Point(12, 80);
            this.rbLimiterGenerate.Name = "rbLimiterGenerate";
            this.rbLimiterGenerate.Size = new System.Drawing.Size(72, 17);
            this.rbLimiterGenerate.TabIndex = 183;
            this.rbLimiterGenerate.Text = "Generate";
            this.rbLimiterGenerate.UseVisualStyleBackColor = true;
            this.rbLimiterGenerate.CheckedChanged += new System.EventHandler(this.rbLimiterGenerate_CheckedChanged);
            // 
            // cbLimiterList
            // 
            this.cbLimiterList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbLimiterList.DataSource = ((object)(resources.GetObject("cbLimiterList.DataSource")));
            this.cbLimiterList.DisplayMember = "Text";
            this.cbLimiterList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLimiterList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLimiterList.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbLimiterList.ForeColor = System.Drawing.Color.White;
            this.cbLimiterList.FormattingEnabled = true;
            this.cbLimiterList.Location = new System.Drawing.Point(12, 22);
            this.cbLimiterList.Name = "cbLimiterList";
            this.cbLimiterList.Size = new System.Drawing.Size(130, 21);
            this.cbLimiterList.TabIndex = 87;
            this.cbLimiterList.Tag = "color:dark";
            this.cbLimiterList.ValueMember = "Value";
            this.cbLimiterList.SelectedIndexChanged += new System.EventHandler(this.cbLimiterList_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(9, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 84;
            this.label6.Text = "Limiter List";
            // 
            // cbClearRewind
            // 
            this.cbClearRewind.AutoSize = true;
            this.cbClearRewind.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbClearRewind.ForeColor = System.Drawing.Color.White;
            this.cbClearRewind.Location = new System.Drawing.Point(3, 25);
            this.cbClearRewind.Name = "cbClearRewind";
            this.cbClearRewind.Size = new System.Drawing.Size(141, 17);
            this.cbClearRewind.TabIndex = 182;
            this.cbClearRewind.Text = "Clear Units on Rewind";
            this.cbClearRewind.UseVisualStyleBackColor = true;
            this.cbClearRewind.CheckedChanged += new System.EventHandler(this.cbClearRewind_CheckedChanged);
            // 
            // cbLoopUnit
            // 
            this.cbLoopUnit.AutoSize = true;
            this.cbLoopUnit.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.cbLoopUnit.ForeColor = System.Drawing.Color.White;
            this.cbLoopUnit.Location = new System.Drawing.Point(3, 43);
            this.cbLoopUnit.Name = "cbLoopUnit";
            this.cbLoopUnit.Size = new System.Drawing.Size(77, 17);
            this.cbLoopUnit.TabIndex = 183;
            this.cbLoopUnit.Text = "Loop Unit";
            this.cbLoopUnit.UseVisualStyleBackColor = true;
            this.cbLoopUnit.CheckedChanged += new System.EventHandler(this.cbLoopUnit_CheckedChanged);
            // 
            // pnStepSettings
            // 
            this.pnStepSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnStepSettings.Controls.Add(this.label11);
            this.pnStepSettings.Controls.Add(this.nmTilt);
            this.pnStepSettings.Controls.Add(this.label9);
            this.pnStepSettings.Controls.Add(this.label10);
            this.pnStepSettings.Controls.Add(this.nmDelay);
            this.pnStepSettings.Controls.Add(this.nmLifetime);
            this.pnStepSettings.Location = new System.Drawing.Point(154, 13);
            this.pnStepSettings.Name = "pnStepSettings";
            this.pnStepSettings.Size = new System.Drawing.Size(154, 87);
            this.pnStepSettings.TabIndex = 188;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(9, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 188;
            this.label9.Text = "Lifetime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(9, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 189;
            this.label10.Text = "Delay";
            // 
            // nmDelay
            // 
            this.nmDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmDelay.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.nmDelay.ForeColor = System.Drawing.Color.White;
            this.nmDelay.Location = new System.Drawing.Point(75, 30);
            this.nmDelay.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.nmDelay.Name = "nmDelay";
            this.nmDelay.Size = new System.Drawing.Size(70, 22);
            this.nmDelay.TabIndex = 187;
            this.nmDelay.Tag = "color:dark";
            this.nmDelay.ValueChanged += new System.EventHandler(this.nmDelay_ValueChanged);
            // 
            // nmLifetime
            // 
            this.nmLifetime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmLifetime.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.nmLifetime.ForeColor = System.Drawing.Color.White;
            this.nmLifetime.Location = new System.Drawing.Point(75, 3);
            this.nmLifetime.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.nmLifetime.Name = "nmLifetime";
            this.nmLifetime.Size = new System.Drawing.Size(70, 22);
            this.nmLifetime.TabIndex = 185;
            this.nmLifetime.Tag = "color:dark";
            this.nmLifetime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmLifetime.ValueChanged += new System.EventHandler(this.nmLifetime_ValueChanged);
            // 
            // pnCheckBoxes
            // 
            this.pnCheckBoxes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnCheckBoxes.Controls.Add(this.cbLockUnits);
            this.pnCheckBoxes.Controls.Add(this.cbClearRewind);
            this.pnCheckBoxes.Controls.Add(this.cbLoopUnit);
            this.pnCheckBoxes.Location = new System.Drawing.Point(341, 12);
            this.pnCheckBoxes.Name = "pnCheckBoxes";
            this.pnCheckBoxes.Size = new System.Drawing.Size(154, 69);
            this.pnCheckBoxes.TabIndex = 189;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(9, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(22, 13);
            this.label11.TabIndex = 191;
            this.label11.Text = "Tilt";
            // 
            // nmTilt
            // 
            this.nmTilt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmTilt.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            this.nmTilt.ForeColor = System.Drawing.Color.White;
            this.nmTilt.Location = new System.Drawing.Point(75, 57);
            this.nmTilt.Maximum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            0});
            this.nmTilt.Minimum = new decimal(new int[] {
            -1981284353,
            -1966660860,
            0,
            -2147483648});
            this.nmTilt.Name = "nmTilt";
            this.nmTilt.Size = new System.Drawing.Size(70, 22);
            this.nmTilt.TabIndex = 190;
            this.nmTilt.Tag = "color:dark";
            this.nmTilt.ValueChanged += new System.EventHandler(this.nmTilt_ValueChanged);
            // 
            // RTC_CustomEngineConfig_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(501, 317);
            this.Controls.Add(this.pnCheckBoxes);
            this.Controls.Add(this.pnStepSettings);
            this.Controls.Add(this.pnLimiterList);
            this.Controls.Add(this.pnBackupSource);
            this.Controls.Add(this.pnValueSource);
            this.Controls.Add(this.pnMinMax);
            this.Controls.Add(this.pnValueList);
            this.Controls.Add(this.gbUnitSource);
            this.Controls.Add(this.btnClearActive);
            this.Controls.Add(this.nmMaxInfinite);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_CustomEngineConfig_Form";
            this.Tag = "color:normal";
            this.Text = "Custom Engine Config";
            this.Load += new System.EventHandler(this.RTC_CustomEngineConfig_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxInfinite)).EndInit();
            this.gbUnitSource.ResumeLayout(false);
            this.gbUnitSource.PerformLayout();
            this.pnMinMax.ResumeLayout(false);
            this.pnMinMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmMaxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmMinValue)).EndInit();
            this.pnValueList.ResumeLayout(false);
            this.pnValueList.PerformLayout();
            this.pnValueSource.ResumeLayout(false);
            this.pnValueSource.PerformLayout();
            this.pnBackupSource.ResumeLayout(false);
            this.pnStoreType.ResumeLayout(false);
            this.pnStoreType.PerformLayout();
            this.pnStoreAddress.ResumeLayout(false);
            this.pnStoreAddress.PerformLayout();
            this.pnStoreTime.ResumeLayout(false);
            this.pnStoreTime.PerformLayout();
            this.pnLimiterList.ResumeLayout(false);
            this.pnLimiterList.PerformLayout();
            this.pnStepSettings.ResumeLayout(false);
            this.pnStepSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmLifetime)).EndInit();
            this.pnCheckBoxes.ResumeLayout(false);
            this.pnCheckBoxes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmTilt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.CheckBox cbLockUnits;
		private System.Windows.Forms.Button btnClearActive;
		public System.Windows.Forms.NumericUpDown nmMaxInfinite;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox gbUnitSource;
		private System.Windows.Forms.RadioButton rbUnitSourceStore;
		private System.Windows.Forms.RadioButton rbUnitSourceValue;
		private System.Windows.Forms.Panel pnMinMax;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		public NumericUpDownHexFix nmMaxValue;
		public NumericUpDownHexFix nmMinValue;
		private System.Windows.Forms.Panel pnValueList;
		public System.Windows.Forms.ComboBox cbValueList;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Panel pnValueSource;
		private System.Windows.Forms.RadioButton rbValueList;
		private System.Windows.Forms.RadioButton rbRandom;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel pnBackupSource;
		private System.Windows.Forms.Panel pnStoreTime;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton rbStoreFirstExecute;
		private System.Windows.Forms.RadioButton rbStoreImmediate;
		private System.Windows.Forms.Panel pnStoreAddress;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbStoreRandom;
		private System.Windows.Forms.RadioButton rbStoreSame;
		private System.Windows.Forms.RadioButton rbRange;
		private System.Windows.Forms.Panel pnStoreType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton rbStoreStep;
		private System.Windows.Forms.RadioButton rbStoreOnce;
		private System.Windows.Forms.Panel pnLimiterList;
		private System.Windows.Forms.RadioButton rbLimiterExecute;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RadioButton rbLimiterFirstExecute;
		private System.Windows.Forms.RadioButton rbLimiterGenerate;
		public System.Windows.Forms.ComboBox cbLimiterList;
		private System.Windows.Forms.Label label6;
		public System.Windows.Forms.CheckBox cbClearRewind;
		public System.Windows.Forms.CheckBox cbLoopUnit;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RadioButton rbLimiterNone;
		private System.Windows.Forms.Panel pnStepSettings;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		public System.Windows.Forms.NumericUpDown nmDelay;
		public System.Windows.Forms.NumericUpDown nmLifetime;
		private System.Windows.Forms.Panel pnCheckBoxes;
		private System.Windows.Forms.Label label11;
		public System.Windows.Forms.NumericUpDown nmTilt;
	}
}
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_BlastGenerator_Form));
            this.btnLoadCorrupt = new System.Windows.Forms.Button();
            this.btnJustCorrupt = new System.Windows.Forms.Button();
            this.btnSendBlastLayerToEditor = new System.Windows.Forms.Button();
            this.cbUseHex = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.updownNudgeParam2 = new RTC.NumericUpDownHexFix();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.updownNudgeParam1 = new RTC.NumericUpDownHexFix();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.updownNudgeEndAddress = new RTC.NumericUpDownHexFix();
            this.btnShiftBlastLayerUp = new System.Windows.Forms.Button();
            this.btnShiftBlastLayerDown = new System.Windows.Forms.Button();
            this.updownNudgeStartAddress = new RTC.NumericUpDownHexFix();
            this.label4 = new System.Windows.Forms.Label();
            this.dgvBlastGenerator = new System.Windows.Forms.DataGridView();
            this.menuStripEx1 = new MenuStripEx();
            this.blastLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFromFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToFileblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBlastlayerblToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvDomain = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvMode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvStepSize = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvStartAddress = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvEndAddress = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvParam1 = new RTC.DataGridViewNumericUpDownColumn();
            this.dgvParam2 = new RTC.DataGridViewNumericUpDownColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeEndAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeStartAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastGenerator)).BeginInit();
            this.menuStripEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadCorrupt
            // 
            this.btnLoadCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadCorrupt.FlatAppearance.BorderSize = 0;
            this.btnLoadCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoadCorrupt.ForeColor = System.Drawing.Color.Black;
            this.btnLoadCorrupt.Location = new System.Drawing.Point(5, 260);
            this.btnLoadCorrupt.Name = "btnLoadCorrupt";
            this.btnLoadCorrupt.Size = new System.Drawing.Size(148, 30);
            this.btnLoadCorrupt.TabIndex = 139;
            this.btnLoadCorrupt.TabStop = false;
            this.btnLoadCorrupt.Tag = "color:light";
            this.btnLoadCorrupt.Text = "Load + Corrupt";
            this.btnLoadCorrupt.UseVisualStyleBackColor = false;
            this.btnLoadCorrupt.Click += new System.EventHandler(this.btnLoadCorrupt_Click);
            // 
            // btnJustCorrupt
            // 
            this.btnJustCorrupt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnJustCorrupt.FlatAppearance.BorderSize = 0;
            this.btnJustCorrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJustCorrupt.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnJustCorrupt.ForeColor = System.Drawing.Color.Black;
            this.btnJustCorrupt.Location = new System.Drawing.Point(5, 296);
            this.btnJustCorrupt.Name = "btnJustCorrupt";
            this.btnJustCorrupt.Size = new System.Drawing.Size(148, 30);
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
            this.btnSendBlastLayerToEditor.Location = new System.Drawing.Point(5, 332);
            this.btnSendBlastLayerToEditor.Name = "btnSendBlastLayerToEditor";
            this.btnSendBlastLayerToEditor.Size = new System.Drawing.Size(148, 30);
            this.btnSendBlastLayerToEditor.TabIndex = 151;
            this.btnSendBlastLayerToEditor.TabStop = false;
            this.btnSendBlastLayerToEditor.Tag = "color:light";
            this.btnSendBlastLayerToEditor.Text = "Send BlastLayer to Editor";
            this.btnSendBlastLayerToEditor.UseVisualStyleBackColor = false;
            this.btnSendBlastLayerToEditor.Click += new System.EventHandler(this.btnSendBlastLayerToEditor_Click);
            // 
            // cbUseHex
            // 
            this.cbUseHex.AutoSize = true;
            this.cbUseHex.Checked = true;
            this.cbUseHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUseHex.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.cbUseHex.ForeColor = System.Drawing.Color.White;
            this.cbUseHex.Location = new System.Drawing.Point(5, 237);
            this.cbUseHex.Name = "cbUseHex";
            this.cbUseHex.Size = new System.Drawing.Size(114, 17);
            this.cbUseHex.TabIndex = 157;
            this.cbUseHex.Text = "Use Hexadecimal";
            this.cbUseHex.UseVisualStyleBackColor = true;
            this.cbUseHex.CheckedChanged += new System.EventHandler(this.cbUseHex_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbUseHex);
            this.panel1.Controls.Add(this.btnJustCorrupt);
            this.panel1.Controls.Add(this.btnSendBlastLayerToEditor);
            this.panel1.Controls.Add(this.btnLoadCorrupt);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(749, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(159, 366);
            this.panel1.TabIndex = 166;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.updownNudgeParam2);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.updownNudgeParam1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.updownNudgeEndAddress);
            this.panel2.Controls.Add(this.btnShiftBlastLayerUp);
            this.panel2.Controls.Add(this.btnShiftBlastLayerDown);
            this.panel2.Controls.Add(this.updownNudgeStartAddress);
            this.panel2.Location = new System.Drawing.Point(5, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(148, 195);
            this.panel2.TabIndex = 159;
            this.panel2.Tag = "color:normal";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(15, 151);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 15);
            this.label5.TabIndex = 172;
            this.label5.Text = "Param 2";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 15);
            this.label3.TabIndex = 171;
            this.label3.Text = "Param 1";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(15, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 170;
            this.label2.Text = "End Address";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 169;
            this.label1.Text = "Start Address";
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button5.ForeColor = System.Drawing.Color.Black;
            this.button5.Location = new System.Drawing.Point(110, 167);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(21, 20);
            this.button5.TabIndex = 167;
            this.button5.TabStop = false;
            this.button5.Tag = "color:light";
            this.button5.Text = "▶";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button6.ForeColor = System.Drawing.Color.Black;
            this.button6.Location = new System.Drawing.Point(18, 167);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(21, 20);
            this.button6.TabIndex = 168;
            this.button6.TabStop = false;
            this.button6.Tag = "color:light";
            this.button6.Text = "◀";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // updownNudgeParam2
            // 
            this.updownNudgeParam2.Hexadecimal = true;
            this.updownNudgeParam2.Location = new System.Drawing.Point(45, 167);
            this.updownNudgeParam2.Name = "updownNudgeParam2";
            this.updownNudgeParam2.Size = new System.Drawing.Size(59, 20);
            this.updownNudgeParam2.TabIndex = 166;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(110, 115);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(21, 20);
            this.button3.TabIndex = 164;
            this.button3.TabStop = false;
            this.button3.Tag = "color:light";
            this.button3.Text = "▶";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(18, 117);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(21, 20);
            this.button4.TabIndex = 165;
            this.button4.TabStop = false;
            this.button4.Tag = "color:light";
            this.button4.Text = "◀";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // updownNudgeParam1
            // 
            this.updownNudgeParam1.Hexadecimal = true;
            this.updownNudgeParam1.Location = new System.Drawing.Point(45, 117);
            this.updownNudgeParam1.Name = "updownNudgeParam1";
            this.updownNudgeParam1.Size = new System.Drawing.Size(59, 20);
            this.updownNudgeParam1.TabIndex = 163;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(110, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(21, 20);
            this.button1.TabIndex = 161;
            this.button1.TabStop = false;
            this.button1.Tag = "color:light";
            this.button1.Text = "▶";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(18, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(21, 20);
            this.button2.TabIndex = 162;
            this.button2.TabStop = false;
            this.button2.Tag = "color:light";
            this.button2.Text = "◀";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // updownNudgeEndAddress
            // 
            this.updownNudgeEndAddress.Hexadecimal = true;
            this.updownNudgeEndAddress.Location = new System.Drawing.Point(45, 69);
            this.updownNudgeEndAddress.Name = "updownNudgeEndAddress";
            this.updownNudgeEndAddress.Size = new System.Drawing.Size(59, 20);
            this.updownNudgeEndAddress.TabIndex = 160;
            // 
            // btnShiftBlastLayerUp
            // 
            this.btnShiftBlastLayerUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShiftBlastLayerUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerUp.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerUp.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerUp.Location = new System.Drawing.Point(110, 19);
            this.btnShiftBlastLayerUp.Name = "btnShiftBlastLayerUp";
            this.btnShiftBlastLayerUp.Size = new System.Drawing.Size(21, 20);
            this.btnShiftBlastLayerUp.TabIndex = 158;
            this.btnShiftBlastLayerUp.TabStop = false;
            this.btnShiftBlastLayerUp.Tag = "color:light";
            this.btnShiftBlastLayerUp.Text = "▶";
            this.btnShiftBlastLayerUp.UseVisualStyleBackColor = false;
            // 
            // btnShiftBlastLayerDown
            // 
            this.btnShiftBlastLayerDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShiftBlastLayerDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnShiftBlastLayerDown.FlatAppearance.BorderSize = 0;
            this.btnShiftBlastLayerDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShiftBlastLayerDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnShiftBlastLayerDown.ForeColor = System.Drawing.Color.Black;
            this.btnShiftBlastLayerDown.Location = new System.Drawing.Point(18, 19);
            this.btnShiftBlastLayerDown.Name = "btnShiftBlastLayerDown";
            this.btnShiftBlastLayerDown.Size = new System.Drawing.Size(21, 20);
            this.btnShiftBlastLayerDown.TabIndex = 159;
            this.btnShiftBlastLayerDown.TabStop = false;
            this.btnShiftBlastLayerDown.Tag = "color:light";
            this.btnShiftBlastLayerDown.Text = "◀";
            this.btnShiftBlastLayerDown.UseVisualStyleBackColor = false;
            // 
            // updownNudgeStartAddress
            // 
            this.updownNudgeStartAddress.Hexadecimal = true;
            this.updownNudgeStartAddress.Location = new System.Drawing.Point(45, 19);
            this.updownNudgeStartAddress.Name = "updownNudgeStartAddress";
            this.updownNudgeStartAddress.Size = new System.Drawing.Size(59, 20);
            this.updownNudgeStartAddress.TabIndex = 157;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 13);
            this.label4.TabIndex = 158;
            this.label4.Text = "Shift Selected Rows";
            // 
            // dgvBlastGenerator
            // 
            this.dgvBlastGenerator.AllowUserToResizeRows = false;
            this.dgvBlastGenerator.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBlastGenerator.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBlastGenerator.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBlastGenerator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlastGenerator.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvEnabled,
            this.dgvDomain,
            this.dgvType,
            this.dgvMode,
            this.dgvStepSize,
            this.dgvStartAddress,
            this.dgvEndAddress,
            this.dgvParam1,
            this.dgvParam2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Symbol", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBlastGenerator.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBlastGenerator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBlastGenerator.Location = new System.Drawing.Point(0, 24);
            this.dgvBlastGenerator.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBlastGenerator.Name = "dgvBlastGenerator";
            this.dgvBlastGenerator.RowHeadersVisible = false;
            this.dgvBlastGenerator.RowTemplate.Height = 24;
            this.dgvBlastGenerator.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBlastGenerator.Size = new System.Drawing.Size(749, 366);
            this.dgvBlastGenerator.TabIndex = 167;
            this.dgvBlastGenerator.Tag = "color:normal";
            // 
            // menuStripEx1
            // 
            this.menuStripEx1.ClickThrough = true;
            this.menuStripEx1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blastLayerToolStripMenuItem});
            this.menuStripEx1.Location = new System.Drawing.Point(0, 0);
            this.menuStripEx1.Name = "menuStripEx1";
            this.menuStripEx1.Size = new System.Drawing.Size(908, 24);
            this.menuStripEx1.TabIndex = 168;
            this.menuStripEx1.Text = "menuStripEx1";
            // 
            // blastLayerToolStripMenuItem
            // 
            this.blastLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFromFileblToolStripMenuItem,
            this.saveAsToFileblToolStripMenuItem,
            this.importBlastlayerblToolStripMenuItem});
            this.blastLayerToolStripMenuItem.Name = "blastLayerToolStripMenuItem";
            this.blastLayerToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.blastLayerToolStripMenuItem.Text = "File";
            // 
            // loadFromFileblToolStripMenuItem
            // 
            this.loadFromFileblToolStripMenuItem.Name = "loadFromFileblToolStripMenuItem";
            this.loadFromFileblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.loadFromFileblToolStripMenuItem.Text = "&Load From File (.bg)";
            // 
            // saveAsToFileblToolStripMenuItem
            // 
            this.saveAsToFileblToolStripMenuItem.Name = "saveAsToFileblToolStripMenuItem";
            this.saveAsToFileblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.saveAsToFileblToolStripMenuItem.Text = "&Save As to File (.bg)";
            // 
            // importBlastlayerblToolStripMenuItem
            // 
            this.importBlastlayerblToolStripMenuItem.Name = "importBlastlayerblToolStripMenuItem";
            this.importBlastlayerblToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
            this.importBlastlayerblToolStripMenuItem.Text = "&Import Generation Params (.bg)";
            // 
            // dgvEnabled
            // 
            this.dgvEnabled.FillWeight = 35F;
            this.dgvEnabled.HeaderText = "Enabled";
            this.dgvEnabled.Name = "dgvEnabled";
            // 
            // dgvDomain
            // 
            this.dgvDomain.FillWeight = 55F;
            this.dgvDomain.HeaderText = "Domain";
            this.dgvDomain.Name = "dgvDomain";
            this.dgvDomain.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvType
            // 
            this.dgvType.FillWeight = 55F;
            this.dgvType.HeaderText = "Type";
            this.dgvType.Items.AddRange(new object[] {
            "BlastByte",
            "BlastCheat",
            "BlastPipe"});
            this.dgvType.Name = "dgvType";
            this.dgvType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvMode
            // 
            this.dgvMode.FillWeight = 55F;
            this.dgvMode.HeaderText = "Mode";
            this.dgvMode.Items.AddRange(new object[] {
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
            this.dgvMode.Name = "dgvMode";
            this.dgvMode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvStepSize
            // 
            this.dgvStepSize.FillWeight = 50F;
            this.dgvStepSize.HeaderText = "Step Size";
            this.dgvStepSize.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvStepSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dgvStepSize.Name = "dgvStepSize";
            // 
            // dgvStartAddress
            // 
            this.dgvStartAddress.FillWeight = 50F;
            this.dgvStartAddress.HeaderText = "Start Address";
            this.dgvStartAddress.Hexadecimal = true;
            this.dgvStartAddress.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvStartAddress.Name = "dgvStartAddress";
            this.dgvStartAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvEndAddress
            // 
            this.dgvEndAddress.FillWeight = 50F;
            this.dgvEndAddress.HeaderText = "End Address";
            this.dgvEndAddress.Hexadecimal = true;
            this.dgvEndAddress.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvEndAddress.Name = "dgvEndAddress";
            this.dgvEndAddress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // dgvParam1
            // 
            this.dgvParam1.FillWeight = 50F;
            this.dgvParam1.HeaderText = "Param 1";
            this.dgvParam1.Hexadecimal = true;
            this.dgvParam1.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvParam1.Name = "dgvParam1";
            // 
            // dgvParam2
            // 
            this.dgvParam2.FillWeight = 50F;
            this.dgvParam2.HeaderText = "Param 2";
            this.dgvParam2.Hexadecimal = true;
            this.dgvParam2.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.dgvParam2.Name = "dgvParam2";
            // 
            // RTC_BlastGenerator_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(908, 390);
            this.Controls.Add(this.dgvBlastGenerator);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStripEx1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(420, 350);
            this.Name = "RTC_BlastGenerator_Form";
            this.Tag = "color:dark";
            this.Text = "BlastLayer Generator";
            this.Load += new System.EventHandler(this.RTC_BlastGeneratorForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeParam1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeEndAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updownNudgeStartAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastGenerator)).EndInit();
            this.menuStripEx1.ResumeLayout(false);
            this.menuStripEx1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnLoadCorrupt;
		private System.Windows.Forms.Button btnJustCorrupt;
		private System.Windows.Forms.Button btnSendBlastLayerToEditor;
		private System.Windows.Forms.CheckBox cbUseHex;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView dgvBlastGenerator;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private NumericUpDownHexFix updownNudgeParam2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private NumericUpDownHexFix updownNudgeParam1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private NumericUpDownHexFix updownNudgeEndAddress;
		private System.Windows.Forms.Button btnShiftBlastLayerUp;
		private System.Windows.Forms.Button btnShiftBlastLayerDown;
		private NumericUpDownHexFix updownNudgeStartAddress;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private MenuStripEx menuStripEx1;
		private System.Windows.Forms.ToolStripMenuItem blastLayerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadFromFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToFileblToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importBlastlayerblToolStripMenuItem;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvEnabled;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvDomain;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvType;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvMode;
		private DataGridViewNumericUpDownColumn dgvStepSize;
		private DataGridViewNumericUpDownColumn dgvStartAddress;
		private DataGridViewNumericUpDownColumn dgvEndAddress;
		private DataGridViewNumericUpDownColumn dgvParam1;
		private DataGridViewNumericUpDownColumn dgvParam2;
	}
}
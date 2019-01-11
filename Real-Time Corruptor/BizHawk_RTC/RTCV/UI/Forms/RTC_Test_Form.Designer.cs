namespace RTCV.UI
{
	partial class RTC_Test_Form
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
            this.numericUpDownHexFix1 = new NumericUpDownHexFix();
            this.numericUpDownHexFix2 = new NumericUpDownHexFix();
            this.numericUpDownHexFix3 = new NumericUpDownHexFix();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new DataGridViewNumericUpDownColumn();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownHexFix1
            // 
            this.numericUpDownHexFix1.Location = new System.Drawing.Point(16, 39);
            this.numericUpDownHexFix1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownHexFix1.Name = "numericUpDownHexFix1";
            this.numericUpDownHexFix1.Size = new System.Drawing.Size(160, 22);
            this.numericUpDownHexFix1.TabIndex = 0;
            // 
            // numericUpDownHexFix2
            // 
            this.numericUpDownHexFix2.Hexadecimal = true;
            this.numericUpDownHexFix2.Location = new System.Drawing.Point(16, 100);
            this.numericUpDownHexFix2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownHexFix2.Name = "numericUpDownHexFix2";
            this.numericUpDownHexFix2.Size = new System.Drawing.Size(160, 22);
            this.numericUpDownHexFix2.TabIndex = 1;
            // 
            // numericUpDownHexFix3
            // 
            this.numericUpDownHexFix3.Location = new System.Drawing.Point(16, 171);
            this.numericUpDownHexFix3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDownHexFix3.Name = "numericUpDownHexFix3";
            this.numericUpDownHexFix3.Size = new System.Drawing.Size(160, 22);
            this.numericUpDownHexFix3.TabIndex = 2;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(184, 175);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 21);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "Toggle Hex";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "NumericUpDownHexFix Decimal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "NumericUpDownHexFix Hex";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 151);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "NumericUpDownHexFix";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            this.dataGridView1.Location = new System.Drawing.Point(335, 11);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(193, 185);
            this.dataGridView1.TabIndex = 7;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(536, 11);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(102, 21);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "Toggle Hex";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // RTC_Test_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 501);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.numericUpDownHexFix3);
            this.Controls.Add(this.numericUpDownHexFix2);
            this.Controls.Add(this.numericUpDownHexFix1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "RTC_Test_Form";
            this.Text = "RTC_Test_Form";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHexFix3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private NumericUpDownHexFix numericUpDownHexFix1;
		private NumericUpDownHexFix numericUpDownHexFix2;
		private NumericUpDownHexFix numericUpDownHexFix3;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.DataGridView dataGridView1;
		private DataGridViewNumericUpDownColumn Column1;
		private System.Windows.Forms.CheckBox checkBox2;
	}
}
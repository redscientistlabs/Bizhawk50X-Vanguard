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
            this.dgvBlastEditor = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBlastEditor
            // 
            this.dgvBlastEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBlastEditor.Location = new System.Drawing.Point(24, 74);
            this.dgvBlastEditor.Name = "dgvBlastEditor";
            this.dgvBlastEditor.RowTemplate.Height = 24;
            this.dgvBlastEditor.Size = new System.Drawing.Size(546, 347);
            this.dgvBlastEditor.TabIndex = 0;
            // 
            // RTC_NewBlastEditor_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 541);
            this.Controls.Add(this.dgvBlastEditor);
            this.Name = "RTC_NewBlastEditor_Form";
            this.Text = "RTC_NewBlastEditor_Form";
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlastEditor)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvBlastEditor;
	}
}
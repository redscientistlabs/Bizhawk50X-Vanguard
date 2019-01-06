namespace RTC
{
    partial class RTC_SettingsReroll_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_SettingsReroll_Form));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbRerollAddress = new System.Windows.Forms.CheckBox();
            this.cbRerollSourceAddress = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.cbRerollAddress);
            this.panel1.Controls.Add(this.cbRerollSourceAddress);
            this.panel1.Location = new System.Drawing.Point(29, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(178, 62);
            this.panel1.TabIndex = 140;
            this.panel1.Tag = "color:dark";
            // 
            // cbRerollAddress
            // 
            this.cbRerollAddress.AutoSize = true;
            this.cbRerollAddress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollAddress.ForeColor = System.Drawing.Color.White;
            this.cbRerollAddress.Location = new System.Drawing.Point(11, 33);
            this.cbRerollAddress.Name = "cbRerollAddress";
            this.cbRerollAddress.Size = new System.Drawing.Size(100, 17);
            this.cbRerollAddress.TabIndex = 1;
            this.cbRerollAddress.Text = "Reroll Address";
            this.cbRerollAddress.UseVisualStyleBackColor = true;
			// 
			// cbRerollSourceAddress
			// 
			this.cbRerollSourceAddress.AutoSize = true;
            this.cbRerollSourceAddress.Checked = true;
            this.cbRerollSourceAddress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRerollSourceAddress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbRerollSourceAddress.ForeColor = System.Drawing.Color.White;
            this.cbRerollSourceAddress.Location = new System.Drawing.Point(11, 10);
            this.cbRerollSourceAddress.Name = "cbRerollSourceAddress";
            this.cbRerollSourceAddress.Size = new System.Drawing.Size(138, 17);
            this.cbRerollSourceAddress.TabIndex = 0;
            this.cbRerollSourceAddress.Text = "Reroll Source Address";
            this.cbRerollSourceAddress.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(30, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 15);
            this.label4.TabIndex = 141;
            this.label4.Text = "Store Settings";
            // 
            // RTC_SettingsReroll_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(240, 129);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RTC_SettingsReroll_Form";
            this.ShowInTaskbar = false;
            this.Tag = "color:normal";
            this.Text = "General";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.CheckBox cbRerollAddress;
		public System.Windows.Forms.CheckBox cbRerollSourceAddress;
		private System.Windows.Forms.Label label4;
	}
}
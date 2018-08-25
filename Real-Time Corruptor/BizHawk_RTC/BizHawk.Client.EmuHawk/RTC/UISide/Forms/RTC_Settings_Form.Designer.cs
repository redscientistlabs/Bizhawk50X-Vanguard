namespace RTC
{
    partial class RTC_Settings_Form
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnRtcFactoryClean = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pnListBoxForm = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.label2.Location = new System.Drawing.Point(11, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 47);
            this.label2.TabIndex = 118;
            this.label2.Text = "Settings and tools";
            // 
            // btnRtcFactoryClean
            // 
            this.btnRtcFactoryClean.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRtcFactoryClean.FlatAppearance.BorderSize = 0;
            this.btnRtcFactoryClean.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRtcFactoryClean.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRtcFactoryClean.ForeColor = System.Drawing.Color.Black;
            this.btnRtcFactoryClean.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.reboot;
            this.btnRtcFactoryClean.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRtcFactoryClean.Location = new System.Drawing.Point(409, 465);
            this.btnRtcFactoryClean.Name = "btnRtcFactoryClean";
            this.btnRtcFactoryClean.Size = new System.Drawing.Size(232, 29);
            this.btnRtcFactoryClean.TabIndex = 127;
            this.btnRtcFactoryClean.Tag = "color:light";
            this.btnRtcFactoryClean.Text = "  RTC Factory Clean";
            this.btnRtcFactoryClean.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRtcFactoryClean.UseVisualStyleBackColor = false;
            this.btnRtcFactoryClean.Click += new System.EventHandler(this.btnRtcFactoryClean_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::BizHawk.Client.EmuHawk.Properties.Resources.undo;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(19, 465);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(215, 29);
            this.button1.TabIndex = 127;
            this.button1.Tag = "color:light";
            this.button1.Text = " Close Settings";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1971, 480);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 23);
            this.button2.TabIndex = 136;
            this.button2.Text = "Open Test Form";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pnListBoxForm
            // 
            this.pnListBoxForm.BackColor = System.Drawing.Color.Gray;
            this.pnListBoxForm.Location = new System.Drawing.Point(19, 66);
            this.pnListBoxForm.Name = "pnListBoxForm";
            this.pnListBoxForm.Size = new System.Drawing.Size(622, 378);
            this.pnListBoxForm.TabIndex = 137;
            this.pnListBoxForm.Tag = "color:normal";
            // 
            // RTC_Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.pnListBoxForm);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRtcFactoryClean);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_Settings_Form";
            this.Tag = "color:dark";
            this.Text = "RTC : Settings";
            this.Load += new System.EventHandler(this.RTC_Settings_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnRtcFactoryClean;
        public System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel pnListBoxForm;
	}
}
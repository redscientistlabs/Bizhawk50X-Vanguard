namespace RTCV.UI
{
    partial class RTC_SettingsAbout_Form
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
            this.lbRTC = new System.Windows.Forms.Label();
            this.lbVersion = new System.Windows.Forms.Label();
            this.lbProcess = new System.Windows.Forms.Label();
            this.lbConnectedTo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbRTC
            // 
            this.lbRTC.AutoSize = true;
            this.lbRTC.Font = new System.Drawing.Font("Segoe UI Semibold", 20F, System.Drawing.FontStyle.Bold);
            this.lbRTC.ForeColor = System.Drawing.Color.White;
            this.lbRTC.Location = new System.Drawing.Point(12, 9);
            this.lbRTC.Name = "lbRTC";
            this.lbRTC.Size = new System.Drawing.Size(267, 37);
            this.lbRTC.TabIndex = 0;
            this.lbRTC.Text = "Real-Time Corruptor";
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbVersion.ForeColor = System.Drawing.Color.White;
            this.lbVersion.Location = new System.Drawing.Point(14, 52);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(72, 21);
            this.lbVersion.TabIndex = 1;
            this.lbVersion.Text = "Version: ";
            // 
            // lbProcess
            // 
            this.lbProcess.AutoSize = true;
            this.lbProcess.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbProcess.ForeColor = System.Drawing.Color.White;
            this.lbProcess.Location = new System.Drawing.Point(14, 76);
            this.lbProcess.Name = "lbProcess";
            this.lbProcess.Size = new System.Drawing.Size(121, 21);
            this.lbProcess.TabIndex = 2;
            this.lbProcess.Text = "Process mode: ";
            // 
            // lbConnectedTo
            // 
            this.lbConnectedTo.AutoSize = true;
            this.lbConnectedTo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbConnectedTo.ForeColor = System.Drawing.Color.White;
            this.lbConnectedTo.Location = new System.Drawing.Point(14, 100);
            this.lbConnectedTo.Name = "lbConnectedTo";
            this.lbConnectedTo.Size = new System.Drawing.Size(118, 21);
            this.lbConnectedTo.TabIndex = 3;
            this.lbConnectedTo.Text = "Connected to: ";
            // 
            // RTC_SettingsAbout_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(458, 378);
            this.Controls.Add(this.lbConnectedTo);
            this.Controls.Add(this.lbProcess);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.lbRTC);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_SettingsAbout_Form";
            this.Tag = "color:dark";
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HandleFormClosing);
            this.Load += new System.EventHandler(this.RTC_SettingsAbout_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HandleMouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.Label lbRTC;
		private System.Windows.Forms.Label lbVersion;
		private System.Windows.Forms.Label lbProcess;
		private System.Windows.Forms.Label lbConnectedTo;
	}
}
namespace WindowsGlitchHarvester
{
    partial class WGH_HookProcessForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WGH_HookProcessForm));
			this.lbProcesses = new System.Windows.Forms.ListBox();
			this.btnSendList = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbProcesses
			// 
			this.lbProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbProcesses.BackColor = System.Drawing.Color.Black;
			this.lbProcesses.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.lbProcesses.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.lbProcesses.ForeColor = System.Drawing.Color.White;
			this.lbProcesses.FormattingEnabled = true;
			this.lbProcesses.Location = new System.Drawing.Point(0, 0);
			this.lbProcesses.Name = "lbProcesses";
			this.lbProcesses.ScrollAlwaysVisible = true;
			this.lbProcesses.Size = new System.Drawing.Size(320, 429);
			this.lbProcesses.TabIndex = 0;
			// 
			// btnSendList
			// 
			this.btnSendList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnSendList.BackColor = System.Drawing.Color.Black;
			this.btnSendList.FlatAppearance.BorderSize = 0;
			this.btnSendList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSendList.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.btnSendList.ForeColor = System.Drawing.Color.OrangeRed;
			this.btnSendList.Location = new System.Drawing.Point(106, 491);
			this.btnSendList.Name = "btnSendList";
			this.btnSendList.Size = new System.Drawing.Size(196, 23);
			this.btnSendList.TabIndex = 1;
			this.btnSendList.Text = "Hook Process to Glitch Harvester";
			this.btnSendList.UseVisualStyleBackColor = false;
			this.btnSendList.Click += new System.EventHandler(this.btnSendList_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnCancel.BackColor = System.Drawing.Color.Black;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.btnCancel.Location = new System.Drawing.Point(12, 491);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(63, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(10, 440);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(293, 39);
			this.label1.TabIndex = 3;
			this.label1.Text = "WARNING: Corrupting a game process with online \r\nfeatures can result in a ban. Bl" +
    "ock network access via \r\nthe Windows Firewall in the Control Panel (If necessary" +
    ")";
			// 
			// WGH_HookProcessForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
			this.ClientSize = new System.Drawing.Size(319, 521);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSendList);
			this.Controls.Add(this.lbProcesses);
			this.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "WGH_HookProcessForm";
			this.Text = "Select Process";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WGH_SelectMultiple_FormClosing);
			this.Load += new System.EventHandler(this.WGH_HookToProcess_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbProcesses;
        private System.Windows.Forms.Button btnSendList;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
	}
}
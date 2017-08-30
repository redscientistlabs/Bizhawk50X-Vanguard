namespace RTC
{
    partial class RTC_VmdPool_Form
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
            this.lbLoadedVMDs = new System.Windows.Forms.ListBox();
            this.btnUnloadVMD = new System.Windows.Forms.Button();
            this.btnLoadVMD = new System.Windows.Forms.Button();
            this.btnSaveVMD = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLoadedVMDs
            // 
            this.lbLoadedVMDs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lbLoadedVMDs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbLoadedVMDs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbLoadedVMDs.ForeColor = System.Drawing.Color.White;
            this.lbLoadedVMDs.FormattingEnabled = true;
            this.lbLoadedVMDs.Location = new System.Drawing.Point(14, 14);
            this.lbLoadedVMDs.Margin = new System.Windows.Forms.Padding(5);
            this.lbLoadedVMDs.Name = "lbLoadedVMDs";
            this.lbLoadedVMDs.Size = new System.Drawing.Size(168, 182);
            this.lbLoadedVMDs.TabIndex = 12;
            this.lbLoadedVMDs.Tag = "color:dark";
            // 
            // btnUnloadVMD
            // 
            this.btnUnloadVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUnloadVMD.FlatAppearance.BorderSize = 0;
            this.btnUnloadVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUnloadVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnUnloadVMD.ForeColor = System.Drawing.Color.Black;
            this.btnUnloadVMD.Location = new System.Drawing.Point(12, 210);
            this.btnUnloadVMD.Name = "btnUnloadVMD";
            this.btnUnloadVMD.Size = new System.Drawing.Size(170, 28);
            this.btnUnloadVMD.TabIndex = 13;
            this.btnUnloadVMD.TabStop = false;
            this.btnUnloadVMD.Tag = "color:light";
            this.btnUnloadVMD.Text = "Unload Selected VMD";
            this.btnUnloadVMD.UseVisualStyleBackColor = false;
            // 
            // btnLoadVMD
            // 
            this.btnLoadVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadVMD.FlatAppearance.BorderSize = 0;
            this.btnLoadVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnLoadVMD.ForeColor = System.Drawing.Color.Black;
            this.btnLoadVMD.Location = new System.Drawing.Point(190, 14);
            this.btnLoadVMD.Name = "btnLoadVMD";
            this.btnLoadVMD.Size = new System.Drawing.Size(188, 28);
            this.btnLoadVMD.TabIndex = 14;
            this.btnLoadVMD.TabStop = false;
            this.btnLoadVMD.Tag = "color:light";
            this.btnLoadVMD.Text = "Load VMD from File";
            this.btnLoadVMD.UseVisualStyleBackColor = false;
            // 
            // btnSaveVMD
            // 
            this.btnSaveVMD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSaveVMD.FlatAppearance.BorderSize = 0;
            this.btnSaveVMD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveVMD.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSaveVMD.ForeColor = System.Drawing.Color.Black;
            this.btnSaveVMD.Location = new System.Drawing.Point(190, 48);
            this.btnSaveVMD.Name = "btnSaveVMD";
            this.btnSaveVMD.Size = new System.Drawing.Size(188, 28);
            this.btnSaveVMD.TabIndex = 15;
            this.btnSaveVMD.TabStop = false;
            this.btnSaveVMD.Tag = "color:light";
            this.btnSaveVMD.Text = "Save Selected VMD to File";
            this.btnSaveVMD.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(194, 82);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(184, 156);
            this.groupBox2.TabIndex = 129;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected VMD Summary";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(8, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 93;
            this.label9.Text = "#####";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 89;
            this.label1.Text = "VMD Size:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 95;
            this.label2.Text = "#####";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 17);
            this.label3.TabIndex = 94;
            this.label3.Text = "Real Domain:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(8, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 97;
            this.label4.Text = "#####";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(8, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 96;
            this.label5.Text = "Source Core:";
            // 
            // RTC_VmdPool_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(390, 250);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSaveVMD);
            this.Controls.Add(this.btnLoadVMD);
            this.Controls.Add(this.btnUnloadVMD);
            this.Controls.Add(this.lbLoadedVMDs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_VmdPool_Form";
            this.Text = "RTC_VmdPool_Form";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lbLoadedVMDs;
        private System.Windows.Forms.Button btnUnloadVMD;
        private System.Windows.Forms.Button btnLoadVMD;
        private System.Windows.Forms.Button btnSaveVMD;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.Label label1;
    }
}
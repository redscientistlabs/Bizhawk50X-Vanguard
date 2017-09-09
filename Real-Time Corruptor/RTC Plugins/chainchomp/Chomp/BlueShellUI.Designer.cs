namespace ChompAndFriends
{
    partial class BlueShellUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.backgroundFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.byteInterval = new System.Windows.Forms.NumericUpDown();
            this.byteFind = new System.Windows.Forms.NumericUpDown();
            this.byteReplace = new System.Windows.Forms.NumericUpDown();
            this.blueShellToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteFind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteReplace)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.blueshell;
            this.backgroundFlowLayout.Controls.Add(this.byteInterval);
            this.backgroundFlowLayout.Controls.Add(this.byteFind);
            this.backgroundFlowLayout.Controls.Add(this.byteReplace);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(58)))), ((int)(((byte)(210)))));
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.White;
            this.byteInterval.Location = new System.Drawing.Point(50, 100);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(50, 100, 3, 3);
            this.byteInterval.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.byteInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.byteInterval.Name = "byteInterval";
            this.byteInterval.Size = new System.Drawing.Size(157, 28);
            this.byteInterval.TabIndex = 1;
            this.byteInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.blueShellToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // byteFind
            // 
            this.byteFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(58)))), ((int)(((byte)(210)))));
            this.byteFind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteFind.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteFind.ForeColor = System.Drawing.Color.White;
            this.byteFind.Hexadecimal = true;
            this.byteFind.Location = new System.Drawing.Point(310, 100);
            this.byteFind.Margin = new System.Windows.Forms.Padding(100, 100, 3, 3);
            this.byteFind.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.byteFind.Name = "byteFind";
            this.byteFind.Size = new System.Drawing.Size(47, 28);
            this.byteFind.TabIndex = 3;
            this.byteFind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.blueShellToolTip.SetToolTip(this.byteFind, "Corrupt if byte equals $0 (0)");
            this.byteFind.ValueChanged += new System.EventHandler(this.byteFind_ValueChanged);
            // 
            // byteReplace
            // 
            this.byteReplace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(58)))), ((int)(((byte)(210)))));
            this.byteReplace.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteReplace.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteReplace.ForeColor = System.Drawing.Color.White;
            this.byteReplace.Hexadecimal = true;
            this.byteReplace.Location = new System.Drawing.Point(410, 100);
            this.byteReplace.Margin = new System.Windows.Forms.Padding(50, 100, 3, 3);
            this.byteReplace.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.byteReplace.Name = "byteReplace";
            this.byteReplace.Size = new System.Drawing.Size(47, 28);
            this.byteReplace.TabIndex = 4;
            this.byteReplace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.blueShellToolTip.SetToolTip(this.byteReplace, "Set byte to $FF (255)");
            this.byteReplace.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.byteReplace.ValueChanged += new System.EventHandler(this.byteReplace_ValueChanged);
            // 
            // BlueShellUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BlueShellUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteFind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteReplace)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        internal System.Windows.Forms.NumericUpDown byteFind;
        internal System.Windows.Forms.NumericUpDown byteReplace;
        private System.Windows.Forms.ToolTip blueShellToolTip;
    }
}

namespace ChompAndFriends
{
    partial class BobombUI
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
            this.targetOffset = new System.Windows.Forms.NumericUpDown();
            this.blastRadius = new System.Windows.Forms.NumericUpDown();
            this.power = new System.Windows.Forms.NumericUpDown();
            this.bobombToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blastRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.power)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.bobomb;
            this.backgroundFlowLayout.Controls.Add(this.targetOffset);
            this.backgroundFlowLayout.Controls.Add(this.blastRadius);
            this.backgroundFlowLayout.Controls.Add(this.power);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // targetOffset
            // 
            this.targetOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(171)))), ((int)(((byte)(131)))));
            this.targetOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.targetOffset.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetOffset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(70)))), ((int)(((byte)(10)))));
            this.targetOffset.Hexadecimal = true;
            this.targetOffset.Location = new System.Drawing.Point(40, 109);
            this.targetOffset.Margin = new System.Windows.Forms.Padding(40, 109, 3, 3);
            this.targetOffset.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.targetOffset.Name = "targetOffset";
            this.targetOffset.Size = new System.Drawing.Size(157, 28);
            this.targetOffset.TabIndex = 1;
            this.targetOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bobombToolTip.SetToolTip(this.targetOffset, "Centre blast at 0x10 (16 bytes from the start)");
            this.targetOffset.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.targetOffset.ValueChanged += new System.EventHandler(this.targetOffset_ValueChanged);
            // 
            // blastRadius
            // 
            this.blastRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(214)))), ((int)(((byte)(114)))));
            this.blastRadius.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.blastRadius.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blastRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(10)))));
            this.blastRadius.Hexadecimal = true;
            this.blastRadius.Location = new System.Drawing.Point(240, 109);
            this.blastRadius.Margin = new System.Windows.Forms.Padding(40, 109, 3, 3);
            this.blastRadius.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.blastRadius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.blastRadius.Name = "blastRadius";
            this.blastRadius.Size = new System.Drawing.Size(157, 28);
            this.blastRadius.TabIndex = 2;
            this.blastRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bobombToolTip.SetToolTip(this.blastRadius, "Corrupt $10 (16) bytes either side of target");
            this.blastRadius.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.blastRadius.ValueChanged += new System.EventHandler(this.blastRadius_ValueChanged);
            // 
            // power
            // 
            this.power.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.power.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.power.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.power.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.power.Hexadecimal = true;
            this.power.Location = new System.Drawing.Point(495, 109);
            this.power.Margin = new System.Windows.Forms.Padding(95, 109, 3, 3);
            this.power.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.power.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.power.Name = "power";
            this.power.Size = new System.Drawing.Size(47, 28);
            this.power.TabIndex = 3;
            this.power.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bobombToolTip.SetToolTip(this.power, "Set corrupted bytes between $1 (1) and original values depending on distance from" +
        " blast centre");
            this.power.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.power.ValueChanged += new System.EventHandler(this.power_ValueChanged);
            // 
            // BobombUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BobombUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.targetOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blastRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.power)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown targetOffset;
        internal System.Windows.Forms.NumericUpDown blastRadius;
        internal System.Windows.Forms.NumericUpDown power;
        private System.Windows.Forms.ToolTip bobombToolTip;
    }
}

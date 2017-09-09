namespace ChompAndFriends
{
    partial class GreenShellUI
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
            this.shift = new System.Windows.Forms.NumericUpDown();
            this.greenShellToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.shift)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.greenShell;
            this.backgroundFlowLayout.Controls.Add(this.byteInterval);
            this.backgroundFlowLayout.Controls.Add(this.shift);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            this.greenShellToolTip.SetToolTip(this.backgroundFlowLayout, "Corrupt every 16th byte");
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(173)))), ((int)(((byte)(20)))));
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.White;
            this.byteInterval.Location = new System.Drawing.Point(50, 80);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(50, 80, 3, 3);
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
            this.greenShellToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // shift
            // 
            this.shift.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(173)))), ((int)(((byte)(20)))));
            this.shift.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.shift.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shift.ForeColor = System.Drawing.Color.White;
            this.shift.Location = new System.Drawing.Point(260, 80);
            this.shift.Margin = new System.Windows.Forms.Padding(50, 80, 3, 3);
            this.shift.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.shift.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.shift.Name = "shift";
            this.shift.Size = new System.Drawing.Size(81, 28);
            this.shift.TabIndex = 2;
            this.shift.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.greenShellToolTip.SetToolTip(this.shift, "Shift bytes 1 place to the left");
            this.shift.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.shift.ValueChanged += new System.EventHandler(this.shift_ValueChanged);
            // 
            // GreenShellUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GreenShellUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.shift)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        internal System.Windows.Forms.NumericUpDown shift;
        private System.Windows.Forms.ToolTip greenShellToolTip;
    }
}

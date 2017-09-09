namespace ChompAndFriends
{
    partial class PSwitchUI
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
            this.pSwitchToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.pswitch;
            this.backgroundFlowLayout.Controls.Add(this.byteInterval);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(48)))), ((int)(((byte)(92)))));
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.White;
            this.byteInterval.Location = new System.Drawing.Point(221, 86);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(221, 86, 3, 3);
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
            this.pSwitchToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // PSwitchUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.Name = "PSwitchUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        private System.Windows.Forms.ToolTip pSwitchToolTip;
    }
}

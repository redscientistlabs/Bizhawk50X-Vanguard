namespace ChompAndFriends
{
    partial class ChompUI
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
            this.backgroundFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.byteInterval = new System.Windows.Forms.NumericUpDown();
            this.byteValue = new System.Windows.Forms.NumericUpDown();
            this.chompToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteValue)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayoutPanel
            // 
            this.backgroundFlowLayoutPanel.BackgroundImage = global::ChompAndFriends.Properties.Resources.chomp;
            this.backgroundFlowLayoutPanel.Controls.Add(this.byteInterval);
            this.backgroundFlowLayoutPanel.Controls.Add(this.byteValue);
            this.backgroundFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayoutPanel.Name = "backgroundFlowLayoutPanel";
            this.backgroundFlowLayoutPanel.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayoutPanel.TabIndex = 1;
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.Black;
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.Crimson;
            this.byteInterval.Location = new System.Drawing.Point(20, 30);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(20, 30, 3, 3);
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
            this.byteInterval.TabIndex = 0;
            this.byteInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.chompToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // byteValue
            // 
            this.byteValue.BackColor = System.Drawing.Color.Black;
            this.byteValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteValue.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteValue.ForeColor = System.Drawing.Color.Crimson;
            this.byteValue.Hexadecimal = true;
            this.byteValue.Location = new System.Drawing.Point(240, 30);
            this.byteValue.Margin = new System.Windows.Forms.Padding(60, 30, 3, 3);
            this.byteValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.byteValue.Name = "byteValue";
            this.byteValue.Size = new System.Drawing.Size(47, 28);
            this.byteValue.TabIndex = 2;
            this.byteValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.chompToolTip.SetToolTip(this.byteValue, "Set byte to $0 (0)");
            this.byteValue.ValueChanged += new System.EventHandler(this.byteValue_ValueChanged);
            // 
            // ChompUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ChompUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayoutPanel;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        internal System.Windows.Forms.NumericUpDown byteValue;
        private System.Windows.Forms.ToolTip chompToolTip;
    }
}

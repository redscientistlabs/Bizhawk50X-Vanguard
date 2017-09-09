namespace ChompAndFriends
{
    partial class BlooperUI
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
            this.logicComboBox = new System.Windows.Forms.ComboBox();
            this.blooperToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.blooper;
            this.backgroundFlowLayout.Controls.Add(this.byteInterval);
            this.backgroundFlowLayout.Controls.Add(this.logicComboBox);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            this.backgroundFlowLayout.WrapContents = false;
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.White;
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(140)))), ((int)(((byte)(152)))));
            this.byteInterval.Location = new System.Drawing.Point(80, 80);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(80, 80, 3, 3);
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
            this.blooperToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // logicComboBox
            // 
            this.logicComboBox.BackColor = System.Drawing.Color.White;
            this.logicComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logicComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logicComboBox.FormattingEnabled = true;
            this.logicComboBox.Items.AddRange(new object[] {
            "AND",
            "OR",
            "XOR",
            "<<",
            ">>"});
            this.logicComboBox.Location = new System.Drawing.Point(164, 114);
            this.logicComboBox.Margin = new System.Windows.Forms.Padding(164, 3, 3, 3);
            this.logicComboBox.Name = "logicComboBox";
            this.logicComboBox.Size = new System.Drawing.Size(73, 28);
            this.logicComboBox.TabIndex = 2;
            this.logicComboBox.SelectedIndexChanged += new System.EventHandler(this.logicComboBox_SelectedIndexChanged);
            // 
            // BlooperUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "BlooperUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        public System.Windows.Forms.ComboBox logicComboBox;
        private System.Windows.Forms.ToolTip blooperToolTip;
    }
}

namespace ChompAndFriends
{
    partial class KirbyUI
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
            this.skipOver = new System.Windows.Forms.NumericUpDown();
            this.sampleSize = new System.Windows.Forms.NumericUpDown();
            this.pasteAt = new System.Windows.Forms.NumericUpDown();
            this.loop = new System.Windows.Forms.CheckBox();
            this.kirbyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skipOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasteAt)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.kirby;
            this.backgroundFlowLayout.Controls.Add(this.skipOver);
            this.backgroundFlowLayout.Controls.Add(this.sampleSize);
            this.backgroundFlowLayout.Controls.Add(this.pasteAt);
            this.backgroundFlowLayout.Controls.Add(this.loop);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // skipOver
            // 
            this.skipOver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(48)))), ((int)(((byte)(69)))));
            this.skipOver.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skipOver.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skipOver.ForeColor = System.Drawing.Color.White;
            this.skipOver.Location = new System.Drawing.Point(80, 150);
            this.skipOver.Margin = new System.Windows.Forms.Padding(80, 150, 3, 3);
            this.skipOver.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.skipOver.Name = "skipOver";
            this.skipOver.Size = new System.Drawing.Size(119, 28);
            this.skipOver.TabIndex = 1;
            this.skipOver.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.kirbyToolTip.SetToolTip(this.skipOver, "Copy offset = 0x10 (ignore the first 16 bytes)");
            this.skipOver.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.skipOver.ValueChanged += new System.EventHandler(this.skipOver_ValueChanged);
            // 
            // sampleSize
            // 
            this.sampleSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(48)))), ((int)(((byte)(69)))));
            this.sampleSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sampleSize.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sampleSize.ForeColor = System.Drawing.Color.White;
            this.sampleSize.Location = new System.Drawing.Point(282, 120);
            this.sampleSize.Margin = new System.Windows.Forms.Padding(80, 120, 3, 3);
            this.sampleSize.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.sampleSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sampleSize.Name = "sampleSize";
            this.sampleSize.Size = new System.Drawing.Size(119, 28);
            this.sampleSize.TabIndex = 2;
            this.sampleSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.kirbyToolTip.SetToolTip(this.sampleSize, "Sample length = $100 (copy the next 256 bytes)");
            this.sampleSize.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.sampleSize.ValueChanged += new System.EventHandler(this.sampleSize_ValueChanged);
            // 
            // pasteAt
            // 
            this.pasteAt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(48)))), ((int)(((byte)(69)))));
            this.pasteAt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pasteAt.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pasteAt.ForeColor = System.Drawing.Color.White;
            this.pasteAt.Location = new System.Drawing.Point(464, 70);
            this.pasteAt.Margin = new System.Windows.Forms.Padding(60, 70, 0, 0);
            this.pasteAt.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.pasteAt.Name = "pasteAt";
            this.pasteAt.Size = new System.Drawing.Size(119, 28);
            this.pasteAt.TabIndex = 3;
            this.pasteAt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.kirbyToolTip.SetToolTip(this.pasteAt, "Paste offset = 0x80 (paste after the next 128 bytes)");
            this.pasteAt.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.pasteAt.ValueChanged += new System.EventHandler(this.pasteAt_ValueChanged);
            // 
            // loop
            // 
            this.loop.AutoSize = true;
            this.loop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(48)))), ((int)(((byte)(69)))));
            this.loop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(14)))), ((int)(((byte)(33)))));
            this.loop.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(48)))), ((int)(((byte)(69)))));
            this.loop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(14)))), ((int)(((byte)(33)))));
            this.loop.Location = new System.Drawing.Point(583, 170);
            this.loop.Margin = new System.Windows.Forms.Padding(0, 170, 0, 0);
            this.loop.Name = "loop";
            this.loop.Size = new System.Drawing.Size(12, 11);
            this.loop.TabIndex = 4;
            this.kirbyToolTip.SetToolTip(this.loop, "Copy once (best for large sizes)");
            this.loop.UseVisualStyleBackColor = false;
            this.loop.CheckedChanged += new System.EventHandler(this.loop_CheckedChanged);
            // 
            // KirbyUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "KirbyUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            this.backgroundFlowLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skipOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasteAt)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown skipOver;
        internal System.Windows.Forms.NumericUpDown sampleSize;
        internal System.Windows.Forms.NumericUpDown pasteAt;
        private System.Windows.Forms.ToolTip kirbyToolTip;
        public System.Windows.Forms.CheckBox loop;
    }
}

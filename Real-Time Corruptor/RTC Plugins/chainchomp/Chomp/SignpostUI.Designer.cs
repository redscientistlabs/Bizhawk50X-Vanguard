namespace ChompAndFriends
{
    partial class SignpostUI
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
            this.offset = new System.Windows.Forms.NumericUpDown();
            this.textBox = new System.Windows.Forms.TextBox();
            this.signpostToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offset)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.signpost;
            this.backgroundFlowLayout.Controls.Add(this.offset);
            this.backgroundFlowLayout.Controls.Add(this.textBox);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // offset
            // 
            this.offset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(135)))), ((int)(((byte)(91)))));
            this.offset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.offset.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(36)))), ((int)(((byte)(8)))));
            this.offset.Hexadecimal = true;
            this.offset.Location = new System.Drawing.Point(120, 65);
            this.offset.Margin = new System.Windows.Forms.Padding(120, 65, 3, 3);
            this.offset.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(103, 22);
            this.offset.TabIndex = 3;
            this.offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.signpostToolTip.SetToolTip(this.offset, "Insert text at 0x5A33");
            this.offset.Value = new decimal(new int[] {
            23091,
            0,
            0,
            0});
            this.offset.ValueChanged += new System.EventHandler(this.offset_ValueChanged);
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(135)))), ((int)(((byte)(91)))));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(36)))), ((int)(((byte)(8)))));
            this.textBox.Location = new System.Drawing.Point(120, 100);
            this.textBox.Margin = new System.Windows.Forms.Padding(120, 10, 3, 3);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(259, 84);
            this.textBox.TabIndex = 4;
            this.textBox.Text = "The quick yellow keaton jumped over the lazy cucco.";
            this.signpostToolTip.SetToolTip(this.textBox, "Characters to insert (encoded as ASCII)");
            // 
            // SignpostUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SignpostUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            this.backgroundFlowLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        public System.Windows.Forms.NumericUpDown offset;
        private System.Windows.Forms.ToolTip signpostToolTip;
        public System.Windows.Forms.TextBox textBox;
    }
}

namespace ChompAndFriends
{
    partial class RedShellUI
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
            this.redShellToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.note = new System.Windows.Forms.TextBox();
            this.backgroundFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.offset = new System.Windows.Forms.NumericUpDown();
            this.byteValue = new System.Windows.Forms.NumericUpDown();
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteValue)).BeginInit();
            this.SuspendLayout();
            // 
            // note
            // 
            this.note.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.note.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.note.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.note.ForeColor = System.Drawing.Color.White;
            this.note.Location = new System.Drawing.Point(246, 95);
            this.note.Margin = new System.Windows.Forms.Padding(20, 95, 3, 3);
            this.note.MaxLength = 0;
            this.note.Name = "note";
            this.note.Size = new System.Drawing.Size(235, 13);
            this.note.TabIndex = 4;
            this.redShellToolTip.SetToolTip(this.note, "\"That\'s why you always leave a note!\"");
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.redShell;
            this.backgroundFlowLayout.Controls.Add(this.offset);
            this.backgroundFlowLayout.Controls.Add(this.byteValue);
            this.backgroundFlowLayout.Controls.Add(this.note);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // offset
            // 
            this.offset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.offset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.offset.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offset.ForeColor = System.Drawing.Color.White;
            this.offset.Hexadecimal = true;
            this.offset.Location = new System.Drawing.Point(50, 90);
            this.offset.Margin = new System.Windows.Forms.Padding(50, 90, 3, 3);
            this.offset.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.offset.Name = "offset";
            this.offset.Size = new System.Drawing.Size(103, 22);
            this.offset.TabIndex = 2;
            this.offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.redShellToolTip.SetToolTip(this.offset, "Modify the value at 0xF0");
            this.offset.Value = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.offset.ValueChanged += new System.EventHandler(this.offset_ValueChanged);
            // 
            // byteValue
            // 
            this.byteValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.byteValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteValue.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteValue.ForeColor = System.Drawing.Color.White;
            this.byteValue.Hexadecimal = true;
            this.byteValue.Location = new System.Drawing.Point(176, 90);
            this.byteValue.Margin = new System.Windows.Forms.Padding(20, 90, 3, 3);
            this.byteValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.byteValue.Name = "byteValue";
            this.byteValue.Size = new System.Drawing.Size(47, 22);
            this.byteValue.TabIndex = 3;
            this.byteValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.redShellToolTip.SetToolTip(this.byteValue, "Set byte to $58 (88)");
            this.byteValue.Value = new decimal(new int[] {
            88,
            0,
            0,
            0});
            this.byteValue.ValueChanged += new System.EventHandler(this.byteValue_ValueChanged);
            // 
            // RedShellUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RedShellUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            this.backgroundFlowLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.byteValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        private System.Windows.Forms.ToolTip redShellToolTip;
        public System.Windows.Forms.TextBox note;
        public System.Windows.Forms.NumericUpDown offset;
        public System.Windows.Forms.NumericUpDown byteValue;
    }
}

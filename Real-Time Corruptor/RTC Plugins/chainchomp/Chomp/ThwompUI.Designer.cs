namespace ChompAndFriends
{
    partial class ThwompUI
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
            this.thwompToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.byteInterval = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.offsetRadioButton = new System.Windows.Forms.RadioButton();
            this.byteValueRadioButton = new System.Windows.Forms.RadioButton();
            this.prevWaveButton = new System.Windows.Forms.Button();
            this.waveformPreview = new System.Windows.Forms.FlowLayoutPanel();
            this.nextWaveButton = new System.Windows.Forms.Button();
            this.backgroundFlowLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.thwomp;
            this.backgroundFlowLayout.Controls.Add(this.byteInterval);
            this.backgroundFlowLayout.Controls.Add(this.flowLayoutPanel1);
            this.backgroundFlowLayout.Controls.Add(this.prevWaveButton);
            this.backgroundFlowLayout.Controls.Add(this.waveformPreview);
            this.backgroundFlowLayout.Controls.Add(this.nextWaveButton);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // byteInterval
            // 
            this.byteInterval.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(189)))), ((int)(((byte)(222)))));
            this.byteInterval.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.byteInterval.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.byteInterval.ForeColor = System.Drawing.Color.White;
            this.byteInterval.Location = new System.Drawing.Point(20, 50);
            this.byteInterval.Margin = new System.Windows.Forms.Padding(20, 50, 3, 3);
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
            this.thwompToolTip.SetToolTip(this.byteInterval, "Corrupt every 16th byte");
            this.byteInterval.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.byteInterval.ValueChanged += new System.EventHandler(this.byteInterval_ValueChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.offsetRadioButton);
            this.flowLayoutPanel1.Controls.Add(this.byteValueRadioButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(190, 47);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(10, 47, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(108, 68);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // offsetRadioButton
            // 
            this.offsetRadioButton.AutoSize = true;
            this.offsetRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.offsetRadioButton.Checked = true;
            this.offsetRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.offsetRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.offsetRadioButton.Location = new System.Drawing.Point(3, 3);
            this.offsetRadioButton.Name = "offsetRadioButton";
            this.offsetRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.offsetRadioButton.Size = new System.Drawing.Size(100, 27);
            this.offsetRadioButton.TabIndex = 0;
            this.offsetRadioButton.TabStop = true;
            this.offsetRadioButton.Text = "                      ";
            this.thwompToolTip.SetToolTip(this.offsetRadioButton, "Wave function takes the byte offset (relative to the Chain sample). This creates " +
        "a continuous wave");
            this.offsetRadioButton.UseVisualStyleBackColor = false;
            // 
            // byteValueRadioButton
            // 
            this.byteValueRadioButton.AutoSize = true;
            this.byteValueRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.byteValueRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.byteValueRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.byteValueRadioButton.Location = new System.Drawing.Point(3, 36);
            this.byteValueRadioButton.Name = "byteValueRadioButton";
            this.byteValueRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.byteValueRadioButton.Size = new System.Drawing.Size(100, 27);
            this.byteValueRadioButton.TabIndex = 1;
            this.byteValueRadioButton.Text = "                      ";
            this.thwompToolTip.SetToolTip(this.byteValueRadioButton, "Wave function takes the value of the byte (0 ~ 255). This creates noise");
            this.byteValueRadioButton.UseVisualStyleBackColor = false;
            // 
            // prevWaveButton
            // 
            this.prevWaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.prevWaveButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.prevWaveButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.prevWaveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Turquoise;
            this.prevWaveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleTurquoise;
            this.prevWaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevWaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prevWaveButton.Location = new System.Drawing.Point(308, 53);
            this.prevWaveButton.Margin = new System.Windows.Forms.Padding(10, 53, 3, 3);
            this.prevWaveButton.Name = "prevWaveButton";
            this.prevWaveButton.Size = new System.Drawing.Size(23, 23);
            this.prevWaveButton.TabIndex = 4;
            this.prevWaveButton.Text = "<";
            this.thwompToolTip.SetToolTip(this.prevWaveButton, "Previous waveform");
            this.prevWaveButton.UseVisualStyleBackColor = false;
            this.prevWaveButton.Click += new System.EventHandler(this.prevWaveButton_Click);
            // 
            // waveformPreview
            // 
            this.waveformPreview.BackColor = System.Drawing.Color.Transparent;
            this.waveformPreview.BackgroundImage = global::ChompAndFriends.Properties.Resources.sineWave;
            this.waveformPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.waveformPreview.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.waveformPreview.Location = new System.Drawing.Point(337, 51);
            this.waveformPreview.Margin = new System.Windows.Forms.Padding(3, 51, 3, 3);
            this.waveformPreview.Name = "waveformPreview";
            this.waveformPreview.Size = new System.Drawing.Size(28, 28);
            this.waveformPreview.TabIndex = 3;
            this.thwompToolTip.SetToolTip(this.waveformPreview, "Sine wave");
            this.waveformPreview.WrapContents = false;
            // 
            // nextWaveButton
            // 
            this.nextWaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextWaveButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextWaveButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.nextWaveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Turquoise;
            this.nextWaveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleTurquoise;
            this.nextWaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextWaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextWaveButton.Location = new System.Drawing.Point(371, 53);
            this.nextWaveButton.Margin = new System.Windows.Forms.Padding(3, 53, 3, 3);
            this.nextWaveButton.Name = "nextWaveButton";
            this.nextWaveButton.Size = new System.Drawing.Size(23, 23);
            this.nextWaveButton.TabIndex = 5;
            this.nextWaveButton.Text = ">";
            this.thwompToolTip.SetToolTip(this.nextWaveButton, "Next waveform");
            this.nextWaveButton.UseVisualStyleBackColor = false;
            this.nextWaveButton.Click += new System.EventHandler(this.nextWaveButton_Click);
            // 
            // ThwompUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ThwompUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.byteInterval)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        internal System.Windows.Forms.NumericUpDown byteInterval;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel waveformPreview;
        private System.Windows.Forms.Button prevWaveButton;
        private System.Windows.Forms.Button nextWaveButton;
        public System.Windows.Forms.RadioButton offsetRadioButton;
        public System.Windows.Forms.RadioButton byteValueRadioButton;
        private System.Windows.Forms.ToolTip thwompToolTip;
    }
}

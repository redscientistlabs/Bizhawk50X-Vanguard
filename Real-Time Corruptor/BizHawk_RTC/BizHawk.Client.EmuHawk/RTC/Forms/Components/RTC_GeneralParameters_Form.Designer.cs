namespace RTC
{
	partial class RTC_GeneralParameters_Form
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.nmIntensity = new System.Windows.Forms.NumericUpDown();
            this.nmErrorDelay = new System.Windows.Forms.NumericUpDown();
            this.labelIntensityTimes = new System.Windows.Forms.Label();
            this.labelErrorDelay = new System.Windows.Forms.Label();
            this.cbBlastRadius = new System.Windows.Forms.ComboBox();
            this.labelBlastRadius = new System.Windows.Forms.Label();
            this.labelIntensity = new System.Windows.Forms.Label();
            this.labelErrorDelaySteps = new System.Windows.Forms.Label();
            this.track_Intensity = new System.Windows.Forms.TrackBar();
            this.track_ErrorDelay = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_ErrorDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // nmIntensity
            // 
            this.nmIntensity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmIntensity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmIntensity.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmIntensity.ForeColor = System.Drawing.Color.White;
            this.nmIntensity.Location = new System.Drawing.Point(93, 71);
            this.nmIntensity.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmIntensity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmIntensity.Name = "nmIntensity";
            this.nmIntensity.Size = new System.Drawing.Size(60, 22);
            this.nmIntensity.TabIndex = 18;
            this.nmIntensity.Tag = "color:dark";
            this.nmIntensity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nmErrorDelay
            // 
            this.nmErrorDelay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nmErrorDelay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmErrorDelay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.nmErrorDelay.ForeColor = System.Drawing.Color.White;
            this.nmErrorDelay.Location = new System.Drawing.Point(93, 11);
            this.nmErrorDelay.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nmErrorDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmErrorDelay.Name = "nmErrorDelay";
            this.nmErrorDelay.Size = new System.Drawing.Size(60, 22);
            this.nmErrorDelay.TabIndex = 16;
            this.nmErrorDelay.Tag = "color:dark";
            this.nmErrorDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelIntensityTimes
            // 
            this.labelIntensityTimes.AutoSize = true;
            this.labelIntensityTimes.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelIntensityTimes.ForeColor = System.Drawing.Color.White;
            this.labelIntensityTimes.Location = new System.Drawing.Point(158, 73);
            this.labelIntensityTimes.Name = "labelIntensityTimes";
            this.labelIntensityTimes.Size = new System.Drawing.Size(39, 17);
            this.labelIntensityTimes.TabIndex = 22;
            this.labelIntensityTimes.Text = "times";
            // 
            // labelErrorDelay
            // 
            this.labelErrorDelay.AutoSize = true;
            this.labelErrorDelay.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelErrorDelay.ForeColor = System.Drawing.Color.White;
            this.labelErrorDelay.Location = new System.Drawing.Point(9, 11);
            this.labelErrorDelay.Name = "labelErrorDelay";
            this.labelErrorDelay.Size = new System.Drawing.Size(80, 17);
            this.labelErrorDelay.TabIndex = 15;
            this.labelErrorDelay.Text = "Error delay :";
            // 
            // cbBlastRadius
            // 
            this.cbBlastRadius.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbBlastRadius.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBlastRadius.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBlastRadius.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbBlastRadius.ForeColor = System.Drawing.Color.White;
            this.cbBlastRadius.FormattingEnabled = true;
            this.cbBlastRadius.Items.AddRange(new object[] {
            "SPREAD",
            "CHUNK",
            "BURST",
            "EVEN",
            "PROPORTIONAL",
            "NORMALIZED"});
            this.cbBlastRadius.Location = new System.Drawing.Point(93, 134);
            this.cbBlastRadius.Name = "cbBlastRadius";
            this.cbBlastRadius.Size = new System.Drawing.Size(100, 21);
            this.cbBlastRadius.TabIndex = 21;
            this.cbBlastRadius.Tag = "color:dark";
            // 
            // labelBlastRadius
            // 
            this.labelBlastRadius.AutoSize = true;
            this.labelBlastRadius.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelBlastRadius.ForeColor = System.Drawing.Color.White;
            this.labelBlastRadius.Location = new System.Drawing.Point(9, 136);
            this.labelBlastRadius.Name = "labelBlastRadius";
            this.labelBlastRadius.Size = new System.Drawing.Size(81, 17);
            this.labelBlastRadius.TabIndex = 20;
            this.labelBlastRadius.Text = "Blast Radius:";
            // 
            // labelIntensity
            // 
            this.labelIntensity.AutoSize = true;
            this.labelIntensity.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelIntensity.ForeColor = System.Drawing.Color.White;
            this.labelIntensity.Location = new System.Drawing.Point(9, 74);
            this.labelIntensity.Name = "labelIntensity";
            this.labelIntensity.Size = new System.Drawing.Size(62, 17);
            this.labelIntensity.TabIndex = 19;
            this.labelIntensity.Text = "Intensity :";
            // 
            // labelErrorDelaySteps
            // 
            this.labelErrorDelaySteps.AutoSize = true;
            this.labelErrorDelaySteps.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.labelErrorDelaySteps.ForeColor = System.Drawing.Color.White;
            this.labelErrorDelaySteps.Location = new System.Drawing.Point(156, 11);
            this.labelErrorDelaySteps.Name = "labelErrorDelaySteps";
            this.labelErrorDelaySteps.Size = new System.Drawing.Size(39, 17);
            this.labelErrorDelaySteps.TabIndex = 17;
            this.labelErrorDelaySteps.Text = "steps";
            // 
            // track_Intensity
            // 
            this.track_Intensity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.track_Intensity.Location = new System.Drawing.Point(3, 92);
            this.track_Intensity.Maximum = 512000;
            this.track_Intensity.Minimum = 2000;
            this.track_Intensity.Name = "track_Intensity";
            this.track_Intensity.Size = new System.Drawing.Size(195, 45);
            this.track_Intensity.TabIndex = 24;
            this.track_Intensity.TickFrequency = 32000;
            this.track_Intensity.Value = 2000;
            // 
            // track_ErrorDelay
            // 
            this.track_ErrorDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.track_ErrorDelay.Location = new System.Drawing.Point(3, 32);
            this.track_ErrorDelay.Maximum = 512000;
            this.track_ErrorDelay.Minimum = 2000;
            this.track_ErrorDelay.Name = "track_ErrorDelay";
            this.track_ErrorDelay.Size = new System.Drawing.Size(194, 45);
            this.track_ErrorDelay.TabIndex = 23;
            this.track_ErrorDelay.TickFrequency = 32000;
            this.track_ErrorDelay.Value = 2000;
            // 
            // RTC_GeneralParameters_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(200, 167);
            this.Controls.Add(this.nmIntensity);
            this.Controls.Add(this.nmErrorDelay);
            this.Controls.Add(this.labelIntensityTimes);
            this.Controls.Add(this.labelErrorDelay);
            this.Controls.Add(this.cbBlastRadius);
            this.Controls.Add(this.labelBlastRadius);
            this.Controls.Add(this.labelIntensity);
            this.Controls.Add(this.labelErrorDelaySteps);
            this.Controls.Add(this.track_Intensity);
            this.Controls.Add(this.track_ErrorDelay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "RTC_GeneralParameters_Form";
            this.Tag = "color:normal";
            this.Text = "General Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_GeneralParameters_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_GeneralParameters_Form_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RTC_GeneralParameters_Form_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.nmIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmErrorDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_Intensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.track_ErrorDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.NumericUpDown nmIntensity;
		public System.Windows.Forms.NumericUpDown nmErrorDelay;
		public System.Windows.Forms.ComboBox cbBlastRadius;
		public System.Windows.Forms.TrackBar track_Intensity;
		public System.Windows.Forms.TrackBar track_ErrorDelay;
		public System.Windows.Forms.Label labelIntensityTimes;
		public System.Windows.Forms.Label labelErrorDelay;
		public System.Windows.Forms.Label labelBlastRadius;
		public System.Windows.Forms.Label labelIntensity;
		public System.Windows.Forms.Label labelErrorDelaySteps;
	}
}
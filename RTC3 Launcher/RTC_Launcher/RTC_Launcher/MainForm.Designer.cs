namespace RTC_Launcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbMOTD = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVersions = new System.Windows.Forms.ListBox();
            this.pnVersionBatchFiles = new System.Windows.Forms.Panel();
            this.btnBatchfile10 = new System.Windows.Forms.Button();
            this.btnBatchfile09 = new System.Windows.Forms.Button();
            this.btnBatchfile08 = new System.Windows.Forms.Button();
            this.btnBatchfile07 = new System.Windows.Forms.Button();
            this.btnBatchfile06 = new System.Windows.Forms.Button();
            this.btnBatchfile05 = new System.Windows.Forms.Button();
            this.btnBatchfile04 = new System.Windows.Forms.Button();
            this.btnBatchfile03 = new System.Windows.Forms.Button();
            this.btnBatchfile01 = new System.Windows.Forms.Button();
            this.btnBatchfile02 = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbOnlineVersions = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDownloadVersion = new System.Windows.Forms.Button();
            this.lbSelectedVersion = new System.Windows.Forms.Label();
            this.pnCorruptionEngine = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOnlineGuide = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnVersionBatchFiles.SuspendLayout();
            this.pnCorruptionEngine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.lbMOTD);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 492);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 36);
            this.panel1.TabIndex = 0;
            // 
            // lbMOTD
            // 
            this.lbMOTD.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbMOTD.ForeColor = System.Drawing.Color.White;
            this.lbMOTD.Location = new System.Drawing.Point(7, 9);
            this.lbMOTD.Name = "lbMOTD";
            this.lbMOTD.Size = new System.Drawing.Size(500, 19);
            this.lbMOTD.TabIndex = 125;
            this.lbMOTD.Text = "...";
            this.lbMOTD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbMOTD.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 19);
            this.label2.TabIndex = 83;
            this.label2.Text = "Version Selector";
            // 
            // lbVersions
            // 
            this.lbVersions.BackColor = System.Drawing.Color.Gray;
            this.lbVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbVersions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbVersions.ForeColor = System.Drawing.Color.White;
            this.lbVersions.FormattingEnabled = true;
            this.lbVersions.IntegralHeight = false;
            this.lbVersions.ItemHeight = 17;
            this.lbVersions.Location = new System.Drawing.Point(11, 62);
            this.lbVersions.Name = "lbVersions";
            this.lbVersions.ScrollAlwaysVisible = true;
            this.lbVersions.Size = new System.Drawing.Size(143, 140);
            this.lbVersions.TabIndex = 82;
            this.lbVersions.Tag = "color:normal";
            this.lbVersions.SelectedIndexChanged += new System.EventHandler(this.lbVersions_SelectedIndexChanged);
            this.lbVersions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbVersions_MouseDown);
            // 
            // pnVersionBatchFiles
            // 
            this.pnVersionBatchFiles.BackColor = System.Drawing.Color.Gray;
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile10);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile09);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile08);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile07);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile06);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile05);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile04);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile03);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile01);
            this.pnVersionBatchFiles.Controls.Add(this.btnBatchfile02);
            this.pnVersionBatchFiles.Controls.Add(this.btnStart);
            this.pnVersionBatchFiles.Location = new System.Drawing.Point(170, 62);
            this.pnVersionBatchFiles.Name = "pnVersionBatchFiles";
            this.pnVersionBatchFiles.Size = new System.Drawing.Size(335, 406);
            this.pnVersionBatchFiles.TabIndex = 123;
            this.pnVersionBatchFiles.Tag = "color:normal";
            this.pnVersionBatchFiles.Visible = false;
            // 
            // btnBatchfile10
            // 
            this.btnBatchfile10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile10.FlatAppearance.BorderSize = 0;
            this.btnBatchfile10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile10.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile10.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile10.Location = new System.Drawing.Point(13, 367);
            this.btnBatchfile10.Name = "btnBatchfile10";
            this.btnBatchfile10.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile10.TabIndex = 127;
            this.btnBatchfile10.TabStop = false;
            this.btnBatchfile10.Tag = "color:light";
            this.btnBatchfile10.Text = "...";
            this.btnBatchfile10.UseVisualStyleBackColor = false;
            this.btnBatchfile10.Visible = false;
            this.btnBatchfile10.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile09
            // 
            this.btnBatchfile09.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile09.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile09.FlatAppearance.BorderSize = 0;
            this.btnBatchfile09.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile09.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile09.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile09.Location = new System.Drawing.Point(13, 334);
            this.btnBatchfile09.Name = "btnBatchfile09";
            this.btnBatchfile09.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile09.TabIndex = 128;
            this.btnBatchfile09.TabStop = false;
            this.btnBatchfile09.Tag = "color:light";
            this.btnBatchfile09.Text = "...";
            this.btnBatchfile09.UseVisualStyleBackColor = false;
            this.btnBatchfile09.Visible = false;
            this.btnBatchfile09.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile08
            // 
            this.btnBatchfile08.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile08.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile08.FlatAppearance.BorderSize = 0;
            this.btnBatchfile08.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile08.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile08.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile08.Location = new System.Drawing.Point(13, 301);
            this.btnBatchfile08.Name = "btnBatchfile08";
            this.btnBatchfile08.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile08.TabIndex = 129;
            this.btnBatchfile08.TabStop = false;
            this.btnBatchfile08.Tag = "color:light";
            this.btnBatchfile08.Text = "...";
            this.btnBatchfile08.UseVisualStyleBackColor = false;
            this.btnBatchfile08.Visible = false;
            this.btnBatchfile08.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile07
            // 
            this.btnBatchfile07.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile07.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile07.FlatAppearance.BorderSize = 0;
            this.btnBatchfile07.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile07.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile07.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile07.Location = new System.Drawing.Point(13, 268);
            this.btnBatchfile07.Name = "btnBatchfile07";
            this.btnBatchfile07.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile07.TabIndex = 125;
            this.btnBatchfile07.TabStop = false;
            this.btnBatchfile07.Tag = "color:light";
            this.btnBatchfile07.Text = "...";
            this.btnBatchfile07.UseVisualStyleBackColor = false;
            this.btnBatchfile07.Visible = false;
            this.btnBatchfile07.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile06
            // 
            this.btnBatchfile06.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile06.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile06.FlatAppearance.BorderSize = 0;
            this.btnBatchfile06.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile06.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile06.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile06.Location = new System.Drawing.Point(13, 235);
            this.btnBatchfile06.Name = "btnBatchfile06";
            this.btnBatchfile06.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile06.TabIndex = 125;
            this.btnBatchfile06.TabStop = false;
            this.btnBatchfile06.Tag = "color:light";
            this.btnBatchfile06.Text = "...";
            this.btnBatchfile06.UseVisualStyleBackColor = false;
            this.btnBatchfile06.Visible = false;
            this.btnBatchfile06.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile05
            // 
            this.btnBatchfile05.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile05.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile05.FlatAppearance.BorderSize = 0;
            this.btnBatchfile05.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile05.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile05.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile05.Location = new System.Drawing.Point(13, 202);
            this.btnBatchfile05.Name = "btnBatchfile05";
            this.btnBatchfile05.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile05.TabIndex = 125;
            this.btnBatchfile05.TabStop = false;
            this.btnBatchfile05.Tag = "color:light";
            this.btnBatchfile05.Text = "...";
            this.btnBatchfile05.UseVisualStyleBackColor = false;
            this.btnBatchfile05.Visible = false;
            this.btnBatchfile05.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile04
            // 
            this.btnBatchfile04.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile04.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile04.FlatAppearance.BorderSize = 0;
            this.btnBatchfile04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile04.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile04.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile04.Location = new System.Drawing.Point(13, 169);
            this.btnBatchfile04.Name = "btnBatchfile04";
            this.btnBatchfile04.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile04.TabIndex = 125;
            this.btnBatchfile04.TabStop = false;
            this.btnBatchfile04.Tag = "color:light";
            this.btnBatchfile04.Text = "...";
            this.btnBatchfile04.UseVisualStyleBackColor = false;
            this.btnBatchfile04.Visible = false;
            this.btnBatchfile04.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile03
            // 
            this.btnBatchfile03.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile03.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile03.FlatAppearance.BorderSize = 0;
            this.btnBatchfile03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile03.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile03.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile03.Location = new System.Drawing.Point(13, 136);
            this.btnBatchfile03.Name = "btnBatchfile03";
            this.btnBatchfile03.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile03.TabIndex = 125;
            this.btnBatchfile03.TabStop = false;
            this.btnBatchfile03.Tag = "color:light";
            this.btnBatchfile03.Text = "...";
            this.btnBatchfile03.UseVisualStyleBackColor = false;
            this.btnBatchfile03.Visible = false;
            this.btnBatchfile03.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile01
            // 
            this.btnBatchfile01.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile01.FlatAppearance.BorderSize = 0;
            this.btnBatchfile01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile01.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile01.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile01.Location = new System.Drawing.Point(13, 70);
            this.btnBatchfile01.Name = "btnBatchfile01";
            this.btnBatchfile01.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile01.TabIndex = 125;
            this.btnBatchfile01.TabStop = false;
            this.btnBatchfile01.Tag = "color:light";
            this.btnBatchfile01.Text = "...";
            this.btnBatchfile01.UseVisualStyleBackColor = false;
            this.btnBatchfile01.Visible = false;
            this.btnBatchfile01.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnBatchfile02
            // 
            this.btnBatchfile02.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchfile02.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnBatchfile02.FlatAppearance.BorderSize = 0;
            this.btnBatchfile02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBatchfile02.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnBatchfile02.ForeColor = System.Drawing.Color.Black;
            this.btnBatchfile02.Location = new System.Drawing.Point(13, 103);
            this.btnBatchfile02.Name = "btnBatchfile02";
            this.btnBatchfile02.Size = new System.Drawing.Size(309, 27);
            this.btnBatchfile02.TabIndex = 125;
            this.btnBatchfile02.TabStop = false;
            this.btnBatchfile02.Tag = "color:light";
            this.btnBatchfile02.Text = "...";
            this.btnBatchfile02.UseVisualStyleBackColor = false;
            this.btnBatchfile02.Visible = false;
            this.btnBatchfile02.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnStart.Location = new System.Drawing.Point(13, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(309, 45);
            this.btnStart.TabIndex = 73;
            this.btnStart.TabStop = false;
            this.btnStart.Tag = "color:darker";
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnBatchfile_Click);
            // 
            // lbOnlineVersions
            // 
            this.lbOnlineVersions.BackColor = System.Drawing.Color.Gray;
            this.lbOnlineVersions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbOnlineVersions.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lbOnlineVersions.ForeColor = System.Drawing.Color.White;
            this.lbOnlineVersions.FormattingEnabled = true;
            this.lbOnlineVersions.IntegralHeight = false;
            this.lbOnlineVersions.ItemHeight = 17;
            this.lbOnlineVersions.Location = new System.Drawing.Point(11, 230);
            this.lbOnlineVersions.Name = "lbOnlineVersions";
            this.lbOnlineVersions.ScrollAlwaysVisible = true;
            this.lbOnlineVersions.Size = new System.Drawing.Size(143, 196);
            this.lbOnlineVersions.TabIndex = 125;
            this.lbOnlineVersions.Tag = "color:normal";
            this.lbOnlineVersions.SelectedIndexChanged += new System.EventHandler(this.lbOnlineVersions_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 208);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 19);
            this.label3.TabIndex = 126;
            this.label3.Text = "Online Downloader";
            // 
            // btnDownloadVersion
            // 
            this.btnDownloadVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDownloadVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnDownloadVersion.FlatAppearance.BorderSize = 0;
            this.btnDownloadVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnDownloadVersion.ForeColor = System.Drawing.Color.Black;
            this.btnDownloadVersion.Location = new System.Drawing.Point(12, 441);
            this.btnDownloadVersion.Name = "btnDownloadVersion";
            this.btnDownloadVersion.Size = new System.Drawing.Size(143, 27);
            this.btnDownloadVersion.TabIndex = 128;
            this.btnDownloadVersion.TabStop = false;
            this.btnDownloadVersion.Tag = "color:light";
            this.btnDownloadVersion.Text = "Download";
            this.btnDownloadVersion.UseVisualStyleBackColor = false;
            this.btnDownloadVersion.Visible = false;
            this.btnDownloadVersion.Click += new System.EventHandler(this.btnDownloadVersion_Click);
            // 
            // lbSelectedVersion
            // 
            this.lbSelectedVersion.AutoSize = true;
            this.lbSelectedVersion.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lbSelectedVersion.ForeColor = System.Drawing.Color.White;
            this.lbSelectedVersion.Location = new System.Drawing.Point(179, 40);
            this.lbSelectedVersion.Name = "lbSelectedVersion";
            this.lbSelectedVersion.Size = new System.Drawing.Size(117, 19);
            this.lbSelectedVersion.TabIndex = 129;
            this.lbSelectedVersion.Text = "Program Selector";
            this.lbSelectedVersion.Visible = false;
            // 
            // pnCorruptionEngine
            // 
            this.pnCorruptionEngine.BackColor = System.Drawing.Color.Gray;
            this.pnCorruptionEngine.Controls.Add(this.pictureBox1);
            this.pnCorruptionEngine.Controls.Add(this.label1);
            this.pnCorruptionEngine.Location = new System.Drawing.Point(224, 125);
            this.pnCorruptionEngine.Name = "pnCorruptionEngine";
            this.pnCorruptionEngine.Size = new System.Drawing.Size(210, 256);
            this.pnCorruptionEngine.TabIndex = 130;
            this.pnCorruptionEngine.Tag = "color:normal";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RTC_Launcher.Properties.Resources.LauncherSkull;
            this.pictureBox1.Location = new System.Drawing.Point(35, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(153, 142);
            this.pictureBox1.TabIndex = 131;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(39, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 19);
            this.label1.TabIndex = 130;
            this.label1.Text = "Select a RTC Version";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.btnOnlineGuide);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(517, 36);
            this.panel2.TabIndex = 131;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(289, 36);
            this.label4.TabIndex = 125;
            this.label4.Text = "Real-Time Corruptor Launcher";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOnlineGuide
            // 
            this.btnOnlineGuide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOnlineGuide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOnlineGuide.FlatAppearance.BorderSize = 0;
            this.btnOnlineGuide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOnlineGuide.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.btnOnlineGuide.ForeColor = System.Drawing.Color.Black;
            this.btnOnlineGuide.Location = new System.Drawing.Point(369, 6);
            this.btnOnlineGuide.Name = "btnOnlineGuide";
            this.btnOnlineGuide.Size = new System.Drawing.Size(143, 24);
            this.btnOnlineGuide.TabIndex = 129;
            this.btnOnlineGuide.TabStop = false;
            this.btnOnlineGuide.Tag = "color:light";
            this.btnOnlineGuide.Text = "Consult online guide";
            this.btnOnlineGuide.UseVisualStyleBackColor = false;
            this.btnOnlineGuide.Click += new System.EventHandler(this.btnOnlineGuide_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 476);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(483, 13);
            this.label5.TabIndex = 132;
            this.label5.Text = "RTC and WGH are developed by Redscientist Media, consult redscientist.com for mor" +
    "e details";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(517, 528);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnVersionBatchFiles);
            this.Controls.Add(this.pnCorruptionEngine);
            this.Controls.Add(this.lbSelectedVersion);
            this.Controls.Add(this.btnDownloadVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbOnlineVersions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbVersions);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Tag = "color:dark";
            this.Text = "RTC Launcher";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.pnVersionBatchFiles.ResumeLayout(false);
            this.pnCorruptionEngine.ResumeLayout(false);
            this.pnCorruptionEngine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbMOTD;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox lbVersions;
        public System.Windows.Forms.Panel pnVersionBatchFiles;
        private System.Windows.Forms.Button btnBatchfile10;
        private System.Windows.Forms.Button btnBatchfile09;
        private System.Windows.Forms.Button btnBatchfile08;
        private System.Windows.Forms.Button btnBatchfile07;
        private System.Windows.Forms.Button btnBatchfile06;
        private System.Windows.Forms.Button btnBatchfile05;
        private System.Windows.Forms.Button btnBatchfile04;
        private System.Windows.Forms.Button btnBatchfile03;
        private System.Windows.Forms.Button btnBatchfile01;
        private System.Windows.Forms.Button btnBatchfile02;
        private System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.ListBox lbOnlineVersions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDownloadVersion;
        private System.Windows.Forms.Label lbSelectedVersion;
        private System.Windows.Forms.Panel pnCorruptionEngine;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOnlineGuide;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}


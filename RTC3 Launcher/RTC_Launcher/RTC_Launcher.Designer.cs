namespace RTC
{
    partial class RTC_Launcher
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTC_Launcher));
            this.btnKill = new System.Windows.Forms.Button();
            this.btnKillAndRestart = new System.Windows.Forms.Button();
            this.pbTimeout = new System.Windows.Forms.ProgressBar();
            this.btnKillResetAndRestart = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.cbDetection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Launcher = new System.Windows.Forms.TabPage();
            this.AutoKillSwitch = new System.Windows.Forms.TabPage();
            this.lbThreshold = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Help = new System.Windows.Forms.TabPage();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label17 = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.label16 = new System.Windows.Forms.Label();
            this.About = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.cbSelectedVersion = new System.Windows.Forms.ComboBox();
            this.btnRunRTC = new System.Windows.Forms.Button();
            this.cbRunWithAKS = new System.Windows.Forms.CheckBox();
            this.btnResetRTC = new System.Windows.Forms.Button();
            this.btnPKGmanaging = new System.Windows.Forms.Button();
            this.btnCleanRTC = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.Launcher.SuspendLayout();
            this.AutoKillSwitch.SuspendLayout();
            this.Help.SuspendLayout();
            this.About.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnKill
            // 
            this.btnKill.BackColor = System.Drawing.Color.Black;
            this.btnKill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKill.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnKill.Location = new System.Drawing.Point(10, 55);
            this.btnKill.Name = "btnKill";
            this.btnKill.Size = new System.Drawing.Size(99, 23);
            this.btnKill.TabIndex = 0;
            this.btnKill.Text = "Kill";
            this.btnKill.UseVisualStyleBackColor = false;
            this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
            // 
            // btnKillAndRestart
            // 
            this.btnKillAndRestart.BackColor = System.Drawing.Color.Black;
            this.btnKillAndRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKillAndRestart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnKillAndRestart.Location = new System.Drawing.Point(10, 77);
            this.btnKillAndRestart.Name = "btnKillAndRestart";
            this.btnKillAndRestart.Size = new System.Drawing.Size(99, 23);
            this.btnKillAndRestart.TabIndex = 1;
            this.btnKillAndRestart.Text = "Kill Restart";
            this.btnKillAndRestart.UseVisualStyleBackColor = false;
            this.btnKillAndRestart.Click += new System.EventHandler(this.btnKillAndRestart_Click);
            // 
            // pbTimeout
            // 
            this.pbTimeout.Location = new System.Drawing.Point(10, 27);
            this.pbTimeout.MarqueeAnimationSpeed = 1;
            this.pbTimeout.Maximum = 4;
            this.pbTimeout.Name = "pbTimeout";
            this.pbTimeout.Size = new System.Drawing.Size(188, 23);
            this.pbTimeout.Step = 1;
            this.pbTimeout.TabIndex = 2;
            this.pbTimeout.Value = 4;
            // 
            // btnKillResetAndRestart
            // 
            this.btnKillResetAndRestart.BackColor = System.Drawing.Color.Black;
            this.btnKillResetAndRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKillResetAndRestart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnKillResetAndRestart.Location = new System.Drawing.Point(10, 99);
            this.btnKillResetAndRestart.Name = "btnKillResetAndRestart";
            this.btnKillResetAndRestart.Size = new System.Drawing.Size(99, 23);
            this.btnKillResetAndRestart.TabIndex = 3;
            this.btnKillResetAndRestart.Text = "Kill Reset Restart";
            this.btnKillResetAndRestart.UseVisualStyleBackColor = false;
            this.btnKillResetAndRestart.Click += new System.EventHandler(this.btnKillResetAndRestart_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.ForeColor = System.Drawing.Color.White;
            this.cbEnabled.Location = new System.Drawing.Point(115, 104);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(76, 17);
            this.cbEnabled.TabIndex = 4;
            this.cbEnabled.Text = "Is Enabled";
            this.cbEnabled.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.cbEnabled.UseVisualStyleBackColor = true;
            this.cbEnabled.CheckedChanged += new System.EventHandler(this.cbEnabled_CheckedChanged);
            // 
            // cbDetection
            // 
            this.cbDetection.BackColor = System.Drawing.Color.Black;
            this.cbDetection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDetection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbDetection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cbDetection.FormattingEnabled = true;
            this.cbDetection.Items.AddRange(new object[] {
            "VIOLENT",
            "HEAVY",
            "MILD",
            "SLOPPY",
            "COMATOSE"});
            this.cbDetection.Location = new System.Drawing.Point(114, 74);
            this.cbDetection.Name = "cbDetection";
            this.cbDetection.Size = new System.Drawing.Size(83, 21);
            this.cbDetection.TabIndex = 5;
            this.cbDetection.SelectedIndexChanged += new System.EventHandler(this.cbDetection_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(114, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Detection rate:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Launcher);
            this.tabControl1.Controls.Add(this.AutoKillSwitch);
            this.tabControl1.Controls.Add(this.Help);
            this.tabControl1.Controls.Add(this.About);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(231, 191);
            this.tabControl1.TabIndex = 8;
            // 
            // Launcher
            // 
            this.Launcher.BackColor = System.Drawing.Color.SteelBlue;
            this.Launcher.Controls.Add(this.btnCleanRTC);
            this.Launcher.Controls.Add(this.btnPKGmanaging);
            this.Launcher.Controls.Add(this.btnResetRTC);
            this.Launcher.Controls.Add(this.cbRunWithAKS);
            this.Launcher.Controls.Add(this.btnRunRTC);
            this.Launcher.Controls.Add(this.label20);
            this.Launcher.Controls.Add(this.cbSelectedVersion);
            this.Launcher.Controls.Add(this.label19);
            this.Launcher.Location = new System.Drawing.Point(4, 22);
            this.Launcher.Name = "Launcher";
            this.Launcher.Padding = new System.Windows.Forms.Padding(3);
            this.Launcher.Size = new System.Drawing.Size(223, 165);
            this.Launcher.TabIndex = 0;
            this.Launcher.Text = "Launcher";
            // 
            // AutoKillSwitch
            // 
            this.AutoKillSwitch.BackColor = System.Drawing.Color.SteelBlue;
            this.AutoKillSwitch.Controls.Add(this.lbThreshold);
            this.AutoKillSwitch.Controls.Add(this.label18);
            this.AutoKillSwitch.Controls.Add(this.label2);
            this.AutoKillSwitch.Controls.Add(this.pbTimeout);
            this.AutoKillSwitch.Controls.Add(this.label1);
            this.AutoKillSwitch.Controls.Add(this.btnKillResetAndRestart);
            this.AutoKillSwitch.Controls.Add(this.btnKill);
            this.AutoKillSwitch.Controls.Add(this.cbEnabled);
            this.AutoKillSwitch.Controls.Add(this.cbDetection);
            this.AutoKillSwitch.Controls.Add(this.btnKillAndRestart);
            this.AutoKillSwitch.Location = new System.Drawing.Point(4, 22);
            this.AutoKillSwitch.Name = "AutoKillSwitch";
            this.AutoKillSwitch.Padding = new System.Windows.Forms.Padding(3);
            this.AutoKillSwitch.Size = new System.Drawing.Size(223, 165);
            this.AutoKillSwitch.TabIndex = 1;
            this.AutoKillSwitch.Text = "Auto-KillSwitch";
            // 
            // lbThreshold
            // 
            this.lbThreshold.AutoSize = true;
            this.lbThreshold.ForeColor = System.Drawing.Color.White;
            this.lbThreshold.Location = new System.Drawing.Point(140, 135);
            this.lbThreshold.Name = "lbThreshold";
            this.lbThreshold.Size = new System.Drawing.Size(55, 13);
            this.lbThreshold.TabIndex = 9;
            this.lbThreshold.Text = "x seconds";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(7, 135);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(135, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "Heartbeat threshold set to: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Heartbeat detection:";
            // 
            // Help
            // 
            this.Help.BackColor = System.Drawing.Color.SteelBlue;
            this.Help.Controls.Add(this.linkLabel5);
            this.Help.Controls.Add(this.linkLabel4);
            this.Help.Controls.Add(this.linkLabel2);
            this.Help.Controls.Add(this.label17);
            this.Help.Controls.Add(this.linkLabel3);
            this.Help.Controls.Add(this.label16);
            this.Help.Location = new System.Drawing.Point(4, 22);
            this.Help.Name = "Help";
            this.Help.Size = new System.Drawing.Size(223, 165);
            this.Help.TabIndex = 3;
            this.Help.Text = "Help";
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel5.LinkColor = System.Drawing.Color.White;
            this.linkLabel5.Location = new System.Drawing.Point(3, 90);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(120, 13);
            this.linkLabel5.TabIndex = 27;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "2 : The Glitch Harvester";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel4.LinkColor = System.Drawing.Color.White;
            this.linkLabel4.Location = new System.Drawing.Point(3, 74);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(200, 13);
            this.linkLabel4.TabIndex = 26;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "1 : Basic usage and the different engines";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.LinkColor = System.Drawing.Color.White;
            this.linkLabel2.Location = new System.Drawing.Point(123, 53);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(79, 15);
            this.linkLabel2.TabIndex = 25;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "WelshGamer";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(6, 55);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(119, 13);
            this.label17.TabIndex = 24;
            this.label17.Text = "Tutorial videos made by";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.LinkColor = System.Drawing.Color.White;
            this.linkLabel3.Location = new System.Drawing.Point(6, 27);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(80, 13);
            this.linkLabel3.TabIndex = 23;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Google Docs";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(6, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(158, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "View the Instructions Manual on";
            // 
            // About
            // 
            this.About.BackColor = System.Drawing.Color.Black;
            this.About.Controls.Add(this.label15);
            this.About.Controls.Add(this.linkLabel1);
            this.About.Controls.Add(this.label8);
            this.About.Controls.Add(this.label14);
            this.About.Controls.Add(this.label13);
            this.About.Controls.Add(this.label12);
            this.About.Controls.Add(this.label11);
            this.About.Controls.Add(this.label10);
            this.About.Controls.Add(this.label9);
            this.About.Controls.Add(this.label7);
            this.About.Controls.Add(this.label6);
            this.About.Controls.Add(this.label5);
            this.About.Controls.Add(this.label4);
            this.About.Controls.Add(this.label3);
            this.About.Location = new System.Drawing.Point(4, 22);
            this.About.Name = "About";
            this.About.Size = new System.Drawing.Size(223, 165);
            this.About.TabIndex = 4;
            this.About.Text = "About";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(7, 20);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 15);
            this.label15.TabIndex = 29;
            this.label15.Text = "Presented by";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.LightCoral;
            this.linkLabel1.Location = new System.Drawing.Point(82, 20);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(130, 15);
            this.linkLabel1.TabIndex = 28;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Redscientist Media";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Honeydew;
            this.label8.Location = new System.Drawing.Point(5, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(214, 18);
            this.label8.TabIndex = 27;
            this.label8.Text = "Real-Time Corruptor (RTC)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(26, 115);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(74, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Wade Morgus";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(26, 85);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(124, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "The BizHawk Dev Team";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(107, 144);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "Rikerz";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(26, 143);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "Maiddog";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(106, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Virus610";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(106, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "WelshGamer";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(26, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Maximemoring";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(26, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Vinny and Joel from Vinesauce";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(26, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Ircluzar";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Special Thanks:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(7, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Mod Developpers:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Honeydew;
            this.label19.Location = new System.Drawing.Point(9, 7);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(116, 18);
            this.label19.TabIndex = 28;
            this.label19.Text = "RTC Launcher";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(9, 35);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(114, 13);
            this.label20.TabIndex = 30;
            this.label20.Text = "Selected RTC version:";
            // 
            // cbSelectedVersion
            // 
            this.cbSelectedVersion.BackColor = System.Drawing.Color.Black;
            this.cbSelectedVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedVersion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbSelectedVersion.ForeColor = System.Drawing.Color.Gold;
            this.cbSelectedVersion.FormattingEnabled = true;
            this.cbSelectedVersion.Location = new System.Drawing.Point(12, 51);
            this.cbSelectedVersion.Name = "cbSelectedVersion";
            this.cbSelectedVersion.Size = new System.Drawing.Size(201, 21);
            this.cbSelectedVersion.TabIndex = 29;
            // 
            // btnRunRTC
            // 
            this.btnRunRTC.BackColor = System.Drawing.Color.Black;
            this.btnRunRTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunRTC.ForeColor = System.Drawing.Color.OrangeRed;
            this.btnRunRTC.Location = new System.Drawing.Point(10, 103);
            this.btnRunRTC.Name = "btnRunRTC";
            this.btnRunRTC.Size = new System.Drawing.Size(79, 23);
            this.btnRunRTC.TabIndex = 31;
            this.btnRunRTC.Text = "Run RTC";
            this.btnRunRTC.UseVisualStyleBackColor = false;
            // 
            // cbRunWithAKS
            // 
            this.cbRunWithAKS.AutoSize = true;
            this.cbRunWithAKS.Checked = true;
            this.cbRunWithAKS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRunWithAKS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRunWithAKS.ForeColor = System.Drawing.Color.White;
            this.cbRunWithAKS.Location = new System.Drawing.Point(10, 80);
            this.cbRunWithAKS.Name = "cbRunWithAKS";
            this.cbRunWithAKS.Size = new System.Drawing.Size(141, 17);
            this.cbRunWithAKS.TabIndex = 81;
            this.cbRunWithAKS.Text = "Run with Auto-KillSwitch";
            this.cbRunWithAKS.UseVisualStyleBackColor = true;
            // 
            // btnResetRTC
            // 
            this.btnResetRTC.BackColor = System.Drawing.Color.Black;
            this.btnResetRTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetRTC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnResetRTC.Location = new System.Drawing.Point(95, 103);
            this.btnResetRTC.Name = "btnResetRTC";
            this.btnResetRTC.Size = new System.Drawing.Size(118, 23);
            this.btnResetRTC.TabIndex = 82;
            this.btnResetRTC.Text = "Reset RTC to factory";
            this.btnResetRTC.UseVisualStyleBackColor = false;
            // 
            // btnPKGmanaging
            // 
            this.btnPKGmanaging.BackColor = System.Drawing.Color.Black;
            this.btnPKGmanaging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPKGmanaging.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnPKGmanaging.Location = new System.Drawing.Point(95, 132);
            this.btnPKGmanaging.Name = "btnPKGmanaging";
            this.btnPKGmanaging.Size = new System.Drawing.Size(117, 23);
            this.btnPKGmanaging.TabIndex = 83;
            this.btnPKGmanaging.Text = "PKG Managing";
            this.btnPKGmanaging.UseVisualStyleBackColor = false;
            // 
            // btnCleanRTC
            // 
            this.btnCleanRTC.BackColor = System.Drawing.Color.Black;
            this.btnCleanRTC.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCleanRTC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCleanRTC.Location = new System.Drawing.Point(12, 132);
            this.btnCleanRTC.Name = "btnCleanRTC";
            this.btnCleanRTC.Size = new System.Drawing.Size(77, 23);
            this.btnCleanRTC.TabIndex = 84;
            this.btnCleanRTC.Text = "Clean RTC";
            this.btnCleanRTC.UseVisualStyleBackColor = false;
            // 
            // RTC_Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(235, 196);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RTC_Launcher";
            this.Text = "Launcher";
            this.Load += new System.EventHandler(this.RTC_Launcher_Load);
            this.tabControl1.ResumeLayout(false);
            this.Launcher.ResumeLayout(false);
            this.Launcher.PerformLayout();
            this.AutoKillSwitch.ResumeLayout(false);
            this.AutoKillSwitch.PerformLayout();
            this.Help.ResumeLayout(false);
            this.Help.PerformLayout();
            this.About.ResumeLayout(false);
            this.About.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnKill;
        private System.Windows.Forms.Button btnKillAndRestart;
        private System.Windows.Forms.ProgressBar pbTimeout;
        private System.Windows.Forms.Button btnKillResetAndRestart;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.ComboBox cbDetection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Launcher;
        private System.Windows.Forms.TabPage AutoKillSwitch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage Help;
        private System.Windows.Forms.TabPage About;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lbThreshold;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnPKGmanaging;
        private System.Windows.Forms.Button btnResetRTC;
        public System.Windows.Forms.CheckBox cbRunWithAKS;
        private System.Windows.Forms.Button btnRunRTC;
        private System.Windows.Forms.Label label20;
        public System.Windows.Forms.ComboBox cbSelectedVersion;
        private System.Windows.Forms.Button btnCleanRTC;
    }
}


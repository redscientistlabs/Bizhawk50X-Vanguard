namespace RTC
{
	partial class RTC_Multiplayer_Form
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
            this.btnStartServer = new System.Windows.Forms.Button();
            this.tbClientAdress = new System.Windows.Forms.TextBox();
            this.btnStartClient = new System.Windows.Forms.Button();
            this.btnPullGameFromServer = new System.Windows.Forms.Button();
            this.lbServer = new System.Windows.Forms.Label();
            this.lbClient = new System.Windows.Forms.Label();
            this.btnPushGameToServer = new System.Windows.Forms.Button();
            this.lbServerStatus = new System.Windows.Forms.Label();
            this.lbClientStatus = new System.Windows.Forms.Label();
            this.btnPullStateFromServer = new System.Windows.Forms.Button();
            this.btnPushStateToServer = new System.Windows.Forms.Button();
            this.btnPushBlastToServer = new System.Windows.Forms.Button();
            this.tbShowIp = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbServerPort = new System.Windows.Forms.TextBox();
            this.btnSwapGameState = new System.Windows.Forms.Button();
            this.pbPeerScreen = new System.Windows.Forms.PictureBox();
            this.btnPushScreenToPear = new System.Windows.Forms.Button();
            this.btnPullScreenToPear = new System.Windows.Forms.Button();
            this.cbStreamScreenToPeer = new System.Windows.Forms.CheckBox();
            this.btnClearNetworkCache = new System.Windows.Forms.Button();
            this.btnGameOfSwap = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnPeerRedBar = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSplitscreen = new System.Windows.Forms.Button();
            this.btnBlastBoard = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCompressStream = new System.Windows.Forms.CheckBox();
            this.cbStreamFps = new System.Windows.Forms.ComboBox();
            this.btnRequestStream = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cbPullStateToGlitchHarvester = new System.Windows.Forms.CheckBox();
            this.btnPushStashkeyToServer = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.lbCheekyHeadline = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPopoutPeerGameScreen = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPeerScreen)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartServer.FlatAppearance.BorderSize = 0;
            this.btnStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnStartServer.ForeColor = System.Drawing.Color.Black;
            this.btnStartServer.Location = new System.Drawing.Point(92, 44);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(228, 22);
            this.btnStartServer.TabIndex = 1;
            this.btnStartServer.TabStop = false;
            this.btnStartServer.Tag = "color:light";
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = false;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            this.btnStartServer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnStartServer_MouseDown);
            // 
            // tbClientAdress
            // 
            this.tbClientAdress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbClientAdress.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbClientAdress.ForeColor = System.Drawing.Color.White;
            this.tbClientAdress.Location = new System.Drawing.Point(92, 99);
            this.tbClientAdress.Name = "tbClientAdress";
            this.tbClientAdress.Size = new System.Drawing.Size(120, 22);
            this.tbClientAdress.TabIndex = 2;
            this.tbClientAdress.Tag = "color:dark";
            this.tbClientAdress.TextChanged += new System.EventHandler(this.tbClientAdress_TextChanged);
            // 
            // btnStartClient
            // 
            this.btnStartClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnStartClient.FlatAppearance.BorderSize = 0;
            this.btnStartClient.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartClient.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnStartClient.ForeColor = System.Drawing.Color.Black;
            this.btnStartClient.Location = new System.Drawing.Point(213, 99);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(107, 22);
            this.btnStartClient.TabIndex = 3;
            this.btnStartClient.TabStop = false;
            this.btnStartClient.Tag = "color:light";
            this.btnStartClient.Text = "Connect";
            this.btnStartClient.UseVisualStyleBackColor = false;
            this.btnStartClient.Click += new System.EventHandler(this.btnStartClient_Click);
            this.btnStartClient.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnStartServer_MouseDown);
            // 
            // btnPullGameFromServer
            // 
            this.btnPullGameFromServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPullGameFromServer.FlatAppearance.BorderSize = 0;
            this.btnPullGameFromServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPullGameFromServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPullGameFromServer.ForeColor = System.Drawing.Color.Black;
            this.btnPullGameFromServer.Location = new System.Drawing.Point(10, 8);
            this.btnPullGameFromServer.Name = "btnPullGameFromServer";
            this.btnPullGameFromServer.Size = new System.Drawing.Size(101, 22);
            this.btnPullGameFromServer.TabIndex = 4;
            this.btnPullGameFromServer.TabStop = false;
            this.btnPullGameFromServer.Tag = "color:light";
            this.btnPullGameFromServer.Text = "PULL Game";
            this.btnPullGameFromServer.UseVisualStyleBackColor = false;
            this.btnPullGameFromServer.Click += new System.EventHandler(this.btnPullGameFromServer_Click);
            // 
            // lbServer
            // 
            this.lbServer.AutoSize = true;
            this.lbServer.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbServer.ForeColor = System.Drawing.Color.Orange;
            this.lbServer.Location = new System.Drawing.Point(14, 45);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(66, 21);
            this.lbServer.TabIndex = 5;
            this.lbServer.Text = "Server :";
            // 
            // lbClient
            // 
            this.lbClient.AutoSize = true;
            this.lbClient.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.lbClient.ForeColor = System.Drawing.Color.Orange;
            this.lbClient.Location = new System.Drawing.Point(14, 100);
            this.lbClient.Name = "lbClient";
            this.lbClient.Size = new System.Drawing.Size(60, 21);
            this.lbClient.TabIndex = 6;
            this.lbClient.Text = "Client :";
            // 
            // btnPushGameToServer
            // 
            this.btnPushGameToServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPushGameToServer.FlatAppearance.BorderSize = 0;
            this.btnPushGameToServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushGameToServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPushGameToServer.ForeColor = System.Drawing.Color.Black;
            this.btnPushGameToServer.Location = new System.Drawing.Point(114, 8);
            this.btnPushGameToServer.Name = "btnPushGameToServer";
            this.btnPushGameToServer.Size = new System.Drawing.Size(101, 22);
            this.btnPushGameToServer.TabIndex = 7;
            this.btnPushGameToServer.TabStop = false;
            this.btnPushGameToServer.Tag = "color:light";
            this.btnPushGameToServer.Text = "PUSH Game";
            this.btnPushGameToServer.UseVisualStyleBackColor = false;
            this.btnPushGameToServer.Click += new System.EventHandler(this.btnPushGameToServer_Click);
            // 
            // lbServerStatus
            // 
            this.lbServerStatus.AutoSize = true;
            this.lbServerStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbServerStatus.ForeColor = System.Drawing.Color.White;
            this.lbServerStatus.Location = new System.Drawing.Point(23, 72);
            this.lbServerStatus.Name = "lbServerStatus";
            this.lbServerStatus.Size = new System.Drawing.Size(117, 13);
            this.lbServerStatus.TabIndex = 8;
            this.lbServerStatus.Text = "Server Status : moody";
            // 
            // lbClientStatus
            // 
            this.lbClientStatus.AutoSize = true;
            this.lbClientStatus.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbClientStatus.ForeColor = System.Drawing.Color.White;
            this.lbClientStatus.Location = new System.Drawing.Point(23, 127);
            this.lbClientStatus.Name = "lbClientStatus";
            this.lbClientStatus.Size = new System.Drawing.Size(112, 13);
            this.lbClientStatus.TabIndex = 9;
            this.lbClientStatus.Text = "Client Status : lonely";
            // 
            // btnPullStateFromServer
            // 
            this.btnPullStateFromServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPullStateFromServer.FlatAppearance.BorderSize = 0;
            this.btnPullStateFromServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPullStateFromServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPullStateFromServer.ForeColor = System.Drawing.Color.Black;
            this.btnPullStateFromServer.Location = new System.Drawing.Point(10, 34);
            this.btnPullStateFromServer.Name = "btnPullStateFromServer";
            this.btnPullStateFromServer.Size = new System.Drawing.Size(101, 22);
            this.btnPullStateFromServer.TabIndex = 10;
            this.btnPullStateFromServer.TabStop = false;
            this.btnPullStateFromServer.Tag = "color:light";
            this.btnPullStateFromServer.Text = "PULL State";
            this.btnPullStateFromServer.UseVisualStyleBackColor = false;
            this.btnPullStateFromServer.Click += new System.EventHandler(this.btnPullStateFromServer_Click);
            // 
            // btnPushStateToServer
            // 
            this.btnPushStateToServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPushStateToServer.FlatAppearance.BorderSize = 0;
            this.btnPushStateToServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushStateToServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPushStateToServer.ForeColor = System.Drawing.Color.Black;
            this.btnPushStateToServer.Location = new System.Drawing.Point(114, 34);
            this.btnPushStateToServer.Name = "btnPushStateToServer";
            this.btnPushStateToServer.Size = new System.Drawing.Size(101, 22);
            this.btnPushStateToServer.TabIndex = 11;
            this.btnPushStateToServer.TabStop = false;
            this.btnPushStateToServer.Tag = "color:light";
            this.btnPushStateToServer.Text = "PUSH State";
            this.btnPushStateToServer.UseVisualStyleBackColor = false;
            this.btnPushStateToServer.Click += new System.EventHandler(this.btnPushStateToServer_Click);
            // 
            // btnPushBlastToServer
            // 
            this.btnPushBlastToServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPushBlastToServer.FlatAppearance.BorderSize = 0;
            this.btnPushBlastToServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushBlastToServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPushBlastToServer.ForeColor = System.Drawing.Color.Black;
            this.btnPushBlastToServer.Location = new System.Drawing.Point(10, 120);
            this.btnPushBlastToServer.Name = "btnPushBlastToServer";
            this.btnPushBlastToServer.Size = new System.Drawing.Size(101, 22);
            this.btnPushBlastToServer.TabIndex = 13;
            this.btnPushBlastToServer.TabStop = false;
            this.btnPushBlastToServer.Tag = "color:light";
            this.btnPushBlastToServer.Text = "PUSH BlastLayer";
            this.btnPushBlastToServer.UseVisualStyleBackColor = false;
            this.btnPushBlastToServer.Click += new System.EventHandler(this.btnPushBlastToServer_Click);
            // 
            // tbShowIp
            // 
            this.tbShowIp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbShowIp.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbShowIp.ForeColor = System.Drawing.Color.White;
            this.tbShowIp.Location = new System.Drawing.Point(17, 9);
            this.tbShowIp.Name = "tbShowIp";
            this.tbShowIp.Size = new System.Drawing.Size(167, 22);
            this.tbShowIp.TabIndex = 15;
            this.tbShowIp.Tag = "color:dark";
            this.tbShowIp.Text = "Click to display your IP";
            this.tbShowIp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbShowIp.Click += new System.EventHandler(this.tbShowIp_TextChanged);
            this.tbShowIp.TextChanged += new System.EventHandler(this.tbShowIp_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label5.ForeColor = System.Drawing.Color.Gainsboro;
            this.label5.Location = new System.Drawing.Point(208, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "TCP Port :";
            // 
            // tbServerPort
            // 
            this.tbServerPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tbServerPort.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbServerPort.ForeColor = System.Drawing.Color.White;
            this.tbServerPort.Location = new System.Drawing.Point(266, 9);
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(54, 22);
            this.tbServerPort.TabIndex = 17;
            this.tbServerPort.Tag = "color:dark";
            this.tbServerPort.Text = "42069";
            this.tbServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbServerPort.TextChanged += new System.EventHandler(this.tbServerPort_TextChanged);
            // 
            // btnSwapGameState
            // 
            this.btnSwapGameState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSwapGameState.FlatAppearance.BorderSize = 0;
            this.btnSwapGameState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapGameState.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnSwapGameState.ForeColor = System.Drawing.Color.Black;
            this.btnSwapGameState.Location = new System.Drawing.Point(10, 94);
            this.btnSwapGameState.Name = "btnSwapGameState";
            this.btnSwapGameState.Size = new System.Drawing.Size(205, 22);
            this.btnSwapGameState.TabIndex = 18;
            this.btnSwapGameState.TabStop = false;
            this.btnSwapGameState.Tag = "color:light";
            this.btnSwapGameState.Text = "SWAP Game+State";
            this.btnSwapGameState.UseVisualStyleBackColor = false;
            this.btnSwapGameState.Click += new System.EventHandler(this.btnSwapGameState_Click);
            // 
            // pbPeerScreen
            // 
            this.pbPeerScreen.BackColor = System.Drawing.Color.Black;
            this.pbPeerScreen.Location = new System.Drawing.Point(10, 9);
            this.pbPeerScreen.Name = "pbPeerScreen";
            this.pbPeerScreen.Size = new System.Drawing.Size(256, 224);
            this.pbPeerScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPeerScreen.TabIndex = 19;
            this.pbPeerScreen.TabStop = false;
            // 
            // btnPushScreenToPear
            // 
            this.btnPushScreenToPear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPushScreenToPear.FlatAppearance.BorderSize = 0;
            this.btnPushScreenToPear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushScreenToPear.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPushScreenToPear.ForeColor = System.Drawing.Color.Black;
            this.btnPushScreenToPear.Location = new System.Drawing.Point(114, 60);
            this.btnPushScreenToPear.Name = "btnPushScreenToPear";
            this.btnPushScreenToPear.Size = new System.Drawing.Size(101, 22);
            this.btnPushScreenToPear.TabIndex = 20;
            this.btnPushScreenToPear.TabStop = false;
            this.btnPushScreenToPear.Tag = "color:light";
            this.btnPushScreenToPear.Text = "PUSH Screen";
            this.btnPushScreenToPear.UseVisualStyleBackColor = false;
            this.btnPushScreenToPear.Click += new System.EventHandler(this.btnPushScreenToPear_Click);
            // 
            // btnPullScreenToPear
            // 
            this.btnPullScreenToPear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPullScreenToPear.FlatAppearance.BorderSize = 0;
            this.btnPullScreenToPear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPullScreenToPear.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPullScreenToPear.ForeColor = System.Drawing.Color.Black;
            this.btnPullScreenToPear.Location = new System.Drawing.Point(10, 60);
            this.btnPullScreenToPear.Name = "btnPullScreenToPear";
            this.btnPullScreenToPear.Size = new System.Drawing.Size(101, 22);
            this.btnPullScreenToPear.TabIndex = 21;
            this.btnPullScreenToPear.TabStop = false;
            this.btnPullScreenToPear.Tag = "color:light";
            this.btnPullScreenToPear.Text = "PULL Screen";
            this.btnPullScreenToPear.UseVisualStyleBackColor = false;
            this.btnPullScreenToPear.Click += new System.EventHandler(this.btnPullScreenToPear_Click);
            // 
            // cbStreamScreenToPeer
            // 
            this.cbStreamScreenToPeer.AutoSize = true;
            this.cbStreamScreenToPeer.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.cbStreamScreenToPeer.ForeColor = System.Drawing.Color.White;
            this.cbStreamScreenToPeer.Location = new System.Drawing.Point(170, 150);
            this.cbStreamScreenToPeer.Name = "cbStreamScreenToPeer";
            this.cbStreamScreenToPeer.Size = new System.Drawing.Size(158, 19);
            this.cbStreamScreenToPeer.TabIndex = 22;
            this.cbStreamScreenToPeer.Text = "Enable Screen Streaming";
            this.cbStreamScreenToPeer.UseVisualStyleBackColor = true;
            this.cbStreamScreenToPeer.CheckedChanged += new System.EventHandler(this.cbStreamScreenToPeer_CheckedChanged);
            // 
            // btnClearNetworkCache
            // 
            this.btnClearNetworkCache.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnClearNetworkCache.FlatAppearance.BorderSize = 0;
            this.btnClearNetworkCache.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearNetworkCache.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnClearNetworkCache.ForeColor = System.Drawing.Color.Black;
            this.btnClearNetworkCache.Location = new System.Drawing.Point(7, 94);
            this.btnClearNetworkCache.Name = "btnClearNetworkCache";
            this.btnClearNetworkCache.Size = new System.Drawing.Size(95, 47);
            this.btnClearNetworkCache.TabIndex = 24;
            this.btnClearNetworkCache.TabStop = false;
            this.btnClearNetworkCache.Tag = "color:light";
            this.btnClearNetworkCache.Text = "Clear Network Cache";
            this.btnClearNetworkCache.UseVisualStyleBackColor = false;
            this.btnClearNetworkCache.Click += new System.EventHandler(this.btnClearNetworkCache_Click);
            // 
            // btnGameOfSwap
            // 
            this.btnGameOfSwap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnGameOfSwap.FlatAppearance.BorderSize = 0;
            this.btnGameOfSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGameOfSwap.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnGameOfSwap.ForeColor = System.Drawing.Color.White;
            this.btnGameOfSwap.Location = new System.Drawing.Point(6, 10);
            this.btnGameOfSwap.Name = "btnGameOfSwap";
            this.btnGameOfSwap.Size = new System.Drawing.Size(130, 29);
            this.btnGameOfSwap.TabIndex = 26;
            this.btnGameOfSwap.TabStop = false;
            this.btnGameOfSwap.Tag = "color:darker";
            this.btnGameOfSwap.Text = "Game of Swap";
            this.btnGameOfSwap.UseVisualStyleBackColor = false;
            this.btnGameOfSwap.Click += new System.EventHandler(this.btnGameOfSwap_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.pnPeerRedBar);
            this.panel1.Controls.Add(this.pbPeerScreen);
            this.panel1.Location = new System.Drawing.Point(363, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(276, 244);
            this.panel1.TabIndex = 28;
            this.panel1.Tag = "color:normal";
            // 
            // pnPeerRedBar
            // 
            this.pnPeerRedBar.BackColor = System.Drawing.Color.Red;
            this.pnPeerRedBar.Location = new System.Drawing.Point(10, 236);
            this.pnPeerRedBar.Name = "pnPeerRedBar";
            this.pnPeerRedBar.Size = new System.Drawing.Size(256, 3);
            this.pnPeerRedBar.TabIndex = 145;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button3.Enabled = false;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(139, 42);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 29);
            this.button3.TabIndex = 29;
            this.button3.TabStop = false;
            this.button3.Tag = "color:darker";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button4.Enabled = false;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(6, 74);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(130, 29);
            this.button4.TabIndex = 30;
            this.button4.TabStop = false;
            this.button4.Tag = "color:darker";
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button5.Enabled = false;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(139, 74);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 29);
            this.button5.TabIndex = 31;
            this.button5.TabStop = false;
            this.button5.Tag = "color:darker";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gray;
            this.panel3.Controls.Add(this.button1);
            this.panel3.Controls.Add(this.button2);
            this.panel3.Controls.Add(this.btnSplitscreen);
            this.panel3.Controls.Add(this.btnBlastBoard);
            this.panel3.Controls.Add(this.button6);
            this.panel3.Controls.Add(this.button7);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.btnGameOfSwap);
            this.panel3.Controls.Add(this.button3);
            this.panel3.Location = new System.Drawing.Point(363, 321);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(276, 177);
            this.panel3.TabIndex = 33;
            this.panel3.Tag = "color:normal";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(139, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 29);
            this.button1.TabIndex = 37;
            this.button1.TabStop = false;
            this.button1.Tag = "color:darker";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button2.Enabled = false;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(6, 138);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 29);
            this.button2.TabIndex = 36;
            this.button2.TabStop = false;
            this.button2.Tag = "color:darker";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // btnSplitscreen
            // 
            this.btnSplitscreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnSplitscreen.FlatAppearance.BorderSize = 0;
            this.btnSplitscreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSplitscreen.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnSplitscreen.ForeColor = System.Drawing.Color.White;
            this.btnSplitscreen.Location = new System.Drawing.Point(6, 42);
            this.btnSplitscreen.Name = "btnSplitscreen";
            this.btnSplitscreen.Size = new System.Drawing.Size(130, 29);
            this.btnSplitscreen.TabIndex = 35;
            this.btnSplitscreen.TabStop = false;
            this.btnSplitscreen.Tag = "color:darker";
            this.btnSplitscreen.Text = "Splitscreen";
            this.btnSplitscreen.UseVisualStyleBackColor = false;
            this.btnSplitscreen.Click += new System.EventHandler(this.btnSplitscreen_Click);
            // 
            // btnBlastBoard
            // 
            this.btnBlastBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.btnBlastBoard.FlatAppearance.BorderSize = 0;
            this.btnBlastBoard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBlastBoard.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnBlastBoard.ForeColor = System.Drawing.Color.White;
            this.btnBlastBoard.Location = new System.Drawing.Point(139, 10);
            this.btnBlastBoard.Name = "btnBlastBoard";
            this.btnBlastBoard.Size = new System.Drawing.Size(130, 29);
            this.btnBlastBoard.TabIndex = 34;
            this.btnBlastBoard.TabStop = false;
            this.btnBlastBoard.Tag = "color:darker";
            this.btnBlastBoard.Text = "BlastBoard";
            this.btnBlastBoard.UseVisualStyleBackColor = false;
            this.btnBlastBoard.Click += new System.EventHandler(this.btnBlastBoard_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button6.Enabled = false;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(139, 106);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(130, 29);
            this.button6.TabIndex = 33;
            this.button6.TabStop = false;
            this.button6.Tag = "color:darker";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.button7.Enabled = false;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(6, 106);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(130, 29);
            this.button7.TabIndex = 32;
            this.button7.TabStop = false;
            this.button7.Tag = "color:darker";
            this.button7.UseVisualStyleBackColor = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(367, 303);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(202, 15);
            this.label8.TabIndex = 138;
            this.label8.Text = "Metagames and Multiplayer Features";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(365, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 15);
            this.label6.TabIndex = 139;
            this.label6.Text = "Multiplayer screen";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gray;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.cbCompressStream);
            this.panel4.Controls.Add(this.cbStreamFps);
            this.panel4.Controls.Add(this.btnRequestStream);
            this.panel4.Controls.Add(this.btnClearNetworkCache);
            this.panel4.Location = new System.Drawing.Point(245, 321);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(110, 177);
            this.panel4.TabIndex = 33;
            this.panel4.Tag = "color:normal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Video framerate";
            // 
            // cbCompressStream
            // 
            this.cbCompressStream.AutoSize = true;
            this.cbCompressStream.Checked = true;
            this.cbCompressStream.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCompressStream.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbCompressStream.ForeColor = System.Drawing.Color.White;
            this.cbCompressStream.Location = new System.Drawing.Point(7, 50);
            this.cbCompressStream.Name = "cbCompressStream";
            this.cbCompressStream.Size = new System.Drawing.Size(76, 17);
            this.cbCompressStream.TabIndex = 24;
            this.cbCompressStream.Text = "Compress";
            this.cbCompressStream.UseVisualStyleBackColor = true;
            // 
            // cbStreamFps
            // 
            this.cbStreamFps.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cbStreamFps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStreamFps.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbStreamFps.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbStreamFps.ForeColor = System.Drawing.Color.White;
            this.cbStreamFps.FormattingEnabled = true;
            this.cbStreamFps.Items.AddRange(new object[] {
            "60fps",
            "45fps",
            "30fps",
            "20fps",
            "10fps",
            "5fps",
            "2fps",
            "1fps"});
            this.cbStreamFps.Location = new System.Drawing.Point(7, 24);
            this.cbStreamFps.Name = "cbStreamFps";
            this.cbStreamFps.Size = new System.Drawing.Size(95, 21);
            this.cbStreamFps.TabIndex = 145;
            this.cbStreamFps.Tag = "color:dark";
            this.cbStreamFps.SelectedIndexChanged += new System.EventHandler(this.cbStreamFps_SelectedIndexChanged);
            // 
            // btnRequestStream
            // 
            this.btnRequestStream.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRequestStream.FlatAppearance.BorderSize = 0;
            this.btnRequestStream.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRequestStream.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnRequestStream.ForeColor = System.Drawing.Color.Black;
            this.btnRequestStream.Location = new System.Drawing.Point(7, 145);
            this.btnRequestStream.Name = "btnRequestStream";
            this.btnRequestStream.Size = new System.Drawing.Size(95, 24);
            this.btnRequestStream.TabIndex = 27;
            this.btnRequestStream.TabStop = false;
            this.btnRequestStream.Tag = "color:light";
            this.btnRequestStream.Text = "Request Stream";
            this.btnRequestStream.UseVisualStyleBackColor = false;
            this.btnRequestStream.Click += new System.EventHandler(this.btnRequestStream_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(248, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 15);
            this.label7.TabIndex = 140;
            this.label7.Text = "Stream Options";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Gray;
            this.panel5.Controls.Add(this.cbPullStateToGlitchHarvester);
            this.panel5.Controls.Add(this.btnPushStashkeyToServer);
            this.panel5.Controls.Add(this.btnPushBlastToServer);
            this.panel5.Controls.Add(this.btnPullGameFromServer);
            this.panel5.Controls.Add(this.btnPushGameToServer);
            this.panel5.Controls.Add(this.btnPullStateFromServer);
            this.panel5.Controls.Add(this.btnPushStateToServer);
            this.panel5.Controls.Add(this.btnSwapGameState);
            this.panel5.Controls.Add(this.btnPushScreenToPear);
            this.panel5.Controls.Add(this.btnPullScreenToPear);
            this.panel5.Location = new System.Drawing.Point(13, 321);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(224, 177);
            this.panel5.TabIndex = 33;
            this.panel5.Tag = "color:normal";
            // 
            // cbPullStateToGlitchHarvester
            // 
            this.cbPullStateToGlitchHarvester.AutoSize = true;
            this.cbPullStateToGlitchHarvester.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.cbPullStateToGlitchHarvester.ForeColor = System.Drawing.Color.White;
            this.cbPullStateToGlitchHarvester.Location = new System.Drawing.Point(11, 153);
            this.cbPullStateToGlitchHarvester.Name = "cbPullStateToGlitchHarvester";
            this.cbPullStateToGlitchHarvester.Size = new System.Drawing.Size(214, 17);
            this.cbPullStateToGlitchHarvester.TabIndex = 23;
            this.cbPullStateToGlitchHarvester.Text = "Send received state to GH Savestates";
            this.cbPullStateToGlitchHarvester.UseVisualStyleBackColor = true;
            // 
            // btnPushStashkeyToServer
            // 
            this.btnPushStashkeyToServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPushStashkeyToServer.FlatAppearance.BorderSize = 0;
            this.btnPushStashkeyToServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPushStashkeyToServer.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPushStashkeyToServer.ForeColor = System.Drawing.Color.Black;
            this.btnPushStashkeyToServer.Location = new System.Drawing.Point(114, 120);
            this.btnPushStashkeyToServer.Name = "btnPushStashkeyToServer";
            this.btnPushStashkeyToServer.Size = new System.Drawing.Size(101, 22);
            this.btnPushStashkeyToServer.TabIndex = 22;
            this.btnPushStashkeyToServer.TabStop = false;
            this.btnPushStashkeyToServer.Tag = "color:light";
            this.btnPushStashkeyToServer.Text = "PUSH Stashkey";
            this.btnPushStashkeyToServer.UseVisualStyleBackColor = false;
            this.btnPushStashkeyToServer.Click += new System.EventHandler(this.btnPushStashkeyToServer_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(16, 305);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 15);
            this.label9.TabIndex = 141;
            this.label9.Text = "Manual Commands";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Gray;
            this.panel6.Controls.Add(this.cbStreamScreenToPeer);
            this.panel6.Controls.Add(this.tbShowIp);
            this.panel6.Controls.Add(this.btnStartServer);
            this.panel6.Controls.Add(this.tbClientAdress);
            this.panel6.Controls.Add(this.btnStartClient);
            this.panel6.Controls.Add(this.lbServer);
            this.panel6.Controls.Add(this.lbClient);
            this.panel6.Controls.Add(this.lbServerStatus);
            this.panel6.Controls.Add(this.lbClientStatus);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.tbServerPort);
            this.panel6.Location = new System.Drawing.Point(13, 109);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(340, 176);
            this.panel6.TabIndex = 33;
            this.panel6.Tag = "color:normal";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(16, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 15);
            this.label10.TabIndex = 142;
            this.label10.Text = "Session Settings";
            // 
            // lbCheekyHeadline
            // 
            this.lbCheekyHeadline.AutoSize = true;
            this.lbCheekyHeadline.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lbCheekyHeadline.ForeColor = System.Drawing.Color.Gainsboro;
            this.lbCheekyHeadline.Location = new System.Drawing.Point(25, 54);
            this.lbCheekyHeadline.Name = "lbCheekyHeadline";
            this.lbCheekyHeadline.Size = new System.Drawing.Size(92, 13);
            this.lbCheekyHeadline.TabIndex = 144;
            this.lbCheekyHeadline.Text = "Cheeky headline";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 47);
            this.label1.TabIndex = 143;
            this.label1.Text = "RTC Multiplayer";
            // 
            // btnPopoutPeerGameScreen
            // 
            this.btnPopoutPeerGameScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPopoutPeerGameScreen.FlatAppearance.BorderSize = 0;
            this.btnPopoutPeerGameScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopoutPeerGameScreen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnPopoutPeerGameScreen.ForeColor = System.Drawing.Color.Black;
            this.btnPopoutPeerGameScreen.Location = new System.Drawing.Point(584, 18);
            this.btnPopoutPeerGameScreen.Name = "btnPopoutPeerGameScreen";
            this.btnPopoutPeerGameScreen.Size = new System.Drawing.Size(54, 22);
            this.btnPopoutPeerGameScreen.TabIndex = 23;
            this.btnPopoutPeerGameScreen.TabStop = false;
            this.btnPopoutPeerGameScreen.Tag = "color:light";
            this.btnPopoutPeerGameScreen.Text = "Popout";
            this.btnPopoutPeerGameScreen.UseVisualStyleBackColor = false;
            this.btnPopoutPeerGameScreen.Click += new System.EventHandler(this.btnPopoutPeerGameScreen_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label3.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.label3.Location = new System.Drawing.Point(276, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 28);
            this.label3.TabIndex = 145;
            this.label3.Text = "(Beta)";
            // 
            // RTC_Multiplayer_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(655, 515);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPopoutPeerGameScreen);
            this.Controls.Add(this.lbCheekyHeadline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "RTC_Multiplayer_Form";
            this.Tag = "color:dark";
            this.Text = "Multiplayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RTC_Multi_Form_FormClosing);
            this.Load += new System.EventHandler(this.RTC_Multi_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPeerScreen)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnStartServer;
		private System.Windows.Forms.TextBox tbClientAdress;
		private System.Windows.Forms.Button btnStartClient;
		private System.Windows.Forms.Button btnPullGameFromServer;
		private System.Windows.Forms.Label lbServer;
		private System.Windows.Forms.Label lbClient;
		private System.Windows.Forms.Button btnPushGameToServer;
		private System.Windows.Forms.Label lbServerStatus;
		private System.Windows.Forms.Label lbClientStatus;
		private System.Windows.Forms.Button btnPullStateFromServer;
		private System.Windows.Forms.Button btnPushStateToServer;
		private System.Windows.Forms.Button btnPushBlastToServer;
		private System.Windows.Forms.TextBox tbShowIp;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbServerPort;
		private System.Windows.Forms.Button btnSwapGameState;
		private System.Windows.Forms.Button btnPushScreenToPear;
		private System.Windows.Forms.Button btnPullScreenToPear;
		public System.Windows.Forms.CheckBox cbStreamScreenToPeer;
		private System.Windows.Forms.Button btnClearNetworkCache;
		private System.Windows.Forms.Button btnGameOfSwap;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label lbCheekyHeadline;
		private System.Windows.Forms.Label label1;
		public System.Windows.Forms.PictureBox pbPeerScreen;
		public System.Windows.Forms.Panel pnPeerRedBar;
		public System.Windows.Forms.Button btnPopoutPeerGameScreen;
		private System.Windows.Forms.Button btnPushStashkeyToServer;
		public System.Windows.Forms.CheckBox cbPullStateToGlitchHarvester;
		private System.Windows.Forms.Button btnRequestStream;
		private System.Windows.Forms.Button btnBlastBoard;
		public System.Windows.Forms.ComboBox cbStreamFps;
		public System.Windows.Forms.CheckBox cbCompressStream;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnSplitscreen;
    }
}
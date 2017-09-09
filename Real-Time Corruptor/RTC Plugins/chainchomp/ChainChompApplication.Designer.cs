namespace ChainChomp
{
    partial class ChainChompApplication
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChainChompApplication));
            this.chainChompToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.emuComboBox = new System.Windows.Forms.ComboBox();
            this.runButton = new System.Windows.Forms.Button();
            this.romImageComboBox = new System.Windows.Forms.ComboBox();
            this.openLibrary = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.libStartTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.libHelpAboutflowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chainTab = new System.Windows.Forms.TabControl();
            this.rootTabPage = new System.Windows.Forms.TabPage();
            this.addChainTab = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2.SuspendLayout();
            this.libStartTableLayout.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.libHelpAboutflowLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.chainTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // emuComboBox
            // 
            this.emuComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emuComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emuComboBox.Enabled = false;
            this.emuComboBox.FormattingEnabled = true;
            this.emuComboBox.Location = new System.Drawing.Point(3, 16);
            this.emuComboBox.Name = "emuComboBox";
            this.emuComboBox.Size = new System.Drawing.Size(338, 21);
            this.emuComboBox.TabIndex = 0;
            this.chainChompToolTip.SetToolTip(this.emuComboBox, "Emulator to run");
            this.emuComboBox.SelectedIndexChanged += new System.EventHandler(this.emuComboBox_SelectedIndexChanged);
            // 
            // runButton
            // 
            this.runButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runButton.Font = new System.Drawing.Font("Lucida Console", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton.Location = new System.Drawing.Point(372, 0);
            this.runButton.Margin = new System.Windows.Forms.Padding(0);
            this.runButton.Name = "runButton";
            this.libStartTableLayout.SetRowSpan(this.runButton, 3);
            this.runButton.Size = new System.Drawing.Size(251, 117);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "Run";
            this.chainChompToolTip.SetToolTip(this.runButton, "Run all enabled chains and boot the Selected Emulator");
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // romImageComboBox
            // 
            this.romImageComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.romImageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.romImageComboBox.Enabled = false;
            this.romImageComboBox.FormattingEnabled = true;
            this.romImageComboBox.Location = new System.Drawing.Point(3, 16);
            this.romImageComboBox.Name = "romImageComboBox";
            this.romImageComboBox.Size = new System.Drawing.Size(338, 21);
            this.romImageComboBox.TabIndex = 0;
            this.chainChompToolTip.SetToolTip(this.romImageComboBox, "Select a ROM Image");
            this.romImageComboBox.SelectedIndexChanged += new System.EventHandler(this.romImageComboBox_SelectedIndexChanged);
            // 
            // openLibrary
            // 
            this.openLibrary.Enabled = false;
            this.openLibrary.Location = new System.Drawing.Point(3, 3);
            this.openLibrary.Name = "openLibrary";
            this.openLibrary.Size = new System.Drawing.Size(81, 23);
            this.openLibrary.TabIndex = 0;
            this.openLibrary.Text = "Edit Library";
            this.chainChompToolTip.SetToolTip(this.openLibrary, "Add and remove ROM Images and Emulators");
            this.openLibrary.UseVisualStyleBackColor = true;
            this.openLibrary.Visible = false;
            this.openLibrary.Click += new System.EventHandler(this.openLibrary_Click);
            // 
            // helpButton
            // 
            this.helpButton.Location = new System.Drawing.Point(216, 3);
            this.helpButton.Margin = new System.Windows.Forms.Padding(129, 3, 3, 3);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(64, 23);
            this.helpButton.TabIndex = 1;
            this.helpButton.Text = "Help";
            this.chainChompToolTip.SetToolTip(this.helpButton, "Open the Chain Chomp wiki");
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(286, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "About";
            this.chainChompToolTip.SetToolTip(this.button2, "Open about dialog");
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.libStartTableLayout, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 611);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(629, 130);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // libStartTableLayout
            // 
            this.libStartTableLayout.BackColor = System.Drawing.SystemColors.Control;
            this.libStartTableLayout.ColumnCount = 3;
            this.libStartTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.28053F));
            this.libStartTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.620462F));
            this.libStartTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.libStartTableLayout.Controls.Add(this.groupBox2, 0, 1);
            this.libStartTableLayout.Controls.Add(this.runButton, 2, 0);
            this.libStartTableLayout.Controls.Add(this.groupBox1, 0, 0);
            this.libStartTableLayout.Controls.Add(this.libHelpAboutflowLayoutPanel, 0, 2);
            this.libStartTableLayout.Location = new System.Drawing.Point(3, 3);
            this.libStartTableLayout.MaximumSize = new System.Drawing.Size(623, 117);
            this.libStartTableLayout.MinimumSize = new System.Drawing.Size(623, 117);
            this.libStartTableLayout.Name = "libStartTableLayout";
            this.libStartTableLayout.RowCount = 3;
            this.libStartTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.libStartTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.libStartTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.libStartTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.libStartTableLayout.Size = new System.Drawing.Size(623, 117);
            this.libStartTableLayout.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.emuComboBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(0, 44);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 42);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Emulator";
            this.groupBox2.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.romImageComboBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.MaximumSize = new System.Drawing.Size(344, 44);
            this.groupBox1.MinimumSize = new System.Drawing.Size(344, 44);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 44);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected ROM Image";
            this.groupBox1.Visible = false;
            // 
            // libHelpAboutflowLayoutPanel
            // 
            this.libHelpAboutflowLayoutPanel.Controls.Add(this.openLibrary);
            this.libHelpAboutflowLayoutPanel.Controls.Add(this.helpButton);
            this.libHelpAboutflowLayoutPanel.Controls.Add(this.button2);
            this.libHelpAboutflowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libHelpAboutflowLayoutPanel.Location = new System.Drawing.Point(0, 86);
            this.libHelpAboutflowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.libHelpAboutflowLayoutPanel.MaximumSize = new System.Drawing.Size(344, 31);
            this.libHelpAboutflowLayoutPanel.MinimumSize = new System.Drawing.Size(344, 31);
            this.libHelpAboutflowLayoutPanel.Name = "libHelpAboutflowLayoutPanel";
            this.libHelpAboutflowLayoutPanel.Size = new System.Drawing.Size(344, 31);
            this.libHelpAboutflowLayoutPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.chainTab);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.MaximumSize = new System.Drawing.Size(629, 4000);
            this.panel1.MinimumSize = new System.Drawing.Size(629, 450);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(629, 611);
            this.panel1.TabIndex = 2;
            // 
            // chainTab
            // 
            this.chainTab.Controls.Add(this.rootTabPage);
            this.chainTab.Controls.Add(this.addChainTab);
            this.chainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chainTab.Location = new System.Drawing.Point(0, 0);
            this.chainTab.Margin = new System.Windows.Forms.Padding(0);
            this.chainTab.MaximumSize = new System.Drawing.Size(629, 4000);
            this.chainTab.MinimumSize = new System.Drawing.Size(629, 645);
            this.chainTab.Name = "chainTab";
            this.chainTab.Padding = new System.Drawing.Point(8, 8);
            this.chainTab.SelectedIndex = 0;
            this.chainTab.Size = new System.Drawing.Size(629, 645);
            this.chainTab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.chainTab.TabIndex = 1;
            this.chainTab.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.chainTab_Selecting);
            // 
            // rootTabPage
            // 
            this.rootTabPage.BackColor = System.Drawing.SystemColors.Control;
            this.rootTabPage.Location = new System.Drawing.Point(4, 32);
            this.rootTabPage.Name = "rootTabPage";
            this.rootTabPage.Size = new System.Drawing.Size(621, 609);
            this.rootTabPage.TabIndex = 0;
            this.rootTabPage.Text = "New Chain";
            // 
            // addChainTab
            // 
            this.addChainTab.BackColor = System.Drawing.SystemColors.Control;
            this.addChainTab.Location = new System.Drawing.Point(4, 32);
            this.addChainTab.Name = "addChainTab";
            this.addChainTab.Size = new System.Drawing.Size(621, 609);
            this.addChainTab.TabIndex = 1;
            this.addChainTab.Text = "+ Add Chain";
            // 
            // ChainChompApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 741);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(645, 4000);
            this.MinimumSize = new System.Drawing.Size(645, 645);
            this.Name = "ChainChompApplication";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chain Chomp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChainChompWindow_FormClosing);
            this.Load += new System.EventHandler(this.ChainChompApplication_Load);
            this.Resize += new System.EventHandler(this.ChainChompApplication_Resize);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.libStartTableLayout.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.libHelpAboutflowLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.chainTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

		internal System.Windows.Forms.ToolTip chainChompToolTip;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TableLayoutPanel libStartTableLayout;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox emuComboBox;
		private System.Windows.Forms.Button runButton;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox romImageComboBox;
		private System.Windows.Forms.FlowLayoutPanel libHelpAboutflowLayoutPanel;
		private System.Windows.Forms.Button openLibrary;
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl chainTab;
		private System.Windows.Forms.TabPage rootTabPage;
		private System.Windows.Forms.TabPage addChainTab;
    }
}


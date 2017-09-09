namespace ChainChomp
{
    partial class LibraryWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryWindow));
            this.libraryTabControl = new System.Windows.Forms.TabControl();
            this.romImageTabPage = new System.Windows.Forms.TabPage();
            this.romImageTable = new System.Windows.Forms.TableLayoutPanel();
            this.romImageListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.romImagePageFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.addROMImageButton = new System.Windows.Forms.Button();
            this.romImageRemoveButton = new System.Windows.Forms.Button();
            this.emuTabPage = new System.Windows.Forms.TabPage();
            this.emuTable = new System.Windows.Forms.TableLayoutPanel();
            this.emuListView = new System.Windows.Forms.ListView();
            this.pathColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.emuPageFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.addEmulatorButton = new System.Windows.Forms.Button();
            this.emuRemoveButton = new System.Windows.Forms.Button();
            this.addRomImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.addEmuDialog = new System.Windows.Forms.OpenFileDialog();
            this.libraryTabControl.SuspendLayout();
            this.romImageTabPage.SuspendLayout();
            this.romImageTable.SuspendLayout();
            this.romImagePageFlowLayout.SuspendLayout();
            this.emuTabPage.SuspendLayout();
            this.emuTable.SuspendLayout();
            this.emuPageFlowLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // libraryTabControl
            // 
            this.libraryTabControl.Controls.Add(this.romImageTabPage);
            this.libraryTabControl.Controls.Add(this.emuTabPage);
            this.libraryTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.libraryTabControl.Location = new System.Drawing.Point(0, 0);
            this.libraryTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.libraryTabControl.Name = "libraryTabControl";
            this.libraryTabControl.SelectedIndex = 0;
            this.libraryTabControl.Size = new System.Drawing.Size(678, 513);
            this.libraryTabControl.TabIndex = 0;
            // 
            // romImageTabPage
            // 
            this.romImageTabPage.Controls.Add(this.romImageTable);
            this.romImageTabPage.Location = new System.Drawing.Point(4, 22);
            this.romImageTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.romImageTabPage.Name = "romImageTabPage";
            this.romImageTabPage.Size = new System.Drawing.Size(670, 487);
            this.romImageTabPage.TabIndex = 0;
            this.romImageTabPage.Text = "ROM Images";
            this.romImageTabPage.UseVisualStyleBackColor = true;
            // 
            // romImageTable
            // 
            this.romImageTable.ColumnCount = 1;
            this.romImageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.49351F));
            this.romImageTable.Controls.Add(this.romImageListView, 0, 1);
            this.romImageTable.Controls.Add(this.romImagePageFlowLayout, 0, 0);
            this.romImageTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.romImageTable.Location = new System.Drawing.Point(0, 0);
            this.romImageTable.Margin = new System.Windows.Forms.Padding(0);
            this.romImageTable.Name = "romImageTable";
            this.romImageTable.RowCount = 2;
            this.romImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.romImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.56626F));
            this.romImageTable.Size = new System.Drawing.Size(670, 487);
            this.romImageTable.TabIndex = 0;
            // 
            // romImageListView
            // 
            this.romImageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.romImageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.romImageListView.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.romImageListView.GridLines = true;
            this.romImageListView.Location = new System.Drawing.Point(3, 44);
            this.romImageListView.Name = "romImageListView";
            this.romImageListView.ShowItemToolTips = true;
            this.romImageListView.Size = new System.Drawing.Size(664, 440);
            this.romImageListView.TabIndex = 3;
            this.romImageListView.UseCompatibleStateImageBehavior = false;
            this.romImageListView.View = System.Windows.Forms.View.List;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Path";
            this.columnHeader1.Width = 660;
            // 
            // romImagePageFlowLayout
            // 
            this.romImagePageFlowLayout.Controls.Add(this.addROMImageButton);
            this.romImagePageFlowLayout.Controls.Add(this.romImageRemoveButton);
            this.romImagePageFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.romImagePageFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.romImagePageFlowLayout.Name = "romImagePageFlowLayout";
            this.romImagePageFlowLayout.Padding = new System.Windows.Forms.Padding(5);
            this.romImagePageFlowLayout.Size = new System.Drawing.Size(670, 41);
            this.romImagePageFlowLayout.TabIndex = 0;
            // 
            // addROMImageButton
            // 
            this.addROMImageButton.Location = new System.Drawing.Point(8, 8);
            this.addROMImageButton.Name = "addROMImageButton";
            this.addROMImageButton.Size = new System.Drawing.Size(121, 23);
            this.addROMImageButton.TabIndex = 0;
            this.addROMImageButton.Text = "Add ROM Image(s)...";
            this.addROMImageButton.UseVisualStyleBackColor = true;
            this.addROMImageButton.Click += new System.EventHandler(this.addROMImageButton_Click);
            // 
            // romImageRemoveButton
            // 
            this.romImageRemoveButton.Location = new System.Drawing.Point(535, 8);
            this.romImageRemoveButton.Margin = new System.Windows.Forms.Padding(403, 3, 3, 3);
            this.romImageRemoveButton.Name = "romImageRemoveButton";
            this.romImageRemoveButton.Size = new System.Drawing.Size(121, 23);
            this.romImageRemoveButton.TabIndex = 1;
            this.romImageRemoveButton.Text = "Remove Selected";
            this.romImageRemoveButton.UseVisualStyleBackColor = true;
            this.romImageRemoveButton.Click += new System.EventHandler(this.romImageRemoveButton_Click);
            // 
            // emuTabPage
            // 
            this.emuTabPage.Controls.Add(this.emuTable);
            this.emuTabPage.Location = new System.Drawing.Point(4, 22);
            this.emuTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.emuTabPage.Name = "emuTabPage";
            this.emuTabPage.Size = new System.Drawing.Size(670, 487);
            this.emuTabPage.TabIndex = 1;
            this.emuTabPage.Text = "Emulators";
            this.emuTabPage.UseVisualStyleBackColor = true;
            // 
            // emuTable
            // 
            this.emuTable.ColumnCount = 1;
            this.emuTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.49351F));
            this.emuTable.Controls.Add(this.emuListView, 0, 1);
            this.emuTable.Controls.Add(this.emuPageFlowLayout, 0, 0);
            this.emuTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emuTable.Location = new System.Drawing.Point(0, 0);
            this.emuTable.Margin = new System.Windows.Forms.Padding(0);
            this.emuTable.Name = "emuTable";
            this.emuTable.RowCount = 2;
            this.emuTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.433735F));
            this.emuTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 91.56626F));
            this.emuTable.Size = new System.Drawing.Size(670, 487);
            this.emuTable.TabIndex = 1;
            // 
            // emuListView
            // 
            this.emuListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.pathColumn});
            this.emuListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emuListView.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emuListView.GridLines = true;
            this.emuListView.Location = new System.Drawing.Point(3, 44);
            this.emuListView.Name = "emuListView";
            this.emuListView.ShowItemToolTips = true;
            this.emuListView.Size = new System.Drawing.Size(664, 440);
            this.emuListView.TabIndex = 2;
            this.emuListView.UseCompatibleStateImageBehavior = false;
            this.emuListView.View = System.Windows.Forms.View.List;
            // 
            // pathColumn
            // 
            this.pathColumn.Text = "Path";
            this.pathColumn.Width = 660;
            // 
            // emuPageFlowLayout
            // 
            this.emuPageFlowLayout.Controls.Add(this.addEmulatorButton);
            this.emuPageFlowLayout.Controls.Add(this.emuRemoveButton);
            this.emuPageFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emuPageFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.emuPageFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.emuPageFlowLayout.Name = "emuPageFlowLayout";
            this.emuPageFlowLayout.Padding = new System.Windows.Forms.Padding(5);
            this.emuPageFlowLayout.Size = new System.Drawing.Size(670, 41);
            this.emuPageFlowLayout.TabIndex = 0;
            // 
            // addEmulatorButton
            // 
            this.addEmulatorButton.Location = new System.Drawing.Point(8, 8);
            this.addEmulatorButton.Name = "addEmulatorButton";
            this.addEmulatorButton.Size = new System.Drawing.Size(121, 23);
            this.addEmulatorButton.TabIndex = 0;
            this.addEmulatorButton.Text = "Add Emulator...";
            this.addEmulatorButton.UseVisualStyleBackColor = true;
            this.addEmulatorButton.Click += new System.EventHandler(this.addEmulatorButton_Click);
            // 
            // emuRemoveButton
            // 
            this.emuRemoveButton.Location = new System.Drawing.Point(535, 8);
            this.emuRemoveButton.Margin = new System.Windows.Forms.Padding(403, 3, 3, 3);
            this.emuRemoveButton.Name = "emuRemoveButton";
            this.emuRemoveButton.Size = new System.Drawing.Size(121, 23);
            this.emuRemoveButton.TabIndex = 2;
            this.emuRemoveButton.Text = "Remove Selected";
            this.emuRemoveButton.UseVisualStyleBackColor = true;
            this.emuRemoveButton.Click += new System.EventHandler(this.emuRemoveButton_Click);
            // 
            // addRomImageDialog
            // 
            this.addRomImageDialog.Filter = "All files|*.*";
            this.addRomImageDialog.Multiselect = true;
            this.addRomImageDialog.Title = "Add ROM Image(s)";
            // 
            // addEmuDialog
            // 
            this.addEmuDialog.DefaultExt = "exe";
            this.addEmuDialog.Filter = "Executables |*.exe|All files|*.*";
            this.addEmuDialog.Title = "Add Emulator";
            // 
            // LibraryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 513);
            this.Controls.Add(this.libraryTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LibraryWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ROM Image Library";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LibraryWindow_FormClosing);
            this.libraryTabControl.ResumeLayout(false);
            this.romImageTabPage.ResumeLayout(false);
            this.romImageTable.ResumeLayout(false);
            this.romImagePageFlowLayout.ResumeLayout(false);
            this.emuTabPage.ResumeLayout(false);
            this.emuTable.ResumeLayout(false);
            this.emuPageFlowLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl libraryTabControl;
        private System.Windows.Forms.TabPage romImageTabPage;
        private System.Windows.Forms.TableLayoutPanel romImageTable;
        private System.Windows.Forms.FlowLayoutPanel romImagePageFlowLayout;
        private System.Windows.Forms.Button addROMImageButton;
        private System.Windows.Forms.TabPage emuTabPage;
        private System.Windows.Forms.TableLayoutPanel emuTable;
        private System.Windows.Forms.FlowLayoutPanel emuPageFlowLayout;
        private System.Windows.Forms.Button addEmulatorButton;
        private System.Windows.Forms.OpenFileDialog addRomImageDialog;
        private System.Windows.Forms.OpenFileDialog addEmuDialog;
        private System.Windows.Forms.Button romImageRemoveButton;
        private System.Windows.Forms.ListView emuListView;
        private System.Windows.Forms.Button emuRemoveButton;
        private System.Windows.Forms.ColumnHeader pathColumn;
        private System.Windows.Forms.ListView romImageListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;

    }
}
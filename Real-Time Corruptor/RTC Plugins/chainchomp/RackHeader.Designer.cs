namespace ChainChomp
{
    partial class RackHeader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RackHeader));
            this.openPresetDialog = new System.Windows.Forms.OpenFileDialog();
            this.savePresetDialog = new System.Windows.Forms.SaveFileDialog();
            this.headerTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.savePresetButton = new System.Windows.Forms.Button();
            this.presetsList = new System.Windows.Forms.ComboBox();
            this.loadPresetButton = new System.Windows.Forms.Button();
            this.removePluginButton = new System.Windows.Forms.Button();
            this.positionSwitch1 = new ChainChomp.PositionSwitch();
            this.hiddenButton = new System.Windows.Forms.Button();
            this.rachHeaderToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.headerTableLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // openPresetDialog
            // 
            this.openPresetDialog.Title = "Open Preset";
            // 
            // savePresetDialog
            // 
            this.savePresetDialog.Title = "Save Preset";
            // 
            // headerTableLayout
            // 
            this.headerTableLayout.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("headerTableLayout.BackgroundImage")));
            this.headerTableLayout.ColumnCount = 6;
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 316F));
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.headerTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.headerTableLayout.Controls.Add(this.savePresetButton, 3, 0);
            this.headerTableLayout.Controls.Add(this.presetsList, 1, 0);
            this.headerTableLayout.Controls.Add(this.loadPresetButton, 2, 0);
            this.headerTableLayout.Controls.Add(this.removePluginButton, 5, 0);
            this.headerTableLayout.Controls.Add(this.positionSwitch1, 0, 0);
            this.headerTableLayout.Controls.Add(this.hiddenButton, 4, 0);
            this.headerTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerTableLayout.Location = new System.Drawing.Point(0, 0);
            this.headerTableLayout.Margin = new System.Windows.Forms.Padding(0);
            this.headerTableLayout.Name = "headerTableLayout";
            this.headerTableLayout.Padding = new System.Windows.Forms.Padding(6);
            this.headerTableLayout.RowCount = 1;
            this.headerTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.headerTableLayout.Size = new System.Drawing.Size(600, 41);
            this.headerTableLayout.TabIndex = 0;
            // 
            // savePresetButton
            // 
            this.savePresetButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.savePresetButton.BackgroundImage = global::ChainChomp.Properties.Resources.disk16;
            this.savePresetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.savePresetButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.savePresetButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.savePresetButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.savePresetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.savePresetButton.Location = new System.Drawing.Point(413, 6);
            this.savePresetButton.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.savePresetButton.Name = "savePresetButton";
            this.savePresetButton.Size = new System.Drawing.Size(28, 28);
            this.savePresetButton.TabIndex = 4;
            this.rachHeaderToolTip.SetToolTip(this.savePresetButton, "Save corruptor settings to preset");
            this.savePresetButton.UseVisualStyleBackColor = false;
            this.savePresetButton.Click += new System.EventHandler(this.savePresetButton_Click);
            // 
            // presetsList
            // 
            this.presetsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.presetsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetsList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetsList.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.presetsList.FormattingEnabled = true;
            this.presetsList.Location = new System.Drawing.Point(48, 8);
            this.presetsList.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.presetsList.Name = "presetsList";
            this.presetsList.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.presetsList.Size = new System.Drawing.Size(316, 24);
            this.presetsList.TabIndex = 1;
            this.rachHeaderToolTip.SetToolTip(this.presetsList, "Quick-select a preset from the most recently visited directory");
            this.presetsList.DropDown += new System.EventHandler(this.presetsList_DropDown);
            this.presetsList.SelectionChangeCommitted += new System.EventHandler(this.presetsList_SelectionChangeCommitted);
            this.presetsList.DropDownClosed += new System.EventHandler(this.presetsList_DropDownClosed);
            // 
            // loadPresetButton
            // 
            this.loadPresetButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.loadPresetButton.BackgroundImage = global::ChainChomp.Properties.Resources.folder16;
            this.loadPresetButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.loadPresetButton.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.loadPresetButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.loadPresetButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.loadPresetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadPresetButton.Location = new System.Drawing.Point(371, 6);
            this.loadPresetButton.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.loadPresetButton.Name = "loadPresetButton";
            this.loadPresetButton.Size = new System.Drawing.Size(28, 28);
            this.loadPresetButton.TabIndex = 3;
            this.rachHeaderToolTip.SetToolTip(this.loadPresetButton, "Load corruptor settings from preset");
            this.loadPresetButton.UseVisualStyleBackColor = false;
            this.loadPresetButton.Click += new System.EventHandler(this.loadPresetButton_Click);
            // 
            // removePluginButton
            // 
            this.removePluginButton.BackColor = System.Drawing.Color.Crimson;
            this.removePluginButton.BackgroundImage = global::ChainChomp.Properties.Resources.cross16;
            this.removePluginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.removePluginButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(7)))), ((int)(((byte)(34)))));
            this.removePluginButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(7)))), ((int)(((byte)(34)))));
            this.removePluginButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(29)))), ((int)(((byte)(80)))));
            this.removePluginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removePluginButton.Location = new System.Drawing.Point(555, 6);
            this.removePluginButton.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.removePluginButton.Name = "removePluginButton";
            this.removePluginButton.Size = new System.Drawing.Size(28, 28);
            this.removePluginButton.TabIndex = 5;
            this.rachHeaderToolTip.SetToolTip(this.removePluginButton, "Remove this corruptor (un-saved changes will be lost)");
            this.removePluginButton.UseVisualStyleBackColor = false;
            this.removePluginButton.Click += new System.EventHandler(this.removePluginButton_Click);
            // 
            // positionSwitch1
            // 
            this.positionSwitch1.Location = new System.Drawing.Point(8, 6);
            this.positionSwitch1.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.positionSwitch1.Name = "positionSwitch1";
            this.positionSwitch1.Size = new System.Drawing.Size(36, 28);
            this.positionSwitch1.TabIndex = 6;
            // 
            // hiddenButton
            // 
            this.hiddenButton.BackColor = System.Drawing.Color.Transparent;
            this.hiddenButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hiddenButton.FlatAppearance.BorderSize = 0;
            this.hiddenButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.hiddenButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.hiddenButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hiddenButton.ForeColor = System.Drawing.Color.Transparent;
            this.hiddenButton.Location = new System.Drawing.Point(451, 9);
            this.hiddenButton.Name = "hiddenButton";
            this.hiddenButton.Size = new System.Drawing.Size(94, 23);
            this.hiddenButton.TabIndex = 7;
            this.hiddenButton.TabStop = false;
            this.hiddenButton.UseVisualStyleBackColor = false;
            this.hiddenButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hiddenButton_MouseDown);
            // 
            // RackHeader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SlateGray;
            this.Controls.Add(this.headerTableLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RackHeader";
            this.Size = new System.Drawing.Size(600, 41);
            this.headerTableLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openPresetDialog;
        private System.Windows.Forms.SaveFileDialog savePresetDialog;
        private System.Windows.Forms.TableLayoutPanel headerTableLayout;
        public System.Windows.Forms.ComboBox presetsList;
        private System.Windows.Forms.Button loadPresetButton;
        private System.Windows.Forms.Button savePresetButton;
        private System.Windows.Forms.Button removePluginButton;
        private PositionSwitch positionSwitch1;
        private System.Windows.Forms.ToolTip rachHeaderToolTip;
        private System.Windows.Forms.Button hiddenButton;
    }
}

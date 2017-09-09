namespace ChainChomp
{
    partial class ChainEditor
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
            this.saveChainDialog = new System.Windows.Forms.SaveFileDialog();
            this.openChainDialog = new System.Windows.Forms.OpenFileDialog();
            this.editorToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pluginDropDown = new System.Windows.Forms.ComboBox();
            this.addToChainButton = new System.Windows.Forms.Button();
            this.openChainButton = new System.Windows.Forms.Button();
            this.saveChainButton = new System.Windows.Forms.Button();
            this.enableChainCheckBox = new System.Windows.Forms.CheckBox();
            this.removeChainButton = new System.Windows.Forms.Button();
            this.startOffsetTicker = new System.Windows.Forms.NumericUpDown();
            this.endOffsetTicker = new System.Windows.Forms.NumericUpDown();
            this.setToROMEnd = new System.Windows.Forms.Button();
            this.rack = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.startOffsetTicker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endOffsetTicker)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveChainDialog
            // 
            this.saveChainDialog.DefaultExt = "chain";
            this.saveChainDialog.FileName = "myChain";
            this.saveChainDialog.Filter = "Chain files|*.chain|All files|*.*";
            this.saveChainDialog.Title = "Save Chain";
            // 
            // openChainDialog
            // 
            this.openChainDialog.DefaultExt = "chain";
            this.openChainDialog.Filter = "Chain files|*.chain|All files|*.*";
            this.openChainDialog.Title = "Open Chain";
            // 
            // pluginDropDown
            // 
            this.pluginDropDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pluginDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pluginDropDown.FormattingEnabled = true;
            this.pluginDropDown.Location = new System.Drawing.Point(3, 71);
            this.pluginDropDown.Name = "pluginDropDown";
            this.pluginDropDown.Size = new System.Drawing.Size(301, 21);
            this.pluginDropDown.TabIndex = 0;
            this.editorToolTip.SetToolTip(this.pluginDropDown, "Select an available corruptor");
            this.pluginDropDown.SelectedIndexChanged += new System.EventHandler(this.pluginDropDown_SelectedIndexChanged);
            // 
            // addToChainButton
            // 
            this.addToChainButton.AutoSize = true;
            this.addToChainButton.Image = global::ChainChomp.Properties.Resources.plus8;
            this.addToChainButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addToChainButton.Location = new System.Drawing.Point(3, 2);
            this.addToChainButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.addToChainButton.Name = "addToChainButton";
            this.addToChainButton.Size = new System.Drawing.Size(87, 24);
            this.addToChainButton.TabIndex = 0;
            this.addToChainButton.Text = "   Add to Chain";
            this.editorToolTip.SetToolTip(this.addToChainButton, "Add the selected corruptor to the chain rack");
            this.addToChainButton.UseVisualStyleBackColor = true;
            this.addToChainButton.Click += new System.EventHandler(this.addToChainButton_Click);
            // 
            // openChainButton
            // 
            this.openChainButton.Location = new System.Drawing.Point(3, 3);
            this.openChainButton.Name = "openChainButton";
            this.openChainButton.Size = new System.Drawing.Size(75, 23);
            this.openChainButton.TabIndex = 0;
            this.openChainButton.Text = "Open Chain";
            this.editorToolTip.SetToolTip(this.openChainButton, "Load chain from file");
            this.openChainButton.UseVisualStyleBackColor = true;
            this.openChainButton.Click += new System.EventHandler(this.openChainButton_Click);
            // 
            // saveChainButton
            // 
            this.saveChainButton.Location = new System.Drawing.Point(84, 3);
            this.saveChainButton.Name = "saveChainButton";
            this.saveChainButton.Size = new System.Drawing.Size(75, 23);
            this.saveChainButton.TabIndex = 1;
            this.saveChainButton.Text = "Save Chain";
            this.editorToolTip.SetToolTip(this.saveChainButton, "Save chain to file");
            this.saveChainButton.UseVisualStyleBackColor = true;
            this.saveChainButton.Click += new System.EventHandler(this.saveChainButton_Click);
            // 
            // enableChainCheckBox
            // 
            this.enableChainCheckBox.AutoSize = true;
            this.enableChainCheckBox.Checked = true;
            this.enableChainCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableChainCheckBox.Location = new System.Drawing.Point(182, 7);
            this.enableChainCheckBox.Margin = new System.Windows.Forms.Padding(20, 7, 3, 3);
            this.enableChainCheckBox.Name = "enableChainCheckBox";
            this.enableChainCheckBox.Size = new System.Drawing.Size(87, 17);
            this.enableChainCheckBox.TabIndex = 3;
            this.enableChainCheckBox.Text = "enable chain";
            this.editorToolTip.SetToolTip(this.enableChainCheckBox, "This chain is enabled and will be run");
            this.enableChainCheckBox.UseVisualStyleBackColor = true;
            this.enableChainCheckBox.CheckedChanged += new System.EventHandler(this.enableChainCheckBox_CheckedChanged);
            // 
            // removeChainButton
            // 
            this.removeChainButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.removeChainButton.Location = new System.Drawing.Point(518, 3);
            this.removeChainButton.Name = "removeChainButton";
            this.removeChainButton.Size = new System.Drawing.Size(94, 29);
            this.removeChainButton.TabIndex = 2;
            this.removeChainButton.Text = "Remove Chain";
            this.editorToolTip.SetToolTip(this.removeChainButton, "Remove this Chain (un-saved changes will be lost!)");
            this.removeChainButton.UseVisualStyleBackColor = true;
            this.removeChainButton.Click += new System.EventHandler(this.removeChainButton_Click);
            // 
            // startOffsetTicker
            // 
            this.startOffsetTicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startOffsetTicker.Hexadecimal = true;
            this.startOffsetTicker.Location = new System.Drawing.Point(69, 3);
            this.startOffsetTicker.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.startOffsetTicker.Name = "startOffsetTicker";
            this.startOffsetTicker.Size = new System.Drawing.Size(120, 20);
            this.startOffsetTicker.TabIndex = 1;
            this.startOffsetTicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.editorToolTip.SetToolTip(this.startOffsetTicker, "The chain will run over all bytes starting at this address");
            this.startOffsetTicker.ValueChanged += new System.EventHandler(this.startOffsetTicker_ValueChanged);
            // 
            // endOffsetTicker
            // 
            this.endOffsetTicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endOffsetTicker.Hexadecimal = true;
            this.endOffsetTicker.Location = new System.Drawing.Point(258, 3);
            this.endOffsetTicker.Maximum = new decimal(new int[] {
            1661992959,
            1808227885,
            5,
            0});
            this.endOffsetTicker.Name = "endOffsetTicker";
            this.endOffsetTicker.Size = new System.Drawing.Size(120, 20);
            this.endOffsetTicker.TabIndex = 3;
            this.endOffsetTicker.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.editorToolTip.SetToolTip(this.endOffsetTicker, "The chain will finish running once it has reached this address.");
            this.endOffsetTicker.Value = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.endOffsetTicker.ValueChanged += new System.EventHandler(this.endOffsetTicker_ValueChanged);
            // 
            // setToROMEnd
            // 
            this.setToROMEnd.Location = new System.Drawing.Point(384, 2);
            this.setToROMEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
            this.setToROMEnd.Name = "setToROMEnd";
            this.setToROMEnd.Size = new System.Drawing.Size(106, 23);
            this.setToROMEnd.TabIndex = 4;
            this.setToROMEnd.Text = "Set to ROM End";
            this.editorToolTip.SetToolTip(this.setToROMEnd, "Sets the End Offset to the length of the current ROM Image");
            this.setToROMEnd.UseVisualStyleBackColor = true;
            this.setToROMEnd.Click += new System.EventHandler(this.setToROMEnd_Click);
            // 
            // rack
            // 
            this.rack.AutoScroll = true;
            this.rack.AutoScrollMinSize = new System.Drawing.Size(0, 800);
            this.rack.BackColor = System.Drawing.Color.SlateGray;
            this.rack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rack.Dock = System.Windows.Forms.DockStyle.Top;
            this.rack.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.rack.Location = new System.Drawing.Point(0, 100);
            this.rack.Margin = new System.Windows.Forms.Padding(0);
            this.rack.MaximumSize = new System.Drawing.Size(621, 4000);
            this.rack.MinimumSize = new System.Drawing.Size(621, 250);
            this.rack.Name = "rack";
            this.rack.Size = new System.Drawing.Size(621, 546);
            this.rack.TabIndex = 3;
            this.editorToolTip.SetToolTip(this.rack, "Chain rack");
            this.rack.WrapContents = false;
            this.rack.Click += new System.EventHandler(this.rack_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel6);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(621, 100);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.pluginDropDown, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel2, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.removeChainButton, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel4, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.MaximumSize = new System.Drawing.Size(615, 98);
            this.tableLayoutPanel6.MinimumSize = new System.Drawing.Size(615, 98);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.81967F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.18033F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(615, 98);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.addToChainButton);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(307, 68);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(308, 30);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.openChainButton);
            this.flowLayoutPanel3.Controls.Add(this.saveChainButton);
            this.flowLayoutPanel3.Controls.Add(this.enableChainCheckBox);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(307, 35);
            this.flowLayoutPanel3.TabIndex = 2;
            // 
            // flowLayoutPanel4
            // 
            this.tableLayoutPanel6.SetColumnSpan(this.flowLayoutPanel4, 2);
            this.flowLayoutPanel4.Controls.Add(this.label1);
            this.flowLayoutPanel4.Controls.Add(this.startOffsetTicker);
            this.flowLayoutPanel4.Controls.Add(this.label2);
            this.flowLayoutPanel4.Controls.Add(this.endOffsetTicker);
            this.flowLayoutPanel4.Controls.Add(this.setToROMEnd);
            this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(0, 35);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(615, 33);
            this.flowLayoutPanel4.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start Offset";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 6);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "End Offset";
            // 
            // ChainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.rack);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(621, 4000);
            this.MinimumSize = new System.Drawing.Size(621, 646);
            this.Name = "ChainEditor";
            this.Size = new System.Drawing.Size(621, 646);
            ((System.ComponentModel.ISupportInitialize)(this.startOffsetTicker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endOffsetTicker)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.SaveFileDialog saveChainDialog;
        private System.Windows.Forms.OpenFileDialog openChainDialog;
		private System.Windows.Forms.ToolTip editorToolTip;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.ComboBox pluginDropDown;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button addToChainButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Button openChainButton;
		private System.Windows.Forms.Button saveChainButton;
		public System.Windows.Forms.CheckBox enableChainCheckBox;
		public System.Windows.Forms.Button removeChainButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown startOffsetTicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown endOffsetTicker;
		private System.Windows.Forms.Button setToROMEnd;
		private System.Windows.Forms.FlowLayoutPanel rack;
    }
}

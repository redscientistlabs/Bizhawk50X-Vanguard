namespace ChainChomp
{
    partial class PositionSwitch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PositionSwitch));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.positionSwitchToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.downButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.upButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(36, 28);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // downButton
            // 
            this.downButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.downButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("downButton.BackgroundImage")));
            this.downButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.downButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.downButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.downButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downButton.Location = new System.Drawing.Point(0, 14);
            this.downButton.Margin = new System.Windows.Forms.Padding(0);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(36, 14);
            this.downButton.TabIndex = 1;
            this.positionSwitchToolTip.SetToolTip(this.downButton, "Move this corruptor down");
            this.downButton.UseVisualStyleBackColor = false;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.upButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("upButton.BackgroundImage")));
            this.upButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.upButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.upButton.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ButtonShadow;
            this.upButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upButton.Location = new System.Drawing.Point(0, 0);
            this.upButton.Margin = new System.Windows.Forms.Padding(0);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(36, 14);
            this.upButton.TabIndex = 0;
            this.positionSwitchToolTip.SetToolTip(this.upButton, "Move this corruptor up");
            this.upButton.UseVisualStyleBackColor = false;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // PositionSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PositionSwitch";
            this.Size = new System.Drawing.Size(36, 28);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.ToolTip positionSwitchToolTip;
    }
}

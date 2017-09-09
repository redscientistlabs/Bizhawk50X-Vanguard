namespace ChompAndFriends
{
    partial class KamekUI
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
            this.kamekToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.seed = new System.Windows.Forms.TextBox();
            this.maxOffset = new System.Windows.Forms.TextBox();
            this.jumpFixedRadioButton = new System.Windows.Forms.RadioButton();
            this.jumpVariableRadioButton = new System.Windows.Forms.RadioButton();
            this.valueFixedRadioButton = new System.Windows.Forms.RadioButton();
            this.valueVariableRadioButton = new System.Windows.Forms.RadioButton();
            this.backgroundFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.resetOnRun = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.backgroundFlowLayout.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // seed
            // 
            this.seed.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seed.Location = new System.Drawing.Point(5, 5);
            this.seed.Margin = new System.Windows.Forms.Padding(5);
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(98, 18);
            this.seed.TabIndex = 0;
            this.seed.Text = "314159265358979323846264338327950288419716939937510582097494459230781640628620899" +
    "8628034825342117067982148";
            this.kamekToolTip.SetToolTip(this.seed, "Seed for the randomiser");
            this.seed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seed_KeyPress);
            // 
            // maxOffset
            // 
            this.maxOffset.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxOffset.Location = new System.Drawing.Point(5, 43);
            this.maxOffset.Margin = new System.Windows.Forms.Padding(5, 15, 5, 5);
            this.maxOffset.Name = "maxOffset";
            this.maxOffset.Size = new System.Drawing.Size(98, 18);
            this.maxOffset.TabIndex = 3;
            this.maxOffset.Text = "256";
            this.maxOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.kamekToolTip.SetToolTip(this.maxOffset, "Jump offset maximum (smaller jumps = more corrupted bytes)");
            this.maxOffset.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.seed_KeyPress);
            // 
            // jumpFixedRadioButton
            // 
            this.jumpFixedRadioButton.AutoSize = true;
            this.jumpFixedRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.jumpFixedRadioButton.Checked = true;
            this.jumpFixedRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.jumpFixedRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jumpFixedRadioButton.Location = new System.Drawing.Point(3, 3);
            this.jumpFixedRadioButton.Name = "jumpFixedRadioButton";
            this.jumpFixedRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.jumpFixedRadioButton.Size = new System.Drawing.Size(100, 22);
            this.jumpFixedRadioButton.TabIndex = 3;
            this.jumpFixedRadioButton.TabStop = true;
            this.kamekToolTip.SetToolTip(this.jumpFixedRadioButton, "Generate jump offset before corruption. All jumps will use this number");
            this.jumpFixedRadioButton.UseVisualStyleBackColor = false;
            // 
            // jumpVariableRadioButton
            // 
            this.jumpVariableRadioButton.AutoSize = true;
            this.jumpVariableRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.jumpVariableRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.jumpVariableRadioButton.Location = new System.Drawing.Point(3, 31);
            this.jumpVariableRadioButton.MinimumSize = new System.Drawing.Size(100, 0);
            this.jumpVariableRadioButton.Name = "jumpVariableRadioButton";
            this.jumpVariableRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.jumpVariableRadioButton.Size = new System.Drawing.Size(100, 22);
            this.jumpVariableRadioButton.TabIndex = 2;
            this.kamekToolTip.SetToolTip(this.jumpVariableRadioButton, "Generate jump offset for each jump");
            this.jumpVariableRadioButton.UseVisualStyleBackColor = false;
            // 
            // valueFixedRadioButton
            // 
            this.valueFixedRadioButton.AutoSize = true;
            this.valueFixedRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.valueFixedRadioButton.Checked = true;
            this.valueFixedRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.valueFixedRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.valueFixedRadioButton.Location = new System.Drawing.Point(3, 3);
            this.valueFixedRadioButton.Name = "valueFixedRadioButton";
            this.valueFixedRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.valueFixedRadioButton.Size = new System.Drawing.Size(100, 22);
            this.valueFixedRadioButton.TabIndex = 3;
            this.valueFixedRadioButton.TabStop = true;
            this.kamekToolTip.SetToolTip(this.valueFixedRadioButton, "Generate value before corruption. All corrupted bytes will be set to this value");
            this.valueFixedRadioButton.UseVisualStyleBackColor = false;
            // 
            // valueVariableRadioButton
            // 
            this.valueVariableRadioButton.AutoSize = true;
            this.valueVariableRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.valueVariableRadioButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.valueVariableRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.valueVariableRadioButton.Location = new System.Drawing.Point(3, 31);
            this.valueVariableRadioButton.MinimumSize = new System.Drawing.Size(100, 0);
            this.valueVariableRadioButton.Name = "valueVariableRadioButton";
            this.valueVariableRadioButton.Padding = new System.Windows.Forms.Padding(5);
            this.valueVariableRadioButton.Size = new System.Drawing.Size(100, 22);
            this.valueVariableRadioButton.TabIndex = 2;
            this.kamekToolTip.SetToolTip(this.valueVariableRadioButton, "Generate value for each corrupted byte");
            this.valueVariableRadioButton.UseVisualStyleBackColor = false;
            // 
            // backgroundFlowLayout
            // 
            this.backgroundFlowLayout.BackgroundImage = global::ChompAndFriends.Properties.Resources.kamek;
            this.backgroundFlowLayout.Controls.Add(this.flowLayoutPanel1);
            this.backgroundFlowLayout.Controls.Add(this.flowLayoutPanel2);
            this.backgroundFlowLayout.Controls.Add(this.flowLayoutPanel3);
            this.backgroundFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.backgroundFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.backgroundFlowLayout.Name = "backgroundFlowLayout";
            this.backgroundFlowLayout.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel1.Controls.Add(this.seed);
            this.flowLayoutPanel1.Controls.Add(this.maxOffset);
            this.flowLayoutPanel1.Controls.Add(this.resetOnRun);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(48, 60);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(48, 60, 3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(108, 92);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // resetOnRun
            // 
            this.resetOnRun.AutoSize = true;
            this.resetOnRun.Checked = true;
            this.resetOnRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resetOnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetOnRun.Location = new System.Drawing.Point(5, 74);
            this.resetOnRun.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.resetOnRun.MinimumSize = new System.Drawing.Size(100, 0);
            this.resetOnRun.Name = "resetOnRun";
            this.resetOnRun.Padding = new System.Windows.Forms.Padding(2);
            this.resetOnRun.Size = new System.Drawing.Size(100, 15);
            this.resetOnRun.TabIndex = 2;
            this.resetOnRun.UseVisualStyleBackColor = false;
            this.resetOnRun.CheckedChanged += new System.EventHandler(this.resetOnRun_CheckedChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel2.Controls.Add(this.jumpFixedRadioButton);
            this.flowLayoutPanel2.Controls.Add(this.jumpVariableRadioButton);
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(169, 60);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(10, 60, 3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(108, 60);
            this.flowLayoutPanel2.TabIndex = 4;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel3.Controls.Add(this.valueFixedRadioButton);
            this.flowLayoutPanel3.Controls.Add(this.valueVariableRadioButton);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(290, 60);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(10, 60, 3, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(108, 60);
            this.flowLayoutPanel3.TabIndex = 5;
            this.flowLayoutPanel3.WrapContents = false;
            // 
            // KamekUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.backgroundFlowLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "KamekUI";
            this.Size = new System.Drawing.Size(600, 200);
            this.backgroundFlowLayout.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip kamekToolTip;
        private System.Windows.Forms.FlowLayoutPanel backgroundFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        public System.Windows.Forms.TextBox seed;
        public System.Windows.Forms.CheckBox resetOnRun;
        public System.Windows.Forms.RadioButton jumpFixedRadioButton;
        public System.Windows.Forms.RadioButton jumpVariableRadioButton;
        public System.Windows.Forms.RadioButton valueFixedRadioButton;
        public System.Windows.Forms.RadioButton valueVariableRadioButton;
        public System.Windows.Forms.TextBox maxOffset;
    }
}

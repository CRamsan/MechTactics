namespace MechTactics
{
    partial class Launcher
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
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.groupBoxDisplay = new System.Windows.Forms.GroupBox();
            this.glControl = new OpenTK.GLControl();
            this.watchBox = new System.Windows.Forms.GroupBox();
            this.btnVisualize = new System.Windows.Forms.Button();
            this.btnFileChooser = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileChosen = new System.Windows.Forms.TextBox();
            this.fileChooseDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxControls = new System.Windows.Forms.GroupBox();
            this.buttonListen = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.groupBoxOutput.SuspendLayout();
            this.groupBoxDisplay.SuspendLayout();
            this.watchBox.SuspendLayout();
            this.groupBoxControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutput.Controls.Add(this.textBoxOutput);
            this.groupBoxOutput.Location = new System.Drawing.Point(380, 567);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(292, 104);
            this.groupBoxOutput.TabIndex = 16;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(9, 19);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(277, 67);
            this.textBoxOutput.TabIndex = 10;
            // 
            // groupBoxDisplay
            // 
            this.groupBoxDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxDisplay.Controls.Add(this.glControl);
            this.groupBoxDisplay.Location = new System.Drawing.Point(12, 12);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new System.Drawing.Size(654, 549);
            this.groupBoxDisplay.TabIndex = 15;
            this.groupBoxDisplay.TabStop = false;
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(6, 19);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(642, 524);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // watchBox
            // 
            this.watchBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.watchBox.Controls.Add(this.btnVisualize);
            this.watchBox.Controls.Add(this.btnFileChooser);
            this.watchBox.Controls.Add(this.label1);
            this.watchBox.Controls.Add(this.txtFileChosen);
            this.watchBox.Location = new System.Drawing.Point(12, 568);
            this.watchBox.Name = "watchBox";
            this.watchBox.Size = new System.Drawing.Size(362, 41);
            this.watchBox.TabIndex = 17;
            this.watchBox.TabStop = false;
            this.watchBox.Text = "Saved Game";
            // 
            // btnVisualize
            // 
            this.btnVisualize.Enabled = false;
            this.btnVisualize.Location = new System.Drawing.Point(309, 11);
            this.btnVisualize.Name = "btnVisualize";
            this.btnVisualize.Size = new System.Drawing.Size(47, 23);
            this.btnVisualize.TabIndex = 6;
            this.btnVisualize.Text = "Play";
            this.btnVisualize.UseVisualStyleBackColor = true;
            // 
            // btnFileChooser
            // 
            this.btnFileChooser.Location = new System.Drawing.Point(255, 11);
            this.btnFileChooser.Name = "btnFileChooser";
            this.btnFileChooser.Size = new System.Drawing.Size(48, 23);
            this.btnFileChooser.TabIndex = 4;
            this.btnFileChooser.Text = "...";
            this.btnFileChooser.UseVisualStyleBackColor = true;
            this.btnFileChooser.Click += new System.EventHandler(this.btnFileChooser_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select a File";
            // 
            // txtFileChosen
            // 
            this.txtFileChosen.Location = new System.Drawing.Point(77, 13);
            this.txtFileChosen.Name = "txtFileChosen";
            this.txtFileChosen.Size = new System.Drawing.Size(172, 20);
            this.txtFileChosen.TabIndex = 3;
            // 
            // groupBoxControls
            // 
            this.groupBoxControls.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxControls.Controls.Add(this.buttonListen);
            this.groupBoxControls.Controls.Add(this.buttonStop);
            this.groupBoxControls.Controls.Add(this.buttonPlay);
            this.groupBoxControls.Location = new System.Drawing.Point(12, 614);
            this.groupBoxControls.Name = "groupBoxControls";
            this.groupBoxControls.Size = new System.Drawing.Size(362, 57);
            this.groupBoxControls.TabIndex = 18;
            this.groupBoxControls.TabStop = false;
            this.groupBoxControls.Text = "Controls";
            // 
            // buttonListen
            // 
            this.buttonListen.Location = new System.Drawing.Point(6, 19);
            this.buttonListen.Name = "buttonListen";
            this.buttonListen.Size = new System.Drawing.Size(75, 23);
            this.buttonListen.TabIndex = 2;
            this.buttonListen.Text = "Listen";
            this.buttonListen.UseVisualStyleBackColor = true;
            this.buttonListen.Click += new System.EventHandler(this.buttonListen_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(168, 19);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Enabled = false;
            this.buttonPlay.Location = new System.Drawing.Point(87, 19);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(75, 23);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 683);
            this.Controls.Add(this.groupBoxControls);
            this.Controls.Add(this.groupBoxOutput);
            this.Controls.Add(this.watchBox);
            this.Controls.Add(this.groupBoxDisplay);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "Launcher";
            this.Text = "Server";
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.groupBoxDisplay.ResumeLayout(false);
            this.watchBox.ResumeLayout(false);
            this.watchBox.PerformLayout();
            this.groupBoxControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.GroupBox groupBoxDisplay;
        private OpenTK.GLControl glControl;
        private System.Windows.Forms.GroupBox watchBox;
        private System.Windows.Forms.Button btnVisualize;
        private System.Windows.Forms.Button btnFileChooser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileChosen;
        private System.Windows.Forms.OpenFileDialog fileChooseDialog;
        private System.Windows.Forms.GroupBox groupBoxControls;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonListen;

    }
}
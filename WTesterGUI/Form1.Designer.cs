namespace stillbreathing.co.uk.WTesterGUI
{
    partial class WTesterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WTesterForm));
            this.btnRun = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabScript = new System.Windows.Forms.TabPage();
            this.editor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabResults = new System.Windows.Forms.TabPage();
            this.tbOutput = new System.Windows.Forms.RichTextBox();
            this.tabHelp = new System.Windows.Forms.TabPage();
            this.rtbHelp = new System.Windows.Forms.RichTextBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnSave = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabs.SuspendLayout();
            this.tabScript.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editor)).BeginInit();
            this.tabResults.SuspendLayout();
            this.tabHelp.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Image = ((System.Drawing.Image)(resources.GetObject("btnRun.Image")));
            this.btnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRun.Location = new System.Drawing.Point(556, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(77, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "Run Test";
            this.btnRun.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabScript);
            this.tabs.Controls.Add(this.tabResults);
            this.tabs.Controls.Add(this.tabHelp);
            this.tabs.Location = new System.Drawing.Point(12, 41);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(621, 461);
            this.tabs.TabIndex = 5;
            // 
            // tabScript
            // 
            this.tabScript.Controls.Add(this.editor);
            this.tabScript.Location = new System.Drawing.Point(4, 22);
            this.tabScript.Name = "tabScript";
            this.tabScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabScript.Size = new System.Drawing.Size(613, 435);
            this.tabScript.TabIndex = 0;
            this.tabScript.Text = "Test Script";
            this.tabScript.UseVisualStyleBackColor = true;
            // 
            // editor
            // 
            this.editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.editor.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.editor.BackBrush = null;
            this.editor.CharHeight = 14;
            this.editor.CharWidth = 8;
            this.editor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editor.IsReplaceMode = false;
            this.editor.Location = new System.Drawing.Point(3, 3);
            this.editor.Name = "editor";
            this.editor.Paddings = new System.Windows.Forms.Padding(0);
            this.editor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editor.Size = new System.Drawing.Size(607, 429);
            this.editor.TabIndex = 0;
            this.editor.Zoom = 100;
            this.editor.MouseDown += new System.Windows.Forms.MouseEventHandler(this.editor_MouseDown);
            this.editor.MouseMove += new System.Windows.Forms.MouseEventHandler(this.editor_MouseMove);
            this.editor.TextChanged += editor_TextChanged;
            // 
            // tabResults
            // 
            this.tabResults.Controls.Add(this.tbOutput);
            this.tabResults.Location = new System.Drawing.Point(4, 22);
            this.tabResults.Name = "tabResults";
            this.tabResults.Padding = new System.Windows.Forms.Padding(3);
            this.tabResults.Size = new System.Drawing.Size(613, 435);
            this.tabResults.TabIndex = 1;
            this.tabResults.Text = "Results";
            this.tabResults.UseVisualStyleBackColor = true;
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOutput.Location = new System.Drawing.Point(3, 3);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(607, 429);
            this.tbOutput.TabIndex = 5;
            this.tbOutput.Text = "";
            // 
            // tabHelp
            // 
            this.tabHelp.Controls.Add(this.rtbHelp);
            this.tabHelp.Location = new System.Drawing.Point(4, 22);
            this.tabHelp.Name = "tabHelp";
            this.tabHelp.Size = new System.Drawing.Size(613, 435);
            this.tabHelp.TabIndex = 2;
            this.tabHelp.Text = "Help";
            this.tabHelp.UseVisualStyleBackColor = true;
            // 
            // rtbHelp
            // 
            this.rtbHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbHelp.Location = new System.Drawing.Point(3, 3);
            this.rtbHelp.Name = "rtbHelp";
            this.rtbHelp.Size = new System.Drawing.Size(607, 427);
            this.rtbHelp.TabIndex = 0;
            this.rtbHelp.Text = "";
            // 
            // btnOpen
            // 
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(84, 23);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "Open Test";
            this.btnOpen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
            this.btnPause.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPause.Location = new System.Drawing.Point(467, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(83, 23);
            this.btnPause.TabIndex = 7;
            this.btnPause.Text = "Pause test";
            this.btnPause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(376, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel test";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbProgress.Location = new System.Drawing.Point(12, 508);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(621, 23);
            this.pbProgress.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(102, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save test";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // WTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 553);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.btnRun);
            this.MinimumSize = new System.Drawing.Size(661, 580);
            this.Name = "WTesterForm";
            this.Text = "WTester";
            this.tabs.ResumeLayout(false);
            this.tabScript.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editor)).EndInit();
            this.tabResults.ResumeLayout(false);
            this.tabHelp.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabScript;
        private System.Windows.Forms.TabPage tabResults;
        private System.Windows.Forms.RichTextBox tbOutput;
        private System.Windows.Forms.TabPage tabHelp;
        private System.Windows.Forms.RichTextBox rtbHelp;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private FastColoredTextBoxNS.FastColoredTextBox editor;
    }
}


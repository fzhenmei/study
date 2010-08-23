namespace JsMinifier
{
    partial class JsMinifierForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsMinifierForm));
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutputFile = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnInputFile = new System.Windows.Forms.Button();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbCompress = new System.Windows.Forms.ToolStripButton();
            this.tbOpenFolder = new System.Windows.Forms.ToolStripButton();
            this.Toolbar = new System.Windows.Forms.ToolStrip();
            this.tbCopyText = new System.Windows.Forms.ToolStripButton();
            this.tbExit = new System.Windows.Forms.ToolStripButton();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.Status.SuspendLayout();
            this.Toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInputFile
            // 
            this.txtInputFile.AllowDrop = true;
            this.txtInputFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtInputFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtInputFile.Location = new System.Drawing.Point(13, 49);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(504, 20);
            this.txtInputFile.TabIndex = 0;
            this.txtInputFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragDrop);
            this.txtInputFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input File (.js file or *.js for batch conversion):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(354, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Output File (if left out files are automatically created with .min.js extension):" +
                "";
            // 
            // txtOutputFile
            // 
            this.txtOutputFile.AllowDrop = true;
            this.txtOutputFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtOutputFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtOutputFile.Location = new System.Drawing.Point(13, 92);
            this.txtOutputFile.Name = "txtOutputFile";
            this.txtOutputFile.Size = new System.Drawing.Size(504, 20);
            this.txtOutputFile.TabIndex = 3;
            this.txtOutputFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragDrop);
            this.txtOutputFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtInputFile_DragEnter);
            // 
            // txtOutput
            // 
            this.txtOutput.AcceptsReturn = true;
            this.txtOutput.AcceptsTab = true;
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(13, 125);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(531, 216);
            this.txtOutput.TabIndex = 5;
            this.txtOutput.WordWrap = false;
            // 
            // btnInputFile
            // 
            this.btnInputFile.AutoSize = true;
            this.btnInputFile.Location = new System.Drawing.Point(518, 48);
            this.btnInputFile.Name = "btnInputFile";
            this.btnInputFile.Size = new System.Drawing.Size(26, 23);
            this.btnInputFile.TabIndex = 6;
            this.btnInputFile.Text = "...";
            this.btnInputFile.UseVisualStyleBackColor = true;
            this.btnInputFile.Click += new System.EventHandler(this.btnInputFile_Click);
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.AutoSize = true;
            this.btnOutputFile.Location = new System.Drawing.Point(518, 89);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(26, 23);
            this.btnOutputFile.TabIndex = 7;
            this.btnOutputFile.Text = "...";
            this.btnOutputFile.UseVisualStyleBackColor = true;
            this.btnOutputFile.Click += new System.EventHandler(this.btnOutputFile_Click);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.Status.Location = new System.Drawing.Point(0, 344);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(556, 22);
            this.Status.TabIndex = 8;
            this.Status.Text = "Ready";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(39, 17);
            this.StatusLabel.Text = "Ready";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbCompress
            // 
            this.tbCompress.Image = ((System.Drawing.Image)(resources.GetObject("tbCompress.Image")));
            this.tbCompress.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCompress.Name = "tbCompress";
            this.tbCompress.Size = new System.Drawing.Size(80, 22);
            this.tbCompress.Text = "&Compress";
            this.tbCompress.Click += new System.EventHandler(this.btnCompress_Click);
            // 
            // tbOpenFolder
            // 
            this.tbOpenFolder.Image = ((System.Drawing.Image)(resources.GetObject("tbOpenFolder.Image")));
            this.tbOpenFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbOpenFolder.Name = "tbOpenFolder";
            this.tbOpenFolder.Size = new System.Drawing.Size(92, 22);
            this.tbOpenFolder.Text = "&Open Folder";
            this.tbOpenFolder.Click += new System.EventHandler(this.tbOpenFolder_Click);
            // 
            // Toolbar
            // 
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbCompress,
            this.tbCopyText,
            this.tbOpenFolder,
            this.tbExit});
            this.Toolbar.Location = new System.Drawing.Point(0, 0);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Size = new System.Drawing.Size(556, 25);
            this.Toolbar.TabIndex = 9;
            this.Toolbar.Text = "Toolbar";
            this.Toolbar.Paint += new System.Windows.Forms.PaintEventHandler(this.Toolbar_Paint);
            // 
            // tbCopyText
            // 
            this.tbCopyText.Image = ((System.Drawing.Image)(resources.GetObject("tbCopyText.Image")));
            this.tbCopyText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbCopyText.Name = "tbCopyText";
            this.tbCopyText.Size = new System.Drawing.Size(80, 22);
            this.tbCopyText.Text = "Copy Text";
            this.tbCopyText.Click += new System.EventHandler(this.tbCopyText_Click);
            // 
            // tbExit
            // 
            this.tbExit.Image = ((System.Drawing.Image)(resources.GetObject("tbExit.Image")));
            this.tbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbExit.Name = "tbExit";
            this.tbExit.Size = new System.Drawing.Size(45, 22);
            this.tbExit.Text = "E&xit";
            this.tbExit.Click += new System.EventHandler(this.tbExit_Click);
            // 
            // imgLogo
            // 
            this.imgLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.imgLogo.Image = global::JsMinifier.Properties.Resources.wwtoollogo_text;
            this.imgLogo.Location = new System.Drawing.Point(424, 346);
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.Size = new System.Drawing.Size(115, 19);
            this.imgLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgLogo.TabIndex = 10;
            this.imgLogo.TabStop = false;
            this.imgLogo.Click += new System.EventHandler(this.imgLogo_Click);
            // 
            // JsMinifierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 366);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.Toolbar);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.btnOutputFile);
            this.Controls.Add(this.btnInputFile);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOutputFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInputFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(562, 400);
            this.Name = "JsMinifierForm";
            this.Text = "JavaScript Minifier";
            this.Load += new System.EventHandler(this.JsMinifierForm_Load);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOutputFile;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnInputFile;
        private System.Windows.Forms.Button btnOutputFile;
        private System.Windows.Forms.StatusStrip Status;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripButton tbCompress;
        private System.Windows.Forms.ToolStripButton tbOpenFolder;
        private System.Windows.Forms.ToolStrip Toolbar;
        private System.Windows.Forms.ToolStripButton tbCopyText;
        private System.Windows.Forms.ToolStripButton tbExit;
        private System.Windows.Forms.PictureBox imgLogo;
    }
}
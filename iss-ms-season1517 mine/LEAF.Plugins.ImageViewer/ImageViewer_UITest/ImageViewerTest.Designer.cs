namespace LEAF.Plugins.ImageViewer
{
    partial class ImageViewerV2
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
            this.textBoxDownloadFolder = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.prevBtn = new System.Windows.Forms.Button();
            this.nextBtn = new System.Windows.Forms.Button();
            this.chkFit = new System.Windows.Forms.CheckBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.pictureBoxInstImage = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInstImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDownloadFolder
            // 
            this.textBoxDownloadFolder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDownloadFolder.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.textBoxDownloadFolder.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxDownloadFolder.Location = new System.Drawing.Point(6, 9);
            this.textBoxDownloadFolder.Margin = new System.Windows.Forms.Padding(0);
            this.textBoxDownloadFolder.Multiline = true;
            this.textBoxDownloadFolder.Name = "textBoxDownloadFolder";
            this.textBoxDownloadFolder.Size = new System.Drawing.Size(143, 22);
            this.textBoxDownloadFolder.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.prevBtn);
            this.panel1.Controls.Add(this.nextBtn);
            this.panel1.Controls.Add(this.chkFit);
            this.panel1.Controls.Add(this.textBoxDownloadFolder);
            this.panel1.Controls.Add(this.btnDownload);
            this.panel1.Controls.Add(this.btnFolderBrowse);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 40);
            this.panel1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(573, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "1/1";
            // 
            // prevBtn
            // 
            this.prevBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.prevBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.prevBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.prevBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevBtn.Image = global::ImageViewer_UITest.Properties.Resources.prev;
            this.prevBtn.Location = new System.Drawing.Point(547, 12);
            this.prevBtn.Name = "prevBtn";
            this.prevBtn.Size = new System.Drawing.Size(24, 21);
            this.prevBtn.TabIndex = 10;
            this.prevBtn.UseVisualStyleBackColor = true;
            this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
            // 
            // nextBtn
            // 
            this.nextBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.nextBtn.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.nextBtn.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.nextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextBtn.Image = global::ImageViewer_UITest.Properties.Resources.next;
            this.nextBtn.Location = new System.Drawing.Point(598, 12);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(24, 21);
            this.nextBtn.TabIndex = 9;
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // chkFit
            // 
            this.chkFit.AutoSize = true;
            this.chkFit.Location = new System.Drawing.Point(202, 14);
            this.chkFit.Name = "chkFit";
            this.chkFit.Size = new System.Drawing.Size(37, 17);
            this.chkFit.TabIndex = 8;
            this.chkFit.Text = "Fit";
            this.chkFit.UseVisualStyleBackColor = true;
            this.chkFit.CheckedChanged += new System.EventHandler(this.chkFit_CheckedChanged);
            // 
            // btnDownload
            // 
            this.btnDownload.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnDownload.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnDownload.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownload.Image = global::ImageViewer_UITest.Properties.Resources.download;
            this.btnDownload.Location = new System.Drawing.Point(174, 8);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(25, 22);
            this.btnDownload.TabIndex = 1;
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnFolderBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.Control;
            this.btnFolderBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnFolderBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFolderBrowse.Image = global::ImageViewer_UITest.Properties.Resources.folder;
            this.btnFolderBrowse.Location = new System.Drawing.Point(152, 10);
            this.btnFolderBrowse.Margin = new System.Windows.Forms.Padding(0);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(24, 21);
            this.btnFolderBrowse.TabIndex = 7;
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.Window;
            this.panel2.Controls.Add(this.pictureBoxLoading);
            this.panel2.Controls.Add(this.pictureBoxInstImage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(634, 287);
            this.panel2.TabIndex = 10;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Location = new System.Drawing.Point(268, 101);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(66, 58);
            this.pictureBoxLoading.TabIndex = 1;
            this.pictureBoxLoading.TabStop = false;
            // 
            // pictureBoxInstImage
            // 
            this.pictureBoxInstImage.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxInstImage.Name = "pictureBoxInstImage";
            this.pictureBoxInstImage.Size = new System.Drawing.Size(634, 287);
            this.pictureBoxInstImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxInstImage.TabIndex = 0;
            this.pictureBoxInstImage.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // ImageViewerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(634, 327);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ImageViewerTest";
            this.Resize += new System.EventHandler(this.ImageViewerTest_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInstImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDownloadFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.CheckBox chkFit;
        private System.Windows.Forms.Button prevBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBoxInstImage;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
    }
}

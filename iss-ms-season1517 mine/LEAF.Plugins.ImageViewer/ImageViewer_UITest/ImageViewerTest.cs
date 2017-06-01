
using ImageViewer_UITest.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEAF.Plugins.ImageViewer
{
    public partial class ImageViewerV2 : Form
    {
        //private LocDataGridView m_view;

       
        private string url = "";
        List<Image> imageLists = new List<Image>();
        int currentIndex = 1;
        private GifImage gifImage = null;

        public ImageViewerV2()
        {
            InitializeComponent();
            //m_view = ResourcePane.MainDataGridPane as LocDataGridView;
           // m_view.DataGridView.CellMouseClick += DataGridView_CellMouseClick;
            gifImage = new GifImage(Resources.carga);
            timer1.Enabled = true;
            pictureBoxLoading.Visible = false;

            string instruction = @"no image";
            //string instruction = @"asdads https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1   asdasd""asdads https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1   asdasd https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1 https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1 https://pactors.sharepoint.com/sites/D00034/PGSKM/Microsoft/Skype/_layouts/15/guestaccess.aspx?guestaccesstoken=6Cc%2fARc42J4d2UhieYyZBkVsoMczkVG1GF4ozIjPs7w%3d&docid=1318c15bbc32a470b98241c06a2367eb5&rev=1";
            DownloadAllImage(instruction);

        }

      
     
        public bool CanCopy
        {
            get
            {
                return false;
            }
        }

        public bool CanLookUp
        {
            get
            {
                return false;
            }
        }

        public string PluginAuthor
        {
            get
            {
                return "SkypeIntl";
            }
        }

        public string PluginDescription
        {
            get
            {
                return "Display URL Image";
            }
        }

        public string PluginDisplayName
        {
            get
            {
                return "Image Viewer";
            }
        }

        //public Image PluginImage
        //{
        //    get
        //    {
        //        return Resources.icon as Image;
        //    }
        //}

        public string PluginName
        {
            get
            {
                return "Image Viewer";
            }
        }

        public object Settings
        {
            get
            {
                return new object();
            }

            set
            {
            }
        }

        //public void OnActivate(IPluginContext context)
        //{
        //}

        public void OnCleanUp()
        {
        }

        public object OnCopy()
        {
            return new object();
        }

        //public void OnExecute(IPluginContext context)
        //{
        //}

        //public void OnLookUp(IPluginContext context, object obj)
        //{
        //}

        public void OnSettings(string xmlConfig)
        {
        }

        public void OnShutDown()
        {
        }

        public void OnStartUp()
        {
        }

        public void OnSync()
        {
        }

        //public void OnUpdateContext(IPluginContext context)
        //{
        //}
        private void CenterLoadingPicture() {
            Bitmap picImage = Resources.NoUrlImage;
            pictureBoxLoading.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxLoading.Anchor = AnchorStyles.None;
            pictureBoxLoading.Location = new Point((pictureBoxLoading.Parent.ClientSize.Width / 2) - (picImage.Width / 2),
                              (pictureBoxLoading.Parent.ClientSize.Height / 2) - (picImage.Height / 2));
            pictureBoxLoading.Refresh();
        }
        private void DeleteFiles(string path) {
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
        private async void DownloadAllImage(string instructionContent)
        {
            DeleteFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image"));
            string pattern = @"(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)( |\r\n)";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            var result = r.Matches(instructionContent);
            int i = 0;
            foreach (Match m in result)
            {

                string url = m.Value.Trim();
                if (url.Contains("https://pactors.sharepoint.com"))
                {
                    await DownloadTask(i + ".png", url);
                    i++;
                }
            }
            
            pictureBoxLoading.Visible = false;
            pictureBoxInstImage.Image = GetImage(currentIndex - 1);
            label1.Text = currentIndex + " / " + imageLists.Count;
            pictureBoxLoading.Visible = false;
        }

        private async Task DownloadTask(string fileName, string url)
        {
            pictureBoxLoading.Visible = true;
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            await Task.Run(() =>
            {
                var webRequest = WebRequest.Create(url);

                using (var response = webRequest.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        try
                        {
                            var img = Image.FromStream(stream);
                            img.Save(Path.Combine(folderPath, fileName), ImageFormat.Png);
                            Image image = System.Drawing.Image.FromFile(Path.Combine(folderPath, fileName));
                            imageLists.Add(image);
                        
                        }
                        catch (Exception ex)
                        {
                            // OutputPane.AddReport(ex.ToString());
                        }
                    }
                }
            });
        }

        private void SaveToUserFolder() {
             
            try
            {

                string downLoadFolderPath = Path.Combine(textBoxDownloadFolder.Text);
                if (string.IsNullOrWhiteSpace(downLoadFolderPath))
                {
                    MessageBox.Show("Please input a download path before downloading");
                    return;
                }
                else if (!Directory.Exists(downLoadFolderPath))
                {
                    MessageBox.Show(string.Format("{0} does not exist!", downLoadFolderPath));
                    return;
                }
                string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image"), "*.png", SearchOption.AllDirectories);
               
                foreach (string path in files)
                {
                    string fileName=Path.GetFileName(path);
                    string fromPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "image", fileName);
                    string toPath = Path.Combine(downLoadFolderPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png");
                    File.Copy(fromPath, toPath, true);
                }
                // (new Toast(null)).ShowToast("Download Completed!", "See log for more infomation", MessageType.None);
                //OutputPane.AddReport(string.Format("Url Image is downloaded to {0}{1}url:{2}", downloadFilePath, Environment.NewLine, this.url));

            }
            catch (Exception ex)
            {
                //OutputPane.AddReport(ex.ToString());
                return;
            }
        
        }

        private Image GetImage(int i) {
            if (imageLists.Count <= 0)
            {
               

                return Resources.NoUrlImage;
            }
            else {
                var image = imageLists[i];
                if (image != null)
                {
                    return image;
                }
                else return Resources.NoUrlImage;
            }
        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveToUserFolder();
         }

        private async void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (sender is DataGridView)
                {
                    var dataGridview = sender as DataGridView;
                    string instructionContent = dataGridview.SelectedCells[10].Value.ToString();
                    DownloadAllImage(instructionContent);
                }
            }
            catch (Exception ex)
            {
                // OutputPane.AddReport(ex.ToString());
            }
        }

        private void chkFit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFit.Checked)
            {
                var image = GetImage(currentIndex - 1);
                   int imageWidth= image.Width/image.Height*panel2.Height;
                pictureBoxInstImage.Size = new System.Drawing.Size(imageWidth, panel2.Height);

                pictureBoxInstImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            }
            else {
                pictureBoxInstImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            }
        }

        private void prevBtn_Click(object sender, EventArgs e)
        {
            currentIndex--;
            if (currentIndex <= 1)
                currentIndex = 1;
            pictureBoxInstImage.Image = GetImage(currentIndex - 1);
            label1.Text = currentIndex + " / " + imageLists.Count;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            currentIndex++;
            if (currentIndex >= imageLists.Count)
                currentIndex = imageLists.Count;
            pictureBoxInstImage.Image = GetImage(currentIndex - 1);
            label1.Text = currentIndex + " / " + imageLists.Count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBoxLoading.Image = gifImage.GetNextFrame();
        }

        private void btnFolderBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxDownloadFolder.Text=folderBrowserDialog1.SelectedPath;            }
        }

        private void ImageViewerTest_Resize(object sender, EventArgs e)
        {
            CenterLoadingPicture();
        }

      
       
    }
}
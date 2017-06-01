using LEAF.Plugins.ImageViewer.Properties;
using Microsoft.Localization.Framework;
using Microsoft.Localization.Framework.Controls;
using Microsoft.Localization.Framework.OM;
using Microsoft.Localization.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LEAF.Plugins.ImageViewer
{
    public partial class ImageViewer : UserControl, IViewer
    {
        private LocDataGridView m_view;
        private Dictionary<string, Image> imageCache;
        private List<Image> imageLists = new List<Image>();
        private int currentIndex = 1;
        private GifImage gifImage = null;
        private bool isNoImage = false;
        private CancellationTokenSource cts;
        private ThrottleService throttleService;
        private CancelTaskWithCancellationTokenService cancelTaskService = new CancelTaskWithCancellationTokenService();
        private MD5Service md5Service = new MD5Service();
        private MD5 md5 = MD5.Create();
        private string imagesFolderPathOnDisk = "";
        private static object locker = new object();

        public ImageViewer()
        {
            InitializeComponent();
            imagesFolderPathOnDisk = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image");
            if (!Directory.Exists(imagesFolderPathOnDisk))
                Directory.CreateDirectory(imagesFolderPathOnDisk);
            imageCache = new Dictionary<string, Image>();
            DirectoryInfo directInfo = new DirectoryInfo(imagesFolderPathOnDisk);
            int totalFilesToKeep = 500;
            foreach (FileInfo fileInfo in directInfo.GetFiles().OrderByDescending(c => c.CreationTime).Skip(totalFilesToKeep))
            {
                lock (locker)
                {
                    fileInfo.Delete();
                }
            }
            foreach (string filePath in Directory.EnumerateFiles(imagesFolderPathOnDisk, "*.jpg", SearchOption.AllDirectories))
            {
                string urlHash = Path.GetFileNameWithoutExtension(filePath);
                byte[] binary = File.ReadAllBytes(filePath);
                MemoryStream ms = new MemoryStream(binary);
                Image imageFromDisk = new Bitmap(ms);
                imageCache.Add(urlHash, imageFromDisk);
            }
            this.throttleService = new ThrottleService();

            m_view = ResourcePane.MainDataGridPane as LocDataGridView;
            m_view.DataGridView.RowEnter += DataGridView_RowEnter;

            gifImage = new GifImage(Resources.carga);
            timer1.Enabled = true;
            pictureBoxLoading.Visible = false;

            textBoxDownloadFolder.Text = ReadFolderPath();
        }

        private async void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            await SwitchRow(sender, e.RowIndex);
        }

        private async void DataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            await SwitchRow(sender, e.RowIndex);
        }

        private async Task SwitchRow(object sender, int rowIndex)
        {
            if (sender is DataGridView)
            {
                var dataGridview = sender as DataGridView;
                try
                {
                    string instructionContent = ParseInstruction(dataGridview, rowIndex);
                    if (!string.IsNullOrWhiteSpace(instructionContent))
                    {
                        await DownloadAllImagesFromComments(instructionContent);
                    }
                }
                catch (Exception ex)
                {
                    pictureBoxLoading.Visible = false;
                    pictureBoxInstImage.Visible = true;
                    OutputPane.AddReport(ex.ToString());
                }
            }
        }

        private string ParseInstruction(DataGridView dataGridview, int rowIndex)
        {
            string instructionContent = null;
            if (dataGridview.SelectedCells.Count > 0)
            {
                DataGridViewColumn instructionColumn = dataGridview.Columns.Cast<DataGridViewColumn>().FirstOrDefault(c => c.Name == "ColumnInstruction");

                if (instructionColumn != null)
                {
                    instructionContent = dataGridview.Rows[rowIndex].Cells[instructionColumn.Index].Value.ToString();
                }
            }
            return instructionContent;
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

        public Image PluginImage
        {
            get
            {
                return Resources.icon as Image;
            }
        }

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

        public void OnActivate(IPluginContext context)
        {
        }

        public void OnCleanUp()
        {
        }

        public object OnCopy()
        {
            return new object();
        }

        public void OnExecute(IPluginContext context)
        {
        }

        public void OnLookUp(IPluginContext context, object obj)
        {
        }

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

        public void OnUpdateContext(IPluginContext context)
        {
        }

        private async Task DownloadAllImagesFromComments(string instructionContent)
        {
            this.pictureBoxLoading.Visible = true;
            this.pictureBoxInstImage.Visible = false;

            this.throttleService.Throttle(new TimeSpan(0, 0, 0, 0, 200), async () =>
            {
                //cancel existing tasks
                if (this.cts != null && !this.cts.IsCancellationRequested)
                    this.cts.Cancel();
                this.cts = new CancellationTokenSource();

                this.imageLists.Clear();

                string pattern = @"(http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)";
                Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

                // Match the regular expression pattern against a text string.
                MatchCollection result = r.Matches(instructionContent);
                foreach (Match m in result)
                {
                    string url = m.Value.Trim();
                    if (url.Contains("https://pactors.sharepoint.com"))
                    {
                        string urlHash = md5Service.GetMd5Hash(md5, url);
                        if (this.imageCache.ContainsKey(urlHash))
                            this.imageLists.Add(imageCache[urlHash]);
                        else
                        {
                            Image image = await DownloadImage(url, this.cts.Token);
                            if (image != null)
                            {
                                lock (locker)
                                {
                                    image.Save(Path.Combine(this.imagesFolderPathOnDisk, urlHash + ".jpg"));
                                }
                                this.imageCache.Add(urlHash, image);
                                this.imageLists.Add(image);
                            }
                        }
                    }
                }

                pictureBoxLoading.Visible = false;
                pictureBoxInstImage.Visible = true;
                pictureBoxInstImage.Image = GetImage(0);
                chkFit_CheckedChanged(null, null);
                label1.Text = currentIndex + " / " + imageLists.Count;
            });
        }

        private async Task<Image> DownloadImage(string url, CancellationToken token)
        {
            Image result = null;
            pictureBoxLoading.Visible = true;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(url, token))
                    {
                        if (token.IsCancellationRequested)
                            return null;
                        var urlContents = await response.Content.ReadAsByteArrayAsync();
                        using (var stream = new MemoryStream(urlContents))
                        {
                            try
                            {
                                response.EnsureSuccessStatusCode();
                                if (token.IsCancellationRequested)
                                    return null;

                                var img = new Bitmap(stream);
                                return img;
                            }
                            catch (Exception ex)
                            {
                                OutputPane.AddReport(ex.ToString());
                            }
                            OutputPane.AddReport(url);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }

            return result;
        }

        private void SaveToUserFolder()
        {
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

                foreach (var item in imageLists)
                {
                    string toPath = Path.Combine(downLoadFolderPath, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".png");
                    item.Save(toPath);
                    OutputPane.AddReport(string.Format("Url Image is downloaded to {0}", toPath));
                }
                (new Toast(null)).ShowToast("Download Completed!", "See log for more infomation", MessageType.None);
            }
            catch (Exception ex)
            {
                OutputPane.AddReport(ex.ToString());
                return;
            }
        }

        private Image GetImage(int i)
        {
            if (imageLists.Count <= 0)
            {
                isNoImage = true;
                return Resources.NoUrlImage;
            }
            else
            {
                var image = imageLists[i];
                if (image != null)
                {
                    isNoImage = false;
                    return image;
                }
                else
                {
                    isNoImage = true;
                    return Resources.NoUrlImage;
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            SaveToUserFolder();
        }

        private void chkFit_CheckedChanged(object sender, EventArgs e)
        {
            var image = GetImage(currentIndex - 1);
            if (isNoImage == false)
            {
                if (chkFit.Checked)
                {
                    int imageWidth = (int)(((float)image.Width / (float)image.Height) * panel2.Height);
                    pictureBoxInstImage.Size = new System.Drawing.Size(imageWidth, panel2.Height);
                    pictureBoxInstImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pictureBoxInstImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                }
            }
            else
            {
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
                textBoxDownloadFolder.Text = folderBrowserDialog1.SelectedPath;
                SaveFolderPath(folderBrowserDialog1.SelectedPath);
            }
        }

        private void SaveFolderPath(string folderPath)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.AppSettings.Settings.Add("LEAF.Plugins.ImageViewer.Path", folderPath);
            config.Save(ConfigurationSaveMode.Minimal);
        }

        private string ReadFolderPath()
        {
            string path = ConfigurationManager.AppSettings["LEAF.Plugins.ImageViewer.Path"];
            return path;
        }
    }
}
using EmailService;
using Microsoft.Practices.Unity;
using Sfb.Core;
using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;

namespace Sfb.UI.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApkInfoCollection ApkInfoes = new ApkInfoCollection();
        private IUnityContainer container;
        public HistoryCollection Histories = new HistoryCollection();
        private Sfb.Core.SfbInstallationInfo _sfbInstallationInfo = null;

        public Type OfficeAutomationType { get; set; }

        private string ResultFolderPath;

        #region Initialize

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Initialize();
            this.grid_History.ItemsSource = new ObservableCollection<History>();
            LoadLogFromFile();
        }

        private void Initialize()
        {
            var buildVersion = ConfigurationManager.AppSettings["BuildVersion"].ToString();

            //this will trigger text changed event for buildversion textbox
            this.textbox_BuildVersion.Text = buildVersion;

            //load history
            this.grid_History.ItemsSource = this.Histories.Info.OrderBy(h => h.grp).ToList();
        }

        #endregion Initialize

        #region Languages

        private void LoadLanguages(string version, OfficeType officeType)
        {
            this._sfbInstallationInfo = Sfb.Core.SfbUtils.GetSfbInstallationInfo(officeType, version);
            this.radlistbox_languages.ItemsSource = this._sfbInstallationInfo.LanguagePackInfos.Select(c => new Language(c.Language, c)).ToList();
        }

        private List<Language> GetSelectedLanguages()
        {
            var langs = this.radlistbox_languages.ItemsSource as List<Language>;
            var selectedLanguages = new List<Language>();
            foreach (Language lang in langs)
            {
                if (lang.IsChecked == true)
                {
                    selectedLanguages.Add(lang);
                }
            }
            return selectedLanguages;
        }

        #endregion Languages

        #region TestCases

        private List<TestCase> LoadTtestCases(OfficeType officeType)
        {
            var result = new List<TestCase>();
            switch (officeType)
            {
                case OfficeType.O15:
                    this.OfficeAutomationType = typeof(O15.O15BVT_15_0_4809);
                    break;

                case OfficeType.O16:
                    this.OfficeAutomationType = typeof(O16.O16BVT_16_0_4366);
                    break;
            }
            var publicMethods = this.OfficeAutomationType.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(c => c.DeclaringType.FullName != "System.Object" && !c.Name.Contains("Edit_Location_Dlg")).ToList();
            foreach (var method in publicMethods)
            {
                result.Add(new TestCase { IsChecked = false, Name = method.Name });
            }
            return result;
        }

        private List<TestCase> GetSelectedTestCases()
        {
            var testCases = this.radlistbox_testCases.ItemsSource as List<TestCase>;
            var selectedTestCases = new List<TestCase>();
            foreach (TestCase lang in testCases)
            {
                if (lang.IsChecked == true)
                {
                    selectedTestCases.Add(lang);
                }
            }
            return selectedTestCases;
        }

        #endregion TestCases

        #region UI-Events

        private void textbox_BuildVersion_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Version buildVersion = null;
            string version = this.textbox_BuildVersion.Text;
            if (Version.TryParse(version, out buildVersion))
            {
                OfficeType? officeType = null;
                switch (buildVersion.Major)
                {
                    case 15:
                        officeType = OfficeType.O15;
                        break;

                    case 16:
                        officeType = OfficeType.O16;
                        break;

                    default:
                        break;
                }

                if (officeType != null)
                {
                    //1 load all the languages available
                    this.LoadLanguages(version, officeType.Value);

                    this.combobox_BuildVersion.ItemsSource = Enum.GetValues(typeof(OfficeType));

                    //2 load all the test cases from the file
                    this.radlistbox_testCases.ItemsSource = this.LoadTtestCases(officeType.Value);

                    //3 Save build version in config
                    var config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    config.AppSettings.Settings["BuildVersion"].Value = version;
                    config.Save();

                    //4 Set Result folder path path
                    this.ResultFolderPath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, string.Format(@"logs\{0}", Enum.GetName(typeof(OfficeType), officeType)), DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
                }
            }
        }

        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                History obj = ((FrameworkElement)sender).DataContext as History;
                MessageBox.Show(obj.Msg);
            }
        }

        private void radbutton_start_Click(object sender, RoutedEventArgs e)
        {
            var version = textbox_BuildVersion.Text;
            var officeType = (OfficeType)this.combobox_BuildVersion.SelectedValue;
            var task = Task.Factory.StartNew(() =>
            {
                UnFreezeUI(false);

                try
                {
                    //1 - check if build version if valid
                    Version buildVersion = null;
                    string msg = null;
                    if (Version.TryParse(version, out buildVersion))
                    {
                        DateTime startDate = DateTime.Now;
                        string grp = version + " " + startDate.ToString("(MMMM, dd yyyy HH:mm:ss)");

                        var selectedTestCases = this.GetSelectedTestCases();
                        var selectedLanguages = this.GetSelectedLanguages();
                        if (selectedLanguages.Any() && selectedTestCases.Any())
                        {
                            List<History> batchHistory = new List<History>();
                            //run all the tests for each languages
                            foreach (Language lang in selectedLanguages)
                            {
                                //****
                                //switch to current language
                                //****

                                foreach (TestCase testCase in selectedTestCases)
                                {
                                    //****
                                    //switch to current test case
                                    //****
                                    this.grid_History.Dispatcher.Invoke(new Action(() =>
                                    {
                                        UpdateUIAddHistory(batchHistory, grp, lang, testCase);
                                    }));
                                }
                            }
                            var historyByLanguage = batchHistory.GroupBy(g => g.Language);
                            var testResults = new List<TestResult>();

                            foreach (var grpp in historyByLanguage)
                            {
                                string langResultPath = Path.Combine(this.ResultFolderPath, grpp.Key.CultureName);
                                var SfbInstance = Activator.CreateInstance(this.OfficeAutomationType, new object[] { langResultPath });
                                try
                                {
                                    var t = grpp.FirstOrDefault();

                                    UpdateUILog("--------  Switch Language  ------------\n" + t.Language.CultureName + "\n---------------------------\n");
                                    string langInstallFile = Path.Combine(t.Language.LanguagePackInfo.LanguagePackFolderPath, t.Language.LanguagePackInfo.LanguagePackInstallationFileName);
                                    SfbUtils.SwitchLanguage(t.Language.CultureName, t.Language.LocCulture.Lcid, officeType, langInstallFile);

                                    foreach (var tt in grpp)
                                    {
                                        UpdateUILog("--------  Run   ------------\n" + tt.Testcase.Name + "\n---------------------------\n");

                                        try
                                        {
                                            this.OfficeAutomationType.InvokeMember(tt.Testcase.Name, BindingFlags.InvokeMethod, Type.DefaultBinder, SfbInstance, null);
                                            msg = "Done";
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = ex.ToString();
                                            var errorMsg = ex.Message;
                                            if (ex.InnerException != null)
                                                errorMsg += Environment.NewLine + ex.InnerException.Message;
                                            UpdateUILog("--------  ERROR   ------------\n" + errorMsg + "\n---------------------------\n");
                                        }

                                        this.grid_History.Dispatcher.Invoke(new Action(() =>
                                        {
                                            UpdateUIForHistory(tt, msg);
                                        }));

                                        string status = "n/a";
                                        if (tt.Status == "F")
                                            status = "Failed";
                                        else if (tt.Status == "S")
                                            status = "Passed";

                                        var res = new TestResult
                                        {
                                            Language = grpp.Key.CultureName,
                                            Result = status,
                                            TestCaseName = tt.Testcase.Name
                                        };

                                        testResults.Add(res);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg = ex.Message;
                                    if (ex.InnerException != null)
                                        msg += Environment.NewLine + ex.InnerException.Message;

                                    UpdateUILog("--------  ERROR   ------------\n" + msg + "\n---------------------------\n");
                                }
                            }

                            DateTime endDate = DateTime.Now;
                            string excelfileName = string.Format("TestResults_{0}.xlsx", endDate.ToString("yyyy-MM-dd_HH-mm-ss"));
                            string excelPath = Path.Combine(Logger.LogDirectory, excelfileName);
                            Utils.SaveToExcel(version, startDate, endDate, testResults, excelPath);

                            StreamReader sr = new StreamReader("email_template.html");
                            String emailcontent = sr.ReadToEnd();
                            var emailSetting = EmailService.EmailService.Deserialize<Setting>("emailConfig.xml");

                            FileStream fs = new FileStream(excelPath, FileMode.Open);
                            Attachment[] atts = new Attachment[1];
                            atts[0] = new Attachment(fs, excelfileName);
                            foreach (string emailTo in emailSetting.EmailToList.EmailTo)
                            {
                                EmailService.EmailService.SendEmail(emailSetting.EmailServer, emailSetting.EmailFrom,
                                    emailTo,
                                    emailSetting.EmailTitle,
                                    emailcontent,
                                   atts);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select at least one language and test case");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please put a valid build version (i.e: \"15.0.4823.1000\")");
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    UnFreezeUI(true);
                }
            });
        }

        #endregion UI-Events

        private void UpdateUILog(string msg)
        {
            this.log.Dispatcher.Invoke(new Action(() =>
            {
                Logger.LogMessage(msg);
                this.log.Text += Environment.NewLine + msg;
                this.log.ScrollToEnd();
            }));
        }

        private void UpdateUIForHistory(History currentHistory, string msg)
        {
            Logger.LogMessage(msg);
            var itemSource = this.grid_History.ItemsSource as ObservableCollection<History>;
            History history = itemSource.Where(hh => hh.guid.Equals(currentHistory.guid)).FirstOrDefault();
            history.runTime = DateTime.Now;
            history.Msg += msg;
            if (msg.Contains("Exception") ||
                msg.Contains("Exceptions") ||
                msg.Contains("Error") ||
                msg.Contains("Errors"))
                history.Status = "F";
            else
                history.Status = "S";
            SaveLogToFile();

            this.log.Text += Environment.NewLine + msg;
            IGroup group = (IGroup)this.grid_History.Items.Groups.LastOrDefault();
            this.grid_History.ExpandGroup(group);
        }

        private void UpdateUIAddHistory(List<History> batchHistory,
            string grp,
            Language lang,
            TestCase testCase)
        {
            History history = new History();
            history.grp = grp;
            history.Language = lang;
            history.Testcase = testCase;
            history.runTime = DateTime.Now;
            history.Status = "P";
            history.guid = Guid.NewGuid().ToString();
            batchHistory.Add(history);

            var itemSource = this.grid_History.ItemsSource as ObservableCollection<History>;
            itemSource.Add(history);

            IGroup group = (IGroup)this.grid_History.Items.Groups.LastOrDefault();
            this.grid_History.ExpandGroup(group);
            SaveLogToFile();
        }

        //Logger
        private void UnFreezeUI(bool flag)
        {
            this.busyIndicator.Dispatcher.Invoke(new Action(() =>
            {
                this.busyIndicator.IsBusy = !flag;
            }));
        }

        private void SendEmail(string excelPath, string excelfileName)
        {
            StreamReader sr = new StreamReader("email_template.html");
            String emailcontent = sr.ReadToEnd();
            var emailSetting = EmailService.EmailService.Deserialize<Setting>("emailConfig.xml");

            FileStream fs = new FileStream(excelPath, FileMode.Open);
            Attachment[] atts = new Attachment[1];
            atts[0] = new Attachment(fs, excelfileName);
            foreach (string emailTo in emailSetting.EmailToList.EmailTo)
            {
                EmailService.EmailService.SendEmail(emailSetting.EmailServer, emailSetting.EmailFrom,
                    emailTo,
                    emailSetting.EmailTitle,
                    emailcontent,
                   atts);
            }
        }

        private void SaveLogToFile()
        {
            // Serialize the list to a file
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenWrite("log.dat"))
            {
                serializer.Serialize(stream, this.Histories.Info);
            }
        }

        private void LoadLogFromFile()
        {
            ObservableCollection<History> lp = new ObservableCollection<History>();

            if (File.Exists("log.dat"))
            {
                // Deserialize the list from a file
                var serializer = new BinaryFormatter();
                using (var stream = File.OpenRead("log.dat"))
                {
                    if (stream != null && stream.Length != 0)
                    {
                        lp = (ObservableCollection<History>)serializer.Deserialize(stream);
                    }
                }
            }
            this.Histories.Info = lp;
            this.grid_History.ItemsSource = lp;
        }
    }
}
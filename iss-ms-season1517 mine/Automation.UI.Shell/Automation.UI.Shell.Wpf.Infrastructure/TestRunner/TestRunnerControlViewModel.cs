using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.LinkDialog;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Automation.UI.Shell.Core;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public class TestRunnerControlViewModel : ShellViewModelBase, ITestRunnerViewModel
    {
        private ObservableCollection<Language> languagesList;
        private ObservableCollection<TestCase> testCasesList;
        private ObservableCollection<History> historyList;
        private string logContent;
        private string applicationName;
        private bool isBusy;
        private bool canSendEmail;
        private bool canDelay;
        private string delayTime;
        private string delayCountDown;
        private bool isCheckedAllLanguage;
        private bool isCheckedAllTestCases;

        private readonly StringBuilder logBuilder;
        protected readonly ILogStatusHistory statusHistoryService;
        protected readonly IExpandStatusGroupView expandStatusService;
        protected readonly ILanguagesProvider languagesProvider;
        protected readonly ITestCasesProvider testCasesProvider;
        protected readonly ISendEmail emailService;
        protected readonly ISaveToExcel saveToExcelService;
        protected readonly ITestCaseImageStore testCaseImageStore;

        public event Action<Language> onLanguageSwitching;

        public event Func<string> onBegin;

        public event Func<string> onFinish;

        public TestRunnerControlViewModel(
            IDispatcher dispatcher,
            ILanguagesProvider languagesProvider,
            ITestCasesProvider testCasesProvider,
            ILogStatusHistory statusHistoryService,
            IExpandStatusGroupView expandStatusService,
            ISendEmail emailService,
            ISaveToExcel saveToExcelService,
            ITestRunnerView view,
            ITestCaseImageStore testCaseImageStore
            )
            : base(dispatcher)
        {
            this.DelayTime = "0";
            this.CanSendEmail = true;
            this.CanDelay = true;
            this.StartCommand = new DelegateCommand(OnStart);
            this.LinkPopUpCommand = new DelegateCommand(OnLinkPopUp);
            this.CheckAllLanguageCommand = new DelegateCommand(onCheckAllLanguage);
            this.CheckAllTestCaseCommand = new DelegateCommand(onCheckAllTestCase);
            this.logBuilder = new StringBuilder();

            this.languagesProvider = languagesProvider;
            this.testCasesProvider = testCasesProvider;
            this.expandStatusService = expandStatusService;
            this.statusHistoryService = statusHistoryService;
            this.emailService = emailService;
            this.saveToExcelService = saveToExcelService;

            this.HistoryList = this.statusHistoryService.GetStatusHistory();

            this.LanguagesList = this.languagesProvider.GetLanguagesList();
            this.TestCasesList = this.testCasesProvider.GetTestCasesList();

            view.DataContext = this;

            this.testCaseImageStore = testCaseImageStore;
        }

        #region Properties

        public ObservableCollection<Language> LanguagesList
        {
            get { return languagesList; }
            set
            {
                languagesList = value;
                OnPropertyChanged("LanguagesList");
            }
        }

        public ObservableCollection<TestCase> TestCasesList
        {
            get { return testCasesList; }
            set
            {
                testCasesList = value;
                OnPropertyChanged("TestCasesList");
            }
        }

        public string LogContent
        {
            get { return logContent; }
            set
            {
                logContent = value;
                OnPropertyChanged("LogContent");
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ObservableCollection<History> HistoryList
        {
            get { return historyList; }
            set
            {
                historyList = value;
                OnPropertyChanged("HistoryList");
            }
        }

        public bool CanSendEmail
        {
            get { return canSendEmail; }
            set
            {
                canSendEmail = value;
                OnPropertyChanged("CanSendEmail");
            }
        }

        public bool CanDelay
        {
            get { return canDelay; }
            set
            {
                canDelay = value;
                OnPropertyChanged("CanDelay");
            }
        }

        public string DelayTime
        {
            get { return delayTime; }
            set
            {
                delayTime = value;
                OnPropertyChanged("DelayTime");
            }
        }

        public string DelayCountDown
        {
            get { return delayCountDown; }
            set
            {
                delayCountDown = value;
                OnPropertyChanged("DelayCountDown");
            }
        }

        public bool IsCheckedAllLanguage
        {
            get { return isCheckedAllLanguage; }
            set
            {
                isCheckedAllLanguage = value;
                OnPropertyChanged("IsCheckedAllLanguage");
            }
        }

        public bool IsCheckedAllTestCases
        {
            get { return isCheckedAllTestCases; }
            set
            {
                isCheckedAllTestCases = value;
                OnPropertyChanged("IsCheckedAllTestCases");
            }
        }

        #endregion Properties

        #region Commands

        public ICommand StartCommand { get; set; }

        public ICommand LinkPopUpCommand { get; set; }
        public ICommand CheckAllLanguageCommand { get; set; }

        public ICommand CheckAllTestCaseCommand { get; set; }

        #endregion Commands

        private void LogMessage(string msg)
        {
            this.logBuilder.AppendLine();
            this.logBuilder.AppendLine(msg);
            this.LogContent = this.logBuilder.ToString();
        }

        protected virtual void onCheckAllTestCase(object param)
        {
            if (this.IsCheckedAllTestCases)
            {
                foreach (var item in this.TestCasesList)
                {
                    item.IsChecked = true;
                }
            }
            else
            {
                foreach (var item in this.TestCasesList)
                {
                    item.IsChecked = false;
                }
            }
        }

        protected virtual void onCheckAllLanguage(object param)
        {
            if (this.IsCheckedAllLanguage)
            {
                foreach (var item in this.LanguagesList)
                {
                    item.IsChecked = true;
                }
            }
            else
            {
                foreach (var item in this.LanguagesList)
                {
                    item.IsChecked = false;
                }
            }
        }

        protected virtual void OnStart(object param)
        {
            this.IsBusy = true;

            var taskRunner = Task.Factory.StartNew(() =>
            {
                if (CanDelay)
                {
                    int delayCount;
                    var IsDelayTimeValid = int.TryParse(this.DelayTime, out delayCount);
                    if (!IsDelayTimeValid)
                    {
                        MessageBox.Show("The Hours is not valid!");
                        return;
                    }

                    if (delayCount != 0)
                    {
                        var watch = new Stopwatch();
                        var span = new TimeSpan(0, delayCount, 0, 0);
                        watch.Start();

                        while (true)
                        {
                            if (watch.Elapsed.Hours < delayCount)
                            {
                                dispatcher.Invoke(() =>
                              {
                                  var remaining = span.Subtract(watch.Elapsed);

                                  this.DelayCountDown = remaining.ToString(@"hh\:mm\:ss");
                              });
                                Thread.Sleep(1000);
                            }
                            else
                                break;
                        }

                        watch.Stop();
                    }
                }

                var historyGroup = new List<History>();
                DateTime startDate = DateTime.Now;
                string version = "TBD";
                string grp = version + " " + startDate.ToString("(MMMM, dd yyyy HH:mm:ss)");
                if (onBegin != null)
                {
                    string msg = "";
                    try
                    {
                        msg = onBegin();
                    }
                    catch (Exception e)
                    {
                        msg = e.ToString();
                    }
                    finally
                    {
                        this.LogMessage(msg);
                    }
                }
                if (!this.languagesList.Any(c => c.IsChecked))
                {
                    MessageBox.Show("Must choose at least one language!");
                    return;
                }
                else if (!this.TestCasesList.Any(c => c.IsChecked))
                {
                    MessageBox.Show("Must choose at least one Test Case!");
                    return;
                }
                //1 Prepare history group
                foreach (var language in this.LanguagesList.Where(c => c.IsChecked))
                {
                    foreach (var testCase in this.TestCasesList.Where(c => c.IsChecked))
                    {
                        try
                        {
                            historyGroup.Add(new History
                            {
                                Group = grp,
                                Language = language,
                                Testcase = testCase,
                                RunTime = startDate,
                                Msg = this.statusHistoryService.FormatMsg(startDate, HistoryStatus.Waiting, "Not Started"),
                                Status = HistoryStatus.Waiting,
                                Guid = Guid.NewGuid().ToString()
                            });
                        }
                        catch (Exception e)
                        {
                            this.LogMessage(e.ToString());
                        }
                    }
                }

                this.statusHistoryService.AddHistoryGroup(historyGroup);
                //use the dispatcher to refresh the UI,so the gridview can have updated info
                this.dispatcher.Invoke(() =>
                {
                    this.HistoryList = this.statusHistoryService.GetStatusHistory();
                });
                //after the gridview is refreshed, go in expandgroup
                this.dispatcher.Invoke(() =>
                {
                    this.expandStatusService.ExpandGroup(grp);
                });

                //2 Execute automation workflow
                var historyLangGroups = historyGroup.GroupBy(g => g.Language);

                this.LogMessage(string.Format("Starting automation for cultures: \"{0}\"", historyLangGroups.Select(c => c.Key.CultureName).Aggregate((a, b) => a + ", " + b)));

                //run the remote handler call inside a try catch in case it fails so that it doesn't
                //break the automation workflow
                string batchID = "";
                this.ExecuteSafely(() => batchID = RemotLogger.Client.Lib.StateLoggerLib.ClientPostLogStart(), "Warning: Failed to contact remote StateLogger server.");
                if (string.IsNullOrEmpty(batchID))
                    batchID = Guid.NewGuid().ToString();
                string appName = applicationName = testCasesProvider.GetType().Assembly.GetName().Name;
                foreach (var historyLanguageGroup in historyGroup.GroupBy(g => g.Language))
                {
                    //add a hook to allow modules to know when a language is being switched
                    if (this.onLanguageSwitching != null)
                    {
                        onLanguageSwitching(historyLanguageGroup.Key);
                        this.LogMessage(string.Format("Swtich Language done."));
                    }

                    var currentLanguage = historyLanguageGroup.First().Language;
                    this.LogMessage(string.Format("Processing culture: \"{0}\"", currentLanguage.CultureName));
                    string modelName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

                    foreach (var historyItem in historyLanguageGroup)
                    {
                        HistoryStatus outcomeStatus = HistoryStatus.Passed;
                        string outcomeMsg = "";
                        DateTime startRunDate = DateTime.Now;
                        bool isSuccess = true;
                        try
                        {
                            outcomeMsg = historyItem.Testcase.RunAction(currentLanguage);
                        }
                        catch (Exception e)
                        {
                            isSuccess = false;
                            outcomeStatus = HistoryStatus.Failed;
                            outcomeMsg = e.ToString();
                            //run the remote handler call inside a try catch in case it fails so that it doesn't
                            //break the automation workflow
                            //if the connection to exception remote handler failed, log a message
                            this.ExecuteSafely(() =>
                            {
                                RemotLogger.Client.Lib.ExceptionLoggerLib.ClientPostException(batchID, appName, e);
                            }, "Warning: Failed to contact remote ExceptionLogger server.");
                        }
                        finally
                        {

                            //Post the test Case Image from run testCaseImageStore
                            //run the remote handler call inside a try catch in case it fails so that it doesn't
                            //break the automation workflow
                            var testCaseImageItems = this.testCaseImageStore.GetAllItems();
                            List<string> itemPaths = new List<string>();
                            List<string> imageBinary = new List<string>();
                            foreach (TestCaseImageStoreItem item in testCaseImageItems.FindAll(i => (i.Path.Contains(currentLanguage.CultureName)) && (i.TestCase.Name == historyItem.Testcase.Name)))
                            {
                                itemPaths.Add(item.Path);
                                imageBinary.Add(Convert.ToBase64String(item.Image));
                            }

                            this.ExecuteSafely(() => RemotLogger.Client.Lib.StateLoggerLib.ClientPostState(batchID, appName, historyItem.Testcase.Name, currentLanguage.CultureName, isSuccess, itemPaths, imageBinary, startRunDate), "Warning: Failed to contact remote StateLogger server. Failed to upload test case image from " + (itemPaths.Count > 0 ? itemPaths[0] : "") + "!");
                            this.LogMessage(outcomeMsg);
                            this.statusHistoryService.UpdateHistory(historyItem, outcomeStatus, outcomeMsg);
                           
                        }
                    }
                }
                //run the remote handler call inside a try catch in case it fails so that it doesn't
                //break the automation workflow
                this.ExecuteSafely(() => RemotLogger.Client.Lib.StateLoggerLib.ClientPostLogEnd(batchID), "Warning: Failed to contact remote StateLogger server.");
                this.LogMessage(string.Format("Automation execution done."));

                var endDate = DateTime.Now;
                //Get Build Version
                if (onFinish != null)
                {
                    string msg = "";
                    try
                    {
                        msg = onFinish();
                    }
                    catch (Exception e)
                    {
                        msg = e.ToString();
                    }
                    finally
                    {
                        this.LogMessage(msg);
                    }
                }

                var testResultList = historyGroup.Select(c => new TestResult
                {
                    TestCaseName = c.Testcase.Name,
                    Language = c.Language.CultureName,
                    Result = c.Status.ToString()
                }).ToList();
                string excelfileName = string.Format("TestResults_{0}.xlsx", endDate.ToString("yyyy-MM-dd_HH-mm-ss"));
                string excelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, excelfileName);
                if (string.IsNullOrEmpty(BuildVersion))
                    BuildVersion = "Automation";
                this.saveToExcelService.SaveToExcel(BuildVersion, startDate, endDate, testResultList, excelPath);
                //3 Send Email
                if (this.CanSendEmail)
                {
                    if (OnSendingEmail != null)
                        OnSendingEmail(excelfileName);
                    this.LogMessage(string.Format("Sending email..."));
                    try
                    {
                        this.emailService.SendEmail();
                        this.LogMessage(string.Format("Email Sent!"));
                    }
                    catch (Exception e)
                    {
                        this.LogMessage(e.Message);
                    }
                }
            });

            taskRunner.ContinueWith(antecedent =>
            {
                this.IsBusy = false;
            });
        }

        protected virtual void OnLinkPopUp(object param)
        {
            var historyObject = ((History)(((RadButton)param).DataContext));
            string errorMsg = historyObject.Msg;
            LinkDialogForm dialog = new LinkDialogForm();
            dialog.Height = 400;
            dialog.Width = 800;
            dialog.AutoScroll = true;
            dialog.BindContent(errorMsg);
            dialog.ShowDialog();
            //MessageBox.Show(errorMsg, "Detail of the testcase run");
        }

        private void ExecuteSafely(Action action, string logMessage = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(logMessage))
                {
                    this.LogMessage(logMessage + "/r/n" + ex.Message);
                }
            }
        }

        #region IEmailService Implementation

        public event Action<string> OnSendingEmail;

        public string BuildVersion { get; set; }

        public void SendEmail()
        {
            this.emailService.SendEmail();
        }

        public string EmailSubject
        {
            get
            {
                return this.emailService.EmailSubject;
            }

            set
            {
                this.emailService.EmailSubject = value;
            }
        }

        public string EmailBody
        {
            get
            {
                return this.emailService.EmailBody;
            }

            set
            {
                this.emailService.EmailBody = value;
            }
        }

        public string[] Attachments
        {
            get
            {
                return this.emailService.Attachments;
            }

            set
            {
                this.emailService.Attachments = value;
            }
        }

        #endregion IEmailService Implementation
    }
}
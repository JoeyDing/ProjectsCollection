using Sfb.Core;
using Sfb.Installer.Core.Interfaces;
using Sfb.Installer.Core.Services;
using Sfb.LanguageInstaller.Wpf.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Telerik.Windows.Controls;

namespace Sfb.LanguageInstaller.Wpf
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string currentBuildVersion;
        private string userTypedBuildVersion;
        private string installedOfficeVersion;
        private ICanDeserialize deserializer;
        private IGetCurrentOfficeVersion getCurrentOfficeVersionService;
        private IGetInstallationInfo getInstallationInfoService;
        private ISfbOfficeInstaller installOfficeService;
        private SfbInstallationInfo sfbInstallationInfo;
        private ISfbOfficeUnInstaller removeOfficeService;
        private ISfbOfficeLanguageInstaller installLanguageService;
        private ISfbOfficeLanguageUninstaller removeLanguagesService;
        private ILoadHistory loadHistoryService;

        private string userSelectedOfficeVersion;

        private ObservableCollection<InstallerHistory> histories;
        private string logText;
        private bool isBusy;
        private bool officeInstalled;
        private bool noCheckedInstalledLanguage;
        private bool noCheckedUninstalledLanguage;
        private bool noUserTypedBuildVersion;

        #region Properties

        public List<Operation> LastRunOperations { get; set; }
        public ICanExpand ExpandService { get; set; }

        public bool OfficeInstalled
        {
            get { return officeInstalled; }
            set
            {
                officeInstalled = value;
                OnPropertyChanged("OfficeInstalled");
            }
        }

        private ObservableCollection<SfbLanguagePackInfo> languagesPackages;
        private ObservableCollection<SfbLanguagePackInfo> installedLanguagesPackages;
        private ObservableCollection<SfbLanguagePackInfo> uninstalledLanguagesPackages;

        public ObservableCollection<SfbLanguagePackInfo> LanguagesPackages
        {
            get { return languagesPackages; }
            set
            {
                languagesPackages = value;
                OnPropertyChanged("LanguagesPackages");
            }
        }

        public ObservableCollection<SfbLanguagePackInfo> InstalledLanguagesPackages
        {
            get { return installedLanguagesPackages; }
            set
            {
                installedLanguagesPackages = value;
                OnPropertyChanged("InstalledLanguagesPackages");
            }
        }

        public ObservableCollection<SfbLanguagePackInfo> UninstalledLanguagesPackages
        {
            get { return uninstalledLanguagesPackages; }
            set
            {
                uninstalledLanguagesPackages = value;
                OnPropertyChanged("UninstalledLanguagesPackages");
            }
        }

        public ObservableCollection<InstallerHistory> Histories
        {
            get { return histories; }
            set
            {
                histories = value;
                OnPropertyChanged("Histories");
            }
        }

        public string CurrentBuildVersion
        {
            get { return currentBuildVersion; }
            set
            {
                currentBuildVersion = value;
                OnPropertyChanged("CurrentBuildVersion");
            }
        }

        public string InstalledOfficeVersion
        {
            get { return installedOfficeVersion; }
            set
            {
                installedOfficeVersion = value;
                OnPropertyChanged("InstalledOfficeVersion");
            }
        }

        public string UserTypedBuildVersion
        {
            get { return userTypedBuildVersion; }
            set
            {
                userTypedBuildVersion = value;
                NoUserTypedBuildVersion = false;
                OnPropertyChanged("UserTypedBuildVersion");
                this.DisplayOfficeType();
            }
        }

        public string UserSelectedOfficeVersion
        {
            get { return userSelectedOfficeVersion; }
            set
            {
                userSelectedOfficeVersion = value;
                OnPropertyChanged("UserSelectedOfficeVersion");
            }
        }

        public bool NoCheckedInstalledLanguage
        {
            get { return noCheckedInstalledLanguage; }
            set
            {
                noCheckedInstalledLanguage = value;
                OnPropertyChanged("NoCheckedInstalledLanguage");
            }
        }

        public bool NoCheckedUninstalledLanguage
        {
            get { return noCheckedUninstalledLanguage; }
            set
            {
                noCheckedUninstalledLanguage = value;
                OnPropertyChanged("NoCheckedUninstalledLanguage");
            }
        }

        public bool NoUserTypedBuildVersion
        {
            get { return noUserTypedBuildVersion; }
            set
            {
                noUserTypedBuildVersion = value;
                OnPropertyChanged("NoUserTypedBuildVersion");
            }
        }

        public string LogText
        {
            get { return logText; }
            set
            {
                logText = value;
                OnPropertyChanged("LogText");
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

        private string RootDrive
        {
            get
            {
                return Path.GetPathRoot(System.Environment.SystemDirectory);
            }
        }

        public DelegateCommand InstallOfficeCommand { get; set; }

        public DelegateCommand RemoveOfficeCommand { get; set; }

        public DelegateCommand CheckAllLanguagesCommand { get; set; }

        public DelegateCommand RemoveLanguagesCommand { get; set; }

        public DelegateCommand InstallLanguagesCommand { get; set; }

        public DelegateCommand LinkClickCommand { get; set; }

        #endregion Properties

        public MainWindowViewModel(
            IGetCurrentOfficeVersion getCurrentOfficeVersionService,
            IGetInstallationInfo getInstallationInfoService,
            ISfbOfficeInstaller InstalleOfficeService,
            ISfbOfficeUnInstaller removeOfficeService,
            ISfbOfficeLanguageUninstaller removeLanguagesService,
            ISfbOfficeLanguageInstaller installLanguageService,
            ILoadHistory loadHistoryService
            //ICanDeserialize deserializer = null
            )
        {
            //initialization

            //get current version info
            this.getCurrentOfficeVersionService = getCurrentOfficeVersionService;
            var currentVersionInfo = getCurrentOfficeVersionService.GetCurrentOfficeVersion();
            this.getInstallationInfoService = getInstallationInfoService;
            this.sfbInstallationInfo = this.GetSfbInstallationInfo();
            this.deserializer = new DeserializeService();
            this.FillUpLanguagesLists(currentVersionInfo);
            this.installOfficeService = InstalleOfficeService;
            this.removeOfficeService = removeOfficeService;
            this.removeLanguagesService = removeLanguagesService;
            this.installLanguageService = installLanguageService;
            this.loadHistoryService = loadHistoryService;

            InstallOfficeCommand = new DelegateCommand(async (x) => await OnInstallOffice(x));
            RemoveOfficeCommand = new DelegateCommand(async (x) => await OnRemoveOffice(x));
            RemoveLanguagesCommand = new DelegateCommand(async (x) => OnRemoveLanguages(x));
            InstallLanguagesCommand = new DelegateCommand(async (x) => OnInstallLanguages(x));
            CheckAllLanguagesCommand = new DelegateCommand(OnCheckAllLanguages);
            LinkClickCommand = new DelegateCommand(OnHistoryLinkClicked);
            this.Histories = loadHistoryService.Load();
        }

        #region install office

        private void DisplayOfficeType()
        {
            SfbInstallationInfo sfbInstallationInfo = this.GetSfbInstallationInfo(this.UserTypedBuildVersion);
            this.sfbInstallationInfo = sfbInstallationInfo;
            this.UserSelectedOfficeVersion = sfbInstallationInfo.OfficeType.ToString();
            this.LanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>(sfbInstallationInfo.LanguagePackInfos.Where(l => l.Language.CultureName.ToLower() != "en-us"));
        }

        public async Task OnInstallOffice(object obj)
        {
            if (this.UserTypedBuildVersion == null)
            {
                this.NoUserTypedBuildVersion = true;
            }
            else
            {
                List<Operation> operations = new List<Operation>();
                //operataion option 1: install office
                var opInstallOffice = new Operation()
                {
                    OperationName = "Installing office",
                    BuildVersion = this.UserTypedBuildVersion,
                    Version = this.sfbInstallationInfo.OfficeType.ToString(),
                };

                opInstallOffice.Action += (language) =>
                {
                    this.installOfficeService.InstallOffice(sfbInstallationInfo.SfbInstallationFolderPath + @"\" + sfbInstallationInfo.SfbInstallationFileName);
                    this.LogText += "your installed office path is : " + sfbInstallationInfo.SfbInstallationFolderPath + @"\" + sfbInstallationInfo.SfbInstallationFileName;
                    return string.Format("{0} is done.", opInstallOffice.OperationName);
                };

                //operataion option 2: remove office
                var opRemoveOffice = new Operation()
                {
                    OperationName = "Removing Office",
                    BuildVersion = this.CurrentBuildVersion,
                    Version = GetSfbInstallationInfo().OfficeType.ToString(),
                };

                opRemoveOffice.Action += (language) =>
                {
                    this.removeOfficeService.UninstallOffice(this.GetSfbInstallationInfo().SfbInstallationFolderPath + @"\" + this.GetSfbInstallationInfo().SfbInstallationFileName);
                    return string.Format("{0} for {1} done.", opRemoveOffice.OperationName, language);
                };

                //operataion option 3: install languages
                List<Operation> installedLanguagesList = new List<Operation>();
                foreach (SfbLanguagePackInfo sfbLanguagePackInfo in this.LanguagesPackages.Where(l => l.IsChecked == true))
                {
                    var opInstallLanguage = new Operation()
                    {
                        Language = sfbLanguagePackInfo.Language.CultureName,
                        OperationName = "Installing language package",
                        BuildVersion = this.UserTypedBuildVersion,
                        Version = this.sfbInstallationInfo.OfficeType.ToString(),
                    };
                    opInstallLanguage.Action += (language) =>
                    {
                        this.installLanguageService.InstallOfficeLanguage(sfbLanguagePackInfo.Language, sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName);
                        this.LogText += "your installed language path is : " + sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName;
                        return string.Format("{0} for {1} done.", opInstallLanguage.OperationName, language);
                    };
                    installedLanguagesList.Add(opInstallLanguage);
                }

                //operataion option 4: remove languages
                List<Operation> removedLanguagesList = new List<Operation>();
                if (InstalledLanguagesPackages != null)
                {
                    foreach (SfbLanguagePackInfo sfbLanguagePackInfo in this.InstalledLanguagesPackages)
                    {
                        var opRemoveLanguages = new Operation();
                        opRemoveLanguages.Language = sfbLanguagePackInfo.Language.CultureName;
                        opRemoveLanguages.OperationName = "Removing language package";
                        opRemoveLanguages.BuildVersion = this.CurrentBuildVersion;
                        opRemoveLanguages.Version = (sfbLanguagePackInfo.LanguagePackFolderPath.Contains("office15") ? "O15" : "O16");

                        opRemoveLanguages.Action += (language) =>
                        {
                            this.removeLanguagesService.UninstallOfficeLanguage(sfbLanguagePackInfo.Language, sfbLanguagePackInfo);
                            return string.Format("{0} is done.", opRemoveLanguages.OperationName);
                        };
                        removedLanguagesList.Add(opRemoveLanguages);
                    }
                }

                if (this.CurrentBuildVersion == null || this.CurrentBuildVersion.Contains("No"))
                {
                    //check if single language page's installed
                    if (this.InstalledLanguagesPackages == null || this.InstalledLanguagesPackages.Count == 0)
                    {
                        operations.Add(opInstallOffice);
                        //install langauges from the second tab
                        foreach (Operation op in installedLanguagesList)
                        {
                            operations.Add(op);
                        }
                    }
                    else
                    {
                        //remove installed language package
                        foreach (Operation op in removedLanguagesList)
                        {
                            operations.Add(op);
                        }

                        //install office
                        operations.Add(opInstallOffice);
                        //install langauges
                        foreach (Operation op in installedLanguagesList)
                        {
                            operations.Add(op);
                        }
                    }
                    await StartRunningOperations(operations);
                }
                else
                {
                    //if the user typed version same as the installed office version,show the messsage
                    if (this.UserTypedBuildVersion == this.CurrentBuildVersion)
                    {
                        this.NoUserTypedBuildVersion = true;
                    }
                    else
                    {
                        //remove office
                        operations.Add(opRemoveOffice);
                        //remove languages
                        foreach (Operation op in removedLanguagesList)
                        {
                            operations.Add(op);
                        }
                        //install new version
                        operations.Add(opInstallOffice);
                        //install langauges
                        foreach (Operation op in installedLanguagesList)
                        {
                            operations.Add(op);
                        }
                        await StartRunningOperations(operations);
                    }
                }
            }
        }

        #endregion install office

        #region remove office

        public async Task OnRemoveOffice(object obj)
        {
            List<Operation> operations = new List<Operation>();

            var op = new Operation();
            op.OperationName = "Removing Office";
            op.BuildVersion = this.CurrentBuildVersion;
            op.Version = GetSfbInstallationInfo().OfficeType.ToString();
            //op.Version = this.sfbInstallationInfo.OfficeType.ToString();
            op.Action += (language) =>
            {
                this.LogText += this.removeOfficeService.UninstallOffice(this.GetSfbInstallationInfo().SfbInstallationFolderPath + @"\" + this.GetSfbInstallationInfo().SfbInstallationFileName);
                return string.Format("{0} is done.", op.OperationName);
            };
            operations.Add(op);
            await StartRunningOperations(operations);
        }

        #endregion remove office

        private SfbInstallationInfo GetSfbInstallationInfo(string userTypedBuildVersion = null)
        {
            SfbInstallationInfo result = null;
            if (userTypedBuildVersion == null)
            {
                if (this.CurrentBuildVersion == "No office's installed now" || this.CurrentBuildVersion == null)
                    result = this.getInstallationInfoService.GetInstallationInfo("15.0.4853.1000");
                else
                    result = this.getInstallationInfoService.GetInstallationInfo(this.CurrentBuildVersion);
            }
            else
            {
                try
                {
                    result = getInstallationInfoService.GetInstallationInfo(userTypedBuildVersion);
                }
                catch (Exception)
                {
                    //if the build number is not valid, show user an error message.
                    NoUserTypedBuildVersion = true;
                }
            }
            return result;
        }

        private async Task StartRunningOperations(List<Operation> operations)
        {
            this.IsBusy = true;
            await Task.Factory.StartNew(() =>
            {
                DateTime startDate = DateTime.Now;
                string group = startDate.ToString("(MMMM, dd yyyy HH:mm:ss)");

                foreach (var operation in operations)
                {
                    try
                    {
                        this.Histories.Add(new InstallerHistory
                        {
                            Group = group,
                            Language = operation.Language,
                            Op = operation,
                            RunTime = startDate,
                            Msg = "",
                            Status = InstallerHistory.HistoryStatus.Waiting,
                        });
                    }
                    catch (Exception e)
                    {
                        this.LogText += e.ToString() + "\r\n";
                    }
                }
                loadHistoryService.Save(this.Histories);
                this.ExpandService.ExpandHistory();
                foreach (var history in this.Histories)
                {
                    try
                    {
                        if (history.Status == InstallerHistory.HistoryStatus.Waiting)
                        {
                            this.LogText += "Process already started...";
                            history.Msg = history.Op.RunAction();
                            //refresh UI/ language list
                            if (this.getCurrentOfficeVersionService != null)
                            {
                                this.FillUpLanguagesLists(getCurrentOfficeVersionService.GetCurrentOfficeVersion());
                            }
                            history.Status = InstallerHistory.HistoryStatus.Passed;
                        }
                    }
                    catch (Exception e)
                    {
                        history.Status = InstallerHistory.HistoryStatus.Failed;
                        history.Msg = e.ToString();
                    }
                    finally
                    {
                        this.LogText += history.Msg + "\r\n";
                    }
                }

                loadHistoryService.Save(this.Histories);
                this.LastRunOperations = operations;
            });
            this.LogText += "Automation execution done.";
            this.IsBusy = false;
            //try
            //{
            //    string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "emailConfig.xml");
            //    var deserializeResult = this.deserializer.Deserialize<EmailSetting>(fileName);
            //    var sendEmailService = new SendEmailService(deserializeResult.EmailServer, deserializeResult.EmailFrom, deserializeResult.EmailToList.EmailTo, deserializeResult.EmailCcList.EmailCc);
            //    sendEmailService.EmailSubject = "Install language is done!";
            //    sendEmailService.EmailBody = "Language installation is finished!";
            //    sendEmailService.SendEmail();
            //    //this.IsBusy = false;
            //}
            //catch (Exception e)
            //{
            //    this.LogText += e.ToString() + "Send email failed! Please check your email configuration and system account.";
            //}
        }

        public void OnCheckAllLanguages(object obj)
        {
            foreach (SfbLanguagePackInfo sfbLanguagePackInfo in this.InstalledLanguagesPackages)
            {
                sfbLanguagePackInfo.IsChecked = true;
            }
        }

        public async Task OnRemoveLanguages(object obj)
        {
            if (!this.installedLanguagesPackages.Any(i => i.IsChecked == true))
            {
                this.NoCheckedInstalledLanguage = true;
            }
            else
            {
                List<Operation> operations = new List<Operation>();

                var packages = this.installedLanguagesPackages.Select(x =>
                        new SfbLanguagePackInfo
                        {
                            IsChecked = x.IsChecked,
                            Language = x.Language,
                            LanguagePackFolderPath = x.LanguagePackFolderPath,
                            LanguagePackInstallationFileName = x.LanguagePackInstallationFileName
                        }
                    ).ToList();
                foreach (SfbLanguagePackInfo sfbLanguagePackInfo in packages.Where(i => i.IsChecked == true).ToList())
                {
                    var op = new Operation();
                    op.Language = sfbLanguagePackInfo.Language.CultureName;
                    op.OperationName = "Removing language package";
                    op.BuildVersion = this.CurrentBuildVersion;
                    op.Version = (sfbLanguagePackInfo.LanguagePackFolderPath.Contains("office15") ? "O15" : "O16");

                    op.Action += (language) =>
                    {
                        this.removeLanguagesService.UninstallOfficeLanguage(sfbLanguagePackInfo.Language, sfbLanguagePackInfo);
                        return string.Format("{0} is done.", op.OperationName);
                    };
                    operations.Add(op);
                }
                await StartRunningOperations(operations);
            }
        }

        private void OnHistoryLinkClicked(object obj)
        {
            string msg = obj.ToString();
            MessageBox.Show(msg);
        }

        public async Task OnInstallLanguages(object obj)
        {
            if (!this.UninstalledLanguagesPackages.Any(i => i.IsChecked == true))
            {
                this.NoCheckedInstalledLanguage = true;
            }
            else
            {
                List<Operation> operations = new List<Operation>();
                foreach (SfbLanguagePackInfo sfbLanguagePackInfo in this.uninstalledLanguagesPackages.Where(l => l.IsChecked == true))
                {
                    var op = new Operation();
                    op.Language = sfbLanguagePackInfo.Language.CultureName;
                    op.OperationName = "Installing language package";
                    op.BuildVersion = this.CurrentBuildVersion;
                    //op.Version = this.sfbInstallationInfo.OfficeType.ToString();
                    op.Version = GetSfbInstallationInfo().OfficeType.ToString();
                    op.Action += (language) =>
                    {
                        this.installLanguageService.InstallOfficeLanguage(sfbLanguagePackInfo.Language, sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName).ToString();
                        this.LogText += "your installed language path is : " + sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName;
                        return string.Format("{0} for {1} done.", op.OperationName, language);
                    };
                    operations.Add(op);
                }
                await StartRunningOperations(operations);
            }
        }

        private void FillUpLanguagesLists(CurrentOfficeInfo currentVersionInfo)
        {
            if (currentVersionInfo != null)
            {
                //enable first tab
                this.OfficeInstalled = true;
                this.CurrentBuildVersion = currentVersionInfo.BuildVersion;
                this.InstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>(this.GetSfbInstallationInfo().LanguagePackInfos.Where(u => (currentVersionInfo.InstalledLanguages.Select(i => i.ToLower())).Contains(u.Language.CultureName.ToLower()) && u.Language.CultureName.ToLower() != "en-us"));
                this.UninstalledLanguagesPackages = new ObservableCollection<SfbLanguagePackInfo>(this.GetSfbInstallationInfo().LanguagePackInfos.Where(u => !(currentVersionInfo.InstalledLanguages.Select(i => i.ToLower())).Contains(u.Language.CultureName.ToLower())));
            }
            else
            {
                //disable the first Tab
                this.OfficeInstalled = false;
                this.CurrentBuildVersion = "No office's installed now";
            }
        }
    }
}
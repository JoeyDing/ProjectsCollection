using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using Microsoft.Practices.Unity;
using Sfb.Core;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using Sfb.Installer.Core.Services;
using Sfb.UI.Shell.Module.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Telerik.Windows.Controls;

namespace Sfb.UI.Shell.Module
{
    public class SfbAutomationContentViewModel : ViewModelBase
    {
        private TestRunnerControlViewModel testRunnerViewModel;

        private List<TestCase> alltestcases = new List<TestCase>();
        private string buildVersion;
        private List<string> officeVersion;
        private string selectedOfficeVersion;
        private ISwitchLanguage switchLanguageService;
        private IGetConfigurationLanguages getConfigurationLanguagesService;
        private List<LocCulture> allLanguages;
        private TestcaseProvider testcaseProvider;

        public string BuildVersion
        {
            get { return buildVersion; }
            set
            {
                buildVersion = value;
                OnPropertyChanged("BuildVersion");
                OnBuildVersionChanged();
            }
        }

        public List<string> OfficeVersion
        {
            get { return officeVersion; }
            set
            {
                officeVersion = value;
                OnPropertyChanged("OfficeVersion");
            }
        }

        public string SelectedOfficeVersion
        {
            get { return selectedOfficeVersion; }
            set
            {
                selectedOfficeVersion = value;
                OnPropertyChanged("SelectedOfficeVersion");
                OnOfficeVersionChanged();
            }
        }

        private IUnityContainer container;

        public SfbAutomationContentViewModel(IUnityContainer _container, TestRunnerControlViewModel viewModel, TestcaseProvider testcaseProvider, ISwitchLanguage switchLanguageService, IGetConfigurationLanguages getConfigurationLanguagesService)
        {
            this.container = _container;
            viewModel.EmailSubject = "Sfb Automation Report";
            viewModel.EmailBody = "You can find test result in the attachments";
            viewModel.BuildVersion = "Sfb";
            viewModel.OnSendingEmail += ViewModel_OnSendingEmail;
            this.switchLanguageService = switchLanguageService;
            this.getConfigurationLanguagesService = getConfigurationLanguagesService;
            this.allLanguages = this.getConfigurationLanguagesService.GetLanguages();
            this.testRunnerViewModel = viewModel;
            this.testRunnerViewModel.onLanguageSwitching += TestRunnerViewModel_onLanguageSwitching;
            this.testRunnerViewModel.onBegin += TestRunnerViewModel_onBegin;

            foreach (var testcase in testRunnerViewModel.TestCasesList)
            {
                alltestcases.Add(testcase);
            }

            officeVersion = new List<string>();
            OfficeVersion.Add(TestcaseProvider.OFFICE15);
            OfficeVersion.Add(TestcaseProvider.OFFICE16);
            //BuildVersion = ConfigurationManager.AppSettings["BuildVersion"].ToString();
            GetCurrentOfficeVersionService getCurrentOfficeVersionService = new GetCurrentOfficeVersionService();
            CurrentOfficeInfo currentVersionInfo = getCurrentOfficeVersionService.GetCurrentOfficeVersion();
            BuildVersion = currentVersionInfo.BuildVersion;
            //BuildVersion = "16.0.4823.1000";
            this.testcaseProvider = testcaseProvider;
        }

        private void ViewModel_OnSendingEmail(string excelFileName)
        {
            testRunnerViewModel.Attachments = new string[] { excelFileName };
        }

        public string TestRunnerViewModel_onBegin()
        {
            if (this.SelectedOfficeVersion == TestcaseProvider.OFFICE15)
            {
                string resultFolderPath15 = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O15", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
                if (!Directory.Exists(resultFolderPath15))
                    Directory.CreateDirectory(resultFolderPath15);

                this.testcaseProvider.ResultFolderPath15 = resultFolderPath15;
            }
            else if (this.SelectedOfficeVersion == TestcaseProvider.OFFICE16)
            {
                string resultFolderPath16 = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O16", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
                if (!Directory.Exists(resultFolderPath16))
                    Directory.CreateDirectory(resultFolderPath16);
                this.testcaseProvider.ResultFolderPath16 = resultFolderPath16;
            }

            return "ResultFolderPath Reset Done!";
        }

        private void TestRunnerViewModel_onLanguageSwitching(Language obj)
        {
            var locCulture = this.allLanguages.Where(c => c.CultureName == obj.CultureName).First();

            var officeType = this.SelectedOfficeVersion == TestcaseProvider.OFFICE15 ? OfficeType.O15 : OfficeType.O16;
            this.switchLanguageService.SwitchLanguage(locCulture, officeType);
        }

        private void RebindTestcase()
        {
            var testcases = alltestcases.Where(t => t.Name.Contains(selectedOfficeVersion)).ToList();
            testRunnerViewModel.TestCasesList.Clear();
            testRunnerViewModel.TestCasesList.AddRange(testcases);
        }

        private void OnBuildVersionChanged()
        {
            Version buildInfo = null;
            string version = this.BuildVersion;
            if (Version.TryParse(version, out buildInfo))
            {
                switch (buildInfo.Major)
                {
                    case 15:
                        this.SelectedOfficeVersion = TestcaseProvider.OFFICE15;

                        break;

                    case 16:
                        this.SelectedOfficeVersion = TestcaseProvider.OFFICE16;
                        break;

                    default:
                        break;
                }
            }
        }

        private void OnOfficeVersionChanged()
        {
            RebindTestcase();
            if (this.SelectedOfficeVersion.Equals(TestcaseProvider.OFFICE15))
                RebindLanguages(this.BuildVersion, OfficeType.O15);
            else
                RebindLanguages(this.BuildVersion, OfficeType.O16);
        }

        private void RebindLanguages(string version, OfficeType officeType)
        {
            this.testRunnerViewModel.LanguagesList.Clear();

            GetInstallationInfoService getInstallationInfoService = new GetInstallationInfoService();
            GetCurrentOfficeVersionService getCurrentOfficeVersionService = new GetCurrentOfficeVersionService();
            CurrentOfficeInfo currentVersionInfo = getCurrentOfficeVersionService.GetCurrentOfficeVersion();
            List<SfbLanguagePackInfo> languagesPackInfo = new List<SfbLanguagePackInfo>();
            if (version == currentVersionInfo.BuildVersion)
            {
                SfbInstallationInfo sfBInfo = getInstallationInfoService.GetInstallationInfo(version);
                languagesPackInfo = sfBInfo.LanguagePackInfos.Where(u => (currentVersionInfo.InstalledLanguages.Select(i => i.ToLower())).Contains(u.Language.CultureName.ToLower())).ToList();
            }
            List<Language> languages = languagesPackInfo.Select(i => new Language() { CultureName = i.Language.CultureName }).ToList();
            this.testRunnerViewModel.LanguagesList.AddRange(languages);
        }
    }
}
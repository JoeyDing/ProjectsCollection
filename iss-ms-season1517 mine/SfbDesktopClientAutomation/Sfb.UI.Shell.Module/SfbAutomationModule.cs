using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using Sfb.UI.Shell.Module.Services;

namespace Sfb.UI.Shell.Module
{
    public class SfbAutomationModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;

        public SfbAutomationModule(IRegionManager regionManager, IUnityContainer container)
        {
            this.regionManager = regionManager;
            this.container = container;

            //register types

            this.container.RegisterType<SfbAutomationContentView>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<object, SfbAutomationContentView>("SfbAutomationContentView", new InjectionFactory((uc) =>
            {
                return uc.Resolve<SfbAutomationContentView>();
            }));

            this.container.RegisterType<ICloseSfbClient, CloseSfbClientService>();
            this.container.RegisterType<ISwitchLanguage, SwitchLanguageService>();
            this.container.RegisterType<IGetConfigurationLanguages, GetConfigurationLanguagesService>();

            this.container.RegisterType<SfbAutomationContentViewModel>(new ContainerControlledLifetimeManager(), new InjectionFactory(uc =>
             {
                 var view = uc.Resolve<SfbAutomationContentView>("SfbAutomationContentView");
                 IDispatcher dispatcher = uc.Resolve<IDispatcher>();
                 ILogStatusHistory logStatusHistory = uc.Resolve<ILogStatusHistory>();
                 ISendEmail sendEmail = uc.Resolve<ISendEmail>();
                 var saveToExcel = uc.Resolve<ISaveToExcel>();

                 ILanguagesProvider languagesProvider = uc.Resolve<LanguageProvider>();

                 IExpandStatusGroupView expandGridView = new GridViewStatusGroupView(view.runnerControl);
                 ISwitchLanguage switchLanguageService = uc.Resolve<ISwitchLanguage>();
                 IGetConfigurationLanguages getConfigurationLanguages = uc.Resolve<IGetConfigurationLanguages>();
                 TestcaseProvider testCaseProvider = uc.Resolve<TestcaseProvider>();
                 logStatusHistory.LogFileName = "sfb_log.dat";

                 var runnerModel = new TestRunnerControlViewModel(dispatcher,
                     languagesProvider,
                     testCaseProvider,
                     logStatusHistory,
                     expandGridView,
                     sendEmail,
                     saveToExcel,
                     view);

                 runnerModel.HistoryList = logStatusHistory.ReadLogFromFile();
                 var vm = new SfbAutomationContentViewModel(uc, runnerModel, testCaseProvider, switchLanguageService, getConfigurationLanguages);
                 view.DataContext = runnerModel;
                 view.runnerControl.AdditionalContentDataContext = vm;

                 return vm;
             }));
        }

        public void Initialize()
        {
            this.regionManager.RegisterViewWithRegion(RegionNames.TabsRegion, typeof(SfBAutomationNavigationView));
        }
    }
}
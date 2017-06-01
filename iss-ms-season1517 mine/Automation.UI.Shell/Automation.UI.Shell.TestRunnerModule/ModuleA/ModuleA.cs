using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Core;

namespace Automation.UI.Shell.TestRunnerModule.ModuleA
{
    public class ModuleA : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private ITestCaseImageStore store;

        public ModuleA(IRegionManager regionManager, IUnityContainer container, ITestCaseImageStore store)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.store = store;

            //register types

            //force instance of view to be singleton
            this.container.RegisterType<IModuleAView, ModuleAView>(new ContainerControlledLifetimeManager());

            //used for navigation, when we navigate to ModuleAView, an instance of view will be requested
            //with the view name, so when that happens we return our singleton
            this.container.RegisterType<object, ModuleAView>(ModuleAView.ViewName, new InjectionFactory((uc) =>
            {
                return uc.Resolve<IModuleAView>();
            }));

            //build viewModel
            this.container.RegisterType<IModuleAViewModel, ModuleAViewModel>(new InjectionFactory((uc) =>
             {
                 var v = uc.Resolve<IModuleAView>();
                 var dispatcher = uc.Resolve<IDispatcher>();
                 var languageProvider = uc.Resolve<DummyLanguagesProvider>();
                 var testCasesProvider = uc.Resolve<DummyTestCasesProvider>();
                 var logStatusHistory = uc.Resolve<ILogStatusHistory>();
                 var emailService = uc.Resolve<ISendEmail>();
                 var saveToExcelService = uc.Resolve<ISaveToExcel>();
                 var expandGridView = new GridViewStatusGroupView(v.RunnerControl);
                 return new ModuleAViewModel(dispatcher, languageProvider, testCasesProvider, logStatusHistory, expandGridView, emailService, saveToExcelService, v.RunnerControl, store);
             }));
        }

        public void Initialize()
        {
            //register navigation view
            //no need to register content view as it will be loaded through navigation
            this.regionManager.RegisterViewWithRegion(RegionNames.TabsRegion, typeof(ModuleANavigationView));
        }
    }
}
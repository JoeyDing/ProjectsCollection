using Automation.UI.Shell.Wpf.Infrastructure.ConfiguratonUserControl;
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

namespace Automation.UI.Shell.TestRunnerModule.ModuleB
{
    public class ModuleB : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer container;
        private ITestCaseImageStore store;

        public ModuleB(IRegionManager regionManager, IUnityContainer container, ITestCaseImageStore store)
        {
            this.regionManager = regionManager;
            this.container = container;
            this.store = store;

            //register types

            //force instance of view to be singleton
            this.container.RegisterType<ModuleBView>(new ContainerControlledLifetimeManager());

            //used for navigation, when we navigate to ModuleAView, an instance of view will be requested
            //with the view name, so when that happens we return our singleton
            this.container.RegisterType<object, ModuleBView>(ModuleBView.ViewName, new InjectionFactory((uc) =>
            {
                return uc.Resolve<ModuleBView>();
            }));

            //build viewModel
            this.container.RegisterType<ModuleBViewModel>(new InjectionFactory((uc) =>
            {
                var v = uc.Resolve<ModuleBView>();
                var dispatcher = uc.Resolve<IDispatcher>();
                var languageProvider = uc.Resolve<ModuleBLanguagesProvider>();
                var testCasesProvider = uc.Resolve<ModuleBTestCasesProvider>();
                var logStatusHistory = uc.Resolve<ILogStatusHistory>();
                var emailService = uc.Resolve<ISendEmail>();
                var expandGridView = new GridViewStatusGroupView(v.RunnerControl);
                var savetoExcelService = uc.Resolve<ISaveToExcel>();
                var runnerViewModel = new TestRunnerControlViewModel(dispatcher, languageProvider, testCasesProvider, logStatusHistory, expandGridView, emailService, savetoExcelService, v.RunnerControl, store);

                var viewModel = new ModuleBViewModel(runnerViewModel);
                v.DataContext = viewModel;

                //set additional Content Context to the parent viewModel
                v.RunnerControl.AdditionalContentDataContext = viewModel;
                return viewModel;
            }));
        }

        public void Initialize()
        {
            //register navigation view
            //no need to register content view as it will be loaded through navigation
            this.regionManager.RegisterViewWithRegion(RegionNames.TabsRegion, typeof(ModuleBNavigationView));
        }
    }
}
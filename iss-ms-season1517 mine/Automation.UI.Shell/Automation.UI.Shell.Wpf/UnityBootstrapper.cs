using Automation.UI.Shell.Wpf.Email;
using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Automation.UI.Shell.Core;

namespace Automation.UI.Shell.Wpf
{
    public class NewBootStrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
        }

        /// <summary>
        /// create aggregate catalog
        /// </summary>
        /// <returns></returns>
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new AggregateModuleCatalog();
        }

        /// <summary>
        /// add different type of catalog to discover modules in Aggregate catalog
        /// </summary>
        protected override void ConfigureModuleCatalog()
        {
            string modulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var directoryCatalog = new DirectoryModuleCatalog() { ModulePath = modulePath };
            ((AggregateModuleCatalog)ModuleCatalog).AddCatalog(directoryCatalog);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            //register infrastructure types
            this.Container.RegisterType<IDispatcher, DispatcherService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ICanDeserialize, DeserializeService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ILogStatusHistory, LogStatusHistoryService>();
            this.Container.RegisterType<ISendEmail, SendEmailService>(new InjectionFactory(uc =>
            {
                var deserializer = uc.Resolve<ICanDeserialize>();
                var emailConfig = deserializer.Deserialize<EmailSetting>(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Email\emailConfig.xml"));
                return new SendEmailService(
                    emailConfig.EmailServer,
                    emailConfig.EmailFrom,
                    emailConfig.EmailToList.EmailTo,
                    emailConfig.EmailCcList.EmailCc);
            }));
            this.Container.RegisterType<ISaveToExcel, SaveToExcelService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ITakeScreenshotFull, TakeScreenshotFullService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ITestCaseImageStore, TestCaseImageStoreService>(new ContainerControlledLifetimeManager());

        }

    }
}
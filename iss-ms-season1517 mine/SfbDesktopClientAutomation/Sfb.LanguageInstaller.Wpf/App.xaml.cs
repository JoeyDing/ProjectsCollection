using Microsoft.Practices.Unity;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using Sfb.Installer.Core;
using Sfb.Installer.Core.Interfaces;
using Sfb.Installer.Core.Services;
using Sfb.LanguageInstaller.Wpf.Interface;
using Sfb.LanguageInstaller.Wpf.Service;
using System.Windows;

namespace Sfb.LanguageInstaller.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<MainWindow>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MainWindowViewModel>();
            unityContainer.RegisterType<IGetInstallationInfo, GetInstallationInfoService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IGetCurrentOfficeVersion, GetCurrentOfficeVersionService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IRunCmdCommand, RunCmdCommandService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ICloseSfbClient, CloseSfbClientService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISfbOfficeInstaller, SfbOfficeInstallationService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISfbOfficeUnInstaller, SfbOfficeUninstallationService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISfbOfficeLanguageUninstaller, SfbOfficeLanguageUninstallationService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ISfbOfficeLanguageInstaller, SfbOfficeLanguageInstallationService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ILoadHistory, LoadHistoryService>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ICanDeserialize, DeserializeService>(new ContainerControlledLifetimeManager());
            unityContainer.Resolve<MainWindow>().Show();
        }
    }
}
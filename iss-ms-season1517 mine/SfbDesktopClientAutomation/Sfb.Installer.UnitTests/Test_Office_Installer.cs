using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using Sfb.Installer.Core;
using Sfb.Installer.Core.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Sfb.Installer.UnitTests
{
    [TestClass]
    public class Test_Office_Installer
    {
        private static IUnityContainer _container = new UnityContainer();

        [TestMethod]
        public void InstallOffice15()
        {
            //IUnityContainer container = SetUpContainer();
            //resolve
            //var SfbOfficeInstallationService = container.Resolve<SfbOfficeInstallationService>();

            //Set up
            var runCmdCommandService = new RunCmdCommandService();
            var closeSfbClientService = new CloseSfbClientService();
            //Execute
            var SfbOfficeInstallationService = new SfbOfficeInstallationService(runCmdCommandService, closeSfbClientService);
            bool result = SfbOfficeInstallationService.InstallOffice(@"P:\office15\client\en - us\15.0.4853.1000_ProfessionalPlus_volume_ship_x64_en - us\setup.com");
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InstallOffice16()
        {
            string officeInstallationFilePath = @"P:\office16\client\en-us\16.0.6925.1025_ProfessionalPlus_volume_ship_x64_en-us\setup.com / config";
            IUnityContainer container = SetUpContainer(officeInstallationFilePath);
            //resolve
            var SfbOfficeInstallationService = container.Resolve<SfbOfficeInstallationService>();

            //Set up
            //RunCmdCommandService runCmdCommandService = new RunCmdCommandService();
            //CloseSfbClientService closeSfbClientService = new CloseSfbClientService("lync");
            //string officeInstallationFilePath = @"P:\office16\client\en-us\16.0.6925.1025_ProfessionalPlus_volume_ship_x64_en-us\setup.com";
            //var SfbOfficeInstallationService = new SfbOfficeInstallationService(OfficeType.O16, officeInstallationFilePath, runCmdCommandService, closeSfbClientService);

            //Execute
            bool result = SfbOfficeInstallationService.InstallOffice(@"P:\office16\client\en-us\16.0.6925.1025_ProfessionalPlus_volume_ship_x64_en-us\setup.com");
            //assert
            Assert.IsTrue(result);
        }

        private IUnityContainer SetUpContainer(string officeInstallationFilePath)
        {
            //unity
            _container.RegisterType<IRunCmdCommand, RunCmdCommandService>();
            _container.RegisterType<ICloseSfbClient, CloseSfbClientService>(new InjectionConstructor("lync"));
            //container.RegisterType<ICloseSfbClientView, CloseSfbClientService>(new InjectionFactory((uc) =>
            //{
            //    return new CloseSfbClientService("lync");
            //}));
            _container.RegisterType<ISfbOfficeInstaller, SfbOfficeInstallationService>(new InjectionFactory((uc) =>
            {
                var runCmdCommandService = uc.Resolve<RunCmdCommandService>();
                var closeSfbClientService = uc.Resolve<CloseSfbClientService>();
                return new SfbOfficeInstallationService(runCmdCommandService, closeSfbClientService);
            }));
            return _container;
        }

        [TestMethod]
        public void UnInstallOffice15()
        {
            ////Set Up
            //var runCmdCommandService = new RunCmdCommandService();
            //var closeSfbClientService = new CloseSfbClientService();
            //var sfbOfficeUninstallationService = new SfbOfficeUninstallationService(runCmdCommandService, closeSfbClientService);
            ////Execute
            //bool result = sfbOfficeUninstallationService.UninstallOffice(@"P:\office15\client\en-us\15.0.4853.1000_ProfessionalPlus_volume_ship_x64_en-us\setup.com");
            ////Assert
            //Assert.IsTrue(result);
        }

        [TestMethod]
        public void UnInstallOffice16()
        {
            ////Set Up
            //var runCmdCommandService = new RunCmdCommandService();
            //var closeSfbClientService = new CloseSfbClientService();
            //var sfbOfficeUninstallationService = new SfbOfficeUninstallationService(runCmdCommandService, closeSfbClientService);
            ////Execute
            //bool result = sfbOfficeUninstallationService.UninstallOffice(@"P:\office16\client\en-us\16.0.6925.1025_ProfessionalPlus_volume_ship_x64_en-us\setup.com");
            ////Assert
            //Assert.IsTrue(result);
        }

        //[TestMethod]
        //public void InstallOffice15Language()
        //{
        //    //Set up
        //    var runCmdCommandService = new RunCmdCommandService();
        //    var closeSfbClientService = new CloseSfbClientService();
        //    var sfbOfficeLanguageInstallationService = new SfbOfficeLanguageInstallationService(runCmdCommandService, closeSfbClientService);
        //    //var sfbOfficeLanguageInstallationService = new SfbOfficeLanguageInstallationService("fr-fr", @"P:\office15\client\fr - fr\15.0.4853.1000_SingleLanguagePack_none_ship_x64_fr - fr\setup.com", runCmdCommandService, closeSfbClientService);
        //    //execute
        //    bool result = sfbOfficeLanguageInstallationService.InstallOfficeLanguage("fr-fr", @"P:\office15\client\fr - fr\15.0.4853.1000_SingleLanguagePack_none_ship_x64_fr - fr\setup.com");
        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void InstallOffice16Language()
        //{
        //    //Set up
        //    var runCmdCommandService = new RunCmdCommandService();
        //    var closeSfbClientService = new CloseSfbClientService();
        //    var sfbOfficeLanguageInstallationService = new SfbOfficeLanguageInstallationService(runCmdCommandService, closeSfbClientService);
        //    //execute
        //    bool result = sfbOfficeLanguageInstallationService.InstallOfficeLanguage("fr-fr", @"P:\office16\client\fr-fr\16.0.6925.1025_SingleLanguagePack_none_ship_x64_fr-fr\setup.com");
        //    //Assert
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void UnInstallOffice15Language()
        //{
        //    //Set up
        //    var runCmdCommandService = new RunCmdCommandService();
        //    var closeSfbClientService = new CloseSfbClientService();
        //    var sfbOfficeLanguageInstallationService = new SfbOfficeLanguageInstallationService(runCmdCommandService, closeSfbClientService);
        //    //execute
        //    bool result = sfbOfficeLanguageInstallationService.InstallOfficeLanguage("fr-fr", @"C:\Program Files\Common Files\Microsoft Shared\OFFICE16\Office Setup Controller\setup.exe");
        //    //Assert
        //    Assert.IsTrue(result);
        //}

        [TestMethod]
        public void GetLCIDS()
        {
            string officePath = @"C:\Program Files\Microsoft Office\Office16\lync.exe";
            int parseInt;
            var officeDir = Path.GetDirectoryName(@"C:\Program Files\Microsoft Office\Office16\lync.exe");
            DirectoryInfo dirInfo = new DirectoryInfo(officeDir);
            List<DirectoryInfo> lcidsDirecotories = dirInfo.GetDirectories().Where(subdir => int.TryParse(subdir.Name, out parseInt)).ToList();
            foreach (DirectoryInfo lcidDirectory in lcidsDirecotories)
            {
                if (lcidDirectory.GetFiles().Any(f => f.Name == "LYNC.HXS"))
                {
                    var culture = CultureInfo.GetCultureInfo(int.Parse(lcidDirectory.Name));
                    //cultuire.Name;
                }
            }
        }

        //[TestMethod]
        //public void UnInstallOfficeLipLanguage()
        //{
        //    //Set up
        //    var runCmdCommandService = new RunCmdCommandService();
        //    var closeSfbClientService = new CloseSfbClientService();
        //    var removeLipLanService = new SfbOfficeLanguageUninstallationService(runCmdCommandService, closeSfbClientService);
        //    //execute

        //    //bool result = removeLipLanService.UninstallOfficeLanguage();
        //    bool result = sfbOfficeLanguageInstallationService("fr-fr", @"C:\Program Files\Common Files\Microsoft Shared\OFFICE16\Office Setup Controller\setup.exe");
        //    //Assert
        //    Assert.IsTrue(result);
        //}
    }
}
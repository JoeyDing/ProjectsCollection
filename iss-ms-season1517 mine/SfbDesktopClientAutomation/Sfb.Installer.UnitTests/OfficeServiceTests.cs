using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfb.Core;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using Sfb.Installer.Core;
using Sfb.Installer.Core.Interfaces;
using Sfb.Installer.Core.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sfb.Installer.UnitTests
{
    [TestClass]
    public class OfficeServiceTests
    {
        [TestMethod]
        public void OfficeServiceTests_InstallOffice_ReturnsExpectedCmdPath()
        {
            //arrange
            var runCmdServiceMoq = new Mock<IRunCmdCommand>();
            var closeSfbClientMoq = new Mock<ICloseSfbClient>();
            var sfbOfficeInstallationService = new SfbOfficeInstallationService(runCmdServiceMoq.Object, closeSfbClientMoq.Object);

            var officePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\setup.com");
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config_mui.xml");

            string expectedResult = string.Format(@"{0} /config ""{1}""", officePath, xmlPath);
            string result = null;

            //act
            runCmdServiceMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            {
                result = param;
            });

            sfbOfficeInstallationService.InstallOffice(officePath);

            //assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_UninstallOffice_ReturnsExpectedCmdPath()
        {
            Mock<IRunCmdCommand> iRunCmdCommandViewMoq = new Mock<IRunCmdCommand>();
            Mock<ICloseSfbClient> iCloseSfbClientViewMoq = new Mock<ICloseSfbClient>();
            SfbOfficeUninstallationService uninstallServiceMoq = new SfbOfficeUninstallationService(iRunCmdCommandViewMoq.Object, iCloseSfbClientViewMoq.Object);
            var uninstallationOfficePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\setup.com");
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config_mui.xml");
            var expectedResult = string.Format("{0} /uninstall ProPlus /config \"{1}\"", uninstallationOfficePath, xmlPath);
            string result = null;
            iRunCmdCommandViewMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            {
                result = param;
            });

            uninstallServiceMoq.UninstallOffice(uninstallationOfficePath);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_installLanguage_ReturnsExpectedCmdPath()
        {
            Mock<IRunCmdCommand> iRunCmdCommandViewMoq = new Mock<IRunCmdCommand>();
            Mock<ICloseSfbClient> iCloseSfbClientViewMoq = new Mock<ICloseSfbClient>();
            LocCulture languageInfo = new LocCulture();
            languageInfo.CultureName = "sq-AL";
            languageInfo.IsLip = false;
            SfbOfficeLanguageInstallationService installLanguageServiceMoq = new SfbOfficeLanguageInstallationService(iRunCmdCommandViewMoq.Object, iCloseSfbClientViewMoq.Object);
            var languagePackInstallationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\setup.com");
            var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"config_mui.xml");
            var expectedResult = string.Format(@"{0} /config {1}", languagePackInstallationPath, xmlPath);
            string result = null;
            iRunCmdCommandViewMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            {
                result = param;
            });

            installLanguageServiceMoq.InstallOfficeLanguage(languageInfo, languagePackInstallationPath);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_installLanguage_IsLibTrue_ReturnsExpectedCmdPath()
        {
            Mock<IRunCmdCommand> iRunCmdCommandViewMoq = new Mock<IRunCmdCommand>();
            Mock<ICloseSfbClient> iCloseSfbClientViewMoq = new Mock<ICloseSfbClient>();
            LocCulture languageInfo = new LocCulture();
            languageInfo.CultureName = "sq-AL";
            languageInfo.IsLip = true;
            SfbOfficeLanguageInstallationService installLanguageServiceMoq = new SfbOfficeLanguageInstallationService(iRunCmdCommandViewMoq.Object, iCloseSfbClientViewMoq.Object);
            var languagePackInstallationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\lip.msi");
            var expectedResult = string.Format(@"msiexec /i {0} /qn", languagePackInstallationPath);
            string result = null;
            iRunCmdCommandViewMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            {
                result = param;
            });

            installLanguageServiceMoq.InstallOfficeLanguage(languageInfo, languagePackInstallationPath);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_UninstallLanguage_ReturnsExpectedCmdPath()
        {
            //Mock<IRunCmdCommand> iRunCmdCommandViewMoq = new Mock<IRunCmdCommand>();
            //Mock<ICloseSfbClient> iCloseSfbClientViewMoq = new Mock<ICloseSfbClient>();
            //SfbOfficeLanguageUninstallationService uninstallServiceMoq = new SfbOfficeLanguageUninstallationService(iRunCmdCommandViewMoq.Object, iCloseSfbClientViewMoq.Object);
            //LocCulture languageInfo = new LocCulture()
            //{
            //    CultureName = "de-DE",
            //    EnglishName = "German",
            //    IsLip = false,
            //    Lcid = 1005
            //};

            //var uninstallationOfficePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\setup.com");
            //var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"sfb_langUninstaller_config.xml");
            //var expectedResult = string.Format(@"""{0}"" /uninstall OMUI.{1} /config {2}", uninstallationOfficePath, languageInfo.CultureName, xmlPath);
            //string result = null;
            //iRunCmdCommandViewMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            //        {
            //            result = param;
            //        });

            //uninstallServiceMoq.UninstallOfficeLanguage(languageInfo, uninstallationOfficePath);
            //Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_UninstallLanguage_IsLibTrue_ReturnsExpectedCmdPath()
        {
            //Mock<IRunCmdCommand> iRunCmdCommandViewMoq = new Mock<IRunCmdCommand>();
            //Mock<ICloseSfbClient> iCloseSfbClientViewMoq = new Mock<ICloseSfbClient>();
            //SfbOfficeLanguageUninstallationService uninstallServiceMoq = new SfbOfficeLanguageUninstallationService(iRunCmdCommandViewMoq.Object, iCloseSfbClientViewMoq.Object);
            //LocCulture languageInfo = new LocCulture()
            //{
            //    CultureName = "sq-AL",
            //    IsLip = true,
            //};

            //var uninstallationOfficePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TestDependencies\lip.msi");
            //var expectedResult = string.Format(@"msiexec /i {0} /qn REMOVE=ALL", uninstallationOfficePath);
            //string result = null;
            //iRunCmdCommandViewMoq.Setup(x => x.RunCmdCommand(It.Is<string>((value) => true))).Callback<string>((param) =>
            //{
            //    result = param;
            //});

            //uninstallServiceMoq.UninstallOfficeLanguage(languageInfo, uninstallationOfficePath);
            //Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void OfficeServiceTests_GetCurrentOfficeVersion_ReturnsExpectedOfficeVersion()
        {
            var getCurrentOfficeVersionService = new GetCurrentOfficeVersionService();
            CurrentOfficeInfo curretnOfficeInfo = getCurrentOfficeVersionService.GetCurrentOfficeVersion();
            Assert.AreEqual("O16", curretnOfficeInfo.OfficeType.ToString());
        }

        [TestMethod]
        public void OfficeServiceTests_GetInstalltionInfo_ReturnsExpectedOfficeInfo()
        {
            string buildVersion = "15.0.0.0";
            var getInstallationInfoService = new GetInstallationInfoService();
            Assert.IsTrue(getInstallationInfoService.GetInstallationInfo(buildVersion).LanguagePackInfos.Count == 52);
            Assert.IsTrue(getInstallationInfoService.GetInstallationInfo(buildVersion).OfficeType.ToString() == "O15");
            Assert.IsTrue(getInstallationInfoService.GetInstallationInfo(buildVersion).SfbInstallationFolderPath == @"P:\office15\client\en-us\15.0.0.0_ProfessionalPlus_volume_ship_x64_en-us");
        }

        [TestMethod]
        public void OfficeServiceTests_CloseSfbClientService_ReturnsProcessWithoutLync()
        {
            var closeSfbClientService = new CloseSfbClientService();
            closeSfbClientService.CloseSfbClient();
            Assert.IsTrue(Process.GetProcessesByName("lync").FirstOrDefault() == null);
        }
    }
}
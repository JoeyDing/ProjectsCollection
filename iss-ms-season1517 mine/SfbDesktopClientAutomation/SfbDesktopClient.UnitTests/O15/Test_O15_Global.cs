using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfb.Core;
using System.IO;

namespace O15.UnitTests.O15
{
    [TestClass]
    public class Test_O15_Global
    {
        [TestMethod]
        public void System_Global_GetSystemPath()
        {
            //prepare

            //execute
            var result = Path.GetPathRoot(System.Environment.SystemDirectory);
        }

        [TestMethod]
        public void O15_Global_GetLanguages()
        {
            //prepare

            //execute
            var result = SfbUtils.GetLanguages();
        }

        [TestMethod]
        public void O15_Global_GetSfbInstallationInfo()
        {
            //prepare
            OfficeType type = OfficeType.O15;
            string buildNumber = "15.0.4823.1000";

            //execute
            var result = SfbUtils.GetSfbInstallationInfo(type, buildNumber);
        }

        [TestMethod]
        public void O15_Global_SwitchLanguage()
        {
            //prepare
            int lcid = 1036;
            string cultureId = "fr-fr";
            string buildNumber = "15.0.4823.1000";
            string languagePackPath = @"P:\office15\client\<cultureid>\<build>_SingleLanguagePack_none_ship_x64_<cultureid>";
            string fullLanguagePath = languagePackPath.Replace("<build>", buildNumber);
            fullLanguagePath = fullLanguagePath.Replace("<cultureid>", cultureId);

            //execute
            SfbUtils.SwitchLanguage(cultureId, lcid, OfficeType.O15, fullLanguagePath);
        }

        [TestMethod]
        public void O15_Global_InstallOffice()
        {
            //prepare

            string buildNumber = "15.0.4832.1000";
            string languagePackPath = @"P:\office15\client\en-us\%buildnumber%_ProfessionalPlus_volume_ship_x64_en-us\setup.com";
            string fullInstallPath = languagePackPath.Replace("%buildnumber%", buildNumber);

            //execute
            SfbUtils.InstallOffice(OfficeType.O15, fullInstallPath);
        }

        [TestMethod]
        public void O15_Global_RemoveOffice()
        {
            //prepare

            string buildNumber = "15.0.4832.1000";
            string languagePackPath = @"P:\office15\client\en-us\%buildnumber%_ProfessionalPlus_volume_ship_x64_en-us\setup.com";
            string fullInstallPath = languagePackPath.Replace("%buildnumber%", buildNumber);

            //execute
            SfbUtils.RemoveOffice(OfficeType.O15, fullInstallPath);
        }
    }
}
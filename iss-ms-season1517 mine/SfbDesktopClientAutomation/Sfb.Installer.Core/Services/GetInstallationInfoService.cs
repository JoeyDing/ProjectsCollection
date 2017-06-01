using Sfb.Core;
using Sfb.Core.Services;
using Sfb.Installer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Sfb.Installer.Core.Services
{
    public class GetInstallationInfoService : IGetInstallationInfo
    {
        private List<LocCulture> GetLanguages()
        {
            var configurationService = new GetConfigurationLanguagesService();
            var result = configurationService.GetLanguages();
            return result;
        }

        public SfbInstallationInfo GetInstallationInfo(string buildNumber)
        {
            //check if the version number is valid
            //public static bool TryParse(string input, out Version result);
            Version validVersion;
            bool isValidVersionNumber = Version.TryParse(buildNumber, out validVersion);
            SfbInstallationInfo result = null;
            if (isValidVersionNumber)
            {
                //0 - Load config info

                string locCulturesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sfb_config.xml");
                XDocument xmlFile = XDocument.Load(locCulturesPath);
                //1 - Get different paths according to build number and office type
                result = new SfbInstallationInfo();

                string officeTypeString = null;
                if (buildNumber.StartsWith("15"))
                {
                    officeTypeString = "O15";
                    result.OfficeType = OfficeType.O15;
                }
                if (buildNumber.StartsWith("16"))
                {
                    officeTypeString = "O16";
                    result.OfficeType = OfficeType.O16;
                }
                XNamespace ns = "sfb";
                var installationFile = new FileInfo(xmlFile.Root.Element(ns + officeTypeString).Element(ns + "InstallationFile").Value.Trim().Replace("%buildnumber%", buildNumber));
                var languagePackageInstallationFile = new FileInfo(xmlFile.Root.Element(ns + officeTypeString).Element(ns + "LanguagePackageInstallationFile").Value.Trim());
                var lipLanguagesInstallationFile = new FileInfo(xmlFile.Root.Element(ns + officeTypeString).Element(ns + "LipLanguagesInstallationFile").Value.Trim());

                //1.1 add Sfb client installation paths
                result.SfbInstallationFolderPath = installationFile.DirectoryName;
                result.SfbInstallationFileName = installationFile.Name;
                //1.2 add Language packs installation paths
                result.LanguagePackInfos = new List<SfbLanguagePackInfo>();

                foreach (var culture in GetLanguages())
                {
                    var packInfo = new SfbLanguagePackInfo();
                    packInfo.Language = culture;

                    if (!culture.IsLip)
                    {
                        packInfo.LanguagePackFolderPath = languagePackageInstallationFile.DirectoryName.Replace("%cultureid%", culture.CultureName).Replace("%buildnumber%", buildNumber);
                        packInfo.LanguagePackInstallationFileName = languagePackageInstallationFile.Name;
                    }
                    else
                    {
                        packInfo.LanguagePackFolderPath = lipLanguagesInstallationFile.DirectoryName.Replace("%cultureid%", culture.CultureName).Replace("%buildnumber%", buildNumber);
                        packInfo.LanguagePackInstallationFileName = lipLanguagesInstallationFile.Name;
                    }

                    result.LanguagePackInfos.Add(packInfo);
                }
            }
            return result;
        }
    }
}
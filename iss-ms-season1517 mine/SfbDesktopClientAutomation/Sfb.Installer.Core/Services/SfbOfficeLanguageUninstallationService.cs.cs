using Sfb.Core;
using Sfb.Core.Interfaces;
using Sfb.Installer.Core.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace Sfb.Installer.Core
{
    public class SfbOfficeLanguageUninstallationService : ISfbOfficeLanguageUninstaller
    {
        private IRunCmdCommand iRunCmdCommandView;
        private ICloseSfbClient iCloseSfbClientView;

        public SfbOfficeLanguageUninstallationService(IRunCmdCommand iRunCmdCommandView, ICloseSfbClient iCloseSfbClientView)
        {
            this.iRunCmdCommandView = iRunCmdCommandView;
            this.iCloseSfbClientView = iCloseSfbClientView;
        }

        private string RootDrive
        {
            get
            {
                return Path.GetPathRoot(System.Environment.SystemDirectory);
            }
        }

        public string UninstallOfficeLanguage(LocCulture languageInfo, SfbLanguagePackInfo sfbLanguagePackInfo)
        //public string UninstallOfficeLanguage(LocCulture languageInfo, string languagePackUninstallationFile)
        {
            string result = null;
            //install from path
            string languagePackUninstallationFilePath = sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName;
            if (File.Exists(languagePackUninstallationFilePath))
            {
                if (!sfbLanguagePackInfo.Language.IsLip)
                {
                    if (sfbLanguagePackInfo.LanguagePackFolderPath.Contains("office15"))
                    {
                        languagePackUninstallationFilePath = Path.Combine(this.RootDrive, @"Program Files\Common Files\Microsoft Shared\OFFICE15\Office Setup Controller\setup.exe");
                    }
                    if (sfbLanguagePackInfo.LanguagePackFolderPath.Contains("office16"))
                    {
                        languagePackUninstallationFilePath = Path.Combine(this.RootDrive, @"Program Files\Common Files\Microsoft Shared\OFFICE16\Office Setup Controller\setup.exe");
                    }
                }
                else
                {
                    languagePackUninstallationFilePath = sfbLanguagePackInfo.LanguagePackFolderPath + @"\" + sfbLanguagePackInfo.LanguagePackInstallationFileName;
                }

                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sfb_langUninstaller_config.xml");
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(configPath);
                xmlFile.Root.Attribute("Product").Value = string.Format("OMUI.{0}", languageInfo.CultureName);
                xmlFile.Save(configPath);
                //run cmd line to remove language
                string output = null;
                if (!languageInfo.IsLip)
                {
                    result = string.Format(@"""{0}"" /uninstall OMUI.{1} /config {2}", languagePackUninstallationFilePath, languageInfo.CultureName, configPath);
                    output = this.iRunCmdCommandView.RunCmdCommand(result);
                }
                else
                {
                    result = string.Format(@"msiexec /i {0} /qn REMOVE=ALL", languagePackUninstallationFilePath);
                    output = this.iRunCmdCommandView.RunCmdCommand(result);
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", languagePackUninstallationFilePath));
            }
            return result;
        }
    }
}
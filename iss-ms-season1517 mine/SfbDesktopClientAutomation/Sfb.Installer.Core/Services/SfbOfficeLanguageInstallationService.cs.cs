using Sfb.Core;
using Sfb.Core.Interfaces;
using Sfb.Installer.Core.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace Sfb.Installer.Core
{
    public class SfbOfficeLanguageInstallationService : ISfbOfficeLanguageInstaller
    {
        private IRunCmdCommand iRunCmdCommandView;
        private ICloseSfbClient iCloseSfbClientView;

        public SfbOfficeLanguageInstallationService(IRunCmdCommand iRunCmdCommandView, ICloseSfbClient iCloseSfbClientView)
        {
            this.iRunCmdCommandView = iRunCmdCommandView;
            this.iCloseSfbClientView = iCloseSfbClientView;
        }

        public bool InstallOfficeLanguage(LocCulture languageInfo, string languagePackInstallationFile)
        {
            //install from path
            if (File.Exists(languagePackInstallationFile))
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(configPath);
                xmlFile.Root.Attribute("Product").Value = string.Format("OMUI.{0}", languageInfo.CultureName);
                xmlFile.Save(configPath);
                //run cmd line to install language
                string output = null;
                if (!languageInfo.IsLip)
                {
                    output = this.iRunCmdCommandView.RunCmdCommand(string.Format(@"{0} /config ""{1}""", languagePackInstallationFile, configPath));
                }
                else
                    output = this.iRunCmdCommandView.RunCmdCommand(string.Format(@"msiexec /i {0} /qn", languagePackInstallationFile));
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", languagePackInstallationFile));
            }
            return true;
        }
    }
}
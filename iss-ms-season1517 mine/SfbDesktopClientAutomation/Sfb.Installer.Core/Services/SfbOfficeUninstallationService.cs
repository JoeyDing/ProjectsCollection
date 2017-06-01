using Sfb.Core.Interfaces;
using Sfb.Installer.Core.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace Sfb.Installer.Core
{
    public class SfbOfficeUninstallationService : ISfbOfficeUnInstaller
    {
        private IRunCmdCommand iRunCmdCommandView;
        private ICloseSfbClient iCloseSfbClientView;

        public SfbOfficeUninstallationService(IRunCmdCommand iRunCmdCommandView, ICloseSfbClient iCloseSfbClientView)
        {
            this.iRunCmdCommandView = iRunCmdCommandView;
            this.iCloseSfbClientView = iCloseSfbClientView;
        }

        public string UninstallOffice(string officeUninstallationFile)
        {
            string result = "";
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
            //install from path
            if (File.Exists(officeUninstallationFile))
            {
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(configPath);

                xmlFile.Root.Attribute("Product").Value = string.Format("{0}", "ProPlus");

                xmlFile.Save(configPath);
                //run cmd line to install language
                //string output = this.iRunCmdCommandView.RunCmdCommand(string.Format("{0} /config {1}", officeUninstallationFile, configPath));
                string output = this.iRunCmdCommandView.RunCmdCommand(string.Format("{0} /uninstall ProPlus /config \"{1}\"", officeUninstallationFile, configPath));
                this.iCloseSfbClientView.CloseSfbClient();
                result = output;
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", officeUninstallationFile));
            }
            return result;
        }
    }
}
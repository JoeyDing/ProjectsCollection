using Microsoft.Win32;
using Sfb.Core.Interfaces;
using Sfb.Installer.Core.Interfaces;
using System;
using System.IO;
using System.Xml.Linq;

namespace Sfb.Installer.Core
{
    public class SfbOfficeInstallationService : ISfbOfficeInstaller
    {
        private IRunCmdCommand iRunCmdCommandView;
        private ICloseSfbClient iCloseSfbClientView;
        public string ConfigPath { get; set; }
        public string ConfigPath_ocapi_test_source { get; set; }
        public string ConfigPath_ocapi_test_destination { get; set; }

        private string RootDrive
        {
            get
            {
                return Path.GetPathRoot(System.Environment.SystemDirectory);
            }
        }

        public SfbOfficeInstallationService(IRunCmdCommand runCmdCommandService, ICloseSfbClient closeSfbClientService)
        {
            this.iRunCmdCommandView = runCmdCommandService;
            this.iCloseSfbClientView = closeSfbClientService;
            ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
            ConfigPath_ocapi_test_source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ocapi_test_config.xml");
        }

        public bool InstallOffice(string officeInstallationFile)
        {
            //install from path
            if (File.Exists(officeInstallationFile))
            {
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(ConfigPath);

                xmlFile.Root.Attribute("Product").Value = string.Format("{0}", "ProPlus");

                xmlFile.Save(ConfigPath);
                if (officeInstallationFile.Contains("office15"))
                {
                    this.CreateNewKeyInLanguageResource();
                    ConfigPath_ocapi_test_destination = Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office15\", "ocapi_test.config.xml");
                }
                if (officeInstallationFile.Contains("office16"))
                {
                    ConfigPath_ocapi_test_destination = Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office16\", "ocapi_test.config.xml");
                }

                //run cmd line to install language
                string output = this.iRunCmdCommandView.RunCmdCommand(string.Format("{0} /config \"{1}\"", officeInstallationFile, ConfigPath));

                string configPath_ocapi_test_source = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ocapi_test_config.xml");
                XDocument xmlfile = XDocument.Load(configPath_ocapi_test_source);
                xmlfile.Save(ConfigPath_ocapi_test_destination);
                this.iCloseSfbClientView.CloseSfbClient();
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", officeInstallationFile));
            }
            return true;
        }

        private void CreateNewKeyInLanguageResource()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Common\LanguageResources");
            if (key != null)
            {
                Object o = key.GetValue("FollowSystemUI");
                if (o == null)
                {
                    RegistryKey lk = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Office\15.0\Common\LanguageResources");
                    lk.SetValue("FollowSystemUI", "Off");
                }
            }
        }
    }
}
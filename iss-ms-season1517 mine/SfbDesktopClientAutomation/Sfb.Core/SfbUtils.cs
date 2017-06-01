using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Sfb.Core
{
    public static class SfbUtils
    {
        private static string RunCmdCommand(string command)
        {
            Thread.Sleep(2000);
            string output = "";

            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd", @"/c " + command)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true
            };

            Process cmd = Process.Start(cmdsi);
            cmd.WaitForExit();
            try
            {
                while (!cmd.StandardOutput.EndOfStream)
                {
                    output += cmd.StandardOutput.ReadLine();
                    output += cmd.StandardError.ReadLine();
                }

                return output;
            }
            catch (Exception e)
            {
                return e.Message + Environment.NewLine + e.InnerException;
            }
        }

        public static List<LocCulture> GetLanguages()
        {
            XNamespace ns = "sfb";
            string locCulturesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sfb_config.xml");
            XDocument xmlFile = XDocument.Load(locCulturesPath);

            var result = xmlFile.Root.Element(ns + "LocCultures").Elements().Select(c => new LocCulture
            {
                CultureName = c.Attribute("CultureName").Value.ToString(),
                EnglishName = c.Attribute("EnglishName").Value.ToString(),
                Lcid = int.Parse(c.Attribute("LCID").Value.ToString()),
            }).ToList();

            return result;
        }

        public static void CloseSfbClient()
        {
            //process
            var sfbProcess = Process.GetProcessesByName("lync").FirstOrDefault();
            if (sfbProcess != null)
            {
                sfbProcess.Kill();
                Thread.Sleep(3000);
            }
        }

        public static bool AppIsLaunched()
        {
            //process
            var sfbProcess = Process.GetProcessesByName("lync").FirstOrDefault();

            return sfbProcess != null;
        }

        public static bool AppIsLaunchedForRecordingManager()
        {
            //process
            var sfbProcess = Process.GetProcessesByName("ocpubmgr").FirstOrDefault();

            return sfbProcess != null;
        }

        public static SfbInstallationInfo GetSfbInstallationInfo(OfficeType officeType, string buildNumber, string sfbRootClientFolderPath = null)
        {
            //0 - Load config info

            string locCulturesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sfb_config.xml");
            XDocument xmlFile = XDocument.Load(locCulturesPath);

            string officeTypeString = null;
            switch (officeType)
            {
                case OfficeType.O15:
                    officeTypeString = "O15";
                    break;

                case OfficeType.O16:
                    officeTypeString = "O16";
                    break;

                default:
                    break;
            }
            XNamespace ns = "sfb";
            var installationFile = new FileInfo(xmlFile.Root.Element(ns + officeTypeString).Element(ns + "InstallationFile").Value.Trim().Replace("%buildnumber%", buildNumber));
            var languagePackageInstallationFile = new FileInfo(xmlFile.Root.Element(ns + officeTypeString).Element(ns + "LanguagePackageInstallationFile").Value.Trim());

            //1 - Get different paths according to build number and office type
            SfbInstallationInfo result = new SfbInstallationInfo();

            //1.1 add Sfb client installation paths
            result.SfbInstallationFolderPath = installationFile.DirectoryName;
            result.SfbInstallationFileName = installationFile.Name;

            //1.2 add Language packs installation paths
            result.LanguagePackInfos = new List<SfbLanguagePackInfo>();

            foreach (var culture in GetLanguages())

            {
                var packInfo = new SfbLanguagePackInfo();
                packInfo.Language = culture;

                packInfo.LanguagePackFolderPath = languagePackageInstallationFile.DirectoryName.Replace("%cultureid%", culture.CultureName).Replace("%buildnumber%", buildNumber);

                packInfo.LanguagePackInstallationFileName = languagePackageInstallationFile.Name;
                result.LanguagePackInfos.Add(packInfo);
            }

            return result;
        }

        public static void SwitchLanguage(string cultureId, int lcid, OfficeType officeType, string languagePackInstallationFile)
        {
            //1 - Check if the given language is installed on the system
            string officeRegPath = null;
            switch (officeType)
            {
                case OfficeType.O15:
                    officeRegPath = "15.0";
                    break;

                case OfficeType.O16:
                    officeRegPath = "16.0";
                    break;

                default:
                    break;
            }
            RegistryKey languageResourcesKey = Registry.CurrentUser.OpenSubKey(string.Format(@"SOFTWARE\Microsoft\Office\{0}\Common\LanguageResources", officeRegPath), false);

            if (languageResourcesKey != null)
            {
                //2 -If the language is missing, install it
                var lcidKeyValue = languageResourcesKey.GetValue("UISnapshot") as string[];
                var installedLcids = lcidKeyValue.SelectMany(c => c.Split(new char[] { ';' })).ToList();
                if (!installedLcids.Contains(lcid.ToString()))
                {
                    //install from path
                    if (File.Exists(languagePackInstallationFile))
                    {
                        string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
                        //update config file to add the language
                        XDocument xmlFile = XDocument.Load(configPath);
                        xmlFile.Root.Attribute("Product").Value = string.Format("OMUI.{0}", cultureId);
                        xmlFile.Save(configPath);
                        //run cmd line to install language
                        string output = RunCmdCommand(string.Format(@"{0} /config {1}", languagePackInstallationFile, configPath));
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", languagePackInstallationFile));
                    }
                }

                //3 - Switch language
                // 3.1 Close SfbClient

                // 3.2 Update registry keys
                languageResourcesKey = Registry.CurrentUser.OpenSubKey(string.Format(@"SOFTWARE\Microsoft\Office\{0}\Common\LanguageResources", officeRegPath), true);
                var previousLcid = languageResourcesKey.GetValue("UILanguage").ToString();
                if (lcid.ToString() != previousLcid)
                {
                    CloseSfbClient();
                    languageResourcesKey.SetValue("UILanguage", lcid, RegistryValueKind.DWord);
                    languageResourcesKey.SetValue("HelpLanguage", lcid, RegistryValueKind.DWord);
                    languageResourcesKey.SetValue("PreviousUI", previousLcid, RegistryValueKind.DWord);
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Sorry, it seems like \"{0}\" is not installed.", Enum.GetName(typeof(OfficeType), officeType)));
            }
        }

        public static void InstallOffice(OfficeType officeType, string officeInstallationFile)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
            //install from path
            if (File.Exists(officeInstallationFile))
            {
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(configPath);

                xmlFile.Root.Attribute("Product").Value = string.Format("{0}", "ProPlus");

                xmlFile.Save(configPath);
                //run cmd line to install language
                string output = RunCmdCommand(string.Format(@"{0} /config {1}", officeInstallationFile, configPath));

                CloseSfbClient();
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", officeInstallationFile));
            }
        }

        public static void RemoveOffice(OfficeType officeType, string officeInstallationFile)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config_mui.xml");
            //install from path
            if (File.Exists(officeInstallationFile))
            {
                //update config file to add the language
                XDocument xmlFile = XDocument.Load(configPath);

                xmlFile.Root.Attribute("Product").Value = string.Format("{0}", "ProPlus");

                xmlFile.Save(configPath);
                //run cmd line to install language
                string output = RunCmdCommand(string.Format(@"{0} /uninstall ProPlus /config {1}", officeInstallationFile, configPath));

                CloseSfbClient();
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot find file \"{0}\", make sure it exists and that you have access to it.", officeInstallationFile));
            }
        }
    }
}
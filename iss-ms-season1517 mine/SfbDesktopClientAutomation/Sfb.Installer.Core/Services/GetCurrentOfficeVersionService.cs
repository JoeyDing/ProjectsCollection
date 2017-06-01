using Microsoft.Win32;
using Sfb.Core;
using Sfb.Installer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Sfb.Installer.Core.Services
{
    public class CurrentOfficeInfo
    {
        public string BuildVersion { get; set; }

        public OfficeType OfficeType { get; set; }

        public IEnumerable<string> InstalledLanguages { get; set; }
    }

    public class GetCurrentOfficeVersionService : IGetCurrentOfficeVersion
    {
        private string RootDrive
        {
            get
            {
                return Path.GetPathRoot(System.Environment.SystemDirectory);
            }
        }

        private List<string> GetInstalledLanguages(string officePath)
        {
            var result = new List<string>();
            int parseInt;
            var officeDir = Path.GetDirectoryName(officePath);
            DirectoryInfo dirInfo = new DirectoryInfo(officeDir);
            List<DirectoryInfo> lcidsDirecotories = dirInfo.GetDirectories().Where(subdir => int.TryParse(subdir.Name, out parseInt)).ToList();
            foreach (DirectoryInfo lcidDirectory in lcidsDirecotories)
            {
                //if (lcidDirectory.GetFiles().Any(f => f.Name == "LYNC.HXS"))
                if (lcidDirectory.GetFiles().Any(f => f.Name == "CollectSignatures_Init.xsn"))
                {
                    var culture = CultureInfo.GetCultureInfo(int.Parse(lcidDirectory.Name));
                    result.Add(culture.Name);
                }
            }
            return result;
        }

        public CurrentOfficeInfo GetCurrentOfficeVersion()
        {
            CurrentOfficeInfo result = new CurrentOfficeInfo();
            var o15Path = Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office15\lync.exe");
            var o16Path = Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office16\lync.exe");

            if (File.Exists(o15Path))
            {
                //result.BuildVersion = FileVersionInfo.GetVersionInfo(o15Path).ProductVersion;
                //new way to get the current installed office version
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Common\ProductVersion");
                if (key != null)
                {
                    Object o = key.GetValue("LastProduct");
                    if (o != null)
                    {
                        result.BuildVersion = o.ToString();
                    }
                }
                result.OfficeType = OfficeType.O15;
                result.InstalledLanguages = this.GetInstalledLanguages(o15Path);
            }
            else if (File.Exists(o16Path))
            {
                //result.BuildVersion = FileVersionInfo.GetVersionInfo(o16Path).ProductVersion;
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Office\16.0\Common\ProductVersion");
                if (key != null)
                {
                    Object o = key.GetValue("LastProduct");
                    if (o != null)
                    {
                        result.BuildVersion = o.ToString();
                    }
                }
                result.OfficeType = OfficeType.O16;
                result.InstalledLanguages = this.GetInstalledLanguages(o16Path);
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}
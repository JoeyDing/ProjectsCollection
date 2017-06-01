using System.Collections.Generic;

namespace Sfb.Core
{
    public class SfbInstallationInfo
    {
        public string SfbInstallationFolderPath { get; set; }

        public string SfbInstallationFileName { get; set; }

        public List<SfbLanguagePackInfo> LanguagePackInfos { get; set; }

        public OfficeType OfficeType { get; set; }
    }
}
using Sfb.Core;
using System;

namespace Sfb.UI.Wpf
{
    [Serializable]
    public class Language
    {
        public LocCulture LocCulture { get; private set; }
        public SfbLanguagePackInfo LanguagePackInfo { get; set; }

        public string CultureName
        {
            get
            {
                return LocCulture.CultureName;
            }
        }

        public Language(LocCulture culture, SfbLanguagePackInfo languagePackInfo)
        {
            this.LocCulture = culture;
            this.LanguagePackInfo = languagePackInfo;
        }

        public bool IsChecked { get; set; }
    }
}
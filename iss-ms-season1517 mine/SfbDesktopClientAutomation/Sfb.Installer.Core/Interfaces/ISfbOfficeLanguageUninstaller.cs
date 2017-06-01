using Sfb.Core;

namespace Sfb.Installer.Core.Interfaces
{
    public interface ISfbOfficeLanguageUninstaller
    {
        string UninstallOfficeLanguage(LocCulture languageInfo, SfbLanguagePackInfo sfbLanguagePackInfo);
    }
}
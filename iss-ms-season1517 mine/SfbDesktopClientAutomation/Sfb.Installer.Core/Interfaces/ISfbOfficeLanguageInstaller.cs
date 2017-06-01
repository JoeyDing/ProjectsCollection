using Sfb.Core;

namespace Sfb.Installer.Core.Interfaces
{
    public interface ISfbOfficeLanguageInstaller
    {
        bool InstallOfficeLanguage(LocCulture languageInfo, string languagePackInstallationFile);
    }
}
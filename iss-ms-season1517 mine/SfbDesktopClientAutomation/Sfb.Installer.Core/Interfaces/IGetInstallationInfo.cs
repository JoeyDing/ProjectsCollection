using Sfb.Core;

namespace Sfb.Installer.Core.Interfaces
{
    public interface IGetInstallationInfo
    {
        SfbInstallationInfo GetInstallationInfo(string buildNumber);
    }
}
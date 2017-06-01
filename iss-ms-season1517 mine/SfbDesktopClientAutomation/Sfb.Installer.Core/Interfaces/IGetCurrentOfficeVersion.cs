using Sfb.Installer.Core.Services;

namespace Sfb.Installer.Core.Interfaces
{
    public interface IGetCurrentOfficeVersion
    {
        CurrentOfficeInfo GetCurrentOfficeVersion();
    }
}
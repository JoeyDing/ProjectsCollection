using System.Collections.Generic;

namespace Sfb.Core.Interfaces
{
    public interface IGetConfigurationLanguages
    {
        List<LocCulture> GetLanguages();
    }
}
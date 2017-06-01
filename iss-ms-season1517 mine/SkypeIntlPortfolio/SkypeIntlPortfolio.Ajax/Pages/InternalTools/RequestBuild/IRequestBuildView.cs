using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.RequestBuild
{
    public interface IRequestBuildView
    {
        event Action<BuildRequest> SaveBuildRequestToDb;
    }
}
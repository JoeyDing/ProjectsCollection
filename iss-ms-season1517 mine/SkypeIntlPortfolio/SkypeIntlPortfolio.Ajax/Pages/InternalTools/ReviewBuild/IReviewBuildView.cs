using CopyCommandLine;
using ImportCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ReviewBuild
{
    public interface IReviewBuildView
    {
        event Func<string, BuildRequest> GetRequestBuildFromDb;

        event Func<BuildRequest, string, string, Tuple<CopyModel, ImportModel>> LoadCopyAndImport;

        event Action<BuildRequest, bool> UpdateRequestBuildInDb;
    }
}
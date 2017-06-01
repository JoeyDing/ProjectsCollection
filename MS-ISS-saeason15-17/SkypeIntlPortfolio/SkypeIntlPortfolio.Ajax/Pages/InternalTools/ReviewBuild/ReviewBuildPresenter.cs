using CopyCommandLine;
using ImportCommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.ReviewBuild
{
    public class ReviewBuildPresenter
    {
        private IReviewBuildView view;

        public ReviewBuildPresenter(IReviewBuildView view)
        {
            this.view = view;
            this.view.GetRequestBuildFromDb += View_GetRequestBuildFromDb;
            this.view.LoadCopyAndImport += View_LoadCopyAndImport;
            this.view.UpdateRequestBuildInDb += View_UpdateRequestBuildInDb;
        }

        private void View_UpdateRequestBuildInDb(BuildRequest request, bool ifOnlyBranchID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                context.BuildRequests.Attach(request);
                context.Entry(request).Property(c => c.BranchID).IsModified = true;
                context.Entry(request).Property(c => c.ProductName).IsModified = true;
                if (!ifOnlyBranchID)
                {
                    context.Entry(request).Property(c => c.BranchName).IsModified = true;
                    context.Entry(request).Property(c => c.SourcePath).IsModified = true;
                    context.Entry(request).Property(c => c.DestinationPath).IsModified = true;
                    context.Entry(request).Property(c => c.FromBuildVersion).IsModified = true;
                    context.Entry(request).Property(c => c.Tenant).IsModified = true;
                    context.Entry(request).Property(c => c.LyncServerComponentList).IsModified = true;
                    context.Entry(request).Property(c => c.IsComponentListAll).IsModified = true;
                    context.Entry(request).Property(c => c.BuildToKeep).IsModified = true;
                }
                context.SaveChanges();
            }
        }

        private BuildRequest View_GetRequestBuildFromDb(string requestID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                BuildRequest request = context.BuildRequests.First(c => c.RequestID == requestID);
                return request;
            }
        }

        private Tuple<CopyModel, ImportModel> View_LoadCopyAndImport(BuildRequest request, string skypeConfigPath, string lyncServerConfigPath)
        {
            bool isLyncServer = request.BuildType == "lync server";
            CopyModel copy = new CopyModel
            {
                BuildType = request.BuildType,
                FromBuildVersion = request.FromBuildVersion,
                SourcePath = request.SourcePath,
                DestinationPath = request.DestinationPath,
                LyncServerComponentList = isLyncServer ? request.LyncServerComponentList.Split(',').ToList() : new List<string>(),
                ProductName = request.ProductName,
                IsComponetListAll = isLyncServer ? request.IsComponentListAll.Value : false,
                Tenant = request.Tenant,
                NumbersOfBuildsToKeep = request.BuildToKeep.HasValue ? request.BuildToKeep.Value : 5
            };
            ImportModel import = new ImportModel
            {
                xmlPath = isLyncServer ? lyncServerConfigPath : skypeConfigPath,
                BuildType = request.BuildType,
                DestinationPath = request.DestinationPath,
                FromBuildVersion = request.FromBuildVersion,
                LyncServerComponentList = isLyncServer ? request.LyncServerComponentList.Split(',').ToList() : new List<string>(),


                ProductName = request.ProductName,
                IsComponetListAll = isLyncServer ? request.IsComponentListAll.Value : false,
                Tenant = request.Tenant,
            };
            if (request.SkypeLctFilePaths.Any())
            {
                foreach (var lct in request.SkypeLctFilePaths.Split(',').ToList())
                {
                    var newSkypelctFilePath = new SkypeLctLcgFilePaths() { SkypelctFilePaths = lct };
                    if (import.SkypelctFilePaths == null) import.SkypelctFilePaths = new List<SkypeLctLcgFilePaths>();
                    import.SkypelctFilePaths.Add(newSkypelctFilePath);
                }
            }
            CopyCommandLineUtils.GetCopyInfo(copy);
            ImportCommandLineUtils.GetImportInfo(import);

            return new Tuple<CopyModel, ImportModel>(copy, import);
        }
    }
}
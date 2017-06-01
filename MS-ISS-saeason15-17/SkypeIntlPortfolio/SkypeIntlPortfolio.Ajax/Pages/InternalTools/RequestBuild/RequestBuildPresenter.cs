using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools.RequestBuild
{
    public class RequestBuildPresenter
    {
        private IRequestBuildView view;

        public RequestBuildPresenter(IRequestBuildView view)
        {
            this.view = view;
            this.view.SaveBuildRequestToDb += View_SaveBuildRequestToDb;
        }

        private void View_SaveBuildRequestToDb(BuildRequest request)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                context.BuildRequests.Add(request);
                context.SaveChanges();
            }
        }
    }
}
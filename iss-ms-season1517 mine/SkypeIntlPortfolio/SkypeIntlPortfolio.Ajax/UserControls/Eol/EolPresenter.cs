using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public class EolPresenter
    {
        private IEolView _view;

        public event Action<List<string>> EolPresenterNavigation;

        public EolPresenter(IEolView view, Dictionary<int, string> product)
        {
            this._view = view;
            this._view.Product = product;
            this._view.Navigation += _view_EolNavigation;
            this._view.IsProductCancelled += _iBridge_IsProductCancelled;
        }

        private bool _iBridge_IsProductCancelled(int productKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                bool result;
                int statusCacelledKey = db.Status.FirstOrDefault(x => x.Status1 == "Cancelled").StatusKey;
                result = db.Products_New.Any(c => c.ProductKey == productKey && c.StatusKey == statusCacelledKey);
                return result;
            }
        }

        private void _view_EolNavigation(List<string> navInfo)
        {
            if (this.EolPresenterNavigation != null)
                this.EolPresenterNavigation(navInfo);
        }
    }
}
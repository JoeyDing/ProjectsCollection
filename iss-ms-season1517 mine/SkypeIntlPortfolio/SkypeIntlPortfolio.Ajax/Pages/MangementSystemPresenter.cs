using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public class MangementSystemPresenter
    {
        private IManagementSystemView _iBridge;

        public MangementSystemPresenter(IManagementSystemView iBridge)
        {
            this._iBridge = iBridge;
            this._iBridge.GetProductsFamilyList += _iBridge_GetProductsFamilyList;
        }

        private IEnumerable<IGrouping<string, Products_New>> _iBridge_GetProductsFamilyList(string currentTabName)
        {
            IEnumerable<IGrouping<string, Products_New>> result;
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                if (currentTabName == "productinfo")
                    result = db.Products_New.GroupBy(p => p.ProductFamily.Product_Family).ToList();
                else
                    result = db.Products_New.Where(p => p.StatusKey != db.Status.FirstOrDefault(s => s.Status1 == "Cancelled").StatusKey).GroupBy(p => p.ProductFamily.Product_Family).ToList();
            }
            return result;
        }

        private bool _iBridge_IsCancelledProduct(List<int> selectedProductKeys)
        {
            //check if all selected products are cancelled
            bool isCancelled;
            using (SkypeIntlPlanningPortfolioEntities db = new SkypeIntlPlanningPortfolioEntities())
            {
                int cancelledProductKey = db.Status.FirstOrDefault(s => s.Status1 == "Cancelled").StatusKey;
                isCancelled = db.Products_New.Where(p => selectedProductKeys.Any(s => s == p.ProductKey)).All(p => p.StatusKey == cancelledProductKey);

            }
            return isCancelled;
        }
    }
}
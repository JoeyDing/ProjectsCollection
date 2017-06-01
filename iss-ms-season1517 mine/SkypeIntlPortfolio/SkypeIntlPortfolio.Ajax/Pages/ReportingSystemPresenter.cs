using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public class ReportingSystemPresenter
    {
        private IReportingSystemView _iBridge;

        public ReportingSystemPresenter()
        {
        }

        public ReportingSystemPresenter(IReportingSystemView iBridge)
        {
            this._iBridge = iBridge;
            this._iBridge.GetProductsFamilyList += _iBridge_GetProductsFamilyList;
            this._iBridge.GetProductsWithCheckedLocations += _iBridge_GetProductsWithCheckedLocations;
            this._iBridge.GetProductInfo += _iBridge_GetProductInfo;
            this._iBridge.IsCancelledProduct += _iBridge_IsCancelledProduct;
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

        private IEnumerable<IGrouping<string, Products_New>> _iBridge_GetProductsFamilyList()
        {
            //check if all selected products are cancelled
            IEnumerable<IGrouping<string, Products_New>> result = null;
            using (SkypeIntlPlanningPortfolioEntities db = new SkypeIntlPlanningPortfolioEntities())
            {
                result = db.Products_New.Where(p => p.StatusKey != db.Status.FirstOrDefault(s => s.Status1 == "Cancelled").StatusKey).GroupBy(p => p.ProductFamily.Product_Family).ToList();
            }
            return result;
        }

        private List<ProductInfo> _iBridge_GetProductsInfo(int[] productkeysList)
        {
            List<ProductInfo> result = null;
            result = Utils.GetProductInfo(productkeysList);
            return result;
        }

        public Dictionary<int, string> _iBridge_GetProductsWithCheckedLocations(IEnumerable<string> selectedLocations)
        {
            using (SkypeIntlPlanningPortfolioEntities context = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedProducts = context.Products_New.Where(c => selectedLocations.Contains(c.Location.Location1)).ToDictionary(c => c.ProductKey, c => c.Product_Name);
                return selectedProducts;
            }
        }

        private List<ProductInfo> _iBridge_GetProductInfo(Dictionary<int, string> selectedProducts)
        {
            List<ProductInfo> productInfoList = null;
            productInfoList = Utils.GetProductInfo(selectedProducts.Keys.ToArray()).Where(c => c.Releases != null).ToList();
            return productInfoList;
        }
    }
}
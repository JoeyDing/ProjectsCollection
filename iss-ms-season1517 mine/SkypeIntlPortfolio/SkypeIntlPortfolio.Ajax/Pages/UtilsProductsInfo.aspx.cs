using SkypeIntlPortfolio.Ajax.UserControls.Service;
using SkypeIntlPortfolio.Ajax.UserControls.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class UtilsProductsInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static AutoCompleteBoxData search_existingProductNames(object context)
        {
            string searchItem = ((Dictionary<string, object>)context)["Text"].ToString();

            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            using (var portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var productsList = portfolioContext.Products_New.Where(p => p.Product_Name.Contains(searchItem)).ToList();
                foreach (var product in productsList)
                {
                    AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                    childNode.Text = product.Product_Name;
                    childNode.Value = product.ProductKey.ToString();
                    result.Add(childNode);
                }
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();
            return res;
        }

        [WebMethod]
        public static AutoCompleteBoxData search_existingPeople(object context)
        {
            string searchItem = ((Dictionary<string, object>)context)["Text"].ToString();

            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            VacationInfoService vacationInfoService = new VacationInfoService();
            List<AffectedInfo> peopleAffected = vacationInfoService.GetPeopleAffectedByPeopleName(searchItem);
            foreach (var peopleInfo in peopleAffected)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                childNode.Text = peopleInfo.PeopleName;
                childNode.Value = peopleInfo.PeopleName;
                //childNode
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();
            return res;
        }
    }
}
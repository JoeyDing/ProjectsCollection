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
    public partial class UtilsReleasesInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static AutoCompleteBoxData search_existingReleasesNames(object context)
        {
            string searchItem = ((Dictionary<string, object>)context)["Text"].ToString();

            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            using (var portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //var releaseList = context.Releases.Where(r => r.VSO_Title == typedReleaseName && r.ProductKey == currentProductId && r.Deleted == false).ToList();
                var releaseList = portfolioContext.Releases.Where(r => r.VSO_Title.Contains(searchItem) && r.Deleted == false).ToList();

                foreach (var reList in releaseList)
                {
                    AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();
                    childNode.Text = reList.VSO_Title;
                    childNode.Value = reList.VSO_ID.ToString();
                    result.Add(childNode);
                }
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData();
            res.Items = result.ToArray();
            return res;
        }
    }
}
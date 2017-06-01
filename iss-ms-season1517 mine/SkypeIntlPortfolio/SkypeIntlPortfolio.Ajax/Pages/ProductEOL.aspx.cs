using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Model.Mock;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class ProductEOL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                using (SkypeIntlPlanningPortfolioEntities productsContext = new SkypeIntlPlanningPortfolioEntities())
                {
                    //get all the products from db
                    var productsList = (from f in productsContext.Products
                                        select new { f.Product_Name, f.ProductKey }).ToList();

                    this.RadComboBox.Items.Clear();
                    foreach (var item in productsList)
                    {
                        var radItem = new RadComboBoxItem();
                        radItem.Value = item.ProductKey.ToString();
                        radItem.Text = item.Product_Name;
                        this.RadComboBox.Items.Add(radItem);
                        radItem.DataBind();
                    }
                    if (custom_eolControl.ProductInfo != null)
                    {
                        if (productsList.Any())
                        {
                            this.custom_eolControl.ProductInfo.ProductKey = productsList.First().ProductKey;
                        }
                    }
                }
            }
        }

        protected void RadComboBox_ProductsRequested(object sender, EventArgs e)
        {
        }

        protected void RadComboBoxProducts_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //this.custom_eolControl.Product.ProductKey = Convert.ToInt32(e.Value);
            //this.custom_eolControl.ProductInfo.ProductKey = 1;
            //int productKey = int.Parse(e.Value);
            //using (var context = new SkypeIntlPlanningPortfolioEntities())
            //{
            //    Product product = context.Products.FirstOrDefault(p => p.ProductKey == productKey);
            //    this.custom_eolControl.Product = product;
            //}
        }
    }
}
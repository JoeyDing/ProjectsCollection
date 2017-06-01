using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class ManagementSystem_old : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.raddgant_milestones.Provider = new GanttMilestoneProvider(null);
            if (!this.IsPostBack)
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    var products = context.Products.Select(c => new { c.Product_Name, ProductKey = c.ProductKey.ToString() }).ToList();

                    foreach (var product in products)
                    {
                        var radItem = new RadListBoxItem();
                        radItem.Attributes.Add("ProductName", product.Product_Name);
                        radItem.Attributes.Add("ProductKey", product.ProductKey.ToString());
                        radItem.Text = product.Product_Name;
                        this.radListBox_products.Items.Add(radItem);
                        radItem.DataBind();
                    }
                }
            }
        }

        protected void radListBox_products_SelectedIndexChanged(object sender, EventArgs e)
        {
            var productKeys = this.radListBox_products.SelectedItems.Select(c => int.Parse(c.Attributes["ProductKey"])).ToArray();
            this.ReloadMainPanel();

            this.raddgant_milestones.Provider = new GanttMilestoneProvider(productKeys);
            this.raddgant_milestones.DataBind();
        }

        private void ReloadMainPanel()
        {
            var products = this.radListBox_products.SelectedItems;
            if (products.Any())
            {
                this.radpanelbar_products_root.Items.Clear();

                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    foreach (RadListBoxItem product in products)
                    {
                        var productKey = int.Parse(product.Attributes["ProductKey"]);
                        string productName = product.Attributes["ProductName"];

                        //set product info
                        var productItem = new RadPanelItem();

                        productItem.Attributes["ProductKey"] = productKey.ToString();
                        productItem.Attributes["ProductName"] = productName;

                        this.radpanelbar_products_root.Items.Add(productItem);

                        var dbProduct = context.Products.First(c => c.ProductKey == productKey);

                        //set projects info
                        var radpanelbar_products_child = productItem.FindControl("radpanelbar_products_child") as RadPanelBar;
                        radpanelbar_products_child.Items[0].Expanded = true;
                        //var scheduleControl = radpanelbar_products_child.Items[0].FindControl("custom_schedulecontrol") as ScheduleControl;

                        //scheduleControl.ProductInfo = dbProduct;
                        //scheduleControl.Refresh();
                    }
                    this.radpanelbar_products_root.ExpandMode = PanelBarExpandMode.MultipleExpandedItems;
                    this.radpanelbar_products_root.DataBind();
                }
            }
            //this.panel_mainContentPanel.Update();
        }
    }
}
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using SkypeIntlPortfolio.Ajax.UserControls;
using SkypeIntlPortfolio.Ajax.UserControls.LOS;
using SkypeIntlPortfolio.Ajax.UserControls.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class ReportingSystem : System.Web.UI.Page, IReportingSystemView
    {
        public bool KeepProductsInUrl
        {
            get
            {
                if (Session.Contents["KeepProductsInUrlReporting"] == null)
                    Session.Contents["KeepProductsInUrlReporting"] = false;
                return (bool)Session.Contents["KeepProductsInUrlReporting"];
            }
            set { Session.Contents["KeepProductsInUrlReporting"] = value; }
        }

        public Dictionary<int, string> SelectedProducts
        {
            get
            {
                if (Session.Contents["SelectedProductsReporting"] == null)
                    Session.Contents["SelectedProductsReporting"] = new Dictionary<int, string>();
                return Session.Contents["SelectedProductsReporting"] as Dictionary<int, string>;
            }
            set { Session.Contents["SelectedProductsReporting"] = value; }
        }

        public Dictionary<int, string> SourcePageProducts
        {
            get
            {
                if (Session.Contents["SourcePageProducts"] == null)
                    Session.Contents["SourcePageProducts"] = new Dictionary<int, string>();
                return Session.Contents["SourcePageProducts"] as Dictionary<int, string>;
            }
            set { Session.Contents["SourcePageProducts"] = value; }
        }

        public Dictionary<string, bool> CheckedLocations
        {
            get
            {
                if (Session.Contents["CheckedLocationsReporting"] == null)
                    Session.Contents["CheckedLocationsReporting"] = new Dictionary<string, bool>();
                return Session.Contents["CheckedLocationsReporting"] as Dictionary<string, bool>;
            }
            set { Session.Contents["CheckedLocationsReporting"] = value; }
        }

        public event Func<IEnumerable<IGrouping<string, Products_New>>> GetProductsFamilyList;

        public event Func<IEnumerable<string>, Dictionary<int, string>> GetProductsWithCheckedLocations;

        public event Func<Dictionary<int, string>, List<ProductInfo>> GetProductInfo;

        public event Func<List<int>, bool> IsCancelledProduct;

        protected void Page_Load(object sender, EventArgs e)
        {
            ReportingSystemPresenter rsPresenter = new ReportingSystemPresenter(this);
            this.custom_lineOfSightControl.OnRedirecToUrlWithQueryStringClicked += RedirectToUrlWithQueryString;
            if (!this.Page.IsPostBack)
            {
                this.LoadQueryString(sender, e);
                //set the link inside master page "active"
                var navlink = Master.FindControl("li_reportingSystem") as HtmlGenericControl;
                navlink.Attributes.Clear();
                navlink.Attributes.Add("class", "active");
                IEnumerable<IGrouping<string, Products_New>> productsWithoutCancelledProducts = null;
                if (this.GetProductsFamilyList != null)
                {
                    productsWithoutCancelledProducts = this.GetProductsFamilyList();
                    foreach (var familyGroup in productsWithoutCancelledProducts)
                    {
                        RadPanelItem panelItem = new RadPanelItem();
                        panelItem.Attributes["UserTypePlusFamily"] = familyGroup.Key;
                        this.radPanelBar_product_root.Items.Add(panelItem);
                        RadPanelBar radPanelBar_product_child = panelItem.FindControl("radPanelBar_product_child") as RadPanelBar;
                        var familyItem = radPanelBar_product_child.Items[0];
                        RadListBox productListBoxControl = familyItem.FindControl("radListBoxProducts") as RadListBox;

                        var orderedFamilyGroup = familyGroup.OrderBy(f => f.Product_Name).ToList();
                        foreach (var product in orderedFamilyGroup)
                        {
                            //fill the list with items
                            RadListBoxItem radItem = new RadListBoxItem();
                            radItem.Attributes.Add("Product_Name", product.Product_Name);
                            radItem.Attributes.Add("ProductKey", product.ProductKey.ToString());
                            if (this.SelectedProducts != null)
                            {
                                if (SelectedProducts.ContainsKey(product.ProductKey))
                                {
                                    radItem.Checked = true;
                                    familyItem.Expanded = true;
                                }
                            }
                            radItem.Text = product.Product_Name;
                            productListBoxControl.Items.Add(radItem);
                            radItem.DataBind();

                            this.radPanelBar_product_root.DataBind();
                        }
                    }
                }
                this.UpdatePageControls();
            }

            this.ShowProductList();
            //loading vacation presenter
            var vacationPresenter = new VacationPresenter(this.custom_vacationDaysEntryControl);
            //loading LOS presenter
            var losPresenter = new LineOfSightPresenter(this.custom_lineOfSightControl);
        }

        protected void ShowProductList()
        {
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    this.radpane_products.Collapsed = false;
                    this.radsplitbar.Visible = true;
                    this.RadListBox_locations.Visible = true;
                    this.radbutton_deselectAllProjects.Visible = true;
                    this.label_selectLocation.Visible = true;
                    this.custom_lineOfSightControl.Visible = true;
                    this.custom_vacationDaysEntryControl.Visible = false;
                    break;

                case 1:
                    //this.radpane_products.Collapsed = true;
                    this.custom_lineOfSightControl.Visible = false;
                    this.custom_vacationDaysEntryControl.Visible = true;
                    this.radpane_products.Visible = false;
                    this.radsplitbar.Visible = false;
                    this.RadListBox_locations.Visible = false;
                    this.radbutton_deselectAllProjects.Visible = false;
                    this.label_selectLocation.Visible = false;
                    break;

                default:
                    break;
            }

            this.radpane_products.Width = Unit.Percentage(15);
            this.radpane_content.Width = Unit.Percentage(85);
        }

        private void LoadQueryString(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Tab"]))
            {
                string tabName = Request.QueryString["Tab"].ToLower();
                switch (tabName)
                {
                    case "lineofsight":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 0;
                        break;

                    case "vacationdaysentry":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 1;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //default
                this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 0;
            }

            if (this.RadTabStripOne.SelectedIndex == 0)
            {
                //load selected products based on internal selected products
                if (!string.IsNullOrEmpty(Request.QueryString["Products"]))
                {
                    int temp = 0;
                    IEnumerable<int> products = Request.QueryString["Products"].ToLower().Split(new char[] { '|' }).Where(c => int.TryParse(c, out temp)).Select(c => int.Parse(c));
                    if (this.SelectedProducts == null)
                        this.SelectedProducts = new Dictionary<int, string>();

                    this.SelectedProducts.Clear();
                    this.SourcePageProducts.Clear();
                    foreach (var productKey in products)
                    {
                        if (!this.SelectedProducts.ContainsKey(productKey))
                            this.SelectedProducts.Add(productKey, "");
                    }
                    this.SourcePageProducts = this.SelectedProducts.ToDictionary(p => p.Key, p => p.Value);
                    //if use selected products from left, now click any location(avoid : All related products'll be added to current url)
                    if (this.CheckedLocations.Any(c => c.Value == true))
                    {
                        RedirectToUrlWithQueryString(sender, e);
                    }
                }
                else
                {
                    //load selected products based on selected products from other Page
                    if (this.SourcePageProducts != null)
                    {
                        if (this.SelectedProducts == null)
                            this.SelectedProducts = new Dictionary<int, string>();

                        this.SelectedProducts.Clear();
                        foreach (var productKey in this.SourcePageProducts.Keys)
                        {
                            if (!this.SelectedProducts.ContainsKey(productKey))
                                this.SelectedProducts.Add(productKey, "");
                        }
                        if (this.SelectedProducts.Count() != 0 && !this.CheckedLocations.Any(c => c.Value == true))
                        {
                            RedirectToUrlWithQueryString(sender, e);
                        }
                    }
                }

                //load selected locations
                if (!string.IsNullOrEmpty(Request.QueryString["Locations"]))
                {
                    var locations = new HashSet<string>(Request.QueryString["Locations"].ToLower().Split(new char[] { '|' }));
                    if (locations.Any())
                    {
                        foreach (RadListBoxItem checkedLocation in this.RadListBox_locations.Items)
                        {
                            checkedLocation.Checked = locations.Contains(checkedLocation.Text.ToLower());
                        }

                        this.CheckedLocations = this.RadListBox_locations.Items.ToDictionary(c => c.Text, c => c.Checked);
                        var selectedLocations = this.CheckedLocations.Where(c => c.Value == true).Select(c => c.Key);

                        this.SelectedProducts.Clear();
                        this.SourcePageProducts.Clear();
                        if (this.GetProductsWithCheckedLocations != null)
                        {
                            this.SelectedProducts = this.GetProductsWithCheckedLocations(selectedLocations);
                        }
                    }
                }
                else
                {
                    //clean the RadListBox_locations
                    foreach (var checkedLocation in this.RadListBox_locations.CheckedItems)
                    {
                        checkedLocation.Checked = false;
                    }

                    this.CheckedLocations.Clear();
                }

                //load selected RelaseMilestone
                if (!string.IsNullOrEmpty(Request.QueryString["RelMilTest"]))
                {
                    var rm = new HashSet<string>(Request.QueryString["RelMilTest"].ToLower().Split(new char[] { '|' }));
                    if (rm.Any())
                    {
                        foreach (RadListBoxItem checkedRM in this.custom_lineOfSightControl.RadlistboxReleaseAndMilestone.Items)
                        {
                            checkedRM.Checked = rm.Contains(checkedRM.Value.ToLower());
                        }
                    }
                }
                //only for the first time, add three work item names into the URL
                if (string.IsNullOrEmpty(Request.QueryString["RelMilTest"]) && this.custom_lineOfSightControl.RadlistboxReleaseAndMilestone.CheckedItems.Count == 3)
                {
                    RedirectToUrlWithQueryString(sender, e);
                }
            }
        }

        public void RedirectToUrlWithQueryString(object sender, EventArgs e)
        {
            string urlToNavigate = "~/Pages/ReportingSystem.aspx?";

            // Get Current selected tab
            string currentTabName = "";
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    currentTabName = "LineofSight";
                    break;

                case 1:
                    currentTabName = "VacationDaysEntry";
                    break;

                default:
                    break;
            }

            urlToNavigate += "Tab=" + currentTabName;
            if (currentTabName == "LineofSight")
            {
                string url = HttpContext.Current.Request.Url.AbsoluteUri;
                if (url.Contains("Products"))
                {
                    this.KeepProductsInUrl = true;
                    //this case is : user selected products from left, then click any location
                    //we want to prevent all location-related products adding to the url
                    if (this.CheckedLocations.Any(c => c.Value == true))
                    {
                        this.KeepProductsInUrl = false;
                    }
                }
                else
                {
                    //this case is only for the senario when user switch pages
                    if (this.SourcePageProducts != null && !this.CheckedLocations.Any(c => c.Value == true))
                    {
                        this.KeepProductsInUrl = true;
                    }
                }
                if (this.KeepProductsInUrl == true)
                {
                    //get currrent selected products
                    if (this.SelectedProducts.Any() && !url.Contains("Locations"))
                    {
                        string currentSelectedProducts = this.SelectedProducts.Select(p => p.Key.ToString()).Aggregate((a, b) => a + "|" + b);
                        urlToNavigate += "&Products=" + currentSelectedProducts;
                    }
                }
                else
                {
                    // get current selected locations
                    if (this.CheckedLocations.Any(c => c.Value == true))
                    {
                        string currentSelectedLocations = this.CheckedLocations.Where(c => c.Value == true).Select(c => c.Key).Aggregate((a, b) => a + "|" + b);
                        urlToNavigate += "&Locations=" + currentSelectedLocations.ToLower();
                    }
                }

                //if (currentTabName == "LineofSight" || currentTabName == "VacationDaysEntry")
                if (currentTabName == "LineofSight")
                {
                    string currentSelectedRM = "";
                    if (this.custom_lineOfSightControl.CheckedReleaseMilestone.Any())
                    {
                        currentSelectedRM = this.custom_lineOfSightControl.CheckedReleaseMilestone.Where(c => c.Value == true).Select(c => c.Key).Aggregate((a, b) => a + "|" + b);
                    }

                    if (this.CheckedLocations.Any(c => c.Value == true))
                    {
                        string newSelection = "release";

                        if (currentSelectedRM.Contains("|testPlan"))
                            newSelection += "|testPlan";

                        if (currentSelectedRM.Contains("|milestone"))
                            newSelection += "|milestone";
                        if (currentSelectedRM.Contains("|vacation"))
                            newSelection += "|vacation";
                        currentSelectedRM = newSelection;
                    }

                    if (currentSelectedRM != "")
                    {
                        urlToNavigate += "&RelMilTest=" + currentSelectedRM;
                    }
                }
            }
            Response.Redirect(urlToNavigate, false);
        }

        private void UpdatePageControls()
        {
            //if(this.GetProductsFamilyList(SelectedProducts))
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    if (SelectedProducts != null)
                    {
                        if (SelectedProducts.Count > 0)
                        {
                            if (!this.IsCancelledProduct(SelectedProducts.Keys.ToList()))
                            {
                                if (GetProductInfo != null)
                                {
                                    //this.GetProductInfo(SelectedProducts);
                                    this.custom_lineOfSightControl.ProductInfos = this.GetProductInfo(SelectedProducts);
                                }
                            }
                            else
                            {
                                //make the radscheudle invisible
                                this.custom_lineOfSightControl.IsRadScheduleHidden = true;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        protected void RadTabStripOne_TabClick(object sender, RadTabStripEventArgs e)
        {
            this.RedirectToUrlWithQueryString(sender, e);
        }

        protected void radListBoxProducts_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            this.KeepProductsInUrl = true;
            this.CheckedLocations.Clear();
            if (SelectedProducts != null)
            {
                this.SelectedProducts.Clear();
                this.SourcePageProducts.Clear();
            }
            else
            {
                this.SelectedProducts = new Dictionary<int, string>();
            }

            this.SourcePageProducts.Clear();
            foreach (RadPanelItem panelItem in radPanelBar_product_root.Items)
            {
                RadPanelBar radPanelBar_product_child = panelItem.FindControl("radPanelBar_product_child") as RadPanelBar;
                RadListBox productListBoxControl = radPanelBar_product_child.Items[0].FindControl("radListBoxProducts") as RadListBox;

                foreach (RadListBoxItem radItem in productListBoxControl.CheckedItems)
                {
                    var productKey = int.Parse(radItem.Attributes["ProductKey"]);
                    var productName = radItem.Attributes["Product_Name"];
                    this.SelectedProducts.Add(productKey, productName);
                    //used for retrieving when switching pages
                    this.SourcePageProducts.Add(productKey, productName);
                }
                if (productListBoxControl.CheckedItems.Count == 0)
                {
                    //make sure clean up the productinfo also
                    this.custom_lineOfSightControl.ProductInfos = new List<ProductInfo>();
                }
            }

            this.RedirectToUrlWithQueryString(sender, e);
        }

        protected void RadListBox_locations_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            this.KeepProductsInUrl = false;
            //clear all products and reset locations
            this.SelectedProducts.Clear();
            this.SourcePageProducts.Clear();

            this.CheckedLocations = this.RadListBox_locations.Items.ToDictionary(c => c.Text, c => c.Checked);
            var selectedLocations = this.CheckedLocations.Where(c => c.Value == true).Select(c => c.Key);

            if (selectedLocations.Any())
            {
                if (this.GetProductsWithCheckedLocations != null)
                {
                    this.SelectedProducts = this.GetProductsWithCheckedLocations(selectedLocations);
                }
            }
            else
            {
                this.SourcePageProducts.Clear();
            }

            this.RedirectToUrlWithQueryString(sender, e);
        }

        protected void radbutton_deselectAllProjects_Click(object sender, EventArgs e)
        {
            this.SelectedProducts.Clear();
            this.SourcePageProducts.Clear();
            this.CheckedLocations.Clear();
            //make sure clean up the productinfo also
            this.custom_lineOfSightControl.ProductInfos = new List<ProductInfo>();
            this.RedirectToUrlWithQueryString(sender, e);
        }
    }
}
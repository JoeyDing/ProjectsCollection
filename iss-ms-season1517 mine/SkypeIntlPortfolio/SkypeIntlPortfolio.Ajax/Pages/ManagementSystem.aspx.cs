using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.UserControls;
using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile;
using SkypeIntlPortfolio.Ajax.UserControls.Schedule;
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
    public partial class ManagementSystem : CompressViewState, IManagementSystemView
    {
        public Dictionary<int, string> SelectedProducts
        {
            get { return Session.Contents["SelectedProducts"] as Dictionary<int, string>; }
            set { Session.Contents["SelectedProducts"] = value; }
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

        public Dictionary<int, string> AllProducts
        {
            get { return Session.Contents["AllProducts"] as Dictionary<int, string>; }
            set { Session.Contents["AllProducts"] = value; }
        }

        public event Func<string, IEnumerable<IGrouping<string, Products_New>>> GetProductsFamilyList;

        public event Func<int[], bool> IsCancelledProduct;

        protected void ShowProductList()
        {
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    this.radpane_products.Collapsed = false;
                    this.radsplitbar.Visible = true;
                    break;

                case 1:
                    this.radpane_products.Collapsed = false;
                    this.radsplitbar.Visible = true;
                    break;

                case 2:
                    this.radpane_products.Collapsed = false;
                    this.radsplitbar.Visible = true;
                    break;

                case 3:
                    this.radpane_products.Collapsed = false;
                    this.radsplitbar.Visible = true;
                    break;

                default:
                    break;
            }

            this.radpane_products.Width = Unit.Percentage(15);
            this.radpane_content.Width = Unit.Percentage(85);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var msPresenter = new MangementSystemPresenter(this);
            if (this.AllProducts == null)
                this.AllProducts = new Dictionary<int, string>();
            if (this.SelectedProducts == null)
                this.SelectedProducts = new Dictionary<int, string>();
            //clear the checked locations for reporting system page
            if (this.CheckedLocations != null)
            {
                this.CheckedLocations.Clear();
            }

            if (!this.IsPostBack)
            {
                IEnumerable<IGrouping<string, Products_New>> familyList = null;
                string currentTabName = this.SelectTabFromQueryString();
                if (this.GetProductsFamilyList != null)
                    familyList = this.GetProductsFamilyList(currentTabName);
                this.LoadAllProducts(familyList);
                //this method has to be after LoadAllProducts to wait for allProducts to be loaded
                this.LoadSelectedProducts();
                //set the link inside master page "active"
                var navlink = Master.FindControl("li_managementSystem") as HtmlGenericControl;
                navlink.Attributes.Clear();
                navlink.Attributes.Add("class", "active");
                //this method has to be after LoadSelectedProducts to wait for selectedProducts is finally loaded
                LoadCheckedItemsFromSelectedProducts(familyList);
                this.UpdatePageControls();
            }
            this.ShowProductList();

            //load ProductsInfo and Eol Presenter
            var eolPresenter = new EolPresenter(this.custom_eolControl, SelectedProducts);
            eolPresenter.EolPresenterNavigation += NavigationInPresenter;
            var productInfoPresenter = new ProductInfoNewPresenter(this.custom_productInfoNewControl, SelectedProducts);
            productInfoPresenter.ProductPresenterNavigation += NavigationInPresenter;
            var schedulePresenter = new SchedulePresenter(this.custom_scheduleControl, SelectedProducts);
        }

        private void LoadCheckedItemsFromSelectedProducts(IEnumerable<IGrouping<string, Products_New>> familyList)
        {
            foreach (var familyGroup in familyList)
            {
                RadPanelItem panelItem = new RadPanelItem();
                panelItem.Attributes["UserTypePlusFamily"] = familyGroup.Key;
                //panelItem.Text = familyGroup.Key;
                this.radPanelBar_product_root.Items.Add(panelItem);
                RadPanelBar radPanelBar_product_child = panelItem.FindControl("radPanelBar_product_child") as RadPanelBar;
                RadPanelItem familyItem = radPanelBar_product_child.Items[0];
                RadListBox productListBoxControl = familyItem.FindControl("radListBox_products") as RadListBox;
                //if the curretn tab is productInfo or eol, disable the multiple selection
                if (this.RadTabStripOne.SelectedIndex == 1 || this.RadTabStripOne.SelectedIndex == 2)
                {
                    productListBoxControl.ShowCheckAll = false;
                }
                else
                {
                    productListBoxControl.ShowCheckAll = true;
                }
                var orderedFamilyGroup = familyGroup.OrderBy(f => f.Product_Name).ToList();
                foreach (var project in orderedFamilyGroup)
                {
                    //fill the list with items from SelectedProducts
                    var radItem = new RadListBoxItem();
                    radItem.Attributes.Add("ProductName", project.Product_Name);
                    radItem.Attributes.Add("ProductKey", project.ProductKey.ToString());

                    if (this.SelectedProducts != null)
                    {
                        if (SelectedProducts.ContainsKey(project.ProductKey))
                        {
                            radItem.Checked = true;
                            familyItem.Expanded = true;
                        }
                    }
                    radItem.Text = project.Product_Name;
                    productListBoxControl.Items.Add(radItem);
                    radItem.DataBind();
                    this.radPanelBar_product_root.DataBind();
                }
            }
        }

        private void LoadAllProducts(IEnumerable<IGrouping<string, Products_New>> familyList)
        {
            this.AllProducts = new Dictionary<int, string>();
            foreach (var familyGroup in familyList)
            {
                foreach (var project in familyGroup)
                {
                    this.AllProducts[project.ProductKey] = project.Product_Name;
                }
            }
        }

        private void NavigationInPresenter(List<string> navinfo)
        {
            string urlToNavigate = "~/Pages/ManagementSystem.aspx?";
            if (navinfo[0] == "NewProduct")
            {
                string url = Request.Url.AbsoluteUri;
                string _urlToNavigate = url + "&CreateProduct=true";
                Response.Redirect(_urlToNavigate, false);
            }
            else if (navinfo[0] == "ProductInfo" || navinfo[0] == "Eol")
            {
                string currentTabName = navinfo[1];
                urlToNavigate += string.Format("Tab={0}" + "/" + currentTabName, navinfo[0]);
                urlToNavigate += (this.SelectedProducts != null && this.SelectedProducts.Count == 1) ? "&Product=" + this.SelectedProducts.Keys.First() : "";
                Response.Redirect(urlToNavigate, false);
            }
            //After click save and Next when Create New Product
            else
            {
                string _currentTabName = navinfo[1];
                string newProductKey = navinfo[0];
                urlToNavigate += string.Format("Tab={0}" + "/" + _currentTabName, "ProductInfo");
                urlToNavigate += "&Product=" + newProductKey;
                Response.Redirect(urlToNavigate, false);
            }
        }

        protected void RadTabStripOne_TabClick(object sender, RadTabStripEventArgs e)
        {
            this.RedirectToUrl_SwitchTab();
        }

        protected void radListBox_products_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            if (SelectedProducts != null)
            {
                foreach (RadPanelItem panelItem in radPanelBar_product_root.Items)
                {
                    RadPanelBar radPanelBar_product_child = panelItem.FindControl("radPanelBar_product_child") as RadPanelBar;
                    RadListBox productListBoxControl = radPanelBar_product_child.Items[0].FindControl("radListBox_products") as RadListBox;

                    if (Request.QueryString["Tab"] != null && !Request.QueryString["Tab"].Contains("Schedule"))
                    {
                        //For ProductInfo and EOl, uncheck products that are in the history
                        foreach (RadListBoxItem radItem in productListBoxControl.CheckedItems)
                        {
                            var productKey = int.Parse(radItem.Attributes["ProductKey"]);
                            if (SelectedProducts.ContainsKey(productKey))
                            {
                                radItem.Checked = false;
                            }
                        }
                    }
                }
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
                RadListBox productListBoxControl = radPanelBar_product_child.Items[0].FindControl("radListBox_products") as RadListBox;

                foreach (RadListBoxItem radItem in productListBoxControl.CheckedItems)
                {
                    var productKey = int.Parse(radItem.Attributes["ProductKey"]);
                    var productName = radItem.Attributes["ProductName"];
                    this.SelectedProducts.Add(productKey, productName);
                    //SourcePageProducts'll be stored even user switch pages
                    this.SourcePageProducts.Add(productKey, productName);
                }
            }

            this.RedirectToUrl_CheckProduct();
        }

        protected void radButton_OK_Click(object sender, EventArgs e)
        {
            //switch to the schedule page
            //close window
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", "modal_addNewProduct"), true);
            Response.Redirect(Request.RawUrl, false);
        }

        private void LoadSelectedProducts()
        {
            string tabName = SelectTabFromQueryString();

            if (!string.IsNullOrEmpty(Request.QueryString["Products"]))
            {
                if (tabName.Contains("schedule"))
                {
                    int temp = 0;
                    var productKeys = Request.QueryString["Products"].ToLower().Split(new char[] { '|' }).Where(c => int.TryParse(c, out temp)).Select(c => int.Parse(c));
                    this.SelectedProducts.Clear();
                    this.SourcePageProducts.Clear();

                    foreach (var productKey in productKeys)
                    {
                        //if (!this.SelectedProducts.ContainsKey(productKey) && !CanceledProductsList.Contains(productKey))
                        if (!this.SelectedProducts.ContainsKey(productKey))
                            this.SelectedProducts.Add(productKey, (AllProducts.ContainsKey(productKey)) ? AllProducts[productKey] : " ");
                    }
                    //make sure sourcepageproducts(be used when switch page)
                    this.SourcePageProducts = this.SelectedProducts.ToDictionary(p => p.Key, p => p.Value);
                }
                else
                {
                    //remove the URL-Key 'Products' from URL, coz it's not valid when current tab is not Schedule
                    RedirectToUrl_RemoveProducts();
                }
            }
            else if (!string.IsNullOrEmpty(Request.QueryString["Product"]))
            {
                int temp = 0;
                var productKeys = Request.QueryString["Product"].ToLower().Split('|').Where(c => int.TryParse(c, out temp)).Select(c => int.Parse(c));
                if (productKeys.Any())
                {
                    //make sure no duplicated key'll be added
                    this.SelectedProducts.Clear();
                    this.SourcePageProducts.Clear();
                    //make sure the last key
                    this.SelectedProducts.Add(productKeys.Last(), (AllProducts.ContainsKey(productKeys.Last())) ? AllProducts[productKeys.Last()] : " ");
                    this.SourcePageProducts = this.SelectedProducts.ToDictionary(p => p.Key, p => p.Value);
                }
                //this is to check if user input in url: .....&Product=1|4,then take Product=4 and reload the page
                if (Request.QueryString["Product"].Contains('|'))
                {
                    RedirectToUrl_TakeSingleProduct();
                }
            }
            else
            {
                this.SelectedProducts = null;
                //this case is for user switching pages
                if (this.SourcePageProducts.Count() != 0)
                {
                    RedirectToUrl_CheckProduct();
                }
            }
        }

        private string SelectTabFromQueryString()
        {
            string tabName = "";
            if (!string.IsNullOrEmpty(Request.QueryString["Tab"]))
            {
                tabName = Request.QueryString["Tab"].ToLower();
                if (Request.QueryString["Tab"].Contains("/"))
                {
                    tabName = Request.QueryString["Tab"].ToLower().Split('/')[0];
                }

                switch (tabName)
                {
                    case "schedule new":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 0;
                        break;

                    case "productinfo":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 1;
                        break;

                    case "eol":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 2;
                        break;

                    case "schedule":
                        this.RadTabStripOne.SelectedIndex = this.RadMultiPageOne.SelectedIndex = 3;
                        break;

                    default:
                        break;
                }
            }
            return tabName;
        }

        private string GetCurrentNameBySelectIndex()
        {
            string currentTabName = "";
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    currentTabName = "Schedule New";
                    break;

                case 1:
                    currentTabName = "ProductInfo";
                    break;

                case 2:
                    currentTabName = "Eol";
                    break;

                case 3:

                    currentTabName = "Schedule";
                    break;
            }
            return currentTabName;
        }

        private void RedirectToUrl_TakeSingleProduct()
        {
            string urlToNavigate = Request.Url.AbsoluteUri;
            if (urlToNavigate.Contains("Products"))
            {
                //in order to redirect to new url when switch back from multiple products to single product
                urlToNavigate = urlToNavigate.Split(new string[] { "&Products=" }, StringSplitOptions.RemoveEmptyEntries)[0];
            }

            if (this.SourcePageProducts.Count() == 1 && this.SelectedProducts == null)
            {
                //switch back from reporting page
                this.SelectedProducts = new Dictionary<int, string>();
                this.SelectedProducts.Add(this.SourcePageProducts.First().Key, this.SourcePageProducts.First().Value);
                urlToNavigate += (this.SelectedProducts != null && this.SelectedProducts.Count == 1) ? "?&Product=" + this.SelectedProducts.Keys.First() : "";
            }

            if (this.SourcePageProducts.Count() == 1 && this.SelectedProducts.Count() == 1)
            {
                if (urlToNavigate.Contains("Tab"))
                {
                    //if (Request.QueryString["Tab"] == "ProductInfo" || Request.QueryString["Tab"] == "Eol")
                    //can't use the line above cos think of the scenario: ProductInfo/File also should be considered, we should split the '&Product=' for it as well
                    if (Request.QueryString["Tab"].Contains("ProductInfo") || Request.QueryString["Tab"].Contains("Eol"))
                    {
                        urlToNavigate = urlToNavigate.Split(new string[] { "&Product=" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    //loading MS page with selected product but url doesn't contain "Product=" and Tab is not null
                    urlToNavigate += "&Product=" + this.SelectedProducts.Keys.First();
                }
                else
                {
                    //for first time loading MS page with selected product but url doesn't contain "Product="
                    if (!urlToNavigate.Contains("Product"))
                    {
                        urlToNavigate += "?&Product=" + this.SelectedProducts.Keys.First();
                    }
                }
            }
            urlToNavigate = urlToNavigate.Replace("&CreateProduct=true", "");
            Response.Redirect(urlToNavigate, false);
        }

        private void RedirectToUrl_SwitchTab()
        {
            string urlToNavigate = "~/Pages/ManagementSystem.aspx?";
            // Get Current selected tab
            string currentTabName = GetCurrentNameBySelectIndex();
            urlToNavigate += "Tab=" + currentTabName;
            IEnumerable<IGrouping<string, Products_New>> familyList = null;
            if (this.GetProductsFamilyList != null)
                familyList = this.GetProductsFamilyList(currentTabName);

            this.LoadAllProducts(familyList);
            var hashsetdict = new HashSet<int>(SelectedProducts.Keys);
            hashsetdict.IntersectWith(AllProducts.Keys);
            if (hashsetdict.Any())
            {
                //For Schedule case: .....&Products=1|4|3
                if (currentTabName.Contains("Schedule"))
                {
                    //get currrent selected products
                    string currentSelectedProducts = hashsetdict.Select(p => p.ToString()).Aggregate((a, b) => a + "|" + b);
                    urlToNavigate += "&Products=" + currentSelectedProducts;
                }
                else
                {
                    int currentSelectedProduct = hashsetdict.Last();
                    urlToNavigate += "&Product=" + currentSelectedProduct;
                }
            }
            Response.Redirect(urlToNavigate, false);
        }

        private void RedirectToUrl_CheckProduct()
        {
            string urlToNavigate = "~/Pages/ManagementSystem.aspx?";
            // Get Current selected tab
            string currentTabName = GetCurrentNameBySelectIndex();
            urlToNavigate += "Tab=" + currentTabName;
            var hashsetDict = new HashSet<int>(SourcePageProducts.Keys);
            hashsetDict.IntersectWith(AllProducts.Keys);
            SourcePageProducts = hashsetDict.ToDictionary(c => c, c => "");
            if (!currentTabName.Contains("Schedule"))
            {
                if (hashsetDict.Count() == 0)
                {
                    //no product's selected
                    urlToNavigate += (this.SelectedProducts != null && this.SelectedProducts.Any()) ? "&Products=" + this.SelectedProducts.Select(p => p.Key.ToString()).Aggregate((a, b) => a + "|" + b) : "";
                    Response.Redirect(urlToNavigate, false);
                }
                else
                {
                    RedirectToUrl_TakeSingleProduct();
                }
            }
            else
            {
                //get current selected products
                if (hashsetDict.Count() > 1)
                {
                    urlToNavigate += (hashsetDict != null && hashsetDict.Any()) ? "&Products=" + hashsetDict.Select(p => p.ToString()).Aggregate((a, b) => a + "|" + b) : "";
                    Response.Redirect(urlToNavigate, false);
                }
                else if (hashsetDict.Count() == 1)
                {//for first time loading MS page with selected product but url doesn't contain "Product="
                    RedirectToUrl_TakeSingleProduct();
                }
                else
                {
                    //no product's selected
                    urlToNavigate += (this.SelectedProducts != null && this.SelectedProducts.Any()) ? "&Products=" + this.SelectedProducts.Select(p => p.Key.ToString()).Aggregate((a, b) => a + "|" + b) : "";
                    Response.Redirect(urlToNavigate, false);
                }
            }
        }

        private void RedirectToUrl_RemoveProducts()
        {
            string urlToNavigate = "~/Pages/ManagementSystem.aspx?";
            // Get Current selected tab
            string currentTabName = GetCurrentNameBySelectIndex();
            urlToNavigate += "Tab=" + currentTabName;
            Response.Redirect(urlToNavigate, false);
        }

        private IEnumerable<T> GetChildrenOfType<T>(Control parent) where T : class
        {
            var childQueue = new Queue<Control>();
            foreach (Control child in parent.Controls.Cast<object>().Where(c => c is Control))
            {
                childQueue.Enqueue(child);
            }
            while (childQueue.Any())
            {
                Control currentItem = childQueue.Dequeue();
                if (currentItem is T)
                {
                    yield return currentItem as T;
                }

                foreach (Control child in currentItem.Controls.Cast<object>().Where(c => c is Control))
                {
                    childQueue.Enqueue(child);
                }
            }
        }

        private void UpdatePageControls()
        {
            switch (this.RadTabStripOne.SelectedIndex)
            {
                case 0:
                    break;

                case 1:
                    break;

                case 2:
                    break;

                case 3:
                    this.radPanelBar_schedule_root.Items.Clear();
                    AddProductItemsInTabs<ScheduleControl_old>(radPanelBar_schedule_root, "radPanelBar_schedule_child", "custom_scheduleControl_old");
                    this.radPanelBar_schedule_root.DataBind();
                    break;

                default:
                    break;
            }
        }

        private void AddProductItemsInTabs<T>(RadPanelBar rootControl, string radPanelBar_child, string userControl) where T : UserControl, ICustomProjectControl
        {
            if (this.SelectedProducts != null)
            {
                var productInfos = Utils.GetProductInfo(this.SelectedProducts.Keys.ToArray());
                foreach (var product in productInfos)
                {
                    RadPanelItem panelItem = new RadPanelItem();
                    panelItem.Attributes["ProductKey"] = product.ProductKey.ToString();
                    panelItem.Attributes["ProductName"] = product.Product_Name;

                    rootControl.Items.Add(panelItem);

                    RadPanelBar radPanelBar_products_child = panelItem.FindControl(radPanelBar_child) as RadPanelBar;
                    T customControl = radPanelBar_products_child.Items[0].FindControl(userControl) as T;
                    if (customControl != null)
                    {
                        customControl.ProductInfo = product;

                        // if Loc_PM_Location has multipule values separated by '/' , only take the first one for now.
                        if (customControl.ProductInfo.Loc_PM_Location != null && customControl.ProductInfo.Loc_PM_Location.Contains("/"))
                        {
                            customControl.ProductInfo.Loc_PM_Location = customControl.ProductInfo.Loc_PM_Location.Split('/').ToArray()[0];
                        }
                        customControl.Refresh();
                    }

                    rootControl.DataBind();
                }
            }
            else
            {
                this.radPanelBar_schedule_root.Visible = false;
                this.label_warning_cancelledProduct.Visible = true;
                this.label_warning_cancelledProduct.Text = "Sorry, the product you select doesn't exist,please change it to another one";
            }
        }
    }
}
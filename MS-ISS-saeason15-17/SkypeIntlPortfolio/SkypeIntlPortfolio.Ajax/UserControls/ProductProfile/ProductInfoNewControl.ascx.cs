using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.BuildsAndSource;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.CertsAndSignoff;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Files;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Localization;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.People;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Product;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.PseudoInfo;
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.ReleaseInfo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class ProductInfoNewControl : System.Web.UI.UserControl, IProductInfoNewView
    {
        private const string C_Product_Family = "ProductFamily";
        private const string C_Product_Status = "ProductStatus";
        private const string C_Product_Voice = "ProductVoice";
        private const string C_Fabric_Tenant = "FabricTenant";

        public event Action<List<string>> Navigation;

        public Dictionary<int, string> Product
        {
            get
            {
                return this.ViewState["Product"] as Dictionary<int, string>;
            }
            set
            {
                this.ViewState["Product"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //even if int.TryParse(HttpContext.Current.Request.QueryString["Product"], out selectedProductKey) return false,code in  if(!PosyBack) also should be executed
            //if (this.Visible && int.TryParse(HttpContext.Current.Request.QueryString["Product"], out selectedProductKey))
            bool createNewProIsVisible = true;
            if (this.Visible)
            {
                if (!this.IsPostBack)
                {
                    this.SelectTabFromQueryString();

                    //if current tab is product and createproduct is null, show linkbutton
                    if (Request.QueryString["Tab"].Contains("ProductInfo"))
                    {
                        if (Request.QueryString["CreateProduct"] == null)
                        {
                            var tabNameArray = Request.QueryString["Tab"].Split('/');
                            if (tabNameArray.Count() == 2 && tabNameArray[1] != "Product")
                            {
                                createNewProIsVisible = false;
                            }
                            this.link_CreateNewPro.Visible = createNewProIsVisible;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Request.QueryString["CreateProduct"]))
                {
                    ProductPresenter productPresenter = new ProductPresenter(this.ProductControl, null);
                    productPresenter.OnClickNext += Product_OnClickNext;
                }

                if (Product != null && Product.Count == 1)
                {
                    int productKey = Product.Keys.First();
                    using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
                    {
                        var dbProductKeys = new HashSet<int>(portfolioContext.Products_New.Select(c => c.ProductKey));
                        if (dbProductKeys.Any(c => c == productKey))
                        {
                            var productInfo = portfolioContext.Products_New.First(p => p.ProductKey == productKey);
                            //load presenters with data
                            var presenterLocalization = new LocalizationPresenter(this.LocalizationControl, productInfo);
                            presenterLocalization.OnClickNext += ProductInfoSubs_OnClickNext;
                            var presenterPeople = new PeoplePresenter(this.PeopleControl, productInfo);
                            presenterPeople.OnClickNext += ProductInfoSubs_OnClickNext;
                            var presenterLinks = new LinksPresenter(this.PPLinks, productInfo);
                            presenterLinks.OnClickNext += ProductInfoSubs_OnClickNext;
                            var presenterReleaseInfo = new ReleaseInfoPresenter(this.ReleaseInfoControl, productInfo);
                            presenterReleaseInfo.OnClickNext += ProductInfoSubs_OnClickNext;
                            var certs = new CertsAndSignoffPresenter(this.CertsAndSignoffControl, productInfo);
                            certs.OnClickNext += ProductInfoSubs_OnClickNext;
                            var buildsAndSourcePresenter = new BuildsAndSourcePresenter(this.BuildsAndSourceControl, productInfo);
                            buildsAndSourcePresenter.OnClickNext += ProductInfoSubs_OnClickNext;
                            var filesPresenter = new FilesPresenter(this.FilesControl, productInfo);
                            filesPresenter.OnClickNext += ProductInfoSubs_OnClickNext;
                            var productPresenter = new ProductPresenter(this.ProductControl, productInfo);
                            productPresenter.OnClickNext += Product_OnClickNext;
                            var pseudoInfoPresenter = new PseudoInfoPresenter(this.PseudoInfoControl, productInfo);
                            pseudoInfoPresenter.OnClickNext += ProductInfoSubs_OnClickNext;
                        }
                    }
                    createNewProIsVisible = false;
                    this.link_CreateNewPro.Visible = createNewProIsVisible;
                }
                else
                {
                    if (Request.QueryString["Tab"] != null && Request.QueryString["Tab"].Contains("ProductInfo"))
                    {
                        var tabNameArray = Request.QueryString["Tab"].Split('/');
                        if (tabNameArray.Count() == 2 && tabNameArray[1] != "Product")
                        {
                            this.label_warning_unExistingProduct.Visible = true;
                        }
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["CreateProduct"]))
                        this.Panel_PageViews.Visible = true;
                    else
                        this.Panel_PageViews.Visible = false;
                }
            }
        }

        private void Product_OnClickNext(int newProductKey)
        {
            SwichToNextPage(newProductKey);
        }

        private void ProductInfoSubs_OnClickNext(object sender, EventArgs e)
        {
            SwichToNextPage();
        }

        protected void SelectTabFromQueryString()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Tab"]) && Request.QueryString["Tab"].Contains(@"/"))
            {
                if (Request.QueryString["Tab"].Contains("ProductInfo"))
                {
                    string tabName = Request.QueryString["Tab"].ToLower().Split('/')[1];

                    switch (tabName)
                    {
                        case "product":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 0;
                            break;

                        case "files":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 1;
                            break;

                        case "people":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 2;
                            break;

                        case "releaseinfo":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 3;
                            break;

                        case "links":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 4;
                            break;

                        case "buildsandsource":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 5;
                            break;

                        case "localization":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 6;
                            break;

                        case "certsandsignoff":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 7;
                            break;

                        case "pseudoinfo":
                            this.RadTabStripProductInfo.SelectedIndex = this.RadMultiPageInfo.SelectedIndex = 8;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        protected void RadTabStripProductInfo_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            // Get Current selected tab
            string currentTabName = "";
            switch (this.RadTabStripProductInfo.SelectedIndex)
            {
                case 0:
                    currentTabName = "Product";
                    break;

                case 1:
                    currentTabName = "Files";
                    break;

                case 2:
                    currentTabName = "People";
                    break;

                case 3:
                    currentTabName = "ReleaseInfo";
                    break;

                case 4:
                    currentTabName = "Links";
                    break;

                case 5:
                    currentTabName = "BuildsAndSource";
                    break;

                case 6:
                    currentTabName = "Localization";
                    break;

                case 7:
                    currentTabName = "CertsAndSignoff";
                    break;

                case 8:
                    currentTabName = "PseudoInfo";
                    break;

                default:
                    break;
            }

            //bubble up to the parent class which is productinfonew presenter
            if (this.Navigation != null)
                this.Navigation(new List<string> { "ProductInfo", currentTabName });
        }

        protected void RadButton_tab_product_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            //var radTextBox_productName = radPanelBar_ProductInfo.Items[0].FindControl("radTextBox_ProductName") as RadTextBox;
            //var radCombobox_productFamily = radPanelBar_ProductInfo.Items[0].FindControl("radCombobox_ProductFamily") as RadComboBox;
            //var radioButtonList_status = radPanelBar_ProductInfo.Items[0].FindControl("radioButtonList_Status") as RadioButtonList;
            //var radioButtonList_voice = radPanelBar_ProductInfo.Items[0].FindControl("radioButtonList_Voice") as RadioButtonList;
            //var radTextBox_description = radPanelBar_ProductInfo.Items[0].FindControl("radTextBox_description") as RadTextBox;
            //var radioButtonList_fabricTenant = radPanelBar_ProductInfo.Items[0].FindControl("radioButtonList_FabricTenant") as RadioButtonList;

            //using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            //{
            //    //todo: later url'll only support one product
            //    if (string.IsNullOrEmpty(Request.QueryString["Product"]))
            //    {
            //        string currentProductName = radTextBox_productName.Text.Trim();

            //        if (portfolioContext.Products_New.Any(p => p.Product_Name == currentProductName))
            //        {
            //            //var label_warning_productName = this.label_warning_productName;
            //            label_warning_productName.Visible = true;
            //        }
            //        else
            //        {
            //            //save data in the db
            //            int currentFamilyKey = GetKeyByName(C_Product_Family, radCombobox_productFamily.SelectedItem.Text);
            //            int currentProductStatusKey = GetKeyByName(C_Product_Status, radioButtonList_status.SelectedItem.Text);
            //            int currentProductVoiceKey = GetKeyByName(C_Product_Voice, radioButtonList_voice.SelectedItem.Text);
            //            int currentProductFabricTenantKey = GetKeyByName(C_Fabric_Tenant, radioButtonList_fabricTenant.SelectedItem.Text);

            //            string currentProductDescription = radTextBox_description.Text;

            //            Products_New newProduct = new Products_New();
            //            newProduct.Product_Name = currentProductName;
            //            newProduct.ProductFamilyKey = currentFamilyKey;
            //            newProduct.StatusKey = currentProductStatusKey;
            //            newProduct.PVoiceKey = currentProductVoiceKey;
            //            newProduct.Description = currentProductDescription;
            //            newProduct.FabricTenantKey = currentProductFabricTenantKey;
            //            portfolioContext.Products_New.Add(newProduct);
            //            portfolioContext.SaveChanges();
            //            //update current url,add new added product number in the url
            //            if (SelectedProducts != null)
            //            {
            //                this.SelectedProducts.Clear();
            //            }
            //            else
            //            {
            //                this.SelectedProducts = new Dictionary<int, string>();
            //            }

            //            this.SelectedProducts.Add(newProduct.ProductKey, newProduct.Product_Name);
            //            //switch to next page
            //            SwichToNextPage();
            //        }
            //    }
            //    else
            //    {
            //        //update database
            //        //miltiple selction should be disable in advance
            //        int currentSelectedProduct = Convert.ToInt32(Request.QueryString["Product"]);
            //        var productToUpdate = portfolioContext.Products_New.FirstOrDefault(p => p.ProductKey == currentSelectedProduct);

            //        int newFamilyKey = GetKeyByName(C_Product_Family, radCombobox_productFamily.SelectedItem.Text);
            //        int newProductStatusKey = GetKeyByName(C_Product_Status, radioButtonList_status.SelectedItem.Text);
            //        int newProductVoiceKey = GetKeyByName(C_Product_Voice, radioButtonList_voice.SelectedItem.Text);
            //        int newProductFabricTenantKey = GetKeyByName(C_Fabric_Tenant, radioButtonList_fabricTenant.SelectedItem.Text);

            //        string newProductName = radTextBox_productName.Text.Trim();
            //        string newProductDescription = radTextBox_description.Text;

            //        productToUpdate.Product_Name = newProductName;
            //        productToUpdate.ProductFamilyKey = newFamilyKey;
            //        productToUpdate.StatusKey = newProductStatusKey;
            //        productToUpdate.PVoiceKey = newProductVoiceKey;
            //        productToUpdate.Description = newProductDescription;
            //        productToUpdate.FabricTenantKey = newProductFabricTenantKey;

            //        portfolioContext.SaveChanges();

            //        SwichToNextPage();
            //    }
            //}
        }

        /// <summary>
        /// The purpose of this method is to get the corresponding key by a given name,like product name, family name,voice,fabric tenant...
        /// </summary>
        /// <returns></returns>
        protected int GetKeyByName(string tableName, string selectedName)
        {
            int key = 0;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                switch (tableName)
                {
                    case C_Product_Family:
                        key = portfolioContext.ProductFamilies.Where(f => f.Product_Family == selectedName).Select(f => f.FamilyKey).FirstOrDefault();
                        break;

                    case C_Product_Status:
                        key = portfolioContext.Status.Where(s => s.Status1 == selectedName).Select(s => s.StatusKey).FirstOrDefault();
                        break;

                    case C_Product_Voice:
                        key = portfolioContext.ProductVoices.Where(v => v.Product_Voice == selectedName).Select(v => v.VoiceKey).FirstOrDefault();
                        break;

                    case C_Fabric_Tenant:
                        key = portfolioContext.FabricTenants.Where(t => t.Fabric_Tenant == selectedName).Select(t => t.TenantKey).FirstOrDefault();
                        break;

                    default:
                        break;
                }
            }
            return key;
        }

        protected void RadButton_tab_people_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            //SwichToNextPage();
        }

        protected void RadButton_tab_BuildsAndSource_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            //SwichToNextPage();
        }

        protected void RadButton_tab_CertsAndSignoff_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            //SwichToNextPage();
        }

        protected void SwichToNextPage(int newProductKey = 0)
        {
            string currentTabName = "";
            if (this.RadTabStripProductInfo.SelectedIndex == this.RadTabStripProductInfo.Tabs.Count - 1)
            {
                this.RadTabStripProductInfo.SelectedIndex = 0;
            }
            else
            {
                this.RadTabStripProductInfo.SelectedIndex += 1;
            }
            switch (this.RadTabStripProductInfo.SelectedIndex)
            {
                case 0:
                    currentTabName = "Product";
                    break;

                case 1:
                    currentTabName = "Files";
                    break;

                case 2:
                    currentTabName = "People";
                    break;

                case 3:
                    currentTabName = "ReleaseInfo";
                    break;

                case 4:
                    currentTabName = "Links";
                    break;

                case 5:
                    currentTabName = "BuildsAndSource";
                    break;

                case 6:
                    currentTabName = "Localization";
                    break;

                case 7:
                    currentTabName = "CertsAndSignoff";
                    break;

                case 8:
                    currentTabName = "PseudoInfo";
                    break;
            }
            if (newProductKey == 0)
            {
                if (this.Navigation != null)
                    this.Navigation(new List<string> { "ProductInfo", currentTabName });
            }
            else
            {
                if (this.Navigation != null)
                    this.Navigation(new List<string> { newProductKey.ToString(), currentTabName });
            }
        }

        protected void link_CreateNewPro_Click(object sender, EventArgs e)
        {
            this.Panel_PageViews.Visible = true;
            if (this.Navigation != null)
                this.Navigation(new List<string> { "NewProduct", null });
        }
    }
}
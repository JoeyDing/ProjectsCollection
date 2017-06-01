using SkypeIntlPortfolio.Ajax.Core.Service;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Model.Mock;
using SkypeIntlPortfolio.Ajax.UserControls.Eol;
using SkypeIntlPortfolio.Ajax.UserControls.Eol.UALanguageProduct;
using SkypeIntlPortfolio.Ajax.UserControls.Eol.UILanguageProduct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public partial class EOLControl : System.Web.UI.UserControl, IEolView
    {
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

        public bool isCancelled
        {
            get
            {
                if (Session.Contents["isCancelled"] == null)
                    Session.Contents["isCancelled"] = false;
                return (bool)Session.Contents["isCancelled"];
            }
            set
            {
                Session.Contents["isCancelled"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (!IsPostBack)
                    //after resonse.redirect, do this
                    SelectTabFromQueryString();
                else
                    isCancelled = false;
                //since event will be null again after postback(propery won't since we have them stored in view state), you need to call the constructor of the presenters no matter IsPostBack is true or false
                if (Product != null && Product.Count == 1)
                {
                    int productKey = Product.Keys.First();

                    isCancelled = this.IsProductCancelled(productKey);
                    using (SkypeIntlPlanningPortfolioEntities db = new SkypeIntlPlanningPortfolioEntities())
                    {
                        var dbProductKeys = new HashSet<int>(db.Products_New.Select(c => c.ProductKey));
                        //check if Products_New has this productKey
                        if (dbProductKeys.Contains(productKey))
                        {
                            //1.TextInputOutput
                            var textInputOutputPresenter = new TextInputOutputPresenter(this.tio, productKey);
                            //2.SpokenInputOutput
                            var spokenInputOutputPresenter = new SpokenInputOutputPresenter(this.sio, productKey);

                            var bulkInsertService = new BulkInsertService();
                            var bulkUpdateService = new BulkUpdateService();

                            //4.UILanguageProduct and UALanguageProduct
                            var uiLanguageProduct = new UILanguageProductPresenter(this.uiLanguageProduct, productKey, bulkInsertService, bulkUpdateService);
                            var uaLanguageProduct = new UALanguageProductPresenter(this.uaLanguageProduct, productKey, bulkInsertService, bulkUpdateService);
                            //5. LocalizedFile
                            var localizedFilePresenter = new LocalizedFilePresenter(this.locFile, productKey);
                        }
                    }
                }
                else
                {
                    foreach (RadPageView pageView in this.RadMultiPageEOL.PageViews)
                    {
                        pageView.Visible = false;
                    }
                    label_warning_unExistingProduct.Visible = true;
                    if (!isCancelled)
                        label_warning_unExistingProduct.Text = "Sorry, the product you select doesn't exist,please change it to another one";
                    else
                        label_warning_unExistingProduct.Text = "Sorry, the product you select is cancelled";
                }
            }
        }

        protected void RadTabStripEOL_TabClick(object sender, RadTabStripEventArgs e)
        {
            // Get Current selected tab
            string currentTabName = "";
            switch (this.RadTabStripEOL.SelectedIndex)
            {
                case 0:
                    currentTabName = "UILanguageProduct";
                    break;

                case 1:
                    currentTabName = "UALanguageProduct";
                    break;

                case 2:
                    currentTabName = "LocalizedFile";
                    break;

                case 3:
                    currentTabName = "TextInputOutput";
                    break;

                case 4:
                    currentTabName = "SpokenInputOutput";
                    break;
            }
            if (this.Navigation != null)
                this.Navigation(new List<string> { "Eol", currentTabName });
        }

        protected void SelectTabFromQueryString()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["Tab"]) && Request.QueryString["Tab"].Contains(@"/"))
            {
                //if (Request.QueryString["Tab"].Contains("Eol"))
                //{
                string tabName = Request.QueryString["Tab"].ToLower().Split('/')[1];

                switch (tabName)
                {
                    case "uilanguageproduct":
                        this.RadTabStripEOL.SelectedIndex = this.RadMultiPageEOL.SelectedIndex = 0;
                        break;

                    case "ualanguageproduct":
                        this.RadTabStripEOL.SelectedIndex = this.RadMultiPageEOL.SelectedIndex = 1;
                        break;

                    case "localizedfile":
                        this.RadTabStripEOL.SelectedIndex = this.RadMultiPageEOL.SelectedIndex = 2;
                        break;

                    case "textinputoutput":
                        this.RadTabStripEOL.SelectedIndex = this.RadMultiPageEOL.SelectedIndex = 3;
                        break;

                    case "spokeninputoutput":
                        this.RadTabStripEOL.SelectedIndex = this.RadMultiPageEOL.SelectedIndex = 4;
                        break;
                }
            }
        }

        public event Action<List<string>> Navigation;

        public event Func<int, bool> IsProductCancelled;
    }
}
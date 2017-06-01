using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links
{
    public class LinksPresenter
    {
        private ILinksView _view;
        private Products_New _product;
        public EventHandler OnClickNext;

        public LinksPresenter(ILinksView view, Products_New selectedProduct)
        {
            this._view = view;
            this._product = selectedProduct;
            this._view.LoadLinkData += LoadLinkData;
            view.OnClickNext += view_OnClickNext;
        }

        private void LoadLinkData()
        {
            if (_product != null)
            {
                this._view.VSOLinkLocalization = this._product.Localization_VSO_URL;
                this._view.VSOlinkCore = this._product.Core_VSO_Backlog_URL;
                //this.view.CoreTeamContact = SelectedProduct.Core_Team_OneNote;
                this._view.BuildLocation = this._product.Release_build_Location;
                this._view.LCGLocation = this._product.LCG_File_Path;
                this._view.LCTLocation = this._product.LCT_File_Path;
                this._view.LCLLocation = this._product.LCL_File_Path;
            }
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            //check if can save data
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var targetProduct = portfolioContext.Products_New.Where(p => p.ProductKey == this._product.ProductKey).FirstOrDefault();
                targetProduct.Localization_VSO_URL = this._view.VSOLinkLocalization;
                targetProduct.Core_VSO_Backlog_URL = this._view.VSOlinkCore;
                targetProduct.Release_build_Location = this._view.BuildLocation;
                targetProduct.LCG_File_Path = this._view.LCGLocation.Trim().TrimStart(';');
                targetProduct.LCT_File_Path = this._view.LCTLocation.Trim().TrimStart(';');
                targetProduct.LCL_File_Path = this._view.LCLLocation.Trim().TrimStart(';');
                //todo: later reset all corresponding data
                portfolioContext.SaveChanges();
            }

            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.PseudoInfo
{
    public class PseudoInfoPresenter
    {
        private IPseudoInfoView _view;
        private Products_New _selectedProduct;

        public event EventHandler OnClickNext;

        public PseudoInfoPresenter(IPseudoInfoView view, Products_New selectedProduct)
        {
            this._view = view;
            this._selectedProduct = selectedProduct;
            this._view.LoadPseudoInfo += _view_LoadPseudoInfo;
            this._view.OnClickNext += _view_OnClickNext;
        }

        private void _view_LoadPseudoInfo()
        {
            this._view.PseudoBuildEnabled = this._selectedProduct.Pseudo_Build_Enabled;
            this._view.PseudoRunBeofreCheckIn = this._selectedProduct.Pseudo_Run_Beofre_Check_In;
            this._view.PseudoTestingNLocChecks = this._selectedProduct.Pseudo_Testing_And_Loc_Checks;
            this._view.PseudoTestingRunRegular = this._selectedProduct.Pseudo_Testing_Run_Regular;
        }

        private void _view_OnClickNext(object sender, EventArgs e)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var product = new Products_New { };
                product.ProductKey = this._selectedProduct.ProductKey;
                product.Pseudo_Build_Enabled = this._view.PseudoBuildEnabled;
                product.Pseudo_Run_Beofre_Check_In = this._view.PseudoRunBeofreCheckIn;
                product.Pseudo_Testing_And_Loc_Checks = this._view.PseudoTestingNLocChecks;
                product.Pseudo_Testing_Run_Regular = this._view.PseudoTestingRunRegular;

                context.Products_New.Attach(product);
                context.Entry(product).Property(x => x.Pseudo_Build_Enabled).IsModified = true;
                context.Entry(product).Property(x => x.Pseudo_Run_Beofre_Check_In).IsModified = true;
                context.Entry(product).Property(x => x.Pseudo_Testing_And_Loc_Checks).IsModified = true;
                context.Entry(product).Property(x => x.Pseudo_Testing_Run_Regular).IsModified = true;

                context.SaveChanges();
            }
            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
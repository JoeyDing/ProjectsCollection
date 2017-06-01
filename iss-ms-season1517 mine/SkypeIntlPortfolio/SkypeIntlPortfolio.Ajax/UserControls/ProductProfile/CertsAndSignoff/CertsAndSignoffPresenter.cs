using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.CertsAndSignoff
{
    public class CertsAndSignoffPresenter
    {
        private ICertsAndSignoffView _view;
        private Products_New _selectedProduct;

        public event EventHandler OnClickNext;

        public CertsAndSignoffPresenter(ICertsAndSignoffView view, Products_New selectedProduct)
        {
            this._view = view;
            this._selectedProduct = selectedProduct;
            this._view.OnClickNext += _view_OnClickNext;
            this._view.LoadCertsAndSignoffData += _view_LoadCertsAndSignoffData;
        }

        private void _view_LoadCertsAndSignoffData()
        {
            //load data
            this._view.GBImpacting = this._selectedProduct.GB_Impacting;
            this._view.FrenchLocRequired = this._selectedProduct.French_Loc_Required;
            this._view.PrivacyStatementrequired = _selectedProduct.Privacy_Statement_Required;
            this._view.VoicePromptLocrequirement = this._selectedProduct.Voice_Prompt_Loc_Required;
            this._view.TelemetryDataAvailable = this._selectedProduct.Telemetry_Data_available;
            this._view.CertType = this._selectedProduct.Certification_Type;
            this._view.CertLocation = this._selectedProduct.Cert_Location;
        }

        private void _view_OnClickNext(object sender, EventArgs e)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var product = new Products_New { };
                product.ProductKey = this._selectedProduct.ProductKey;
                product.GB_Impacting = this._view.GBImpacting;
                product.French_Loc_Required = this._view.FrenchLocRequired;
                product.Privacy_Statement_Required = this._view.PrivacyStatementrequired;
                product.Voice_Prompt_Loc_Required = this._view.VoicePromptLocrequirement;
                product.Telemetry_Data_available = this._view.TelemetryDataAvailable;
                product.Certification_Type = this._view.CertType;
                product.Cert_Location = this._view.CertLocation;

                context.Products_New.Attach(product);
                context.Entry(product).Property(x => x.GB_Impacting).IsModified = true;
                context.Entry(product).Property(x => x.French_Loc_Required).IsModified = true;
                context.Entry(product).Property(x => x.Privacy_Statement_Required).IsModified = true;
                context.Entry(product).Property(x => x.Voice_Prompt_Loc_Required).IsModified = true;
                context.Entry(product).Property(x => x.Telemetry_Data_available).IsModified = true;
                context.Entry(product).Property(x => x.Certification_Type).IsModified = true;
                context.Entry(product).Property(x => x.Cert_Location).IsModified = true;

                context.SaveChanges();
            }

            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
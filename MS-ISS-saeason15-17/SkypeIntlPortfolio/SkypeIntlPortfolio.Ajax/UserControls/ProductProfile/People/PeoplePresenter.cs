using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.People
{
    public class PeoplePresenter
    {
        private IPeopleView view;

        public Products_New SelectedProduct
        {
            get;
            set;
        }

        private Dictionary<int, string> _product;

        public event EventHandler OnClickNext;

        public PeoplePresenter(IPeopleView view, Products_New selectedProduct)
        {
            this.view = view;
            SelectedProduct = selectedProduct;
            this.view.OnClickNext += view_OnClickNext;
            this.view.LoadPPPeopleInfo += view_LoadPPPeopleInfo;
        }

        private void view_LoadPPPeopleInfo()
        {
            view_GetMSPMOwner();
            view_GetISSOwner();
            view_GetISSIPE();
            view_GetISSTester();
            view_GetCoreTeamContact();
            view_GetTelemetryContact();
            view_GetCoreEngineeringContact();
            view_GetCoreTeamSharePoint();
            view_GetCoreDesignContact();
            view_GetCoreTeamLocation();
            view_GetMSPMOwnerLocation();
        }

        private void view_GetTelemetryContact()
        {
            this.view.TelemetryContact = this.SelectedProduct.Telemetry_Contact;
        }

        private void view_GetCoreEngineeringContact()
        {
            this.view.CoreEngineeringContact = this.SelectedProduct.Core_Engineering_Contact;
        }

        private void view_GetMSPMOwner()
        {
            this.view.MSPMOwner = this.SelectedProduct.Product_Owner;
        }

        private void view_GetISSOwner()
        {
            this.view.ISSOwner = this.SelectedProduct.ISS_Ops_Driver;
        }

        private void view_GetISSIPE()
        {
            this.view.ISSIPE = this.SelectedProduct.IPE_Owner;
        }

        private void view_GetISSTester()
        {
            this.view.ISSTester = this.SelectedProduct.Test_Owner;
        }

        //core team one note
        private void view_GetCoreTeamContact()
        {
            this.view.CoreTeamContact = this.SelectedProduct.Core_PO;
        }

        private void view_GetCoreTeamSharePoint()
        {
            this.view.CoreTeamSharePoint = this.SelectedProduct.Core_Team_SharePoint;
        }

        private void view_GetCoreDesignContact()
        {
            this.view.CoreDesignContact = this.SelectedProduct.Core_Design_Contact;
        }

        private void view_GetMSPMOwnerLocation()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedselectedMSPMOwnerLocationKey = 0;
                string selectedselectedMSPMOwnerLocationName = "";
                if (SelectedProduct != null)
                {
                    selectedselectedMSPMOwnerLocationKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).Select(c => c.OwnerLocationKey).FirstOrDefault());
                    selectedselectedMSPMOwnerLocationName = portfolioContext.Locations.Where(c => c.LocationKey == selectedselectedMSPMOwnerLocationKey).Select(c => c.Location1).FirstOrDefault();
                }

                var MSPMOwnerLocatioList = portfolioContext.Locations.Select(r => r.Location1).ToList();
                var result = new List<MSPMOwnerLocation>();
                foreach (string olItem in MSPMOwnerLocatioList)
                {
                    MSPMOwnerLocation newOL = new MSPMOwnerLocation();
                    newOL.MSPMOwnerLocationName = olItem;
                    result.Add(newOL);
                    if (selectedselectedMSPMOwnerLocationName == olItem)
                    {
                        newOL.IsChecked = true;
                    }
                }
                this.view.MSPMOwnerLocation = result;
            }
        }

        private void view_GetCoreTeamLocation()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, int> selectedCoreTeamLocationList = new Dictionary<string, int>();
                //since the selectProduct has been closed in the aboved "using" chunk from PPReleaseInfoPresenter, here we should requery data
                if (SelectedProduct != null)
                {
                    //add selected channels into the channellist
                    foreach (var item in portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).SelectMany(c => c.Locations))
                    {
                        selectedCoreTeamLocationList.Add(item.Location1, item.LocationKey);
                    }
                }

                var coreTeamLocationList = portfolioContext.Locations.Select(c => c.Location1).ToList();
                var result = new List<CoreTeamLocation>();
                foreach (string ctlItem in coreTeamLocationList)
                {
                    CoreTeamLocation newCTL = new CoreTeamLocation();
                    newCTL.CoreTeamLocationName = ctlItem;
                    result.Add(newCTL);

                    if (selectedCoreTeamLocationList.ContainsKey(ctlItem))
                    {
                        newCTL.IsChecked = true;
                    }
                    else
                    {
                        newCTL.IsChecked = false;
                    }
                }

                this.view.CoreTeamLocation = result;
            }
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            //check if can save data
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Products_New product = portfolioContext.Products_New.Where(p => p.ProductKey == SelectedProduct.ProductKey).FirstOrDefault();
                product.Product_Owner = this.view.MSPMOwner;
                product.ISS_Ops_Driver = this.view.ISSOwner;
                product.IPE_Owner = this.view.ISSIPE;
                product.Test_Owner = this.view.ISSTester;
                product.Core_PO = this.view.CoreTeamContact;
                product.Core_Design_Contact = this.view.CoreDesignContact;
                product.Core_Team_SharePoint = this.view.CoreTeamSharePoint;
                product.Telemetry_Contact = this.view.TelemetryContact;
                product.Core_Engineering_Contact = this.view.CoreEngineeringContact;

                var currentCoreTeamLocations = product.Locations.ToList().ToDictionary(c => c.LocationKey, c => c);

                foreach (var addedItem in this.view.CoreTeamLocation)
                {
                    int coreTeamLocationKey = GetCoreTeamLocationKeyByName(addedItem.CoreTeamLocationName);
                    if (addedItem.IsChecked)
                    {
                        if (!currentCoreTeamLocations.ContainsKey(coreTeamLocationKey))
                        {
                            //attach it to right table so that it doesn't create a new Source Control together with the link
                            Ajax.Location existingCoreTeamLocation = new Ajax.Location { LocationKey = coreTeamLocationKey };
                            portfolioContext.Locations.Attach(existingCoreTeamLocation);
                            product.Locations.Add(existingCoreTeamLocation);
                        }
                    }
                    else
                    {
                        if (currentCoreTeamLocations.ContainsKey(coreTeamLocationKey))
                        {
                            var itemToDelete = currentCoreTeamLocations[coreTeamLocationKey];
                            product.Locations.Remove(itemToDelete);
                        }
                    }
                }

                string mSPMOwnerLocation = "";
                foreach (var olItem in this.view.MSPMOwnerLocation)
                {
                    if (olItem.IsChecked)
                        mSPMOwnerLocation = olItem.MSPMOwnerLocationName;
                }
                int mSPMOwnerLocationKey = GetMSPMOwnerLocationKeyByName(mSPMOwnerLocation);
                product.OwnerLocationKey = mSPMOwnerLocationKey;

                portfolioContext.SaveChanges();
            }

            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        private int GetMSPMOwnerLocationKeyByName(string mSPMOwnerLocation)
        {
            int mSPMOwnerLocationKey = 0;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedMSPMOwnerLocation = portfolioContext.Locations.Where(r => r.Location1 == mSPMOwnerLocation).FirstOrDefault();
                if (selectedMSPMOwnerLocation != null)
                {
                    mSPMOwnerLocationKey = selectedMSPMOwnerLocation.LocationKey;
                }
            }
            return mSPMOwnerLocationKey;
        }

        private int GetCoreTeamLocationKeyByName(string coreTeamLocation)
        {
            int coreTeamLocationKey;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedCoreTeamLocation = portfolioContext.Locations.Where(r => r.Location1 == coreTeamLocation).FirstOrDefault();
                coreTeamLocationKey = selectedCoreTeamLocation.LocationKey;
            }
            return coreTeamLocationKey;
        }
    }
}
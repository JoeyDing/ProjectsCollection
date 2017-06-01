using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.ReleaseInfo
{
    public class ReleaseInfoPresenter
    {
        private IReleaseInfoView view;

        public EventHandler OnClickNext;

        public Products_New SelectedProduct
        {
            get;
            set;
        }

        public ReleaseInfoPresenter(IReleaseInfoView view, Products_New selectedProduct)
        {
            this.view = view;
            SelectedProduct = selectedProduct;
            this.view.OnClickNext += view_OnClickNext;
            this.view.LoadPPReleaseInfo += view_LoadPPReleaseInfo;
        }

        private void view_LoadPPReleaseInfo()
        {
            view_GetReleaseChannel();
            view_GetReleaseCadence();
            view_GetReleaseLanguageSelection();
            view_GetReleaseContentLocation();
            view_GetReleasePlatform();
        }

        private void view_GetReleaseCadence()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, int> selectedReleaseChannelList = new Dictionary<string, int>();
                //since the selectProduct has been closed in the aboved "using" chunk from PPReleaseInfoPresenter, here we should requery data
                int selectedReleaseCadenceKey = 0;
                string selectedReleaseCadenceName = "";
                if (SelectedProduct != null)
                {
                    selectedReleaseCadenceKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).Select(c => c.ReleaseCadenceKey).FirstOrDefault());
                    selectedReleaseCadenceName = portfolioContext.ReleaseCadences.Where(c => c.CadenceKey == selectedReleaseCadenceKey).Select(c => c.Release_Cadence).FirstOrDefault();
                }

                var releaseCadenceList = portfolioContext.ReleaseCadences.Select(r => r.Release_Cadence).ToList();
                var result = new List<ReleaseCadence>();
                foreach (string rcItem in releaseCadenceList)
                {
                    ReleaseCadence newRC = new ReleaseCadence();
                    newRC.ReleaseCadenceName = rcItem;
                    result.Add(newRC);
                    if (selectedReleaseCadenceName == rcItem)
                    {
                        newRC.IsChecked = true;
                    }
                }
                this.view.ReleaseCadence = result;
            }
        }

        private void view_GetReleaseContentLocation()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedReleaseContentLocationKey = 0;
                string selectedReleaseContentLocationName = "";
                if (SelectedProduct != null)
                {
                    selectedReleaseContentLocationKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).Select(c => c.ContentPublishedToKey).FirstOrDefault());
                    selectedReleaseContentLocationName = portfolioContext.ProductContents.Where(c => c.ContentKey == selectedReleaseContentLocationKey).Select(c => c.Product_Content_Published_To).FirstOrDefault();
                }

                var releaseContentLocationList = portfolioContext.ProductContents.Select(r => r.Product_Content_Published_To).ToList();
                var result = new List<ContentLocation>();
                foreach (string lsItem in releaseContentLocationList)
                {
                    ContentLocation newCL = new ContentLocation();
                    newCL.ContentLocationName = lsItem;
                    result.Add(newCL);
                    if (selectedReleaseContentLocationName == lsItem)
                    {
                        newCL.IsChecked = true;
                    }
                }
                this.view.ContentLocation = result;
            }
        }

        private void view_GetReleaseLanguageSelection()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedReleaseLanguageKey = 0;
                string selectedReleaseLanguageName = "";
                if (SelectedProduct != null)
                {
                    selectedReleaseLanguageKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).Select(c => c.LanguageSettingsKey).FirstOrDefault());
                    selectedReleaseLanguageName = portfolioContext.LanguageSettings.Where(c => c.LangSettingsKey == selectedReleaseLanguageKey).Select(c => c.Language_Settings).FirstOrDefault();
                }

                var releaseLanguageList = portfolioContext.LanguageSettings.Select(r => r.Language_Settings).ToList();
                var result = new List<LanguageSelection>();
                foreach (string lsItem in releaseLanguageList)
                {
                    LanguageSelection newLS = new LanguageSelection();
                    newLS.LanguageSelectionName = lsItem;
                    result.Add(newLS);
                    if (selectedReleaseLanguageName == lsItem)
                    {
                        newLS.IsChecked = true;
                    }
                }
                this.view.LanguageSelection = result;
            }
        }

        private void view_GetReleasePlatform()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, int> selectedReleasePlatformList = new Dictionary<string, int>();
                if (SelectedProduct != null)
                {
                    foreach (var item in portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).SelectMany(c => c.Platforms))
                    {
                        selectedReleasePlatformList.Add(item.Platform1, item.PlatformKey);
                    }
                }

                var releasePlatformList = portfolioContext.Platforms.Select(c => c.Platform1).ToList();
                var result = new List<ReleasePlatform>();
                foreach (string rpItem in releasePlatformList)
                {
                    ReleasePlatform newRPF = new ReleasePlatform();
                    newRPF.ReleasePlatformName = rpItem;
                    result.Add(newRPF);

                    if (selectedReleasePlatformList.ContainsKey(rpItem))
                    {
                        newRPF.IsChecked = true;
                    }
                    else
                    {
                        newRPF.IsChecked = false;
                    }
                }

                this.view.ReleasePlatform = result;
            }
        }

        /// <summary>
        /// this method 'll be executed only when page is not post back, and after save button's clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_GetReleaseChannel()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, int> selectedReleaseChannelList = new Dictionary<string, int>();
                //since the selectProduct has been closed in the aboved "using" chunk from PPReleaseInfoPresenter, here we should requery data
                if (SelectedProduct != null)
                {
                    //add selected channels into the channellist
                    foreach (var item in portfolioContext.Products_New.Where(c => c.ProductKey == SelectedProduct.ProductKey).SelectMany(c => c.ReleaseChannels))
                    {
                        selectedReleaseChannelList.Add(item.Release_Channel, item.ReleaseChannelKey);
                    }
                }

                var releaseChannelList = portfolioContext.ReleaseChannels.Select(c => c.Release_Channel).ToList();
                var result = new List<ReleaseChannel>();
                foreach (string rclItem in releaseChannelList)
                {
                    ReleaseChannel newRCL = new ReleaseChannel();
                    newRCL.ReleaseChannelName = rclItem;
                    result.Add(newRCL);

                    if (selectedReleaseChannelList.ContainsKey(rclItem))
                    {
                        newRCL.IsChecked = true;
                    }
                    else
                    {
                        newRCL.IsChecked = false;
                    }
                }

                this.view.ReleaseChannel = result;
            }
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Products_New product = portfolioContext.Products_New.Where(p => p.ProductKey == SelectedProduct.ProductKey).FirstOrDefault();
                string cadenceName = "";
                foreach (var rcItem in this.view.ReleaseCadence)
                {
                    if (rcItem.IsChecked)
                        cadenceName = rcItem.ReleaseCadenceName.ToString();
                }
                int cadenceKey = GetReleaseCadenceKeyByName(cadenceName);
                product.ReleaseCadenceKey = cadenceKey;

                string LocationName = "";
                foreach (var rcItem in this.view.ContentLocation)
                {
                    if (rcItem.IsChecked)
                        LocationName = rcItem.ContentLocationName.ToString();
                }
                int locationKey = GetReleaseLocationKeyByName(LocationName);
                product.ContentPublishedToKey = locationKey;

                string LanguageName = "";
                foreach (var lsItem in this.view.LanguageSelection)
                {
                    if (lsItem.IsChecked)
                        LanguageName = lsItem.LanguageSelectionName;
                }
                int languageKey = GetReleaseLanguageKeyByName(LanguageName);
                product.LanguageSettingsKey = languageKey;

                var currentReleaseChannels = product.ReleaseChannels.ToList().ToDictionary(c => c.ReleaseChannelKey, c => c);

                foreach (var addedItem in this.view.ReleaseChannel)
                {
                    int releaseChannelKey = GetReleaseChannelKeyByName(addedItem.ReleaseChannelName);
                    if (addedItem.IsChecked)
                    {
                        if (!currentReleaseChannels.ContainsKey(releaseChannelKey))
                        {
                            //attach it to right table so that it doesn't create a new Source Control together with the link
                            Ajax.ReleaseChannel existingReleaseChannel = new Ajax.ReleaseChannel { ReleaseChannelKey = releaseChannelKey };
                            portfolioContext.ReleaseChannels.Attach(existingReleaseChannel);
                            product.ReleaseChannels.Add(existingReleaseChannel);
                        }
                    }
                    else
                    {
                        if (currentReleaseChannels.ContainsKey(releaseChannelKey))
                        {
                            var itemToDelete = currentReleaseChannels[releaseChannelKey];
                            product.ReleaseChannels.Remove(itemToDelete);
                        }
                    }
                }

                var currentReleasePlatforms = product.Platforms.ToList().ToDictionary(c => c.PlatformKey, c => c);

                foreach (var addedItem in this.view.ReleasePlatform)
                {
                    int releasePlatformKey = GetReleasePlatformKeyByName(addedItem.ReleasePlatformName);
                    if (addedItem.IsChecked)
                    {
                        if (!currentReleasePlatforms.ContainsKey(releasePlatformKey))
                        {
                            //attach it to right table so that it doesn't create a new Source Control together with the link
                            Ajax.Platform existingReleasePlatform = new Ajax.Platform { PlatformKey = releasePlatformKey };
                            portfolioContext.Platforms.Attach(existingReleasePlatform);
                            product.Platforms.Add(existingReleasePlatform);
                        }
                    }
                    else
                    {
                        if (currentReleasePlatforms.ContainsKey(releasePlatformKey))
                        {
                            var itemToDelete = currentReleasePlatforms[releasePlatformKey];
                            product.Platforms.Remove(itemToDelete);
                        }
                    }
                }

                portfolioContext.SaveChanges();
            }

            //bubble up click event for any parent handler
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        private int GetReleaseLocationKeyByName(string locationName)
        {
            int releaseLocationKey = 0;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //var selectedReleaseLocation = portfolioContext.Locations.Where(r => r.Location1 == locationName).FirstOrDefault();
                var selectedReleaseLocation = portfolioContext.ProductContents.Where(r => r.Product_Content_Published_To == locationName).FirstOrDefault();
                if (selectedReleaseLocation != null)
                {
                    releaseLocationKey = selectedReleaseLocation.ContentKey;
                }
            }
            return releaseLocationKey;
        }

        private int GetReleaseCadenceKeyByName(string releaseCadenceName)
        {
            int releaseCadenceKey = 0;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedReleaseCadence = portfolioContext.ReleaseCadences.Where(r => r.Release_Cadence == releaseCadenceName).FirstOrDefault();
                if (selectedReleaseCadence != null)
                {
                    releaseCadenceKey = selectedReleaseCadence.CadenceKey;
                }
            }
            return releaseCadenceKey;
        }

        private int GetReleaseChannelKeyByName(string releaseChannelName)
        {
            int releaseChannelKey;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedReleaseChannel = portfolioContext.ReleaseChannels.Where(r => r.Release_Channel == releaseChannelName).FirstOrDefault();
                releaseChannelKey = selectedReleaseChannel.ReleaseChannelKey;
            }
            return releaseChannelKey;
        }

        private int GetReleasePlatformKeyByName(string releasePlatformName)
        {
            int releasePlatformKey;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedReleasePlatform = portfolioContext.Platforms.Where(r => r.Platform1 == releasePlatformName).FirstOrDefault();
                releasePlatformKey = selectedReleasePlatform.PlatformKey;
            }
            return releasePlatformKey;
        }

        private int GetReleaseLanguageKeyByName(string releaseLanguageName)
        {
            int releaseLanguageKey;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedReleaseLanguage = portfolioContext.LanguageSettings.Where(r => r.Language_Settings == releaseLanguageName).FirstOrDefault();
                releaseLanguageKey = selectedReleaseLanguage.LangSettingsKey;
            }
            return releaseLanguageKey;
        }
    }
}
using Microsoft.Win32;
using Sfb.Core.Interfaces;
using System;

namespace Sfb.Core.Services
{
    public class SwitchLanguageService : ISwitchLanguage
    {
        private ICloseSfbClient closeSfbClientService;

        public SwitchLanguageService(ICloseSfbClient closeSfbClientService)
        {
            this.closeSfbClientService = closeSfbClientService;
        }

        public void SwitchLanguage(LocCulture locCulture, OfficeType officeType)
        {
            //1 - Access the right registry key
            string officeRegPath = officeType == OfficeType.O15 ? "15.0" : "16.0";
            var languageResourcesKey = Registry.CurrentUser.OpenSubKey(string.Format(@"SOFTWARE\Microsoft\Office\{0}\Common\LanguageResources", officeRegPath), true);

            //2 - SwitchLanguage
            string currentCulture = null;
            string newCulture = null;
            switch (officeType)
            {
                case OfficeType.O15:

                    currentCulture = languageResourcesKey.GetValue("UILanguage").ToString();
                    newCulture = locCulture.Lcid.ToString();

                    if (newCulture != currentCulture)
                    {
                        closeSfbClientService.CloseSfbClient();
                        languageResourcesKey.SetValue("UILanguage", newCulture, RegistryValueKind.DWord);
                        languageResourcesKey.SetValue("HelpLanguage", newCulture, RegistryValueKind.DWord);
                        languageResourcesKey.SetValue("PreviousUI", currentCulture, RegistryValueKind.DWord);
                    }

                    var followSystemUi = languageResourcesKey.GetValue("FollowSystemUI");
                    if (followSystemUi == null || followSystemUi.ToString().ToLower() == "on")
                    {
                        languageResourcesKey.SetValue("FollowSystemUI", "off", RegistryValueKind.String);
                    }

                    break;

                case OfficeType.O16:
                    //get the previous culture code
                    currentCulture = languageResourcesKey.GetValue("UILanguageTag").ToString();
                    var newCultureLcid = locCulture.Lcid.ToString();
                    newCulture = locCulture.CultureName.ToString();
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Office\16.0\Common\LanguageResources");
                    if (key != null)
                    {
                        Object o = key.GetValue("FollowSystemUI");
                        if (o == null)
                        {
                            RegistryKey lk = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Office\16.0\Common\LanguageResources");
                            lk.SetValue("FollowSystemUI", "Off");
                        }
                    }

                    if (newCulture.ToLower() != currentCulture.ToLower())
                    {
                        closeSfbClientService.CloseSfbClient();
                        languageResourcesKey.SetValue("UILanguageTag", newCulture, RegistryValueKind.String);
                        languageResourcesKey.SetValue("HelpLanguageTag", newCulture, RegistryValueKind.String);
                        languageResourcesKey.SetValue("UIFallbackLanguages", newCulture, RegistryValueKind.String);
                        languageResourcesKey.SetValue("UILanguage", newCultureLcid, RegistryValueKind.DWord);
                        languageResourcesKey.SetValue("HelpLanguage", newCultureLcid, RegistryValueKind.DWord);
                        languageResourcesKey.SetValue("PreferredEditingLanguage", newCulture, RegistryValueKind.String);
                        languageResourcesKey.SetValue("PreviousUILanguage", currentCulture, RegistryValueKind.String);
                        languageResourcesKey.SetValue("PreviousPreferredEditingLanguage", currentCulture, RegistryValueKind.String);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
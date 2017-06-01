using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeLocFeedbackAndReviewBot.Services
{
    public class GetFamiliesProductsListService
    {
        //combination of family and product
        private readonly string[] familiesProducts;

        public GetFamiliesProductsListService()
        {
            familiesProducts = new string[]
            {
                @"Microsoft Teams\Microsoft Teams for Android",
                @"Microsoft Teams\Microsoft Teams for iOS",
                @"Microsoft Teams\Microsoft Teams for Mac",
                @"Microsoft Teams\Microsoft Teams for Web",
                @"Microsoft Teams\Microsoft Teams for Windows",
                @"Microsoft Teams\Microsoft Teams for Windows Phone",
                @"Skype Clients\Skype Click to Call",
                @"Skype Clients\Skype Desktop (Delphi)",
                @"Skype Clients\Skype Desktop Flex",
                @"Skype Clients\Skype for Linux",
                @"Skype Clients\Skype for Mac",
                @"Skype Clients\Skype for Windows Desktop (Threshold)",
                @"Skype Clients\Skype HoloLens Companion",
                @"Skype Clients\Skype Translator",
                @"Skype for Life\S4L",
                @"Skype for Life\S4L Android",
                @"Skype for Life\S4L iOS",
                @"Skype for Life\Electron (Desktop)",
                @"Skype for Life\Electron (Linux)",
                @"Skype Web Expirences\Skype for Web",
                @"Skype Lite \ M2 Android",
                @"Skype for Business Clients\Lync for Mac",
                @"Skype for Business Clients\Skype for Business Client (Dev15 PU)",
                @"Skype for Business Clients\Skype for Business Client (Dev16 PU)",
                @"Skype for Business Clients\Skype for Business Client (DevMain C2R)",
                @"Skype for Business Clients\Skype for Business Client (DevMain MonthlyRel)",
                @"Skype for Business Clients\Skype for Business for Mac",
                @"Skype for Business Clients\Skype for Business for PPI",
                @"Skype for Business Clients\SkypeCast"
            };
        }

        public string[] GetFamiliesProductsList()
        {
            return familiesProducts;
        }
    }
}
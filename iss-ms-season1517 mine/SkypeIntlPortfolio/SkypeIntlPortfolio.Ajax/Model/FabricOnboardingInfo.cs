using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class FabricOnboardingInfo
    {
        public string Product { get; set; }

        public string EpicLabel { get; set; }

        public string Core_Intl_Folder_Location { get; set; }

        public string Source_File_Path { get; set; }

        public bool EuropesOrKWTTAD_RW_Permission { get; set; }

        public DateTime? Expected_Date_for_Walking
        {
            get;
            set;
        }

        public DateTime? Expected_Date_for_Running
        {
            get;
            set;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    [Serializable]
    public class VacationRelatedInfo
    {
        public int VacationID { get; set; }
        public string VacationName { get; set; }
        public string VacationDescription { get; set; }
        public DateTime VacationStartDate { get; set; }
        public DateTime VacationEndDate { get; set; }

        //public List<string> ProductsAffected { get; set; }
        //public List<string> AffectedInfo { get; set; }
        public string ProductsAffected { get; set; }

        public string PeopleAffected { get; set; }

        public int UICategoryID { get; set; }
    }
}
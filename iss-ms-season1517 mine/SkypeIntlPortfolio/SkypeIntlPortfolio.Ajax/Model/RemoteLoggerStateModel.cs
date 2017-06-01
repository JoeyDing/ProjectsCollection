using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    [Serializable]
    public class RemoteLoggerStateOverviewModel {
            public string TestCase { get; set; }
            public string PassRate { get; set; }
    }
    [Serializable]
    public class RemoteLoggerStateModel
    {
        public int? Id { get; set; }
        public string TestcaseName { get; set; }
        public string LanguageName { get; set; }
        public List<string> ImagePath { get; set; }
        public bool? State { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ApplicationName { get; set; }
        public string BatchID { get; set; }

        public int? ExceptionID { get; set; }

        public string UserIdentity { get; set; }
    }


   
    
}
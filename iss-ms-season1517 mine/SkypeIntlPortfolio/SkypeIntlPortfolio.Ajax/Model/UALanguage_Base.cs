using SkypeIntlPortfolio.Ajax.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class UALanguage_Base
    {
        public int UALanguagesKey { get; set; }
        public Nullable<int> ProductKey { get; set; }

        [ImportIgnore]
        public int FileKey { get; set; }

        [ImportIgnore]
        public string LanguageName { get; set; }

        [ImportIgnore]
        public string LanguageIDPlusName { get; set; }

        public Nullable<int> LanguageKey { get; set; }

        [DBNameAttribute("Language Released")]
        public Nullable<bool> Language_Released { get; set; }

        [DBNameAttribute("Language Blocked")]
        public Nullable<bool> Language_Blocked { get; set; }

        [DBNameAttribute("Language Planned")]
        public Nullable<bool> Language_Planned { get; set; }

        [DBNameAttribute("Release Date")]
        public Nullable<System.DateTime> Release_Date { get; set; }

        [DBNameAttribute("Blocked Reason")]
        public string Blocked_Reason { get; set; }

        public Nullable<int> QualityID { get; set; }

        [ImportIgnore]
        public string QualityName { get; set; }
    }
}
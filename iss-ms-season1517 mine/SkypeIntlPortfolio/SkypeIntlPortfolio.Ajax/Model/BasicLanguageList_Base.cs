using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class BasicLanguageList_Base
    {
        public int LanguageKey { get; set; }
        public string Language { get; set; }
        public string CultureName { get; set; }

        public string LanguageIDPlusName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(CultureName))
                    return CultureName + @" \ " + Language;
                else
                    return Language;
            }
        }
    }
}
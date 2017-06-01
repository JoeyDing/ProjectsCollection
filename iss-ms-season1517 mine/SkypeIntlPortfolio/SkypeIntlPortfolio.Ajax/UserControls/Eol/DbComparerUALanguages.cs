using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class DbComparerUALanguages : IEqualityComparer<UALanguage_Base>
    {
        public bool Equals(UALanguage_Base x, UALanguage_Base y)
        {
            return x.LanguageKey == y.LanguageKey && x.ProductKey == y.ProductKey;
        }

        public int GetHashCode(UALanguage_Base obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.LanguageKey.GetHashCode();
            hash = hash * 31 + obj.ProductKey.GetHashCode();

            return hash;
        }
    }
}
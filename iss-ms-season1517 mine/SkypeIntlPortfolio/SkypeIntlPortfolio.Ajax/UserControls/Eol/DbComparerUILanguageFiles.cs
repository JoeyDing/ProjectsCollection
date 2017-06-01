using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class DbComparerUILanguageFiles : IEqualityComparer<UILanguageFiles_Base>
    {
        public bool Equals(UILanguageFiles_Base x, UILanguageFiles_Base y)
        {
            return x.FileKey == y.FileKey && x.UILanguagesKey == y.UILanguagesKey;
        }

        public int GetHashCode(UILanguageFiles_Base obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.FileKey.GetHashCode();
            hash = hash * 31 + obj.UILanguagesKey.GetHashCode();
            return hash;
        }
    }
}
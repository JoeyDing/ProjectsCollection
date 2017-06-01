using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class DbComparerUALanguageFiles : IEqualityComparer<UALanguageFiles_Base>
    {
        public bool Equals(UALanguageFiles_Base x, UALanguageFiles_Base y)
        {
            return x.FileKey == y.FileKey && x.UALanguagesKey == y.UALanguagesKey;
        }

        public int GetHashCode(UALanguageFiles_Base obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.FileKey.GetHashCode();
            hash = hash * 31 + obj.UALanguagesKey.GetHashCode();
            return hash;
        }
    }
}
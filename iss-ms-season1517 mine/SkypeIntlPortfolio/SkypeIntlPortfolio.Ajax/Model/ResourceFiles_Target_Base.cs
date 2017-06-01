using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class ResourceFiles_Target_Base
    {
        public int SourceFileKey { get; set; }
        public int TargetFileKey { get; set; }
        public int LanguageKey { get; set; }
        public string LanguageName { get; set; }
        public string CultureName { get; set; }
        public string File_Name { get; set; }
        public string Target_File_Location { get; set; }
        public string RepoURL { get; set; }
        public string repoBranch { get; set; }
        public string repoType { get; set; }
        public Nullable<int> Number_of_Strings__Fabric_ { get; set; }
    }
}
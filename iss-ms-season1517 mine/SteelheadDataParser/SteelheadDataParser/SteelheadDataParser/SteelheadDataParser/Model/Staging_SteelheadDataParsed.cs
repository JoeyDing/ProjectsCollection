//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SteelheadDataParser.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Staging_SteelheadDataParsed
    {
        public int SteelHeadDataParsedKey { get; set; }
        public string SymbolicName { get; set; }
        public string ParserIdentifier { get; set; }
        public Nullable<short> Revision { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public string Language { get; set; }
        public string ProjectName { get; set; }
        public string FileName { get; set; }
        public Nullable<System.DateTime> ResultDate { get; set; }
        public string Result { get; set; }
        public string ResultLoggedBy { get; set; }
        public Nullable<int> BugNumber { get; set; }
    }
}

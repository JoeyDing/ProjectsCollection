//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkypeIntlMonitoring.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tool
    {
        public Tool()
        {
            this.LogFiles = new HashSet<LogFile>();
            this.SqlJobSteps = new HashSet<SqlJobStep>();
        }
    
        public int ToolID { get; set; }
        public string Name { get; set; }
        public string LocalWorkingDirectory { get; set; }
        public string NetworkWorkingDirectory { get; set; }
    
        public virtual ICollection<LogFile> LogFiles { get; set; }
        public virtual ICollection<SqlJobStep> SqlJobSteps { get; set; }
    }
}

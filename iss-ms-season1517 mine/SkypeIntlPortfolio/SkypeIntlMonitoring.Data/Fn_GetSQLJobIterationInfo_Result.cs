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
    
    public partial class Fn_GetSQLJobIterationInfo_Result
    {
        public System.Guid JobID { get; set; }
        public string JobName { get; set; }
        public Nullable<int> run_status { get; set; }
        public Nullable<int> step_id { get; set; }
        public string step_name { get; set; }
        public string last_outcome_message { get; set; }
        public Nullable<int> run_date { get; set; }
        public Nullable<int> run_time { get; set; }
    }
}

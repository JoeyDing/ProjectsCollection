//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VsoWorkItemsSync.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestSuiteRevision
    {
        public long ID { get; set; }
        public int Rev { get; set; }
        public string Work_Item_Type { get; set; }
        public string Title { get; set; }
        public string Assigned_To { get; set; }
        public string State { get; set; }
        public string Tags { get; set; }
        public string Area_Path { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public string Changed_By { get; set; }
        public Nullable<System.DateTime> Changed_Date { get; set; }
        public string Iteration_Path { get; set; }
        public string Team_Project { get; set; }
        public string Reason { get; set; }
        public string DevSignoff { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Revised_Date { get; set; }
        public string Test_Suite_Type { get; set; }
        public string Test_Suite_Audit { get; set; }
        public string Query_Text { get; set; }
        public string Resolved_Reason { get; set; }
    }
}

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
    
    public partial class TestRunResult
    {
        public long Test_Run_ID { get; set; }
        public long Test_Plan_ID { get; set; }
        public long Test_Case_ID { get; set; }
        public string Test_Run_Name { get; set; }
        public string Test_Outcome { get; set; }
        public string Tester_Display_Name { get; set; }
        public string Tester_Unique_Name { get; set; }
        public Nullable<System.DateTime> Start_Date { get; set; }
        public Nullable<System.DateTime> Completed_Date { get; set; }
    }
}

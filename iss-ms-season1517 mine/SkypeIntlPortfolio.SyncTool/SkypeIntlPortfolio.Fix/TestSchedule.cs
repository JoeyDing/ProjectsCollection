//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkypeIntlPortfolio.Fix
{
    using System;
    using System.Collections.Generic;
    
    public partial class TestSchedule
    {
        public int ProductKey { get; set; }
        public int ReleaseKey { get; set; }
        public int TestScheduleKey { get; set; }
        public string TestSchedule_Name { get; set; }
        public Nullable<System.DateTime> TestSchedule_Start_Date { get; set; }
        public Nullable<System.DateTime> TestSchedule_End_Date { get; set; }
        public string Vso_Web_Url { get; set; }
        public Nullable<int> AssignedResources { get; set; }
        public Nullable<int> MilestoneCategoryKey { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> VSO_ChangedDate { get; set; }
    
        public virtual MilestoneCategory MilestoneCategory { get; set; }
        public virtual Release Release { get; set; }
    }
}

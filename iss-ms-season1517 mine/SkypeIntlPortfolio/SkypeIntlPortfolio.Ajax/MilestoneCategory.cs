//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkypeIntlPortfolio.Ajax
{
    using System;
    using System.Collections.Generic;
    
    public partial class MilestoneCategory
    {
        public MilestoneCategory()
        {
            this.Milestones = new HashSet<Milestone>();
            this.TestSchedules = new HashSet<TestSchedule>();
        }
    
        public int MilestoneCategoryKey { get; set; }
        public string Milestone_Category_Name { get; set; }
    
        public virtual ICollection<Milestone> Milestones { get; set; }
        public virtual ICollection<TestSchedule> TestSchedules { get; set; }
    }
}

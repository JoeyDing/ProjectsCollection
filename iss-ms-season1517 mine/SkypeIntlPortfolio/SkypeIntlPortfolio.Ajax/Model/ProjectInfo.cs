using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    [Serializable]
    public class ProductInfo
    {
        //product
        public string Product_Name { get; set; }

        public int ProductKey { get; set; }

        //public string User_Voice { get; set; }

        public string Family { get; set; }

        //public string Product_Description { get; set; }

        public string Loc_PM_Location { get; set; }

        //public string Core_Team_Location { get; set; }

        //public string PM_Alias { get; set; }

        //public string Resource_File_Path { get; set; }

        //public string Core_Team_Contacts { get; set; }

        //public string Epic_Label { get; set; }

        //public string Fabric_Status { get; set; }

        //public bool Fabric_Onboarding_Request { get; set; }

        public ReleaseInfo[] Releases { get; set; }
    }

    [Serializable]
    public class ReleaseInfo
    {
        //release
        public int ReleaseKey { get; set; }

        public int ProductKey { get; set; }

        public string Release_Name { get; set; }

        public DateTime? Release_Start_Date { get; set; }

        public DateTime? Release_End_Date { get; set; }

        public string Release_Assigned_To { get; set; }

        public string Release_Tags { get; set;}

        public int MilestoneKey { get; set; }

        public MilestoneInfo[] Milestones { get; set; }

        public string Release_Url { get; set; }

        public bool IsHidden { get; set; }
        public TestScheduleInfo[] TestSchedules { get; set; }
    }

    [Serializable]
    public class MilestoneInfo
    {
        //milestone

        public int ProductKey { get; set; }

        public int ReleaseKey { get; set; }

        public int MilestoneKey { get; set; }

        public int MilestoneCategoryKey { get; set; }

        public string Milestone_Name { get; set; }

        public string Milestone_Assigned_To { get; set; }

        public DateTime? Milestone_Start_Date { get; set; }

        public DateTime? Milestone_End_Date { get; set; }

        public string MilestoneCategoryName { get; set; }

        //not used
        public string RecurrenceRule { get; set; }

        public string RecurrenceParentId { get; set; }
    }

    [Serializable]
    public class TestScheduleInfo
    {
        public int ProductKey { get; set; }
        public int ReleaseKey { get; set; }
        public int TestScheduleKey { get; set; }
        public string TestScheduleName { get; set; }
        public DateTime? TestScheduleStartDate { get; set; }
        public DateTime? TestScheduleEndDate { get; set; }
        public string TestScheduleUrl { get; set; }
        public int AssignedResources { get; set; }

        public int MilestoneCategoryKey { get; set; }

        //not used
        public string RecurrenceRule { get; set; }

        public string RecurrenceParentId { get; set; }
    }
}
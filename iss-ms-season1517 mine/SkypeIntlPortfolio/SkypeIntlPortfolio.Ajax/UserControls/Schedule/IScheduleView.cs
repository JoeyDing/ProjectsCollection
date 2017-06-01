using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Schedule
{
    public interface IScheduleView
    {
        // key is expanded product key,which is used for databinding
        Dictionary<int, List<GetReleaseOfProduct_Result>> ReleaseOfProduct_Result { get; set; }

        // key is expanded release key
        Dictionary<int, List<GetMilestoneOfRelease_Result>> MilestoneOfRelease_Result { get; set; }

        // key is expanded release key
        Dictionary<int, List<GetTestPlanOfRelease_Result>> TestPlanOfRelease_Result { get; set; }

        List<Products_New> ProductsList { get; set; }
        Dictionary<int, string> SelectedPruductFromManagments { get; set; }
        List<MilestoneCategory> MilestoneCategoryList { get; set; }

        int TotalReleases { get; set; }
        int TotalTestSchedules { get; set; }
        int TotalMileStones { get; set; }

        List<EspecInfo> ESpecList { get; set; }

        //used for databinding
        event Func<int, int, int, List<GetReleaseOfProduct_Result>> GetReleases;

        event Func<int, Release> GetReleasesByReleaseKey;

        event Func<int, List<Milestone>> GetMilestonesByReleaseKey;

        event Func<int, int, int, List<GetMilestoneOfRelease_Result>> GetMileStones;

        event Func<int, int, int, List<GetTestPlanOfRelease_Result>> GetTestPlans;

        //event Action<Release, int, string> InsertRelease;

        event Func<Release, int, string, int> InsertRelease;

        event Action<List<List<MilestoneInfoFromModal>>, int> InsertMilestone;

        event Action<List<List<TestScheduleInfoFromModal>>, int> InsertTestPlan;

        event Action<Release> UpdateRelease;

        event Action<MilestoneInfoFromModal> UpdateMilestone;

        event Action<TestSchedule> UpdateTestSchedule;

        event Action<int> DeleteRelease;

        event Action<int> DeleteMilestone;

        event Action<int> DeleteTestSchedule;

        event Func<int, int> GetTotalReleases;

        event Func<int, int> GetTotalTestSchedules;

        event Func<int, int> GetTotalMileStones;

        event Func<List<MilestoneCategory>> GetMilestoneCategory;

        event Func<string, string, List<string>> GetEspecList;

        //event Func<int, TestSchedule> GetTestScheduleByKey;

        event Func<int, Milestone> GetMilestoneByKey;

        //event Func<int, string> GetProductFamily;

        event Func<string, List<string>> GetCustomTagsByReleaseTag;

        event Func<List<int>, bool> IsProductCancelled;
    }
}
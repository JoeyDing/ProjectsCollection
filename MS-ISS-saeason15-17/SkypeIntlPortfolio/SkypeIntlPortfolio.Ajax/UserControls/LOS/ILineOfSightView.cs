using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.LOS
{
    public interface ILineOfSightView
    {
        event Func<List<int>> GetAllProductKeys;

        event Func<int, VacationInfo> GetVacationInfoesByID;

        event Func<int, Dictionary<string, string>> GetAffectedProductsByVacID;

        event Func<int, List<int>> GetAffectedLocationsIDsByVacationID;

        event Func<IEnumerable<string>, Dictionary<int, string>> GetProductsWithCheckedLocations;

        event Func<List<VacationRelatedInfo>> GetVacationRelatedInfoList;

        event Action<Milestone, string> UpdateMileStone;

        event Action<Release> UpdateRelease;

        event Action<TestSchedule, string> UpdateTestPlan;

        event Action<VacationInfo, IEnumerable<int>, List<int>> UpdateVacation;

        event Action<Milestone> UpdateMileStoneForDragAndDrop;

        event Action<Release> UpdateReleaseForDragAndDrop;

        event Action<TestSchedule> UpdateTestPlanForDragAndDrop;

        event Action<VacationInfo> UpdateVacationForDragAndDrop;
    }
}
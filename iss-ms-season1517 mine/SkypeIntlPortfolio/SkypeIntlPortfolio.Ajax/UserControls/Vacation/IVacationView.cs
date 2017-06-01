using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Vacation
{
    public interface IVacationView
    {
        event Action<VacationInfo, List<int>, List<int>> InsertVacationInfo;

        event Action<VacationInfo, List<int>, List<int>> UpdateVacationInfo;

        event Action<int> DeleteVacationInfo;

        event Func<int, Dictionary<string, string>> GetVacationAffectedProductsByVacationID;

        event Func<List<int>> GetAllProducts;

        event Func<int, List<int>> GetAffectedLocationsIDsByVacationID;

        event Func<List<string>, List<AffectedInfo>> GetAffectedPeopleByProductIDs;

        event Func<HashSet<string>, List<string>, List<string>> GetUpdatedAffectedPeopleByProductIDs;

        event Func<Dictionary<int, string>, List<string>, List<AffectedInfo>> GetAffectedProductsByPeopleNames;
    }
}
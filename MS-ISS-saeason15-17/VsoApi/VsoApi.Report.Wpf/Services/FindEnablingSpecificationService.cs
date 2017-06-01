using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Report.Wpf.Contracts;
using VsoApi.Report.Wpf.Data;

namespace VsoApi.Report.Wpf.Services
{
    public class FindEnablingSpecificationService : IFindEnablingSpecification
    {
        public IEnumerable<VsoItemResult> GetEnablingSpecifications(long[] ids)
        {
            var result = new List<VsoItemResult>();
            using (var context = new SkypeVsoWorkItemsEntities())
            {
                var tasks = context.EnablingSpecificationRevisions
                    .Where(c => ids.Contains(c.ID))
                    .GroupBy(c => c.ID)
                    .Select(g => g.OrderByDescending(c => c.Rev).FirstOrDefault()).ToList();

                string urlFormat = "https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_workitems?id={0}&_a=edit";
                foreach (var item in tasks)
                {
                    var vsoItem = new VsoItemResult
                    {
                        ID = item.ID,
                        Rev = item.Rev,
                        ParentID = null,
                        Title = item.Title,
                        VsoType = item.Work_Item_Type,
                        CompletedWork = null,
                        RemainingWork = null,
                        EstimatedWork = null,
                        AssignedTo = item.Assigned_To,
                        State = item.State,
                        VsoUrl = string.Format(urlFormat, item.ID),
                    };
                    result.Add(vsoItem);
                }
            }
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Report.Wpf.Contracts;
using VsoApi.Report.Wpf.Data;

namespace VsoApi.Report.Wpf.Services
{
    public class FindTaskService : IFindTask
    {
        private readonly IFindItemLink findLinkService;
        private readonly IFindEnablingSpecification findEsService;

        public FindTaskService(IFindItemLink findLinkService, IFindEnablingSpecification findEsService)
        {
            this.findLinkService = findLinkService;
            this.findEsService = findEsService;
        }

        public IEnumerable<VsoItemResult> GetTasksByParent(long parentID)
        {
            var result = new List<VsoItemResult>();
            var links = findLinkService.GetChildrenInfos(parentID);

            if (links.Count() > 0)
            {
                var parent = this.findEsService.GetEnablingSpecifications(new long[] { parentID }).First();
                using (var context = new SkypeVsoWorkItemsEntities())
                {
                    var childrenIDs = links.ToDictionary(c => c.Target_ID, c => c.Source_ID);
                    var tasks = context.TaskRevisions
                        .Where(c => childrenIDs.Keys.Contains(c.ID))
                        .GroupBy(c => c.ID)
                        .Select(g => g.OrderByDescending(c => c.Rev).FirstOrDefault()).ToList();

                    string urlFormat = "https://skype.visualstudio.com/DefaultCollection/LOCALIZATION/_workitems?id={0}&_a=edit";
                    foreach (var item in tasks)
                    {
                        var vsoItem = new VsoItemResult
                        {
                            ID = item.ID,
                            Rev = item.Rev,
                            ParentID = childrenIDs[item.ID],
                            ParentTitle = parent.Title,
                            Title = item.Title,
                            VsoType = item.Work_Item_Type,
                            CompletedWork = item.Completed_Work,
                            RemainingWork = item.Remaining_Work,
                            EstimatedWork = item.Original_Estimate,
                            AssignedTo = item.Assigned_To,
                            State = item.State,
                            VsoUrl = string.Format(urlFormat, item.ID),
                        };
                        result.Add(vsoItem);
                    }
                }
            }
            return result;
        }
    }
}
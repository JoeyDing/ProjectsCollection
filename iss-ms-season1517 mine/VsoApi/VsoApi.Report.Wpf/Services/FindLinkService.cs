using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Report.Wpf.Contracts;
using VsoApi.Report.Wpf.Data;

namespace VsoApi.Report.Wpf.Services
{
    public class FindLinkService : IFindItemLink
    {
        public IEnumerable<WorkItemLink> GetChildrenInfos(long parentId)
        {
            List<WorkItemLink> result = null;
            using (var context = new SkypeVsoWorkItemsEntities())
            {
                var allRelationInfo = context.Database.SqlQuery<WorkItemLink>(string.Format(@"
                            SELECT x.[Source ID] Source_ID
                                  ,x.[Target ID] Target_ID
                                  ,x.[Link Type] Link_Type
                                  ,x.[Is Active] Is_Active
                                  ,x.[Changed Date] Changed_Date
                              FROM [SkypeVsoWorkItems].[dbo].[WorkItemLinks] x
                              join
                              (SELECT [Source ID]
                                  ,[Target ID]
                            	  ,MAX([Changed Date])[Changed Date]
                            	   FROM [WorkItemLinks]
                            	   group by [Source ID],[Target ID]) y
                               on  x.[Source ID] = y.[Source ID]
                               and x.[Target ID] = y.[Target ID]
                               and x.[Changed Date] = y.[Changed Date]
                               where x.[Source ID] = {0}", parentId)).ToList();

                result = allRelationInfo.Where(c => c.Link_Type == "System.LinkTypes.Hierarchy" && c.Is_Active).ToList();
            }

            return result;
        }

        public int GetParentInfo(int childrenid)
        {
            throw new NotImplementedException();
        }
    }
}
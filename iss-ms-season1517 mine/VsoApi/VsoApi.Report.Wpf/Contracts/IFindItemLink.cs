using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Report.Wpf.Data;

namespace VsoApi.Report.Wpf.Contracts
{
    public interface IFindItemLink
    {
        IEnumerable<WorkItemLink> GetChildrenInfos(long parentId);

        int GetParentInfo(int childrenid);
    }
}
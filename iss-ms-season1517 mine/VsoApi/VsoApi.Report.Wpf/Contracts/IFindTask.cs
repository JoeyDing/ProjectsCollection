using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Report.Wpf
{
    public interface IFindTask
    {
        IEnumerable<VsoItemResult> GetTasksByParent(long parentID);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Core
{
    public interface IRevisionItem
    {
        long ID { get; }

        int Rev { get; }
    }
}
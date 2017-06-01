using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.Core
{
    public class WorkItemAttachmentComparer : IEqualityComparer<WorkItemAttachment>
    {
        public bool Equals(WorkItemAttachment x, WorkItemAttachment y)
        {
            return x.WorkItemID == y.WorkItemID && x.AttachmentUrl == y.AttachmentUrl;
        }

        public int GetHashCode(WorkItemAttachment obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.WorkItemID.GetHashCode();
            hash = hash * 31 + obj.AttachmentUrl.GetHashCode();
            return hash;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Core.Exception
{
    public class SyncException : ExceptionBase
    {
        public SyncException(List<string> errors) : base(errors)
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Core.Exception
{
    public abstract class ExceptionBase : System.Exception
    {
        public IReadOnlyCollection<string> InnerExceptionsList { get; set; }

        public ExceptionBase(List<string> errors)
        {
            this.InnerExceptionsList = errors;
        }
    }
}
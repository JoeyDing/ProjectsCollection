using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Core.Exception
{
    internal class TypeMappingException : ExceptionBase
    {
        public TypeMappingException(List<string> errors) : base(errors)
        {
        }
    }
}
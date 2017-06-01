using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace ExtensibleVSOAPI
{
    public class FetchParamsInfo
    {
        public TaskType TaskType { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
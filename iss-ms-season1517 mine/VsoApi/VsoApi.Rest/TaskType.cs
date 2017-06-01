using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Rest
{
    public class TaskType
    {
        public string Value { get; set; }

        public TaskType(string value)
        {
            this.Value = value;
        }
    }
}
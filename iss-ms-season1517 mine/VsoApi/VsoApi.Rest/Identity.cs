using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Rest
{
    public class Identity
    {
        public string ID { get; set; }

        public string DisplayName { get; set; }

        public string UniqueName { get; set; }

        public string AssignedTo { get; set; }
    }
}
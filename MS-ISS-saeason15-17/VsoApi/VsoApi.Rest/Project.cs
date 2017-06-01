using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Rest
{
    public class Project
    {
        public string Description { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string ApiUrl { get; set; }

        public string WebUrl { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoApi.Rest
{
    public class LinkType
    {
        public string Value { get; set; }

        public LinkType(string value)
        {
            this.Value = value;
        }
    }
}
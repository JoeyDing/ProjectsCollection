using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public class JsonEndPoint
    {
        public string JsonQueryUrl { get; set; }

        public string HeaderContent { get; set; }

        public List<BodyFormContent> FormContent { get; set; }

        public string Token { get; set; }

        public int? TimeOut { get; set; }

        public RequestType RequestType { get; set; }

        public CustomLib CustomLib { get; set; }

        public string ResultTemplate { get; set; }

        public string ResultItemTemplate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public class Field
    {
        public string JsonFieldName { get; set; }

        public string SqlColumnName { get; set; }

        public bool IsIdentity { get; set; }
    }
}
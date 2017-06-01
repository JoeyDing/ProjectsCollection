using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    [Serializable]
    public class Mapping
    {
        public string SqlTableName { get; set; }

        public List<Field> Fields { get; set; }

        public DataSaveType.SaveType SaveType { get; set; }

        public bool CleanOnSchemaChange { get; set; }
    }
}
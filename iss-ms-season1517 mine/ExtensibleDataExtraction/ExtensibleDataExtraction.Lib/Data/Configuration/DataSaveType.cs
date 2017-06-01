using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Configuration
{
    public class DataSaveType
    {
        public enum SaveType
        {
            Incremental,
            Full
        };
    }
}
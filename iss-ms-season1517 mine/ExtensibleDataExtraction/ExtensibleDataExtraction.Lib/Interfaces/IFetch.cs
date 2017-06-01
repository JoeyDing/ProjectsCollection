using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleDataExtraction.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Interfaces
{
    public interface IFetch
    {
        string FetchData(ExtensibleItem param, ExtensibleDbEntity extensibleDbEntity);
    }
}
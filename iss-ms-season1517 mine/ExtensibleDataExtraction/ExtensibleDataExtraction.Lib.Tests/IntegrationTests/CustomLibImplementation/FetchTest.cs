using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ExtensibleDataExtraction.Lib.Interfaces;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Services;
using ExtensibleDataExtraction.Lib.Data.Sql;

namespace ExtensibleDataExtraction.Lib.Tests.IntegrationTests.CustomLibImplementation
{
    public class FetchTest : IFetch
    {
        public string FetchData(ExtensibleItem param, ExtensibleDbEntity extensibleDbEntity)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"json.json");
            string jsonString;
            using (StreamReader sr = new StreamReader(configPath))
            {
                jsonString = sr.ReadToEnd();
            }
            return jsonString;
        }
    }
}
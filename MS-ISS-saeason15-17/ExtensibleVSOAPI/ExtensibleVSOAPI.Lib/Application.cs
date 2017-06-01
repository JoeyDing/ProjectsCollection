using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleVSOAPI
{
    public class Application
    {
        public static void Main(string[] args)
        {
            //Replace this section with Fetch method
            ConfigurationSerializerService service = new ConfigurationSerializerService();
            var context = new ExtensibleContext("VSOAPI.log");
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
            var extensibleConfig = service.GetExtensibleConfigFromConfig(configPath);
            List<ExtensibleItem> extensibleItem = extensibleConfig.Items.Items;
            foreach (var item in extensibleItem)
            {
                if (item.Mapping.SaveType.Equals("Full"))
                    context.logger.LogException(new Exception("Value of SaveType must be Incremental"));
            }
            context.StartProcess(configPath);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace VsoWorkItemsSync.Core.Configuration
{
    public class ConfigurationLoader
    {
        public ConfigurationLoader()
        {
        }

        public ConfigurationMappings Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(string.Format("Cannot find Configuration File at path: {0}", path));
            var result = this.Deserialize<ConfigurationMappings>(path);
            return result;
        }

        private T Deserialize<T>(string path) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result = null;
            using (XmlReader reader = XmlReader.Create(path))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
    }
}
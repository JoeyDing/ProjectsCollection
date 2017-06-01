using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core
{
    public class DeserializeService : ICanDeserialize
    {
        public T Deserialize<T>(string filePath) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result = null;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                result = (T)serializer.Deserialize(fileStream);
            }
            return result;
        }

        public object Deserialize(string filePath, Type T)
        {
            XmlSerializer serializer = new XmlSerializer(T);
            object result = null;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                result = serializer.Deserialize(fileStream);
            }
            return result;
        }
    }
}
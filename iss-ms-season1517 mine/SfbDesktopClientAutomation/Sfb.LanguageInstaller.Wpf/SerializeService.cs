using Sfb.LanguageInstaller.Wpf.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sfb.LanguageInstaller.Wpf
{
    internal class SerializeService : ICanSerialize
    {
        public void Serialize(string fileName, object item)
        {
            var serializer = new XmlSerializer(item.GetType());
            using (var writer = new StreamWriter(fileName, false))
            {
                serializer.Serialize(writer, item);
            }
        }
    }
}
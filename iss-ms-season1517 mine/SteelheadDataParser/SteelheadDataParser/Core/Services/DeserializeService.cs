using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SteelheadDataParser.Services
{
    public static class DeserializeService
    {
        public static T Deserialize<T>(string xmlString) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result = null;
            using (Stream fileStream = GenerateStreamFromString(xmlString))
            {
                result = (T)serializer.Deserialize(fileStream);
            }
            return result;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
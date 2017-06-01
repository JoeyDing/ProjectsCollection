using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiFxMappingParser.Reader
{
    public class CultureReader
    {
        public string Read(string filePath)
        {
            filePath = Path.GetFileNameWithoutExtension(filePath);
            var split = filePath.Split('\\');
            for (int i = split.Length - 1; i >= 0; i--)
            {
                var part = split[i];
                if (part.Contains("_"))
                    return part.Split('_')[1];
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser
{
    public interface IDeserialize
    {
        T Deserialize<T>(string xmlString) where T : class;
    }
}
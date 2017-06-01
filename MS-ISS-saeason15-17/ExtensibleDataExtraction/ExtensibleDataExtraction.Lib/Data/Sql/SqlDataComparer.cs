using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Sql
{
    internal class SqlDataComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] x, string[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }
            return true;
        }

        public int GetHashCode(string[] obj)
        {
            int allHashCode = 0;
            for (int i = 0; i < obj.Length; i++)
            {
                if (obj[i] != null)
                {
                    allHashCode += obj[i].GetHashCode();
                }
            }
            return allHashCode;
        }
    }
}
using SteelheadDataParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.Core
{
    public class SteelHeadDataComparer : IEqualityComparer<Staging_SteelheadDataParsed>
    {
        public bool Equals(Staging_SteelheadDataParsed x, Staging_SteelheadDataParsed y)
        {
            return x.SymbolicName == y.SymbolicName && x.ParserIdentifier == y.ParserIdentifier
                   && x.Revision == y.Revision && x.Deleted == y.Deleted && x.Language == y.Language
                   && x.ProjectName == y.ProjectName && x.FileName == y.FileName;
        }

        public int GetHashCode(Staging_SteelheadDataParsed obj)
        {
            int hash = 23;
            hash = hash * 31 + obj.SymbolicName.GetHashCode();
            hash = hash * 31 + obj.ParserIdentifier.GetHashCode();
            hash = hash * 31 + obj.Revision.GetHashCode();
            hash = hash * 31 + obj.Deleted.GetHashCode();
            hash = hash * 31 + obj.Language.GetHashCode();
            hash = hash * 31 + obj.ProjectName.GetHashCode();
            hash = hash * 31 + obj.FileName.GetHashCode();
            return hash;
        }
    }
}
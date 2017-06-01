using SteelheadDataParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelheadDataParser.Core
{
    public class SteelHeadDataComparer : IEqualityComparer<Staging_FabricBackup_SteelheadDataParsed>
    {
        public bool Equals(Staging_FabricBackup_SteelheadDataParsed x, Staging_FabricBackup_SteelheadDataParsed y)
        {
            return x.SymbolicName == y.SymbolicName && x.ParserIdentifier == y.ParserIdentifier
                   && x.Revision == y.Revision && x.Deleted == y.Deleted && x.Language == y.Language
                   && x.ProjectName == y.ProjectName && x.FileName == y.FileName
            && x.ResultDate == y.ResultDate && x.Result == y.Result
            && x.ResultLoggedBy == y.ResultLoggedBy && x.BugNumber == y.BugNumber
            && x.ResourceIdentity == y.ResourceIdentity;
        }

        public int GetHashCode(Staging_FabricBackup_SteelheadDataParsed obj)
        {
            if (obj != null)
            {
                int hash = 23;
                hash = hash * 31 + obj.SymbolicName.GetHashCode();
                hash = hash * 31 + obj.ParserIdentifier.GetHashCode();
                hash = hash * 31 + obj.Revision.GetHashCode();
                hash = hash * 31 + obj.Deleted.GetHashCode();
                hash = hash * 31 + obj.Language.GetHashCode();
                hash = hash * 31 + obj.ProjectName.GetHashCode();
                hash = hash * 31 + obj.FileName.GetHashCode();
                hash = hash * 31 + obj.ResultDate.GetHashCode();
                hash = hash * 31 + obj.Result.GetHashCode();
                hash = hash * 31 + obj.ResultLoggedBy.GetHashCode();
                hash = hash * 31 + (obj.BugNumber == null ? 0 : obj.BugNumber.GetHashCode());
                hash = hash * 31 + obj.ResourceIdentity.GetHashCode();
                return hash;
            }
            else
            {
                return 0;
            }
        }
    }
}
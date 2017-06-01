using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Sql
{
    public class ExtensibleDbEntity : DbContext
    {
        public ExtensibleDbEntity(string connectionString)
            : base(connectionString)
        {
        }

        static ExtensibleDbEntity()
        {
            var type = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, ensures static reference to System.Data.Entity.SqlServer");
        }
    }
}
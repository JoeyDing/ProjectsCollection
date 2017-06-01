using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Model
{
    public partial class VwsContext
    {
        public VwsContext(string connectionString) : base(BuildConnectionString(connectionString))
        {
        }

        private static string BuildConnectionString(string connectionString)
        {
            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder =
            new SqlConnectionStringBuilder();

            sqlBuilder.DataSource = "skypeintl";
            sqlBuilder.InitialCatalog = "SkypeVsoWorkItems";
            sqlBuilder.IntegratedSecurity = true;
            // Set the properties for the data source.
            //sqlBuilder.ConnectionString = connectionString;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
            new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = "System.Data.SqlClient";

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"metadata=res://*/Model.VsoWorkItemsModel.csdl|res://*/Model.VsoWorkItemsModel.ssdl|res://*/Model.VsoWorkItemsModel.msl";

            string result = entityBuilder.ToString();
            //return result;

            var conn = @"metadata=res://*/Model.VsoWorkItemsModel.csdl|res://*/Model.VsoWorkItemsModel.ssdl|res://*/Model.VsoWorkItemsModel.msl;provider=System.Data.SqlClient;provider connection string='data source=skypeintl;initial catalog=SkypeVsoWorkItems;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework'";
            return conn;
        }
    }
}
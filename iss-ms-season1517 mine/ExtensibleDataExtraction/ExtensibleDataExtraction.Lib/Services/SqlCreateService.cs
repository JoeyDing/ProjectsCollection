using ExtensibleDataExtraction.Lib.Data.Processing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib.Data.Sql
{
    public class SqlCreateService
    {
        public void CreateTable(string sqlTableName, string[] fields, DbContext dbContext)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("CREATE TABLE [dbo].[{0}](", sqlTableName));
            //add identity column
            builder.AppendLine("[ID] [int] IDENTITY(1,1) NOT NULL,");

            //add other columns
            var fieldsSql = fields.Select(c => string.Format("[{0}] [nvarchar](255) NULL", c))
                    .Aggregate((a, b) => a + "," + Environment.NewLine + b);
            builder.AppendLine(fieldsSql);

            //add primary key
            builder.AppendLine(string.Format(",CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED (ID ASC)", sqlTableName));

            builder.AppendLine(")ON [PRIMARY]");

            //run create query
            var createTableQuery = builder.ToString();

            dbContext.Database.ExecuteSqlCommand(createTableQuery);
        }

        public void CreateMissingColumns(string sqlTableName, string[] fields, DbContext dbContext)
        {
            var expectedColumns = new HashSet<string>(fields, StringComparer.InvariantCultureIgnoreCase);

            var currentColumns = dbContext.Database.SqlQuery<string>(
                string.Format(@"select COLUMN_NAME
                                from INFORMATION_SCHEMA.COLUMNS
                                where TABLE_NAME='{0}'", sqlTableName)).ToList();

            expectedColumns.ExceptWith(currentColumns);

            if (expectedColumns.Any())
            {
                var sb = new StringBuilder();

                sb.AppendLine(string.Format("ALTER TABLE {0} ADD ", sqlTableName));

                var columnsString = expectedColumns
                    .Select(c => string.Format("{0} NVARCHAR(MAX)", c))
                    .Aggregate((a, b) => a + "," + b);
                sb.AppendLine(columnsString);

                string command = sb.ToString();
                dbContext.Database.ExecuteSqlCommand(command);
            }
        }

        public void EmptyTable(string sqlTableName, DbContext dbContext)
        {
            string command = string.Format("TRUNCATE TABLE {0} ", sqlTableName);
            dbContext.Database.ExecuteSqlCommand(command);
        }

        public void DropColumns(ExtensibleDbEntity dbContext, ExtensibleDataSet dataSet, string[] fields)
        {
            var columnsList = dbContext.Database.SqlQuery<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CountryLanguageCount'").ToList();
            List<string> distinctList = columnsList.Except(fields.ToList()).ToList();
            distinctList.Remove("ID");
            if (dataSet.Mapping.CleanOnSchemaChange == true && distinctList != null && fields != null)
                foreach (var column in distinctList)
                    dbContext.Database.ExecuteSqlCommand(String.Format("ALTER TABLE CountryLanguageCount DROP COLUMN {0}", column));
        }
    }
}
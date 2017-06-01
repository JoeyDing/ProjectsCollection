using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Core.Interface
{
    public interface IBulkInsert
    {
        void InsertData_Bulk<T>(SkypeIntlPlanningPortfolioEntities dbContext, IEnumerable<T> newItems, string destinationTableName);
    }
}
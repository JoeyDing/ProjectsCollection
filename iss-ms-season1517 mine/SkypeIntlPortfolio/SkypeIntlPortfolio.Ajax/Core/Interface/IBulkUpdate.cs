using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Core.Interface
{
    public interface IBulkUpdate
    {
        void UpdateData_Bulk<T>(SkypeIntlPlanningPortfolioEntities dbContext, IEnumerable<T> list, string TableName, string primaryKey, string[] fieldsToUpdate);
    }
}
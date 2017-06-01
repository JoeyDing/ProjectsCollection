using SkypeIntlPortfolio.Ajax.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.Core.Interface
{
    public interface IVacationInfo
    {
        List<VacationRelatedInfo> GetVacationRelatedInfoList();
    }
}
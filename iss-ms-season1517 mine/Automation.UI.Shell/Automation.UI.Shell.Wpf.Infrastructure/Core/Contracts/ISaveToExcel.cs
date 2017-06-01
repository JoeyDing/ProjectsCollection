using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts
{
    public interface ISaveToExcel
    {
        void SaveToExcel(string buildVersion, DateTime startDate, DateTime endDate, List<TestResult> data, string filePath);
    }
}
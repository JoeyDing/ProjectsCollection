using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts
{
    public interface ILogStatusHistory
    {
        string LogFileName { get; set; }

        void UpdateHistory(History currentHistory, HistoryStatus status, string msg);

        void AddHistoryGroup(IEnumerable<History> historyList);

        ObservableCollection<History> GetStatusHistory();

        ObservableCollection<History> ReadLogFromFile();

        string FormatMsg(DateTime timeStamp, HistoryStatus status, string msg);
    }
}
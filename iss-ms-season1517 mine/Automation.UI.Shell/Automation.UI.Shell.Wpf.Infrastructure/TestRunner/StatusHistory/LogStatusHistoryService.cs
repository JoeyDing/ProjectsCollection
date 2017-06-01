using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.StatusHistory.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Automation.UI.Shell.Wpf.Infrastructure.TestRunner
{
    public class LogStatusHistoryService : ILogStatusHistory
    {
        private ObservableCollection<History> statusHistoryList;
        private string _logFileName = "";

        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        public void UpdateHistory(History currentHistory, HistoryStatus status, string msg)
        {
            History history = this.statusHistoryList.Where(hh => hh.Guid.Equals(currentHistory.Guid)).FirstOrDefault();
            history.RunTime = DateTime.Now;
            history.Msg += FormatMsg(DateTime.Now, status, msg);
            history.Status = status;
            SaveLogToFile();
        }

        public void AddHistoryGroup(IEnumerable<History> historyList)
        {
            foreach (var history in historyList)
            {
                this.statusHistoryList.Add(history);
            }
        }

        public string FormatMsg(DateTime timeStamp, HistoryStatus status, string msg)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Time Stamp:{0},Status:{1}{2}", timeStamp, status, new string('-', 20)));
            sb.AppendLine(msg);
            return sb.ToString();
        }

        private void SaveLogToFile()
        {
            // Serialize the list to a file
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenWrite(this.LogFileName))
            {
                serializer.Serialize(stream, this.statusHistoryList);
            }
        }

        public ObservableCollection<History> ReadLogFromFile()
        {
            ObservableCollection<History> lp = new ObservableCollection<History>();

            if (File.Exists(this.LogFileName))
            {
                // Deserialize the list from a file
                var serializer = new BinaryFormatter();
                using (var stream = File.OpenRead(this.LogFileName))
                {
                    if (stream != null && stream.Length != 0)
                    {
                        lp = (ObservableCollection<History>)serializer.Deserialize(stream);
                    }
                }
            }
            this.statusHistoryList = lp;
            return lp;
        }

        public ObservableCollection<History> GetStatusHistory()
        {
            return this.statusHistoryList;
        }
    }
}
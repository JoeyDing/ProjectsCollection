using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.JobStatus
{
    public interface IJobStatusView
    {
        List<New_SqlJobModel> JobList { get; set; }

        event Func<DateTime, DateTime, List<New_SqlJobModel>> GetNewJobTree;

        event Func<string, DateTime, DateTime, List<New_SqlJobRecordModel>> GetJobRecords;

        event Func<string, int, DateTime, List<New_SqlJobStepModel>> GetStepStatus;
    }
}
using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;

namespace SkypeIntlPortfolio.Ajax.UserControls.RunningJobs
{
    public interface IRunningJobsView
    {
        event Func<DateTime> GetServerTime;
        event Func<DateTime, List<NewRunningJobModel>> GetRunningJobs;
        event Func<int, List<New_RuningStepsInPeriodModel>> GetRuningStepsInPeriod;
        event Func<string, List<New_LiveJobStepModel>> GetJobLiveStepStatus;
    }
}
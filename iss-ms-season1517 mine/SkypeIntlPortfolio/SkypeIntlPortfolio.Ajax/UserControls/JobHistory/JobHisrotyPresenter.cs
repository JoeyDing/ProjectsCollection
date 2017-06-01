using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkypeIntlPortfolio.Ajax.UserControls.JobHistory
{
    public class JobHistoryPresenter
    {
        private IJobHistoryView _jobHistoryView;

        public JobHistoryPresenter(IJobHistoryView jobHistoryView)
        {
            this._jobHistoryView = jobHistoryView;
            this._jobHistoryView.GetNewJobTree += _jobStatusView_GetNewJobList;
            this._jobHistoryView.GetJobRecords += _jobHistoryView_GetJobRecords;
            this._jobHistoryView.GetStepStatus += _jobHistoryView_GetStepStatus;
        }

        private List<New_SqlJobModel> _jobStatusView_GetNewJobList(DateTime startDate, DateTime endDate)
        {
            //Show default tab
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var newJobList = entities.New_SqlJobs
                .Select(a => new New_SqlJobModel
                {
                    //Get Job List from New_SqlJobModel Table
                    JobName = a.JobName
                })
                .OrderBy(a => a.JobName)
                .ToList();
                return newJobList;
            }
        }

        private List<New_SqlJobRecordModel> _jobHistoryView_GetJobRecords(string jobName, DateTime startTime, DateTime endTime)
        {
            //query the job runing records for job
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var newJobRecordList = entities.Fn_New_GetJobsExecution(jobName, startTime, endTime)
                    .Select(a => new New_SqlJobRecordModel
                    {
                        JobInstanceID = a.instance_id,
                        JobRunStatus = a.run_status,
                        JobRunDateTime = (DateTime)a.RunDateTime,
                        JobDurationRunTime = (int)a.RunDurationMinutes,
                        JobOutMessage = (a.message.Substring(0, 19) == "The job is succeed.") ? "" : a.message
                    })
                    .OrderByDescending(a => a.JobRunDateTime)
                    .ToList();
                return newJobRecordList;
            }
        }

        private List<New_SqlJobStepModel> _jobHistoryView_GetStepStatus(string jobName, int jobInstanceId, DateTime jobStartTime)
        {
            DateTime oldTime = Convert.ToDateTime("1900-1-1");
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var stepStausList =
                    entities.Fn_New_GetJobStepsforExecution(jobName, jobInstanceId, jobStartTime)
                    .Select(s => new New_SqlJobStepModel
                    {
                        JobStepName = s.step_name,
                        JobStepID = s.step_id,
                        StepRunStatus = (s.run_status == 4) ? 2 : s.run_status, //When the status is 4, it means progress. Due them as successed.
                        RunDateTime = (s.RunDateTime == null) ? oldTime : (DateTime)s.RunDateTime,
                        RunDurationTime = (int)s.RunDurationMinutes,
                        Failed_Outcome_Message = s.message
                    }
                    )
                    .ToList();

                var results = (from ssi in stepStausList
                               group ssi by new { ssi.JobStepID, ssi.StepRunStatus } into s
                               select new New_SqlJobStepModel
                               {
                                   JobStepName = s.FirstOrDefault().JobStepName,
                                   JobStepID = s.Key.JobStepID,
                                   StepRunStatus = s.Key.StepRunStatus,
                                   RunDateTime = s.FirstOrDefault().RunDateTime,
                                   RunDurationTime = s.Sum(n => n.RunDurationTime),
                                   Failed_Outcome_Message = String.Join("<br> ", s.Select(n => n.Failed_Outcome_Message))
                               }
                               ).ToList();


                return results;
            }
        }

    }
}
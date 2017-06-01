using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkypeIntlPortfolio.Ajax.UserControls.JobStatus
{
    public class JobStatusPresenter
    {
        private IJobStatusView _jobStatusView;

        public JobStatusPresenter(IJobStatusView jobStatusView)
        {
            this._jobStatusView = jobStatusView;
            this._jobStatusView.GetNewJobTree += _jobHistoryView_GetNewJobTree;
            this._jobStatusView.GetJobRecords += _jobStatusView_GetJobRecords;
            this._jobStatusView.GetStepStatus += _jobStatusView_GetStepStatus;
        }

        private List<New_SqlJobModel> _jobHistoryView_GetNewJobTree(DateTime startDate, DateTime endDate)
        {
            //Show default tab
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var newJobList = entities.Fn_New_GetJobsLatestStatus(startDate, endDate).ToList()
                .Select(a => new New_SqlJobModel
                {
                    //Get Job List from New_SqlJobModel Table
                    JobName = a.job_name,
                    JobLatestRunTime = (DateTime)a.LatestRunDateTime,

                    //if latest status is failed, shows red. status=0
                    //If all status don't have failed, shows green. status=1
                    //If latest status is succeed. But has failed in history, shows yellow. status=5
                    JobStatus = (a.LatestRunStatus == 0) ? 0 : ((a.PreviouslyFailed == 1) ? 5 : 1),

                    JobLatestDurationRunTime = (int)a.LatestRunDurationMinutes,

                    JobLatestRunFinishTime = a.LatestRunDateTime.Value.Add(TimeSpan.FromSeconds(a.LatestRunDurationMinutes))
                })
                .OrderBy(a => a.JobName)
                .ToList();
                return newJobList;
            }
        }

        private List<New_SqlJobRecordModel> _jobStatusView_GetJobRecords(string jobName, DateTime startTime, DateTime endTime)
        {
            //query the job runing records for job
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                return entities.Fn_New_GetJobsExecution(jobName, startTime, endTime)
                    .Select(a => new New_SqlJobRecordModel
                    {
                        JobInstanceID = a.instance_id,
                        JobRunStatus = a.run_status,
                        JobRunDateTime = (DateTime)a.RunDateTime,
                        JobDurationRunTime = (int)a.RunDurationMinutes,
                        JobOutMessage = (a.message.Substring(0, 18) == "The job succeeded.") ? "" : a.message
                    })
                    .OrderByDescending(a => a.JobRunDateTime)
                    .ToList();
                //return entities.vw_GetJobExecutionView
                //    .Where(a => a.job_name == jobName && a.RunDateTime > startTime)
                //    .Select(a => new New_SqlJobRecordModel
                //    {
                //        JobInstanceID = a.instance_id,
                //        JobRunStatus = a.run_status,
                //        JobRunDateTime = (DateTime)a.RunDateTime,
                //        JobDurationRunTime = (int)a.RunDurationMinutes,
                //        JobOutMessage = (a.message.Substring(0, 18) == "The job succeeded.") ? "" : a.message
                //    })
                //    .OrderByDescending(a => a.JobRunDateTime)
                //    .ToList();
            }
        }

        private List<New_SqlJobStepModel> _jobStatusView_GetStepStatus(string jobName, int jobInstanceId, DateTime jobStartTime)
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
                    .OrderByDescending(a => a.RunDateTime)
                    .ToList();
                var results = (from ssi in stepStausList
                               group ssi by new { ssi.JobStepID, ssi.StepRunStatus } into s
                               select new New_SqlJobStepModel
                               {
                                   JobStepName = s.FirstOrDefault().JobStepName,
                                   JobStepID = s.Key.JobStepID,
                                   StepRunStatus = s.Key.StepRunStatus,
                                   RunDateTime = s.FirstOrDefault().RunDateTime,
                                   RunDurationTime = s.FirstOrDefault().RunDurationTime,
                                   Failed_Outcome_Message = String.Join("<br> ", s.Select(n => n.Failed_Outcome_Message))
                               }
                ).ToList();

                return results;
            }
        }
    }
}
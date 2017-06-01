using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SkypeIntlPortfolio.Ajax.Model.Monitor;
using System.Data.Entity;

namespace SkypeIntlPortfolio.Ajax.UserControls.RunningJobs
{
    public class RunningJobsPresenter
    {
        private IRunningJobsView _runningJobsView;

        public RunningJobsPresenter(IRunningJobsView runningJobsView)
        {
            this._runningJobsView = runningJobsView;
            this._runningJobsView.GetRunningJobs += _runningJobsView_GetRunningJobs;
            this._runningJobsView.GetRuningStepsInPeriod += _runningJobsView_GetRuningStepsInPeriod;
            this._runningJobsView.GetServerTime += _runningJobsView_GetServerTime;
            this._runningJobsView.GetJobLiveStepStatus += _runningJobsView_GetJobLiveStepStatus;
        }



        private DateTime _runningJobsView_GetServerTime()
        {
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var tempTime = entities.Fn_New_GetServerTime()
                    .FirstOrDefault();
                return Convert.ToDateTime(tempTime);
            }
        }

        private List<NewRunningJobModel> _runningJobsView_GetRunningJobs(DateTime ServerTime)
        {
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var newJobList = entities.Fn_New_GetRunningJobs()
                    .Select(a => new NewRunningJobModel
                    {
                        JobName = a.job_name,
                        JobStartTime = (DateTime)a.start_execution_date,
                        CurrentStepId = (int)a.current_executed_step_id,
                        CurrentStepName = a.step_name,
                        JobRunningTime = (int)DbFunctions.DiffSeconds((DateTime)a.start_execution_date, ServerTime)
                    })
                .OrderByDescending(a => a.JobName)
                .ToList();
                return newJobList;
            }
        }

        private List<New_RuningStepsInPeriodModel> _runningJobsView_GetRuningStepsInPeriod(int selectIndex)
        {
            int timePeriod;
            switch (selectIndex)
            {
                case 1:
                    timePeriod = -20;
                    break;

                case 2:
                    timePeriod = -30;
                    break;

                case 3:
                    timePeriod = -60;
                    break;

                case 4:
                    timePeriod = -120;
                    break;

                default:
                    timePeriod = -10;
                    break;
            }

            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var newStepsList = entities.Fn_New_GetStepInPeriod(timePeriod)
                    .Select(a => new New_RuningStepsInPeriodModel
                    {
                        JobName = a.name,
                        StepRunStatus = a.run_status,
                        JobStepName = a.step_name,
                        JobStepID = a.step_id,
                        RunDateTime = (DateTime)a.RunDateTime,
                        RunDurationTimeSeconds = (int)a.RunDurationTimeSeconds,
                    })
                .ToList();

                var results = (from ssi in newStepsList
                               group ssi by new { ssi.JobStepID, ssi.StepRunStatus, ssi.JobName } into s
                               select new New_RuningStepsInPeriodModel
                               {
                                   JobName = s.Key.JobName,
                                   StepRunStatus = (s.Key.StepRunStatus == 0) ? 0 : ((s.Key.StepRunStatus == 1 || s.Key.StepRunStatus == 4 || s.Key.StepRunStatus == 2) ? 1 : s.Key.StepRunStatus),
                                   JobStepName = s.FirstOrDefault().JobStepName,
                                   JobStepID = s.Key.JobStepID,
                                   RunDateTime = s.FirstOrDefault().RunDateTime,
                                   RunDurationTimeSeconds = s.FirstOrDefault().RunDurationTimeSeconds,
                                   RunEndTime = s.FirstOrDefault().RunDateTime.AddSeconds(Convert.ToDouble(s.FirstOrDefault().RunDurationTimeSeconds))
                               }
                )
                .OrderByDescending(a => a.RunEndTime)
                .ToList();

                return results;
            }
        }

        private List<New_LiveJobStepModel> _runningJobsView_GetJobLiveStepStatus(string JobName)
        {
            using (var entities = new SkypeIntlMonitoringEntities())
            {
                var tempTime = entities.Fn_New_GetStepForRunningJob(JobName)
                     .Select(a => new New_LiveJobStepModel
                     {
                         JobStepName = a.step_name,
                         JobStepID = a.step_id,
                         StartTime = (DateTime)a.RunDateTime,
                         RunDurationTime = (int)a.RunDurationTimeSeconds
                     })
                     .OrderByDescending(a=>a.JobStepID)
                     .ToList();
                return tempTime;
            }
        }
    }
}
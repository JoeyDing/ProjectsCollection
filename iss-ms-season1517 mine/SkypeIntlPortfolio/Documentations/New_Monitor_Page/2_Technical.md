Technical 
=======

### Summary

>This document is used to show the technical point in monitor page.

### Framework

All the data comes though Entity framework.
UI depends on telerik.ajax.
Design Model is MVC

### SQL function

All the data in job status tab and job history tab is real time data. Get by the function in DB. 

Those seven functions are used to get data:

>Fn_New_GetJobsLatestStatus
>Fn_New_GetJobsExecution
>Fn_New_GetJobStepsforExecution
>Fn_New_GetJobStepsForNotification
>Fn_New_GetJobListForHistory
>Fn_New_GetRunningJobs
>Fn_New_GetStepInPeriod

Jobs and steps data comes form three tables :  
msdb.dbo.sysjobs_view, msdb.dbo.sysjobhistory, msdb.dbo.sysjobs

Function of get steps for one job run record is special
the function code is as below:

```sql
ALTER function [dbo].[Fn_New_GetJobsExecution](@jobName nvarchar(250),@fromDate datetime,@endDate datetime)
returns table as return (
 With sjht as(SELECT sjh.instance_id,sjh.job_id,sjh.run_date,sjh.run_time,sjh.run_status,sjh.run_duration,nt.JobName
    FROM [msdb].[dbo].[sysjobhistory] sjh
    JOIN 
    (SELECT sj.job_id, sj.name AS JobName From msdb.dbo.sysjobs sj WHERE sj.name = @jobName) nt
    ON sjh.job_id =nt.job_id
    WHERE sjh.run_date>= convert(varchar(10),@fromDate, 112)
    AND step_id =0) 

SELECT * FROM 
(SELECT sj.message,sj.run_status,sj.instance_id, j.JobName as job_name,
((run_duration/10000*3600 + (run_duration/100)%100*60 + run_duration%100 + 31 ) / 60) 
         as 'RunDurationMinutes',
   msdb.dbo.agent_datetime(run_date, run_time) as 'RunDateTime' 
   FROM msdb.dbo.sysjobhistory sj

Join (SELECT sjht.instance_id,sjht.JobName FROM sjht where sjht.JobName = @jobName) j
ON sj.instance_id = j.instance_id   ) mess


WHERE mess.RunDateTime > @fromDate and mess.RunDateTime <DATEADD(day,1,@endDate))
```

This function depend on the DBStructure of sysjobhistory

![DBSysJobHistory](image/DBSysJobHistory.JPG)
 
 All the steps and jobs are shows in this table. 
 Jobs records showss '0' in 'step ID' coloumn. Steps show step oreder id in 'step ID' coloumn.
 We use 'jobId' column to judge steps and job records belong to which job.

**All job run record started with the step which stepID is 1, end with record which stepID is 0**

stepID is 1 means it's the first step of the job. stepID is 0 means it's the jobself record.
all the steps which time later than step 1 and instanceID less than the jobself belone to this job.


Function Fn_New_GetStepInPeriod has special logic.
When page input a period time, function return two parts of data
First, the finished steps in the running jobs.
Second, if there are any jobs finished in the selected time period, shows all step for those jobs running records.
The Function union all the two part of data.

### SQL view

Tool relationships use view vw_ToolsJobsStepsInfo to get real time relationship. 


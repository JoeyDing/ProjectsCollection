using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger
{
    public class RemoteLoggerPresenter
    {
        private IRemoteLoggerView _remoteLoggerView;
        private static SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities();

        public RemoteLoggerPresenter(IRemoteLoggerView remoteLoggerView)
        {
            this._remoteLoggerView = remoteLoggerView;
            this._remoteLoggerView.GetApplicationList += _remoteLoggerView_GetApplicationList;
            this._remoteLoggerView.GetBatchTimeList += _remoteLoggerView_GetBatchTimeList;
            this._remoteLoggerView.GetStateList += _remoteLoggerView_GetStateList;
            this._remoteLoggerView.GetUserList += _remoteLoggerView_GetUserList;
            this._remoteLoggerView.GetImageList += _remoteLoggerView_GetImagePath;
            this._remoteLoggerView.GetStateOverviewList += _remoteLoggerView_GetStateOverviewList;
            this._remoteLoggerView.GetStateOverviewDetailList += _remoteLoggerView_GetStateOverviewDetailList;
        }

        private List<Model.RemoteLoggerStateModel> _remoteLoggerView_GetStateOverviewDetailList(string batchID, string testcaseName)
        {
            var query = from s in dbContext.RemoteStateLoggers
                        join ex in dbContext.RemoteLoggers on s.Id equals ex.StateID
                        into temp
                        from ex in temp.DefaultIfEmpty()
                        where s.BatchID == batchID && s.TestcaseName == testcaseName
                        select new Model.RemoteLoggerStateModel()
                        {
                            Id = s.Id,
                            TestcaseName = s.TestcaseName,
                            LanguageName = s.LanguageName,
                            State = s.State,
                            UpdateDate = s.UpdateDate,
                            ApplicationName = s.ApplicationName,
                            ExceptionID = ex.Id,
                            UserIdentity = s.UserIdentity,
                        };
            var testPassStates = query.ToList().OrderBy(c => c.Id).ToList();
            return testPassStates;
        }

        private List<Model.RemoteLoggerStateOverviewModel> _remoteLoggerView_GetStateOverviewList(string batchID)
        {
            var lstTestcaseName = dbContext.RemoteStateLoggers.Where(r => r.BatchID == batchID).GroupBy(r => r.TestcaseName).OrderBy(g => g.FirstOrDefault().Id).Select(group => group.Key).ToList();
            List<Model.RemoteLoggerStateOverviewModel> lstOverview = new List<Model.RemoteLoggerStateOverviewModel>();
            foreach (var testcaseName in lstTestcaseName)
            {
                int total = dbContext.RemoteStateLoggers.Where(r => r.BatchID == batchID && r.TestcaseName == testcaseName).Count();
                int success = dbContext.RemoteStateLoggers.Where(r => r.BatchID == batchID && r.TestcaseName == testcaseName && r.State == true).Count();
                Model.RemoteLoggerStateOverviewModel m = new Model.RemoteLoggerStateOverviewModel();
                m.TestCase = testcaseName;
                m.PassRate = ((float)success / (float)total * 100).ToString("0.00") + " %";
                lstOverview.Add(m);
            }
            return lstOverview;
        }

        private List<RemoteStateLoggerImage> _remoteLoggerView_GetImagePath(int remoteStateLoggerId)
        {
            var query = from s in dbContext.RemoteStateLoggerImages
                        where s.RemoteStateLoggerID == remoteStateLoggerId
                        select s;

            List<RemoteStateLoggerImage> images = query.Distinct().ToList();
            return images;
        }

        private List<DropDownModel> _remoteLoggerView_GetUserList(string appName)
        {
            var queryuser = (dbContext.RemoteStateLoggers.Where(r => r.ApplicationName == appName).Select(c => c.UserIdentity).Distinct().Select(c => new DropDownModel
            {
                DataValueField = c,
                DataTextField = c
            })).ToList();
            return queryuser;
        }

        private List<Model.RemoteLoggerStateModel> _remoteLoggerView_GetStateList(string batchID)
        {
            var query = from s in dbContext.RemoteStateLoggers
                        join ex in dbContext.RemoteLoggers on s.Id equals ex.StateID
                        into temp
                        from ex in temp.DefaultIfEmpty()
                        where s.BatchID == batchID
                        select new Model.RemoteLoggerStateModel()
                        {
                            Id = s.Id,
                            TestcaseName = s.TestcaseName,
                            LanguageName = s.LanguageName,
                            State = s.State,
                            UpdateDate = s.UpdateDate,
                            ApplicationName = s.ApplicationName,
                            ExceptionID = ex.Id,
                            UserIdentity = s.UserIdentity,
                        };
            var testPassStates = query.ToList().OrderBy(c => c.Id).ToList();
            return testPassStates;
        }

        private List<DropDownModel> _remoteLoggerView_GetBatchTimeList(string appName, string userAcc)
        {
            var queryBatch = from s in dbContext.RemoteStateLoggers
                             where s != null && s.ApplicationName == appName && s.UserIdentity == userAcc
                             group s by s.BatchID into g
                             let f = g.FirstOrDefault()
                             orderby f.UpdateDate descending
                             select new DropDownModel()
                             {
                                 DataValueField = g.Key,
                                 DataTextField = g.Max(c => c.UpdateDate).ToString() + " - " + g.Min(c => c.UpdateDate).ToString() + "(" + g.Count() + ")"
                             };

            var batchGroup = queryBatch.ToList();
            return batchGroup;
        }

        private List<string> _remoteLoggerView_GetApplicationList()
        {
            return dbContext.RemoteStateLoggers.Select(r => r.ApplicationName).Distinct().ToList();
        }
    }
}
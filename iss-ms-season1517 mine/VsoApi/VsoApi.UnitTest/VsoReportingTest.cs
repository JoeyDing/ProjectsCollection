using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace VsoApi.UnitTest
{
    [TestClass]
    public class VsoReportingTest
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        private VsoContext GetContext()
        {
            string vsoPrivateKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            var context = new VsoContext("skype-test2", vsoPrivateKey);
            return context;
        }

        private VsoContext GetProdContext()
        {
            string vsoPrivateKey = ConfigurationManager.AppSettings["ProdVsoPrivateKey"];
            var context = new VsoContext("skype", vsoPrivateKey);
            return context;
        }

        private VsoContext GetProdContextForDomoreexp()
        {
            string vsoPrivateKey = ConfigurationManager.AppSettings["ProdVsoPrivateKey"];
            var context = new VsoContext("domoreexp", vsoPrivateKey);
            return context;
        }

        [TestMethod]
        public void VsoReportingTest_GetAllBugsRevisions()
        {
            var context = this.GetContext();
            var result = context.Reporting_GetAllBugsRevisions("LOCALIZATION");
        }

        [TestMethod]
        public void VsoReportingTest_GetWorkItemRevByIdAndRev()
        {
            int id = 183235;
            int rev = 12;
            var context = this.GetProdContext();
            var result = context.GetWorkItemRevByIdAndRev(id, rev);
        }

        [TestMethod]
        public void VsoReportingTest_GetWorkItemRevById()
        {
            int id = 183235;

            var context = this.GetProdContext();
            var result = context.GetWorkItemRevById(id);
        }

        [TestMethod]
        public void VsoReportingTest_GetDictWorkItem()
        {
            int id = 183235;
            var context = this.GetProdContext();
            var result = context.GetDictRevToTagsByID(id);
        }

        [TestMethod]
        public void VsoReportingTest_GetAllRelations()
        {
            var context = this.GetProdContext();

            var result = context.Reporting_GetAllRelations("LOCALIZATION");
        }

        [TestMethod]
        public void VsoReportingTest_GetAllWorkItemsRevisions_Epics()
        {
            var context = this.GetProdContext();

            var result = context.Reporting_GetAllWorkItemRevisions("LOCALIZATION", TaskTypes.Epic, null, 1);
        }

        [TestMethod]
        public void VsoReportingTest_GetDictAllWorkItemsRevisions_EnablingSpecifications()
        {
            var context = this.GetProdContext();
            string[] fieldsToQuery =
            {
                "System.ID",
                "System.WorkItemType",
                "System.Title",
                "System.AssignedTo",
                "System.State",
                "System.Tags",
                "System.AreaPath",
                "Microsoft.VSTS.Common.ResolvedBy",
                "Microsoft.VSTS.Common.ResolvedDate",
                "System.ChangedBy",
                "System.ChangedDate",
                "Microsoft.VSTS.Common.ClosedBy",
                "System.CreatedBy",
                "System.CreatedDate",
                "Microsoft.VSTS.Common.ClosedDate",
                "Microsoft.VSTS.Scheduling.DueDate",
                "System.IterationPath",
                "Skype.Priority",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "Skype.LocalizationImpact",
                "Skype.WorldReadinessImpact",
                "Skype.Initiator",
                "Skype.ReviewRequired",
                "Skype.Keywords",
                "Skype.Blocking",
                "Skype.EstimatePoints",
                "Skype.EstimateTShirt",
                "Skype.Release",
                "Skype.EngineeringEffort"
            };
            var result = context.Reporting_GetDictAllWorkItemRevisions_FromDate("LOCALIZATION", TaskTypes.EnablingSpecification, DateTime.Today.AddDays(-14), fieldsToQuery, 1);
        }

        [TestMethod]
        public void VsoReporting_TestCreateTestPlan()
        {
            var context = this.GetProdContext();
            context.CreateTestPlan("LOCALIZATION", "newCreatedPlan", @"LOCALIZATION\_Trash_", @"LOCALIZATION\_Trash_", DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        public void VsoReportingTest_GetAllWorkItemsRevisions_TestSuite()
        {
            var context = this.GetProdContext();
            string[] fieldsToQuery =
            {
                "System.ID",
                "System.WorkItemType",
                "System.Title",
                "System.AssignedTo",
                "System.State",
                "System.Tags",
                "System.AreaPath",
                "System.CreatedBy",
                "System.CreatedDate",
                "System.ChangedBy",
                "System.ChangedDate",
                "System.IterationPath",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "System.RevisedDate",
                "Microsoft.VSTS.TCM.TestSuiteAudit",
                "Microsoft.VSTS.TCM.QueryText"
            };
            var result = context.Reporting_GetAllWorkItemRevisions("LOCALIZATION", TaskTypes.TestSuite, fieldsToQuery, 1);
        }

        [TestMethod]
        public void VsoReportingTest_GetAllWorkItemsRevisionsByFromDate_TestSuite()
        {
            var context = this.GetProdContext();
            string[] fieldsToQuery =
            {
                "System.ID",
                "System.WorkItemType",
                "System.Title",
                "System.AssignedTo",
                "System.State",
                "System.Tags",
                "System.AreaPath",
                "System.CreatedBy",
                "System.CreatedDate",
                "System.ChangedBy",
                "System.ChangedDate",
                "System.IterationPath",
                "System.TeamProject",
                "System.Reason",
                "System.Description",
                "Skype.DevSignoff",
                "System.RevisedDate",
                "Microsoft.VSTS.TCM.TestSuiteAudit",
                "Microsoft.VSTS.TCM.QueryText"
            };
            DateTime fromDate = DateTime.Today.AddDays(-16);
            var result = context.Reporting_GetAllWorkItemRevisionsFromDate("LOCALIZATION", TaskTypes.TestSuite, fromDate);
        }

        [TestMethod]
        public void VsoReportingTest_GetAllWorkItemsRevisionsByFromDate_EnablingSpecification()
        {
            var context = this.GetProdContext();
            string[] fieldsToQuery =
            {
                "System.Tags"
            };
            DateTime fromDate = DateTime.Today.AddDays(-16);
            var result = context.Reporting_GetAllWorkItemRevisions("LOCALIZATION", TaskTypes.EnablingSpecification, fieldsToQuery, 1);
        }

        [TestMethod]
        public void VsoReportngTest_GetTestPlanAndTestSuiteByTestCaseId()
        {
            var context = this.GetProdContext();
            string project = "LOCALIZATION";
            int testcaseId = 244632;
            var result = context.GetTestPlanAndTestSuiteByTestCaseId(project, testcaseId);
        }

        [TestMethod]
        public void VSORest_Reporting_GetTestRunByitemsPerBatch()
        {
            VsoContext context = GetProdContext();
            string project = "LOCALIZATION";
            var json = context.GetTestRunByitemsPerBatchAndSleepTime(project, 40, 1);
        }

        [TestMethod]
        public void VSORest_Reporting_GetTestRunByTestPlanId()
        {
            VsoContext context = GetProdContext();
            string project = "LOCALIZATION";
            var testResultJSON = context.GetTestRunResultByPlanID(project, 257965);
        }

        [TestMethod]
        public void VSORest_Reporting_CloneTestCases2()
        {
            var context = GetProdContext();
            var testcaseIDsFromQUery = new List<int>() { 246576, 246577, 246578 };
            context.CloneTestSuiteInsidePlan2(143834, "LOCALIZATION", 348277, new string[] { "Cherokee" }, new string[] { "348282", "348284" }, testcaseIDsFromQUery);
        }

        [TestMethod]
        public void VSORest_GetListOfTestCaseBySuiteId()
        {
            VsoContext context = GetProdContext();
            var result = context.GetTestCaseIDsBySuiteID("LOCALIZATION", 319094, 319095);
        }

        [TestMethod]
        public void VSoRest_RunQuery()
        {
            var wiql = @"SELECT [System.Id], [System.WorkItemType], [System.Title], [System.State], [System.AreaPath], [System.IterationPath], [System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project and [System.WorkItemType] = 'Bug' and [Skype.ResourceID] = '123' ORDER BY [System.ChangedDate] DESC";
            //var wiql = @"select [System.Id], [System.WorkItemType], [System.Title], [Microsoft.VSTS.Common.Priority], [System.AssignedTo], [System.AreaPath] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] in group 'Microsoft.TestCaseCategory' and [System.AreaPath] under 'LOCALIZATION\Skype Mobile\Skype for Windows Phone (Threshold)' and [System.Title] contains 'functionality' ";
            var context = GetProdContext();
            var result = context.RunQuery("LOCALIZATION", wiql);
        }

        [TestMethod]
        public void VSORest_GetQueryTextByQueryID()
        {
            string queryID = "2a1a6dce-32be-4268-b77d-092c98de9594";
            //string queryID = "ss";
            var context = GetProdContext();
            string result = context.GetQueryTextByQueryID(queryID);
        }

        [TestMethod]
        public void VSoRest_Reporting_RunComplexQuery()
        {
            var wiql = "SELECT [System.Id], [System.State], [Microsoft.VSTS.Common.ResolvedReason], [Skype.Triaged], [System.CreatedDate], [System.Title], [System.AssignedTo], [Skype.IssueType], [Skype.Language], [Skype.HowFound], [Microsoft.VSTS.Common.Priority], [Microsoft.VSTS.Common.Severity], [System.AreaPath], [System.IterationPath], [Skype.Source], [System.Tags] FROM WorkItems WHERE [System.WorkItemType] = 'Bug' and [System.CreatedBy] = 'Defect Prevention <skdevlog@microsoft.com>' and ([Skype.WorldReadinessImpact] = 'Yes' or [Skype.LocalizationImpact] = 'Yes') ORDER BY [System.Id] DESC";
            var context = GetProdContext();
            var result = context.RunComplexQuery(null, wiql);
        }

        [TestMethod]
        public void VSoRest_Reporting_RunComplexQuery2()
        {
            var context = GetProdContext();
            var wiql = "SELECT [System.Id],[System.WorkItemType],[System.Title],[System.AssignedTo],[System.State],[System.Tags] FROM WorkItemLinks WHERE ([Source].[System.WorkItemType] = 'Bug' AND [Source].[System.State] <> '' AND [Source].[System.IterationPath] NOT UNDER 'LOCALIZATION') AND ([System.Links.LinkType] = 'System.LinkTypes.Related-Forward') AND ([Target].[System.WorkItemType] = 'Bug' AND [Target].[System.IterationPath] UNDER 'LOCALIZATION') ORDER BY [System.Title],[System.Id] mode(MustContain)";
            var result = context.RunCrossProjectQuery("LOCALIZATION", wiql);
        }

        [TestMethod]
        public void VSORest_GetListOfWorkItemsByIDs_AllFields()
        {
            //epic id
            var ids = new int[] { 939226, 935892, 935867, 915936, 915873 };

            var context = GetProdContext();
            var result = context.GetListOfWorkItemsByIDs(ids);
        }

        [TestMethod]
        public void VsoReportingTest_GetSprintData()
        {
            var context = new VsoContext("skype", "dcdli5jw6xteqs43ojlzlucoiy53tywckvyxckgmqtqdjybxd6gq");
            string project = "LOCALIZATION";
            var iterations = new string[]
                {
                 //@"LOCALIZATION\Intl Tools Backlog\Intl Tools S56",
                 @"LOCALIZATION\Intl Tools Backlog\Intl Tools S60",
                 @"LOCALIZATION\Intl Tools Backlog\Intl Tools S61",
                };
            var data = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (var iteration in iterations)
            {
                var items = new Dictionary<string, List<string>>();
                data.Add(iteration, items);

                string query = @"select [System.Id] from WorkItems where [System.TeamProject] = '" + project + @"' and ([System.WorkItemType] = 'Enabling Specification' or [System.WorkItemType] = 'Bug' or [System.WorkItemType] = 'Impediment') and [System.AreaPath] UNDER 'LOCALIZATION\Intl Tools' and [System.IterationPath] = '" + iteration + "' AND ( [System.AssignedTo] = 'Alimjan Mettursun <v-almett@microsoft.com>' OR [System.AssignedTo] = 'Ning Li <v-ninli@microsoft.com>' OR [System.AssignedTo] = 'Haoni Li <v-haonli@microsoft.com>' OR [System.AssignedTo] = 'Joey Ding <v-joding@microsoft.com>' OR [System.AssignedTo] = 'Yan Huang <v-huangy@microsoft.com>' OR [System.AssignedTo] = 'Gopal Aryal <v-goarya@microsoft.com>' OR [System.AssignedTo] = 'Rickas Razafison <v-riraza@microsoft.com>' )";
                var ids = context.RunQuery(project, query);

                var result = context.GetListOfWorkItemsByIDs(ids.Keys);
                foreach (var pbi in result["value"])
                {
                    var pbiTitle = (string)pbi["fields"]["System.Title"];
                    string parentTitle = "Others";
                    JToken parent = null;
                    if (pbi["relations"] != null && (parent = pbi["relations"].FirstOrDefault(i => (string)i["rel"] == "System.LinkTypes.Hierarchy-Reverse")) != null)
                    {
                        string parentID = ((string)parent["url"]).Split(new char[] { '/' }).Last();
                        parentTitle = (string)context.GetListOfWorkItemsByIDs(new int[] { int.Parse(parentID) })["value"][0]["fields"]["System.Title"];
                    }

                    if (!items.ContainsKey(parentTitle))
                    {
                        var child = new List<string>() { pbiTitle };
                        items.Add(parentTitle, child);
                    }
                    else
                    {
                        var child = items[parentTitle];
                        child.Add(pbiTitle);
                    }
                }
            }
            var sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.AppendLine(item.Key);
                sb.AppendLine();
                foreach (var epic in item.Value)
                {
                    sb.AppendLine(epic.Key);
                    foreach (var pbi in epic.Value)
                    {
                        sb.AppendLine(string.Format("	{0}", pbi));
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
                sb.AppendLine();
            }

            var str = sb.ToString();
        }

        public class Task
        {
            public string Title { get; set; }

            public int? CompletedHours { get; set; }

            public int? EstimatedHours { get; set; }
        }

        [TestMethod]
        public void VsoReportingTest_GetSprintHours()
        {
            var context = new VsoContext("skype", "dcdli5jw6xteqs43ojlzlucoiy53tywckvyxckgmqtqdjybxd6gq");
            string project = "LOCALIZATION";
            var iterations = new string[]
                {
                 @"LOCALIZATION\Intl Tools Backlog\Intl Tools S46",
                 @"LOCALIZATION\Intl Tools Backlog\Intl Tools S47",
                 @"LOCALIZATION\Intl Tools Backlog\Intl Tools S48",
                };
            var data = new Dictionary<string, Dictionary<string, List<Task>>>();
            foreach (var iteration in iterations)
            {
                var items = new Dictionary<string, List<Task>>();
                data.Add(iteration, items);

                string query = @"select [System.Id] from WorkItems where [System.TeamProject] = '" + project + @"' and [System.WorkItemType] = 'Task' and [System.AreaPath] UNDER 'LOCALIZATION\Intl Tools' and [System.IterationPath] = '" + iteration + "' AND ( [System.AssignedTo] = 'Alimjan Mettursun <v-almett@microsoft.com>' OR [System.AssignedTo] = 'Ning Li <v-ninli@microsoft.com>' OR [System.AssignedTo] = 'Haoni Li <v-haonli@microsoft.com>' OR [System.AssignedTo] = 'Joey Ding <v-joding@microsoft.com>' OR [System.AssignedTo] = 'Yan Huang <v-huangy@microsoft.com>' OR [System.AssignedTo] = 'Gopal Aryal <v-goarya@microsoft.com>' OR [System.AssignedTo] = 'Rickas Razafison <v-riraza@microsoft.com>' )";
                var ids = context.RunQuery(project, query);

                var result = context.GetListOfWorkItemsByIDs(ids.Keys);
                foreach (var task in result["value"])
                {
                    var pbiTitle = (string)task["fields"]["System.Title"];
                    int? estimatedHour = task["fields"]["Microsoft.VSTS.Scheduling.OriginalEstimate"] == null ? (int?)null : (int)task["fields"]["Microsoft.VSTS.Scheduling.OriginalEstimate"];
                    int? completedHour = task["fields"]["Microsoft.VSTS.Scheduling.CompletedWork"] == null ? (int?)null : (int)task["fields"]["Microsoft.VSTS.Scheduling.CompletedWork"];
                    string parentTitle = "Others";
                    JToken parent = null;
                    if ((parent = task["relations"].FirstOrDefault(i => (string)i["rel"] == "System.LinkTypes.Hierarchy-Reverse")) != null)
                    {
                        string parentID = ((string)parent["url"]).Split(new char[] { '/' }).Last();
                        parentTitle = (string)context.GetListOfWorkItemsByIDs(new int[] { int.Parse(parentID) })["value"][0]["fields"]["System.Title"];
                    }

                    if (!items.ContainsKey(parentTitle))
                    {
                        var child = new List<Task>() { new Task { Title = pbiTitle, EstimatedHours = estimatedHour, CompletedHours = completedHour } };
                        items.Add(parentTitle, child);
                    }
                    else
                    {
                        var child = items[parentTitle];
                        child.Add(new Task { Title = pbiTitle, EstimatedHours = estimatedHour, CompletedHours = completedHour });
                    }
                }
            }
            var sb = new StringBuilder();
            foreach (var item in data)
            {
                int totalEstimatedSprint = 0;
                int totalCompletedSprint = 0;
                sb.AppendLine(item.Key);
                sb.AppendLine();
                foreach (var pbi in item.Value)
                {
                    int totalEstimatedPbi = 0;
                    int totalCompletedPbi = 0;
                    sb.AppendLine(pbi.Key);
                    foreach (var task in pbi.Value)
                    {
                        int completed = task.CompletedHours ?? 0;
                        int estimated = task.EstimatedHours ?? 0;
                        sb.AppendLine(string.Format("	{0}", task.Title));
                        //sb.AppendLine(string.Format("-----Completed/Estimated: {0}/{1}", completed, estimated));
                        totalEstimatedSprint += estimated;
                        totalCompletedSprint += completed;
                        totalEstimatedPbi += estimated;
                        totalCompletedPbi += completed;
                    }

                    sb.AppendLine();
                    //sb.AppendLine(string.Format("Completed/Estimated: {0}/{1}", totalCompletedPbi, totalEstimatedPbi));
                    // sb.AppendLine();
                }
                sb.AppendLine();

                //sb.AppendLine(string.Format("SPRINT Completed/Estimated: {0}/{1}", totalCompletedSprint, totalEstimatedSprint));

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
            }

            var str = sb.ToString();
        }

        [TestMethod]
        public void VSORest_GetTestRun()
        {
            VsoContext context = GetProdContext();
            int planId = 301628;
            string project = "LOCALIZATION";
            var json = context.GetTestRunResultByPlanID(project, planId);
        }

        [TestMethod]
        public void VSORest_GetTestRunDetail()
        {
            VsoContext context = GetProdContext();
            int runId = 77209;
            string project = "LOCALIZATION";
            var json = context.GetTestRunDetailByRunId(project, runId);
        }

        [TestMethod]
        public void VSORest_GetRepoByProjectNameForAllScrope()
        {
        }

        [TestMethod]
        public void VSORest_GetRepositoryByProjectNameForSkypeMac()
        {
            var context = GetProdContext();

            string project = "LOCALIZATION";
            string repoName = "internal_test_loc-automation";
            var json = context.GetGitRepoByRepoName(project, repoName);
        }

        [TestMethod]
        public void VSORest_GetRepositoryByProjectName2()
        {
            var context = GetProdContextForDomoreexp();

            string project = "teamspace";
            string repoName = "bots";
            var json = context.GetGitRepoByRepoName(project, repoName);
        }

        [TestMethod]
        public void VSORest_GetGitFile()
        {
            var context = GetProdContext();
            string repoId = "907b42e3-73da-4c49-83ca-c2c27cdbaee4";
            string filePath = @"/config/ConcreteFileGroups_AUTOTEST.xml";
            var memoryStream = context.GetGitRepoFileByRepoIDAndFilePath(repoId, filePath);
            using (FileStream file = new FileStream("ConcreteFileGroups_AUTOTEST.xml", FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(file);
            }
        }

        [TestMethod]
        public void VSORest_GetGitFilePaths()
        {
            var context = GetProdContext();
            string repoId = "ff595170-0c05-46ea-862c-041039769a61";
            string folderPath = @"/";
            var files = context.GetGitRepoFilePathsByFolderPath(repoId, folderPath, "dev");
        }

        [TestMethod]
        public void VSORest_GetZipFolder()
        {
            var context = GetProdContext();

            string repoId = "a894ec4d-24d4-48ae-8325-6e0e5b3a2f78";
            string folderPath = @"GroupMe/GroupMe/en.lproj/Localizable.strings";
            //folderPath = folderPath.Replace(@"\", "/");
            string branchName = "translations";

            //string repoId = "93547408-de0d-4e3a-94ed-5232bdb66a2e";
            //string folderPath = @"/SkypeMac/Extras/Localizations";
            //string branchName = "master";
            var memoryStream = context.GetGitZipFolder(repoId, folderPath, branchName);
            //memoryStream.Read()
            //using (FileStream zipfolder = new FileStream("Original.zip", FileMode.Create, FileAccess.Write))
            //{
            //    memoryStream.WriteTo(zipfolder);
            //}
            string dropPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Extract");
            if (!Directory.Exists(dropPath))
            {
                Directory.CreateDirectory(dropPath);
            }
            var zip = ZipFile.Read(memoryStream);
            var items = zip.Entries.ToList();
            foreach (var item in items)
            {
                if (!item.IsDirectory)
                {
                    string folderPath_ = folderPath.Replace("\\", "/");
                    int lastindex = folderPath_.LastIndexOf('/');
                    string filePath = Path.Combine(folderPath_.Substring(0, lastindex + 1), item.FileName);

                    var outputMs = new MemoryStream();
                    item.Extract(outputMs);

                    string gitPath = item.FileName;
                    string gitFileName = Path.GetFileName(gitPath);
                    string gitFolder = Path.GetDirectoryName(gitPath);
                    string gitFolder_ = Path.Combine(dropPath, gitFolder);
                    if (!Directory.Exists(gitFolder_))
                    {
                        Directory.CreateDirectory(gitFolder_);
                    }
                    using (FileStream file = new FileStream(Path.Combine(gitFolder_, gitFileName), FileMode.Create, FileAccess.Write))
                    {
                        outputMs.WriteTo(file);
                    }
                }
            }

            //var reader = item.OpenReader();

            //zip.ExtractAll(dropPath, ExtractExistingFileAction.OverwriteSilently);
        }

        [TestMethod]
        public void VSORest_GetAllGitFilesUnderOneFolder()
        {
            var context = GetProdContext();
            string repoId = "5de408a3-f5c5-42de-9992-fabedd032fd2";

            string folderPath = @"/";
            var list = context.GetGitRepoFilePathsByFolderPath(repoId, folderPath, "master");
        }

        [TestMethod]
        public void Rest_GetLatestBuildFromQuickBuild()
        {
            var context = GetProdContext();
            LatestBuildInfo buildInfo = context.GetLatestBuildInfoAsAnObject(200615);
        }

        [TestMethod]
        public void Rest_GetChildBuildFromQuickBuild()
        {
            var context = GetProdContext();
            string content = context.GetBuildInfoAsString(189001);
        }

        [TestMethod]
        public void VSORest_GetListOfWorkItemsByIDs()
        {
            var ids = new int[] { 294563 };
            var fields = new string[] { "System.Id", "System.WorkItemType", "System.Title", "System.IterationPath", "System.AreaPath" };

            var context = GetProdContext();
            //var result = context.GetListOfWorkItemsByIDs(ids, fields);
            var result = context.GetListOfWorkItemsByIDs(ids, null);
        }

        [TestMethod]
        public void VSORest_GetAttachmentsUrls()
        {
            var ids = new int[] { 662449 };

            var context = GetProdContext();
            var result = context.GetAttachmentsUrls(ids);
        }

        [TestMethod]
        public void VSORest_DownloadAttachments()
        {
            var ids = new int[] { 662449, 746815 };

            var context = GetProdContext();
            string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Attachment");

            var result = context.DownloadAttachments(ids, destFolder);
        }

        [TestMethod]
        public void Test_CommentsHistory()
        {
            var context = GetProdContext();
            int bugID = 799853;
            var result = context.GetWorkItemCommentAsync(bugID, cts.Token);
        }

        [TestMethod]
        public void Test_UTC()
        {
            string utc = "2017-01-22T04:32:17.727Z";
            DateTime date = Convert.ToDateTime(utc);
        }

        [TestMethod]
        public void Test_GetBug()
        {
            var context = GetContext();
            int bugID = 440586;
            var result = context.GetListOfWorkItemsByIDs(new int[] { bugID });
        }

        [TestMethod]
        public void Test_CreateVsoWorkItem()
        {
            var context = GetContext();
            var timeStamp = DateTime.Now.ToString();
            var result = context.CreateVsoWorkItem(TaskTypes.Bug, "LOCALIZATION", "test workItem " + timeStamp, "LOCALIZATION", "LOCALIZATION", null);
        }
    }
}
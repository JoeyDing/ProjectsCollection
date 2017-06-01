using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using VsoApi.Rest;
using static VsoApi.Rest.VsoContext;

namespace VsoApi.UnitTest
{
    [TestClass]
    public class VsoRestTest
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

        [TestMethod]
        public void VsoRest_FindUser()
        {
            var context = GetContext();
            var result = context.SearchIdentity("ri");
        }

        [TestMethod]
        public void VsoRest_UpdateAssignedTo()
        {
            var context = GetContext();
            var result = context.SearchIdentity("v-huangy");
            string uniName = result.Select(c => c.UniqueName).FirstOrDefault();
            string displayName = result.Select(c => c.DisplayName).FirstOrDefault();
            string id = result.Select(c => c.ID).FirstOrDefault();
            string assignedTo = displayName + " <" + uniName + ">";
            context.UpdateVsoWorkItem(266597, new Dictionary<string, string>() { { "System.AssignedTo", uniName } });
        }

        [TestMethod]
        public void VsoRest_GetPRojectsAndTeams()
        {
            var context = GetContext();
            var projects = context.GetProjects();

            foreach (var project in projects)
            {
                var team = context.GetProjectTeams(project.ID, project.Name);
            }
        }

        [TestMethod]
        public void VsoRest_CreateWorkItem()
        {
            var context = GetProdContext();

            //create an epic

            var newEpic = context.CreateVsoWorkItem(
                TaskTypes.Epic,
                "LOCALIZATION",
                "test epic from rest api " + DateTime.Now.ToString(),
                @"LOCALIZATION\Skype for Business Clients",
                @"LOCALIZATION\Skype Localization Monthly Sprints",
                "Rickas Razafison <v-riraza@microsoft.com>");

            string newEpicUrl = (string)newEpic["_links"]["self"]["href"];

            //create Enabling specification and associate it to the epic above

            var newESpec = context.CreateVsoWorkItem(
                TaskTypes.EnablingSpecification,
                "LOCALIZATION",
                "test espec from rest api " + DateTime.Now.ToString(),
                @"LOCALIZATION\Skype for Business Clients",
                @"LOCALIZATION\Skype Localization Monthly Sprints",
                "Rickas Razafison <v-riraza@microsoft.com>", newEpicUrl, LinkTypes.Child);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void VsoRest_CreateWorkItem_TestFail()
        {
            var context = GetProdContext();

            //create an epic

            var newEpic = context.CreateVsoWorkItem(
                TaskTypes.Epic,
                "LOCALIZATION",
                "test epic from rest api " + DateTime.Now.ToString(),
                @"LOCALIZATION",
                @"Fake\Iteration\Path",
                "Rickas Razafison <v-riraza@microsoft.com>");
        }

        [TestMethod]
        public void VsoRest_UploadAttachment()
        {
            var context = GetContext();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TempFile\langTemplate.csv");
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TempFile\lang1Template.csv");
            int bugId = 361907;

            //FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            //FileStream fs1 = new FileStream(path1, FileMode.Open, FileAccess.Read);

            var dict = Enumerable.Range(1, 20).ToDictionary(c => c.ToString() + ".csv",
                c =>
                {
                    Stream fs = new FileStream
                    (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"TempFile\{0}.csv", c)), FileMode.Open, FileAccess.Read);
                    return fs;
                });
            context.UploadAttachmentToVsoWorkItems(bugId, dict);
            //context.UploadAttachmentToVsoWorkItems(bugId, new Dictionary<string, Stream>() { { "langTemplate.csv", fs }, { "langTemplate1.csv", fs1 } });
        }

        [TestMethod]
        public void VsoRest_UploadAttachmentInBatch()
        {
            var context = GetContext();

            int bugId = 361907;
            int bugID2 = 487780;
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TempFile\langTemplate.csv");
            FileStream fs2 = new FileStream(path2, FileMode.Open, FileAccess.Read);
            var dict2 = new Dictionary<string, Stream> { { "sss.csv", fs2 } };
            var dict = Enumerable.Range(1, 20).ToDictionary(c => c.ToString() + ".csv",
                c =>
                {
                    Stream fs = new FileStream
                    (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"TempFile\{0}.csv", c)), FileMode.Open, FileAccess.Read);
                    return fs;
                });
            context.UploadAttachmentToVsoWorkItemInBatch(new Dictionary<int, Dictionary<string, Stream>> { { bugId, dict }, { bugID2, dict2 } });
        }

        [TestMethod]
        public void VsoRest_CreateChildItemsInBatchAndUploadFilesinEachItemsInBatch()
        {
            var context = GetContext();

            Dictionary<string, int> dict_LanguageToBugID = context.CreateNewParentItemAndChildItemsInBatch
                            ("[Non Language-Specific]", "TestForParallelParentBug58658", new List<LanguageAndTitle>
                            { new LanguageAndTitle { Language = "Albanian",Title= "AlbanianTitle" },
                    new LanguageAndTitle { Language = "Arabic", Title = "ArabicTitle" } },
                                "LOCALIZATION", LinkTypes.Child, TaskTypes.Bug, "LOCALIZATION", @"LOCALIZATION\Skype Mobile", @"Yan Huang <v-huangy@microsoft.com>");

            var dict1 = Enumerable.Range(1, 10).ToDictionary(c => c.ToString() + ".csv",
               c =>
               {
                   Stream fs = new FileStream
                   (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"TempFile\{0}.csv", c)), FileMode.Open, FileAccess.Read);
                   return fs;
               });
            var dict2 = Enumerable.Range(11, 10).ToDictionary(c => c.ToString() + ".csv",
               c =>
               {
                   Stream fs = new FileStream
                   (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"TempFile\{0}.csv", c)), FileMode.Open, FileAccess.Read);
                   return fs;
               });
            var source = new Dictionary<int, Dictionary<string, Stream>> { { dict_LanguageToBugID["Albanian"], dict1 }, { dict_LanguageToBugID["Arabic"], dict2 } };
            context.UploadAttachmentToVsoWorkItemInBatch(source);
        }

        [TestMethod]
        public void VsoRest_CreateChildItemsInBatch()
        {
            var context = GetContext();
            int parentBugId = 361907;
            var jsonSampleBug = context.GetListOfWorkItemsByIDs(new int[] { parentBugId });
            var subItems = jsonSampleBug["value"];
            string iterationPath = (string)subItems[0]["fields"]["System.IterationPath"];
            string teamProject = (string)subItems[0]["fields"]["System.TeamProject"];
            string newBugUrl = (string)subItems[0]["url"];

            context.CreateChildItemsForExistingParentItemInBatch
                (new List<LanguageAndTitle>
                { new LanguageAndTitle { Language = "Albanian",Title= "AlbanianTitle" },
                    new LanguageAndTitle { Language = "Arabic", Title = "ArabicTitle" } },
                    teamProject, LinkTypes.Child, newBugUrl, TaskTypes.Bug, "LOCALIZATION", iterationPath, @"Yan Huang <v-huangy@microsoft.com>");
        }

        [TestMethod]
        public void VsoRest_CreateParentAndChildItemsInBatch()
        {
            var context = GetContext();

            context.CreateNewParentItemAndChildItemsInBatch
                            ("[Non Language-Specific]", "TestForParallelParentBug", new List<LanguageAndTitle>
                            { new LanguageAndTitle { Language = "Albanian",Title= "AlbanianTitle" },
                    new LanguageAndTitle { Language = "Arabic", Title = "ArabicTitle" } },
                                "LOCALIZATION", LinkTypes.Child, TaskTypes.Bug, "LOCALIZATION", @"LOCALIZATION\Skype Mobile", @"Yan Huang <v-huangy@microsoft.com>");
        }

        [TestMethod]
        public void VSoRest_RunQuery()
        {
            //var wiql = "SELECT [System.Id], [System.WorkItemType], [System.Title], [System.IterationPath] FROM WorkItems WHERE [System.WorkItemType] = 'Epic' and [System.AreaPath] under 'LOCALIZATION'";
            var wiql = @"select [System.Id], [System.WorkItemType], [System.Title], [Microsoft.VSTS.Common.Priority], [System.AssignedTo], [System.AreaPath] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] in group 'Microsoft.TestCaseCategory' and [System.AreaPath] under 'LOCALIZATION\Templates and Processes'";
            var context = GetContext();
            var result = context.RunQuery("LOCALIZATION", wiql);
        }

        [TestMethod]
        public void VSORest_RunQueryWithTag()
        {
            var wiql = @"SELECT [System.Id], [System.WorkItemType], [System.Title], [System.IterationPath]"
                        + " FROM WorkItems"
                        + " WHERE [System.WorkItemType] = 'Epic'"
                        + " and [System.AreaPath] under 'LOCALIZATION'"
                        + " and [System.Tags] contains 'Loc_Release'";

            var context = GetContext();
            var result = context.RunQuery("LOCALIZATION", wiql);
        }

        [TestMethod]
        public void VSORest_GetTestPlanUrl()
        {
            //planUrl = "https://skype-test2.visualstudio.com/DefaultCollection/LOCALIZATION/_apis/test/Plans/285373";
            int planId = 448497;
            var context = GetContext();
            string url = context.GetTestPlanUrl(planId);
        }

        [TestMethod]
        public void VSORest_UpdateVsoWorkItem()
        {
            var id = 361857;
            var fields = new Dictionary<string, string> { { "System.Title", "test epic updated on " + DateTime.Now.ToString() } };

            var context = GetContext();
            var newBug = context.CreateVsoWorkItem(
                TaskTypes.Bug,
                "LOCALIZATION",
                "test Parent Bug from rest api " + DateTime.Now.ToString(),
                @"LOCALIZATION\Skype for Business Clients",
                @"LOCALIZATION\Skype Localization Monthly Sprints",
                "Yan Huang <v-riraza@microsoft.com>");
            string newBugUrl = (string)newBug["url"];

            var result = context.UpdateVsoWorkItem(id, null, (c) =>
            {
                var f_relation = new Dictionary<string, object>() {
                           { "op", "add" },
                           { "path", string.Format("/relations/-") },
                           { "value", new Dictionary<string,object>()
                               {
                                    { "rel", "System.LinkTypes.Hierarchy-Reverse"},
                                    { "url", newBugUrl}
                               }
                           }
                };
                c.Add(f_relation);
            });
        }

        [TestMethod]
        public void VSORest_GetListOfWorkItemsByIDs()
        {
            var ids = new int[] { 488954 };
            var context = GetContext();
            string[] fields =
                {
                "System.Id",
                "System.WorkItemType"
            };
            //var result = context.GetListOfWorkItemsByIDs(ids, fields);
            var result = context.GetListOfWorkItemsByIDs(ids, null);
        }

        [TestMethod]
        public void VSORest_GetListOfWorkItemsByIDs_AllFieldsWithValue_Epic()
        {
            //epic id
            var ids = new int[] { 276431 };

            var context = GetContext();
            var result = context.GetListOfWorkItemsByIDs(ids);
        }

        [TestMethod]
        public void VSORest_GetListOfWorkItemsByIDs_AllFieldsWithValue_Bug()
        {
            //epic id
            var ids = new int[] { 605611 };

            var context = GetProdContext();
            var result = context.GetListOfWorkItemsByIDs(ids);
        }

        [TestMethod]
        public void TestMethod()
        {
            var result = DateTime.Now.ToString("M/d/yy");
        }

        [TestMethod]
        public void VSORest_GetAreaPaths()
        {
            var context = GetContext();
            var result = context.GetAreas("LOCALIZATION", 2);
            var builder = new StringBuilder();
            foreach (var family in result["children"])
            {
                if ((bool)family["hasChildren"] == true)
                {
                    foreach (var product in family["children"])
                    {
                        builder.AppendLine(string.Format("{0},{1}", family["name"], product["name"]));
                    }
                }
            }
            var csv = builder.ToString();
        }

        [TestMethod]
        public void VSORest_GetWorkItemsUnderAreaPath()
        {
            var context = GetContext();
            var result = context.GetWorkItemsUnderAreaPath(TaskTypes.EnablingSpecification, "LOCALIZATION", @"LOCALIZATION\Skype for Business Clients\SkypeCast", new string[] { "System.ID", "System.Title" }, true);
        }

        [TestMethod]
        public void VSORest_GenerateCustomQueryUrl()
        {
            var context = GetContext();
            string wiql = "SELECT [System.Id],[System.Title] FROM WorkItems WHERE [System.ID] = 322552";
            string project = "LOCALIZATION";
            var result = context.GenerateCustomQueryUrl(wiql, project);

            var result2 = context.GenerateCustomQueryUrl(wiql, project, team: "Skype Mobile");
        }

        [TestMethod]
        public void VSORest_GetProjectTeamDefaultArea()
        {
            var context = GetContext();
            string project = "LOCALIZATION";
            string team = "Skype for Business Clients";
            var result = context.GetProjectTeamDefaultArea(project, team);
        }

        [TestMethod]
        public void VSORest_CopyTestSuiteUnderTestPlan()
        {
            var context = GetContext();
            //Dictionary<int, List<int>> result = context.GetTestCaseIDsBySuiteID("LOCALIZATION", 285373, 364121);
            //int parentId = result.Keys.First();
            //List<int> listId = result.Values.First();
            var suiteId = context.CreateTestSuite("LOCALIZATION", "NewTestSuite_1", "StaticTestSuite", 285373, 285374);

            //var json = context.AddTestCases("LOCALIZATION", 285373, suiteId, listId);
        }

        [TestMethod]
        public void VSORest_CloneTestSuite2()
        {
            var context = GetContext();
            string[] suiteNames = Enumerable.Range(1, 51).Select(c => c.ToString()).ToArray();
            context.CloneTestSuiteInsidePlan2(441413, "LOCALIZATION", 441415, suiteNames, new string[] { }, new List<int>());
        }

        [TestMethod]
        public void VSORest_CloneTestSuite2_Retry()
        {
            /*need to modifty code as
            lock (locker)
            {
                newSuiteId = this.CreateTestSuite(projectName, itemToProcess, "StaticTestSuite", planid, parentId);
            }
            if (itemToProcess.ToLower() == "arabic") throw new Exception("Test");
            var json = this.AddTestCases(projectName, planid, newSuiteId, IdsForTestCasesToBeAdded);
             */
            var context = GetContext();
            string[] suiteNames = { "ssss" };
            context.CloneTestSuiteInsidePlan2(441617, "LOCALIZATION", 441619, suiteNames, new string[] { }, new List<int>());
        }

        [TestMethod]
        public void VSORest_CloneTestSuiteUnderTestPlan()
        {
            var context = GetContext();

            string project = "LOCALIZATION";
            string[] suitNames = new string[4];
            suitNames[0] = "ca-ES";
            suitNames[1] = "cs-CZ";
            suitNames[2] = "el-GR";
            suitNames[3] = "fi-FI";

            var json = context.CloneTestSuiteInsidePlan(338617, project, 351805, suitNames);
        }

        [TestMethod]
        public void VSORest_GetTestPlanByPlanID()
        {
            VsoContext context = this.GetProdContext();
            var result = context.GetTestPlanByPlanID("LOCALIZATION", 428948);
            //var result2 = context.GetListOfWorkItemsByIDs(new int[] { 362188 });
        }

        [TestMethod]
        public void VSORest_GetTestRunResultByPlanID()
        {
            VsoContext context = this.GetProdContext();
            var result = context.GetTestRunResultByPlanID("LOCALIZATION", 448654);
        }

        [TestMethod]
        public void VSORest_GetListOfTestSuitesByPlanId()
        {
            VsoContext context = GetContext();
            var result = context.GetListOfTestSuitesByPlanID("LOCALIZATION", 441467);
        }

        [TestMethod]
        public void VSORest_GetListOfTestCaseBySuiteId()
        {
            VsoContext context = GetContext();
            var result = context.GetTestCaseIDsBySuiteID("LOCALIZATION", 20208, 20209);
        }

        [TestMethod]
        public void VSORest_GetBugUrl()
        {
            VsoContext context = GetContext();
            int bugId = 362188;
            string url = context.GetBugUrl(bugId);
        }

        [TestMethod]
        public void VSORest_GetTestRun()
        {
            VsoContext context = GetContext();
            int planId = 285373;
            string project = "LOCALIZATION";
            var json = context.GetTestRunResultByPlanID(project, planId);
        }

        [TestMethod]
        public void VSORest_GetTestRunDetail()
        {
            VsoContext context = GetProdContext();
            int runId = 6305;
            string project = "LOCALIZATION";
            var json = context.GetTestRunDetailByRunId(project, runId);
        }

        [TestMethod]
        public void VSORest_GetAllTestRun()
        {
            VsoContext context = GetContext();
            string project = "LOCALIZATION";
            var json = context.GetAllTestRun(project, 0, 10);
        }

        [TestMethod]
        public void VSORest_GetTestRunByitemsPerBatch()
        {
            VsoContext context = GetContext();
            string project = "LOCALIZATION";
            var json = context.GetTestRunByitemsPerBatchAndSleepTime(project, 40, 1);
        }

        [TestMethod]
        public void VSORest_ResolveSuccessor()
        {
            VsoContext context = GetContext();
            int locBugId = 380387;
            string reason = "Fixed";
            context.UpdateVsoWorkItem(locBugId, new Dictionary<string, string>() { { "System.State", "Resolved" }, { "Skype.FixProvidedBy", "Core" }, { "Skype.RootCauseCategory", "Intl Design" }, { "System.Reason", reason } });
        }

        [TestMethod]
        public void Git_VSORest_GetGitRepoByRepoNameAndBranch()
        {
            VsoContext context = GetProdContext();
            var repoInfo = context.GetGitRepoByRepoName("SCONSUMER", "client_android_skype");
            var repId = (string)repoInfo["id"];
            var files = context.GetGitRepoFilePathsByFolderPath(repId, @"skype-android-codegen", "master");
            var files2 = context.GetGitRepoFilePathsByFolderPath(repId, @"skype-android-codegen", "avatar");
        }

        [TestMethod]
        public void Git_VSORest_GetCustomUrl()
        {
            VsoContext context = GetProdContext();
            var res = context.GenerateCustomQueryUrl("SELECT [System.Id],[System.WorkItemType],[System.Title] FROM WorkItems Where [System.WorkItemType] = 'Impediment'", "LOCALIZATION");
        }

        [TestMethod]
        public void Git_VSORest_GetIterations()
        {
            VsoContext context = GetProdContext();
            var res = context.GetIterations("LOCALIZATION", 5);
        }

        [TestMethod]
        public void Git_VSORest_Getinternal_tools_intltoolsItemList()
        {
            VsoContext context = GetProdContext();
            var repoInfo = context.GetGitRepoByRepoName("LOCALIZATION", "internal_tools_intltools");
            var repId = (string)repoInfo["id"];
            var files = context.GetGitRepoFilePathsByFolderPath(repId, @"/", "master", false);
            var mds = files.Where(x => x.EndsWith(".md")).ToList();
            int total = mds.Count;
        }

        [TestMethod]
        public void Git_VSORest_GetGitRepoFileByRepoIDAndFilePath()
        {
            VsoContext context = GetProdContext();
            //var repoInfo = context.GetGitRepoByRepoName("LOCALIZATION", "internal_tools_intltools");
            //var repId = (string) repoInfo["id"];
            var files = context.GetGitRepoFileByRepoIDAndFilePath("5de408a3-f5c5-42de-9992-fabedd032fd2", @"OnePseudo/dev/OnePseudo/Documentation/OnePseudo_CommanLine.jpg", "master");
            byte[] bytes = null;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.JPG");

            using (var s = File.Create(path))
            {
                byte[] buffer = new byte[8 * 1024];
                while (files.Read(buffer, 0, buffer.Length) > 0)
                {
                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }

        [TestMethod]
        public void VsoReportingTest_GetBugForLeaf()
        {
            var context = GetContext();

            List<BugItemFromVSO> result = context.GetBugItemForLeaf("def", "1", "2", "en-us", CancellationToken.None).Result;
        }

        [TestMethod]
        public void VsoReportingTest_CreateBugForLeaf()
        {
            var context = GetContext();
            var result = context.CreateBugItemForLeaf("13; \"ABOUT_THIRDPARTYNOTICE_BUTTON\"", "SkypeWifiiOS", "TEST_Localizable.strings", "abc", "Why-1", cts.Token);
        }

        [TestMethod]
        public void VsoReportingTest_UpdateBugForLeaf()
        {
            var context = this.GetContext();
            int locBugId = 433444;
            string answer = "Because2";
            var result = context.AnswerBugItemForLeaf(locBugId, answer, cts.Token);
        }

        [TestMethod]
        public void Test_ReplaceIlegalChars()
        {
            var context = this.GetContext();
            string jsonString = "\\\"Microsoft.VSTS.TCM.ReproSteps\\\":\\\"<div><font style=\\\\\\\"background-color:rgb(255, 255, 255);\\\\\\\">aaa</font></div>\\\"";
            string result = context.ReplaceIlegalCharsInJson(jsonString);
        }
    }
}
using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VsoApi.Rest;
using System.Configuration;
using SkypeIntlPortfolio.Ajax.Core.Service.Vso;
using System.IO;
using System.Linq;

namespace SkypeIntlPortfolio.Tests
{
    /// <summary>
    /// Summary description for UnitTest_VsoContextService
    /// </summary>
    [TestClass]
    public class UnitTest_VsoContextService
    {
        private VsoContext GetContext()
        {
            string vsoPrivateKey = ConfigurationManager.AppSettings["TestVsoPrivateKey"];
            var context = new VsoContext("skype-test2", vsoPrivateKey);
            return context;
        }

        [TestMethod]
        public void VsoService_UploadAttachmentToVsoWorkItemInBatch()
        {
            var context = GetContext();
            var vsoService = new VsoContextService(context);
            int bugId = 361907;
            int bugID2 = 487780;
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TempFile\langTemplate.csv");
            FileStream fs2 = new FileStream(path2, FileMode.Open, FileAccess.Read);
            var dict2 = new Dictionary<string, Stream> { { "langTemplate.csv", fs2 } };
            var dict = Enumerable.Range(1, 20).ToDictionary(c => c.ToString() + ".csv",
                c =>
                {
                    Stream fs = new FileStream
                    (Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"TempFile\{0}.csv", c)), FileMode.Open, FileAccess.Read);
                    return fs;
                });
            vsoService.UploadAttachmentToVsoWorkItemInBatch(new Dictionary<int, Dictionary<string, Stream>> { { bugId, dict }, { bugID2, dict2 } }, 40, 3);
        }

        [TestMethod]
        public void VsoService_CreateChildItemsForExistingParentItemInBatch()
        {
            var context = GetContext();
            var vsoService = new VsoContextService(context);
            int parentBugId = 361907;
            var jsonSampleBug = context.GetListOfWorkItemsByIDs(new int[] { parentBugId });
            var subItems = jsonSampleBug["value"];
            string iterationPath = (string)subItems[0]["fields"]["System.IterationPath"];
            string teamProject = (string)subItems[0]["fields"]["System.TeamProject"];
            string newBugUrl = (string)subItems[0]["url"];

            vsoService.CreateChildItemsForExistingParentItemInBatch
                (new List<LanguageAndTitle>
                { new LanguageAndTitle { Language = "Albanian",Title= "AlbanianTitle" },
                    new LanguageAndTitle { Language = "Arabic", Title = "ArabicTitle" } },
                    teamProject, LinkTypes.Child, newBugUrl, TaskTypes.Bug, "LOCALIZATION", iterationPath, @"Yan Huang <v-huangy@microsoft.com>", totalRetry: 3);
        }

        [TestMethod]
        public void VsoService_CreateNewParentItemAndChildItemsInBatch()
        {
            var context = GetContext();
            var vsoService = new VsoContextService(context);
            vsoService.CreateNewParentItemAndChildItemsInBatch
                            ("[Non Language-Specific]", "TestForParallelParentBug", new List<LanguageAndTitle>
                            { new LanguageAndTitle { Language = "Albanian",Title= "AlbanianTitle" },
                    new LanguageAndTitle { Language = "Arabic", Title = "ArabicTitle" } },
                                "LOCALIZATION", LinkTypes.Child, TaskTypes.Bug, "LOCALIZATION", @"LOCALIZATION\Skype Mobile", @"Yan Huang <v-huangy@microsoft.com>");
        }
    }
}
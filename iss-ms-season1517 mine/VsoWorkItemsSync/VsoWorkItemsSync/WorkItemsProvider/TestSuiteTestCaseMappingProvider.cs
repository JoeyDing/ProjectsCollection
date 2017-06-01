using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsoApi.Rest;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    internal class TestSuiteTestCaseMappingProvider : WorkItemProviderBase<TestSuiteTestCaseMapping>
    {
        public TestSuiteTestCaseMappingProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public HashSet<TestSuiteTestCaseMapping> hashset_mapping { get; set; }

        private HashSet<TestSuiteTestCaseMapping> GetWorkItemsInParralel(IEnumerable<TestSuiteTestCaseMapping> testPlans)
        {
            //1 create our producer list of testPlans
            var producer = new BlockingCollection<TestSuiteTestCaseMapping>();
            var hashset_parralel = new BlockingCollection<TestSuiteTestCaseMapping>();

            foreach (var item in testPlans)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();
            var allitem = new HashSet<TestSuiteTestCaseMapping>(new testSuiteTestCaseMappingComparer());
            BlockingCollection<int> counter = new BlockingCollection<int>();
            BlockingCollection<int> failed_testIds = new BlockingCollection<int>();
            //2 create our action to be run in parallel
            var action = new Action(() =>
            {
                TestSuiteTestCaseMapping itemToProcess = null;
                while (producer.TryTake(out itemToProcess))
                {
                    int testcaseId = 0;
                    try
                    {
                        testcaseId = itemToProcess.Test_Case_ID;

                        Stopwatch watcher = new Stopwatch();
                        watcher.Start();

                        var json = this.VsoContext.GetTestPlanAndTestSuiteByTestCaseId(base.ProjectName, testcaseId);
                        var json_value = json["value"];
                        if (json_value != null)
                        {
                            foreach (var item_ in json_value)
                            {
                                int testSuiteId = (int)item_["id"];
                                int testPlanId = (int)item_["plan"]["id"];
                                hashset_parralel.TryAdd(new TestSuiteTestCaseMapping
                                {
                                    Test_Case_ID = testcaseId,
                                    Test_Suite_ID = testSuiteId,
                                    Test_Plan_ID = testPlanId
                                });
                            }
                        }

                        //Except with

                        watcher.Stop();
                        //Console.WriteLine("processing {0} from thread {1}, total: {2} s"
                        //    , itemToProcess.Test_Plan_ID, System.Threading.Thread.CurrentThread.ManagedThreadId, watcher.Elapsed.Seconds);

                        //log
                        if (hashset_parralel.Count > (counter.Count + 1) * 1000)
                        {
                            counter.TryAdd(0);
                            Console.WriteLine(DateTime.Now.ToString() + " " + hashset_parralel.Count + "( failded:" + failed_testIds.Count + " )");
                        }
                    }
                    catch (Exception e)
                    {
                        failed_testIds.Add(testcaseId);
                    }
                }
            });

            //3 Start our process in parallel
            int totalThread = 100;
            //var p = new Action[totalThread];
            var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
            Parallel.Invoke(p);
            foreach (var item in hashset_parralel)
            {
                allitem.Add(item);
            }

            return allitem;
        }

        public override JObject GetWorkItems()
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                // TODO: Implement this method
                var mapping = base.FetchData(dbContext, null,
                     query: "SELECT DISTINCT(ID) FROM TestCaseRevisions",
                     convertFunction: (reader) =>
                     {
                         var testPlan = new TestSuiteTestCaseMapping();
                         testPlan.Test_Case_ID = (int)reader.GetInt64(0);

                         return testPlan;
                     }
                    );

                hashset_mapping = GetWorkItemsInParralel(mapping);

                JObject result = null;
                return result;
            }
        }

        public override HashSet<TestSuiteTestCaseMapping> PrepareDbItems(JObject workItems)
        {
            return hashset_mapping;
        }

        public override HashSet<TestSuiteTestCaseMapping> UpdateDatabase(HashSet<TestSuiteTestCaseMapping> workItems)
        {
            var result = base.InsertNewVsoItemsInDB(vsoWorkItems: workItems,
                                         query: "Select Test Plan ID, Test Suite ID,Test Case ID from TestSuiteTestCaseMapping",
                                         convertFunction: (reader) =>
                                         {
                                             var mapping = new TestSuiteTestCaseMapping();
                                             mapping.Test_Plan_ID = reader.GetInt32(0);
                                             mapping.Test_Suite_ID = reader.GetInt32(1);
                                             mapping.Test_Case_ID = reader.GetInt32(2);
                                             return mapping;
                                         },
                                         destinationTableName: "TestSuiteTestCaseMapping");

            return result;
        }
    }
}
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
    internal class TestRunResultProvider : WorkItemProviderBase<TestRunResult>
    {
        public TestRunResultProvider(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                var testPlans = base.FetchData(dbContext, null,
                    query: "SELECT DISTINCT(ID) FROM TestPlanRevisions",
                     convertFunction: (reader) =>
                                           {
                                               var testPlan = new TestRunResult();
                                               testPlan.Test_Plan_ID = reader.GetInt64(0);

                                               return testPlan;
                                           }
                    );

                var result = new JObject();
                var jArray = new JArray();
                result.Add("value", jArray);

                var res = GetWorkItemsInParralel(testPlans);
                foreach (var testRun in res)
                {
                    if (testRun["value"] != null)
                    {
                        foreach (var testResult in testRun["value"])
                        {
                            jArray.Add(testResult);
                        }
                    }
                }
                return result;
            }
        }

        private BlockingCollection<JObject> GetWorkItemsInParralel(IEnumerable<TestRunResult> testPlans)
        {
            //1 create our producer list of testPlans
            var producer = new BlockingCollection<TestRunResult>();
            var result = new BlockingCollection<JObject>();

            foreach (var item in testPlans)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();

            //2 create our action to be run in parallel
            var action = new Action(() =>
            {
                TestRunResult itemToProcess = null;
                while (producer.TryTake(out itemToProcess))
                {
                    try
                    {
                        Stopwatch watcher = new Stopwatch();
                        watcher.Start();

                        JObject testResultJSON = base.VsoContext.GetTestRunResultByPlanID(base.ProjectName, (int) itemToProcess.Test_Plan_ID, 3);
                        //Except with

                        watcher.Stop();
                        Console.WriteLine("processing {0} from thread {1}, total: {2} s"
                            , itemToProcess.Test_Plan_ID, System.Threading.Thread.CurrentThread.ManagedThreadId, watcher.Elapsed.Seconds);

                        result.TryAdd(testResultJSON);
                    }
                    catch (Exception e)
                    {
                    }
                }
            });

            //3 Start our process in parallel
            int totalThread = 20;
            //var p = new Action[totalThread];
            var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
            Parallel.Invoke(p);

            //Parallel.Invoke(
            //    () => action(),
            //    () => action(),
            //    () => action(),
            //    () => action(),
            //    () => action()
            //    );

            return result;
        }

        public override HashSet<TestRunResult> PrepareDbItems(JObject workItems)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                var producer = new BlockingCollection<TestRunResult>();
                //var result = new BlockingCollection<JObject>();

                var values = workItems["value"] as IList<JToken>;
                //turn it into hashset For Db Exceptwith
                var allItemsForDb = new HashSet<TestRunResult>(new TestRunItemComparer1());
                foreach (JObject item in values)
                {
                    int runId = (int) item["id"];
                    int planId = (int) item["plan"]["id"];
                    allItemsForDb.Add(new TestRunResult
                    {
                        Test_Run_ID = runId,
                        Test_Plan_ID = planId
                    });
                }

                var testRuns = base.FetchData(dbContext, null,
                     query: "Select [Test Run ID] from TestRunResults",
                     convertFunction: (reader) =>
                     {
                         var testRun = new TestRunResult();
                         testRun.Test_Run_ID = reader.GetInt64(0);

                         return testRun;
                     }
                    );
                allItemsForDb.ExceptWith(testRuns);

                #region Mutlithread When there are lots of test Runs

                var allItems = new HashSet<TestRunResult>(new TestRunItemComparer());
                //1 create our producer list of testRuns
                foreach (TestRunResult item in allItemsForDb)
                {
                    producer.Add(item);
                }
                producer.CompleteAdding();
                //2 create our action to be run in parallel
                var action = new Action(() =>
                {
                    TestRunResult itemToProcess = null;
                    while (producer.TryTake(out itemToProcess))
                    {
                        try
                        {
                            Stopwatch watcher = new Stopwatch();
                            watcher.Start();

                            int runId = (int) itemToProcess.Test_Run_ID;

                            int testPlanId = (int) itemToProcess.Test_Plan_ID;
                            JObject jsonTestRunDetail = base.VsoContext.GetTestRunDetailByRunId(base.ProjectName, runId, 1);
                            //from test run detail json
                            var jresults = jsonTestRunDetail["value"];
                            foreach (JObject jresult in jresults)
                            {
                                string testRunName = (jresult["testRun"] == null) ? null : base.GetValue<string>((JObject) jresult["testRun"], "name");
                                int testCaseID = (jresult["testCase"] == null) ? default(int) : base.GetValue<int>((JObject) jresult["testCase"], "id");
                                string testOutcome = base.GetValue<string>(jresult, "outcome") ?? "None";
                                DateTime? startDate = base.GetValue<DateTime?>(jresult, "startedDate") ?? base.GetValue<DateTime?>(jresult, "createdDate");
                                DateTime? completedDate = base.GetValue<DateTime?>(jresult, "completedDate") ?? base.GetValue<DateTime?>(jresult, "createdDate");
                                string testerDisplayName = (jresult["runBy"] == null) ? base.GetValue<string>((JObject) jresult["owner"], "displayName") : base.GetValue<string>((JObject) jresult["runBy"], "displayName");
                                string testerUniqueName = (jresult["runBy"] == null) ? base.GetValue<string>((JObject) jresult["owner"], "uniqueName") : base.GetValue<string>((JObject) jresult["runBy"], "uniqueName");

                                allItems.Add(new TestRunResult
                                {
                                    Test_Plan_ID = testPlanId,
                                    Test_Run_ID = runId,

                                    //from test run detail json
                                    Test_Run_Name = testRunName,
                                    Test_Case_ID = testCaseID,
                                    Test_Outcome = testOutcome,
                                    Start_Date = startDate,
                                    Completed_Date = completedDate,
                                    Tester_Display_Name = testerDisplayName,
                                    Tester_Unique_Name = testerUniqueName
                                });
                            }
                            watcher.Stop();
                            Console.WriteLine("processing {0} from thread {1}, total: {2} s"
                                , (int) itemToProcess.Test_Run_ID, System.Threading.Thread.CurrentThread.ManagedThreadId, watcher.Elapsed.Seconds);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                });
                //3 Start our process in parallel
                int totalThread = 50;
                //var p = new Action[totalThread];
                var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
                Parallel.Invoke(p);

                return allItems;

                #endregion Mutlithread When there are lots of test Runs
            }
        }

        public override HashSet<TestRunResult> UpdateDatabase(HashSet<TestRunResult> workItems)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        var result = new HashSet<TestRunResult>();
                        base.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, workItems, "TestRunResults");
                        dbContextTransaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
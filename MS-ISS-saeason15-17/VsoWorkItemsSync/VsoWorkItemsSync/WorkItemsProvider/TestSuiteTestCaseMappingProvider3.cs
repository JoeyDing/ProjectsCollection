using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Model;

namespace VsoWorkItemsSync.WorkItemsProvider
{
    internal class TestSuiteTestCaseMappingProvider3 : WorkItemProviderBase<TestSuiteTestCaseMapping>
    {
        public HashSet<TestSuiteTestCaseMapping> HashsetMapping { get; set; }

        public TestSuiteTestCaseMappingProvider3(DateTime? fromDate)
            : base(fromDate)
        {
        }

        public override JObject GetWorkItems()
        {
            HashSet<TestSuiteTestCaseMapping> allitem = new HashSet<TestSuiteTestCaseMapping>(new testSuiteTestCaseMappingComparer());
            using (var dbContext = new VsoWorkItemsContext())
            {
                //1. Use Parallel to get a dict of testplan and testsuites relations
                var testplan = Utils.FetchData(dbContext, null,
                    query: "SELECT DISTINCT(ID) FROM TestPlanRevisions",
                    convertFunction: (reader) =>
                    {
                        var testplan_ = new TestPlanRevision();
                        testplan_.ID = (int)reader.GetInt64(0);
                        return testplan_;
                    });
                var dict_testSuiteAndTestplans = this.GetWorkItemsInParralel(testplan);

                //2. get a dict of testsuites and testCases relations
                var dict_staticTestSuiteAndTestCases = this.GetTestCaseIDsByStaticTestSuiteID(dict_testSuiteAndTestplans);
                var dict_dynamicTestSuiteAndTestCases = this.GetTestCaseIDsByDynamicTestSuiteID(dict_testSuiteAndTestplans);
                //3.load hashset of mapping table

                //var data = dict_dynamicTestSuiteAndTestCases.Union(dict_staticTestSuiteAndTestCases)
                //    .SelectMany(c => c.Value.TestCases
                //        .Select(x => new TestSuiteTestCaseMapping
                //        {
                //            Test_Plan_ID = c.Value.TestPlanId,
                //            Test_Suite_ID = c.Value.TestSuiteId,
                //            Test_Case_ID = x,
                //            Test_Suite_Rev = c.Value.Rev
                //        }));

                foreach (var item in dict_staticTestSuiteAndTestCases)
                {
                    TestSuiteRevCases currentItem = item.Value;
                    int testplanId = currentItem.TestPlanId;
                    int testSuiteId = currentItem.TestSuiteId;
                    int rev = currentItem.Rev;
                    foreach (var testcaseId in currentItem.TestCases)
                    {
                        allitem.Add(new TestSuiteTestCaseMapping
                        {
                            Test_Plan_ID = testplanId,
                            Test_Suite_ID = testSuiteId,
                            Test_Case_ID = testcaseId,
                            Test_Suite_Rev = rev
                        });
                    }
                }

                foreach (var item in dict_dynamicTestSuiteAndTestCases)
                {
                    TestSuiteRevCases currentItem = item.Value;
                    int testplanId = currentItem.TestPlanId;
                    int testSuiteId = currentItem.TestSuiteId;
                    int rev = currentItem.Rev;
                    foreach (var testcaseId in currentItem.TestCases)
                    {
                        allitem.Add(new TestSuiteTestCaseMapping
                        {
                            Test_Plan_ID = testplanId,
                            Test_Suite_ID = testSuiteId,
                            Test_Case_ID = testcaseId,
                            Test_Suite_Rev = rev
                        });
                    }
                }
            }
            HashsetMapping = allitem;
            JObject result = null;
            return result;
        }

        private ConcurrentDictionary<int, int> GetWorkItemsInParralel(IEnumerable<TestPlanRevision> testPlans)
        {
            var producer = new BlockingCollection<TestPlanRevision>();
            ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
            var failed_testPlanIds = new BlockingCollection<int>();
            var counter = new BlockingCollection<int>();

            foreach (var item in testPlans)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();

            var action = new Action(() =>
            {
                TestPlanRevision itemToProcess = null;
                while (producer.TryTake(out itemToProcess))
                {
                    BlockingCollection<int> list_testSuitIds = new BlockingCollection<int>();

                    int testplanId = (int)itemToProcess.ID;

                    try
                    {
                        var json = this.VsoContext.GetListOfTestSuitesByPlanID(base.ProjectName, testplanId);
                        var json_value = json["value"];
                        if (json_value != null)
                        {
                            foreach (var testSuite in json_value)
                            {
                                int testSuiteId = (int)testSuite["id"];
                                dict.TryAdd(testSuiteId, testplanId);
                            }
                        }
                        if (dict.Count > (counter.Count + 1) * 500)
                        {
                            counter.TryAdd(0);
                            Console.WriteLine(DateTime.Now.ToString() + " " + dict.Count + "( failed:" + failed_testPlanIds.Count + " )");
                        }
                    }
                    catch (Exception)
                    {
                        failed_testPlanIds.TryAdd(0);
                    }
                }
            });

            //3 Start our process in parallel
            int totalThread = 20;
            //var p = new Action[totalThread];
            var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
            Parallel.Invoke(p);
            return dict;
        }

        public class TestSuiteRevCases
        {
            public TestSuiteRevCases()
            {
                this.TestCases = new HashSet<int>();
            }

            public int TestPlanId { get; set; }

            public int TestSuiteId { get; set; }

            public int Rev { get; set; }

            public HashSet<int> TestCases { get; set; }
        }

        public Dictionary<int, TestSuiteRevCases> GetTestCaseIDsByStaticTestSuiteID(ConcurrentDictionary<int, int> dict_testsuiteAndTestPlan)
        {
            Dictionary<int, TestSuiteRevCases> result = new Dictionary<int, TestSuiteRevCases>();
            using (var dbContext = new VsoWorkItemsContext())
            {
                //check existing mapping
                var testCasesMappings = Utils.FetchData(dbContext, null,
                    query: @" select distinct T2.[Test Suite ID] ,T3.[Test Suite Type], T3.Rev,
                     STUFF((Select ','+ CONVERT(varchar(10), T1.[Test Case ID])
                     from [dbo].[TestSuiteTestCaseMapping] T1
                     where T1.[Test Suite ID]=T2.[Test Suite ID]
                     FOR XML PATH('')),1,1,'') as TestCaseIds
                    from [dbo].[TestSuiteTestCaseMapping] T2
                    join (select [ID], [Test Suite Type], max(rev) Rev from [dbo].[TestSuiteRevisions] group by [ID], [Test Suite Type]) T3
                    on T2.[Test Suite ID] = T3.[ID] where T3.[Test Suite Type] = 'Static'",
                    convertFunction: (reader) =>
                    {
                        var testSuiteInfo = new TestSuiteInfo();
                        testSuiteInfo.ID = reader.GetInt32(0);
                        testSuiteInfo.Rev = reader.GetInt32(2);
                        testSuiteInfo.TestCaseIds = reader.GetString(3);

                        return testSuiteInfo;
                    }
                   );
                //dealing with exisiting testCaseIds
                foreach (var item in testCasesMappings)
                {
                    int mappingSuiteId = item.ID;
                    int mappingRev = item.Rev;
                    string[] array_mappingCaseIds = item.TestCaseIds.Trim().Split(',');
                    HashSet<int> hashset_mappingCaseIds = new HashSet<int>();
                    //first check if mapping suiteID exists in the dict
                    if (dict_testsuiteAndTestPlan.ContainsKey(mappingSuiteId))
                    {
                        int testplanID = dict_testsuiteAndTestPlan[mappingSuiteId];
                        foreach (var caseId in array_mappingCaseIds)
                        {
                            int existingId = int.Parse(caseId);
                            hashset_mappingCaseIds.Add(existingId);
                        }

                        result.Add(mappingSuiteId, new TestSuiteRevCases()
                        {
                            TestPlanId = testplanID,
                            TestSuiteId = mappingSuiteId,
                            Rev = mappingRev,
                            TestCases = hashset_mappingCaseIds
                        });
                    }
                }

                //2.get history of static testSuite increment
                var testCasesMissingHistory = Utils.FetchData(dbContext, null,
                    query: @"SELECT ID, rev,  [Test Suite Audit]
                  FROM [TestSuiteRevisions] x
                  LEFT JOIN (select [Test Plan ID], [Test Suite ID], [Test Suite Rev] from TestSuiteTestCaseMapping group by [Test Plan ID], [Test Suite ID], [Test Suite Rev]) y
                  on x.ID = y.[Test Suite ID]
                  where [Test Suite Type]='Static' and  ([Test Suite Audit] like 'Added these test cases%' OR [Test Suite Audit] like 'Removed these test cases%')
                  and  (x.rev > y. [Test Suite Rev] or y.[Test Suite ID] is null)
                  order by ID, rev ",
                    convertFunction: (reader) =>
                    {
                        var testSuiteAudit = new TestSuiteInfo();
                        testSuiteAudit.ID = (int)reader.GetInt64(0);
                        testSuiteAudit.Rev = reader.GetInt32(1);
                        testSuiteAudit.Test_Suite_Audit = reader.GetString(2);

                        return testSuiteAudit;
                    }
                   );

                var testCasesMissingHistoryGrouped = testCasesMissingHistory.GroupBy(c => c.ID);
                foreach (var testSuiteMissingHistoryGroup in testCasesMissingHistoryGrouped)
                {
                    int testSuiteId = testSuiteMissingHistoryGroup.Key;
                    //first check if this testSuiteId is in the existing testSuiteIds
                    if (dict_testsuiteAndTestPlan.ContainsKey(testSuiteId))
                    {
                        int testplanId = dict_testsuiteAndTestPlan[testSuiteId];
                        // check if there is some cases already in the mapping table and load them
                        TestSuiteRevCases currentItem = new TestSuiteRevCases();
                        HashSet<int> hashset_testcaseIDs = new HashSet<int>();
                        if (result.ContainsKey(testSuiteId))
                        {
                            hashset_testcaseIDs = result[testSuiteId].TestCases;
                            currentItem = result[testSuiteId];
                        }
                        else
                        {
                            currentItem.TestSuiteId = testSuiteId;
                            currentItem.TestCases = hashset_testcaseIDs;
                            result.Add(testSuiteId, currentItem);
                        }
                        currentItem.TestPlanId = testplanId;
                        currentItem.Rev = testSuiteMissingHistoryGroup.Max(c => c.Rev);

                        foreach (var item in testSuiteMissingHistoryGroup.OrderBy(c => c.Rev))
                        {
                            string testSuiteAudit = "";
                            if (testSuiteAudit != item.Test_Suite_Audit)
                            {
                                testSuiteAudit = item.Test_Suite_Audit;
                                string operation = "";
                                List<int> testcaseIds = Utils.GetListOfTestCaseIds(testSuiteAudit, out operation);
                                if (operation == "Added")
                                {
                                    foreach (var id in testcaseIds)
                                    {
                                        hashset_testcaseIDs.Add(id);
                                    }
                                }
                                else if (operation == "Removed")
                                {
                                    foreach (var id in testcaseIds)
                                    {
                                        hashset_testcaseIDs.Remove(id);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public ConcurrentDictionary<int, TestSuiteRevCases> GetTestCaseIDsByDynamicTestSuiteID(ConcurrentDictionary<int, int> dict_testsuiteAndTestPlan)
        {
            using (var dbContext = new VsoWorkItemsContext())
            {
                var hashset_testSuiteInfo = new HashSet<TestSuiteInfo>(dict_testsuiteAndTestPlan.Select(c => new TestSuiteInfo { ID = c.Key }));
                var testCasesMapping = Utils.FetchData(dbContext, null,
                    query: @"SELECT a.ID, a.rev,  a.[Query Text]
                        from [TestSuiteRevisions] a
                        JOIN (SELECT ID, max(rev) rev
                        FROM [TestSuiteRevisions]
                        where [test suite type] = 'query based' and [Query Text] is not null
                        group by id ) b
                        on a.ID = b.ID and a.rev = b.rev",
                    convertFunction: (reader) =>
                    {
                        var testSuiteInfo = new TestSuiteInfo();
                        testSuiteInfo.ID = (int)reader.GetInt64(0);
                        testSuiteInfo.Rev = reader.GetInt32(1);
                        testSuiteInfo.Query_Text = reader.GetString(2);
                        return testSuiteInfo;
                    }
                   );
                var hashset_testCasesMapping = new HashSet<TestSuiteInfo>(testCasesMapping, new TestSuiteInfoComparer());
                hashset_testCasesMapping.IntersectWith(hashset_testSuiteInfo);

                IEnumerable<IGrouping<string, TestSuiteInfo>> groupBy_queryText = hashset_testCasesMapping.GroupBy(c => c.Query_Text);
                var dict_testSuite_testCase = this.GetTestCasesInParralel(groupBy_queryText, dict_testsuiteAndTestPlan);
                return dict_testSuite_testCase;
            }
        }

        private ConcurrentDictionary<int, TestSuiteRevCases> GetTestCasesInParralel(IEnumerable<IGrouping<string, TestSuiteInfo>> groupByResult, ConcurrentDictionary<int, int> dict_testSuiteTestplan)
        {
            Console.WriteLine("Start Getting Dynamic Suites");
            var producer = new BlockingCollection<IGrouping<string, TestSuiteInfo>>();
            var result = new ConcurrentDictionary<int, TestSuiteRevCases>();
            //output in the console

            int failed_testSuiteIds_count = 0;
            var failed_testSuitesByGroup = new BlockingCollection<IGrouping<string, TestSuiteInfo>>();
            BlockingCollection<int> counter = new BlockingCollection<int>();

            foreach (var item in groupByResult)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();

            var action = new Action(() =>
            {
                IGrouping<string, TestSuiteInfo> itemToProcess = null;
                while (producer.TryTake(out itemToProcess))
                {
                    string queryText = itemToProcess.Key;

                    try
                    {
                        var dict_testcaseids = this.VsoContext.RunQuery(base.ProjectName, queryText);
                        var hashset_testcaseids = new HashSet<int>(dict_testcaseids.Keys);
                        foreach (var item in itemToProcess)
                        {
                            int testSuitId = item.ID;
                            int rev = item.Rev;
                            int testplanId = dict_testSuiteTestplan[testSuitId];
                            //if (!result.ContainsKey(testSuitId))
                            //{
                            result.TryAdd(testSuitId, new TestSuiteRevCases
                            {
                                TestPlanId = testplanId,
                                TestCases = hashset_testcaseids,
                                TestSuiteId = testSuitId,
                                Rev = rev
                            });
                            //}
                        }
                        if (result.Count > (counter.Count + 1) * 1000)
                        {
                            counter.TryAdd(0);
                            Console.WriteLine(DateTime.Now.ToString() + " " + result.Count + "( failed:" + failed_testSuiteIds_count + " )");
                        }
                    }
                    catch (Exception)
                    {
                        failed_testSuitesByGroup.TryAdd(itemToProcess);
                        failed_testSuiteIds_count += itemToProcess.Count();
                    }
                }
            });

            //3 Start our process in parallel
            int totalThread = 20;
            //var p = new Action[totalThread];
            var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
            Parallel.Invoke(p);

            //4.deal with failed testSuites by group
            foreach (var falied_testSuites in failed_testSuitesByGroup)
            {
                try
                {
                    int sampleTestSuiteId = falied_testSuites.FirstOrDefault().ID;
                    int sampleTestPlanId = dict_testSuiteTestplan[sampleTestSuiteId];
                    List<int> list_testcases = this.VsoContext.GetTestCaseIDsBySuiteID(base.ProjectName, sampleTestPlanId, sampleTestSuiteId).Values.ToList()[0];
                    //deal with every suite in a group
                    foreach (var item in falied_testSuites)
                    {
                        int testSuiteId = item.ID;
                        int rev = item.Rev;
                        int testPlanId = dict_testSuiteTestplan[testSuiteId];
                        result.TryAdd(item.ID, new TestSuiteRevCases
                        {
                            TestPlanId = testPlanId,
                            TestSuiteId = testSuiteId,
                            TestCases = new HashSet<int>(list_testcases),
                            Rev = rev
                        });
                        Console.WriteLine(DateTime.Now.ToString() + "( Fixed failed testSuiteID: " + testSuiteId + " )");
                    }
                }
                catch (Exception)
                {
                }
            }

            return result;
        }

        public override HashSet<TestSuiteTestCaseMapping> PrepareDbItems(JObject workItems)
        {
            return HashsetMapping;
        }

        public override HashSet<TestSuiteTestCaseMapping> UpdateDatabase(HashSet<TestSuiteTestCaseMapping> workItems)
        {
            HashSet<TestSuiteTestCaseMapping> result = new HashSet<TestSuiteTestCaseMapping>();
            using (var dbContext = new VsoWorkItemsContext())
            {
                using (var dbContextTransaction = dbContext.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //delete data from table
                        var deleteQuery = "truncate table TestSuiteTestCaseMapping";
                        int iRow = base.DeleteData(dbContext, dbContextTransaction.UnderlyingTransaction, deleteQuery);
                        //insert new data
                        base.InsertData(dbContext, dbContextTransaction.UnderlyingTransaction, workItems, "TestSuiteTestCaseMapping");

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }
    }
}
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;
using VsoWorkItemsSync.Core;
using VsoWorkItemsSync.Helper;
using VsoWorkItemsSync.WorkItemsProvider;

namespace VsoWorkItemsSync
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var test = new VsoWorkItemsSync.Model.VsoWorkItemsContext();
            var option = new Options();
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, option))
                {
                    Logger.LogStart();
                    DateTime? fromDate = null;
                    if (option.SyncDays != 0)
                    {
                        fromDate = DateTime.Today.AddDays(option.SyncDays * (-1));
                        Logger.LogMessage(string.Format("Getting sync FromDate..."));
                    }
                    if (option.SyncEpics)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Epics..."));
                        SyncWithVso(new EpicProvider(fromDate));
                    }

                    if (option.SyncBugs)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Bugs..."));
                        SyncWithVso(new BugProvider(fromDate));
                    }
                    if (option.SyncImpediments)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Impediments..."));
                        SyncWithVso(new ImpedimentProvider(fromDate));
                    }
                    if (option.SyncLinks)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Links..."));
                        SyncWithVso(new LinkProvider(fromDate));
                    }
                    if (option.SyncEnablingSpecifications)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Enabling Specifications..."));
                        SyncWithVso(new EnablingSpecificationProvider(fromDate));
                    }
                    if (option.SyncTasks)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Tasks..."));
                        SyncWithVso(new TaskProvider(fromDate));
                    }
                    if (option.SyncTestCases)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Test Cases..."));
                        SyncWithVso(new TestCaseProvider(fromDate));
                    }
                    if (option.SyncTestPlans)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Test Plans..."));
                        SyncWithVso(new TestPlanProvider(fromDate));
                    }
                    if (option.SyncTestSuites)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Test Suites..."));
                        SyncWithVso(new TestSuiteProvider(fromDate));
                    }
                    if (option.SyncTestRunResults)
                    {
                        Logger.LogMessage(string.Format("Starting syncing TestSuitesTestCasesMapping ..."));
                        SyncWithVso(new TestSuiteTestCaseMappingProvider3(fromDate));
                        GC.Collect();
                        Logger.LogMessage(string.Format("Starting syncing Test Run Results..."));
                        SyncWithVso(new TestRunResultProvider(fromDate));
                    }
                    if (option.SyncCoreBugInfos)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Core Bug Infos..."));
                        SyncWithVso(new CoreBugProvider(fromDate));
                    }
                    if (option.SyncCoreBugFromFeedbackInfos)
                    {
                        Logger.LogMessage(string.Format("Starting syncing Core Bug Infos from feedback query..."));
                        SyncWithVso(new CoreBugFromFeedbackProvider(fromDate));
                    }
                    if (option.SyncWorkItemAttachmentInfos)
                    {
                        Logger.LogMessage(string.Format("Starting syncing WorkItemAttachment Infos..."));
                        SyncWithVso(new WorkItemAttachmentProvider(fromDate));
                    }

                    Logger.LogMessage(string.Format("Update done."));
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                throw;
            }
            finally
            {
                Logger.LogEnd();
            }
        }

        private static void SyncWithVso<T>(WorkItemProviderBase<T> vsoProvider) where T : class
        {
            //1 - Get the propper provider based on command line argument
            var workItemProvider = vsoProvider;

            //2 - Get all interesting vso work items
            Logger.LogMessage(string.Format("Getting vso work items..."));
            var vsoItems = workItemProvider.GetWorkItems();

            //3 - Prepare new data to be inserted to db
            var newItemsToCreate = workItemProvider.PrepareDbItems(vsoItems);
            Logger.LogMessage(string.Format("Total items found: {0}. Comparing with db data...", newItemsToCreate.Count()));

            //4 - Update db
            workItemProvider.UpdateDatabase(newItemsToCreate);
            Logger.LogMessage(string.Format("{0} missing items created.", newItemsToCreate.Count()));
            Logger.LogMessage(string.Format("--"));
        }
    }
}
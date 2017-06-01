using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.SyncTool.Core;
using SkypeIntlPortfolio.SyncTool.SyncActionProvider.Git;
using SkypeIntlPortfolio.SyncTool.SyncActionProvider.VsoWorkItem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.SyncTool
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var logger = new Logger();
            var exceptions = new List<Exception>();

            //setup list of actions to run
            var syncActions = new List<ISyncActionProvider>();
            var syncGitDocAction = new SyncGitDocumentationAction();
            var syncReleaseAction = new SyncReleaseAction(logger);
            var syncTestPlanAction = new SyncTestPlanAction(logger);
            syncActions.Add(new SyncActionLogDecorator(logger, syncGitDocAction));
            syncActions.Add(new SyncActionLogDecorator(logger, syncReleaseAction));
            syncActions.Add(new SyncActionLogDecorator(logger, syncTestPlanAction));

            logger.LogStart();
            //execute each action, and log errors
            foreach (var action in syncActions)
            {
                try
                {
                    action.Sync();
                    logger.LogMessage("-  -  -");
                }
                catch (Exception e)
                {
                    logger.LogException(e);
                }
            }

            logger.LogEnd();

            //if there are any exceptions that occured, throw it to the console
            if (exceptions.Any())
                throw new AggregateException(exceptions);
        }
    }
}
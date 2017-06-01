using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.SyncTool.Core
{
    public class SyncActionLogDecorator : ISyncActionProvider
    {
        private readonly Logger logger;
        private readonly ISyncActionProvider action;

        public string ActionName
        {
            get
            {
                return action.ActionName;
            }
        }

        public SyncActionLogDecorator(Logger logger, ISyncActionProvider action)
        {
            this.logger = logger;
            this.action = action;
        }

        public void Sync()
        {
            logger.LogMessage(string.Format("Starting {0}..", this.ActionName));
            action.Sync();
            logger.LogMessage(string.Format(".. {0} done.", this.ActionName));
        }
    }
}
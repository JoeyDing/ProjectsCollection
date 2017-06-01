using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.SyncTool.Core
{
    public interface ISyncActionProvider
    {
        string ActionName { get; }

        void Sync();
    }
}
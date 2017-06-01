using SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships.JobRelatedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships
{
    public interface IToolRelationshipsView
    {
        event Func<IEnumerable<ToolName>> GetToolsInfo;

        event Func<string, IEnumerable<JobName>> GetJobsInfo;

        event Func<string, IEnumerable<StepName>> GetStepsInfo;
    }
}
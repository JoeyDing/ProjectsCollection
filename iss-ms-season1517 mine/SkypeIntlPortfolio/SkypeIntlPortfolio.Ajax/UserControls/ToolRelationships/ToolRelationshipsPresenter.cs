using SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships.JobRelatedClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships
{
    public class ToolRelationshipsPresenter
    {
        private IToolRelationshipsView _toolRelationshipsView;

        public ToolRelationshipsPresenter(IToolRelationshipsView toolRelationshipsView)
        {
            this._toolRelationshipsView = toolRelationshipsView;
            this._toolRelationshipsView.GetToolsInfo += _toolRelationshipsView_GetToolsInfo;
            this._toolRelationshipsView.GetJobsInfo += _toolRelationshipsView_GetJobsInfo;
            this._toolRelationshipsView.GetStepsInfo += _toolRelationshipsView_GetStepsInfo;
        }

        private IEnumerable<ToolName> _toolRelationshipsView_GetToolsInfo()
        {
            using (SkypeIntlMonitoringEntities db = new SkypeIntlMonitoringEntities())
            {
                return db.vw_ToolsJobsStepsInfo.Select(x =>
                    new ToolName
                    {
                        Tool_Name = x.Tool_Name
                    }
                    ).Distinct().ToList();
            }
        }

        private IEnumerable<JobName> _toolRelationshipsView_GetJobsInfo(string toolKey)
        {
            using (SkypeIntlMonitoringEntities db = new SkypeIntlMonitoringEntities())
            {
                return db.vw_ToolsJobsStepsInfo.Where(x => x.Tool_Name == toolKey).Select(x =>
                        new JobName
                        {
                            Job_Name = x.Job_Name
                        }
                    ).Distinct().ToList();
            }
        }

        private IEnumerable<StepName> _toolRelationshipsView_GetStepsInfo(string jobKey)
        {
            using (SkypeIntlMonitoringEntities db = new SkypeIntlMonitoringEntities())
            {
                return db.vw_ToolsJobsStepsInfo.Where(x => x.Job_Name == jobKey).Select(x =>
                    new StepName
                    {
                        Step_Name = x.Step_Name
                    }
                    ).Distinct().ToList();
            }
        }
    }
}
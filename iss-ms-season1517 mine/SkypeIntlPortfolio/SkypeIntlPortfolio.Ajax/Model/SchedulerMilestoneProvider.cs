using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class SchedulerMilestoneProvider
    {
        private MilestoneInfo[] _milestones;

        public DateTime? MaxDate { get; set; }

        public DateTime? MinDate { get; set; }

        public SchedulerMilestoneProvider(MilestoneInfo[] productInfos)
        {
            if (productInfos == null)
                throw new ArgumentNullException("parameter \"productKeys\" cannot be null");
            this._milestones = productInfos;

            this.Init();
        }

        private void Init()
        {
            //using (var context = new SkypeIntlPlanningPortfolioEntities())
            //{
            //    this._milestones.AddRange(context.Milestones.Where(c => _productKeys.Contains(c.ProductKey))
            //        .Select(c => new MilestoneInfo
            //        {
            //            //ProductKey = c.ProductKey,
            //            // ProjectKey = c.ProjectKey,
            //            // MilestoneKey = c.MilestoneKey,
            //            MilestoneCategoryKey = c.MilestoneCategoryKey,
            //            // Project_Name = c.Project.Project_Name,
            //            Milestone_Name = c.Milestone_Name,
            //            Milestone_Start_Date = c.Milestone_Start_Date,
            //            Milestone_End_Date = c.Milestone_End_Date,
            //            MilestoneCategoryName = c.MilestoneCategory.Milestone_Category_Name
            //        }));
            //}
        }

        public List<MilestoneInfo> GetData()
        {
            return null;
            //return _milestones;
        }
    }
}
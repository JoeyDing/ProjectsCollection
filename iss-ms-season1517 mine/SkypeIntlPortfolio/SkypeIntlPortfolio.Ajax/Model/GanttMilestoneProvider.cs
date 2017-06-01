using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using Telerik.Web.UI.Gantt;

namespace SkypeIntlPortfolio.Ajax.Model
{
    public class MilestoneExtended
    {
        public string Product_Name { get; set; }

        public string Release_Name { get; set; }

        public string Milestone_Name { get; set; }

        public DateTime? Milestone_Start_Date { get; set; }

        public DateTime? Milestone_End_Date { get; set; }
    }

    public class GanttMilestoneProvider : GanttProviderBase
    {
        private List<MilestoneExtended> _milestones;
        private List<IDependency> _dependencies;
        private List<ITask> _tasks;

        public GanttMilestoneProvider(int[] products)
            : base()
        {
            _milestones = new List<MilestoneExtended>();
            _dependencies = new List<IDependency>();
            _tasks = new List<ITask>();
            if (products != null && products.Any())
            {
                using (var context = new SkypeIntlPlanningPortfolioEntities())
                {
                    this._milestones.AddRange(context.Milestones.Where(c => products.Contains(c.ProductKey))
                        .Select(c => new MilestoneExtended
                        {
                            Product_Name = c.Product.Product_Name,
                            Release_Name = c.Release.VSO_Title,
                            Milestone_Name = c.Milestone_Name,
                            Milestone_Start_Date = c.Milestone_Start_Date,
                            Milestone_End_Date = c.Milestone_End_Date,
                        }));
                }
            }

            this.LoadTasksAndDependencies();
        }

        private void LoadTasksAndDependencies()
        {
            this._tasks.Clear();
            this._dependencies.Clear();

            int i = 1;
            foreach (var productGroup in _milestones.GroupBy(c => c.Product_Name))
            {
                var productMinDate = productGroup.Min(c => c.Milestone_Start_Date).Value.Date;
                var productMaxDate = productGroup.Max(c => c.Milestone_End_Date).Value.Date.AddDays(1);
                Task productTask = new Task
                {
                    ID = i,
                    ParentID = null,
                    OrderID = 0,
                    PercentComplete = 100,
                    Title = productGroup.Key,
                    Start = productMinDate,
                    End = productMaxDate,
                    Expanded = true,
                    Summary = true,
                };

                this._tasks.Add(productTask);

                int k = 1;
                foreach (var releaseGroup in productGroup.GroupBy(c => c.Release_Name))
                {
                    var releaseMinDate = releaseGroup.Min(c => c.Milestone_Start_Date).Value.Date;
                    var releaseMaxDate = releaseGroup.Max(c => c.Milestone_End_Date).Value.Date.AddDays(1);
                    Task releaseTask = new Task
                    {
                        ID = int.Parse(string.Format("{0}{1}", i, k)),
                        ParentID = productTask.ID,
                        OrderID = k,
                        PercentComplete = 100,
                        Title = releaseGroup.Key,
                        Start = releaseMinDate,
                        End = releaseMaxDate,
                        Expanded = true,
                        Summary = true
                    };

                    this._tasks.Add(releaseTask);

                    var releaseDep = new Dependency
                    {
                        ID = releaseTask.ID,
                        PredecessorID = releaseTask.ParentID,
                        SuccessorID = releaseTask.ID,
                        Type = DependencyType.StartStart
                    };

                    _dependencies.Add(releaseDep);

                    int l = 1;
                    foreach (var milestone in releaseGroup)
                    {
                        Task milestoneTask = new Task
                        {
                            ID = int.Parse(string.Format("{0}{1}{2}", i, k, l)),
                            ParentID = releaseTask.ID,
                            OrderID = k,
                            PercentComplete = 100,
                            Title = milestone.Milestone_Name,
                            Start = milestone.Milestone_Start_Date.Value.Date,
                            End = milestone.Milestone_End_Date.Value.Date.AddDays(1),
                            Expanded = true,
                            Summary = false
                        };

                        this._tasks.Add(milestoneTask);

                        var milestoneDep = new Dependency
                        {
                            ID = milestoneTask.ID,
                            PredecessorID = milestoneTask.ParentID,
                            SuccessorID = milestoneTask.ID,
                            Type = DependencyType.StartStart
                        };

                        _dependencies.Add(milestoneDep);

                        l++;
                    }

                    k++;
                }
            }
            i++;
        }

        public override List<ITask> GetTasks()
        {
            //var result = new List<ITask>() {
            //    new Task {
            //        ID = 100,
            //        ParentID = null,
            //        OrderID = 0,
            //        PercentComplete = 100,
            //        Title = "product",
            //        Start = DateTime.Now.Date,
            //        End = DateTime.Now.Date.AddDays(61),
            //        Expanded= true,
            //        Summary = true
            //    },
            //    new Task {
            //        ID = 101,
            //        ParentID = 100,
            //        OrderID = 0,
            //        PercentComplete = 100,
            //        Title = "product",
            //        Start = DateTime.Now.Date.AddDays(30),
            //        End = DateTime.Now.Date.AddDays(61),
            //        Expanded= true,
            //        Summary = true
            //    },
            //    new Task {
            //        ID = 102,
            //        ParentID = 101,
            //        OrderID = 0,
            //        PercentComplete = 100,
            //        Title = "milestone 1",
            //        Start = DateTime.Now.Date.AddDays(30),
            //        End = DateTime.Now.Date.AddDays(31),
            //        Expanded= true,
            //        Summary= false,
            //    },
            //    new Task {
            //        ID = 103,
            //        ParentID = 101,
            //        OrderID = 0,
            //        PercentComplete = 100,
            //        Title = "milestone 2",
            //        Start = DateTime.Now.Date.AddDays(60),
            //        End = DateTime.Now.Date.AddDays(61),
            //        Expanded= true,
            //        Summary= false,
            //    }
            //};

            return this._tasks;
        }

        public override ITask UpdateTask(ITask task)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public override ITask DeleteTask(ITask task)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public override ITask InsertTask(ITask task)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public override List<IDependency> GetDependencies()
        {
            //return new List<IDependency>() {
            //     new Dependency
            //            {
            //                ID =101,
            //                PredecessorID= 100,
            //                SuccessorID = 101,
            //                Type=DependencyType.StartStart
            //            },
            //    new Dependency
            //            {
            //                ID =102,
            //                PredecessorID= 101,
            //                SuccessorID = 102,
            //                Type=DependencyType.StartStart
            //            },
            //    new Dependency
            //    {
            //        ID =102,
            //        PredecessorID= 102,
            //        SuccessorID = 103,
            //        Type=DependencyType.StartStart
            //    },
            //};

            return this._dependencies;
            //return new List<IDependency>();
        }
    }
}
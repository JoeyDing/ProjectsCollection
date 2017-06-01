using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.LOS
{
    public class LineOfSightPresenter
    {
        private ILineOfSightView view;

        public LineOfSightPresenter(ILineOfSightView view)
        {
            this.view = view;
            this.view.GetAllProductKeys += View_GetAllProductKeys;
            this.view.GetVacationInfoesByID += View_GetVacationInfoesByID;
            this.view.GetAffectedProductsByVacID += View_GetAffectedProductsByVacID;
            this.view.GetAffectedLocationsIDsByVacationID += View_GetAffectedLocationsIDsByVacationID;
            this.view.GetProductsWithCheckedLocations += View_GetProductsWithCheckedLocations;
            this.view.GetVacationRelatedInfoList += View_GetVacationRelatedInfoList;
            this.view.UpdateMileStone += View_UpdateMileStone;
            this.view.UpdateRelease += View_UpdateRelease;
            this.view.UpdateTestPlan += View_UpdateTestPlan;
            this.view.UpdateVacation += View_UpdateVacation;
            this.view.UpdateVacationForDragAndDrop += View_UpdateVacationForDragAndDrop;
            this.view.UpdateMileStoneForDragAndDrop += View_UpdateMileStoneForDragAndDrop;
            this.view.UpdateTestPlanForDragAndDrop += View_UpdateTestPlanForDragAndDrop;
            this.view.UpdateReleaseForDragAndDrop += View_UpdateReleaseForDragAndDrop;
        }

        private void View_UpdateReleaseForDragAndDrop(Release rl)
        {
            using (SkypeIntlPlanningPortfolioEntities context = new SkypeIntlPlanningPortfolioEntities())
            {
                Release releaseObject = context.Releases.FirstOrDefault(r => r.VSO_ID == rl.VSO_ID);
                releaseObject.VSO_LocStartDate = rl.VSO_LocStartDate;
                releaseObject.VSO_DueDate = rl.VSO_DueDate;
                context.SaveChanges();
            }
        }

        private void View_UpdateTestPlanForDragAndDrop(TestSchedule ts)
        {
            using (SkypeIntlPlanningPortfolioEntities context = new SkypeIntlPlanningPortfolioEntities())
            {
                TestSchedule testObject = context.TestSchedules.FirstOrDefault(t => t.TestScheduleKey == ts.TestScheduleKey);
                testObject.TestSchedule_Start_Date = ts.TestSchedule_Start_Date;
                testObject.TestSchedule_End_Date = ts.TestSchedule_End_Date;
                context.SaveChanges();
            }
        }

        private void View_UpdateMileStoneForDragAndDrop(Milestone ms)
        {
            using (SkypeIntlPlanningPortfolioEntities context = new SkypeIntlPlanningPortfolioEntities())
            {
                Milestone milestoneObject = context.Milestones.FirstOrDefault(m => m.MilestoneKey == ms.MilestoneKey);
                milestoneObject.Milestone_Start_Date = ms.Milestone_Start_Date;
                milestoneObject.Milestone_End_Date = ms.Milestone_End_Date;
                context.SaveChanges();
            }
        }

        private void View_UpdateVacationForDragAndDrop(VacationInfo vac)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var vacationInfo = context.VacationInfoes.First(c => c.VacationID == vac.VacationID);
                vacationInfo.VacationStartDate = vac.VacationStartDate;
                vacationInfo.VacationEndDate = vac.VacationEndDate;
                context.SaveChanges();
            }
        }

        private List<int> View_GetAllProductKeys()
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.Products_New.AsNoTracking().Select(c => c.ProductKey).ToList();
                return result;
            }
        }

        private void View_UpdateVacation(VacationInfo vacationInfos, IEnumerable<int> affectedProductsId, List<int> affectedLocationIds)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                //this line of code load vacation and its navigation which is products_new and location to context(eger loading)
                var vacation = context.VacationInfoes.First(v => v.VacationID == vacationInfos.VacationID);

                //get products as a dict from the context since later in the remove we can not use attach(product entity with same key in the context)
                var existedProductDict = vacation.Products_New.ToDictionary(c => c.ProductKey, c => c);
                var existedProductIds = existedProductDict.Keys;

                var prosRemoved = existedProductIds.Except(affectedProductsId).ToList();
                var prosInserted = affectedProductsId.Except(existedProductIds).ToList();
                //var p = context.ChangeTracker.Entries<Products_New>().ToDictionary(c=>c.Entity.ProductKey,c=>c.Entity);
                foreach (int proId in prosRemoved)
                {
                    var removedPro = existedProductDict[proId];
                    //var removedPro = new Products_New { ProductKey = proId };
                    //context.Products_New.Attach(removedPro);
                    vacation.Products_New.Remove(removedPro);
                }

                foreach (int proId in prosInserted)
                {
                    var addedPro = new Products_New { ProductKey = proId };
                    context.Products_New.Attach(addedPro);
                    vacation.Products_New.Add(addedPro);
                }

                //update mappingtable between location and vacation
                var existedLocation = vacation.Locations.ToDictionary(c => c.LocationKey, c => c);
                var existedLocationIds = existedLocation.Keys;
                var locationIdsToAdd = affectedLocationIds.Except(existedLocationIds).ToList();
                var locationIdsToRemove = existedLocationIds.Except(affectedLocationIds).ToList();

                foreach (var id in locationIdsToAdd)
                {
                    var locToAdd = new Location { LocationKey = id };
                    context.Locations.Attach(locToAdd);
                    vacation.Locations.Add(locToAdd);
                }

                foreach (var id in locationIdsToRemove)
                {
                    var locToRemove = existedLocation[id];
                    //var locToRemove = new Location { LocationKey = id };
                    //context.Locations.Attach(locToRemove);
                    vacation.Locations.Remove(locToRemove);
                }

                vacation.VacationName = vacationInfos.VacationName;
                vacation.VacationDescription = vacationInfos.VacationDescription;
                vacation.VacationStartDate = vacationInfos.VacationStartDate;
                vacation.VacationEndDate = vacationInfos.VacationEndDate;
                vacation.UICategoryID = vacationInfos.UICategoryID;

                context.SaveChanges();
            }
        }

        private void View_UpdateTestPlan(TestSchedule ts, string categoriesName)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                TestSchedule testObject = context.TestSchedules.FirstOrDefault(t => t.TestScheduleKey == ts.TestScheduleKey);

                testObject.TestSchedule_Name = ts.TestSchedule_Name;

                int categoryKey = context.MilestoneCategories.FirstOrDefault(m => m.Milestone_Category_Name == categoriesName).MilestoneCategoryKey;
                testObject.MilestoneCategoryKey = categoryKey;

                testObject.TestSchedule_Start_Date = ts.TestSchedule_Start_Date;
                testObject.TestSchedule_End_Date = ts.TestSchedule_End_Date;
                testObject.AssignedResources = ts.AssignedResources;

                context.SaveChanges();
            }
        }

        private void View_UpdateRelease(Release rl)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                Release releaseObject = context.Releases.FirstOrDefault(r => r.VSO_ID == rl.VSO_ID);
                releaseObject.VSO_Title = rl.VSO_Title;
                releaseObject.VSO_LocStartDate = rl.VSO_LocStartDate;
                releaseObject.VSO_DueDate = rl.VSO_DueDate;

                context.SaveChanges();
            }
        }

        private void View_UpdateMileStone(Milestone ms, string categoriesName)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var milestoneObject = context.Milestones.FirstOrDefault(m => m.MilestoneKey == ms.MilestoneKey);
                int categoryKey = context.MilestoneCategories.FirstOrDefault(m => m.Milestone_Category_Name == categoriesName).MilestoneCategoryKey;
                milestoneObject.Milestone_Name = ms.Milestone_Name;
                milestoneObject.MilestoneCategoryKey = categoryKey;
                milestoneObject.Milestone_Start_Date = ms.Milestone_Start_Date;
                milestoneObject.Milestone_End_Date = ms.Milestone_End_Date;

                context.SaveChanges();
            }
        }

        private List<VacationRelatedInfo> View_GetVacationRelatedInfoList()
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var vacInfos = db.VacationInfoes;
                var result = new List<VacationRelatedInfo>();
                foreach (var vac in vacInfos)
                {
                    int uiCateID = vac.UICategoryID;
                    switch (uiCateID)
                    {
                        case 1:
                            result.Add(new VacationRelatedInfo
                            {
                                VacationDescription = vac.VacationDescription,
                                VacationID = vac.VacationID,
                                VacationName = vac.VacationName,
                                VacationStartDate = vac.VacationStartDate,
                                VacationEndDate = vac.VacationEndDate,
                                //ProductsAffected = String.Join(String.Empty, c.Products_New.Select(x => x.Product_Name).ToList().ToArray()),
                                ProductsAffected = String.Join(String.Empty, vac.Products_New.Select(x => string.Format("<b>{0};</b>", x.Product_Name)).ToList().ToArray()).Replace(";", "<br />"),
                                PeopleAffected = String.Join(String.Empty, vac.Products_New.Select(s => new { s.Product_Name, s.Product_Owner, s.IPE_Owner, s.Test_Owner }).Distinct().ToList()
                                                                                              .Select(x => string.Format("<b>Product Name:{0}</b>;Product Owner:{1};IPE Owner:{2};Test Owner:{3};", x.Product_Name, x.Product_Owner, x.IPE_Owner, x.Test_Owner)).ToList().ToArray()).Replace(";", "<br />"),
                                UICategoryID = uiCateID
                            });
                            break;

                        case 2:
                            result.Add(new VacationRelatedInfo
                            {
                                VacationDescription = vac.VacationDescription,
                                VacationID = vac.VacationID,
                                VacationName = vac.VacationName,
                                VacationStartDate = vac.VacationStartDate,
                                VacationEndDate = vac.VacationEndDate,
                                //ProductsAffected = String.Join(String.Empty, c.Products_New.Select(x => x.Product_Name).ToList().ToArray()),
                                ProductsAffected = string.Format("<b>Location:{0}</b>", string.Join(";", vac.Locations.Select(c => string.Format("<b>{0}</b>", c.Location1)))),
                                UICategoryID = uiCateID
                            });
                            break;

                        case 3:
                            result.Add(new VacationRelatedInfo
                            {
                                VacationDescription = vac.VacationDescription,
                                VacationID = vac.VacationID,
                                VacationName = vac.VacationName,
                                VacationStartDate = vac.VacationStartDate,
                                VacationEndDate = vac.VacationEndDate,
                                //ProductsAffected = String.Join(String.Empty, c.Products_New.Select(x => x.Product_Name).ToList().ToArray()),
                                ProductsAffected = "<b>All Products</b>",
                                UICategoryID = uiCateID
                            });
                            break;
                    }
                }
                return result;
            }
        }

        private Dictionary<int, string> View_GetProductsWithCheckedLocations(IEnumerable<string> selectedLocations)
        {
            using (SkypeIntlPlanningPortfolioEntities context = new SkypeIntlPlanningPortfolioEntities())
            {
                var selectedProducts = context.Products_New.Where(c => selectedLocations.Contains(c.Location.Location1)).ToDictionary(c => c.ProductKey, c => c.Product_Name);
                return selectedProducts;
            }
        }

        private List<int> View_GetAffectedLocationsIDsByVacationID(int vacationID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = context.VacationInfoes.First(c => c.VacationID == vacationID).Locations.Select(c => c.LocationKey).ToList();
                return result;
            }
        }

        private Dictionary<string, string> View_GetAffectedProductsByVacID(int vacationID)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.VacationInfoes.First(c => c.VacationID == vacationID).Products_New.ToDictionary(x => x.ProductKey.ToString(), x => x.Product_Name);
                return result;
            }
        }

        private VacationInfo View_GetVacationInfoesByID(int vacationID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var vacationInfo = context.VacationInfoes.AsNoTracking().First(c => c.VacationID == vacationID);
                return vacationInfo;
            }
        }
    }
}
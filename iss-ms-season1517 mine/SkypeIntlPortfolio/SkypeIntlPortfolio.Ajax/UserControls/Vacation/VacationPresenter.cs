using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using SkypeIntlPortfolio.Ajax.UserControls.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.UserControls.Vacation
{
    public class VacationPresenter
    {
        private IVacationView _view;
        private VsoContext vsoContext;

        public VacationPresenter()
        {
        }

        public VacationPresenter(IVacationView view)
        {
            this.vsoContext = Utils.GetVsoContext();
            this._view = view;
            this._view.InsertVacationInfo += _view_InsertVacationInfo;
            this._view.UpdateVacationInfo += _view_UpdateVacationInfo;
            this._view.DeleteVacationInfo += _view_DeleteVacationInfo;
            this._view.GetVacationAffectedProductsByVacationID += _view_GetVacationAffectedProductsByVacationID;
            this._view.GetAllProducts += _view_GetAllProducts;
            this._view.GetAffectedLocationsIDsByVacationID += _view_GetAffectedLocationsIDsByVacationID;
            this._view.GetAffectedPeopleByProductIDs += _view_GetAffectedPeopleByProductIDs;
            this._view.GetAffectedProductsByPeopleNames += _view_GetAffectedProductsByPeopleNames;
            this._view.GetUpdatedAffectedPeopleByProductIDs += _view_GetUpdatedAffectedPeopleByProductIDs;
        }

        private List<AffectedInfo> _view_GetAffectedProductsByPeopleNames(Dictionary<int, string> existingProducts, List<string> peopleNames)
        {
            List<AffectedInfo> result = new List<AffectedInfo>();
            VacationInfoService vacationInfoService = new VacationInfoService();
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                //1.add existing products into the list firstly
                foreach (var ePro in existingProducts)
                {
                    result.Add(new AffectedInfo { ProductKey = ePro.Key, ProductName = ePro.Value });
                }
                List<List<AffectedInfo>> productAffectedList = vacationInfoService.GetProductsAffectedByPeopleNames(peopleNames);
                List<AffectedInfo> mergeedProductsAffected = productAffectedList.SelectMany(x => x).ToList();
                //2.add remaining products in to list
                foreach (AffectedInfo productInfo in mergeedProductsAffected)
                {
                    if (!result.Any(x => x.ProductKey == productInfo.ProductKey))
                        result.Add(productInfo);
                }
                return result;
            }
        }

        //this one is used to get all the unique affected people based on the selected products "from product autocompletebox"
        private List<AffectedInfo> _view_GetAffectedPeopleByProductIDs(List<string> productIDs)
        {
            List<AffectedInfo> result = new List<AffectedInfo>();
            VacationInfoService vacationInfoService = new VacationInfoService();
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                //get all people affected
                List<List<AffectedInfo>> peopleAffectedList = vacationInfoService.GetPeopleAffectedByProductIDs(productIDs);

                List<AffectedInfo> mergedPeopleList = peopleAffectedList.SelectMany(x => x).ToList();
                foreach (AffectedInfo people in mergedPeopleList)
                {
                    if (!result.Any(x => x.PeopleName == people.PeopleName))
                        result.Add(people);
                }
                return result;
            }
        }

        //the use of this is to update the people selection autocompltebox "after you select people" from the autocompletebox(the result contains exsting list and the list affected by people selected)
        private List<string> _view_GetUpdatedAffectedPeopleByProductIDs(HashSet<string> existingPeopleList, List<string> productIDsList)
        {
            HashSet<string> finalAffectedPeopleList = new HashSet<string>();
            //add existing people into the list firstly
            foreach (string epeo in existingPeopleList)
            {
                finalAffectedPeopleList.Add(epeo);
            }
            VacationInfoService vis = new VacationInfoService();

            List<List<AffectedInfo>> peopleAffectedInfo = vis.GetPeopleAffectedByProductIDs(productIDsList);
            List<AffectedInfo> mergedPeopleList = peopleAffectedInfo.SelectMany(x => x).ToList();
            foreach (AffectedInfo peopleInfo in mergedPeopleList)
            {
                finalAffectedPeopleList.Add(peopleInfo.PeopleName);
            }
            return finalAffectedPeopleList.ToList();
        }

        public List<int> _view_GetAffectedLocationsIDsByVacationID(int vacationID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = context.VacationInfoes.First(c => c.VacationID == vacationID).Locations.Select(c => c.LocationKey).ToList();
                return result;
            }
        }

        public List<int> _view_GetAllProducts()
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.Products_New.AsNoTracking().Select(c => c.ProductKey).ToList();
                return result;
            }
        }

        public Dictionary<string, string> _view_GetVacationAffectedProductsByVacationID(int vacationID)
        {
            return GetAffectedProductsByVacID(vacationID);
        }

        public Dictionary<string, string> GetAffectedProductsByVacID(int vacationID)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = db.VacationInfoes.First(c => c.VacationID == vacationID).Products_New.ToDictionary(x => x.ProductKey.ToString(), x => x.Product_Name);
                return result;
            }
        }

        private void _view_DeleteVacationInfo(int vationInfoID)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                VacationInfo removedVac = db.VacationInfoes.First(x => x.VacationID == vationInfoID);
                //remove products under this vacation first
                if (removedVac.Products_New.Any())
                    removedVac.Products_New.Clear();
                if (removedVac.Locations.Any())
                    removedVac.Locations.Clear();

                db.VacationInfoes.Remove(removedVac);
                db.SaveChanges();
            }
        }

        public void _view_UpdateVacationInfo(VacationInfo vacationInfo, List<int> affectedProductsId, List<int> affectedLocIds)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                UpdateVacation(vacationInfo, affectedProductsId, context, affectedLocIds);

                context.SaveChanges();
            }
        }

        public void UpdateVacation(VacationInfo vacationInfos, IEnumerable<int> affectedProductsId, SkypeIntlPlanningPortfolioEntities context, List<int> affectedLocationIds)
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
        }

        //private void _view_InsertVacationInfo(VacationInfo vacationInfo, List<int> affectedProductsId)
        private void _view_InsertVacationInfo(VacationInfo vacationInfos, List<int> affectedProductsId, List<int> affectedLocationIds)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                foreach (int proId in affectedProductsId)
                {
                    Products_New affectedProduct = new Products_New { ProductKey = proId };
                    db.Products_New.Attach(affectedProduct);
                    affectedProduct.VacationInfoes.Add(vacationInfos);
                    // after this line vacationInfos is in the context
                }

                foreach (var locId in affectedLocationIds)
                {
                    var affctedLocation = new Location { LocationKey = locId };
                    //for mapping table adding, you have to do attach since you want to keep location entity's state as unchanged
                    db.Locations.Attach(affctedLocation);
                    vacationInfos.Locations.Add(affctedLocation);
                }
                db.SaveChanges();
            }
        }
    }
}
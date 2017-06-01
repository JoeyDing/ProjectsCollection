using SkypeIntlPortfolio.Ajax.Core.Interface;
using SkypeIntlPortfolio.Ajax.UserControls;
using SkypeIntlPortfolio.Ajax.UserControls.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Service
{
    public class VacationInfoService : IVacationInfo
    {
        public List<VacationRelatedInfo> GetVacationRelatedInfoList()
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

        public VacationInfo GetVacationInfoesByID(int vacationID)
        {
            using (var context = new SkypeIntlPlanningPortfolioEntities())
            {
                var vacationInfo = context.VacationInfoes.AsNoTracking().First(c => c.VacationID == vacationID);
                return vacationInfo;
            }
        }

        public List<AffectedInfo> GetPeopleAffectedByPeopleName(string searchKey)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                var peopleAffectedList = db.Database.SqlQuery<AffectedInfo>(@"select distinct x.PeopleName from
                (select[product Owner] as PeopleName from Products_New
                union
                select [IPE Owner] as PeopleName from Products_New
                 union
                select [Test Owner] as PeopleName from Products_New) x where x.PeopleName IS NOT NULL and x.PeopleName <> '' and x.PeopleName!='N/A' and x.PeopleName like @p0", "%" + searchKey + "%").ToList();
                return peopleAffectedList;
            }
        }

        public List<List<AffectedInfo>> GetPeopleAffectedByProductIDs(List<string> productIDs)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                List<List<AffectedInfo>> peopleAffectedList = new List<List<AffectedInfo>>();
                foreach (string productID in productIDs)
                {
                    var peopleAffected = db.Database.SqlQuery<AffectedInfo>(@"select distinct x.PeopleName, x.ProductKey, x.ProductName from
                    (select[product Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New
                    union
                    select [IPE Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New
                     union
                    select [Test Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New) x where x.PeopleName IS NOT NULL and x.PeopleName <> '' and x.PeopleName!='N/A' and x.ProductKey = @p0", productID).ToList();
                    peopleAffectedList.Add(peopleAffected);
                }
                return peopleAffectedList;
            }
        }

        public List<List<AffectedInfo>> GetProductsAffectedByPeopleNames(List<string> peopleNames)
        {
            using (var db = new SkypeIntlPlanningPortfolioEntities())
            {
                List<List<AffectedInfo>> productAffectedList = new List<List<AffectedInfo>>();
                foreach (string peopleName in peopleNames)
                {
                    var productAffected = db.Database.SqlQuery<AffectedInfo>(@"select distinct x.PeopleName, x.ProductKey,x.ProductName from
                (select[product Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New
                union
                select [IPE Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New
                 union
                select [Test Owner] as PeopleName, ProductKey,[Product Name] as ProductName from Products_New) x where x.PeopleName IS NOT NULL and x.PeopleName <> '' and x.PeopleName!='N/A' and x.PeopleName = @p0", peopleName).ToList();
                    productAffectedList.Add(productAffected);
                }

                return productAffectedList;
            }
        }
    }
}
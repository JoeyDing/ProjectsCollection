using SkypeIntlPortfolio.SyncTool.Core;
using SkypeIntlPortfolio.SyncTool.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.SyncTool.SyncActionProvider.VsoWorkItem
{
    public class SyncReleaseAction : ISyncActionProvider
    {
        private readonly Logger logger;

        public SyncReleaseAction(Logger logger)
        {
            this.logger = logger;
        }

        public string ActionName
        {
            get
            {
                return "Sync Release";
            }
        }

        public void Sync()
        {
            //1 get Epic tagged as "Loc_Release in VSO

            // 1.1 Get authentication key from AppConfig and use it to instatiate the VSO rest API Wrapper
            string authenticationKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoRootAccount = ConfigurationManager.AppSettings["VsoRootAccount"];

            string projectName = "LOCALIZATION";
            var vsoContext = new VsoContext(vsoRootAccount, authenticationKey);
            // 1.2 Run query to get interesting releases
            var wiql = "SELECT [System.Id]"
                        + " FROM WorkItems"
                        + " WHERE [System.WorkItemType] = 'Epic'"
                        + " and [System.AreaPath] under '" + projectName + "'"
                        + " and [System.Tags] contains 'Loc_Release'"
                        + " and [Microsoft.VSTS.Scheduling.DueDate] <> ''";

            logger.LogMessage("Querying Epics....");
            var vsoIds = vsoContext.RunQuery(projectName, wiql);

            var allWorkItems = new Dictionary<int, WorkItem>();

            if (vsoIds.Any())
            {
                //query the work items (with column) by page as there is a limit of 200 items per query with vso apis
                int pageSize = 200;
                int totalPage = vsoIds.Count / pageSize + (vsoIds.Count % pageSize > 0 ? 1 : 0);
                for (int i = 1; i <= totalPage; i++)
                {
                    var idsToQuery = vsoIds.Keys.Skip(pageSize * (i - 1)).Take(pageSize);
                    var jsonResult = vsoContext.GetListOfWorkItemsByIDs(idsToQuery, new string[] { "System.Id", "System.WorkItemType", "System.Title", "System.IterationPath", "System.AreaPath", "System.Tags", "Microsoft.VSTS.Scheduling.StartDate", "Microsoft.VSTS.Scheduling.DueDate", "System.ChangedDate", "System.AssignedTo", "System.State", "System.CreatedDate" });

                    foreach (var c in jsonResult["value"])
                    {
                        int id = int.Parse((string) c["fields"]["System.Id"]);
                        //check that the product name is specified in the area path
                        //string areaPath = (string)c["fields"]["System.AreaPath"];
                        //var areaSplit = areaPath.Split(new char[] { '\\' });
                        //if (areaSplit.Length == 3)
                        //{
                        //}

                        //old logic to get start date from tags

                        ////check that a locStartDate is specified and it is valid
                        //var tags = ((string)c["fields"]["System.Tags"]).Split(new char[] { ';' }).ToList();
                        //var locStart = tags.FirstOrDefault(x => x.Trim().StartsWith("Loc_ReleaseStartDate:"));
                        //DateTime locstartDateOut;
                        //if no StartDate tag is indicated, use the Created data of Epic
                        //if (locStart != null && DateTime.TryParse(locStart.Split(new string[] { "Loc_ReleaseStartDate:" }, StringSplitOptions.None).Last(), out locstartDateOut))
                        //{
                        //    finalLocstartDate = locstartDateOut;
                        //}

                        //get start date from start date field
                        var startDate = c["fields"]["Microsoft.VSTS.Scheduling.StartDate"];
                        DateTime finalLocstartDate;
                        var tags = ((string) c["fields"]["System.Tags"]).Split(new char[] { ';' }).ToList();

                        //if (locStartDate != null)
                        //{
                        //    finalLocstartDate = locStartDate;
                        //}

                        if (startDate != null)
                        {
                            finalLocstartDate = DateTime.Parse(startDate.ToString());
                        }
                        else
                        {
                            var createdDate = DateTime.Parse((string) c["fields"]["System.CreatedDate"]);
                            //use ".Date" so that LocStart Date starts at "00:00" on the same date
                            finalLocstartDate = createdDate.Date;
                        }

                        //check that due date is bigger than locStartDate
                        var dueDate = DateTime.Parse((string) c["fields"]["Microsoft.VSTS.Scheduling.DueDate"]).AddDays(1).AddSeconds(-1);
                        if (dueDate > finalLocstartDate)
                        {
                            //int id = int.Parse((string)c["fields"]["System.Id"]);
                            var url = string.Format("{0}/DefaultCollection/{1}/_workitems/edit/{2}", vsoContext.VsoUrl, projectName, id);

                            //convert json result to anonymous type
                            allWorkItems.Add(id, new WorkItem
                            {
                                ID = id,
                                Title = (string) c["fields"]["System.Title"],
                                AreaPath = (string) c["fields"]["System.AreaPath"],
                                IterationPath = (string) c["fields"]["System.IterationPath"],
                                WorkItemType = (string) c["fields"]["System.WorkItemType"],
                                Tags = tags,
                                DueDate = dueDate,
                                LocStartDate = finalLocstartDate,
                                //Family = areaSplit[1],
                                //ProductName = areaSplit[2].Trim(),
                                ChangedDate = DateTime.Parse((string) c["fields"]["System.ChangedDate"]).AddDays(1).AddSeconds(-1),
                                AssignedTo = (string) c["fields"]["System.AssignedTo"],
                                State = (string) c["fields"]["System.State"],
                                Url = url
                            });
                        }
                    }
                }
            }

            logger.LogMessage(string.Format("Total valid VSO item Found: {0}.", allWorkItems.Count));
            //2 Update database

            using (var dbContext = new SkypeIntlPortfolioContext())
            {
                if (allWorkItems.Any())
                {
                    //2.1 Create missing products

                    //get all product ids and names from the db
                    var products = dbContext.Products_New.Where(c => c.Localization_VSO_Path != null)
                        .Select(c => new { c.Localization_VSO_Path, ProductKey = c.ProductKey }).ToList()
                        .GroupBy(c => c.Localization_VSO_Path).Select(c => c.First())
                        .ToDictionary(c => c.Localization_VSO_Path.Trim().ToLower(), d => d.ProductKey);

                    //check that all the products in the fetched items exists
                    var nonExistingProducts = new HashSet<WorkItem>(allWorkItems.Values, new WorkItemAreaPathComparer());
                    nonExistingProducts.ExceptWith(products.Select(c => new WorkItem { AreaPath = c.Key }));

                    //create the ones that doesn't exist
                    //if (nonExistingProducts.Any())
                    //{
                    //    foreach (var item in nonExistingProducts)
                    //    {
                    //        var product = new Product
                    //        {
                    //            Product_Name = item.ProductName,
                    //            Family = item.Family,
                    //        };
                    //        dbContext.Products.Add(product);
                    //    }

                    //    dbContext.SaveChanges();

                    //    //refetch the products
                    //    products = dbContext.Products
                    //    .Select(c => new { ProductName = c.Product_Name, ProductKey = c.ProductKey })
                    //    .ToDictionary(c => c.ProductName.Trim().ToLower(), d => d.ProductKey);
                    //}

                    // 2.2 Update DB releases that were modified in VSO

                    //get all releases id with their change date, so that we can compare them with the fetched Vso items
                    var allDBWorkItems = dbContext.Releases.Select(c => new { ID = c.VSO_ID, ChangedDate = c.VSO_ChangedDate, ProductKey = c.ProductKey }).ToDictionary(c => c.ID, d => d);

                    var vsoSet = new HashSet<int>(allWorkItems.Keys);
                    var commonItems = new HashSet<int>(allDBWorkItems.Keys);
                    commonItems.IntersectWith(vsoSet);
                    var changedItems = commonItems.Select(c => allWorkItems[c]).Where(c => allDBWorkItems[c.ID].ChangedDate != c.ChangedDate).ToList();
                    var newItems = vsoSet;
                    newItems.ExceptWith(new HashSet<int>(allDBWorkItems.Keys));
                    var nonMatchItems_Update = new List<WorkItem>();
                    var nonMatchItems_New = new List<WorkItem>();
                    if (changedItems.Any())
                    {
                        //get productIds of changed just in case the VSO item was moved
                        //var changedProductInfo = products
                        foreach (WorkItem vsoItem in changedItems)
                        {
                            var updatedItem = new Release()
                            {
                                VSO_ID = vsoItem.ID,
                                VSO_ChangedDate = vsoItem.ChangedDate,
                                VSO_AreaPath = vsoItem.AreaPath,
                                //VSO_Family = vsoItem.Family,
                                VSO_DueDate = vsoItem.DueDate,
                                VSO_IterationPath = vsoItem.IterationPath,
                                VSO_LocStartDate = vsoItem.LocStartDate,
                                VSO_Title = vsoItem.Title,
                                VSO_Type = vsoItem.WorkItemType,
                                VSO_Assigned_To = vsoItem.AssignedTo,
                                VSO_Tags = String.Join(";", vsoItem.Tags),
                                //VSO_ProductName = vsoItem.ProductName,
                                VSO_Status = vsoItem.State,
                                VSO_Url = vsoItem.Url,
                                Deleted = false
                            };

                            dbContext.Releases.Attach(updatedItem);
                            //dbContext.Entry(updatedItem).State = System.Data.Entity.EntityState.Modified;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_ChangedDate).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_AreaPath).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_Status).IsModified = true;
                            //dbContext.Entry(updatedItem).Property(a => a.VSO_Family).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_Tags).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_DueDate).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_IterationPath).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_LocStartDate).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_Title).IsModified = true;
                            dbContext.Entry(updatedItem).Property(a => a.VSO_Assigned_To).IsModified = true;

                            //if the product changed in vso, update the name and ProductKey in db
                            int newProductKey;
                            if (products.ContainsKey(vsoItem.AreaPath.Trim().ToLower()))
                            {
                                if (allDBWorkItems[vsoItem.ID].ProductKey != (newProductKey = products[vsoItem.AreaPath.Trim().ToLower()]))
                                {
                                    updatedItem.ProductKey = newProductKey;
                                    //dbContext.Entry(updatedItem).Property(a => a.VSO_ProductName).IsModified = true;
                                    dbContext.Entry(updatedItem).Property(a => a.ProductKey).IsModified = true;
                                }
                            }
                            else
                            {
                                nonMatchItems_Update.Add(vsoItem);
                            }
                        }
                    }

                    // 2.3 Create DB releases that are new in VSO
                    int totalAdded = 0;
                    if (newItems.Any())
                    {
                        foreach (int ID in newItems)
                        {
                            WorkItem vsoItem = allWorkItems[ID];
                            if (products.ContainsKey(vsoItem.AreaPath.Trim().ToLower()))
                            {
                                var newItem = new Release()
                                {
                                    VSO_ID = vsoItem.ID,
                                    VSO_ChangedDate = vsoItem.ChangedDate,
                                    VSO_AreaPath = vsoItem.AreaPath,
                                    //VSO_Family = vsoItem.Family,
                                    VSO_DueDate = vsoItem.DueDate,
                                    VSO_IterationPath = vsoItem.IterationPath,
                                    //VSO_ProductName = vsoItem.ProductName,
                                    ProductKey = products[vsoItem.AreaPath.Trim().ToLower()],
                                    VSO_LocStartDate = vsoItem.LocStartDate,
                                    VSO_Title = vsoItem.Title,
                                    VSO_Type = vsoItem.WorkItemType,
                                    VSO_Assigned_To = vsoItem.AssignedTo,
                                    VSO_Status = vsoItem.State,
                                    VSO_Tags = String.Join(";", vsoItem.Tags),
                                    VSO_Url = vsoItem.Url,
                                    Deleted = false
                                };

                                dbContext.Releases.Add(newItem);
                                totalAdded++;
                            }
                            else
                            {
                                nonMatchItems_New.Add(vsoItem);
                            }
                        }
                    }

                    dbContext.SaveChanges();

                    logger.LogMessage(string.Format("Updating db.....Item updated: {0}, item created: {1}.", changedItems.Count(), totalAdded));
                }
            }
        }
    }
}
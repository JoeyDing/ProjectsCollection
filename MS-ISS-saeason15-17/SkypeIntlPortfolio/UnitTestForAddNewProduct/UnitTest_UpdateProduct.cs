using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestForAddNewProduct
{
    [TestClass]
    public class UnitTest_UpdateProduct
    {
        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void UpdateProduct_TwoEmptyLists_ThrowException()
        {
            //set up
            int productKey = 10;
            List<string> eolToAdd = new List<string>();
            List<string> eolToDelete = new List<string>();
            //execute
            List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void UpdateProduct_DuplicatedElementsInBothLists_ThrowException()
        {
            //set up
            int productKey = 10;
            List<string> eolToAdd = new List<string>();
            eolToAdd.Add("aa-ER");
            eolToAdd.Add("aa-ET");
            eolToAdd.Add("aa-KW");
            List<string> eolToDelete = new List<string>();
            eolToDelete.Add("af-NA");
            eolToDelete.Add("aa-ER");
            eolToDelete.Add("aa-ET");
            //execute
            List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void UpdateProduct_SameLists_ThrowException()
        {
            //set up
            int productKey = 10;
            List<string> eolToAdd = new List<string>();
            eolToAdd.Add("aa-ER");
            eolToAdd.Add("aa-ET");

            List<string> eolToDelete = new List<string>();
            eolToDelete.Add("aa-ER");
            eolToDelete.Add("aa-ET");
            //execute
            List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void UpdateProduct_InsertExistingLLCC_ThrowException()
        {
            //set up
            int productKey = 8;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, EOL> existingProductLlccList = (from l in portfolioContext.EOLs
                                                                   where l.ProductKey == productKey
                                                                   select l).ToDictionary(c => c.llCC, c => c);

                List<string> eolToAdd = new List<string>();
                eolToAdd.Add("aa-ER");
                eolToAdd.Add("aa-ET");

                List<string> eolToDelete = new List<string>();
                eolToDelete.Add("aa-OM");

                Dictionary<string, string> languageNames = portfolioContext.suppData_ExtendedEOL
                        .Where(c => eolToAdd
                        .Contains(c.llCC))
                        .Select(c => new { c.llCC, c.Language_Name })
                        .ToDictionary(c => c.llCC, v => v.Language_Name);

                foreach (string llccToAdd in eolToAdd)
                {
                    if (!existingProductLlccList.ContainsKey(llccToAdd))
                    {
                        var newEol = new EOL();
                        newEol.ProductKey = productKey;
                        newEol.llCC = llccToAdd;
                        newEol.Language_Support_Level = null;

                        string languageName = languageNames[llccToAdd];
                        newEol.Language_Name = languageName;
                        portfolioContext.EOLs.Add(newEol);
                    }
                }
                portfolioContext.SaveChanges();
                //execute
                List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void UpdateProduct_DeleteUnexistingLLCC_ThrowException()
        {
            //set up
            int productKey = 10;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, EOL> existingProductLlccList = new Dictionary<string, EOL>();
                List<int> ProductKeyExist = (from l in portfolioContext.EOLs
                                             where l.ProductKey == productKey
                                             select l.ProductKey).ToList();
                if (ProductKeyExist.Contains(productKey))
                {
                    existingProductLlccList = (from l in portfolioContext.EOLs
                                               where l.ProductKey == productKey
                                               select l).ToDictionary(c => c.llCC, c => c);
                }

                List<string> eolToAdd = new List<string>();
                List<string> eolToDelete = new List<string>();
                eolToDelete.Add("aa-OM");

                foreach (string llccToDelete in eolToDelete)
                {
                    EOL eolItemToDelete = null;
                    if (existingProductLlccList.Count != 0)
                    {
                        if ((eolItemToDelete = existingProductLlccList[llccToDelete]) != null)
                        {
                            portfolioContext.EOLs.Remove(eolItemToDelete);
                        }
                    }
                }
                portfolioContext.SaveChanges();
                //execute
                List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
            }
        }

        [TestMethod]
        public void UpdateProduct_DeleteExistingLLCC_NoElementIntheList()
        {
            //set up,
            int productKey = 6;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, EOL> existingProductLlccList = new Dictionary<string, EOL>();

                //check if the given productkey exist the current eol table, if no,assign null to the dictionary.
                List<int> ProductKeyExist = (from l in portfolioContext.EOLs
                                             where l.ProductKey == productKey
                                             select l.ProductKey).ToList();
                if (ProductKeyExist.Contains(productKey))
                {
                    existingProductLlccList = (from l in portfolioContext.EOLs
                                               where l.ProductKey == productKey
                                               select l).ToDictionary(c => c.llCC, c => c);
                }

                List<string> eolToAdd = new List<string>();
                List<string> eolToDelete = new List<string>();
                eolToDelete.Add("aa-ER");
                eolToDelete.Add("aa-ET");
                Dictionary<string, string> languageNames = portfolioContext.suppData_ExtendedEOL
                        .Where(c => eolToDelete
                        .Contains(c.llCC))
                        .Select(c => new { c.llCC, c.Language_Name })
                        .ToDictionary(c => c.llCC, v => v.Language_Name);

                foreach (string llccToDelete in eolToDelete)
                {
                    if (!existingProductLlccList.ContainsKey(llccToDelete))
                    {
                        var newEol = new EOL();
                        newEol.ProductKey = productKey;
                        newEol.llCC = llccToDelete;
                        newEol.Language_Support_Level = null;

                        string languageName = languageNames[llccToDelete];
                        newEol.Language_Name = languageName;
                        portfolioContext.EOLs.Add(newEol);
                    }
                }

                portfolioContext.SaveChanges();
                //execute
                List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
                //Assert
                Assert.IsTrue(!eolList.Any());
            }
        }

        [TestMethod]
        public void UpdateProduct_InsertTwoNewLLCCy_TwoLLCCAdded()
        {
            //set up,
            int productKey = 8;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, EOL> existingProductLlccList = (from l in portfolioContext.EOLs
                                                                   where l.ProductKey == productKey
                                                                   select l).ToDictionary(c => c.llCC, c => c);

                List<string> eolToAdd = new List<string>();
                eolToAdd.Add("aa-ER");
                eolToAdd.Add("aa-ET");
                List<string> eolToDelete = new List<string>();

                Dictionary<string, string> languageNames = portfolioContext.suppData_ExtendedEOL
                        .Where(c => eolToAdd
                        .Contains(c.llCC))
                        .Select(c => new { c.llCC, c.Language_Name })
                        .ToDictionary(c => c.llCC, v => v.Language_Name);
                //clean up the existing eol data from the eol table
                foreach (string llccToAdd in eolToAdd)
                {
                    if (existingProductLlccList.ContainsKey(llccToAdd))
                    {
                        EOL eolItemToDelete = null;
                        if (existingProductLlccList.Count != 0)
                        {
                            if ((eolItemToDelete = existingProductLlccList[llccToAdd]) != null)
                            {
                                portfolioContext.EOLs.Remove(eolItemToDelete);
                            }
                        }
                    }
                }

                portfolioContext.SaveChanges();
                //execute
                List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
                //Assert
                EOL eolOne = (from p in eolList
                              where p.ProductKey == 8 && p.llCC == "aa-ER" && p.Language_Name == "Afar (Eritrea)"
                              select p).FirstOrDefault();
                EOL eolTwo = (from p in eolList
                              where p.ProductKey == 8 && p.llCC == "aa-ET" && p.Language_Name == "Afar (Ethiopia)"
                              select p).FirstOrDefault();
                Assert.IsTrue(eolList.Contains(eolOne) && eolList.Contains(eolOne));
            }
        }

        [TestMethod]
        public void UpdateProduct_InsertTwoNewLLCCDeleteExistingLLCC_TwoLLCCAddedOneDeleted()
        {
            //set up,
            int productKey = 5;
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, EOL> existingProductLlccList = new Dictionary<string, EOL>();

                //check if the given productkey exist the current eol table, if no,assign null to the dictionary.
                List<int> ProductKeyExist = (from l in portfolioContext.EOLs
                                             where l.ProductKey == productKey
                                             select l.ProductKey).ToList();
                if (ProductKeyExist.Contains(productKey))
                {
                    existingProductLlccList = (from l in portfolioContext.EOLs
                                               where l.ProductKey == productKey
                                               select l).ToDictionary(c => c.llCC, c => c);
                }

                List<string> eolToAdd = new List<string>();
                eolToAdd.Add("af-NA");
                eolToAdd.Add("af-ZA");
                List<string> eolToDelete = new List<string>();
                eolToDelete.Add("am-ET");

                Dictionary<string, string> languageNames = portfolioContext.suppData_ExtendedEOL
                        .Where(c => eolToDelete
                        .Contains(c.llCC))
                        .Select(c => new { c.llCC, c.Language_Name })
                        .ToDictionary(c => c.llCC, v => v.Language_Name);

                foreach (string llccToAdd in eolToAdd)
                {
                    if (existingProductLlccList.ContainsKey(llccToAdd))
                    {
                        EOL eolItemToDelete = null;
                        if (existingProductLlccList.Count != 0)
                        {
                            if ((eolItemToDelete = existingProductLlccList[llccToAdd]) != null)
                            {
                                portfolioContext.EOLs.Remove(eolItemToDelete);
                            }
                        }
                    }
                }

                foreach (string llccToDelete in eolToDelete)
                {
                    if (!existingProductLlccList.ContainsKey(llccToDelete))
                    {
                        var newEol = new EOL();
                        newEol.ProductKey = productKey;
                        newEol.llCC = llccToDelete;
                        newEol.Language_Support_Level = null;

                        string languageName = languageNames[llccToDelete];
                        newEol.Language_Name = languageName;
                        portfolioContext.EOLs.Add(newEol);
                    }
                }

                portfolioContext.SaveChanges();
                //execute
                List<EOL> eolList = Utils.UpdateProduct(productKey, eolToAdd, eolToDelete);
                //Assert
                EOL eolOne = (from p in eolList
                              where p.ProductKey == 5 && p.llCC == "af-NA"
                              select p).FirstOrDefault();
                EOL eolTwo = (from p in eolList
                              where p.ProductKey == 5 && p.llCC == "af-ZA"
                              select p).FirstOrDefault();
                EOL eolThree = (from p in eolList
                                where p.ProductKey == 5 && p.llCC == "am-ET"
                                select p).FirstOrDefault();
                Assert.IsTrue(eolList.Contains(eolOne) && eolList.Contains(eolOne));

                Assert.IsTrue(!eolList.Contains(eolThree));
            }
        }
    }
}
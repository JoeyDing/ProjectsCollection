using SkypeIntlPortfolio.Ajax.Core.Interface;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol.UALanguageProduct
{
    public class UALanguageProductPresenter
    {
        private IUAlanguageProductBridge _bridge;
        private int _productKey;
        private IBulkInsert _iBulkInsert;
        private IBulkUpdate _iBulkUpdate;

        public UALanguageProductPresenter(IUAlanguageProductBridge bridge, int productKey, IBulkInsert iBulkinsert, IBulkUpdate iBulkupdate)
        {
            this._iBulkInsert = iBulkinsert;
            this._iBulkUpdate = iBulkupdate;
            this._productKey = productKey;
            this._bridge = bridge;
            this._bridge.onGetBasicLanguage += _bridge_onGetBasicLanguage;
            this._bridge.GetUALanguageUnderPrudctByType += _bridge_GetUALanguageUnderPrudctByType;
            this._bridge.GetTotalCountOfUALanguageUnderPrudctByType += _bridge_GetTotalCountUALanguageUnderPrudctByType;
            this._bridge.GetFilesUnderLanguage += _bridge_GetFilesUnderLanguage;
            this._bridge.GetTotalCountOfFilesUnderLanguage += _bridge_GetTotalCountOfFilesUnderLanguage;
            this._bridge.GetFilesListUnderProductForInsert += _bridge_GetFilesListUnderProductForInsert;
            this._bridge.onInsertLanguage += _bridge_onInsertLanguage;
            this._bridge.onUpdateLanguage += _bridge_onUpdateLanguage;
            this._bridge.onDeleteFiles += _bridge_onDeleteFiles;
            this._bridge.DeleteUALanguage += _bridge_DeleteUALanguage;
            this._bridge.GetFilteredLanguageListForProductLevel += _bridge_GetFilteredLanguageListForProductLevel;
            this._bridge.GetAllProducts += _bridge_GetAllProducts;
            this._bridge.InsertFromAnotherProject += _bridge_InsertFromAnotherProject;
            this._bridge.MoveEolFromOneTypeToAnother += _bridge_MoveEolFromOneTypeToAnother;
        }

        private void _bridge_MoveEolFromOneTypeToAnother(string fromType, string toType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var ualanguageBaseList = new List<UALanguage_Base>();
                var ualanguageList = new List<UALanguage>();
                if (fromType == "Planned" && toType == "Released")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Planned == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = true,
                            Language_Planned = false,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                else if (fromType == "Blocked" && toType == "Released")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Blocked == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = true,
                            Language_Planned = false,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                else if (fromType == "Released" && toType == "Planned")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Released == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = true,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                else if (fromType == "Blocked" && toType == "Planned")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Blocked == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = true,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                else if (fromType == "Released" && toType == "Blocked")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Released == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = false,
                            Language_Blocked = true,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                else if (fromType == "Planned" && toType == "Blocked")
                {
                    ualanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UALanguages.Where(c => c.Language_Planned == true).ToList();
                    foreach (var item in ualanguageList)
                    {
                        ualanguageBaseList.Add(new UALanguage_Base()
                        {
                            UALanguagesKey = item.UALanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = false,
                            Language_Blocked = true,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        });
                    }
                }
                var fileds = new[] { "Language Released", "Language Blocked", "Language Planned" };
                _iBulkUpdate.UpdateData_Bulk(entity, ualanguageBaseList, "UALanguages", "UALanguagesKey", fileds);
            }
        }

        private void _bridge_InsertFromAnotherProject(string uaOrui, int productKeyToCopyFrom, string uaLanguageType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uaLanguageBaseList = new List<UALanguage_Base>();

                if (uaLanguageType == "Released")
                {
                    if (uaOrui == "UA")
                    {
                        List<UALanguage> ualanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UALanguages.Where(c => c.Language_Released == true).ToList();
                        uaLanguageBaseList = ualanguageList.Select(item => new UALanguage_Base()
                        {
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = item.Language_Released,
                            Language_Planned = item.Language_Planned,
                            Language_Blocked = item.Language_Blocked,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        }).ToList();
                    }
                    else
                    {
                        List<UILanguage> uilanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UILanguages.Where(c => c.Language_Released == true).ToList();
                        uaLanguageBaseList = uilanguageList.Select(item => new UALanguage_Base()
                        {
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = item.Language_Released,
                            Language_Planned = item.Language_Planned,
                            Language_Blocked = item.Language_Blocked,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = 2
                        }).ToList();
                    }
                }
                else if (uaLanguageType == "Planned")
                {
                    if (uaOrui == "UA")
                    {
                        List<UALanguage> ualanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UALanguages.Where(c => c.Language_Planned == true).ToList();
                        uaLanguageBaseList = ualanguageList.Select(item => new UALanguage_Base()
                        {
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = item.Language_Released,
                            Language_Planned = item.Language_Planned,
                            Language_Blocked = item.Language_Blocked,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = item.QualityID
                        }).ToList();
                    }
                    else
                    {
                        List<UILanguage> uilanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UILanguages.Where(c => c.Language_Planned == true).ToList();
                        uaLanguageBaseList = uilanguageList.Select(item => new UALanguage_Base()
                        {
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = item.Language_Released,
                            Language_Planned = item.Language_Planned,
                            Language_Blocked = item.Language_Blocked,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason,
                            QualityID = 2
                        }).ToList();
                    }
                }

                var hashsetUALanguage = new HashSet<UALanguage_Base>(uaLanguageBaseList, new DbComparerUALanguages());
                var dbUALanguage = entity.UALanguages.Where(c => c.ProductKey == _productKey)
                    .Select(c => new UALanguage_Base()
                    {
                        ProductKey = this._productKey,
                        LanguageKey = c.LanguageKey,
                        Language_Blocked = c.Language_Blocked,
                        Language_Planned = c.Language_Planned,
                        Language_Released = c.Language_Released,
                        Blocked_Reason = c.Blocked_Reason,
                        Release_Date = c.Release_Date,
                    });
                hashsetUALanguage.ExceptWith(dbUALanguage);

                this._iBulkInsert.InsertData_Bulk(entity, hashsetUALanguage, "UALanguages");
            }
        }

        private List<Products_New> _bridge_GetAllProducts()
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = entity.Products_New.OrderBy(c => c.ProductFamilyKey).ThenBy(c => c.Product_Name).ToList();
                return result;
            }
        }

        private List<BasicLanguageList_Base> _bridge_GetFilteredLanguageListForProductLevel(string uaLangType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<BasicLanguageList_Base>();

                var listFileKeys = entity.Products_New.First(c => c.ProductKey == this._productKey).ResourceFiles.Select(c => c.FileKey);
                List<int> alreadyExistingLangKeys = null;
                if (listFileKeys.Any())
                {
                    alreadyExistingLangKeys = entity.UALanguages.Where(c => c.ProductKey == this._productKey).Select(c => new { LanguageKey = c.LanguageKey, FileKeysUnderLanguage = c.ResourceFiles.Select(x => x.FileKey) })
                   .Where(c => listFileKeys.All(g => c.FileKeysUnderLanguage.Contains(g)))
                   .Select(c => c.LanguageKey).ToList();
                    IEnumerable<int> langKeysinOtherLanguageType = null;
                    switch (uaLangType)
                    {
                        case "ReleasedDetails":
                            langKeysinOtherLanguageType = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == false).Select(c => c.LanguageKey);
                            break;

                        case "PlannedDetails":
                            langKeysinOtherLanguageType = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == false).Select(c => c.LanguageKey);
                            break;

                        case "BlockedDetails":
                            langKeysinOtherLanguageType = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == false).Select(c => c.LanguageKey);
                            break;
                    }

                    alreadyExistingLangKeys = alreadyExistingLangKeys.Union(langKeysinOtherLanguageType).Distinct().ToList();
                }
                else
                {
                    alreadyExistingLangKeys = entity.UALanguages.Where(c => c.ProductKey == this._productKey && !c.ResourceFiles.Any()).Select(c => c.LanguageKey).ToList();
                }

                result = entity.BasicLanguageLists.Where(b => !alreadyExistingLangKeys.Contains(b.LanguageKey)).ToList().
                    Select(c => new BasicLanguageList_Base { LanguageKey = c.LanguageKey, CultureName = c.CultureName, Language = c.Language }).OrderBy(c => c.LanguageIDPlusName).ToList();
                return result;
            }
        }

        private void _bridge_DeleteUALanguage(int uaLanguageKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uaLanguage = entity.UALanguages.First(c => c.UALanguagesKey == uaLanguageKey);
                uaLanguage.ResourceFiles.Clear();
                if (!uaLanguage.ResourceFiles.Any())
                    entity.UALanguages.Remove(uaLanguage);
                entity.SaveChanges();
            }
        }

        private void _bridge_onDeleteFiles(int ualanguageKey, int fileKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var file = new ResourceFile() { FileKey = fileKey };
                entity.ResourceFiles.Attach(file);

                var ualanguage = entity.UALanguages.First(c => c.UALanguagesKey == ualanguageKey).ResourceFiles.Remove(file);
                entity.SaveChanges();
            }
        }

        private void _bridge_onUpdateLanguage(bool ifModifyQualityID, UALanguage e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.UALanguages.Attach(e);
                entity.Entry(e).Property(x => x.Blocked_Reason).IsModified = true;
                entity.Entry(e).Property(x => x.Release_Date).IsModified = true;
                if (ifModifyQualityID)
                    entity.Entry(e).Property(x => x.QualityID).IsModified = true;

                entity.SaveChanges();
            }
        }

        private void _bridge_onGetBasicLanguage(object sender, List<BasicLanguageList> e)
        {
            e.Clear();
            //load list of features
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                e.AddRange(entity.BasicLanguageLists.ToList());
            }
        }

        private void _bridge_onInsertLanguage(bool productContainsFiles, List<UALanguage_Base> e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                //1.UAlanguage
                e.ForEach(c => c.ProductKey = this._productKey);
                var hashsetUaLanguage = new HashSet<UALanguage_Base>(e, new DbComparerUALanguages());
                var dbUALanguage = entity.UALanguages.Where(c => c.ProductKey == this._productKey)
                .Select(c => new UALanguage_Base()
                {
                    ProductKey = this._productKey,
                    LanguageKey = c.LanguageKey,
                    Language_Blocked = c.Language_Blocked,
                    Language_Planned = c.Language_Planned,
                    Language_Released = c.Language_Released,
                    Blocked_Reason = c.Blocked_Reason,
                    Release_Date = c.Release_Date,
                });
                hashsetUaLanguage.ExceptWith(dbUALanguage);

                this._iBulkInsert.InsertData_Bulk(entity, hashsetUaLanguage, "UALanguages");

                if (productContainsFiles)
                {
                    //2.UALanguageFile

                    var temp = entity.UALanguages.Where(c => c.ProductKey == this._productKey).ToList();
                    var uaLanguageFileInsert = temp.Join(e, uaLan => uaLan.LanguageKey, uaLanBase => uaLanBase.LanguageKey, (uaLan, uaLanBase) => new UALanguageFiles_Base { UALanguagesKey = uaLan.UALanguagesKey, FileKey = uaLanBase.FileKey });
                    var hashsetUaLanguageFiles = new HashSet<UALanguageFiles_Base>(uaLanguageFileInsert, new DbComparerUALanguageFiles());
                    var dbUaLanguageFile = entity.UALanguages.Where(c => c.ProductKey == this._productKey)
                        .SelectMany(c => c.ResourceFiles.Select(x => new UALanguageFiles_Base() { UALanguagesKey = c.UALanguagesKey, FileKey = x.FileKey }));
                    hashsetUaLanguageFiles.ExceptWith(dbUaLanguageFile);

                    this._iBulkInsert.InsertData_Bulk(entity, hashsetUaLanguageFiles, "UALanguageFiles");
                }
            }
        }

        private List<ResourceFile> _bridge_GetFilesListUnderProductForInsert()
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<ResourceFile>();

                result = entity.Products_New.First(c => c.ProductKey == _productKey).ResourceFiles.OrderBy(c => c.Source_File_Location).ToList();
                return result;
            }
        }

        private int _bridge_GetTotalCountOfFilesUnderLanguage(string uaLanguageType, int uaLanguageKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                int count = 0;
                var files = entity.Products_New.First(c => c.ProductKey == _productKey).ResourceFiles;
                var fileKeys = new HashSet<int>(files.Select(c => c.FileKey));

                switch (uaLanguageType)
                {
                    case "Released":
                        count = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Released == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;

                    case "Planned":
                        count = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Planned == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;

                    case "Blocked":
                        count = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Blocked == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;
                }

                return count;
            }
        }

        private List<ResourceFile> _bridge_GetFilesUnderLanguage(string uaLanguageType, int uaLanguageKey, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<ResourceFile>();

                switch (uaLanguageType)
                {
                    case "Released":
                        result = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Released == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;

                    case "Planned":
                        result = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Planned == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;

                    case "Blocked":
                        result = entity.UALanguages.Where(c => c.UALanguagesKey == uaLanguageKey && c.Language_Blocked == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;
                }

                return result;
            }
        }

        private int _bridge_GetTotalCountUALanguageUnderPrudctByType(string uaLanguageType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                int count = 0;

                switch (uaLanguageType)
                {
                    case "Released":
                        count = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == true).Count();
                        break;

                    case "Planned":
                        count = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == true).Count();
                        break;

                    case "Blocked":
                        count = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == true).Count();
                        break;
                }
                return count;
            }
        }

        private List<UALanguage_Base> _bridge_GetUALanguageUnderPrudctByType(string uaLanguageType, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<int, string> qualitiesDict = entity.UAQualities.ToDictionary(c => c.QualityID, c => c.QualityName);
                var result = new List<UALanguage_Base>();
                IQueryable<UALanguage> uaLanguageResult = null;

                switch (uaLanguageType)
                {
                    case "Released":
                        uaLanguageResult = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == true);
                        break;

                    case "Planned":
                        uaLanguageResult = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == true);
                        break;

                    case "Blocked":
                        uaLanguageResult = entity.UALanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == true);
                        break;
                }

                result = uaLanguageResult
                   .Join(entity.BasicLanguageLists, uaLan => uaLan.LanguageKey, basicLan => basicLan.LanguageKey, (ualan, basicLan) => new UALanguage_Base() { UALanguagesKey = ualan.UALanguagesKey, LanguageKey = ualan.LanguageKey, Blocked_Reason = ualan.Blocked_Reason, Release_Date = ualan.Release_Date, LanguageName = basicLan.Language, QualityID = ualan.QualityID, LanguageIDPlusName = basicLan.CultureName + @" \ " + basicLan.Language })
                   .OrderBy(c => c.LanguageIDPlusName).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                result.ForEach(c => c.QualityName = c.QualityID.HasValue ? qualitiesDict[c.QualityID.Value] : "");

                return result;
            }
        }
    }
}
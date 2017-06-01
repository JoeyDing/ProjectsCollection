using SkypeIntlPortfolio.Ajax.Core;
using SkypeIntlPortfolio.Ajax.Core.Interface;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol.UILanguageProduct
{
    public class UILanguageProductPresenter
    {
        private IUIlanguageProductBridge _bridge;
        private int _productKey;
        private IBulkInsert _iBulkInsert;
        private IBulkUpdate _iBulkUpdate;

        public UILanguageProductPresenter(IUIlanguageProductBridge bridge, int productKey, IBulkInsert iBulkinsert, IBulkUpdate iBulkupdate)
        {
            this._iBulkInsert = iBulkinsert;
            this._iBulkUpdate = iBulkupdate;
            this._productKey = productKey;
            this._bridge = bridge;
            this._bridge.onGetBasicLanguage += _bridge_onGetBasicLanguage;
            this._bridge.GetUILanguageUnderPrudctByType += _bridge_GetUILanguageUnderPrudctByType;
            this._bridge.GetTotalCountOfUILanguageUnderPrudctByType += _bridge_GetTotalCountUILanguageUnderPrudctByType;
            this._bridge.GetFilesUnderLanguage += _bridge_GetFilesUnderLanguage;
            this._bridge.GetTotalCountOfFilesUnderLanguage += _bridge_GetTotalCountOfFilesUnderLanguage;
            this._bridge.GetFilesListUnderProductForInsert += _bridge_GetFilesListUnderProductForInsert;
            this._bridge.onInsertLanguage += _bridge_onInsertLanguage;
            this._bridge.onUpdateLanguage += _bridge_onUpdateLanguage;
            this._bridge.onDeleteFiles += _bridge_onDeleteFiles;
            this._bridge.DeleteUiLanguage += _bridge_DeleteUiLanguage;
            this._bridge.GetFilteredLanguageListForProductLevel += _bridge_GetFilteredLanguageListForProductLevel;
            this._bridge.GetAllProducts += _bridge_GetAllProducts;
            this._bridge.InsertFromAnotherProject += _bridge_InsertFromAnotherProject;
            this._bridge.MoveEolFromOneTypeToAnother += _bridge_MoveEolFromOneTypeToAnother;
        }

        private void _bridge_MoveEolFromOneTypeToAnother(string fromType, string toType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uilanguageBaseList = new List<UILanguage_Base>();
                var uilanguageList = new List<UILanguage>();
                if (fromType == "Planned" && toType == "Released")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Planned == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = true,
                            Language_Planned = false,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                else if (fromType == "Blocked" && toType == "Released")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Blocked == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = true,
                            Language_Planned = false,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                else if (fromType == "Released" && toType == "Planned")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Released == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = true,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                else if (fromType == "Blocked" && toType == "Planned")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Blocked == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = true,
                            Language_Blocked = false,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                else if (fromType == "Released" && toType == "Blocked")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Released == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = false,
                            Language_Blocked = true,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                else if (fromType == "Planned" && toType == "Blocked")
                {
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == _productKey).UILanguages.Where(c => c.Language_Planned == true).ToList();
                    foreach (var item in uilanguageList)
                    {
                        uilanguageBaseList.Add(new UILanguage_Base()
                        {
                            UILanguagesKey = item.UILanguagesKey,
                            ProductKey = _productKey,
                            LanguageKey = item.LanguageKey,
                            Language_Released = false,
                            Language_Planned = false,
                            Language_Blocked = true,
                            Release_Date = item.Release_Date,
                            Blocked_Reason = item.Blocked_Reason
                        });
                    }
                }
                var fileds = new[] { "Language Released", "Language Blocked", "Language Planned" };
                _iBulkUpdate.UpdateData_Bulk(entity, uilanguageBaseList, "UILanguages", "UILanguagesKey", fileds);
            }
        }

        private void _bridge_InsertFromAnotherProject(int productKeyToCopyFrom, string uiLanguageType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uiLanguageBaseList = new List<UILanguage_Base>();
                var uilanguageList = new List<UILanguage>();
                if (uiLanguageType == "Released")
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UILanguages.Where(c => c.Language_Released == true).ToList();
                else if (uiLanguageType == "Planned")
                    uilanguageList = entity.Products_New.First(c => c.ProductKey == productKeyToCopyFrom).UILanguages.Where(c => c.Language_Planned == true).ToList();

                foreach (var item in uilanguageList)
                {
                    uiLanguageBaseList.Add(new UILanguage_Base()
                    {
                        ProductKey = _productKey,
                        LanguageKey = item.LanguageKey,
                        Language_Released = item.Language_Released,
                        Language_Planned = item.Language_Planned,
                        Language_Blocked = item.Language_Blocked,
                        Release_Date = item.Release_Date,
                        Blocked_Reason = item.Blocked_Reason
                    });
                }

                var hashsetUiLanguage = new HashSet<UILanguage_Base>(uiLanguageBaseList, new DbComparerUILanguages());
                var dbUiLanguage = entity.UILanguages.Where(c => c.ProductKey == _productKey)
                    .Select(c => new UILanguage_Base()
                    {
                        ProductKey = this._productKey,
                        LanguageKey = c.LanguageKey,
                        Language_Blocked = c.Language_Blocked,
                        Language_Planned = c.Language_Planned,
                        Language_Released = c.Language_Released,
                        Blocked_Reason = c.Blocked_Reason,
                        Release_Date = c.Release_Date,
                    });
                hashsetUiLanguage.ExceptWith(dbUiLanguage);

                this._iBulkInsert.InsertData_Bulk(entity, hashsetUiLanguage, "UILanguages");
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

        private List<BasicLanguageList_Base> _bridge_GetFilteredLanguageListForProductLevel(string uiLangType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<BasicLanguageList_Base>();

                var listFileKeys = entity.Products_New.First(c => c.ProductKey == this._productKey).ResourceFiles.Select(c => c.FileKey);
                List<int> alreadyExistingLangKeys = null;
                if (listFileKeys.Any())
                {
                    alreadyExistingLangKeys = entity.UILanguages.Where(c => c.ProductKey == this._productKey).Select(c => new { LanguageKey = c.LanguageKey.Value, FileKeysUnderLanguage = c.ResourceFiles.Select(x => x.FileKey) })
                   .Where(c => listFileKeys.All(g => c.FileKeysUnderLanguage.Contains(g)))
                   .Select(c => c.LanguageKey).ToList();
                    IEnumerable<int> langKeysinOtherLanguageType = null;
                    switch (uiLangType)
                    {
                        case "ReleasedDetails":
                            langKeysinOtherLanguageType = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == false).Select(c => c.LanguageKey.Value);
                            break;

                        case "PlannedDetails":
                            langKeysinOtherLanguageType = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == false).Select(c => c.LanguageKey.Value);
                            break;

                        case "BlockedDetails":
                            langKeysinOtherLanguageType = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == false).Select(c => c.LanguageKey.Value);
                            break;
                    }

                    alreadyExistingLangKeys = alreadyExistingLangKeys.Union(langKeysinOtherLanguageType).Distinct().ToList();
                }
                else
                {
                    alreadyExistingLangKeys = entity.UILanguages.Where(c => c.ProductKey == this._productKey && !c.ResourceFiles.Any()).Select(c => c.LanguageKey.Value).ToList();
                }

                result = entity.BasicLanguageLists.Where(b => !alreadyExistingLangKeys.Contains(b.LanguageKey)).ToList().
                    Select(c => new BasicLanguageList_Base { LanguageKey = c.LanguageKey, CultureName = c.CultureName, Language = c.Language }).OrderBy(c => c.LanguageIDPlusName).ToList();

                return result;
            }
        }

        private void _bridge_DeleteUiLanguage(int uiLanguageKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uiLanguage = entity.UILanguages.First(c => c.UILanguagesKey == uiLanguageKey);
                uiLanguage.ResourceFiles.Clear();
                if (!uiLanguage.ResourceFiles.Any())
                    entity.UILanguages.Remove(uiLanguage);
                entity.SaveChanges();
            }
        }

        private void _bridge_onDeleteFiles(int uilanguageKey, int fileKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var file = new ResourceFile() { FileKey = fileKey };
                entity.ResourceFiles.Attach(file);

                var uilanguage = entity.UILanguages.First(c => c.UILanguagesKey == uilanguageKey).ResourceFiles.Remove(file);
                entity.SaveChanges();
            }
        }

        private void _bridge_onUpdateLanguage(object sender, UILanguage e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.UILanguages.Attach(e);
                entity.Entry(e).Property(x => x.Blocked_Reason).IsModified = true;
                entity.Entry(e).Property(x => x.Release_Date).IsModified = true;

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

        private void _bridge_onInsertLanguage(bool productContainsFiles, List<UILanguage_Base> e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                //1.UIlanguage
                e.ForEach(c => c.ProductKey = this._productKey);
                var hashsetUiLanguage = new HashSet<UILanguage_Base>(e, new DbComparerUILanguages());
                var dbUiLanguage = entity.UILanguages.Where(c => c.ProductKey == this._productKey)
                .Select(c => new UILanguage_Base()
                {
                    ProductKey = this._productKey,
                    LanguageKey = c.LanguageKey,
                    Language_Blocked = c.Language_Blocked,
                    Language_Planned = c.Language_Planned,
                    Language_Released = c.Language_Released,
                    Blocked_Reason = c.Blocked_Reason,
                    Release_Date = c.Release_Date,
                });
                hashsetUiLanguage.ExceptWith(dbUiLanguage);

                this._iBulkInsert.InsertData_Bulk(entity, hashsetUiLanguage, "UILanguages");

                if (productContainsFiles)
                {
                    //2.UILanguageFile
                    //entity.Products_New.Join(entity.UILanguages, p => p.ProductKey, u => u.ProductKey, (p, u) => p.ResourceFiles.Select(c=>new UILanguageFiles_Base() {UILanguagesKey=u.UILanguagesKey,FileKey=c.FileKey }) );

                    var temp = entity.UILanguages.Where(c => c.ProductKey == this._productKey).ToList();
                    var uiLanguageFileInsert = temp.Join(e, uiLan => uiLan.LanguageKey, uiLanBase => uiLanBase.LanguageKey, (uiLan, uiLanBase) => new UILanguageFiles_Base { UILanguagesKey = uiLan.UILanguagesKey, FileKey = uiLanBase.FileKey });
                    var hashsetUiLanguageFiles = new HashSet<UILanguageFiles_Base>(uiLanguageFileInsert, new DbComparerUILanguageFiles());
                    var dbUiLanguageFile = entity.UILanguages.Where(c => c.ProductKey == this._productKey)
                        .SelectMany(c => c.ResourceFiles.Select(x => new UILanguageFiles_Base() { UILanguagesKey = c.UILanguagesKey, FileKey = x.FileKey }));
                    hashsetUiLanguageFiles.ExceptWith(dbUiLanguageFile);

                    this._iBulkInsert.InsertData_Bulk(entity, hashsetUiLanguageFiles, "UILanguageFiles");
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

        private int _bridge_GetTotalCountOfFilesUnderLanguage(string uiLanguageType, int uiLanguageKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                int count = 0;
                var files = entity.Products_New.First(c => c.ProductKey == _productKey).ResourceFiles;
                var fileKeys = new HashSet<int>(files.Select(c => c.FileKey));

                switch (uiLanguageType)
                {
                    case "Released":
                        count = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Released == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;

                    case "Planned":
                        count = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Planned == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;

                    case "Blocked":
                        count = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Blocked == true)
                        .SelectMany(c => c.ResourceFiles).Count();
                        break;
                }

                return count;
            }
        }

        private List<ResourceFile> _bridge_GetFilesUnderLanguage(string uiLanguageType, int uiLanguageKey, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<ResourceFile>();

                switch (uiLanguageType)
                {
                    case "Released":
                        result = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Released == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;

                    case "Planned":
                        result = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Planned == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;

                    case "Blocked":
                        result = entity.UILanguages.Where(c => c.UILanguagesKey == uiLanguageKey && c.Language_Blocked == true)
                        .SelectMany(c => c.ResourceFiles).OrderBy(x => x.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                        break;
                }

                return result;
            }
        }

        private int _bridge_GetTotalCountUILanguageUnderPrudctByType(string uiLanguageType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                int count = 0;

                switch (uiLanguageType)
                {
                    case "Released":
                        count = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == true).Count();
                        break;

                    case "Planned":
                        count = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == true).Count();
                        break;

                    case "Blocked":
                        count = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == true).Count();
                        break;
                }
                return count;
            }
        }

        private List<UILanguage_Base> _bridge_GetUILanguageUnderPrudctByType(string uiLanguageType, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<UILanguage_Base>();
                IQueryable<UILanguage> uiLanguageResult = null;

                switch (uiLanguageType)
                {
                    case "Released":
                        uiLanguageResult = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Released == true);
                        break;

                    case "Planned":
                        uiLanguageResult = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Planned == true);
                        break;

                    case "Blocked":
                        uiLanguageResult = entity.UILanguages.Where(c => c.ProductKey == this._productKey && c.Language_Blocked == true);
                        break;
                }

                result = uiLanguageResult
                   .Join(entity.BasicLanguageLists, uiLan => uiLan.LanguageKey, basicLan => basicLan.LanguageKey, (uilan, basicLan) => new UILanguage_Base() { UILanguagesKey = uilan.UILanguagesKey, LanguageKey = uilan.LanguageKey, Blocked_Reason = uilan.Blocked_Reason, Release_Date = uilan.Release_Date, LanguageName = basicLan.Language, LanguageIDPlusName = basicLan.CultureName + @" \ " + basicLan.Language })
                   .OrderBy(c => c.LanguageIDPlusName).Skip(pageIndex * pageSize).Take(pageSize).ToList();

                return result;
            }
        }
    }
}
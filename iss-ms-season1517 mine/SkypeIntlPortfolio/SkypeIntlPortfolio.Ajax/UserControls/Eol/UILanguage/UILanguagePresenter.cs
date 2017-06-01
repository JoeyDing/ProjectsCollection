using SkypeIntlPortfolio.Ajax.Core;
using SkypeIntlPortfolio.Ajax.Core.Interface;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class UILanguagePresenter
    {
        private IUILanguageBridge _bridge;
        private int _productId;
        private IBulkInsert _iBulkInsert;

        public UILanguagePresenter(IUILanguageBridge bridge, int productId, IBulkInsert iBulkInsert)
        {
            this._iBulkInsert = iBulkInsert;
            this._productId = productId;
            this._bridge = bridge;
            this._bridge.onUpdateLanguage += _bridge_onUpdateLanguage;
            this._bridge.onInsertLanguage += _bridge_onInsertLanguage;
            this._bridge.onGetUILanguageFileOfProduct += _bridge_onGetUILanguage;
            this._bridge.onGetBasicLanguage += _bridge_onGetBasicLanguage;
            this._bridge.onDeleteLanguage += _bridge_onDeleteLanguage;
            this._bridge.onGetTotalRecord += _bridge_onGetTotalRecord;
            this._bridge.onGetUILanguagePerFile += _bridge_onGetUILanguagePerFile;
            this._bridge.onGetTotalRecordPerFile += _bridge_onGetTotalRecordPerFile;
            this._bridge.GetUILanguageByUiLanguageKey += _bridge_GetUILanguageByUiLanguageKey;
            this._bridge.GetFilteredLanguageListForFileLevel += _bridge_GetFilteredLanguageListForFileLevel;
        }

        private List<BasicLanguageList> _bridge_GetFilteredLanguageListForFileLevel(int fileKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = entity.BasicLanguageLists.Where(c => !entity.UILanguages.Any(u => u.LanguageKey == c.LanguageKey && u.FileKey == fileKey)).ToList();
                return result;
            }
        }

        private UILanguage _bridge_GetUILanguageByUiLanguageKey(int uiLanguageKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = entity.UILanguages.First(c => c.UILanguagesKey == uiLanguageKey);
                return result;
            }
        }

        private int _bridge_onGetTotalRecord()
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetTotalNumOfUILanguageFileOfProduct(_productId).FirstOrDefault().Value;
            }
        }

        private void _bridge_onDeleteLanguage(object sender, int e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var uilanguage = entity.UILanguages.Where(et => et.UILanguagesKey == e).FirstOrDefault();
                entity.UILanguages.Remove(uilanguage);
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

        private List<GetUILanguageFileOfProductByPage_Result> _bridge_onGetUILanguage(int startRow, int endRow)
        {
            List<GetUILanguageFileOfProductByPage_Result> rs = new List<GetUILanguageFileOfProductByPage_Result>();
            //load list of features
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                rs.AddRange(entity.GetUILanguageFileOfProductByPage(_productId, startRow, endRow));
            }
            return rs;
        }

        private List<UILanguage_Base> _bridge_onGetUILanguagePerFile(int fileKey, string uiLanguageType, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = new List<UILanguage_Base>();
                var uiLanguageResult = new List<UILanguage>();
                switch (uiLanguageType)
                {
                    case "Released":
                        uiLanguageResult = entity.UILanguages.Where(c => c.Language_Released == true && c.FileKey == fileKey).ToList();
                        break;

                    case "Planned":
                        uiLanguageResult = entity.UILanguages.Where(c => c.Language_Planned == true && c.FileKey == fileKey).ToList();
                        break;

                    case "Blocked":
                        uiLanguageResult = entity.UILanguages.Where(c => c.Language_Blocked == true && c.FileKey == fileKey).ToList();

                        break;
                }
                var dictLanguage = entity.BasicLanguageLists.ToDictionary(c => c.LanguageKey, c => c.Language);
                foreach (var item in uiLanguageResult)
                {
                    int languageKey = item.LanguageKey.Value;
                    string lanName = dictLanguage[languageKey];
                    result.Add(new UILanguage_Base()
                    {
                        UILanguagesKey = item.UILanguagesKey,
                        FileKey = item.FileKey,
                        Release_Date = item.Release_Date,
                        Blocked_Reason = item.Blocked_Reason,
                        Language_Blocked = true,
                        Language_Planned = false,
                        Language_Released = false,
                        LanguageName = lanName
                    });
                }
                return result.OrderBy(c => c.LanguageName).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
        }

        private int _bridge_onGetTotalRecordPerFile(int fileKey, string uiLanguageType)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                int total = 0;
                switch (uiLanguageType)
                {
                    case "Released":
                        total = entity.UILanguages.Where(c => c.Language_Released == true && c.FileKey == fileKey).Count();
                        break;

                    case "Planned":
                        total = entity.UILanguages.Where(c => c.Language_Planned == true && c.FileKey == fileKey).Count();
                        break;

                    case "Blocked":
                        total = entity.UILanguages.Where(c => c.Language_Blocked == true && c.FileKey == fileKey).Count();
                        break;
                }
                return total;
            }
        }

        private void _bridge_onInsertLanguage(object sender, List<UILanguage_Base> e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                this._iBulkInsert.InsertData_Bulk(entity, e, "UILanguages");

                //foreach (var item in e)
                //{
                //    entity.UILanguages.Add(item);
                //}

                //entity.SaveChanges();
            }
        }

        private void _bridge_onUpdateLanguage(object sender, UILanguage e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.UILanguages.Attach(e);
                //entity.Entry(e).Property(x => x.LanguageKey).IsModified = true;
                entity.Entry(e).Property(x => x.Blocked_Reason).IsModified = true;
                //entity.Entry(e).Property(x => x.FileKey).IsModified = true;
                //entity.Entry(e).Property(x => x.Language_Blocked).IsModified = true;
                //entity.Entry(e).Property(x => x.Language_Planned).IsModified = true;
                //entity.Entry(e).Property(x => x.Language_Released).IsModified = true;
                entity.Entry(e).Property(x => x.Release_Date).IsModified = true;

                entity.SaveChanges();
            }
        }
    }
}
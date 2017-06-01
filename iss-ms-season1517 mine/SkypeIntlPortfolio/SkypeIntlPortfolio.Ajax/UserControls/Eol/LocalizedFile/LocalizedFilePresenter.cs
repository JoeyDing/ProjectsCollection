using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class LocalizedFilePresenter
    {
        private ILocalizedFileBridge _bridge;
        private int _productKey;

        public LocalizedFilePresenter(ILocalizedFileBridge bridge, int productKey)
        {
            this._bridge = bridge;
            this._productKey = productKey;
            this._bridge.GetResourceFileOfProduct += _bridge_GetResourceFileOfProduct;
            this._bridge.onGetTotalRecord += _bridge_onGetTotalRecord;
            this._bridge.GetTargetFileByResourceFileKey += _bridge_GetTargetFileByResourceFileKey;
            this._bridge.onGetTotalRecordForTargetFile += _bridge_onGetTotalRecordForTargetFile;
        }

        private int _bridge_onGetTotalRecordForTargetFile(int fileKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var count = entity.ResourceFiles.First(c => c.FileKey == fileKey).ResourceFiles_Target.Count();
                return count;
            }
        }

        private List<ResourceFiles_Target_Base> _bridge_GetTargetFileByResourceFileKey(int fileKey, int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = entity.ResourceFiles.First(c => c.FileKey == fileKey).ResourceFiles_Target
                    .Join(entity.BasicLanguageLists, targetFile => targetFile.LanguageKey, basicLanguage => basicLanguage.LanguageKey, (targetFile, language) => new ResourceFiles_Target_Base() { TargetFileKey = targetFile.TargetFileKey, Target_File_Location = targetFile.Target_File_Location, LanguageName = language.Language, CultureName = language.CultureName });
                //.Select(x => new ResourceFiles_Target_Base() { File_Name = x.file.File_Name, Target_File_Location = x.file.Target_File_Location, LanguageName = x.language.Language, CultureName = x.language.CultureName });
                return result.OrderBy(c => c.LanguageName).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
        }

        private int _bridge_onGetTotalRecord()
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var count = entity.Products_New.First(c => c.ProductKey == _productKey).ResourceFiles.Count();
                return count;
            }
        }

        private List<ResourceFile> _bridge_GetResourceFileOfProduct(int pageIndex, int pageSize)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var result = entity.Products_New.First(c => c.ProductKey == _productKey).ResourceFiles.OrderBy(c => c.Source_File_Location).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                return result;
            }
        }
    }
}
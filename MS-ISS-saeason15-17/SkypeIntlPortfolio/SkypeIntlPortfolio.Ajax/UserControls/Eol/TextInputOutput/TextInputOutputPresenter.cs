using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class TextInputOutputPresenter
    {
        private ITextInputOutputBridge _bridge;
        private int _productKey;

        public TextInputOutputPresenter(ITextInputOutputBridge bridge, int productId)
        {
            this._productKey = productId;
            this._bridge = bridge;
            this._bridge.InsertFeature += _bridge_InsertFeature;
            this._bridge.UpdateFeature += _bridge_UpdateFeature;
            this._bridge.onInsertInputOutput += _bridge_onInsertTextInputOutput;
            this._bridge.onUpdateInputOutput += _bridge_onUpdateTextInputOutput;
            this._bridge.onDeleteInputOutput += _bridge_onDeleteTextInputOutput;
            this._bridge.GetFeatures += OnNeedFeaure;
            this._bridge.GetTextInputOutput += OnNeedTextInputOutput;
            this._bridge.GetLanguages += OnNeedLanguages;
            this._bridge.GetTotalRecord += _bridge_GetTotalRecord;
        }

        private int _bridge_GetTotalRecord()
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetTotalFeatureOfProduct(_productKey).FirstOrDefault().Value;
            }
        }

        private void _bridge_UpdateFeature(Feature e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.Features.Attach(e);
                entity.Entry(e).Property(x => x.FeatureName).IsModified = true;
                entity.SaveChanges();
            }
        }

        private List<GetFeatureOfProduct_Result> OnNeedFeaure(int startRow, int endRow)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetFeatureOfProduct(_productKey, startRow, endRow).ToList();
            }
        }

        private void _bridge_InsertFeature(string featureName)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                //entity.Features.Add(new Feature
                //{
                //    FeatureName = featureName,
                //    ProductKey = _productKey
                //});
                var feature = new Feature { FeatureName = featureName };
                entity.Products_New.First(c => c.ProductKey == _productKey).Features.Add(feature);
                entity.SaveChanges();
            }
        }

        private List<BasicLanguageList> OnNeedLanguages()
        {
            //load list of languages
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.BasicLanguageLists.ToList();
            }
        }

        private List<GetTextInputOutputOfProduct_Result> OnNeedTextInputOutput(int featureKey)
        {
            //load list of features
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetTextInputOutputOfProduct(featureKey).ToList();
            }
        }

        private void _bridge_onUpdateTextInputOutput(object sender, TextInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.TextInputOutputs.Attach(e);
                entity.Entry(e).Property(x => x.LanguageKey).IsModified = true;
                entity.Entry(e).Property(x => x.Text_Input).IsModified = true;
                entity.Entry(e).Property(x => x.Text_Output).IsModified = true;
                entity.Entry(e).Property(x => x.Comments).IsModified = true;

                entity.SaveChanges();
            }
        }

        private void _bridge_onInsertTextInputOutput(object sender, TextInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.TextInputOutputs.Add(e);
                entity.SaveChanges();
            }
        }

        private void _bridge_onDeleteTextInputOutput(object sender, TextInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var tio = entity.TextInputOutputs.First(c => c.TextInputOutputKey == e.TextInputOutputKey);
                entity.TextInputOutputs.Remove(tio);
                entity.SaveChanges();

                //int prodKey = 4;
                //var prod = entity.Products_New.First(c => c.ProductKey == prodKey);
                //prod.Features.Clear();

                //if (!prod.Features.Any())
                //    entity.Products_New.Remove(prod);
            }
        }
    }
}
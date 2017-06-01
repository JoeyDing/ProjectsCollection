using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public class SpokenInputOutputPresenter
    {
        private ISpokenInputOutputView _bridge;
        private int _productKey;

        public SpokenInputOutputPresenter(ISpokenInputOutputView bridge, int productId)
        {
            this._productKey = productId;
            this._bridge = bridge;
            this._bridge.onInsertInputOutput += _bridge_onInsertSpokenInputOutput;
            this._bridge.onUpdateInputOutput += _bridge_onUpdateSpokenInputOutput;
            this._bridge.onDeleteInputOutput += _bridge_onDeleteSpokenInputOutput;
            this._bridge.GetFeatures += OnNeedFeatures;
            this._bridge.GetSpokenInputOutput += OnNeedSpokenInputOutput;
            this._bridge.GetLanguages += OnNeedLanguages;
            this._bridge.InsertFeature += _bridge_InsertFeature;
            this._bridge.UpdateFeature += _bridge_UpdateFeature;
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

        private void _bridge_InsertFeature(string featureName)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.Features.Add(new Feature
                {
                    FeatureName = featureName,
                    ProductKey = _productKey
                });
                entity.SaveChanges();
            }
        }

        private List<GetSpokenInputOutputOfProduct_Result> OnNeedSpokenInputOutput(int featureKey)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetSpokenInputOutputOfProduct(featureKey).ToList();
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

        private List<GetFeatureOfProduct_Result> OnNeedFeatures(int startRow, int endRow)
        {
            //load list of features
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                return entity.GetFeatureOfProduct(_productKey, startRow, endRow).ToList();
            }
        }

        private void _bridge_onUpdateSpokenInputOutput(object sender, SpokenInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.SpokenInputOutputs.Attach(e);
                entity.Entry(e).Property(x => x.LanguageKey).IsModified = true;
                entity.Entry(e).Property(x => x.Spoken_Input).IsModified = true;
                entity.Entry(e).Property(x => x.Spoken_Output).IsModified = true;
                entity.Entry(e).Property(x => x.Comments).IsModified = true;

                entity.SaveChanges();
            }
        }

        private void _bridge_onInsertSpokenInputOutput(object sender, SpokenInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                entity.SpokenInputOutputs.Add(e);
                entity.SaveChanges();
            }
        }

        private void _bridge_onDeleteSpokenInputOutput(object sender, SpokenInputOutput e)
        {
            using (var entity = new SkypeIntlPlanningPortfolioEntities())
            {
                var tio = entity.SpokenInputOutputs.First(c => c.SpokenInputOutputKey == e.SpokenInputOutputKey);
                entity.SpokenInputOutputs.Remove(tio);
                entity.SaveChanges();
            }
        }
    }
}
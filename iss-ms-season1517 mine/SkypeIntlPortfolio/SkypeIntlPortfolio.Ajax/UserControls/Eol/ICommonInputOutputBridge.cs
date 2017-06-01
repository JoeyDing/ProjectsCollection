using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface ICommonInputOutputBridge
    {
        List<BasicLanguageList> BasicLanguage_List { get; set; }
        List<GetFeatureOfProduct_Result> FeatureOfProduct_Result { get; set; }

        int TotalRecord { get; set; }

        event Action<string> InsertFeature;

        event Action<Feature> UpdateFeature;

        event Func<int, int, List<GetFeatureOfProduct_Result>> GetFeatures;

        event Func<List<BasicLanguageList>> GetLanguages;

        event Func<int> GetTotalRecord;
    }
}
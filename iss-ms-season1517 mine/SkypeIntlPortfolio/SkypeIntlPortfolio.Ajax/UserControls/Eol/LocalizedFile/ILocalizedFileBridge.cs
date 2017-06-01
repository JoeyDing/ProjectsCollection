using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public interface ILocalizedFileBridge
    {
        event Func<int, int, List<ResourceFile>> GetResourceFileOfProduct;

        event Func<int> onGetTotalRecord;

        event Func<int, int, int, List<ResourceFiles_Target_Base>> GetTargetFileByResourceFileKey;

        event Func<int, int> onGetTotalRecordForTargetFile;
    }

    [Serializable]
    public partial class LocalizedFile
    {
        private string FabricProduct;
        private string ProjectFile;
        private string culture;
        private string Fabric_Tenant;
        private string VSO;
        private string Product_Family;
    }
}
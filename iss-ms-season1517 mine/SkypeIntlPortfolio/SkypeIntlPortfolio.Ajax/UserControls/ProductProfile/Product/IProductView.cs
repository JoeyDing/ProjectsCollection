using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Product
{
    public interface IProductView : IClickNext
    {
        string ProductName { get; set; }
        string ProductDecsription { get; set; }

        string VsoAreaPath { get; set; }

        //List<PFamily> ProductFamily { get; set; }
        List<PStatus> ProductStatus { get; set; }

        List<PVoice> ProductVoice { get; set; }
        List<PFabricTenant> FabricTenant { get; set; }

        List<PThread> ProductThread { get; set; }

        List<PPillar> ProductPillar { get; set; }

        bool isVisible { get; set; }

        event Action LoadPPProduct;

        IReadOnlyList<SelectableItem> ProductFamiles { get; set; }
    }

    [Serializable]
    public class PFamily
    {
        public string Family { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class PStatus
    {
        public string Status { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class PVoice
    {
        public string Voice { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class PFabricTenant
    {
        public string FabricTenant { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class PThread
    {
        public string Thread { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class PPillar
    {
        public string Pillar { get; set; }
        public bool IsChecked { get; set; }
    }
}
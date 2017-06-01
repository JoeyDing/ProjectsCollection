using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Localization
{
    public interface ILocalizationView : IClickNext
    {
        IReadOnlyCollection<SelectableItem> IntlBuildProcess { get; set; }
        IReadOnlyCollection<SelectableItem> LocProcess { get; set; }

        event Action LoadLocalizationData;
    }
}
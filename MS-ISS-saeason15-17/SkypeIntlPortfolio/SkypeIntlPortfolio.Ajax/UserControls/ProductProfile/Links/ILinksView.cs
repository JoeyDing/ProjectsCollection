using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links
{
    public interface ILinksView : IClickNext
    {
        string VSOLinkLocalization { get; set; }
        string VSOlinkCore { get; set; }
        string BuildLocation { get; set; }
        string LCGLocation { get; set; }
        string LCTLocation { get; set; }
        string LCLLocation { get; set; }
        event Action LoadLinkData;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.PseudoInfo
{
    public interface IPseudoInfoView : IClickNext
    {
        bool? PseudoBuildEnabled { get; set; }
        bool? PseudoTestingRunRegular { get; set; }
        bool? PseudoTestingNLocChecks { get; set; }
        bool? PseudoRunBeofreCheckIn { get; set; }

        event Action LoadPseudoInfo;
    }
}
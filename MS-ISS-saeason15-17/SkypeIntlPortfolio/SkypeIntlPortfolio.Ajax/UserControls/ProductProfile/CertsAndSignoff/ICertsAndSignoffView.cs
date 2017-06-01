using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.CertsAndSignoff
{
    public interface ICertsAndSignoffView : IClickNext
    {
        bool? GBImpacting { get; set; }
        bool? FrenchLocRequired { get; set; }
        bool? PrivacyStatementrequired { get; set; }
        bool? VoicePromptLocrequirement { get; set; }
        bool? TelemetryDataAvailable { get; set; }
        string CertType { get; set; }
        string CertLocation { get; set; }

        event Action LoadCertsAndSignoffData;
    }
}
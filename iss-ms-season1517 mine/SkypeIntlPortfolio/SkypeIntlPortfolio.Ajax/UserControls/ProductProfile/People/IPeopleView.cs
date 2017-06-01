using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.People
{
    public interface IPeopleView : IClickNext
    {
        string MSPMOwner { get; set; }
        string ISSOwner { get; set; }
        string ISSIPE { get; set; }
        string ISSTester { get; set; }
        string CoreTeamContact { get; set; }
        string CoreDesignContact { get; set; }
        string CoreTeamSharePoint { get; set; }
        string TelemetryContact { get; set; }
        string CoreEngineeringContact { get; set; }

        List<CoreTeamLocation> CoreTeamLocation { get; set; }
        List<MSPMOwnerLocation> MSPMOwnerLocation { get; set; }

        event Action LoadPPPeopleInfo;
    }

    [Serializable]
    public class CoreTeamLocation
    {
        public string CoreTeamLocationName { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class MSPMOwnerLocation
    {
        public string MSPMOwnerLocationName { get; set; }
        public bool IsChecked { get; set; }
    }
}
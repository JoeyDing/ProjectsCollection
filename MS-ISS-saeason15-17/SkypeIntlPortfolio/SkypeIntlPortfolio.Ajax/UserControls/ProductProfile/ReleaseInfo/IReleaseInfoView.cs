using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.ReleaseInfo
{
    public interface IReleaseInfoView : IClickNext
    {
        List<ReleaseCadence> ReleaseCadence { get; set; }
        List<ReleaseChannel> ReleaseChannel { get; set; }
        List<ReleasePlatform> ReleasePlatform { get; set; }
        List<LanguageSelection> LanguageSelection { get; set; }
        List<ContentLocation> ContentLocation { get; set; }
        bool Disable { get; set; }

        event EventHandler GetReleaseCadence;

        event EventHandler GetReleaseChannel;

        event Action LoadPPReleaseInfo;
    }

    [Serializable]
    public class ReleaseCadence
    {
        public string ReleaseCadenceName { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class ReleaseChannel
    {
        public string ReleaseChannelName { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class ReleasePlatform
    {
        public string ReleasePlatformName { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class LanguageSelection
    {
        public string LanguageSelectionName { get; set; }
        public bool IsChecked { get; set; }
    }

    [Serializable]
    public class ContentLocation
    {
        public string ContentLocationName { get; set; }
        public bool IsChecked { get; set; }
    }
}
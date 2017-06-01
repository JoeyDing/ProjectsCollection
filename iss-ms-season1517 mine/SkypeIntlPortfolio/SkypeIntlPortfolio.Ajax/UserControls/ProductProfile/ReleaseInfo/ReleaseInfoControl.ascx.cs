using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.ReleaseInfo
{
    public partial class ReleaseInfoControl : System.Web.UI.UserControl, IReleaseInfoView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.ReleaseCadence == null)
                this.ReleaseCadence = new List<ReleaseCadence>();

            if (this.ReleaseChannel == null)
                this.ReleaseChannel = new List<ReleaseChannel>();

            if (this.ReleasePlatform == null)
                this.ReleasePlatform = new List<ReleasePlatform>();

            if (this.ContentLocation == null)
                this.ContentLocation = new List<ContentLocation>();

            if (this.LanguageSelection == null)
                this.LanguageSelection = new List<LanguageSelection>();

            //there is a ! mark, means it's page refresh
            if (!IsPostBack)
            {
                if (this.Visible == true && this.LoadPPReleaseInfo != null)
                {
                    this.LoadPPReleaseInfo();
                    ////load Channel
                    //if (this.GetReleaseChannel != null)
                    //    this.GetReleaseChannel(sender, e);

                    ////load Cadence
                    //if (this.GetReleaseCadence != null)
                    //    this.GetReleaseCadence(sender, e);

                    foreach (var releaseCadenceItem in this.ReleaseCadence)
                    {
                        this.radioButtonList_ReleaseCadence.Items.Add(new ListItem { Selected = releaseCadenceItem.IsChecked, Text = releaseCadenceItem.ReleaseCadenceName });
                    }

                    foreach (var releaseChannelItem in this.ReleaseChannel)
                    {
                        this.radListBox_ReleaseChannel.Items.Add(new RadListBoxItem { Checked = releaseChannelItem.IsChecked, Text = releaseChannelItem.ReleaseChannelName });
                    }

                    foreach (var releasePlatformItem in this.ReleasePlatform)
                    {
                        this.radListBox_platforms.Items.Add(new RadListBoxItem { Checked = releasePlatformItem.IsChecked, Text = releasePlatformItem.ReleasePlatformName });
                    }

                    foreach (var contentLocationItem in this.ContentLocation)
                    {
                        this.radioButtonList_ContentLocation.Items.Add(new ListItem { Selected = contentLocationItem.IsChecked, Text = contentLocationItem.ContentLocationName });
                    }

                    foreach (var languageSelectionItem in this.LanguageSelection)
                    {
                        this.radioButtonList_LanguageSelection.Items.Add(new ListItem { Selected = languageSelectionItem.IsChecked, Text = languageSelectionItem.LanguageSelectionName });
                    }
                }
            }
        }

        public event EventHandler OnClickNext;

        public List<ReleaseCadence> ReleaseCadence
        {
            get
            {
                return this.ViewState["ReleaseCadence"] as List<ReleaseCadence>;
            }
            set
            {
                this.ViewState["ReleaseCadence"] = value;
            }
        }

        public List<ReleaseChannel> ReleaseChannel
        {
            get
            {
                return this.ViewState["ReleaseChannel"] as List<ReleaseChannel>;
            }
            set
            {
                this.ViewState["ReleaseChannel"] = value;
            }
        }

        public List<ReleasePlatform> ReleasePlatform
        {
            get
            {
                return this.ViewState["ReleasePlatform"] as List<ReleasePlatform>;
            }
            set
            {
                this.ViewState["ReleasePlatform"] = value;
            }
        }

        public List<LanguageSelection> LanguageSelection
        {
            get
            {
                return this.ViewState["LanguageSelection"] as List<LanguageSelection>;
            }
            set
            {
                this.ViewState["LanguageSelection"] = value;
            }
        }

        public List<ContentLocation> ContentLocation
        {
            get
            {
                return this.ViewState["ContentLocation"] as List<ContentLocation>;
            }
            set
            {
                this.ViewState["ContentLocation"] = value;
            }
        }

        public bool Disable
        {
            get
            {
                return true;
            }
            set
            {
                DisableControls();
            }
        }

        private void DisableControls()
        {
            //To enable/disable all validators of a validation group
            foreach (BaseValidator val in Page.Validators)
            {
                if (val.ValidationGroup == "group_releaseInfo")
                {
                    val.Enabled = false;
                }
            }
        }

        protected void RadButton_tab_ReleaseInfo_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (OnClickNext != null)
                OnClickNext(sender, e);
        }

        protected void radioButtonList_ReleaseCadence_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedReleaseCadence = this.radioButtonList_ReleaseCadence.SelectedItem.ToString();
            foreach (var item in this.ReleaseCadence)
            {
                if (selectedReleaseCadence == item.ReleaseCadenceName)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radListBox_ReleaseChannel_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            Dictionary<string, bool> releaseChannelNameList = this.radListBox_ReleaseChannel.CheckedItems.ToDictionary(x => x.Text, x => x.Checked);

            foreach (var item in this.ReleaseChannel)
            {
                item.IsChecked = true;
                if (releaseChannelNameList.ContainsKey(item.ReleaseChannelName))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radioButtonList_ContentLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedReleaseLocation = this.radioButtonList_ContentLocation.SelectedItem.ToString();
            foreach (var item in this.ContentLocation)
            {
                if (selectedReleaseLocation == item.ContentLocationName)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radioButtonList_LanguageSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedReleaseLanguage = this.radioButtonList_LanguageSelection.SelectedItem.ToString();
            foreach (var item in this.LanguageSelection)
            {
                if (selectedReleaseLanguage == item.LanguageSelectionName)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radListBox_platforms_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            Dictionary<string, bool> releasePlatformNameList = this.radListBox_platforms.CheckedItems.ToDictionary(x => x.Text, x => x.Checked);

            foreach (var item in this.ReleasePlatform)
            {
                item.IsChecked = true;
                if (releasePlatformNameList.ContainsKey(item.ReleasePlatformName))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        public event EventHandler GetReleaseCadence;

        public event EventHandler GetReleaseChannel;

        public event Action LoadPPReleaseInfo;
    }
}
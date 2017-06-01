using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.People
{
    public partial class PeopleControl : System.Web.UI.UserControl, IPeopleView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.CoreTeamLocation == null)
                this.CoreTeamLocation = new List<CoreTeamLocation>();

            if (this.MSPMOwnerLocation == null)
                this.MSPMOwnerLocation = new List<MSPMOwnerLocation>();

            if (!IsPostBack)
            {
                if (this.Visible == true && this.LoadPPPeopleInfo != null)
                {
                    this.LoadPPPeopleInfo();

                    foreach (var mSPMOwnerLocation in this.MSPMOwnerLocation)
                    {
                        this.radioButtonList_MSPMOwnerLocation.Items.Add(new ListItem { Selected = mSPMOwnerLocation.IsChecked, Text = mSPMOwnerLocation.MSPMOwnerLocationName });
                    }

                    foreach (var coreTeamLocationItem in this.CoreTeamLocation)
                    {
                        this.radListBox_CoreTeamLocation.Items.Add(new RadListBoxItem { Checked = coreTeamLocationItem.IsChecked, Text = coreTeamLocationItem.CoreTeamLocationName });
                    }
                }
            }
        }

        public event EventHandler OnClickNext;

        protected void RadButton_tab_people_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        public string MSPMOwner
        {
            get
            {
                return this.radTextBox_MSPMOwner.Text;
            }
            set
            {
                this.radTextBox_MSPMOwner.Text = value;
            }
        }

        public string ISSOwner
        {
            get
            {
                return this.radTextBox_iSSowner.Text;
            }
            set
            {
                this.radTextBox_iSSowner.Text = value;
            }
        }

        public string ISSIPE
        {
            get
            {
                return this.radTextBox_ISSIPE.Text;
            }
            set
            {
                this.radTextBox_ISSIPE.Text = value;
            }
        }

        public string ISSTester
        {
            get
            {
                return this.radTextBox_ISSTester.Text;
            }
            set
            {
                this.radTextBox_ISSTester.Text = value;
            }
        }

        public string CoreTeamContact
        {
            get
            {
                return this.radTextBox_CoreTeamContact.Text;
            }
            set
            {
                this.radTextBox_CoreTeamContact.Text = value;
            }
        }

        public string CoreTeamSharePoint
        {
            get
            {
                return this.radTextBox_CoreTeamSharePoint.Text;
            }
            set
            {
                this.radTextBox_CoreTeamSharePoint.Text = value;
            }
        }

        public bool Disable
        {
            get
            {
                //return !this.radTextBox_MSPMOwner.Enabled;
                return true;
            }
            set
            {
                disableControls();
            }
        }

        private void disableControls()
        {
            //this.radTextBox_MSPMOwner.Enabled = false;
            //this.radTextBox_iSSowner.Enabled = false;
            ////this.radTextBox_ISSIPE.Enabled = false;
            //this.radTextBox_ISSTester.Enabled = false;
            //this.radTextBox_CoreTeamContact.Enabled = false;
            //this.radTextBox_CoreTeamLocation.Enabled = false;
            //this.radTextBox_MSPMOwnerLocation.Enabled = false;

            //To enable/disable all validators of a validation group
            //foreach (BaseValidator val in Page.Validators)
            //{
            //    if (val.ValidationGroup == "group_people")
            //    {
            //        val.Enabled = false;
            //    }
            //}
        }

        protected void radioButtonList_MSPMOwnerLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMSPMOwnerLocation = this.radioButtonList_MSPMOwnerLocation.SelectedItem.ToString();
            foreach (var item in this.MSPMOwnerLocation)
            {
                if (selectedMSPMOwnerLocation == item.MSPMOwnerLocationName)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radListBox_CoreTeamLocation_ItemCheck(object sender, Telerik.Web.UI.RadListBoxItemEventArgs e)
        {
            Dictionary<string, bool> coreTeamLocationList = this.radListBox_CoreTeamLocation.CheckedItems.ToDictionary(x => x.Text, x => x.Checked);

            foreach (var item in this.CoreTeamLocation)
            {
                item.IsChecked = true;
                if (coreTeamLocationList.ContainsKey(item.CoreTeamLocationName))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        public event Action LoadPPPeopleInfo;

        public List<CoreTeamLocation> CoreTeamLocation
        {
            get
            {
                return this.ViewState["CoreTeamLocation"] as List<CoreTeamLocation>;
            }
            set
            {
                this.ViewState["CoreTeamLocation"] = value;
            }
        }

        public List<MSPMOwnerLocation> MSPMOwnerLocation
        {
            get
            {
                return this.ViewState["MSPMOwnerLocation"] as List<MSPMOwnerLocation>;
            }
            set
            {
                this.ViewState["MSPMOwnerLocation"] = value;
            }
        }

        public string TelemetryContact
        {
            get
            {
                return this.radTextBox_TelemetryContact.Text;
            }
            set
            {
                this.radTextBox_TelemetryContact.Text = value;
            }
        }

        public string CoreEngineeringContact
        {
            get
            {
                return this.radTextBox_CoreEngineeringContact.Text;
            }
            set
            {
                this.radTextBox_CoreEngineeringContact.Text = value;
            }
        }

        public string CoreDesignContact
        {
            get
            {
                return this.radTextBox_CoreDesignContact.Text;
            }
            set
            {
                this.radTextBox_CoreDesignContact.Text = value;
            }
        }
    }
}
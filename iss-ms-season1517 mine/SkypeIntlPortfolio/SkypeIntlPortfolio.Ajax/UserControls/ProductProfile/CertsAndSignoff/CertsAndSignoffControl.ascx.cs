using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.CertsAndSignoff
{
    public partial class CertsAndSignoffControl : System.Web.UI.UserControl, ICertsAndSignoffView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Visible == true && this.LoadCertsAndSignoffData != null)
                {
                    this.LoadCertsAndSignoffData();
                }
            }
        }

        protected void RadButton_tab_CertsAndSignoff_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        public bool? GBImpacting
        {
            get
            {
                return this.radioButtonList_GBimpacting.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_GBimpacting.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_GBimpacting.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_GBimpacting.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_GBimpacting.ClearSelection();
                }
            }
        }

        public bool? FrenchLocRequired
        {
            get
            {
                return this.radioButtonList_FrenchLocrequired.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_FrenchLocrequired.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_FrenchLocrequired.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_FrenchLocrequired.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_FrenchLocrequired.ClearSelection();
                }
            }
        }

        public bool? PrivacyStatementrequired
        {
            get
            {
                return this.radioButtonList_PrivacyStatementRequired.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_PrivacyStatementRequired.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_PrivacyStatementRequired.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_PrivacyStatementRequired.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_PrivacyStatementRequired.ClearSelection();
                }
            }
        }

        public bool? VoicePromptLocrequirement
        {
            get
            {
                return this.radioButtonList_VoicePromptLocRequirement.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_VoicePromptLocRequirement.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_VoicePromptLocRequirement.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_VoicePromptLocRequirement.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_VoicePromptLocRequirement.ClearSelection();
                }
            }
        }

        public bool? TelemetryDataAvailable
        {
            get
            {
                return this.radioButtonList_TelemetryDataAvailable.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_TelemetryDataAvailable.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_TelemetryDataAvailable.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_TelemetryDataAvailable.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_VoicePromptLocRequirement.ClearSelection();
                }
            }
        }

        public string CertType
        {
            get
            {
                return this.radTextBox_CertType.Text;
            }
            set
            {
                this.radTextBox_CertType.Text = value;
            }
        }

        public string CertLocation
        {
            get
            {
                return this.radTextBox_CertLocation.Text;
            }
            set
            {
                this.radTextBox_CertLocation.Text = value;
            }
        }

        public event EventHandler OnClickNext;

        public event Action LoadCertsAndSignoffData;
    }
}
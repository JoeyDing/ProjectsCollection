using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.PseudoInfo
{
    public partial class PseudoInfoControl : System.Web.UI.UserControl, IPseudoInfoView
    {
        public bool? PseudoBuildEnabled
        {
            get
            {
                return this.radioButtonList_PseudoBuildEnabled.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_PseudoBuildEnabled.SelectedItem.Text == "Yes";
            }
            set
            {
                if (value.HasValue)
                {
                    if (value.Value)

                        this.radioButtonList_PseudoBuildEnabled.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_PseudoBuildEnabled.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_PseudoBuildEnabled.ClearSelection();
                }
            }
        }

        public bool? PseudoRunBeofreCheckIn
        {
            get
            {
                return this.radioButtonList_pseRunDevtime.SelectedItem == null ?
                default(Nullable<bool>) :
                this.radioButtonList_pseRunDevtime.SelectedItem.Text == "Yes";
            }

            set
            {
                if (value.HasValue)
                {
                    if (value.Value)
                        this.radioButtonList_pseRunDevtime.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_pseRunDevtime.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_pseRunDevtime.ClearSelection();
                }
            }
        }

        public bool? PseudoTestingNLocChecks
        {
            get
            {
                return this.radioButtonList_PseTestAndLocCheck.SelectedItem == null ?
                default(Nullable<bool>) :
                this.radioButtonList_PseTestAndLocCheck.SelectedItem.Text == "Yes";
            }

            set
            {
                if (value.HasValue)
                {
                    if (value.Value)
                        this.radioButtonList_PseTestAndLocCheck.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_PseTestAndLocCheck.Items.FindByText("No").Selected = true;
                }
                else
                {
                    this.radioButtonList_PseTestAndLocCheck.ClearSelection();
                }
            }
        }

        public bool? PseudoTestingRunRegular
        {
            get
            {
                return this.radioButtonList_PseTestingRunOnRegular.SelectedItem == null ?
                    default(Nullable<bool>) :
                    this.radioButtonList_PseTestingRunOnRegular.SelectedItem.Text == "Yes";
            }

            set
            {
                if (value.HasValue)
                {
                    if (value.Value)
                        this.radioButtonList_PseTestingRunOnRegular.Items.FindByText("Yes").Selected = true;
                    else
                        this.radioButtonList_PseTestingRunOnRegular.Items.FindByText("No").Selected = true;
                }
                else
                    this.radioButtonList_PseTestingRunOnRegular.ClearSelection();
            }
        }

        public event Action LoadPseudoInfo;

        public event EventHandler OnClickNext;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (this.Visible == true && this.LoadPseudoInfo != null)
                {
                    this.LoadPseudoInfo();
                }
            }
        }

        protected void RadButton_tab_PseudoInfo_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }
    }
}
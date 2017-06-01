using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Localization
{
    public partial class LocalizationControl : System.Web.UI.UserControl, ILocalizationView
    {
        public event EventHandler OnClickNext;

        public event Action LoadLocalizationData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (!this.IsPostBack && this.LoadLocalizationData != null)
                {
                    this.LoadLocalizationData();
                }
            }
        }

        protected void RadButton_tab_Localization_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        public IReadOnlyCollection<Mvp.SelectableItem> IntlBuildProcess
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetSelectable(this.radlistbox_intlBuildProcess);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetSelectable(this.radlistbox_intlBuildProcess, value);
            }
        }

        public IReadOnlyCollection<Mvp.SelectableItem> LocProcess
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetSelectable(this.radlistbox_locProcess);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetSelectable(this.radlistbox_locProcess, value);
            }
        }
    }
}
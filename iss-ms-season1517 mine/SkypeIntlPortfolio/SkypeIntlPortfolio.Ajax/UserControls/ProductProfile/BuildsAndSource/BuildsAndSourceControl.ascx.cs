using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.BuildsAndSource
{
    public partial class BuildsAndSourceControl : System.Web.UI.UserControl, IBuildsAndSourceView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.SourceControl == null)
                this.SourceControl = new List<CheckableItem>();

            if (this.SourceStorage == null)
                this.SourceStorage = new List<CheckableItem>();

            if (this.CodeReviewSystem == null)
                this.CodeReviewSystem = new List<SelectableItem>();

            if (this.Visible && !this.IsPostBack)
            {
                if (this.LoadBuildsAndSourceData != null)
                {
                    this.LoadBuildsAndSourceData();
                }
            }
        }

        protected void RadButton_tab_BuildsAndSource_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        public string SourceCodeLocation
        {
            get
            {
                return this.radTextBox_SourceCodeLocation.Text;
            }
            set
            {
                this.radTextBox_SourceCodeLocation.Text = value;
            }
        }

        public IReadOnlyList<CheckableItem> SourceControl
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetCheckable(this.radListBox_sourceControls);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetCheckable(this.radListBox_sourceControls, value);
            }
        }

        public IReadOnlyList<CheckableItem> SourceStorage
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetCheckable(this.radListBox_SourceStorage);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetCheckable(this.radListBox_SourceStorage, value);
            }
        }

        public IReadOnlyList<SelectableItem> CodeReviewSystem
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetSelectable(this.radListBox_codeReviewSystem);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetSelectable(this.radListBox_codeReviewSystem, value);
            }
        }

        public IReadOnlyList<SelectableItem> BuildSystems
        {
            get
            {
                return ViewHelper.Instance.RadListBox_GetSelectable(this.radListBox_BuildSystems);
            }
            set
            {
                ViewHelper.Instance.RadListBox_SetSelectable(this.radListBox_BuildSystems, value);
            }
        }

        public event Action LoadBuildsAndSourceData;

        public event EventHandler OnClickNext;
    }
}
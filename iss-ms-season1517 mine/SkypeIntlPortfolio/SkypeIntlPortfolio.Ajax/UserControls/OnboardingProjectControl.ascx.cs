using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class OnboardingProjectControl : System.Web.UI.UserControl
    {
        #region Properties

        public ViewMode ViewMode { get; set; }

        private bool disableComboBox;

        public event EventHandler OnOkButtonClicked;

        public event EventHandler OnCancelButtonClicked;

        public string ProductName
        {
            get
            {
                if (this.radComboBox_Products != null)
                {
                    return this.radComboBox_Products.Text;
                }
                return "";
            }
            set
            {
                if (this.radComboBox_Products != null)
                {
                    this.radComboBox_Products.Text = value;
                    this.radComboBox_Products.DataBind();
                }
            }
        }

        public string ProductKey
        {
            get
            {
                if (this.radComboBox_Products != null)
                {
                    return this.radComboBox_Products.SelectedValue.ToString();
                }
                return "";
            }
        }

        public string EpicLabel
        {
            get
            {
                if (this.radTextBoxEpicLabel != null)
                {
                    return this.radTextBoxEpicLabel.Text;
                }
                return "";
            }
            set
            {
                if (this.radTextBoxEpicLabel != null)
                {
                    this.radTextBoxEpicLabel.Text = value;
                }
            }
        }

        public string Core_intl_folders_location
        {
            get
            {
                if (this.radTextCoreIntlFolderLocation != null)
                {
                    return this.radTextCoreIntlFolderLocation.Text;
                }
                return "";
            }

            set
            {
                if (this.radTextCoreIntlFolderLocation != null)
                {
                    this.radTextCoreIntlFolderLocation.Text = value;
                }
            }
        }

        public string Core_source_file_path
        {
            get
            {
                if (this.radTextBox_CoreSourceFilePath != null)
                {
                    return this.radTextBox_CoreSourceFilePath.Text;
                }
                return "";
            }

            set
            {
                if (this.radTextBox_CoreSourceFilePath != null)
                {
                    this.radTextBox_CoreSourceFilePath.Text = value;
                }
            }
        }

        public bool Europe_skwttad_RW_permission
        {
            get
            {
                if (this.radButton_PermissionCheck != null)
                {
                    return this.radButton_PermissionCheck.Checked;
                }
                return this.radButton_PermissionCheck.Checked;
            }

            set
            {
                if (this.radButton_PermissionCheck != null)
                {
                    this.radButton_PermissionCheck.Checked = value;
                }
            }
        }

        public string EOL
        {
            get
            {
                if (this.radTextBox_EOL != null)
                {
                    return this.radTextBox_EOL.Text;
                }
                return "";
            }

            set
            {
                if (this.radTextBox_EOL != null)
                {
                    this.radTextBox_EOL.Text = value;
                }
            }
        }

        public DateTime? Expected_Date_for_Walking
        {
            get
            {
                if (this.radDatePickerWalking != null)
                {
                    return this.radDatePickerWalking.SelectedDate.Value;
                }
                return null;
            }
        }

        public DateTime? Expected_Date_for_Running
        {
            get
            {
                if (this.radDatePickerRunning != null)
                {
                    return this.radDatePickerRunning.SelectedDate.Value;
                }
                return null;
            }
        }

        public string WarningMessageLabel { get; set; }

        public bool IsValid
        {
            get
            {
                if (this.radComboBox_Products_Validator != null)
                {
                    this.radComboBox_Products_Validator.Validate();
                    this.epicLabel_RequiredFieldValidator.Validate();
                    this.coreIntlFolderLocation_Validator.Validate();
                    this.coreSourceFilePath_Validator.Validate();
                    this.eol_Validator.Validate();
                    this.walkingDate_Validator.Validate();
                    this.runningDate_Validator.Validate();

                    return this.radComboBox_Products_Validator.IsValid &&
                        this.epicLabel_RequiredFieldValidator.IsValid &&
                        this.coreSourceFilePath_Validator.IsValid &&
                        this.coreIntlFolderLocation_Validator.IsValid &&
                        this.eol_Validator.IsValid &&
                    this.walkingDate_Validator.IsValid &&
                    this.runningDate_Validator.IsValid;
                }
                return false;
            }
        }

        public bool ProductsRequiredFieldValidatorEnabled
        {
            get
            {
                if (this.radComboBox_Products_Validator != null)
                {
                    return this.radComboBox_Products_Validator.Enabled;
                }
                return true;
            }
            set
            {
                if (this.radComboBox_Products_Validator != null)
                {
                    this.radComboBox_Products_Validator.Enabled = value;
                }
            }
        }

        public bool DisableCombobox
        {
            get { return disableComboBox; }
            set
            {
                disableComboBox = value;
                this.radComboBox_Products.Enabled = value;
            }
        }

        #endregion Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            //var manager = RadAjaxManager.GetCurrent(this.Page);
            //manager.AjaxSettings.AddAjaxSetting(this.radButton_Ok, this.panel_productForm, this.loadingPanel2);

            if (!this.IsPostBack)
            {
                if (this.ViewMode == ViewMode.Page)
                {
                    using (SkypeIntlPlanningPortfolioEntities products = new SkypeIntlPlanningPortfolioEntities())
                    {
                        var productsList = (from p in products.Products
                                            where p.Fabric_Status == null
                                            select new { p.Product_Name, p.ProductKey }).ToList();

                        foreach (var item in productsList)
                        {
                            var radItem = new RadComboBoxItem();
                            radItem.Value = item.ProductKey.ToString();
                            radItem.Text = item.Product_Name;
                            this.radComboBox_Products.Items.Add(radItem);
                            radItem.DataBind();
                        }
                    }
                }
            }
            if (this.ViewMode == ViewMode.Window)
            {
                this.radButton_Ok.Visible = true;
                this.radButton_Cancel.Visible = true;
                if (this.IsValid == false)
                {
                    this.LabelWarningMessage.Visible = true;
                }
                else
                {
                    this.LabelWarningMessage.Visible = false;
                }
            }
            else if (this.ViewMode == ViewMode.Page)
            {
                this.radButton_Ok.Visible = false;
                this.radButton_Cancel.Visible = false;
                this.LabelWarningMessage.Visible = false;
            }
        }

        protected void RadComboBoxProducts_SelectedIndexChanged(object o, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            this.radTextBoxEpicLabel.Text = "Fabric_" + e.Text.Trim().Replace(" ", "_");
            this.radComboBox_Products.Text = e.Text;
        }

        protected void Check_Clicked(Object sender, EventArgs e)
        {
            RadButton clickedButton = (RadButton)sender;
            if (clickedButton.Checked)
            {
                this.LabelMessage.Visible = false;
            }
            else
                this.LabelMessage.Visible = true;
        }

        protected void OK_Click(object sender, EventArgs e)
        {
            this.LabelWarningMessage.Visible = !this.IsValid;
            if (this.OnOkButtonClicked != null)
                this.OnOkButtonClicked(this, e);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            if (this.OnCancelButtonClicked != null)
                this.OnCancelButtonClicked(this, e);
        }

        public void DisplayWarningMessage(object sender, EventArgs e)
        {
            this.LabelWarningMessage.Visible = true;
        }

        public void HideTheProductForm(object sender, EventArgs e)
        {
            this.panel_productForm.Visible = false;
        }
    }
}
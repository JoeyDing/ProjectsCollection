using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using SkypeIntlPortfolio.Ajax.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls
{
    public partial class ProductInfoControl : System.Web.UI.UserControl, ICustomProjectControl
    {
        #region Properties

        private bool canRefresh;
        private ProductInfo productInfo;
        private List<string> productFamilyList;

        //In the parent user control, expose child control through a property
        public OnboardingProjectControl OnboardingProjectControl
        {
            get
            {
                return this.custom_onboardingControl;
            }
        }

        public bool IsValid
        {
            get
            {
                {
                    this.requiredFieldValidator_productname.Validate();
                    this.requiredFieldValidator_famliy.Validate();
                    this.requiredFieldValidator_voice.Validate();
                    this.radTextBox_HL_Description_validator.Validate();

                    return this.requiredFieldValidator_productname.IsValid &&
                        this.requiredFieldValidator_famliy.IsValid &&
                        this.requiredFieldValidator_voice.IsValid &&
                        this.radTextBox_HL_Description_validator.IsValid;
                }
                return false;
            }
        }

        public string ProductName
        {
            get
            {
                if (this.radTextBox_productName != null)
                {
                    return this.radTextBox_productName.Text;
                }
                return "";
            }
            set
            {
                if (this.radTextBox_productName != null)
                {
                    this.radTextBox_productName.Text = value;
                }
            }
        }

        public string Family
        {
            get
            {
                if (this.radButton_List_Family != null)
                {
                    return this.radButton_List_Family.SelectedItem.Text;
                }
                return "";
            }
            set
            {
                if (this.radButton_List_Family != null)
                {
                    this.radButton_List_Family.SelectedItem.Text = value;
                }
            }
        }

        public string UserVoice
        {
            get
            {
                if (this.RadioButton_List_UserVoice != null)
                {
                    return this.RadioButton_List_UserVoice.SelectedItem.Text;
                }
                return "";
            }
            set
            {
                if (this.RadioButton_List_UserVoice != null)
                {
                    this.RadioButton_List_UserVoice.SelectedItem.Text = value;
                }
            }
        }

        public string PMLocation
        {
            get
            {
                if (this.radioButton_List_Location != null)
                {
                    return this.radioButton_List_Location.SelectedItem.Text;
                }
                return "";
            }
            set
            {
                if (this.radioButton_List_Location != null)
                {
                    this.radioButton_List_Location.SelectedItem.Text = value;
                }
            }
        }

        public string HighLevelProdDescription
        {
            get
            {
                if (this.radTextBox_HL_Description != null)
                {
                    return this.radTextBox_HL_Description.Text;
                }
                return "";
            }
            set
            {
                this.radTextBox_HL_Description.Text = value;
            }
        }

        public string PMAlias
        {
            get
            {
                if (this.radTextBox_PM_Alias != null)
                {
                    return this.radTextBox_PM_Alias.Text;
                }
                return "";
            }
            set
            {
                this.radTextBox_PM_Alias.Text = value;
            }
        }

        public string ResourceFilePath
        {
            get
            {
                if (this.radTextBox_resourceFilePath != null)
                {
                    return this.radTextBox_resourceFilePath.Text;
                }
                return "";
            }
            set
            {
                this.radTextBox_resourceFilePath.Text = value;
            }
        }

        public string CoreTeamLocation
        {
            get
            {
                if (this.radTextBox_CoreTeam_Location != null)
                {
                    return this.radTextBox_CoreTeam_Location.Text;
                }
                return "";
            }
            set
            {
                this.radTextBox_CoreTeam_Location.Text = value;
            }
        }

        public string CoreTeamContacts
        {
            get
            {
                if (this.radTextBox_coreTeamContacts != null)
                {
                    return this.radTextBox_CoreTeam_Location.Text;
                }
                return "";
            }
            set
            {
                this.radTextBox_CoreTeam_Location.Text = value;
            }
        }

        public string EditButtonText
        {
            get
            {
                if (this.radButton_FabricOnboardingEdit != null)
                {
                    return radButton_FabricOnboardingEdit.Text;
                }
                return "";
            }
            set
            {
                if (this.radButton_FabricOnboardingEdit != null)
                {
                    radButton_FabricOnboardingEdit.Text = value.ToString();
                }
            }
        }

        public bool FabricOnBoardingChecked
        {
            get
            {
                if (this.radButton_FabricOnBoardingCheck != null)
                {
                    return radButton_FabricOnBoardingCheck.Checked;
                }
                return false;
            }
            set
            {
                if (this.radButton_FabricOnBoardingCheck != null)
                {
                    radButton_FabricOnBoardingCheck.Checked = value;
                }
            }
        }

        public string ControlName
        {
            get;
            set;
        }

        public ProductInfo ProductInfo
        {
            get
            {
                return productInfo;
            }
            set
            {
                productInfo = value;
                canRefresh = true;
            }
        }

        #endregion Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            //var manager = RadAjaxManager.GetCurrent(this.Page);
            // managerProxy.AjaxSettings.AddAjaxSetting(this.radButton_FabricOnboardingEdit, this.panel_productForm, this.loadingPanel1);
            //manager.AjaxSettings.AddAjaxSetting(this.panel_productForm, this.radButton_FabricOnBoardingCheck, this.loadingPanel1);
            //manager.AjaxSettings.AddAjaxSetting(this.radButton_FabricOnboardingEdit, this.custom_onboardingControl, this.loadingPanel1);
            if (ControlName != null)
            {
                if (ControlName == "ProductInfoControl")
                {
                    DisablePartialControl();
                }
            }
        }

        public void DisablePartialControl()
        {
            this.radButton_FabricOnBoardingCheck.Enabled = false;
            this.radButton_FabricOnboardingEdit.Visible = false;
            this.radTextBox_productName.AutoPostBack = true;
            this.radButton_List_Family.AutoPostBack = true;
            this.radTextBox_productName.AutoPostBack = true;
            this.radButton_List_Family.AutoPostBack = true;
            this.RadioButton_List_UserVoice.AutoPostBack = true;
            this.radTextBox_HL_Description.AutoPostBack = true;
            this.radTextBox_PM_Alias.AutoPostBack = true;
            this.radioButton_List_Location.AutoPostBack = true;
            this.radTextBox_resourceFilePath.AutoPostBack = true;
            this.radTextBox_coreTeamContacts.AutoPostBack = true;
        }

        public void Refresh()
        {
            if (canRefresh)
            {
                if (productInfo != null)
                {
                    this.radTextBox_productName.Text = this.productInfo.Product_Name;

                    using (var context = new SkypeIntlPlanningPortfolioEntities())
                    {
                        productFamilyList = context.Products.Select(c => c.Family).Distinct().OrderBy(c => c).ToList();
                    }

                    if (productFamilyList.Any())
                    {
                        foreach (var family in productFamilyList)
                        {
                            var familyItem = new ListItem();
                            familyItem.Text = family;
                            familyItem.Value = family;
                            this.radButton_List_Family.Items.Add(familyItem);

                            //if (familyItem.Text == this.ProductInfo.Family)
                            //{
                            //    this.radButton_List_Family.SelectedValue = familyItem.Value;
                            //}
                        }
                    }

                    //if (!string.IsNullOrWhiteSpace(this.productInfo.User_Voice))
                    //{
                    //    this.RadioButton_List_UserVoice.Text = this.productInfo.User_Voice;
                    //}

                    //this.radTextBox_HL_Description.Text = this.productInfo.Product_Description;
                    //this.radTextBox_PM_Alias.Text = this.productInfo.PM_Alias;

                    //if (!string.IsNullOrWhiteSpace(this.productInfo.Loc_PM_Location))
                    //{
                    //    this.radioButton_List_Location.Text = this.productInfo.Loc_PM_Location;
                    //}

                    //this.radTextBox_CoreTeam_Location.Text = productInfo.Core_Team_Location;
                    //this.radTextBox_resourceFilePath.Text = productInfo.Resource_File_Path;
                    //this.radTextBox_coreTeamContacts.Text = productInfo.Core_Team_Contacts;

                    //if (!string.IsNullOrWhiteSpace(this.productInfo.Epic_Label) && !string.IsNullOrWhiteSpace(this.productInfo.Fabric_Status))
                    //{
                    //    this.radButton_FabricOnBoardingCheck.Checked = true;
                    //}
                    //else
                    //{
                    //    this.radButton_FabricOnBoardingCheck.Checked = false;
                    //}
                }
            }
        }

        protected void custom_onboardingControl_Load(object sender, EventArgs e)
        {
            //update custom control properties
            if (this.custom_onboardingControl != null)
            {
                this.custom_onboardingControl.ProductName = this.radTextBox_productName.Text;
                //disable the validator of combobox of products
                if (this.custom_onboardingControl.ProductName != "")
                {
                    this.custom_onboardingControl.ProductsRequiredFieldValidatorEnabled = false;
                }
                else
                {
                    this.custom_onboardingControl.ProductsRequiredFieldValidatorEnabled = true;
                }
                this.custom_onboardingControl.EpicLabel = "Fabric_" + this.radTextBox_productName.Text.Trim().Replace(" ", "_");
            }
        }

        protected void radButton_FabricOnboardingEdit_Click(object sender, EventArgs e)
        {
            //open window
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ShowWindow", string.Format("ShowExistingWindow('{0}');", "modal_fabricOnboarding"), true);
        }

        protected void customControl_OkButton_Click(object sender, EventArgs e)
        {
            if (this.custom_onboardingControl != null)
            {
                if (this.custom_onboardingControl.IsValid)
                {
                    this.radButton_FabricOnBoardingCheck.Checked = true;
                    this.EditButtonText = "Edit Onboarding details";
                    //close window
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", "modal_fabricOnboarding"), true);
                }
            }
        }

        protected void customControl_CancelButton_Click(object sender, EventArgs e)
        {
            if (this.custom_onboardingControl != null)
            {
                //close window
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CloseWindow", string.Format("CloseWindow('{0}');", "modal_fabricOnboarding"), true);
            }

            if (this.custom_onboardingControl.ProductName == "")
            {
                this.custom_onboardingControl.ProductsRequiredFieldValidatorEnabled = true;
            }
        }

        protected void radTextBox_productName_TextChanged(object sender, EventArgs e)
        {
        }

        protected void radButton_List_Family_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
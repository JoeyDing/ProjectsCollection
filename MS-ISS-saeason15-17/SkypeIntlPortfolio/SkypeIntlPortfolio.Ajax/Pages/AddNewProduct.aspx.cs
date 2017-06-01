using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class AddNewProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.custom_ProductInfoControl.SubmitButtonDisplay = true;
            this.radButton_submit.Visible = true;
        }

        protected void radButton_submit_Click(object sender, EventArgs e)
        {
            //insert the new product into table products
            //this.Page.Validate();
            if (true)
            {
                ProductInfo productInfo = new ProductInfo();
                FabricOnboardingInfo fabricOnboardingInfo = new FabricOnboardingInfo();
                //assign data to properties of ProductInfo
                productInfo.Product_Name = this.custom_ProductInfoControl.ProductName;
                //productInfo.Family = this.custom_ProductInfoControl.Family;
                //productInfo.User_Voice = this.custom_ProductInfoControl.UserVoice;
                //productInfo.Product_Description = this.custom_ProductInfoControl.HighLevelProdDescription;
                //productInfo.Loc_PM_Location = this.custom_ProductInfoControl.PMLocation;
                //productInfo.PM_Alias = this.custom_ProductInfoControl.PMAlias;
                //productInfo.Resource_File_Path = this.custom_ProductInfoControl.ResourceFilePath;
                //productInfo.Core_Team_Location = this.custom_ProductInfoControl.CoreTeamLocation;
                //productInfo.Core_Team_Contacts = this.custom_ProductInfoControl.CoreTeamContacts;

                //productInfo.Fabric_Onboarding_Request = this.custom_ProductInfoControl.FabricOnBoardingChecked;

                if (this.custom_ProductInfoControl.OnboardingProjectControl.IsValid)
                {
                    //if (productInfo.Fabric_Onboarding_Request == true)
                    //{
                    //    //assign data to properties of ProductOnboarding window
                    //    fabricOnboardingInfo.Product = this.custom_ProductInfoControl.OnboardingProjectControl.ProductName;
                    //    productInfo.Epic_Label = this.custom_ProductInfoControl.OnboardingProjectControl.EpicLabel;
                    //    fabricOnboardingInfo.EpicLabel = productInfo.Epic_Label;
                    //    fabricOnboardingInfo.Core_Intl_Folder_Location = this.custom_ProductInfoControl.OnboardingProjectControl.Core_intl_folders_location;
                    //    fabricOnboardingInfo.Source_File_Path = this.custom_ProductInfoControl.OnboardingProjectControl.Core_source_file_path;

                    //    if (this.custom_ProductInfoControl.OnboardingProjectControl.Europe_skwttad_RW_permission == true)
                    //    {
                    //        fabricOnboardingInfo.EuropesOrKWTTAD_RW_Permission = true;
                    //    }
                    //    else if (this.custom_ProductInfoControl.OnboardingProjectControl.Europe_skwttad_RW_permission == false)
                    //    {
                    //        fabricOnboardingInfo.EuropesOrKWTTAD_RW_Permission = false;
                    //    }
                    //    fabricOnboardingInfo.Expected_Date_for_Walking = this.custom_ProductInfoControl.OnboardingProjectControl.Expected_Date_for_Walking;
                    //    fabricOnboardingInfo.Expected_Date_for_Running = this.custom_ProductInfoControl.OnboardingProjectControl.Expected_Date_for_Running;

                    //    Utils.AddProduct(productInfo, fabricOnboardingInfo);
                    //    this.panel_productForm.Visible = false;
                    //    this.panel_Feedback.Visible = true;
                    //}
                }
                else if (!this.custom_ProductInfoControl.OnboardingProjectControl.IsValid)
                {
                    //productInfo.Epic_Label = "Null";
                    //fabricOnboardingInfo = null;
                    //Utils.AddProduct(productInfo, fabricOnboardingInfo);
                }
            }
            else
            {
                this.WarningMessageLabel.Visible = true;
            }
        }
    }
}
using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.Pages
{
    public partial class OnbardingProject : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void radButton_submit_Click(object sender, EventArgs e)
        {
            this.Validate();
            ProductInfo productInfo = new ProductInfo();
            FabricOnboardingInfo fabricOnboardingInfo = new FabricOnboardingInfo();
            if (this.custom_onboardingControl.IsValid)
            {
                //get the slected product key
                int productKey = Convert.ToInt32(this.custom_onboardingControl.ProductKey);

                //add data from controls to corresponding peoperties
                fabricOnboardingInfo.Product = this.custom_onboardingControl.ProductName;
                fabricOnboardingInfo.EpicLabel = this.custom_onboardingControl.EpicLabel;
                fabricOnboardingInfo.Core_Intl_Folder_Location = this.custom_onboardingControl.Core_intl_folders_location;
                fabricOnboardingInfo.Source_File_Path = this.custom_onboardingControl.Core_source_file_path;
                if (this.custom_onboardingControl.Europe_skwttad_RW_permission == true)
                {
                    fabricOnboardingInfo.EuropesOrKWTTAD_RW_Permission = true;
                }
                else if (this.custom_onboardingControl.Europe_skwttad_RW_permission == false)
                {
                    fabricOnboardingInfo.EuropesOrKWTTAD_RW_Permission = false;
                }
                fabricOnboardingInfo.Expected_Date_for_Walking = this.custom_onboardingControl.Expected_Date_for_Walking;
                fabricOnboardingInfo.Expected_Date_for_Running = this.custom_onboardingControl.Expected_Date_for_Running;

                Utils.OnboardProduct(productKey, fabricOnboardingInfo);
                //hide the form panel and display the feedback panel
                this.custom_onboardingControl.HideTheProductForm(sender, e);
                this.radButton_submit.Visible = false;
                this.panel_Feedback.Visible = true;
                this.panel_productForm.Visible = false;
            }
            else
            {
                this.custom_onboardingControl.DisplayWarningMessage(sender, e);
            }
        }
    }
}
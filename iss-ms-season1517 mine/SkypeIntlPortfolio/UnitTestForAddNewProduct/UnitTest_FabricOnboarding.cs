using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using System;

namespace UnitTestForAddNewProduct
{
    [TestClass]
    public class UnitTest_FabricOnboarding
    {
        [TestMethod]
        public void OnboardProduct_EpicLabelAndFabricStatus_NewAndFabricProdName()
        {
            //Set up
            ProductInfo productInfo = new ProductInfo();
            FabricOnboardingInfo fabricOnboardingInfo = new FabricOnboardingInfo();

            productInfo.Product_Name = "NNN";
            //productInfo.Product_Description = "DASDA";
            productInfo.Family = "Client";
            //productInfo.User_Voice = "Consumer";
            //insert a new product by calling AddNewProduct
            Product addedProduct = Utils.AddProduct(productInfo, fabricOnboardingInfo);
            //get the returned product key
            int productKey = addedProduct.ProductKey;
            fabricOnboardingInfo.EpicLabel = "Fabric_" + addedProduct.Product_Name;
            //Execute
            Product updatedProduct = Utils.OnboardProduct(productKey, fabricOnboardingInfo);
            //Assert
            Assert.AreEqual(updatedProduct.Fabric_Status, "New");
            Assert.AreEqual(updatedProduct.Fabric_Onboarding_EpicLabel, "Fabric_NNN");
        }
    }
}
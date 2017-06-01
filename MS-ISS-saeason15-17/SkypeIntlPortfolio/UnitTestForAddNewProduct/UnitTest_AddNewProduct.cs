using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntlPortfolio.Ajax;
using SkypeIntlPortfolio.Ajax.Model;
using SkypeIntlPortfolio.Ajax.Pages;
using System;

namespace UnitTestForAddNewProduct
{
    [TestClass]
    public class UnitTest_AddNewProduct
    {
        [TestMethod]
        public void AddProduct_CreateANewProduct_ExpectedResult()
        {
            //Set up
            ProductInfo productInfo = new ProductInfo();
            FabricOnboardingInfo fabricOnboardingInfo = new FabricOnboardingInfo();

            productInfo.Product_Name = "VVV";
            //productInfo.Product_Description = "VVVVV";
            productInfo.Family = "Client";
            //productInfo.User_Voice = "Consumer";
            fabricOnboardingInfo = null;
            //insert a new product by calling AddNewProduct
            //execute
            Product product = Utils.AddProduct(productInfo, fabricOnboardingInfo);
            //assert
            Assert.AreEqual(product.Product_Name, "VVV");
            Assert.AreEqual(product.Product_Description, "VVVVV");
            Assert.AreEqual(product.Family, "Client");
            Assert.AreEqual(product.Product_Status, null);
            Assert.AreEqual(product.Fabric_Onboarding_EpicLabel, null);
            Assert.AreEqual(product.User_Type, "Consumer");
        }

        [TestMethod]
        public void AddProduct_WithFabricOnboarding_FabricStatusAndEpicLabelChanged()
        {
            //Set up
            ProductInfo productInfo = new ProductInfo();
            FabricOnboardingInfo fabricOnboardingInfo = new FabricOnboardingInfo();
            productInfo.Product_Name = "XXX";
            //productInfo.Epic_Label = "Fabric_" + productInfo.Product_Name;
            //productInfo.Product_Description = "XXXXX";
            productInfo.Family = "Mobile";
            //productInfo.User_Voice = "Business";
            //insert a new product by calling AddNewProductWithFabricOnboarding
            //execute
            Product product = Utils.AddProduct(productInfo, fabricOnboardingInfo);
            //assert
            Assert.AreEqual(product.Product_Name, "XXX");
            Assert.AreEqual(product.Product_Description, "XXXXX");
            Assert.AreEqual(product.Family, "Mobile");
            Assert.AreEqual(product.Fabric_Status, "New");
            Assert.AreEqual(product.Fabric_Onboarding_EpicLabel, "Fabric_XXX");
            Assert.AreEqual(product.User_Type, "Business");
        }
    }
}
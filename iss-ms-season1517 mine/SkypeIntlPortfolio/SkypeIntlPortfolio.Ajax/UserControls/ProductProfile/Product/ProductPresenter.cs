using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Product
{
    public class ProductPresenter : PresenterBase
    {
        private IProductView view;

        private Products_New _productInfo;

        private const string C_Product_Family = "ProductFamily";
        private const string C_Product_Status = "ProductStatus";
        private const string C_Product_Voice = "ProductVoice";
        private const string C_Fabric_Tenant = "FabricTenant";
        private const string C_Product_Thread = "ProductThread";
        private const string C_Product_Pillar = "ProductPillar";

        //public event EventHandler OnClickNext;
        public event Action<int> OnClickNext;

        public ProductPresenter(IProductView view, Products_New productInfo)
        {
            this.view = view;
            _productInfo = productInfo;
            this.view.OnClickNext += view_OnClickNext;
            this.view.LoadPPProduct += view_LoadPPProduct;
        }

        private void view_LoadPPProduct()
        {
            if (this._productInfo != null)
            {
                //Get Product Name
                this.view.ProductName = this._productInfo.Product_Name;

                //get area path
                this.view.VsoAreaPath = this._productInfo.Localization_VSO_Path;

                //get description
                this.view.ProductDecsription = this._productInfo.Description;
            }
            view_GetProductFamily();
            view_GetStatus();
            view_GetVoice();
            view_GetFabricTenant();
            view_GetThread();
            view_GetPillar();
        }

        private void view_GetProductFamily()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedFamilyKey = 0;
                string selectedFamilyName = "";

                var familyList = portfolioContext.ProductFamilies.ToList();
                var productFamily = new List<ProductFamily>();
                if (_productInfo != null)
                {
                    selectedFamilyKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).Select(c => c.ProductFamilyKey).FirstOrDefault());
                    selectedFamilyName = portfolioContext.ProductFamilies.Where(c => c.FamilyKey == selectedFamilyKey).Select(c => c.Product_Family).FirstOrDefault();

                    if (this._productInfo.ProductFamilyKey != null)
                        productFamily.Add(familyList.First(c => c.FamilyKey == this._productInfo.ProductFamilyKey));
                }

                var unionItems = base.GetUnionOfItems(familyList, productFamily, (family) => family.FamilyKey);

                this.view.ProductFamiles = unionItems.Select(c => new SelectableItem
                {
                    Text = c.Value.Product_Family,
                    Value = c.Value.FamilyKey.ToString(),
                    IsSelected = c.IsCommon
                })
                    .ToList();
            }
        }

        private void view_GetStatus()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //since the selectProduct has been closed in the aboved "using" chunk from PPReleaseInfoPresenter, here we should requery data
                int selectedStatusKey = 0;
                string selectedStatusName = "";
                if (_productInfo != null)
                {
                    selectedStatusKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).Select(c => c.StatusKey).FirstOrDefault());
                    selectedStatusName = portfolioContext.Status.Where(c => c.StatusKey == selectedStatusKey).Select(c => c.Status1).FirstOrDefault();
                }

                var statusList = portfolioContext.Status.Select(r => r.Status1).ToList();
                var result = new List<PStatus>();
                foreach (string rcItem in statusList)
                {
                    PStatus newS = new PStatus();
                    newS.Status = rcItem;
                    result.Add(newS);
                    if (this._productInfo != null)
                    {
                        if (selectedStatusName == rcItem)
                        {
                            newS.IsChecked = true;
                        }
                    }
                }
                this.view.ProductStatus = result;
            }
        }

        private void view_GetVoice()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                //since the selectProduct has been closed in the aboved "using" chunk from PPReleaseInfoPresenter, here we should requery data
                int selectedVoiceKey = 0;
                string selectedVoiceName = "";
                if (_productInfo != null)
                {
                    selectedVoiceKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).Select(c => c.PVoiceKey).FirstOrDefault());
                    selectedVoiceName = portfolioContext.ProductVoices.Where(c => c.VoiceKey == selectedVoiceKey).Select(c => c.Product_Voice).FirstOrDefault();
                }

                var voiceList = portfolioContext.ProductVoices.Select(r => r.Product_Voice).ToList();
                var result = new List<PVoice>();
                foreach (string rcItem in voiceList)
                {
                    PVoice newS = new PVoice();
                    newS.Voice = rcItem;
                    result.Add(newS);
                    if (this._productInfo != null)
                    {
                        if (selectedVoiceName == rcItem)
                        {
                            newS.IsChecked = true;
                        }
                    }
                }
                this.view.ProductVoice = result;
            }
        }

        private void view_GetFabricTenant()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedFTKey = 0;
                string selectedFTName = "";
                if (_productInfo != null)
                {
                    selectedFTKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).Select(c => c.FabricTenantKey).FirstOrDefault());
                    selectedFTName = portfolioContext.FabricTenants.Where(c => c.TenantKey == selectedFTKey).Select(c => c.Fabric_Tenant).FirstOrDefault();
                }

                var ftList = portfolioContext.FabricTenants.Select(r => r.Fabric_Tenant).ToList();
                var result = new List<PFabricTenant>();
                foreach (string rcItem in ftList)
                {
                    PFabricTenant newFT = new PFabricTenant();
                    newFT.FabricTenant = rcItem;
                    result.Add(newFT);
                    if (this._productInfo != null)
                    {
                        if (selectedFTName == rcItem)
                        {
                            newFT.IsChecked = true;
                        }
                    }
                }
                this.view.FabricTenant = result;
            }
        }

        private void view_GetThread()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                int selectedThreadKey = 0;
                string selectedThread = "";
                if (_productInfo != null)
                {
                    selectedThreadKey = Convert.ToInt32(portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).Select(c => c.ThreadKey).FirstOrDefault());
                    selectedThread = portfolioContext.ProductThreads.Where(c => c.ThreadKey == selectedThreadKey).Select(c => c.Product_Thread).FirstOrDefault();
                }

                var threadList = portfolioContext.ProductThreads.Select(r => r.Product_Thread).ToList();
                var result = new List<PThread>();
                foreach (string rcItem in threadList)
                {
                    PThread newS = new PThread();
                    newS.Thread = rcItem;
                    result.Add(newS);
                    if (this._productInfo != null)
                    {
                        if (selectedThread == rcItem)
                        {
                            newS.IsChecked = true;
                        }
                    }
                }
                this.view.ProductThread = result;
            }
        }

        private void view_GetPillar()
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                Dictionary<string, int> selectedPillarList = new Dictionary<string, int>();
                if (_productInfo != null)
                {
                    //add selected pillars into the channellist
                    foreach (var item in portfolioContext.Products_New.Where(c => c.ProductKey == _productInfo.ProductKey).SelectMany(c => c.ProductPillars))
                    {
                        selectedPillarList.Add(item.Product_Pillar, item.PillarKey);
                    }
                }

                var pillarList = portfolioContext.ProductPillars.Select(c => c.Product_Pillar).ToList();
                var result = new List<PPillar>();
                foreach (string rclItem in pillarList)
                {
                    PPillar newPillar = new PPillar();
                    newPillar.Pillar = rclItem;
                    result.Add(newPillar);

                    if (selectedPillarList.ContainsKey(rclItem))
                    {
                        newPillar.IsChecked = true;
                    }
                    else
                    {
                        newPillar.IsChecked = false;
                    }
                }

                this.view.ProductPillar = result;
            }
        }

        private void view_OnClickNext(object sender, EventArgs e)
        {
            int newProductKey;
            this.view.isVisible = false;
            //insert new product to DB
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["Product"]))
            {
                //bubble up click event for any parent handler
                if (canSaveNewProductDataInDB(true, out newProductKey))
                {
                    if (this.OnClickNext != null)
                        this.OnClickNext(newProductKey);
                }
            }
            else
            {
                //update existing product
                if (canSaveNewProductDataInDB(false, out newProductKey))
                {
                    //bubble up click event for any parent handler
                    if (this.OnClickNext != null)
                        this.OnClickNext(newProductKey);
                }
            }
        }

        /// <summary>
        /// if input data can be inserted into db successfully, canSwitchPage is true
        /// </summary>
        /// <param name="isNewProduct"></param>
        /// <param name="targetProduct"></param>
        /// <returns></returns>
        protected bool canSaveNewProductDataInDB(bool isNewProduct, out int newProductKey)
        {
            using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
            {
                string currentProductName = this.view.ProductName;
                if (isNewProduct)
                {
                    if (portfolioContext.Products_New.Any(p => p.Product_Name == currentProductName))
                    {
                        this.view.isVisible = true;
                        newProductKey = 0;
                        return false;
                    }
                    else
                    {
                        Products_New newProduct = new Products_New();
                        AssignDataToProduct(newProduct, portfolioContext);
                        portfolioContext.Products_New.Add(newProduct);
                        portfolioContext.SaveChanges();
                        newProductKey = newProduct.ProductKey;
                        //todo:
                        //update current url,add new added product number in the url
                        //this.SelectedProducts.Add(newProduct.ProductKey, newProduct.Product_Name);
                        return true;
                    }
                }
                else
                {
                    int currentSelectedProduct = Convert.ToInt32(HttpContext.Current.Request.QueryString["Product"]);
                    var productToUpdate = portfolioContext.Products_New.FirstOrDefault(p => p.ProductKey == currentSelectedProduct);
                    AssignDataToProduct(productToUpdate, portfolioContext);
                    portfolioContext.SaveChanges();
                    newProductKey = 0;
                    return true;
                }
            }
        }

        protected void AssignDataToProduct(Products_New targetProduct, SkypeIntlPlanningPortfolioEntities portfolioContext)
        {
            //save data in the db
            int selectedFamilyKey = this.view.ProductFamiles.Where(x => x.IsSelected).Select(x => int.Parse(x.Value)).First();
            string selectedStatus = this.view.ProductStatus.Where(x => x.IsChecked).Select(x => x.Status).FirstOrDefault();
            string selectedVoice = this.view.ProductVoice.Where(x => x.IsChecked).Select(x => x.Voice).FirstOrDefault();
            string selectedFabricTenant = this.view.FabricTenant.Where(x => x.IsChecked).Select(x => x.FabricTenant).FirstOrDefault();
            string selectedThread = this.view.ProductThread.Where(x => x.IsChecked).Select(x => x.Thread).FirstOrDefault();

            int tempCurrentProductStatusKey = 0;
            int tempCurrentProductVoiceKey = 0;
            int tempCurrentProductFabricTenantKey = 0;
            int tempCurrentProductThreadKey = 0;

            string currentProductName = this.view.ProductName;
            string currentProductDescription = this.view.ProductDecsription;

            targetProduct.Product_Name = currentProductName;
            targetProduct.Description = currentProductDescription;
            targetProduct.Localization_VSO_Path = this.view.VsoAreaPath;

            targetProduct.ProductFamilyKey = selectedFamilyKey;

            if (int.TryParse(GetKeyByName(C_Product_Status, selectedStatus), out tempCurrentProductStatusKey))
            {
                targetProduct.StatusKey = tempCurrentProductStatusKey;
            }
            else
            {
                targetProduct.StatusKey = null;
            }

            if (int.TryParse(GetKeyByName(C_Product_Voice, selectedVoice), out tempCurrentProductVoiceKey))
            {
                targetProduct.PVoiceKey = tempCurrentProductVoiceKey;
            }
            else
            {
                targetProduct.PVoiceKey = null;
            }

            if (int.TryParse(GetKeyByName(C_Fabric_Tenant, selectedFabricTenant), out tempCurrentProductFabricTenantKey))
            {
                targetProduct.FabricTenantKey = tempCurrentProductFabricTenantKey;
            }
            else
            {
                targetProduct.FabricTenantKey = null;
            }

            if (int.TryParse(GetKeyByName(C_Product_Thread, selectedThread), out tempCurrentProductThreadKey))
            {
                targetProduct.ThreadKey = tempCurrentProductThreadKey;
            }
            else
            {
                targetProduct.ThreadKey = null;
            }

            var currentProductPillars = targetProduct.ProductPillars.ToList().ToDictionary(c => c.PillarKey, c => c);

            List<string> selectedPillarsFromDB = targetProduct.ProductPillars.Select(p => p.Product_Pillar).ToList();
            List<string> currentSelectedPillars = this.view.ProductPillar.Where(s => s.IsChecked).Select(s => s.Pillar).ToList();

            //items should be added to target list
            List<string> addedItems = currentSelectedPillars.Except(selectedPillarsFromDB).ToList();
            //items should be removed from target list
            List<string> removedItems = selectedPillarsFromDB.Except(currentSelectedPillars).ToList();
            ProductPillar propi = null;
            foreach (string itemA in addedItems)
            {
                propi = portfolioContext.ProductPillars.First(x => x.Product_Pillar == itemA);
                portfolioContext.Products_New.First(p => p.ProductKey == targetProduct.ProductKey).ProductPillars.Add(propi);
            }

            foreach (string itemR in removedItems)
            {
                propi = portfolioContext.ProductPillars.First(x => x.Product_Pillar == itemR);
                portfolioContext.Products_New.First(p => p.ProductKey == targetProduct.ProductKey).ProductPillars.Remove(propi);
            }
        }

        protected string GetKeyByName(string tableName, string selectedName)
        {
            string key = "";
            if (selectedName != null)
            {
                using (SkypeIntlPlanningPortfolioEntities portfolioContext = new SkypeIntlPlanningPortfolioEntities())
                {
                    switch (tableName)
                    {
                        case C_Product_Family:
                            key = portfolioContext.ProductFamilies.Where(f => f.Product_Family == selectedName).Select(f => f.FamilyKey).FirstOrDefault().ToString();
                            break;

                        case C_Product_Status:
                            key = portfolioContext.Status.Where(s => s.Status1 == selectedName).Select(s => s.StatusKey).FirstOrDefault().ToString();
                            break;

                        case C_Product_Voice:
                            key = portfolioContext.ProductVoices.Where(v => v.Product_Voice == selectedName).Select(v => v.VoiceKey).FirstOrDefault().ToString();
                            break;

                        case C_Fabric_Tenant:
                            key = portfolioContext.FabricTenants.Where(t => t.Fabric_Tenant == selectedName).Select(t => t.TenantKey).FirstOrDefault().ToString();
                            break;

                        case C_Product_Thread:
                            key = portfolioContext.ProductThreads.Where(t => t.Product_Thread == selectedName).Select(t => t.ThreadKey).FirstOrDefault().ToString();
                            break;

                        case C_Product_Pillar:
                            key = portfolioContext.ProductPillars.Where(r => r.Product_Pillar == selectedName).Select(r => r.PillarKey).FirstOrDefault().ToString();
                            break;

                        default:
                            break;
                    }
                }
            }
            return key;
        }
    }
}
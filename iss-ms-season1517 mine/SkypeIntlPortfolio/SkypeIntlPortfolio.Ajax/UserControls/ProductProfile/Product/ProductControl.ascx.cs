using SkypeIntlPortfolio.Ajax.Mvp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.ComboBox;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Product
{
    public partial class ProductControl : System.Web.UI.UserControl, IProductView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.ProductStatus == null)
                this.ProductStatus = new List<PStatus>();

            if (this.ProductVoice == null)
                this.ProductVoice = new List<PVoice>();

            if (this.FabricTenant == null)
                this.FabricTenant = new List<PFabricTenant>();

            if (this.ProductThread == null)
                this.ProductThread = new List<PThread>();

            if (this.ProductPillar == null)
                this.ProductPillar = new List<PPillar>();

            if (this.Visible == true && this.LoadPPProduct != null)
            {
                if (!IsPostBack)
                {
                    this.LoadPPProduct();

                    foreach (var productStatus in this.ProductStatus)
                    {
                        this.radioButtonList_Status.Items.Add(new ListItem { Selected = productStatus.IsChecked, Text = productStatus.Status });
                    }

                    foreach (var productVoice in this.ProductVoice)
                    {
                        this.radioButtonList_Voice.Items.Add(new ListItem { Selected = productVoice.IsChecked, Text = productVoice.Voice });
                    }

                    foreach (var fabricTenant in this.FabricTenant)
                    {
                        this.radioButtonList_FabricTenant.Items.Add(new ListItem { Selected = fabricTenant.IsChecked, Text = fabricTenant.FabricTenant });
                    }

                    foreach (var productThread in this.ProductThread)
                    {
                        this.radioButtonList_Thread.Items.Add(new ListItem { Selected = productThread.IsChecked, Text = productThread.Thread });
                    }

                    foreach (var pillarItem in this.ProductPillar)
                    {
                        this.radListBox_Pillar.Items.Add(new RadListBoxItem { Checked = pillarItem.IsChecked, Text = pillarItem.Pillar });
                    }

                    if (!this.ProductFamiles.Any(x => x.IsSelected == true))
                    {
                        this.radCombobox_ProductFamily.DefaultItem.Text = "-Please select family-";
                        this.radCombobox_ProductFamily.DefaultItem.Value = "";
                    }
                }
            }
        }

        public string ProductName
        {
            get
            {
                return this.radTextBox_ProductName.Text;
            }
            set
            {
                this.radTextBox_ProductName.Text = value;
            }
        }

        public string VsoAreaPath
        {
            get
            {
                return this.radTextBox_localizationVsoPath.Text;
            }
            set
            {
                this.radTextBox_localizationVsoPath.Text = value;
            }
        }

        public string ProductDecsription
        {
            get
            {
                return this.radTextBox_Description.Text;
            }
            set
            {
                this.radTextBox_Description.Text = value;
            }
        }

        public IReadOnlyList<Mvp.SelectableItem> ProductFamiles
        {
            get
            {
                return ViewHelper.Instance.RadComboBox_GetSelectable(this.radCombobox_ProductFamily);
            }
            set
            {
                ViewHelper.Instance.RadComboBox_SetSelectable(this.radCombobox_ProductFamily, value);
            }
        }

        public List<PStatus> ProductStatus
        {
            get
            {
                return this.ViewState["ProductStatus"] as List<PStatus>;
            }
            set
            {
                this.ViewState["ProductStatus"] = value;
            }
        }

        public List<PVoice> ProductVoice
        {
            get
            {
                return this.ViewState["ProductVoice"] as List<PVoice>;
            }
            set
            {
                this.ViewState["ProductVoice"] = value;
            }
        }

        public List<PFabricTenant> FabricTenant
        {
            get
            {
                return this.ViewState["FabricTenant"] as List<PFabricTenant>;
            }
            set
            {
                this.ViewState["FabricTenant"] = value;
            }
        }

        public bool isVisible
        {
            get
            {
                return this.label_warning_productName.Visible;
            }
            set
            {
                this.label_warning_productName.Visible = value;
            }
        }

        public List<PThread> ProductThread
        {
            get
            {
                return this.ViewState["ProductThread"] as List<PThread>;
            }
            set
            {
                this.ViewState["ProductThread"] = value;
            }
        }

        public List<PPillar> ProductPillar
        {
            get
            {
                return this.ViewState["ProductPillar"] as List<PPillar>;
            }
            set
            {
                this.ViewState["ProductPillar"] = value;
            }
        }

        public event EventHandler OnClickNext;

        public event Action LoadPPProduct;

        protected void RadButton_tab_product_SaveAndNextPage_Click(object sender, EventArgs e)
        {
            if (this.OnClickNext != null)
                this.OnClickNext(sender, e);
        }

        protected void radioButtonList_FabricTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedFabricTenant = this.radioButtonList_FabricTenant.SelectedItem.ToString();
            foreach (var item in this.FabricTenant)
            {
                if (selectedFabricTenant == item.FabricTenant)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radioButtonList_Voice_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProductVoice = this.radioButtonList_Voice.SelectedItem.ToString();
            foreach (var item in this.ProductVoice)
            {
                if (selectedProductVoice == item.Voice)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radioButtonList_Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProductStatus = this.radioButtonList_Status.SelectedItem.ToString();
            foreach (var item in this.ProductStatus)
            {
                if (selectedProductStatus == item.Status)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radCombobox_ProductFamily_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //var selectedProductFamily = this.radCombobox_ProductFamily.SelectedItem.Text;
            //foreach (var item in this.ProductFamily)
            //{
            //    if (selectedProductFamily == item.Family)
            //    {
            //        item.IsChecked = true;
            //    }
            //    else
            //    {
            //        item.IsChecked = false;
            //    }
            //}
        }

        protected void radioButtonList_Thread_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProductThread = this.radioButtonList_Thread.SelectedItem.ToString();
            foreach (var item in this.ProductThread)
            {
                if (selectedProductThread == item.Thread)
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }

        protected void radListBox_Pillar_ItemCheck(object sender, RadListBoxItemEventArgs e)
        {
            Dictionary<string, bool> productPillarList = this.radListBox_Pillar.CheckedItems.ToDictionary(x => x.Text, x => x.Checked);

            foreach (var item in this.ProductPillar)
            {
                item.IsChecked = true;
                if (productPillarList.ContainsKey(item.Pillar))
                {
                    item.IsChecked = true;
                }
                else
                {
                    item.IsChecked = false;
                }
            }
        }
    }
}
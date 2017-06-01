using SkypeIntlPortfolio.Ajax.Pages;
using SkypeIntlPortfolio.Ajax.UserControls.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Vacation
{
    public partial class VacationDaysEntryControl : System.Web.UI.UserControl, IVacationView
    {
        public event Action<VacationInfo, List<int>, List<int>> InsertVacationInfo;

        public event Action<VacationInfo, List<int>, List<int>> UpdateVacationInfo;

        public event Action<int> DeleteVacationInfo;

        public event Func<List<int>> GetAllProducts;

        public event Func<int, List<int>> GetAffectedLocationsIDsByVacationID;

        public event Func<int, Dictionary<string, string>> GetVacationAffectedProductsByVacationID;

        public event Func<List<string>, List<AffectedInfo>> GetAffectedPeopleByProductIDs;

        public event Func<Dictionary<int, string>, List<string>, List<AffectedInfo>> GetAffectedProductsByPeopleNames;

        public event Func<HashSet<string>, List<string>, List<string>> GetUpdatedAffectedPeopleByProductIDs;

        private const string C_EntryAddProduct = "AddProduct";
        private const string C_EntryAddPeople = "AddPeople";
        private const string C_EntryRemoveProduct = "RemovepRroduct";

        public List<int> AllProductsIds
        {
            get { return ViewState["AllProductsIds"] as List<int>; }
            set { ViewState["AllProductsIds"] = value; }
        }

        protected void RadGridVacation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (this.Visible == true)
            {
                if (this.GetAllProducts != null)
                    AllProductsIds = this.GetAllProducts();
                VacationInfoService vifService = new VacationInfoService();
                this.RadGridVacation.DataSource = vifService.GetVacationRelatedInfoList();
            }
        }

        protected void RadGridVacation_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.Item != null && e.CommandName.Equals("Edit"))
            {
                e.Item.OwnerTableView.EditFormSettings.CaptionFormatString = "Update Vacation";
            }
            else if (e.Item != null && e.CommandName.Equals("PerformInsert"))
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;

                RadTextBox rtx_vcationName = (RadTextBox)editableItem.FindControl("radtextbox_VacationName");
                RadTextBox rtx_vcationDes = (RadTextBox)editableItem.FindControl("radtextbox_VacationDescription");
                RadDatePicker dtp_vcationStartDate = (RadDatePicker)editableItem.FindControl("raddatepicker_VacationStartDate");
                RadDatePicker dtp_vcationEndDate = (RadDatePicker)editableItem.FindControl("raddatepicker_VacationEndDate");
                RadioButtonList radioButtonList_UIVacationCategory = (RadioButtonList)editableItem.FindControl("radioButtonList_UIVacationCategory");
                RadAutoCompleteBox racb_existingVcations = (RadAutoCompleteBox)editableItem.FindControl("RadAutoCompleteBox_existingProducts");
                RadListBox radListBoxLocation = (RadListBox)editableItem.FindControl("radListBox_Location");

                List<int> vacationAffectedProductIds = new List<int>();
                var vacationAffectedLocationIds = new List<int>();
                VacationInfo vrInfo = new VacationInfo
                {
                    VacationName = rtx_vcationName.Text,
                    VacationDescription = rtx_vcationDes.Text,
                    VacationStartDate = dtp_vcationStartDate.SelectedDate.Value,
                    VacationEndDate = dtp_vcationEndDate.SelectedDate.Value,
                };
                int selectedIndex = radioButtonList_UIVacationCategory.SelectedIndex;
                int uiCateId = 0;
                switch (selectedIndex)
                {
                    case 0:
                        //OBO
                        uiCateId = 1;
                        foreach (AutoCompleteBoxEntry racb in racb_existingVcations.Entries)
                        {
                            vacationAffectedProductIds.Add(int.Parse(racb.Value));
                        }

                        break;

                    case 1:
                        //BYLOC
                        uiCateId = 2;
                        var reportingPresenter = new ReportingSystemPresenter();
                        foreach (RadListBoxItem item in radListBoxLocation.CheckedItems)
                        {
                            vacationAffectedLocationIds.Add(int.Parse(item.Value));
                        }
                        vacationAffectedProductIds = reportingPresenter._iBridge_GetProductsWithCheckedLocations(radListBoxLocation.CheckedItems.Select(c => c.Text)).Keys.ToList();
                        break;

                    case 2:
                        //ALL
                        uiCateId = 3;

                        vacationAffectedProductIds = AllProductsIds;

                        break;
                }
                vrInfo.UICategoryID = uiCateId;
                this.InsertVacationInfo(vrInfo, vacationAffectedProductIds, vacationAffectedLocationIds);
            }
            else if (e.Item != null && e.CommandName.Equals("Update"))
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                RadTextBox rtx_vcationName = (RadTextBox)editableItem.FindControl("radtextbox_VacationName");
                RadTextBox rtx_vcationDes = (RadTextBox)editableItem.FindControl("radtextbox_VacationDescription");
                RadDatePicker dtp_vcationStartDate = (RadDatePicker)editableItem.FindControl("raddatepicker_VacationStartDate");
                RadDatePicker dtp_vcationEndDate = (RadDatePicker)editableItem.FindControl("raddatepicker_VacationEndDate");

                Label label_affectedProductsValidator = (Label)editableItem.FindControl("Label_affectedProductsValidator");
                List<int> vacationAffectedProductIds = new List<int>();
                List<int> vacationAffectedLocationIds = new List<int>();
                int currentVacationID = Convert.ToInt32(editableItem["VacationID"].Text);
                var radioList = (RadioButtonList)editableItem.FindControl("radioButtonList_UIVacationCategory");
                int uiCatId = int.Parse(radioList.SelectedValue);

                VacationInfo vrInfo = new VacationInfo
                {
                    VacationID = currentVacationID,
                    VacationName = rtx_vcationName.Text,
                    VacationDescription = rtx_vcationDes.Text,
                    VacationStartDate = dtp_vcationStartDate.SelectedDate.Value,
                    VacationEndDate = dtp_vcationEndDate.SelectedDate.Value,
                    UICategoryID = uiCatId
                };
                if (uiCatId == 3)
                    vacationAffectedProductIds = this.AllProductsIds;
                else if (uiCatId == 2)
                {
                    var locationNames = new List<string>();
                    RadListBox radListBoxLocation = (RadListBox)editableItem.FindControl("radListBox_Location");
                    foreach (RadListBoxItem item in radListBoxLocation.CheckedItems)
                    {
                        vacationAffectedLocationIds.Add(int.Parse(item.Value));
                        locationNames.Add(item.Text);
                    }
                    vacationAffectedProductIds = new ReportingSystemPresenter()._iBridge_GetProductsWithCheckedLocations(locationNames).Keys.ToList();
                }
                else if (uiCatId == 1)
                {
                    RadAutoCompleteBox racb_existingVcations = (RadAutoCompleteBox)editableItem.FindControl("RadAutoCompleteBox_existingProducts");
                    foreach (AutoCompleteBoxEntry racb in racb_existingVcations.Entries)
                    {
                        vacationAffectedProductIds.Add(Convert.ToInt32(racb.Value));
                    }
                }
                this.UpdateVacationInfo(vrInfo, vacationAffectedProductIds, vacationAffectedLocationIds);
            }
            else if (e.Item != null && e.CommandName.Equals("Delete"))
            {
                //DeleteVacationInfo
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                int vacationID = Convert.ToInt32(editableItem["VacationID"].Text);
                this.DeleteVacationInfo(vacationID);
            }
        }

        protected void RadGridVacation_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                //the two lines of code are used to center the rad grid popup window
                GridEditFormItem editForm = (GridEditFormItem)e.Item;
                editForm.EditFormCell.CssClass = "centerPopUpModal";

                var item = (GridEditableItem)e.Item;
                RadTextBox rtb_name = (RadTextBox)item.FindControl("radtextbox_VacationName");
                RadTextBox rtb_description = (RadTextBox)item.FindControl("radtextbox_VacationDescription");
                RadDatePicker datepickerStart = (RadDatePicker)item.FindControl("raddatepicker_VacationStartDate");
                RadDatePicker datepickerEnd = (RadDatePicker)item.FindControl("raddatepicker_VacationEndDate");
                RadioButtonList radioButtonList_UIVacationCategory = (RadioButtonList)item.FindControl("radioButtonList_UIVacationCategory");

                if (item.RowIndex != -1)
                {
                    // edit
                    rtb_name.Text = item["VacationName"].Text;
                    rtb_description.Text = item["VacationDescription"].Text;
                    datepickerStart.SelectedDate = DateTime.Parse(item["VacationStartDate"].Text);
                    datepickerEnd.SelectedDate = DateTime.Parse(item["VacationEndDate"].Text);
                    int currentVacationID = Convert.ToInt32(item["VacationID"].Text);
                    Label label_affectedPeople = (Label)item.FindControl("label_affectedPeople") as Label;
                    RadAutoCompleteBox racb_existingPeople = (RadAutoCompleteBox)item.FindControl("RadAutoCompleteBox_existingPeopleNames");
                    //retrieve the affected products by passing vacationID
                    if (GetVacationAffectedProductsByVacationID != null)
                    {
                        int uiCatId = new VacationInfoService().GetVacationInfoesByID(currentVacationID).UICategoryID;
                        switch (uiCatId)
                        {
                            //OBO
                            case 1:
                                label_affectedPeople.Visible = true;
                                racb_existingPeople.Visible = true;
                                Panel oboPanel = (Panel)item.FindControl("panel_checkProductsOneByOne");
                                oboPanel.Visible = true;
                                RadAutoCompleteBox racb_existingProducts = (RadAutoCompleteBox)item.FindControl("RadAutoCompleteBox_existingProducts");

                                var retrievedProducts = this.GetVacationAffectedProductsByVacationID(currentVacationID);
                                foreach (var productInfo in retrievedProducts)
                                {
                                    racb_existingProducts.Entries.Add(new AutoCompleteBoxEntry(productInfo.Value, productInfo.Key.ToString()));
                                }
                                radioButtonList_UIVacationCategory.SelectedIndex = 0;

                                var retrievedPeopleInfo = this.GetAffectedPeopleByProductIDs(retrievedProducts.Keys.ToList());
                                foreach (var productInfo in retrievedPeopleInfo)
                                {
                                    racb_existingPeople.Entries.Add(new AutoCompleteBoxEntry(productInfo.PeopleName, productInfo.PeopleName));
                                }

                                break;
                            //ByLoc
                            case 2:
                                label_affectedPeople.Visible = false;
                                racb_existingPeople.Visible = false;
                                Panel locPanel = (Panel)item.FindControl("panel_checkProductsByLoc");
                                locPanel.Visible = true;
                                RadListBox radListBoxLocation = (RadListBox)item.FindControl("radListBox_Location");

                                if (this.GetAffectedLocationsIDsByVacationID != null)
                                {
                                    var locationIds = this.GetAffectedLocationsIDsByVacationID(currentVacationID);
                                    foreach (RadListBoxItem radListItem in radListBoxLocation.Items)
                                    {
                                        if (locationIds.Contains(int.Parse(radListItem.Value)))
                                            radListItem.Checked = true;
                                    }
                                }
                                radioButtonList_UIVacationCategory.SelectedIndex = 1;
                                break;
                            //All
                            case 3:
                                label_affectedPeople.Visible = false;
                                racb_existingPeople.Visible = false;
                                radioButtonList_UIVacationCategory.SelectedIndex = 2;
                                break;
                        }
                    }
                }
                else
                {
                    //insert
                    Panel oboPanel = (Panel)item.FindControl("panel_checkProductsOneByOne");
                    oboPanel.Visible = true;

                    radioButtonList_UIVacationCategory.SelectedIndex = 0;
                    RadListBox radListBoxLocation = (RadListBox)item.FindControl("radListBox_Location");
                    radListBoxLocation.Items[0].Checked = true;
                }
            }
        }

        #region Autocompletebox edting

        protected void RadAutoCompleteBox_existingProducts_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            RadAutoCompleteBox currentRadAutoCompleteBox = sender as RadAutoCompleteBox;
            updateAutoCompleteboxEntries(C_EntryAddProduct, currentRadAutoCompleteBox, e);
        }

        protected void RadAutoCompleteBox_existingProducts_EntryRemoved(object sender, AutoCompleteEntryEventArgs e)
        {
            RadAutoCompleteBox currentRadAutoCompleteBox = sender as RadAutoCompleteBox;
            updateAutoCompleteboxEntries(C_EntryRemoveProduct, currentRadAutoCompleteBox, e);
        }

        protected void RadAutoCompleteBox_existingPeopleNames_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            RadAutoCompleteBox currentRadAutoCompleteBox = sender as RadAutoCompleteBox;
            currentRadAutoCompleteBox.Entries.Clear();
            currentRadAutoCompleteBox.Entries.Add(new AutoCompleteBoxEntry(e.Entry.Text, e.Entry.Value));
            updateAutoCompleteboxEntries(C_EntryAddPeople, currentRadAutoCompleteBox, e);
        }

        protected void updateAutoCompleteboxEntries(string entryState, RadAutoCompleteBox currentRadAutoCompleteBox, AutoCompleteEntryEventArgs e)
        {
            //1-update one of the autocomplteboxes,filter out the duplicate items
            List<string> filteredList = new List<string>();
            for (int i = 0; i < currentRadAutoCompleteBox.Entries.Count; i++)
            {
                //itemKey could be people name or product key
                string itemKey = currentRadAutoCompleteBox.Entries[i].Value;
                string newUpdatedOne = e.Entry.Value;
                //avoid insert duplicate item
                if (filteredList.Any(x => x == itemKey))
                    currentRadAutoCompleteBox.Entries.Remove(e.Entry);
                filteredList.Add(itemKey);
            }
            //2-the update from one autocomplteboxes'll impact on another one
            RadAutoCompleteBox affectedRadAutoCompleteBox;
            if (entryState == C_EntryAddProduct || entryState == C_EntryRemoveProduct)
            {
                affectedRadAutoCompleteBox = (RadAutoCompleteBox)currentRadAutoCompleteBox.Parent.Parent.FindControl("RadAutoCompleteBox_existingPeopleNames");
                this.RadAutoCompleteBox_population(entryState, affectedRadAutoCompleteBox, filteredList);
            }
            else
            {
                affectedRadAutoCompleteBox = (RadAutoCompleteBox)currentRadAutoCompleteBox.Parent.Parent.FindControl("RadAutoCompleteBox_existingProducts");
                //this case two boxes should be updated
                this.RadAutoCompleteBox_population(entryState, affectedRadAutoCompleteBox, filteredList, currentRadAutoCompleteBox);
            }
        }

        protected void RadAutoCompleteBox_population(string entryState, RadAutoCompleteBox affectedRadAutoCompleteBox, List<string> filteredList, RadAutoCompleteBox sourceRadAutoCompleteBox = null)
        {
            List<AffectedInfo> affectedList = null;
            if (entryState == C_EntryAddProduct || entryState == C_EntryRemoveProduct)
            {
                affectedRadAutoCompleteBox.Entries.Clear();
                affectedList = this.GetAffectedPeopleByProductIDs(filteredList);
                foreach (var affectedInfo in affectedList)
                {
                    affectedRadAutoCompleteBox.Entries.Add(new AutoCompleteBoxEntry(affectedInfo.PeopleName, affectedInfo.PeopleName));
                }
            }
            else
            {
                //1.1.get the existing products
                Dictionary<int, string> existingProducts = new Dictionary<int, string>();
                foreach (AutoCompleteBoxEntry existingPro in affectedRadAutoCompleteBox.Entries)
                {
                    existingProducts.Add(Convert.ToInt32(existingPro.Value), existingPro.Text);
                }
                //1.2.get the total effected products base on selected people
                affectedList = this.GetAffectedProductsByPeopleNames(existingProducts, filteredList);
                //1.3.clean up the product slection box
                affectedRadAutoCompleteBox.Entries.Clear();
                //1.4.insert all the effected products into the box
                foreach (var affectedInfo in affectedList)
                {
                    affectedRadAutoCompleteBox.Entries.Add(new AutoCompleteBoxEntry(affectedInfo.ProductName, affectedInfo.ProductKey.ToString()));
                }
                //2.1.get the existing people
                HashSet<string> existingPeople = new HashSet<string>();
                foreach (AutoCompleteBoxEntry existingPeo in sourceRadAutoCompleteBox.Entries)
                {
                    existingPeople.Add(existingPeo.Text);
                }
                //2.2.get the involved people base on selected people
                List<string> updatedPeopleList = this.GetUpdatedAffectedPeopleByProductIDs(existingPeople, affectedList.Select(x => x.ProductKey.ToString()).ToList());
                //2.3.clean up the people section box
                sourceRadAutoCompleteBox.Entries.Clear();
                //2.4.insert all the involved people into the box
                foreach (string peopleAffected in updatedPeopleList)
                {
                    sourceRadAutoCompleteBox.Entries.Add(new AutoCompleteBoxEntry(peopleAffected, peopleAffected));
                }
            }
        }

        #endregion Autocompletebox edting

        protected void radioButtonList_UIVacationCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList radioButtonList_UIVacationCategory = sender as RadioButtonList;
            Panel panel_byLocation = radioButtonList_UIVacationCategory.Parent.FindControl("panel_checkProductsByLoc") as Panel;
            Panel panel_onebyone = radioButtonList_UIVacationCategory.Parent.FindControl("panel_checkProductsOneByOne") as Panel;
            Label label_affectedPeople = radioButtonList_UIVacationCategory.Parent.FindControl("label_affectedPeople") as Label;
            RadAutoCompleteBox radAutoCompleteBox_existingPeopleNames = radioButtonList_UIVacationCategory.Parent.FindControl("RadAutoCompleteBox_existingPeopleNames") as RadAutoCompleteBox;
            int selectedIndex = radioButtonList_UIVacationCategory.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    //OBO
                    panel_onebyone.Visible = true;
                    panel_byLocation.Visible = false;
                    label_affectedPeople.Visible = true;
                    radAutoCompleteBox_existingPeopleNames.Visible = true;
                    break;

                case 1:
                    //By Location
                    panel_byLocation.Visible = true;
                    panel_onebyone.Visible = false;
                    label_affectedPeople.Visible = false;
                    radAutoCompleteBox_existingPeopleNames.Visible = false;
                    break;

                case 2:
                    //All
                    panel_byLocation.Visible = false;
                    panel_onebyone.Visible = false;
                    label_affectedPeople.Visible = false;
                    radAutoCompleteBox_existingPeopleNames.Visible = false;
                    break;
            }
        }
    }
}
using SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ProductProfile.Links
{
    public partial class LinksControl : System.Web.UI.UserControl, ILinksView
    {
        public event Action LoadLinkData;

        public event EventHandler OnClickNext;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (!this.IsPostBack && this.LoadLinkData != null)
                {
                    this.LoadLinkData();
                }
            }
        }

        protected void RadButton_tab_Links_SveAndNextPage_Click(object sender, EventArgs e)
        {
            if (OnClickNext != null)
                OnClickNext(sender, e);
        }

        public string VSOLinkLocalization
        {
            get
            {
                return this.radTextBox_VSOlink_Localization.Text;
            }
            set
            {
                this.radTextBox_VSOlink_Localization.Text = value;
            }
        }

        public string VSOlinkCore
        {
            get
            {
                return this.radTextBox_VSOlink_Core.Text;
            }
            set
            {
                this.radTextBox_VSOlink_Core.Text = value;
            }
        }

        public string BuildLocation
        {
            get
            {
                return this.radTextBox_BuildLocation.Text;
            }
            set
            {
                this.radTextBox_BuildLocation.Text = value;
            }
        }

        public string LCGLocation
        {
            get
            {
                return this.ViewState["LCGLocation"] as string;
            }
            set
            {
                this.ViewState["LCGLocation"] = value;
            }
        }

        public string LCTLocation
        {
            get
            {
                return this.ViewState["LCTlocation"] as string;
            }
            set
            {
                this.ViewState["LCTlocation"] = value;
            }
        }

        public string LCLLocation
        {
            get
            {
                return this.ViewState["LCLLocation"] as string;
            }
            set
            {
                this.ViewState["LCLLocation"] = value;
            }
        }

        /// <summary>
        /// will be called by LCG/LCT/LCL file path fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid_Location_DataSource(object sender, GridNeedDataSourceEventArgs e, string fileLoationType)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Location");
            string[] lclLocations = new string[] { };
            if (fileLoationType != null)
                lclLocations = fileLoationType.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string location in lclLocations)
            {
                if (location.ToLower() != "none")
                {
                    DataRow newLocationRow = dt.NewRow();
                    newLocationRow["Location"] = location;
                    dt.Rows.Add(newLocationRow);
                }
            }
            RadGrid currentRadGrid = sender as RadGrid;
            currentRadGrid.DataSource = dt;
        }

        /// <summary>
        /// will be called by LCG/LCT/LCL file path fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadGrid_Location_ItemCommand(object sender, GridCommandEventArgs e, string existingLocation, Action<string> update)
        {  
            if (e.Item != null && e.CommandName.Equals("PerformInsert"))
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                TextBox locationtxtBox = (TextBox)editableItem["Location"].Controls[0];
                update(String.Format("{0};{1}",existingLocation, locationtxtBox.Text));
            }

            else if (e.Item != null && e.CommandName.Equals("Update"))
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                TextBox locationtxtBox = (TextBox)editableItem["Location"].Controls[0];
                //modify the element by using index of the location array
                int editItemIndex = editableItem.ItemIndex;
                //replaced the element from exsting location string(array)
                string[] locations = existingLocation.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                locations[editItemIndex] = locationtxtBox.Text;
                //convert string array into string and later the propery will be updated, cos the action jsut take a calculated input and return a proper result/property.
                update(string.Join(";", locations));
            }

            else if (e.Item != null && e.CommandName.Equals("Delete"))
            {
                GridEditableItem editableItem = (GridEditableItem)e.Item;
                //update this location property add element with ';' ahead of it
                int editItemIndex = editableItem.ItemIndex;
                string[] locations = existingLocation.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                locations[editItemIndex] = "";
                //convert string array into string and later the propery will be updated, cos the action jsut take a calculated input and return a proper result/property.
                update(string.Join(";", locations));
            }
        }

        protected void RadGrid_LCL_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGrid_Location_DataSource(sender,e,  LCLLocation);
        }

        protected void RadGrid_LCL_Location_ItemCommand(object sender, GridCommandEventArgs e)
        {
            //wrong version
            //this.RadGrid_Location_ItemCommand(sender,e, this.LCLLocation,  x => this.LCLLocation = x);
            //send all rows into location property as input parameter
            this.RadGrid_Location_ItemCommand(sender, e, this.LCLLocation, x => this.LCLLocation = x);
        }

        protected void RadGrid_LCT_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGrid_Location_DataSource(sender, e,  LCTLocation);
        }

        protected void RadGrid_LCT_Location_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.RadGrid_Location_ItemCommand(sender, e, this.LCTLocation, x => this.LCTLocation = x);
        }

        protected void RadGrid_LCG_Location_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGrid_Location_DataSource(sender, e,  LCGLocation);
        }

        protected void RadGrid_LCG_Location_ItemCommand(object sender, GridCommandEventArgs e)
        {
            this.RadGrid_Location_ItemCommand(sender, e,  this.LCGLocation, x => this.LCGLocation = x);
        }
    }
}
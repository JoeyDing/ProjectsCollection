using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.RemoteLogger
{
    public partial class RemoteLoggerControl : System.Web.UI.UserControl, IRemoteLoggerView
    {
        public List<RemoteLoggerStateModel> StateList
        {
            get
            {
                return ViewState["BatchTimeList"] as List<RemoteLoggerStateModel>;
            }
            set { ViewState["BatchTimeList"] = value; }
        }

        public List<DropDownModel> BatchTimeList
        {
            get
            {
                return ViewState["BatchTimeList"] as List<DropDownModel>;
            }
            set { ViewState["BatchTimeList"] = value; }
        }

        public List<DropDownModel> UserList
        {
            get
            {
                return ViewState["UserList"] as List<DropDownModel>;
            }
            set { ViewState["UserList"] = value; }
        }

        public List<string> ApplicationList
        {
            get
            {
                return ViewState["ApplicationList"] as List<string>;
            }
            set { ViewState["ApplicationList"] = value; }
        }

        public List<RemoteLoggerStateOverviewModel> StateOverviewList
        {
            get
            {
                return ViewState["StateOverviewList"] as List<RemoteLoggerStateOverviewModel>;
            }
            set { ViewState["StateOverviewList"] = value; }
        }

        public List<RemoteLoggerStateModel> StateOverviewDetailList
        {
            get
            {
                return ViewState["StateOverviewDetailList"] as List<RemoteLoggerStateModel>;
            }
            set { ViewState["StateOverviewDetailList"] = value; }
        }

        public event Func<string, List<DropDownModel>> GetUserList;

        public event Func<string, string, List<DropDownModel>> GetBatchTimeList;

        public event Func<string, List<RemoteLoggerStateModel>> GetStateList;

        public event Func<List<string>> GetApplicationList;

        public event Func<string, List<RemoteLoggerStateOverviewModel>> GetStateOverviewList;

        public event Func<string, string, List<RemoteLoggerStateModel>> GetStateOverviewDetailList;

        public event Func<int, List<RemoteStateLoggerImage>> GetImageList;

        public static string ConvertBooleanToImage(string state)
        {
            bool bState = Boolean.Parse(state);
            if (bState)
                return "~/Content/image/complete.png";
            else
                return "~/Content/image/error.png";
        }

        public static string ConvertRelativePath(string imagePath)
        {
            string relativePath = string.Empty;
            if (!string.IsNullOrEmpty(imagePath))
            {
                var fileUri = new Uri(imagePath);
                var referenceUri = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\\Pages\\Monitor\\RemoteLogger\\RemoteStateLoggerDashboard");
                relativePath = referenceUri.MakeRelativeUri(fileUri).ToString();
            }
            return relativePath;
        }

        public static string FormatExceptionLink(object exceptionID)
        {
            string id = exceptionID == null ? "" : exceptionID.ToString();
            return String.Format("~/Pages/Monitor/RemoteLogger/RemoteLoggerDetail.aspx?Id={0}", id);
        }

        public static bool ConvertBooleanToVisible(object exceptionID)
        {
            return exceptionID == null ? false : true;
        }

        public static string GetImageName(object imagePath)
        {
            List<string> array = imagePath.ToString().Split('/').ToList();
            return array[array.Count - 1];
        }

        private string GetImagesListHtml(object id)
        {
            StringBuilder html = new StringBuilder();
            List<RemoteStateLoggerImage> imageList = this.GetImageList(Int32.Parse(id.ToString()));
            if (imageList.Count > 0)
            {
                //List<string> imagePathList = imagePath.ToString().Split(',').ToList();
                foreach (var item in imageList)
                {
                    string imageUrl = ResolveUrl("~/Pages/Monitor/RemoteLogger/DisplayImage.ashx") + "?pid=" + item.Id;
                    string imageName = GetImageName(item.ImagePath);
                    if (!imageName.Equals(".png"))
                    {
                        html.Append("<table>");
                        html.Append("<tr>");
                        html.Append("<td>");
                        html.Append("<a target=\"_blank\"  runat=\"server\" href=\"" + imageUrl + "\" style=\"color:#0000FF;text-decoration: underline;..................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................\" >");
                        html.Append(imageName);
                        html.Append("</a>");
                        html.Append("</td>");
                        html.Append("</tr>");
                        html.Append("</table>");
                        string result = html.ToString();
                    }
                }
            }
            return html.ToString();
        }

        private void BindingUserAccountDropdown(string appName)
        {
            this.UserList = this.GetUserList(appName);
            this.rddUserAccount.DataSource = this.UserList;
            this.rddUserAccount.DataBind();
        }

        private void BindingBatchDropDown(string appName, string userAcc)
        {
            this.BatchTimeList = this.GetBatchTimeList(appName, userAcc);

            this.rddBatchTimeRange.DataSource = this.BatchTimeList;
            this.rddBatchTimeRange.DataBind();
        }

        private void BindTestcaseGrid(string batchID)
        {
            this.StateList = this.GetStateList(batchID);
            this.RadGridTestCases.DataSource = this.StateList;
            this.RadGridTestCases.DataBind();
        }

        private void BindApplication()
        {
            List<string> applicationNames = this.GetApplicationList();
            this.RemoteLoggerSideBar.DataSource = applicationNames;
            this.RemoteLoggerSideBar.DataBind();
        }

        private void BindOverviewGrid(string batchID)
        {
            this.StateOverviewList = this.GetStateOverviewList(batchID);
            this.RadGridTestcaseOverview.DataSource = this.StateOverviewList;
            this.RadGridTestcaseOverview.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string batchID = Request["batchID"];

            if (!IsPostBack)
            {
                //Binding application Names
                BindApplication();

                if (string.IsNullOrEmpty(batchID))
                {
                    this.ApplicationList = this.GetApplicationList();
                    string currentAppName = this.ApplicationList.FirstOrDefault();
                    if (!String.IsNullOrEmpty(currentAppName))
                        this.RemoteLoggerSideBar.SelectedTab.Text = currentAppName;

                    BindingUserAccountDropdown(currentAppName);
                    BindingBatchDropDown(currentAppName, this.rddUserAccount.SelectedValue);
                    BindTestcaseGrid(this.rddBatchTimeRange.SelectedValue);
                    BindOverviewGrid(this.rddBatchTimeRange.SelectedValue);
                    this.lbTitle.Text = currentAppName + " Status Log";
                }
                else
                {
                    var statesRecords = this.StateList.Where(r => r.BatchID == batchID).FirstOrDefault();
                    var currentAppName = statesRecords.ApplicationName;
                    this.RemoteLoggerSideBar.SelectedTab.Text = currentAppName;

                    BindingUserAccountDropdown(currentAppName);
                    this.rddUserAccount.SelectedValue = statesRecords.UserIdentity;

                    BindingBatchDropDown(currentAppName, statesRecords.UserIdentity);
                    this.rddBatchTimeRange.SelectedValue = statesRecords.BatchID;

                    BindTestcaseGrid(statesRecords.BatchID);
                    BindOverviewGrid(statesRecords.BatchID);
                }
            }
        }

        protected void rddApplication_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            string currentAppName = this.RemoteLoggerSideBar.SelectedTab.Text;
            BindingUserAccountDropdown(currentAppName);
            BindingBatchDropDown(currentAppName, this.rddUserAccount.SelectedText);
            BindTestcaseGrid(this.rddBatchTimeRange.SelectedValue);
            BindOverviewGrid(this.rddBatchTimeRange.SelectedValue);
            this.lbTitle.Text = currentAppName + " Status Log";
        }

        protected void rddUserAccount_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            string currentAppName = this.RemoteLoggerSideBar.SelectedTab.Text;
            BindingBatchDropDown(currentAppName, this.rddUserAccount.SelectedText);
            BindTestcaseGrid(this.rddBatchTimeRange.SelectedValue);
            BindOverviewGrid(this.rddBatchTimeRange.SelectedValue);
            this.lbTitle.Text = currentAppName + " Status Log";
        }

        protected void rddBatchTimeRange_SelectedIndexChanged(object sender, Telerik.Web.UI.DropDownListEventArgs e)
        {
            string currentAppName = this.RemoteLoggerSideBar.SelectedTab.Text;
            BindTestcaseGrid(this.rddBatchTimeRange.SelectedValue);
            BindOverviewGrid(this.rddBatchTimeRange.SelectedValue);
            this.lbTitle.Text = currentAppName + " Status Log";
        }

        protected void RadGridTestCases_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                string strID = item.GetDataKeyValue("Id").ToString();
                int testcaseId = int.Parse(strID);
                item["ScreenShot"].Text = this.GetImagesListHtml(testcaseId);
            }
        }

        protected void RadGridTestcaseOverview_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "TestcaseOverviewDetail":
                    {
                        string batchID = this.rddBatchTimeRange.SelectedValue;
                        string testcaseName = dataItem.GetDataKeyValue("Testcase").ToString();
                        e.DetailTableView.DataSource = this.GetStateOverviewDetailList(batchID, testcaseName);
                        break;
                    }
            }
        }

        protected void RadGridTestcaseOverview_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && e.Item.OwnerTableView.Name == "TestcaseOverviewDetail")
            {
                GridDataItem item = e.Item as GridDataItem;
                string strID = item.GetDataKeyValue("Id").ToString();
                int testcaseId = int.Parse(strID);
                item["ScreenShot"].Text = this.GetImagesListHtml(testcaseId);
            }
        }

        protected void RadTabStripJobStatusNew_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            string currentTabName = RemoteLoggerSideBar.SelectedTab.Text;
            BindingUserAccountDropdown(currentTabName);
            BindingBatchDropDown(currentTabName, this.rddUserAccount.SelectedText);
            BindTestcaseGrid(this.rddBatchTimeRange.SelectedValue);
            BindOverviewGrid(this.rddBatchTimeRange.SelectedValue);
            this.lbTitle.Text = currentTabName + " Status Log";
        }
    }
}
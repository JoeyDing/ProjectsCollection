using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public partial class SpokenInputOutputControl : System.Web.UI.UserControl, ISpokenInputOutputView
    {
        protected void RadGridSpokenInputOutput_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                foreach (GridItem item in RadGridSpokenInputOutput.MasterTableView.Items)
                {
                    item.Expanded = true;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.FeatureOfProduct_Result == null)
                this.FeatureOfProduct_Result = new List<GetFeatureOfProduct_Result>();
            if (this.DictSpokenInputOutputs_Result == null)
                this.DictSpokenInputOutputs_Result = new Dictionary<int, List<GetSpokenInputOutputOfProduct_Result>>();

            if (this.Visible && !IsPostBack && this.GetFeatures != null && this.GetTotalRecord != null && this.GetLanguages() != null)
            {
                this.TotalRecord = this.GetTotalRecord();
                int currentPageIndex = this.RadGridSpokenInputOutput.CurrentPageIndex;
                int pageSize = this.RadGridSpokenInputOutput.MasterTableView.PageSize;
                this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                this.BasicLanguage_List = this.GetLanguages();
            }
            else
                this.label_errorMsg.Visible = false;
        }

        protected void RadGridSpokenInputOutput_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            int featureKey = int.Parse(dataItem.GetDataKeyValue("FeatureKey").ToString());
            //When expand detail table, make featureEidt invisible
            dataItem["FeatureEdit"].Visible = false;
            dataItem["TBFeature"].Enabled = false;

            if (this.DictSpokenInputOutputs_Result.ContainsKey(featureKey))
                e.DetailTableView.DataSource = this.DictSpokenInputOutputs_Result[featureKey];
            else if (this.GetSpokenInputOutput != null)
            {
                var resultList = this.GetSpokenInputOutput(featureKey);
                e.DetailTableView.DataSource = resultList;
                this.DictSpokenInputOutputs_Result.Add(featureKey, resultList);
            }
        }

        protected void RadGridSpokenInputOutput_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGridSpokenInputOutput.VirtualItemCount = this.TotalRecord;

            // var features = this.FeatureOfProduct_Result.Select(u => new { FeatureName = u.FeatureName, FeatureKey = u.FeatureKey }).Distinct().ToList();
            this.RadGridSpokenInputOutput.DataSource = this.FeatureOfProduct_Result;
        }

        protected void RadGridSpokenInputOutput_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            var hash = new Dictionary<object, object>();
            editableItem.ExtractValues(hash);

            string name = e.Item.OwnerTableView.Name;
            if (name == "InputOutputs")
            {
                int featureKey = int.Parse(editableItem.OwnerTableView.ParentItem.GetDataKeyValue("FeatureKey").ToString());
                int spokenInputOutputKey = int.Parse(editableItem.GetDataKeyValue("SpokenInputOutputKey").ToString());

                var sio = new SpokenInputOutput();
                sio.SpokenInputOutputKey = spokenInputOutputKey;
                DropDownList ddlLanguage = (DropDownList)editableItem.FindControl("DDLLanguage");
                int language = int.Parse(ddlLanguage.SelectedValue);
                sio.LanguageKey = language;
                sio.Spoken_Input = bool.Parse(hash["Spoken_Input"].ToString());
                sio.Spoken_Output = bool.Parse(hash["Spoken_Output"].ToString());
                sio.Comments = (hash["Comments"] == null) ? null : hash["Comments"].ToString();
                if (this.onUpdateInputOutput != null && this.GetSpokenInputOutput != null)
                {
                    this.onUpdateInputOutput(sender, sio);
                    this.DictSpokenInputOutputs_Result[featureKey] = this.GetSpokenInputOutput(featureKey);
                }
            }
            else
            {
                int featureKey = int.Parse(editableItem.GetDataKeyValue("FeatureKey").ToString());
                string featureName = (hash["FeatureName"] == null) ? null : hash["FeatureName"].ToString();

                if (!string.IsNullOrEmpty(featureName))
                {
                    if (this.UpdateFeature != null && this.GetFeatures != null)
                    {
                        this.UpdateFeature(new Feature { FeatureKey = featureKey, FeatureName = featureName });
                        int currentPageIndex = this.RadGridSpokenInputOutput.CurrentPageIndex;
                        int pageSize = this.RadGridSpokenInputOutput.MasterTableView.PageSize;
                        this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                    }
                    else
                    {
                        this.label_errorMsg.Visible = true;
                        this.label_errorMsg.Text = "No Product Selected! ";
                    }
                }
            }
        }

        protected void RadGridSpokenInputOutput_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                string name = e.Item.OwnerTableView.Name;
                if (name == "InputOutputs")
                {
                    var item = (GridEditableItem)e.Item;
                    var ddlLanguage = (DropDownList)item.FindControl("DDLLanguage");
                    var languageList = this.BasicLanguage_List;
                    ddlLanguage.DataSource = languageList;
                    ddlLanguage.DataTextField = "Language";
                    ddlLanguage.DataValueField = "LanguageKey";
                    ddlLanguage.DataBind();

                    string language = DataBinder.Eval(e.Item.DataItem, "Language").ToString();
                    if (!String.IsNullOrEmpty(language))
                        ddlLanguage.SelectedValue = languageList.First(c => c.Language.Equals(language)).LanguageKey.ToString();
                }
            }
        }

        protected void RadGridSpokenInputOutput_InsertCommand(object sender, GridCommandEventArgs e)
        {
            this.label_errorMsg.Visible = false;
            this.label_errorMsg.Text = "";
            if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
            {
                var editableItem = ((GridEditableItem)e.Item);
                var hash = new Dictionary<object, object>();
                editableItem.ExtractValues(hash);
                string name = e.Item.OwnerTableView.Name;
                if (name == "InputOutputs")
                {
                    DropDownList ddlLanguage = (DropDownList)editableItem.FindControl("DDLLanguage");
                    int languageKey = int.Parse(ddlLanguage.SelectedValue);

                    GridDataItem parentItem = e.Item.OwnerTableView.ParentItem;
                    int featureKey = int.Parse(parentItem.GetDataKeyValue("FeatureKey").ToString());

                    var sio = new SpokenInputOutput
                    {
                        FeatureKey = featureKey,
                        LanguageKey = languageKey,
                        Spoken_Output = bool.Parse(hash["Spoken_Output"].ToString()),
                        Spoken_Input = bool.Parse(hash["Spoken_Input"].ToString()),
                        Comments = (hash["Comments"] == null) ? null : hash["Comments"].ToString()
                    };
                    if (this.onInsertInputOutput != null && this.GetSpokenInputOutput != null)
                    {
                        this.onInsertInputOutput(sender, sio);
                        this.DictSpokenInputOutputs_Result[featureKey] = this.GetSpokenInputOutput(featureKey);
                    }
                }
                else
                {
                    string featureName = (hash["FeatureName"] == null) ? null : hash["FeatureName"].ToString();
                    if (!string.IsNullOrEmpty(featureName))
                    {
                        if (this.InsertFeature != null && this.GetFeatures != null)
                        {
                            this.InsertFeature(featureName);
                            int currentPageIndex = this.RadGridSpokenInputOutput.CurrentPageIndex;
                            int pageSize = this.RadGridSpokenInputOutput.MasterTableView.PageSize;
                            this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                            this.TotalRecord = this.GetTotalRecord();
                        }
                        else
                        {
                            this.label_errorMsg.Visible = true;
                            this.label_errorMsg.Text = "No Product Selected! ";
                        }
                    }
                }
            }
        }

        protected void RadGridSpokenInputOutput_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var editableItem = ((GridEditableItem)e.Item);
                int featureKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("FeatureKey").ToString());
                string sSpokenInputOutputKey = editableItem.GetDataKeyValue("SpokenInputOutputKey").ToString();
                int spokenInputOutputKey = int.Parse(sSpokenInputOutputKey);

                var sio = new SpokenInputOutput { SpokenInputOutputKey = spokenInputOutputKey };
                if (this.onDeleteInputOutput != null && this.GetSpokenInputOutput != null)
                {
                    this.onDeleteInputOutput(sender, sio);
                    this.DictSpokenInputOutputs_Result[featureKey] = this.GetSpokenInputOutput(featureKey);
                }
            }
            else if (e.CommandName == "ExpandCollapse")
            {
                var dataItem = (GridDataItem)e.Item;
                dataItem["FeatureEdit"].Visible = true;
                dataItem["TBFeature"].Enabled = true;
            }
        }

        protected void RadGridSpokenInputOutput_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            int currentPageIndex = e.NewPageIndex;
            int pageSize = this.RadGridSpokenInputOutput.MasterTableView.PageSize;
            this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
        }

        protected void RadGridSpokenInputOutput_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            int currentPageIndex = this.RadGridSpokenInputOutput.CurrentPageIndex;
            int pageSize = e.NewPageSize;
            this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
        }

        public Dictionary<int, List<GetSpokenInputOutputOfProduct_Result>> DictSpokenInputOutputs_Result
        {
            get
            {
                return this.ViewState["DictSpokenInputOutputs_Result"] as Dictionary<int, List<GetSpokenInputOutputOfProduct_Result>>;
            }
            set
            {
                this.ViewState["DictSpokenInputOutputs_Result"] = value;
            }
        }

        public List<BasicLanguageList> BasicLanguage_List
        {
            get
            {
                return this.ViewState["BasicLanguage_List"] as List<BasicLanguageList>;
            }
            set
            {
                this.ViewState["BasicLanguage_List"] = value;
            }
        }

        public event Func<int, int, List<GetFeatureOfProduct_Result>> GetFeatures;

        public event Func<List<BasicLanguageList>> GetLanguages;

        public event EventHandler<SpokenInputOutput> onUpdateInputOutput;

        public event EventHandler<SpokenInputOutput> onInsertInputOutput;

        public event EventHandler<SpokenInputOutput> onDeleteInputOutput;

        public List<GetFeatureOfProduct_Result> FeatureOfProduct_Result
        {
            get
            {
                return this.ViewState["FeatureOfProduct_Result"] as List<GetFeatureOfProduct_Result>;
            }
            set
            {
                this.ViewState["FeatureOfProduct_Result"] = value;
            }
        }

        public event Func<int, List<GetSpokenInputOutputOfProduct_Result>> GetSpokenInputOutput;

        public event Action<string> InsertFeature;

        public event Action<Feature> UpdateFeature;

        public int TotalRecord
        {
            get
            {
                return (this.ViewState["TotalRecord"] != null) ? (int)this.ViewState["TotalRecord"] : 0;
            }
            set
            {
                this.ViewState["TotalRecord"] = value;
            }
        }

        public event Func<int> GetTotalRecord;
    }
}
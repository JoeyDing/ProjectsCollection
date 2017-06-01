using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.Eol
{
    public partial class TextInputOutputControl : System.Web.UI.UserControl, ITextInputOutputBridge
    {
        protected void RadGridTextInputOutput_PreRender(object sender, EventArgs e)

        {
            if (!Page.IsPostBack)
            {
                foreach (GridItem item in RadGridTextInputOutput.MasterTableView.Items)
                {
                    item.Expanded = true;
                }
                //RadGridTextInputOutput.MasterTableView.Items[0].Expanded = true;
                //RadGridTextInputOutput.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if these 2 property is null,it will throw errors when you use them, see how they are used below ; so initialize them here

            if (this.FeatureOfProduct_Result == null)
                this.FeatureOfProduct_Result = new List<GetFeatureOfProduct_Result>();
            if (this.DictTextInputOutputs_Result == null)
                this.DictTextInputOutputs_Result = new Dictionary<int, Dictionary<int, GetTextInputOutputOfProduct_Result>>();
            //Visible: When this control is clicked
            //IsPostBack: When Response.Redirect happens and changes the url to *****TextinputOutput****
            if (this.Visible && !IsPostBack && this.GetFeatures != null && this.GetTotalRecord != null && this.GetLanguages != null)
            {
                this.TotalRecord = this.GetTotalRecord();
                int currentPageIndex = this.RadGridTextInputOutput.CurrentPageIndex;
                int pageSize = this.RadGridTextInputOutput.MasterTableView.PageSize;
                this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
                this.BasicLanguage_List = this.GetLanguages();
            }
            else
                this.label_errorMsg.Visible = false;
        }

        protected void RadGridTextInputOutput_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            int featureKey = int.Parse(dataItem.GetDataKeyValue("FeatureKey").ToString());
            //When expand detail table, make featureEidt invisible
            dataItem["FeatureEdit"].Visible = false;
            dataItem["TBFeature"].Enabled = false;

            if (!this.DictTextInputOutputs_Result.ContainsKey(featureKey) && this.GetTextInputOutput != null)
            {
                var innerItem = this.GetTextInputOutput(featureKey).ToDictionary(c => c.TextInputOutputKey, c => c);
                this.DictTextInputOutputs_Result.Add(featureKey, innerItem);
            }
            e.DetailTableView.DataSource = DictTextInputOutputs_Result[featureKey].Values.ToList();
        }

        protected void RadGridTextInputOutput_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.RadGridTextInputOutput.VirtualItemCount = this.TotalRecord;

            //var features = this.FeatureOfProduct_Result.Select(u => new { FeatureName = u.FeatureName, FeatureKey = u.FeatureKey }).Distinct().ToList();
            this.RadGridTextInputOutput.DataSource = this.FeatureOfProduct_Result;
        }

        protected void RadGridTextInputOutput_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            this.label_errorMsg.Visible = false;
            this.label_errorMsg.Text = "";

            var editableItem = ((GridEditableItem)e.Item);

            var hash = new Dictionary<object, object>();
            editableItem.ExtractValues(hash);

            string name = e.Item.OwnerTableView.Name;
            if (name == "InputOutputs")
            {
                int featureKey = int.Parse(editableItem.OwnerTableView.ParentItem.GetDataKeyValue("FeatureKey").ToString());
                int textInputOutputKey = int.Parse(editableItem.GetDataKeyValue("TextInputOutputKey").ToString());

                var tio = new TextInputOutput();
                tio.TextInputOutputKey = textInputOutputKey;
                DropDownList ddlLanguage = (DropDownList)editableItem.FindControl("DDLLanguage");
                int languageKey = int.Parse(ddlLanguage.SelectedValue);
                string languageName = ddlLanguage.SelectedItem.Text;
                tio.LanguageKey = languageKey;
                tio.Text_Input = bool.Parse(hash["Text_Input"].ToString());
                tio.Text_Output = bool.Parse(hash["Text_Output"].ToString());
                tio.Comments = (hash["Comments"] == null) ? null : hash["Comments"].ToString();
                if (this.onUpdateInputOutput != null)
                    this.onUpdateInputOutput(sender, tio);
                RefreshTioResult(featureKey, textInputOutputKey, tio, languageName, false);
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
                        int currentPageIndex = this.RadGridTextInputOutput.CurrentPageIndex;
                        int pageSize = this.RadGridTextInputOutput.MasterTableView.PageSize;
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

        private void RefreshTioResult(int featureKey, int textInputOutputKey, TextInputOutput tio, string languageName, bool isDelete)
        {
            if (!isDelete)
            {
                var tioResult = new GetTextInputOutputOfProduct_Result();
                tioResult.TextInputOutputKey = tio.TextInputOutputKey;
                tioResult.Comments = tio.Comments;
                tioResult.Language = languageName;
                tioResult.Text_Input = tio.Text_Input;
                tioResult.Text_Output = tio.Text_Output;

                //works for both insert and update tioResult
                DictTextInputOutputs_Result[featureKey][textInputOutputKey] = tioResult;
            }
            else
                DictTextInputOutputs_Result[featureKey].Remove(textInputOutputKey);
        }

        protected void RadGridTextInputOutput_ItemDataBound(object sender, GridItemEventArgs e)
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
                    var data = e.Item.DataItem as GetTextInputOutputOfProduct_Result;

                    if (data != null)
                    {
                        string language = DataBinder.Eval(data, "Language").ToString();
                        if (!String.IsNullOrEmpty(language))
                            ddlLanguage.SelectedValue = languageList.First(c => c.Language.Equals(language)).LanguageKey.ToString();
                    }
                }
            }
        }

        protected void RadGridTextInputOutput_InsertCommand(object sender, GridCommandEventArgs e)
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
                    string languageName = ddlLanguage.SelectedItem.Text;
                    int featureKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("FeatureKey").ToString());

                    var tio = new TextInputOutput
                    {
                        FeatureKey = featureKey,
                        LanguageKey = languageKey,
                        Text_Output = bool.Parse(hash["Text_Output"].ToString()),
                        Text_Input = bool.Parse(hash["Text_Input"].ToString()),
                        Comments = (hash["Comments"] == null) ? null : hash["Comments"].ToString()
                    };
                    if (this.onInsertInputOutput != null)
                        this.onInsertInputOutput(sender, tio);
                    RefreshTioResult(featureKey, tio.TextInputOutputKey, tio, languageName, false);
                }
                else
                {
                    string featureName = (hash["FeatureName"] == null) ? null : hash["FeatureName"].ToString();
                    if (!string.IsNullOrEmpty(featureName))
                    {
                        if (this.InsertFeature != null && this.GetFeatures != null && this.GetTotalRecord != null)
                        {
                            this.InsertFeature(featureName);
                            int currentPageIndex = this.RadGridTextInputOutput.CurrentPageIndex;
                            int pageSize = this.RadGridTextInputOutput.MasterTableView.PageSize;
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

        protected void RadGridTextInputOutput_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                var editableItem = ((GridDataItem)e.Item);
                int featureKey = int.Parse(e.Item.OwnerTableView.ParentItem.GetDataKeyValue("FeatureKey").ToString());
                string sTextInputOutputKey = editableItem.GetDataKeyValue("TextInputOutputKey").ToString();
                int textInputOutputKey = int.Parse(sTextInputOutputKey);

                var tio = new TextInputOutput { TextInputOutputKey = textInputOutputKey };
                if (this.onDeleteInputOutput != null)
                    this.onDeleteInputOutput(sender, tio);

                RefreshTioResult(featureKey, textInputOutputKey, null, "", true);
            }
            else if (e.CommandName == "ExpandCollapse")
            {
                var dataItem = (GridDataItem)e.Item;
                dataItem["FeatureEdit"].Visible = true;
                dataItem["TBFeature"].Enabled = true;
            }
        }

        protected void RadGridTextInputOutput_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            int currentPageIndex = e.NewPageIndex;
            int pageSize = this.RadGridTextInputOutput.MasterTableView.PageSize;
            this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
        }

        protected void RadGridTextInputOutput_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            int currentPageIndex = this.RadGridTextInputOutput.CurrentPageIndex;
            int pageSize = e.NewPageSize;
            this.FeatureOfProduct_Result = this.GetFeatures(currentPageIndex * pageSize, (currentPageIndex + 1) * pageSize);
        }

        public Dictionary<int, Dictionary<int, GetTextInputOutputOfProduct_Result>> DictTextInputOutputs_Result
        {
            get
            {
                return this.ViewState["DictTextInputOutputs_Result"] as Dictionary<int, Dictionary<int, GetTextInputOutputOfProduct_Result>>;
            }

            set
            {
                this.ViewState["DictTextInputOutputs_Result"] = value;
            }
        }

        public event EventHandler<TextInputOutput> onUpdateInputOutput;

        public event EventHandler<TextInputOutput> onInsertInputOutput;

        public event EventHandler<TextInputOutput> onDeleteInputOutput;

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

        public event Func<List<BasicLanguageList>> GetLanguages;

        public event Func<int, List<GetTextInputOutputOfProduct_Result>> GetTextInputOutput;

        public event Action<string> InsertFeature;

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

        public event Func<int, int, List<GetFeatureOfProduct_Result>> GetFeatures;

        public event Action<Feature> UpdateFeature;

        public event Func<int> GetTotalRecord;

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
    }
}
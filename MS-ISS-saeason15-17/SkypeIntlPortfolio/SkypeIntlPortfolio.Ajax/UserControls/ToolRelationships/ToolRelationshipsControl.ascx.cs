using SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships.JobRelatedClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace SkypeIntlPortfolio.Ajax.UserControls.ToolRelationships
{
    public partial class ToolRelationshipsControl : System.Web.UI.UserControl, IToolRelationshipsView
    {
        public event Func<IEnumerable<ToolName>> GetToolsInfo;

        public event Func<string, IEnumerable<JobName>> GetJobsInfo;

        public event Func<string, IEnumerable<StepName>> GetStepsInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RadGrid_ToolRelationships_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (GetToolsInfo != null)
                this.RadGrid_ToolRelationships.DataSource = this.GetToolsInfo().ToList();
            //DataTable dt = new DataTable();
            //dt.Clear();
            //dt.Columns.Add("TestName");
            //DataRow dataRow = dt.NewRow();
            //dataRow["TestName"] = "this record is for testing";
            //dt.Rows.Add(dataRow);
            //this.RadGrid_ToolRelationships.DataSource = dt;
        }

        protected void RadGrid_ToolRelationships_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = e.DetailTableView.ParentItem;
            switch (e.DetailTableView.Name)
            {
                case "JobDetails":
                    {
                        string toolName = dataItem.GetDataKeyValue("Tool_Name").ToString();
                        if (this.GetJobsInfo != null)
                            e.DetailTableView.DataSource = this.GetJobsInfo(toolName);
                        break;
                    }
                case "StepDetails":
                    {
                        string jobName = dataItem.GetDataKeyValue("Job_Name").ToString();
                        if (this.GetStepsInfo != null)
                            e.DetailTableView.DataSource = this.GetStepsInfo(jobName);
                        break;
                    }
            }
        }

        protected List<string> GetSearchRusult(string searchContent, List<string> searchType)
        {
            List<string> ToolsHaveSerachConetent = new List<string>();


            //if user seach empty, return all tools name
            if (searchContent.Trim() == "")
            {
                foreach (var item in GetToolsInfo())
                {
                    ToolsHaveSerachConetent.Add(item.Tool_Name);
                }
                return ToolsHaveSerachConetent;
            }


            foreach (var tempToolItem in GetToolsInfo())
            {
                if (searchType.Exists(x => x == "SearchInTool"))//Judge wether search Tool content
                {
                    //Judge whether job contains the search content
                    if (tempToolItem.Tool_Name.ToLower().Contains(searchContent.ToLower()))
                    {
                        ToolsHaveSerachConetent.Add(tempToolItem.Tool_Name);
                        continue; //search foreach tool level
                    }
                }

                int whetherFoundResultInStep = 0;
                //Go into Job Level
                if (searchType.Exists(x => x == "SearchInJob") || searchType.Exists(x => x == "SearchInStep")) //If no need to seach in job or step, don't requir data from back end
                {
                    foreach (var tempJobItem in GetJobsInfo(tempToolItem.Tool_Name))
                    {
                        if (whetherFoundResultInStep == 1) break; //If already found content in step name, stop search in current tool.
                        if (searchType.Exists(x => x == "SearchInJob")) //Judge wether search Job content
                        {
                            //Judge whether job contains the search content
                            if(tempJobItem.Job_Name.ToLower().Contains(searchContent.ToLower()))
                            {
                                ToolsHaveSerachConetent.Add(tempToolItem.Tool_Name);
                                break; //search foreach job level
                            }
                        }
                        //Go into Step Level
                        if (searchType.Exists(x => x == "SearchInStep")) //Judge wether search Job content
                        {
                            foreach (var tempStepItem in GetStepsInfo(tempJobItem.Job_Name))
                            {

                                //Judge whether step contains the search content
                                if (tempStepItem.Step_Name.ToLower().Contains(searchContent.ToLower()))
                                {
                                    ToolsHaveSerachConetent.Add(tempToolItem.Tool_Name);
                                    whetherFoundResultInStep = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return ToolsHaveSerachConetent;
        }
        protected IEnumerable<ToolName> SwitchToDataSouce(List<string> ToolsHaveSerachConetent)
        {
            List<ToolName> switchResult = new List<ToolName>();
            foreach (var item in ToolsHaveSerachConetent)
            {
                ToolName newToolName = new ToolName();
                newToolName.Tool_Name = item;
                switchResult.Add(newToolName);
            }
            return switchResult;
        }

        protected List<string> GetSearchType()
        {
            string searchContent = this.SearchContentText.Text;

            List<string> searchType = new List<string>();
            if (SearchInTool.Checked) { searchType.Add("SearchInTool"); }
            if (SearchInJob.Checked) { searchType.Add("SearchInJob"); }
            if (SearchInStep.Checked) { searchType.Add("SearchInStep"); }
            return searchType;
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            string searchContent = this.SearchContentText.Text;
            var returnList = GetSearchRusult(searchContent, GetSearchType());
            this.RadGrid_ToolRelationships.DataSource = SwitchToDataSouce(returnList);
            this.RadGrid_ToolRelationships.Rebind();
        }


    }
}
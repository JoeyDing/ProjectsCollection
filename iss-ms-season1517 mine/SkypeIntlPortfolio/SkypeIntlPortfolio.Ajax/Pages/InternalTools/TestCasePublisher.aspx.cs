using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools
{
    public partial class TestCasePublisher : System.Web.UI.Page
    {
        private const string VSO_PROJECT = "LOCALIZATION";

        private Dictionary<string, string> ListExistedLang
        {
            get { return Session["ListExistedLang"] as Dictionary<string, string>; }
            set { Session["ListExistedLang"] = value; }
        }

        private void ValidatePage()
        {
            this.Validate();
            this.label_languagesNotSelected.Visible = !this.radlistbox_languages.CheckedItems.Any();
        }

        private bool IsPageValid()
        {
            //force the validator controls to validate
            this.Validate();
            return this.IsValid && this.radlistbox_languages.CheckedItems.Any();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack == false)
            {
                this.label_listboxNotChecked.Visible = false;
                //populate listbox of languages
                var languages = Utils.GetLanguages();
                foreach (var language in languages)
                {
                    this.radlistbox_languages.Items.Add(new RadListBoxItem(language));
                }

                ListExistedLang = new Dictionary<string, string>();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            label_submitSuccess.Visible = false;

            //validate the page
            this.ValidatePage();
            if (this.IsPageValid())
            {
                //create suites inside test plan
                var context = this.GetVsoContext();
                //use caseQuery to get testcases
                string queryUrl = this.radtextbox_caseQuery.Text;
                var testcaseIdsFromQuery = new List<int>();
                if (!string.IsNullOrEmpty(queryUrl))
                {
                    try
                    {
                        var queryUrlList = queryUrl.Split(new string[] { "tempQueryId=" }, StringSplitOptions.RemoveEmptyEntries);
                        string queryId = queryUrlList[1];
                        string queryText = context.GetQueryTextByQueryID(queryId);
                        var dictTestCases = context.RunQuery(VSO_PROJECT, queryText);
                        testcaseIdsFromQuery = dictTestCases.Keys.ToList();
                    }
                    catch (Exception)
                    {
                        this.label_WrongQueryURL.Visible = true;
                        return;
                    }
                }

                //
                int planId = (int)this.txtPlanID.Value.Value;
                int selectedSuiteId = 0;
                if (radcombobox_suites.Items.Any())
                {
                    selectedSuiteId = int.Parse(this.radcombobox_suites.SelectedItem.Value);
                }

                string[] suiteNames = this.radlistbox_languages.CheckedItems.Where(c => c.BackColor != System.Drawing.Color.Yellow).Select(c => c.Text).ToArray();
                string[] existedSuiteNames = this.radlistbox_languages.CheckedItems.Where(c => c.BackColor == System.Drawing.Color.Yellow).Select(c => c.Text).ToArray();
                string existedSuiteIds = "";
                string[] existedIds = new string[] { };
                if (existedSuiteNames.Any())
                {
                    foreach (var existedSuiteName in existedSuiteNames)
                    {
                        existedSuiteIds += ListExistedLang[existedSuiteName] + ",";
                    }
                    existedSuiteIds = existedSuiteIds.Substring(0, existedSuiteIds.Length - 1);
                    existedIds = existedSuiteIds.Split(',');
                }

                int totalCheckedLang = this.radlistbox_languages.CheckedItems.Count();
                string errorMsg = "";
                if (suiteNames.Any() || existedIds.Any())
                {
                    try
                    {
                        int totalRetry = int.Parse(WebConfigurationManager.AppSettings["TestCasePublisher:CreateTestItems_RetryTimes"].ToString());
                        context.CloneTestSuiteInsidePlan2(planId, VSO_PROJECT, selectedSuiteId, suiteNames, existedIds, testcaseIdsFromQuery, totalRetry);
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.ToString();
                    }
                    try
                    {
                        label_submitSuccess.Text = "Test Cases have been published!" + " Total of Languages: " + totalCheckedLang + ";" + errorMsg;
                        label_submitSuccess.Visible = true;
                        this.label_wrongTemplate.Visible = false;
                        this.label_listboxNotChecked.Visible = false;
                        this.label_errorMsg.Visible = false;
                        this.label_WrongQueryURL.Visible = false;

                        //reload listbox of languages and combobox of languages
                        this.ListExistedLang.Clear();
                        this.radcombobox_suites.Items.Clear();

                        var result = context.GetListOfTestSuitesByPlanID(VSO_PROJECT, planId);
                        //add the items inside the combobox
                        var subItems = result["value"];
                        foreach (var item in subItems.Where(c => c["parent"] != null))
                        {
                            string name = (string)item["name"];
                            string id = (string)item["id"];
                            this.ListExistedLang.Add(name, id);

                            var comboItem = new RadComboBoxItem();
                            comboItem.Value = id;
                            comboItem.Text = string.Format("{0} ({1})", name, id);
                            this.radcombobox_suites.Items.Add(comboItem);
                        }
                        this.CheckAlreadyCreatedItems(this.ListExistedLang);
                        btnSubmit.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
            }
        }

        protected void txtPlanID_TextChanged(object sender, EventArgs e)
        {
            this.radAsyncUpload_Template.Enabled = false;
            this.label_wrongTemplate.Visible = false;
            this.label_listboxNotChecked.Visible = false;
            this.label_errorMsg.Visible = false;
            this.label_WrongQueryURL.Visible = false;
            this.label_submitSuccess.Visible = false;
            this.radTabStrip_testCaseOptions.Visible = false;
            this.RadMultiPage_testCaseOptions.Visible = false;

            this.btnSubmit.Enabled = false;

            //1 - cleanup combobox and listbox checkedItems and planName and planUrl
            this.radcombobox_suites.Items.Clear();
            this.radcombobox_suites.Enabled = false;
            this.radlistbox_languages.ClearChecked();
            this.label_testPlanName.Text = "";
            this.hyperlink_testPlanUrl.Text = "";
            //2 - check if the test plan exist
            var context = this.GetVsoContext();
            if (this.txtPlanID.Value.HasValue)
            {
                try
                {
                    this.ListExistedLang.Clear();
                    int testPlanId = (int)this.txtPlanID.Value;
                    var result = context.GetListOfTestSuitesByPlanID(VSO_PROJECT, testPlanId);
                    this.radAsyncUpload_Template.Enabled = true;
                    this.label_errorMsg.Visible = false;
                    this.radTabStrip_testCaseOptions.Visible = true;
                    this.RadMultiPage_testCaseOptions.Visible = true;
                    this.radTabStrip_testCaseOptions.SelectedIndex = 0;
                    this.RadMultiPage_testCaseOptions.SelectedIndex = 0;
                    this.radtextbox_caseQuery.Text = "";
                    this.btnSubmit.Enabled = true;

                    //add the items inside the combobox
                    var subItems = result["value"];
                    string planName = (string)subItems[0]["plan"]["name"];
                    string planUrl = context.GetTestPlanUrl(testPlanId);
                    label_testPlanName.Text = planName;
                    hyperlink_testPlanUrl.NavigateUrl = planUrl;
                    hyperlink_testPlanUrl.Text = planUrl;

                    foreach (var item in subItems.Where(c => c["parent"] != null))
                    {
                        this.radcombobox_suites.Enabled = true;
                        string name = (string)item["name"];
                        string id = (string)item["id"];
                        this.ListExistedLang.Add(name, id);

                        var comboItem = new RadComboBoxItem();
                        comboItem.Value = id;
                        comboItem.Text = string.Format("{0} ({1})", name, id);
                        this.radcombobox_suites.Items.Add(comboItem);
                    }
                    this.CheckAlreadyCreatedItems(this.ListExistedLang);
                }
                catch (Exception ex)
                {
                    //show error message
                    this.label_errorMsg.Text = ex.Message;
                    this.label_errorMsg.Visible = true;
                }
            }

            //2- if the test plan exist, populate the combobox with the available test suites
        }

        private VsoContext GetVsoContext()
        {
            string key = WebConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoAccount = WebConfigurationManager.AppSettings["VsoRootAccount"];
            return new VsoContext(vsoAccount, key);
        }

        //protected void radlistbox_languages_CheckAllCheck(object sender, RadListBoxCheckAllCheckEventArgs e)
        //{
        //    //CheckAlreadyCreatedItems(ListExistedLang);
        //}

        private void CheckAlreadyCreatedItems(Dictionary<string, string> existingItems)
        {
            foreach (RadListBoxItem item in this.radlistbox_languages.Items)
            {
                if (existingItems.ContainsKey(item.Text))
                {
                    item.Checked = true;
                    //item.Enabled = false;
                    item.BackColor = System.Drawing.Color.Yellow;
                }
                else
                {
                    item.BackColor = System.Drawing.Color.Transparent;
                }
            }
        }

        protected void btnDummy_Click(object sender, EventArgs e)
        {
            this.label_wrongTemplate.Visible = false;
            UploadedFile file = this.radAsyncUpload_Template.UploadedFiles[0];
            Stream fileStream = file.InputStream;
            StreamReader reader = new StreamReader(fileStream);
            string languages = reader.ReadToEnd();
            Dictionary<string, string> dictLang = new Dictionary<string, string>();
            Dictionary<string, string> dictAllLang = new Dictionary<string, string>();
            string[] languagePackage = languages.Split(new char[] { ',' });
            string[] allLanguage = Utils.GetLanguages();

            foreach (var lang in allLanguage)
            {
                dictAllLang.Add(lang, lang);
            }
            if (languagePackage != null)
            {
                foreach (var lang in languagePackage)
                {
                    if (dictAllLang.ContainsKey(lang))
                    {
                        dictLang.Add(lang, lang);
                    }
                    else
                    {
                        this.label_wrongTemplate.Visible = true;
                        break;
                    }
                }
                if (!label_wrongTemplate.Visible)
                {
                    foreach (RadListBoxItem item in radlistbox_languages.Items)
                    {
                        if (!item.Enabled)
                        {
                            item.BackColor = System.Drawing.Color.Yellow;
                        }
                        if (dictLang.ContainsKey(item.Text))
                        {
                            item.Checked = true;
                        }
                    }
                }
            }
            else
            {
                this.label_wrongTemplate.Visible = true;
            }
        }

        protected void radbutton_download_Click(object sender, EventArgs e)
        {
            this.label_listboxNotChecked.Visible = true;
            if (radlistbox_languages.CheckedItems.Any())
            {
                // Get the physical Path of the file
                string languages = "";
                foreach (RadListBoxItem item in radlistbox_languages.CheckedItems)
                {
                    languages += item.Text + ",";
                }
                if (!string.IsNullOrWhiteSpace(languages))
                {
                    languages = languages.Substring(0, languages.Length - 1);
                }
                this.label_listboxNotChecked.Visible = false;
                Response.AddHeader("Content-disposition", "attachment; filename=langTemplate.csv");
                Response.ContentType = "application/octet-stream";
                Response.Write(languages);
                Response.End();

                this.updatePanel_main.Update();
            }
            else
            {
                this.label_listboxNotChecked.Visible = true;
            }
        }

        protected void radbutton_1_Click(object sender, EventArgs e)
        {
            radcombobox_suites.Enabled = true;
            radtextbox_caseQuery.Enabled = false;
        }

        protected void radbutton_2_Click(object sender, EventArgs e)
        {
            radcombobox_suites.Enabled = false;
            radtextbox_caseQuery.Enabled = true;
        }

        protected void radTabStrip_testCaseOptions_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (this.radTabStrip_testCaseOptions.SelectedIndex == 0)
            {
                this.radtextbox_caseQuery.Text = "";
            }
        }
    }
}
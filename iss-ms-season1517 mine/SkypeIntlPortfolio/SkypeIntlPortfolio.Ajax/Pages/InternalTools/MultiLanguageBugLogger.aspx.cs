using Ionic.Zip;
using Newtonsoft.Json.Linq;
using SkypeIntlPortfolio.Ajax.Core.Service.Vso;
using SkypeIntlPortfolio.Ajax.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.Pages.InternalTools
{
    public partial class MultiLanguageBugLogger : System.Web.UI.Page
    {
        public class ZipFileInfo
        {
            public string path;
            public Stream stream;
            public int uploadVsoStatus;
        }

        public class ZipCultureInfo
        {
            public string language;
            public string vsoLinks;
            public bool isSupported;
            public Dictionary<string, ZipFileInfo> files = new Dictionary<string, ZipFileInfo>();
        }

        private const string VSO_PROJECT = "LOCALIZATION";

        private static string title = "";
        private static string areaPath = "";
        private static string iterationPath = "";
        private static string assignedTo = "";
        private static string state = "";
        private static string reason = "";
        private static string priority = "";
        private static string severity = "";
        private static string issueType = "";
        private static string issueSubType = "";
        private static string triaged = "";
        private static string keywords = "";
        private static string howFound = "";
        private static string source = "";
        private static string fixProvidedBy = "";
        private static string rootCauseCategory = "";
        private static string branchFound = "";
        private static string buildFound = "";
        private static string fixedBuild = "";
        private static string description = "";
        private static string reproSteps = "";
        private static string systemInfo = "";
        private static string dueDate = "";
        private static string tags = "";

        private static string newBugUrl = "";
        private static int totalThread = 1;
        private static int totalRetry = 3;

        protected void Page_Load(object sender, EventArgs e)
        {
            totalThread = int.Parse(WebConfigurationManager.AppSettings["MultiBugLogger:AddAttachmentsToBug_TotalThread"]);
            totalRetry = int.Parse(WebConfigurationManager.AppSettings["MultiBugLogger:CreateBugsWithAttachments_RetryTimes"]);

            if (!IsPostBack)
            {
                var languages = Utils.GetLanguages();
                //populate listbox of languages
                for (int i = 0; i < languages.Length; i++)
                {
                    this.radlistbox_languages.Items.Add(new RadListBoxItem(languages[i]));
                    Label radlistbox_languagesLabel = this.radlistbox_languages.Items[i].FindControl("languageLabel") as Label;
                    radlistbox_languagesLabel.Text = languages[i];
                }
            }
        }

        protected void radtextbox_bugID_TextChanged(object sender, EventArgs e)
        {
            this.radgrid_lang.Visible = this.label_gridCaption.Visible = false;
            this.label_sampleBugAssignedTo.Visible = this.label_generalErrorMsg.Visible = this.label_languagesNotSelected.Visible = this.label_templateTitleWrong.Visible = this.label_errorMsg.Visible = false;

            this.label_submitSuccess.Visible = false;
            this.label_submitFail.Visible = false;
            this.label_new.Visible = false;
            this.radtextbox_newParentBugTitle.Visible = false;
            this.radtextbox_newParentBugTitle.Enabled = true;
            this.label_existing.Visible = false;
            this.label_existingParentBugTitle.Visible = false;
            this.radiobuttonlist_parentBug.Visible = false;
            this.label_parentBugUrl.Visible = false;
            this.hyperlink_parentBugUrl.Visible = false;

            radlistbox_languages.ClearChecked();
            this.radlistbox_languages_new.Items.Clear();

            //leave all the textbox and label empty
            this.label_sampleBugLanguage.Text = this.label_sampleTitle.Text = this.radtextbox_templateTitle.Text = "";

            if (this.radtextbox_bugID.Value.HasValue)
            {
                //2 - check if the bug exist
                var context = this.GetVsoContext();
                try
                {
                    int bugId = (int)this.radtextbox_bugID.Value;
                    List<int> workItemIDs = new List<int> { bugId };
                    var result = context.GetListOfWorkItemsByIDs(workItemIDs);

                    this.label_sampleBugAssignedTo.Visible = true;
                    var subItems = result["value"];
                    string language = (string)subItems[0]["fields"]["Skype.Language"];
                    label_sampleBugLanguage.Text = language;
                    //create fields for submit to use
                    title = (string)subItems[0]["fields"]["System.Title"];
                    label_sampleTitle.Text = title;

                    foreach (var lang in Utils.GetLanguages())
                    {
                        if (!title.Contains("Malayalam") && title.Contains(lang))
                        {
                            title = title.Replace(lang, "[lang]");
                            break;
                        }
                        else if (title.Contains("Malayalam"))
                        {
                            title = title.Replace("Malayalam", "[lang]");
                            break;
                        }
                    }
                    radtextbox_templateTitle.Text = title;

                    areaPath = (string)subItems[0]["fields"]["System.AreaPath"];
                    iterationPath = (string)subItems[0]["fields"]["System.IterationPath"];
                    assignedTo = (string)subItems[0]["fields"]["System.AssignedTo"];
                    label_sampleBugAssignedTo.Text = assignedTo;
                    state = (string)subItems[0]["fields"]["System.State"];
                    reason = (string)subItems[0]["fields"]["System.Reason"];
                    priority = (string)subItems[0]["fields"]["Skype.Priority"];
                    severity = (string)subItems[0]["fields"]["Microsoft.VSTS.Common.Severity"];
                    issueType = (string)subItems[0]["fields"]["Skype.IssueType"];
                    issueSubType = (string)subItems[0]["fields"]["Skype.IssueSubtype"];
                    triaged = (string)subItems[0]["fields"]["Skype.Triaged"];
                    //triaged for child bugs is always "No"
                    //triaged = "No";
                    keywords = (string)subItems[0]["fields"]["Skype.Keywords"];
                    howFound = (string)subItems[0]["fields"]["Skype.HowFound"];
                    source = (string)subItems[0]["fields"]["Skype.Source"];
                    fixProvidedBy = (string)subItems[0]["fields"]["Skype.FixProvidedBy"];
                    rootCauseCategory = (string)subItems[0]["fields"]["Skype.RootCauseCategory"];
                    branchFound = (string)subItems[0]["fields"]["Skype.BranchFound"];
                    buildFound = (string)subItems[0]["fields"]["Skype.BuildFound"];
                    fixedBuild = (string)subItems[0]["fields"]["Skype.FixedBuild"];
                    description = (string)subItems[0]["fields"]["System.Description"];
                    reproSteps = (string)subItems[0]["fields"]["Microsoft.VSTS.TCM.ReproSteps"];
                    systemInfo = (string)subItems[0]["fields"]["Microsoft.VSTS.TCM.SystemInfo"];
                    dueDate = (string)subItems[0]["fields"]["Microsoft.VSTS.Scheduling.DueDate"];
                    tags = (string)subItems[0]["fields"]["System.Tags"];

                    //check to see if sample bug has a parent bug
                    string parentBugUrl = "";

                    if (subItems[0]["relations"] != null)
                    {
                        foreach (var item in subItems[0]["relations"])
                        {
                            string rel = (string)item["rel"];
                            if (rel == "System.LinkTypes.Hierarchy-Reverse")
                            {
                                string url = (string)item["url"];
                                var json = context.GetParentBugByUrl(url);
                                string workItemType = (string)json["fields"]["System.WorkItemType"];
                                string existedParentBugTitle = (string)json["fields"]["System.Title"];
                                if (workItemType == "Bug")
                                {
                                    parentBugUrl = url;
                                    int parentBugId = (int)json["id"];
                                    label_existingParentBugTitle.Text = existedParentBugTitle;
                                    label_parentBugUrl.Visible = hyperlink_parentBugUrl.Visible = true;
                                    hyperlink_parentBugUrl.Text = hyperlink_parentBugUrl.NavigateUrl = context.GetBugUrl(parentBugId);
                                }
                            }
                        }
                    }

                    //parent bug Exists
                    if (parentBugUrl != "")
                    {
                        label_existing.Visible = label_existingParentBugTitle.Visible = true;
                        newBugUrl = parentBugUrl;
                    }
                    //parent bug does not Exist
                    else
                    {
                        radiobuttonlist_parentBug.Visible = label_new.Visible = radtextbox_newParentBugTitle.Visible = true;
                        //if sample bug's language is [Non Language-Specific]
                        if (label_sampleBugLanguage.Text == "[Non Language-Specific]")
                        {
                            radiobuttonlist_parentBug.SelectedIndex = 1;
                            radtextbox_newParentBugTitle.Enabled = false;
                            label_parentBugUrl.Visible = hyperlink_parentBugUrl.Visible = true;
                            hyperlink_parentBugUrl.Text = hyperlink_parentBugUrl.NavigateUrl = context.GetBugUrl((int)radtextbox_bugID.Value.Value);
                        }
                        else
                        {
                            radiobuttonlist_parentBug.SelectedIndex = 0;
                        }

                        radtextbox_newParentBugTitle.Text = title.Replace("[lang]", "[Non Language-Specific]");
                    }
                    RadTabStrip_ImageLoader.Enabled = true;
                }
                catch (Exception ex)
                {
                    this.label_errorMsg.Visible = true;
                }
            }
        }

        private VsoContext GetVsoContext()
        {
            string key = WebConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoAccount = WebConfigurationManager.AppSettings["VsoRootAccount"];
            return new VsoContext(vsoAccount, key);
        }

        private void ValidatePage()
        {
            this.Validate();
            //this.label_languagesNotSelected.Visible = !this.radlistbox_languages.CheckedItems.Any();
            //string templateTitle = radtextbox_templateTitle.Text;
            //this.label_templateTitleWrong.Visible = !templateTitle.Contains("[lang]");
            //this.label_generalErrorMsg.Visible = (label_errorMsg.Visible || this.label_languagesNotSelected.Visible || this.label_templateTitleWrong.Visible);
        }

        private bool IsPageValid()
        {
            //force the validator controls to validate
            this.Validate();
            return this.IsValid && this.radlistbox_languages.CheckedItems.Any() && !label_templateTitleWrong.Visible;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.radgrid_lang.Visible = this.label_gridCaption.Visible = false;
            btnSubmit.Enabled = true;
            this.label_submitSuccess.Visible = false;
            this.label_submitFail.Visible = false;
            //validate the page
            this.ValidatePage();
            //if (this.IsPageValid())
            {
                string childBugsassignedTo = "";
                if (radautocompletebox_childBugsAssignedTo.Text != "")
                {
                    childBugsassignedTo = radautocompletebox_childBugsAssignedTo.Entries[0].Attributes["testerName"] + " <" + radautocompletebox_childBugsAssignedTo.Entries[0].Value + ">";
                }
                else
                    childBugsassignedTo = assignedTo;

                //create bugs using the sample bug
                var context = this.GetVsoContext();

                //create new parentBug if needed
                string newParentBugTitle = "";
                bool ifCreateNewParentBug = false;
                if (label_new.Visible)
                {
                    newParentBugTitle = radtextbox_newParentBugTitle.Text;
                    int parentBugId = 0;

                    //Use  new parent bug as parent
                    if (radiobuttonlist_parentBug.SelectedIndex == 0)
                        ifCreateNewParentBug = true;
                    else
                    {
                        parentBugId = (int)this.radtextbox_bugID.Value.Value;
                        var jsonSampleBug = context.GetListOfWorkItemsByIDs(new int[] { parentBugId });
                        newBugUrl = (string)jsonSampleBug["value"][0]["url"];
                    }

                    //hyperlink for parentBug Url
                    label_parentBugUrl.Visible = hyperlink_parentBugUrl.Visible = true;
                    hyperlink_parentBugUrl.Text = hyperlink_parentBugUrl.NavigateUrl = context.GetBugUrl(parentBugId);
                }

                Action<List<Dictionary<string, object>>> prepareFunction = (fields) =>
                {
                    if (!string.IsNullOrWhiteSpace(state))
                    {
                        var f_state = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.State" }, { "value", state } };
                        fields.Add(f_state);
                    }
                    if (!string.IsNullOrWhiteSpace(priority))
                    {
                        var f_priority = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Priority" }, { "value", priority } };
                        fields.Add(f_priority);
                    }
                    if (!string.IsNullOrWhiteSpace(severity))
                    {
                        var f_severity = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Common.Severity" }, { "value", severity } };
                        fields.Add(f_severity);
                    }
                    if (!string.IsNullOrWhiteSpace(issueType))
                    {
                        var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueType" }, { "value", issueType } };
                        fields.Add(f_issueType);
                    }
                    if (!string.IsNullOrEmpty(issueSubType))
                    {
                        var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueSubtype" }, { "value", issueSubType } };
                        fields.Add(f_issueType);
                    }
                    if (!string.IsNullOrWhiteSpace(triaged))
                    {
                        var f_triaged = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Triaged" }, { "value", triaged } };
                        fields.Add(f_triaged);
                    }
                    if (!string.IsNullOrWhiteSpace(keywords))
                    {
                        var f_keywords = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Keywords" }, { "value", keywords } };
                        fields.Add(f_keywords);
                    }
                    if (!string.IsNullOrWhiteSpace(howFound))
                    {
                        var f_howFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.HowFound" }, { "value", howFound } };
                        fields.Add(f_howFound);
                    }
                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        var f_source = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Source" }, { "value", source } };
                        fields.Add(f_source);
                    }
                    if (!string.IsNullOrWhiteSpace(fixProvidedBy))
                    {
                        var f_fixProvidedBy = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixProvidedBy" }, { "value", fixProvidedBy } };
                        fields.Add(f_fixProvidedBy);
                    }
                    if (!string.IsNullOrWhiteSpace(rootCauseCategory))
                    {
                        var f_rootCauseCategory = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.RootCauseCategory" }, { "value", rootCauseCategory } };
                        fields.Add(f_rootCauseCategory);
                    }
                    if (!string.IsNullOrWhiteSpace(branchFound))
                    {
                        var f_branchFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BranchFound" }, { "value", branchFound } };
                        fields.Add(f_branchFound);
                    }
                    if (!string.IsNullOrWhiteSpace(buildFound))
                    {
                        var f_buildFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BuildFound" }, { "value", buildFound } };
                        fields.Add(f_buildFound);
                    }
                    if (!string.IsNullOrWhiteSpace(fixedBuild))
                    {
                        var f_fixedBuild = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixedBuild" }, { "value", fixedBuild } };
                        fields.Add(f_fixedBuild);
                    }
                    if (!string.IsNullOrWhiteSpace(description))
                    {
                        var f_description = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", description } };
                        fields.Add(f_description);
                    }
                    if (!string.IsNullOrWhiteSpace(reproSteps))
                    {
                        var f_reproSteps = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.ReproSteps" }, { "value", reproSteps } };
                        fields.Add(f_reproSteps);
                    }
                    if (!string.IsNullOrWhiteSpace(systemInfo))
                    {
                        var f_systemInfo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.SystemInfo" }, { "value", systemInfo } };
                        fields.Add(f_systemInfo);
                    }
                    if (!string.IsNullOrWhiteSpace(dueDate))
                    {
                        var f_dueDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.DueDate" }, { "value", dueDate } };
                        fields.Add(f_dueDate);
                    }
                    if (!string.IsNullOrWhiteSpace(tags))
                    {
                        var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags } };
                        fields.Add(f_tags);
                    }
                };
                VsoContextService vsoService = new VsoContextService(context);
                string errorMSg = null;
                if (RadTabStrip_ImageLoader.SelectedIndex == 0)
                {
                    errorMSg = CreateBugFromListLanguage(vsoService, ifCreateNewParentBug, childBugsassignedTo, "[Non Language-Specific]", newParentBugTitle, newBugUrl, prepareFunction, totalRetry);
                }
                else
                {
                    errorMSg = CreateBugFromZip(vsoService, ifCreateNewParentBug, childBugsassignedTo, "[Non Language-Specific]", newParentBugTitle, newBugUrl, prepareFunction, totalRetry);
                }
                if (errorMSg != null)
                {
                    this.label_submitFail.Visible = true;
                    this.label_submitFail.Text = errorMSg;
                }
                btnSubmit.Enabled = RadTabStrip_ImageLoader.Enabled = RadMultiPageImageLoader.Enabled = RadAutoCompleteBox_Language.Enabled = AsyncUploadZip.Enabled = radtextbox_bugID.Enabled = false;
            }
        }

        private Dictionary<String, ZipCultureInfo> SetStatusOfFiles(Dictionary<String, ZipCultureInfo> zipInfo, HashSet<string> culture, int flag)
        {
            foreach (var pair in zipInfo)
            {
                string lang = pair.Key;
                if (culture.Contains(lang))
                {
                    foreach (var pair2 in pair.Value.files)
                    {
                        zipInfo[lang].files[pair2.Key].uploadVsoStatus = flag;
                    }
                }
            }
            return zipInfo;
        }

        private string CreateBugFromZip(VsoContextService vsoService, bool ifCreateNewParentBug, string childBugsassignedTo, string parentBugLanguage, string newParentBugTitle, string parentBugUrl, Action<List<Dictionary<string, object>>> action, int totalRetry)
        {
            string errorMsg = null;
            string fileName = lbUploadedZip.Text;
            String filePath = WebConfigurationManager.AppSettings["FileSharedUploadTempPath"];
            Dictionary<String, ZipCultureInfo> zipInfo = ReadZipFromDisk(filePath, fileName);
            if (zipInfo != null && zipInfo.Count > 0)
            {
                List<LanguageAndTitle> list_childLangAndTile = new List<LanguageAndTitle>();
                HashSet<string> langsAllowed = new HashSet<string>(Utils.GetLanguages());
                langsAllowed.IntersectWith(radlistbox_languages_new.CheckedItems.Select(c => c.Text));

                foreach (var language in langsAllowed)
                {
                    //Replace [lang] in title with language name from each language
                    string templateTitle = radtextbox_templateTitle.Text;

                    string childBugTitle = templateTitle.Replace("[lang]", language);
                    list_childLangAndTile.Add(new LanguageAndTitle { Language = language, Title = childBugTitle });
                }

                try
                {
                    //1.create all children bug in a batch
                    var dict_LanguageToBugID = new Dictionary<string, int>();
                    if (ifCreateNewParentBug)
                        dict_LanguageToBugID = vsoService.CreateNewParentItemAndChildItemsInBatch
                            (parentBugLanguage, newParentBugTitle, list_childLangAndTile, "LOCALIZATION", LinkTypes.Child, TaskTypes.Bug, areaPath, iterationPath, childBugsassignedTo, prepareFunction: action, totalRetry: totalRetry);
                    else
                        dict_LanguageToBugID = vsoService.CreateChildItemsForExistingParentItemInBatch(list_childLangAndTile, "LOCALIZATION", LinkTypes.Child, parentBugUrl, TaskTypes.Bug, areaPath, iterationPath, childBugsassignedTo, prepareFunction: action, totalRetry: totalRetry);

                    //add vso link
                    Dictionary<string, Dictionary<string, Stream>> dict_LanguageToFileStream = new Dictionary<string, Dictionary<string, Stream>>();

                    foreach (var language in langsAllowed)
                    {
                        Dictionary<string, Stream> dictAttachments = new Dictionary<string, Stream>();

                        ZipCultureInfo cultureInfo = zipInfo[language];
                        //Cache the files that needed to uploaded
                        List<ZipFileInfo> fileInfo = cultureInfo.files.Values.ToList();
                        foreach (var file in fileInfo)
                        {
                            dictAttachments.Add(file.path, file.stream);
                        }
                        dict_LanguageToFileStream[language] = dictAttachments;
                        if (dictAttachments.Any() && dict_LanguageToBugID.ContainsKey(language))
                        {
                            int bugID = dict_LanguageToBugID[language];
                            string bugUrl = vsoService.context.GetBugUrl(bugID);
                            cultureInfo.vsoLinks = bugUrl;
                        }
                        zipInfo[language] = cultureInfo;
                    }

                    //2. update all children bug with attachment in one batch api call
                    var source = new Dictionary<int, Dictionary<string, Stream>>();
                    foreach (var item in dict_LanguageToFileStream)
                    {
                        if (dict_LanguageToBugID.ContainsKey(item.Key))
                            source.Add(dict_LanguageToBugID[item.Key], item.Value);
                    }
                    vsoService.UploadAttachmentToVsoWorkItemInBatch(source, totalThread, totalRetry);
                    label_languagesNotSelected.Visible = label_templateTitleWrong.Visible = label_generalErrorMsg.Visible = false;
                    label_submitSuccess.Visible = true;
                    label_submitSuccess.Text = "Bugs have been created! Total languages checked: " + zipInfo.Count;
                    DeleteFileFromDisk(filePath, fileName);
                    zipInfo = SetStatusOfFiles(zipInfo, langsAllowed, 1);
                }
                catch (Exception ex)
                {
                    zipInfo = SetStatusOfFiles(zipInfo, langsAllowed, 2);
                    errorMsg = ex.ToString();
                }
                finally
                {
                    this.PopulateLnaguegeList(zipInfo, true);
                }
            }
            return errorMsg;
        }

        private string CreateBugFromListLanguage(VsoContextService vsoService, bool ifCreateNewParentBug, string childBugsassignedTo, string parentBugLanguage, string newParentBugTitle, string parentBugUrl, Action<List<Dictionary<string, object>>> action, int totalRetry)
        {
            string errorMsg = null;
            List<CheckedLang> listCheckedLang = new List<CheckedLang>();
            Dictionary<string, Dictionary<string, Stream>> dict_LanguageToFileStream = new Dictionary<string, Dictionary<string, Stream>>();
            List<LanguageAndTitle> list_childLangAndTile = new List<LanguageAndTitle>();

            for (int i = 0; i < radlistbox_languages.CheckedItems.Count; i++)
            {
                CheckedLang checkedLang = new CheckedLang();

                string language = radlistbox_languages.CheckedItems[i].Text;
                Dictionary<string, Stream> dictAttachments = new Dictionary<string, Stream>();

                var uploadcontrol = radlistbox_languages.CheckedItems[i].FindControl("AsyncUploadManully") as RadAsyncUpload;
                UploadedFileCollection files = uploadcontrol.UploadedFiles;
                string fileNamesForGrid = "";
                if (files.Count > 0)
                {
                    foreach (UploadedFile file in files)
                    {
                        string fileName = file.FileName;
                        fileNamesForGrid += fileName + "<br/>";
                        Stream fileStream = file.InputStream;
                        dictAttachments.Add(fileName, fileStream);
                    }
                }
                checkedLang.LangName = language;
                checkedLang.Attachments = fileNamesForGrid;
                listCheckedLang.Add(checkedLang);

                dict_LanguageToFileStream.Add(language, dictAttachments);

                //Replace [lang] in title with language name from each language
                string templateTitle = radtextbox_templateTitle.Text;

                string childBugTitle = templateTitle.Replace("[lang]", language);
                list_childLangAndTile.Add(new LanguageAndTitle { Language = language, Title = childBugTitle });
            }

            try
            {
                //1.create all children bug in on batch api call
                var dict_LanguageToBugID = new Dictionary<string, int>();
                if (ifCreateNewParentBug)
                    dict_LanguageToBugID = vsoService.CreateNewParentItemAndChildItemsInBatch
                        (parentBugLanguage, newParentBugTitle, list_childLangAndTile, "LOCALIZATION", LinkTypes.Child, TaskTypes.Bug, areaPath, iterationPath, childBugsassignedTo, prepareFunction: action, totalRetry: totalRetry);
                else
                    dict_LanguageToBugID = vsoService.CreateChildItemsForExistingParentItemInBatch(list_childLangAndTile, "LOCALIZATION", LinkTypes.Child, parentBugUrl, TaskTypes.Bug, areaPath, iterationPath, childBugsassignedTo, prepareFunction: action, totalRetry: totalRetry);

                //2. update all children bug with attachment in one batch api call
                var source = new Dictionary<int, Dictionary<string, Stream>>();
                foreach (var item in dict_LanguageToFileStream)
                {
                    if (dict_LanguageToBugID.ContainsKey(item.Key))
                        source.Add(dict_LanguageToBugID[item.Key], item.Value);
                }

                vsoService.UploadAttachmentToVsoWorkItemInBatch(source, totalThread, totalRetry);

                label_languagesNotSelected.Visible = label_templateTitleWrong.Visible = label_generalErrorMsg.Visible = false;
                label_submitSuccess.Visible = true;
                label_submitSuccess.Text = "Bugs have been created! Total languages checked: " + radlistbox_languages.CheckedItems.Count;

                //display submitted infos
                this.radgrid_lang.Visible = this.label_gridCaption.Visible = true;
                radgrid_lang.DataSource = listCheckedLang;
                radgrid_lang.DataBind();
            }
            catch (Exception e)
            {
                errorMsg = e.ToString();
            }
            return errorMsg;
        }

        private static JObject CreateParentBug(string childBugsassignedTo, VsoContext context, string newParentBugTitle)
        {
            return context.CreateVsoWorkItem(TaskTypes.Bug, VSO_PROJECT, newParentBugTitle, areaPath, iterationPath, childBugsassignedTo, null, (fields) =>
            {
                var f_laguage = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Language" }, { "value", "[Non Language-Specific]" } };
                fields.Add(f_laguage);
                if (!string.IsNullOrWhiteSpace(state))
                {
                    var f_state = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.State" }, { "value", state } };
                    fields.Add(f_state);
                }
                if (!string.IsNullOrWhiteSpace(priority))
                {
                    var f_priority = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Priority" }, { "value", priority } };
                    fields.Add(f_priority);
                }
                if (!string.IsNullOrWhiteSpace(severity))
                {
                    var f_severity = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Common.Severity" }, { "value", severity } };
                    fields.Add(f_severity);
                }
                if (!string.IsNullOrWhiteSpace(issueType))
                {
                    var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueType" }, { "value", issueType } };
                    fields.Add(f_issueType);
                }
                if (!string.IsNullOrEmpty(issueSubType))
                {
                    var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueSubtype" }, { "value", issueSubType } };
                    fields.Add(f_issueType);
                }
                if (!string.IsNullOrWhiteSpace(triaged))
                {
                    var f_triaged = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Triaged" }, { "value", triaged } };
                    fields.Add(f_triaged);
                }
                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    var f_keywords = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Keywords" }, { "value", keywords } };
                    fields.Add(f_keywords);
                }
                if (!string.IsNullOrWhiteSpace(howFound))
                {
                    var f_howFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.HowFound" }, { "value", howFound } };
                    fields.Add(f_howFound);
                }
                if (!string.IsNullOrWhiteSpace(source))
                {
                    var f_source = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Source" }, { "value", source } };
                    fields.Add(f_source);
                }
                if (!string.IsNullOrWhiteSpace(fixProvidedBy))
                {
                    var f_fixProvidedBy = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixProvidedBy" }, { "value", fixProvidedBy } };
                    fields.Add(f_fixProvidedBy);
                }
                if (!string.IsNullOrWhiteSpace(rootCauseCategory))
                {
                    var f_rootCauseCategory = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.RootCauseCategory" }, { "value", rootCauseCategory } };
                    fields.Add(f_rootCauseCategory);
                }
                if (!string.IsNullOrWhiteSpace(branchFound))
                {
                    var f_branchFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BranchFound" }, { "value", branchFound } };
                    fields.Add(f_branchFound);
                }
                if (!string.IsNullOrWhiteSpace(buildFound))
                {
                    var f_buildFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BuildFound" }, { "value", buildFound } };
                    fields.Add(f_buildFound);
                }
                if (!string.IsNullOrWhiteSpace(fixedBuild))
                {
                    var f_fixedBuild = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixedBuild" }, { "value", fixedBuild } };
                    fields.Add(f_fixedBuild);
                }
                if (!string.IsNullOrWhiteSpace(description))
                {
                    var f_description = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", description } };
                    fields.Add(f_description);
                }
                if (!string.IsNullOrWhiteSpace(reproSteps))
                {
                    var f_reproSteps = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.ReproSteps" }, { "value", reproSteps } };
                    fields.Add(f_reproSteps);
                }
                if (!string.IsNullOrWhiteSpace(systemInfo))
                {
                    var f_systemInfo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.SystemInfo" }, { "value", systemInfo } };
                    fields.Add(f_systemInfo);
                }
                if (!string.IsNullOrWhiteSpace(dueDate))
                {
                    var f_dueDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.DueDate" }, { "value", dueDate } };
                    fields.Add(f_dueDate);
                }
                if (!string.IsNullOrWhiteSpace(tags))
                {
                    var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags } };
                    fields.Add(f_tags);
                }
            });
        }

        private static JObject CreateChildBug(string childBugsassignedTo, VsoContext context, string language, string childBugTitle, string parentUrl)
        {
            return context.CreateVsoWorkItem(
                                        TaskTypes.Bug, VSO_PROJECT, childBugTitle, areaPath, iterationPath, childBugsassignedTo, parentUrl, LinkTypes.Child, null, (fields) =>
                                        {
                                            if (!string.IsNullOrWhiteSpace(language))
                                            {
                                                var f_laguage = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Language" }, { "value", language } };
                                                fields.Add(f_laguage);
                                            }
                                            if (!string.IsNullOrWhiteSpace(state))
                                            {
                                                var f_state = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.State" }, { "value", state } };
                                                fields.Add(f_state);
                                            }
                                            if (!string.IsNullOrWhiteSpace(priority))
                                            {
                                                var f_priority = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Priority" }, { "value", priority } };
                                                fields.Add(f_priority);
                                            }
                                            if (!string.IsNullOrWhiteSpace(severity))
                                            {
                                                var f_severity = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Common.Severity" }, { "value", severity } };
                                                fields.Add(f_severity);
                                            }
                                            if (!string.IsNullOrWhiteSpace(issueType))
                                            {
                                                var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueType" }, { "value", issueType } };
                                                fields.Add(f_issueType);
                                            }
                                            if (!string.IsNullOrEmpty(issueSubType))
                                            {
                                                var f_issueType = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.IssueSubtype" }, { "value", issueSubType } };
                                                fields.Add(f_issueType);
                                            }
                                            if (!string.IsNullOrWhiteSpace(triaged))
                                            {
                                                var f_triaged = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Triaged" }, { "value", triaged } };
                                                fields.Add(f_triaged);
                                            }
                                            if (!string.IsNullOrWhiteSpace(keywords))
                                            {
                                                var f_keywords = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Keywords" }, { "value", keywords } };
                                                fields.Add(f_keywords);
                                            }
                                            if (!string.IsNullOrWhiteSpace(howFound))
                                            {
                                                var f_howFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.HowFound" }, { "value", howFound } };
                                                fields.Add(f_howFound);
                                            }
                                            if (!string.IsNullOrWhiteSpace(source))
                                            {
                                                var f_source = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Source" }, { "value", source } };
                                                fields.Add(f_source);
                                            }
                                            if (!string.IsNullOrWhiteSpace(fixProvidedBy))
                                            {
                                                var f_fixProvidedBy = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixProvidedBy" }, { "value", fixProvidedBy } };
                                                fields.Add(f_fixProvidedBy);
                                            }
                                            if (!string.IsNullOrWhiteSpace(rootCauseCategory))
                                            {
                                                var f_rootCauseCategory = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.RootCauseCategory" }, { "value", rootCauseCategory } };
                                                fields.Add(f_rootCauseCategory);
                                            }
                                            if (!string.IsNullOrWhiteSpace(branchFound))
                                            {
                                                var f_branchFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BranchFound" }, { "value", branchFound } };
                                                fields.Add(f_branchFound);
                                            }
                                            if (!string.IsNullOrWhiteSpace(buildFound))
                                            {
                                                var f_buildFound = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.BuildFound" }, { "value", buildFound } };
                                                fields.Add(f_buildFound);
                                            }
                                            if (!string.IsNullOrWhiteSpace(fixedBuild))
                                            {
                                                var f_fixedBuild = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FixedBuild" }, { "value", fixedBuild } };
                                                fields.Add(f_fixedBuild);
                                            }
                                            if (!string.IsNullOrWhiteSpace(description))
                                            {
                                                var f_description = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Description" }, { "value", description } };
                                                fields.Add(f_description);
                                            }
                                            if (!string.IsNullOrWhiteSpace(reproSteps))
                                            {
                                                var f_reproSteps = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.ReproSteps" }, { "value", reproSteps } };
                                                fields.Add(f_reproSteps);
                                            }
                                            if (!string.IsNullOrWhiteSpace(systemInfo))
                                            {
                                                var f_systemInfo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.TCM.SystemInfo" }, { "value", systemInfo } };
                                                fields.Add(f_systemInfo);
                                            }
                                            if (!string.IsNullOrWhiteSpace(dueDate))
                                            {
                                                var f_dueDate = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Microsoft.VSTS.Scheduling.DueDate" }, { "value", dueDate } };
                                                fields.Add(f_dueDate);
                                            }
                                            if (!string.IsNullOrWhiteSpace(tags))
                                            {
                                                var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags } };
                                                fields.Add(f_tags);
                                            }
                                        }
                                    );
        }

        [WebMethod]
        public static AutoCompleteBoxData search_Language(object context)
        {
            string searchItem = ((Dictionary<string, object>)context)["Text"].ToString();
            var languages = Utils.GetLanguages();
            List<string> list_names = languages.Where(s => s.ToLower().Contains(searchItem.ToLower())).ToList();
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            foreach (var name in list_names)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();

                childNode.Text = name;
                childNode.Value = name;
                result.Add(childNode);
            }
            AutoCompleteBoxData res = new AutoCompleteBoxData();

            res.Items = result.ToArray();

            return res;
        }

        [WebMethod]
        public static AutoCompleteBoxData search_testerNames(object context)
        {
            string searchItem = ((Dictionary<string, object>)context)["Text"].ToString();

            string key = WebConfigurationManager.AppSettings["VsoPrivateKey"];
            string vsoAccount = WebConfigurationManager.AppSettings["VsoRootAccount"];
            var contextVso = new VsoContext(vsoAccount, key);

            List<Identity> list_names = contextVso.SearchIdentity(searchItem);
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();
            foreach (var name in list_names)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData();

                childNode.Text = name.DisplayName + " (" + name.UniqueName.Split(new char[] { '@' }).First() + ")";
                childNode.Value = name.UniqueName;
                childNode.Attributes.Add("testerName", name.DisplayName);
                result.Add(childNode);
            }
            AutoCompleteBoxData res = new AutoCompleteBoxData();

            res.Items = result.ToArray();

            return res;
        }

        protected Dictionary<string, ZipCultureInfo> ProcessZipFromFile(string zipfilePath)
        {
            Dictionary<string, ZipCultureInfo> zipInfo = new Dictionary<string, ZipCultureInfo>();
            try
            {
                using (ZipFile zip = ZipFile.Read(zipfilePath))
                {
                    foreach (ZipEntry entry in zip)
                    {
                        var filePath = entry.FileName;
                        int idx = filePath.IndexOf("/");
                        string culture = "";
                        if (idx != -1)
                        {
                            culture = filePath.Substring(idx + 1);
                            idx = culture.IndexOf('/');
                            if (idx != -1)
                            {
                                culture = culture.Substring(0, idx);
                                if (zipInfo.ContainsKey(culture) == false)
                                {
                                    ZipCultureInfo cultureInfo = new ZipCultureInfo();
                                    cultureInfo.language = culture;
                                    cultureInfo.vsoLinks = "";
                                    cultureInfo.isSupported = false;
                                    zipInfo[culture] = cultureInfo;
                                }
                            }
                        }
                        if (filePath.Contains(".png"))
                        {
                            int idx2 = filePath.LastIndexOf("/");
                            if (idx2 != -1)
                            {
                                string fileName = filePath.Substring(idx2 + 1);

                                MemoryStream ms = new MemoryStream();
                                entry.Extract(ms);
                                ms.Position = 0;
                                ZipFileInfo fileInfo = new ZipFileInfo();
                                fileInfo.path = fileName;
                                fileInfo.stream = ms;
                                fileInfo.uploadVsoStatus = 0;
                                if (zipInfo.ContainsKey(culture) == true)
                                {
                                    zipInfo[culture].files.Add(fileName, fileInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return CheckLanguageIsSupported(zipInfo);
        }

        protected Dictionary<string, ZipCultureInfo> ProcessZipFromMemoryAndSave(Stream stream, string zipfilePath, string zipfileName)
        {
            Dictionary<string, ZipCultureInfo> zipInfo = new Dictionary<string, ZipCultureInfo>();
            try
            {
                using (ZipFile zip = ZipFile.Read(stream))
                {
                    if (Directory.Exists(zipfilePath) == false)
                    {
                        Directory.CreateDirectory(zipfilePath);
                    }
                    zip.Save(zipfilePath + zipfileName);
                    foreach (ZipEntry entry in zip)
                    {
                        var filePath = entry.FileName;
                        int idx = filePath.IndexOf("/");
                        string culture = "";
                        if (idx != -1)
                        {
                            culture = filePath.Substring(idx + 1);
                            idx = culture.IndexOf('/');
                            if (idx != -1)
                            {
                                culture = culture.Substring(0, idx);
                                if (zipInfo.ContainsKey(culture) == false)
                                {
                                    ZipCultureInfo cultureInfo = new ZipCultureInfo();
                                    cultureInfo.language = culture;
                                    cultureInfo.vsoLinks = "";
                                    cultureInfo.isSupported = false;
                                    zipInfo[culture] = cultureInfo;
                                }
                            }
                        }
                        if (filePath.Contains(".png"))
                        {
                            int idx2 = filePath.LastIndexOf("/");
                            if (idx2 != -1)
                            {
                                string fileName = filePath.Substring(idx2 + 1);

                                MemoryStream ms = new MemoryStream();
                                entry.Extract(ms);
                                ms.Position = 0;
                                ZipFileInfo fileInfo = new ZipFileInfo();
                                fileInfo.path = fileName;
                                fileInfo.stream = ms;
                                fileInfo.uploadVsoStatus = 0;
                                if (zipInfo.ContainsKey(culture) == true)
                                {
                                    zipInfo[culture].files.Add(fileName, fileInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return CheckLanguageIsSupported(zipInfo);
        }

        protected Dictionary<string, ZipCultureInfo> ReadZipFromDisk(string filePath, string fileName)
        {
            if (Directory.Exists(filePath))
            {
                Dictionary<string, ZipCultureInfo> zipInfo = ProcessZipFromFile(filePath + fileName);
                return zipInfo;
            }
            return null;
        }

        private void DeleteFileFromDisk(string filePath, string fileName)
        {
            if (Directory.Exists(filePath))
            {
                if (System.IO.File.Exists(filePath + fileName))
                {
                    // Use a try block to catch IOExceptions, to
                    // handle the case of the file already being
                    // opened by another process.
                    try
                    {
                        System.IO.File.Delete(filePath + fileName);
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return;
                    }
                }
            }
        }

        private Dictionary<string, ZipCultureInfo> CheckLanguageIsSupported(Dictionary<string, ZipCultureInfo> zipInfo)
        {
            HashSet<string> langsAllowed = new HashSet<string>(Utils.GetLanguages());
            langsAllowed.IntersectWith(zipInfo.Keys.ToList());
            foreach (var lang in langsAllowed)
            {
                zipInfo[lang].isSupported = true;
            }
            return zipInfo;
        }

        private void PopulateLnaguegeList(Dictionary<string, ZipCultureInfo> zipInfo, bool updateVsoInfo)
        {
            //populate listbox of new languages based on ZipInfo
            int totalLanguage = 0;
            int totalUploadedLanguage = 0;
            int totalFiles = 0;
            int totalUploadedFiles = 0;
            int totalUnSupportedLanguage = 0;
            if (zipInfo != null)
            {
                List<string> cultureList = zipInfo.Keys.ToList();
                if (updateVsoInfo == false)
                {
                    for (int i = 0; i < cultureList.Count; i++)
                    {
                        RadListBoxItem cultureItem = new RadListBoxItem(cultureList[i]);
                        cultureItem.Checked = true;
                        this.radlistbox_languages_new.Items.Add(cultureItem);
                        string lang = cultureList[i];
                        List<ZipFileInfo> filesInfoList = zipInfo[lang].files.Values.ToList();
                        RadListBox radlistbox_files_list = this.radlistbox_languages_new.Items[i].FindControl("radlistbox_files_list") as RadListBox;
                        for (int j = 0; j < filesInfoList.Count; j++)
                        {
                            RadListBoxItem fileItem = new RadListBoxItem();
                            radlistbox_files_list.Items.Add(fileItem);
                        }
                    }
                }

                for (int i = 0; i < cultureList.Count; i++)
                {
                    string lang = cultureList[i];
                    this.radlistbox_languages_new.Items[i].Checked = true;
                    Label radlistbox_languagesLabel = this.radlistbox_languages_new.Items[i].FindControl("languageLabel") as Label;
                    ZipCultureInfo cultureInfo = zipInfo[lang];
                    //Culture Text (Culture (0 Images) -red if not supported)
                    radlistbox_languagesLabel.Text = String.Format("{0} ({1} images)", lang, cultureInfo.files.Count);
                    if (cultureInfo.isSupported == false)
                    {
                        radlistbox_languagesLabel.ForeColor = Color.Red;
                        this.radlistbox_languages_new.Items[i].Checked = false;
                        totalUnSupportedLanguage++;
                    }
                    //VSOLink after culture text
                    if (!String.IsNullOrEmpty(cultureInfo.vsoLinks) && cultureInfo.isSupported)
                    {
                        HyperLink hyperLink = this.radlistbox_languages_new.Items[i].FindControl("vsoLink") as HyperLink;
                        hyperLink.NavigateUrl = cultureInfo.vsoLinks;
                        hyperLink.Visible = true;
                    }
                    //populate the files list
                    if (cultureInfo.files.Count > 0)
                    {
                        var radlistbox_files_list = this.radlistbox_languages_new.Items[i].FindControl("radlistbox_files_list") as RadListBox;
                        if (cultureInfo.isSupported == false)
                        {
                            radlistbox_files_list.Enabled = false;
                        }
                        int totalFilesUploadedInCulture = 0;
                        List<ZipFileInfo> filesInfoList = cultureInfo.files.Values.ToList();
                        for (int j = 0; j < filesInfoList.Count; j++)
                        {
                            Label fileInfo = radlistbox_files_list.Items[j].FindControl("fileName") as Label;
                            fileInfo.Text = filesInfoList[j].path;
                            System.Web.UI.WebControls.Image statusImage = radlistbox_files_list.Items[j].FindControl("statusImage") as System.Web.UI.WebControls.Image;
                            totalFiles++;
                            switch (filesInfoList[j].uploadVsoStatus)
                            {
                                case 0:
                                    statusImage.ImageUrl = "~/images/b_icon.png";
                                    break;

                                case 1:
                                    statusImage.ImageUrl = "~/images/g_icon.png";
                                    totalUploadedFiles++;
                                    totalFilesUploadedInCulture++;
                                    break;

                                case 2:
                                    statusImage.ImageUrl = "~/images/r_icon.png";
                                    break;
                            }
                        }
                        if (totalFilesUploadedInCulture > 0)
                        {
                            totalUploadedLanguage++;
                        }
                    }
                    totalLanguage = zipInfo.Keys.Count;
                }

                lbZipInfo.Text = String.Format("Total Cultures Uploaded:{0}/{1}</br> Total Files Uploaded:{2}/{3} </br>Total Culture Not Supported:{4}",
                    totalUploadedLanguage, totalLanguage, totalUploadedFiles, totalFiles, totalUnSupportedLanguage);
                //if (updateVsoInfo)
                //{
                //    lbVsoUploadedInfo.Text = String.Format("Total Cultures Uploaded:{0}. Total Files Uploaded:{1}", totalUploadedVsoCulture, totalUploadedVsoFiles);
                //}
            }
        }

        protected void btnDummy_Click(object sender, EventArgs e)
        {
            this.radlistbox_languages_new.Items.Clear();
            label_submitSuccess.Visible = label_submitFail.Visible = label_gridCaption.Visible = radgrid_lang.Visible = false;
            var files = AsyncUploadZip.UploadedFiles;
            if (files.Count > 0)
            {
                UploadedFile file = files[0];
                Stream stream = file.InputStream;
                string fileName = Guid.NewGuid().ToString() + ".zip";
                String filePath = WebConfigurationManager.AppSettings["FileSharedUploadTempPath"];
                Dictionary<string, ZipCultureInfo> zipInfo = ProcessZipFromMemoryAndSave(stream, filePath, fileName);

                lbUploadedZip.Text = fileName;
                this.PopulateLnaguegeList(zipInfo, false);
            }
        }

        protected void RadAutoCompleteBox_Language_EntryAdded(object sender, AutoCompleteEntryEventArgs e)
        {
            label_submitSuccess.Visible = label_submitFail.Visible = false;
            var language = e.Entry.Text;
            var item = radlistbox_languages.FindItemByValue(language);
            item.Checked = true;
        }

        protected void RadAutoCompleteBox_Language_EntryRemoved(object sender, AutoCompleteEntryEventArgs e)
        {
            label_submitSuccess.Visible = label_submitFail.Visible = false;
            var language = e.Entry.Text;
            var item = radlistbox_languages.FindItemByValue(language);
            item.Checked = false;
        }
    }
}
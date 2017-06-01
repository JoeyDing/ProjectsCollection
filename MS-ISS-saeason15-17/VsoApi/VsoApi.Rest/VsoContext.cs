using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace VsoApi.Rest
{
    public class VsoContext
    {
        private readonly string _privateKey;
        public readonly string VsoRootAccount;

        public readonly string VsoUrl;

        public VsoContext(string vsoRootAccount, string privateAuthenticationKey)
        {
            this._privateKey = privateAuthenticationKey;
            this.VsoRootAccount = vsoRootAccount;
            this.VsoUrl = string.Format("https://{0}.visualstudio.com", vsoRootAccount);
        }

        #region Create

        public Dictionary<string, int> CreateChildItemsForExistingParentItemInBatch(List<LanguageAndTitle> languageAndTitles, string projectName, LinkType linkType, string referenceWorkItemUrl, TaskType type, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            JObject json = this.ProcessVsoWorkItemInBatch((c) =>
            {
                int counter = -2;
                foreach (var item in languageAndTitles)
                {
                    string language = item.Language;
                    string title = item.Title;
                    var body = new List<Dictionary<string, object>>();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        var f_title = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", title } };
                        body.Add(f_title);
                    }

                    if (!string.IsNullOrWhiteSpace(areaPath))
                    {
                        var f_areaPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AreaPath" }, { "value", areaPath } };
                        body.Add(f_areaPath);
                    }
                    if (!string.IsNullOrWhiteSpace(iterationPath))
                    {
                        var f_iterationPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } };
                        body.Add(f_iterationPath);
                    }

                    if (!string.IsNullOrWhiteSpace(assignedTo))
                    {
                        var f_assignedTo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AssignedTo" }, { "value", assignedTo } };
                        body.Add(f_assignedTo);
                    }

                    if (tags != null && tags.Any())
                    {
                        var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags.Aggregate((a, b) => a + ";" + b) } };
                        body.Add(f_tags);
                    }
                    if (!string.IsNullOrWhiteSpace(language))
                    {
                        var f_laguage = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Language" }, { "value", language } };
                        body.Add(f_laguage);
                    }

                    if (prepareFunction != null)
                        prepareFunction(body);

                    body.Add(new Dictionary<string, object> { { "op", "add" }, { "path", "/id" }, { "value", counter } });
                    counter--;

                    var f_relation = new Dictionary<string, object>() { { "op", "add" }, { "path", "/relations/-" },
                    { "value", new Dictionary<string,object>{{ "rel", linkType.Value}, { "url", referenceWorkItemUrl}} }};
                    body.Add(f_relation);
                    var field = new Dictionary<string, object>()
                                          {
                                                { "method","PATCH"},
                                                { "uri",string.Format("/{0}/_apis/wit/workitems/${1}?api-version=1.0", projectName, type.Value)},
                                                {
                                                    "headers",new Dictionary<string,string>()
                                                    {
                                                        { "Content-Type","application/json-patch+json" }
                                                    }
                                                },
                                                {
                                                    "body",body
                                                }
                                          };
                    c.Add(field);
                }
            });
            var result = new Dictionary<string, int>();
            json = JsonConvert.DeserializeObject(ReplaceIlegalCharsInJson(json.ToString())) as JObject;
            foreach (var item in json["value"])
            {
                string language = item["body"]["fields"]["Skype.Language"].ToString();
                int id = int.Parse(item["body"]["id"].ToString());
                result[language] = id;
            }

            return result;
        }

        public string ReplaceIlegalCharsInJson(string jsonString)
        {
            return jsonString.Replace("\\\"", "\"").Replace("\\\\\"", "\\\"").Replace("\"{", "{").Replace("}\"", "}");
        }

        public Dictionary<string, int> CreateNewParentItemAndChildItemsInBatch(string parentLanguage, string parentBugTitle, List<LanguageAndTitle> languageAndTitles, string projectName, LinkType linkType, TaskType type, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            JObject json = this.ProcessVsoWorkItemInBatch((c) =>
            {
                //create parent
                List<Dictionary<string, object>> parentBody = LoadBody(areaPath, iterationPath, assignedTo, tags, prepareFunction, parentLanguage, parentBugTitle);
                parentBody.Add(new Dictionary<string, object> { { "op", "add" }, { "path", "/id" }, { "value", -1 } });
                var parentField = new Dictionary<string, object>()
                                          {
                                                { "method","PATCH"},
                                                { "uri",string.Format("/{0}/_apis/wit/workitems/${1}?api-version=1.0", projectName, type.Value)},
                                                {
                                                    "headers",new Dictionary<string,string>()
                                                    {
                                                        { "Content-Type","application/json-patch+json" }
                                                    }
                                                },
                                                {
                                                    "body",parentBody
                                                }
                                          };
                c.Add(parentField);

                string referenceWorkItemUrl = string.Format("{0}/DefaultCollection/_apis/wit/workItems/-1", this.VsoUrl);
                int counter = -2;
                foreach (var item in languageAndTitles)
                {
                    string language = item.Language;
                    string title = item.Title;
                    List<Dictionary<string, object>> body = LoadBody(areaPath, iterationPath, assignedTo, tags, prepareFunction, language, title);

                    body.Add(new Dictionary<string, object> { { "op", "add" }, { "path", "/id" }, { "value", counter } });
                    counter--;

                    var f_relation = new Dictionary<string, object>() { { "op", "add" }, { "path", "/relations/-" },
                    { "value", new Dictionary<string,object>{{ "rel", linkType.Value}, { "url", referenceWorkItemUrl}} }};
                    body.Add(f_relation);
                    var field = new Dictionary<string, object>()
                                          {
                                                { "method","PATCH"},
                                                { "uri",string.Format("/{0}/_apis/wit/workitems/${1}?api-version=1.0", projectName, type.Value)},
                                                {
                                                    "headers",new Dictionary<string,string>()
                                                    {
                                                        { "Content-Type","application/json-patch+json" }
                                                    }
                                                },
                                                {
                                                    "body",body
                                                }
                                          };
                    c.Add(field);
                }
            });
            var result = new Dictionary<string, int>();
            json = JsonConvert.DeserializeObject(ReplaceIlegalCharsInJson(json.ToString())) as JObject;
            foreach (var item in json["value"])
            {
                string language = item["body"]["fields"]["Skype.Language"].ToString();
                int id = int.Parse(item["body"]["id"].ToString());
                result[language] = id;
            }

            return result;
        }

        private static List<Dictionary<string, object>> LoadBody(string areaPath, string iterationPath, string assignedTo, string[] tags, Action<List<Dictionary<string, object>>> prepareFunction, string language, string title)
        {
            var body = new List<Dictionary<string, object>>();
            if (!string.IsNullOrWhiteSpace(title))
            {
                var f_title = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", title } };
                body.Add(f_title);
            }

            if (!string.IsNullOrWhiteSpace(areaPath))
            {
                var f_areaPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AreaPath" }, { "value", areaPath } };
                body.Add(f_areaPath);
            }
            if (!string.IsNullOrWhiteSpace(iterationPath))
            {
                var f_iterationPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } };
                body.Add(f_iterationPath);
            }

            if (!string.IsNullOrWhiteSpace(assignedTo))
            {
                var f_assignedTo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AssignedTo" }, { "value", assignedTo } };
                body.Add(f_assignedTo);
            }

            if (tags != null && tags.Any())
            {
                var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags.Aggregate((a, b) => a + ";" + b) } };
                body.Add(f_tags);
            }
            if (!string.IsNullOrWhiteSpace(language))
            {
                var f_laguage = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Language" }, { "value", language } };
                body.Add(f_laguage);
            }

            if (prepareFunction != null)
                prepareFunction(body);
            return body;
        }

        /// <summary>
        /// Create a VSO work item
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectName"></param>
        /// <param name="title"></param>
        /// <param name="areaPath"></param>
        /// <param name="iterationPath"></param>
        /// <param name="assignedTo"></param>
        /// <param name="prepareFunction"></param>
        /// <returns></returns>
        public async Task<JObject> CreateVsoWorkItemAsync(TaskType type, string projectName, string title, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null, CancellationToken? token = null)
        {
            var fields = new List<Dictionary<string, object>>();

            var f_title = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", title } };
            fields.Add(f_title);

            if (!string.IsNullOrWhiteSpace(areaPath))
            {
                var f_areaPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AreaPath" }, { "value", areaPath } };
                fields.Add(f_areaPath);
            }
            if (!string.IsNullOrWhiteSpace(iterationPath))
            {
                var f_iterationPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } };
                fields.Add(f_iterationPath);
            }

            if (!string.IsNullOrWhiteSpace(assignedTo))
            {
                var f_assignedTo = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AssignedTo" }, { "value", assignedTo } };
                fields.Add(f_assignedTo);
            }

            if (tags != null && tags.Any())
            {
                var f_tags = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Tags" }, { "value", tags.Aggregate((a, b) => a + ";" + b) } };
                fields.Add(f_tags);
            }

            if (prepareFunction != null)
                prepareFunction(fields);
            var jsonString = JsonConvert.SerializeObject(fields);

            var content = new StringContent(jsonString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json-patch+json");
            var query = await this.ExecutePatchRequest("application/json", string.Format("{0}/{1}/{2}", this.VsoUrl,
                string.Format("{0}/{1}", "DefaultCollection", projectName), string.Format("{0}{1}{2}", "_apis/wit/workitems/$", type.Value, "?api-version=1.0")), content, token);
            var json = JObject.Parse(query);
            return json;
        }

        public JObject CreateVsoWorkItem(TaskType type, string projectName, string title, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null, CancellationToken? token = null)
        {
            return CreateVsoWorkItemAsync(type, projectName, title, areaPath, iterationPath, assignedTo, tags, prepareFunction, token).Result;
        }

        public async Task<int> CreateBugItemForLeaf(string resourceID, string fabricProject, string fileName, string keywords, string question, CancellationToken token)
        {
            string areaPath = "";
            string iterationPath = "";

            var result = await CreateVsoWorkItemAsync(TaskTypes.Bug, @"LOCALIZATION", question, areaPath, iterationPath, "",
                prepareFunction: (fields) =>
                 {
                     var f_resourceID = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.ResourceID" }, { "value", resourceID } };
                     var f_FaricProject = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FabricProject" }, { "value", fabricProject } };
                     var f_FileName = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.FileName" }, { "value", fileName } };
                     var f_Keywords = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/Skype.Keywords" }, { "value", keywords } };
                     fields.Add(f_resourceID);
                     fields.Add(f_FaricProject);
                     fields.Add(f_FileName);
                     fields.Add(f_Keywords);
                 }, token: token);
            int bugID = int.Parse(result["id"].ToString());
            return bugID;
        }

        public async Task<JObject> AnswerBugItemForLeaf(int bugID, string answer, CancellationToken token)
        {
            var f_history = new Dictionary<string, string>() { { "System.History", answer } };
            return await UpdateVsoWorkItemAsync(bugID, new Dictionary<string, string>() { { "System.History", answer } }, token: token);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="planID"></param>
        /// <param name="planUrl"></param>
        /// <returns></returns>
        public string GetTestPlanUrl(int planID)
        {
            //"https://skype-test2.visualstudio.com/DefaultCollection/LOCALIZATION/Skype for Business%20Clients/_testManagement#planId=285373"
            //var result = this.GetListOfTestSuitesByPlanID("LOCALIZATION", planID);
            //var subItems = result["value"];
            //string planUrl = (string)subItems[0]["plan"]["url"];
            //var json = JObject.Parse(this.ExecuteGetRequest("application/json", planUrl).Result);
            var result = this.GetTestPlanByPlanID("LOCALIZATION", planID);
            string areaName = (string)result["project"]["name"];

            string url = string.Format("{0}/DefaultCollection/{1}/_testManagement#planId={2}", this.VsoUrl, areaName, planID);
            return url;
        }

        public string GetBugUrl(int bugId)
        {
            string projectName = "LOCALIZATION";
            string url = string.Format("{0}/DefaultCollection/{1}/_workitems#id={2}&_a=edit", this.VsoUrl, projectName, bugId);
            return url;
        }

        /// <summary>
        /// Create a VSO Work Item with a link
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectName"></param>
        /// <param name="title"></param>
        /// <param name="areaPath"></param>
        /// <param name="iterationPath"></param>
        /// <param name="assignedTo"></param>
        /// <param name="referenceWorkItemUrl"></param>
        /// <param name="linkType"></param>
        /// <param name="prepareFunction"></param>
        /// <returns></returns>
        public JObject CreateVsoWorkItem(TaskType type, string projectName, string title, string areaPath, string iterationPath, string assignedTo, string referenceWorkItemUrl, LinkType linkType, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            return this.CreateVsoWorkItem(type, projectName, title, areaPath, iterationPath, assignedTo, tags, prepareFunction: (fields) =>
            {
                var f_relation = new Dictionary<string, object>() { { "op", "add" }, { "path", "/relations/-" },
            { "value", new Dictionary<string,object>{{ "rel", linkType.Value}, { "url", referenceWorkItemUrl}} }};
                fields.Add(f_relation);

                if (prepareFunction != null)
                    prepareFunction(fields);
            });
        }

        /// <summary>
        /// Create a test suite
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <param name="parentSuite"></param>
        /// <returns></returns>
        public String CreateTestSuite(string project, string name, string type, int planId, int parentSuiteID)
        {
            var query = string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}?api-version=1.0", this.VsoUrl, project, planId, parentSuiteID);

            string jsonContent = string.Format("{{ \"suiteType\": \"StaticTestSuite\", \"name\": \"{0}\", \"plan\": \"{1}\" }}", name, planId);
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            var result = this.ExecutePostRequest("application/json", query, httpContent);

            var json = JObject.Parse(result.Result);
            var valueItems = json["value"];
            String suiteId = null;

            foreach (var item in valueItems.Children())
            {
                suiteId = (String)item["id"];
            }

            return suiteId != null ? suiteId : null;
        }

        #endregion Create

        #region Add

        /// <summary>
        /// Add test cases for test suite
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="plan"></param>
        /// <param name="parentSuite
        /// <returns></returns>
        public JObject AddTestCases(string project, int planId, String suiteId, List<int> casesIds)
        {
            StringBuilder builder = new StringBuilder();
            var jresult = new JObject();
            var jArray = new JArray();
            jresult.Add("value", jArray);
            //
            int itemsPerPage = 20;
            int totalItems = casesIds.Count;
            int totalPages = totalItems / itemsPerPage;
            int remaining = totalItems % itemsPerPage;
            int remaningPages = (remaining == 0) ? 0 : 1;
            totalPages = totalPages + remaningPages;

            string smallCasesIdString = "";
            List<int> smallcaseIds = new List<int>();
            for (int i = 0; i < totalPages; i++)
            {
                builder.Clear();
                smallcaseIds = casesIds.Skip(i * itemsPerPage).Take(itemsPerPage).ToList();
                foreach (int id in smallcaseIds)
                {
                    builder.Append(id).Append(",");
                }

                smallCasesIdString = builder.ToString();

                //
                var query = string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}/testcases/{4}?api-version=1.0", this.VsoUrl, project, planId, suiteId, smallCasesIdString);

                var stringContent = new StringContent("{}");
                stringContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                var result = this.ExecutePostRequest("application/json", query, stringContent);

                JObject json = JObject.Parse(result.Result);
                if (json["value"] != null)
                {
                    foreach (var item in json["value"])
                    {
                        jArray.Add(item);
                    }
                }
            }
            //
            return jresult;
        }

        #endregion Add

        #region Get

        /// <summary>
        ///
        /// </summary>
        /// <param name="repoID"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public LatestBuildInfo GetLatestBuildInfoAsAnObject(int buildId)
        {
            var content = GetLatestBuildInfoAsString(buildId);
            var fs = this.GetMemorySteam(content);
            var lastestBuild = Deserialize<LatestBuildInfo>(fs);

            return lastestBuild;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="buildId"></param>
        /// <returns></returns>
        public string GetLatestBuildInfoAsString(int buildId)
        {
            string httpQuery = string.Format(@"https://quickbuild.skype.net/rest/latest_builds/{0}", buildId);
            var content = this.ExecuteAuthorizedGetRequest(httpQuery);
            return content;
        }

        public string GetBuildInfoAsString(int buildId)
        {
            string httpQuery = string.Format(@"https://quickbuild.skype.net/rest/latest_builds?parent_configuration_id={0}&recursive=false", buildId);
            var content = this.ExecuteAuthorizedGetRequest(httpQuery);
            return content;
        }

        public static T Deserialize<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result = (T)serializer.Deserialize(stream);

            return result;
        }

        /// <summary>
        /// Use ID in query URL to get real vso sql query
        /// </summary>
        /// <returns></returns>
        public string GetQueryTextByQueryID(string queryID)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/properties/{1}", this.VsoUrl, queryID));
            string result = "";
            try
            {
                var json = JObject.Parse(query.Result);

                if (json["value"] != null)
                {
                    result = (string)json["value"]["queryText"];
                }
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetGitZipFolder(string repoID, string folderPath, string branchName = "master")
        {
            string httpQuery = string.Format("{0}/DefaultCollection/_apis/git/repositories/{1}/items?api-version=1.0&scopepath={2}&versionType=branch&version={3}", this.VsoUrl, repoID, folderPath, branchName);
            var query = this.ExecuteGetStreamRequest("application/zip", httpQuery);
            var fs = new MemoryStream(query.Result);

            return fs;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public JObject GetGitRepoByRepoName(string projectName, string repoName)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/git/repositories/{2}/?api-version=1.0", this.VsoUrl, projectName, repoName));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public async Task<List<Tuple<string, DateTime>>> GetWorkItemCommentAsync(int workItemID, CancellationToken token)
        {
            var query = await this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems/{1}/updates?api-version=1.0", this.VsoUrl, workItemID), token: token);
            var json = JObject.Parse(query);
            List<Tuple<string, DateTime>> result = new List<Tuple<string, DateTime>>();
            foreach (var item in json["value"])
            {
                if (item["fields"] != null && item["fields"]["System.History"] != null)
                {
                    string comment = item["fields"]["System.History"]["newValue"] != null ? item["fields"]["System.History"]["newValue"].ToString() : "";
                    DateTime changedDate = Convert.ToDateTime(item["fields"]["System.ChangedDate"]["newValue"].ToString());

                    result.Add(new Tuple<string, DateTime>(comment, changedDate));
                }
            }

            return result;
        }

        public JObject GetGitRepoByRepoNameForTeamSpce(string projectName, string repoName)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/git/repositories/{2}/?api-version=1.0", "https://domoreexp.visualstudio.com", projectName, repoName));
            var json = JObject.Parse(query.Result);
            return json;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public MemoryStream GetGitRepoFileByRepoIDAndFilePath(string repoID, string filePath, string branchName = "master")
        {
            string httpQuery = string.Format("{0}/DefaultCollection/_apis/git/repositories/{1}/items?api-version=1.0&scopepath={2}&versionType=branch&version={3}", this.VsoUrl, repoID, filePath, branchName);
            var query = this.ExecuteGetStreamRequest("application/octet-stream", httpQuery);
            var fs = new MemoryStream(query.Result);
            return fs;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private MemoryStream GetMemorySteam(string strData)
        {
            MemoryStream memStream = new MemoryStream();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strData);
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;
            return memStream;
        }

        public List<string> GetGitRepoFilePathsByFolderPath(string repoID, string folderPath, string branchName = "master", bool includeMetataData = true)
        {
            var result = new List<string>();
            string httpQuery = string.Format("{0}/DefaultCollection/_apis/git/repositories/{1}/items?api-version=1.0&scopepath={2}&recursionLevel=Full&query=md&includeContentMetadata={3}&versionType=branch&version={4}", this.VsoUrl, repoID, folderPath, includeMetataData ? "true" : "false", branchName);
            var query = this.ExecuteGetRequest("application/json", httpQuery);
            var json = JObject.Parse(query.Result);
            foreach (var item in json["value"])
            {
                if (item["path"] != null && Path.HasExtension(item["path"].ToString()) && !item["path"].ToString().Contains(".git"))
                {
                    string path = item["path"].ToString();
                    result.Add(path);
                }
            }
            return result;
        }

        /// <summary>
        /// Get the list of projects that exist under the VSO account
        /// </summary>
        /// <returns></returns>
        public List<Project> GetProjects()
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/{1}", this.VsoUrl, "DefaultCollection/_apis/projects?api-version=1.0"));
            var json = JObject.Parse(query.Result);
            var result = new List<Project>();

            foreach (var item in json["value"].Select(t => new
            {
                Description = (string)t["description"],
                ID = (string)t["id"],
                Name = (string)t["name"],
                ApiUrl = (string)t["url"],
            }).ToList())
            {
                var proj = new Project();
                proj.Description = item.Description;
                proj.ID = item.ID;
                proj.Name = item.Name;
                proj.ApiUrl = item.ApiUrl;
                proj.WebUrl = string.Format("{0}/{1}/{2}", this.VsoUrl, "DefaultCollection", proj.Name);

                result.Add(proj);
            }

            return result;
        }

        /// <summary>
        /// Get the list of sub teams that exists under a VSO project
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public List<Team> GetProjectTeams(string projectID, string projectName)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/{1}/{2}/{3}", this.VsoUrl, "DefaultCollection/_apis/projects", projectID, "teams?api-version=1.0"));
            var json = JObject.Parse(query.Result);
            var result = new List<Team>();

            foreach (var item in json["value"].Select(t => new
            {
                Description = (string)t["description"],
                ID = (string)t["id"],
                Name = (string)t["name"],
                ApiUrl = (string)t["url"],
            }).ToList())
            {
                var team = new Team();
                team.Description = item.Description;
                team.ID = item.ID;
                team.Name = item.Name;
                team.ApiUrl = item.ApiUrl;
                team.WebUrl = string.Format("{0}/{1}/{2}/{3}", this.VsoUrl, "DefaultCollection", projectName, team.Name);

                result.Add(team);
            }

            return result;
        }

        /// <summary>
        /// Search for a valid user in VSO using a search term
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<Identity> SearchIdentity(string searchTerm)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/{1}{2}", this.VsoUrl, "DefaultCollection/_api/_wit/searchIdentities?&searchTerm=", searchTerm));
            var json = JObject.Parse(query.Result);
            var result = new List<Identity>();

            foreach (var item in json["__wrappedArray"].Select(t => new
            {
                ID = (string)t["id"],
                UniqueName = (string)t["uniqueName"],
                DisplayName = (string)t["displayName"],
            }).ToList())
            {
                var identity = new Identity();
                identity.ID = item.ID;
                identity.UniqueName = item.UniqueName;
                identity.DisplayName = item.DisplayName;
                identity.AssignedTo = item.DisplayName + " <" + item.UniqueName + ">";

                result.Add(identity);
            }

            return result;
        }

        /// <summary>
        /// Run WIQL query and get work items ids and url
        /// </summary>
        /// <param name="project"></param>
        /// <param name="wiql"></param>
        /// <returns>a dictionary of ids/URL</returns>
        public Dictionary<int, string> RunQuery(string project, string wiql, CancellationToken? token = null)
        {
            return RunQueryAsync(project, wiql, token).Result;
        }

        public async Task<Dictionary<int, string>> RunQueryAsync(string project, string wiql, CancellationToken? token = null)
        {
            string jsonString = JsonConvert.SerializeObject(new { query = wiql });
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
            string url = string.Format("{0}/DefaultCollection/{1}_apis/wit/wiql?api-version=1.0", this.VsoUrl, !string.IsNullOrWhiteSpace(project) ? project + '/' : "");
            var request = await this.ExecutePostRequest("application/json", url, content, token: token);

            var result = new Dictionary<int, string>();
            JObject json = JObject.Parse(request);
            var workItems = json["workItems"];
            if (workItems != null && workItems.Any())
            {
                foreach (var item in workItems)
                {
                    result.Add(int.Parse((string)item["id"]), (string)item["url"]);
                }
            }

            return result;
        }

        public async Task<List<BugItemFromVSO>> GetBugItemForLeaf(string resourceID, string fabricProject, string fileName, string keywords, CancellationToken token)
        {
            List<BugItemFromVSO> result = new List<BugItemFromVSO>();
            //string wiql = string.Format(@"SELECT [System.Id],[System.WorkItemType],[System.Title],[System.State],[System.AreaPath],[System.IterationPath],[System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Bug' AND [Skype.ResourceID] = '{0}' AND [Skype.FabricProject] = '{1}' AND [Skype.FileName] = '{2}' AND [Skype.Keywords] = '{3}' ORDER BY [System.ChangedDate] DESC", resourceID, fabricProject, fileName, keywords);
            string wiql = string.Format(@"SELECT [System.Id] FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Bug' AND [Skype.ResourceID] = '{0}' AND [Skype.FabricProject] = '{1}' AND [Skype.FileName] = '{2}' AND [Skype.Keywords] = '{3}' ORDER BY [System.ChangedDate] DESC", resourceID, fabricProject, fileName, keywords);
            Dictionary<int, string> bugItems = await RunQueryAsync("LOCALIZATION", wiql, token);
            if (bugItems.Any())
            {
                List<int> bugIds = bugItems.Keys.ToList();
                var fields = new string[] { "System.Title", "System.State" };
                JObject json = await GetListOfWorkItemsByIDsAsync(bugIds, fields: fields, token: token);
                foreach (var item in json["value"])
                {
                    int bugId = int.Parse(item["id"].ToString());
                    if (item["fields"] != null)
                    {
                        string title = item["fields"]["System.Title"] != null ? item["fields"]["System.Title"].ToString() : "";
                        List<Tuple<string, DateTime>> comments = await GetWorkItemCommentAsync(bugId, token);
                        string bugState = item["fields"]["System.State"] != null ? item["fields"]["System.State"].ToString() : "";
                        result.Add(new BugItemFromVSO { BugID = bugId, BugState = bugState, Title = title, Comments = comments });
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Run WIQL query and get work items ids and url
        /// </summary>
        /// <param name="project"></param>
        /// <param name="wiql"></param>
        /// <returns>a dictionary of ids/URL</returns>
        public JObject RunComplexQuery(string project, string wiql)
        {
            string jsonString = JsonConvert.SerializeObject(new { query = wiql, removeCommonTeamProject = true });
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
            string url = string.Format("{0}/DefaultCollection/{1}_apis/wit/wiql?api-version=1.0", this.VsoUrl, !string.IsNullOrWhiteSpace(project) ? project + '/' : "");
            var request = this.ExecutePostRequest("application/json", url, content);

            JObject json = JObject.Parse(request.Result);

            return json;
        }

        public JObject RunCrossProjectQuery(string project, string wiql)
        {
            //string jsonString = JsonConvert.SerializeObject(new { query = wiql, removeCommonTeamProject = true });

            // This is the postdata
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("wiql", wiql));
            postData.Add(new KeyValuePair<string, string>("removeCommonTeamProject ", "true"));

            HttpContent content = new FormUrlEncodedContent(postData);

            string url = string.Format("{0}/DefaultCollection/{1}_api/_wit/query?__v=5", this.VsoUrl, !string.IsNullOrWhiteSpace(project) ? project + '/' : "");
            var request = this.ExecutePostRequest("application/json", url, content);

            JObject json = JObject.Parse(request.Result);

            return json;
        }

        /// <summary>
        /// Get a list of work items by IDs
        /// </summary>
        /// <param name="ids">A comma-separated list of up to 200 IDs of the work items to get.</param>
        /// <param name="fields">A comma-separated list of up to 100 fields to get with each work item.If not specified, all fields are returned.</param>
        /// <returns></returns>
        public async Task<JObject> GetListOfWorkItemsByIDsAsync(IEnumerable<int> ids, IEnumerable<string> fields = null, CancellationToken? token = null)
        {
            string query = null;
            if (fields != null && fields.Any())
            {
                query = await this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems?ids={1}&fields={2}&api-version=1.0", this.VsoUrl, ids.Select(i => i.ToString()).Aggregate((a, b) => a + "," + b), fields.Aggregate((a, b) => a + "," + b)), token: token);
            }
            else
            {
                query = await this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems?ids={1}&$expand=all&api-version=1.0", this.VsoUrl, ids.Select(i => i.ToString()).Aggregate((a, b) => a + "," + b)), token: token);
            }

            var json = JObject.Parse(query);
            return json;
        }

        public JObject GetListOfWorkItemsByIDs(IEnumerable<int> ids, IEnumerable<string> fields = null, CancellationToken? token = null)
        {
            return GetListOfWorkItemsByIDsAsync(ids, fields, token).Result;
        }

        public Dictionary<string, string> GetAttachmentsUrls(IEnumerable<int> ids)
        {
            var json = GetListOfWorkItemsByIDs(ids);
            var result = new Dictionary<string, string>();
            foreach (var value in json["value"])
            {
                foreach (var relation in value["relations"])
                {
                    if (relation["rel"] != null && relation["rel"].ToString() == "AttachedFile")
                    {
                        string attachmentName = relation["attributes"]["name"].ToString();
                        string url = relation["url"].ToString();
                        result.Add(attachmentName, url);
                    }
                }
            }

            return result;
        }

        public List<string> DownloadAttachments(IEnumerable<int> ids, string destFolderPath)
        {
            if (!Directory.Exists(destFolderPath))
                Directory.CreateDirectory(destFolderPath);

            var dict = GetAttachmentsUrls(ids);
            var result = new List<string>();
            foreach (var item in dict)
            {
                string fileName = item.Key;
                string url = item.Value;

                var query = this.ExecuteGetStreamRequest("application/zip", url);

                var data = query.Result;

                string destFilePath = Path.Combine(destFolderPath, fileName);
                File.WriteAllBytes(destFilePath, data);

                result.Add(fileName);
            }
            return result;
        }

        ///<summary>
        ///Get WorkitemRevision by Id and rev
        ///</summary>
        public JObject GetWorkItemRevByIdAndRev(int id, int rev)
        {
            Task<string> query = null;
            query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems/{1}/revisions/{2}?api-version=1.0", this.VsoUrl, id, rev));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public JObject GetWorkItemRevById(int id)
        {
            Task<string> query = null;
            query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems/{1}/revisions?api-version=1.0", this.VsoUrl, id));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public Dictionary<int, string> GetDictRevToTagsByID(int id)
        {
            var result = new Dictionary<int, string>();
            Task<string> query = null;
            query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/wit/workitems/{1}/revisions?api-version=1.0", this.VsoUrl, id));
            var json = JObject.Parse(query.Result);
            foreach (var item in json["value"])
            {
                int rev = (int)item["rev"];
                string tags = item["fields"]["System.Tags"] == null ? "" : item["fields"]["System.Tags"].ToString();
                result.Add(rev, tags);
            }
            return result;
        }

        /// <summary>
        /// Get all areas located under a project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="level">Depth level of areas to query</param>
        /// <returns></returns>
        public JObject GetAreas(string projectName, int level)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/wit/classificationNodes/areas?$depth={2}&api-version=1.0", this.VsoUrl, projectName, level));
            var json = JObject.Parse(query.Result);
            return json;
        }

        /// <summary>
        /// Get all iterations located under a project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="level">Depth level of areas to query</param>
        /// <returns></returns>
        public JObject GetIterations(string projectName, int level)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/wit/classificationNodes/iterations?$depth={2}&api-version=1.0", this.VsoUrl, projectName, level));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public JObject GetParentBugByUrl(string url)
        {
            var query = this.ExecuteGetRequest("application/json", url);
            var json = JObject.Parse(query.Result);
            return json;
        }

        /// <summary>
        /// Get a list of Work Items for the specified area path
        /// </summary>
        /// <param name="type"></param>
        /// <param name="project"></param>
        /// <param name="fields"></param>
        /// <param name="fetchChildren"></param>
        /// <returns></returns>
        public JObject GetWorkItemsUnderAreaPath(TaskType type, string project, string areaPath, string[] fields, bool fetchChildren = false)
        {
            JObject result = null;

            //get list of ids
            var query = string.Format("SELECT [System.Id] FROM WorkItems WHERE [System.WorkItemType] = '{0}' and [System.AreaPath] under '{1}'", type.Value, areaPath);
            var ids = this.RunQuery(project, query);

            if (ids.Any())
            {
                //retrieve interesting items from ids
                if (fields != null && fetchChildren == false)
                {
                    result = this.GetListOfWorkItemsByIDs(ids.Keys, fields);
                }
                else
                {
                    result = this.GetListOfWorkItemsByIDs(ids.Keys);
                    var items = result["value"];
                    if (items != null && items.Any())
                    {
                        //get parentId/childrenId of tasks that has children
                        var parentChild = new Dictionary<int, List<int>>();
                        foreach (var item in items.Where(c => c["relations"] != null && c["relations"].Any()))
                        {
                            var childrenIds = new List<int>();
                            foreach (var childItem in item["relations"])
                            {
                                if ((string)childItem["rel"] == "System.LinkTypes.Hierarchy-Forward")
                                {
                                    var url = (string)childItem["url"];
                                    int id = int.Parse(url.Split(new char[] { '/' }).Last());
                                    childrenIds.Add(id);
                                }
                            }
                            if (childrenIds.Any())
                            {
                                int parentId = int.Parse((string)item["id"]);
                                parentChild.Add(parentId, childrenIds);
                            }
                        }

                        var childResult = this.GetListOfWorkItemsByIDs(parentChild.SelectMany(c => c.Value).Distinct())["value"].ToDictionary(c => (int)c["id"], c => c);

                        foreach (JObject item in result["value"])
                        {
                            var currentId = (int)item["id"];
                            if (parentChild.ContainsKey(currentId))
                            {
                                var array = new JArray();
                                foreach (int childId in parentChild[currentId])
                                {
                                    var jChild = childResult[childId];
                                    array.Add(jChild);
                                }
                                item.Add("hasChildren", true);
                                item.Add("children", array);
                            }
                            else
                            {
                                item.Add("hasChildren", false);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generate a custom query that can be open in the browser
        /// </summary>
        /// <param name="wiql"></param>
        /// <param name="project"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public string GenerateCustomQueryUrl(string wiql, string project, string team = null)
        {
            string encodedWiql = HttpUtility.UrlEncode(wiql);
            string url;
            if (!string.IsNullOrWhiteSpace(team))
                url = string.Format("{0}/DefaultCollection/{1}/{2}/_workitems#path=Custom+query&wiql={3}&name=Custom+query&_a=query", this.VsoUrl, project, team, encodedWiql);
            else
                url = string.Format("{0}/DefaultCollection/{1}/_workitems#path=Custom+query&wiql={2}&name=Custom+query&_a=query", this.VsoUrl, project, encodedWiql);

            return url;
        }

        //work but prefer above solution
        private string GenerateCustomQueryUrl_V2(string wiql, string project, string team = null)
        {
            string url;
            if (!string.IsNullOrWhiteSpace(team))
                url = string.Format("{0}/DefaultCollection/{1}/{2}/_workitems#_a=query&wiql={3}", this.VsoUrl, project, team, wiql);
            else
                url = string.Format("{0}/DefaultCollection/{1}/_workitems#_a=query&wiql={2}", this.VsoUrl, project, wiql);

            return url;
        }

        /// <summary>
        /// Get team default area
        /// </summary>
        /// <param name="project"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public string GetProjectTeamDefaultArea(string project, string team)
        {
            string url = string.Format("{0}/DefaultCollection/{1}/{2}/_apis/work/teamsettings/teamfieldvalues", this.VsoUrl, project, team);
            var json = JObject.Parse(this.ExecuteGetRequest("application/json", url).Result);
            var defaultArea = json["defaultValue"];
            return defaultArea != null ? defaultArea.ToString() : null;
        }

        /// <summary>
        /// Get test plan that exists under a VSO project
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public JObject GetTestPlanByPlanID(string projectName, int planID)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}?api-version=1.0", this.VsoUrl, projectName, planID));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public void RefreshTestPlanURL(string projectName, int planID)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_testManagement?planid={2}", this.VsoUrl, projectName, planID));
        }

        /// <summary>
        /// Get a list of test suites under test plan.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="plan"></param>
        /// <returns></returns>
        public JObject GetListOfTestSuitesByPlanID(string projectName, int planID)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites?api-version=1.0", this.VsoUrl, projectName, planID));
            var json = JObject.Parse(query.Result);
            return json;
        }

        public JObject GetTestPlanAndTestSuiteByTestCaseId(string projectName, int testCaseId)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/_apis/test/suites?testCaseId={2}&api-version=2.0-preview", this.VsoUrl, projectName, testCaseId));
            var json = JObject.Parse(query.Result);
            return json;
        }

        /// <summary>
        /// Get a list of test cases case under suite.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="plan"></param>
        /// <param name="suite"></param>
        /// <returns></returns>
        public Dictionary<int, List<int>> GetTestCaseIDsBySuiteID(string projectName, int planID, int suiteID)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}?api-version=1.0", this.VsoUrl, projectName, planID, suiteID));
            JObject json = JObject.Parse(query.Result);

            String testCasesUrl = json["testCasesUrl"].ToString();
            var result = new Dictionary<int, List<int>>();
            var parent = json["parent"];
            if (parent != null)
            {
                int parentId = (int)parent["id"];

                query = this.ExecuteGetRequest("application/json", string.Format("{0}", testCasesUrl));
                json = JObject.Parse(query.Result);
                var listId = new List<int>();

                var valueItems = json["value"];

                foreach (var item in valueItems.Children())
                {
                    JProperty details = item.First.Value<JProperty>();

                    var reference = details.Name;
                    JObject propertyList = (JObject)item[reference];

                    int value = (int)propertyList["id"];
                    listId.Add(value);
                }
                result.Add(parentId, listId);
            }

            return result;
        }

        public JObject GetTestRunResultByPlanID(string projectName, int planID, int timeOut = 6)
        {
            var query = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/runs?api-version=1.0&planid={2}&includerundetails=true", this.VsoUrl, projectName, planID), timeOut);
            JObject json = JObject.Parse(query.Result);

            //get the latest completeDate
            //var valueItems = json["value"];
            //Dictionary<DateTime, int> dict = new Dictionary<DateTime, int>();
            //DateTime completeDate = new DateTime();
            //foreach (var item in valueItems.Children())
            //{
            //    if (item["completedDate"] != null)
            //    {
            //        DateTime temp = (DateTime)item["completedDate"];
            //        int runId = (int)item["id"];
            //        if (DateTime.Compare(temp, completeDate) > 0)
            //        {
            //            completeDate = temp;
            //            dict.Add(completeDate, runId);
            //        }
            //    }
            //}
            //int latestRunId = dict[completeDate];

            return json;
        }

        public JObject GetTestRunDetailByRunId(string projectName, int runId, int timeOut = 6)
        {
            var query2 = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/runs/{2}/results?api-version=1.0", this.VsoUrl, projectName, runId), timeOut);
            JObject json2 = JObject.Parse(query2.Result);
            return json2;
        }

        public JObject GetAllTestRun(string projectName, int skipCount = 0, int stopCount = 10000000)
        {
            string testRunquery = "{\"query\": \"Select * From TestRun Where [completed Date] > '2015-06-25T02:10:13.04Z' \"}";
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/test/runs/query?$top={2}&$skip={3}&api-version=2.0-preview", this.VsoUrl, projectName, stopCount, skipCount);
            var httpContent = new StringContent(testRunquery);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);

            return json;
        }

        public JObject GetTestRunByitemsPerBatchAndSleepTime(string projectName, int itemsPerBatch = 100, int sleepTime = 1)
        {
            int startPoint = 0;
            int i = 0;

            var result = new JObject();
            var jArray = new JArray();
            result.Add("value", jArray);

            JObject json = null;
            do
            {
                string testRunquery = "{\"query\": \"Select * From TestRun \"}";
                string url = string.Format("{0}/DefaultCollection/{1}/_apis/test/runs/query?$top={2}&$skip={3}&api-version=2.0-preview", this.VsoUrl, projectName, itemsPerBatch, startPoint);
                var httpContent = new StringContent(testRunquery);
                httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);
                System.Threading.Thread.Sleep(sleepTime * 1000);
                i++;
                if (startPoint == 0)
                {
                    startPoint = itemsPerBatch;
                }
                startPoint = startPoint * i;
                //
                jArray.Add(json);
            }
            while ((int)json["count"] == itemsPerBatch);
            return result;
        }

        public JObject GetCoreAndLocBugStatus(string projectName)
        {
            string testRunquery = "{\"query\": \"select [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State], [System.Tags] from WorkItemLinks where (Source.[System.WorkItemType] = 'Bug' and Source.[System.State] <> '' and not Source.[System.IterationPath] under 'LOCALIZATION') and ([System.Links.LinkType] = 'System.LinkTypes.Related-Forward') and (Target.[System.WorkItemType] = 'Bug' and Target.[System.IterationPath] under 'LOCALIZATION') order by [System.Title], [System.Id] mode (MustContain)\"}";
            string url = string.Format("{0}/DefaultCollection/_apis/wit/wiql?api-version=1.0", this.VsoUrl, projectName);

            var httpContent = new StringContent(testRunquery);

            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);

            return json;
        }

        #region Reporting

        /// <summary>
        /// Get All bugs revisions
        /// </summary>
        /// <param name="project"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public JObject Reporting_GetAllBugsRevisions(string project, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            return this.Reporting_GetAllWorkItemRevisions(project, TaskTypes.Bug, fields, pauseTimeBetweenBatchInSec);
        }

        /// <summary>
        /// Get all work item revisions for a specific type of item
        /// </summary>
        /// <param name="project"></param>
        /// <param name="type"></param>
        /// <param name="fields"></param>
        /// <param name="pauseTimeBetweenBatch">Pause during each batch request In second</param>
        /// <returns></returns>
        public JObject Reporting_GetAllWorkItemRevisions(string project, TaskType type, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?api-version=2.0", this.VsoUrl, project);

            var postContentDic = new Dictionary<string, object>();

            postContentDic.Add("types", new string[] { type.Value });

            if (fields != null && fields.Any())
                postContentDic.Add("fields", fields);

            string jsonContent = JObject.FromObject(postContentDic).ToString();
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);
            var result = json;

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];

            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];

                var postContent = new StringContent(jsonContent);
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                json = JObject.Parse(this.ExecutePostRequest("application/json", nextLink, postContent).Result);

                foreach (var item in json["values"])
                {
                    (result["values"] as JArray).Add(item);
                }

                isLastBatch = (bool)json["isLastBatch"];
            }
            return result;
        }

        /// <summary>
        /// Get all work item revisions for a specific type of item
        /// </summary>
        /// <param name="project"></param>
        /// <param name="type"></param>
        /// <param name="fields"></param>
        /// <param name="pauseTimeBetweenBatch">Pause during each batch request In second</param>
        /// <returns></returns>
        public JObject Reporting_GetAllWorkItemRevisions(string project, string type, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?api-version=2.0", this.VsoUrl, project);

            var postContentDic = new Dictionary<string, object>();

            postContentDic.Add("types", new string[] { type });

            if (fields != null && fields.Any())
                postContentDic.Add("fields", fields);

            string jsonContent = JObject.FromObject(postContentDic).ToString();
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);
            var result = json;

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];

            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];

                var postContent = new StringContent(jsonContent);
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                json = JObject.Parse(this.ExecutePostRequest("application/json", nextLink, postContent).Result);

                foreach (var item in json["values"])
                {
                    (result["values"] as JArray).Add(item);
                }

                isLastBatch = (bool)json["isLastBatch"];
            }
            return result;
        }

        public JObject Reporting_GetAllWorkItemRevisionsFromDate(string project, TaskType type, DateTime? startDate, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?startDateTime={2}&api-version=2.0", this.VsoUrl, project, startDate);

            var postContentDic = new Dictionary<string, object>();

            postContentDic.Add("types", new string[] { type.Value });

            if (fields != null && fields.Any())
                postContentDic.Add("fields", fields);

            string jsonContent = JObject.FromObject(postContentDic).ToString();
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);
            var result = json;

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];

            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];

                var postContent = new StringContent(jsonContent);
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                json = JObject.Parse(this.ExecutePostRequest("application/json", nextLink, postContent).Result);

                foreach (var item in json["values"])
                {
                    (result["values"] as JArray).Add(item);
                }

                isLastBatch = (bool)json["isLastBatch"];
            }
            return result;
        }

        public Dictionary<int, Dictionary<int, JObject>> Reporting_GetDictAllWorkItemRevisions_FromDate(string project, TaskType type, DateTime? startDate, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            var dict = new Dictionary<int, Dictionary<int, JObject>>();
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?startDateTime={2}&api-version=2.0", this.VsoUrl, project, startDate);

            var postContentDic = new Dictionary<string, object>();

            postContentDic.Add("types", new string[] { type.Value });

            if (fields != null && fields.Any())
                postContentDic.Add("fields", fields);

            string jsonContent = JObject.FromObject(postContentDic).ToString();
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];

            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];

                var postContent = new StringContent(jsonContent);
                postContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                json = JObject.Parse(this.ExecutePostRequest("application/json", nextLink, postContent).Result);

                foreach (var item in json["values"])
                {
                    int itemId = (int)item["id"];
                    int rev = (int)item["rev"];

                    if (!dict.ContainsKey(itemId))
                    {
                        var innerItem = new Dictionary<int, JObject>();
                        innerItem.Add(rev, item as JObject);
                        dict.Add(itemId, innerItem);
                    }
                    else
                    {
                        //dict[itemId][rev] = item as JObject;
                        dict[itemId].Add(rev, item as JObject);
                    }
                }

                isLastBatch = (bool)json["isLastBatch"];
            }
            return dict;
        }

        public JObject CreateTestPlan(string project, string testPlanName, string testAreaPath, string itearaionPath, DateTime startDate, DateTime endDate)
        {
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/test/plans?api-version=1.0", this.VsoUrl, project);

            var postContentDic = new Dictionary<string, object>();

            postContentDic.Add("name", testPlanName);
            postContentDic.Add("area", new Dictionary<string, object> { { "name", testAreaPath } });
            postContentDic.Add("iteration", itearaionPath);
            postContentDic.Add("startDate", startDate);
            postContentDic.Add("endDate", endDate);

            string jsonContent = JObject.FromObject(postContentDic).ToString();
            var httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

            var json = JObject.Parse(this.ExecutePostRequest("application/json", url, httpContent).Result);
            return json;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="project"></param>
        /// <param name="type"></param>
        /// <param name="pauseTimeBetweenBatchInSec">Pause during each batch request In second</param>
        /// <returns></returns>
        public JObject Reporting_GetAllRelations(string project, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemLinks?api-version=2.0", this.VsoUrl, project);

            var json = JObject.Parse(this.ExecuteGetRequest("application/json", url).Result);
            var result = json;

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];
            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];
                json = JObject.Parse(this.ExecuteGetRequest("application/json", nextLink).Result);

                foreach (var item in json["values"])
                {
                    (result["values"] as JArray).Add(item);
                }
                isLastBatch = (bool)json["isLastBatch"];
            }
            return result;
        }

        public JObject Reporting_GetAllRelationsFromDate(string project, DateTime? startDateTime, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemLinks?startDateTime={2}&api-version=2.0", this.VsoUrl, project, startDateTime);

            var json = JObject.Parse(this.ExecuteGetRequest("application/json", url).Result);
            var result = json;

            //2 get all next batches
            bool isLastBatch = (bool)json["isLastBatch"];
            while (isLastBatch == false)
            {
                System.Threading.Thread.Sleep(pauseTimeBetweenBatchInSec * 1000);

                string nextLink = (string)json["nextLink"];
                json = JObject.Parse(this.ExecuteGetRequest("application/json", nextLink).Result);

                foreach (var item in json["values"])
                {
                    (result["values"] as JArray).Add(item);
                }
                isLastBatch = (bool)json["isLastBatch"];
            }
            return result;
        }

        #endregion Reporting

        #endregion Get

        #region Update

        /// <summary>
        /// Update work item by ID
        /// </summary>
        /// <param name="id">ID of the work Item</param>
        /// <param name="fields">specify a list of "field reference"/"string-int value" pairs to be updated</param>
        /// <param name="prepareFunction">function to allow custom addings on the fields list</param>
        /// <returns></returns>
        public async Task<JObject> UpdateVsoWorkItemAsync(int id, Dictionary<string, string> fields, Action<List<Dictionary<string, object>>> prepareFunction = null, CancellationToken? token = null)
        {
            string jsonString = null;
            var allFields = new List<Dictionary<string, object>>();

            if (fields == null || fields.Count == 0)
            {
                if (prepareFunction != null)
                    prepareFunction(allFields);
            }
            else
            {
                foreach (var field in fields)
                {
                    var f_field = new Dictionary<string, object>() { { "op", "add" }, { "path", string.Format("/fields/{0}", field.Key) }, { "value", field.Value } };
                    allFields.Add(f_field);
                }
                if (prepareFunction != null)
                    prepareFunction(allFields);
            }

            jsonString = JsonConvert.SerializeObject(allFields);
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json-patch+json");
            var query = await this.ExecutePatchRequest("application/json", string.Format("{0}//DefaultCollection/_apis/wit/workitems/{1}?api-version=1.0", this.VsoUrl,
                id.ToString()), content, token);
            var json = JObject.Parse(query);
            return json;
        }

        public JObject ProcessVsoWorkItemInBatch(Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            var allFields = new List<Dictionary<string, object>>();
            prepareFunction(allFields);
            string jsonString = JsonConvert.SerializeObject(allFields);
            var content = new StringContent(jsonString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
            var query = this.ExecutePostRequest("application/json", string.Format("{0}//DefaultCollection/_apis/wit/$batch?api-version=1.0", this.VsoUrl), content);
            var json = JObject.Parse(query.Result);
            return json;
        }

        public JObject UpdateVsoWorkItem(int id, Dictionary<string, string> fields, Action<List<Dictionary<string, object>>> prepareFunction = null, CancellationToken? token = null)
        {
            return UpdateVsoWorkItemAsync(id, fields, prepareFunction, token).Result;
        }

        #endregion Update

        #region Upload

        public class AttachmentFiles
        {
            public string FileName { get; set; }
            public Stream FileContent { get; set; }
        }

        public JObject UploadAttachmentToVsoWorkItemInBatch(Dictionary<int, Dictionary<string, Stream>> dict_WorkItemIDToFiles, int totalThread = 1)
        {
            BlockingCollection<KeyValuePair<int, AttachmentFiles>> producer = new BlockingCollection<KeyValuePair<int, AttachmentFiles>>();
            BlockingCollection<KeyValuePair<int, string>> listOfWorkItemIDToUrls = new BlockingCollection<KeyValuePair<int, string>>();

            foreach (var item in dict_WorkItemIDToFiles)
            {
                foreach (var file in item.Value)
                {
                    producer.Add(new KeyValuePair<int, AttachmentFiles>(item.Key, new AttachmentFiles { FileName = file.Key, FileContent = file.Value }));
                }
            }
            producer.CompleteAdding();
            Action a = () =>
            {
                KeyValuePair<int, AttachmentFiles> itemToProcess = new KeyValuePair<int, AttachmentFiles>();
                while (producer.TryTake(out itemToProcess))
                {
                    int workItemID = itemToProcess.Key;
                    var files = itemToProcess.Value;
                    var listOfUrls = new BlockingCollection<string>();

                    string fileName = files.FileName;
                    using (Stream fileStream = files.FileContent)
                    {
                        var content = new StreamContent(fileStream);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                        //lock (locker)
                        //{
                        var request = this.ExecutePostRequest("application/json", string.Format("{0}/{1}&{2}", this.VsoUrl, string.Format("{0}{1}", "DefaultCollection/_apis/wit/attachments?filename=", fileName), "api-version=1.0"), content);
                        JObject json = JObject.Parse(request.Result);
                        string url = (string)json["url"];
                        listOfWorkItemIDToUrls.TryAdd(new KeyValuePair<int, string>(workItemID, url));
                        //}
                    }
                }
            };

            var array = Enumerable.Range(0, totalThread).Select(c => a).ToArray();
            Parallel.Invoke(array);

            var result = listOfWorkItemIDToUrls.GroupBy(c => c.Key);

            JObject jsonResult = this.ProcessVsoWorkItemInBatch(
                            (c) =>
                                {
                                    foreach (var item in result)
                                    {
                                        int workItemID = item.Key;
                                        var dictOfUpdate = new List<Dictionary<string, object>>();
                                        foreach (var url in item)
                                        {
                                            dictOfUpdate.Add(new Dictionary<string, object>()
                                                    {
                                            { "op", "add" },
                                            { "path", string.Format("/relations/-") },
                                            { "value", new Dictionary<string, object>()
                                                {
                                                    { "rel", "AttachedFile" },
                                                    { "url", url.Value },
                                                    { "attributes", new Dictionary<string, object>()
                                                        { { "comments", "" } }
                                                    }
                                                }
                                            }
                                                    });
                                        }
                                        var field = new Dictionary<string, object>()
                                          {
                                                { "method","PATCH"},
                                                { "uri",string.Format("/_apis/wit/workItems/{0}?api-version=1.0",workItemID)},
                                                {
                                                    "headers",new Dictionary<string,string>()
                                                    {
                                                        { "Content-Type","application/json-patch+json" }
                                                    }
                                                },
                                                {
                                                    "body",dictOfUpdate
                                                }
                                          };
                                        c.Add(field);
                                    }
                                });
            return jsonResult;
        }

        public void UploadAttachmentToVsoWorkItems(int bugId, Dictionary<string, Stream> files)
        {
            BlockingCollection<KeyValuePair<string, Stream>> producer = new BlockingCollection<KeyValuePair<string, Stream>>();
            foreach (var item in files)
            {
                producer.Add(item);
            }
            producer.CompleteAdding();

            Action a = () =>
            {
                KeyValuePair<string, Stream> itemToProcess = new KeyValuePair<string, Stream>();
                while (producer.TryTake(out itemToProcess))
                {
                    string fileName = itemToProcess.Key;
                    using (Stream fileStream = itemToProcess.Value)
                    {
                        var content = new StreamContent(fileStream);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                        string url = "";

                        var request = this.ExecutePostRequest("application/json", string.Format("{0}/{1}&{2}", this.VsoUrl, string.Format("{0}{1}", "DefaultCollection/_apis/wit/attachments?filename=", fileName), "api-version=1.0"), content);
                        JObject json = JObject.Parse(request.Result);
                        url = (string)json["url"];

                        lock (locker)
                        {
                            JObject result = this.UpdateVsoWorkItem(bugId, null, (c) =>
                            {
                                var field = new Dictionary<string, object>()
                              {
                                { "op", "add" },
                                { "path", string.Format("/relations/-") },
                                { "value", new Dictionary<string, object>()
                                    {
                                        { "rel", "AttachedFile" },
                                        { "url", url },
                                        { "attributes", new Dictionary<string, object>()
                                            { { "comments", "" } }
                                        }
                                    }
                                }
                              };
                                c.Add(field);
                            });
                        }
                    }
                }
            };
            int totalThread = 40;
            var array = Enumerable.Range(0, totalThread).Select(c => a).ToArray();
            Parallel.Invoke(array);
        }

        #endregion Upload

        #region Core

        private async Task<string> ExecutePatchRequest(string header, string requestUri, HttpContent content, CancellationToken? token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", _privateKey))));

                var method = new HttpMethod("PATCH");
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                using (HttpResponseMessage response = client.SendAsync(request, token == null ? CancellationToken.None : token.Value).Result)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        return responseBody;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(responseBody);
                    }
                }
            }
        }

        public async Task<string> ExecutePostRequest(string header, string httpQuery, HttpContent content, int timeOut = 10, CancellationToken? token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", _privateKey))));

                using (HttpResponseMessage response = client.PostAsync(
                            httpQuery, content, token == null ? CancellationToken.None : token.Value).Result)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        return responseBody;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(responseBody);
                    }
                }
            }
        }

        private async Task<string> ExecuteGetRequest(string header, string httpQuery, int timeOut = 10, CancellationToken? token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", _privateKey))));

                using (HttpResponseMessage response = client.GetAsync(
                            httpQuery, token == null ? CancellationToken.None : token.Value).Result)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        return responseBody;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(responseBody);
                    }
                }
            }
        }

        private async Task<Byte[]> ExecuteGetStreamRequest(string header, string httpQuery, int timeOut = 10)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", _privateKey))));

                using (HttpResponseMessage response = client.GetAsync(
                            httpQuery).Result)
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = await response.Content.ReadAsByteArrayAsync();
                    return responseBody;
                }
            }
        }

        private string ExecuteAuthorizedGetRequest(string httpQuery)
        {
            WebRequest myRequest = WebRequest.Create(httpQuery);

            string username = "skwttad";
            string password = "Uj2aeve!Phiol2te";
            string usernamePassword = username + ":" + password;
            CredentialCache mycache = new CredentialCache();
            mycache.Add(new Uri(httpQuery), "Basic", new NetworkCredential(username, password));
            myRequest.Credentials = mycache;
            myRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)));

            WebResponse response = myRequest.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
            string content = reader.ReadToEnd();

            return content;
        }

        public JObject CloneTestSuiteInsidePlan(int planid, string project, int suiteId, string[] suiteNames)
        {
            var getQuery = this.ExecuteGetRequest("application/json", string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}?api-version=1.0", this.VsoUrl, project, planid, suiteId));
            JObject json = JObject.Parse(getQuery.Result);

            String testCasesUrl = json["testCasesUrl"].ToString();
            var parent = json["parent"];
            int parentId = (int)parent["id"];

            getQuery = this.ExecuteGetRequest("application/json", string.Format("{0}", testCasesUrl));
            json = JObject.Parse(getQuery.Result);
            var idList = new List<int>();

            var itemsValue = json["value"];

            foreach (var item in itemsValue.Children())
            {
                JProperty details = item.First.Value<JProperty>();

                var reference = details.Name;
                JObject propertyList = (JObject)item[reference];

                int value = (int)propertyList["id"];
                idList.Add(value);
            }

            foreach (string suiteName in suiteNames)
            {
                var postQuery = string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}?api-version=1.0", this.VsoUrl, project, planid, parentId);

                string jsonContent = string.Format("{{ \"suiteType\": \"StaticTestSuite\", \"name\": \"{0}\", \"plan\": \"{1}\" }}", suiteName, planid);
                var httpContent = new StringContent(jsonContent);
                httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                var result = this.ExecutePostRequest("application/json", postQuery, httpContent);

                json = JObject.Parse(result.Result);

                var items = json["value"];
                String newSuiteId = null;

                foreach (var item in items.Children())
                {
                    newSuiteId = (String)item["id"];
                }

                StringBuilder builder = new StringBuilder();

                foreach (int id in idList)
                {
                    builder.Append(id).Append(",");
                }

                string casesIdString = builder.ToString();
                var query = string.Format("{0}/DefaultCollection/{1}/_apis/test/plans/{2}/suites/{3}/testcases/{4}?api-version=1.0", this.VsoUrl, project, planid, newSuiteId, casesIdString);

                result = this.ExecutePostRequest("application/json", query, null);

                json = JObject.Parse(result.Result);
            }
            return json;
        }

        private static object locker = new object();

        public void CloneTestSuiteInsidePlan2(int planid, string projectName, int selectedsuiteId, string[] suiteNames, string[] existedSuiteIds, List<int> testcaseIdsFromQuery, int totalRetry = 3)
        {
            BlockingCollection<string> list_FailToCreateTestSuite = new BlockingCollection<string>();
            BlockingCollection<string> list_FailToCreateTestCase = new BlockingCollection<string>();
            List<string> list_FinalFailToCreateTestSuite = new List<string>();

            List<string> list_FailToUpdate = new List<string>();
            var IdsForTestCasesToBeAdded = new List<int>();
            //put TC ids in the selected TS into a Dict
            if (!testcaseIdsFromQuery.Any())
            {
                var resultSelectedSuitTC = this.GetTestCaseIDsBySuiteID(projectName, planid, selectedsuiteId);
                IdsForTestCasesToBeAdded = resultSelectedSuitTC.Values.First();
            }
            else
            {
                IdsForTestCasesToBeAdded = testcaseIdsFromQuery;
            }

            var producer = new BlockingCollection<string>();
            //if Tcs in selected TestSuite Exists
            if (IdsForTestCasesToBeAdded.Any())
            {
                //Adding NonExisted TCs into Existed Suites from TestCasess in Selected Suite or TestCases from query
                if (existedSuiteIds.Any())
                {
                    foreach (var existedSuiteId in existedSuiteIds)
                    {
                        try
                        {
                            var result2 = this.GetTestCaseIDsBySuiteID(projectName, planid, int.Parse(existedSuiteId));
                            //int parentId2 = result2.Keys.First();
                            List<int> listId2 = result2.Values.First();
                            //compare testcases
                            HashSet<int> hashsetExistedTC = new HashSet<int>(listId2);

                            List<int> nonExistedIds = new List<int>();
                            foreach (var tcId in IdsForTestCasesToBeAdded)
                            {
                                if (!hashsetExistedTC.Contains(tcId))
                                {
                                    nonExistedIds.Add(tcId);
                                }
                            }
                            if (nonExistedIds.Any())
                            {
                                var json1 = this.AddTestCases(projectName, planid, existedSuiteId.ToString(), nonExistedIds);
                            }
                        }
                        catch (Exception)
                        {
                            list_FailToUpdate.Add(existedSuiteId);
                        }
                    }
                }

                //Add New Suite and put all TC in Selected Suite in it
                if (suiteNames.Any())
                {
                    var result = this.GetListOfTestSuitesByPlanID(projectName, planid);
                    var subitems = result["value"];
                    var item = subitems.Where(c => c["parent"] == null).FirstOrDefault();
                    int parentId = (int)item["id"];

                    //1 create our producer list of testPlans

                    foreach (var suiteName in suiteNames)
                    {
                        producer.Add(suiteName);
                    }
                    producer.CompleteAdding();

                    //2 create our action to be run in parallel
                    var action = new Action(() =>
                    {
                        string itemToProcess = "";

                        while (producer.TryTake(out itemToProcess))
                        {
                            string newSuiteId = "";
                            try
                            {
                                lock (locker)
                                {
                                    newSuiteId = this.CreateTestSuite(projectName, itemToProcess, "StaticTestSuite", planid, parentId);
                                }
                                var json = this.AddTestCases(projectName, planid, newSuiteId, IdsForTestCasesToBeAdded);
                            }
                            catch (Exception)
                            {
                                if (newSuiteId == "")
                                    list_FailToCreateTestSuite.Add(itemToProcess);
                                else
                                    list_FailToCreateTestCase.Add(newSuiteId);
                            }
                        }
                    });

                    //3 Start our process in parallel
                    int totalThread = 40;
                    //var p = new Action[totalThread];
                    var p = Enumerable.Range(0, totalThread).Select(i => action).ToArray();
                    Parallel.Invoke(p);

                    //retry failed ones
                    foreach (var suiteName in list_FailToCreateTestSuite)
                    {
                        string newSuiteId = "";
                        this.RetryMethod(totalRetry, list_FinalFailToCreateTestSuite, suiteName,
                            () =>
                                {
                                    newSuiteId = this.CreateTestSuite(projectName, suiteName, "StaticTestSuite", planid, parentId);
                                });
                        if (newSuiteId != "")
                            this.RetryMethod(totalRetry, list_FinalFailToCreateTestSuite, newSuiteId,
                                () =>
                                {
                                    this.AddTestCases(projectName, planid, newSuiteId, IdsForTestCasesToBeAdded);
                                });
                    }

                    foreach (var suiteID in list_FailToCreateTestCase)
                    {
                        this.RetryMethod(totalRetry, list_FinalFailToCreateTestSuite, suiteID,
                            () =>
                            {
                                this.AddTestCases(projectName, planid, suiteID, IdsForTestCasesToBeAdded);
                            });
                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            if (list_FailToUpdate.Any())
            {
                builder.AppendLine("suites ids Not updated: ");
                foreach (var errorUpdate in list_FailToUpdate)
                {
                    builder.Append(errorUpdate).Append(',');
                }
            }
            if (list_FinalFailToCreateTestSuite.Any())
            {
                builder.AppendLine("suites Not created: ");
                foreach (var errorCreate in list_FailToCreateTestSuite)
                {
                    builder.Append(errorCreate).Append(',');
                }
            }
            string errorMsg = builder.ToString();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                throw new Exception(errorMsg.Substring(0, errorMsg.Length - 1));
            }
        }

        private void RetryMethod(int totalRetry, List<string> errorList, string errorName, Action action)
        {
            int counter = 0;
            bool isPassed = false;
            while (counter < totalRetry)
            {
                counter++;
                try
                {
                    action();
                    isPassed = true;
                    break;
                }
                catch (Exception)
                {
                }
            }
            if (!isPassed)
            {
                errorList.Add(errorName);
            }
        }

        #endregion Core
    }
}
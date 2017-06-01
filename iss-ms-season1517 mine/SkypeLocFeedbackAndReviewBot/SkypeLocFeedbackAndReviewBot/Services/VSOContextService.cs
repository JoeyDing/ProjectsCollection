using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkypeLocFeedbackAndReviewBot.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace SkypeLocFeedbackAndReviewBot.Services
{
    public class VSOContextService
    {
        private readonly string _privateKey;
        public readonly string vsoRootAccount;
        public readonly string vsoUrl;

        public VSOContextService()
        {
            this._privateKey = ConfigurationManager.AppSettings["VsoPrivateKey"];
            this.vsoRootAccount = "skype-test2";
            this.vsoUrl = string.Format("https://{0}.visualstudio.com", vsoRootAccount);
        }

        public async Task<JObject> CreateVsoWorkItem(TaskType type, string projectName, string title, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            var fields = new List<Dictionary<string, object>>();

            var f_title = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.Title" }, { "value", title } };
            fields.Add(f_title);

            var f_areaPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.AreaPath" }, { "value", areaPath } };
            fields.Add(f_areaPath);

            var f_iterationPath = new Dictionary<string, object>() { { "op", "add" }, { "path", "/fields/System.IterationPath" }, { "value", iterationPath } };
            fields.Add(f_iterationPath);

            fields.Add(f_iterationPath);

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
            var query = this.ExecutePatchRequest("application/json", string.Format("{0}/{1}/{2}", this.vsoUrl,
                string.Format("{0}/{1}", "DefaultCollection", projectName), string.Format("{0}{1}{2}", "_apis/wit/workitems/$", type.Value, "?api-version=1.0")), content);
            var json = JObject.Parse(query.Result);
            return json;
        }

        public async Task<JObject> CreateVsoWorkItem(TaskType type, string projectName, string title, string areaPath, string iterationPath, string assignedTo, string referenceWorkItemUrl, LinkType linkType, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null)
        {
            return await CreateVsoWorkItem(type, projectName, title, areaPath, iterationPath, assignedTo, tags, prepareFunction: (fields) =>
            {
                var f_relation = new Dictionary<string, object>() { { "op", "add" }, { "path", "/relations/-" },
            { "value", new Dictionary<string,object>{{ "rel", linkType.Value}, { "url", referenceWorkItemUrl}} }};
                fields.Add(f_relation);

                if (prepareFunction != null)
                    prepareFunction(fields);
            });
        }

        public async Task<JObject> UpdateVsoWorkItem(int id, Dictionary<string, string> fields, Action<List<Dictionary<string, object>>> prepareFunction = null)
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
            var query = this.ExecutePatchRequest("application/json", string.Format("{0}//DefaultCollection/_apis/wit/workitems/{1}?api-version=1.0", this.vsoUrl,
                id.ToString()), content);
            var json = JObject.Parse(query.Result);
            return json;
        }

        private async Task<string> ExecutePatchRequest(string header, string requestUri, HttpContent content)
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

                using (HttpResponseMessage response = client.SendAsync(request).Result)
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

        public async Task UploadAttachmentToVsoWorkItems(int bugId, Dictionary<string, Stream> files)
        {
            foreach (var item in files)
            {
                string fileName = item.Key;
                Stream fileStream = item.Value;

                var content = new StreamContent(fileStream);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");

                var request = this.ExecutePostRequest("application/json", string.Format("{0}/{1}&{2}", this.vsoUrl, string.Format("{0}{1}", "DefaultCollection/_apis/wit/attachments?filename=", fileName), "api-version=1.0"), content);

                JObject json = JObject.Parse(request.Result);

                string url = (string)json["url"];

                JObject result = await UpdateVsoWorkItem(bugId, null, (c) =>
                {
                    var field = new Dictionary<string, object>() {
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

        private async Task<string> ExecutePostRequest(string header, string httpQuery, HttpContent content, int timeOut = 10)
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
                            httpQuery, content).Result)
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
    }
}
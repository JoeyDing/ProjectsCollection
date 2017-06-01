using ExtensibleDataExtraction.Lib;
using ExtensibleDataExtraction.Lib.Data.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VsoApi.Rest;

namespace ExtensibleVSOAPI.Services
{
    /// <summary>
    /// VSO context service
    /// </summary>
    public class VSOContextService
    {
        public readonly string VsoUrl;
        public string authenticationKey;
        public string VsoPrivateKey;
        public ExtensibleContext extensibleContext = new ExtensibleContext("VSOAPI.log");
        private static ConfigurationSerializerService service = new ConfigurationSerializerService();
        private static string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config.xml");
        private static ExtensibleConfig extensibleItems = service.GetExtensibleConfigFromConfig(configPath);
        private List<ExtensibleItem> items = extensibleItems.Items.Items;

        public VSOContextService()
        {
            this.VsoPrivateKey = extensibleItems.GlobalConfigItem.Params.ParamsCollection.Where(k => k.Key == "Token").Select(k => k.Value).First();
            this.VsoUrl = string.Format("https://{0}.visualstudio.com", extensibleItems.GlobalConfigItem.Params.ParamsCollection.Where(k => k.Key == "RootAccount").Select(k => k.Value).First());
        }

        /// <summary>
        /// Get all VSO work items by date
        /// </summary>
        /// <param name="project">Project name</param>
        /// <param name="type">Work item type value</param>
        /// <param name="startDate">Start date time</param>
        /// <param name="fields">Name of fields</param>
        /// <param name="pauseTimeBetweenBatchInSec"></param>
        /// <returns></returns>
        public JObject GetAllWorkItemRevisionsFromDate(string project, TaskType type, DateTime? startDate, string[] fields = null, int pauseTimeBetweenBatchInSec = 0)
        {
            //1 Get first Batch
            string url = null;
            if (startDate.HasValue)
                url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?startDateTime={2}&api-version=2.0", this.VsoUrl, project, startDate);
            else
                url = string.Format("{0}/DefaultCollection/{1}/_apis/wit/reporting/workItemRevisions?api-version=2.0", this.VsoUrl, project);

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

        private async Task<string> ExecutePostRequest(string header, string httpQuery, HttpContent content, int timeOut = 10, CancellationToken? token = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", this.VsoPrivateKey))));

                using (HttpResponseMessage response = await client.PostAsync(
                            httpQuery, content, token == null ? CancellationToken.None : token.Value))
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        return responseBody;
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException(responseBody);
                    }
                }
            }
        }
    }
}
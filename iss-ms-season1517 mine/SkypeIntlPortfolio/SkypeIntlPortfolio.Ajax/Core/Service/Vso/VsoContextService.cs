using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using VsoApi;
using VsoApi.Rest;

namespace SkypeIntlPortfolio.Ajax.Core.Service.Vso
{
    public class VsoContextService
    {
        public VsoContext context;

        public VsoContextService(VsoContext context)
        {
            this.context = context;
        }

        public class AttachmentFiles
        {
            public string FileName { get; set; }
            public Stream FileContent { get; set; }
        }

        public JObject UploadAttachmentToVsoWorkItemInBatch(Dictionary<int, Dictionary<string, Stream>> dict_WorkItemIDToFiles, int totalThread = 1, int totalRetry = 3)
        {
            BlockingCollection<KeyValuePair<int, string>> listOfWorkItemIDToUrls = new BlockingCollection<KeyValuePair<int, string>>();
            BlockingCollection<KeyValuePair<int, AttachmentFiles>> producer = new BlockingCollection<KeyValuePair<int, AttachmentFiles>>();

            foreach (var item in dict_WorkItemIDToFiles)
            {
                foreach (var file in item.Value)
                {
                    producer.Add(new KeyValuePair<int, AttachmentFiles>(item.Key, new AttachmentFiles { FileName = file.Key, FileContent = file.Value }));
                }
            }

            this.RetryForParallel_Recursively(producer, listOfWorkItemIDToUrls, totalRetry, totalThread);

            var result = listOfWorkItemIDToUrls.GroupBy(c => c.Key);
            return this.RetryMethod(totalRetry, () =>
             {
                 JObject jsonResult = context.ProcessVsoWorkItemInBatch(
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
             });
        }

        public Dictionary<string, int> CreateChildItemsForExistingParentItemInBatch(List<LanguageAndTitle> languageAndTitles, string projectName, LinkType linkType, string referenceWorkItemUrl, TaskType type, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null, int totalRetry = 3)
        {
            return RetryMethod(totalRetry, () => context.CreateChildItemsForExistingParentItemInBatch(languageAndTitles, projectName, linkType, referenceWorkItemUrl, type, areaPath, iterationPath, assignedTo, tags, prepareFunction));
        }

        public Dictionary<string, int> CreateNewParentItemAndChildItemsInBatch(string parentLanguage, string parentBugTitle, List<LanguageAndTitle> languageAndTitles, string projectName, LinkType linkType, TaskType type, string areaPath, string iterationPath, string assignedTo, string[] tags = null, Action<List<Dictionary<string, object>>> prepareFunction = null, int totalRetry = 3)
        {
            return RetryMethod(totalRetry, () => context.CreateNewParentItemAndChildItemsInBatch(parentLanguage, parentBugTitle, languageAndTitles, projectName, linkType, type, areaPath, iterationPath, assignedTo, tags, prepareFunction));
        }

        private void RetryForParallel_Recursively(BlockingCollection<KeyValuePair<int, AttachmentFiles>> producer, BlockingCollection<KeyValuePair<int, string>> result, int totalRety, int totalThread)
        {
            if (totalRety == 0 || !producer.Any())
                return;
            producer.CompleteAdding();
            BlockingCollection<KeyValuePair<int, AttachmentFiles>> failedItems = new BlockingCollection<KeyValuePair<int, AttachmentFiles>>();
            Action a = () =>
            {
                KeyValuePair<int, AttachmentFiles> itemToProcess = new KeyValuePair<int, AttachmentFiles>();
                while (producer.TryTake(out itemToProcess))
                {
                    try
                    {
                        int workItemID = itemToProcess.Key;
                        var files = itemToProcess.Value;
                        var listOfUrls = new BlockingCollection<string>();

                        string fileName = files.FileName;
                        using (Stream fileStream = files.FileContent)
                        {
                            var content = new StreamContent(fileStream);
                            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json");
                            var request = context.ExecutePostRequest("application/json", string.Format("{0}/{1}&{2}", context.VsoUrl, string.Format("{0}{1}", "DefaultCollection/_apis/wit/attachments?filename=", fileName), "api-version=1.0"), content);
                            JObject json = JObject.Parse(request.Result);
                            string url = (string)json["url"];
                            result.TryAdd(new KeyValuePair<int, string>(workItemID, url));
                        }
                    }
                    catch (Exception)
                    {
                        failedItems.TryAdd(itemToProcess);
                    }
                }
            };

            var array = Enumerable.Range(0, totalThread).Select(c => a).ToArray();
            Parallel.Invoke(array);

            producer = failedItems;
            totalRety--;
            RetryForParallel_Recursively(producer, result, totalRety, totalThread);
        }

        private T RetryMethod<T>(int totalRetry, Func<T> func) where T : class
        {
            int counter = 0;
            bool isPassed = false;
            string errorMsg = "";
            T result = null;
            while (counter < totalRetry)
            {
                counter++;
                try
                {
                    result = func();
                    isPassed = true;
                    break;
                }
                catch (Exception e)
                {
                    errorMsg = e.ToString();
                }
            }
            if (!isPassed)
            {
                throw new Exception(errorMsg);
            }
            return result;
        }
    }
}
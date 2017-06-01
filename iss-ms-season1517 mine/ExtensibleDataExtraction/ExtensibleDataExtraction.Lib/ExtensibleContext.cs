using ExtensibleDataExtraction.Lib.Data.Configuration;
using ExtensibleDataExtraction.Lib.Data.Processing;
using ExtensibleDataExtraction.Lib.Data.Sql;
using ExtensibleDataExtraction.Lib.Interfaces;
using ExtensibleDataExtraction.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib
{
    public class ExtensibleContext
    {
        //services
        private readonly SqlCreateService sqlCreateService;

        private readonly SqlReadService sqlReadService;

        private readonly SqlWriteService sqlWriteService;
        public Logger logger;

        public ExtensibleContext(string logName)
        {
            this.sqlCreateService = new SqlCreateService();
            this.sqlReadService = new SqlReadService();
            this.sqlWriteService = new SqlWriteService();
            this.logger = new Logger(logName);
        }

        #region Core

        public async Task<string> ExecutePostRequest(string header, string httpQuery, HttpContent content, string privateKey, int? timeOut)
        {
            using (HttpClient client = new HttpClient())
            {
                int queryTimeout = timeOut.HasValue ? timeOut.Value : 30;
                client.Timeout = new TimeSpan(0, 0, queryTimeout * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                if (!string.IsNullOrWhiteSpace(privateKey))
                {
                    //TODO: Extract authentication information and format as parameters
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                string.Format("{0}", privateKey))));
                }

                var requestMessage = new HttpRequestMessage();
                requestMessage.Content = content;
                using (HttpResponseMessage response = client.PostAsync(
                            httpQuery, requestMessage.Content).Result)
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

        public async Task<string> ExecuteGetRequest(string header, string httpQuery, string privateKey, int? timeOut)
        {
            using (HttpClient client = new HttpClient())
            {
                int queryTimeout = timeOut.HasValue ? timeOut.Value : 30;
                client.Timeout = new TimeSpan(0, 0, queryTimeout * 60);
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(header));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", "", privateKey))));

                using (HttpResponseMessage response = client.GetAsync(
                            httpQuery).Result)
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

        #endregion Core

        /// <summary>
        /// Call api URL with specified parameters and return a jsonString
        /// </summary>
        /// <param name="extensibleItem"></param>
        /// <returns></returns>
        public string RetrieveJsonData(ExtensibleItem extensibleItem, string connectionString_global)
        {
            if (extensibleItem.JsonEndPoint == null)
                throw new ArgumentException(@"JsonEndPoint cannot be null or empty");

            if (extensibleItem.JsonEndPoint.JsonQueryUrl == null
                && extensibleItem.JsonEndPoint.CustomLib == null)
                throw new ArgumentException(@"JsonQueryUrl or CustomLib cannot be null or empty");

            if (extensibleItem.JsonEndPoint.JsonQueryUrl != null
              && extensibleItem.JsonEndPoint.CustomLib != null)
                throw new ArgumentException(@"JsonQueryUrl && CustomLib cannot be used together, pick one.");

            string result = null;

            if (extensibleItem.JsonEndPoint.CustomLib != null)
            {
                //use reflection to get the json string
                Assembly myDll = Assembly.Load(extensibleItem.JsonEndPoint.CustomLib.DllName);
                Type MyLoadClass = myDll.GetType(extensibleItem.JsonEndPoint.CustomLib.FetchClass);
                //create instance of class(library name adn class name should be given)
                object obj = Activator.CreateInstance(MyLoadClass);
                var fetchClass = (IFetch)obj;
                string connectionString = extensibleItem.ConnectionString;
                if (connectionString == null)
                    if (connectionString_global != null)
                        connectionString = connectionString_global;
                    else
                        throw new Exception("ConnectionString can not be Null");

                using (var extensibleDbEntity = new ExtensibleDbEntity(connectionString))
                    result = fetchClass.FetchData(extensibleItem, extensibleDbEntity);
            }
            else
            {
                if (extensibleItem.JsonEndPoint.HeaderContent == null)
                    throw new ArgumentException(@"HeaderContent cannot be null or empty");

                switch (extensibleItem.JsonEndPoint.RequestType)
                {
                    case RequestType.GET:

                        result = this.ExecuteGetRequest(extensibleItem.JsonEndPoint.HeaderContent, extensibleItem.JsonEndPoint.JsonQueryUrl, extensibleItem.JsonEndPoint.Token, extensibleItem.JsonEndPoint.TimeOut).Result;

                        break;

                    case RequestType.POST:

                        //get form body content
                        FormUrlEncodedContent formBodyContent = null;
                        if (extensibleItem.JsonEndPoint.FormContent != null && extensibleItem.JsonEndPoint.FormContent.Any())
                        {
                            var bodyDict = new Dictionary<string, string>();
                            foreach (var formContentItem in extensibleItem.JsonEndPoint.FormContent)
                            {
                                bodyDict.Add(formContentItem.Key, formContentItem.Value);
                            }
                            formBodyContent = new FormUrlEncodedContent(bodyDict);
                        }

                        result = this.ExecutePostRequest(extensibleItem.JsonEndPoint.HeaderContent, extensibleItem.JsonEndPoint.JsonQueryUrl, formBodyContent, extensibleItem.JsonEndPoint.Token, extensibleItem.JsonEndPoint.TimeOut).Result;

                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Convert jsonData (json array of objects) to ExtensibleDataSet
        /// </summary>
        /// <param name="jsonData"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public ExtensibleDataSet ParseJsonData(string jsonData, ExtensibleItem extensibleitem)
        {
            //check parameters
            if (string.IsNullOrWhiteSpace(jsonData))
                throw new ArgumentException(@"jsonData cannot be null or empty");

            if (extensibleitem.Mapping == null || extensibleitem.Mapping.Fields == null || !extensibleitem.Mapping.Fields.Any())
                throw new ArgumentException(@"mapping\fields cannot be null or empty");

            if (extensibleitem.Mapping.Fields.Any(x => x.JsonFieldName == null || x.SqlColumnName == null))
                throw new ArgumentException("JsonFieldName or SqlColumnName can't be null");

            if (string.IsNullOrWhiteSpace(extensibleitem.JsonEndPoint.ResultTemplate) || string.IsNullOrWhiteSpace(extensibleitem.JsonEndPoint.ResultItemTemplate))
                throw new ArgumentException(@"ResultTemplate or ResultItemTemplate cannot be null or empty");

            ExtensibleDataSet result = new ExtensibleDataSet
            {
                Mapping = extensibleitem.Mapping,
                Rows = new List<ExtensibleDataRow>()
            };

            //Convert jsonData to JContainer
            JContainer root = jsonData.StartsWith("[") ? JArray.Parse(jsonData) as JContainer : JObject.Parse(jsonData) as JContainer;
            //Get TesultTemplate data and convert it to JArray
            var array = root is JArray ? (JArray)root : JArray.Parse(this.GetJsonSection(root, extensibleitem.JsonEndPoint.ResultTemplate).ToString());
            List<string> jsonFieldName = new List<string>();
            foreach (var item in array)
            {
                //Get ResultItemTemplate data and return string Object.
                var childItem = this.GetJsonSection(item as JObject, extensibleitem.JsonEndPoint.ResultItemTemplate);
                //Get json field item from config file
                if (childItem == null || !(childItem is JObject))
                    childItem = new JObject();

                jsonFieldName = extensibleitem.Mapping.Fields.Select(f => f.JsonFieldName).ToList();
                foreach (var fieldName in jsonFieldName)
                    if (childItem[fieldName] == null)
                        (childItem as JObject).Add(fieldName, item[fieldName]);

                this.RetrieveResult(childItem, extensibleitem, result);
            }
            return result;
        }

        public JContainer GetJsonSection(JContainer jsonData, string template)
        {
            if (template != null)
            {
                var keys = template.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var key in keys)
                    jsonData = jsonData[key] as JContainer;
            }
            return jsonData;
        }

        private void RetrieveResult(JToken itemTemplateObject, ExtensibleItem extensibleitem, ExtensibleDataSet result)
        {
            var row = new ExtensibleDataRow
            {
                Values = new List<string>(extensibleitem.Mapping.Fields.Count)
            };
            foreach (var jsonField in extensibleitem.Mapping.Fields)
                row.Values.Add(itemTemplateObject[jsonField.JsonFieldName].Value<string>());

            result.Rows.Add(row);
        }

        ///
        /// <summary>
        /// Save converted data to database.
        /// If the table doesn't exist, it will first create it.
        /// If a column doesn't exist, it will add it.
        /// Last step is to insert the rows inside the table.
        /// </summary>
        /// <param name="dataSet"></param>
        ///<param name="noIncrementInsert">if there is no value, default is false</param>
        public void SaveToDatabase(string connectionString, ExtensibleDataSet dataSet)
        {
            if (dataSet == null
                || dataSet.Mapping == null || string.IsNullOrWhiteSpace(dataSet.Mapping.SqlTableName)
                || dataSet.Mapping.Fields == null
                || dataSet.Rows == null)

                throw new ArgumentException(@"dataSet\mapping\SqlTableName\Fields cannot be null or empty");

            string[] identityFields = dataSet.Mapping.Fields.Where(x => x.IsIdentity == true).Select(x => x.SqlColumnName).ToArray();

            if (!identityFields.Any())
                throw new ArgumentException("No identify field found: At least one Identify field should be specified.");
            if (connectionString == null)
                throw new Exception("ConnectionString can not be Null");

            using (var dbContext = new ExtensibleDbEntity(connectionString))
            {
                //If the table doesn't exist create it
                bool tableExist = sqlReadService.TableExists(dataSet.Mapping.SqlTableName, dbContext);

                if (!tableExist)
                {
                    this.sqlCreateService.CreateTable(dataSet.Mapping.SqlTableName, dataSet.Mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);
                }

                //if a column doesn't exist, add it
                this.sqlCreateService.CreateMissingColumns(dataSet.Mapping.SqlTableName, dataSet.Mapping.Fields.Select(c => c.SqlColumnName).ToArray(), dbContext);

                //Insert data
                if (dataSet.Mapping.SaveType == DataSaveType.SaveType.Full)
                {
                    this.sqlCreateService.EmptyTable(dataSet.Mapping.SqlTableName, dbContext);
                }

                string[] fields = dataSet.Mapping.Fields.Select(c => c.SqlColumnName).ToArray();
                string[][] values = dataSet.Rows.Select(x => x.Values.ToArray()).ToArray();

                //Drop table columns that are mismatching with configuration fields.
                sqlCreateService.DropColumns(dbContext, dataSet, fields);

                this.sqlWriteService.UpdateData(dataSet.Mapping.SqlTableName, fields, values, identityFields, dbContext, this.logger);
            }
        }

        /// <summary>
        ///Work flow of current project
        /// </summary>
        /// <param name="xmlConfigurationFilePath"></param>
        public void StartProcess(string xmlConfigurationFilePath)
        {
            try
            {
                //1.Get exensible items from Config file , will return extensibleItems.
                ConfigurationSerializerService configService = new ConfigurationSerializerService();
                ExtensibleConfig globalConfig = configService.GetExtensibleConfigFromConfig(xmlConfigurationFilePath);

                //ExtensibleItems extensibleItems = gloablConfig.exe
                var context = new ExtensibleContext(logger.logFileName);

                //2.for each of the elements, fetch the data to get the converted file of specific type
                foreach (ExtensibleItem extensibleItem in globalConfig.Items.Items)
                {
                    //3. parse json data. a data set will be generated
                    var jsonData = context.RetrieveJsonData(extensibleItem, globalConfig.GlobalConfigItem.ConnectionString);
                    //var dataSet = context.ParseJsonData(jsonString, extensibleItem);
                    var dataSet = context.ParseJsonData(jsonData, extensibleItem);

                    //4. save to db(pass the savetype in, hence user could decide how do thay want to insert data)
                    context.SaveToDatabase(extensibleItem.ConnectionString == null ? globalConfig.GlobalConfigItem.ConnectionString : extensibleItem.ConnectionString, dataSet);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogException(ex);
            }
        }
    }
}
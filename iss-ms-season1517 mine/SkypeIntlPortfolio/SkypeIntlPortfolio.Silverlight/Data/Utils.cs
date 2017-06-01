using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SkypeIntlPortfolio.Silverlight.Data
{
    public static class Utils
    {
        public static readonly string WebApiRootPath;

        static Utils()
        {
            var rootUri = System.Windows.Browser.HtmlPage.Document.DocumentUri;
            WebApiRootPath = new Uri(rootUri, "api").ToString();
        }

        public static T ConvertResult<T>(HttpWebResponse res)
        {
            string stringResponse = null;

            try
            {
                using (StreamReader streamReader = new StreamReader(res.GetResponseStream()))
                    stringResponse = streamReader.ReadToEnd();
                var jsonResult = JsonConvert.DeserializeObject<T>(stringResponse);
                return jsonResult;
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(stringResponse))
                    throw new Exception("Server didn't respond.");
                else
                    throw new Exception(string.Format("Couldn't parse server response:\"{0}\"", stringResponse));
            }
        }

        public static Task<WebResponse> ExecuteRequest(Uri uri, string method = "GET", string header = null)
        {
            HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
            //httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;
            //httpWebRequest.UserAgent = "Skype International Team Tool Service";

            httpWebRequest.UseDefaultCredentials = true;

            if (header != null)
            {
                using (var streamWriter = new StreamWriter(Task.Factory.FromAsync<Stream>(httpWebRequest.BeginGetRequestStream, httpWebRequest.EndGetRequestStream, null).Result))
                {
                    streamWriter.Write(header);
                    streamWriter.Flush();
                }
            }

            Task<WebResponse> response = null;
            try
            {
                response = Task.Factory.FromAsync<WebResponse>(httpWebRequest.BeginGetResponse, httpWebRequest.EndGetResponse, TaskCreationOptions.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                throw;
            }

            return response;
        }

        public static Task<T> GetData<T>(string uri)
        {
            var data = Utils.ExecuteRequest(new Uri(uri));
            var res = data.ContinueWith<T>((ant) =>
                {
                    return Utils.ConvertResult<T>((HttpWebResponse)ant.Result);
                });

            return res;
        }
    }
}
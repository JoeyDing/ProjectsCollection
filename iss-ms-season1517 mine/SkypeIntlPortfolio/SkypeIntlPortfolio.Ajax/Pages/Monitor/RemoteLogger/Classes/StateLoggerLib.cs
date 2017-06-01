
using SkypeIntlPortfolio.Ajax;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Globalization;
using System.Web.Caching;

namespace RemoteLogger.Lib.Classes
{

    public class StateLoggerLib
    {



      
        public static void ServerPocessPosted(HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                string values = reader.ReadToEnd();
                if (!String.IsNullOrEmpty(values))
                {
                    //1. Extract information from post data
                    RemotLogger.Client.Lib.StateLoggerLib.StateLoggerBundle bundle = ExtractState(values);

                    if (bundle.OperationCommand.Equals("ed"))
                    {
                        ResponseLogEnd(context, bundle.BatchID);
                    }
                    else if (bundle.OperationCommand.Equals("state"))
                    {
                        SaveToStateDatabase(bundle);
                    }
                    else if (bundle.OperationCommand.Equals("st"))
                    {
                        ResponseLogStart(context);
                    }

                }
            }
        }

        private static void ResponseLogStart(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            string batchId = Guid.NewGuid().ToString();
            context.Response.Write("{'batchid': '" + batchId + "'}");
        }

        private static void ResponseLogEnd(HttpContext context, string batchID)
        {
            context.Response.ContentType = "application/json";
            context.Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            using (SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities())
            {
                var logger = dbContext.RemoteStateLoggers.Where(r => r.BatchID == batchID).OrderByDescending(r => r.UpdateDate).FirstOrDefault();
                string bundle = "{'batchid': '" + batchID + "','appName':'" + logger.ApplicationName + "'}";
                context.Response.Write(bundle);
            }
        }

        private static RemotLogger.Client.Lib.StateLoggerLib.StateLoggerBundle ExtractState(string value)
        {
            value = HttpUtility.UrlDecode(value);
            value = Uri.UnescapeDataString(value);
            RemotLogger.Client.Lib.StateLoggerLib.StateLoggerBundle bundle = new RemotLogger.Client.Lib.StateLoggerLib.StateLoggerBundle();
            string[] values = value.Split('&');
            foreach (string inf in values)
            {
                string[] pair = inf.Split('=');
                string key = pair[0];
                string val = pair[1];
                if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.ApplicationName))
                {
                    bundle.ApplicationName = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.LanguageName))
                {
                    bundle.LanguageName = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.TestcaseName))
                {
                    bundle.TestcaseName = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.State))
                {
                    bundle.State = Convert.ToBoolean(val);
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.UserIdentity))
                {
                    bundle.UserIdentity = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.BatchID))
                {
                    bundle.BatchID = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.OperationCommand))
                {
                    bundle.OperationCommand = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.ImagePath))
                {
                    bundle.ImagePath = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.ImageBinary))
                {
                    bundle.ImageBinary = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.StartRunDate))
                {
                    bundle.StartRunDate = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerKey.DateTimeToStringKey))
                {
                    bundle.DateTimeToStringKey = val;
                }
            }
            return bundle;
        }
        private static void SaveToStateDatabase(RemotLogger.Client.Lib.StateLoggerLib.StateLoggerBundle bundle)
        {
            using (SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities())
            {
                RemoteStateLogger log = new RemoteStateLogger();
                string name = bundle.ApplicationName.ToLower();
                string mapPath = System.AppDomain.CurrentDomain.BaseDirectory + "//Pages//Monitor//RemoteLogger//Content//map.txt";

                System.IO.StreamReader file = new System.IO.StreamReader(mapPath);
                string line = "";
                Dictionary<string, string> dictMap = new Dictionary<string, string>();
                while ((line = file.ReadLine()) != null)
                {
                    string[] pair = line.Split(',');
                    dictMap[pair[0]] = pair[1];
                }

                foreach (var dict in dictMap)
                {
                    if (bundle.ApplicationName.ToLower().Contains(dict.Key))
                    {
                        name = dict.Value;
                    }
                }
                DateTime startRunTime = DateTime.ParseExact(bundle.StartRunDate, bundle.DateTimeToStringKey, CultureInfo.InvariantCulture);

                //Save a run result row to [RemoteStateLoggers]
                log.ApplicationName = name;
                log.LanguageName = bundle.LanguageName;
                log.UpdateDate = DateTime.Now;
                log.TestcaseName = bundle.TestcaseName;
                log.BatchID = bundle.BatchID;
                log.UserIdentity = bundle.UserIdentity;
                log.State = bundle.State;
                log.StartDate = startRunTime;

                dbContext.RemoteStateLoggers.Add(log);
                dbContext.SaveChanges();

                //Get RemoteStateLoggersId
                int remoteStateLoggerId = log.Id;


                List<string> paths = bundle.ImagePath.Split(';').ToList();
                List<string> imageBinary = bundle.ImageBinary.Split(';').ToList();
                for (int i = 0; i < paths.Count; i++)
                {

                    RemoteStateLoggerImage image = new RemoteStateLoggerImage();
                    //Get ImageName
                    List<string> devidePathArray = paths[i].Split('\\').ToList();
                    List<string> imageSplitByDot = devidePathArray[devidePathArray.Count - 1].Split('.').ToList();
                    string imageName = imageSplitByDot[0];

                    //Get Base64 Image according image path
                    string imageBaseString = imageBinary[i];
                    int mod4 = imageBaseString.Length % 4;
                    if (mod4 > 0)
                    {
                        imageBaseString += new string('=', 4 - mod4);
                    }
                    var bytes = Convert.FromBase64String(imageBaseString);

                    //Create image folder in server
                    string remoteLoggerImagePath= ConfigurationManager.AppSettings["RemoteLoggerImagePath"].ToString();
                    string imgFolderPath = String.Format("{0}//RemoteLogger//Upload//{1}//{2}//{3}", remoteLoggerImagePath, name, bundle.BatchID, bundle.LanguageName);
                    if (!Directory.Exists(imgFolderPath))
                    {
                        Directory.CreateDirectory(imgFolderPath);
                    }
                    string imgPath = string.Empty;
                    imgPath = imgFolderPath + "//" + imageName + ".png";
                    using (var imageFile = new FileStream(imgPath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                    }

                    //Save image to table [RemoteStateLoggerImages]
                    image.RemoteStateLoggerID = remoteStateLoggerId;
                    image.ImagePath = imgPath;
                    dbContext.RemoteStateLoggerImages.Add(image);

                    dbContext.SaveChanges();
                }

            }
            } 

    }
}
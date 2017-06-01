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

namespace RemoteLogger.Lib.Classes
{
    public class ExceptionLoggerLib
    {
        private static SkypeIntlMonitoringEntities dbContext = new SkypeIntlMonitoringEntities();

        public static void ServerProcessPostedExceptions(HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                string values = reader.ReadToEnd();
                if (!String.IsNullOrEmpty(values))
                {
                    //1. Extract information from post data
                    RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerBundle bundle = ExtractException(values);

                    //2. Save the data to the database
                    SaveToDatabase(bundle);
                }
            }
        }

        private static RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerBundle ExtractException(string value)
        {
            value = HttpUtility.UrlDecode(value);
            value = Uri.UnescapeDataString(value);
            RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerBundle bundle = new RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerBundle();
            string[] values = value.Split('&');
            foreach (string inf in values)
            {
                string[] pair = inf.Split('=');
                string key = pair[0];
                string val = pair[1];
                if (key.Equals(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerKey.ApplicationName))
                {
                    bundle.ApplicationName = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerKey.Exception))
                {
                    bundle.Exception = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerKey.ExceptionStackTrace))
                {
                    bundle.ExceptionStackTrace = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerKey.UserIdentity))
                {
                    bundle.user = val;
                }
                else if (key.Equals(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerKey.BatchID))
                {
                    bundle.BatchID = val;
                }
            }
            return bundle;
        }

        private static void SaveToDatabase(RemotLogger.Client.Lib.ExceptionLoggerLib.LoggerBundle bundle)
        {
            SkypeIntlPortfolio.Ajax.RemoteLogger log = new SkypeIntlPortfolio.Ajax.RemoteLogger();

            //Get Application Name from map.txt
            string name = "";
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

            var stateLogger = dbContext.RemoteStateLoggers.OrderByDescending(l => l.UpdateDate).FirstOrDefault();
            if (stateLogger != null)
            {
                log.BatchID = stateLogger.BatchID;
                log.StateID = stateLogger.Id;
            }
            log.ApplicationName = name;
            log.Exception = bundle.Exception;
            log.ExceptionStackTrace = bundle.ExceptionStackTrace;
            log.UpdateDate = DateTime.Now;
            log.UserIdentity = bundle.user;
            log.BatchID = bundle.BatchID;
            dbContext.RemoteLoggers.Add(log);
            dbContext.SaveChanges();
        }
    }
}
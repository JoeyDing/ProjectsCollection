using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsoWorkItemsSync.Helper
{
    public static class Logger
    {
        public static string LogFileName = "VsoWorkItemsSync.log";

        public static void LogMessage(string msg)
        {
            Log(msg, "--[INFO]");
        }

        public static void LogException(Exception e)
        {
            Log(e.Message, "--[EXCEPTION]");
        }

        public static void LogStart()
        {
            Log(new String('-', 90), "[BEGIN]");
        }

        public static void LogEnd()
        {
            Log("--", "[END]");
        }

        private static void Log(string msg, string prefix)
        {
            string logDirectory = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "logs" });
            string logFile = Path.Combine(logDirectory, LogFileName);
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            StreamWriter stream = null;

            if (!File.Exists(logFile))
                stream = new StreamWriter(logFile);
            else
                stream = new StreamWriter(logFile, true);
            using (stream)
            {
                var msgBuilder = new StringBuilder();
                msgBuilder.AppendLine();
                msgBuilder.AppendLine(string.Format("{0}\t {1}: {2}", prefix, DateTime.Now.ToString(), msg));
                stream.Write(msgBuilder.ToString());
            }
        }
    }
}
using System;
using System.IO;
using System.Text;

namespace Sfb.UI.Wpf
{
    public static class Logger
    {
        private static readonly object locker = new object();
        public static string LogFileName = "AndroidAutomationTool.log";
        public static string LogDirectory;

        static Logger()
        {
            LogDirectory = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "logs" });
        }

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
            lock (locker)
            {
                string logFile = Path.Combine(LogDirectory, LogFileName);
                if (!Directory.Exists(LogDirectory))
                    Directory.CreateDirectory(LogDirectory);

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
}
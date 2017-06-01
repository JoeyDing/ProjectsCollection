using System;
using System.IO;
using System.Text;

namespace SkypeIntlPortfolio.SyncTool.Core
{
    public class Logger
    {
        public string LogFileName { get; set; }

        public Logger()
        {
            this.LogFileName = "SkypeIntlPortfolio.SyncTool.log";
        }

        public void LogMessage(string msg)
        {
            Log(msg, "--[INFO]");
        }

        public void LogException(Exception e)
        {
            Log(e.ToString(), "--[EXCEPTION]");
        }

        public void LogStart()
        {
            Log(new String('-', 90), "[BEGIN]");
        }

        public void LogEnd()
        {
            Log("--", "[END]");
        }

        private void Log(string msg, string prefix)
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

                Console.WriteLine(msgBuilder.ToString());
            }
        }
    }
}
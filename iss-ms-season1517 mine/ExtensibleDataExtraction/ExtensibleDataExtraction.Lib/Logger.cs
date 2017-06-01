using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtensibleDataExtraction.Lib
{
    public class Logger
    {
        public string logFileName = "ExtensibleDataExtraction.log";
        public Logger(string logName)
        {
            this.logFileName = logName;
        } 

        

        public void LogMessage(string msg)
        {
            Log(msg, "--[INFO]");
        }

        public void LogException(Exception e)
        {
            Log(e.Message, "--[EXCEPTION]");
        }

        public void LogStart()
        {
            Log("Start Time is : " + System.DateTime.Now, "--[BEGIN]");
        }

        public void LogEnd()
        {
            Log("End Time is : " + System.DateTime.Now, "--[END]");
        }

        private void Log(string msg, string prefix)
        {
            string logDirectory = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "logs" });
            string logFile = Path.Combine(logDirectory, logFileName);
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
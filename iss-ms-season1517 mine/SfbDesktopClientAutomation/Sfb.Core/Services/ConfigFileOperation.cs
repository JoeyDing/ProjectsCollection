using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;

namespace Sfb.Core.Services
{
    public class ConfigFileOperation
    {
        private static string rootDrive = Path.GetPathRoot(Environment.SystemDirectory);
        private string sfbRootPath_O15 = Path.Combine(rootDrive, @"Program Files\Microsoft Office\Office15");
        private string sfbRootPath_O16 = Path.Combine(rootDrive, @"Program Files\Microsoft Office\Office16");
        private string configFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConfigFile", "ocapi_test.config.xml");

        public Application ReplaceConfigFile_O15(Application app, string sfbPath)
        {
            //Close Sfb
            if (app != null)
                app.Close();
            Thread.Sleep(3000);
            //Create config file
            string configFile = "ocapi_test.config.xml";
            var sfbProgramFilesConfigPath = Path.Combine(sfbRootPath_O15, configFile);
            //Copy Config file
            using (var fileStream = new FileStream(sfbProgramFilesConfigPath, FileMode.Create))
            {
                byte[] binaryData = null;
                using (var originalConfigStream = new FileStream(configFilePath, FileMode.Open))
                {
                    binaryData = new byte[originalConfigStream.Length];
                    originalConfigStream.Read(binaryData, 0, (int)originalConfigStream.Length);
                }
                fileStream.Write(binaryData, 0, binaryData.Length);
            }

            Thread.Sleep(2000);
            Application sfb = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(sfbPath));
            Thread.Sleep(8000);
            return sfb;
        }

        public Application ReplaceConfigFile_O16(Application app, string sfbPath)
        {
            //Close Sfb
            if (app != null)
                app.Close();
            Thread.Sleep(3000);
            //Create config file
            string configFile = "ocapi_test.config.xml";
            var sfbProgramFilesConfigPath = Path.Combine(sfbRootPath_O16, configFile);
            //Copy Config file
            using (var fileStream = new FileStream(sfbProgramFilesConfigPath, FileMode.Create))
            {
                byte[] binaryData = null;
                using (var originalConfigStream = new FileStream(configFilePath, FileMode.Open))
                {
                    binaryData = new byte[originalConfigStream.Length];
                    originalConfigStream.Read(binaryData, 0, (int)originalConfigStream.Length);
                }
                fileStream.Write(binaryData, 0, binaryData.Length);
            }

            Thread.Sleep(2000);
            Application sfb = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(sfbPath));
            Thread.Sleep(8000);
            return sfb;
        }

        public void RemoveConfigFile_O15(Application app)
        {
            //Close Sfb
            if (app != null)
                app.Close();
            Thread.Sleep(3000);
            //Remove the config file if exists
            string configFile = "ocapi_test.config.xml";
            var sfbProgramFilesConfigPath = Path.Combine(sfbRootPath_O15, configFile);
            File.SetAttributes(sfbProgramFilesConfigPath, FileAttributes.Normal);
            if (File.Exists(sfbProgramFilesConfigPath))
                File.Delete(sfbProgramFilesConfigPath);
        }

        public void RemoveConfigFile_O16(Application app)
        {
            //Close Sfb
            if (app != null)
                app.Close();
            Thread.Sleep(3000);
            //Remove the config file if exists
            string configFile = "ocapi_test.config.xml";
            var sfbProgramFilesConfigPath = Path.Combine(sfbRootPath_O16, configFile);
            File.SetAttributes(sfbProgramFilesConfigPath, FileAttributes.Normal);
            if (File.Exists(sfbProgramFilesConfigPath))
                File.Delete(sfbProgramFilesConfigPath);
        }
    }
}
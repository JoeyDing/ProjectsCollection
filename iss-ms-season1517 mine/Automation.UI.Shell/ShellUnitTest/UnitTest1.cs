using Automation.UI.Shell.Wpf.Infrastructure;
using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShellUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Serialize_Test()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testXmlConfig");
            string path = Path.Combine(folderPath, "emailConfigReflection.xml");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var serializeService = new SerializeService();
            var configObject = new EmailSettingReflection();
            configObject.EmailFrom = "ss";
            configObject.EmailServer = "aa";
            configObject.EmailToList = new List<string>() { "cc", "dd" };
            configObject.EmailCcList = new List<string>() { "jj", "kk" };
            configObject.IsSent = false;
            configObject.TSoptions = TestCasesOptions.SendIM;
            configObject.AccountName = "";
            serializeService.Serialize(path, configObject);
        }

        [TestMethod]
        public void Deserialize_Test()
        {
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testXmlConfig");
            string path = Path.Combine(folderPath, "emailConfigReflection.xml");
            var deserialize = new DeserializeService();
            var emailsettingObject = deserialize.Deserialize(path, typeof(EmailSettingReflection));
        }

        [TestMethod]
        public void SaveToExcel_Test()
        {
        }
    }
}
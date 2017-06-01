using Automation.UI.Shell.Wpf.Infrastructure;
using Automation.UI.Shell.Wpf.Infrastructure.ConfigPopUp;
using Automation.UI.Shell.Wpf.Infrastructure.Core;
using Automation.UI.Shell.Wpf.Infrastructure.Core.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ShellUnitTest
{
    [TestClass]
    public class MockTest
    {
        [TestMethod]
        public void LoadDict_Test()
        {
            //arrange

            string fileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emailConfigReflection.xml");
            //reflection
            var deserializeService = new DeserializeService();
            var configxmlObject = deserializeService.Deserialize(fileName, typeof(EmailSettingReflection));

            var loadDictServiceMoq = new LoadDictForPopupService(new ConfigPopUpControl());
            int index = 0;
            var dict = new Dictionary<string, List<string>>();
            Grid dynamicGrid = new Grid();

            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            loadDictServiceMoq.LoadControlsRecurrsively(dynamicGrid, configxmlObject, ref index, dict);
            //assert
            Assert.AreEqual(dict.Any(), true);
        }
    }
}
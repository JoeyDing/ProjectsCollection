using Sfb.Core.Interfaces;
using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System.IO;
using TestStack.White;
using Sfb.Core.Services;

namespace Sfb.Core.Services.O15
{
    public class MyPictureService_O15 : IMyPictureTest
    {
        private readonly IExecuteAction execute;
        private readonly IRunApp runApp;
        private readonly IGetSfbClientWindow getSfbClientWindow;
        private readonly ICloseAllModals closeAllModals;
        private readonly IGetOptionWindow getOptionWindow;
        private readonly ISelectOptionWindowTabByIndex selectOptionWindowTabByIndex;
        private readonly ITakeScreenshotTestStack takeScreenShot;
        private readonly ITakeScreenshotFull takeFullScreenShot;
        private readonly ITestCaseImageStore testCaseImageStore;
        private readonly string resultFolderPath15;
        private readonly string sfbPath;
        private ConfigFileOperation configFileOperation = new ConfigFileOperation();

        public MyPictureService_O15(IExecuteAction _execute,
            IRunApp _runApp,
            IGetSfbClientWindow _getSfbClientWindow,
            ICloseAllModals _closeAllModals,
            IGetOptionWindow _getOptionWindow,
            ISelectOptionWindowTabByIndex _selectOptionWindowTabByIndex,
            ITakeScreenshotTestStack _takeScreenShot,
            ITakeScreenshotFull _takeFullScreenShot,
              string _resultFolderPath15,
            string _sfbPath,
            ITestCaseImageStore _testCaseImageStore)
        {
            execute = _execute;
            runApp = _runApp;
            getSfbClientWindow = _getSfbClientWindow;
            closeAllModals = _closeAllModals;
            getOptionWindow = _getOptionWindow;
            selectOptionWindowTabByIndex = _selectOptionWindowTabByIndex;
            takeScreenShot = _takeScreenShot;
            takeFullScreenShot = _takeFullScreenShot;
            resultFolderPath15 = _resultFolderPath15;
            sfbPath = _sfbPath;
            testCaseImageStore = _testCaseImageStore;
        }

        public bool Options_MyPicture(string languageShortName, TestCase testCase)
        {
            var app = execute.ExecuteAction(
                "get instance of app", () =>
                {
                    return runApp.RunApp(this.sfbPath);
                }
            );

            //Close Sfb and replace the config file
            Application sfb = configFileOperation.ReplaceConfigFile_O15(app, this.sfbPath);

            Window sfbClient = execute.ExecuteAction(
                "get sfb client window", () =>
                {
                    return getSfbClientWindow.GetSfbClientWindow(sfb);
                }
            );

            Window option_window = execute.ExecuteAction(
               "get option window", () =>
               {
                   return getOptionWindow.GetOptionWindow(sfbClient, sfb);
               }
           );

            UIItemCollection items = execute.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(3, option_window);
               }
           );

            //Check radio button 'Show a picture from a webside'
            var radioButton = option_window.Get<RadioButton>(SearchCriteria.ByAutomationId("1011"));
            if (!radioButton.Enabled)
            {
                return false;
            }
            execute.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    takeScreenShot.SetUIItem(option_window);
                    takeScreenShot.TakeScreenShot();

                    string langFolder = Path.Combine(this.resultFolderPath15, languageShortName);
                    if (!Directory.Exists(langFolder))
                        Directory.CreateDirectory(langFolder);

                    string filePath = Path.Combine(langFolder, "MyPicture.png");
                    takeScreenShot.SaveScreenshot(filePath, takeScreenShot.TakeScreenShot().Screenshot);
                    TestCaseImageStoreItem testCaseImageStoreItem = new TestCaseImageStoreItem();
                    testCaseImageStoreItem.Image = takeScreenShot.TakeScreenShot().Screenshot;
                    testCaseImageStoreItem.Path = filePath;
                    testCaseImageStoreItem.TestCase = testCase;
                    testCaseImageStore.AddItem(testCaseImageStoreItem);
                }
            );

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, sfb);
                }
            );

            //Remove config file
            configFileOperation.RemoveConfigFile_O15(sfb);

            return true;
        }
    }
}
using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services.O16
{
    public class CallHandling_Service_O16 : ICallHandlingTest
    {
        private readonly IExecuteAction execute;
        private readonly IRunApp runApp;
        private readonly IGetSfbClientWindow getSfbClientWindow;
        private readonly ICloseAllModals closeAllModals;
        private readonly IGetOptionWindow getOptionWindow;
        private readonly ISelectOptionWindowTabByIndex selectOptionWindowTabByIndex;
        private readonly ITakeScreenshotTestStack takeScreenShot;
        private readonly ITakeScreenshotFull takeFullScreenShot;
        private ConfigFileOperation configFileOperation = new ConfigFileOperation();
        private readonly ITestCaseImageStore testCaseImageStore;
        private readonly string resultFolderPath16;
        private readonly string sfbPath;
        private static int index = 11;
        private static int checkBoxAutoID = 1030;

        public CallHandling_Service_O16(IExecuteAction _execute,
            IRunApp _runApp,
            IGetSfbClientWindow _getSfbClientWindow,
            ICloseAllModals _closeAllModals,
            IGetOptionWindow _getOptionWindow,
            ISelectOptionWindowTabByIndex _selectOptionWindowTabByIndex,
            ITakeScreenshotTestStack _takeScreenShot,
            ITakeScreenshotFull _takeFullScreenShot,
            string _resultFolderPath16,
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
            resultFolderPath16 = _resultFolderPath16;
            sfbPath = _sfbPath;
            testCaseImageStore = _testCaseImageStore;
        }

        public bool Options_CallHandling(string languageShortName, TestCase testCase)
        {
            var app = execute.ExecuteAction(
                "get instance of app", () =>
                {
                    return runApp.RunApp(this.sfbPath);
                }
            );

            //Close Sfb and replace the config file
            Application sfb = configFileOperation.ReplaceConfigFile_O16(app, this.sfbPath);

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
               "get call handling tab elements in option window", () =>
               {
                   return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(index, option_window);
               }
           );
            //Get checkbox

            CheckBox outgoing_calls = execute.ExecuteAction(
               "get call handling tab checkbox element", () =>
               {
                   return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId(checkBoxAutoID.ToString()));
                   //return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("1030"));
                   //return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("1020"));
               }
           );
            outgoing_calls.Click();
            Thread.Sleep(3000);
            execute.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    //Check if the 'Outgoing Calls' is checked
                    if (!outgoing_calls.Checked)
                    {
                        //Check the checkbox
                        outgoing_calls.Click();
                    }
                    {
                        takeScreenShot.SetUIItem(option_window);
                        takeScreenShot.TakeScreenShot();

                        string langFolder = Path.Combine(this.resultFolderPath16, languageShortName);
                        if (!Directory.Exists(langFolder))
                            Directory.CreateDirectory(langFolder);

                        string filePath = Path.Combine(langFolder, "Options_CallHandling.png");
                        takeScreenShot.SaveScreenshot(filePath, takeScreenShot.TakeScreenShot().Screenshot);
                        TestCaseImageStoreItem testCaseImageStoreItem = new TestCaseImageStoreItem();
                        testCaseImageStoreItem.Image = takeScreenShot.TakeScreenShot().Screenshot;
                        testCaseImageStoreItem.Path = filePath;
                        testCaseImageStoreItem.TestCase = testCase;
                        testCaseImageStore.AddItem(testCaseImageStoreItem);
                    }
                }
               );

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, sfb);
                }
            );
            //Remove config file
            configFileOperation.RemoveConfigFile_O16(sfb);
            return true;
        }
    }
}
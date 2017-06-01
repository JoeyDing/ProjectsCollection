﻿using Microsoft.Practices.Unity;
using Sfb.Core.Interfaces;
using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System.IO;

namespace Sfb.Core.Services.O16
{
    public class SkypeMeetingsService_O16 : ISkypeMeetingTest
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
        private readonly string resultFolderPath16;
        private readonly string sfbPath;
        private static int tabIndex = 15;

        public SkypeMeetingsService_O16(IExecuteAction _execute,
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

        public bool Options_SkypeMeetings(string languageShortName, TestCase testCase)
        {
            var app = execute.ExecuteAction(
                "get instance of app", () =>
                {
                    return runApp.RunApp(this.sfbPath);
                }
            );

            Window sfbClient = execute.ExecuteAction(
                "get sfb client window", () =>
                {
                    return getSfbClientWindow.GetSfbClientWindow(app);
                }
            );

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = execute.ExecuteAction(
               "get option window", () =>
               {
                   return getOptionWindow.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = execute.ExecuteAction(
                           "get Skype Meetings tab elements in option window", () =>
                           {
                               return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(tabIndex, option_window);
                           }
                       );

            execute.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    takeScreenShot.SetUIItem(option_window);
                    takeScreenShot.TakeScreenShot();

                    string langFolder = Path.Combine(this.resultFolderPath16, languageShortName);
                    if (!Directory.Exists(langFolder))
                        Directory.CreateDirectory(langFolder);

                    string filePath = Path.Combine(langFolder, "SkypeMeetings_SkypeMeetingsTab.png");
                    takeScreenShot.SaveScreenshot(filePath, takeScreenShot.TakeScreenShot().Screenshot);
                    TestCaseImageStoreItem testCaseImageStoreItem = new TestCaseImageStoreItem();
                    testCaseImageStoreItem.Image = takeScreenShot.TakeScreenShot().Screenshot;
                    testCaseImageStoreItem.Path = filePath;
                    testCaseImageStoreItem.TestCase = testCase;
                    testCaseImageStore.AddItem(testCaseImageStoreItem);
                }
            );

            execute.ExecuteAction(
                "Click Change Button", () =>
                {
                    Button changeBtn = items.FirstOrDefault(c => c.AutomationElement.Current.AutomationId == "496") as Button;
                    if (changeBtn != null)
                    {
                        changeBtn.Click();
                        Thread.Sleep(3000);
                    }
                    else
                        throw new Exception("Cannot find Change button");
                }
            );

            execute.ExecuteAction(
                "Click ok button on popup", () =>
                {
                    //var windows = app.GetWindows();
                    Button okButton = option_window.Get<Button>(SearchCriteria.ByAutomationId("Button0"));
                    okButton.Click();
                    Thread.Sleep(2000);
                }
            );

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }
    }
}
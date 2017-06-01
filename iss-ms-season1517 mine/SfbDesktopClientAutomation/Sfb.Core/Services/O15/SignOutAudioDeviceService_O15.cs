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

namespace Sfb.Core.Services.O15
{
    public class SignOutAudioDeviceService_O15 : ISignOutAudioDeviceTest
    {
        private readonly IExecuteAction execute;
        private readonly IRunApp runApp;
        private readonly IGetSfbClientWindow getSfbClientWindow;
        private readonly ICloseAllModals closeAllModals;
        private readonly IGetOptionWindow getOptionWindow;
        private readonly ISelectOptionWindowTabByIndex selectOptionWindowTabByIndex;
        private readonly ITakeScreenshotTestStack takeScreenShot;
        private readonly ITakeScreenshotFull takeFullScreenShot;
        private readonly IGetOptionWindow_LogOut getOptionLogOut;
        private readonly ISign_In signIn;
        private readonly ITestCaseImageStore testCaseImageStore;
        private readonly string resultFolderPath15;
        private readonly string sfbPath;

        public SignOutAudioDeviceService_O15(IExecuteAction _execute,
            IRunApp _runApp,
            IGetSfbClientWindow _getSfbClientWindow,
            ICloseAllModals _closeAllModals,
            IGetOptionWindow _getOptionWindow,
            ISelectOptionWindowTabByIndex _selectOptionWindowTabByIndex,
            ITakeScreenshotTestStack _takeScreenShot,
            ITakeScreenshotFull _takeFullScreenShot,
            IGetOptionWindow_LogOut _getOptionLogOut,
            ISign_In _signIn,
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
            getOptionLogOut = _getOptionLogOut;
            signIn = _signIn;
            resultFolderPath15 = _resultFolderPath15;
            sfbPath = _sfbPath;
            testCaseImageStore = _testCaseImageStore;
        }

        public bool Options__SignOut_Audio_Device(string languageShortName, TestCase testCase)
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
               "log out", () =>
               {
                   return getOptionLogOut.GetOptionWindow_LogOut(sfbClient, app);
               }
           );

            UIItemCollection items = execute.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(1, option_window);
               }
           );

            execute.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    takeScreenShot.SetUIItem(option_window);
                    takeScreenShot.TakeScreenShot();

                    string langFolder = Path.Combine(this.resultFolderPath15, languageShortName);
                    if (!Directory.Exists(langFolder))
                        Directory.CreateDirectory(langFolder);

                    string filePath = Path.Combine(langFolder, "Audio Device Sign Out_Audio Device Sign Out.png");
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
                    closeAllModals.CloseAllModals(sfbClient, app);
                }
            );

            execute.ExecuteAction(
                "Sign in", () =>
                {
                    signIn.Sign_In(sfbClient);
                }
            );

            return true;
        }
    }
}
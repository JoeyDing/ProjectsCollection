using Sfb.Core.Interfaces;
using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System.IO;
using System.Diagnostics;

namespace Sfb.Core.Services.O16
{
    public class RecordingPublishRecordingService_O16 : IRecordingPublishRecordingTest
    {
        private readonly IExecuteAction execute;
        private readonly IRunApp runApp;
        private readonly IGetSfbClientWindow getSfbClientWindow;
        private readonly ICloseAllModals closeAllModals;
        private readonly IGetOptionWindow getOptionWindow;
        private readonly ISelectOptionWindowTabByIndex selectOptionWindowTabByIndex;
        private readonly ITakeScreenshotTestStack takeScreenShot;
        private readonly ITakeScreenshotFull takeFullScreenShot;
        private readonly ITraverseItemNative traverse;
        private readonly IRunAppForRecordingManager runForRecordingManager;
        private readonly ITestCaseImageStore testCaseImageStore;
        private readonly string resultFolderPath16;
        private readonly string sfbPath;

        public RecordingPublishRecordingService_O16(IExecuteAction _execute,
            IRunApp _runApp,
            IGetSfbClientWindow _getSfbClientWindow,
            ICloseAllModals _closeAllModals,
            IGetOptionWindow _getOptionWindow,
            ISelectOptionWindowTabByIndex _selectOptionWindowTabByIndex,
            ITakeScreenshotTestStack _takeScreenShot,
            ITakeScreenshotFull _takeFullScreenShot,
            ITraverseItemNative _traverse,
            IRunAppForRecordingManager _runForRecordingManager,
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
            traverse = _traverse;
            runForRecordingManager = _runForRecordingManager;
            resultFolderPath16 = _resultFolderPath16;
            sfbPath = _sfbPath;
            testCaseImageStore = _testCaseImageStore;
        }

        public bool Recording_Publish_Recording(string languageShortName, TestCase testCase)
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

            #region Uncheck the checkbox in 'Options -> Skype Meetings' to ensure there will no 'OK/Cancel' window appears.

            Window option_window = execute.ExecuteAction(
               "get option window", () =>
               {
                   return getOptionWindow.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = execute.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(14, option_window);
               }
           );

            //Get checkbox

            CheckBox checkboxformeeting = execute.ExecuteAction(
               "get Skype Meetings tab checkbox element", () =>
               {
                   return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("92"));
               }
           );
            //Check if the Checkbox is checked
            if (checkboxformeeting.Checked)
            {
                //Uncheck the checkbox
                checkboxformeeting.Click();
            }

            Button ConfirmButton = execute.ExecuteAction(
               "Click 'OK' to save changes", () =>
               {
                   return option_window.Get<Button>(SearchCriteria.ByAutomationId("1"));
               }
           );
            ConfirmButton.Click();
            Thread.Sleep(3000);

            #endregion Uncheck the checkbox in 'Options -> Skype Meetings' to ensure there will no 'OK/Cancel' window appears.

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[1] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[6] as Button;
            button_location.Click();

            Thread.Sleep(3000);

            var traverseItemNative = traverse.TraverseItemNative(pane_contact.AutomationElement);
            if (sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Group)).Length > 0)
            {
                var traverseItemNativeMeetNow = traverseItemNative.Item2.Items[11].Items[2].Items[0].Items[0].Items[1];
                //create wrapper for element
                var meetNowWrapperShowMenu = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeMeetNow.Element, pane_contact.ActionListener);
                meetNowWrapperShowMenu.Click();
                Thread.Sleep(2000);
                try
                {
                    traverseItemNative = traverse.TraverseItemNative(meetNowWrapperShowMenu.AutomationElement);
                    var traverseItemNativeMeetNow_subList = traverseItemNative.Item2.Items[0].Items[0].Items[0].Items[0];
                    var meetNowWrapperForMyself = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeMeetNow_subList.Element, pane_contact.ActionListener);
                    meetNowWrapperForMyself.Click();
                    Thread.Sleep(10000);
                }
                catch
                {
                }
            }
            else
            {
                var traverseItemNativeMeetNow = traverseItemNative.Item2.Items[10].Items[2].Items[0].Items[0].Items[1];
                //create wrapper for element
                var meetNowWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeMeetNow.Element, pane_contact.ActionListener);
                meetNowWrapper.Click();
                Thread.Sleep(10000);
            }

            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            Window communication_window = windows.First();
            var com_panes = communication_window.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            var menu_pane = com_panes[1] as Panel;

            var menu_button = menu_pane.Items.Last();
            Thread.Sleep(5000);
            menu_button.Click();
            Thread.Sleep(6000);

            //Start recording
            var traversedResult = traverse.TraverseItemNative(menu_button.AutomationElement);
            var traverseItemNativeStartRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[0];
            Thread.Sleep(1000);
            //create wrapper for element
            var startRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeStartRecording.Element, menu_button.ActionListener);
            startRecordingWrapper.Click();
            Thread.Sleep(1000);

            //Stop recording
            menu_button.Click();
            Thread.Sleep(1000);
            traversedResult = traverse.TraverseItemNative(menu_button.AutomationElement);
            var traverseItemNativeStopRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[1];
            //create wrapper for element
            var stopRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeStopRecording.Element, menu_button.ActionListener);
            stopRecordingWrapper.Click();
            Thread.Sleep(1000);

            //Recording manager
            menu_button.Click();
            Thread.Sleep(1000);
            traversedResult = traverse.TraverseItemNative(menu_button.AutomationElement);
            var traverseItemNativeManageRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[1];
            //create wrapper for element
            var manageRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeManageRecording.Element, menu_button.ActionListener);
            manageRecordingWrapper.Click();
            Thread.Sleep(5000);

            //Get Recording Manager window
            var appForRecording = execute.ExecuteAction(
                "get instance of app", () =>
                {
                    return runForRecordingManager.RunAppForRecordingManager();
                }
            );
            Thread.Sleep(5000);
            //Get recording window
            var recording_windows = appForRecording.GetWindows();
            Window recordingmanagerwindow = null;
            try
            {
                recordingmanagerwindow = recording_windows.First();
            }
            catch (Exception)
            {
                //terminate process
                var sfbProcess = Process.GetProcessesByName("OcPubMgr").FirstOrDefault();
                if (sfbProcess != null)
                {
                    sfbProcess.Kill();
                    Thread.Sleep(3000);
                }
            }

            var publish_button = recordingmanagerwindow.Get<Button>(SearchCriteria.ByAutomationId("idExportBtn"));
            while (!publish_button.Enabled)
            {
                Thread.Sleep(1000);
            }
            publish_button.Click();
            Thread.Sleep(3000);

            //Get publish window
            var recording_windows_publishWindow = appForRecording.GetWindows().Where(w => w.Name != sfbClient.Name);
            Window publishWindow = recording_windows_publishWindow.Last();
            var buttonTypes = publishWindow.GetMultiple(SearchCriteria.ByControlType(ControlType.Button));
            //Take screen shot of publish window to ensure the 'Publish' button works.
            execute.ExecuteAction(
            "take a screenshot of window", () =>
            {
                takeFullScreenShot.SetProcessName("lync");
                var screenshot = takeFullScreenShot.TakeScreenShot().Screenshot;
                Thread.Sleep(3000);
                string langFolder = Path.Combine(this.resultFolderPath16, languageShortName);
                if (!Directory.Exists(langFolder))
                    Directory.CreateDirectory(langFolder);

                string filePath = Path.Combine(langFolder, "Publish_Recording.png");
                takeFullScreenShot.SaveScreenshot(filePath, takeFullScreenShot.TakeScreenShot().Screenshot);
                TestCaseImageStoreItem testCaseImageStoreItem = new TestCaseImageStoreItem();
                testCaseImageStoreItem.Image = screenshot;
                testCaseImageStoreItem.Path = filePath;
                testCaseImageStoreItem.TestCase = testCase;
                testCaseImageStore.AddItem(testCaseImageStoreItem);
            }
            );
            //Push 'Cancel' button to close the publish window
            var publish_cancelButton = buttonTypes[3] as Button;
            publish_cancelButton.Click();

            //All windows
            var all_windows = app.GetWindows();

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, app);
                }
            );
            Thread.Sleep(2000);
            communication_window.KeyIn(KeyboardInput.SpecialKeys.LEFT);
            Thread.Sleep(1000);
            communication_window.KeyIn(KeyboardInput.SpecialKeys.RETURN);
            Thread.Sleep(5000);
            //Get feedback window,check if the feedback window appears.
            if (app.GetWindows().Where(w => w.Name != sfbClient.Name).Count() > 0)
            {
                var feedback_windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
                Window feedbackWindow = feedback_windows.First();
                var confirm_closeWindow_panes = feedbackWindow.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
                var panel0 = confirm_closeWindow_panes[0] as Panel;
                var buttons = panel0.GetMultiple(SearchCriteria.ByControlType(ControlType.Button));
                Button close_button = buttons[6] as Button;
                close_button.Click();
            }
            Thread.Sleep(1000);
            appForRecording.Close();
            Thread.Sleep(3000);
            return true;
        }
    }
}
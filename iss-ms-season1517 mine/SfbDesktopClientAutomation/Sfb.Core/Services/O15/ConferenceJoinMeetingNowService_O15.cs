using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace Sfb.Core.Services.O15
{
    public class ConferenceJoinMeetingNowService_O15 : IConferenceJoinMeetingNowTest
    {
        private readonly IExecuteAction execute;
        private readonly IRunApp runApp;
        private readonly IGetSfbClientWindow getSfbClientWindow;
        private readonly ICloseAllModals closeAllModals;
        private readonly IGetOptionWindow getOptionWindow;
        private readonly ISelectOptionWindowTabByIndex selectOptionWindowTabByIndex;
        private readonly ITakeScreenshotTestStack takeScreenShot;
        private readonly ITraverseItemNative traverse;
        private readonly ITakeScreenshotFull takeFullScreenShot;
        private readonly ITestCaseImageStore testCaseImageStore;
        private readonly string resultFolderPath15;
        private readonly string sfbPath;

        public ConferenceJoinMeetingNowService_O15(IExecuteAction _execute,
            IRunApp _runApp,
            IGetSfbClientWindow _getSfbClientWindow,
            ICloseAllModals _closeAllModals,
            IGetOptionWindow _getOptionWindow,
            ISelectOptionWindowTabByIndex _selectOptionWindowTabByIndex,
            ITakeScreenshotTestStack _takeScreenShot,
            ITraverseItemNative _traverse,
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
            traverse = _traverse;
            resultFolderPath15 = _resultFolderPath15;
            sfbPath = _sfbPath;
            testCaseImageStore = _testCaseImageStore;
        }

        public bool ConferenceJoin_MeetNow(string languageShortName, TestCase testCase)
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
            //Check if the Checkbox is unchecked
            if (checkboxformeeting.Checked)
            {
                //Check the checkbox
                checkboxformeeting.Checked = false;
            }

            Button ConfirmButton = execute.ExecuteAction(
               "Click 'OK' to save changes", () =>
               {
                   return option_window.Get<Button>(SearchCriteria.ByAutomationId("1"));
               }
           );
            ConfirmButton.Click();
            Thread.Sleep(3000);

            #region Show Menu -> Meet Now to open meeting conversation window

            //sfbClient.KeyIn(KeyboardInput.SpecialKeys.ALT);
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[3] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[1] as Button;
            button_location.Click();
            Thread.Sleep(5000);
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
                    var traverseItemNativeMeetNow_subList = traverseItemNative.Item2.Items[1].Items[0].Items[0].Items[0];
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

            #endregion Show Menu -> Meet Now to open meeting conversation window

            //Get the 'Conversation(1 Participate)' window and Click the 'Cancel' button to close the window
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            Window conversation_window = windows.First();
            var conversation_window_panes = conversation_window.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            var join_meeting_pane = conversation_window_panes[0] as Panel;
            var cancel_button = join_meeting_pane.Items[14] as Button;
            execute.ExecuteAction(
                    "Take a full screenshot of window", () =>
                    {
                        takeFullScreenShot.SetProcessName("lync");
                        var screenshot = takeFullScreenShot.TakeScreenShot().Screenshot;
                        Thread.Sleep(3000);
                        string langFolder = Path.Combine(this.resultFolderPath15, languageShortName);
                        if (!Directory.Exists(langFolder))
                            Directory.CreateDirectory(langFolder);

                        string filePath = Path.Combine(langFolder, "Conference_Meeting_Now.png");
                        takeFullScreenShot.SaveScreenshot(filePath, takeFullScreenShot.TakeScreenShot().Screenshot);
                        TestCaseImageStoreItem testCaseImageStoreItem = new TestCaseImageStoreItem();
                        testCaseImageStoreItem.Image = screenshot;
                        testCaseImageStoreItem.Path = filePath;
                        testCaseImageStoreItem.TestCase = testCase;
                        testCaseImageStore.AddItem(testCaseImageStoreItem);
                    }
                );
            if (cancel_button != null)
            {
                cancel_button.Click();
            }

            execute.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    closeAllModals.CloseAllModals(sfbClient, app);
                }
            );

            //close the small popped up window
            conversation_window.KeyIn(KeyboardInput.SpecialKeys.LEFT);
            Thread.Sleep(1000);
            conversation_window.KeyIn(KeyboardInput.SpecialKeys.RETURN);
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
            return true;
        }
    }
}
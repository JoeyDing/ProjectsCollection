using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Sfb.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace Sfb.Core.Services.O16
{
    public class Call_Forwarding_Setting_Service_O16 : ICallForwardingSettingTest
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

        public Call_Forwarding_Setting_Service_O16(IExecuteAction _execute,
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

        public bool Call_Forwarding_Setting(string languageShortName, TestCase testCase)
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
                   return selectOptionWindowTabByIndex.SelectOptionWindowTabByIndex(11, option_window);
               }
           );
            //Get radio button
            var radio_button = option_window.Get<RadioButton>(SearchCriteria.ByAutomationId("184"));
            //Click the radio button
            radio_button.Click();
            //Get checkbox
            var combobox = option_window.Get<ComboBox>(SearchCriteria.ByAutomationId("191"));
            //Dropdown combobox and select the item 'New Number'
            if (combobox.Items.Count == 4)
            {
                combobox.Items[1].Click();
            }
            else
                combobox.Items[0].Click();
            //Get window 'Edit phone number'
            var all_edit_phone_number = app.GetWindows().Select(c => c.Name).ToList();
            Window edit_phone_number;
            IUIItem phone_number;
            try
            {
                edit_phone_number = app.GetWindows()[3];
                //var edit_phone_number = app.GetWindows()[2];
                //Get EditText 'phone number'
                phone_number = edit_phone_number.Get(SearchCriteria.ByAutomationId("314"));
            }
            catch
            {
                edit_phone_number = app.GetWindows()[2];
                phone_number = edit_phone_number.Get(SearchCriteria.ByAutomationId("314"));
            }
            //Type new phone number to the EditText 'phone number'
            phone_number.Enter("18888888888");
            //get 'OK' button
            var OK_button = option_window.Get<Button>(SearchCriteria.ByAutomationId("1"));
            //click 'OK' button
            OK_button.Click();
            execute.ExecuteAction(
            "make a screenshot of window", () =>
            {
                takeScreenShot.SetUIItem(option_window);
                takeScreenShot.TakeScreenShot();

                string langFolder = Path.Combine(this.resultFolderPath16, languageShortName);
                if (!Directory.Exists(langFolder))
                    Directory.CreateDirectory(langFolder);

                string filePath = Path.Combine(langFolder, "Call_Forwarding_Setting.png");
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
            return true;
        }
    }
}
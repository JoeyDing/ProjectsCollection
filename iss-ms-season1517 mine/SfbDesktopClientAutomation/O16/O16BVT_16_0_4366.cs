using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Microsoft.Practices.Unity;
using Sfb.Core;
using Sfb.Core.Interfaces;
using Sfb.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace O16
{
    public class O16BVT_16_0_4366 : SfbBVT,
         IAlertTest,
        ICallForwardingSettingTest,
        ICallHandlingTest,
        IConferenceJoinMeetingNowTest,
        IGeneralTest,
        IIMEmoticonsTest,
        IIMTest,
        IMyPictureTest,
        IRingtonesAndSoundsTest,
        ISignOutAudioDeviceTest,
        ISkypeMeetingTest,
        IStatusTest,
        IVideoDeviceTest,
        IPersonalGTest,
        IRecordingPublishRecordingTest,
        IRecordingTest,
        ISetResultFolderPath
    {
        public O16BVT_16_0_4366(string resultFolderPath)
            : base(resultFolderPath)
        {
        }

        protected override Window GetOptionWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane index 2
            var pane_contact = panes[2] as Panel;

            //get option button inside pane
            var button_option = pane_contact.Items[0] as Button;
            // only open the option modal window
            button_option.Click();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }

        public string ResultFolderPath
        {
            get
            {
                return this._resultFolderPath;
            }

            set
            {
                this._resultFolderPath = value;
            }
        }

        protected override string RecordingManagerPath
        {
            get
            {
                return Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office16\OcPubMgr.exe");
            }
        }

        protected override string SfbPath
        {
            get
            {
                return Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office16\lync.exe");
            }
        }

        public override bool Options_CallHandling(string languageShortName, TestCase testcase)
        {
            return base.Options_CallHandling(11, 1030);
        }

        public override bool Options_SkypeMeetings(string languageShortName, TestCase testcase)
        {
            return base.Options_SkypeMeetings(15);
        }

        public override bool Recording_Publish_Recording(string languageShortName, TestCase testcase)
        {
            var app = base.ExecuteAction(
                "get instance of app", () =>
                {
                    return this.RunApp(this.SfbPath);
                }
            );

            Window sfbClient = this.ExecuteAction(
                "get sfb client window", () =>
                {
                    return this.GetSfbClientWindow(app);
                }
            );

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[2] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[1] as Button;
            button_location.Click();
            Thread.Sleep(1000);

            //click "Meet now"
            var traverseItemNative = TraverseItemNative(pane_contact.AutomationElement);
            var traverseItemNativeMeetNow = traverseItemNative.Item2.Items[13].Items[2].Items[0].Items[0].Items[1];
            //create wrapper for element
            var meetNowWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeMeetNow.Element, pane_contact.ActionListener);
            meetNowWrapper.Click();
            Thread.Sleep(1000);
            //var point = new System.Windows.Point(button_location.Bounds.BottomRight.X, button_location.Bounds.BottomRight.Y + 40);
            //sfbClient.Mouse.Click(point);

            //click "with Myself"

            (pane_contact.Items[4] as Panel).Items[0].Click();
            //for O15 only maybe
            //Thread.Sleep(3000);
            //sfbClient.Mouse.Click(new System.Windows.Point(button_location.Bounds.BottomRight.X + 400, button_location.Bounds.BottomRight.Y + 40));

            //Get communication window
            var communication_windows = app.GetWindows();
            var communication_window = communication_windows[0];
            var com_panes = communication_window.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            Thread.Sleep(10000);
            var menu_pane = com_panes[1] as Panel;
            var menu_button = menu_pane.Items.Last();
            menu_button.Click();
            Thread.Sleep(6000);

            //Start recording
            var traversedResult = TraverseItemNative(menu_button.AutomationElement);

            var traverseItemNativeStartRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[0];
            //create wrapper for element
            var startRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeStartRecording.Element, menu_button.ActionListener);
            startRecordingWrapper.Click();
            Thread.Sleep(1000);

            //Stop recording
            menu_button.Click();
            Thread.Sleep(1000);
            traversedResult = TraverseItemNative(menu_button.AutomationElement);
            var traverseItemNativeStopRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[1];
            //create wrapper for element
            var stopRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeStopRecording.Element, menu_button.ActionListener);
            stopRecordingWrapper.Click();
            Thread.Sleep(1000);

            //Recording manager
            menu_button.Click();
            Thread.Sleep(1000);
            traversedResult = TraverseItemNative(menu_button.AutomationElement);
            var traverseItemNativeManageRecording = traversedResult.Item2.Items[0].Items[1].Items[0].Items[0].Items[1];
            //create wrapper for element
            var manageRecordingWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeManageRecording.Element, menu_button.ActionListener);
            manageRecordingWrapper.Click();
            var appForRecording = base.ExecuteAction(
                "get instance of app", () =>
                {
                    return this.RunAppForRecordingManager(this.RecordingManagerPath);
                }
            );
            var recordingManagerWindow = appForRecording.GetWindows().First();
            var publish_button = recordingManagerWindow.Get<Button>(SearchCriteria.ByAutomationId("idExportBtn"));
            while (!publish_button.Enabled)
            {
                Thread.Sleep(1000);
            }
            publish_button.Click();
            Thread.Sleep(5000);
            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(communication_window, "Recording_Publish_Recording.png");
               }
           );

            //All windows
            var all_windows = app.GetWindows();
            var cancelButton = recordingManagerWindow.Get<Button>(SearchCriteria.ByAutomationId("2"));
            cancelButton.Click();
            appForRecording.Close();
            //since during the period of closing all modals, any new modal might be poped up anytime,so we terminate the process directly.
            CloseSfbClientService closeSfbClientService = new CloseSfbClientService();
            closeSfbClientService.CloseSfbClient();
            return true;
        }
    }
}
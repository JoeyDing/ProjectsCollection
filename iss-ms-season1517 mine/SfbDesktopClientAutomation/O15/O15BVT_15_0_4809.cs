using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Microsoft.Practices.Unity;
using Sfb.Core;
using Sfb.Core.Interfaces;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace O15
{
    public class O15BVT_15_0_4809 : SfbBVT,
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
        public O15BVT_15_0_4809(string resultFolderPath)
            : base(resultFolderPath)
        {
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
                return Path.Combine(this.RootDrive, @"Program Files\Microsoft Office 15\root\office15\ocpubmgr.exe");
            }
        }

        protected override string SfbPath
        {
            get
            {
                return Path.Combine(this.RootDrive, @"Program Files\Microsoft Office\Office15\lync.exe");
            }
        }

        public override bool Options_CallHandling(string languageShortName, TestCase testcase)
        {
            return base.Options_CallHandling(11, 1020);
        }

        public override bool Options_SkypeMeetings(string languageShortName, TestCase testcase)
        {
            return base.Options_SkypeMeetings(14);
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
            var pane_contact = panes[3] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[1] as Button;
            button_location.Click();
            Thread.Sleep(1000);
            var point = new System.Windows.Point(button_location.Bounds.BottomRight.X, button_location.Bounds.BottomRight.Y + 40);
            sfbClient.Mouse.Click(point);
            //for O15 only maybe
            Thread.Sleep(3000);
            sfbClient.Mouse.Click(new System.Windows.Point(button_location.Bounds.BottomRight.X + 400, button_location.Bounds.BottomRight.Y + 40));
            Thread.Sleep(10000);
            //Get communication window
            var communication_window = app.GetWindow("Conversation (1 Participant)");
            var com_panes = communication_window.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            var menu_pane = com_panes[2] as Panel;
            var moreOptions_button = menu_pane.Items.Last();

            //start
            moreOptions_button.Click();
            Thread.Sleep(3000);
            var startRecording = new System.Windows.Point(moreOptions_button.Bounds.BottomRight.X - 40, moreOptions_button.Bounds.BottomRight.Y - 280);
            sfbClient.Mouse.Click(startRecording);
            Thread.Sleep(1000);

            //stop
            moreOptions_button.Click();
            Thread.Sleep(1000);
            var stopRecording = new System.Windows.Point(moreOptions_button.Bounds.BottomRight.X - 40, moreOptions_button.Bounds.BottomRight.Y - 285);
            sfbClient.Mouse.Click(stopRecording);
            Thread.Sleep(3000);

            //manage
            moreOptions_button.Click();
            Thread.Sleep(3000);
            var manageRecording = new System.Windows.Point(moreOptions_button.Bounds.BottomRight.X - 40, moreOptions_button.Bounds.BottomRight.Y - 270);
            sfbClient.Mouse.Click(manageRecording);
            Thread.Sleep(30000);

            //Get Recording Manager window
            var appForRecording = base.ExecuteAction(
                "get instance of app", () =>
                {
                    return this.RunAppForRecordingManager(this.RecordingManagerPath);
                }
            );
            var recordingManagerWindow = appForRecording.GetWindow("Skype for Business Recording Manager");
            var publish_button = recordingManagerWindow.Get<Button>(SearchCriteria.ByAutomationId("idExportBtn"));
            while (!publish_button.Enabled)
            {
                Thread.Sleep(1000);
            }
            publish_button.Click();
            Thread.Sleep(3000);

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeFullScreenShot(appForRecording, "Recording_Publish_Recording.png", ImageFormat.Png);
               }
           );

            //All windows
            var all_windows = app.GetWindows();

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            Thread.Sleep(1000);
            appForRecording.Close();
            Thread.Sleep(3000);

            return true;
        }
    }
}
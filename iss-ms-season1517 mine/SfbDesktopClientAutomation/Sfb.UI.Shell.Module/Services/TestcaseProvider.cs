using Automation.UI.Shell.Core;
using Automation.UI.Shell.TestStack;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner.Contracts;
using Microsoft.Practices.Unity;
using O15;
using O16;
using Sfb.Core.Interfaces;
using Sfb.Core.Interfaces.TestCases;
using Sfb.Core.Services;
using Sfb.Core.Services.O15;
using Sfb.Core.Services.O16;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Sfb.UI.Shell.Module.Services
{
    public class TestcaseProvider : ITestCasesProvider
    {
        private readonly IUnityContainer container;
        public static string OFFICE15 = "O15";
        public static string OFFICE16 = "O16";
        private string resultFolderPath15 = "";
        private string resultFolderPath16 = "";
        public string ResultFolderPath { get; set; }
        private string sfbPath_O15;
        private string sfbPath_O16;
        private string recordingManagerPath_O15;
        private string recordingManagerPath_O16;

        public string ResultFolderPath15
        {
            get
            {
                return this.resultFolderPath15;
            }
            set
            {
                this.resultFolderPath15 = value;
            }
        }

        public string ResultFolderPath16
        {
            get
            {
                return this.resultFolderPath16;
            }
            set
            {
                this.resultFolderPath16 = value;
            }
        }

        public TestcaseProvider(IUnityContainer container)
        {
            string rootDrive = Path.GetPathRoot(Environment.SystemDirectory);
            sfbPath_O15 = Path.Combine(rootDrive, @"Program Files\Microsoft Office\Office15\lync.exe");
            sfbPath_O16 = Path.Combine(rootDrive, @"Program Files\Microsoft Office\Office16\lync.exe");

            recordingManagerPath_O15 = Path.Combine(rootDrive, @"Program Files\Microsoft Office 15\root\office15\ocpubmgr.exe");
            recordingManagerPath_O16 = Path.Combine(rootDrive, @"Program Files\Microsoft Office\Office16\OcPubMgr.exe");

            this.container = container;

            //string resultFolderPath15 = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O15", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
            //string resultFolderPath16 = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, @"result\O16", DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") });
            ////register common
            this.container.RegisterType<ISign_In, Sign_InService>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<IExecuteAction, ExecuteActionService>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<IRunApp, RunAppService>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<ICloseAllModals, CloseAllModalsService>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<ITraverseItemNative, TraverseItemNativeService>(new ContainerControlledLifetimeManager());
            this.container.RegisterType<IGetSfbClientWindow, GetSfbClientWindowService>(new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ICloseAllModals>()));
            this.container.RegisterType<IGetOptionWindow_LogOut, GetOptionWindow_LogOut_Service_O15>(OFFICE15, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ITraverseItemNative>()));
            this.container.RegisterType<IGetOptionWindow_LogOut, GetOptionWindow_LogOut_Service_O16>(OFFICE16, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ITraverseItemNative>()));

            this.container.RegisterType<IGetOptionWindow, GetOptionWindowService_O15>(OFFICE15, new ContainerControlledLifetimeManager());
            this.container.RegisterType<IGetOptionWindow, GetOptionWindowService_O16>(OFFICE16, new ContainerControlledLifetimeManager());
            this.container.RegisterType<ISelectOptionWindowTabByIndex, SelectOptionWindowTabByIndexService>(new ContainerControlledLifetimeManager());

            this.container.RegisterType<IRunAppForRecordingManager, RunAppForRecordingManagerService>(OFFICE15, new ContainerControlledLifetimeManager(), new InjectionConstructor(recordingManagerPath_O15));
            this.container.RegisterType<IRunAppForRecordingManager, RunAppForRecordingManagerService>(OFFICE16, new ContainerControlledLifetimeManager(), new InjectionConstructor(recordingManagerPath_O16));

            this.container.RegisterType<ITakeScreenshot, TakeScreenshotServiceTestStack>();
            //this.container.RegisterType<ITakeScreenshotTestStack, TakeScreenshotServiceTestStack>(OFFICE15, new ContainerControlledLifetimeManager(), new InjectionConstructor(ResultFolderPath15));
            this.container.RegisterType<ITakeScreenshotTestStack, TakeScreenshotServiceTestStack>();
            this.container.RegisterType<ITakeScreenshotFull, TakeScreenshotFullService>();
            this.container.RegisterType<ITestCaseImageStore, TestCaseImageStoreService>();

            //prepare objects for testcase class instantiation
            var signIn = container.Resolve<ISign_In>();
            var execute = container.Resolve<IExecuteAction>();
            var runApp = container.Resolve<IRunApp>();
            var getSfb = container.Resolve<IGetSfbClientWindow>();
            var closeAllModual = container.Resolve<ICloseAllModals>();
            var getOption_O15 = container.Resolve<IGetOptionWindow>(OFFICE15);
            var getOption_O16 = container.Resolve<IGetOptionWindow>(OFFICE16);
            var selectOption = container.Resolve<ISelectOptionWindowTabByIndex>();
            var traverse = container.Resolve<ITraverseItemNative>();
            var runAppForRecordingManager_O15 = container.Resolve<IRunAppForRecordingManager>(OFFICE15);
            var runAppForRecordingManager_O16 = container.Resolve<IRunAppForRecordingManager>(OFFICE16);
            var getOptionLogOut_O15 = container.Resolve<IGetOptionWindow_LogOut>(OFFICE15);
            var getOptionLogOut_O16 = container.Resolve<IGetOptionWindow_LogOut>(OFFICE16);
            var testCaseImageStore_O15 = container.Resolve<ITestCaseImageStore>();
            var testCaseImageStore_O16 = container.Resolve<ITestCaseImageStore>();
            //O15

            this.container.RegisterType<IAlertTest, AlertsService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new AlertsService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IContactListTest, ContactListService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new ContactListService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IPhonesTest, PhonesService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PhonesService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IPersistentChatTest, PersistentChatService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PersistentChatService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<ICallForwardingSettingTest, Call_Forwarding_Setting_Service_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new Call_Forwarding_Setting_Service_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<ICallHandlingTest, CallHandling_Service_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new CallHandling_Service_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IConferenceJoinMeetingNowTest, ConferenceJoinMeetingNowService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new ConferenceJoinMeetingNowService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, traverse, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IGeneralTest, GeneralService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new GeneralService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IIMEmoticonsTest, IMEmoticonsService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new IMEmoticonsService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IMyPictureTest, MyPictureService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new MyPictureService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IPersonalGTest, PersonalGService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PersonalGService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IRecordingPublishRecordingTest, RecordingPublishRecordingService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RecordingPublishRecordingService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, traverse, runAppForRecordingManager_O15, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IRecordingTest, RecordingService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RecordingService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IRingtonesAndSoundsTest, RingtonesAndSoundsService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RingtonesAndSoundsService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<ISignOutAudioDeviceTest, SignOutAudioDeviceService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new SignOutAudioDeviceService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, getOptionLogOut_O15, signIn, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<ISkypeMeetingTest, SkypeMeetingsService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new SkypeMeetingsService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IStatusTest, StatusService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new StatusService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IVideoDeviceTest, VideoDeviceService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new VideoDeviceService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));

            this.container.RegisterType<IIMTest, IMService_O15>(OFFICE15, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new IMService_O15(execute, runApp, getSfb, closeAllModual, getOption_O15, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath15, this.sfbPath_O15, testCaseImageStore_O15);
            }));
            //this.container.RegisterType<ISetResultFolderPath, TakeScreenShotService>(OFFICE15, new ContainerControlledLifetimeManager());
            //this.container.RegisterType<ISetResultFolderPath_Full, TakeFullScreenShotService>(OFFICE15, new ContainerControlledLifetimeManager());

            //O16

            this.container.RegisterType<IAlertTest, AlertsService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new AlertsService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IContactListTest, ContactListService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new ContactListService_O15(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IPhonesTest, PhonesService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PhonesService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IPersistentChatTest, PersistentChatService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PersistentChatService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<ICallForwardingSettingTest, Call_Forwarding_Setting_Service_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new Call_Forwarding_Setting_Service_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<ICallHandlingTest, CallHandling_Service_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new CallHandling_Service_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IConferenceJoinMeetingNowTest, ConferenceJoinMeetingNowService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new ConferenceJoinMeetingNowService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, traverse, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IGeneralTest, GeneralService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeScreenFull = container.Resolve<ITakeScreenshotFull>();
                return new GeneralService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeScreenFull, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IIMEmoticonsTest, IMEmoticonsService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new IMEmoticonsService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IMyPictureTest, MyPictureService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new MyPictureService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IPersonalGTest, PersonalGService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new PersonalGService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IRecordingPublishRecordingTest, RecordingPublishRecordingService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RecordingPublishRecordingService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, traverse, runAppForRecordingManager_O16, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IRecordingTest, RecordingService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RecordingService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IRingtonesAndSoundsTest, RingtonesAndSoundsService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new RingtonesAndSoundsService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<ISignOutAudioDeviceTest, SignOutAudioDeviceService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new SignOutAudioDeviceService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, getOptionLogOut_O16, signIn, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<ISkypeMeetingTest, SkypeMeetingsService_O16>(OFFICE16, new InjectionFactory((uc) =>
             {
                 var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                 var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                 return new SkypeMeetingsService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
             }));

            this.container.RegisterType<IStatusTest, StatusService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new StatusService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IVideoDeviceTest, VideoDeviceService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new VideoDeviceService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));

            this.container.RegisterType<IIMTest, IMService_O16>(OFFICE16, new InjectionFactory((uc) =>
            {
                var takeScreen = container.Resolve<ITakeScreenshotTestStack>();
                var takeFullScreen = container.Resolve<ITakeScreenshotFull>();
                return new IMService_O16(execute, runApp, getSfb, closeAllModual, getOption_O16, selectOption, takeScreen, takeFullScreen, this.ResultFolderPath16, this.sfbPath_O16, testCaseImageStore_O16);
            }));
            //this.container.RegisterType<ISetResultFolderPath, TakeScreenShotService>(OFFICE16, new ContainerControlledLifetimeManager());
            //this.container.RegisterType<ISetResultFolderPath_Full, TakeFullScreenShotService>(OFFICE16, new ContainerControlledLifetimeManager());
        }

        public ObservableCollection<TestCase> GetTestcases(string OfficeVersion)
        {
            ObservableCollection<TestCase> result = new ObservableCollection<TestCase>();
            var t1 = new TestCase();
            t1.Name = "Options_CallHandling_" + OfficeVersion;
            t1.TestAction += (language) =>
            {
                var test = this.container.Resolve<ICallHandlingTest>(OfficeVersion);
                test.Options_CallHandling(language.CultureName, t1);
                return "Options_CallHandling_" + OfficeVersion + " done.";
            };

            result.Add(t1);

            var t2 = new TestCase();
            t2.Name = "Options_SkypeMeetings_" + OfficeVersion;
            t2.TestAction += (language) =>
            {
                var test = this.container.Resolve<ISkypeMeetingTest>(OfficeVersion);
                test.Options_SkypeMeetings(language.CultureName, t2);
                return "Options_SkypeMeetings_" + OfficeVersion + " done.";
            };

            result.Add(t2);

            var t3 = new TestCase();
            t3.Name = "Recording_Publish_Recording_" + OfficeVersion;
            t3.TestAction += (language) =>
            {
                var test = this.container.Resolve<IRecordingPublishRecordingTest>(OfficeVersion);
                test.Recording_Publish_Recording(language.CultureName, t3);
                return "Recording_Publish_Recording_" + OfficeVersion + " done.";
            };

            result.Add(t3);

            var t4 = new TestCase();
            t4.Name = "Options_Status_" + OfficeVersion;
            t4.TestAction += (language) =>
            {
                var test = this.container.Resolve<IStatusTest>(OfficeVersion);
                test.Options_Status(language.CultureName, t4);
                return "Options_Status_" + OfficeVersion + " done.";
            };

            result.Add(t4);

            var t5 = new TestCase();
            t5.Name = "Options_MyPicture_" + OfficeVersion;
            t5.TestAction += (language) =>
            {
                var test = this.container.Resolve<IMyPictureTest>(OfficeVersion);
                test.Options_MyPicture(language.CultureName, t5);
                return "Options_MyPicture_" + OfficeVersion + " done.";
            };

            result.Add(t5);

            var t6 = new TestCase();
            t6.Name = "Options_RingtonesAndSounds_" + OfficeVersion;
            t6.TestAction += (language) =>
            {
                var test = this.container.Resolve<IRingtonesAndSoundsTest>(OfficeVersion);
                test.Options_RingtonesAndSounds(language.CultureName, t6);
                return "Options_RingtonesAndSounds_" + OfficeVersion + " done.";
            };

            result.Add(t6);

            var t7 = new TestCase();
            t7.Name = "Options_IM_Emoticons_" + OfficeVersion;
            t7.TestAction += (language) =>
            {
                var test = this.container.Resolve<IIMEmoticonsTest>(OfficeVersion);
                test.Options_IM_Emoticons(language.CultureName, t7);
                return "Options_IM_Emoticons " + OfficeVersion + "done.";
            };

            result.Add(t7);

            var t8 = new TestCase();
            t8.Name = "Options__SignOut_Audio_Device_" + OfficeVersion;
            t8.TestAction += (language) =>
            {
                var test = this.container.Resolve<ISignOutAudioDeviceTest>(OfficeVersion);
                test.Options__SignOut_Audio_Device(language.CultureName, t8);
                return "Options__SignOut_Audio_Device_" + OfficeVersion + " done.";
            };

            result.Add(t8);

            var t9 = new TestCase();
            t9.Name = "Options_IM_" + OfficeVersion;
            t9.TestAction += (language) =>
            {
                var test = this.container.Resolve<IIMTest>(OfficeVersion);
                test.Options_IM(language.CultureName, t9);
                return "Options_IM " + OfficeVersion + "done.";
            };

            result.Add(t9);

            var t10 = new TestCase();
            t10.Name = "Options_VideoDevice_" + OfficeVersion;
            t10.TestAction += (language) =>
            {
                var test = this.container.Resolve<IVideoDeviceTest>(OfficeVersion);
                test.Options_VideoDevice(language.CultureName, t10);
                return "Options_VideoDevice " + OfficeVersion + "done.";
            };

            result.Add(t10);

            var t11 = new TestCase();
            t11.Name = "Call_Forwarding_Setting_" + OfficeVersion;
            t11.TestAction += (language) =>
            {
                var test = this.container.Resolve<ICallForwardingSettingTest>(OfficeVersion);
                test.Call_Forwarding_Setting(language.CultureName, t11);
                return "Call_Forwarding_Setting " + OfficeVersion + "done.";
            };

            result.Add(t11);

            var t12 = new TestCase();
            t12.Name = "Options_Recording_" + OfficeVersion;
            t12.TestAction += (language) =>
            {
                var test = this.container.Resolve<IRecordingTest>(OfficeVersion);
                test.Options_Recording(language.CultureName, t12);
                return "Options_Recording " + OfficeVersion + "done.";
            };

            result.Add(t12);

            var t13 = new TestCase();
            t13.Name = "Options_PersonalG_" + OfficeVersion;
            t13.TestAction += (language) =>
            {
                var test = this.container.Resolve<IPersonalGTest>(OfficeVersion);
                test.Options_PersonalG(language.CultureName, t13);
                return "Options_PersonalG " + OfficeVersion + "done.";
            };

            result.Add(t13);

            var t14 = new TestCase();
            t14.Name = "Options_Alerts_" + OfficeVersion;
            t14.TestAction += (language) =>
            {
                var test = this.container.Resolve<IAlertTest>(OfficeVersion);
                test.Options_Alerts(language.CultureName, t14);
                return "Options_Alerts " + OfficeVersion + "done.";
            };

            result.Add(t14);

            var t15 = new TestCase();
            t15.Name = "Options_General_" + OfficeVersion;
            t15.TestAction += (language) =>
            {
                var test = this.container.Resolve<IGeneralTest>(OfficeVersion);
                test.Options_General(language.CultureName, t15);
                return "Options_General " + OfficeVersion + "done.";
            };

            result.Add(t15);

            var t16 = new TestCase();
            t16.Name = "ConferenceJoin_MeetNow_" + OfficeVersion;
            t16.TestAction += (language) =>
            {
                var test = this.container.Resolve<IConferenceJoinMeetingNowTest>(OfficeVersion);
                test.ConferenceJoin_MeetNow(language.CultureName, t16);
                return "ConferenceJoin_MeetNow " + OfficeVersion + "done.";
            };

            result.Add(t16);

            var t17 = new TestCase();
            t17.Name = "ContactList__" + OfficeVersion;
            t17.TestAction += (language) =>
            {
                var test = this.container.Resolve<IContactListTest>(OfficeVersion);
                test.Options_ContactList(language.CultureName, t17);
                return "ContactList_ " + OfficeVersion + "done.";
            };

            result.Add(t17);

            var t18 = new TestCase();
            t18.Name = "Phones__" + OfficeVersion;
            t18.TestAction += (language) =>
            {
                var test = this.container.Resolve<IPhonesTest>(OfficeVersion);
                test.Options_Phones(language.CultureName, t18);
                return "Phones_ " + OfficeVersion + "done.";
            };

            result.Add(t18);

            var t19 = new TestCase();
            t19.Name = "PersistentChat__" + OfficeVersion;
            t19.TestAction += (language) =>
            {
                var test = this.container.Resolve<IPersistentChatTest>(OfficeVersion);
                test.Options_PersistentChat(language.CultureName, t19);
                return "PersistentChat_ " + OfficeVersion + "done.";
            };

            result.Add(t19);

            return result;
        }

        public ObservableCollection<TestCase> GetTestCasesList()
        {
            ObservableCollection<TestCase> testcases = new ObservableCollection<TestCase>();
            ObservableCollection<TestCase> o15 = GetTestcases(OFFICE15);
            ObservableCollection<TestCase> o16 = GetTestcases(OFFICE16);
            testcases.AddRange(o15);
            testcases.AddRange(o16);
            return testcases;
        }
    }
}
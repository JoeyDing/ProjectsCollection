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
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.WindowsAPI;

namespace Sfb.Core
{
    public abstract class SfbBVT
    {
        protected readonly string _resultFolderPath;
        protected abstract string SfbPath { get; }

        public SfbBVT(string resultFolderPath)
        {
            this._resultFolderPath = resultFolderPath;
        }

        #region CommonMethods

        private T ExecuteAction<T>(string description, Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                var error = new Exception(string.Format("Failed step: \"{0}\"", description), ex);
                throw error;
            }
        }

        private void ExecuteAction(string description, Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                var error = new Exception(string.Format("Failed step: \"{0}\"", description), ex);
                throw error;
            }
        }

        private void TakeScreenShot(UIItem item, string fileName)
        {
            if (!Directory.Exists(this._resultFolderPath))
                Directory.CreateDirectory(this._resultFolderPath);
            //using (var m = new MemoryStream())
            //{
            //    screenShot.Save(m, ImageFormat.Png);
            //    var img = System.Drawing.Image.FromStream(m);
            //    img.Save(Path.Combine(this._resultFolderPath, "SkypeMeetings_SkypeMeetingsTab.png"));
            //}
            //screenShot.Save("SkypeMeetings_SkypeMeetingsTab.png", ImageFormat.Png);
            Bitmap screenShot = item.VisibleImage;
            screenShot.Save(Path.Combine(this._resultFolderPath, fileName), ImageFormat.Png);
        }

        private void TakeFullScreenShot(Application app, string fileName, ImageFormat imageFormat)
        {
            if (!Directory.Exists(this._resultFolderPath))
                Directory.CreateDirectory(this._resultFolderPath);

            var appScreen = System.Windows.Forms.Screen.FromHandle(app.Process.Handle);

            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(appScreen.Bounds.Width,
                                           appScreen.Bounds.Height,
                                           PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(appScreen.Bounds.X,
                                        appScreen.Bounds.Y,
                                        0,
                                        0,
                                        appScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            // Save the screenshot to the specified path that the user has chosen.
            bmpScreenshot.Save(Path.Combine(this._resultFolderPath, fileName), imageFormat);
        }

        private Application RunApp(string path)
        {
            //if(!SfbUtils.AppIsLaunched())
            //{
            //    Thread.Sleep(10000);
            //}
            if (!SfbUtils.AppIsLaunched())
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(this.SfbPath));
                Thread.Sleep(5000);
                return app;
            }
            else
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(this.SfbPath));
                return app;
            }
        }

        private Window GetSfbClientWindow(Application app)
        {
            Window window = app.GetWindow("Skype for Business ");
            window.DisplayState = DisplayState.Restored;
            window.Focus();
            return window;
        }

        private void CloseAllModals(Window sfbClient, Application app)
        {
            // open window
            //// close any open windows
            sfbClient.Focus();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            foreach (var item in windows)
            {
                item.Close();
            }
        }

        private Window GetOptionWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            var pane_contact = panes[3] as Panel;

            //get option button inside pane
            var button_option = pane_contact.Items[0] as Button;
            // only open the option modal window
            button_option.Click();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }

        private void GetLocationDropDownWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));


            //get main pane
            var pane_main = panes[1] as Panel;
            //get location button inside pane
            var button_location = pane_main.Items[6] as Button;
            
            var point = new System.Windows.Point(button_location.Bounds.BottomRight.X + 5, button_location.Bounds.BottomRight.Y - 5);
            sfbClient.Focus();
            sfbClient.Mouse.Click(point);
            Thread.Sleep(1000);
            sfbClient.Mouse.Click(new System.Windows.Point(button_location.Bounds.BottomRight.X - 10, button_location.Bounds.BottomRight.Y + 10));
        }

      
        private Panel GetEditLocationWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get main pane
            var pane_main = panes[0] as Panel;

            var panel_location = pane_main.Items[0] as Panel;
            var button_open = panel_location.Items[11] as Button;
            button_open.Click();
            Thread.Sleep(2000);
            return panel_location;
        }

        private void closeEditLocationPanel(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get main pane
            var pane_main = panes[0] as Panel;

            var panel_location = pane_main.Items[0] as Panel;

            //get open button ,then click other arae to clost it
            //var button_open = panel_location.Items[11] as Button;

            //Thread.Sleep(6000);

            //var point = new System.Windows.Point(button_open.Bounds.BottomRight.X + 5, button_open.Bounds.BottomRight.Y + 5);
            //sfbClient.Mouse.Click(point);

            //Thread.Sleep(6000);
            var button_close = panel_location.Items[4] as Button;
            button_close.Click();
        }

        private UIItemCollection SelectOptionWindowTabByIndex(int index, Window options_Window)
        {
            Tree tree = options_Window.Get<Tree>(SearchCriteria.ByControlType(ControlType.Tree));
            //move to SKype Meetings tab
            for (int i = 0; i <= index; i++)
            {
                tree.KeyIn(KeyboardInput.SpecialKeys.DOWN);
            }

            return options_Window.Items;
        }


        private IEnumerable<UIItem> GetChildren(UIItem root) {
            var container = root as UIItemContainer;
            if (container != null)
            {
                foreach (UIItem item in container.Items)
                {
                    if (item is UIItemContainer)
                    {
                        foreach (var subItem in this.GetChildren(item as UIItemContainer))
                        {
                            yield return subItem;
                        }
                    }
                    else
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                yield return root;
            }
        }
        #endregion CommonMethods

        #region BVT

        //Skype Meetings
        protected bool Options_SkypeMeetings(int tabIndex)
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
                           "get Skype Meetings tab elements in option window", () =>
                           {
                               return this.SelectOptionWindowTabByIndex(14, option_window);
                           }
                       );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "SkypeMeetings_SkypeMeetingsTab.png");
                }
            );

            this.ExecuteAction(
                "Click Change Button and take screenshot", () =>
                {
                    Button changeBtn = items.FirstOrDefault(c => c.AutomationElement.Current.AutomationId == "496") as Button;
                    if (changeBtn != null)
                    {
                        changeBtn.Click();
                        this.TakeFullScreenShot(app, "SkypeMeetings_ChangeBtn.png", ImageFormat.Png);
                    }
                    else
                        throw new Exception("Cannot find Change button");
                }
            );

            return true;
        }

        public abstract bool Options_SkypeMeetings();

        //Status
        public virtual bool Options_Status()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(2, option_window);
               }
           );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Status_Status.png");
                }
            );

            return true;
        }

        //Ringtones and Sounds
        public virtual bool Options_RingtonesAndSounds()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(8, option_window);
               }
           );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Ringtones and Sounds_Ringtones and Sounds.png");
                }
            );

            return true;
        }

        //IM:Emoticons
        public virtual bool Options_IM_Emoticons()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(6, option_window);
               }
           );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "IM Emoticons_IM Emoticons.png");
                }
            );

            return true;
        }

        //IM:Audio Device(Log Out)
        public virtual bool Options__SignOut_Audio_Device()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "log out", () =>
               {
                   return this.GetOptionWindow_LogOut(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(1, option_window);
               }
           );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Audio Device Sign Out_Audio Device Sign Out.png");
                }
            );

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            this.ExecuteAction(
                "Sign in", () =>
                {
                    this.Sign_In(sfbClient);
                }
            );

            return true;
        }

        //IM:Audio Device(Log In)
        public virtual bool Options_Audio_Device_Settings()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(9, option_window);
               }
           );

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Device_Audio.png");
                }
            );

            return true;
        }

        //IM
        public virtual bool Options_IM()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(6, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "IM_IMTab.png");
               }
           );
            return true;
        }

        //Video device
        public virtual bool Options_VideoDevice()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(10, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "Options_VideoDevice.png");
               }
           );

            return true;
        }

        //Call handling
        protected bool Options_CallHandling(int index)
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(index, option_window);
               }
           );
            //Get checkbox
            var outgoing_calls = option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("1030"));
            //Check if the 'Outgoing Calls' is checked
            if (outgoing_calls.Checked)
            {
                this.ExecuteAction(
               "make a screenshot of window", () =>
                   {
                       this.TakeScreenShot(option_window, "Options_CallHandling.png");
                   }
               );
            }
            else
            {
                //Check the checkbox
                outgoing_calls.Click();
                this.ExecuteAction(
               "make a screenshot of window", () =>
                   {
                       this.TakeScreenShot(option_window, "IM_IMTab.png");
                   }
               );
            }
            return true;
        }

        public abstract bool Options_CallHandling();

        //Call Handling Settings
        public virtual bool Call_Forwarding_Setting()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(11, option_window);
               }
           );
            //Get radio button
            var radio_button = option_window.Get<RadioButton>(SearchCriteria.ByAutomationId("184"));
            //Click the radio button
            radio_button.Click();
            //Get checkbox
            var combobox = option_window.Get<ComboBox>(SearchCriteria.ByAutomationId("191"));
            //Dropdown combobox and select the item 'New Number'
            combobox.Items[3].Click();
            //Get window 'Edit phone number'
            var edit_phone_number = app.GetWindows()[2];
            //Get EditText 'phone number'
            var phone_number = edit_phone_number.Get(SearchCriteria.ByAutomationId("314"));
            //Type new phone number to the EditText 'phone number'
            phone_number.Enter("18888888888");
            //get 'OK' button
            var OK_button = option_window.Get<Button>(SearchCriteria.ByAutomationId("1"));
            //click 'OK' button
            OK_button.Click();
            this.ExecuteAction(
            "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Call_Forwarding_Setting.png");
                }
            );
            return true;
        }

        //Recording
        public virtual bool Options_Recording()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(13, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "Recording_RecordingTab.png");
               }
           );
            return true;
        }

        //Personal(G)
        public virtual bool Options_PersonalG()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(0, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "PersonalG_PersonalGTab.png");
               }
           );
            return true;
        }

        //Phones
        public virtual bool Options_Phones()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(4, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "Phones_PhonesTab.png");
               }
           );
            return true;
        }

        //Alerts
        public virtual bool Options_Alerts()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(5, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "Alerts_AlertsTab.png");
               }
           );
            return true;
        }

        //General
        public virtual bool Options_General()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(-1, option_window);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   this.TakeScreenShot(option_window, "General_GeneralTab.png");
               }
           );
            return true;
        }

        //Location dropdown list
        public virtual bool Edit_Location_Dlg()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "get edit location window", () =>
                {
                    this.GetLocationDropDownWindow(sfbClient, app);
                }
                );

            Panel editLocation_window = this.ExecuteAction(
               "get location dropdown window", () =>
               {
                   return this.GetEditLocationWindow(sfbClient, app);
               }
           );

            this.ExecuteAction(
               "make a screenshot of window", () =>
               {
                   Thread.Sleep(2000);
                   this.TakeFullScreenShot(app, "EditLocation_EditLocationPanel.png", ImageFormat.Png);
                   //this.closeEditLocationPanel(sfbClient, app);
               }
           );

            this.ExecuteAction(
               "make sure all windows are closed", () =>
               {
                   //this.closeEditLocationPanel(sfbClient, app);
                   SfbUtils.CloseSfbClient();
               }
                );

            return true;
            
        }

        private Window GetOptionWindow_LogOut(Window sfbClient, Application app)
        {
            //get all panes
            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = true;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 3;

            #region Log out

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[3] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[1] as Button;
            button_location.Click();
            var point = new System.Windows.Point(button_location.Bounds.BottomRight.X, button_location.Bounds.BottomRight.Y + 20);
            sfbClient.Mouse.Click(point);
            Thread.Sleep(1000);
            sfbClient.Mouse.Click(new System.Windows.Point(button_location.Bounds.BottomRight.X - 50, button_location.Bounds.BottomRight.Y + 20));

            #endregion Log out

            // only open the option modal window

            var panes1 = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            var pane_contact1 = panes1[2] as Panel;

            //get Show Menu Arrow inside pane
            var button_location1 = pane_contact1.Items[0] as Button;
            button_location1.Click();

            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }

        public virtual bool ConferenceJoin_MeetNow()
        {
            var app = this.ExecuteAction(
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
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
            Thread.Sleep(1000);
            //Get conversation window
            var conversation_window = app.GetWindow("Conversation (1 Participant)");
            //Get pane 'Join Meeting Audio'
            var join_meeting_pane = conversation_window.Get<Panel>(SearchCriteria.ByControlType(ControlType.Pane));
            Thread.Sleep(2000);

            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(join_meeting_pane, "ConferenceJoin_MeetNow.png");
                }
            );

            return true;
        }

        private void Sign_In(Window sfbClient)
        {
            // only open the option modal window

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            var pane_contact = panes[2] as Panel;

            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[5] as Button;
            button_location.Click();
        }
        
        #endregion BVT
    }
}
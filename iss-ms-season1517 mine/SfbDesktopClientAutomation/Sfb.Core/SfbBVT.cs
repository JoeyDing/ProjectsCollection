using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using TestStack.White;
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
        protected string _resultFolderPath;

        protected abstract string SfbPath { get; }

        protected abstract string RecordingManagerPath { get; }

        protected string RootDrive
        {
            get
            {
                return Path.GetPathRoot(System.Environment.SystemDirectory);
            }
        }

        public SfbBVT(string resultFolderPath)
        {
            this._resultFolderPath = resultFolderPath;
        }

        #region CommonMethods

        protected T ExecuteAction<T>(string description, Func<T> action)
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

        protected void ExecuteAction(string description, Action action)
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

        protected void TakeScreenShot(UIItem item, string fileName)
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

        protected void TakeFullScreenShot(Application app, string fileName, ImageFormat imageFormat)
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
            Thread.Sleep(3000);
        }

        protected Application RunApp(string path)
        {
            //if(!SfbUtils.AppIsLaunched())
            //{
            //    Thread.Sleep(10000);
            //}
            if (!SfbUtils.AppIsLaunched())
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                Thread.Sleep(5000);
                return app;
            }
            else
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                return app;
            }
        }

        protected Application RunAppForRecordingManager(string path)
        {
            //if(!SfbUtils.AppIsLaunched())
            //{
            //    Thread.Sleep(10000);
            //}
            if (!SfbUtils.AppIsLaunchedForRecordingManager())
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                Thread.Sleep(5000);
                return app;
            }
            else
            {
                Application app = Application.AttachOrLaunch(new System.Diagnostics.ProcessStartInfo(path));
                return app;
            }
        }

        protected Window GetSfbClientWindow(Application app)
        {
            Window window = app.GetWindows().First(c =>
            {
                bool found = false;
                try
                {
                    var item = c.Get(SearchCriteria.ByAutomationId("idBuddyListTab"));
                    found = true;
                }
                catch (Exception)
                {
                }

                return c.IsModal == false && found;
            });

            if (app.GetWindows().Count > 1)
            {
                this.CloseAllModals(window, app);
            }
            window.DisplayState = DisplayState.Restored;
            window.Focus();
            return window;
        }

        protected void CloseAllModals(Window sfbClient, Application app)
        {
            // open window
            //// close any open windows
            sfbClient.Focus();
            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            foreach (var item in windows)
            {
                item.Close();
            }
            Thread.Sleep(3000);
        }

        protected virtual Window GetOptionWindow(Window sfbClient, Application app)
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

        protected void GetLocationDropDownWindow(Window sfbClient, Application app)
        {
            //get all panes
            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get main pane
            var pane_main = panes[1] as Panel;
            //get location button inside pane
            var button_location = pane_main.Items[7] as Button;
            var point = new System.Windows.Point(button_location.Bounds.BottomRight.X + 10, button_location.Bounds.BottomRight.Y - 10);
            sfbClient.Mouse.Click(point);
            Thread.Sleep(1000);
            sfbClient.Mouse.Click(new System.Windows.Point(button_location.Bounds.BottomRight.X + 70, button_location.Bounds.BottomRight.Y + 20));
        }

        protected Panel GetEditLocationWindow(Window sfbClient, Application app)
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

        protected void closeEditLocationPanel(Window sfbClient, Application app)
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

        protected UIItemCollection SelectOptionWindowTabByIndex(int index, Window options_Window)
        {
            Tree tree = options_Window.Get<Tree>(SearchCriteria.ByControlType(ControlType.Tree));
            //move to SKype Meetings tab
            for (int i = 0; i <= index; i++)
            {
                tree.KeyIn(KeyboardInput.SpecialKeys.DOWN);
            }

            return options_Window.Items;
        }

        protected IEnumerable<UIItem> GetChildren(UIItem root)
        {
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

        protected void Click_Menu_Button(IUIItem MenuButton, Window sfbClient, int x, int y)
        {
            var menu_point = new System.Windows.Point(MenuButton.Bounds.BottomRight.X + x, MenuButton.Bounds.BottomRight.Y + y);
            //Click the menu button
            sfbClient.Mouse.Click(menu_point);
            Thread.Sleep(1000);
            var start_meeting = new System.Windows.Point(MenuButton.Bounds.BottomRight.X + x, MenuButton.Bounds.BottomRight.Y - y);
            //Click the start meeting button
            sfbClient.Mouse.Click(start_meeting);
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
                               return this.SelectOptionWindowTabByIndex(tabIndex, option_window);
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
                        Thread.Sleep(1000);
                    }
                    else
                        throw new Exception("Cannot find Change button");
                }
            );

            this.ExecuteAction(
                "Click ok button on popup", () =>
                {
                    //var windows = app.GetWindows();
                    Button okButton = option_window.Get<Button>(SearchCriteria.ByAutomationId("Button0"));
                    okButton.Click();
                }
            );

            return true;
        }

        public abstract bool Options_SkypeMeetings(string languageShortName, TestCase testCase);

        //Status
        public virtual bool Options_Status(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //My picture
        public virtual bool Options_MyPicture(string languageShortName, TestCase testCase)
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

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get Skype Meetings tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(3, option_window);
               }
           );

            //Check radio button 'Show a picture from a webside'
            var radioButton = option_window.Get<RadioButton>(SearchCriteria.ByAutomationId("1011"));
            if (!radioButton.Enabled)
            {
                return false;
            }
            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "MyPicture.png");
                }
            );

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //Ringtones and Sounds
        public virtual bool Options_RingtonesAndSounds(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //IM:Emoticons
        public virtual bool Options_IM_Emoticons(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //IM:Audio Device(Log Out)
        public virtual bool Options__SignOut_Audio_Device(string languageShortName, TestCase testCase)
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
        public virtual bool Options_Audio_Device_Settings(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //IM
        public virtual bool Options_IM(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            return true;
        }

        //Video device
        public virtual bool Options_VideoDevice(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            return true;
        }

        //Call handling
        protected bool Options_CallHandling(int index, int checkBoxAutoID)
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

            Window option_window = this.ExecuteAction(
               "get option window", () =>
               {
                   return this.GetOptionWindow(sfbClient, app);
               }
           );

            UIItemCollection items = this.ExecuteAction(
               "get call handling tab elements in option window", () =>
               {
                   return this.SelectOptionWindowTabByIndex(index, option_window);
               }
           );
            //Get checkbox

            CheckBox outgoing_calls = this.ExecuteAction(
               "get call handling tab checkbox element", () =>
               {
                   return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId(checkBoxAutoID.ToString()));
                   //return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("1030"));
                   //return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("1020"));
               }
           );
            outgoing_calls.Click();
            Thread.Sleep(3000);
            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    //Check if the 'Outgoing Calls' is checked
                    if (!outgoing_calls.Checked)
                    {
                        //Check the checkbox
                        outgoing_calls.Click();
                    }
                    this.TakeScreenShot(option_window, "Options_CallHandling.png");
                }
               );

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            return true;
        }

        public abstract bool Options_CallHandling(string languageShortName, TestCase testCase);

        //Call Handling Settings
        public virtual bool Call_Forwarding_Setting(string languageShortName, TestCase testCase)
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
            combobox.Items[2].Click();
            //Get window 'Edit phone number'
            var all_edit_phone_number = app.GetWindows().Select(c => c.Name).ToList();
            var edit_phone_number = app.GetWindows()[3];
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            return true;
        }

        public class Node<T>
        {
            public T Element { get; set; }
            public List<Node<T>> Items { get; set; }
        }

        protected static Tuple<string, Node<AutomationElement>> TraverseItemNative(AutomationElement node, int depth = 0, string index = "root")
        {
            string stringResult = "";
            var itemResult = new Node<AutomationElement>()
            {
                Element = node,
                Items = new List<Node<AutomationElement>>()
            };
            string indent = "";
            for (int i = 0; i < depth; i++)
                indent += "----";

            itemResult.Element = node;
            stringResult = string.Format("{0}{1}({2}){3}", indent, node.Current.Name, index, Environment.NewLine);
            var tt = node.FindAll(TreeScope.Children, Condition.TrueCondition);
            if (tt != null)
            {
                for (int i = 0; i < tt.Count; i++)
                {
                    var child = tt[i];
                    itemResult.Items.Add(new Node<AutomationElement> { Element = child, Items = new List<Node<AutomationElement>>() });
                    string currentIndex = index + "," + i;
                    var childResult = TraverseItemNative(child, depth + 1, currentIndex);
                    stringResult += childResult.Item1;
                    foreach (var item in childResult.Item2.Items)
                    {
                        itemResult.Items[i].Items.Add(item);
                    }
                }
            }
            return new Tuple<string, Node<AutomationElement>>(stringResult, itemResult);
        }

        //Recording
        public virtual bool Options_Recording(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
            "make sure all windows are closed", () =>
            {
                this.CloseAllModals(sfbClient, app);
            }
        );
            return true;
        }

        //Personal(G)
        public virtual bool Options_PersonalG(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
               "make sure all windows are closed", () =>
               {
                   this.CloseAllModals(sfbClient, app);
               }
           );
            return true;
        }

        //Phones
        public virtual bool Options_Phones(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );
            return true;
        }

        //Alerts
        public virtual bool Options_Alerts(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
               "make sure all windows are closed", () =>
               {
                   this.CloseAllModals(sfbClient, app);
               }
           );
            return true;
        }

        //General
        public virtual bool Options_General(string languageShortName, TestCase testCase)
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

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        //Location dropdown list
        public virtual bool Edit_Location_Dlg(string languageShortName, TestCase testCase)
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

            Thread.Sleep(3000);
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
                   this.TakeFullScreenShot(app, "EditLocation_EditLocationPanel.png", ImageFormat.Png);
               }
           );

            this.ExecuteAction(
               "make sure all windows are closed", () =>
               {
                   SfbUtils.CloseSfbClient();
               }
                );

            return true;
        }

        protected Window GetOptionWindow_LogOut(Window sfbClient, Application app)
        {
            //get all panes
            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = true;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 3;

            #region Log out

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            var pane_contact = panes[2] as Panel;
            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[1] as Button;
            button_location.Click();
            Thread.Sleep(1000);
            //click "File"
            var traverseItemNative = TraverseItemNative(pane_contact.AutomationElement);
            var traverseItemNativeFile = traverseItemNative.Item2.Items[13].Items[2].Items[0].Items[0].Items[0];
            //create wrapper for element
            var fileWrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeFile.Element, pane_contact.ActionListener);
            fileWrapper.Click();
            Thread.Sleep(3000);
            //click "Sign Out",traverseItemNative has been changed since file's been clicked
            traverseItemNative = TraverseItemNative(pane_contact.AutomationElement);
            var traverseItemNativeSignout = traverseItemNative.Item2.Items[13].Items[2].Items[0].Items[0].Items[0].Items[1].Items[0].Items[0].Items[0];
            //create wrapper for element
            var signout_wrapper = new TestStack.White.UIItems.MenuItems.Menu(traverseItemNativeSignout.Element, pane_contact.ActionListener);
            signout_wrapper.Click();

            #endregion Log out

            // only open the option modal window
            var panes1 = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));
            //get contact pane
            (panes1[1] as Panel).Items[0].Click();

            var windows = app.GetWindows().Where(w => w.Name != sfbClient.Name);
            var options_Window = windows.First();

            return options_Window;
        }

        private string TraverseItems(UIItem node, int depth = 0, string index = "root")
        {
            string result = "";
            string indent = "";
            for (int i = 0; i < depth; i++)
                indent += "----";

            result = string.Format("{0}{1}({2}){3}", indent, node.Name, index, Environment.NewLine);

            if (node is UIItemContainer)
            {
                node.Focus();
                UIItemContainer uiitemcontainer = (UIItemContainer)node;
                for (int i = 0; i < uiitemcontainer.Items.Count; i++)
                {
                    var child = (UIItem)uiitemcontainer.Items[i];
                    //var child = (UIItem)uiitemcontainer.GetElement(SearchCriteria.);
                    //what if uiitem is a button without items?
                    string currentIndex = index + "," + i;
                    result += TraverseItems(child, depth + 1, currentIndex);
                }
            }
            return result;
        }

        public virtual bool ConferenceJoin_MeetNow(string languageShortName, TestCase testCase)
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

            //Get checkbox

            CheckBox checkboxformeeting = this.ExecuteAction(
               "get Skype Meetings tab checkbox element", () =>
               {
                   return option_window.Get<CheckBox>(SearchCriteria.ByAutomationId("92"));
               }
           );
            checkboxformeeting.Click();
            Thread.Sleep(3000);
            this.ExecuteAction(
                "make a screenshot of window", () =>
                {
                    this.TakeScreenShot(option_window, "Status_Status.png");
                }
            );

            this.ExecuteAction(
                "make sure all windows are closed", () =>
                {
                    this.CloseAllModals(sfbClient, app);
                }
            );

            return true;
        }

        protected void Sign_In(Window sfbClient)
        {
            // only open the option modal window

            var panes = sfbClient.GetMultiple(SearchCriteria.ByControlType(ControlType.Pane));

            //get contact pane
            var pane_contact = panes[1] as Panel;

            //get Show Menu Arrow inside pane
            var button_location = pane_contact.Items[6] as Button;
            //var button_location = pane_contact.Items[7] as Button;
            button_location.Click();
        }

        public abstract bool Recording_Publish_Recording(string languageShortName, TestCase testCase);

        #endregion BVT
    }
}
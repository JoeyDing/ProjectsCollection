using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Swx.Core
{
    public abstract class SwxSkypeCore : SwxCore
    {
        public SwxSkypeCore(DriverType driverType, string language, string resultFolderPath)
            : base(driverType, language, resultFolderPath)
        {
        }

        #region TestCases

        public Boolean SigninSkype()
        {
            try
            {
                string testURL = this.Url;
                WebDriver.Navigate().GoToUrl(testURL);

                WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));

                //Wait for the login screen appears.
                IWebElement autocomplete = wait.Until<IWebElement>((d) =>
                {
                    return d.FindElement(By.Id("username"));
                });

                //Enter username and password to signin
                IWebElement userNameField = WebDriver.FindElement(By.Id("username"));
                IWebElement passwordField = WebDriver.FindElement(By.Id("password"));
                IWebElement loginButton = WebDriver.FindElement(By.Id("signIn"));
                userNameField.Clear();
                userNameField.SendKeys(this.UserName);
                passwordField.Clear();
                passwordField.SendKeys(this.Password);
                loginButton.Click();
                Thread.Sleep(10000);

                //Wait for the dashboard
                autocomplete = wait.Until<IWebElement>((d) =>
                {
                    return d.FindElement(By.XPath("//input[@placeholder='Search Skype']"));
                });

                Thread.Sleep(5000);
                //Take picture of signin screen
                this.CaptureScreenshot(Path.Combine(this._resultFolderPath, "Signin_Skype.png"));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean DeleteContactOnSkype()
        {
            try
            {
                string name = "aaddsffadsdafsd";
                this.searchContact(name);
                Thread.Sleep(3000);

                var searDirectoryButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@class,'{0}')]", "btn primary searchDirectory list-selectable")));
                searDirectoryButton.Click();
                Thread.Sleep(2000);

                var searchItem = WebDriver.FindElement(By.XPath(String.Format("//li[contains(@data-title,'{0}')]", name)));
                Actions actions = new Actions(WebDriver);
                actions.ContextClick(searchItem).Perform();
                Thread.Sleep(2000);

                IList<IWebElement> buttons = WebDriver.FindElements(By.XPath(String.Format("//li[contains(@class,'{0}')]", "list-selectable")));
                var leaveButton = buttons[5];
                leaveButton.Click();
                Thread.Sleep(2000);

                var deleteButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@data-bind,'{0}')]", "deleteContact")));
                deleteButton.Click();
                Thread.Sleep(8000);

                Thread.Sleep(2000);
                var textInput = WebDriver.FindElement(By.XPath(String.Format("//input[contains(@data-bind,'{0}')]", "textInput")));
                textInput.Clear();
                //this.deleteConversation(name, 4);
                Thread.Sleep(2000);
                this.searchContact(name);
                Thread.Sleep(3000);

                searDirectoryButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@class,'{0}')]", "btn primary searchDirectory list-selectable")));
                searDirectoryButton.Click();
                Thread.Sleep(2000);

                searchItem = WebDriver.FindElement(By.XPath(String.Format("//li[contains(@data-title,'{0}')]", name)));
                searchItem.Click();
                Thread.Sleep(2000);

                if (IsElementPresent(By.XPath(String.Format("//button[contains(@data-bind,'{0}')]", "sendContactRequest"))))
                {
                    return true;
                }

                this.CaptureScreenshot(Path.Combine(this._resultFolderPath, "DeleteContact_Skype"));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean LeaveConversationOnSkype()
        {
            try
            {
                //Search contact
                string name = "Hanqing QU";
                this.searchContact(name);
                Thread.Sleep(3000);

                //Click item
                var searchItem = WebDriver.FindElement(By.XPath(String.Format("//li[contains(@data-title,'{0}')]", name)));
                searchItem.Click();
                Thread.Sleep(2000);

                //Send addContact request
                var addContactButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@data-bind,'{0}')]", "sendContactRequest")));
                addContactButton.Click();
                Thread.Sleep(5000);

                //Add group member
                var addPeople = WebDriver.FindElement(By.XPath("//*[@id='swxContent1']/swx-navigation/div/div/div/swx-header/div[1]/div/div/div/div/swx-button[3]/button"));
                addPeople.Click();
                Thread.Sleep(2000);
                var allContacts = WebDriver.FindElement(By.XPath("//*[@id='swxContent1']/swx-navigation/div/div/div/swx-header/div[2]/swx-contact-picker/div[1]/div[1]/div/div[1]/ul/div[1]/div[2]/li[1]"));
                allContacts.Click();
                var addContact = WebDriver.FindElement(By.XPath("//*[@id='swxContent1']/swx-navigation/div/div/div/swx-header/div[2]/div[2]/div/swx-button[2]/button"));
                addContact.Click();
                Thread.Sleep(5000);

                //Right click group item
                var groupItem = WebDriver.FindElement(By.XPath("//*[@id='timelineComponent']/div/swx-sidebar/swx-recents/div[2]/div[1]/div/swx-recent-item[1]"));
                Actions actions = new Actions(WebDriver);
                actions.ContextClick(groupItem).Perform();
                Thread.Sleep(2000);

                //Click leave conversation button
                IList<IWebElement> buttons = WebDriver.FindElements(By.XPath("//li[@class='list-selectable']"));
                var leaveButton = buttons[0];
                leaveButton.Click();
                Thread.Sleep(2000);

                //Confirm leave conversation
                var confirmButton = WebDriver.FindElement(By.XPath(String.Format("//span[contains(@data-bind,'{0}')]", "confirmButtonTitle")));
                confirmButton.Click();
                Thread.Sleep(5000);

                //Take screen shot of leave conversation
                this.CaptureScreenshot(Path.Combine(this._resultFolderPath, "LeaveConversation_Skype"));
                Thread.Sleep(2000);

                //Delete group conversation
                this.deleteGroupConversation(0, By.XPath("//*[@id='timelineComponent']/div/swx-sidebar/swx-recents/div[2]/div[1]/div/swx-recent-item[1]"));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean AddContactOnSkype()
        {
            try
            {
                string name = "aaddsffadsdafsd";

                this.searchContact(name);
                Thread.Sleep(2000);

                var searDirectoryButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@class,'{0}')]", "btn primary searchDirectory list-selectable")));
                searDirectoryButton.Click();
                Thread.Sleep(10000);

                var searchItem = WebDriver.FindElement(By.XPath(String.Format("//li[contains(@data-title,'{0}')]", name)));
                searchItem.Click();
                Thread.Sleep(2000);

                var sendRequestButton = WebDriver.FindElement(By.XPath(String.Format("//button[contains(@data-bind,'{0}')]", "sendContactRequest")));
                sendRequestButton.Click();
                Thread.Sleep(5000);

                this.CaptureScreenshot(Path.Combine(this._resultFolderPath, "AddContact_Skype"));
                Thread.Sleep(2000);

                this.deleteConversation(name, 5);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean SendIMOnSkype()
        {
            try
            {
                string name = "Skype Test User";
                this.searchContact(name);
                Thread.Sleep(1000);

                var searchItem = WebDriver.FindElement(By.XPath(String.Format("//li[contains(@data-title,'{0}')]", "Skype Test User")));
                searchItem.Click();
                Thread.Sleep(3000);

                Actions action = new Actions(WebDriver);
                action.SendKeys("skype test" + Keys.Enter);
                action.Perform();
                Thread.Sleep(2000);

                this.CaptureScreenshot(Path.Combine(this._resultFolderPath, "SendIM_Skype"));

                IWebElement seachBox = WebDriver.FindElement(By.XPath("//div[@class='iconfont search']"));
                seachBox.Click();
                Thread.Sleep(2000);

                this.deleteConversation(name, 5);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        #endregion TestCases
    }
}
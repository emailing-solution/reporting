using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System.Threading;
using System.Diagnostics;

namespace reporting.data
{
    class hotmail
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private string Proxy { get; set; }
        private string Port { get; set; }

        private IWebDriver Driver { get; set; }
        private ChromeOptions Options { get; set; }
        private WebDriverWait Wait { get; set; }
        private Random Random { get; set; } = new Random();

        public hotmail(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public hotmail(string username, string password, string proxy, string port)
        {
            Username = username;
            Password = password;
            Proxy = proxy;
            Port = port;
        }

        public void Init(bool proxy = true, bool adblock = false, bool incognito = true)
        {
            var useragents = new List<string>
            {
                "user-agent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36 OPR/18.0.1284.63",
            };
            try
            {
                //service
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                //options
                Options = new ChromeOptions();
                Options.AddArguments("disable-infobars");
                //Options.AddArguments(useragents[Random.Next(useragents.Count)]);
                if (incognito)
                {
                    Options.AddArguments("--incognito");
                }
                if (adblock)
                {
                    Options.AddExtension("adblock.crx");
                }
                if (proxy && !string.IsNullOrEmpty(Proxy))
                {
                    Options.AddArguments("--proxy-server=" + Proxy + ":" + Port);
                }
                //resize window
                Options.AddArguments("--window-size=" + Random.Next(500, 1500) + "," + Random.Next(800, 1000));
                Driver = new ChromeDriver(chromeDriverService, Options);
            }
            catch (Exception ex)
            {
                new Logger().Fatal("init error : " + ex.ToString());
            }
        }

        public void Connect(int timeout = 5, string url = "https://login.live.com/login.srf")
        {
            try
            {
                Driver.Navigate().GoToUrl(url);
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

                //username
                IWebElement userfeild = Driver.FindElement(By.Id("i0116"));
                foreach (var item in Username)
                {
                    userfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 100));
                }
                Driver.FindElement(By.Id("idSIButton9")).Submit();

                Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("i0118")));

                IWebElement passfeild = Driver.FindElement(By.Id("i0118"));
                //password
                foreach (var item in Password)
                {
                    passfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 100));
                }
                Driver.FindElement(By.Id("idSIButton9")).Click();
            }

            catch (Exception ex)
            {
                new Logger().Error("Connect Error : " + ex.Message);
            }
        }

        public void Navigate(string url = "https://outlook.live.com/mail/inbox")
        {
            try
            {
                Driver.Url = url;
                Driver.Navigate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public bool CheckIfConnected()
        {
            Navigate();
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("id__5")));
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("This Account Need a verification : " + Username);
                return false;
            }
        }

        public bool CheckIfClosed()
        {
            try
            {
                string title = Driver.Title;
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public bool GoToSpamFolder()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div[title^=Junk]")));
                Driver.FindElement(By.CssSelector("div[title^=Junk]")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Can\'t get spam folder account : " + Username);
                return false;
            }
        }

        public bool GotoInboxFolder()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div[title=Inbox]")));
                Driver.FindElement(By.CssSelector("div[title=Inbox]")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Can\'t get inbox folder account : " + Username);
                return false;
            }
        }
    }
}

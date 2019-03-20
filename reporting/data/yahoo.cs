using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace reporting.data
{
    class Yahoo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Proxy { get; set; }
        public string Port { get; set; }

        private IWebDriver Driver { get; set; }
        private ChromeOptions Options { get; set; }
        private WebDriverWait Wait { get; set; }
        private Random Random { get; set; } = new Random();

        public Yahoo()
        {

        }

        public Yahoo(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public Yahoo(string username, string password, string proxy, string port)
        {
            Username = username;
            Password = password;
            Proxy = proxy;
            Port = port;
        }

        public void Init(bool proxy = true, bool adblock = false, bool incognito = true)
        {
            try
            {
                //service
                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;
                //options
                Options = new ChromeOptions();
                Options.AddArguments("disable-infobars");
                Options.AddArguments("user-agent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36 OPR/18.0.1284.63");
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
            catch(Exception ex)
            {
                new Logger().Fatal("init error : " + ex.ToString());
            }
        }

        public void Connect(int timeout = 5, string url = "https://login.yahoo.com/")
        {
            try
            {
                Driver.Navigate().GoToUrl(url);
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

                //username
                IWebElement userfeild = Driver.FindElement(By.CssSelector("#login-username"));
                foreach (var item in Username)
                {
                    userfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 100));
                }
                Driver.FindElement(By.CssSelector("#login-username")).Submit();

                Wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));

                IWebElement passfeild = Driver.FindElement(By.Name("password"));
                //password
                foreach (var item in Password)
                {
                    passfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 100));
                }
                Driver.FindElement(By.CssSelector("#login-signin")).Click();
            }
            catch(Exception ex)
            {
                new Logger().Error("Connect Error : "+ex.Message);
            }
        }

        public bool CheckIfConnected()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#challenge-selector-challenge > form > ul > li > div.pure-u-1-4 > div")));
                new Logger().Error("This Account Need a verification : " + Username );
                return false;
            }
            catch (WebDriverTimeoutException)
            {
                return true;
            }
        }

        public void Navigate(string url)
        {
            Driver.Url = url;
            Driver.Navigate();
        }

        public bool GoToSpamFolder()
        {
            try
            { 
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"app\"]/div[1]/div/div[1]/nav/div/div[3]/div[1]/ul/li[7]/div/a")));
                Driver.FindElement(By.XPath("//*[@id=\"app\"]/div[1]/div/div[1]/nav/div/div[3]/div[1]/ul/li[7]/div/a")).Click();
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                new Logger().Error("Can\'t get spam folder account : " + Username);
                return false;
            }
        }

        public void ReadNotSpam(string subject = "")
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("button[title='mysite.com']")));
                Driver.FindElement(By.XPath("//*[@id=\"mail - app - component\"]/div/div/div[2]/div/div[2]/div/div[2]/div[2]/ul[1]/li[3]/a")).Click();
            }
            catch(WebDriverTimeoutException)
            {

            }
        }

        ~Yahoo()
        {
            Driver.Quit();
        }
    }
}

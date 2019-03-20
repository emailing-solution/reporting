using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace reporting.data
{
    class Gmail
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Proxy { get; set; }
        public string Port { get; set; }

        private IWebDriver Driver { get; set; }
        private ChromeOptions Options { get; set; }
        private WebDriverWait Wait { get; set; }
        private Random Random { get; set; } = new Random();

        public Gmail()
        {

        }

        public Gmail(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public Gmail(string Username, string Password, string Proxy, string Port)
        {
            this.Username = Username;
            this.Password = Password;
            this.Proxy = Proxy;
            this.Port = Port;
        }

        public void Init(bool proxy = true, bool adblock = false, bool incognito = true)
        {
            //service
            ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            //options
            Options = new ChromeOptions();
            Options.AddArguments("disable-infobars");
            if (incognito)
            {
                Options.AddArguments("--incognito");
            }
            if(adblock)
            {
                Options.AddExtension("adblock.crx");
            }
            if(proxy && this.Proxy != string.Empty)
            {
                Options.AddArguments("--proxy-server=" + this.Proxy + ":" + Port);
            }
            //resize window
            Options.AddArguments("--window-size=" + Random.Next(500, 1500) + "," + Random.Next(800, 1000));
            Driver = new ChromeDriver(chromeDriverService, Options);
            
        }

        public void Connect(int timeout = 10, string url = "https://mail.google.com/")
        {
            Driver.Navigate().GoToUrl(url);
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

            //username
            IWebElement userfeild = Driver.FindElement(By.CssSelector("#identifierId"));
            foreach (var item in Username)
            {
                userfeild.SendKeys(item.ToString());
                Thread.Sleep(Random.Next(0, 100));
            }
            Driver.FindElement(By.CssSelector("#identifierNext")).Click();

            Wait.Until(ExpectedConditions.ElementIsVisible(By.Name("password")));

            IWebElement passfeild = Driver.FindElement(By.Name("password"));
            //password
            foreach (var item in Password)
            {
                passfeild.SendKeys(item.ToString());
                Thread.Sleep(Random.Next(0, 100));
            }
            Driver.FindElement(By.Id("passwordNext")).Click();
        }




    }
}

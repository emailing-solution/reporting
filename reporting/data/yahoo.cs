using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

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
            catch (Exception ex)
            {
                new Logger().Error("Connect Error : " + ex.Message);
            }
        }

        public bool CheckIfConnected()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("#challenge-selector-challenge > form > ul > li > div.pure-u-1-4 > div")));
                new Logger().Error("This Account Need a verification : " + Username);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"bulk\"]/a")));
                Driver.FindElement(By.XPath("//*[@id=\"bulk\"]/a")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Can\'t get spam folder account : " + Username);
                return false;
            }
        }

        public bool ReadNotSpam()
        {
            var click = new List<string>
            {   "//*[@id=\"datatable\"]/tbody/tr[1]/td[4]/div/a",
                "//*[@id=\"datatable\"]/tbody/tr[1]/td[6]/h2/a"
            };

            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            const string scrolltop = @"const scrollToTop=()=>{const o=document.documentElement.scrollTop||document.body.scrollTop;o>0&&(window.requestAnimationFrame(scrollToTop),window.scrollTo(0,o-o/20))};scrollToTop();";

            try
            {
                //wait until spam emails loaded;
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]")));
                Driver.FindElement(By.XPath(click[Random.Next(click.Count)])).Click();
                Thread.Sleep(Random.Next(500, 1000));

                //wait until email loaded and scroll down and up
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                js.ExecuteScript("window.scrollTo(0,document.body.scrollHeight);");
                Thread.Sleep(Random.Next(1000, 2000));
                js.ExecuteScript(scrolltop);
                Thread.Sleep(Random.Next(500, 1000));

                //select not spam button
                SelectElement select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                select.SelectByIndex(3);
                Thread.Sleep(Random.Next(500, 1500));
                Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/span[2]/input")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool CheckNotSpam()
        {
            try
            {
                //checkbox button //*[@id="datatable"]/tbody/tr[1]/td[2]/input
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]/td[2]/input")));
                Driver.FindElement(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]/td[2]/input")).Click();
                Thread.Sleep(Random.Next(500, 3000));

                //click not spam button
                Driver.FindElement(By.XPath("//*[@id=\"top_ham\"]")).Click();
                Thread.Sleep(Random.Next(500, 1000));

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool CheckAllNotSpam()
        {
            try
            {
                //checkbox button //*[@id="datatable"]/tbody/tr[1]/td[2]/input
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]/td[2]/input")));
                Driver.FindElement(By.XPath("//*[@id=\"select_all\"]")).Click();
                Thread.Sleep(Random.Next(500, 3000));

                //click not spam button
                Driver.FindElement(By.XPath("//*[@id=\"top_ham\"]")).Click();
                Thread.Sleep(Random.Next(500, 1000));

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool GotoInboxFolder()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"inbox\"]/a")));
                Driver.FindElement(By.XPath("//*[@id=\"inbox\"]/a")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Can\'t get inbox folder account : " + Username);
                return false;
            }
        }

        public object[] OpenArchive(int size, string subject = "")
        {
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid="+size+"&filterBy=";
            Navigate(link);
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("//*[@id=\"datatable\"]/tbody/tr"));
                foreach (IWebElement tr in tableRows)
                {
                    IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                    string status = ((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].innerHTML", tds[0].FindElements(By.TagName("b"))[0]).ToString();
                    
                    if (status.ToLower().Contains("unread"))
                    {
                        if (!string.IsNullOrEmpty(subject))
                        {
                            if (tds[5].Text.Contains(subject))
                            {
                                tds[5].Click();
                                SelectElement select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                                select.SelectByText("Archive");
                                Thread.Sleep(Random.Next(500, 3000));
                                Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/span[2]/input")).Click();
                                Thread.Sleep(Random.Next(500, 3000));
                                return new object[] { true, size };

                            }
                        }
                        else
                        {
                            tds[5].Click();
                            SelectElement select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                            select.SelectByText("Archive");
                            Thread.Sleep(Random.Next(500, 3000));
                            Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/span[2]/input")).Click();
                            Thread.Sleep(Random.Next(500, 1000));

                        }

                        return new object[] {true, size};
                    }
                }
                return new object[] { true, size+25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("inbox empty or all email are readed : "+Username);
                return new object[] { false };
            }
        }

        public void Destroy()
        {
            Driver.Close();
            Driver.Quit();
        }


        ~Yahoo()
        {
            Driver.Quit();
        }
    }
}

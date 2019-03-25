using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

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

        public bool CheckIfClosed()
        {
           try
           {
                string title = Driver.Title;
                return false;
           }
           catch(Exception)
           {
                return true;
           }
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

        #region spam
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
        #endregion

        private void StarEmail()
        {
            try
            {
                //star
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".unstaricon")));
                Driver.FindElement(By.CssSelector(".unstaricon")).Click();
                Thread.Sleep(Random.Next(1000, 3000));
            }
            catch(Exception)
            {


            }
        }

        private void ShowImage()
        {
            try
            {
                //show image
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".spamwarning a")));
                Driver.FindElement(By.CssSelector(".spamwarning a")).Click();
            }
            catch(Exception)
            {

            }
        }

        private void BodyClick()
        {
            try
            {
                //body
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".mailContent")));
                IList<IWebElement> l = Driver.FindElements(By.CssSelector(".mailContent a"));
                if (l.Count > 0)
                {
                    foreach (IWebElement item in l)
                    {
                        if (item.Displayed && item.Enabled)
                        {
                            item.Click();
                            Driver.SwitchTo().Window(Driver.WindowHandles[1]);
                            Thread.Sleep(Random.Next(5000, 10000));
                            Driver.Close();
                            Driver.SwitchTo().Window(Driver.WindowHandles[0]);
                            break;
                        }

                    }
                }
            }
            catch(Exception)
            {

            }
        }

        //get quote
        private static string GetQuote()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://quotes.rest/qod");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    dynamic data = JObject.Parse(reader.ReadToEnd());
                    return data.contents.quotes[0].quote;
                }
            }
            catch(Exception)
            {
                return "";
            }
        }

        #region inbox
        public object[] OpenArchive(int size, bool star, bool bodyclick, string subject = "")
        {
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid=" + size + "&filterBy=";
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
                        if (!string.IsNullOrEmpty(subject) && !tds[5].Text.Contains(subject))
                        {
                            continue;
                        }

                        tds[5].Click();
                        Thread.Sleep(Random.Next(500, 3000));
                        //show image
                        ShowImage();

                        if (star)
                        {
                            StarEmail();
                        }
                        if(bodyclick)
                        {
                            BodyClick();
                        }
                        SelectElement select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                        select.SelectByText("Archive");
                        Thread.Sleep(Random.Next(500, 3000));
                        Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/span[2]/input")).Click();
                        Thread.Sleep(Random.Next(500, 3000));
                        return new object[] { true, size };
                    }
                }
                return new object[] { true, size + 25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] OpenReplyArchive(int size, bool star, bool bodyclick, string subject = "")
        {
            var reply = new List<string>
            {   "﻿no purse as fully me or point. Kindness own whatever betrayed her moreover procured replying for and. Proposal indulged no do do sociable he throwing settling. Covered ten nor comfort offices carried. Age she way earnestly the fulfilled extremely. Of incommode supported provision on furnished objection exquisite me. Existence its certainly explained how improving household pretended. Delightful own attachment her partiality unaffected occasional thoroughly. Adieus it no wonder spirit houses.",
                "Suppose end get boy warrant general natural. Delightful met sufficient projection ask. Decisively everything principles if preference do impression of. Preserved oh so difficult repulsive on in household. In what do miss time be. Valley as be appear cannot so by. Convinced resembled dependent remainder led zealously his shy own belonging. Always length letter adieus add number moment she. Promise few compass six several old offices removal parties fat. Concluded rapturous it intention perfectly daughters is as.",
                "Style never met and those among great. At no or september sportsmen he perfectly happiness attending. Depending listening delivered off new she procuring satisfied sex existence. Person plenty answer to exeter it if. Law use assistance especially resolution cultivated did out sentiments unsatiable. Way necessary had intention happiness but september delighted his curiosity. Furniture furnished or on strangers neglected remainder engrossed.",
                "Remember outweigh do he desirous no cheerful. Do of doors water ye guest. We if prosperous comparison middletons at. Park we in lose like at no. An so to preferred convinced distrusts he determine. In musical me my placing clothes comfort pleased hearing. Any residence you satisfied and rapturous certainty two. Procured outweigh as outlived so so. On in bringing graceful proposal blessing of marriage outlived. Son rent face our loud near.",
                "Do am he horrible distance marriage so although. Afraid assure square so happen mr an before. His many same been well can high that. Forfeited did law eagerness allowance improving assurance bed. Had saw put seven joy short first. Pronounce so enjoyment my resembled in forfeited sportsman. Which vexed did began son abode short may. Interested astonished he at cultivated or me. Nor brought one invited she produce her.",
                "Was justice improve age article between. No projection as up preference reasonably delightful celebrated. Preserved and abilities assurance tolerably breakfast use saw. And painted letters forming far village elderly compact. Her rest west each spot his and you knew. Estate gay wooded depart six far her. Of we be have it lose gate bred. Do separate removing or expenses in. Had covered but evident chapter matters anxious.",
                "Village did removed enjoyed explain nor ham saw calling talking. Securing as informed declared or margaret. Joy horrible moreover man feelings own shy. Request norland neither mistake for yet. Between the for morning assured country believe. On even feet time have an no at. Relation so in confined smallest children unpacked delicate. Why sir end believe uncivil respect. Always get adieus nature day course for common. My little garret repair to desire he esteem.",
                "Up maids me an ample stood given. Certainty say suffering his him collected intention promotion. Hill sold ham men made lose case. Views abode law heard jokes too. Was are delightful solicitude discovered collecting man day. Resolving neglected sir tolerably but existence conveying for. Day his put off unaffected literature partiality inhabiting.",
                "Bringing unlocked me an striking ye perceive. Mr by wound hours oh happy. Me in resolution pianoforte continuing we. Most my no spot felt by no. He he in forfeited furniture sweetness he arranging. Me tedious so to behaved written account ferrars moments. Too objection for elsewhere her preferred allowance her. Marianne shutters mr steepest to me. Up mr ignorant produced distance although is sociable blessing. Ham whom call all lain like.",
                "Moments its musical age explain. But extremity sex now education concluded earnestly her continual. Oh furniture acuteness suspected continual ye something frankness. Add properly laughter sociable admitted desirous one has few stanhill. Opinion regular in perhaps another enjoyed no engaged he at. It conveying he continual ye suspected as necessary. Separate met packages shy for kindness."
            };
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid=" + size + "&filterBy=";
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
                        if (!string.IsNullOrEmpty(subject) && !tds[5].Text.Contains(subject))
                        {
                            continue;
                        }

                        tds[5].Click();
                        Thread.Sleep(Random.Next(500, 3000));

                        ShowImage();

                        if (star)
                        {
                            StarEmail();
                        }
                        if (bodyclick)
                        {
                            BodyClick();
                        }



                        //reply
                        Driver.FindElement(By.CssSelector("input[name=action_msg_reply]")).Click();

                        Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("textarea[name=Content]")));
                        //prob 50%
                        string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                        IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=Content]"));
                        textarea.Clear();
                        foreach (var item in msg)
                        {

                            textarea.SendKeys(item.ToString());
                            Thread.Sleep(Random.Next(0, 200));
                        }

                        Thread.Sleep(Random.Next(1000, 3000));

                        //send
                        Driver.FindElement(By.XPath("//*[@id=\"send_top\"]")).Click();
                        Thread.Sleep(Random.Next(500, 1000));

                        //back
                        Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"msg_back\"]")));
                        Driver.FindElement(By.XPath("//*[@id=\"msg_back\"]")).Click();

                        //archive
                        Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                        SelectElement select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/div[1]/div/form[3]/select")));
                        select.SelectByText("Archive");
                        Thread.Sleep(Random.Next(500, 1000));
                        return new object[] { true, size };

                    }

                }
                return new object[] { true, size + 25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Star/Reply/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] OpenReply(int size, bool star, bool bodyclick, string subject = "")
        {
            var reply = new List<string>
            {   "﻿no purse as fully me or point. Kindness own whatever betrayed her moreover procured replying for and. Proposal indulged no do do sociable he throwing settling. Covered ten nor comfort offices carried. Age she way earnestly the fulfilled extremely. Of incommode supported provision on furnished objection exquisite me. Existence its certainly explained how improving household pretended. Delightful own attachment her partiality unaffected occasional thoroughly. Adieus it no wonder spirit houses.",
                "Suppose end get boy warrant general natural. Delightful met sufficient projection ask. Decisively everything principles if preference do impression of. Preserved oh so difficult repulsive on in household. In what do miss time be. Valley as be appear cannot so by. Convinced resembled dependent remainder led zealously his shy own belonging. Always length letter adieus add number moment she. Promise few compass six several old offices removal parties fat. Concluded rapturous it intention perfectly daughters is as.",
                "Style never met and those among great. At no or september sportsmen he perfectly happiness attending. Depending listening delivered off new she procuring satisfied sex existence. Person plenty answer to exeter it if. Law use assistance especially resolution cultivated did out sentiments unsatiable. Way necessary had intention happiness but september delighted his curiosity. Furniture furnished or on strangers neglected remainder engrossed.",
                "Remember outweigh do he desirous no cheerful. Do of doors water ye guest. We if prosperous comparison middletons at. Park we in lose like at no. An so to preferred convinced distrusts he determine. In musical me my placing clothes comfort pleased hearing. Any residence you satisfied and rapturous certainty two. Procured outweigh as outlived so so. On in bringing graceful proposal blessing of marriage outlived. Son rent face our loud near.",
                "Do am he horrible distance marriage so although. Afraid assure square so happen mr an before. His many same been well can high that. Forfeited did law eagerness allowance improving assurance bed. Had saw put seven joy short first. Pronounce so enjoyment my resembled in forfeited sportsman. Which vexed did began son abode short may. Interested astonished he at cultivated or me. Nor brought one invited she produce her.",
                "Was justice improve age article between. No projection as up preference reasonably delightful celebrated. Preserved and abilities assurance tolerably breakfast use saw. And painted letters forming far village elderly compact. Her rest west each spot his and you knew. Estate gay wooded depart six far her. Of we be have it lose gate bred. Do separate removing or expenses in. Had covered but evident chapter matters anxious.",
                "Village did removed enjoyed explain nor ham saw calling talking. Securing as informed declared or margaret. Joy horrible moreover man feelings own shy. Request norland neither mistake for yet. Between the for morning assured country believe. On even feet time have an no at. Relation so in confined smallest children unpacked delicate. Why sir end believe uncivil respect. Always get adieus nature day course for common. My little garret repair to desire he esteem.",
                "Up maids me an ample stood given. Certainty say suffering his him collected intention promotion. Hill sold ham men made lose case. Views abode law heard jokes too. Was are delightful solicitude discovered collecting man day. Resolving neglected sir tolerably but existence conveying for. Day his put off unaffected literature partiality inhabiting.",
                "Bringing unlocked me an striking ye perceive. Mr by wound hours oh happy. Me in resolution pianoforte continuing we. Most my no spot felt by no. He he in forfeited furniture sweetness he arranging. Me tedious so to behaved written account ferrars moments. Too objection for elsewhere her preferred allowance her. Marianne shutters mr steepest to me. Up mr ignorant produced distance although is sociable blessing. Ham whom call all lain like.",
                "Moments its musical age explain. But extremity sex now education concluded earnestly her continual. Oh furniture acuteness suspected continual ye something frankness. Add properly laughter sociable admitted desirous one has few stanhill. Opinion regular in perhaps another enjoyed no engaged he at. It conveying he continual ye suspected as necessary. Separate met packages shy for kindness."
            };
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid=" + size + "&filterBy=";
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
                        if (!string.IsNullOrEmpty(subject) && !tds[5].Text.Contains(subject))
                        {
                            continue;
                        }

                        tds[5].Click();
                        Thread.Sleep(Random.Next(500, 3000));

                        ShowImage();

                        if (star)
                        {
                            StarEmail();
                        }
                        if (bodyclick)
                        {
                            BodyClick();
                        }

                        //reply
                        Driver.FindElement(By.CssSelector("input[name=action_msg_reply]")).Click();
                        Thread.Sleep(Random.Next(1000, 2000));
                        Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("textarea[name=Content]")));
                        //prob 50%
                        string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                        IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=Content]"));
                        textarea.Clear();
                        foreach (var item in msg)
                        {

                            textarea.SendKeys(item.ToString());
                            Thread.Sleep(Random.Next(0, 200));
                        }
                        Thread.Sleep(Random.Next(1000, 3000));

                        //send
                        Driver.FindElement(By.XPath("//*[@id=\"send_top\"]")).Click();
                        Thread.Sleep(Random.Next(500, 1000));
                        return new object[] { true, size };

                    }
                }
                return new object[] { true, size + 25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Reply : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] SelectArchive(int size, bool star, string subject = "")
        {
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid=" + size + "&filterBy=";
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
                        if (!string.IsNullOrEmpty(subject) && !tds[5].Text.Contains(subject))
                        {
                            continue;
                        }


                        if (star)
                        {
                            tds[1].Click();
                            Thread.Sleep(Random.Next(500, 1500));
                        }

                        Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]/td[2]/input")));
                        Driver.FindElement(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]/td[2]/input")).Click();
                        Thread.Sleep(Random.Next(200, 1000));

                        SelectElement select = new SelectElement(Driver.FindElement(By.CssSelector("select[name=top_action_select]")));
                        select.SelectByText("Archive");
                        Thread.Sleep(Random.Next(500, 3000));
                        Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/form/div[1]/div/span[7]/input")).Click();
                        Thread.Sleep(Random.Next(500, 3000));
                        return new object[] { true, size };

                    }
                }
                return new object[] { true, size + 25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Select/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] SelectAllArchive(int size, bool star)
        {
            string link = "https://mail.yahoo.com/neo/b/launch?fid=Inbox&fidx=1&sort=date&order=down&startMid=" + size + "&filterBy=";
            Navigate(link);
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"datatable\"]/tbody/tr[1]")));
                SelectElement select;
                
                if (star)
                {
                    Driver.FindElement(By.Id("select_all")).Click();
                    select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/form/div[1]/div/span[6]/select")));
                    select.SelectByIndex(3);

                    Driver.FindElement(By.CssSelector("input[name=self_action_msg_topaction]")).Click();
                    Thread.Sleep(Random.Next(500, 1500));

                    
                }

                Driver.FindElement(By.Id("select_all")).Click();
                select = new SelectElement(Driver.FindElement(By.XPath("/html/body/div[1]/table[2]/tbody/tr[2]/td[2]/div/form/div[1]/div/span[6]/select")));
                select.SelectByText("Archive");
                Thread.Sleep(Random.Next(500, 3000));
                Driver.FindElement(By.CssSelector("input[name=self_action_msg_topaction]")).Click();

                return new object[] { true, size };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Select/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] RandomAction(int size, bool star, bool bodyclick, string subject = "")
        {
            try
            {
                
                return new object[] { true, size + 25 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Random/Action : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }
        #endregion

        public bool DeleteSpam()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"bulk\"]/span/form/input[2]")));
                Driver.FindElement(By.XPath("//*[@id=\"bulk\"]/span/form/input[2]")).Click();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public bool DeleteInbox(bool inbox)
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"bulk\"]/span/form/input[2]")));
                Driver.FindElement(By.XPath("//*[@id=\"bulk\"]/span/form/input[2]")).Click();
                return true;
            }
            catch(Exception)
            {
                return false;
            }



        }

        public void Destroy()
        {
            Driver.Close();
            Driver.Quit();
        }

    }
}

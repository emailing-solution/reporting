using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace reporting.data
{
    class Gmail
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private string Proxy { get; set; }
        private string Port { get; set; }

        private IWebDriver Driver { get; set; }
        private ChromeOptions Options { get; set; }
        private WebDriverWait Wait { get; set; }
        private Random Random { get; set; } = new Random();

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

        public bool Init(bool proxy = true, bool adblock = false, bool incognito = true)
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
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error");
                new Logger().Fatal("init error : " + ex.ToString());
                return false;
            }
        }

        public void Connect(int timeout = 10, string url = "https://mail.google.com/")
        {
            try
            {
                Driver.Navigate().GoToUrl(url);
                Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeout));

                //username
                IWebElement userfeild = Driver.FindElement(By.CssSelector("#identifierId"));
                foreach (var item in Username)
                {
                    userfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 300));
                }
                Driver.FindElement(By.CssSelector("#identifierNext")).Click();

                Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[name=password]")));

                IWebElement passfeild = Driver.FindElement(By.CssSelector("input[name=password]"));
                //password
                foreach (var item in Password)
                {
                    passfeild.SendKeys(item.ToString());
                    Thread.Sleep(Random.Next(0, 300));
                }
                Driver.FindElement(By.Id("passwordNext")).Click();
            }
            catch(Exception ex)
            {
                new Logger().Error("Connect Error : " + ex.Message);
            }
        }

        public void Navigate(string url)
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
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("recoveryBumpPickerEntry")));
                new Logger().Error("This Gmail Account Need a verification : " + Username);
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return true;
            }
        }

        public bool OldGmail()
        {
            try
            {
                Navigate("https://mail.google.com/mail/u/0/h/");
                Wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[type=submit]")));
                Thread.Sleep(Random.Next(100, 800));
                Driver.FindElement(By.CssSelector("input[type=submit]")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
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

        #region SPAM
        public bool GotoSpamFolder()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/table[2]/tbody/tr/td[1]/table[1]/tbody/tr[8]/td")));
                Thread.Sleep(Random.Next(100, 800));
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[1]/table[1]/tbody/tr[8]/td")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool ReadNotSpam()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[3]")).Click();

                Thread.Sleep(Random.Next(1000, 2000));

                //not spam button
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name=nvp_a_us]")));
                Thread.Sleep(Random.Next(1000, 1500));
                Driver.FindElement(By.CssSelector("input[name = nvp_a_us]")).Click();
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
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                Thread.Sleep(Random.Next(500, 2000));
                //click checkbox
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]/td[1]/input")).Click();

                Thread.Sleep(Random.Next(1000, 2000));

                //not spam button
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name=nvp_a_us]")));
                Thread.Sleep(Random.Next(1000, 1500));
                Driver.FindElement(By.CssSelector("input[name = nvp_a_us]")).Click();
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
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                
                //click in all checkbox of non read emails :)
                foreach (IWebElement tr in tableRows)
                {
                    if(tr.GetAttribute("bgcolor") == "#ffffff")
                    {
                        IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                        tds[0].FindElement(By.TagName("input")).Click();
                    }
                    Thread.Sleep(Random.Next(0, 1000));
                }

                //click not spam :)
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name=nvp_a_us]")));
                Thread.Sleep(Random.Next(1000, 1500));
                Driver.FindElement(By.CssSelector("input[name = nvp_a_us]")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Archive : inbox empty or all email are readed : " + Username);
                return false;
            }
        }
        #endregion

        #region INBOX
        private void StarEmail()
        {
            try
            {
                //star
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/table[4]/tbody/tr/td/table[1]/tbody/tr[1]/td[1]/a")));
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/table[4]/tbody/tr/td/table[1]/tbody/tr[1]/td[1]/a")).Click();
                Thread.Sleep(Random.Next(1000, 3000));
            }
            catch (Exception)
            {
            }
        }

        private void ShowImage()
        {
            try
            {
                //show image
                //Wait.Until(ExpectedConditions.ElementExists(By.XPath("./html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/table[4]/tbody/tr/td/table[1]/tbody/tr[4]/td/div/div/a[1]")));
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/table[4]/tbody/tr/td/table[1]/tbody/tr[4]/td/div/div/a[1]")).Click();
            }
            catch (Exception)
            {

            }
        }

        private void BodyClick()
        {
            try
            {
                //body
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".msg")));
                IList<IWebElement> l = Driver.FindElements(By.CssSelector(".msg a"));
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
            catch (Exception)
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
            catch (Exception)
            {
                return "";
            }
        }

        public bool GoToInboxFolder()
        {
            try
            {
                Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("/html/body/table[2]/tbody/tr/td[1]/table[1]/tbody/tr[3]/td/h3/b/a")));
                Thread.Sleep(Random.Next(100, 800));
                Driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td[1]/table[1]/tbody/tr[3]/td/h3/b/a")).Click();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public object[] OpenArchive(int size, bool star, bool bodyclick, string subject = "")
        {

            string link = Driver.Url.Split('?')[0]+"?&st=" + size ;
            Navigate(link);
            try
            {

                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                foreach (IWebElement tr in tableRows)
                {
                    if (tr.GetAttribute("bgcolor") == "#ffffff")
                    {
                        IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                        //subject
                        if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                        {
                            continue;
                        }
                        //open
                        tds[2].FindElement(By.TagName("a")).Click();
                        Thread.Sleep(Random.Next(500, 3000));
                        if(star)
                        {
                            StarEmail();
                        }
                        if(bodyclick)
                        {
                            BodyClick();
                        }

                        Thread.Sleep(Random.Next(500, 3000));
                        //archiver
                        Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                        Driver.FindElement(By.CssSelector("input[name = nvp_a_arch]")).Click();
                        return new object[] { true, size};
                    }  
                }


                return new object[] { true, size + 50 };
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

            string link = Driver.Url.Split('?')[0] + "?&st=" + size;
            Navigate(link);
            try
            {

                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                foreach (IWebElement tr in tableRows)
                {
                    if (tr.GetAttribute("bgcolor") == "#ffffff")
                    {
                        IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                        //subject
                        if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                        {
                            continue;
                        }
                        //open
                        tds[2].FindElement(By.TagName("a")).Click();
                        Thread.Sleep(Random.Next(500, 3000));

                        if (star)
                        {
                            StarEmail();
                        }
                        if (bodyclick)
                        {
                            BodyClick();
                        }
                        Thread.Sleep(Random.Next(500, 3000));

                        //prob 50% textarea
                        string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                        IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=body]"));
                        textarea.Clear();
                        foreach (var item in msg)
                        {
                            textarea.SendKeys(item.ToString());
                            Thread.Sleep(Random.Next(0, 200));
                        }

                        //send
                        Driver.FindElement(By.CssSelector("input[name = nvp_bu_send]")).Click();;
                        Thread.Sleep(Random.Next(500, 3000));

                        //archiver
                        Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                        Driver.FindElement(By.CssSelector("input[name = nvp_a_arch]")).Click();
                        return new object[] { true, size };
                    }
                }
                return new object[] { true, size + 50 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Reply/Archive : inbox empty or all email are readed : " + Username);
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

            string link = Driver.Url.Split('?')[0] + "?&st=" + size;
            Navigate(link);
            try
            {

                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                foreach (IWebElement tr in tableRows)
                {
                    if (tr.GetAttribute("bgcolor") == "#ffffff")
                    {
                        IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                        //subject
                        if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                        {
                            continue;
                        }
                        //open
                        tds[2].FindElement(By.TagName("a")).Click();
                        Thread.Sleep(Random.Next(500, 3000));

                        if (star)
                        {
                            StarEmail();
                        }
                        if (bodyclick)
                        {
                            BodyClick();
                        }
                        Thread.Sleep(Random.Next(500, 3000));

                        //prob 50% textarea
                        string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                        IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=body]"));
                        textarea.Clear();
                        foreach (var item in msg)
                        {
                            textarea.SendKeys(item.ToString());
                            Thread.Sleep(Random.Next(0, 200));
                        }

                        //send
                        Driver.FindElement(By.CssSelector("input[name = nvp_bu_send]")).Click(); ;
                        Thread.Sleep(Random.Next(500, 3000));

                        //back to page

                        Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                        Driver.FindElement(By.ClassName("searchPageLink")).Click();
                        return new object[] { true, size };
                    }
                }


                return new object[] { true, size + 50 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Open/Reply/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] SelectArchive(int size, bool star, string subject = "")
        {
            string link = Driver.Url.Split('?')[0] + "?&st=" + size;
            Navigate(link);
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                foreach (IWebElement tr in tableRows)
                {
                    if (tr.GetAttribute("bgcolor") == "#ffffff")
                    {
                        IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                        //subject
                        if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                        {
                            continue;
                        }

                        if (star)
                        {
                            try
                            {
                                string text = tds[0].FindElement(By.TagName("img")).Text;
                            }
                            catch
                            {
                                tds[0].FindElement(By.TagName("input")).Click();
                                Thread.Sleep(Random.Next(500, 3000));
                                SelectElement selectStar = new SelectElement(Driver.FindElement(By.CssSelector("select[name=tact]")));
                                selectStar.SelectByIndex(3);
                                Driver.FindElement(By.CssSelector("input[name=nvp_tbu_go]")).Click();
                                Thread.Sleep(Random.Next(500, 3000));
                                return new object[] { true, size };
                            }
                        }


                        //select archiver
                        tds[0].FindElement(By.TagName("input")).Click();
                        Driver.FindElement(By.CssSelector("input[name=nvp_a_arch]")).Click();
                        Thread.Sleep(Random.Next(500, 1000));

                        return new object[] { true, size };
                    }
                }
                return new object[] { true, size + 50 };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Error("Select/Archive : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }

        public object[] SelectAllArchive(int size, bool star, string subject = "")
        {
            string link = Driver.Url.Split('?')[0] + "?&st=" + size;
            Navigate(link);
            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));

                object[] archiveall()
                {
                    int counter = 0;
                    IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                    foreach (IWebElement tr in tableRows)
                    {
                        if (tr.GetAttribute("bgcolor") == "#ffffff")
                        {
                            counter++;
                            IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                            //subject
                            if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                            {
                                continue;
                            }
                            tds[0].FindElement(By.TagName("input")).Click();
                        }
                        Thread.Sleep(Random.Next(0, 200));
                    }

                   
                    if (counter == 0)
                    {
                        return new object[] { true, size + 50 };
                    }
                    else
                    {
                        Driver.FindElement(By.CssSelector("input[name=nvp_a_arch]")).Click();
                        return new object[] { true, size };
                    }
                }
                if(star)
                {
                    int counter = 0;
                    IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                    foreach (IWebElement tr in tableRows)
                    {
                        if (tr.GetAttribute("bgcolor") == "#ffffff")
                        {
                            counter++;
                            IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                            //subject
                            if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                            {
                                continue;
                            }
                            tds[0].FindElement(By.TagName("input")).Click();
                        }
                        Thread.Sleep(Random.Next(0, 200));
                    }
                    if (counter > 0)
                    {
                        SelectElement selectStar = new SelectElement(Driver.FindElement(By.CssSelector("select[name=tact]")));
                        selectStar.SelectByIndex(3);
                        Driver.FindElement(By.CssSelector("input[name=nvp_tbu_go]")).Click();
                        Thread.Sleep(Random.Next(500, 1000));
                    }
                }
                return archiveall();
                

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
            string link = Driver.Url.Split('?')[0] + "?&st=" + size;
            Navigate(link);

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

            try
            {
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));

                //open
                if (Random.Next(100) <= 50)
                {
                    //open archive
                   
                    if (Random.Next(100) >= 50)
                    {
                        IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                        foreach (IWebElement tr in tableRows)
                        {
                            if (tr.GetAttribute("bgcolor") == "#ffffff")
                            {
                                IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                                //subject
                                if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                {
                                    continue;
                                }
                                //open
                                tds[2].FindElement(By.TagName("a")).Click();
                                Thread.Sleep(Random.Next(500, 3000));
                                if (star)
                                {
                                    StarEmail();
                                }
                                if (bodyclick)
                                {
                                    BodyClick();
                                }

                                Thread.Sleep(Random.Next(500, 3000));
                                //archiver
                                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                                Driver.FindElement(By.CssSelector("input[name = nvp_a_arch]")).Click();
                                return new object[] { true, size };
                            }
                        }
                        return new object[] { true, size + 50 };
                    }
                    else if (Random.Next(100) <= 50) // open reply archive
                    {
                        IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                        foreach (IWebElement tr in tableRows)
                        {
                            if (tr.GetAttribute("bgcolor") == "#ffffff")
                            {
                                IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                                //subject
                                if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                {
                                    continue;
                                }
                                //open
                                tds[2].FindElement(By.TagName("a")).Click();
                                Thread.Sleep(Random.Next(500, 3000));

                                if (star)
                                {
                                    StarEmail();
                                }
                                if (bodyclick)
                                {
                                    BodyClick();
                                }
                                Thread.Sleep(Random.Next(500, 3000));

                                //prob 50% textarea
                                string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                                IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=body]"));
                                textarea.Clear();
                                foreach (var item in msg)
                                {
                                    textarea.SendKeys(item.ToString());
                                    Thread.Sleep(Random.Next(0, 200));
                                }

                                //send
                                Driver.FindElement(By.CssSelector("input[name = nvp_bu_send]")).Click(); ;
                                Thread.Sleep(Random.Next(500, 3000));

                                //archiver
                                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                                Driver.FindElement(By.CssSelector("input[name = nvp_a_arch]")).Click();
                                return new object[] { true, size };
                            }
                        }
                        return new object[] { true, size + 50 };
                    }
                    else // open reply
                    {
                        IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                        foreach (IWebElement tr in tableRows)
                        {
                            if (tr.GetAttribute("bgcolor") == "#ffffff")
                            {
                                IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                                //subject
                                if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                {
                                    continue;
                                }
                                //open
                                tds[2].FindElement(By.TagName("a")).Click();
                                Thread.Sleep(Random.Next(500, 3000));

                                if (star)
                                {
                                    StarEmail();
                                }
                                if (bodyclick)
                                {
                                    BodyClick();
                                }
                                Thread.Sleep(Random.Next(500, 3000));

                                //prob 50% textarea
                                string msg = Random.Next(100) < 50 ? GetQuote() : reply[Random.Next(reply.Count)];
                                IWebElement textarea = Driver.FindElement(By.CssSelector("textarea[name=body]"));
                                textarea.Clear();
                                foreach (var item in msg)
                                {
                                    textarea.SendKeys(item.ToString());
                                    Thread.Sleep(Random.Next(0, 200));
                                }

                                //send
                                Driver.FindElement(By.CssSelector("input[name = nvp_bu_send]")).Click(); ;
                                Thread.Sleep(Random.Next(500, 3000));

                                //back to page

                                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name = nvp_a_arch]")));
                                Driver.FindElement(By.ClassName("searchPageLink")).Click();
                                return new object[] { true, size };
                            }
                        }
                        return new object[] { true, size + 50 };
                    }
                }
                else //select
                {
                    if (Random.Next(100) <= 50) //select - archive
                    {
                        IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                        foreach (IWebElement tr in tableRows)
                        {
                            if (tr.GetAttribute("bgcolor") == "#ffffff")
                            {
                                IList<IWebElement> tds = tr.FindElements(By.TagName("td"));

                                //subject
                                if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                {
                                    continue;
                                }

                                if (star)
                                {
                                    try
                                    {
                                        string text = tds[0].FindElement(By.TagName("img")).Text;
                                    }
                                    catch
                                    {
                                        tds[0].FindElement(By.TagName("input")).Click();
                                        Thread.Sleep(Random.Next(500, 3000));
                                        SelectElement selectStar = new SelectElement(Driver.FindElement(By.CssSelector("select[name=tact]")));
                                        selectStar.SelectByIndex(3);
                                        Driver.FindElement(By.CssSelector("input[name=nvp_tbu_go]")).Click();
                                        Thread.Sleep(Random.Next(500, 3000));
                                        return new object[] { true, size };
                                    }
                                }


                                //select archiver
                                tds[0].FindElement(By.TagName("input")).Click();
                                Driver.FindElement(By.CssSelector("input[name=nvp_a_arch]")).Click();
                                Thread.Sleep(Random.Next(500, 1000));

                                return new object[] { true, size };
                            }
                        }
                        return new object[] { true, size + 50 };
                    }
                    else ////select ALL - archive
                    {

                        object[] archiveall()
                        {
                            IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                            int counter = 0;
                            foreach (IWebElement tr in tableRows)
                            {
                                if (tr.GetAttribute("bgcolor") == "#ffffff")
                                {
                                    counter++;
                                    IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                                    //subject
                                    if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                    {
                                        continue;
                                    }
                                    tds[0].FindElement(By.TagName("input")).Click();
                                }
                                Thread.Sleep(Random.Next(0, 200));
                            }


                            if (counter == 0)
                            {
                                return new object[] { true, size + 50 };
                            }
                            else
                            {
                                Driver.FindElement(By.CssSelector("input[name=nvp_a_arch]")).Click();
                                return new object[] { true, size };
                            }
                        }
                        if (star)
                        {
                            int counter = 0;
                            IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));
                            foreach (IWebElement tr in tableRows)
                            {
                                if (tr.GetAttribute("bgcolor") == "#ffffff")
                                {
                                    counter++;
                                    IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                                    //subject
                                    if (!string.IsNullOrEmpty(subject) && !tds[2].Text.Contains(subject))
                                    {
                                        continue;
                                    }
                                    tds[0].FindElement(By.TagName("input")).Click();
                                }
                                Thread.Sleep(Random.Next(0, 200));
                            }
                            if (counter > 0)
                            {
                                SelectElement selectStar = new SelectElement(Driver.FindElement(By.CssSelector("select[name=tact]")));
                                selectStar.SelectByIndex(3);
                                Driver.FindElement(By.CssSelector("input[name=nvp_tbu_go]")).Click();
                                Thread.Sleep(Random.Next(500, 1000));
                            }
                        }
                        return archiveall();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                new Logger().Info("Random/Action : inbox empty or all email are readed : " + Username);
                return new object[] { false };
            }
        }
        #endregion

        #region delete
        public bool DeleteSpam()
        {
            try
            {
                GotoSpamFolder();
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                //click in all checkbox of non read emails :)
                foreach (IWebElement tr in tableRows)
                {
                    IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                    tds[0].FindElement(By.TagName("input")).Click();
                    Thread.Sleep(Random.Next(0, 100));
                }

                //click delete :)
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name=nvp_a_dl]")));
                Driver.FindElement(By.CssSelector("input[name = nvp_a_dl]")).Click();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteInbox()
        {
            try
            {
                GoToInboxFolder();
                Wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr[1]")));
                IList<IWebElement> tableRows = Driver.FindElements(By.XPath("/html/body/table[2]/tbody/tr/td[2]/table[1]/tbody/tr/td[2]/form/table[2]/tbody/tr"));

                //click in all checkbox of non read emails :)
                foreach (IWebElement tr in tableRows)
                {
                    IList<IWebElement> tds = tr.FindElements(By.TagName("td"));
                    tds[0].FindElement(By.TagName("input")).Click();
                    Thread.Sleep(Random.Next(0, 100));
                }

                //click delete :)
                Wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[name=nvp_a_tr]")));
                Driver.FindElement(By.CssSelector("input[name = nvp_a_tr]")).Click();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public void Destroy()
        {
            Driver.Close();
            Driver.Quit();
        }

    }
}

using IronPython.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Xml;
using SimpleBrowser;
using System.Net;

namespace FlightsEngine.FlighsBot
{
    public static class ScrappingHelper
    {
        public static bool Run()
        {
            bool result = false;
            try
            {
                // https://stackoverflow.com/questions/5320231/scraping-a-webpage-with-c-sharp-and-htmlagility
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START ScrappingHelper ***");
                string url = "https://www.edreams.com/";
                url = "https://www.edreams.com/travel/?locale=en&mktportal=google-brand#results/type=R;dep=2018-12-19;from=AMS;to=YMQ;ret=2018-12-30;direct=true;collectionmethod=false;airlinescodes=false;internalSearch=true";
             //   HtmlAgilityPack.HtmlDocument doc = new HtmlWeb().Load(url);
             //   var rows = doc.DocumentNode.SelectNodes("//table[@class='data']/tr");

                LoadHtmlWithBrowser(url);

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END ScrappingHelper ***");
            }
            catch(Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return result;
        }

        static bool LastRequestFailed(Browser browser)
        {
            if (browser.LastWebException != null)
            {
                FlightsEngine.Utils.Logger.GenerateError(null, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " There was an error loading the page: " + browser.LastWebException.Message);
                return true;
            }
            return false;
        }

        static void OnBrowserMessageLogged(Browser browser, string log)
        {
            FlightsEngine.Utils.Logger.GenerateError(null, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " log : " + log);
        }

        static void OnBrowserRequestLogged(Browser req, HttpRequestLog log)
        {
            Console.WriteLine(" -> " + log.Method + " request to " + log.Url);
            Console.WriteLine(" <- Response status code: " + log.ResponseCode);
        }

        private static void LoadHtmlWithSimpleBrowser(String url,string proxy=null)
        {
            // https://github.com/SimpleBrowserDotNet/SimpleBrowser
            // https://stackoverflow.com/questions/6563901/clicking-button-automatically-using-htmlagilitypack
            var browser = new Browser();
            try
            {
                // log the browser request/response data to files so we can interrogate them in case of an issue with our scraping
                browser.RequestLogged += OnBrowserRequestLogged;
                browser.MessageLogged += new Action<Browser, string>(OnBrowserMessageLogged);

                // we'll fake the user agent for websites that alter their content for unrecognised browsers
                browser.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.10 (KHTML, like Gecko) Chrome/8.0.552.224 Safari/534.10";
                browser.Cookies= new CookieContainer();
                if (!String.IsNullOrWhiteSpace(proxy))
                {
                    browser.SetProxy(proxy.Split(':')[0], Convert.ToInt32(proxy.Split(':')[1]));
                }
                // browse to GitHub
                browser.Navigate("http://github.com/");
                if (LastRequestFailed(browser)) return; // always check the last request in case the page failed to load

                // click the login link and click it
                browser.Log("First we need to log in, so browse to the login page, fill in the login details and submit the form.");
                var loginLink = browser.Find("a", FindBy.Text, "Login");
                if (!loginLink.Exists)
                    browser.Log("Can't find the login link! Perhaps the site is down for maintenance?");
                else
                {
                    loginLink.Click();
                    if (LastRequestFailed(browser)) return;

                    // fill in the form and click the login button - the fields are easy to locate because they have ID attributes
                    browser.Find("login_field").Value = "youremail@domain.com";
                    browser.Find("password").Value = "yourpassword";
                    browser.Find(ElementType.Button, "name", "commit").Click();
                    if (LastRequestFailed(browser)) return;

                    // see if the login succeeded - ContainsText() is very forgiving, so don't worry about whitespace, casing, html tags separating the text, etc.
                    if (browser.ContainsText("Incorrect login or password"))
                    {
                        browser.Log("Login failed!", LogMessageType.Error);
                    }
                    else
                    {
                        // After logging in, we should check that the page contains elements that we recognise
                        if (!browser.ContainsText("Your Repositories"))
                            browser.Log("There wasn't the usual login failure message, but the text we normally expect isn't present on the page");
                        else
                        {
                            browser.Log("Your News Feed:");
                            // we can use simple jquery selectors, though advanced selectors are yet to be implemented
                            foreach (var item in browser.Select("div.news .title"))
                                browser.Log("* " + item.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                browser.Log(ex.Message, LogMessageType.Error);
                browser.Log(ex.StackTrace, LogMessageType.StackTrace);
            }
            finally
            {
             //   var path = WriteFile("log-" + DateTime.UtcNow.Ticks + ".html", browser.RenderHtmlLogFile("SimpleBrowser Sample - Request Log"));
             //   Process.Start(path);
            }
        }

        private static void LoadHtmlWithBrowser(String url)
        {
            WebBrowser webBrowser1 = new WebBrowser();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(url);

            waitTillLoad(webBrowser1);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            var documentAsIHtmlDocument3 = (mshtml.IHTMLDocument3)webBrowser1.Document.DomDocument;
            StringReader sr = new StringReader(documentAsIHtmlDocument3.documentElement.outerHTML);
            doc.Load(sr);
            string text = doc.Text;
        }

        private static void waitTillLoad(WebBrowser webBrControl)
        {
            WebBrowserReadyState loadStatus;
            int waittime = 100000;
            int counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if ((counter > waittime) || (loadStatus == WebBrowserReadyState.Uninitialized) || (loadStatus == WebBrowserReadyState.Loading) || (loadStatus == WebBrowserReadyState.Interactive))
                {
                    break;
                }
                counter++;
            }

            counter = 0;
            while (true)
            {
                loadStatus = webBrControl.ReadyState;
                Application.DoEvents();
                if (loadStatus == WebBrowserReadyState.Complete && webBrControl.IsBusy != true)
                {
                    break;
                }
                counter++;
            }
            Thread.Sleep(20000);
        }

    }
}
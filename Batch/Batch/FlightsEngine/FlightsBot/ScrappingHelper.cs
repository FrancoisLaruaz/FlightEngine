using IronPython.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;
using FlightsEngine.Models;
using System.Collections.Generic;
using FlightsEngine.Utils;
using FlightsEngine.Models.Constants;

namespace FlightsEngine.FlighsBot
{
    public static class ScrappingHelper
    {

        #region url
        public static string GetEdreamsUrl(AirlineSearch filter)
        {
            string url = null;
            try
            {
                url = "https://www.edreams.com/#results/type=R;dep="+filter.FromDate.Value.ToString("yyyy-MM-dd") + ";from="+filter.FromAirportCode+";to="+filter.ToAirportCode+ ";ret=" + filter.ToDate.Value.ToString("yyyy-MM-dd") + ";collectionmethod=false;airlinescodes=false;internalSearch=true";
            }
            catch(Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,  "Filters = " + filter.ToSpecialString());
            }
            return url;
        }


        public static string GetKayakUrl(AirlineSearch filter)
        {
            string url = null;
            try
            {
                url = "https://www.kayak.com/flights/" + filter.FromAirportCode + "-" + filter.ToAirportCode + "/" + filter.FromDate.Value.ToString("yyyy-MM-dd") + "/" + filter.ToDate.Value.ToString("yyyy-MM-dd") + "?sort=bestflight_a";
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Filters = " + filter.ToSpecialString());
            }
            return url;
        }

        #endregion

        public static ScrappingExecutionResult SearchViaScrapping(ScrappingSearch scrappingSearch)
        {
            ScrappingExecutionResult result = new ScrappingExecutionResult();
            try
            {
                int nbMaxAttempts = 30;
                bool continueProcess = true;
                int attemtNumber = 0;

                while (continueProcess)
                {
                    attemtNumber = attemtNumber + 1;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  START SearchViaScrapping ** : " + attemtNumber);
                    result = Run(scrappingSearch);

                    ProxyItem proxyItemToModify = scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy);
                    scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy).UseNumber = proxyItemToModify.UseNumber + 1;
                    if (!result.Success)
                    {
                        scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy).Failure = proxyItemToModify.Failure + 1;
                        scrappingSearch.Proxy = ProxyHelper.GetBestProxy(scrappingSearch.ProxiesList);
                    }

                    continueProcess = !result.Success && attemtNumber < nbMaxAttempts;
                }
                result.LastProxy = scrappingSearch.Proxy;
                result.AttemptsNumber = attemtNumber;
                result.ProxiesList = scrappingSearch.ProxiesList;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = e.ToString();
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Provider = " + scrappingSearch.Provider + " and Proxy = " + scrappingSearch.Proxy + " and url = " + scrappingSearch.Url);
            }
            finally
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  END SearchViaScrapping **");
            }

            return result;
        }


        public static ScrappingExecutionResult Run(ScrappingSearch scrappingSearch)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Scrapping *** : " + (scrappingSearch?.Provider ?? "") + " | " + (scrappingSearch?.Proxy ?? ""));
            ScrappingExecutionResult result = new ScrappingExecutionResult();
            bool success = false;
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            try
            {
                #region preparation and purge
                //1) PREPARATION
                // model =>   D:\DEV\FlightEngine\Batch\WebScraperBash\PrepareScrapping.cmd "D:\DEV\FlightEngine\Batch\WebScraperBash" "126" "83.166.99.11" 54457
                  string args = "\"" + scrappingSearch.ScrappingFolder + "\" \"" + scrappingSearch.SearchTripProviderId + "\" \"" + scrappingSearch.Proxy.Split(':')[0] + "\" " + scrappingSearch.Proxy.Split(':')[1].Split(' ')[0];
             //   string args = "\"" + scrappingSearch.ScrappingFolder + "\" \"" + scrappingSearch.SearchTripProviderId + "\" \"\" \"\" ";
                System.Diagnostics.ProcessStartInfo startInfoPreparation = new System.Diagnostics.ProcessStartInfo();
                startInfoPreparation.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfoPreparation.FileName = "cmd.exe";
                startInfoPreparation.Arguments = string.Format("/C {0} {1}", scrappingSearch.ScrappingPreparationScript,  args);
                startInfoPreparation.RedirectStandardInput = true;
                startInfoPreparation.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = false;
                startInfoPreparation.UseShellExecute = false; ;

                cmd.StartInfo = startInfoPreparation;
                cmd.Start();
                string strResult = "";
                List<string> resultList = new List<string>();
                while (!cmd.StandardOutput.EndOfStream)
                {
                    strResult = cmd.StandardOutput.ReadLine();
                    resultList.Add(strResult);
                }

                if (!String.IsNullOrWhiteSpace(strResult))
                {
                    if (strResult.StartsWith("OK"))
                    {
                        success = true;
                        result.Error = null;
                    }
                }
                #endregion

                #region Scrapping
                // Check if preparation is OK
                if (success)
                {
                    // 2 SCRAPPING 
                    // "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.exe" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams"
                    //  string url = "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true";
                    args = "\"" + scrappingSearch.Url + "\" \"" + scrappingSearch.SearchTripProviderId + "\" \"" + scrappingSearch.FirefoxExeFolder + "\"  \"" + scrappingSearch.Provider + "\"";
                  //  args= ""C:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.exe" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "16" "C:\Program Files\Mozilla Firefox\firefox.exe" "Edreams""
                    System.Diagnostics.ProcessStartInfo startInfoScrapping = new System.Diagnostics.ProcessStartInfo();
                    startInfoScrapping.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfoScrapping.FileName = "cmd.exe";
                    startInfoScrapping.Arguments = string.Format("/C {0} {1}", scrappingSearch.ScrappingExeScript, args);
                    startInfoScrapping.RedirectStandardInput = true;
                    startInfoScrapping.RedirectStandardOutput = true;
                    cmd.StartInfo.CreateNoWindow = false;
                    startInfoScrapping.UseShellExecute = false; ;

                    cmd.StartInfo = startInfoScrapping;
                    cmd.Start();
                    strResult = "";
                    resultList = new List<string>();
                    while (!cmd.StandardOutput.EndOfStream)
                    {
                        strResult = cmd.StandardOutput.ReadLine();
                        resultList.Add(strResult);
                    }

                    int i = 0;

                    string HtmlFile = "D:\\Html\\search_" + scrappingSearch.SearchTripProviderId + ".html";
                    string HtmlErrorFile = "D:\\Html\\search_" + scrappingSearch.SearchTripProviderId + ".xht";
                    string StopSearchFile = "D:\\Html\\stopsearch_" + scrappingSearch.SearchTripProviderId + ".txt";
                    success = false;
                    while (!File.Exists(HtmlFile) && !File.Exists(HtmlErrorFile) && !File.Exists(StopSearchFile) && i<= 75) //limit the time to whait to 30 sec
                    {
                        Thread.Sleep(500);
                        i++;
                    }
                    if (File.Exists(HtmlFile))
                    {
                        success = true;
                    }

                }
                #endregion
            }
            catch (Exception e)
            {
                success = false;
                result.Error = e.ToString();
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Provider = " + scrappingSearch.Provider + " and Proxy = " + scrappingSearch.Proxy + " and url = " + scrappingSearch.Url);
            }
            finally
            {
                cmd.StandardInput.WriteLine("exit");
                cmd.WaitForExit();
                cmd.Close();
                result.Success = success;
            }

            if (success)
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : SUCCESS  " );
            else
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : FAILURE => " + (result.Error ?? ""));
            return result;
        }

    }
}
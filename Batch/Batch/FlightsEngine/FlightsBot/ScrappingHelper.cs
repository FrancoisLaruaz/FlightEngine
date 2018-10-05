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


        public static ScrappingExecutionResult SearchViaScrapping(AirlineSearch filter, ScrappingSearch scrappingSearch)
        {
            ScrappingExecutionResult result = new ScrappingExecutionResult();
            try
            {
                int nbMaxAttempts = 3;
                bool continueProcess = true;

                while (continueProcess)
                {
                    result.LastProxy = scrappingSearch.Proxy;
                    result.AttemptsNumber = result.AttemptsNumber + 1;
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  START SearchViaScrapping ** : " + result.AttemptsNumber);
                    result = Run(filter, scrappingSearch);

                    ProxyItem proxyItemToModify = scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy);
                    scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy).UseNumber = proxyItemToModify.UseNumber + 1;
                    if (!result.Success)
                    {
                        scrappingSearch.ProxiesList.Find(p => p.Proxy == scrappingSearch.Proxy).Failure = proxyItemToModify.Failure + 1;
                        scrappingSearch.Proxy = ProxyHelper.GetBestProxy(scrappingSearch.ProxiesList);
                    }

                    continueProcess = !result.Success && result.AttemptsNumber < nbMaxAttempts;
                }
                result.ProxiesList = scrappingSearch.ProxiesList;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = e.ToString();
            }
            finally
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  END SearchViaScrapping **");
            }

            return result;
        }


        public static ScrappingExecutionResult Run(AirlineSearch filter, ScrappingSearch scrappingSearch)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Scrapping *** : " + (scrappingSearch?.Provider ?? "") + " | " + (scrappingSearch?.Proxy ?? ""));
            ScrappingExecutionResult result = new ScrappingExecutionResult();
            System.Diagnostics.Process cmd = new System.Diagnostics.Process();
            try
            {
                #region preparation and purge
                //1) PREPARATION
                // model =>   D:\DEV\FlightEngine\Batch\WebScraperBash\PrepareScrapping.cmd "D:\DEV\FlightEngine\Batch\WebScraperBash" "126" "83.166.99.11" 54457
                string args = "\"" + scrappingSearch.ScrappingFolder + "\" \"" + scrappingSearch.SearchTripProviderId + "\" \"" + scrappingSearch.Proxy.Split(':')[0] + "\" " + scrappingSearch.Proxy.Split(':')[1];

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
                        result.Success = true;
                        result.Error = null;
                    }
                }
                #endregion

                #region Scrapping
                // Check if preparation is OK
                if (result.Success)
                {
                    // 2 SCRAPPING 
                    // "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.exe" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams"
                    string url = "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true";
                    args = "\"" + url + "\" \"" + scrappingSearch.SearchTripProviderId + "\" \"" + scrappingSearch.FirefoxExeFolder + "\"  \"" + scrappingSearch.Provider + "\"";
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

                    while (!File.Exists(HtmlFile) && !File.Exists(HtmlErrorFile) && !File.Exists(StopSearchFile) && i<= 120) //limit the time to whait to 30 sec
                    {
                        Thread.Sleep(500);
                        i++;
                        if(File.Exists(HtmlFile))
                        {
                            result.Success = true;
                        }
                    }

                }
                #endregion
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Error = e.ToString();
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Provider = " + scrappingSearch.Provider + " and Proxy = " + scrappingSearch.Proxy + " and filters = " + filter.ToSpecialString());
            }
            finally
            {
                cmd.StandardInput.WriteLine("exit");
                cmd.WaitForExit();
                cmd.Close();

            }

            if (result.Success)
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : SUCCESS  " );
            else
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : FAILURE => " + (result.Error ?? ""));
            return result;
        }

    }
}
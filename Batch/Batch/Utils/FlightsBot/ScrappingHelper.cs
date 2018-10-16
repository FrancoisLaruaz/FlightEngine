using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using FlightsEngine.Models;
using System.Collections.Generic;
using FlightsEngine.Utils;
using FlightsEngine.Models.Constants;
using HtmlAgilityPack;
using System.Linq;

namespace FlightsEngine.Utils
{
    public static class ScrappingHelper
    {

        public static Tuple<string, decimal> GetPriceWithCurrencyFromHtml(string text)
        {
            Tuple<string, decimal> result = new Tuple<string, decimal>("", -1);
            try
            {
                string currencyCode = "";
                string strPrice = "";
                foreach (char c in text)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '.' || c == ',')
                    {
                        strPrice = strPrice + c;
                    }
                    else if (c != ' ')
                    {
                        currencyCode = currencyCode + c;
                    }
                }
                if (!string.IsNullOrWhiteSpace(strPrice) && !string.IsNullOrWhiteSpace(currencyCode))
                {
                    result = new Tuple<string, decimal>(currencyCode, Convert.ToDecimal(strPrice));
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "text = " + text);
            }
            return result;
        }

        public static DateTime GetArrivalFlightDateFromHtml(string text, DateTime BaseDate)
        {
            DateTime result = BaseDate;
            try
            {
                string strDay = "";

                foreach (char c in text)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' )
                    {
                        strDay = strDay + c;
                    }
                }
                int Day = Convert.ToInt32(strDay);
                int BaseDay = BaseDate.Day;
                if(Day> BaseDay)
                {
                    result = new DateTime(BaseDate.Year,BaseDate.Month,Day);
                }
                else
                {
                    if(BaseDate.Month==12)
                    {
                        result = new DateTime(BaseDate.Year+1, 1, Day);
                    }
                    else
                    {
                        result = new DateTime(BaseDate.Year ,  BaseDate.Month+1, Day);
                    }
                }

            }
            catch (Exception e)
            {
                result = BaseDate;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "text = " + text + " and BaseDate = " + BaseDate);
            }
            return result;
        }

        public static string GeFlightDateFromHtml(DateTime BaseDate, string text)
        {
            string result = "";
            try
            {
                text = text.ToUpper().Replace(" ", "").Replace(":", "H");
                int BaseDuration = 0;
                if (text.Contains("PM"))
                {
                    BaseDuration = 12 * 60;
                }
                if (text.Contains("PM") && text.Contains("12H"))
                {
                    text = "0H" + text.Split('H')[1];
                }
                text = text.Replace("AM", "").Replace("PM", "");
                int Duration = BaseDuration + GetDurationFromHtml(text + "M");
                result = BaseDate.AddMinutes(Duration).ToString("dd/MM/yyyy hh:mm").Replace("-","/");

            }
            catch (Exception e)
            {
                result = "";
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "text = " + text + " and BaseDate = " + BaseDate);
            }
            return result;
        }

        public static int GetDurationFromHtml(string text)
        {
            int result = -1;
            try
            {
                text = text.ToUpper();
                int days = 0;
                if (text.Contains('D'))
                {
                    days = Convert.ToInt32(text.Split('D')[0]);
                    text = text.Replace(text.Split('D')[0] + "D", "");
                }

                int hours = 0;
                if (text.Contains('H'))
                {
                    hours = Convert.ToInt32(text.Split('H')[0]);
                    text = text.Replace(text.Split('H')[0] + "H", "");
                }

                int minutes = 0;
                if (text.Contains('M'))
                {
                    minutes = Convert.ToInt32(text.Split('M')[0]);
                    text = text.Replace(text.Split('M')[0] + "M", "");
                }
                result = minutes + (24 * days + hours) * 60;
            }
            catch (Exception e)
            {
                result = -1;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "text = " + text);
            }
            return result;
        }



        public static int GeStopsNumberFromHtml(string text)
        {
            int result = 0;
            try
            {
                string strResult = "";
                foreach (char c in text)
                {
                    if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9')
                    {
                        strResult = strResult + c;
                    }
                }
                if (!string.IsNullOrWhiteSpace(strResult))
                {
                    result = Convert.ToInt32(strResult);
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "text = " + text);
            }
            return result;
        }


        public static bool IsKayakSuccessfullSearch(string file)
        {
            bool result = false;
            try
            {
                if (File.Exists(file))
                {
                    string Html = File.ReadAllText(file);
                    if(Html.Contains("Flights-Search-FlightInlineSearchForm"))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "file = " + file);
            }
            return result;
        }

        #region html parser
        public static TripsFromHtmlResult GetKayakTripsFromHtml(string html, int SearchTripProviderId, string Url, DateTime OneWayDate, DateTime? ReturnDate)
        {
            TripsFromHtmlResult Result = new TripsFromHtmlResult();
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                Result.Success = true;



                var textNodes = doc.DocumentNode.SelectNodes("//*[contains(@class,'Flights-Results-FlightResultItem')]");
                if (textNodes != null)
                {
                    foreach (HtmlNode node in textNodes)
                    {
                        try
                        {
                            TripItem Trip = new TripItem();
                            Trip.SearchTripProviderId = SearchTripProviderId;

                            #region price
                            var strPrice = node.SelectSingleNode(".//span[contains(@class,'price option-text')]").InnerHtml.Replace("\n", "");
                            Tuple<string, decimal> PriceAndCurrency = GetPriceWithCurrencyFromHtml(strPrice);
                            Trip.CurrencyCode = PriceAndCurrency.Item1;
                            Trip.Price = PriceAndCurrency.Item2;

                            if(Trip.Price<0 || String.IsNullOrWhiteSpace(Trip.CurrencyCode))
                            {
                                Logger.GenerateInfo("Problem with the price ("+ Trip.Price + ") or the currency ("+ (Trip.CurrencyCode ??"N/A") + ") and "+ SearchTripProviderId+" and note html : "+node.InnerHtml);
                            }

                            #endregion
                            //  var flightNodes = node.SelectNodes(".//div[contains(@class,'Flights-Results-LegInfo Flights-Results-LegInfoSleek')]");
                            var flightNodes = node.SelectNodes(".//div[contains(@class,'Flights-Results-FlightLegDetails')]");
                            bool IsReturnFlight = false;
                            foreach (HtmlNode flight in flightNodes)
                            {

                                #region airlines logo and name
                                HtmlNode airlinePicture = flight.SelectSingleNode(".//img[@src!='']");
                                if (airlinePicture != null)
                                {
                                    string airlinePictureSrc = airlinePicture.Attributes.Where(i => i.Name == "src").FirstOrDefault().Value;
                                    string airlineName = airlinePicture.Attributes.Where(i => i.Name == "alt").FirstOrDefault().Value.Replace("logo", "").Trim();
                                    if (IsReturnFlight)
                                    {
                                        Trip.ReturnTrip_AirlineLogoSrc = airlinePictureSrc;
                                        Trip.ReturnTrip_AirlineName = airlineName;
                                    }
                                    else
                                    {
                                        Trip.OneWayTrip_AirlineLogoSrc = airlinePictureSrc;
                                        Trip.OneWayTrip_AirlineName = airlineName;
                                    }
                                }
                                #endregion

                                #region stops 
                                int stopsNumber = 0;
                                if (flight.SelectSingleNode(".//div[contains(@class,'stops')]") != null)
                                {
                                    string stopsHtml = flight.SelectSingleNode(".//div[contains(@class,'stops')]").SelectSingleNode(".//div[contains(@class,'top')]").InnerText;
                                    stopsNumber = GeStopsNumberFromHtml(stopsHtml);
                                }
                                else
                                {
                                    stopsNumber=flight.SelectNodes(".//div[contains(@class,'segment-row')]").Count()-1;
                                }
                                #endregion

                                #region duration 
                                string durationHtml = "";

                                if (flight.SelectSingleNode(".//span[contains(@class,'duration')]") != null)
                                {
                                    durationHtml = flight.SelectSingleNode(".//span[contains(@class,'duration')]").InnerText.Replace(" ", "");
                                }
                                else if (flight.SelectSingleNode(".//span[contains(@class,'segmentDuration')]") != null)
                                {
                                    durationHtml = flight.SelectSingleNode(".//span[contains(@class,'segmentDuration')]").InnerText.Replace(" ", "");
                                }
                                int duration = GetDurationFromHtml(durationHtml);
                                #endregion

                                #region time 
                                string arrival = "";
                                string departure = "";

                                if (flight.SelectSingleNode(".//div[contains(@class,'segmentTimes')]") != null)
                                {
                                    departure = flight.SelectSingleNode(".//div[contains(@class,'segmentTimes')]").SelectNodes(".//span[contains(@class,'time')]")[0].InnerText;
                                    arrival = flight.SelectNodes(".//div[contains(@class,'segmentTimes')]")[stopsNumber].SelectNodes(".//span[contains(@class,'time')]")[1].InnerText;
                                    DateTime BaseDate = OneWayDate;

                                    if (IsReturnFlight)
                                    {
                                        BaseDate = ReturnDate.Value;
                                    }

                                    if (flight.SelectSingleNode(".//div[contains(@class,'arrival-date-warning')]") != null)
                                    {
                                        string warningDate = flight.SelectSingleNode(".//div[contains(@class,'arrival-date-warning')]").InnerHtml;
                                        departure = GeFlightDateFromHtml(GetArrivalFlightDateFromHtml(warningDate,BaseDate), departure);
                                    }
                                    else
                                    {
                                        departure = GeFlightDateFromHtml(BaseDate, departure);
                                    }
                                    
                                    arrival = GeFlightDateFromHtml(BaseDate, arrival);
                                }


                                #endregion


                                #region airport codes
                                string airportCodes = flight.SelectSingleNode(".//span[contains(@class,'origin-destination')]").InnerText;
                                #endregion
                                if (IsReturnFlight)
                                {
                                    Trip.ReturnTrip_FromAirportCode = airportCodes.Split('-')[0].Trim();
                                    Trip.ReturnTrip_ToAirportCode = airportCodes.Split('-')[1].Trim();
                                    Trip.ReturnTrip_Stops = stopsNumber;
                                    Trip.ReturnTrip_Duration = duration;
                                    Trip.ReturnTrip_ArrivalDate = arrival;
                                    Trip.ReturnTrip_DepartureDate = departure;
                                }
                                else
                                {
                                    Trip.OneWayTrip_FromAirportCode = airportCodes.Split('-')[0].Trim();
                                    Trip.OneWayTrip_ToAirportCode = airportCodes.Split('-')[1].Trim();
                                    Trip.OneWayTrip_Stops = stopsNumber;
                                    Trip.OneWayTrip_Duration = duration;
                                    Trip.OneWayTrip_ArrivalDate = arrival;
                                    Trip.OneWayTrip_DepartureDate = departure;
                                }

                                IsReturnFlight = !IsReturnFlight;
                            }
                            Trip.Url = Url;
                            Result.Trips.Add(Trip);
                        }
                        catch (Exception ex2)
                        {
                            Result.Success = false;
                            FlightsEngine.Utils.Logger.GenerateError(ex2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "GetKayakTripsFromHtml Loop, SearchTripProviderId = " + SearchTripProviderId + " and node = " + node.InnerHtml);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Result.Success = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripProviderId = " + SearchTripProviderId);
            }
            return Result;
        }

        #endregion


        #region url
        public static string GetEdreamsUrl(AirlineSearch filter)
        {
            string url = null;
            try
            {
                url = "https://www.edreams.com/#results/type=R;dep=" + filter.FromDate.Value.ToString("yyyy-MM-dd") + ";from=" + filter.FromAirportCode + ";to=" + filter.ToAirportCode + ";ret=" + filter.ToDate.Value.ToString("yyyy-MM-dd") + ";collectionmethod=false;airlinescodes=false;internalSearch=true";
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Filters = " + filter.ToSpecialString());
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
                startInfoPreparation.Arguments = string.Format("/C {0} {1}", scrappingSearch.ScrappingPreparationScript, args);
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
                    while (!File.Exists(HtmlFile) && !File.Exists(HtmlErrorFile) && !File.Exists(StopSearchFile) && i <= 75) //limit the time to whait to 30 sec
                    {
                        Thread.Sleep(500);
                        i++;
                    }
                    if (File.Exists(HtmlFile))
                    {

                        if (scrappingSearch.Provider == Providers.ToString(Providers.Kayak))
                        {
                            success = IsKayakSuccessfullSearch(HtmlFile);
                        }
                        else
                        {
                            success = true;
                        }
                        if(!success)
                        {
                            File.Delete(HtmlFile);
                        }
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
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : SUCCESS  ");
            else
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Scrapping *** : FAILURE => " + (result.Error ?? ""));
            return result;
        }

    }
}
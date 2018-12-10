using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FlightsEngine.Models;
using System.Web.Script.Serialization;
using System.Net;
using Models.Models.Shared;
using Data.Model;
using FlightsServices;
using FlightsEngine.Models.Constants;
using FlightsEngine.Models.RyanAir;
using Newtonsoft.Json;
using FlightsEngine.Models.BritishAirways;

namespace FlightsEngine.FlighsAPI
{
    public static class BritishAirways
    {
        public static string Key = "59szstcpxtrezug5hc9xbpy9";

        // https://developer.iairgroup.com/british_airways/apis/BA_Destinations

        #region getRoutes
        public static bool GetRoutes()
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START British Airways GetRoutes ***");
                GetAirportsRoutes();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END British Airways GetRoutes ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

        public static List<AirportItem> getBADestinations()
        {
            List<AirportItem> Airports = new List<AirportItem>();
            try
            {
                var url = "https://api.ba.com/rest-v1/v1/balocations";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("client-key", Key);
                bool result = false;
                string CityCode = "";

                HttpWebResponse httpResponse = null;
                try
                {
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    result = true;
                }
                catch (WebException e)
                {
                    result = false;
                    string webError = FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }

                if (result)
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var requestResult = streamReader.ReadToEnd();

                        if (!String.IsNullOrWhiteSpace(requestResult))
                        {
                            BARouteResponse ResultItem = JsonConvert.DeserializeObject<BARouteResponse>(requestResult);
                            if (ResultItem != null)
                            {
                                foreach (var Country in ResultItem.GetBA_LocationsResponse.Country)
                                {
                                    try
                                    {
                                        if (Country.City != null)
                                        {
                                            JavaScriptSerializer srRequestResult = new JavaScriptSerializer();
                                            dynamic jsonCities = srRequestResult.DeserializeObject(Country.City.ToString());
                                            if (jsonCities != null)
                                            {
                                                if (FlightsEngine.Utils.Utils.IsObjectsArray(jsonCities))
                                                {
                                                    foreach (var jsonCity in jsonCities)
                                                    {
                                                        CityCode = jsonCity["CityCode"];
                                                        if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonCity, "Airport"))
                                                        {
                                                            dynamic jsonAirports = jsonCity["Airport"];
                                                            if (FlightsEngine.Utils.Utils.IsObjectsArray(jsonAirports))
                                                            {
                                                                foreach (var jsonAirport in jsonAirports)
                                                                {
                                                                    if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonAirport, "AirportCode"))
                                                                    {
                                                                        AirportItem Airport = new AirportItem();
                                                                        Airport.Code = Convert.ToString(jsonAirport["AirportCode"]);
                                                                        Airport.CityCode = CityCode;
                                                                        Airports.Add(Airport);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonAirports, "AirportCode"))
                                                                {
                                                                    AirportItem Airport = new AirportItem();
                                                                    Airport.Code = Convert.ToString(jsonAirports["AirportCode"]);
                                                                    Airport.CityCode = CityCode;
                                                                    Airports.Add(Airport);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonCities, "Airport"))
                                                {
                                                    CityCode = jsonCities["CityCode"];
                                                    dynamic jsonAirports = jsonCities["Airport"];
                                                    if (FlightsEngine.Utils.Utils.IsObjectsArray(jsonAirports))
                                                    {
                                                        foreach (var jsonAirport in jsonAirports)
                                                        {
                                                            if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonAirport, "AirportCode"))
                                                            {
                                                                AirportItem Airport = new AirportItem();
                                                                Airport.Code = Convert.ToString(jsonAirport["AirportCode"]);
                                                                Airport.CityCode = CityCode;
                                                                Airports.Add(Airport);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (FlightsEngine.Utils.Utils.IsPropertyExist(jsonAirports, "AirportCode"))
                                                        {
                                                            AirportItem Airport = new AirportItem();
                                                            Airport.Code = Convert.ToString(jsonAirports["AirportCode"]);
                                                            Airport.CityCode = CityCode;
                                                            Airports.Add(Airport);
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FlightsEngine.Utils.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Country = " + JsonConvert.SerializeObject(Country));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return Airports;
        }


        static bool GetAirportsRoutes()
        {
            bool resultRoutes = true; ;
            List<AirportItem> Airports = new List<AirportItem>();
            try
            {
                var context = new TemplateEntities1();
                AirportService _airportService = new AirportService(context);
                //   _airportService.DeleteAirportsTripProvider(Providers.BritishAirways);
                List<AirportItem> AirportsList = getBADestinations();
                AirportsList = AirportsList.OrderBy(a => a.Code).ToList();
                AirportsTrip lastAirportsTrip = _airportService.GetAirportsTripForProviderRoute(Providers.BritishAirways);

                bool result = false;
                int AttemptsNumber = 0;
                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;
                bool NeedToContinueSearch = true;



                foreach (var fromAirport in AirportsList)
                {
                    foreach (var toAirport in AirportsList)
                    {
                        if (lastAirportsTrip != null)
                        {
                            int test = string.Compare(lastAirportsTrip.Airport.Code, fromAirport.Code);
                            int tets2 = string.Compare(lastAirportsTrip.Airport1.Code, toAirport.Code);
                        }
                        if (!String.IsNullOrWhiteSpace(fromAirport.Code) && !String.IsNullOrWhiteSpace(toAirport.Code) && (lastAirportsTrip == null || (string.Compare(lastAirportsTrip.Airport.Code, fromAirport.Code) <= 0 && string.Compare(lastAirportsTrip.Airport1.Code, toAirport.Code) <= 0)))
                        {
                            result = false;
                            AttemptsNumber = 0;
                            NeedToContinueSearch = true;
                            while (!result && AttemptsNumber <= MaxRequestAttempts && NeedToContinueSearch)
                            {
                                AttemptsNumber++;
                                var client = new HttpClient();
                                var queryString = HttpUtility.ParseQueryString(string.Empty);


                                var url = "https://api.ba.com/rest-v1/v1/flightOfferBasic;departureCity=" + fromAirport.CityCode + ";arrivalCity=" + toAirport.CityCode + ";cabin=economy;journeyType=roundTrip;range=yearLow";
                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                // httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "GET";
                                httpWebRequest.Headers.Add("client-key", Key);

                                HttpWebResponse httpResponse = null;
                                try
                                {
                                    AttemptsNumber++;
                                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                    result = true;
                                }
                                catch (WebException e)
                                {
                                    result = false;
                                    if (!e.ToString().Contains("404"))
                                    {
                                        string webError = FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FromAirportId = " + fromAirport.Id + " and toAirportId = " + toAirport.Id);
                                    }
                                    else
                                    {
                                        NeedToContinueSearch = false;
                                    }
                                }


                                if (result)
                                {
                                    NeedToContinueSearch = false;
                                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                    {
                                        var requestResult = streamReader.ReadToEnd();

                                        if (!String.IsNullOrWhiteSpace(requestResult))
                                        {
                                            _airportService.AddAirportsTripProviderItem(fromAirport.Code, toAirport.Code, Providers.BritishAirways);
                                        }
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                                /*
                                            if (response != null)
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        var contents = await response.Content.ReadAsStringAsync();
                                        if (!String.IsNullOrWhiteSpace(contents))
                                        {
                                            List<RyanAirAirports> AirportsList = JsonConvert.DeserializeObject<List<RyanAirAirports>>(contents);
                                            if (AirportsList != null)
                                            {
                                                foreach (RyanAirAirports ResultItem in AirportsList)
                                                {
                                                    string FromAirportCode = ResultItem.iataCode.ToUpper();
                                                    foreach (string route in ResultItem.routes)
                                                    {
                                                        if (route != null && route.ToLower().Contains("airport"))
                                                        {
                                                            _airportService.AddAirportsTripProviderItem(FromAirportCode, route.Split(':')[1].ToUpper(), Providers.RyanAir);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    FlightsEngine.Utils.Logger.GenerateInfo("British Airways Response null ");
                                }

                            */
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                resultRoutes = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return resultRoutes;
        }


        #endregion

        public static bool SearchFlights(APIAirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START British Airways  ***");
                MakeRequest(filter);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END British Airways  ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }


        static bool MakeRequest(APIAirlineSearch filters)
        {
            bool result = false;
            try
            {
                int AttemptsNumber = 0;
                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;

                var context = new TemplateEntities1();
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                TripsService _tripService = new TripsService(context);
                int MaxDaysNumberForSearch = 30;

                DateTime originalFromDateMin = filters.FromDateMin;
                DateTime originalToDateMax = (filters.ToDateMax ?? filters.FromDateMax);

                DateTime FromDateMin = originalFromDateMin;
                DateTime ToDateMax = originalToDateMax;

                if ((originalToDateMax - originalFromDateMin).TotalDays > MaxDaysNumberForSearch)
                {
                    ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                }

                int SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.BritishAirways);

                List<TripItem> Trips = new List<TripItem>();

                while (ToDateMax < originalToDateMax)
                {
                    AttemptsNumber = 1;
                    result = false;
                    while (!result && AttemptsNumber <= MaxRequestAttempts)
                    {
                        AttemptsNumber++;


                    }
                }
                _tripService.InsertTrips(Trips);

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
            return result;
        }


    }
}

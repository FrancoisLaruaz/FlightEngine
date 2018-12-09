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

namespace FlightsEngine.FlighsAPI
{
    public static class RyanAir
    {
        public static string Key = "TaiRJ5T7SFngdrJkiGob9InuEFE4xeJK";
        public static string SecretKey = "7mOsWXal1132p1av";


        #region getRoutes
        public static bool GetRoutes()
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Ryan Air GetRoutes ***");
                GetRoutesAync().Wait();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Ryan Air GetRoutes ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        static async Task GetRoutesAync()
        {
            List<AirportItem> Airports = new List<AirportItem>();
            try
            {
                var context = new TemplateEntities1();
                AirportService _airportService = new AirportService(context);
                _airportService.DeleteAirportsTripProvider(Providers.RyanAir);

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);


                var uri = "https://apigateway.ryanair.com/pub/v1/core/3/airports?apikey=" + Key;
                var response = await client.GetAsync(uri);


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
                    FlightsEngine.Utils.Logger.GenerateInfo("RyanAir Response null ");
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }


        #endregion

        public static bool SearchFlights(APIAirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START Ryan Air  ***");
                MakeRequest(filter);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END Ryan Air  ***");
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
                int MaxDaysNumberForSearch =30;

                DateTime originalFromDateMin = filters.FromDateMin;
                DateTime originalToDateMax = (filters.ToDateMax ?? filters.FromDateMax);

                DateTime FromDateMin = originalFromDateMin;
                DateTime ToDateMax = originalToDateMax;

                if ((originalToDateMax - originalFromDateMin).TotalDays > MaxDaysNumberForSearch)
                {
                    ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                }

                int SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.RyanAir);

                List<TripItem> Trips = new List<TripItem>();

                while (ToDateMax < originalToDateMax)
                {
                    AttemptsNumber = 1;
                    result = false;
                    while (!result && AttemptsNumber <= MaxRequestAttempts)
                    {
                        AttemptsNumber++;
                        string url = "https://apigateway.ryanair.com/pub/v1/farefinder/3/roundTripFares?apikey=" + Key + "&departureAirportIataCode=" + filters.FromAirportCode + "&outboundDepartureDateFrom=" + FromDateMin.ToString("yyyy-MM-dd") + "&outboundDepartureDateTo=" + ToDateMax.ToString("yyyy-MM-dd") + "&inboundDepartureDateFrom=" + FromDateMin.ToString("yyyy-MM-dd") + "&inboundDepartureDateTo=" + ToDateMax.ToString("yyyy-MM-dd") + "&arrivalAirportIataCode=" + filters.ToAirportCode + "&durationFrom=" + filters.DurationMin.Value + "&durationTo=" + filters.DurationMax.Value;

                        WebClient client = new WebClient();
                        string response = "";
                        try
                        {
                            response = client.DownloadString(url);
                            result = true;
                        }
                        catch (WebException e)
                        {
                            result = false;
                            string webError = FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "url= " + url+" and "+filters.ToSpecialString());
                            System.Threading.Thread.Sleep(5000);
                        }

                        if (result)
                        {

                            if (!String.IsNullOrWhiteSpace(response))
                            {
                                RyanAirTripsResponse Item = JsonConvert.DeserializeObject<RyanAirTripsResponse>(response);
                                if (Item != null)
                                {
                                    foreach (var trip in Item.fares)
                                    {
                                        TripItem Trip = new TripItem();
                                        Trip.OneWayTrip_AirlineName = "Ryanair";
                                        Trip.ReturnTrip_AirlineName = "Ryanair";
                                        Trip.OneWayTrip_Stops = 0;
                                        Trip.ReturnTrip_Stops = 0;
                                        Trip.Url = "https://www.ryanair.com/gb/en/booking/home/" + trip.outbound.departureAirport.iataCode + "/" + trip.outbound.arrivalAirport.iataCode + "/"+trip.outbound.departureDate.ToString("yyyy-MM-dd")+"/"+trip.inbound.departureDate.ToString("yyyy-MM-dd") + "/"+filters.AdultsNumber+"/"+filters.ChildrenNumber+"/"+filters.BabiesNumber+"/"+filters.ChildrenNumber;
                                        Trip.ProviderId = Providers.RyanAir;
                                        Trip.Price = Convert.ToDecimal(trip.summary.price.value);
                                        Trip.CurrencyCode = trip.summary.price.currencyCode;
                                        Trip.SearchTripProviderId = SearchTripProviderId;
                                        Trip.OneWayTrip_FromAirportCode = trip.outbound.departureAirport.iataCode;
                                        Trip.OneWayTrip_ToAirportCode = trip.outbound.arrivalAirport.iataCode;
                                        Trip.ReturnTrip_FromAirportCode = trip.inbound.departureAirport.iataCode;
                                        Trip.ReturnTrip_ToAirportCode = trip.inbound.arrivalAirport.iataCode;
                                        Trip.OneWayTrip_DepartureDate = trip.outbound.departureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                        Trip.OneWayTrip_ArrivalDate = trip.outbound.arrivalDate.ToString(DateFormat.Trip).Replace("-", "/");
                                        Trip.ReturnTrip_DepartureDate = trip.inbound.departureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                        Trip.ReturnTrip_ArrivalDate = trip.inbound.arrivalDate.ToString(DateFormat.Trip).Replace("-", "/");
                                        Trip.OneWayTrip_Duration = _tripService.GetTripDuration(trip.outbound.departureDate, trip.outbound.arrivalDate, trip.outbound.departureAirport.iataCode, trip.outbound.arrivalAirport.iataCode);
                                        Trip.ReturnTrip_Duration = _tripService.GetTripDuration(trip.inbound.departureDate, trip.inbound.arrivalDate, trip.inbound.departureAirport.iataCode, trip.inbound.arrivalAirport.iataCode);
                                        Trips.Add(Trip);
                                    }
                                }
                            }

                            FromDateMin = FromDateMin.AddDays(MaxDaysNumberForSearch - (filters.DurationMax + 1 ?? 0));

                            if (ToDateMax == originalToDateMax)
                            {
                                ToDateMax = ToDateMax.AddDays(1);
                            }
                            else if (ToDateMax.AddDays(MaxDaysNumberForSearch) > originalToDateMax)
                            {
                                ToDateMax = originalToDateMax;
                            }
                            else
                            {
                                ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                            }
                            System.Threading.Thread.Sleep(700);
                        }
                      
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

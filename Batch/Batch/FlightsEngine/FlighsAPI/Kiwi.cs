using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using FlightsEngine.Models;
using Transavia.Api.FlightOffers.Client;
using Transavia.Api.FlightOffers.Client.Model;
using System.Web.Script.Serialization;
using FlightsServices;
using FlightsEngine.Models.Constants;
using FlightsEngine.Utils;
using Data.Model;

namespace FlightsEngine.FlighsAPI
{
    public static class Kiwi
    {
        public static string Key = "picky";


        public static bool SearchFlights(KiwiAirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START KIWI ***");
                MakeRequest(filter).Wait();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END KIWI ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }




        static async Task MakeRequest(KiwiAirlineSearch filters)
        {
            List<TripItem> Trips = new List<TripItem>();
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                var context = new TemplateEntities1();
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);

                // Request parameters
                if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                    queryString["flyFrom"] = filters.FromAirportCode;
                if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                    queryString["to"] = filters.ToAirportCode;

                queryString["dateFrom"] = filters.FromDateMin.ToString("dd'/'MM'/'yyyy");
                queryString["dateTo"] = filters.FromDateMax.ToString("dd'/'MM'/'yyyy");

                if (filters.ToDateMin != null)
                {
                    queryString["returnFrom"] = filters.ToDateMin.Value.ToString("dd'/'MM'/'yyyy");
                    queryString["returnTo"] = filters.ToDateMax.Value.ToString("dd'/'MM'/'yyyy");
                    queryString["typeFlight"] = "round";
                    if(filters.DurationMin != null)
                        queryString["daysInDestinationFrom"] = filters.DurationMin.ToString();
                    if (filters.DurationMax != null)
                        queryString["daysInDestinationTo"] = filters.DurationMax.ToString();
                }
                else
                {
                    queryString["typeFlight"] = "oneway";
                }


                queryString["partner"] = Key;
                queryString["maxstopovers"] = filters.MaxStopsNumber.ToString();
                queryString["curr"] = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
                queryString["adults"] = filters.AdultsNumber.ToString();
                queryString["children"] = filters.ChildrenNumber.ToString();
                queryString["limit"] = "200";
                queryString["sort"] = "price";
                /*
                queryString["DestinationDepartureDate"] = "{string}";
                queryString["DestinationArrivalDate"] = "{string}";
                queryString["OriginDepartureTime"] = "{string}";
                queryString["OriginArrivalTime"] = "{string}";
                queryString["DestinationDepartureTime"] = "{string}";
                queryString["DestinationArrivalTime"] = "{string}";
                queryString["OriginDepartureDayOfWeek"] = "{string}";
                queryString["OriginArrivalDayOfWeek"] = "{string}";
                queryString["DestinationDepartureDayOfWeek"] = "{string}";
                queryString["DestinationArrivalDayOfWeek"] = "{string}";
                queryString["DaysAtDestination"] = "{string}";

                queryString["Price"] = "{string}";
                queryString["GroupPricing"] = "{string}";
                queryString["ProductClass"] = "{string}";
                queryString["Offset"] = "{string}";
                queryString["Include"] = "{string}";
                queryString["OrderBy"] = "{string}";
                queryString["LowestPricePerDestination"] = "{string}";
                queryString["Loyalty"] = "{string}";
                queryString["Limit"] = "{string}";
                */
                var uri = "https://api.skypicker.com/flights?" + queryString;
                var response = await client.GetAsync(uri);


                if (response != null)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        JavaScriptSerializer srRequestResult = new JavaScriptSerializer();
                        dynamic jsondataRequestResult = srRequestResult.DeserializeObject(contents);
                        if (jsondataRequestResult != null && FlightsEngine.Utils.Utils.IsPropertyExist(jsondataRequestResult, "data"))
                        {
                            dynamic flightsJson = jsondataRequestResult["data"];
                            foreach (var flightJson in flightsJson)
                            {
                                try
                                {
                                    TripItem Trip = new TripItem();

                                    string lastCity = null;
                                    string lastAirportCode = null;
                                    string LastDepartureDate = null;
                                    string LastArrivalDate = null;
                                    string FlyFrom = Convert.ToString(flightJson["flyFrom"]);
                                    string FlyTo = Convert.ToString(flightJson["flyTo"]);

                                    DateTime? ReturnTrip_DepartureDate = null;
                                    DateTime? OneWayTrip_DepartureDate = null;

                                    dynamic[] routes = flightJson["route"];
                                    bool returnTrip = false;

                                    foreach (var route in routes)
                                    {

                                        string airlineCode = Convert.ToString(route["airline"]);

                                        if (lastCity == null)
                                        {
                                            Trip.OneWayTrip_FromAirportCode = Convert.ToString(route["flyFrom"]);
                                            OneWayTrip_DepartureDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(Convert.ToString(route["dTime"]));
                                            Trip.OneWayTrip_DepartureDate= OneWayTrip_DepartureDate.Value.ToString(DateFormat.Trip).Replace("-", "/");
                                        }
                                        else if (FlyTo == Convert.ToString(route["flyFrom"]))
                                        {
                                            returnTrip = true;
                                            Trip.OneWayTrip_ToAirportCode = lastAirportCode;
                                            Trip.OneWayTrip_ArrivalDate = LastArrivalDate;
                                            ReturnTrip_DepartureDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(Convert.ToString(route["dTime"]));
                                            Trip.ReturnTrip_DepartureDate = ReturnTrip_DepartureDate.Value.ToString(DateFormat.Trip).Replace("-", "/");
                                            Trip.ReturnTrip_FromAirportCode = Convert.ToString(route["flyFrom"]);
                                        }
                                        string flightNumber = Convert.ToString(route["flight_no"]);
                                        if (returnTrip)
                                        {
                                            if (Trip.ReturnTrip_Stops == null)
                                                Trip.ReturnTrip_Stops = 0;
                                            Trip.ReturnTrip_AirlineName = airlineCode;
                                            Trip.ReturnTrip_Stops = Trip.ReturnTrip_Stops + 1;
                                            Trip.ReturnTrip_FlightNumber = Trip.ReturnTrip_FlightNumber == null ? flightNumber : Trip.ReturnTrip_FlightNumber + " - " + flightNumber;
                                            if (Trip.ReturnTrip_Stops > 1)
                                            {
                                                Trip.ReturnTrip_StopInformation = Trip.ReturnTrip_StopInformation == null ? lastCity + " (" + lastAirportCode + ")" : Trip.ReturnTrip_StopInformation + " - " + lastCity + " (" + lastAirportCode + ")";
                                            }
                                        }
                                        else
                                        {
                                            Trip.OneWayTrip_Stops = Trip.OneWayTrip_Stops + 1;
                                            Trip.OneWayTrip_AirlineName = airlineCode;
                                            Trip.OneWayTrip_FlightNumber = Trip.OneWayTrip_FlightNumber == null ? flightNumber : Trip.OneWayTrip_FlightNumber + " - " + flightNumber;
                                            if (Trip.OneWayTrip_Stops > 1)
                                            {
                                                Trip.OneWayTrip_StopInformation = Trip.OneWayTrip_StopInformation == null ? lastCity + " (" + lastAirportCode + ")" : Trip.OneWayTrip_StopInformation + " - " + lastCity + " (" + lastAirportCode + ")";
                                            }
                                        }

                                        lastCity = Convert.ToString(route["cityTo"]);
                                        lastAirportCode = Convert.ToString(route["flyTo"]);
                                        Trip.ReturnTrip_ToAirportCode = lastAirportCode;
                                        LastDepartureDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(Convert.ToString(route["dTime"])).ToString(DateFormat.Trip).Replace("-", "/");
                                        LastArrivalDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(Convert.ToString(route["aTime"])).ToString(DateFormat.Trip).Replace("-", "/");
                                        Trip.ReturnTrip_ArrivalDate = LastArrivalDate;
                                    }
                                    Trip.OneWayTrip_Stops = Trip.OneWayTrip_Stops - 1;
                                    Trip.ReturnTrip_Stops = Trip.ReturnTrip_Stops - 1;


                                    Trip.SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(OneWayTrip_DepartureDate.Value, ReturnTrip_DepartureDate, filters.SearchTripWishesId,Providers.Kiwi);
                                    Trip.CurrencyCode = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
                                    Trip.Price = Convert.ToDecimal(flightJson["price"]);
                                    Trip.Url = Convert.ToString(flightJson["deep_link"]);
                                    Trip.OneWayTrip_Duration = ScrappingHelper.GetDurationFromHtml(Convert.ToString(flightJson["fly_duration"]));
                                    if (FlightsEngine.Utils.Utils.IsPropertyExist(flightJson, "return_duration"))
                                    {
                                        Trip.ReturnTrip_Duration = ScrappingHelper.GetDurationFromHtml(Convert.ToString(flightJson["return_duration"]));
                                    }
                                    Trip.Url = Convert.ToString(flightJson["deep_link"]);


                                    Trips.Add(Trip);
                                }
                                catch (Exception ex2)
                                {
                                    FlightsEngine.Utils.Logger.GenerateError(ex2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
                                }
                            }

                            TripsService _tripService = new TripsService(context);
                            _tripService.InsertTrips(Trips);
                        }
                    }
                    else
                    {
                        FlightsEngine.Utils.Logger.GenerateInfo("Kiwi Response unsucessfull :  " + response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode + " and " + response.ToString() + " and " + filters.ToSpecialString());
                    }
                }
                else
                {
                    FlightsEngine.Utils.Logger.GenerateInfo("Kiwi Response null :  " + filters.ToSpecialString());
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
        }
    }
}

﻿using System;
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

namespace FlightsEngine.FlighsAPI
{
    public static class Kiwi
    {
        public static string Key = "picky";


        public static bool SearchFlights(AirlineSearch filter)
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




        static async Task MakeRequest(AirlineSearch filters)
        {
            List<TripItem> Trips = new List<TripItem>();
            try
            {


                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);


                // exemple : https://api.skypicker.com/flights?flyFrom=CZ&to=porto&dateFrom=30/08/2018&dateTo=08/09/2018&partner=picky

                // Request parameters
                if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                    queryString["flyFrom"] = filters.FromAirportCode;
                if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                    queryString["to"] = filters.ToAirportCode;
                if (filters.FromDate != null)
                {
                    queryString["dateFrom"] = filters.FromDate.Value.AddDays(-30).ToString("dd'/'MM'/'yyyy");
                    queryString["dateTo"] = filters.FromDate.Value.AddDays(300).ToString("dd'/'MM'/'yyyy");
                }
                if (filters.ToDate != null)
                {
                    queryString["returnFrom"] = filters.FromDate.Value.AddDays(-30).ToString("dd'/'MM'/'yyyy");
                    queryString["returnTo"] = filters.FromDate.Value.AddDays(300).ToString("dd'/'MM'/'yyyy");
                    queryString["typeFlight"] = "round";
                    queryString["daysInDestinationFrom"] = "5";
                    queryString["daysInDestinationTo"] = "9";
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

                                    /*
                                     * alter table dbo.Flight
                                        add StopInformation varchar(1000) null
                                     * 
                                     * 1554425400
                                     */

                                    List<List<Object>> routes = flightJson["route"];
                                    foreach(var route in routes)
                                    {
                                        Trip.OneWayTrip_Stops = Trip.OneWayTrip_Stops + 1;
                                    }
                                    Trip.OneWayTrip_Stops = Trip.OneWayTrip_Stops - 1;
                                    if (filters.ToDate != null)
                                    {
                                        Trip.ReturnTrip_Stops = Trip.ReturnTrip_Stops - 1;
                                    }

                                    Trip.OneWayTrip_FromAirportCode = Convert.ToString(routes[0][0]);
                                    Trip.OneWayTrip_ToAirportCode = filters.ToAirportCode;

                                    if (filters.ToDate != null)
                                    {
                                        Trip.ReturnTrip_ToAirportCode = filters.FromAirportCode;
                                        Trip.ReturnTrip_FromAirportCode = filters.ToAirportCode;
                                    }
                                    Trip.CurrencyCode = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
                                    Trip.Price = Convert.ToDecimal(flightJson["price"]);
                                    string aTime = Convert.ToString(flightJson["aTimeUTC"]);
                                    Trip.Url = Convert.ToString(flightJson["deep_link"]);

                                    List<Object> transfers = flightJson["transfers"];
                                    Trip.OneWayTrip_Stops = transfers.Count;

                                    Trip.OneWayTrip_ArrivalDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(aTime).Value.ToString(DateFormat.Trip).Replace("-", "/");
                                    Trip.OneWayTrip_Duration = ScrappingHelper.GetDurationFromHtml(Convert.ToString(flightJson["fly_duration"]));
                                    Trip.ReturnTrip_Duration = ScrappingHelper.GetDurationFromHtml(Convert.ToString(flightJson["return_duration"]));
                                    string dTime = Convert.ToString(flightJson["dTimeUTC"]);
                                    Trip.OneWayTrip_DepartureDate = FlightsEngine.Utils.Utils.GetDateFromunixTimeStamp(dTime).Value.ToString(DateFormat.Trip).Replace("-", "/");
                                    Trips.Add(Trip);
                                }
                                catch (Exception ex2)
                                {
                                    FlightsEngine.Utils.Logger.GenerateError(ex2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
                                }
                            }

                            TripsService _tripService = new TripsService();
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

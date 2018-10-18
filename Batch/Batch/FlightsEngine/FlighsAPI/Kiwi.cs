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
                    queryString["dateFrom"] = filters.FromDate.Value.ToString("dd'/'MM'/'yyyy");
                if (filters.ToDate != null)
                    queryString["dateTo"] = filters.ToDate.Value.ToString("dd'/'MM'/'yyyy");
                queryString["partner"] = Key;
                queryString["curr"] = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
                queryString["adults"] = filters.AdultsNumber.ToString();
                queryString["children"] = filters.ChildrenNumber.ToString();
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
                            foreach(var flightJson in flightsJson)
                            {
                                try
                                {
                                    TripItem Trip = new TripItem();
                                    Trips.Add(Trip);
                                }
                                catch(Exception ex2)
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

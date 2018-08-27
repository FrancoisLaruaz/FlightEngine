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

namespace FlightsEngine.FlighsAPI
{
    public static class Transavia
    {
        public static string Key = "53965cdf041d4d9dbcb3326ac6b46461";


        public static bool SearchFlights(AirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START TRANSAVIA ***");
                MakeRequest(filter).Wait();

                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END TRANSAVIA ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }




        static async Task MakeRequest(AirlineSearch filters)
        {
            try
            {

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Request headers
                client.DefaultRequestHeaders.Add("apikey", Key);

                // Request parameters
                if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                    queryString["Origin"] = filters.FromAirportCode;
                if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                    queryString["Destination"] = filters.ToAirportCode;
                if (filters.FromDate != null)
                    queryString["OriginDepartureDate"] = filters.FromDate.Value.ToString("yyyyMMdd");
                if (filters.ToDate != null)
                    queryString["DestinationDepartureDate"] = filters.ToDate.Value.ToString("yyyyMMdd");
                if(filters.MaxStopsNumber==0)
                    queryString["DirectFlight"] = "true";

                queryString["Adults"] = filters.AdultsNumber.ToString();
                queryString["Children"] = filters.ChildrenNumber.ToString();
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
                var uri = "https://api.transavia.com/v1/flightoffers/?" + queryString;
                var response = await client.GetAsync(uri);


                if (response != null)
                {
                    if (response.IsSuccessStatusCode && response.StatusCode.ToString() != "No Content")
                    {
                        var contents = await response.Content.ReadAsStringAsync();
                        JavaScriptSerializer srRequestResult = new JavaScriptSerializer();
                        dynamic jsondataRequestResult = srRequestResult.DeserializeObject(contents);
                        Console.WriteLine("response: " + response.ToString());
                    }
                    else
                    {
                        FlightsEngine.Utils.Logger.GenerateInfo("Transavia Response unsucessfull :  :" + response.ReasonPhrase + " " + response.RequestMessage + " " + response.StatusCode + " and " + response.ToString() +" and "+ filters.ToSpecialString());
                    }
                }
                else
                {
                    FlightsEngine.Utils.Logger.GenerateInfo("Transavia Response null : " + filters.ToSpecialString());
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
        }
    }
}

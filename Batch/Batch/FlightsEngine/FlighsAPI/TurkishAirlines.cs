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
using System.Net;
using FlightsEngine.Utils;
using FlightsEngine.Models.Constants;
using Newtonsoft.Json;
using Data.Model;
using FlightsServices;
using Models.Models.Shared;
using FlightsEngine.Models.TurkishAirlines;

namespace FlightsEngine.FlighsAPI
{
    public static class TurkishAirlines
    {

        public static string Key = "l7xx2c60dd0ba39f47b9aefba0508ab5ee4d";
        public static string Secret = "9451ede937ed4d5982cb4ccfdba76dec";

        // https://developer.turkishairlines.com/documentation

        public static bool SearchFlights(APIAirlineSearch filter)
        {
            bool result = true;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START TurkishAirlines ***");
                result = MakeRequest(filter);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END TurkishAirlines ***");
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }




        static bool MakeRequest(APIAirlineSearch filters)
        {
            bool result = false;
            try
            {
                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;
                int AttemptsNumber = 1;



                DateTime originalFromDateMin = filters.FromDateMin;
                DateTime originalToDateMax = (filters.ToDateMax ?? filters.FromDateMax);

                DateTime FromDateMin = originalFromDateMin;
                DateTime ToDateMax = originalToDateMax;

                var context = new TemplateEntities1();
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                int SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.TurkishAirlines);



                List<TripItem> Trips = new List<TripItem>();

                AttemptsNumber = 1;
                result = false;
                while (!result && AttemptsNumber <= MaxRequestAttempts)
                {
                    try
                    {
                        AttemptsNumber = AttemptsNumber + 1;
                        string url = "https://api.turkishairlines.com/test/getAvailability";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Headers.Add("Accept-Language", "en-US");
                        httpWebRequest.Headers.Add("apisecret", Secret);
                        httpWebRequest.Headers.Add("apikey", Key);

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            RequestBody body = new RequestBody();
                            body.ReducedDataIndicator = false;
                            if (filters.Return)
                            {
                                body.RoutingType = "r";
                            }
                            else
                            {
                                body.RoutingType = "o";
                            }
                            body.PassengerTypeQuantity.Add(new PassengerTypeQuantity("adult", filters.AdultsNumber));
                            body.PassengerTypeQuantity.Add(new PassengerTypeQuantity("child", filters.ChildrenNumber));
                            body.PassengerTypeQuantity.Add(new PassengerTypeQuantity("infant", filters.BabiesNumber));

                            OriginDestinationInformation OneWayItem = new OriginDestinationInformation();
                            OneWayItem.CabinPreferences.Add(new CabinPreference("ECONOMY"));
                            OneWayItem.CabinPreferences.Add(new CabinPreference("BUSINESS"));

                            OneWayItem.DepartureDateTime.Date = "10DEC";
                            OneWayItem.DepartureDateTime.WindowAfter = "P3D";
                            OneWayItem.DepartureDateTime.WindowBefore = "P3D";

                            OneWayItem.OriginLocation.LocationCode = "IST";
                            OneWayItem.OriginLocation.MultiAirportCityInd = false;

                            OneWayItem.DestinationLocation.LocationCode = "ESB";
                            OneWayItem.OriginLocation.MultiAirportCityInd = false;

                            body.OriginDestinationInformation.Add(OneWayItem);

                            if (filters.Return)
                            {

                                OriginDestinationInformation ReturnItem = new OriginDestinationInformation();
                                ReturnItem.CabinPreferences.Add(new CabinPreference("ECONOMY"));
                                ReturnItem.CabinPreferences.Add(new CabinPreference("BUSINESS"));

                                ReturnItem.DepartureDateTime.Date = "27DEC";
                                ReturnItem.DepartureDateTime.WindowAfter = "P3D";
                                ReturnItem.DepartureDateTime.WindowBefore = "P3D";

                                ReturnItem.OriginLocation.LocationCode = "ESB";
                                ReturnItem.OriginLocation.MultiAirportCityInd = false;

                                ReturnItem.DestinationLocation.LocationCode = "IST";
                                ReturnItem.OriginLocation.MultiAirportCityInd = false;

                                body.OriginDestinationInformation.Add(ReturnItem);
                            }



                            string json = new JavaScriptSerializer().Serialize(body);
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }

                        HttpWebResponse httpResponse = null;
                        try
                        {
                            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            result = true;
                        }
                        catch (WebException e)
                        {
                            result = false;
                            string webError = FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
                        }

                        if (result)
                        {

                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var requestResult = streamReader.ReadToEnd();

                                if (!String.IsNullOrWhiteSpace(requestResult))
                                {



                                }
                            }


                        }

                        System.Threading.Thread.Sleep(200);

                    }
                    catch (Exception ex)
                    {
                        result = false;
                        FlightsEngine.Utils.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "MakeRequest LOOP ERROR :" + filters.ToSpecialString());
                    }


                }
                TripsService _tripService = new TripsService(context);
                _tripService.InsertTrips(Trips);
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "MakeRequest ERROR :" + filters.ToSpecialString());
            }
            return result;
        }


    }
}

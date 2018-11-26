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
using FlightsEngine.Models.AirFranceKLM;
using FlightsEngine.Utils;
using FlightsEngine.Models.Constants;
using Newtonsoft.Json;
using Data.Model;
using FlightsServices;

namespace FlightsEngine.FlighsAPI
{
    public static class AirFranceKLM
    {
        public static string Key = "jqgd23tz7qk7u7vu6ayes2w3";

        // Limits : 5/ second, 5000/day
        // https://developer.airfranceklm.com/docs/read/opendata/flightoffer/POST_lowestfareoffers_v3

        public static bool SearchFlights(APIAirlineSearch filter)
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START AirFranceKLM ***");
                MakeRequest(filter, AIRFranceKLMTravelHost.KL);
                MakeRequest(filter, AIRFranceKLMTravelHost.AF);
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END AirFranceKLM ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString());
            }
            return result;
        }


        static bool MakeRequest(APIAirlineSearch filters, string TravelHost)
        {
            bool result = false;
            try
            {
                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;
                int MaxDaysNumberForSearch = 20;
                int AttemptsNumber = 1;

                DateTime originalFromDateMin = filters.FromDateMin;
                DateTime originalToDateMax = (filters.ToDateMax ?? filters.FromDateMax);

                DateTime FromDateMin = originalFromDateMin;
                DateTime ToDateMax = originalToDateMax;

                if ((originalToDateMax - originalFromDateMin).TotalDays > MaxDaysNumberForSearch)
                {
                    ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                }
                var context = new TemplateEntities1();
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                int SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.AirFranceKLM);
                String SearchType = "DAY";
                List<TripItem> Trips = new List<TripItem>();
                while (ToDateMax <= originalToDateMax)
                {
                    AttemptsNumber = 1;
                    result = false;
                    SearchType = "DAY";
                    while (!result && AttemptsNumber <= MaxRequestAttempts)
                    {
                        try
                        {
                            AttemptsNumber = AttemptsNumber + 1;
                            string url = "https://api.klm.com/opendata/flightoffers/v3/lowest-fare-offers?expand-suggested-flights=true&type="+ SearchType;
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            httpWebRequest.Headers.Add("Accept-Language", "en-US");
                            httpWebRequest.Headers.Add("AFKL-TRAVEL-Host", TravelHost);
                            httpWebRequest.Headers.Add("Api-Key", Key);

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                RequestBody body = new RequestBody();
                                body.passengerCount.ADT = filters.AdultsNumber;
                                body.passengerCount.CHD = filters.ChildrenNumber;
                                body.passengerCount.INF = filters.BabiesNumber;


                                connection connection = new connection();
                                connection.dateInterval = FromDateMin.ToString("yyyy-MM-dd") + "/" + ToDateMax.ToString("yyyy-MM-dd"); //filters.FromDate.Value.ToString("yyyy-MM-dd");
                                connection.minDaysOfStay = filters.DurationMin;
                                connection.minDaysOfStay = filters.DurationMax;
                                if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                                    connection.origin.airport.code = filters.FromAirportCode;
                                if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                                    connection.destination.airport.code = filters.ToAirportCode;
                                body.requestedConnections.Add(connection);



                                if (filters.Return)
                                {
                                    connection returnFlight = new connection();
                                    if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                                        returnFlight.destination.airport.code = filters.FromAirportCode;
                                    if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                                        returnFlight.origin.airport.code = filters.ToAirportCode;
                                    body.requestedConnections.Add(returnFlight);
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
                                string webError=FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
                                if(webError.ToUpper().Contains("UNSPECIFIED/TIME_OUT") && MaxDaysNumberForSearch>10)
                                {
                                    MaxDaysNumberForSearch = MaxDaysNumberForSearch - 5;
                                    ToDateMax = ToDateMax.AddDays(-5);
                                }
                                else
                                {
                                    SearchType = "MONTH";
                                    MaxDaysNumberForSearch = FlightsEngine.Models.Constants.Constants.APIMaxDaysNumberForSearch;
                                }
                            }

                            if (result)
                            {
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var requestResult = streamReader.ReadToEnd();

                                    if (!String.IsNullOrWhiteSpace(requestResult))
                                    {
                                        AirFranceKLMResponse ResultItem = JsonConvert.DeserializeObject<AirFranceKLMResponse>(requestResult);
                                        result = true;
                                        foreach(Itinerary itinerary in ResultItem.itineraries)
                                        {
                                            bool AddTrip = true;
                                            TripItem Trip = new TripItem();
                                            Trip.SearchTripProviderId = SearchTripProviderId;
                                            Trip.Comment = AIRFranceKLMTravelHost.ToString(TravelHost);
                                            bool returnTrip = false;
                                            if(TravelHost== AIRFranceKLMTravelHost.AF)
                                                Trip.Url = "https://www.airfrance.com";
                                            else
                                                Trip.Url = "https://www.klm.com";
                                            foreach (var connexion in itinerary.connections)
                                            {
                                                string stopInfo = "";
                                                string flightNumber = "";
                                                string airlineCode = "";
                                                int stopNumber = 0;
                                                DateTime DepartureDate = new DateTime();
                                                DateTime LastDepartureDate = DepartureDate;

                                                foreach (var stop in connexion.segments)
                                                {
                                                    stopNumber++;
                                                    flightNumber = flightNumber + stop.marketingFlight.number + FlightsEngine.Models.Constants.Constants.Separator;
                                                    if(String.IsNullOrWhiteSpace(airlineCode))
                                                        airlineCode = stop.marketingFlight.carrier.code;
                                                    else if(airlineCode!= stop.marketingFlight.carrier.code)
                                                    {
                                                        airlineCode = "SEVERAL";
                                                    }
                                                    if(stopNumber>1)
                                                        stopInfo = stopInfo + stop.origin.city.name + " (" + stop.origin.code + ")" + FlightsEngine.Models.Constants.Constants.Separator;
                                                    else
                                                    {
                                                        DepartureDate = Convert.ToDateTime(stop.departureDateTime);
                                                    }
                                                    LastDepartureDate = Convert.ToDateTime(stop.departureDateTime);
                                                }

                                                if (!String.IsNullOrWhiteSpace(stopInfo))
                                                {
                                                    stopInfo = stopInfo.Substring(0, stopInfo.Length - 3);
                                                }
                                                else
                                                {
                                                    stopInfo = null;
                                                }

                                                if (!String.IsNullOrWhiteSpace(flightNumber))
                                                {
                                                    flightNumber = flightNumber.Substring(0, flightNumber.Length - 3);
                                                }
                                                else
                                                {
                                                    flightNumber = null;
                                                }

                                                if (!returnTrip)
                                                {
                                                    Trip.OneWayTrip_Stops = connexion.segments.Count-1;
                                                    Trip.OneWayTrip_FromAirportCode = connexion.origin.airport.code;
                                                    Trip.OneWayTrip_ToAirportCode = connexion.destination.airport.code;
                                                    Trip.OneWayTrip_StopInformation = stopInfo;
                                                    Trip.OneWayTrip_AirlineName = airlineCode;
                                                    Trip.OneWayTrip_FlightNumber = flightNumber;
                                                    Trip.OneWayTrip_DepartureDate = DepartureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                                    Trip.OneWayTrip_ArrivalDate = LastDepartureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                                    Trip.OneWayTrip_Duration = -1;
                                                }
                                                else
                                                {
                                                    Trip.ReturnTrip_Stops = connexion.segments.Count-1;
                                                    Trip.ReturnTrip_FromAirportCode = connexion.origin.airport.code;
                                                    Trip.ReturnTrip_ToAirportCode = connexion.destination.airport.code;
                                                    Trip.ReturnTrip_StopInformation = stopInfo;
                                                    Trip.ReturnTrip_FlightNumber = flightNumber;
                                                    Trip.ReturnTrip_AirlineName = airlineCode;
                                                    Trip.ReturnTrip_DepartureDate = DepartureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                                    Trip.ReturnTrip_ArrivalDate = LastDepartureDate.ToString(DateFormat.Trip).Replace("-", "/");
                                                    Trip.ReturnTrip_Duration = -1;
                                                }

                                                returnTrip = true;
                                            }
                                            Trip.Price = Convert.ToDecimal(itinerary.flightProducts[0].price.totalPrice);
                                            Trip.CurrencyCode = itinerary.flightProducts[0].price.currency;
                                            AddTrip = Trip.OneWayTrip_Stops <= filters.MaxStopsNumber && (Trip.ReturnTrip_Stops==null || Trip.ReturnTrip_Stops.Value <= filters.MaxStopsNumber);
                                            if (AddTrip)
                                                Trips.Add(Trip);
                                        }

                                        FromDateMin = FromDateMin.AddDays(MaxDaysNumberForSearch);
                                        ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
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

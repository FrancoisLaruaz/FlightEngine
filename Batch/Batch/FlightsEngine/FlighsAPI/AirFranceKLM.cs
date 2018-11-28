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
using Models.Models.Shared;

namespace FlightsEngine.FlighsAPI
{
    public static class AirFranceKLM
    {

        // Limits : 5/ second, 5000/day
        // https://developer.airfranceklm.com/docs/read/opendata/flightoffer/POST_lowestfareoffers_v3

        public static int SearchFlights(APIAirlineSearch filter, int Provider,string Key)
        {
            int RequestsNumbers = 0;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START " + Providers.ToString(Provider) + " ***");
                if (Provider == Providers.KLM)
                {
                    RequestsNumbers=MakeRequest(filter, AIRFranceKLMTravelHost.KL, Key);
                }
                else if (Provider == Providers.AirFrance)
                {
                    RequestsNumbers=MakeRequest(filter, AIRFranceKLMTravelHost.AF, Key);
                }
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END " + Providers.ToString(Provider) + " ***");
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filter.ToSpecialString()+ " and Provider = "+ Provider+ " and Key = "+ Key);
            }
            return RequestsNumbers;
        }

        public static List<APIKey> GetAPIKeys()
        {
            List<APIKey> AFKLMKeys = new List<APIKey>();
            AFKLMKeys.Add(new APIKey("jqgd23tz7qk7u7vu6ayes2w3"));
            AFKLMKeys.Add(new APIKey("e53xg7bdqnptwjxtzyh5zgmq"));
            AFKLMKeys.Add(new APIKey("gg3wtw9utay5nbj2yjyypuss"));
            AFKLMKeys.Add(new APIKey("ah94hzz3kgsf4x9t795dcb22"));
            return AFKLMKeys;
        }


        static int MakeRequest(APIAirlineSearch filters, string TravelHost,string Key)
        {
            int RequestsNumbers = 0;
            bool result = false;
            try
            {
                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;
                int MaxDaysNumberForSearch= 35;
                int AttemptsNumber = 1;
                String OriginalSearchType = "OVERALL";
                String SearchType = OriginalSearchType;
                bool endOfPreSearch = false;


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
                int SearchTripProviderId = -1;
                if (TravelHost == AIRFranceKLMTravelHost.AF)
                {
                    SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.AirFrance);
                }
                else
                {
                    SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(originalFromDateMin, originalToDateMax, filters.SearchTripWishesId, Providers.KLM);
                }

                List<TripItem> Trips = new List<TripItem>();
                bool NeedToContinueSearch = true;
                while (ToDateMax <= originalToDateMax && NeedToContinueSearch)
                {
                    AttemptsNumber = 1;
                    result = false;
                    SearchType = OriginalSearchType;
                    while (!result && AttemptsNumber <= MaxRequestAttempts && NeedToContinueSearch)
                    {
                        try
                        {
                            AttemptsNumber = AttemptsNumber + 1;
                            string url = "https://api.klm.com/opendata/flightoffers/v3/lowest-fare-offers?expand-suggested-flights=true&type=" + SearchType;
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
                                RequestsNumbers++;
                                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                result = true;
                            }
                            catch (WebException e)
                            {
                                result = false;
                                string webError = FlightsEngine.Utils.Logger.GenerateWebError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
                                if (webError.ToUpper().Contains("UNSPECIFIED/TIME_OUT") && MaxDaysNumberForSearch > 10)
                                {
                                    MaxDaysNumberForSearch = MaxDaysNumberForSearch - 5;
                                    ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                                    endOfPreSearch = false;
                                }
                                else if (webError.ToLower().Contains("requestedconnections[0].destination.airport.code"))
                                {
                                    NeedToContinueSearch = false;
                                    endOfPreSearch = true;
                                }
                                else
                                {
                                    SearchType = "OVERALL";
                                    MaxDaysNumberForSearch = MaxDaysNumberForSearch-5;
                                    ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                                    endOfPreSearch = false;
                                }
                            }

                            if (result)
                            {
                                endOfPreSearch = true;
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var requestResult = streamReader.ReadToEnd();

                                    if (!String.IsNullOrWhiteSpace(requestResult))
                                    {
                                        AirFranceKLMResponse ResultItem = JsonConvert.DeserializeObject<AirFranceKLMResponse>(requestResult);
                                        if (ResultItem != null && ResultItem.itineraries != null)
                                        {
                                            foreach (Itinerary itinerary in ResultItem.itineraries)
                                            {
                                                bool AddTrip = true;
                                                TripItem Trip = new TripItem();
                                                Trip.SearchTripProviderId = SearchTripProviderId;
                                                Trip.Comment = AIRFranceKLMTravelHost.ToString(TravelHost);
                                                bool returnTrip = false;
                                                if (TravelHost == AIRFranceKLMTravelHost.AF)
                                                {
                                                    Trip.Url = "https://www.airfrance.com";
                                                    Trip.ProviderId = Providers.AirFrance;
                                                }
                                                else
                                                {
                                                    Trip.Url = "https://www.klm.com";
                                                    Trip.ProviderId = Providers.KLM;
                                                }
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
                                                        if (String.IsNullOrWhiteSpace(airlineCode))
                                                            airlineCode = stop.marketingFlight.carrier.code;
                                                        else if (airlineCode != stop.marketingFlight.carrier.code)
                                                        {
                                                            airlineCode = "SEVERAL";
                                                        }
                                                        if (stopNumber > 1)
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
                                                        Trip.OneWayTrip_Stops = connexion.segments.Count - 1;
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
                                                        Trip.ReturnTrip_Stops = connexion.segments.Count - 1;
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
                                                AddTrip = Trip.OneWayTrip_Stops <= filters.MaxStopsNumber && (Trip.ReturnTrip_Stops == null || Trip.ReturnTrip_Stops.Value <= filters.MaxStopsNumber);
                                                if (AddTrip)
                                                {
                                                    Trips.Add(Trip);
                                                    NeedToContinueSearch = false;
                                                }
                                            }
                                        }
                                    }
                                }

                                FromDateMin = FromDateMin.AddDays(MaxDaysNumberForSearch);
                                ToDateMax = FromDateMin.AddDays(MaxDaysNumberForSearch);
                            }

                            System.Threading.Thread.Sleep(200);

                        }
                        catch (Exception ex)
                        {
                            result = false;
                            FlightsEngine.Utils.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "MakeRequest LOOP ERROR :" + filters.ToSpecialString()+ " and Provider = " + TravelHost + " and Key = " + Key);
                        }

                    }
                }
                TripsService _tripService = new TripsService(context);
                _tripService.InsertTrips(Trips);
                if(endOfPreSearch)
                {
                    SearchTripWishesService _searchTripWishesService = new SearchTripWishesService(context);
                    _searchTripWishesService.DisableSearchTripWishes(filters.SearchTripWishesId);
                }
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "MakeRequest ERROR :" + filters.ToSpecialString() + " and Provider = " + TravelHost + " and Key = " + Key);
            }
            return RequestsNumbers;
        }


    }
}

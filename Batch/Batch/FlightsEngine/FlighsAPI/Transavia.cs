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
using Data.Model;
using Models.Models.Shared;
using Newtonsoft.Json;
using FlightsEngine.Models.Transavia;
using FlightsEngine.Models.Constants;
using System.Globalization;

namespace FlightsEngine.FlighsAPI
{
    public static class Transavia
    {
        public static string Key = "53965cdf041d4d9dbcb3326ac6b46461";


        #region getRoutes
        public static bool GetRoutes()
        {
            bool result = false;
            try
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  START TRANSAVIA GetRoutes ***");
                GetRoutesAync().Wait();
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***  END TRANSAVIA GetRoutes ***");
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
                _airportService.DeleteAirportsTripProvider(Providers.Transavia);
                Airports = _airportService.GetActiveAirports();
                foreach (AirportItem fromAirport in Airports)
                {
                    try
                    {

                        var client = new HttpClient();
                        var queryString = HttpUtility.ParseQueryString(string.Empty);

                        // Request headers
                        client.DefaultRequestHeaders.Add("apikey", Key);

                        var uri = "https://api.transavia.com/v2/airports/" + fromAirport.Code.ToUpper() + "?" + queryString;
                        var response = await client.GetAsync(uri);


                        if (response != null)
                        {
                            if (response.IsSuccessStatusCode && !response.StatusCode.ToString().Contains("NoContent"))
                            {
                                var contents = await response.Content.ReadAsStringAsync();
                                if (!String.IsNullOrWhiteSpace(contents))
                                {
                                    var clientRoutes = new HttpClient();
                                    var queryStringRoutes = HttpUtility.ParseQueryString(string.Empty);

                                    // Request headers
                                    clientRoutes.DefaultRequestHeaders.Add("apikey", Key);
                                    queryStringRoutes["Origin"] = fromAirport.Code.ToUpper();

                                    var uriRoutes = "https://api.transavia.com/v3/routes/?" + queryStringRoutes;
                                    var responseRoutes = await clientRoutes.GetAsync(uriRoutes);


                                    if (responseRoutes != null)
                                    {
                                        if (responseRoutes.IsSuccessStatusCode && !responseRoutes.StatusCode.ToString().Contains("NoContent"))
                                        {
                                            var contentsRoutes = await responseRoutes.Content.ReadAsStringAsync();
                                            if (!String.IsNullOrWhiteSpace(contentsRoutes))
                                            {
                                                var serializer = new JavaScriptSerializer();
                                                dynamic data = serializer.Deserialize(contentsRoutes, typeof(object));
                                                if (data != null && data.Length > 0)
                                                {
                                                    foreach (var route in data)
                                                    {
                                                        _airportService.AddAirportsTripProviderItem(route["id"].Split('-')[0], route["id"].Split('-')[1], Providers.Transavia);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FlightsEngine.Utils.Logger.GenerateInfo("Transavia Response null | FromAirport : " + fromAirport.Code);
                                    }
                                }
                            }
                        }
                        else
                        {
                            FlightsEngine.Utils.Logger.GenerateInfo("Transavia Response null | FromAirport : " + fromAirport.Code);
                        }
                    }
                    catch (Exception ex2)
                    {
                        FlightsEngine.Utils.Logger.GenerateError(ex2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "FromAirport : " + fromAirport.Code);
                    }
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }


        #endregion
        #region getflights
        public static bool SearchFlights(APIAirlineSearch filter)
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

        public static IEnumerable<Tuple<int, int>> MonthsBetween(
        DateTime startDate,
        DateTime endDate)
        {

            DateTime iterator;
            DateTime limit;

            if (endDate > startDate)
            {
                iterator = new DateTime(startDate.Year, startDate.Month, 1);
                limit = endDate;
            }
            else
            {
                iterator = new DateTime(endDate.Year, endDate.Month, 1);
                limit = startDate;
            }

            var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
            while (iterator <= limit)
            {
                yield return Tuple.Create(
                    iterator.Month,
                    iterator.Year);
                iterator = iterator.AddMonths(1);
            }

        }


        static async Task MakeRequest(APIAirlineSearch filters)
        {

            bool result = false;
            try
            {

                int MaxRequestAttempts = FlightsEngine.Models.Constants.Constants.MaxRequestAttempts;
                var ListMonthsFrom = MonthsBetween(filters.FromDateMin, filters.FromDateMax).ToList();
                var ListMonthsTo = MonthsBetween(filters.ToDateMin.Value, filters.ToDateMax.Value).ToList();
                List<TripItem> Trips = new List<TripItem>();
                var context = new TemplateEntities1();
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                TripsService _tripService = new TripsService(context);

                foreach (var tupleFromDateMonth in ListMonthsFrom)
                {
                    foreach (var tupleToDateMonth in ListMonthsTo)
                    {
                        int fromDateMonth = tupleFromDateMonth.Item2*100+ tupleFromDateMonth.Item1;
                        int toDateMonth = tupleToDateMonth.Item2* 100+ tupleToDateMonth.Item1;

                        if (fromDateMonth <= toDateMonth)
                        {
                            int AttemptsNumber = 1;
                            result = false;
                            while (!result && AttemptsNumber <= MaxRequestAttempts)
                            {
                                try
                                {
                                    AttemptsNumber++;
                                    var client = new HttpClient();
                                    var queryString = HttpUtility.ParseQueryString(string.Empty);

                                    // Request headers
                                    client.DefaultRequestHeaders.Add("apikey", Key);

                                    // Request parameters
                                    if (!String.IsNullOrWhiteSpace(filters.FromAirportCode))
                                        queryString["Origin"] = filters.FromAirportCode;
                                    if (!String.IsNullOrWhiteSpace(filters.ToAirportCode))
                                        queryString["Destination"] = filters.ToAirportCode;
                                    queryString["OriginDepartureDate"] = fromDateMonth.ToString();
                                    queryString["DestinationDepartureDate"] = toDateMonth.ToString();
                                    //     if (filters.FromDate != null)
                                    //      queryString["OriginDepartureDate"] = filters.FromDate.Value.ToString("yyyyMMdd");
                                    //   if (filters.ToDate != null)
                                    //     queryString["DestinationDepartureDate"] = filters.ToDate.Value.ToString("yyyyMMdd");
                                    if (filters.MaxStopsNumber == 0)
                                        queryString["DirectFlight"] = "true";

                                    queryString["Adults"] = filters.AdultsNumber.ToString();
                                    queryString["Children"] = filters.ChildrenNumber.ToString();
                                    queryString["Limit"] = "200";

                                    var uri = "https://api.transavia.com/v1/flightoffers/?" + queryString;
                                    var response = await client.GetAsync(uri);


                                    if (response != null)
                                    {
                                        result = true;
                                        if (response.IsSuccessStatusCode && response.StatusCode.ToString() != "NoContent")
                                        {
                                            var contents = await response.Content.ReadAsStringAsync();
                                            if (!String.IsNullOrWhiteSpace(contents))
                                            {
                                                TransaviaResponse ResultItem = JsonConvert.DeserializeObject<TransaviaResponse>(contents);
                                                if (ResultItem != null && ResultItem.flightOffer != null)
                                                {
                                                    foreach (var route in ResultItem.flightOffer)
                                                    {
                                                        int daysAtDestination = Utils.Utils.GetDaysAtDestination(route.outboundFlight.departureDateTime, route.inboundFlight.departureDateTime);
                                                        if (route.outboundFlight.departureDateTime>=filters.FromDateMin && route.outboundFlight.departureDateTime<=filters.FromDateMax.AddDays(1)
                                                            && route.inboundFlight.departureDateTime >= filters.ToDateMin && route.inboundFlight.departureDateTime <= filters.ToDateMax.Value.AddDays(1)
                                                            && daysAtDestination>=filters.DurationMin && daysAtDestination<=filters.DurationMax)
                                                        {
                                                            TripItem Trip = new TripItem();
                                                            Trip.OneWayTrip_AirlineName = Providers.ToString(Providers.Transavia);
                                                            Trip.ReturnTrip_AirlineName = Providers.ToString(Providers.Transavia);
                                                            Trip.OneWayTrip_Stops = 0;
                                                            Trip.ReturnTrip_Stops = 0;
                                                            Trip.ProviderId = Providers.Transavia;
                                                            Trip.OneWayTrip_Duration = _tripService.GetTripDuration(route.outboundFlight.departureDateTime, route.outboundFlight.arrivalDateTime, route.outboundFlight.departureAirport.locationCode, route.outboundFlight.arrivalAirport.locationCode);
                                                            Trip.ReturnTrip_Duration = _tripService.GetTripDuration(route.inboundFlight.departureDateTime, route.inboundFlight.arrivalDateTime, route.inboundFlight.departureAirport.locationCode, route.inboundFlight.arrivalAirport.locationCode);
                                                            Trip.OneWayTrip_FromAirportCode = route.outboundFlight.departureAirport.locationCode;
                                                            Trip.OneWayTrip_ToAirportCode = route.outboundFlight.arrivalAirport.locationCode;
                                                            Trip.ReturnTrip_FromAirportCode = route.inboundFlight.departureAirport.locationCode;
                                                            Trip.ReturnTrip_ToAirportCode = route.inboundFlight.arrivalAirport.locationCode;
                                                            Trip.OneWayTrip_FlightNumber = route.outboundFlight.flightNumber.ToString();
                                                            Trip.ReturnTrip_FlightNumber = route.inboundFlight.flightNumber.ToString();
                                                            Trip.OneWayTrip_DepartureDate = route.outboundFlight.departureDateTime.ToString(DateFormat.Trip).Replace("-", "/");
                                                            Trip.OneWayTrip_ArrivalDate = route.outboundFlight.arrivalDateTime.ToString(DateFormat.Trip).Replace("-", "/");
                                                            Trip.ReturnTrip_DepartureDate = route.inboundFlight.departureDateTime.ToString(DateFormat.Trip).Replace("-", "/");
                                                            Trip.ReturnTrip_ArrivalDate = route.inboundFlight.arrivalDateTime.ToString(DateFormat.Trip).Replace("-", "/");
                                                            Trip.SearchTripProviderId = _searchTripProviderService.GetSearchTripProviderId(route.outboundFlight.departureDateTime, route.inboundFlight.departureDateTime, filters.SearchTripWishesId, Providers.Transavia);
                                                            Trip.Price = Convert.ToDecimal(route.pricingInfoSum.totalPriceAllPassengers);
                                                            Trip.CurrencyCode = route.pricingInfoSum.currencyCode;
                                                            Trip.Url = route.deeplink.href;
                                                            Trips.Add(Trip);

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FlightsEngine.Utils.Logger.GenerateInfo("Transavia Response null : " + filters.ToSpecialString());
                                    }
                                }
                                catch (Exception e)
                                {
                                    FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString() + " and fromDateMonth = " + fromDateMonth + " and toDateMonth = " + toDateMonth + " and AttemptsNumber = " + AttemptsNumber);
                                }
                            }
                        }

                    }
                }
                _tripService.InsertTrips(Trips);
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, filters.ToSpecialString());
            }
        }
        #endregion
    }
}

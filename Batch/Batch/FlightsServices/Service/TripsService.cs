﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Data.Repositories;
using FlightsEngine.Models;
using FlightsEngine.Models.Constants;
using FlightsEngine.Utils;

namespace FlightsServices
{
    public class TripsService : ITripsService
    {

        private readonly IGenericRepository<SearchTripProvider> _searchTripProviderRepo;
        private readonly IGenericRepository<Trip> _tripRepo;
        private readonly IGenericRepository<Flight> _flightRepo;
        private readonly IGenericRepository<Airport> _airportRepo;

        public TripsService(IGenericRepository<SearchTripProvider> SearchTripProvider,
            IGenericRepository<Trip> tripRepo,
            IGenericRepository<Flight> flightRepo,
            IGenericRepository<Airport> airportRepo)
        {
            _searchTripProviderRepo = SearchTripProvider;
            _tripRepo = tripRepo;
            _flightRepo = flightRepo;
            _airportRepo = airportRepo;
        }

        public TripsService(TemplateEntities1 context)
        {
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _tripRepo = new GenericRepository<Trip>(context);
            _flightRepo = new GenericRepository<Flight>(context);
            _airportRepo = new GenericRepository<Airport>(context);
        }

        public TripsService()
        {
            var context = new TemplateEntities1();
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _tripRepo = new GenericRepository<Trip>(context);
            _flightRepo = new GenericRepository<Flight>(context);
            _airportRepo = new GenericRepository<Airport>(context);
        }

        public  int GetTripDuration(DateTime fromDate,DateTime toDate,string fromAirportCode,string toAirportCode)
        {
            int result = 0;

            try
            {
                decimal? offsetFrom = _airportRepo.FindAllBy(a => a.Code.ToUpper() == fromAirportCode.ToUpper()).FirstOrDefault()?.City.OffSetHours ;
                decimal ?offsetTo = _airportRepo.FindAllBy(a => a.Code.ToUpper() == toAirportCode.ToUpper()).FirstOrDefault()?.City.OffSetHours ;

                if(offsetFrom != null && offsetTo!=null)
                {
                    double offset = Convert.ToDouble(offsetTo.Value - offsetFrom.Value)*60;
                    TimeSpan span = toDate - fromDate;
                    result = Convert.ToInt32(span.TotalMinutes-offset);
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "fromDate = " + fromDate+" and toDate = "+toDate+ " and fromAirportCode= "+ fromAirportCode+ " and toAirportCode = "+ toAirportCode);
            }
            return result;
        }

        public bool InsertTrips(int SearchTripProviderId)
        {
            bool result = false;
            try
            {
                var SearchTripProvider = _searchTripProviderRepo.Get(SearchTripProviderId);
                if (SearchTripProvider != null)
                {
                    #region delete old records
                    // Delete old records
                    var TripsToDelete = SearchTripProvider.Trips.ToList();
                    foreach (var trip in TripsToDelete)
                    {
                        var FlightsToDelete = trip.Flights.ToList();
                        foreach (var flight in FlightsToDelete)
                        {
                            _flightRepo.Delete(flight);
                            _flightRepo.Save();
                        }

                        _tripRepo.Delete(trip);
                        _tripRepo.Save();
                    }
                    #endregion
                    string HtmlFile = "D:\\Html\\search_" + SearchTripProviderId + ".html";
                    if (File.Exists(HtmlFile))
                    {
                        string Html = File.ReadAllText(HtmlFile);
                        if (!String.IsNullOrWhiteSpace(Html))
                        {
                            TripsFromHtmlResult Result = new TripsFromHtmlResult();
                            if (SearchTripProvider.ProviderId == Providers.Kayak)
                            {
                                Result = ScrappingHelper.GetKayakTripsFromHtml(Html, SearchTripProviderId, SearchTripProvider.Url, SearchTripProvider.SearchTrip.FromDate, SearchTripProvider.SearchTrip.ToDate);
                            }
                            if (Result.Success)
                            {
                                if (InsertTrips(Result.Trips))
                                {
                                    File.Delete(HtmlFile);
                                }
                            }
                        }
                        else
                        {
                            FlightsEngine.Utils.Logger.GenerateInfo("File empty for SearchTripProviderId = " + SearchTripProviderId);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripProviderId = " + SearchTripProviderId);
            }
            return result;
        }




        public bool InsertTrips(List<TripItem> Trips)
        {
            bool result = false;
            try
            {
                if (Trips != null)
                {
                    result = true;
                    foreach (TripItem Trip in Trips)
                    {
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(Trip.OneWayTrip_ToAirportCode))
                            {
                                List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                                Parameters.Add(new Tuple<string, object>("@SearchTripProviderId", Trip.SearchTripProviderId));
                                Parameters.Add(new Tuple<string, object>("@ProviderId", Trip.ProviderId));
                                Parameters.Add(new Tuple<string, object>("@CurrencyCode", Trip.CurrencyCode));
                                Parameters.Add(new Tuple<string, object>("@Price", Trip.Price));
                                Parameters.Add(new Tuple<string, object>("@Url", Trip.Url));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_AirlineLogoSrc", Trip.OneWayTrip_AirlineLogoSrc));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_AirlineName", Trip.OneWayTrip_AirlineName));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_ArrivalDate", Trip.OneWayTrip_ArrivalDate));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_DepartureDate", Trip.OneWayTrip_DepartureDate));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_Duration", Trip.OneWayTrip_Duration));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_FromAirportCode", Trip.OneWayTrip_FromAirportCode));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_Stops", Trip.OneWayTrip_Stops));
                                Parameters.Add(new Tuple<string, object>("@OneWayTrip_ToAirportCode", Trip.OneWayTrip_ToAirportCode));
                                if (!string.IsNullOrWhiteSpace(Trip.Comment))
                                    Parameters.Add(new Tuple<string, object>("@Comment", Trip.Comment));
                                if (!string.IsNullOrWhiteSpace(Trip.OneWayTrip_StopInformation))
                                    Parameters.Add(new Tuple<string, object>("@OneWayTrip_StopInformation", Trip.OneWayTrip_StopInformation ?? ""));
                                if (!string.IsNullOrWhiteSpace(Trip.OneWayTrip_FlightNumber))
                                    Parameters.Add(new Tuple<string, object>("@OneWayTrip_FlightNumber", Trip.OneWayTrip_FlightNumber ?? ""));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_AirlineLogoSrc))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_AirlineLogoSrc", Trip.ReturnTrip_AirlineLogoSrc));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_AirlineName))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_AirlineName", Trip.ReturnTrip_AirlineName));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_ArrivalDate))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_ArrivalDate", Trip.ReturnTrip_ArrivalDate));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_DepartureDate))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_DepartureDate", Trip.ReturnTrip_DepartureDate));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_FlightNumber))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_FlightNumber", Trip.ReturnTrip_FlightNumber));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_StopInformation))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_StopInformation", Trip.ReturnTrip_StopInformation));
                                if (Trip.ReturnTrip_Duration != null)
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_Duration", Trip.ReturnTrip_Duration));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_FromAirportCode))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_FromAirportCode", Trip.ReturnTrip_FromAirportCode));
                                if (Trip.ReturnTrip_Stops != null)
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_Stops", Trip.ReturnTrip_Stops));
                                if (!string.IsNullOrWhiteSpace(Trip.ReturnTrip_ToAirportCode))
                                    Parameters.Add(new Tuple<string, object>("@ReturnTrip_ToAirportCode", Trip.ReturnTrip_ToAirportCode));
                                bool procedureExecution = _tripRepo.ExecuteStoredProcedure("[dbo].[InsertTripWithTransaction]", Parameters);
                                result = result & procedureExecution;
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                            FlightsEngine.Utils.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "InsertTrips Loop,  = " + Trip.SearchTripProviderId + " and price = " + Trip.Price);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }
}

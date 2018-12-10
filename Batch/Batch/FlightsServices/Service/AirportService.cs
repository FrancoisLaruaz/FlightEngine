using System;
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
using Models.Models.Shared;

namespace FlightsServices
{
    public class AirportService : IAirportService
    {


        private readonly IGenericRepository<Airport> _airportRepo;
        private readonly IGenericRepository<AirportsTrip> _airportsTripRepo;
        private readonly IGenericRepository<Provider> _providerRepo;

        public AirportService(IGenericRepository<Airport> airportRepo, IGenericRepository<AirportsTrip> airportsTripRepo, IGenericRepository<Provider> providerRepo)
        {

            _airportRepo = airportRepo;
            _airportsTripRepo = airportsTripRepo;
            _providerRepo = providerRepo;
        }

        public AirportService(TemplateEntities1 context)
        {
            _airportRepo = new GenericRepository<Airport>(context);
            _airportsTripRepo = new GenericRepository<AirportsTrip>(context);
            _providerRepo = new GenericRepository<Provider>(context);
        }

        public AirportService()
        {
            var context = new TemplateEntities1();
            _airportRepo = new GenericRepository<Airport>(context);
            _airportsTripRepo = new GenericRepository<AirportsTrip>(context);
            _providerRepo = new GenericRepository<Provider>(context);
        }

        public bool AddAirportsTripProviderItem(string fromAirportCode, string toAirportCode, int ProviderId)
        {
            bool result = false;
            try
            {
                AirportsTrip airportsTrip = _airportsTripRepo.FindAllBy(a => (a.Airport.Code.ToLower() == toAirportCode && a.Airport1.Code.ToLower() == fromAirportCode) || ((a.Airport.Code.ToLower() == fromAirportCode && a.Airport1.Code.ToLower() == toAirportCode))).FirstOrDefault();
                if (airportsTrip == null)
                {
                    airportsTrip = new AirportsTrip();
                    airportsTrip.FromAirportId = (_airportRepo.FindAllBy(a => a.Code.ToLower() == fromAirportCode)?.FirstOrDefault())?.Id ?? 0;
                    airportsTrip.ToAirportId = (_airportRepo.FindAllBy(a => a.Code.ToLower() == toAirportCode)?.FirstOrDefault())?.Id ?? 0;
                    if (airportsTrip.ToAirportId > 0 && airportsTrip.FromAirportId > 0)
                    {
                        airportsTrip.Attractiveness = 100;
                        _airportsTripRepo.Add(airportsTrip);
                        _airportsTripRepo.Save();
                    }
                }
                if (airportsTrip != null && airportsTrip.Id > 0 && !airportsTrip.Providers.Where(p => p.Id == ProviderId).Any())
                {
                    airportsTrip.Providers.Add(_providerRepo.Get(ProviderId));
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "fromAirportCode = " + fromAirportCode + " and ToAirpoderCode = " + toAirportCode + " and ProviderId = " + ProviderId);
            }
            finally
            {
                result = _airportsTripRepo.Save();
            }
            return result;
        }

        public List<AirportItem> GetActiveAirports()
        {
            List<AirportItem> result = new List<AirportItem>();
            try
            {
                result = _airportRepo.FindAllBy(a => a.Active).Select(t => new AirportItem(t.Code, t.Id, t.CityId, t.City.CountryId, t.City.Country.ContinentId, t.City.Code)).OrderBy(a => a.Id).ToList();

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }


        public bool DeleteAirportsTripProvider(int ProviderId)
        {
            bool result = false;
            try
            {
                var airportsTrips = _airportsTripRepo.FindAllBy(a => a.Providers.Where(p => p.Id == ProviderId).Any()).ToList();
                foreach (var airportTrip in airportsTrips)
                {
                    airportTrip.Providers.Remove(airportTrip.Providers.Where(p => p.Id == ProviderId).FirstOrDefault());
                    _airportsTripRepo.Edit(airportTrip);
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderId = " + ProviderId);
            }
            finally
            {
                result = _airportsTripRepo.Save();
            }
            return result;
        }

        public AirportsTrip GetAirportsTripForProviderRoute(int ProviderId)
        {
            AirportsTrip result = null;
            try
            {
                var airportsTrips = _airportsTripRepo.FindAllBy(a => a.Providers.Where(p => p.Id == ProviderId).Any()).ToList();
                if (airportsTrips != null && airportsTrips.Count > 0)
                {
                    return airportsTrips.OrderBy(a => a.Id).OrderBy(t => t.Airport1.Code).OrderBy(t => t.Airport.Code).FirstOrDefault();
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderId = " + ProviderId);
            }
            return result;
        }

        public AirportItem GetLastAirportForProviderRoute(int ProviderId)
        {
            AirportItem result =null;
            try
            {
                var airportsTrips = _airportsTripRepo.FindAllBy(a => a.Providers.Where(p => p.Id == ProviderId).Any()).ToList();
                if (airportsTrips != null && airportsTrips.Count > 0)
                {
                    var airportTrip = airportsTrips.OrderBy(a => a.Id).OrderBy(t => t.Airport.Code).FirstOrDefault();
                    if(airportTrip != null)
                    {
                        result = new AirportItem(airportTrip.Airport.Code, airportTrip.Airport.Id, airportTrip.Airport.CityId, airportTrip.Airport.City.CountryId, airportTrip.Airport.City.Country.ContinentId, airportTrip.Airport.City.Code);
                    }
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderId = " + ProviderId);
            }
            return result;
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using FlightsEngine.Models;

namespace FlightsServices
{
    public interface ITripsService
    {

        int GetTripDuration(DateTime fromDate, DateTime toDate, string fromAirportCode, string toAirportCode);
        bool InsertTrips(int SearchTripProviderId);

    }
}

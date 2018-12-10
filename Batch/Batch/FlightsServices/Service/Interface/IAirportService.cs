using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using FlightsEngine.Models;
using Models.Models.Shared;

namespace FlightsServices
{
    public interface IAirportService
    {
        AirportsTrip GetAirportsTripForProviderRoute(int ProviderId);
        AirportItem GetLastAirportForProviderRoute(int ProviderId);
        bool DeleteAirportsTripProvider(int ProviderId);
        List<AirportItem> GetActiveAirports();
        bool AddAirportsTripProviderItem(string fromAirportCode, string toAirportCode, int ProviderId);
    }
}

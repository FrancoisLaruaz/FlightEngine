using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.BritishAirways
{
    public class BACountry
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public object City { get; set; }
    }

    public class BACity
    {
        public string CityName { get; set; }
        public string CityCode { get; set; }
    }

    public class GetBALocationsResponse
    {
        public List<BACountry> Country { get; set; }
    }

    public class BARouteResponse
    {
        public GetBALocationsResponse GetBA_LocationsResponse { get; set; }
    }
}

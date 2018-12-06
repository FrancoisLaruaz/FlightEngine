using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.RyanAir
{


    public class RyanAirAirports
    {
        public string iataCode { get; set; }
        public string name { get; set; }
        public string seoName { get; set; }
        public Coordinates coordinates { get; set; }
        public bool @base { get; set; }
        public string countryCode { get; set; }
        public string regionCode { get; set; }
        public string cityCode { get; set; }
        public string currencyCode { get; set; }
        public List<string> routes { get; set; }
        public List<object> seasonalRoutes { get; set; }
        public List<object> categories { get; set; }
        public int priority { get; set; }
    }
    public class Coordinates
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }


}
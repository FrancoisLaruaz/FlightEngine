using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class TripItem
    {
        public TripItem()
        {

           
        }

        public int ProviderId { get; set; }
        public int SearchTripProviderId { get; set; }

        public decimal Price { get; set; }

        public string CurrencyCode { get; set; }

        public string Url { get; set; }

        public string OneWayTrip_FromAirportCode { get; set; }

        public string OneWayTrip_FlightNumber { get; set; }

        public string OneWayTrip_StopInformation { get; set; }

        public string OneWayTrip_ToAirportCode { get; set; }

        public string OneWayTrip_DepartureDate { get; set; }

        public string OneWayTrip_ArrivalDate { get; set; }

        public int OneWayTrip_Duration { get; set; }

        public string OneWayTrip_AirlineName { get; set; }

        public string OneWayTrip_AirlineLogoSrc { get; set; }

        public int OneWayTrip_Stops { get; set; }

        public string ReturnTrip_FlightNumber { get; set; }

        public string ReturnTrip_StopInformation { get; set; }

        public string ReturnTrip_FromAirportCode { get; set; }

        public string ReturnTrip_ToAirportCode { get; set; }

        public string ReturnTrip_DepartureDate { get; set; }

        public string ReturnTrip_ArrivalDate { get; set; }

        public int? ReturnTrip_Duration { get; set; }

        public string ReturnTrip_AirlineName { get; set; }

        public string ReturnTrip_AirlineLogoSrc { get; set; }

        public int? ReturnTrip_Stops { get; set; }

        public string Comment { get; set; }

    }
}

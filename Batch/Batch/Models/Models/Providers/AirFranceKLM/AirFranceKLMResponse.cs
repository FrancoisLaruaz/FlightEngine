using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.AirFranceKLM
{
    public class AirFranceKLMResponse
    {
        public List<Itinerary> itineraries { get; set; }
        public Disclaimer disclaimer { get; set; }
    }

    public class City
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Airport
    {
        public string name { get; set; }
        public City city { get; set; }
        public string code { get; set; }
    }

    public class Destination
    {
        public Airport airport { get; set; }
    }

    public class City2
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Airport2
    {
        public string name { get; set; }
        public City2 city { get; set; }
        public string code { get; set; }
    }

    public class Origin
    {
        public Airport2 airport { get; set; }
    }

    public class City3
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Destination2
    {
        public string name { get; set; }
        public City3 city { get; set; }
        public string code { get; set; }
    }

    public class Carrier
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class MarketingFlight
    {
        public Carrier carrier { get; set; }
        public string number { get; set; }
    }

    public class City4
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Origin2
    {
        public string name { get; set; }
        public City4 city { get; set; }
        public string code { get; set; }
    }

    public class Segment
    {
        public DateTime departureDateTime { get; set; }
        public Destination2 destination { get; set; }
        public MarketingFlight marketingFlight { get; set; }
        public Origin2 origin { get; set; }
    }

    public class Connection
    {
        public string departureDate { get; set; }
        public Destination destination { get; set; }
        public Origin origin { get; set; }
        public List<Segment> segments { get; set; }
    }

    public class Passenger
    {
        public int id { get; set; }
        public string type { get; set; }
    }

    public class PricePerPassengerType
    {
        public string passengerType { get; set; }
        public double fare { get; set; }
        public double taxes { get; set; }
    }

    public class Price
    {
        public double displayPrice { get; set; }
        public double totalPrice { get; set; }
        public double accuracy { get; set; }
        public List<PricePerPassengerType> pricePerPassengerTypes { get; set; }
        public bool flexibilityWaiver { get; set; }
        public string currency { get; set; }
        public string displayType { get; set; }
    }

    public class FareBasis
    {
        public string code { get; set; }
    }

    public class PricePerPassengerType2
    {
        public string passengerType { get; set; }
        public double fare { get; set; }
        public double taxes { get; set; }
    }

    public class Price2
    {
        public double displayPrice { get; set; }
        public double totalPrice { get; set; }
        public double accuracy { get; set; }
        public List<PricePerPassengerType2> pricePerPassengerTypes { get; set; }
        public bool flexibilityWaiver { get; set; }
        public string currency { get; set; }
        public string displayType { get; set; }
    }

    public class Cabin
    {
        public string @class { get; set; }
    }

    public class SellingClass
    {
        public string code { get; set; }
    }

    public class Segment2
    {
        public Cabin cabin { get; set; }
        public SellingClass sellingClass { get; set; }
    }

    public class Connection2
    {
        public int numberOfSeatsAvailable { get; set; }
        public FareBasis fareBasis { get; set; }
        public Price2 price { get; set; }
        public List<Segment2> segments { get; set; }
    }

    public class FlightProduct
    {
        public List<Passenger> passengers { get; set; }
        public Price price { get; set; }
        public List<Connection2> connections { get; set; }
        public string type { get; set; }
    }

    public class Itinerary
    {
        public List<Connection> connections { get; set; }
        public List<FlightProduct> flightProducts { get; set; }
    }

    public class Disclaimer
    {
        public string displayPriceText { get; set; }
        public string totalPriceText { get; set; }
    }


}
using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.RyanAir
{


    public class DepartureAirport
    {
        public string iataCode { get; set; }
        public string name { get; set; }
        public string seoName { get; set; }
        public string countryName { get; set; }
    }

    public class ArrivalAirport
    {
        public string iataCode { get; set; }
        public string name { get; set; }
        public string seoName { get; set; }
        public string countryName { get; set; }
    }

    public class Price
    {
        public double value { get; set; }
        public string valueMainUnit { get; set; }
        public string valueFractionalUnit { get; set; }
        public string currencyCode { get; set; }
        public string currencySymbol { get; set; }
    }

    public class Outbound
    {
        public DepartureAirport departureAirport { get; set; }
        public ArrivalAirport arrivalAirport { get; set; }
        public DateTime departureDate { get; set; }
        public DateTime arrivalDate { get; set; }
        public Price price { get; set; }
        public string sellKey { get; set; }
    }

    public class DepartureAirport2
    {
        public string iataCode { get; set; }
        public string name { get; set; }
        public string seoName { get; set; }
        public string countryName { get; set; }
    }

    public class ArrivalAirport2
    {
        public string iataCode { get; set; }
        public string name { get; set; }
        public string seoName { get; set; }
        public string countryName { get; set; }
    }

    public class Price2
    {
        public double value { get; set; }
        public string valueMainUnit { get; set; }
        public string valueFractionalUnit { get; set; }
        public string currencyCode { get; set; }
        public string currencySymbol { get; set; }
    }

    public class Inbound
    {
        public DepartureAirport2 departureAirport { get; set; }
        public ArrivalAirport2 arrivalAirport { get; set; }
        public DateTime departureDate { get; set; }
        public DateTime arrivalDate { get; set; }
        public Price2 price { get; set; }
        public string sellKey { get; set; }
    }

    public class Price3
    {
        public double value { get; set; }
        public string valueMainUnit { get; set; }
        public string valueFractionalUnit { get; set; }
        public string currencyCode { get; set; }
        public string currencySymbol { get; set; }
    }

    public class Summary
    {
        public Price3 price { get; set; }
        public bool newRoute { get; set; }
    }

    public class Fare
    {
        public Outbound outbound { get; set; }
        public Inbound inbound { get; set; }
        public Summary summary { get; set; }
    }

    public class RyanAirTripsResponse
    {
        public int total { get; set; }
        public object arrivalAirportCategories { get; set; }
        public List<Fare> fares { get; set; }
        public int size { get; set; }
    }


}
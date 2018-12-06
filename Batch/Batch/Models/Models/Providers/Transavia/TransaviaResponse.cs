using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.Transavia
{


    public class ResultSet
    {
        public int count { get; set; }
    }

    public class MarketingAirline
    {
        public string companyShortName { get; set; }
    }

    public class DepartureAirport
    {
        public string locationCode { get; set; }
    }

    public class ArrivalAirport
    {
        public string locationCode { get; set; }
    }

    public class PricingInfo
    {
        public double totalPriceAllPassengers { get; set; }
        public double totalPriceOnePassenger { get; set; }
        public double baseFare { get; set; }
        public double taxSurcharge { get; set; }
        public string currencyCode { get; set; }
        public string productClass { get; set; }
    }

    public class OutboundFlight
    {
        public string id { get; set; }
        public DateTime departureDateTime { get; set; }
        public DateTime arrivalDateTime { get; set; }
        public MarketingAirline marketingAirline { get; set; }
        public int flightNumber { get; set; }
        public DepartureAirport departureAirport { get; set; }
        public ArrivalAirport arrivalAirport { get; set; }
        public PricingInfo pricingInfo { get; set; }
    }

    public class MarketingAirline2
    {
        public string companyShortName { get; set; }
    }

    public class DepartureAirport2
    {
        public string locationCode { get; set; }
    }

    public class ArrivalAirport2
    {
        public string locationCode { get; set; }
    }

    public class PricingInfo2
    {
        public double totalPriceAllPassengers { get; set; }
        public double totalPriceOnePassenger { get; set; }
        public double baseFare { get; set; }
        public double taxSurcharge { get; set; }
        public string currencyCode { get; set; }
        public string productClass { get; set; }
    }

    public class InboundFlight
    {
        public string id { get; set; }
        public DateTime departureDateTime { get; set; }
        public DateTime arrivalDateTime { get; set; }
        public MarketingAirline2 marketingAirline { get; set; }
        public int flightNumber { get; set; }
        public DepartureAirport2 departureAirport { get; set; }
        public ArrivalAirport2 arrivalAirport { get; set; }
        public PricingInfo2 pricingInfo { get; set; }
    }

    public class PricingInfoSum
    {
        public double totalPriceAllPassengers { get; set; }
        public double totalPriceOnePassenger { get; set; }
        public double baseFare { get; set; }
        public double taxSurcharge { get; set; }
        public string currencyCode { get; set; }
        public string productClass { get; set; }
    }

    public class Deeplink
    {
        public string href { get; set; }
    }

    public class FlightOffer
    {
        public OutboundFlight outboundFlight { get; set; }
        public InboundFlight inboundFlight { get; set; }
        public PricingInfoSum pricingInfoSum { get; set; }
        public Deeplink deeplink { get; set; }
    }

    public class TransaviaResponse
    {
        public ResultSet resultSet { get; set; }
        public List<FlightOffer> flightOffer { get; set; }
    }




}
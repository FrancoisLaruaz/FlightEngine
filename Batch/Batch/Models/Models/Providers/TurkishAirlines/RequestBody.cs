using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.TurkishAirlines
{
    public class RequestBody
    {
        public RequestBody()
        {
            OriginDestinationInformation = new List<OriginDestinationInformation>();
            PassengerTypeQuantity = new List<PassengerTypeQuantity>();
        }

        public bool ReducedDataIndicator { get; set; }
        public string RoutingType { get; set; }
        public List<PassengerTypeQuantity> PassengerTypeQuantity { get; set; }
        public List<OriginDestinationInformation> OriginDestinationInformation { get; set; }
    }

    public class PassengerTypeQuantity
    {
        public PassengerTypeQuantity(string _code, int _Quantity)
        {
            Code = _code;
            Quantity = _Quantity;
        }

        public string Code { get; set; }
        public int Quantity { get; set; }
    }

    public class DepartureDateTime
    {
        public DepartureDateTime()
        {

        }

        public string WindowAfter { get; set; }
        public string WindowBefore { get; set; }
        public string Date { get; set; }
    }

    public class OriginLocation
    {

        public OriginLocation()
        {

        }
        public string LocationCode { get; set; }
        public bool MultiAirportCityInd { get; set; }
    }

    public class DestinationLocation
    {
        public DestinationLocation()
        {

        }

        public string LocationCode { get; set; }
        public bool MultiAirportCityInd { get; set; }
    }

    public class CabinPreference
    {
        public CabinPreference(string _Cabin)
        {
            Cabin = _Cabin;
        }

        public CabinPreference()
        {
   
        }
        public string Cabin { get; set; }
    }

    public class OriginDestinationInformation
    {
        public OriginDestinationInformation()
        {
            DepartureDateTime = new DepartureDateTime();
            OriginLocation = new OriginLocation();
            DestinationLocation = new DestinationLocation();
            CabinPreferences = new List<CabinPreference>();
        }
        public DepartureDateTime DepartureDateTime { get; set; }
        public OriginLocation OriginLocation { get; set; }
        public DestinationLocation DestinationLocation { get; set; }
        public List<CabinPreference> CabinPreferences { get; set; }
    }


}

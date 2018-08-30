using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.AirHob
{
    public class RequestBody
    {
        public RequestBody()
        {
            ClassType = "Economy";
            OriginDestination = new OriginDestination();
            Currency = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
        }

        public string TripType { get; set; }
        public string NoOfAdults { get; set; }

        public string NoOfChilds { get; set; }

        public string NoOfInfants { get; set; }

        public string ClassType { get; set; }

        public string Currency { get; set; }

        public OriginDestination OriginDestination { get; set; }
    }

    public class OriginDestination
    {
        public OriginDestination()
        {

        }

        public string Origin { get; set; }
        public string Destination { get; set; }

        public string TravelDate { get; set; }

    }
}

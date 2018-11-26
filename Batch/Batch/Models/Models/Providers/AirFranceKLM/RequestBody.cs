using FlightsEngine.Models.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models.AirFranceKLM
{
    public class RequestBody
    {
        public RequestBody()
        {
            commercialCabins = new List<string>() { "ECONOMY" };
            discountCode = "";
            currency = FlightsEngine.Models.Constants.Constants.DefaultCurrency;
            passengerCount = new passengerCount();
            passengerCount.ADT = 1;
            requestedConnections = new List<connection>();
        }

        public string discountCode { get; set; }
        public bool shortest { get; set; }

        public string minimumAccuracy { get; set; }

        public List<string> commercialCabins { get; set; }

        public string currency { get; set; }

        public passengerCount passengerCount { get; set; }

        public List<connection> requestedConnections { get; set; }
    }

    public class airport
    {
        public airport()
        {

        }

        public string code { get; set; }
    }

    public class place
    {
        public airport airport { get; set; }
        public place()
        {
            airport = new airport();
        }
    }

    public class connection
    {
        public connection()
        {
            origin = new place();
            destination = new place();
        }

        public place origin { get; set; }

        public place destination { get; set; }

        public string dateInterval { get; set; }

        public int? minDaysOfStay { get; set; }

        public int? maxDaysOfStay { get; set; }
    }



    public class passengerCount
    {
        public passengerCount()
        {

        }

        public int YTH { get; set; }
        public int INF { get; set; }

        public int ADT { get; set; }
        public int CHD { get; set; }


    }
}

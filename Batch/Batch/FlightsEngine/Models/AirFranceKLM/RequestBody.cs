﻿using FlightsEngine.Utils;
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
            cabinClass = "ECONOMY";
            discountCode = "";
            currency = Constants.DefaultCurrency;
            passengerCount = new passengerCount();
            passengerCount.ADULT = 1;
            minimumAccuracy = "";
            requestedConnections = new List<connection>();
        }

        public string discountCode { get; set; }
        public bool shortest { get; set; }

        public string minimumAccuracy { get; set; }

        public string cabinClass { get; set; }

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

        public string departureDate { get; set; }
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

    public class passengerCount
    {
        public passengerCount()
        {

        }

        public int YOUNG_ADULT { get; set; }
        public int INFANT { get; set; }

        public int ADULT { get; set; }
        public int CHILD { get; set; }


    }
}

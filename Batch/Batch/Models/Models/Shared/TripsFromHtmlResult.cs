using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class TripsFromHtmlResult
    {
        public TripsFromHtmlResult()
        {
            Trips = new List<TripItem>();
        }

        public List<TripItem> Trips { get; set; }
        public bool Success { get; set; }


    }
}

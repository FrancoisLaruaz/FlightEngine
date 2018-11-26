using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class APIAirlineSearch
    {
        public APIAirlineSearch()
        {

        }

        public int SearchTripWishesId { get; set; }

        public string FromAirportCode { get; set; }

        public string ToAirportCode { get; set; }

        public DateTime FromDateMin { get; set; }

        public DateTime? ToDateMin { get; set; }

        public DateTime? ToDateMax { get; set; }

        public DateTime FromDateMax { get; set; }

        public int MaxStopsNumber { get; set; }

        public int AdultsNumber { get; set; }

        public int ChildrenNumber { get; set; }

        public int BabiesNumber { get; set; }

        public int? DurationMax { get; set; }

        public int? DurationMin { get; set; }


        public bool Return { get; set; }

        public string ToSpecialString()
        {
            string error = " FILTER : ";
            try
            {
                error = error + "SearchTripWishesId =  " + SearchTripWishesId ;
                error = error + " | FromAirportCode =  " + FromAirportCode ?? "[NULL]";
                error = error + " | ToAirportCode =  " + ToAirportCode ?? "[NULL]";
                error = error + " | Return =  " + Return;
                error = error + " | FromDateMin =  " +FromDateMin.ToString();
                error = error + " | FromDateMax =  " +  FromDateMax.ToString();
                error = error + " | ToDateMin =  " + (ToDateMin.HasValue ? ToDateMin.ToString() : "[NULL]");
                error = error + " | ToDateMax =  " + (ToDateMax.HasValue ? ToDateMax.ToString() : "[NULL]");
                error = error + " | DurationMin =  " + (DurationMin == null ? "[NULL]" : DurationMin.ToString());
                error = error + " | DurationMax =  " + (DurationMax == null? "[NULL]": DurationMax.ToString());
                error = error + " | MaxStopsNumber =  " + MaxStopsNumber;
                error = error + " | AdultsNumber =  " + AdultsNumber;
                error = error + " | ChildrenNumber =  " + ChildrenNumber;
                error = error + " | BabiesNumber =  " + BabiesNumber;
            }
            catch
            {
                
            }
            return error;
        }

    }
}

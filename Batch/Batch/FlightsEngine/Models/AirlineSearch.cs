using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class AirlineSearch
    {
        public AirlineSearch()
        {

        }


        public string FromAirportCode { get; set; }

        public string ToAirportCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int MaxStopsNumber { get; set; }

        public int AdultsNumber { get; set; }

        public int ChildrenNumber { get; set; }

        public int BabiesNumber { get; set; }


        public bool Return { get; set; }

        public string ToSpecialString()
        {
            string error = " FILTER : ";
            try
            {
                error = error + " FromAirportCode =  " + FromAirportCode ?? "[NULL]";
                error = error + " | ToAirportCode =  " + ToAirportCode ?? "[NULL]";
                error = error + " | Return =  " + BabiesNumber;
                error = error + " | FromDate =  " + (FromDate.HasValue ? FromDate.ToString() : "[NULL]");
                error = error + " | ToDate =  " + (ToDate.HasValue ? FromDate.ToString() : "[NULL]");
                error = error + " | MaxStopsNumber =  " + MaxStopsNumber;
                error = error + " | AdultsNumber =  " + AdultsNumber;
                error = error + " | ChildrenNumber =  " + ChildrenNumber;
                error = error + " | BabiesNumber =  " + BabiesNumber;
            }
            catch (Exception ex)
            {
                FlightsEngine.Utils.Logger.GenerateInfo("Error while creating the string of the filter : "+ex.Message);
            }
            return error;
        }

    }
}

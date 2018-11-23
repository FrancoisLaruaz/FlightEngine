using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Weather
{
    public class HistoricWeatherItem
    {

        public HistoricWeatherItem()
        {

        }

        public double latitude { get; set; }
        public double longitude { get; set; }
        public string timezone { get; set; }
        public Daily daily { get; set; }
        public double offset { get; set; }
    }
}

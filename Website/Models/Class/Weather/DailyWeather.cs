using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Weather
{
    public class Daily
    {
        public List<DataWeather> data { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Localization
{
    public class LocalizationItem
    {
        public LocalizationItem()
        {

        }

        public LocalizationItem(double _numLat,double _numLon,string _city,string _ipAddress)
        {
            city = _city;
            numLat = _numLat;
            numLon = _numLon;
            userHostAddress = _ipAddress;
        }

        public string userHostAddress { get; set; }

        public string status { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string regionName { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public string timezone { get; set; }
        public string isp { get; set; }
        public string org { get; set; }
        public string @as { get; set; }
        public string query { get; set; }

        public double numLat { get; set; }
        public double numLon { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Product
{
    public class ProductFilter
    {
        public ProductFilter()
        {

        }

         public int? ProductTypeId { get; set; }

        public int? ProductSubtypeId { get; set; }

        public string Address { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public int DistanceMax { get; set; }

    }
}

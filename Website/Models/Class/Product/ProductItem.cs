using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models.Class;

namespace Models.Class.Product
{
    public class ProductItem
    {
        public ProductItem()
        {

        }
        public int Id { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageSrc { get; set; }

        public string ProductTypeName { get; set; }

         public int ProductTypeId { get; set; }

        public string ProductSubtypeName { get; set; }

        public int ProductSubtypeId { get; set; }

        public int CreatorUserId { get; set; }

        public string Venue { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string CreatorUserName { get; set; }

        public string PostalCode { get; set; }

        public string Unit { get; set; }

        public string ProvinceName { get; set; }

        public string SubProvinceName { get; set; }

        public string CountryName { get; set; }

        public int? SubProvinceId { get; set; }
        public int? ProvinceId { get; set; }
        public int CountryId { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }
}

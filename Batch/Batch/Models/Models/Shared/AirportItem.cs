using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Shared
{
    public class AirportItem
    {
        public string Code { get; set; }

        public int Id { get; set; }

        public int CityId { get; set; }

        public int CountryId { get; set; }

        public int ContinentId { get; set; }

        public AirportItem()
        {

        }

        public AirportItem(string _Code,int _Id,int _CityId,int _CountryId,int _ContinentId)
        {
            Code = _Code;
            Id = _Id;
            CityId = _CityId;
            CountryId = _CountryId;
            ContinentId = _ContinentId;
        }

    }

}

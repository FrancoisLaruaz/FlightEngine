using Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class SearchTripWishesItem
    {
        public SearchTripWishesItem()
        {
            ProvidersToSearch = new List<Provider>();
        }

        public SearchTripWish _SearchTripWishes { get; set; }

        public List<Provider> ProvidersToSearch { get; set; }

    }
}

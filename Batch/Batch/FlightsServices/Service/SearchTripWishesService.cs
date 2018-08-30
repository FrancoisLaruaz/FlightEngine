using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Data.Repositories;
using FlightsEngine.Models;

namespace FlightsServices
{
    public class SearchTripWishesService : ISearchTripWishesService
    {

        private readonly IGenericRepository<SearchTripWish> _searchTripWishRepo;
        private readonly IGenericRepository<Provider> _providerRepo;

        public SearchTripWishesService(IGenericRepository<SearchTripWish> searchTripWishRepo,
            IGenericRepository<Provider> providerRepo)
        {
            _searchTripWishRepo = searchTripWishRepo;
            _providerRepo = providerRepo;
        }

        public SearchTripWishesService(TemplateEntities1 context)
        {
            _searchTripWishRepo = new GenericRepository<SearchTripWish>(context);
            _providerRepo = new GenericRepository<Provider>(context);
        }


        public SearchTripWishesItem GetSearchTripWishesById(int SearchTripWishesId)
        {
            SearchTripWishesItem result = new SearchTripWishesItem();
            try
            {
                result._SearchTripWishes = _searchTripWishRepo.Get(SearchTripWishesId);

                int FromCountryId = result._SearchTripWishes.FromAirport?.City?.CountryId ?? (result._SearchTripWishes.FromCity?.CountryId ?? 0);
                int ToCountryId = result._SearchTripWishes.ToAirport?.City?.CountryId ?? (result._SearchTripWishes.ToCity?.CountryId ?? 0);

                var Providers = _providerRepo.List().ToList();
                foreach(var provider in Providers)
                {
                    bool addProvider = false;
                    if(provider.IsSearchEngine)
                    {
                        addProvider = true;
                    }
                    else if(FromCountryId>0 &&ToCountryId>0)
                    {
                        addProvider = provider.Countries.Where(c => c.Id == FromCountryId).Any() && provider.Countries.Where(c => c.Id == ToCountryId).Any();
                    }

                    if(addProvider)
                    {
                        result.ProvidersToSearch.Add(provider);
                    }
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + SearchTripWishesId);
            }
            return result;
        }


    }
}

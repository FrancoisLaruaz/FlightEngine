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

        public SearchTripWishesService()
        {
            var context = new TemplateEntities1();
            _searchTripWishRepo = new GenericRepository<SearchTripWish>(context);
            _providerRepo = new GenericRepository<Provider>(context);
        }

        public bool DisableSearchTripWishes(int SearchTripWishesId)
        {
            bool result = false;
            try
            {
                var _SearchTripWishes = _searchTripWishRepo.Get(SearchTripWishesId);
                if (_SearchTripWishes != null)
                {
                    _SearchTripWishes.Active = false;
                    _searchTripWishRepo.Edit(_SearchTripWishes);
                    result = _searchTripWishRepo.Save();
                }

            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + SearchTripWishesId);
            }
            return result;
        }


        public List<SearchTripWishesItem> GetSearchTripWishesById(int? SearchTripWishesId, int? ProviderId)
        {
            List<SearchTripWishesItem> result = new List<SearchTripWishesItem>();
            try
            {
                List<SearchTripWish> SearchTripWishes = new List<SearchTripWish>();
                if (SearchTripWishesId == null)
                {
                    SearchTripWishes = _searchTripWishRepo.FindAllBy(w => w.Active).ToList();
                }
                else
                {
                    var _SearchTripWishes = _searchTripWishRepo.Get(SearchTripWishesId.Value);
                    if (_SearchTripWishes != null)
                    {
                        SearchTripWishes.Add(_SearchTripWishes);
                    }
                }

                // All or only API
                bool APIOnly = false;
                if (ProviderId < 0 || ProviderId == null)
                {
                    APIOnly = true;
                }

                var Providers = new List<Provider>();

                // Specific provider
                if (ProviderId > 0)
                {
                    Providers.Add(_providerRepo.FindAllBy(p => p.Active && p.Id == ProviderId.Value).FirstOrDefault());
                }
                else
                {
                    Providers = _providerRepo.FindAllBy(p => p.Active && (!APIOnly || p.HasAPI)).ToList();
                }

                foreach (var SearchTripWish in SearchTripWishes)
                {

                    SearchTripWishesItem item = new SearchTripWishesItem();

                    item._SearchTripWishes = SearchTripWish;

                    int FromCountryId = item._SearchTripWishes.FromAirport?.City?.CountryId ?? (item._SearchTripWishes.FromCity?.CountryId ?? 0);
                    int ToCountryId = item._SearchTripWishes.ToAirport?.City?.CountryId ?? (item._SearchTripWishes.ToCity?.CountryId ?? 0);


                    foreach (var provider in Providers)
                    {
                        bool addProvider = false;
                        if (provider.IsSearchEngine)
                        {
                            addProvider = true;
                        }
                        else
                        {
                            addProvider = provider.AirportsTrips.Where(a => (a.FromAirportId == SearchTripWish.FromAirportId && a.ToAirportId == SearchTripWish.ToAirportId) || (a.ToAirportId == SearchTripWish.FromAirportId && a.FromAirportId == SearchTripWish.ToAirportId)).Any();
                            if (!addProvider && FromCountryId > 0 && ToCountryId > 0)
                            {
                                addProvider = provider.Countries.Where(c => c.Id == FromCountryId).Any() || provider.Countries.Where(c => c.Id == ToCountryId).Any();
                            }
                        }

                        if (addProvider)
                        {
                            item.ProvidersToSearch.Add(provider);
                        }
                    }

                    if (item.ProvidersToSearch.Count > 0)
                    {
                        result.Add(item);
                    }
                }

            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + (SearchTripWishesId ?? -1) + " and  ProviderId = " + (ProviderId ?? -1));
            }
            return result;
        }


    }
}

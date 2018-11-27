using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Model;
using Data.Repositories;
using FlightsEngine.FlighsAPI;
using FlightsEngine.FlighsBot;
using FlightsEngine.Models;
using FlightsEngine.Models.Constants;
using FlightsEngine.Utils;
using FlightsServices;
using Models.Models.Shared;

namespace FlightsEngine
{
    public static class Program
    {






        public static bool SearchFlights(int? SearchTripWishesId, string ScrappingFolder, string FirefoxExeFolder,int? ProviderId=null)
        {
            bool result = false;
            try
            {

                var context = new TemplateEntities1();
                SearchTripWishesService _searchTripWishesService = new SearchTripWishesService(context);
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                TripsService _tripService = new TripsService(context);



                List<ProxyItem> Proxies =null;

                string lastSuccessfullProxy = null;

                List<SearchTripWishesItem> SearchTripWishesItems = _searchTripWishesService.GetSearchTripWishesById(SearchTripWishesId, ProviderId);
                foreach (var SearchTripWishesItem in SearchTripWishesItems)
                {
                    if (SearchTripWishesItem != null && SearchTripWishesItem._SearchTripWishes != null)
                    {
                        var SearchTripWishes = SearchTripWishesItem._SearchTripWishes;

                        #region Kiwi

                        APIAirlineSearch APIAirlineSearchItem = new APIAirlineSearch();
                        APIAirlineSearchItem.SearchTripWishesId = SearchTripWishes.Id;
                        if (SearchTripWishes.FromAirport != null)
                            APIAirlineSearchItem.FromAirportCode = SearchTripWishes.FromAirport.Code;
                        APIAirlineSearchItem.FromDateMax = SearchTripWishes.FromDateMax;
                        APIAirlineSearchItem.FromDateMin = SearchTripWishes.FromDateMin;
                        if (SearchTripWishes.ToAirport != null)
                            APIAirlineSearchItem.ToAirportCode = SearchTripWishes.ToAirport.Code;
                        if (SearchTripWishes.ToDateMin != null)
                        {
                            APIAirlineSearchItem.Return = true;
                            APIAirlineSearchItem.ToDateMin = SearchTripWishes.ToDateMin;
                            APIAirlineSearchItem.ToDateMax = SearchTripWishes.ToDateMax;
                        }
                        APIAirlineSearchItem.AdultsNumber = 1;
                        APIAirlineSearchItem.MaxStopsNumber = SearchTripWishes.MaxStopNumber;
                        APIAirlineSearchItem.DurationMin = SearchTripWishes.DurationMin;
                        APIAirlineSearchItem.DurationMax = SearchTripWishes.DurationMax;
                        APIAirlineSearchItem.MaxStopsNumber = SearchTripWishes.MaxStopNumber;

                        List<APIKey> AFKLMKeys = new List<APIKey>();
                        AFKLMKeys.Add(new APIKey("jqgd23tz7qk7u7vu6ayes2w3"));
                        AFKLMKeys.Add(new APIKey("e53xg7bdqnptwjxtzyh5zgmq"));

                        if (SearchTripWishesItem.ProvidersToSearch.Select(p => p.Id).Contains(Providers.Kiwi))
                        {
                            Kiwi.SearchFlights(APIAirlineSearchItem);
                        }
                        if (SearchTripWishesItem.ProvidersToSearch.Select(p => p.Id).Contains(Providers.AirFrance))
                        {
                            APIKey KeyToUse = AFKLMKeys.Where(k => k.RequestsNumber < 5000).OrderBy(k => k.RequestsNumber).FirstOrDefault();
                            if(KeyToUse!=null)
                                AFKLMKeys.Where(k => k.Key == KeyToUse.Key).FirstOrDefault().RequestsNumber = KeyToUse.RequestsNumber + FlightsEngine.FlighsAPI.AirFranceKLM.SearchFlights(APIAirlineSearchItem, Providers.AirFrance, KeyToUse.Key);
                        }
                        if (SearchTripWishesItem.ProvidersToSearch.Select(p => p.Id).Contains(Providers.KLM))
                        {
                            APIKey KeyToUse = AFKLMKeys.Where(k => k.RequestsNumber < 5000).OrderBy(k => k.RequestsNumber).FirstOrDefault();
                            if (KeyToUse != null)
                                AFKLMKeys.Where(k => k.Key == KeyToUse.Key).FirstOrDefault().RequestsNumber = KeyToUse.RequestsNumber + FlightsEngine.FlighsAPI.AirFranceKLM.SearchFlights(APIAirlineSearchItem, Providers.KLM, KeyToUse.Key);
                        }
                        #endregion kiwi

                        result = true;
                        bool newProxy = true;
                        foreach (var searchTrip in SearchTripWishes.SearchTrips)
                        {

                            try
                            {
                                AirlineSearch filter = new AirlineSearch();
                                if (SearchTripWishes.FromAirport != null)
                                    filter.FromAirportCode = SearchTripWishes.FromAirport.Code;

                                filter.FromDate = searchTrip.FromDate;
                                if (SearchTripWishes.ToAirport != null)
                                    filter.ToAirportCode = SearchTripWishes.ToAirport.Code;
                                if (searchTrip.ToDate != null)
                                {
                                    filter.Return = true;
                                    filter.ToDate = searchTrip.ToDate.Value;
                                }
                                filter.AdultsNumber = 1;
                                filter.MaxStopsNumber = SearchTripWishes.MaxStopNumber;



                                foreach (var provider in SearchTripWishesItem.ProvidersToSearch)
                                {
                                    if (!provider.HasAPI)
                                    {

                                        string Proxy = lastSuccessfullProxy;
                                        if (String.IsNullOrWhiteSpace(Proxy))
                                        {
                                            if (Proxies == null)
                                            {
                                                Proxies = ProxyHelper.GetProxies();
                                            }

                                            Proxy = ProxyHelper.GetBestProxy(Proxies);
                                            if (Proxy == null)
                                            {
                                                Proxies = ProxyHelper.GetProxies();
                                                Proxy = ProxyHelper.GetBestProxy(Proxies);
                                            }
                                            newProxy = true;
                                        }

                                        ScrappingSearch scrappingSearch = new ScrappingSearch();
                                        if (provider.Id == Providers.Edreams)
                                        {
                                            scrappingSearch.Url = ScrappingHelper.GetEdreamsUrl(filter);
                                        }
                                        else if (provider.Id == Providers.Kayak)
                                        {
                                            scrappingSearch.Url = ScrappingHelper.GetKayakUrl(filter);
                                        }
                                        int SearchTripProviderId = _searchTripProviderService.InsertSearchTripProvider(provider.Id, searchTrip.Id, Proxy, scrappingSearch.Url);
                                        if (!String.IsNullOrWhiteSpace(scrappingSearch.Url) && SearchTripProviderId > 0)
                                        {

                                            scrappingSearch.Proxy = Proxy;
                                            scrappingSearch.FirefoxExeFolder = FirefoxExeFolder;
                                            scrappingSearch.ScrappingFolder = ScrappingFolder;
                                            scrappingSearch.NewProxy = newProxy;
                                            if (SearchTripProviderId > 0 && !String.IsNullOrWhiteSpace(Proxy))
                                            {
                                                filter.SearchTripProviderId = SearchTripProviderId;
                                                scrappingSearch.Provider = provider.Name;
                                                scrappingSearch.ProxiesList = Proxies;

                                                var ScrappingResult = ScrappingHelper.SearchViaScrapping(scrappingSearch, filter.SearchTripProviderId);
                                                Proxies = ScrappingResult.ProxiesList;
                                                _searchTripProviderService.SetSearchTripProviderAsEnded(SearchTripProviderId, ScrappingResult.Success, ScrappingResult.LastProxy, ScrappingResult.AttemptsNumber);
                                                result = result && ScrappingResult.Success;
                                                if (ScrappingResult.Success)
                                                {
                                                    lastSuccessfullProxy = ScrappingResult.LastProxy;
                                                    _tripService.InsertTrips(SearchTripProviderId);
                                                    newProxy = false;
                                                    //Task.Factory.StartNew(() => { _tripService.InsertTrips(SearchTripProviderId); });
                                                }
                                                else
                                                {
                                                    lastSuccessfullProxy = null;
                                                }
                                            }
                                            else
                                            {
                                                result = false;
                                            }
                                        }
                                        else
                                        {
                                            FlightsEngine.Utils.Logger.GenerateInfo("No url for SearchTripProviderId : " + SearchTripProviderId + " and provider = " + provider.Name);
                                        }
                                    }
                                    else
                                    {

                               
                                            int SearchTripProviderId = _searchTripProviderService.InsertSearchTripProvider(provider.Id, searchTrip.Id, null, null);
                                            filter.SearchTripProviderId = SearchTripProviderId;
                                        
                                    }
                                }

                                //  Task.Factory.StartNew(() => FlighsBot.PythonHelper.Run(filter, scrappingSearch));
                                // Console.WriteLine("Pythonresult = "+ Pythonresult.Success+" and Error = "+ (Pythonresult.Error??""));

                                //   FlighsBot.Kayak.SearchFlights(filter);
                                //   FlighsBot.Kayak.SearchFlights(filter);

                                //   FlightsEngine.FlighsAPI.AirFranceKLM.SearchFlights(filter);
                                //    FlightsEngine.FlighsAPI.AirHob.SearchFlights(filter);
                                //   FlightsEngine.FlighsAPI.Kiwi.SearchFlights(filter);
                                // FlightsEngine.FlighsAPI.RyanAir.SearchFlights(filter);
                                //  FlightsEngine.FlighsAPI.Transavia.SearchFlights(filter);

                            }
                            catch (Exception e2)
                            {
                                result = false;
                                FlightsEngine.Utils.Logger.GenerateError(e2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "searchTrip.Id = " + searchTrip.Id);
                            }
                        }
                    }
                }
        
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + (SearchTripWishesId ??-1) + " and ScrappingFolder = " + ScrappingFolder + " and FirefoxExeFolder = " + FirefoxExeFolder);
            }
            return result;
        }


    }
}

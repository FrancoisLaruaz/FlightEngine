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

namespace FlightsEngine
{
    public static class Program
    {






        public static bool SearchFlights(int SearchTripWishesId, string ScrappingFolder, string FirefoxExeFolder)
        {
            bool result = false;
            try
            {

                var context = new TemplateEntities1();
                SearchTripWishesService _searchTripWishesService = new SearchTripWishesService(context);
                SearchTripProviderService _searchTripProviderService = new SearchTripProviderService(context);
                TripsService _tripService = new TripsService(context);

                Task.Factory.StartNew(() => { _tripService.InsertTrips(428); });
                /*
                List<ProxyItem> Proxies =  ProxyHelper.GetProxies(); 

                string lastSuccessfullProxy = null;

                SearchTripWishesItem SearchTripWishesItem = _searchTripWishesService.GetSearchTripWishesById(SearchTripWishesId);
                if (SearchTripWishesItem != null && SearchTripWishesItem._SearchTripWishes != null)
                {
                    var SearchTripWishes = SearchTripWishesItem._SearchTripWishes;
                    result = true;
                    foreach (var searchTrip in SearchTripWishes.SearchTrips)
                    {

                        try
                        {
                            AirlineSearch filter = new AirlineSearch();
                            filter.SearchTripId = searchTrip.Id;
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
                                        Proxy = ProxyHelper.GetBestProxy(Proxies);
                                        if (Proxy == null)
                                        {
                                            Proxies = ProxyHelper.GetProxies();
                                            Proxy = ProxyHelper.GetBestProxy(Proxies);
                                        }
                                    }

                                    ScrappingSearch scrappingSearch = new ScrappingSearch();
                                    if (provider.Id == Providers.Edreams)
                                    {
                                       scrappingSearch.Url = FlighsBot.ScrappingHelper.GetEdreamsUrl(filter);
                                    }
                                    else if (provider.Id == Providers.Kayak)
                                    {
                                        scrappingSearch.Url = ScrappingHelper.GetKayakUrl(filter);
                                    }
                                    int SearchTripProviderId = _searchTripProviderService.InsertSearchTripProvider(provider.Id, searchTrip.Id, Proxy);
                                    if (!String.IsNullOrWhiteSpace(scrappingSearch.Url) && SearchTripProviderId>0)
                                    {

                                        scrappingSearch.Proxy = Proxy;
                                        scrappingSearch.FirefoxExeFolder = FirefoxExeFolder;
                                        scrappingSearch.ScrappingFolder = ScrappingFolder;
                                        if (SearchTripProviderId > 0)
                                        {
                                            scrappingSearch.SearchTripProviderId = SearchTripProviderId;
                                            scrappingSearch.Provider = provider.Name;
                                            scrappingSearch.ProxiesList = Proxies;

                                            var ScrappingResult = ScrappingHelper.SearchViaScrapping(scrappingSearch);
                                            Proxies = ScrappingResult.ProxiesList;
                                            _searchTripProviderService.SetSearchTripProviderAsEnded(SearchTripProviderId, ScrappingResult.Success, ScrappingResult.LastProxy, ScrappingResult.AttemptsNumber);
                                            result = result && ScrappingResult.Success;
                                            if (result)
                                            {
                                                lastSuccessfullProxy = ScrappingResult.LastProxy;
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
                                        FlightsEngine.Utils.Logger.GenerateInfo("No url fror SearchTripProviderId : " + SearchTripProviderId+" and provider = "+ provider.Name);
                                    }
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
              */
            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + SearchTripWishesId.ToString() + " and ScrappingFolder = " + ScrappingFolder + " and FirefoxExeFolder = " + FirefoxExeFolder);
            }
            return result;
        }


    }
}

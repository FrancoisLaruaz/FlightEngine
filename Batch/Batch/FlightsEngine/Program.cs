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
using FlightsServices;

namespace FlightsEngine
{
    public static class Program
    {






        public static  bool SearchFlights(int SearchTripWishesId, string MainPythonScriptPath,string PythonPath)
        {
            bool result = false;
            try
            {

                var context = new TemplateEntities1();
                SearchTripWishesService _searchTripWishesService = new SearchTripWishesService(context);

                List<ProxyItem> Proxies = ProxyHelper.GetProxies();


                SearchTripWishesItem SearchTripWishesItem = _searchTripWishesService.GetSearchTripWishesById(SearchTripWishesId);
                if (SearchTripWishesItem != null && SearchTripWishesItem._SearchTripWishes!=null)
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

                            if (Proxies != null && Proxies.Count > 0)
                            {
                                string Proxy = ProxyHelper.GetBestProxy(Proxies);
                                if (Proxy == null)
                                {
                                    Proxies = ProxyHelper.GetProxies();
                                    Proxy = ProxyHelper.GetBestProxy(Proxies);
                                }



                                ScrappingSearch scrappingSearch = new ScrappingSearch();
                                scrappingSearch.Proxy = Proxy;
                                scrappingSearch.PythonPath = PythonPath;
                                scrappingSearch.MainPythonScriptPath = MainPythonScriptPath;
                                scrappingSearch.SearchTripProviderId = 1;
                                scrappingSearch.Provider = "Edreams";
                                scrappingSearch.ProxiesList = Proxies;

                                var ScrappingResult = FlighsBot.PythonHelper.SearchViaScrapping(filter, scrappingSearch);
                                Proxies = ScrappingResult.ProxiesList;

                                result = result && ScrappingResult.Success;
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
            catch(Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = "+  SearchTripWishesId.ToString()+ " and MainPythonScriptPath = "+ MainPythonScriptPath+ " and PythonPath = "+ PythonPath);
            }
            return result;
        }


    }
}

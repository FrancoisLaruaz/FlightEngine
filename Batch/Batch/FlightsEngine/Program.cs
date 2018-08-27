using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsEngine.FlighsAPI;
using FlightsEngine.FlighsBot;
using FlightsEngine.Models;

namespace FlightsEngine
{
    public static class Program
    {
        public static  bool SearchFlights(int? SearchTripId,string MainPythonScriptPath,string PythonPath)
        {
            bool result = false;
            try
            {
                AirlineSearch filter1 = new AirlineSearch();
                filter1.FromAirportCode = "AMS";
                filter1.FromDate = new DateTime(2018, 10, 20);
                filter1.ToAirportCode = "BCN";
                filter1.Return = true;
                filter1.ToDate = new DateTime(2018, 10, 28);
                filter1.AdultsNumber = 1;
                filter1.MaxStopsNumber = 2;

                List<ProxyItem> Proxies = ProxyHelper.GetProxies();
                if (Proxies != null && Proxies.Count > 0)
                {
                    // https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt
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

                    var ScrappingResult=FlighsBot.PythonHelper.SearchViaScrapping(filter1, scrappingSearch);
                    Proxies = ScrappingResult.ProxiesList;

                    result = ScrappingResult.Success;
          
                }
              //  Task.Factory.StartNew(() => FlighsBot.PythonHelper.Run(filter1, scrappingSearch));
                // Console.WriteLine("Pythonresult = "+ Pythonresult.Success+" and Error = "+ (Pythonresult.Error??""));

                //   FlighsBot.Kayak.SearchFlights(filter1);
                //   FlighsBot.Kayak.SearchFlights(filter1);

                //   FlightsEngine.FlighsAPI.AirFranceKLM.SearchFlights(filter1);
                //    FlightsEngine.FlighsAPI.AirHob.SearchFlights(filter1);
                //   FlightsEngine.FlighsAPI.Kiwi.SearchFlights(filter1);
                // FlightsEngine.FlighsAPI.RyanAir.SearchFlights(filter1);
                //  FlightsEngine.FlighsAPI.Transavia.SearchFlights(filter1);
            }
            catch(Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripId = "+ ( SearchTripId==null?"NULL" : SearchTripId.ToString())+ " and MainPythonScriptPath = "+ MainPythonScriptPath+ " and PythonPath = "+ PythonPath);
            }
            return result;
        }


    }
}

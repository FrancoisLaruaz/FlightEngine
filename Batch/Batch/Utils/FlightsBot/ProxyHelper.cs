using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using FlightsEngine.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;

namespace FlightsEngine.Utils
{
    public static class ProxyHelper
    {
        // https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt


        //  public static List<string> CountriesToAvoid = new List<string>() { "CA", "US", "FR", "JP" ,"GB" ,"SE" ,"NO", "NE"};
        public static List<string> CountriesToAvoid = new List<string>() {  };
        public static List<ProxyItem>  GetProxies()
        {
            List<ProxyItem> result = new List<ProxyItem>();
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  START Proxy Helper **");
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead("https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt");
                StreamReader reader = new StreamReader(stream);
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if( !String.IsNullOrWhiteSpace(line) && !line.StartsWith("Proxy") && !line.StartsWith("Mirrors")  && !line.StartsWith("IP") && !line.StartsWith("Free"))
                    {
                        string[] tabProxy = line.Split(' ');
                        ProxyItem item = new ProxyItem();
                        item.Proxy = line;
                        item.CountryCode= tabProxy[1].Substring(0,2);
                        item.CountryToAvoid = CountriesToAvoid.Contains(item.CountryCode);
                        result.Add(item);
                    }
                }
                reader.Close();


            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " **  END Proxy Helper **");
            return result;
        }

        public static string GetBestProxy(List<ProxyItem> Proxies)
        {
            string result = null;
            try
            {
                if (Proxies.Count > 0)
                {
                    int nbAttempts = Proxies.Sum(p => p.UseNumber);
                    if (nbAttempts <200)
                    {


                        List<ProxyItem> BaseList = null;
                        List<ProxyItem> ProxiesWithNoFailure = Proxies.FindAll(p => p.Failure == 0);
                        if (ProxiesWithNoFailure != null && ProxiesWithNoFailure.Count > 0)
                        {
                            List<ProxyItem> ProxiesWithNoFailureAndNotCountryToAvoid = ProxiesWithNoFailure.FindAll(p => p.CountryToAvoid == false);
                            if (ProxiesWithNoFailureAndNotCountryToAvoid != null && ProxiesWithNoFailure.Count > 0)
                            {
                                BaseList = ProxiesWithNoFailureAndNotCountryToAvoid;
                            }
                            else
                            {
                                BaseList = ProxiesWithNoFailure;
                            }
                            IEnumerable<ProxyItem> IEnumerableProxies = (IEnumerable<ProxyItem>)BaseList;
                            IEnumerableProxies= IEnumerableProxies.OrderByDescending(x => x.UseNumber).ThenBy(x => Guid.NewGuid());
                            BaseList = IEnumerableProxies.ToList();
                        }
                        else
                        {
                            BaseList = Proxies;
                            IEnumerable<ProxyItem> IEnumerableProxies = (IEnumerable<ProxyItem>)BaseList;
                            IEnumerableProxies = IEnumerableProxies.OrderBy(x => x.Failure).ThenBy(x => Guid.NewGuid());
                            BaseList = IEnumerableProxies.ToList();
                        }
  
                        result = BaseList[0].Proxy;
                    }
                }
            }
            catch (Exception e)
            {
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return result;
        }

    }
}
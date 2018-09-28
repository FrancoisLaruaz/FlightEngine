using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class ProxyItem
    {
        public ProxyItem()
        {

        }

        public bool CountryToAvoid { get; set; }
        public string Proxy { get; set; }

        public string CountryCode { get; set; }

        public int UseNumber { get; set; }

        public int Failure { get; set; }


        public string Port
        {
            get
            {
                try
                {
                    if(!String.IsNullOrWhiteSpace(Proxy))
                    {
                        return Proxy.Split(':')[1];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ProxyItem , Port ERROR : " + e.ToString());
                }
                return null;
            }
        }

        public string Host
        {
            get
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(Proxy))
                    {
                        return Proxy.Split(':')[0];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("ProxyItem , Host ERROR : " + e.ToString());
                }
                return null;
            }
        }

    }
}

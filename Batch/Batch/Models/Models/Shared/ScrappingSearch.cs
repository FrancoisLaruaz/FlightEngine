using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class ScrappingSearch
    {
        public ScrappingSearch()
        {
            ProxiesList = new List<ProxyItem>();
        }

        public List<ProxyItem> ProxiesList { get; set; }

        public string Proxy { get; set; }

        public string Provider { get; set; }

        public string MainPythonScriptPath { get; set; }

        public string PythonPath { get; set; }


        public int SearchTripProviderId { get; set; }


    }
}

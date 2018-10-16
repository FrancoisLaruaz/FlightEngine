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

        public string Url { get; set; }

        public string Provider { get; set; }

        public string ScrappingFolder { get; set; }

        public string ScrappingPreparationScript { get { return ScrappingFolder + "\\PrepareScrapping.cmd"; } }

        public string ScrappingExeScript { get { return ScrappingFolder + "\\Scrapper.exe"; } }

        public string FirefoxExeFolder { get; set; }


        public int SearchTripProviderId { get; set; }

        public bool NewProxy { get; set; }


    }
}

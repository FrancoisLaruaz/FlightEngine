using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class ScrappingExecutionResult
    {
        public ScrappingExecutionResult()
        {

            ProxiesList = new List<ProxyItem>();
        }

        public List<ProxyItem> ProxiesList { get; set; }


        public int AttemptsNumber { get; set; }

        public string Error { get; set; }

        public bool Success { get; set; }

        public string LastProxy { get; set; }


    }
}

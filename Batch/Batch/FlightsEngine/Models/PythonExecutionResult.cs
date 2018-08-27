using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightsEngine.Models
{
    public class PythonExecutionResult
    {
        public PythonExecutionResult()
        {

            ProxiesList = new List<ProxyItem>();
        }

        public List<ProxyItem> ProxiesList { get; set; }

        public int FoundTripsNumber { get; set; }

        public string Error { get; set; }

        public bool Success { get; set; }


    }
}

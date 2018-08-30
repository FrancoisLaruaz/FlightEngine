using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using FlightsEngine.Models;

namespace FlightsServices
{
    public interface ISearchTripProviderService
    {
        int SetSearchTripProviderAsEnded(int SearchTripProviderId, bool Success, string LastProxy, int AttemptsNumber);

        int InsertSearchTripProvider(int ProviderId,int SearchTripId,string Proxy);

    }
}

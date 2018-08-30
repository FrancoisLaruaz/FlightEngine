using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Data.Repositories;
using FlightsEngine.Models;

namespace FlightsServices
{
    public class SearchTripProviderService : ISearchTripProviderService
    {

        private readonly IGenericRepository<SearchTripProvider> _searchTripProviderRepo;


        public SearchTripProviderService(IGenericRepository<SearchTripProvider> searchTripProviderRepo)
        {
            _searchTripProviderRepo = searchTripProviderRepo;
        }

        public SearchTripProviderService(TemplateEntities1 context)
        {
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
        }

        public int SetSearchTripProviderAsEnded(int SearchTripProviderId, bool Success,string LastProxy,int AttemptsNumber)
        {
            int result = -1;
            try
            {
                SearchTripProvider item = _searchTripProviderRepo.Get(SearchTripProviderId);
                if(item!=null)
                {
                    item.SearchSuccess = Success;
                    item.EndSearchDate = DateTime.UtcNow;
                    item.Proxy = LastProxy;
                    item.AttemptsNumber = AttemptsNumber;
                    _searchTripProviderRepo.Edit(item);
                    _searchTripProviderRepo.Save();
                }

            }
            catch (Exception e)
            {
                result = -1;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripProviderId = " + SearchTripProviderId + " and Success = " + Success);
            }
            return result;
        }

        public int InsertSearchTripProvider(int ProviderId, int SearchTripId, string Proxy)
        {
            int result = -1;
            try
            {
                SearchTripProvider item = new SearchTripProvider();
                item.SearchSuccess = false;
                item.CreationDate = DateTime.UtcNow;
                item.Proxy = Proxy;
                item.ProviderId = ProviderId;
                item.SearchTripId = SearchTripId;
                _searchTripProviderRepo.Add(item);
                if (_searchTripProviderRepo.Save())
                {
                    result = item.Id;
                }

            }
            catch (Exception e)
            {
                result = -1;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "ProviderId = " + ProviderId+ " and SearchTripId = "+ SearchTripId+ " and Proxy = "+ (Proxy?? ""));
            }
            return result;
        }


    }
}

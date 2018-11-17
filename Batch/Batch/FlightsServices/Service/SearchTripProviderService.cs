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

        private  IGenericRepository<SearchTripProvider> _searchTripProviderRepo;
        private IGenericRepository<SearchTrip> _searchTripRepo;


        public SearchTripProviderService(IGenericRepository<SearchTripProvider> searchTripProviderRepo,
            IGenericRepository<SearchTrip> searchTripRepo)
        {
            _searchTripProviderRepo = searchTripProviderRepo;
            _searchTripRepo = searchTripRepo;
        }

        public SearchTripProviderService(TemplateEntities1 context)
        {
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _searchTripRepo = new GenericRepository<SearchTrip>(context);
        }

        public SearchTripProviderService()
        {
            var context = new TemplateEntities1();
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _searchTripRepo = new GenericRepository<SearchTrip>(context);
        }

        public int GetSearchTripProviderId(DateTime FromDate, DateTime? ToDate,int SearchTripWishesId,int ProviderId)
        {
            int result = -1;
            try
            {
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);
                if(ToDate!=null)
                    ToDate = new DateTime(ToDate.Value.Year, ToDate.Value.Month, ToDate.Value.Day);

                var SearchTrip = _searchTripRepo.FindAllBy(s => s.SearchTripWishesId== SearchTripWishesId && s.FromDate==FromDate && s.ToDate==s.ToDate).FirstOrDefault();
                if(SearchTrip==null)
                {
                    SearchTrip = new SearchTrip();
                    SearchTrip.SearchTripWishesId = SearchTripWishesId;
                    SearchTrip.ToDate = ToDate;
                    SearchTrip.FromDate = FromDate;
                    _searchTripRepo.Add(SearchTrip);
                   if(!_searchTripRepo.Save())
                    {
                        SearchTrip = null;
                    }
                }

                if(SearchTrip!=null)
                {
                    DateTime CutOff = DateTime.UtcNow.AddHours(-1);
                    var SearchTripProvider = _searchTripProviderRepo.FindAllBy(s => s.ProviderId==ProviderId && s.SearchTripId== SearchTrip.Id && s.CreationDate> CutOff).FirstOrDefault();
                    if(SearchTripProvider==null)
                    {
                        SearchTripProvider = new SearchTripProvider();
                        SearchTripProvider.AttemptsNumber = 1;
                        SearchTripProvider.CreationDate = DateTime.UtcNow;
                        SearchTripProvider.SearchTripId = SearchTrip.Id;
                        SearchTripProvider.SearchSuccess = true;
                        SearchTripProvider.ProviderId = ProviderId;
                        SearchTripProvider.EndSearchDate = DateTime.UtcNow;

                        _searchTripProviderRepo.Add(SearchTripProvider);
                        if(_searchTripProviderRepo.Save())
                        {
                            result= SearchTripProvider.Id;
                        }
                    }
                    else
                    {
                        result = SearchTripProvider.Id;
                    }
                }
            }
            catch (Exception e)
            {
                result = -1;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripWishesId = " + SearchTripWishesId + " and FromDate = " + FromDate.ToString()+ " and ToDate= "+(ToDate==null?"NULL":ToDate.Value.ToString()));
            }
            return result;
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

        public int InsertSearchTripProvider(int ProviderId, int SearchTripId, string Proxy=null,string Url=null)
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
                item.Url = Url;
                if(_searchTripProviderRepo==null)
                {
                    var context = new TemplateEntities1();
                    _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
                }
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

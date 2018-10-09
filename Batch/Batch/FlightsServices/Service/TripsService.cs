using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Data.Repositories;
using FlightsEngine.Models;
using FlightsEngine.Models.Constants;
using FlightsEngine.Utils;

namespace FlightsServices
{
    public class TripsService : ITripsService
    {

        private readonly IGenericRepository<SearchTripProvider> _searchTripProviderRepo;
        private readonly IGenericRepository<Trip> _tripRepo;

        public TripsService(IGenericRepository<SearchTripProvider> SearchTripProvider,
            IGenericRepository<Trip> tripRepo)
        {
            _searchTripProviderRepo = SearchTripProvider;
            _tripRepo = tripRepo;
        }

        public TripsService(TemplateEntities1 context)
        {
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _tripRepo = new GenericRepository<Trip>(context);
        }

        public TripsService()
        {
            var context = new TemplateEntities1();
            _searchTripProviderRepo = new GenericRepository<SearchTripProvider>(context);
            _tripRepo = new GenericRepository<Trip>(context);
        }


        public bool InsertTrips(int SearchTripProviderId)
        {
            bool result =false;
            try
            {
                var SearchTripProvider = _searchTripProviderRepo.Get(SearchTripProviderId);
                if(SearchTripProvider != null)
                {
                #region delete old records
                    // Delete old records
                    var TripsToDelete = SearchTripProvider.Trips.ToList();
                    foreach (var trip in TripsToDelete)
                    {
                        _tripRepo.Delete(trip);
                        _tripRepo.Save();
                    }
                    #endregion
                    string HtmlFile = "D:\\Html\\search_" + SearchTripProviderId + ".html";
                    if (File.Exists(HtmlFile))
                    {
                        string Html = File.ReadAllText(HtmlFile);
                        if (!String.IsNullOrWhiteSpace(Html))
                        {
                            if (SearchTripProvider.ProviderId == Providers.Kayak)
                            {
                                ScrappingHelper.GetKayakTripsFromHtml(Html, SearchTripProviderId);
                            }
                            if (result)
                            {
                                File.Delete(HtmlFile);
                            }
                        }
                        else
                        {
                            FlightsEngine.Utils.Logger.GenerateInfo("File empty for SearchTripProviderId = "+ SearchTripProviderId);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                FlightsEngine.Utils.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchTripProviderId = " + SearchTripProviderId);
            }
            return result;
        }


    }
}

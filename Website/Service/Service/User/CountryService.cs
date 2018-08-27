using Commons;
using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;


namespace Service.UserArea
{
    public  class CountryService : ICountryService
    {

        private readonly IGenericRepository<Country> _countryRepo;

        public CountryService(IGenericRepository<Country> countryRepo)
        {
            _countryRepo = countryRepo;
        }


        public  List<SelectionListItem> GetCountryList()
        {
            List<SelectionListItem> newList = new List<SelectionListItem>();
            try
            {
                List<Country> countries = GeListCountries();
                if (countries != null && countries.Count > 0)
                {
                    foreach (var item in countries)
                    {
                        newList.Add(new SelectionListItem(item.Id, item.Name));
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return newList;
        }

        public  List<Country> GeListCountries()
        {
            List<Country> result = null;
            try
            {
                result = _countryRepo.List().ToList();
            }
            catch (Exception e)
            {
                result = new List<Country>() ;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return result;
        }

    }
}

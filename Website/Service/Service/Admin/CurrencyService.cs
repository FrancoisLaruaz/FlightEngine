using Commons;

using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Service.Admin.Interface;
using Models.ViewModels.Admin.Logs;

namespace Service.Admin
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IGenericRepository<Currency> _currencyRepo;

        public CurrencyService(IGenericRepository<Currency> currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }

        public CurrencyService()
        {
            var context = new TemplateEntities();
            _currencyRepo = new GenericRepository<Currency>(context);
        }



        /// <summary>
        /// Delete old logs
        /// </summary>
        /// <returns></returns>
        public bool ConvertCurrency()
        {
            bool result = true;
            try
            {
                List<Currency> Currencies = _currencyRepo.List().OrderBy(x => Guid.NewGuid()).ToList();
                foreach (var currency in Currencies)
                {
                    try
                    {
                        if (currency.Code != "N/A" && currency.Code != CommonsConst.Const.DefaultCurrency)
                        {
                            decimal rate = CurrencyHelper.GetDefaultCurrencyConversionRate(currency.Code.ToUpper());
                            if (rate > 0)
                            {
                                currency.EuroConversationRate = rate;
                                _currencyRepo.Edit(currency);
                                result = result & _currencyRepo.Save();
                            }

                            System.Threading.Thread.Sleep(500);
                        }
                    }
                    catch (Exception e2)
                    {
                        result = false;
                        Commons.Logger.GenerateError(e2, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,"Currency Loop, Id = "+currency.Id);
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return result;
        }


    }
}

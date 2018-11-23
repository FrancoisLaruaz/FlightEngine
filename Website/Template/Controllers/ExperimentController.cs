using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models;
using Models.ViewModels;

using Service;
using Commons;
using i18n;
using System.Configuration;
using System.IO;
using Service.UserArea.Interface;
using Service.Admin.Interface;

namespace Website.Controllers
{
    public class ExperimentController : BaseController
    {
        private IEMailService _emailService;
        private IWeatherService _weatherService;

        public ExperimentController(
            IUserService userService,
            IEMailService emailService,
             IWeatherService weatherService
            ) : base(userService)
        {
            _emailService = emailService;
            _weatherService = weatherService;
        }
        public ActionResult Index()
        {

            try
            {
                _weatherService.SetAirportWeather();

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View();
        }





    }
}

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

namespace Website.Controllers
{
    public class ExperimentController : BaseController
    {
        private IEMailService _emailService;

        public ExperimentController(
            IUserService userService,
            IEMailService emailService
            ) : base(userService)
        {
            _emailService = emailService;
        }
        public ActionResult Index()
        {

            try
            {
                _emailService.SendEMailToUser("francois@frontfundr.com", CommonsConst.EmailTypes.UserWelcome);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View();
        }





    }
}

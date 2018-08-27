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


        public ExperimentController(
            IUserService userService
            ) : base(userService)
        {

        }
        public ActionResult Index()
        {

            try
            {


            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View();
        }





    }
}

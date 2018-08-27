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
using Models.Class;
using System.Web.Hosting;
using Service.UserArea.Interface;

namespace Website.Controllers
{
    public class CountryController : BaseController
    {
        private readonly IProvinceService _provinceService;

        public CountryController(
            IUserService userService, IProvinceService provinceService
            ) : base(userService)
        {
            _provinceService = provinceService;
        }

        public ActionResult GetProvinces(int? CountryId)
        {

            List<SelectionListItem> list = new List<SelectionListItem>();
            try
            {
                list = _provinceService.GetProvinceList(CountryId);
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "CountryId = " + CountryId);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}

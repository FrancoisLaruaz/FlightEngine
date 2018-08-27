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
using Models.ViewModels.Search;
using DataEntities.Model;
using Models.Class.Search;
using CommonsConst;
using Models.ViewModels.Browse;
using Models.Class.Product;
using Models.Class.Localization;
using Newtonsoft.Json;

namespace Website.Controllers
{
    public class BrowseController : BaseController
    {

        private readonly IProductService _productService;

        public BrowseController(
            IUserService userService,
             IProductService productService
            ) : base(userService)
        {
            _productService = productService;
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                setLastConnectionDate(User?.Identity?.Name, filterContext);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }


        [HttpGet]
        public ActionResult GetProducts(decimal longitude,decimal latitude,int distanceMax=5000)
        {
            List<ProductItem> _List = new List<ProductItem>();
            try
            {


            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "longitude = " + longitude + " and latitude = " + latitude);
            }
            return Json(new { List = _List });

        }


        [HttpGet]
        public ActionResult Index()
        {
            BrowseIndexViewModel model = new BrowseIndexViewModel();
            try
            {
                ViewBag.Title = "[[[Browse]]]";

                ProductFilter filter = new ProductFilter();

                LocalizationItem loc = DefaultUserLocalization;
                model.LocalizationJson= JsonConvert.SerializeObject(loc);
                model.ProductsListJson= JsonConvert.SerializeObject(_productService.GetProducts(filter));
                model.Language = WebsiteLanguage;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return View(model);

        }




    }
}

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

namespace Website.Controllers
{
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchService;

        public SearchController(
            IUserService userService, ISearchService searchService
            ) : base(userService)
        {
            _searchService = searchService;
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

        [HttpPost]
        public JsonResult GetSearchAutocomplete(string term)
        {

            try
            {
                int LoggedUserId = 0;
                if (User != null && User.Identity.IsAuthenticated)
                {
                    LoggedUserId = UserSession.UserId;
                }
                SearchFilter filter = new SearchFilter(HttpUtility.UrlDecode(term));
                var searchList = _searchService.GetSearch(filter, LoggedUserId, BrowserLanguage);

                return Json(searchList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "term = " + term);
            }

            return null;
        }




        [HttpPost]
        public ActionResult SearchItemClicked(int SearchId, string Url)
        {
            bool _success = false;
            string _Error = "";
            try
            {

                _success = _searchService.SetUrlClickedForSearch(SearchId, Url);
            }
            catch (Exception e)
            {
                _success = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "SearchId = " + SearchId + " and Url = " + Url);
            }
            return Json(new { Result = _success, Error = _Error });
        }


        [HttpGet]
        public ActionResult Index(string pattern = "")
        {
            SearchIndexViewModel model = new SearchIndexViewModel();
            try
            {
                ViewBag.Title = "[[[Search]]]";
                model.Pattern = pattern;
                model.ShowUsers = true;
                model.ShowPages = true;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "pattern = " + pattern);
            }
            return View(model);

        }

        [HttpGet]
        public ActionResult SearchPages()
        {
            SearchIndexViewModel model = new SearchIndexViewModel();
            try
            {
                model.Pattern = "";
                model.ShowPages = true;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View("~/Views/Search/Index.cshtml", model);

        }

        [HttpGet]
        public ActionResult SearchUsers()
        {
            SearchIndexViewModel model = new SearchIndexViewModel();
            try
            {
                model.Pattern = "";
                model.ShowUsers = true;
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return View("~/Views/Search/Index.cshtml", model);

        }

        [HttpGet]
        public ActionResult _IndexSearchResult(SearchFilter filter)
        {
            try
            {
                SearchIndexResultViewModel model = new SearchIndexResultViewModel();
                int LoggedUserId = 0;
                if (User != null && User.Identity.IsAuthenticated)
                {
                    LoggedUserId = UserSession.UserId;
                }
                model = _searchService.GetSearchIndexResultViewModel(filter, LoggedUserId,BrowserLanguage);

                return PartialView(model);

            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "pattern = " + filter.Pattern);
            }
            return Content(PartialViewResults.UnknownError);
        }


    }
}

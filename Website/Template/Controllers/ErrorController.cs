using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commons;
using Service.UserArea.Interface;

namespace Website.Controllers
{
    public class ErrorController : BaseController
    {
    

        public ErrorController(
            IUserService userService
            ) : base(userService)
        {

        }

        public ActionResult Index()
        {
            ViewBag.Title = "[[[Error]]]";
            return View();
        }

        public ActionResult DisplayError(string Message)
        {
            ViewBag.ErrorType = Message;
            ViewBag.Title = "[[[Error]]]";
            return View("~/Views/Error/Index.cshtml");
        }

        public ActionResult Error404()
        {
            ViewBag.Title = "[[[Error]]]";
            return View("~/Views/Error/Error404.cshtml");
        }


        public JsonResult LogJavascriptError(string errorMsg="", string url = "", string lineNumber = "", string col = "", string error = "", string browser = "", bool custom=false)
        {
            bool _success = true;

            try
            {
                if (errorMsg == null)
                    errorMsg = "";
                if (url == null)
                    url = "";
                if (lineNumber == null)
                    lineNumber = "";
                if (col == null)
                    col = "";
                if (error == null)
                    error = "";
                if (browser == null)
                    browser = "";
                Logger.GenerateJavascriptError(errorMsg, url, lineNumber, col, error, browser, custom);
            }
            catch(Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "errorMsg = "+ errorMsg+ " and url = "+ url);
            }
            return Json(new { success = _success }, JsonRequestBehavior.AllowGet);
        }



    }
}

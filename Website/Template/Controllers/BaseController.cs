using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using i18n;
using Commons;
using Service;
using Microsoft.AspNet.Identity;

using Models.Class;
using System.Web.Script.Serialization;
using System.Reflection;
using CommonsConst;
using Service.UserArea.Interface;

namespace Website.Controllers
{
    public class BaseController : Controller
    {

        protected readonly IUserService _userService;

        public BaseController(
            IUserService userService
            )
        {
            _userService = userService;
        }

        public  string CurrentLangTag
        {
            get
            {
                try
                {
                    return Request.RequestContext.HttpContext.GetPrincipalAppLanguageForRequest().GetLanguage()?? CommonsConst.Const.DefaultCulture;
                }
                catch
                {
                    return CommonsConst.Const.DefaultCulture;
                }
            }
        }

        protected string JsonConstants
        {
            get
            {
                if (Session[CommonsConst.Const.JsonConstantsSession] == null)
                {
                    var constants = CommonsConst.ConstantsHelper.GetJSonConstants();
                    Session[CommonsConst.Const.JsonConstantsSession] = new JavaScriptSerializer().Serialize(constants);
                }
                return Session[CommonsConst.Const.JsonConstantsSession].ToString();
            }

            set
            {
                Session[CommonsConst.Const.JsonConstantsSession] = value;
            }
        }


        public void setLastConnectionDate(string userName, ActionExecutingContext filterContext)
        {
            try
            {

                if (!String.IsNullOrWhiteSpace(userName) && LastConnectionDate != null && LastConnectionDate.AddHours(1) < DateTime.UtcNow)
                {
                    if (_userService.SetUserLastConnectionDate(userName))
                    {
                        LastConnectionDate = DateTime.UtcNow;
                    }
                }
            }
            catch (Exception ex)
            {
                Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userName = "+ userName);
            }
        }

        protected DateTime LastConnectionDate
        {
            get
            {
                if (Session[CommonsConst.Const.LastConnectionDate] == null)
                {
                    UserSession ConnectedUser = UserSession;
                }
                try
                {
                    if (Session[CommonsConst.Const.LastConnectionDate] == null) return DateTime.UtcNow;

                    string _lastConnectionDate = Session[CommonsConst.Const.LastConnectionDate].ToString();
                    if (string.IsNullOrWhiteSpace(_lastConnectionDate)) return DateTime.UtcNow;

                    return DateTime.Parse(_lastConnectionDate);
                }
                catch (Exception ex)
                {
                    Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                    return DateTime.UtcNow;
                }
            }

            set
            {
                Session[CommonsConst.Const.LastConnectionDate] = value;
            }
        }

        /// <summary>
        /// Get the id of the authenticated user
        /// </summary>
        public UserSession UserSession
        {
            get
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return null;
                    }
                    else
                    {
                        if (Session[CommonsConst.Const.UserSession] == null)
                        {
                            UserSession ConnectedUser = _userService.GetUserSession(User.Identity.GetUserName());
                            if(ConnectedUser==null)
                            {
                                return null;
                            }
                            else
                            {
                                Session[CommonsConst.Const.UserSession] = ConnectedUser;
                                LastConnectionDate = ConnectedUser.DateLastConnection;
                                return ConnectedUser;
                            }
                        }
                        else
                        {
                            UserSession ConnectedUser = Session[CommonsConst.Const.UserSession] as UserSession;
                            LastConnectionDate = ConnectedUser.DateLastConnection;
                            return ConnectedUser;
                        }
                    }

                }
                catch(Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "User.Identity.IsAuthenticated = "+ User.Identity.IsAuthenticated + " and User.Identity.GetUserName() = "+ User.Identity.GetUserName());
                    return null;
                }
            }
            set
            {
                Session[CommonsConst.Const.UserSession] = value;
            }
        }


        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home",new { area=""});
            }
        }


        /// <summary>
        /// Send constants to javascript
        /// </summary>
        /// <returns></returns>
        public ActionResult Constants()
        {
            try {
                return JavaScript("var Constants = " + JsonConstants + ";");
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return null;
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);
                String ControllerName = filterContext.Controller.ToString();
                if (ControllerName.Contains("Website.Areas.Admin.Controllers") && filterContext.ActionDescriptor != null && filterContext.ActionDescriptor.ActionName != "Logs" && filterContext.ActionDescriptor.ActionName != "_DisplayLogs")
                {
                  
                    if (!User.Identity.IsAuthenticated)
                        filterContext.Result = RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri.ToString(), area = "" });
                    else if(!User.IsInRole(CommonsConst.UserRoles.Admin))
                        filterContext.Result = RedirectToAction("Index", "Home", new {  area = "" });
                    return;
                  
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

        }
    }
}

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
using Models.Class.Localization;
using System.Security.Claims;

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

        public string CurrentLangTag
        {
            get
            {
                try
                {
                    return Request.RequestContext.HttpContext.GetPrincipalAppLanguageForRequest().GetLanguage() ?? CommonsConst.Const.DefaultCulture;
                }
                catch
                {
                    return CommonsConst.Const.DefaultCulture;
                }
            }
        }

        protected string BrowserLanguage
        {
            get
            {
                string languageBrowser = CommonsConst.Const.DefaultCulture;
                try
                {
                    if (Request != null)
                    {
                        string[] languages = Request?.UserLanguages;
                        if (languages != null && languages.Length > 0)
                        {
                            string language = languages[0];
                            if (language.IndexOf("fr") > -1)
                            {
                                languageBrowser = "fr";
                            }
                        }
                    }
                }
                catch
                {

                }
                return languageBrowser;
            }
        }

        protected string JsonConstants
        {
            get
            {
                if (Session[CommonsConst.Const.JsonConstantsSession] == null)
                {
                    var constants = CommonsConst.ConstantsHelper.GetJSonConstants();
                    constants.Add("APIKeys", new Dictionary<string, object>() { { "GoogleMap", System.Configuration.ConfigurationManager.AppSettings["GoogleMapKey"].ToString() } });
                    constants.Add("Environment", new Dictionary<string, object>() { { "IsLocalhost", Utils.IsLocalhost() }, { "IsProduction", Utils.IsProductionWebsite() } });

                    var userDic = new Dictionary<string, object>() { };
                    List<object> roles = Utils.GetPropertiesList(typeof(UserRoles));
                    if (User.Identity.IsAuthenticated)
                    {
                        foreach (var role in roles)
                        {
                            if (User.IsInRole(role.ToString()))
                            {
                                userDic.Add("Is_" + role.ToString().Replace(" ", "").ToLower(), true);
                            }
                            else
                            {
                                userDic.Add("Is_" + role.ToString().Replace(" ", "").ToLower(), false);
                            }
                        }
                        userDic.Add("IsAuthenticated", true);
                        userDic.Add("UserName", User.Identity.Name);
                    }
                    else
                    {
                        foreach (var role in roles)
                        {
                            userDic.Add("Is_" + role.ToString().Replace(" ", "").ToLower(), false);
                        }
                        userDic.Add("IsAuthenticated", false);
                        userDic.Add("UserName", "");
                    }
                    constants.Add("User", userDic);

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
                Commons.Logger.GenerateError(ex, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userName = " + userName);
            }
        }

        public string WebsiteLanguage
        {
            get
            {
                if (Session[CommonsConst.Const.WebsiteLanguageSession] == null)
                {
                    return CommonsConst.Const.DefaultCulture;
                }

                try
                {
                    return Session[CommonsConst.Const.WebsiteLanguageSession].ToString();
                }
                catch
                {
                    return CommonsConst.Const.DefaultCulture;
                }
            }

            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    Session[CommonsConst.Const.WebsiteLanguageSession] = value.Split('-')[0];
                }
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


        public LocalizationItem DefaultUserLocalization
        {
            get
            {
                try
                {
                    if (Session[CommonsConst.Const.DefaultUserLocalization] == null)
                    {
                        LocalizationItem loc = GeoLocalizationHelper.GetLocalizationItemFromIpAddress(Request?.UserHostAddress);
                        DefaultUserLocalization = loc;
                        return loc;
                    }
                    else
                    {
                        LocalizationItem loc = Session[CommonsConst.Const.DefaultUserLocalization] as LocalizationItem;
                        if (Request?.UserHostAddress != null && loc.userHostAddress!=null && Request?.UserHostAddress == loc.userHostAddress)
                        {
                            return loc;
                        }
                        else
                        {
                            loc = GeoLocalizationHelper.GetLocalizationItemFromIpAddress(Request?.UserHostAddress);
                            DefaultUserLocalization = loc;
                            return loc;
                        }
                    }


                }
                catch (Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "User.Identity.IsAuthenticated = " + User.Identity.IsAuthenticated + " and Request?.UserHostAddress = " + (Request?.UserHostAddress ?? "NULL"));
                    return null;
                }
            }
            set
            {
                Session[CommonsConst.Const.DefaultUserLocalization] = value;
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
                            if (ConnectedUser == null)
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
                catch (Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "User.Identity.IsAuthenticated = " + User.Identity.IsAuthenticated + " and User.Identity.GetUserName() = " + User.Identity.GetUserName());
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
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }


        /// <summary>
        /// Send constants to javascript
        /// </summary>
        /// <returns></returns>
        public ActionResult Constants()
        {
            try
            {
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
                    else if (!User.IsInRole(CommonsConst.UserRoles.Admin))
                        filterContext.Result = RedirectToAction("Index", "Home", new { area = "" });
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

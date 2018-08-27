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
using Models.ViewModels.Home;
using Models.Class.Email;
using Models.ViewModels.Shared;


namespace Website.Controllers
{
    public class HomeController : BaseController
    {
        private IEMailService _emailService;

        public HomeController(
            IUserService userService,
            IEMailService emailService
            ) : base(userService)
        {
            _emailService=emailService;
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

        public ActionResult RefreshHeader()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    UserSession = _userService.GetUserSession(User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return RedirectToAction("_Header");
        }

        public ActionResult _Header()
        {
            HeaderViewModel model = new HeaderViewModel();
            try
            {

                if (User.Identity.IsAuthenticated)
                {
                    model.UserFirstName = UserSession.FirstName;
                    model.UserNameDecrypt = UserSession.UserName;
                    model.PictureThumbnailSrc = FileHelper.GetDecryptedFilePath(UserSession.PictureThumbnailSrc, true, true);
                    model.PictureThumbnailSrc = model.PictureThumbnailSrc.Replace("~", "");
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return PartialView("~/Views/Shared/Layout/_Header.cshtml", model);
        }

        #region contactUs
        [HttpGet]
        public ActionResult ContactUs()
        {
            ViewBag.Title = "[[[Contact Us]]]";
            return View(new ContactUsViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactUsViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            try
            {

                if (ModelState.IsValid)
                {
                    if (_emailService.IsEmailAddressValid(model.Email.Trim().ToLower()))
                    {

                        Email email = new Email();
                        email.Subject = "Contact : " + model.Subject + " from " + model.Name;
                        email.ToEmail = ConfigurationManager.AppSettings["ContactMail"];
                        email.EMailTypeId = CommonsConst.EmailTypes.Contact;
                        email.Comment = "'" + model.Subject + "' from " + model.Email;
                        email.EmailContent.Add(new Tuple<string, string>("#Subject#", model.Subject));
                        email.EmailContent.Add(new Tuple<string, string>("#Name#", model.Name));
                        email.EmailContent.Add(new Tuple<string, string>("#PhoneNumber#", model.PhoneNumber ?? ""));
                        email.EmailContent.Add(new Tuple<string, string>("#Question#", model.Question));
                        email.EmailContent.Add(new Tuple<string, string>("#Email#", model.Email));
                        _Result = _emailService.SendMail(email);
                    }
                    else
                    {
                        _Error = "[[[Please enter a valid email address.]]]";
                    }
                }
                else
                {
                    _Error = "[[[Sorry, an error has been detected in the form :(.]]]";
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _Result, Error = _Error.Trim() });
        }

        #endregion

        public ActionResult Index(bool SignUp = false,
            bool PromptLogin = false,
            string RedirectTo = "/")
        {
            var model = new HomeViewModel();
            try
            {
                ViewBag.ShowVideo = true;
                model.SignUp = SignUp;
                model.PromptLogin = PromptLogin;
                model.RedirectTo = RedirectTo;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View(model);
        }

        /// <summary>
        /// Action executed to change the language of the website
        /// </summary>
        /// <param name="langtag"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SetLanguage(string langtag, string returnUrl)
        {
            try
            {
                // If valid 'langtag' passed.
                i18n.LanguageTag lt = i18n.LanguageTag.GetCachedInstance(langtag);
                if (lt.IsValid())
                {
                    // Set persistent cookie in the client to remember the language choice.
                    Response.Cookies.Add(new HttpCookie(CommonsConst.Const.i18nlangtag)
                    {
                        Value = lt.ToString(),
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddYears(1)
                    });

                    if (User.Identity.IsAuthenticated)
                    {
                        _userService.UpdateLanguageUser(langtag, User.Identity.Name);
                        UserSession LoggedUser = UserSession;
                        LoggedUser.LanguageTag = langtag;
                        UserSession = LoggedUser;
                    }
                }
                // Owise...delete any 'language' cookie in the client.
                else
                {
                    var cookie = Response.Cookies[CommonsConst.Const.i18nlangtag];
                    if (cookie != null)
                    {
                        cookie.Value = null;
                        cookie.Expires = DateTime.UtcNow.AddMonths(-1);
                    }
                }
                // Update PAL setting so that new language is reflected in any URL patched in the 
                // response (Late URL Localization).
                System.Web.HttpContext.Current.SetPrincipalAppLanguageForRequest(lt);
                // Patch in the new langtag into any return URL.
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = LocalizedApplication.Current.UrlLocalizerForApp.SetLangTagInUrlPath(HttpContext, returnUrl, UriKind.RelativeOrAbsolute, lt == null ? null : lt.ToString()).ToString();
                    returnUrl = returnUrl.Replace(lt.ToString() + "?", lt.ToString() + "/Home?");
                }
                // Redirect user agent as approp.
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Lanuguage = " + langtag + " and url = " + returnUrl);
            }
            if (returnUrl != "")
                return this.Redirect(returnUrl);
            else
                return null;
        }
    }
}

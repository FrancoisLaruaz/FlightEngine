using Commons;
using Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Models.ViewModels;
using Models.Class;
using i18n;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Script.Serialization;
using Identity;
using System.Net;
using System.IO;
using Identity.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Models.Class.ExternalAuthentification;
using Facebook;
using CommonsConst;
using System.Security.Claims;
using Models.Class.FileUpload;
using Service.UserArea.Interface;
using Models.ViewModels.Account;
using Models.Class.SignUp;
using Commons.Attributes;
using System.Configuration;

namespace Website.Controllers
{
    public class AccountController : BaseController
    {

        private IEMailService _emailService;
        private IASPNetUsersService _aspNetUsersService;
        private ISocialMediaConnectionService _socialMediaConnectionService;

        public AccountController(
            IUserService userService,
            IEMailService emailService,
            IASPNetUsersService aspNetUsersService,
            ISocialMediaConnectionService socialMediaConnectionService
            ) : base(userService)
        {
            _emailService = emailService;
            _aspNetUsersService = aspNetUsersService;
            _socialMediaConnectionService = socialMediaConnectionService;
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IUserService userService, IEMailService emailService, IASPNetUsersService aspNetUsersService, ISocialMediaConnectionService socialMediaConnectionService) : base(userService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _emailService = emailService;
            _aspNetUsersService = aspNetUsersService;
            _socialMediaConnectionService = socialMediaConnectionService;
        }

        #region loginstuff
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_userManager != null)
                    {
                        _userManager.Dispose();
                        _userManager = null;
                    }

                    if (_signInManager != null)
                    {
                        _signInManager.Dispose();
                        _signInManager = null;
                    }
                }

                base.Dispose(disposing);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
        }




        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        private const string XsrfKey = "XsrfId";





        public class FacebookBackChannelHandler : HttpClientHandler
        {
            protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                try
                {
                    var result = await base.SendAsync(request, cancellationToken);
                    if (!request.RequestUri.AbsolutePath.Contains("access_token"))
                        return result;

                    // For the access token we need to now deal with the fact that the response is now in JSON format, not form values. Owin looks for form values.
                    var content = await result.Content.ReadAsStringAsync();
                    var facebookOauthResponse = JsonConvert.DeserializeObject<FacebookOauthResponse>(content);

                    var outgoingQueryString = HttpUtility.ParseQueryString(string.Empty);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.access_token), facebookOauthResponse.access_token);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.expires_in), facebookOauthResponse.expires_in + string.Empty);
                    outgoingQueryString.Add(nameof(facebookOauthResponse.token_type), facebookOauthResponse.token_type);
                    var postdata = outgoingQueryString.ToString();

                    var modifiedResult = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(postdata)
                    };

                    string accessToken = facebookOauthResponse.access_token;
                    FacebookAccessToken = accessToken;
                    return modifiedResult;
                }

                catch (Exception e)
                {
                    Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }
                return null;
            }
        }




        public static string FacebookAccessToken
        {
            get;
            set;
        }

        #endregion


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

        #region My Profile

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _MyProfileTrustAndVerifications(MyProfileTrustAndVerificationsViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        if (model.UserId > 0 && (UserSession.UserId == model.UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                        {

                        }
                        else
                        {
                            _Error = "[[[You don't have the rights to edit this user.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[Please log in to perform the action.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error });
        }

        public ActionResult _MyProfileTrustAndVerifications(int userId)
        {
            MyProfileTrustAndVerificationsViewModel model = new MyProfileTrustAndVerificationsViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserIdToCheck = UserSession.UserId;
                    if (userId > 0 && User.IsInRole(CommonsConst.UserRoles.Admin))
                    {
                        UserIdToCheck = userId;
                    }
                    model = _userService.GetMyProfileTrustAndVerificationsViewModel(userId);
                }
                else
                {
                    return Content("NotLoggedIn");
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId = " + userId);
                return Content("ERROR");
            }
            return PartialView("~/Views/Account/MyProfile/_MyProfileTrustAndVerifications.cshtml", model);
        }



        [HttpPost]
        public ActionResult UpdateMyProfilePicture(int UserId)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var portraitInputFile = Request.Files[i];
                    // legacy portrait image upload
                    if (portraitInputFile != null)
                    {
                        if (ModelState.IsValid)
                        {
                            if (User.Identity.IsAuthenticated)
                            {
                                if (UserId > 0 && (UserSession.UserId == UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                                {
                                    var fileName = Path.GetFileName(portraitInputFile.FileName);


                                    string ext = Path.GetExtension(fileName);
                                    fileName = FileHelper.GetFileName("UserPicture", ext);

                                    FileUpload newFile = new FileUpload()
                                    {
                                        File = portraitInputFile,
                                        UploadName = fileName,
                                        IsImage = true,
                                        EncryptFile = false
                                    };
                                    if (FileHelper.IsValidImage(portraitInputFile))
                                    {
                                        string PictureSrc = FileHelper.UploadFile(newFile);
                                        if (PictureSrc != "KO")
                                        {
                                            string PictureThumbnailSrc = "";
                                            _success = _userService.SaveMyProfilePhotos(UserId, PictureSrc, PictureThumbnailSrc);
                                            if (_success)
                                            {
                                                _success = _userService.CreateThumbnailUserPicture(UserId);
                                            }
                                        }
                                        else
                                        {
                                            _Error = "[[[An error occured while uploading the image.]]]";
                                        }
                                    }
                                    else
                                    {
                                        _Error = "[[[Please upload an image.]]]";
                                    }
                                }
                                else
                                {
                                    _Error = "[[[You don't have the rights to edit this user.]]]";
                                }
                            }
                            else
                            {
                                _Error = "[[[Please log in to perform the action.]]]";
                            }
                        }
                        else
                        {
                            _Error = "[[[An error occured while saving yhe profile. Please try again .]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[The file is empty. Please upload another file.]]]";
                    }
                }

            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _MyProfilePhotos(MyProfilePhotosViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _PreviewPath = "";
            try
            {

                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        if (model.UserId > 0 && (UserSession.UserId == model.UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                        {
                            _Result = _userService.SaveMyProfilePhotos(model.UserId, model.PictureSrc, model.PictureThumbnailSrc);
                            if (_Result)
                            {
                                _Result = _userService.CreateThumbnailUserPicture(model.UserId);
                                _PreviewPath = FileHelper.GetDecryptedFilePath(model.PictureSrc);
                            }
                        }
                        else
                        {
                            _Error = "[[[You don't have the rights to edit this user.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[Please log in to perform the action.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error, PreviewPath = _PreviewPath });
        }



        public ActionResult _MyProfilePhotos(int userId)
        {
            MyProfilePhotosViewModel model = new MyProfilePhotosViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserIdToCheck = UserSession.UserId;
                    if (userId > 0 && User.IsInRole(CommonsConst.UserRoles.Admin))
                    {
                        UserIdToCheck = userId;
                    }
                    model = _userService.GetMyProfilePhotosViewModel(userId);
                }
                else
                {
                    return Content("NotLoggedIn");
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId = " + userId);
                return Content("ERROR");
            }

            return PartialView("~/Views/Account/MyProfile/_MyProfilePhotos.cshtml", model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _MyProfileAddress(MyProfileAddressViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            try
            {
                if (model.ProvinceId == 0)
                {
                    model.ProvinceId = null;
                }

                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        if (model.UserId > 0 && (UserSession.UserId == model.UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                        {
                            _Result = _userService.SaveMyProfileAddress(model);
                        }
                        else
                        {
                            _Error = "[[[You don't have the rights to edit this user.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[Please log in to perform the action.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error });
        }

        public ActionResult _MyProfileAddress(int userId)
        {
            MyProfileAddressViewModel model = new MyProfileAddressViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserIdToCheck = UserSession.UserId;
                    if (userId > 0 && User.IsInRole(CommonsConst.UserRoles.Admin))
                    {
                        UserIdToCheck = userId;
                    }
                    model = _userService.GetMyProfileAddressViewModel(userId);
                }
                else
                {
                    return Content("NotLoggedIn");
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId = " + userId);
                return Content("ERROR");
            }
            return PartialView("~/Views/Account/MyProfile/_MyProfileAddress.cshtml", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _MyProfileEdit(MyProfileEditViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _LanguageRedirect = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        if (model.UserId > 0 && (UserSession.UserId == model.UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                        {


                            _Result = _userService.SaveMyProfileEdit(model);
                            if (_Result && UserSession.LanguageTag != CommonsConst.Languages.ToString(model.LanguageId) && UserSession.UserId == model.UserId)
                            {
                                _LanguageRedirect = CommonsConst.Languages.ToString(model.LanguageId);
                                UserSession = _userService.GetUserSession(User.Identity.Name);
                            }

                        }
                        else
                        {
                            _Error = "[[[You don't have the rights to edit this user.]]]";
                        }
                    }
                    else
                    {
                        _Error = "[[[Please log in to perform the action.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error, LanguageRedirect = _LanguageRedirect });
        }

        public ActionResult _MyProfileEdit(int userId)
        {
            MyProfileEditViewModel model = new MyProfileEditViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    int UserIdToCheck = UserSession.UserId;
                    if (userId > 0 && User.IsInRole(CommonsConst.UserRoles.Admin))
                    {
                        UserIdToCheck = userId;
                    }
                    model = _userService.GetMyProfileEditViewModel(userId);
                }
                else
                {
                    return Content("NotLoggedIn");
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId = " + userId);
                return Content("ERROR");
            }
            return PartialView("~/Views/Account/MyProfile/_MyProfileEdit.cshtml", model);
        }

        /// <summary>
        /// Delete a profile
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProfile(int UserId)
        {
            bool _success = false;
            try
            {
                if (User.Identity.IsAuthenticated && UserId > 0 && (UserSession.UserId == UserId || User.IsInRole(CommonsConst.UserRoles.Admin)))
                {
                    _success = _userService.DeleteUserById(UserId);
                    if (UserSession.UserId == UserId)
                    {
                        Session[CommonsConst.Const.UserSession] = null;
                        Session.Clear();
                        Session.Abandon();
                        AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    }
                }
            }
            catch (Exception e)
            {
                _success = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _success });
        }

        public ActionResult MyProfile(int? id = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    ViewBag.Title = "[[[My Profile]]]";
                    MyProfileViewModel model = new MyProfileViewModel();
                    int UserIdToCheck = UserSession.UserId;
                    if (id != null && User.IsInRole(CommonsConst.UserRoles.Admin))
                    {
                        UserIdToCheck = id.Value;

                        if (!_userService.DoesUserExist(UserIdToCheck))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    model.UserId = UserIdToCheck;


                    return View(model);
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "userId = " + id);
            }
            return RedirectToAction("Login", "Account", new { returnUrl = Request.Url.AbsoluteUri.ToString() });
        }

        public ActionResult Index()
        {
            return Redirect("/MyProfile");
        }
        #endregion

        #region ExternalLogin

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AntiForgeryExceptionAttributeExternalAuthentification]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }



        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            bool _Result = false;
            string _Error = "";
            string _Media = "";
            string _ImageSrc = "";
            string _Language = CurrentLangTag;
            string _Redirection = "";
            string _FirstName = "";
            string _LastName = "";
            UserSession userSession = null;
            bool _IsAlreadyLoggedIn = false;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _Error = "[[[You are already logged in.]]]";
                    _IsAlreadyLoggedIn = true;
                }
                else
                {
                    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                    ExternalSignUpInformation ExternalSignUpInformation = null;
                    if (loginInfo == null)
                    {
                        _Error = "[[[No login information have been provided.]]]";
                    }
                    else
                    {
                        var externalIdentity = HttpContext.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                        _Media = loginInfo.Login.LoginProvider;
                        // Sign in the user with this external login provider if the user already has a login
                        var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: true);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                _Result = true;
                                Session[CommonsConst.Const.JsonConstantsSession] = null;
                                userSession = _userService.GetUserSession(User.Identity.GetUserName());
                                if (userSession != null)
                                {
                                    _aspNetUsersService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                    _Language = userSession.LanguageTag;
                                    UserSession = userSession;
                                    WebsiteLanguage = _Language;
                                }


                                if (loginInfo.Login.LoginProvider == LoginProviders.Facebook && !String.IsNullOrWhiteSpace(FacebookAccessToken))
                                {
                                    ExternalSignUpInformation = SocialMediaHelper.GetFacebookInformation(FacebookAccessToken);

                                }
                                else if (loginInfo.Login.LoginProvider == LoginProviders.Google)
                                {
                                    var GoogleAccessToken = loginInfo.ExternalIdentity.Claims.Where(c => c.Type.Equals("urn:google:accesstoken")).Select(c => c.Value).FirstOrDefault();

                                    if (!String.IsNullOrWhiteSpace(GoogleAccessToken))
                                    {
                                        ExternalSignUpInformation = SocialMediaHelper.GetGoogleInformation(GoogleAccessToken);
                                    }
                                }

                                if (ExternalSignUpInformation != null)
                                {
                                    _socialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                }

                                break;
                            case SignInStatus.LockedOut:
                                _Error = "[[[The user is currently locked out.]]]";
                                break;
                            case SignInStatus.RequiresVerification:
                                _Error = "[[[A user verification is required.]]]";
                                break;
                            case SignInStatus.Failure:
                            default:

                                if (loginInfo.Login.LoginProvider == LoginProviders.Facebook && !String.IsNullOrWhiteSpace(FacebookAccessToken))
                                {
                                    ExternalSignUpInformation = SocialMediaHelper.GetFacebookInformation(FacebookAccessToken);

                                }
                                else if (loginInfo.Login.LoginProvider == LoginProviders.Google)
                                {
                                    var GoogleAccessToken = loginInfo.ExternalIdentity.Claims.Where(c => c.Type.Equals("urn:google:accesstoken")).Select(c => c.Value).FirstOrDefault();

                                    if (!String.IsNullOrWhiteSpace(GoogleAccessToken))
                                    {
                                        ExternalSignUpInformation = SocialMediaHelper.GetGoogleInformation(GoogleAccessToken);
                                    }
                                }

                                if (ExternalSignUpInformation != null)
                                {
                                    _socialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                    if (ExternalSignUpInformation.EmailPermission)
                                    {
                                        if (_aspNetUsersService.IsUserRegistered(ExternalSignUpInformation.Email))
                                        {

                                            bool success = _aspNetUsersService.CreateExternalLogin(ExternalSignUpInformation);

                                            if (success)
                                            {
                                                result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
                                                switch (result)
                                                {
                                                    case SignInStatus.Success:
                                                        _Result = true;
                                                        Session[CommonsConst.Const.JsonConstantsSession] = null;
                                                        userSession = _userService.GetUserSession(User.Identity.GetUserName());
                                                        if (userSession != null)
                                                        {
                                                            _aspNetUsersService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                                            _socialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                                            _Language = userSession.LanguageTag;
                                                            UserSession = userSession;
                                                            WebsiteLanguage = _Language;
                                                        }

                                                        break;
                                                    case SignInStatus.Failure:
                                                    default:
                                                        _Error = ExternalSignUpInformation.Email + "[[[ registered using an email address. Please log in with this email using a password.]]]";
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                _Error = ExternalSignUpInformation.Email + "[[[ registered using an email address. Please log in with this email using a password.]]]";
                                            }
                                        }
                                        else
                                        {
                                            _Redirection = ExternalAuthentificationRedirection.RedirectToExternalSignUp;
                                            _Error = "[[[No account exists for that ]]]" + ExternalSignUpInformation.LoginProvider + "[[[ login. Try signing up instead.]]]";
                                        }
                                    }
                                    else
                                    {
                                        _Error = "[[[You must authorize ]]]" + CommonsConst.Const.WebsiteTitle + "[[[ to access your email address in order to sign up.]]]";
                                        if (!string.IsNullOrWhiteSpace(FacebookAccessToken))
                                        {
                                            var fb = new FacebookClient(FacebookAccessToken);
                                            if (fb != null)
                                            {
                                                var res = fb.Delete("me/permissions");
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    _Redirection = ExternalAuthentificationRedirection.RedirectToExternalSignUp;
                                    _Error = "[[[No account exists for that login. Try signing up instead.]]]";
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View("~/Views/Account/ExternalAuthentificationResult.cshtml", new ExternalAuthentificationResult(_Result, returnUrl, _Error.Trim(), _Media, _ImageSrc, false, _Language, _IsAlreadyLoggedIn,_Redirection, _FirstName, _LastName));
        }

        #endregion

        #region ExternalSignUp

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AntiForgeryExceptionAttributeExternalAuthentification]
        public ActionResult ExternalSignUp(string provider, string returnUrl)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalSignUpCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        public ActionResult ExternalAuthentificationError()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            return View("~/Views/Account/ExternalAuthentificationResult.cshtml", new ExternalAuthentificationResult(false, null, null, null, null, false, null,  true));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalSignUpCallback(string returnUrl)
        {
            bool _Result = false;
            string _Error = "";
            string _Media = "";
            string _ImageSrc = null;
            string _Language = CurrentLangTag;
            string _Redirection = "";
            string _FirstName = "";
            string _LastName = "";
            bool _IsAlreadyLoggedIn = false;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _Error = "[[[You are already logged in.]]]";
                    _IsAlreadyLoggedIn = true;
                }
                else
                {

                    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
                    if (loginInfo == null)
                    {
                        _Error = "[[[No login information have been provided.]]]";
                    }
                    else
                    {

                        _Media = loginInfo.Login.LoginProvider;
                        ExternalSignUpInformation ExternalSignUpInformation = null;
                        // added the following lines
                        if (loginInfo.Login.LoginProvider == LoginProviders.Facebook)
                        {
                            var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                            if (!String.IsNullOrWhiteSpace(FacebookAccessToken))
                            {
                                ExternalSignUpInformation = SocialMediaHelper.GetFacebookInformation(FacebookAccessToken);
                            }

                        }
                        else if (loginInfo.Login.LoginProvider == LoginProviders.Google)
                        {
                            var GoogleAccessToken = loginInfo.ExternalIdentity.Claims.Where(c => c.Type.Equals("urn:google:accesstoken")).Select(c => c.Value).FirstOrDefault();

                            if (!String.IsNullOrWhiteSpace(GoogleAccessToken))
                            {
                                ExternalSignUpInformation = SocialMediaHelper.GetGoogleInformation(GoogleAccessToken);
                            }
                        }


                        if (ExternalSignUpInformation != null && ExternalSignUpInformation.EmailPermission)
                        {

                            if (!String.IsNullOrWhiteSpace(ExternalSignUpInformation.Email))
                            {
                                if (!_userService.IsEmailWaitingForConfirmation(ExternalSignUpInformation.Email))
                                {

                                    int CurrentLanguageId = CommonsConst.Languages.ToInt(CurrentLangTag);
                                    if (!String.IsNullOrWhiteSpace(ExternalSignUpInformation.ImageSrc))
                                    {
                                        ExternalSignUpInformation.ImageSrc = FileHelper.SaveFileFromWeb(ExternalSignUpInformation.ImageSrc, "user", ".jpg");
                                        _ImageSrc = ExternalSignUpInformation.ImageSrc;
                                    }
                                    var user = new ApplicationUser { UserName = ExternalSignUpInformation.Email, Email = ExternalSignUpInformation.Email };

                                    var result = await UserManager.CreateAsync(user);
                                    if (result != null && result.Succeeded)
                                    {

                                        UserSignUp userSignUp = new UserSignUp();
                                        userSignUp.FirstName = ExternalSignUpInformation.FirstName;
                                        userSignUp.LastName = ExternalSignUpInformation.LastName;
                                        userSignUp.LanguageId = CurrentLanguageId;
                                        userSignUp.GenderId = ExternalSignUpInformation.GenderId;
                                        userSignUp.ReceiveNews = false;
                                        userSignUp.PictureSrc = _ImageSrc;
                                        userSignUp.UserName = ExternalSignUpInformation.Email;
                                        userSignUp.FacebookLink = ExternalSignUpInformation.FacebookLink;
                                        if (_userService.CreateUser(userSignUp) > 0)
                                        {
                                            result = await UserManager.AddLoginAsync(user.Id, loginInfo.Login);
                                            if (result.Succeeded)
                                            {
                                                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                                Session[CommonsConst.Const.JsonConstantsSession] = null;
                                                if (!String.IsNullOrWhiteSpace(ExternalSignUpInformation.Email))
                                                {
                                                    UserSession userSession = _userService.GetUserSession(ExternalSignUpInformation.Email);
                                                    _Result = true;
                                                    _Language = userSession.LanguageTag;
                                                    _socialMediaConnectionService.InsertSocialMediaConnections(ExternalSignUpInformation.FriendsList, ExternalSignUpInformation.ProviderKey, ExternalSignUpInformation.LoginProvider);
                                                    _emailService.SendEMailToUser(ExternalSignUpInformation.Email, CommonsConst.EmailTypes.UserWelcome);
                                                    _Result = _userService.CreateThumbnailUserPicture(userSession.UserId);
                                                }
                                                _Result = true;


                                            }
                                        }
                                        else
                                        {
                                            _Error = "[[[An error occured while creating the user.]]]";
                                        }

                                    }
                                    else if (result != null)
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            string message = error;
                                            _Error = _Error + " " + message;
                                        }
                                        if (_Error.Contains("is already taken"))
                                        {
                                            _Error = ExternalSignUpInformation.Email + "[[[ is already registered on ]]]" + CommonsConst.Const.WebsiteTitle + ". [[[Please log in. ]]]";
                                            _Redirection = ExternalAuthentificationRedirection.RedirectToLogin;
                                        }
                                    }
                                    else
                                    {
                                        _Error = "[[[An error occured while creating the user.]]]";
                                    }
                                }
                                else
                                {
                                    _Error = "[[[This email address is already used.]]]";
                                }
                            }
                            else
                            {
                                _Redirection = ExternalAuthentificationRedirection.RedirectToEmailSignUp;
                                _FirstName = ExternalSignUpInformation.FirstName;
                                _LastName = ExternalSignUpInformation.LastName;
                                _Error = "[[[Please review and provide any missing information to finish signing up.]]]";
                            }

                        }
                        else
                        {
                            _Error = "[[[You must authorize ]]]" + CommonsConst.Const.WebsiteTitle + "[[[ to access your email address in order to sign up.]]]";

                            if (!string.IsNullOrWhiteSpace(FacebookAccessToken))
                            {
                                var fb = new FacebookClient(FacebookAccessToken);
                                if (fb != null)
                                {
                                    var res = fb.Delete("me/permissions");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }


            return View("~/Views/Account/ExternalAuthentificationResult.cshtml", new ExternalAuthentificationResult(_Result, returnUrl, _Error.Trim(), _Media, _ImageSrc, true, _Language, _IsAlreadyLoggedIn,_Redirection, _FirstName, _LastName));
        }
        #endregion

        #region SignUp
        public ActionResult _SignUpForm()
        {
            SignUpViewModel model = new SignUpViewModel();
            try
            {
                model.ReceiveNews = true;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return PartialView("~/Views/Account/SignUp/_SignUpForm.cshtml", model);
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AntiForgeryExceptionAttribute]
        public async Task<ActionResult> _SignUpForm(SignUpViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserFirstName = "";
            bool _IsUserAlreadyLoggedIn = false;

            try
            {
                if (CaptchaHelper.CheckCaptcha(Request["g-recaptcha-response"]))
                {
                    if (!User.Identity.IsAuthenticated)
                    {

                        if (ModelState.IsValid)
                        {
                            model.Email = model.Email.Trim().ToLower();
                            if (_emailService.IsEmailAddressValid(model.Email))
                            {
                                if (!_userService.IsEmailWaitingForConfirmation(model.Email))
                                {

                                    int CurrentLanguageId = CommonsConst.Languages.ToInt(CurrentLangTag);
                                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                                    var result = await UserManager.CreateAsync(user, model.Password);
                                    if (result != null && result.Succeeded)
                                    {
                                        UserSignUp userSignUp = new UserSignUp();
                                        userSignUp.FirstName = model.FirstName;
                                        userSignUp.LastName = model.LastName;
                                        userSignUp.LanguageId = CurrentLanguageId;
                                        userSignUp.ReceiveNews = model.ReceiveNews;
                                        userSignUp.UserName = model.Email;

                                        if (_userService.CreateUser(userSignUp) > 0)
                                        {

                                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                            if (!String.IsNullOrWhiteSpace(model.Email))
                                            {
                                                UserSession = _userService.GetUserSession(model.Email);
                                                _Result = true;
                                                Session[CommonsConst.Const.JsonConstantsSession] = null;
                                                string UserName = model.Email;
                                                _emailService.SendEMailToUser(UserName, CommonsConst.EmailTypes.UserWelcome);
                                            }
                                        }
                                        else
                                        {
                                            _Error = "[[[Error while creating the user.]]]";
                                        }
                                    }
                                    else
                                    {
                                        if (_aspNetUsersService.IsUserRegistered(model.Email))
                                        {
                                            _Error = "[[[This email address is already used.]]]";
                                        }
                                        else
                                        {
                                            _Error = "[[[Error while creating the user.]]]";
                                        }
                                    }
                                }
                                else
                                {
                                    _Error = "[[[This email address is already used.]]]";
                                }
                            }
                            else
                            {
                                _Error = "[[[Please enter a valid email address.]]]";
                            }
                        }
                        else
                        {
                            _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                        }


                    }
                    else
                    {
                        _Error = "[[[You are already logged in.]]]";
                        _Result = false;
                        _IsUserAlreadyLoggedIn = true;
                    }
                }
                else
                {
                    _Error = "[[[Please prove that you are not a robot by clicking on the captcha.]]]";
                    _Result = false;
                }
            }
            catch (Exception e)
            {
                _Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email + " and FirstName = " + model.FirstName + " and LastName = " + model.LastName);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName , IsUserAlreadyLoggedIn = _IsUserAlreadyLoggedIn });
        }


        [HttpGet]
        public ActionResult _SignUpPictureForm()
        {
            SignUpPictureViewModel model = new SignUpPictureViewModel();
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userLogged = _userService.GetUserByUserName(User.Identity.Name);
                    if (userLogged != null)
                    {
                        model.PictureSrc = userLogged.PictureSrc;
                        model.PicturePreviewSrc = FileHelper.GetDecryptedFilePath(model.PictureSrc, true);
                        return PartialView("~/Views/Account/SignUp/_SignUpPictureForm.cshtml", model);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return null;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult _SignUpPictureForm(SignUpPictureViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        int UserId = UserSession.UserId;
                        if (UserId > 0 && !String.IsNullOrWhiteSpace(model.PictureSrc))
                        {
                            _Result = _userService.UpdateProfilePicture(UserId, model.PictureSrc);
                            if (_Result)
                            {
                                UserSession = _userService.GetUserSession(UserSession.UserName);
                            }
                        }
                    }
                    else
                    {
                        _Error = "[[[You are not logged in.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return Json(new { Result = _Result, Error = _Error });
        }


        #endregion


        #region LoginModal
        public ActionResult _LoginForm(string returnUrl = null)
        {
            LoginViewModel model = new LoginViewModel();
            try
            {
                model.URLRedirect = returnUrl;
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "returnUrl = " + returnUrl);
            }

            return PartialView(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AntiForgeryExceptionAttribute]
        public async Task<ActionResult> _LoginForm(LoginViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserFirstName = "";
            string _LangTag = CommonsConst.Const.DefaultCulture;
            bool _IsAlreadyLoggedIn = false;

            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    if (ModelState.IsValid)
                    {
                        model.Email = model.Email.Trim().ToLower();
                        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                _Result = true;
                                Session[CommonsConst.Const.JsonConstantsSession] = null;
                                UserSession userSession = _userService.GetUserSession(model.Email);
                                if (userSession != null)
                                {
                                    _UserFirstName = userSession.FirstName;
                                    _aspNetUsersService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                    model.LanguageTag = userSession.LanguageTag;
                                    _LangTag = userSession.LanguageTag; ;
                                    UserSession = userSession;
                                    WebsiteLanguage = _LangTag;
                                }
                                break;
                            case SignInStatus.LockedOut:
                                _Error = "[[[[Your account is currently lockout.]]]";
                                break;
                            case SignInStatus.RequiresVerification:
                                _Error = "[[[Invalid login attempt.]]]";
                                break;
                            case SignInStatus.Failure:
                                if (!_aspNetUsersService.IsUserRegistered(model.Email))
                                {
                                    _Error = "[[[This user is not registered. Please sign-up.]]]";
                                }
                                else
                                {
                                    _aspNetUsersService.UpdateUserIdentityLoginFailure(model.Email);
                                    _Error = "[[[Incorrect password.]]]";
                                }
                                break;
                            default:
                                _Error = "[[[Invalid login attempt.]]]";
                                break;
                        }
                    }
                    else
                    {
                        _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                    }
                }
                else
                {
                    _IsAlreadyLoggedIn = true;
                }
            }
            catch (Exception e)
            {
                _Result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + model.Email);
            }

            return Json(new { Result = _Result, Error = _Error, UserFirstName = _UserFirstName, URLRedirect = model.URLRedirect, LangTag = _LangTag, IsAlreadyLoggedIn= _IsAlreadyLoggedIn });
        }
        #endregion

        #region Logoff

        [HttpGet]
        public ActionResult AutomaticLogOffAndRedirectToLogin(string RedirectTo = null)
        {
            try
            {

                Session[CommonsConst.Const.UserSession] = null;
                Session[CommonsConst.Const.JsonConstantsSession] = null;
                Session.Clear();
                Session.Abandon();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                if (String.IsNullOrWhiteSpace(RedirectTo))
                {
                    RedirectTo = Request?.Url?.AbsoluteUri?.ToString();
                    if (RedirectTo != null && RedirectTo.Contains("AutomaticLogOffAndRedirectToLogin"))
                        RedirectTo = null;
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "RedirectTo = " + RedirectTo);
            }
            return RedirectToAction("Index", "Home", new { PromptLogin = true, SignUp = false, RedirectTo = RedirectTo, LogOff=true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken, AntiForgeryExceptionAttributeLogOff]
        public ActionResult LogOff()
        {
            try
            {
                Session[CommonsConst.Const.UserSession] = null;
                Session[CommonsConst.Const.JsonConstantsSession] = null;
                Session.Clear();
                Session.Abandon();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return Redirect(string.Format("{0}/{1}/?LogOff=True", ConfigurationManager.AppSettings["Website"], CurrentLangTag));
        }

        [HttpGet]
        public ActionResult AutomaticLogOff()
        {
            try
            {
                Session[CommonsConst.Const.UserSession] = null;
                Session[CommonsConst.Const.JsonConstantsSession] = null;
                Session.Clear();
                Session.Abandon();
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
               
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, null);
            }
            return Redirect(string.Format("{0}/{1}/?LogOff=True", ConfigurationManager.AppSettings["Website"], CurrentLangTag));
        }

        /// <summary>
        /// Usd in the case where the last action of the user is Account/LogOff and redirected here
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOff(int a = 0)
        {
            return Redirect(string.Format("{0}/{1}/?LogOff=True", ConfigurationManager.AppSettings["Website"], CurrentLangTag));
        }

        #endregion

        #region Login

        public ActionResult Login(string returnUrl = null)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (String.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToLocal(returnUrl);
                    }
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "returnUrl = " + returnUrl);
            }

            return RedirectToAction("Index", "Home", new { SignUp = false, PromptLogin = true, RedirectTo = returnUrl });
        }

        /// <summary>
        /// Terms and conditions of the website
        /// </summary>
        /// <returns></returns>
        public ActionResult TermsAndConditions()
        {

            try
            {
                ViewBag.Title = "[[[Terms And Conditions]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View();
        }

        /// <summary>
        /// Privacy policy of the website
        /// </summary>
        /// <returns></returns>
        public ActionResult PrivacyPolicy()
        {

            try
            {
                ViewBag.Title = "[[[Privacy Policy]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View();
        }


        #endregion

        #region changepassword
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            try
            {
                ViewBag.Title = "[[[Change My Password]]]";
                if (User.Identity.IsAuthenticated)
                {
                    return View(model);
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _Langtag = CommonsConst.Const.DefaultCulture;
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        if (UserSession != null && UserSession.UserId>0)
                        {
                            _Langtag = UserSession.LanguageTag;
                            IdentityResult result = await UserManager.ChangePasswordAsync(UserSession.UserIdentityId, model.OldPassword, model.NewPassword);
                            if (result.Succeeded)
                            {
                                _Result = _emailService.SendEMailToUser(UserSession.UserId, EmailTypes.ResetPassword);
                            }
                            else
                            {
                                foreach (var error in result.Errors)
                                {
                                    string displayedError = error;
                                    if (error == "Incorrect password.")
                                    {
                                        displayedError = "[[[The old password is incorrect.]]]";
                                    }
                                    _Error = _Error + displayedError + " ";
                                }

                            }
                        }
                        else
                        {
                            _Error = "[[[Sorry, the user has not been found.]]]";
                        }
                    }
                    else
                    {
                        _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error.Trim(), Langtag = _Langtag });
        }

        #endregion

        #region PasswordChanged

        public ActionResult PasswordChanged()
        {

            try
            {
                ViewBag.Title = "[[[Password Changed]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            }

            return View();
        }

        #endregion

        #region Resetpassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(int UserId, string Token)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            try
            {
                model.Token = Token;
                model.UserId = UserId;

                ViewBag.Title = "[[[Reset My Password]]]";
                var user = _userService.GetUserById(model.UserId);
                if (user == null || String.IsNullOrWhiteSpace(user.ResetPasswordToken) || Token != HashHelpers.HashEncode(user.ResetPasswordToken))
                {
                    return View("InvalidToken");
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _Langtag = CommonsConst.Const.DefaultCulture;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetUserById(model.UserId);
                    if (user != null)
                    {
                        _Langtag = user.Language?.Code ?? CommonsConst.Const.DefaultCulture;
                        var applicationUser = UserManager.FindByName(user.AspNetUser.UserName);
                        UserManager.RemovePassword(user.AspNetUser.Id);
                        UserManager.AddPassword(user.AspNetUser.Id, model.Password);
                        var result = await SignInManager.PasswordSignInAsync(user.AspNetUser.UserName, model.Password, true, shouldLockout: false);
                        switch (result)
                        {
                            case SignInStatus.Success:
                                _Result = true;
                                UserSession userSession = _userService.GetUserSession(user.AspNetUser.UserName);
                                if (userSession != null)
                                {
                                    _aspNetUsersService.UpdateUserIdentityLoginSuccess(userSession.UserIdentityId);
                                    UserSession = userSession;
                                }
                                break;
                            default:
                                _Error = "[[[Invalid login attempt.]]]";
                                break;
                        }
                        _Result = _userService.SetPasswordToken(user.Id, null);

                        if (_Result)
                        {
                            _Result = _emailService.SendEMailToUser(user.Id, EmailTypes.ResetPassword);
                        }
                    }
                    else
                    {
                        _Error = "[[[Sorry, the user has not been found.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return Json(new { Result = _Result, Error = _Error, Langtag = _Langtag });
        }

        #endregion

        #region forgotpassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult _ForgotPasswordForm()
        {
            return PartialView(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult _ForgotPasswordForm(ForgotPasswordViewModel model)
        {
            bool _Result = false;
            string _Error = "";
            string _UserMail = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userService.GetUserByUserName(model.UserName.Trim().ToLower());
                    if (user != null)
                    {
                        if (String.IsNullOrWhiteSpace(user.ResetPasswordToken))
                        {
                            _Result = _userService.SetPasswordToken(user.Id, HashHelpers.RandomString(32));
                        }
                        else
                        {
                            _Result = true;
                        }
                        if (_Result)
                        {
                            _Result = _emailService.SendEMailToUser(user.Id, EmailTypes.Forgotpassword);
                            _UserMail = user.AspNetUser.Email;
                        }
                    }
                    else
                    {
                        _Error = "[[[Sorry, this user has not been found.]]]";
                    }
                }
                else
                {
                    _Error = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + model.UserName);
            }
            return Json(new { Result = _Result, Error = _Error, UserMail = _UserMail });
        }

        #endregion

        #region emailVerification

        public ActionResult ConfirmEmail(int UserId, string Token)
        {
            try
            {

                var user = _userService.GetUserById(UserId);
                if (user != null && !String.IsNullOrWhiteSpace(user.EmailConfirmationToken) && Token == HashHelpers.HashEncode(user.EmailConfirmationToken))
                {
                    bool UserNameModified = !String.IsNullOrWhiteSpace(user.UserNameModification);
                    if (_userService.VerifyUserEmailAddress(UserId))
                    {

                        if (UserNameModified)
                            return RedirectToAction("AutomaticLogOffAndRedirectToLogin", "Account", new { RedirectTo = MyProfileUrl + "/email" });
                        else
                            return  Redirect(MyProfileUrl + "/email");
                    }
                }
                ViewBag.Title = "[[[Email Confirmation]]]";
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId+ " and Token  = "+ Token);
            }
            return View("~/Views/Account/InvalidToken.cshtml");
        }

        #endregion

    }
}

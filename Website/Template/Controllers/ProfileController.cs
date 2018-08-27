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
using Models.ViewModels.Profile;
using Models.Class.UserFollow;

namespace Website.Controllers
{
    public class ProfileController : BaseController
    {
        private IEMailService _emailService;
        private IUserFollowService _userFollowService;

        public ProfileController(
            IUserService userService,
            IEMailService emailService,
            IUserFollowService userFollowService,
            ApplicationUserManager userManager
            ) : base(userService)
        {
            _emailService = emailService;
            _userFollowService = userFollowService;
            _userManager = userManager;
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

        private ApplicationUserManager _userManager;

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

        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        #region Profile
        public ActionResult Index(int? id, string show = MyProfileShow.Profile)
        {
            PublicProfileViewModel model = new PublicProfileViewModel();
            try
            {
                ViewBag.Title = "[[[My Profile]]]";
                model.Show = show;
                if (id == null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserId = UserSession.UserId;
                        model.CanUserEdit = true;
                        model.IsLoggedUserProfile = true;

                        ViewBag.MetaKeyWords = "Profile,Profil,My Profile," + CommonsConst.BaseMetaData.KeyWords;
                        ViewBag.MetaDescription = "My " + CommonsConst.Const.WebsiteTitle + " Profile. " + CommonsConst.BaseMetaData.Description;
                        ViewBag.MetaTitle = "My " + CommonsConst.Const.WebsiteTitle + " Profile";
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Profile", new { show }) });
                    }
                }
                else //ie. url id has value
                {
                    var user = _userService.GetUserById(id.Value);
                    if (user != null)
                    {

                        model.UserId = id.Value;
                        model.CanUserEdit = _userService.CanUserEditProfile(model.UserId, User.Identity.Name);

                        ViewBag.MetaTitle = user.FirstName + " " + user.LastName + " Profile On " + CommonsConst.Const.WebsiteTitle;
                        ViewBag.MetaImageSrc = String.IsNullOrWhiteSpace(user.PictureSrc) ? null : ConfigurationManager.AppSettings["Website"] + user.PictureSrc;
                        ViewBag.MetaDescription = "Check out " + user.FirstName + " " + user.LastName + " profile on " + CommonsConst.Const.WebsiteTitle + ". " + CommonsConst.BaseMetaData.Description;
                        if (user.PublicProfile)
                        {
                            ViewBag.MetaKeyWords = user.FirstName + "," + user.LastName + "," + user.FirstName + " " + user.LastName + "," + user.LastName + " " + user.FirstName + ",Profile,Profil,My Profile," + CommonsConst.BaseMetaData.KeyWords;
                        }
                        if (User.Identity.IsAuthenticated && user.Id == UserSession.UserId)
                        {
                            model.IsLoggedUserProfile = true;
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }


                if (!model.CanUserEdit || model.Show == null)
                {
                    model.Show = MyProfileShow.Profile;
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = " + id);
            }
            return View("~/Views/Profile/Profile.cshtml", model);
        }

        #endregion

        #region _Profile




        [HttpPost]
        public ActionResult ToggleUserFollow(int UserToFollowId)
        {
            bool _success = false;
            string _Error = "";
            bool _UserFollowed = false;
            try
            {
                if (User.Identity.IsAuthenticated && UserSession.UserId != UserToFollowId)
                {
                    if (UserSession.UserId > 0 && UserToFollowId > 0)
                    {
                        ToggleUserFollowResult result = _userFollowService.ToggleUserFollow(UserSession.UserId, UserToFollowId);
                        if (result != null)
                        {
                            _success = result.Result;
                            _UserFollowed = result.Followed;
                        }
                    }
                    else
                    {
                        _Error = ErrorMessages.UnknownError;
                    }
                }
                else
                {
                    _Error = ErrorMessages.NotAuthorized;
                }

            }
            catch (Exception e)
            {
                _success = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserToFollowId = " + UserToFollowId);
            }
            return Json(new { Result = _success, UserFollowed = _UserFollowed, Error = _Error });
        }




        public ActionResult _EditGeneralInformation(int UserId)
        {
            try
            {
                EditGeneralInformationViewModel model = new EditGeneralInformationViewModel();

                model = _userService.GetEditGeneralInformationViewModel(UserId);
                if (model != null && model.UserId > 0)
                {
                    if (_userService.CanUserEditProfile(UserId, User.Identity.Name))
                    {
                        return PartialView("~/Views/Profile/Profile/_EditGeneralInformation.cshtml", model);
                    }
                    else
                    {
                        return Content(PartialViewResults.NotAuthorized);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Content(PartialViewResults.UnknownError);
        }

        [HttpPost]
        public ActionResult UpdateBackgroundPicture(int UserId)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            try
            {
                if (_userService.CanUserEditProfile(UserId, User.Identity.Name))
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var backgroundInputFile = Request.Files[i];
                        // legacy portrait image upload
                        if (backgroundInputFile != null)
                        {
                            if (!FileHelper.IsValidImage(backgroundInputFile))
                            {
                                _Error = "[[[Plese upload a valid image.]]]";
                            }
                            else
                            {
                                var imageSrc = FileHelper.UploadDecryptedFile(backgroundInputFile, string.Format("backgroundpicture_{0}", UserId));
                                if (!String.IsNullOrWhiteSpace(imageSrc) && imageSrc != "KO")
                                {
                                    _success = _userService.UpdateBackgroundPicture(UserId, imageSrc);
                                    _PathFile = imageSrc;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _Error = ErrorMessages.NotAuthorized;
                }

            }
            catch (Exception e)
            {
                _success = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error });
        }


        [HttpPost]
        public ActionResult UpdateProfilePicture(int UserId)
        {
            bool _success = false;
            string _Error = "";
            string _PathFile = "";
            try
            {
                if (_userService.CanUserEditProfile(UserId, User.Identity.Name))
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var portraitInputFile = Request.Files[i];
                        // legacy portrait image upload
                        if (portraitInputFile != null)
                        {
                            if (!FileHelper.IsValidImage(portraitInputFile))
                            {
                                _Error = "[[[Plese upload a valid image.]]]";
                            }
                            else
                            {
                                var imageSrc = FileHelper.UploadDecryptedFile(portraitInputFile, string.Format("user_{0}", UserId));
                                if (!String.IsNullOrWhiteSpace(imageSrc) && imageSrc != "KO")
                                {
                                    _success = _userService.UpdateProfilePicture(UserId, imageSrc);
                                    _PathFile = imageSrc;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _Error = ErrorMessages.NotAuthorized;
                }

            }
            catch (Exception e)
            {
                _success = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return Json(new { Result = _success, PathFile = _PathFile, Error = _Error });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult _EditGeneralInformation(EditGeneralInformationViewModel model)
        {
            bool _success = false;
            string _errors = "";
            string _firsName = "";
            try
            {
                if (model != null && model.UserId > 0)
                {
                    if (ModelState.IsValid)
                    {
                        if (_userService.CanUserEditProfile(model.UserId, User.Identity.Name))
                        {
                            if (String.IsNullOrWhiteSpace(model.FirstName) && String.IsNullOrWhiteSpace(model.LastName))
                            {
                                _errors = "[[[Please enter your first name and your last name.]]]";

                            }
                            else if (String.IsNullOrWhiteSpace(model.FirstName))
                            {
                                _errors = "[[[Please enter your first name.]]]";
                            }
                            else if (String.IsNullOrWhiteSpace(model.LastName))
                            {
                                _errors = "[[[Please enter your last name.]]]";
                            }
                            else
                            {
                                _success = _userService.SaveGeneralInformation(model);
                                if (_success)
                                {
                                    _firsName = model.FirstName;
                                }
                            }
                        }
                        else
                        {
                            _errors = ErrorMessages.NotAuthorized;
                        }
                    }
                    else
                    {
                        _errors = ModelStateHelper.GetModelErrorsToDisplay(ModelState);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " model.UserId = " + model.UserId);
            }
            return Json(new { Result = _success, Errors = _errors, FirstName = _firsName });
        }


        public ActionResult _Followers(int id)
        {
            try
            {
                FollowersViewModel model = new FollowersViewModel();

                model = _userService.GetFollowers(id);
                if (model != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.IsLoggedUserProfile = (id == UserSession.UserId);
                    }
                    return PartialView("~/Views/Profile/Profile/_Followers.cshtml", model);
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, " id = " + id);
            }
            return Content(PartialViewResults.UnknownError);
        }

        public ActionResult _GeneralInformation(int id)
        {
            try
            {
                GeneralInformationViewModel model = new GeneralInformationViewModel();

                int LoggedUserId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    LoggedUserId = UserSession.UserId;
                }

                model = _userService.GetGeneralInformationViewModel(id, LoggedUserId);
                if (model != null && model.UserId > 0)
                {
                    model.CanUserEditProfile = _userService.CanUserEditProfile(id, User.Identity.Name);

                    if (User.Identity.IsAuthenticated)
                    {
                        model.IsLoggedUserProfile = (id == UserSession.UserId);
                    }


                    return PartialView("~/Views/Profile/Profile/_GeneralInformation.cshtml", model);
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = " + id);
            }
            return Content(PartialViewResults.UnknownError);
        }


        public ActionResult _Profile(int id)
        {
            try
            {
                ProfileViewModel model = new ProfileViewModel();
                int LoggedUserId = 0;
                if (User.Identity.IsAuthenticated)
                {
                    LoggedUserId = UserSession.UserId;
                }
                model = _userService.GetProfileViewModel(id, LoggedUserId);
                if (model != null && model.UserId > 0)
                {
                    model.CanUserEditProfile = _userService.CanUserEditProfile(id, User.Identity.Name);
                    model.GeneralInformation.CanUserEditProfile = model.CanUserEditProfile;

                    if (User.Identity.IsAuthenticated)
                    {
                        model.GeneralInformation.IsLoggedUserProfile = (id == UserSession.UserId);
                        model.Followers.IsLoggedUserProfile = (id == UserSession.UserId);
                        model.PeopleYouFollow.IsLoggedUserProfile = (id == UserSession.UserId);
                    }

                    return PartialView(model);
                }
            }
            catch (Exception e)
            {
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = " + id);
            }
            return Content(PartialViewResults.UnknownError);
        }



        #endregion

    }
}

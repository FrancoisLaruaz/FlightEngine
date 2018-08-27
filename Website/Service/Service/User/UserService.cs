using Commons;
using Models.Class;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.UserArea.Interface;
using DataEntities.Repositories;
using DataEntities.Model;
using DataEntities;
using CommonsConst;
using Models.ViewModels.Account;
using Models.Class.SignUp;
using Service.Admin.Interface;
using Models.ViewModels.Shared;
using Models.ViewModels.Profile;
using Models.Class.UserFollow;

namespace Service.UserArea
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<DataEntities.Model.AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly IGenericRepository<DataEntities.Model.UserFollow> _userFollowRepo;
        private readonly ICategoryService _categoryService;
        private readonly IProvinceService _provinceService;
        private readonly ICountryService _countryService;
        private readonly IScheduledTaskService _scheduledTaskService;
        public readonly IUserRolesService _userRolesService;
        public readonly IUserFollowService _userFollowService;

        public UserService(IGenericRepository<DataEntities.Model.User> userRepo, IUserFollowService userFollowService, IGenericRepository<DataEntities.Model.AspNetUser> aspNetUserRepo, ICategoryService categoryService,
            IProvinceService provinceService, ICountryService countryService, IScheduledTaskService scheduledTaskService, IUserRolesService userRolesService, IGenericRepository<DataEntities.Model.UserFollow> userFollowRepo)
        {
            _userFollowService = userFollowService;
            _userRepo = userRepo;
            _aspNetUserRepo = aspNetUserRepo;
            _categoryService = categoryService;
            _provinceService = provinceService;
            _countryService = countryService;
            _scheduledTaskService = scheduledTaskService;
            _userRolesService = userRolesService;
            _userFollowRepo = userFollowRepo;
        }

        public UserService()
        {
            var context = new TemplateEntities();
            _userRepo =new  GenericRepository<DataEntities.Model.User>(context);
            _aspNetUserRepo = new GenericRepository<AspNetUser>(context);
        }


        public ProfileViewModel GetProfileViewModel(int UserId, int LoggedUserId)
        {
            ProfileViewModel model = new ProfileViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;

                    model.GeneralInformation = GetGeneralInformationViewModel(UserId, LoggedUserId);
                    string PeopleYouFollowTitle = "[[[Following]]]";
                    if (UserId != LoggedUserId)
                    {
                        PeopleYouFollowTitle = "[[[Following]]]";
                    }


                    model.PeopleYouFollow = new FollowersViewModel(PeopleYouFollowTitle, _userFollowService.GetUsersFollowedByUser(UserId));
                    model.Followers = new FollowersViewModel("[[[Followers]]]", _userFollowService.GetUsersFollowingUser(UserId));

                }
            }
            catch (Exception e)
            {
                model = new ProfileViewModel();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        public GeneralInformationViewModel GetGeneralInformationViewModel(int UserId, int LoggedUserId)
        {
            GeneralInformationViewModel model = new GeneralInformationViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.BackgroundPictureSrc =( user.BackgroundPictureSrc ?? DefaultImage.BackgroundPicture).Replace("~", "");
                    model.ImageSrc =( user.PictureSrc ?? DefaultImage.DefaultImageUser).Replace("~","");
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.CreationDateTxt = "[[[Joined ]]]" + user.CreationDate.ToLocalTime().ToString("MMMM yyyy");
                    model.Description = user.Description;
                    model.Facebook = user.FacebookLink;
                    model.LastConnectionDate = user.DateLastConnection;
                    //   model.City = user.Address?.ci
                    model.Province = user.Address?.Province.Name;
                    model.Country = user.Address?.Province?.Country.Name;



                    if (LoggedUserId > 0)
                    {
                        model.FollowedUser = _userFollowRepo.FindAllBy(u => u.FollowedUserId == UserId && u.UserId == LoggedUserId).Any();
                    }
                    else
                    {
                        model.FollowedUser = false;
                    }
                }
            }
            catch (Exception e)
            {
                model = new GeneralInformationViewModel();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId+ " and LoggedUserId = "+ LoggedUserId);
            }
            return model;
        }

        public bool UpdateBackgroundPicture(int id, string path)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(id);
                if (user != null)
                {
                    user.BackgroundPictureSrc = path;
                    user.DateLastConnection = DateTime.UtcNow;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "id = " + id + " and path = " + path);
            }
            return result;
        }

        public FollowersViewModel GetFollowers(int UserId)
        {
            FollowersViewModel model = new FollowersViewModel();
            try
            {
                model = new FollowersViewModel("[[[Followers]]]", _userFollowService.GetUsersFollowingUser(UserId));
            }
            catch (Exception e)
            {
                model = new FollowersViewModel();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }


        public bool SaveGeneralInformation(EditGeneralInformationViewModel model)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName =model.LastName;
                    user.Description = model.Description;
                    user.FacebookLink = model.Facebook;
                    user.Description = model.Description;
                    user.DateLastConnection = DateTime.UtcNow;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + model.UserId);
            }
            return result;
        }

        public EditGeneralInformationViewModel GetEditGeneralInformationViewModel(int UserId)
        {
            EditGeneralInformationViewModel model = new EditGeneralInformationViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.FirstName =user.FirstName;
                    model.LastName = user.LastName;
                    model.Description = user.Description;
                    model.Facebook = user.FacebookLink;
                }
            }
            catch (Exception e)
            {
                model = new EditGeneralInformationViewModel();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId );
            }
            return model;
        }


        public bool CanUserEditProfile(int toBeEditedUserId, string currentUserName)
        {
            bool result = false;
            try
            {
                if (String.IsNullOrWhiteSpace(currentUserName) || toBeEditedUserId <= 0)
                {
                    result = false;
                }
                else
                {
                    User currentUser = _userRepo.FindAllBy(u => u.AspNetUser.UserName.ToLower().Trim() == currentUserName.ToLower().Trim()).FirstOrDefault();
                    if (currentUser != null)
                    {
                        if (toBeEditedUserId == currentUser.Id)
                        {
                            result = true;
                        }
                        else if (_userRolesService.IsInRole(currentUserName, UserRoles.Admin))
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "toBeEditedUserId = " + toBeEditedUserId + " and currentUserName = " + currentUserName);
            }
            return result;
        }

        public int CreateUser(UserSignUp model)
        {
            int InsertedId = -1;
            try
            {

                AspNetUser aspNetUser = _aspNetUserRepo.FindAllBy(u => u.UserName == model.UserName).FirstOrDefault();

                if (aspNetUser != null)
                {
                    User user = new User();
                    user.PictureSrc = model.PictureSrc ?? CommonsConst.DefaultImage.DefaultImageUser;
                    user.FirstName =model.FirstName;
                    user.LastName = model.LastName;
                    user.GenderId = model.GenderId;
                    user.LanguageId = model.LanguageId==null? Languages.ToInt(CommonsConst.Const.DefaultCulture):  model.LanguageId.Value;
                    user.EmailConfirmationToken = Commons.HashHelpers.RandomString(32);
                    user.DateLastConnection = DateTime.UtcNow;
                    user.CreationDate = DateTime.UtcNow;
                    user.ModificationDate = DateTime.UtcNow;
                    user.ReceiveNews = model.ReceiveNews;
                    user.CreationDate = DateTime.UtcNow;
                    user.AspNetUserId = aspNetUser.Id;
                    user.FacebookLink = model.FacebookLink;

                    _userRepo.Add(user);
                    if (_userRepo.Save())
                    {
                        InsertedId = user.Id;
                    }
                }
            }
            catch (Exception e)
            {
                InsertedId = -1;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + model.UserName);
            }
            return InsertedId;
        }

        /// <summary>
        /// Indicate if the user exist or not
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DoesUserExist(int UserId)
        {
            bool result = false;
            try
            {
                result = _userRepo.FindAllBy(u => u.Id == UserId).Any();

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

        public DataEntities.Model.User GetUserByUserName(string UserName)
        {
            DataEntities.Model.User result = null;
            try
            {
                result = _userRepo.FindAllBy(u => u.AspNetUser.UserName.Trim().ToLower() == UserName.Trim().ToLower()).FirstOrDefault();

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }



        public bool UpdateLanguageUser(string Language, string UserName)
        {
            bool result = false;
            try
            {
                var user = GetUserByUserName(UserName);
                if (user != null)
                {
                    user.LanguageId = Languages.ToInt(Language);
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Language = " + Language + " and UserName = " + UserName);
            }
            return result;
        }



        /// <summary>
        /// Get an object to put in the session
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public UserSession GetUserSession(string UserName)
        {
            UserSession result = new UserSession();
            try
            {
                var user = GetUserByUserName(UserName);
                if (user != null)
                {
                    result.FirstName = user.FirstName;
                    result.LastName = user.LastName;
                    result.UserName = user.AspNetUser.UserName;
                    result.UserIdentityId = user.AspNetUser.Id;
                    result.UserId = user.Id;
                    result.DateLastConnection = user.DateLastConnection;
                    result.LanguageTag = user.Language?.Code??CommonsConst.Const.DefaultCulture;
                    result.PictureThumbnailSrc = user.PictureThumbnailSrc ?? CommonsConst.DefaultImage.DefaultThumbnailUser;
                    result.EmailConfirmed = user.AspNetUser.EmailConfirmed == null ? false : user.AspNetUser.EmailConfirmed.Value;
                }

            }
            catch (Exception e)
            {
                result = new UserSession();
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        public bool SetUserLastConnectionDate(string UserName)
        {
            bool result = false;
            try
            {
                if (String.IsNullOrWhiteSpace(UserName))
                {
                    result = false;
                }
                else
                {
                    User User = _userRepo.FindAllBy(u => u.AspNetUser!=null && u.AspNetUser.UserName.ToLower().Trim() == UserName.ToLower().Trim()).FirstOrDefault();
                    if (User != null)
                    {
                        User.DateLastConnection = DateTime.UtcNow;
                        result = _userRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }


        public MyProfileAddressViewModel GetMyProfileAddressViewModel(int UserId)
        {
            MyProfileAddressViewModel model = new MyProfileAddressViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    /*
                    model.CountryId = user.CountryId;
                    model.ProvinceId = user.ProvinceId;
                    model.Adress1 = user.Adress1;
                    model.Adress2 = user.Adress2;
                    model.Adress3 = user.Adress3;
                    */
                }
                model.ProvinceList = _provinceService.GetProvinceList(model.CountryId);
                model.CountryList = _countryService.GetCountryList();
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }




        public MyProfileTrustAndVerificationsViewModel GetMyProfileTrustAndVerificationsViewModel(int UserId)
        {
            MyProfileTrustAndVerificationsViewModel model = new MyProfileTrustAndVerificationsViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.EmailConfirmed = user.AspNetUser.EmailConfirmed == null ? false : user.AspNetUser.EmailConfirmed.Value;
                    model.Email = user.AspNetUser.Email;
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        public MyProfilePhotosViewModel GetMyProfilePhotosViewModel(int UserId)
        {
            MyProfilePhotosViewModel model = new MyProfilePhotosViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.PictureSrc = user.PictureSrc;
                    model.PictureThumbnailSrc = user.PictureThumbnailSrc;
                    model.PictureDecryptSrc = FileHelper.GetDecryptedFilePath(model.PictureSrc).Replace("~", "");
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }



        public bool SaveMyProfilePhotos(int UserId, string PictureSrc, string PictureThumbnailSrc)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    user.PictureThumbnailSrc = PictureThumbnailSrc;
                    user.PictureSrc = PictureSrc;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }




        public bool SaveMyProfileAddress(MyProfileAddressViewModel model)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {/*
                    user.Adress1 = model.Adress1;
                    user.Adress2 = model.Adress2;
                    user.CountryId = model.CountryId;
                    user.CountryId = model.CountryId;
                    user.ProvinceId = model.ProvinceId;
                    */
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public bool SaveMyProfileEdit(MyProfileEditViewModel model)
        {
            bool result = false;
            try
            {

                var user = _userRepo.Get(model.UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName =model.LastName;
                    user.DateOfBirth = model.DateOfBirth;
                    user.Description = model.Description;
                    user.GenderId = model.GenderId;
                    user.ReceiveNews = model.ReceiveNews;
                    user.LanguageId = model.LanguageId;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "model.UserId = " + model.UserId);
            }
            return result;
        }


        public MyProfileEditViewModel GetMyProfileEditViewModel(int UserId)
        {
            MyProfileEditViewModel model = new MyProfileEditViewModel();
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    model.UserId = user.Id;
                    model.UserName = user.AspNetUser.UserName;
                    model.FirstName =user.FirstName;
                    model.LastName = user.LastName;
                    model.ReceiveNews = user.ReceiveNews;
                    model.LanguageId = user.LanguageId;
                    model.Description = user.Description;
                    model.DateOfBirth = user.DateOfBirth;
                    model.GenderId = user.GenderId;
                }
                model.LanguageList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.Language);
                model.GenderList = _categoryService.GetSelectionList(CommonsConst.CategoryTypes.Gender);
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return model;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteUserById(int UserId)
        {
            bool result = false;
            try
            {
                var UserToDelete = _userRepo.Get(UserId);
                if (UserToDelete != null)
                {
                    _scheduledTaskService.CancelTaskByUserId(UserToDelete.Id);
                    List<Tuple<string, object>> Parameters = new List<Tuple<string, object>>();
                    Parameters.Add(new Tuple<string, object>("@UserId", UserId));
                    result = _userRepo.ExecuteStoredProcedure("DeleteUserById", Parameters);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }

        /// <summary>
        /// Delete a user by username
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool DeleteUserByUserName(string UserName)
        {
            bool result = false;
            try
            {
                var UserToDelete = GetUserByUserName(UserName);
                if (UserToDelete != null)
                {
                    DeleteUserById(UserToDelete.Id);
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        /// <summary>
        /// Create a thumnail for the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool CreateThumbnailUserPicture(int UserId)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    string PictureThumbnailSrc = FileHelper.CreateThumbnail(user.PictureSrc, 40);
                    if (!String.IsNullOrWhiteSpace(PictureThumbnailSrc))
                    {
                        user.PictureThumbnailSrc = PictureThumbnailSrc;
                        _userRepo.Edit(user);
                        result = _userRepo.Save();
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }

        /// <summary>
        /// Update the path of the profile picture of the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="PictureSrc"></param>
        /// <returns></returns>
        public bool UpdateProfilePicture(int UserId, string PictureSrc)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    user.PictureSrc = PictureSrc;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                    if (result)
                    {
                        result = CreateThumbnailUserPicture(UserId);
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId.ToString());
            }
            return result;
        }

        public bool IsEmailWaitingForConfirmation(string UserName)
        {
            bool result = true;
            try
            {
                if (String.IsNullOrWhiteSpace(UserName))
                {
                    result = false;
                }
                else
                {
                    UserName = UserName.ToLower().Trim();
                    result = _userRepo.FindAllBy(u => u.UserNameModification.ToLower().Trim() == UserName).Any();
                }

            }
            catch (Exception e)
            {
                result = true;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }


        public bool IsEmailAvailable(string UserName)
        {
            bool result = true;
            try
            {
                if (String.IsNullOrWhiteSpace(UserName))
                {
                    result = false;
                }
                else
                {
                    UserName = UserName.ToLower().Trim();
                    result = !_userRepo.FindAllBy(u => u.AspNetUser.UserName.ToLower().Trim() == UserName || u.UserNameModification.ToLower().Trim() == UserName).Any();
                }

            }
            catch (Exception e)
            {
                result = false;
                Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }

        /// <summary>
        /// Return the specified user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DataEntities.Model.User GetUserById(int UserId)
        {
            DataEntities.Model.User result = null;
            try
            {
                result = _userRepo.Get(UserId);
            }
            catch (Exception e)
            {
                result = null;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }


        /// <summary>
        /// Set the token of a user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public bool SetPasswordToken(int UserId, string Token)
        {
            bool result = false;
            try
            {
                User user = _userRepo.Get(UserId);
                if(user!=null)
                {
                    user.ResetPasswordToken = Token;
                    _userRepo.Edit(user);
                    result = _userRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId);
            }
            return result;
        }
    }
}

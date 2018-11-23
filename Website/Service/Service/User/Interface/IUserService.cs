using Commons;

using Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.ViewModels;
using DataEntities.Repositories;
using DataEntities.Model;
using Models.ViewModels.Account;
using Models.ViewModels.Home;
using Models.Class.SignUp;
using Models.ViewModels.Profile;

namespace Service.UserArea.Interface
{
    public interface IUserService
    {
        bool VerifyUserEmailAddress(int UserId);
        bool CanUserEditProfile(int toBeEditedUserId, string currentUserName);
        bool SetUserLastConnectionDate(string UserName);
        bool IsEmailAvailable(string UserName);

        bool IsEmailWaitingForConfirmation(string UserName);

        bool DoesUserExist(int UserId);

        DataEntities.Model.User GetUserByUserName(string UserName);

        bool UpdateLanguageUser(string Language, string UserName);

        bool UpdateBackgroundPicture(int id, string path);

        FollowersViewModel GetFollowers(int UserId);

        GeneralInformationViewModel GetGeneralInformationViewModel(int UserId, int LoggedUserId);

        ProfileViewModel GetProfileViewModel(int UserId, int LoggedUserId);

        bool SaveGeneralInformation(EditGeneralInformationViewModel model);

        EditGeneralInformationViewModel GetEditGeneralInformationViewModel(int UserId);

        UserSession GetUserSession(string UserName);

        int CreateUser(UserSignUp model);

        MyProfileAddressViewModel GetMyProfileAddressViewModel(int UserId);

        MyProfileTrustAndVerificationsViewModel GetMyProfileTrustAndVerificationsViewModel(int UserId);

        MyProfilePhotosViewModel GetMyProfilePhotosViewModel(int UserId);


        bool SaveMyProfilePhotos(int UserId, string PictureSrc, string PictureThumbnailSrc);

        bool SaveMyProfileAddress(MyProfileAddressViewModel model);

        bool SaveMyProfileEdit(MyProfileEditViewModel model);

        MyProfileEditViewModel GetMyProfileEditViewModel(int UserId);

        bool DeleteUserById(int UserId);

        bool DeleteUserByUserName(string UserName);

        bool CreateThumbnailUserPicture(int UserId);

        bool UpdateProfilePicture(int UserId, string PictureSrc);

        DataEntities.Model.User GetUserById(int UserId);

        bool SetPasswordToken(int UserId, string Token);

    }
}

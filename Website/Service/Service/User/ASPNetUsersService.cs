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
using Models.Class.ExternalAuthentification;

namespace Service.UserArea
{
    public class ASPNetUsersService : IASPNetUsersService
    {

        private readonly IGenericRepository<DataEntities.Model.AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;
        private readonly IGenericRepository<DataEntities.Model.AspNetUserLogin> _aspNetUserLoginRepo;


        public ASPNetUsersService(IGenericRepository<DataEntities.Model.User> userRepo, IGenericRepository<DataEntities.Model.AspNetUser> aspNetUserRepo, IGenericRepository<DataEntities.Model.AspNetUserLogin> aspNetUserLoginRepo)
        {
            _userRepo = userRepo;
            _aspNetUserRepo = aspNetUserRepo;
            _aspNetUserLoginRepo = aspNetUserLoginRepo;
        }

        /// <summary>
        /// Create an external login if a registered user decide to use a social media to log in
        /// </summary>
        /// <param name="Info"></param>
        /// <returns></returns>
        public bool CreateExternalLogin(ExternalSignUpInformation Info)
        {
            bool result = false;
            try
            {

                AspNetUser aspNetUser = _aspNetUserRepo.FindAllBy(a => a.UserName == Info.Email).FirstOrDefault();
                if (aspNetUser != null)
                {
                    AspNetUserLogin login = new AspNetUserLogin();
                    login.LoginProvider = Info.LoginProvider;
                    login.ProviderKey = Info.ProviderKey;
                    login.UserId = aspNetUser.Id;
                    _aspNetUserLoginRepo.Add(login);
                    result = _aspNetUserLoginRepo.Save();
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Email = " + Info.Email);
            }
            return result;
        }

        /// <summary>
        /// Update the user after an unsuccessfull login
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool UpdateUserIdentityLoginFailure(string UserName)
        {
            bool result = false;
            try
            {
                AspNetUser aspNetuser = _aspNetUserRepo.FindAllBy(a => a.UserName.ToLower().Trim() == UserName.ToLower().Trim()).FirstOrDefault();
                if (aspNetuser != null)
                {
                    aspNetuser.AccessFailedCount = aspNetuser.AccessFailedCount + 1;
                    _aspNetUserRepo.Edit(aspNetuser);
                    result = _aspNetUserRepo.Save();
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
        /// Check if the user is registered
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public bool IsUserRegistered(string UserName)
        {
            bool result = false;
            try
            {
                result = _aspNetUserRepo.FindAllBy(u => u.UserName.Trim().ToLower() == UserName.Trim().ToLower()).Any();

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserName = " + UserName);
            }
            return result;
        }


        /// <summary>
        /// Update the user after a successfull login
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool UpdateUserIdentityLoginSuccess(string Id)
        {
            bool result = false;
            try
            {

                AspNetUser aspNetuser = _aspNetUserRepo.FindAllBy(a => a.Id == Id).FirstOrDefault();
                if (aspNetuser != null)
                {
                    aspNetuser.Users.FirstOrDefault().DateLastConnection = DateTime.UtcNow;
                    aspNetuser.AccessFailedCount = 0;
                    _aspNetUserRepo.Edit(aspNetuser);
                    result = _aspNetUserRepo.Save();

                    if (result)
                    {
                        _userRepo.Edit(aspNetuser.Users.FirstOrDefault());
                        result = _userRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return result;
        }



        /// <summary>
        /// Save the new email of the user
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public bool SaveMyProfileEmail(int UserId, string Email)
        {
            bool result = false;
            try
            {
                var user = _userRepo.Get(UserId);
                if (user != null)
                {
                    var aspnetuser = user.AspNetUser;
                    aspnetuser.EmailConfirmed = false;
                    _aspNetUserRepo.Edit(aspnetuser);
                    result = _aspNetUserRepo.Save();

                    user.UserNameModification = Email;
                    _userRepo.Edit(user);
                    result = result && _userRepo.Save();
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
        /// Set theemail of the user as confirmed
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool SetUserEmailAsConfirmed(string Id)
        {
            bool result = false;
            try
            {
                AspNetUser aspNetuser = _aspNetUserRepo.FindAllBy(a => a.Id == Id).FirstOrDefault();
                if (aspNetuser != null)
                {
                    var user = aspNetuser.Users.FirstOrDefault();
                    string newUserName = user.UserNameModification ?? aspNetuser.UserName;
                    user.EmailConfirmationToken = null;

                    aspNetuser.Email = newUserName;
                    aspNetuser.UserName = newUserName;
                    aspNetuser.EmailConfirmed = true;
                    _aspNetUserRepo.Edit(aspNetuser);
                    result = _aspNetUserRepo.Save();

                    if (result)
                    {
                        _userRepo.Edit(user);
                        result = _userRepo.Save();
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Id = " + Id);
            }
            return result;
        }

    }
}

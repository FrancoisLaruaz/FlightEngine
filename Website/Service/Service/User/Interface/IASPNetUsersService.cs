using Commons;

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
using Models.Class.ExternalAuthentification;

namespace Service.UserArea.Interface
{
    public interface IASPNetUsersService
    {
        bool IsUserRegistered(string UserName);

        bool CreateExternalLogin(ExternalSignUpInformation Info);

        bool UpdateUserIdentityLoginFailure(string UserName);


        bool UpdateUserIdentityLoginSuccess(string Id);

        bool SetUserEmailAsConfirmed(string Id);

        bool SaveMyProfileEmail(int UserId, string Email);
    }
}

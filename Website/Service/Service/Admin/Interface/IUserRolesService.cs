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
using Models.ViewModels.Admin.Users;
using Models.Class.UserRoles;

namespace Service.Admin.Interface
{
    public interface IUserRolesService
    {
        DisplayUsersViewModel GetDisplayUsersViewModel(string Pattern, int StartAt, int PageSize);

        UserRoleItem GetUserRolesByUseridentityId(string UserIdentity);

        bool IsInRole(int UserId, string roleName);

        bool AddUserRole(string UserId, string RoleId);

        bool DeleteUserRoleByUserIdAndRoleId(string UserId, string RoleId);

    }
}

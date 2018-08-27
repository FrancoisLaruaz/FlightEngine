using Commons;

using Models.Class;
using Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataEntities.Model;
using Service.Admin.Interface;
using DataEntities.Repositories;
using Models.ViewModels.Admin.Users;
using Models.Class.UserRoles;

namespace Service.Admin
{
    public  class UserRolesService : IUserRolesService
    {
        private readonly IGenericRepository<DataEntities.Model.AspNetUser> _aspNetUserRepo;
        private readonly IGenericRepository<DataEntities.Model.AspNetRole> _aspNetRoleRepo;
        private readonly IGenericRepository<DataEntities.Model.User> _userRepo;


        public UserRolesService(IGenericRepository<DataEntities.Model.AspNetRole> aspNetRoleRepo,
            IGenericRepository<DataEntities.Model.AspNetUser> aspNetUserRepo,
            IGenericRepository<DataEntities.Model.User> userRepo)
        {
            _aspNetRoleRepo = aspNetRoleRepo;
            _aspNetUserRepo = aspNetUserRepo;
            _userRepo = userRepo;
        }

        public UserRolesService()
        {
            var context = new TemplateEntities();
            _aspNetRoleRepo = new GenericRepository<AspNetRole>(context);
            _aspNetUserRepo = new GenericRepository<AspNetUser>(context);
            _userRepo = new GenericRepository<User>(context);
        }



        /// <summary>
        /// Get the list of the users with their roles
        /// </summary>
        /// <param name="Pattern"></param>
        /// <param name="StartAt"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public  DisplayUsersViewModel GetDisplayUsersViewModel(string Pattern, int StartAt, int PageSize)
        {
            DisplayUsersViewModel model = new DisplayUsersViewModel();
            try
            {
                List<RoleItem> AllRolesList = _aspNetRoleRepo.List().Select(a => new RoleItem { RoleId = a.Id, RoleName = a.Name }).ToList();

                model.Pattern = Pattern;
                model.PageSize = PageSize;
                model.StartAt = StartAt;
                if (Pattern == null)
                    Pattern = "";
                Pattern = Pattern.ToLower();
                var Users = new List<AspNetUser>();
                if (String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    var FullUsersList = _aspNetUserRepo.List().ToList();
                    model.Count = FullUsersList.Count;
                    Users = FullUsersList.OrderByDescending(e => e.Id).Skip(StartAt).Take(PageSize).ToList();
                }
                else
                {
                    Users = _aspNetUserRepo.List().ToList();
                }

                foreach (var user in Users)
                {
                    UserRoleItem userRolItem = TransformAspNetUserIntoUserRoleItem(user, AllRolesList);
                    if(userRolItem!=null)
                    {
                        model.UserRolesList.Add(userRolItem);
                    }
                }
                if (!String.IsNullOrWhiteSpace(Pattern) && StartAt >= 0 && PageSize >= 0)
                {
                    IEnumerable<UserRoleItem> resultIEnumerable = model.UserRolesList as IEnumerable<UserRoleItem>;
                    resultIEnumerable = resultIEnumerable.Where(a =>  Commons.FormaterHelper.GetFormatStringForDateDisplay(a.DateLastConnection).ToLower().Contains(Pattern) || (a.UserRolesList.ToList().Where(r => r.RoleName.ToLower().Contains(Pattern.ToLower())).Any()) || (a.UserFirstNameDecrypt != null && a.UserFirstNameDecrypt.Contains(Pattern)) || a.UserLastNameDecrypt.Contains(Pattern) || (a.UserNameDecrypt != null && a.UserNameDecrypt.Contains(Pattern)));
                    model.Count = resultIEnumerable.ToList().Count;
                    model.UserRolesList = resultIEnumerable.OrderByDescending(a => a.UserFirstNameDecrypt).OrderByDescending(a => a.UserLastNameDecrypt).Skip(StartAt).Take(PageSize).ToList();
                }

            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Pattern = " + Pattern);
            }
            return model;
        }



        public bool IsInRole(int UserId, string roleName)
        {
            try
            {
                var role = _aspNetRoleRepo.FindAllBy(x => x.Name == roleName).FirstOrDefault();
                var user = _userRepo.Get(UserId);
                if (user != null && user.AspNetUser!=null)
                {
                    if (role != null && role.AspNetUsers.Any(x => x.UserName == user.AspNetUser?.UserName)) return true;

                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserId = " + UserId + " and roleName = " + roleName);
            }
            return false;
        }

        /// <summary>
        /// Get the user roles informations for one user
        /// </summary>
        /// <param name="UserIdentity"></param>
        /// <returns></returns>
        public  UserRoleItem GetUserRolesByUseridentityId(string UserIdentity)
        {
            UserRoleItem model = new UserRoleItem();
            try
            {
                var aspNetuser = _aspNetUserRepo.FindAllBy(u => u.Id == UserIdentity).FirstOrDefault();

                if(aspNetuser!=null)
                {
                    List<RoleItem> AllRolesList=_aspNetRoleRepo.List().Select(a => new RoleItem { RoleId = a.Id, RoleName = a.Name }).ToList();
                    model = TransformAspNetUserIntoUserRoleItem(aspNetuser, AllRolesList);
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentity = " + UserIdentity);
            }
            return model;
        }


        public UserRoleItem TransformAspNetUserIntoUserRoleItem(AspNetUser aspnetUser,List<RoleItem> AllRolesList)
        {
            UserRoleItem model = new UserRoleItem();
            try
            {
                if (aspnetUser != null)
                {
                    var user = aspnetUser.Users.FirstOrDefault();
                    model.DateLastConnection = user.DateLastConnection.ToLocalTime();
                    model.UserId = user.Id;
                    model.UseridentityId = aspnetUser.Id;
                    model.UserFirstNameDecrypt = user.FirstName;
                    model.UserLastNameDecrypt =user.LastName;
                    model.UserNameDecrypt =user.AspNetUser.UserName;

                    model.UserRolesList = aspnetUser.AspNetRoles.ToList().Select(a => new RoleItem { RoleId = a.Id, RoleName = a.Name }).ToList(); 


                    List<RoleItem> UserNotInRoleList = new List<RoleItem>();
                    foreach (RoleItem role in AllRolesList)
                    {
                        string strRole = role.RoleName;
                        if (!model.UserRolesList.Where(a => a.RoleName == strRole).Any())
                        {
                            RoleItem roleItemn = new RoleItem();
                            roleItemn.RoleName = strRole;
                            roleItemn.RoleId = role.RoleId;
     
                            UserNotInRoleList.Add(roleItemn);
                        }
                    }
                    model.UserNotInRoleList = UserNotInRoleList;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "UserIdentityId = " + aspnetUser.Id);
            }
            return model;
        }

        /// <summary>
        /// Add a role to the user
        /// </summary>
        /// <param name="AspNetUserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public bool AddUserRole(string AspNetUserId, string RoleId)
        {
            bool result = false;
            try
            {
                AspNetUser user = _aspNetUserRepo.FindAllBy(u => u.Id == AspNetUserId).FirstOrDefault();
                AspNetRole role = _aspNetRoleRepo.FindAllBy(r => r.Id == RoleId).FirstOrDefault();

                if (user != null && role != null)
                {
                    if (!user.AspNetRoles.Contains(role))
                    {
                        user.AspNetRoles.Add(role);
                        _aspNetUserRepo.Edit(user);
                        result = _aspNetUserRepo.Save();
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "AspNetUserId = " + AspNetUserId + " and RoleId = " + RoleId.ToString());
            }
            return result;
        }




        /// <summary>
        /// Delete a userrole by userid and roleid
        /// </summary>
        /// <param name="AspNetUserId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public bool DeleteUserRoleByUserIdAndRoleId(string AspNetUserId, string RoleId)
        {
            bool result = false;
            try
            {

                AspNetUser user = _aspNetUserRepo.FindAllBy(u => u.Id == AspNetUserId).FirstOrDefault();
                AspNetRole role = _aspNetRoleRepo.FindAllBy(r => r.Id == RoleId).FirstOrDefault();

                if(user!=null && role!=null)
                {
                    if(user.AspNetRoles.Any() && user.AspNetRoles.Contains(role))
                    {
                        user.AspNetRoles.Remove(role);
                        _aspNetUserRepo.Edit(user);
                        result = _aspNetUserRepo.Save();
                    }
                    else
                    {
                        result = true;
                    }
                }

            }
            catch (Exception e)
            {
                result = false;
                Commons.Logger.GenerateError(e, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "AspNetUserId = " + AspNetUserId+ " and RoleId = "+ RoleId);
            }
            return result;
        }



    }
}
